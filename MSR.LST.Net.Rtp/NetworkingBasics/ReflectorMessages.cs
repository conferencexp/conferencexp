using System;
using System.Net;


namespace ReflectorCommonLib
{
    /// <summary>
    /// The message type which could be one of the followings:
    /// 1. Joins: Sent from Client to Server to request joining a multicast group
    /// 2. Leave: Sent from Client to server to request leaving a multicast group
    /// 3. Confirm: Sent from Server to Client to confirm a join or leave.
    /// 4. ReConfirm: Sent from Server to Client when a client sends a join request to a previously 
    ///    joinned multicast group.
    /// 5. LeaveWithoutJoinError: Sent from Server to Client when a client sends a leave without being 
    ///    joint previously.
    /// 6. ConfirmNumMismatchError: Sent from Server to Client when the confirmation number provided by 
    ///    the client does not match the one kept by the server.
    /// 7. UnknownError: Sent from Server to Client when an unexpected error happens.
    /// 8. InvalidRequest: Sent from Server to Client if the message type is neither Join nor Lwave.
    /// </summary>
    public enum MessageType
    {
        Join, Leave, Confirm, ReConfirm, LeaveWithoutJoinError, ConfirmNumMismatchError, 
        UnknownError, InvalidRequest};

    /// <summary>
    /// This type is used to specify whether a traffic type is RTP or RTCP and has been used in the RegistrarServer only.
    /// </summary>
    public enum TrafficType
    {
        None = 0,
        RTP = 1,
        RTCP = 2,
        IPv4 = 4,
        IPv6 = 8,
        Unicast = 16,
        Multicast = 32,
        UCandMC = Unicast | Multicast,
        UCv4RTP =  Unicast | IPv4 | RTP,
        UCv6RTP =  Unicast | IPv6 | RTP,
        MCv4RTP =  Multicast | IPv4 | RTP,
        MCv6RTP =  Multicast | IPv6 | RTP,
        UCv4RTCP = Unicast | IPv4 | RTCP,
        UCv6RTCP = Unicast | IPv6 | RTCP,
        MCv4RTCP = Multicast | IPv4 | RTCP,
        MCv6RTCP = Multicast | IPv6 | RTCP,
        IPv4andIPv6 = IPv4 | IPv6,
        IPv4RTP = IPv4 | RTP,
        IPv4RTCP = IPv4 | RTCP,
        IPv6RTP = IPv6 | RTP,
        IPv6RTCP = IPv6 | RTCP
    }

    /// <summary>
    /// Message Class for communication between RegistrarServer and Client.
    /// </summary>
    [Serializable]
    public class RegisterMessage
    {
        #region Data Members

        /// <summary>
        /// The message type.
        /// </summary>
        public MessageType msgType;

        /// <summary>
        /// The group IP to join/leave by a request or getting confirmed.
        /// </summary>
        public IPAddress groupIP;

        /// <summary>
        /// The group port to join/leave by a request or getting confirmed.
        /// </summary>
        public int groupPort;

        /// <summary>
        /// The confirmation number sent back to the Client when confirming a join or sent to server
        /// when leaving.
        /// </summary>
        public Int32 confirmNumber;
        
        /// <summary>
        /// The RTP unicast port sent back to the Client after a Join request
        /// </summary>
        public int unicastPort;

        #endregion

        #region Constructors
      
        public RegisterMessage(MessageType msgTypeArg, IPAddress groupIPArg) : 
            this(msgTypeArg, groupIPArg, 0, 0, 0){}
      
        public RegisterMessage(MessageType msgTypeArg, IPAddress groupIPArg, int groupPortArg) : 
            this(msgTypeArg, groupIPArg, groupPortArg, 0, 0){}
        
        public RegisterMessage(MessageType msgTypeArg, IPAddress groupIPArg, int groupPortArg, Int32 confirmNumberArg) : 
            this(msgTypeArg, groupIPArg, groupPortArg, confirmNumberArg, 0){}

        /// <summary>
        /// The constructor for the RegisterMessage class. This constructor only initalizes the public 
        /// property values.
        /// </summary>
        public RegisterMessage(MessageType msgTypeArg, IPAddress groupIPArg, int groupPortArg, 
            Int32 confirmNumberArg, int unicastPortArg) 
        {
            msgType = msgTypeArg;
            groupIP = groupIPArg;
            groupPort = groupPortArg;
            confirmNumber = confirmNumberArg;
            unicastPort = unicastPortArg;
        }

        #endregion

        #region Methods

        /// <summary>
        /// A method for debugging purposes which converts a Register Message object to a nice formatted
        /// string.
        /// </summary>
        public override String  ToString() 
        {
            return msgType + " => " + groupIP + ":" + groupPort + " (" + confirmNumber + ")";
        }

        #endregion
    }
}
