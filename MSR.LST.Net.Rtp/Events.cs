using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Threading;


namespace MSR.LST.Net.Rtp
{
    /// <summary>
    /// Events contains all the events raised by the Rtp/Rtcp code
    /// 
    /// Each event consists of 4 items - an EventArgs, a Delegate, an Event and an internal method
    /// Raise*Event that actually fires the event, and if the event is not hooked logs information
    /// to the EventLog.
    /// 
    /// Because all our events follow the same pattern, they have been streamlined to call a
    /// single method (FireEvent) to actually fire them.  This method uses the delegate's 
    /// invocation list to call the hooked methods in a non-linked list format
    /// 
    /// Due to our use of the static EventThrower which calls all events from a single thread,
    /// all methods in this class are static as well. They are not meant to be called in a 
    /// multi-threaded way (although it would probably work fine since no static members are
    /// touched).
    /// 
    /// Consideration was given to breaking these down by Rtp, Rtcp and shared (Rtp + Rtcp) events
    /// but due to the limited number of events it wasn't deemed necessary.
    /// 
    /// It would be interesting to query our API users to see how many of them use the events and
    /// which events they use - Rtp, Rtcp or Shared
    /// </summary>
    public class RtpEvents
    {
        #region Members

        private static RtpEL eventLog = new RtpEL(RtpEL.Source.Events);

        #endregion Members
        
        #region Generic Raise*Event Delegate

        /// <summary>
        /// There should be 2 objects in args
        /// 1) object sender;
        /// 2) EventArgs ea; (Of a specialized type for the event being raised)
        /// </summary>
        internal delegate void RaiseEvent(object[] args);

        #endregion Generic Raise*Event Delegate

        #region AppPacketReceived

        // TODO - don't return an AppPacket, return the properties broken out JVE
        public class AppPacketReceivedEventArgs : EventArgs
        {
            public uint SSRC;
            public string Name;
            public byte Subtype;
            public byte[] Data;
        
            public AppPacketReceivedEventArgs(uint ssrc, string name, byte subtype, byte[] data)
            {
                SSRC = ssrc;
                Name = name;
                Subtype = subtype;
                Data = data;
            }
        }

        public delegate void AppPacketReceivedEventHandler(object sender, AppPacketReceivedEventArgs ea);
        
        public static event AppPacketReceivedEventHandler AppPacketReceived;
        
        internal static void RaiseAppPacketReceivedEvent(object[] args)
        {
            FireEvent(AppPacketReceived, args);
        }

        
        #endregion AppPacketReceived
        
        #region DuplicateCNameDetected

        public class DuplicateCNameDetectedEventArgs : EventArgs
        {
            public IPAddress[] IPAddresses;
        
            public DuplicateCNameDetectedEventArgs(IPAddress[] ipAddresses)
            {
                IPAddresses = ipAddresses;
            }
        }

        
        public delegate void DuplicateCNameDetectedEventHandler(object sender, DuplicateCNameDetectedEventArgs ea);
        
        /// <summary>
        /// This event is raised just prior to the RtpSession disposing itself, which will raise
        /// RtpStreamRemoved and RtpParticipantRemoved events.  There is no need for external 
        /// sources to try and clean up the RtpSession although it won't hurt.
        /// </summary>
        public static event DuplicateCNameDetectedEventHandler DuplicateCNameDetected;
        
        internal static void RaiseDuplicateCNameDetectedEvent(object[] args)
        {
            if(!FireEvent(DuplicateCNameDetected, args))
            {
                DuplicateCNameDetectedEventArgs ea = (DuplicateCNameDetectedEventArgs)args[1];

                string message = string.Format(CultureInfo.CurrentCulture, Strings.DuplicateCNameDetected, 
                    ea.IPAddresses[0].ToString(), ea.IPAddresses[1].ToString());

                eventLog.WriteEntry(message, EventLogEntryType.Warning, (int)RtpEL.ID.DuplicateCNameEvent);
            }
        }


        #endregion DuplicateCNameDetected

        #region FrameOutOfSequence

        public class FrameOutOfSequenceEventArgs : EventArgs
        {
            public RtpStream RtpStream;
            public int LostFrames;
            public string Message;
            
            public FrameOutOfSequenceEventArgs(RtpStream rtpStream, int lostFrames, string message)
            {
                RtpStream = rtpStream;
                LostFrames = lostFrames;
                Message = message;
            }
        }

        
        public delegate void FrameOutOfSequenceEventHandler(object sender, FrameOutOfSequenceEventArgs ea);
        
        /// <summary>
        /// FrameOutOfSequence occurs when packets come in out of sequence.  While this occasionally happens due to the network reordering packets,
        /// it is much more likely to occur because packets were either dropped on the receiving PC (lack of CPU or a sudden performance dip is
        /// the general cause) or packets were dropped on the network.
        /// </summary>
        public static event FrameOutOfSequenceEventHandler FrameOutOfSequence;
        
        internal static void RaiseFrameOutOfSequenceEvent(object[] args)
        {
            if(!FireEvent(FrameOutOfSequence, args))
            {
                FrameOutOfSequenceEventArgs ea = (FrameOutOfSequenceEventArgs)args[1];

                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.FrameOutOfSequenceEvent, 
                    ea.RtpStream, ea.LostFrames, ea.Message), EventLogEntryType.Warning, (int)RtpEL.ID.FrameOutOfSequence);
            }
        }


        #endregion FrameOutOfSequence

        #region InvalidPacket

        public class InvalidPacketEventArgs : EventArgs
        {
            public string Reason;
            
            public InvalidPacketEventArgs(string reason)
            {
                Reason = reason;
            }
        }

        public delegate void InvalidPacketEventHandler(object sender, InvalidPacketEventArgs ea);
        
        ///<summary>
        /// When an invalid Rtp Packet is received, this event fires.
        /// 
        /// This should really never occur and is an indication that corrupted data has made its way into the system.  Bad, bad.
        /// </summary>
        public static event InvalidPacketEventHandler InvalidPacket;

        internal static void RaiseInvalidPacketEvent(object[] args)
        {
            if(!FireEvent(InvalidPacket, args))
            {
                InvalidPacketEventArgs ea = (InvalidPacketEventArgs)args[1];

                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.InvalidPacketEvent, ea.Reason), 
                    EventLogEntryType.Error, (int)RtpEL.ID.InvalidPacket);
            }
        }

        
        #endregion InvalidPacket

        #region InvalidPacketInFrame

        public class InvalidPacketInFrameEventArgs : EventArgs
        {
            public RtpStream RtpStream;
            public string Reason;
            public InvalidPacketInFrameEventArgs(RtpStream rtpStream, string reason)
            {
                RtpStream = rtpStream;
                Reason = reason;
            }
        }

        public delegate void InvalidPacketInFrameEventHandler(object sender, InvalidPacketInFrameEventArgs ea);
        
        /// <summary>
        /// If a packet is processed in the RtpFrame logic that just doesn't belong, then an InvalidPacketInFrame event is thrown.
        /// </summary>
        public static event InvalidPacketInFrameEventHandler InvalidPacketInFrame;
        
        internal static void RaiseInvalidPacketInFrameEvent(object[] args)
        {
            if(!FireEvent(InvalidPacketInFrame, args))
            {
                InvalidPacketInFrameEventArgs ea = (InvalidPacketInFrameEventArgs)args[1];
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.InvalidPacketInFrameEvent, 
                    ea.RtpStream, ea.Reason), EventLogEntryType.Error, (int)RtpEL.ID.InvalidPacketInFrame);
            }
        }

        #endregion InvalidPacketInFrame

        #region NetworkTimeout
        
        public class NetworkTimeoutEventArgs : EventArgs
        {
            public string Message;

            public NetworkTimeoutEventArgs(string message)
            {
                Message = message;
            }
        }


        public delegate void NetworkTimeoutEventHandler(object sender, NetworkTimeoutEventArgs ea);
        
        /// <summary>
        /// If no data comes over the network from anywhere for a period of time, this event fires.  Generally this means your modem is on fire
        /// or your ISP has suspended your account due to non-payment.  It can also mean that a network connection was added (such as a dialup
        /// PPP connection) which is rerouting your traffic or you've hit a bug such as multicast traffic going off into the ether once a VPN
        /// connection is established.
        /// </summary>
        public static event NetworkTimeoutEventHandler NetworkTimeout;
        
        internal static void RaiseNetworkTimeoutEvent(object[] args)
        {
            if(!FireEvent(NetworkTimeout, args))
            {
                NetworkTimeoutEventArgs ea = (NetworkTimeoutEventArgs)args[1];

                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, "NetworkTimeoutEvent - {0}", 
                    ea.Message), EventLogEntryType.Warning, (int)RtpEL.ID.NetworkTimeout);
            }
        }

        
        #endregion NetworkTimeout

        #region HiddenSocketException

        public class HiddenSocketExceptionEventArgs : EventArgs
        {
            public RtpSession Session;
            public System.Net.Sockets.SocketException Exception;

            public HiddenSocketExceptionEventArgs( RtpSession session, System.Net.Sockets.SocketException ex )
            {
                Session = session;
                Exception = ex;
            }
        }

        public delegate void HiddenSocketExceptionEventHandler(object sender, HiddenSocketExceptionEventArgs args);

        /// <summary>
        /// When a socket exception is thrown inside an RTCP sender or RTP/RTCP listener, which are on
        /// seperate threads, this event will fire in order to inform the class consuming the RtpSession
        /// that an internal exception has occurred, and the network may be down.  Parsing the exception to
        /// determine the status of the network is the job of the consuming class.  Also, in most cases, the
        /// Rtp object will continue to run, ignoring the exception(s), until it is shutdown by the consuming
        /// class.
        /// </summary>
        public static event HiddenSocketExceptionEventHandler HiddenSocketException;

        internal static void RaiseHiddenSocketExceptionEvent(object[] args)
        {
            FireEvent(HiddenSocketException, args);
        }

        #endregion

        #region PacketOutOfSequence

        public class PacketOutOfSequenceEventArgs : EventArgs
        {
            public RtpStream RtpStream;
            public int LostPackets;
            public string Message;

            public PacketOutOfSequenceEventArgs ( RtpStream rtpStream, int lostPackets, string message )
            {
                RtpStream = rtpStream;
                LostPackets = lostPackets;
                Message = message;
            }
        }

        public delegate void PacketOutOfSequenceEventHandler(object sender, PacketOutOfSequenceEventArgs ea);
        
        /// <summary>
        /// When an RtpStream detects a packet that has arrived out of sequence, it fires this event.
        /// 
        /// Since this is a very commonly occuring event, this event should only be hooked for
        /// diagnostic applications -- rather the RtpStream.PacketOutOfSequenceEvents property or the performance counter should be used
        /// in most cases.
        /// </summary>
        public static event PacketOutOfSequenceEventHandler PacketOutOfSequence;
        
        internal static void RaisePacketOutOfSequenceEvent(object[] args)
        {
            if(!FireEvent(PacketOutOfSequence, args))
            {
                PacketOutOfSequenceEventArgs ea = (PacketOutOfSequenceEventArgs)args[1];

                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                    "PacketOutOfSequenceEvent - RtpStream:{0}, LostPackets:{1}, Message:{2}", 
                    ea.RtpStream, ea.LostPackets, ea.Message), EventLogEntryType.Warning, 
                    (int)RtpEL.ID.PacketOutOfSequence);
            }
        }

        
        #endregion PacketOutOfSequence

        #region RtpParticipantEventArgs

        public class RtpParticipantEventArgs : EventArgs
        {
            public RtpParticipant RtpParticipant;
            
            public RtpParticipantEventArgs(RtpParticipant participant)
            {
                RtpParticipant = participant;
            }
        }

        #endregion RtpParticipantEventArgs
        
        #region RtpParticipantAdded

        public delegate void RtpParticipantAddedEventHandler(object sender, RtpParticipantEventArgs ea);
        /// <summary>
        /// This ia a commonly used event which notifies you when a new RtpParticipant has been detected.  This event is very important if you want to
        /// know who is on the network with you.
        /// </summary>
        public static event RtpParticipantAddedEventHandler RtpParticipantAdded;

        internal static void RaiseRtpParticipantAddedEvent(object[] args)
        {
            RtpParticipantEventArgs ea = (RtpParticipantEventArgs)args[1];

            eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.RtpParticipantAddedEvent, 
                ea.RtpParticipant.ToString()), EventLogEntryType.Information, (int)RtpEL.ID.RtpParticipantAdded);
            
            if(!FireEvent(RtpParticipantAdded, args))
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.EventNotHookedRtpParticipantAdded, 
                    ea.RtpParticipant.ToString()), EventLogEntryType.Warning, (int)RtpEL.ID.EventNotHooked);
            }
        }

        
        #endregion RtpParticipantAdded
        
        #region RtpParticipantRemoved

        public delegate void RtpParticipantRemovedEventHandler(object sender, RtpParticipantEventArgs ea);
        
        /// <summary>
        /// This is a commonly used event which notifies you when a Participant has gone away.  This event is very important if you want to know
        /// when someone leaves the network.
        /// </summary>
        public static event RtpParticipantRemovedEventHandler RtpParticipantRemoved;
        
        internal static void RaiseRtpParticipantRemovedEvent(object[] args)
        {
            // TODO - is the participant disposed before this call, who needs to know this anyhow? JVE
            RtpParticipantEventArgs ea = (RtpParticipantEventArgs)args[1];

            eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.ParticipantRemovedEvent, 
                ea.RtpParticipant.ToString()), EventLogEntryType.Information, (int)RtpEL.ID.RtpParticipantRemoved);

            if(!FireEvent(RtpParticipantRemoved, args))
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.EventNotHookedParticipantRemoved, 
                    ea.RtpParticipant.ToString()), EventLogEntryType.Warning, (int)RtpEL.ID.EventNotHooked);
            }
        }

        
        #endregion RtpParticipantRemoved

        #region RtpParticipantDataChanged
        
        public delegate void RtpParticipantDataChangedEventHandler(object sender, RtpParticipantEventArgs ea);
        
        /// <summary>
        /// Fired when a Participant's Note property changes (a la Instant Messenger status)
        /// </summary>
        public static event RtpParticipantDataChangedEventHandler RtpParticipantDataChanged;
        
        internal static void RaiseRtpParticipantDataChangedEvent(object[] args)
        {
            if(!FireEvent(RtpParticipantDataChanged, args))
            {
                RtpParticipantEventArgs ea = (RtpParticipantEventArgs)args[1];

                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                    Strings.ParticipantStatusChangedEvent, ea.RtpParticipant), EventLogEntryType.Information, 
                    (int)RtpEL.ID.ParticipantStatusChanged);
            }
        }

        
        #endregion RtpParticipantDataChanged

        #region RtpParticipantTimeout

        public delegate void RtpParticipantTimeoutEventHandler(object sender, RtpParticipantEventArgs ea);
        
        /// <summary>
        /// Sometimes you don't receive a BYE from a sender, they just stop transmitting.  Perhaps their BYE packets were lost due to a network,
        /// glitch or perhaps their application crashed.  In this case, a RtpParticipantTimeout event will fire.
        /// </summary>
        public static event RtpParticipantTimeoutEventHandler RtpParticipantTimeout;
        
        internal static void RaiseRtpParticipantTimeoutEvent(object[] args)
        {
            RtpParticipantEventArgs ea = (RtpParticipantEventArgs)args[1];

            eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.ParticipantTimeoutEvent, 
                ea.RtpParticipant.ToString()), EventLogEntryType.Warning, (int)RtpEL.ID.RtpParticipantTimeout);
            
            if(!FireEvent(RtpParticipantTimeout, args))
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.EventNotHookedParticipantTimeout, 
                    ea.RtpParticipant.ToString()), EventLogEntryType.Warning, (int)RtpEL.ID.EventNotHooked);
            }
        }

        
        #endregion RtpParticipantTimeout
        
        #region ReceiverReport

        public class ReceiverReportEventArgs : EventArgs
        {
            public uint rrSSRC;
            public uint dataSourceSSRC;
            public ReceiverReport ReceiverReport;
    
            public ReceiverReportEventArgs(uint rrSSRC, uint dataSourceSSRC, ReceiverReport receiverReport)
            {
                this.rrSSRC = rrSSRC;
                this.dataSourceSSRC = dataSourceSSRC;
                ReceiverReport = receiverReport;
            }
        }

        public delegate void ReceiverReportEventHandler(object sender, ReceiverReportEventArgs ea);
        
        /// <summary>
        /// If you want to analyze other computer's packet reception statistics, this event will deliver incoming Receiver Reports
        /// </summary>
        public static event ReceiverReportEventHandler ReceiverReport;

        internal static void RaiseReceiverReportEvent(object[] args)
        {
            FireEvent(ReceiverReport, args);
        }

        
        #endregion ReceiverReport

        #region SenderReport

        public class SenderReportEventArgs : EventArgs
        {
            public uint ssrc;
            public SenderReport senderReport;

            public SenderReportEventArgs(uint ssrc, SenderReport senderReport)
            {
                this.ssrc = ssrc;
                this.senderReport = senderReport;
            }
        }

        public delegate void SenderReportEventHandler(object sender, SenderReportEventArgs ea);

        /// <summary>
        /// If you want to analyze other computer's packet sending statistics, 
        /// this event will deliver incoming Sender Reports
        /// </summary>
        public static event SenderReportEventHandler SenderReport;

        internal static void RaiseSenderReportEvent(object[] args)
        {
            FireEvent(SenderReport, args);
        }

        #endregion SenderReport

        #region RtpStreamEventArgs

        public class RtpStreamEventArgs : EventArgs
        {
            public RtpStream RtpStream;
            
            public RtpStreamEventArgs(RtpStream rtpStream)
            {
                RtpStream = rtpStream;
            }
        }

        #endregion RtpStreamEventArgs
        
        #region RtpStreamAdded

        public delegate void RtpStreamAddedEventHandler(object sender, RtpStreamEventArgs ea);
        
        /// <summary>
        /// This is a commonly used event which notifies you when a RtpStream has been detected.  
        /// This event is very important if you want to receive data from someone.
        /// </summary>
        public static event RtpStreamAddedEventHandler RtpStreamAdded;
        
        internal static void RaiseRtpStreamAddedEvent(object[] args)
        {
            // Log the event, so we can keep track of stream creation
            RtpStreamEventArgs ea = (RtpStreamEventArgs)args[1];

            eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.RtpStreamAddedEvent, 
                ea.RtpStream.ToString()), EventLogEntryType.Information, (int)RtpEL.ID.RtpStreamAdded);

            if(!FireEvent(RtpStreamAdded, args))
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.EventNotHookedRtpStreamAdded, 
                    ea.RtpStream.ToString()), EventLogEntryType.Warning, (int)RtpEL.ID.EventNotHooked);
            }
        }

        
        #endregion RtpStreamAdded
        
        #region RtpStreamRemoved

        public delegate void RtpStreamRemovedEventHandler(object sender, RtpStreamEventArgs ea);
        
        /// <summary>
        /// This is a commonly used event which notifies you when a RtpStream has gone away.
        /// </summary>
        public static event RtpStreamRemovedEventHandler RtpStreamRemoved;
        
        internal static void RaiseRtpStreamRemovedEvent(object[] args)
        {
            RtpStreamEventArgs ea = (RtpStreamEventArgs)args[1];

            eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.RtpStreamRemovedEvent, 
                ea.RtpStream.ToString()), EventLogEntryType.Information, (int)RtpEL.ID.RtpStreamRemoved);

            if(!FireEvent(RtpStreamRemoved, args))
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.EventNotHookedRtpStreamRemoved, 
                    ea.RtpStream.ToString()), EventLogEntryType.Warning, (int)RtpEL.ID.EventNotHooked);
            }
        }

        
        #endregion RtpStreamRemoved

        #region RtpStreamTimeout
        
        public delegate void RtpStreamTimeoutEventHandler(object sender, RtpStreamEventArgs ea);
        
        /// <summary>
        /// Sometimes you don't receive a BYE from a sender, they just stop transmitting.  Perhaps their BYE packets were lost due to a network,
        /// glitch or perhaps their application crashed.  In this case, a RtpStreamTimeout event will fire.
        /// </summary>
        public static event RtpStreamTimeoutEventHandler RtpStreamTimeout;
        
        internal static void RaiseRtpStreamTimeoutEvent(object[] args)
        {
            RtpStreamEventArgs ea = (RtpStreamEventArgs)args[1];

            eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.RtpStreamTimeoutEvent, 
                ea.RtpStream.ToString()), EventLogEntryType.Warning, (int)RtpEL.ID.RtpStreamTimeout);
            
            if(!FireEvent(RtpStreamTimeout, args))
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.EventNotHookedRtpStreamTimeout, 
                    ea.RtpStream.ToString()), EventLogEntryType.Warning, (int)RtpEL.ID.EventNotHooked);
            }
        }

        
        #endregion RtpStreamTimeout

        #region FireEvent

        /// <summary>
        /// This method is used to actually fire an event
        /// 
        /// It returns true if it called any delegates, false otherwise (event wasn't hooked)
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static bool FireEvent(Delegate del, object[] args)
        {
            bool ret = false;
            
            if(del != null)
            {
                // Invoke the delegates 1 by 1 instead of in linked list
                // this gives them each a chance to execute
                Delegate[] sinks = del.GetInvocationList();

                foreach(Delegate sink in sinks)
                {
                    try
                    {
                        sink.DynamicInvoke(args);
                    }
                    catch(Exception e)
                    {
                        eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                            Strings.ErrorCallingAnEventDelegate, e.ToString()), EventLogEntryType.Error, 
                            (int)RtpEL.ID.Error);
                    }
                }

                ret = true;
            }

            return ret;
        }

        
        #endregion FireEvent
    }
}
