#define USE_APP_PACKET

using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;

using Microsoft.Win32;

using MSR.LST.Net;

using ReflectorRegistrarClient;
using System.Collections.Specialized;

namespace MSR.LST.Net.Rtp
{
    public class RtpSession : RtcpSender.IRtpSession,
                              RtcpListener.IRtpSession,
                              RtpSender.IRtpSession,
                              RtpListener.IRtpSession,
                              IDisposable
    {
        #region Statics

        /// <summary>
        /// If desired, the caller should enable the Diagnostics prior to creating RtpSession.
        /// </summary>
        public static bool DiagnosticsEnabled = false;

        /// <summary>
        /// If diagnostics are enabled, the caller should set the Diagnostic Server Host name prior to creating RtpSession.
        /// </summary>
        public static string DiagnosticsServer = null;

        //Default port number for diagnostic service:
        private static readonly int DEFAULT_DIAGNOSTICS_PORT = 1776;
        private static int DiagnosticsPort = DEFAULT_DIAGNOSTICS_PORT;
        //String used to override the port with app.config:
        private static readonly string DIAGNOSTICS_PORT_KEY = "MSR.LST.Net.DiagnosticPort";

        public static readonly IPEndPoint DefaultEndPoint = new IPEndPoint(IPAddress.Parse("234.5.7.9"), 5004);

        private static readonly UTF8Encoding utf8 = new UTF8Encoding();

        internal static readonly Random rnd = new Random();
        
        // TODO - use DF (do not fragment) flag to find out what packet size routers on the path support JVE
        internal const int IPHeaderOverhead = 42;
        internal const int DefaultNetworkTimeout = -1; // we don't like timeouts!
        internal const int RtcpMissedIntervalsTimeout = 5;
        internal static readonly ushort TimeToLive = 255;

        private DateTime lastCleanUp = DateTime.Now;

        static RtpSession()
        {
            // Check for installation
            Install();

            #region App.Config overrides

            string setting;

            #region MTU

            if ((setting = ConfigurationManager.AppSettings[AppConfig.RTP_MTU]) != null) {
                ushort mtu;
                if (ushort.TryParse(setting, out mtu)) {
                    Rtp.MTU = mtu;
                }
            }

            #endregion MTU      
      
            #region TimeToLive

            // Check config file for TTL and FEC settings which override app defaults
            if ((setting = ConfigurationManager.AppSettings[AppConfig.RTP_TimeToLive]) != null)
            {
                ushort ttl = ushort.Parse(setting, CultureInfo.InvariantCulture);
                ValidateTimeToLive(ttl);
                TimeToLive = ttl;
            }

            #endregion TimeToLive



            #endregion App.Config overrides
        }


        #region Installation

        /// <summary>
        /// Ensure that Rtp is properly installed on each run, attempting to make ".NET File Copy Deployment" a reality.
        /// 
        /// This mostly works, but two problems arise from "File Copy Deployment".  First, there's a large delay upon first run while setup executes.
        /// Second, uninstall is never run if the files are just deleted so niggling things like registry entries, performance counters, event logs,
        /// etc. get left around.
        /// 
        /// Because of this, we still deploy the application to the clients using MSIs which call custom actions that run the Installer classes on both
        /// setup and when Add/Remove Programs -> Remove is called.
        /// 
        /// This Dll knows whether to self-install by checking a registry entry that is set upon installation.  If you delete the files without calling
        /// uninstall, be sure to delete HKLM\SOFTWARE\Microsoft Research\RtpInstalled before reinstalling Rtp in another location.  The clean way to
        /// uninstall without MSIs or Add/Remove Programs is to call "installutil /u MSR.LST.Net.Rtp.dll" before deleting the file or by having your
        /// app call Installer.Uninstall on the MSR.LST.Net.Rtp assembly programatically.  See Installation.cs for an example of how to programmatically
        /// install/uninstall an assembly dependency.
        /// </summary>
        private static void Install()
        {
            if (!Installation.Installed)
            {
                // Install myself
                IDictionary state = new Hashtable();
                state.Clear();
                Installation inst = new Installation();
                inst.Install(state);
                inst.Commit(state);
            }
        }


        #endregion

        #region Validate

        internal static void ValidateTimeToLive(ushort ttl)
        {
            if(ttl == 0)
            {
                throw new ArgumentException(Strings.TimeToLiveShouldBePositive);
            }
        }


        #endregion Validate
        
        #endregion Statics

        #region Members
        
        /// <summary>
        /// The local participant for this session
        /// </summary>
        private RtpParticipant participant = null;

        /// <summary>
        /// RtpEndPoint of the "next hop".  For multicast session, this is the multicast group and port.
        /// For reflector-based sessions, this is the address of the reflector.
        /// </summary>
        private IPEndPoint nextHopEP = null;

        /// <summary>
        /// The multicast group,port.  This is different than the nextHopEP for reflector-based sessions.
        /// </summary>
        private IPEndPoint groupEP = null;

        /// <summary>
        /// Flag indicating whether this session is handling Rtp traffic, or only Rtcp traffic
        /// </summary>
        private bool rtpTraffic = false;

        /// <summary>
        /// Flag indicating whether this session is receiving RTP and RTCP data as well as sending it
        /// </summary>
        private bool receiveData = false;

        /// <summary>
        /// The RtcpListener for this session
        /// </summary>
        private RtcpListener rtcpListener = null;

        /// <summary>
        /// The RtcpSender for this session
        /// </summary>
        private RtcpSender rtcpSender = null;

        /// <summary>
        /// The RtpListener for this session, if the session is handling Rtp traffic
        /// </summary>
        private RtpListener rtpListener = null;
       

        private SSRCToParticipantHashtable ssrcToParticipant = new SSRCToParticipantHashtable();
        private IPStreamPairHashtable streamsAndIPs = new IPStreamPairHashtable();
        private CNameToParticipantHashtable participants = new CNameToParticipantHashtable();
        private SSRCToSenderHashtable rtpSenders = new SSRCToSenderHashtable();

        private bool disposed = false;

        // TODO - Revisit where this should be logged JVE
        private RtpEL eventLog = new RtpEL(RtpEL.Source.RtpSession);

        private CompoundPacketBuilder cpb = new CompoundPacketBuilder();

        private Thread threadPerf = null;

        private RtpSessionPC pc;
        private object pcLock = new object();

        /// <summary>
        /// The PacketTransform is a hook that allows for arbitrary packet processing.
        /// This is used to provide encryption.
        /// </summary>
        private PacketTransform packetTransform = null;

        public PacketTransform PacketTransform
        {
            get { return packetTransform; }
            set { packetTransform = value; }
        }

        /// <summary>
        /// An optional attribute, which (if defined), is passed along in RTCP traffic as an APP packet
        /// </summary>
        private String venueName = null;

        public String VenueName
        {
            get { return venueName; }
            set { venueName = value; }
        }

        #endregion Members

        #region Constructors

        /// <summary>
        /// The RtpSession can be in 1 of 4 states:
        /// 
        /// 1. Sending/Receiving Rtp + Rtcp traffic - this is what most users want
        /// 2. Sending/Receiving Rtcp traffic - used mainly by diagnostic tools for discovery of 
        ///     people while also announcing the local user
        /// 3. Receiving Rtcp traffic only - a special diagnostic case used by Pipecleaner to 
        ///     discover if an Agent is already running.
        /// 4. Sending Rtp + Rtcp traffic - a rare case only used for sending test data or
        ///     for "playback" of data in a scenario where SSRC and CNAME conflicts aren't of
        ///     interest to the sender.  THIS SHOULD ONLY BE USED IN EXCEPTIONAL CASES.
        /// 
        /// -If no participant is provided (null), then RtpSession cannot send Rtp or Rtcp data (state 3)
        /// -Else a valid participant is provided and Rtcp traffic can be sent (states 1, 2, or 4)
        ///    -If rtpTraffic is true, then Rtp traffic is sent and/or received (state 1 or 4)
        ///       -If receiveData is true, then Rtp traffic is received as well as sent (state 1)
        ///       -Else Rtp and Rtcp traffic are not received (state 4)
        ///    -Else rtpTraffic is neither sent nor received (state 2)
        /// </summary>
        /// <remarks>
        /// Note that receiving Rtp data without sending Rtcp data is seen as a privacy concern and is not allowed.
        /// </remarks>
        /// <param name="multicastEP">Rtp endpoint, Rtcp endpoint will be derived from it</param>
        /// <param name="participant">Person joining the session</param>
        /// <param name="rtpTraffic">Flag indicating whether or not to monitor or allow sending of Rtp traffic</param>
        /// <param name="receiveData">Flag indicating whether or not to monitor any incoming data</param>
        public RtpSession(IPEndPoint multicastEP, RtpParticipant participant, bool rtpTraffic, bool receiveData)
        {
            #region Parameter Validation

            if(multicastEP == null)
            {
                throw new ArgumentNullException("multicastEP");
            }

            // A null participant is a special state for diagnostic purposes
            // The rtpTraffic flag must be false in order to be consistent
            if(participant == null && rtpTraffic)
            {
                throw new ArgumentException(Strings.IncompatibleArgumentsStatus);
            }

            if(!receiveData && (participant == null || !rtpTraffic))
            {
                throw new ArgumentException(Strings.IncompatibleArgumentsMessageLong);
            }

            #endregion Parameter Validation

            Utility.multicastIP = multicastEP.Address;

            // Store parameters
            this.nextHopEP = multicastEP;
            this.groupEP = multicastEP;
            this.participant = participant;
            this.rtpTraffic = rtpTraffic;
            this.receiveData = receiveData;
            
            // Initialize threads, perf counters, network, etc.
            Initialize();
        }

        /// <summary>
        /// This constructor is the same as "RtpSession(IPEndPoint multicastEP, RtpParticipant participant, bool rtpTraffic, bool receiveData)",
        /// except that it is capable of using a Unicast-Multicast reflector.
        /// </summary>
        /// <param name="multicastEP"></param>
        /// <param name="participant"></param>
        /// <param name="rtpTraffic"></param>
        /// <param name="reflectorEPArg">The IP address and port number of the reflector</param>
        public RtpSession(IPEndPoint multicastEP, RtpParticipant participant, bool rtpTraffic, bool receiveData, IPEndPoint reflectorEP)
        {
            #region Parameter Validation

            if(multicastEP == null)
            {
                throw new ArgumentNullException("multicastEP");
            }

            // A null participant is a special state for diagnostic purposes
            // The rtpTraffic flag must be false in order to be consistent
            if(participant == null && rtpTraffic)
            {
                throw new ArgumentException(Strings.IncompatibleArgumentsStatus);
            }

            if(!receiveData && (participant != null || !rtpTraffic))
            {
                throw new ArgumentException(Strings.IncompatibleArgumentsMessageShort);
            }
            
            if (reflectorEP == null)
            {
                throw new ArgumentNullException(Strings.ReflectorEPNotSetProperly);
            }

            #endregion Parameter Validation

            Utility.multicastIP = multicastEP.Address;

            // Use the reflector as a Unicast EndPoint
            this.nextHopEP = reflectorEP;
            this.groupEP = multicastEP;

            // Same as the "old" three argument constructor ...
            this.participant = participant;
            this.rtpTraffic = rtpTraffic;
            this.receiveData = receiveData;
                       
            // Initialize threads, perf counters, network, etc.
            Initialize();
        }

        
        
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion Constructors

        #region Public

        public IPEndPoint RtpEndPoint
        {
            get
            {
                ValidateRtpTraffic();
                return nextHopEP;
            }
        }

        public bool RtpTraffic
        {
            get{return rtpTraffic;}
        }

        public bool ReceiveData
        {
            get{return receiveData;}
        }


        /// <summary>
        /// Finishes the creation of an RtpSender by adding it to local collections and announcing
        /// it to the remote sites via an Rtcp packet.  Also, add any packet transform functionality.
        /// </summary>
        private void _CreateRtpSender(RtpSender rtpSender)
        {
            if (packetTransform != null)
                rtpSender.PacketTransform = packetTransform;

            // Add it to the collection
            lock(rtpSenders)
            {
                rtpSenders.Add(rtpSender.SSRC, rtpSender);
            }

            // We would like to try and have the stream constructed at the remote sites before the 
            // Rtp data starts arriving so that none of it is missed.  To help with that, when an
            // RtpSender is constructed, it sets an Sdes private extension indicating its payload
            // type.  Here we force an Rtcp compound packet to be sent and give it a little time.
            rtcpSender.SendRtcpDataNow();
            Thread.Sleep(250);
        }
        
        /// <summary>
        /// Creates a basic RtpSender
        /// 
        /// This RtpSender simply sends data over the wire.  It uses the least amount of CPU and
        /// network bandwidth at the cost of reliability (i.e. it makes no attempts to improve data
        /// arrival at the remote site).
        /// </summary>
        /// <param name="Name">string Friendly Name of the device/sender of this RtpStream</param>
        /// <param name="payloadType">Rtp Payload Type</param>
        /// <param name="priExns">Private extensions for this RtpSender/Stream</param>
        public RtpSender CreateRtpSender(string name, PayloadType pt, Hashtable priExns)
        {
            ValidateRtpTraffic();

            // andrew: give the first RTP Sender the local participant's SSRC...
            RtpSender rtpSender = null;

            //if (rtpSenders.Count == 0)
            //    rtpSender = RtpSender.CreateInstance(this, name, pt, priExns,participant.SSRC);
            //else rtpSender = RtpSender.CreateInstance(this, name, pt, priExns);


            rtpSender = RtpSender.CreateInstance(this, name, pt, priExns);

            _CreateRtpSender(rtpSender);

            return rtpSender;
        }

        /// <summary>
        /// Creates an RtpSender with Forward Error Correction
        /// 
        /// This RtpSender sends the data requested by the user, plus some extra data in order to
        /// help recover lost data.  The recovery packets are sent per the specified ratio of
        /// cDataPx : cFecPx, e.g. 3:1 means for every 3 data packets we will send 1 fec packet.
        /// This means we can lose any 1 of the 4 packets and recover the data.  3:2 means we
        /// can lose any 2 of the 5 and still recover the data.
        /// 
        /// It takes 1 FEC packet to recover 1 lost data packet.  It is important to balance the
        /// extra CPU and network bandwidth against the reliability of the data.  Increasing either
        /// CPU or network bandwidth too much may cause worse data loss than not correcting at all.
        /// 
        /// If the FEC ratio uses only 1 FEC packet (2:1, 3:1, etc.) we use an XOR algorithm, which
        /// is very fast.  If the ratio uses more than one FEC packet (3:2, 5:3, etc.), we use the 
        /// Reed-Solomon algorithm.  Reed-Solomon is more complicated and therefore requires more 
        /// CPU, however it allows you to recover from a broader range of loss patterns.
        /// </summary>
        /// <param name="Name">string Friendly Name of the device/sender of this RtpStream</param>
        /// <param name="payloadType">Rtp Payload Type</param>
        /// <param name="priExns">Private extensions for this RtpSender/Stream</param>
        /// <param name="dataPackets">Count of data packets to protect with FEC packets</param>
        /// <param name="perFECPackets">Count of FEC packets to protect data, 1:1 recovery</param>
        public RtpSender CreateRtpSenderFec(string name, PayloadType pt, Hashtable priExns, ushort cDataPx, ushort cFecPx)
        {
            ValidateRtpTraffic();
            RtpSender rtpSender = null;
 

            //rtpSender = RtpSender.CreateInstance(this, name, pt, priExns, cDataPx, cFecPx, ssrc);
            rtpSender = RtpSender.CreateInstance(this, name, pt, priExns, cDataPx, cFecPx);

            _CreateRtpSender(rtpSender);

            return rtpSender;
        }

        public IPAddress MulticastInterface
        {
            get{return participant.IPAddress;}
        }
        

        /// <summary>
        /// SendAppPacket is used to send application specific data to all other RtpListeners on the network.
        /// </summary>
        /// <param name="name">4 ASCII characters</param>
        /// <param name="subtype">0 to 31, app specific "type of data"</param>
        /// <param name="data">data is size sensitive, the total size of the packet cannot exceed 255 bytes</param>
        public void SendAppPacket(uint ssrc, string name, byte subtype, byte[] data, Rtcp.RtcpInterval when)
        {
            if(participant == null)
            {
                throw new RtcpSendingDisabledException(Strings.SendAppPacketNotAllowed);
            }

            // Add the packet to the AppPacket queue
            cpb.Add_APPReport(ssrc, name, subtype, data);

            if(when == Rtcp.RtcpInterval.Now)
            {
                rtcpSender.SendRtcpDataNow();
            }
        }
        
        
        public RtpParticipant Participant(string cName)
        {
            RtpParticipant ret;

            lock(participants)
            {
                ret = participants[cName];
            }

            return ret;
        }

        public RtpParticipant Partcipant(uint ssrc)
        {
            RtpParticipant ret;

            lock (ssrcToParticipant)
            {
                ret = ssrcToParticipant[ssrc];
            }

            return ret;
        }

        public bool ContainsStream(RtpStream stream)
        {
            return (streamsAndIPs.GetStream(stream.SSRC) != null);
        }


        /// <summary>
        /// Returns a clone of the participants collection
        /// 
        /// Used by Pipecleaner agent to display all the participants
        /// </summary>
        public CNameToParticipantHashtable Participants
        {
            get
            {
                CNameToParticipantHashtable ret;

                lock(participants)
                {
                    ret = (CNameToParticipantHashtable)participants.Clone();
                }

                return ret;
            }
        }
        

        public SSRCToStreamHashtable Streams
        {
            get{return (SSRCToStreamHashtable)streamsAndIPs;}
        }

        
        #endregion Public

        #region Private

        #region Initialize / Dispose

        private void Initialize()
        {
            // Initialize objects and performance counters before the threads that use them
            InitializeNetwork();
            InitializePerformanceCounters();
            InitializeThreads();
            InitializeDiagnostics();
        }

        private void InitializeDiagnostics()
        {
            if (DiagnosticsEnabled) {
                if (DiagnosticsServer != null) {
                    string portString = ConfigurationManager.AppSettings[DIAGNOSTICS_PORT_KEY];
                    int port;
                    if (Int32.TryParse(portString, out port)) {
                        DiagnosticsPort = port;
                    }

                    try {
                        IPAddress[] addr = Dns.GetHostAddresses(DiagnosticsServer);
                        rtcpSender.EnableDiagnostics(new IPEndPoint(addr[0], DiagnosticsPort));
                    }
                    catch (Exception) {
                        eventLog.WriteEntry("Diagnostic configuration error", EventLogEntryType.Error, RtpEL.ID.DiagnosticsError);
                    }
                }
            }     
        }

        private void Dispose(bool disposing)
        {
            lock(this)
            {
                if(!disposed)
                {
                    disposed = true;

                    if(disposing)
                    {
                        DisposeThreads();
                        DisposePerformanceCounters();
                        DisposeNetwork();
                    }
                }
            }
        }

        /// <summary>
        /// Matches the signature of the WaitCallback so we can launch it from a ThreadPool thread
        /// during a CNameConflict
        /// </summary>
        /// <param name="state"></param>
        private void Dispose(object state)
        {
            Dispose(true);
        }
        
        private void InitializeThreads()
        {
            // Check to see if user has permissions to read performance counter data
            if (BasePC.PerformanceCounterWrapper.HasUserPermissions())
            {
                threadPerf = new Thread(new ThreadStart(UpdatePerformanceCountersThread));
                threadPerf.IsBackground = true;
                threadPerf.Name = "Performance Counter Thread";
                
                threadPerf.Start();
            }
        }
        
        private void DisposeThreads()
        {
            if(threadPerf != null)
            {
                threadPerf.Abort();
                threadPerf = null;
            }
        }

        private void InitializePerformanceCounters()
        {
            // Check to see if user has permissions to read performance counter data
            if (BasePC.PerformanceCounterWrapper.HasUserPermissions())
            {
                lock(pcLock)
                {
                    if(participant != null)
                    {
                        pc = new RtpSessionPC(participant.CName + " - " + nextHopEP.ToString());
                    }
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
        /// Initialize the Rtcp/Rtp listeners and senders.  See the primary constructor's summary
        /// for an explanation of the "states" an RtpSession can be constructed in.
        /// 
        /// Performance note - it takes .6+ seconds to initialize the performance counter code in
        /// the .Net framework (the first time per process).  In the current code, that happens
        /// during RtcpSender construction.
        /// </summary>
        private void InitializeNetwork()
        {
            if( receiveData )
            {
                if(participant != null && rtpTraffic)
                {
                   rtpListener = new RtpListener(nextHopEP, groupEP, this);
                }

                // RtcpListener can create streams.  A stream needs a valid RtpListener in order to
                // return used packets, so start this object/thread after RtpListener.
                
                rtcpListener = new RtcpListener(RtcpNextHopEndPoint, RtcpGroupEndPoint, this); 
            }

            if(participant != null)
            {
                // Set the participant's IP address
                participant.SetIPAddress = Utility.GetLocalMulticastInterface();

                // Get an ssrc for the local participant
                uint ssrc = NextSSRC();
                AddParticipant(ssrc, participant);
                AddSsrcToIp(ssrc, participant.IPAddress);

                rtcpSender = new RtcpSender(RtcpNextHopEndPoint, this);   
            }
        }


        /// <summary>
        /// Dispose the Rtcp/Rtp listeners and senders
        /// 
        /// Usually one would dispose items in the opposite order from which they were created in
        /// order to maintain symmetry.  However, because objects are created due to data
        /// received from the network, we first shut down the Rtcp/Rtp listeners so as not to
        /// create new objects during shutdown.
        /// 
        /// Since we are no longer listening on the network, we won't receive our own "dispose"
        /// messages that we send out, so we dispose the items manually using the same methods as
        /// if the data had come in off the wire.  This allows for proper eventing and logging.
        /// </summary>
        private void DisposeNetwork()
        {
            if (participant != null) {
                AddBye(participant.SSRC);
                rtcpSender.SendRtcpDataNow();                   
            }

            if(receiveData)
            {
                // Prevent new streams from being created
                rtcpListener.Dispose();
            }

            if(participant != null)
            {
                if(rtpTraffic)
                {
                    DisposeRtpSenders();
                    DisposeRtpStreams();

                    if(receiveData)
                    {
                        rtpListener.Dispose();
                    }
                }

                RemoveParticipant(participant);
                rtcpSender.Dispose();
            }

        }

        private void DisposeRtpSenders()
        {
            lock(rtpSenders)
            {
                foreach(RtpSender rtpSender in ((SSRCToSenderHashtable)rtpSenders.Clone()).Values)
                {
                    // This calls back into RtpSender.IRtpSession.Dispose
                    // Which removes the RtpSender from the rtpSenders collection
                    rtpSender.Dispose(); 
                }
            }
        }

        private void DisposeRtpStreams()
        {
            lock(streamsAndIPs)
            {
                uint[] ssrcs = new uint[streamsAndIPs.Keys.Count];
                streamsAndIPs.Keys.CopyTo(ssrcs, 0);
                foreach(uint ssrc in ssrcs)
                {
                    RtpStream stream = streamsAndIPs.GetStream(ssrc);
                    if( stream != null )
                        RemoveSSRC(ssrc,true);
                }
            }
        }


        #endregion Initialize / Dispose
        
        #region Stream - Add / Update / Stale / Remove 

        /// <summary>
        /// Called to create a stream via Rtcp and have all the "links" created
        /// 
        /// CXP always stores the Participant ssrc/SdesData first in the SdesPacket.  So a
        /// participant should always exist before the stream is created in this code.
        /// 
        /// AddStream is called by the RtcpListener thread via ProcessSdesPacket
        /// </summary>
        private void AddOrUpdateStream(uint ssrc, SdesData sdes)
        {
            lock(participants)
            {
                lock(streamsAndIPs)
                {
                    RtpParticipant participant = participants[sdes.CName];
                    if(participants[sdes.CName] == null)
                    {
                        Debug.Assert(false);
                        throw new InvalidOperationException(Strings.CantCreateAStreamNoParticipant);
                    }

                    IPStreamPairHashtable.IPStreamPair ipsp = streamsAndIPs[ssrc];
                    Debug.Assert(ipsp != null);

                    if(ipsp.stream == null)
                    {
                        ipsp.stream = RtpStream.CreateStream(rtpListener, ssrc, sdes);

                        ipsp.stream.PacketTransform = this.packetTransform;

                        ssrcToParticipant[ssrc] = participant;
                        participant.AddSSRC(ssrc);

                        RaiseRtpStreamAddedEvent(ipsp.stream);
                    }
                    else // Update
                    {
                        ipsp.stream.Stale = 0;
                        ipsp.stream.Properties.UpdateData(sdes);
                    }
                }
            }
        }
        

        /// <summary>
        /// Called on the RtcpSender thread every RtcpIntervals
        /// So that the session can iterate through the streams and see if they are doing anything
        /// </summary>
        private void CheckForStaleStreams()
        {
            lock(streamsAndIPs)
            {
                // Streams are stored in a hashtable, you can't remove an item in a foreach loop
                // So make a copy of the streams to iterate on, but delete from the hashtable
                foreach(IPStreamPairHashtable.IPStreamPair ipsp in new ArrayList(streamsAndIPs.Values))
                {
                    if (ipsp.stream != null && ipsp.stream.Stale++ >= RtcpMissedIntervalsTimeout)
                    {
                        RemoveSSRC(ipsp.stream.SSRC,true);
                    }
                }
            }
        }


        /// <summary>
        /// This method is one extra layer of checks to make sure someone isn't messing with us
        /// (trying to remove streams they don't own by sending BYE packets for instance)
        /// 
        /// If we know the IPAddress of the request to RemoveStream, make sure it is the same
        /// IPAddress that owns the stream
        /// </summary>
        /// <param name="ssrc"></param>
        /// <param name="ip"></param>
        private void RemoveSSRC(uint ssrc, IPAddress ip)
        {
            lock(streamsAndIPs)
            {
                // TODO - perhaps this should just be an SSRCConflict check (or it has already been done) JVE
                if(streamsAndIPs.Contains(ssrc))
                {
                    if(!ip.Equals(streamsAndIPs[ssrc].senderIP))
                    {
                        eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                            Strings.IPAddressTryingToRemoveString, ip.ToString(), ssrc, 
                            streamsAndIPs[ssrc].senderIP.ToString()), EventLogEntryType.Error, 
                            (int)RtpEL.ID.RemoveStreamAttack);

                        return;  // Don't actually remove the stream
                    }
                }
            }

            RemoveSSRC(ssrc,true);
        }

        
        /// <summary>
        /// Called to remove an  ssrc/stream and have all the links cleaned up
        /// 
        /// A stream can either be "associated" (meaning it is mapped to a participant) or not.
        /// 
        /// Whether the stream is associated or not, it will be added to/removed from the "streams"
        /// and "ssrcToIPAddress" collections.
        /// 
        /// If the stream is "associated" it will be added to/removed from the participant's 
        /// streams collection and the "ssrcToParticipant" collection.
        /// </summary>
        /// <param name="stream"></param>
        private void RemoveSSRC(uint ssrc, bool removeParticipant)
        {
            lock(participants)
            {
                RtpParticipant participant = ssrcToParticipant[ssrc];

                if(participant != null)
                {
                    if(ssrc == participant.SSRC && removeParticipant)
                    {
                        RemoveParticipant(participant);
                    }
                    else
                    {
                        lock(streamsAndIPs)
                        {
                            RtpStream stream = streamsAndIPs.GetStream(ssrc);
                            stream.Dispose();

                            // Remove the stream from the associated collections
                            if(participant.SSRCs.Contains(ssrc))
                            {
                                // Mappings between ssrc and Participant
                                 // 1
                                participant.RemoveSSRC(ssrc);
                                // 2
                                ssrcToParticipant.Remove(ssrc);

                                // Only raise the event for a stream that was associated as it might
                                // otherwise confuse clients who never received an AddRtpStreamEvent
                                RaiseRtpStreamRemovedEvent(stream);
                            }
                        }
                    }
                }

                // Remove ssrc/stream from the associated collections 
                lock(streamsAndIPs)
                {
                    // 3
                    streamsAndIPs.Remove(ssrc);
                }
            }
        }

        
        #endregion Stream - Add / Update / Stale / Remove 

        #region Participant - Add / Update / Stale / Remove 

        /// <summary>
        /// AddOrUpdateParticipant is called by the RtpSession ctor for adding the local participant and
        /// by ProcessSdesPacket when an SDES packet arrives on the RtcpListener thread
        /// 
        /// If the participant does not exist in the session, we add them
        /// If the participant does exist in the session, we make sure there is no CName conflict
        /// </summary>
        /// <param name="ssrc">Unique identifier of the stream</param>
        /// <param name="sdes">CName, Name, Email, etc from which to create the Participant</param>
        /// <param name="ip">Originating IP address of the ssrc and SdesData</param>
        private void AddOrUpdateParticipant(uint ssrc, SdesData sdes, IPAddress ip)
        {
            lock(participants)
            {
                string cName = sdes.CName;
                RtpParticipant participant = participants[cName];

                // Participant does not exist
                if(participant == null)
                {
                    // Create a new participant
                    AddParticipant(ssrc, new RtpParticipant(sdes, ip));
                }
                else // Participant exists
                {
                    CheckForCNameConflict(cName, new IPAddress[]{participant.IPAddress, ip});
                    
                    participant.Stale = 0;
                    participant.UpdateData(sdes);
                }
            }
        }

        private void AddParticipant(uint ssrc, RtpParticipant participant)
        {
            lock(participants)
            {
                // Update collections
                participants.Add(participant.CName, participant);
                participant.SSRC = ssrc;
                ssrcToParticipant.Add(ssrc, participant);

                // Raise event
                RaiseParticipantAddedEvent(participant);
            }
        }


        /// <summary>
        /// Check to see if a participant is stale (has not received Rtcp Sdes packets)
        /// </summary>
        private void CheckForStaleParticipants()
        {
            lock(participants)
            {
                // Participants are stored in a hashtable, you can't remove an item in a foreach loop
                // So make a copy of the participants to iterate on, but delete from the hashtable
                foreach(RtpParticipant participant in new ArrayList(participants.Values))
                {
                    if(participant.SSRCs.Count == 0)
                    {
                        if(participant.Stale++ >= RtcpMissedIntervalsTimeout)
                        {
                            RaiseParticipantTimeoutEvent(participant);
                            RemoveParticipant(participant);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Removes a participant and does all the necessary cleanup of streams and associations
        /// </summary>
        /// <param name="participant"></param>
        private void RemoveParticipant(RtpParticipant participant)
        {
            lock(participants)
            {
                if(participants.Contains(participant.CName))
                {
                    foreach(uint ssrc in participant.SSRCs)
                    {
                        if(streamsAndIPs.GetStream(ssrc) != null)
                        {
                            // 3
                            RemoveSSRC(ssrc,false);
                        }

                        // 1
                        participant.RemoveSSRC(ssrc);
                        // 2
                        ssrcToParticipant.Remove(ssrc);
                    }

                    // 4 
                    participants.Remove(participant.CName);
                    // 2
                    ssrcToParticipant.Remove(participant.SSRC);

                    RaiseParticipantRemovedEvent(participant);
                }
            }
        }

        
        #endregion Participant - Add / Update / Stale / Remove 

        #region Add Rtcp data

        private void AddBye(uint ssrc)
        {
            cpb.Add_BYEReport(ssrc);
        }

        private void AddReceiverReport(ReceiverReport rr)
        {
            cpb.Add_ReceiverReport(rr);
        }

        private void AddSenderReport(uint ssrc, SenderReport sr)
        {
            cpb.Add_SenderReport(ssrc, sr);
        }

        private void AddSdes(uint ssrc, SdesData sdes)
        {
            cpb.Add_SDESReport(ssrc, sdes);
        }

        #endregion Add Rtcp data

        #region Process Rtcp Packets

        /// <summary>
        /// Converts a generic RtcpPacket into a specific RtcpPacket type and forwards it to the
        /// appropriate method to be processed
        /// </summary>
        /// <param name="packet">Generic RtcpPacket</param>
        /// <param name="ipAddress">IPAddress packet was received from</param>
        private void ProcessPacket(RtcpPacket packet, IPAddress ipAddress)
        {
            switch(packet.PacketType)
            {
                case (byte)Rtcp.PacketType.SR:
                {
                    ProcessSRPacket(new SrPacket(packet), ipAddress);
                    break;
                }

                case (byte)Rtcp.PacketType.RR:
                {
                    ProcessRRPacket(new RrPacket(packet), ipAddress);
                    break;
                }

                case (byte)Rtcp.PacketType.SDES:
                {
                    ProcessSDESPacket(new SdesPacket(packet), ipAddress);
                    break;
                }

                case (byte)Rtcp.PacketType.BYE:
                {
                    ProcessBYEPacket(new ByePacket(packet), ipAddress);
                    break;
                }

                case (byte)Rtcp.PacketType.APP:
                {
                    ProcessAPPPacket(new AppPacket(packet));
                    break;
                }

                default:
                {
                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                        Strings.ReceivedAnUnhandledRtcpType, packet.ToString()), EventLogEntryType.Warning, 
                        (int)RtpEL.ID.UnhandledRtcpType);
                    break;
                }
            }
        }

        
        /// <summary>
        /// An SdesPacket can contain multiple Sdes reports (ssrc, SdesData) which we process here
        /// 
        /// Due to our architecture with 1 RtpSession / RtpParticipant and multiple RtpSenders / 
        /// RtpStreams (each of which get their own ssrc and SdesData) it is difficult to properly 
        /// map the SdesData to the participant, because they all share the same CName.
        ///  
        /// The participant properties will have a CName, Name, Email, Phone, etc plus any private 
        /// extensions for the participant.  In order to conserve Rtcp bandwidth, the streams will 
        /// only send out CName, Name and any private extensions for the stream. And the stream's 
        /// Name may be completely different from the participant's name.  
        /// 
        /// The problem is that we don't want the participant to be updated from the stream's 
        /// properties due to sharing a CName.  So we use the private extension "Source" to 
        /// distinguish between a participant's SdesData and a Stream's SdesData
        /// 
        /// The last complication is that AccessGrid doesn't have this private extension, probably
        /// because they have a 1:1 mapping between a sender and receiver.  In order to not break
        /// interop, we allow for the case of the private extension not being there, in which case
        /// the participant data will be updated from any and all SdesData that share that CName
        /// </summary>
        /// <param name="packet">Rtcp packet to process</param>
        /// <param name="ipAddress">Originating IP address of the packet</param>
        private void ProcessSDESPacket(SdesPacket packet, IPAddress ipAddress)
        {
            foreach(SdesReport report in packet.Reports())
            {
                uint ssrc = report.SSRC;
                SdesData sdes = report.SdesData;

                // Check for SSRC conflict
                AddSsrcToIp(ssrc, ipAddress);

                // Find out what source generated this data
                string role = sdes.GetPrivateExtension(Rtcp.PEP_SOURCE);

                if(role != null)
                {
                    // Update the participant's properties
                    if(role == Rtcp.PED_PARTICIPANT)
                    {
                        AddOrUpdateParticipant(ssrc, sdes, ipAddress);
                    }
                    else if(role == Rtcp.PED_STREAM)
                    {
                        if(RtpTraffic)
                        {
                            AddOrUpdateStream(ssrc, sdes);
                        }
                    }
                    else
                    {
                        Debug.Assert(false);
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, 
                            Strings.UnsupportedRole, role));
                    }
                }
                else // Doesn't have CXP specific extensions
                {
                    // This code is not currently guaranteed to work
                    Debug.Assert(false);
                }
            }
        }

        
        /// <summary>
        /// A ByePacket can contain multiple SSRCs so we need to process them in a loop
        /// </summary>
        /// <param name="packet">ByePacket</param>
        /// <param name="ipAddress">IPAddress packet was received from</param>
        private void ProcessBYEPacket(ByePacket packet, IPAddress ipAddress)
        {
            // Remove stream does a special form of AddSsrcToIp
            // to try and prevent mischievous programming
            foreach(uint ssrc in packet.SSRCs)
            {
                RemoveSSRC(ssrc, ipAddress);
            }
        }

        private void ProcessSRPacket(SrPacket packet, IPAddress ipAddress)
        {
            AddSsrcToIp(packet.SSRC, ipAddress);

            RaiseSenderReportReceivedEvent(packet.SSRC, packet.SenderReport);
            // TODO - Need SenderReportReceived(RtpStream, SenderReport) JVE
        }

        
        private void ProcessRRPacket(RrPacket packet, IPAddress ipAddress)
        {
            AddSsrcToIp(packet.SSRC, ipAddress);

            foreach(ReceiverReport rr in packet.ReceiverReports)
            {
                    // AJW: always raise an event, regardless of whether we know the corresponding
                    // CNAME...
                RaiseReceiverReportEvent(packet.SSRC, rr.SSRC, rr);

            }
        }

        
        private void ProcessAPPPacket(AppPacket packet)
        {
            RaiseAppPacketReceivedEvent(packet.SSRC, packet.Name, packet.Subtype, packet.Data);
        }

        
        #endregion Process Rtcp Packets

        #region SSRC Conflict

        /// <summary>
        /// Verifies that the SSRC matches the IPAddress, if this is a known SSRC, and returns the
        /// associated RtpStream.  If the SSRC is new, we add the IPAddress to the lookup table.
        /// </summary>
        private RtpStream AddSsrcToIp(uint ssrc, IPAddress ipAddress)
        {
            IPStreamPairHashtable.IPStreamPair ipsp;
            lock(streamsAndIPs)
            {
                ipsp = streamsAndIPs[ssrc];
                if(ipsp != null)
                {
                    if (!ipsp.senderIP.Equals(ipAddress))
                    {
                        HandleSSRCConflict(ssrc, ipAddress);
                    }
                }
                else
                {
                    ipsp = new IPStreamPairHashtable.IPStreamPair(ipAddress, null);
                    streamsAndIPs.Add(ssrc, ipsp);
                }
            }

            return ipsp.stream;
        }

        
        // TODO - understand this logic so I can adapt it to new OM - JVE
        private void HandleSSRCConflict(uint ssrc, IPAddress ipAddress)
        {

            if(rtpSenders.ContainsKey(ssrc))
            {
                RtpSender sender = rtpSenders[ssrc];
                sender.SSRCConflictDetected();
                
                // update hash table
                rtpSenders[ssrc] = null;
                rtpSenders[sender.SSRC] = sender;

                // update the local particpant, if necessary
                if (ssrc == participant.SSRC)
                {
                    participant.SSRC = sender.SSRC;
                    ssrcToParticipant[ssrc] = null;
                    ssrcToParticipant[participant.SSRC] = participant;
                }

               
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                    Strings.SSRCConflictDetectedLocalSender, ipAddress.ToString()), EventLogEntryType.Warning, 
                    (int)RtpEL.ID.SSRCConflictDetected);
            }
            else
            {
                // SSRC conflict is between two remote Streams, do nothing and let them take care of it.
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                    Strings.SSRCConflictDetectedRemoteMachines, ipAddress.ToString()), EventLogEntryType.Warning, 
                    (int)RtpEL.ID.SSRCConflictDetected);
            }     
        }
        

        #endregion SSRC Conflict

        #region CNameConflict

        /// <summary>
        /// Determine if there is a CNameConflict between the local participant and a remote
        /// participant or 2 remote participants.
        /// 
        /// If the conflict involves a local participant, raise the event and then Dispose the
        /// RtpSession, bringing it back to a clean state.
        /// 
        /// If it is a remote conflict, do nothing, as we will eventually receive the Rtcp data
        /// from them.
        /// </summary>
        private void CheckForCNameConflict(string cname, IPAddress[] ipAddresses)
        {
            Trace.Assert(ipAddresses.Length == 2);

            // If the IPAddresses don't match
            if (!ipAddresses[0].Equals(ipAddresses[1]))
            {
                // Is the conflict with the local participant
                if (participant.CName == cname)
                {
                    RaiseDuplicateCNameDetectedEvent(ipAddresses);

                    // WARNING:
                    // You can not call Dispose from this thread, as it will terminate itself!
                    ThreadPool.QueueUserWorkItem(new WaitCallback(Dispose));
                }
                else // Or remote streams
                {
                    // Don't do anything here.  The machines encountering the conflict will dispose
                    // themselves and we will receive the Rtcp updates about them leaving the session

                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                        Strings.CNameConflictDetectedRemoteMachines, ipAddresses[0], ipAddresses[1], cname), 
                        EventLogEntryType.Warning, (int)RtpEL.ID.HandleCNameConflict);
                }
            }
        }

        
        #endregion CNameConflict
        
        #region Performance Counters

        private void UpdatePerformanceCountersThread()
        {
            int interval = 1000; // Milliseconds

            while(!disposed)
            {
                Thread.Sleep(interval);

                // Session
                UpdatePerformanceCounters(interval);

                // Rtcp
                if(participant != null)
                {
                    rtcpSender.UpdatePerformanceCounters(interval);
                }

                // Rtp
                if(rtpTraffic)
                {
                    // Listener
                    if(receiveData)
                    {
                        rtpListener.UpdatePerformanceCounters(interval);
                    }

                    // Senders
                    lock(rtpSenders)
                    {
                        foreach(RtpSender sender in rtpSenders.Values)
                        {
                            sender.UpdatePerformanceCounters(interval);
                        }
                    }

                    // Streams
                    lock(streamsAndIPs)
                    {
                        foreach(IPStreamPairHashtable.IPStreamPair ipsp in streamsAndIPs.Values)
                        {
                            if( ipsp.stream != null )
                            {
                                ipsp.stream.UpdatePerformanceCounters(interval);
                            }
                        }
                    }
                }
            }
        }
        
        
        /// <summary>
        /// Updates the local perf counters
        /// </summary>
        /// <param name="ms">An approximate delay in milli-seconds since the last update</param>
        private void UpdatePerformanceCounters(int ms)
        {
            lock(pcLock)
            {
                if(pc != null) // Null if participant == null (Rtcp listening only)
                {
                    pc[RtpSessionPC.ID.NetworkTimeoutEvents] = networkTimeoutEvents;
                    pc[RtpSessionPC.ID.Participants] = participants.Count;
                    pc[RtpSessionPC.ID.RtpStreams] = streamsAndIPs.Count - participants.Count;
                    pc[RtpSessionPC.ID.RtpStreamTimeoutEvents] = rtpStreamTimeoutEvents;
                    pc[RtpSessionPC.ID.InvalidPacketEvents] = invalidPacketEvents;

                    pc[RtpSessionPC.ID.Ssrc] = (int)participant.SSRC;

                    pc[RtpSessionPC.ID.EventThrowerQueueLength] = EventThrower.WorkItemQueueLength;
                    pc[RtpSessionPC.ID.PeakEventQueueLength] = EventThrower.PeakEventQueueLength;
                }
            }
        }

        
        #endregion Performance Counters

        #region Miscellaneous Helper Methods

        private void LogEvent(string source, string msg, EventLogEntryType et, int id)
        {
            eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.OnBehalfOf, source, msg), et, 
                id);
        }

        
        private IPEndPoint RtcpGroupEndPoint
        {
            get
            {
                return new IPEndPoint(groupEP.Address, groupEP.Port + 1);
            }
        }

        private IPEndPoint RtcpNextHopEndPoint
        {
            get
            {
                return new IPEndPoint(nextHopEP.Address, nextHopEP.Port + 1);
            }
        }

        private void ValidateRtpTraffic()
        {
            if(!rtpTraffic)
            {
                throw new RtpTrafficDisabledException(Strings.CannotAccessRtpMethods);
            }
        }

        
        private uint NextSSRC()
        {
            uint potentialSSRC;

            do
            {
                potentialSSRC = (uint)rnd.Next(1, int.MaxValue) * (uint)rnd.Next(1,2);
            }
            while(streamsAndIPs.ContainsKey(potentialSSRC));

            return potentialSSRC;
        }
        

        #endregion Miscellaneous Helper Methods

        #endregion Private

        #region Interface Implementations

        #region RtcpSender.IRtpSession

        /// <summary>
        /// Called by RtcpSender when it is time to collect Rtcp data
        /// </summary>
        /// <returns>A CompoundPacketBuilder</returns>
        CompoundPacketBuilder RtcpSender.IRtpSession.RtcpReportIntervalReached()
        {
            // Limit the frequency of the cleanup in order to avoid participants and streams being
            // inappropriately designated as stale.  This can happen if we start up a lot of senders at once.
            // Each sender triggers an RTCP packet which without the frequency limitation could 
            // increment the stale counter to its limit and cause the participant or stream to be removed.
            bool cleanUpNow = false;
            if (DateTime.Now > this.lastCleanUp.AddSeconds(5)) {
                cleanUpNow = true;
                this.lastCleanUp = DateTime.Now;
            }

            // A stale participant is one who is not sending Rtcp data
            if (cleanUpNow)
                CheckForStaleParticipants();

            // Add participant data
            Debug.Assert(participant.SSRC != 0);
            cpb.ParticipantData(participant);

            // Add Rtp data
            if(rtpTraffic)
            {
                // A stale stream is one not sending Rtp traffic
                if (cleanUpNow)
                    CheckForStaleStreams();

                // Collect SenderReportPackets and SDESReports from each Sender
                lock(rtpSenders)
                {

                    foreach(RtpSender sender in rtpSenders.Values)
                    {
                        // this adds the SR and SDES packets
                        sender.UpdateRtcpData();
                    }
                }

                // add a sdes (source description)
                //cpb.Add_SDESReport(participant.SSRC, participant);

                // Collect ReceiverReports from each of the streams
                lock(streamsAndIPs)
                {
                    foreach(IPStreamPairHashtable.IPStreamPair ipsp in streamsAndIPs.Values)
                    {
                        if( ipsp.stream != null )
                        {
                            ipsp.stream.AddReceiverReport(cpb);
                        }
                    }
                }
            }



#if USE_APP_PACKET
            // andrew: Add an app packet that reflects the time and the current venue...
            if ((VenueName != null) && (this.groupEP != null))
            {
                string appPayload = VenueName + "#" + this.groupEP.Address.ToString();
                long time = DateTime.Now.Ticks;
                // 8  bytes for time + venue name
                int count = 8 + utf8.GetByteCount(appPayload);

                // count must be 4-byte alligned
                int mod = count % 4;
                if (mod != 0)
                {
                    count += (4 - mod);
                }

                BufferChunk buffer = new BufferChunk(count);
                buffer.Reset(0, count);
                buffer.Clear();

                buffer.SetInt64(0, time);
                buffer.SetUTF8String(8, appPayload);
                byte[] rawBytes = buffer.Buffer;

                cpb.Add_APPReport(participant.SSRC, Rtcp.APP_PACKET_NAME, Rtcp.VENUE_APP_PACKET_SUBTYPE, rawBytes);
            }
#endif
            return cpb;
        }
        
        void RtcpSender.IRtpSession.LogEvent(string source, string msg, EventLogEntryType et, int id)
        {
            LogEvent(source, msg, et, id);
        }


        int RtcpSender.IRtpSession.ParticipantCount
        {
            get{return participants.Count;}
        }

        uint RtcpSender.IRtpSession.SSRC
        {
            get{return participant.SSRC;}
        }

        string RtcpSender.IRtpSession.CName
        {
            get{return participant.CName;}
        }

        //IPEndPoint RtcpSender.IRtpSession.RtcpEndPoint
        //{
        //    get{return RtcpEndPoint;}
        //}

        ushort RtcpSender.IRtpSession.TTL
        {
            get{return TimeToLive;}
        }
        
        #endregion RtcpSender.IRtpSession

        #region RtcpListener.IRtpSession

        void RtcpListener.IRtpSession.ProcessCompoundPacket(CompoundPacket cp, IPAddress ip)
        {
            foreach(RtcpPacket packet in cp)
            {
                ProcessPacket(packet, ip);
            }
        }

        void RtcpListener.IRtpSession.LogEvent(string source, string msg, EventLogEntryType et, int id)
        {
            LogEvent(source, msg, et, id);
        }

        void RtcpListener.IRtpSession.RaiseNetworkTimeoutEvent(string source)
        {
            RaiseNetworkTimeoutEvent(source);
        }

        
        int RtcpListener.IRtpSession.DropPacketsPercent
        {
            get{return 0;}
        }

        Random RtcpListener.IRtpSession.Rnd
        {
            get{return rnd;}
        }
       
        #endregion RtcpListener.IRtpSession

        #region RtpSender.IRtpSession

        void RtpSender.IRtpSession.AddBye(uint ssrc)
        {
            AddBye(ssrc);
        }

        void RtpSender.IRtpSession.AddSdes(uint ssrc, SdesData sdes)
        {
            AddSdes(ssrc, sdes);
        }

        void RtpSender.IRtpSession.AddSenderReport(uint ssrc, SenderReport sr)
        {
            AddSenderReport(ssrc, sr);
        }

        uint RtpSender.IRtpSession.NextSSRC()
        {
            return NextSSRC();
        }

        
        SdesData RtpSender.IRtpSession.Sdes
        {
            get{return participant;}
        }

        IPEndPoint RtpSender.IRtpSession.RtpEndPoint
        {
            get{return nextHopEP;}
        }


        void RtpSender.IRtpSession.Dispose(uint ssrc)
        {
            lock(rtpSenders)
            {
                if(!rtpSenders.Contains(ssrc))
                {
                    string msg = string.Format(CultureInfo.CurrentCulture, Strings.RtpSessionDoesNotContainASender, 
                        ssrc.ToString(CultureInfo.InvariantCulture));

                    Debug.Assert(false, string.Format(CultureInfo.CurrentCulture,
                        "RtpSession does not contain a sender for the given SSRC - {0}", 
                        ssrc.ToString(CultureInfo.InvariantCulture)));
                    throw new ArgumentOutOfRangeException(msg);
                }

                rtpSenders.Remove(ssrc);
            }
        }
        ushort RtpSender.IRtpSession.TTL
        {
            get{return TimeToLive;}
        }

        
        #endregion RtpSender.IRtpSession

        #region RtpListener.IRtpSession
        
        RtpStream RtpListener.IRtpSession.GetStream(uint ssrc, IPAddress ip)
        {
            return AddSsrcToIp(ssrc, ip);
        }

        void RtpListener.IRtpSession.RaiseInvalidPacketEvent(string msg)
        {
            RaiseInvalidPacketEvent(msg);
        }
        
        SdesData RtpListener.IRtpSession.Sdes
        {
            get{return participant;}
        }

        void RtpListener.IRtpSession.LogEvent(string source, string msg, EventLogEntryType et, int id)
        {
            LogEvent(source, msg, et, id);
        }
        
        int RtpListener.IRtpSession.DropPacketsPercent
        {
            get{return 10;}
        }

        Random RtpListener.IRtpSession.Rnd
        {
            get{return rnd;}
        }
        
        #endregion RtpListener.IRtpSession

        #endregion Interface Implementations

        #region Events
        
        #region NetworkTimeoutEvent

        private int networkTimeoutEvents = 0;
        
        internal void RaiseNetworkTimeoutEvent(string source)
        {
            networkTimeoutEvents++;

            object[] args = {this, new RtpEvents.NetworkTimeoutEventArgs(source)};
            EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RtpEvents.RaiseNetworkTimeoutEvent), args);
        }

        
        #endregion NetworkTimeoutEvent
        
        #region ParticipantAddedEvent

        internal void RaiseParticipantAddedEvent(RtpParticipant participant)
        {
            object[] args = {this, new RtpEvents.RtpParticipantEventArgs(participant)};
            EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RtpEvents.RaiseRtpParticipantAddedEvent), args);
        }

        
        #endregion ParticipantAddedEvent
        
        #region ParticipantRemovedEvent

        internal void RaiseParticipantRemovedEvent(RtpParticipant participant)
        {
            object[] args = {this, new RtpEvents.RtpParticipantEventArgs(participant)};
            EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RtpEvents.RaiseRtpParticipantRemovedEvent), args);
        }

        
        #endregion ParticipantRemovedEvent
        
        #region ParticipantTimeoutEvent

        private void RaiseParticipantTimeoutEvent(RtpParticipant participant)
        {
            object[] args = {this, new RtpEvents.RtpParticipantEventArgs(participant)};
            EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RtpEvents.RaiseRtpParticipantTimeoutEvent), args);
        }

        
        #endregion ParticipantTimeoutEvent
        
        #region RtpStreamAddedEvent

        internal void RaiseRtpStreamAddedEvent(RtpStream stream)
        {
            object[] args = {this, new RtpEvents.RtpStreamEventArgs(stream)};
            EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RtpEvents.RaiseRtpStreamAddedEvent), args);
        }

        
        #endregion RtpStreamAddedEvent
        
        #region RtpStreamRemovedEvent

        internal void RaiseRtpStreamRemovedEvent(RtpStream stream)
        {
            object[] args = {this, new RtpEvents.RtpStreamEventArgs(stream)};
            EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RtpEvents.RaiseRtpStreamRemovedEvent), args);
        }

        
        #endregion RtpStreamRemovedEvent
        
        #region RtpStreamTimeoutEvent
        
        private int rtpStreamTimeoutEvents = 0;
        
        internal void RaiseRtpStreamTimeoutEvent(RtpStream stream)
        {
            rtpStreamTimeoutEvents++;

            object[] args = {this, new RtpEvents.RtpStreamEventArgs(stream)};
            EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RtpEvents.RaiseRtpStreamTimeoutEvent), args);
        }

        
        #endregion RtpStreamTimeoutEvent

        #region RaiseReceiverReportEvent

        private void RaiseReceiverReportEvent(uint rrSSRC, uint dataSourceSSRC, ReceiverReport rr)
        {
            object[] args = {this, new RtpEvents.ReceiverReportEventArgs(rrSSRC, dataSourceSSRC, rr)};
            EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RtpEvents.RaiseReceiverReportEvent), args);
        }

        
        #endregion RaiseReceiverReportEvent

        #region RaiseAppPacketReceivedEvent

        private void RaiseAppPacketReceivedEvent(uint ssrc, string name, byte subtype, byte[] data)
        {
            object[] args = {this, new RtpEvents.AppPacketReceivedEventArgs(ssrc, name, subtype, data)};
            EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RtpEvents.RaiseAppPacketReceivedEvent), args);
        }

        
        #endregion RaiseAppPacketReceivedEvent

        #region RaiseSenderReportReceivedEvent
        private void RaiseSenderReportReceivedEvent(uint ssrc,SenderReport senderReport)
        {
            object[] args = { this, new RtpEvents.SenderReportEventArgs(ssrc, senderReport) };

            EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RtpEvents.RaiseSenderReportEvent), args);
        }

        #endregion RaiseSenderReportReceivedEvent
        #region RaiseDuplicateCNameDetectedEvent

        private void RaiseDuplicateCNameDetectedEvent(IPAddress[] ipAddresses)
        {
            object[] args = {this, new RtpEvents.DuplicateCNameDetectedEventArgs(ipAddresses)};
            EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RtpEvents.RaiseDuplicateCNameDetectedEvent), args);
        }

        
        #endregion RaiseDuplicateCNameDetectedEvent

        #region InvalidPacketEvent

        // TODO I Imagine this event creates more noise than anything useful, imagine millions of packets... JVE

        private int invalidPacketEvents = 0;

        private void RaiseInvalidPacketEvent(string msg)
        {
            invalidPacketEvents++;

            object[] args = {this, new RtpEvents.InvalidPacketEventArgs(msg)};
            EventThrower.QueueUserWorkItem(new RtpEvents.RaiseEvent(RtpEvents.RaiseInvalidPacketEvent), args);
        }

        #endregion InvalidPacketEvent

        #endregion Events

        #region Exceptions

        /// <summary>
        /// SessionExeption is thrown if anything goes wrong in the session object
        /// </summary>
        public class SessionException : ApplicationException
        {
            public SessionException(string msg) : base(msg) {}
        }
        /// <summary>
        /// RtpTrafficDisabledException is thrown if Rtp methods in the Session are accessed
        /// while in rtpTraffic == false mode
        /// </summary>
        public class RtpTrafficDisabledException : ApplicationException
        {
            public RtpTrafficDisabledException(string msg) : base(msg) {}
        }
        /// <summary>
        /// RtcpSendingDisabledException is thrown if Rtcp sender methods in the Session are accessed
        /// while participant == null
        /// </summary>
        public class RtcpSendingDisabledException : ApplicationException
        {
            public RtcpSendingDisabledException(string msg) : base(msg) {}
        }
       

        #endregion Exceptions

        #region Fault Injection
        
        #if FaultInjection
        private int dropPacketsReceivedPercent = 0;

        public int DropPacketsReceivedPercent
        {
            get{return dropPacketsReceivedPercent;}
            
            set
            {
                if ( value < 0 || value > 100)
                {
                    throw new ArgumentException("DropPacketsReceivedPercent must be between 0 and 100");
                }

                dropPacketsReceivedPercent = value;
            }
        }
        #endif

        #endregion

    }
}
