using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;


namespace MSR.LST.Net.Rtp
{
    #region RtpStream

    /// <summary>
    /// An RtpStream represents data coming from the network
    /// It could originate from a local RtpSender, or a remote RtpSender
    /// </summary>
    public class RtpStream
    {
        #region Statics

        internal static RtpStream CreateStream(RtpListener rtpListener, uint ssrc, SdesData sdes)
        {
            string sPT = sdes.GetPrivateExtension(Rtcp.PEP_PAYLOADTYPE);
            PayloadType pt = (PayloadType)Enum.Parse(typeof(PayloadType), sPT);

            // RtpStreamFec
            string fecData = sdes.GetPrivateExtension(Rtcp.PEP_FEC);
            if(fecData != null)
            {
                string[] args = fecData.Split(new char[]{':'});
                ushort dataPxExp = ushort.Parse(args[0], CultureInfo.InvariantCulture);
                ushort fecPxExp = ushort.Parse(args[1], CultureInfo.InvariantCulture);

                if(fecPxExp == 0)
                {
                    throw new ArgumentOutOfRangeException("fecPxExp", fecPxExp, Strings.MustBePositive);
                }
                
                // Frame based Fec
                if(dataPxExp == 0)
                {
                    return new RtpStreamFFec(rtpListener, ssrc, sdes, pt, fecPxExp);
                }
                else // Constant Fec
                {
                    return new RtpStreamCFec(rtpListener, ssrc, sdes, pt, dataPxExp, fecPxExp);
                }
            }

            // RtpStream
            return new RtpStream(rtpListener, ssrc, sdes, pt);
        }

        
        #endregion Statics

        #region Members

        /// <summary>
        /// The unique identifier of this stream
        /// </summary>
        private uint ssrc;
        
        /// <summary>
        /// A stream is considered stale if it isn't receiving Rtp traffic
        /// 
        /// If a stream goes N Rtcp intervals (currently 5, see RtpSession.RtcpMissedIntervalsTimeout)
        /// without Rtp traffic, it is timed out and cleaned up by the manager, which checks this
        /// value during each RtcpSender.RtcpTransmissionInterval
        /// </summary>
        private int stale = 0;

        /// <summary>
        /// Used to communicate with Rtcp about what data this stream has received
        /// </summary>
        private ReceiverReport rr = new ReceiverReport();

        /// <summary>
        /// For logging interesting data to the EventLog
        /// 
        /// TODO - Instance based or static? JVE
        /// </summary>
        internal RtpEL eventLog = new RtpEL(RtpEL.Source.RtpStream);

        internal RtpListener.ReturnBufferHandler returnBufferHandler = null;

        private UInt32 currentFrameTS = 0;
        private RtpFrame frame;

        private int maxSeq = -1;

        private PacketTransform packetTransform = null;

        internal PacketTransform PacketTransform
        {
            get { return packetTransform; }
            set { packetTransform = value; }
        }

        /// <summary>
        /// Used in sequence checking by stream and frame
        /// 
        /// We use "rollover" math on a ushort to determine if a packet arrived late
        /// If the packets arrived 1, 2 the diff is 2 - 1 = 1 (good)
        /// If the packets arrived 2, 1 the diff is 1 - 2 = 65535 (late)
        /// 
        /// If the sequence number is more than 32K different than the last one they came in late
        /// It was difficult to pick a smaller number, as a group of packets might get lost and
        /// you have to work around whatever that amount is
        /// </summary>
        private const uint HALF_UINT_MAX = uint.MaxValue / 2;
        protected const ushort HALF_USHORT_MAX = ushort.MaxValue / 2;

        /// <summary>
        /// Currently, only CName, Name and a private extension CapabilityChannelIdentifier are
        /// used.  We get the CName from the Owner.  Name is passed in the constructor but can be
        /// updated by the Sender, which is how ChannelID is updated.
        /// </summary>
        private SdesData properties;

        /// <summary>
        /// The type of data this stream receives
        /// </summary>
        protected PayloadType pt;

        #region NextFrame

        /// <summary>
        /// Indicates whether or not the user of the RtpStream is intending to use the NextFrame
        /// method (or if they will hooking the FrameReceived event)
        /// </summary>
        private bool isUsingNextFrame = false;

        /// <summary>
        /// Unblocks the caller of NextFrame so they can receive the frame
        /// </summary>
        private AutoResetEvent nextFrameEvent = new AutoResetEvent(false);

        /// <summary>
        /// Flag to keep track of which event to fire next - DataStopped, DataStarted
        /// We start by assuming data will arrive, and then prove it hasn't
        /// This is raised for informational purposes to the caller of NextFrame
        /// </summary>
        private bool dataStoppedEvent = false;

        private int defaultNextFrameTimeout = 1500;

        /// <summary>
        /// How many milliseconds to wait before firing the DataStoppedEvent
        /// Allows control of the firing of the DataStoppedEvent for callers of NextFrame
        /// 
        /// This is set when IsUsingNextFrame is called with true
        /// </summary>
        private int nextFrameTimeout;

        /// <summary>
        /// Used to throw a NextFrameUnblockedException that indicates the NextFrame event was 
        /// manually Set.  This is used to unblock the caller of NextFrame if you can't control 
        /// calling the thread any other way.
        /// </summary>
        private bool nextFrameUnblocked = false;
        
        /// <summary>
        /// Used to indicate when the lastFrame has been collected
        /// </summary>
        private bool frameReceived = true;
        
        private BufferChunk lastFrame = null;
        private BufferChunk firstFrame = null;

        #endregion NextFrame

        #region Performance Counters

        internal RtpStreamPC pc;
        protected object pcLock = new object();

        // The following members are used for PerfCounters and Rtcp reporting
        private int bytesReceived = 0;
        private int bytesReceivedLast = 0;
        
        private int framesLost = 0;
        private int framesReceived = 0;
        private int framesReceivedLast = 0;

        private int invalidPacketInFrameEvents = 0; // TODO - this an event counter... JVE

        internal int packetsLost = 0;
        internal uint packetsReceived = 0;
        private uint packetsReceivedLast = 0;
        private uint packetsReceivedLate = 0;

        #endregion Performance Counters
        
        #endregion Members

        #region Constructors

        internal RtpStream(RtpListener rtpListener, uint ssrc, SdesData sdes, PayloadType pt)
        {
            Debug.Assert(rtpListener != null);
            Debug.Assert(ssrc != 0);
            Debug.Assert(sdes != null);

            this.returnBufferHandler = rtpListener.ReturnBufferCallback;
            this.ssrc = ssrc;
            this.pt = pt;

            properties = new SdesData(sdes);

            InitializePerformanceCounters();
        }

        private void Dispose(bool disposing)
        {
            // Always lock to prevent multiple threads from accessing (including GC thread)
            lock(this)
            {
                // Both of these methods are re-entrant, and we don't keep track of our disposed
                // state anywhere or prevent this object from being used in a disposed state
                if(isUsingNextFrame)
                {
                    UnblockNextFrame();
                }

                DisposePerformanceCounters();
            }
        }

        internal void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        ~RtpStream()
        {
            Dispose(false);
        }

        
        #endregion Constructors
        
        #region Public

        /// <summary>
        /// Currently, only CName, Name and a private extension CapabilityChannelIdentifier are
        /// used.  We get the CName from the Owner.  Name is passed in the constructor but can be
        /// updated by the Sender, which is how ChannelID is updated.
        /// 
        /// 'Set' updates the local properties from those provided, but does not keep a reference
        /// </summary>
        public SdesData Properties
        {
            get
            {
                return properties;
            }
        }

        public uint SSRC
        {
            get
            {
                return ssrc;
            }
        }

        public PayloadType PayloadType
        {
            get
            {
                return pt;
            }
        }

        /// <summary>
        /// For diagnostics.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // No CheckDisposition here, since we want to include this info
            // when object is being accessed internally during CheckDisposition

            string name = null;
            if(properties != null)
            {
                name = properties.Name;
            }

            return string.Format(CultureInfo.CurrentCulture, "RtpStream [ ssrc := {0}, name := {1}, " +
                "PayloadType := {2} ]", ssrc, name, pt.ToString());
        }


        public bool IsUsingNextFrame
        {
            get{return isUsingNextFrame;}
            set
            {
                isUsingNextFrame = value;
                if(isUsingNextFrame)
                {
                    nextFrameTimeout = defaultNextFrameTimeout;
                }
            }
        }

        public int NextFrameTimeout
        {
            get
            {
                ValidateUsingNextFrame();
                return nextFrameTimeout;
            }
            set
            {
                ValidateUsingNextFrame();
                defaultNextFrameTimeout = value;
                nextFrameTimeout = defaultNextFrameTimeout;
            }
        }

        /// <summary>
        /// This method is used to receive Frames by polling the RtpStream.  This method should only be used by applications
        /// that already bring a dedicated reading thread (such as DirectShow Filters and their FillBuffer method).  Most
        /// applications should instead use the RtpStream.FrameReceived event to receive frames.
        /// </summary>
        /// <remarks>
        /// The FrameReceived event causes overhead because it uses DynamicInvoke to fire the 
        /// event, and it is serialized on the EventThrower thread.  NextFrame allows for more 
        /// efficient retrieval of the data at the cost of needing to provide an extra thread.
        /// 
        /// This method is meant to be called single threaded (as the DShow filters do).
        /// </remarks>
        /// <returns>A frame of data or a NextFrameUnblockedException</returns>
        public BufferChunk NextFrame()
        {
            ValidateUsingNextFrame();

            // Not an infinite loop.  
            while(nextFrameEvent.WaitOne(nextFrameTimeout, false) == false)
            {
                // It should only fire as false once, before it loops back around
                // for an infinite timeout (has to be released by the Set method).
                dataStoppedEvent = true;
                nextFrameTimeout = Timeout.Infinite;
                OnDataStoppedEvent();
            }

            if(nextFrameUnblocked)
            {
                frameReceived = true; // Claim we received this frame so we can come through next time
                nextFrameUnblocked = false;
                throw new NextFrameUnblockedException(Strings.NextFrameCallHasBeenUnblocked);
            }

            if(dataStoppedEvent)
            {
                dataStoppedEvent = false;
                nextFrameTimeout = defaultNextFrameTimeout;
                OnDataStartedEvent();
            }

            BufferChunk ret = lastFrame;
            frameReceived = true;

            return ret;
        }
        
        public void UnblockNextFrame()
        {
            ValidateUsingNextFrame();
            nextFrameUnblocked = true;
            nextFrameEvent.Set();
        }

        public void BlockNextFrame()
        {
            ValidateUsingNextFrame();
            nextFrameUnblocked = false;
        }

        public BufferChunk FirstFrame()
        {
            ValidateUsingNextFrame();
            return firstFrame.Peek(firstFrame.Index, firstFrame.Length);
        }


        public int FramesReceived
        {
            get{return framesReceived;}
        }

        public uint PacketsReceived
        {
            get{return packetsReceived;}
        }

        public int FramesLost
        {
            get{return framesLost;}
        }

        public int PacketsLost
        {
            get{return packetsLost;}
        }


        protected int LostPackets(ref int maxSeq, ushort seq)
        {
            int ret = 0;

            if (maxSeq == -1)
            {
                // startup case
                maxSeq = seq;
            }
            else
            {
                ushort diff = unchecked((ushort)(seq - maxSeq));

                if (diff < HALF_USHORT_MAX)
                {
                    maxSeq = seq;

                    if (diff > 1)
                    {
                        ret = diff - 1;
                    }
                    else
                    {
                        // duplicate...
                    }
                }
                else
                {
                    // a large jump occured; reset the maxSeq value
                    maxSeq = seq;
                }
            }

            return ret;
        }

        

        #endregion Public

        #region Internal
        
        /// <summary>
        /// Method that converts RtpPacketBase to an RtpPacket and forwards
        /// </summary>
        internal virtual void ProcessPacket(RtpPacketBase packet)
        {
            ProcessPacket((RtpPacket)packet);
        }

        /// <summary>
        /// newPacket is how RtpListener sends an incoming RtpPacket who's SSRC matches this
        /// RtpStream into the RtpStream's queue for processing
        /// 
        /// This method assumes there is only one thread calling newPacket
        /// </summary>
        internal virtual void ProcessPacket(RtpPacket packet)
        {
            #region Mark this stream and its participant as not stale
            stale = 0;

            // TODO - mark participant or we will destroy and recreate streams JVE
            //if (participant != null)
            //{
            //    participant.stale = 0;
            //}
            #endregion

            // Mark raw packet received
            packetsLost += LostPackets(ref maxSeq, packet.Sequence);
            packetsReceived++;
            bytesReceived += packet.PayloadSize;

            #region Reject Late Packets

            uint packetTS = packet.TimeStamp;
            uint deltaTS = 0;

            if (packetsReceived != 1) // Skip this test if we're in a startup state
            {
                deltaTS = unchecked(packetTS - currentFrameTS);

                if (deltaTS > HALF_UINT_MAX)
                {
                    packetsReceivedLate++;
                    returnBufferHandler(packet.ReleaseBuffer());
                    return;
                }
            }

            #endregion

            // A packet from a future frame has been received, so send out the incomplete current frame
            // Note we cannot check for deltaTS == 1 because we may miss more than 1 frame
            if (deltaTS > 0)
            {
                if(frame != null)
                {
                    frame.Dispose();
                    frame = null;
                }

                RtpRetransmit.FrameIncomplete(this, (int)deltaTS);
            }

            // We're receiving the first packet for a new frame
            if (frame == null) 
            {
                frame = new RtpFrame(packet.PacketsInFrame, packetTS, returnBufferHandler);
                currentFrameTS = packetTS;
            }

            try
            {
                // Store the packet
                frame[packet.FrameIndex] = packet;

                // Apply any transformation (such as decryption)
                if (packetTransform != null)
                    packetTransform.Decode(packet);
            }
            catch(DuplicatePacketException)
            {
                // Note: We have already updated the performance counters for these bytes / this packet
                // and we are rejecting it now.  So pcs could be higher than expected.
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                    Strings.DuplicatePacketReceivedInRtpStream, packet.ToString()), EventLogEntryType.Error, 
                    (int)RtpEL.ID.DuplicatePacketReceived);

                return;
            }

            // We've received the correct number of data packets, so we can now re-create the frame
            if (frame.Complete)
            {
  
                // Raise the event
                OnFrameReceivedEvent(frame.Data);

                // Clean up frame - like returning packets to RtpListener
                frame.Dispose();
                frame = null;

                // CXP RtpSenders increment by 1, to do otherwise is a signal from the Sender
                // which means the stream needs to know what the signal means
                currentFrameTS++; 
            }
        }

        
        /// <summary>
        /// A stream is considered stale if it isn't receiving Rtp traffic
        /// 
        /// If a stream goes N Rtcp intervals (currently 5, see RtpSession.RtcpMissedIntervalsTimeout)
        /// without Rtp traffic, it is timed out and cleaned up by the manager, which checks this
        /// value during each RtcpSender.RtcpTransmissionInterval
        /// </summary>
        internal int Stale
        {
            get{return stale;}
            set{stale = value;}
        }

        
        internal void AddReceiverReport(CompoundPacketBuilder cpb)
        {
            // Update the values
            rr.SSRC = ssrc;
            rr.PacketsLost = packetsLost;

            // andrew: this is obviously not what the spec says, but it's easier to compute and 
            //  to use for diagnostics...
            rr.ExtendedHighestSequence = packetsReceived;

            // andrew: this also is incorrect: the spec says this should be calculated per
            // interval rather than per session
            rr.FractionLost = (byte)((float)packetsLost / (float)(packetsReceived + packetsLost) * 100F);

            // TODO - figure out how to calculate / interpret these values, especially Seq - JVE
//            rr.Jitter = ?;
//            rr.LastSenderReport = ?;
//            rr.DelaySinceLastSenderReport = ?;
//            rr.ExtendedHighestSequence = ?;

            cpb.Add_ReceiverReport(rr);
        }

        
        /// <summary>
        /// Updates the local perf counters
        /// </summary>
        /// <param name="ms">An approximate delay in milli-seconds since the last update</param>
        internal virtual void UpdatePerformanceCounters(int ms)
        {
            lock(pcLock)
            {
                // A stream may exist before it has an owner and hence Sdes properties on which the
                // performance counter instance name is based
                if(pc != null)
                {
                    pc[RtpStreamPC.ID.BytesPerSecond] = bytesReceived - bytesReceivedLast;
                    pc[RtpStreamPC.ID.Bytes] = bytesReceived;
                    bytesReceivedLast = bytesReceived;

                    pc[RtpStreamPC.ID.FramesLost] = framesLost;
                    pc[RtpStreamPC.ID.FramesLostPer10000] = (long)FramesLostIn10000;
                    pc[RtpStreamPC.ID.FramesPerSecond] = (long)((framesReceived - framesReceivedLast) * (1000.0 / ms));
                    pc[RtpStreamPC.ID.Frames] = framesReceived;
                    framesReceivedLast = framesReceived;
        
                    pc[RtpStreamPC.ID.PacketsInvalid] = invalidPacketInFrameEvents;
                    pc[RtpStreamPC.ID.PacketsLost] = packetsLost;
                    pc[RtpStreamPC.ID.PacketsLostPer10000] = (long)PacketsLostIn10000;
                    pc[RtpStreamPC.ID.PacketsPerSecond] = (long)((packetsReceived - packetsReceivedLast) * (1000.0 / ms));
                    pc[RtpStreamPC.ID.PacketsLate] = packetsReceivedLate;
                    pc[RtpStreamPC.ID.Packets] = packetsReceived;
                    packetsReceivedLast = packetsReceived;

                    pc[RtpStreamPC.ID.Ssrc] = ssrc;
                }
            }
        }

        #endregion Internal
        
        #region Private
        
        private void InitializePerformanceCounters()
        {
        // Check to see if user has permissions to read performance counter data
            if (BasePC.PerformanceCounterWrapper.HasUserPermissions())
            {
                lock (pcLock)
                {
                    pc = new RtpStreamPC(Properties.CName + " - " + PayloadType.ToString());
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

        /// <summary>
        /// How many packets are lost per 10,000 sent?  We needed to be more detailed than integer percent.
        /// </summary>
        private float PacketsLostIn10000
        {
            get
            {
                if (packetsLost == 0)
                    return 0;
                else if (packetsReceived == 0)
                    return 0;
                else
                    return (float)packetsLost / (float)(packetsReceived + packetsLost) * 10000F;
            }
        }

        
        /// <summary>
        /// How many frames are lost per 10,000 sent?  We needed to be more detailed than integer percent.
        /// </summary>
        private float FramesLostIn10000
        {
            get
            {
                if (framesLost == 0)
                    return 0;
                else if (framesReceived == 0)
                    return 0;
                else
                    return (float)framesLost / (float)(framesReceived + framesLost) * 10000F;
            }
        }
        

        private void ValidateUsingNextFrame()
        {
            if(!isUsingNextFrame)
            {
                string msg = Strings.CallingANextFrameMethod;

                Debug.Assert(false, msg);
                throw new RtpException(msg);
            }
        }


        #endregion

        #region Events
        
        // All of these events are called by Frame, and hence are internal
        // TODO - try to decouple JVE

        #region PacketOutOfSequence
        
        internal void RaisePacketOutOfSequenceEvent(int packetsLost, string message)
        {
            this.packetsLost += packetsLost;
                
            object[] args = {this, new RtpEvents.PacketOutOfSequenceEventArgs(this, packetsLost, message)};
            EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RtpEvents.RaisePacketOutOfSequenceEvent), args);
        }
            
            
        #endregion PacketOutOfSequence

        #region InvalidPacketInFrame

        internal void RaiseInvalidPacketInFrameEvent(string reason)
        {
            ++invalidPacketInFrameEvents;

            object[] args = {this, new RtpEvents.InvalidPacketInFrameEventArgs(this, reason)};
            EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RtpEvents.RaiseInvalidPacketInFrameEvent), args);
        }

        #endregion InvalidPacketInFrame

        #region FramesOutOfSequence

        internal void RaiseFrameOutOfSequenceEvent(int framesLost, string message)
        {
            Trace.Assert(framesLost > 0, "framesLost should be greater than 0");

            this.framesLost += framesLost;
                
            object[] args = {this, new RtpEvents.FrameOutOfSequenceEventArgs(this, framesLost, message)};
            EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RtpEvents.RaiseFrameOutOfSequenceEvent), args);
        }

            
        #endregion FramesOutOfSequence
        
        #region FrameReceived

        public class FrameReceivedEventArgs : EventArgs
        {
            public BufferChunk Frame;
            public RtpStream RtpStream;

            public FrameReceivedEventArgs(RtpStream rtpStream, BufferChunk frame)
            {
                RtpStream = rtpStream;
                Frame = frame;
            }
        }
        
        public delegate void FrameReceivedEventHandler(object sender, FrameReceivedEventArgs ea);
        
        internal void RaiseFrameReceivedEvent(object[] args)
        {
            if(!RtpEvents.FireEvent(FrameReceived, args))
            {
                FrameReceivedEventArgs ea = (FrameReceivedEventArgs)args[1];

                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.FrameReceivedEvent, 
                    ea.RtpStream.SSRC), EventLogEntryType.Warning, (int)RtpEL.ID.FrameReceived);
            }
        }


        /// <summary>
        /// This is an instance based event, so that one doesn't have to listen to all 
        /// FrameReceived events and pick out the desired ones per frame
        /// </summary>
        public event FrameReceivedEventHandler FrameReceived;

        internal void OnFrameReceivedEvent(BufferChunk frame)
        {
            framesReceived++;

            // Don't trigger the event if it isn't hooked
            // Although RtpEvents.FireEvent does this same check, we add it here for efficiency
            // As audio / video streams create alot of data, but don't use the FrameReceived event
            if(FrameReceived != null)
            {
                if(framesReceived == 1)
                {
                    OnFirstFrameReceivedEvent();
                }

                object[] args = {this, new FrameReceivedEventArgs(this, frame)};
                EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RaiseFrameReceivedEvent), args);
            }
            else // Audio and Video
            {
                if(frameReceived)
                {
                    if(framesReceived == 1)
                    {
                        firstFrame = frame.Peek(frame.Index, frame.Length);
                        OnFirstFrameReceivedEvent();
                    }

                    lastFrame = frame;
                    frameReceived = false;
                    nextFrameEvent.Set();
                }
            }
        }

        
        #endregion FrameReceived
        
        #region FirstFrameReceived
        
        public delegate void FirstFrameReceivedEventHandler(object sender, EventArgs ea);
        
        internal void RaiseFirstFrameReceivedEvent(object[] args)
        {
            RtpEvents.FireEvent(FirstFrameReceived, args);
        }


        /// <summary>
        /// This is a static event, so that it can be hooked before the stream is created
        /// And therefore prevent race conditions with hooking it in time
        /// </summary>
        public static event FirstFrameReceivedEventHandler FirstFrameReceived;

        internal void OnFirstFrameReceivedEvent()
        {
            object[] args = {this, null};
            EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RaiseFirstFrameReceivedEvent), args);
        }

        
        #endregion FirstFrameReceived
        
        #region DataStarted
        
        public delegate void DataStartedEventHandler(object sender, EventArgs ea);
        
        internal void RaiseDataStartedEvent(object[] args)
        {
            RtpEvents.FireEvent(DataStarted, args);
        }

        public event DataStartedEventHandler DataStarted;

        internal void OnDataStartedEvent()
        {
            object[] args = {this, null};
            EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RaiseDataStartedEvent), args);
        }

        
        #endregion DataStarted
        
        #region DataStopped
        
        public delegate void DataStoppedEventHandler(object sender, EventArgs ea);
        
        internal void RaiseDataStoppedEvent(object[] args)
        {
            RtpEvents.FireEvent(DataStopped, args);
        }

        public event DataStoppedEventHandler DataStopped;

        internal void OnDataStoppedEvent()
        {
            object[] args = {this, null};
            EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RaiseDataStoppedEvent), args);
        }

        
        #endregion DataStopped

        #endregion Events
    }
    
    
    #endregion RtpStream

    #region RtpStreamFec

    internal abstract class RtpStreamFec : RtpStream
    {
        #region Members

        // parameters we were told to construct the stream with
        protected ushort dataPxExp;
        protected ushort fecPxExp;

        protected RtpPacketBase[] dataPx = null;
        protected BufferChunk[] data = null;
        protected BufferChunk[] recovery = null;

        private RtpListener.GetBufferHandler getBufferHandler = null;
        
        private bool rangeInitialized = false;

        private ushort minSeq;
        private ushort maxSeq;

        private int maxDataSeq = -1;
        private int maxFecSeq = -1;

        private int dataPxAct;
        private int fecPxAct;

        private int recoveryIndex;

        /// <summary>
        /// Decoder class which will handle the decoding
        /// </summary>
        protected IFec fecDecoder;

        #region Performance Counters

        private int pcDataBytes;
        private int pcDataPackets;
        private int pcDataPacketsLate;
        private int pcDataPacketsLost;

        private int pcDecodePath;

        private int pcFecBytes;
        private int pcFecPackets;
        private int pcFecPacketsLate;
        private int pcFecPacketsLost;

        private int pcUndecodable;

        protected int pcFecType;

        #endregion Performance Counters

        #endregion Members

        #region Constructor

        internal RtpStreamFec(RtpListener rtpListener, uint ssrc, SdesData sdes, PayloadType pt) 
            : base(rtpListener, ssrc, sdes, pt)
        {
            this.getBufferHandler = rtpListener.GetBufferCallback;
        }


        #endregion Constructor
        
        /// <summary>
        /// Updates the local perf counters
        /// </summary>
        /// <param name="ms">An approximate delay in milli-seconds since the last update</param>
        internal override void UpdatePerformanceCounters(int ms)
        {
            lock(pcLock)
            {
                if(pc != null)
                {
                    pc[RtpStreamPC.ID.f_DataBytes] = pcDataBytes;
                    pc[RtpStreamPC.ID.f_DataPackets] = pcDataPackets;
                    pc[RtpStreamPC.ID.f_DataPacketsLate] = pcDataPacketsLate;
                    pc[RtpStreamPC.ID.f_DataPacketsLost] = pcDataPacketsLost;
                    pc[RtpStreamPC.ID.f_DecodePath] = pcDecodePath;
                    pc[RtpStreamPC.ID.f_FecBytes] = pcFecBytes;
                    pc[RtpStreamPC.ID.f_FecPackets] = pcFecPackets;
                    pc[RtpStreamPC.ID.f_FecPacketsLate] = pcFecPacketsLate;
                    pc[RtpStreamPC.ID.f_FecPacketsLost] = pcFecPacketsLost;
                    pc[RtpStreamPC.ID.f_RatioChecksum] = fecPxExp;
                    pc[RtpStreamPC.ID.f_RatioData] = dataPxExp;
                    pc[RtpStreamPC.ID.f_Type] = pcFecType;
                    pc[RtpStreamPC.ID.f_Undecodable] = pcUndecodable;
                }

                base.UpdatePerformanceCounters(ms);
            }
        }
    

        internal override void ProcessPacket(RtpPacketBase packet)
        {
            // If any part of this code throws an exception we need to clean up what we have and
            // reset back to a default state so we can start over.
            try
            {
                PayloadType pt = packet.PayloadType;
                ValidatePayloadType(pt);

                // Process packet, depending on payload type
                if(pt == this.pt)
                {
                    ProcessPacketData((RtpPacket)packet);
                }
                else
                {
                    ProcessPacketFec((RtpPacketFec)packet);
                }

                // See if we have collected enough packets
                if(dataPxAct == dataPxExp)
                {
                    ForwardDataPacketsToBase();
                }
                else if(dataPxAct + fecPxAct == dataPxExp)
                {
                    Decode();
                    ForwardDataPacketsToBase();
                }
            }
            catch(ThreadAbortException){} // Because we catch a generic exception next
            catch(Exception e)
            {
                eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, (int)RtpEL.ID.Error);
                ForwardDataPacketsToBase();
            }
        }

        
        protected void InitializeDCRStorage()
        {
            // Data and checksum are combined
            dataPx = new RtpPacketBase[dataPxExp + fecPxExp];
            data = new BufferChunk[dataPxExp + fecPxExp];
            
            recovery = new BufferChunk[fecPxExp];
        }

        
        #region Private

        private void ProcessPacketData(RtpPacket packet)
        {
            pcDataPacketsLost += LostPackets(ref maxDataSeq, packet.Sequence);
            pcDataPackets++;
            pcDataBytes += packet.PayloadSize;

            if(InRangeData(packet))
            {
                // New sequence of packets
                if(dataPxAct + fecPxAct == 0) 
                {
                    ResetDecodingState(packet);
                }
                
                int dataFecIndex = packet.FecIndex;

                // Duplicate packet check for flaky networks
                if(dataPx[dataFecIndex] != null)
                {
                    eventLog.WriteEntry(Strings.DuplicatePacketReceived, EventLogEntryType.Warning,
                        (int)RtpEL.ID.DuplicatePacketReceived);
                    return;
                }

                dataPxAct++;

                dataPx[dataFecIndex] = packet;
                data[dataFecIndex] = (BufferChunk)packet;
            }
        }

        private void ProcessPacketFec(RtpPacketFec packet)
        {
            pcFecPacketsLost += LostPackets(ref maxFecSeq, packet.Sequence);
            pcFecPackets++;
            pcFecBytes += packet.PayloadSize;

            if(InRangeFec(packet))
            {
                // New sequence of packets
                if(dataPxAct + fecPxAct == 0) 
                {
                    ResetDecodingState(packet);
                }
                
                int checksumFecIndex = packet.FecIndex;

                // Duplicate packet check for flaky networks
                if(dataPx[dataPxExp + checksumFecIndex] != null)
                {
                    eventLog.WriteEntry(Strings.DuplicatePacketReceived, EventLogEntryType.Warning,
                        (int)RtpEL.ID.DuplicatePacketReceived);
                    return;
                }
                
                fecPxAct++;

                dataPx[dataPxExp + checksumFecIndex] = packet;
                data[dataPxExp + checksumFecIndex] = packet.Payload;

                recovery[recoveryIndex++] = getBufferHandler();
            }
        }

        // Check to see if packet is inside the recovery range
        protected bool InRangeFec(RtpPacketFec packet)
        {
            bool inRange = true;

            if(rangeInitialized)
            {
                ushort minDiff = unchecked((ushort)(packet.DataRangeMin - minSeq));
                
                // Correct data range but not needed - late (frame already complete)
                // OR Wrong data range - really late
                if((minDiff == 0 && (dataPxAct + fecPxAct == 0)) || minDiff > HALF_USHORT_MAX) 
                {
                    inRange = false;
                    pcFecPacketsLate++;
                    ReturnFecBuffer(packet.ReleaseBuffer());
                }                
                else if(minDiff != 0) // Wrong data range - too new
                {
                    Undecodable();
                }
            }

            return inRange;
        }

        // Check to see if packet is inside the recovery range
        protected bool InRangeData(RtpPacket packet)
        {
            bool inRange = true;

            if(rangeInitialized)
            {
                ushort minDiff = unchecked((ushort)(packet.Sequence - minSeq));
                ushort maxDiff = unchecked((ushort)(maxSeq - packet.Sequence));

                if(minDiff > HALF_USHORT_MAX) // Wrong data range - too late
                {
                    inRange = false;
                    pcDataPacketsLate++;
                    base.ProcessPacket(packet);
                }
                else if(maxDiff > HALF_USHORT_MAX) // Wrong data range - too new
                {
                    Undecodable();
                }
            }

            return inRange;
        }

        
        /// <summary>
        /// Sends the data packets (original or reconstructed) to the base RtpStream to process
        /// into a frame.
        /// </summary>
        protected void ForwardDataPacketsToBase()
        {
            try
            {
                if(dataPxAct > 0)
                {
                    for(int i = 0; i < dataPxExp; i++)
                    {
                        if(dataPx[i] != null)
                        {
                            // Base should not raise an exception
                            // Instead, it should log the problem and return the packet to the pool
                            base.ProcessPacket(dataPx[i]);
                            dataPx[i] = null;
                            data[i] = null;
                        }
                    }
                }

                if(fecPxAct > 0)
                {
                    for(int i = 0; i < fecPxExp; i++)
                    {
                        int index = dataPxExp + i;

                        if(dataPx[index] != null)
                        {
                            ReturnFecBuffer(dataPx[index].ReleaseBuffer());
                            dataPx[index] = null;
                            data  [index] = null;
                        }
                    }
                }
            }
            finally // Just in case something has gone terribly wrong, attempt to reset
            {
                dataPxAct = 0;
                fecPxAct = 0;
            }
        }
    

        protected void ValidatePayloadType(PayloadType pt)
        {
            if(pt != PayloadType && pt != PayloadType.FEC)
            {
                throw new ArgumentException(Strings.UnexpectedPayloadType);
            }
        }
    

        protected void ReturnDataBuffer(BufferChunk buffer)
        {
            returnBufferHandler(buffer);
        }

        
        protected void ReturnFecBuffer(BufferChunk buffer)
        {
            returnBufferHandler(buffer);
        }


        protected virtual void ResetDecodingState(RtpPacketBase packet)
        {
            if(packet.PayloadType == PayloadType)
            {
                RtpPacket rtpPacket = (RtpPacket)packet;
                minSeq = unchecked((ushort)(rtpPacket.Sequence - rtpPacket.FecIndex));
            }
            else
            {
                minSeq = ((RtpPacketFec)packet).DataRangeMin;
            }

            maxSeq = unchecked((ushort)(minSeq + dataPxExp - 1));
            rangeInitialized = true;
            recoveryIndex = 0;
        }

        
        private void Decode()
        {
            pcDecodePath++;

            try
            {
                fecDecoder.Decode(data, fecPxExp, recovery);
                dataPxAct = dataPxExp; // Trigger ForwardDataPacketsToBase

                // Cast BufferChunks back into RtpPackets
                int recoveryIndex = 0;
                for(int i = 0; i < dataPxExp; i++)
                {
                    if(dataPx[i] == null)
                    {
                        try
                        {
                            dataPx[i] = new RtpPacket(recovery[recoveryIndex]);
                        }
                        catch(InvalidRtpPacketException e)
                        {
                            eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, (int)RtpEL.ID.InvalidPacket);
                            ReturnDataBuffer(recovery[recoveryIndex]);
                        }
                        finally
                        {
                            recovery[recoveryIndex] = null;
                            recoveryIndex++;
                        }
                    }
                }
            }
            catch(ThreadAbortException){} // Because we catch a generic exception next
            catch(Exception e)
            {
                eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, (int)RtpEL.ID.Error);

                // Return recovery packets
                for(int recoveryIndex = 0; recoveryIndex < fecPxAct; recoveryIndex++)
                {
                    ReturnDataBuffer(recovery[recoveryIndex]);
                    recovery[recoveryIndex] = null;
                }
            }
        }

        private void Undecodable()
        {
            // We're undecodable if there are packets we couldn't process
            if(dataPxAct + fecPxAct == 0) return;

            pcUndecodable++;

            // Return any recovery packets we weren't able to use
            // Do this before the call to ForwardDataPacketsToBase
            for(int i = 0; i < fecPxAct; i++)
            {
                ReturnDataBuffer(recovery[i]);
            }

            ForwardDataPacketsToBase();
        }
        
        
        #endregion Private
    }

    #endregion RtpStreamFec

    #region RtpStreamCFec

    internal class RtpStreamCFec : RtpStreamFec
    {
        #region Constructor

        internal RtpStreamCFec(RtpListener rtpListener, uint ssrc, SdesData sdes, PayloadType pt, ushort dataPxExp, ushort fecPxExp) 
            : base(rtpListener, ssrc, sdes, pt)
        {
            pcFecType = 1;

            this.dataPxExp = dataPxExp;
            this.fecPxExp = fecPxExp;

            SetDecoder();
            InitializeDCRStorage();
        }


        #endregion Constructor

        private void SetDecoder()
        {
            if(fecPxExp == 1)
            {
                fecDecoder = new XOR_Fec();
            }
            else
            {
                fecDecoder = new RS_Fec();
            }
        }
    }

    
    #endregion RtpStreamFec

    #region RtpStreamFFec

    internal class RtpStreamFFec : RtpStreamFec
    {
        #region Members

        private int fecPercent;

        private XOR_Fec xorDecoder = new XOR_Fec();
        private RS_Fec rsDecoder = new RS_Fec();

        #endregion Members

        #region Constructor

        internal RtpStreamFFec(RtpListener rtpListener, uint ssrc, SdesData sdes, PayloadType pt, ushort fecPxExp) 
            : base(rtpListener, ssrc, sdes, pt)
        {
            fecPercent = fecPxExp;
            pcFecType = 2;
        }


        #endregion Constructor
        
        protected override void ResetDecodingState(RtpPacketBase packet)
        {
            ushort packetsInFrame;

            if(packet.PayloadType == PayloadType)
            {
                packetsInFrame = ((RtpPacket)packet).PacketsInFrame;
            }
            else
            {
                packetsInFrame = ((RtpPacketFec)packet).PacketsInFrame;
            }

            if(dataPxExp != packetsInFrame)
            {
                // Resize data structures appropriately
                dataPxExp = packetsInFrame;

                fecPxExp = (ushort)(dataPxExp * fecPercent / 100);
                if(dataPxExp * fecPercent % 100 > 0)
                {
                    fecPxExp++;
                }

                fecPxExp = Math.Min(fecPxExp, RS_Fec.MaxChecksumPackets);

                SetDecoder();
                InitializeDCRStorage();
            }

            base.ResetDecodingState(packet);
        }
        
        
        private void SetDecoder()
        {
            if(fecPxExp == 1)
            {
                fecDecoder = xorDecoder;
            }
            else
            {
                fecDecoder = rsDecoder;
            }
        }
    }

    #endregion RtpStreamFec
}
