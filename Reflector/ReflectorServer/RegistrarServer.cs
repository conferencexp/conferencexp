using System; // Used for Console and other basic definitions
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Net; // Used for IPEndPoint, IPAddress
using System.Net.Sockets; // Used for TcpClient, TcpListener, Socket and NetworkStream.
using System.Runtime.Serialization.Formatters.Binary; // Used for sending an object through the network stream
using System.Threading;

using MSR.LST.Net;

using ReflectorCommonLib;
using System.Collections.Generic;
using edu.washington.cs.cct.cxp.utilities;
using System.Configuration;  // Reflector Common Library used to share the message class definition between client and server.


namespace MSR.LST.ConferenceXP.ReflectorService
{
    /// <summary>
    /// Represents an unicast client entry in the clientRegTable
    /// </summary>
    [Serializable]
    public class ClientEntry
    {
        #region Members

        // andrew: clients need a port number to work with NATs
        private IPEndPoint clientEP;
        private IPEndPoint groupEP;
        private DateTime joinTime;

        #endregion Members

        #region Constructors

        internal ClientEntry(IPEndPoint clientIP, IPEndPoint groupEP, DateTime joinTime)
        {
            this.clientEP = clientIP;
            this.groupEP = groupEP;
            this.joinTime = joinTime;
        }

        
        #endregion Constructors

        #region Public 

        public IPEndPoint ClientEP
        {
            get
            {
                return clientEP;
            }
        }        

        public IPEndPoint GroupEP
        {
            get
            {
                return groupEP;
            }
        }

        public DateTime JoinTime
        {
            get
            {
                return joinTime;
            }
        }

        public override string ToString()
        {
            string time = joinTime.ToString();

            return clientEP.ToString() + "\t\t " + groupEP.ToString() + "             " + time;
        }

        #endregion Public

    }
    
    
    
    /// <summary>
    /// RegistrarServer will handle registering unicast enabled clients to join multicast groups.
    /// This class communicates with other classes only through clientRegTable. 
    /// </summary>
    public class RegistrarServer
    {

        private static readonly int CLIENT_ENTRY_TIMEOUT = 30000; // 30 seconds
        private static readonly int MULTICAST_GROUP_TIMEOUT = 60000; //60 seconds
        private static readonly int MULTICAST_NODE_TIMEOUT = 5000; //5 seconds

        /// <summary>
        /// This dataTable is used to keep track of multicast disabled clients who are
        /// joining different groups. The table schema is as follows:
        ///            
        /// | Client IP | Client Port | Group IP | Group Port | Join Time | Confirm. Num. |                     
        /// 
        /// The key is the Client IP and Client Port.
        /// 
        /// </summary>
        private readonly IDictionary<IPEndPoint, ClientEntry> clientRegTable =
            new TimeoutDictionary<IPEndPoint, ClientEntry>(CLIENT_ENTRY_TIMEOUT);

        /// <summary>
        /// The reflector does not allow a single remote IP address to send data over both unicast and multicast.
        /// This is to prevent an accidental or malicious packet flood, in which multiple reflectors send
        /// each other data over both unicast and multicast.  In this case, each inbound packet generates two
        /// outbound packets -- clearly, this is very bad.
        /// 
        /// Conceivably, this could create problems for NAT-enabled networks, in which multiple client machines
        /// share an IP address.  However, in most cases, multicast will work for all or none of the NATed nodes.
        /// This assumption will break if some NATed clients use multicast, and some use the reflector.
        /// 
        /// Note that the timeout also impacts the normal use case of a node using multicast changing to a 
        /// reflector.  The timeout should be short enough so as not to cause confusion.
        /// 
        /// This is a dictionary simply because the set data structure is not supported.
        /// </summary>
        private readonly IDictionary<IPAddress, IPAddress> multicastEnabledIPs =
            new TimeoutDictionary<IPAddress, IPAddress>(MULTICAST_NODE_TIMEOUT);

        private IDictionary<IPAddress, IPAddress> subscribedMulticastGroups =
            new Dictionary<IPAddress, IPAddress>();

        /// <summary>
        /// Upstream reflector in a multi-reflector setup...
        /// </summary>
        private IPAddress ParentReflector = null;

        public IDictionary<IPEndPoint,ClientEntry> ClientRegTable
        {
            get { return clientRegTable; }
        }

        public void MarkIPAddressAsUsingMulticast(IPAddress ipaddr)
        {
            multicastEnabledIPs.Add(ipaddr, ipaddr);
        }

        public bool IsIPAddressUsingMulticast(IPAddress ipaddr)
        {
            return multicastEnabledIPs.ContainsKey(ipaddr);
        }

        /// <summary>
        /// This method gets a group IP address and port as the input and returns a list of IP addresses 
        /// which have joined this group IP and port
        /// </summary>
        public void MemberLookup(IList<IPEndPoint> members, IPEndPoint groupEP)
        {
            if (members == null || groupEP == null)
            {
                throw new NullReferenceException(Strings.ArrayListAndIPAddressMustBeInitiated);
            }

            members.Clear();

            lock (clientRegTable)
            {
                foreach (ClientEntry entry in clientRegTable.Values)
                {
                    if (groupEP.Equals(entry.GroupEP))
                    {
                        members.Add(entry.ClientEP);
                    }
                }
            }
        }
        
        
        /// <summary>
        /// This method gets an IP address as the input and returns the group IP and port 
        /// which this client has joined before.
        /// </summary>
        public IPEndPoint GroupLookup(IPEndPoint clientEP)
        {
            lock (clientRegTable)
            {
                if (clientRegTable.ContainsKey(clientEP))
                {
                    ClientEntry entry = clientRegTable[clientEP];
                    return entry.GroupEP;
                }
                else return null;
            }

        }

        private ReflectorEventLog eventLog = new ReflectorEventLog(ReflectorEventLog.Source.RegistrarServer);

        public RegistrarServer() {
            Timer timer = new Timer(MulticastGroupGarbageCollector, null, 
                MULTICAST_GROUP_TIMEOUT, MULTICAST_GROUP_TIMEOUT);

            // initialize parent reflector, if any
            String setting = ConfigurationManager.AppSettings[AppConfig.ParentReflector];
            if (setting != null)
            {
                try
                {
                    IPAddress[] addrs = Dns.GetHostAddresses(setting);
                    ParentReflector = addrs[0];
                }
                catch (Exception e) 
                {
                    eventLog.WriteEntry("Failed to initialize parent reflector: " + e,
                        EventLogEntryType.Warning,ReflectorEventLog.ID.Error);
                }
            }
        }

        /// <summary>
        /// Background thread that periodically un-subscribes from inactive multicast groups
        /// </summary>
        private void MulticastGroupGarbageCollector(object state)
        {
            IDictionary<IPAddress, IPAddress> trulyActiveGroups = new Dictionary<IPAddress, IPAddress>();

            lock (clientRegTable)
            {
                lock (subscribedMulticastGroups)
                {

                    ICollection<ClientEntry> entries = clientRegTable.Values;                 
                    foreach (ClientEntry entry in entries)          
                    {
                        trulyActiveGroups.Add(entry.GroupEP.Address, entry.GroupEP.Address);   
                    }

                    foreach (IPAddress addr in subscribedMulticastGroups.Keys)
                    {
                        if (!trulyActiveGroups.Keys.Contains(addr))
                        {
                            LeaveMulticastGroup(addr);
                        }
                    }

                    this.subscribedMulticastGroups = trulyActiveGroups;
                }   
            }


        }

        private static void LeaveMulticastGroup(IPAddress addr)
        {
            // leave the group
            try
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    MulticastOption mo = new MulticastOption(addr, ReflectorMgr.MulticastInterfaceIP);
                    ReflectorMgr.Sockets.SockMCv4RTP.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DropMembership, mo);
                    ReflectorMgr.Sockets.SockMCv4RTCP.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DropMembership, mo);
                }
                else
                {
                    IPv6MulticastOption mo = new IPv6MulticastOption(addr);
                    ReflectorMgr.Sockets.SockMCv6RTP.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.DropMembership, mo);
                    ReflectorMgr.Sockets.SockMCv6RTCP.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.DropMembership, mo);
                }
            }
            catch (Exception) {}
        }

        public void ProcessMessage(UdpReflectorMessage message,IPEndPoint source,Socket localSocket)
        {

            IPEndPoint localEP = localSocket.LocalEndPoint as IPEndPoint;

            if (message.Type == UdpReflectorMessageType.LEAVE)
            {
                // a background thread will eventually remove the multicast group, if necessary
                lock (clientRegTable) {
                    clientRegTable.Remove(source);
                }
                return;
            }
            else if (message.Type == UdpReflectorMessageType.JOIN)
            {
                IPEndPoint multicastEP = message.MulticastEP;

                JoinMulticastGroup(multicastEP);

                subscribedMulticastGroups[multicastEP.Address] = multicastEP.Address;
                ClientEntry entry = new ClientEntry(source, multicastEP, DateTime.Now);
                lock (clientRegTable) {
                    clientRegTable.Add(source, entry);
                }
                //  Multi-reflector cascading join, if required.  We send an identical join request on the
                // same port to the parent's IP address
                if (ParentReflector != null)
                {
                    IPEndPoint parentEP = new IPEndPoint(ParentReflector, localEP.Port);
                    UdpReflectorMessage joinMessage = 
                        new UdpReflectorMessage(UdpReflectorMessageType.JOIN, multicastEP);

                    BufferChunk chunk = joinMessage.ToBufferChunk();

                    localSocket.SendTo(chunk.Buffer, chunk.Index, chunk.Length, SocketFlags.None,parentEP);

                    // Need to add the parent to our local dispatch table (to simulate a remote join)

                    ClientEntry parentEntry = new ClientEntry(parentEP, multicastEP, DateTime.Now);
                    lock (clientRegTable) {
                        clientRegTable.Add(parentEP, parentEntry);
                    }
                }
                
            }
            else if (message.Type == UdpReflectorMessageType.PING)
            {
                // acknowledge that we're alive...
                UdpReflectorMessage pingReply = new UdpReflectorMessage(UdpReflectorMessageType.PING_REPLY);
                BufferChunk bufferChunk = pingReply.ToBufferChunk();
                localSocket.SendTo(bufferChunk.Buffer, bufferChunk.Index, bufferChunk.Length,
                            SocketFlags.None, source);
            }
            else
            {
                // unknown or unhandled message type...
            }

           
           
        }

        private static void JoinMulticastGroup(IPEndPoint multicastEP)
        {
            try
            {
                if (multicastEP.AddressFamily == AddressFamily.InterNetwork)
                {
                    MulticastOption mo = new MulticastOption(multicastEP.Address, ReflectorMgr.MulticastInterfaceIP);
                    ReflectorMgr.Sockets.SockMCv4RTP.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mo);
                    ReflectorMgr.Sockets.SockMCv4RTCP.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mo);
                }
                else
                {
                    IPv6MulticastOption mo = new IPv6MulticastOption(multicastEP.Address);
                    ReflectorMgr.Sockets.SockMCv6RTP.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.AddMembership, mo);
                    ReflectorMgr.Sockets.SockMCv6RTCP.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.AddMembership, mo);
                }
            }
            catch (SocketException)
            {
                // this appears to happen on duplicate joins; no big thing.
            }
        }

        public void MarkAsActive(ClientEntry entry) 
        {
            lock (clientRegTable) {
                clientRegTable.Add(entry.ClientEP, entry);
            }
        }

        public void ForceLeave(ClientEntry client)
        {
            lock (clientRegTable) {
                clientRegTable.Remove(client.ClientEP);
            }
        }

        internal ClientEntry GetEntry(IPEndPoint ep)
        {
            lock (clientRegTable)
            {
                if (clientRegTable.ContainsKey(ep))
                    return clientRegTable[ep];
                else return null;
            }

        }
    }
}
