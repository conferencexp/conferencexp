using System;
using System.Diagnostics;
using System.Net;
using System.Threading;


namespace MSR.LST.Net.Rtp
{
    /// <summary>
    /// RtcpReceiverThread is responsible for receiving all incoming packets on the Rtcp Listener,
    /// casting them into specific RtcpPackets, and then taking appropriate action with the packet
    /// 
    /// SDES packets are used to maintain the lists of active Participants and Streams, associate
    /// Streams to Participants, and set the SdesData on Participants and Streams.
    /// 
    /// BYE packets are used to remove RtpParticipants and RtpStreams.
    /// 
    /// Currently SR, RR, and APP packets are simply forwarded to Events in case the application 
    /// has interest in them.  TODO - In the future, we should take the information from SR and RR
    /// and adjust our streams sending rates accordingly.
    /// </summary>
    public class RtcpListener
    {
        internal interface IRtpSession
        {
            void ProcessCompoundPacket(CompoundPacket cp, IPAddress ip);
            void RaiseNetworkTimeoutEvent(string source);
            void LogEvent(string source, string msg, EventLogEntryType et, int id);

            int DropPacketsPercent{get;}
            Random Rnd{get;}
        }

        #region Members

        /// <summary>
        /// A reference back to the rtpSession that created this RtcpListener
        /// 
        /// The rtpSession is the focal point of data analysis and exchange between Rtp and Rtcp
        /// </summary>
        private IRtpSession rtpSession;

        /// <summary>
        /// The thread that waits for Rtcp data and then forwards it along to the rtpSession
        /// </summary>
        private Thread threadRtcpListener = null;

        /// <summary>
        /// A 'smart socket' that listens for incoming multicast Rtcp packets.
        /// </summary>
        private INetworkListener rtcpNetworkListener = null;
        
        /// <summary>
        /// Whether this class has been disposed or not
        /// 
        /// Used in the listening thread
        /// </summary>
        private bool disposed = false;

        private readonly IPEndPoint groupEP = null;
        private readonly IPEndPoint nextHopEP = null;


        #endregion Members

        #region Constructors


        /// <summary>
        /// This constructor is used for reflector-based sessions
        /// </summary>
        /// <param name="nextHopEP"></param>
        /// <param name="groupEP"></param>
        /// <param name="rtpSession"></param>
        internal RtcpListener(IPEndPoint nextHopEP, IPEndPoint groupEP, IRtpSession rtpSession)
        {
            this.rtpSession = rtpSession;
            this.nextHopEP = nextHopEP;
            this.groupEP = groupEP;

            Initialize();
        }

        internal RtcpListener(IPEndPoint nextHopEP, IRtpSession rtpSession)
        {
            this.rtpSession = rtpSession;
            this.nextHopEP = nextHopEP;
            this.groupEP = nextHopEP;

            Initialize();
        }

        
        internal void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }


        #endregion Constructors
        
        #region Listen Thread

        public void ListenThread()
        {
            CompoundPacket compoundPacket = new CompoundPacket();
            EndPoint endPoint = null;

            while(!disposed)
            {
                try
                {
                    // Reset the packet for the next round of data
                    compoundPacket.Reset();

                    // Wait for data off the wire
                    rtcpNetworkListener.ReceiveFrom(compoundPacket.Buffer, out endPoint);

                    #region Fault Injection
                    #if FaultInjection
                    // Drop packets
                    if (rtpSession.DropPacketsPercent > 0)
                    {
                        if (rtpSession.Rnd.Next(1, 100) <= rtpSession.DropPacketsPercent)
                        {
                            continue;
                        }
                    }
                    #endif
                    #endregion Fault Injection

                    // Parse the data and rigorously validate it
                    compoundPacket.ParseBuffer();

                    // Pass it on to the rtpSession to analyze and distribute data
                    rtpSession.ProcessCompoundPacket(compoundPacket, ((IPEndPoint)endPoint).Address);
                }
                
                catch (System.Net.Sockets.SocketException e)
                {
                    // No data received before timeout
                    if (e.ErrorCode == 10060)
                    {
                        rtpSession.RaiseNetworkTimeoutEvent("Rtcp");
                    }
                    // We get 10054 socket exceptions in some situations during unicast connections.  Ignore them for now.
                    // Pri2: Find a better solution for the unicast socket exceptions
                    else if(e.ErrorCode == 10054 && !Utility.IsMulticast(rtcpNetworkListener.ExternalInterface) )
                    {
                        continue;
                    }
                    else
                    {
                        Object[] args = new Object[]{this, new RtpEvents.HiddenSocketExceptionEventArgs((RtpSession)rtpSession, e)};
                        EventThrower.QueueUserWorkItem( new RtpEvents.RaiseEvent(RtpEvents.RaiseHiddenSocketExceptionEvent), args );

                        LogEvent(e.ToString(), EventLogEntryType.Error, (int)RtpEL.ID.Error);
                    }
                }
                catch(RtcpHeader.RtcpHeaderException e)
                {
                    // TODO - this is a pretty serious error - what should we do - exit thread, squelch? JVE
                    LogEvent(e.ToString(), EventLogEntryType.Error, (int)RtpEL.ID.RtcpHeaderException);
                }
                catch(CompoundPacket.CompoundPacketException e)
                {
                    // TODO - this is a pretty serious error - what should we do - exit thread, squelch? JVE
                    LogEvent(e.ToString(), EventLogEntryType.Error, (int)RtpEL.ID.CompoundPacketException);
                }
                catch (ThreadAbortException) {}
                catch (Exception e)
                {
                    LogEvent(e.ToString(), EventLogEntryType.Error, (int)RtpEL.ID.Error);
                }
            }
        }


        #endregion Receive Thread

        #region Private

        private void LogEvent(string msg, EventLogEntryType et, int id)
        {
            rtpSession.LogEvent("RtcpListener", msg, et, id);
        }

        private void Initialize()
        {
           
            if (Utility.IsMulticast(groupEP) && (!nextHopEP.Equals(groupEP)))
            {
                // use reflector
                rtcpNetworkListener = new UdpListener(groupEP, nextHopEP, RtpSession.DefaultNetworkTimeout * 2);
            }
            else
            {
                // multicast or direct unicast
                rtcpNetworkListener = new UdpListener(nextHopEP, RtpSession.DefaultNetworkTimeout * 2);
            }

            threadRtcpListener = new Thread(new ThreadStart(ListenThread));
            threadRtcpListener.IsBackground = true;
            threadRtcpListener.Name = "RtcpListener";
            
            threadRtcpListener.Start();
        }


        /// <summary>
        /// Clean up the RtcpListener
        /// 
        /// A thread will not abort until it is in the running state.  Our listening thread is
        /// generally blocked by the call to rtcpNetworkListener.ReceiveFrom().  In order to
        /// dispose things quickly and efficiently, we tell the listening thread to abort as soon 
        /// as it is back in the running state.  Then we clean up the network listener which should
        /// unblock us.  
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            lock(this)
            {
                disposed = true;
                
                threadRtcpListener.Abort();
                threadRtcpListener = null;

                rtcpNetworkListener.Dispose();
                rtcpNetworkListener = null;
            }
        }

        
        #endregion Private
    }
    
}
