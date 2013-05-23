using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Net.Sockets;


namespace MSR.LST.Net.Rtp
{
    public class RtcpSender : IDisposable
    {
        #region IRtpSession

        internal interface IRtpSession
        {
            CompoundPacketBuilder RtcpReportIntervalReached();
            void LogEvent(string source, string msg, EventLogEntryType et, int id);
            
            int ParticipantCount{get;}
            uint SSRC{get;}
            string CName{get;}
            ushort TTL{get;}
        }

        
        #endregion IRtpSession

        #region Statics

        /// <summary>
        /// Maximum bandwidth to be consumed by Rtcp packets in bytes per second
        /// </summary>
        private const int RtcpMaximumBandwidth = 10000;
        
        /// <summary>
        /// Minimum transmission interval in milliseconds
        /// 5 seconds per pg 131 in Colin's book
        /// </summary>
        private const int RtcpMinimumTransmissionInterval = 5000;

        /// <summary>
        /// Euler's constant: e - 1.5 = 1.21828
        /// A compensating factor for the effects of reconsideration
        /// </summary>
        private const float EulersConstant = 1.21828F;

        #endregion Statics

        #region Members

        private IRtpSession rtpSession;

        private Thread threadRtcpSender = null;

        /// <summary>
        /// Object that actually sends outgoing multicast Rtcp packets
        /// </summary>
        private INetworkSender rtcpNetworkSender = null;

        /// <summary>
        /// An optional additional receiver for RTCP messages
        /// </summary>
        private INetworkSender diagnosticSender = null;
        
        DateTime lastSendTime = DateTime.Now;
        bool lastSendForced = false;
        
        private bool disposed = false;

        private readonly IPEndPoint destinationEP;

        #region Performance Counters

        private RtcpSenderPC pc;
        private object pcLock = new object();

        private int bytesPerPacketAvg = 0;
        private int bytesPerPacketMax = 0;
        private int bytesPerPacketMin = 0;
        private uint bytes = 0;

        private short packetsPerIntervalMax = 0;
        private uint   packets = 0;

        private short intervalForced = 0;
        private int   intervalMax = 0;
        private int   intervalMin = 0;
        private int   intervals = 0;
        private uint   intervalsSum = 0;

        #endregion Performance Counters

        #endregion Members

        #region Constructors

        internal RtcpSender(IPEndPoint destinationEP,IRtpSession rtpSession)
        {
            this.destinationEP = destinationEP;
            this.rtpSession = rtpSession;

            Initialize();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        
        #endregion Constructors

        #region Public

        public void SendRtcpDataNow()
        {
            Send(true);
        }


        #endregion Public

        /// <summary>
        /// Updates performance counters
        /// </summary>
        /// <param name="ms">An approximate delay in milli-seconds since the last update</param>
        internal void UpdatePerformanceCounters(int ms)
        {
            lock(pcLock)
            {
                if(intervalsSum != 0) pc[RtcpSenderPC.ID.BandwidthAvg] = (uint)(bytes / (intervalsSum / 1000.0F));

                pc[RtcpSenderPC.ID.Bytes] = bytes;
                pc[RtcpSenderPC.ID.BytesPerPacketAvg] = bytesPerPacketAvg;
                pc[RtcpSenderPC.ID.BytesPerPacketMax] = bytesPerPacketMax;
                pc[RtcpSenderPC.ID.BytesPerPacketMin] = bytesPerPacketMin;

                pc[RtcpSenderPC.ID.Packets] = packets;
                pc[RtcpSenderPC.ID.PacketsPerIntervalMax] = packetsPerIntervalMax;

                pc[RtcpSenderPC.ID.Intervals] = intervals;
                pc[RtcpSenderPC.ID.IntervalForced] = intervalForced;
                pc[RtcpSenderPC.ID.IntervalMax] = intervalMax;
                pc[RtcpSenderPC.ID.IntervalMin] = intervalMin;
                if(intervals != 0) pc[RtcpSenderPC.ID.IntervalAvg] = intervalsSum / intervals;

                pc[RtcpSenderPC.ID.Ssrc] = rtpSession.SSRC;
            }
        }

        
        #region Private

        #region Initialize / Dispose

        private void Initialize()
        {
            rtcpNetworkSender = new UdpSender(destinationEP, RtpSession.TimeToLive);

            threadRtcpSender = new Thread(new ThreadStart(SendThread));
            threadRtcpSender.IsBackground = true;
            threadRtcpSender.Name = "RtcpSender";
            threadRtcpSender.Start();

            InitializePerformanceCounters();
        }

        private void Dispose(bool disposing)
        {
            lock(this)
            {
                if(!disposed)
                {
                    disposed = true;

                    threadRtcpSender.Abort();

                    rtcpNetworkSender.Dispose();

                    DisposePerformanceCounters();
                }
            }
        }

        private void InitializePerformanceCounters()
        {
            // Check to see if user has permissions to read performance counter data
            if (BasePC.PerformanceCounterWrapper.HasUserPermissions())
            {
                lock(pcLock)
                {
                    pc = new RtcpSenderPC(rtpSession.CName + " - " + destinationEP);
                }
            }
        }

        private void DisposePerformanceCounters()
        {
            lock(pcLock)
            {
                if(pc != null)
                {
                    pc.Dispose();
                    pc = null;
                }
            }
        }

        #endregion Initialize / Dispose

        /// <summary>
        /// Collects the Rtcp data from the session, assembles it into CompoundPackets (via the
        /// CompoundPacketBuilder) and sends the packets
        /// 
        /// The current design has a "forced" Send occurring on the thread that makes the call, and
        /// a normal Send occurring on the dedicated RtcpSender thread.
        /// 
        /// To make sure that multiple threads aren't making the call at the same time, which can
        /// lead to data access exceptions (e.g. Queue empty), we lock here.
        /// </summary>
        /// <param name="forced">Is Send being called due to timing rules, or forced?</param>
        private void Send(bool forced)
        {
            lock(this)
            {
                try
                {
                    // We're Sending despite timing rules
                    if(forced)
                    {
                        lastSendForced = true;
                    }
                    else // We're Sending due to timing rules
                    {
                        // The timing rules may now be in conflict with a previous forced Send
                        // Ask the timing rules to reconsider again before Sending
                        if(lastSendForced)
                        {
                            lastSendForced = false;
                            return;
                        }
                    }

                    CompoundPacketBuilder cpb = rtpSession.RtcpReportIntervalReached();
                    cpb.BuildCompoundPackets();

                    Debug.Assert(cpb.PacketCount > 0);

                    // Send all compound packets (in case there is more than 1)
                    short packetCount = cpb.PacketCount;
                    int bytes = 0;

                    foreach(CompoundPacket cp in cpb)
                    {
                        BufferChunk data = cp.Data;
                        bytes += data.Length;
                        rtcpNetworkSender.Send(data);

                        if (diagnosticSender != null)
                        {
                            
                            // andrew: send a "carbon-copy" to the diagnostic server
                            // this is done over unicast to avoid entangling diagnosis with multicast
                            diagnosticSender.Send(data);
                        }
                    }

                    // How long between the last send time and now
                    TimeSpan interval = DateTime.Now - lastSendTime;

                    // Update lastSendTime
                    lastSendTime = DateTime.Now;

                    // Update performance data
                    // Packets before bytes, since packets doesn't have any dependencies
                    // but BytesPerPacket requires a correct reading on the packet count
                    UpdatePerformancePackets(packetCount);
                    UpdatePerformanceBytes(bytes);
                    UpdatePerformanceInterval(interval, forced);
                }
                catch(ThreadAbortException){}
                catch(Exception e)
                {
                    // AAA debugging only
                    System.Console.WriteLine(e.StackTrace);

                    if( e is System.Net.Sockets.SocketException )
                    {
                        Object[] args = new Object[]{this, new RtpEvents.HiddenSocketExceptionEventArgs((RtpSession)rtpSession, 
                                                        (System.Net.Sockets.SocketException)e)};
                        EventThrower.QueueUserWorkItem( new RtpEvents.RaiseEvent(RtpEvents.RaiseHiddenSocketExceptionEvent), args );
                    }

                    rtpSession.LogEvent("RtcpSender", e.ToString(), EventLogEntryType.Error, (int)RtpEL.ID.Error);
                }
            }
        }

        private void SendThread()
        {
            TimeSpan interval;
            DateTime current;

            while(!disposed)
            {
                try
                {
                    // Find out how much time to sleep until our next check
                    current = DateTime.Now;
                    interval = RtcpTransmissionInterval();

                    while(current < lastSendTime.Add(interval))
                    {
                        Thread.Sleep(interval);
                        
                        current = DateTime.Now;
                        interval = RtcpTransmissionInterval() - (current - lastSendTime);
                    }

                    Send(false);
                }
                catch(ThreadAbortException){}
                catch(Exception e)
                {
                    rtpSession.LogEvent("RtcpSender", e.ToString(), EventLogEntryType.Error, (int)RtpEL.ID.Error);
                }
            }
        }

        /// <summary>
        /// Calculates the amount of time that should elapse before the next Rtcp packet is sent
        /// See Colin's book pp 128-142
        /// </summary>
        /// <returns>Wait time (in milliseconds)</returns>
        private TimeSpan RtcpTransmissionInterval()
        {
            // Calculate interval based on known factors
            float transmissionInterval = (bytesPerPacketAvg * rtpSession.ParticipantCount) / RtcpMaximumBandwidth;

            // Don't set the interval to less than the Minimum transmission interval
            if (transmissionInterval < RtcpMinimumTransmissionInterval)
            {
                transmissionInterval = RtcpMinimumTransmissionInterval;
            }

            // Except for the first Rtcp packet should be sent in half the interval
            if(packets == 0)
            {
                transmissionInterval *= 0.5F;
            }

            // Randomize between half and one and a half times the interval
            transmissionInterval *= (RtpSession.rnd.Next(50, 150) / 100F);

            return new TimeSpan((long)(TimeSpan.TicksPerMillisecond * transmissionInterval / EulersConstant));
        }

        
        private void UpdatePerformanceBytes(int bytes)
        {
            // Add overhead of IP nad UDP headers (42 bytes according to netmon)
            bytes += RtpSession.IPHeaderOverhead;

            this.bytes += (uint)bytes;

            if(bytes > bytesPerPacketMax)
            {
                bytesPerPacketMax = bytes;
            }

            if(bytesPerPacketMin == 0 || bytes < bytesPerPacketMin)
            {
                bytesPerPacketMin = bytes;
            }

            if(packets != 0)
            {
                bytesPerPacketAvg = (int)(this.bytes / packets);
            }
        }
        
        private void UpdatePerformancePackets(short packets)
        {
            this.packets += (uint)packets;

            if(packets > packetsPerIntervalMax)
            {
                packetsPerIntervalMax = packets;
            }
        }

        private void UpdatePerformanceInterval(TimeSpan ts, bool forced)
        {
            if(forced)
            {
                intervalForced++;
            }
            else
            {
                intervals++;
            }

            int interval = (int)(ts.Ticks / TimeSpan.TicksPerMillisecond);

            intervalsSum += (uint)interval;

            if(interval > intervalMax)
            {
                intervalMax = interval;
            }

            if(intervalMin == 0 || interval < intervalMin)
            {
                intervalMin = interval;
            }
        }


        #endregion Private

        internal void EnableDiagnostics(IPEndPoint diagnosticServer)
        {
            diagnosticSender = new UdpSender(diagnosticServer, 64);
        }
    }
}
