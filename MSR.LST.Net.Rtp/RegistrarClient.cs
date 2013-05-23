using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary; // Used to send an object over network

using MSR.LST.Net.Rtp;

using ReflectorCommonLib; // used to get the definition of a Message class.


namespace ReflectorRegistrarClient
{
    /// <summary>
    /// This class should run on a CXP client machine and handles the Join and Leave process by 
    /// interacting with the RegistrarServer.
    /// </summary>
    public class RegistrarClient
    {
        #region Data Members

        /// <summary>
        /// The RegistrarServer's IP address
        /// </summary>
        IPAddress regSrvIP;

        /// <summary>
        /// The returned confirmation number from the reflector.
        /// </summary>
        int refSrvCookie=0;
       
        /// <summary>
        /// Saves the requested group to join.
        /// </summary>
        IPEndPoint joiningMulticastEP=null;

        /// <summary>
        /// An instant of the EventLogWrapper to log reflector related errors.
        /// </summary>
        private RtpEL eventLog = new RtpEL(RtpEL.Source.Reflector);

        /// <summary>
        /// The port number RegistrarServer is waiting for incomming connections 
        /// </summary>
        private int regSrvPort;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor assigns values to regSrvIP and regSrvPort.
        /// </summary>
        /// <param name="regSrvIPArg">The RegistrarServer's IP address</param>
        /// <param name="regSrvPortArg">The port number RegistrarServer is waiting for incomming connections</param>
        public RegistrarClient(IPAddress regSrvIPArg, int regSrvPortArg)
        {
            // Assigning inital values for the RegistrarServer
            regSrvIP = regSrvIPArg;
            regSrvPort = regSrvPortArg;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sends a join request to RegistrarServer and returns the confirmation number. This method opens
        /// a connection to the server sends a request message, receives a response message and closes the
        /// connection to the server.
        /// </summary>
        /// <param name="groupIP">The group IP address to join</param>
        /// <param name="groupPort">The group port number to join</param>
        /// <returns>The RTP unicast port</returns>
        public int join(IPAddress groupIP, int groupPort)
        {
            try
            {
                // Creating and initializing the network related objects.
                TcpClient tcpC = new TcpClient(regSrvIP.AddressFamily);
                tcpC.Connect(regSrvIP, regSrvPort);
                NetworkStream netStream = tcpC.GetStream();
                BinaryFormatter bf = new BinaryFormatter();
                joiningMulticastEP = new IPEndPoint(groupIP, groupPort);

                // Building the message to be sent to RegistrarServer (MessageType: Join)
                RegisterMessage regMsg = new RegisterMessage(MessageType.Join, groupIP, groupPort);

                // Sending the message request object to the RegistrarSErver
                bf.Serialize(netStream,regMsg);

                // Receiving the message response object from the RegistrarServer
                Object obj = bf.Deserialize(netStream);
                regMsg = (RegisterMessage) obj;

                // Closing the server connection.
                tcpC.Close();

                // Saving the confirmation number and RTP unicast port
                refSrvCookie = regMsg.confirmNumber;
                return regMsg.unicastPort;
            } 
            catch(Exception e)
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.JoinRequestToServerFailed, 
                    regSrvIP, regSrvPort, joiningMulticastEP, e.ToString()), System.Diagnostics.EventLogEntryType.Error, 
                    (int)RtpEL.ID.ReflectorJoinFailed);
                throw;
            }
        }

        /// <summary>
        /// Sends a leave request to RegistrarServer for groupIP and groupPort.  This method opens
        /// a connection to the server sends a request message, receives a response message and closes the
        /// connection to the server.
        /// </summary>
        /// <param name="groupIP">The group IP address to leave</param>
        /// <param name="groupPort">The group Port number to leave</param>
        /// <param name="confirmNumber">The confirmation number received from the RegistrarServer at the join time</param>
        public void leave()
        {
            try
            {
                // Creating and initializing the network related objects.
                TcpClient tcpC = new TcpClient(regSrvIP.AddressFamily);
                tcpC.Connect(regSrvIP, regSrvPort);
                NetworkStream netStream = tcpC.GetStream();
                BinaryFormatter bf = new BinaryFormatter();

                // Building the message to be sent to RegistrarServer (MessageType: Leave)
                RegisterMessage regMsg = new RegisterMessage(MessageType.Leave,joiningMulticastEP.Address, joiningMulticastEP.Port, refSrvCookie);

                // Sending the message request object to the RegistrarSErver
                bf.Serialize(netStream,regMsg);

                // Receiving the message response object from the RegistrarServer
                Object obj = bf.Deserialize(netStream);
                regMsg = (RegisterMessage) obj;

                // Closing the server connection
                tcpC.Close();
            } 
            catch (Exception e)
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.LeaveRequestToServerFailed, 
                    regSrvIP, regSrvPort, joiningMulticastEP, e.ToString()), System.Diagnostics.EventLogEntryType.Error, 
                    (int)RtpEL.ID.ReflectorJoinFailed);
            }
        }

        #endregion
    }
}
