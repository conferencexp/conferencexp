using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using MSR.LST.Net;

using ReflectorCommonLib;


namespace MSR.LST.ConferenceXP.ReflectorService
{
    /// <summary>
    /// This class is responsible for starting, stopping and reporting the status of the reflector.
    /// </summary>
    public class ReflectorMgr
    {
        #region Statics

        /// <summary>
        /// Allows the user to override the default language with app.config.
        /// </summary>
        private static CultureInfo cultureInfo = null;

        public static CultureInfo ReflectorCultureInfo {
            get { return cultureInfo;  }
        }

        /// <summary>
        /// An instant of ReflectorSockets responsible for initalizing sockets and keeping references to the sockets.
        /// </summary>
        private static ReflectorSockets sockets = null;

        public static ReflectorSockets Sockets
        {
            get { return sockets;}
        }

        /// <summary>
        /// Whether we are using the MultipleInterfaceSupport features (read from the config file)
        /// </summary>
        private static bool multipleInterfaceSupport = false;

        public static bool MultipleInterfaceSupport
        {
            get { return multipleInterfaceSupport; }
        }

        /// <summary>
        /// Turn this feature on, only if you have a dedicated server for the reflector service. 
        /// This feature will prevent you from runing the cxpclient and the reflector on the same machine.
        /// </summary>
        private static bool mcLoopbackOff = false;

        public static bool MCLoopbackOff
        {
            get { return mcLoopbackOff; }
        }

        /// <summary>
        /// Enabled Traffic Types IPv4/IPv6/IPv4andIPv6
        /// </summary>
        private static TrafficType enabledTrafficTypes = TrafficType.None;

        public static TrafficType EnabledTrafficTypes
        {
            get { return enabledTrafficTypes; }
        }

        /// <summary>
        /// Multicast route index (read from the configuration file)
        /// </summary>
        private static int mcInterfaceRouteIndex = 0;

        public static int MulticastInterfaceRouteIndex
        {
            get { return mcInterfaceRouteIndex; }
        }

        /// <summary>
        /// Multicast Interface IP address (read from the configuration file)
        /// </summary>
        private static IPAddress multicastInterfaceIP = null;

        public static IPAddress MulticastInterfaceIP
        {
            get { return multicastInterfaceIP; }
        }

        /// <summary>
        /// IPv6 Multicast Interface IP address (read from the configuration file)
        /// </summary>
        private static IPAddress IPv6multicastInterfaceIP = null;

        public static IPAddress IPv6MulticastInterfaceIP
        {
            get { return IPv6multicastInterfaceIP; }
        }

        /// <summary>
        /// Unicast Interface IP address (read from the configuration file)
        /// </summary>
        private static IPAddress unicastInterfaceIP = null;

        public static IPAddress UnicastInterfaceIP
        {
            get { return unicastInterfaceIP; }
        }
      
        /// <summary>
        /// IPv6 Unicast Interface IP address (read from the configuration file)
        /// </summary>
        private static IPAddress IPv6unicastInterfaceIP = null;

        public static IPAddress IPv6UnicastInterfaceIP
        {
            get { return IPv6unicastInterfaceIP; }
        }
      
        /// <summary>
        /// The port number which the Reflector listens for unicast traffic from multicast disabled
        /// clients. This port will be sent back to clients after a join request.
        /// </summary>
        private static int reflectorUnicastRTPListenPort = 7004;

        public static int ReflectorUnicastRTPListenPort
        {
            get { return reflectorUnicastRTPListenPort; }
        }

        /// <summary>
        /// The port number which the Reflector listens for multicast traffic on the multicast enabled
        /// network. 
        /// </summary>
        private static int reflectorMulticastRTPListenPort = 5004;

        public static int ReflectorMulticastRTPListenPort
        {
            get { return reflectorMulticastRTPListenPort; }
        }

        /// <summary>
        /// Service name
        /// </summary>
        public const string ReflectorServiceName = "ConferenceXP Reflector Service";

        /// <summary>
        /// Performance counters
        /// </summary>
        private static ReflectorPC pC;

        public static ReflectorPC PC
        {
            get { return pC; }
        }

        private static readonly ReflectorMgr instance = new ReflectorMgr();

        public static ReflectorMgr getInstance()
        {
            return instance;
        }

        #endregion Statics

        #region Members

        /// <summary>
        /// An instance of the RegistrarServer used to stop and start the server.
        /// </summary>
        private readonly RegistrarServer regServer = null;

        public RegistrarServer RegServer
        {
            get { return regServer; }
        } 


        /// <summary>
        /// An instance of the MCtoUC used to stop and start the server.
        /// </summary>
        private MCtoUC MUforwarder = null;


        /// <summary>
        /// An instance of the UCtoUCMC used to stop and start the server.
        /// </summary>
        private UCtoUCMC UMforwarder = null;

        /// <summary>
        /// Shows whether the servers are running or they are stopped.
        /// </summary>
        private bool isRunning = false;

        public bool IsRunning
        {
            get { return isRunning; }
        }

        /// <summary>
        /// Log events
        /// </summary>
        private ReflectorEventLog eventLog = new ReflectorEventLog(ReflectorEventLog.Source.ReflectorManager);

        #endregion Members
        
        #region Constructors

        /// <summary>
        /// The constructor which reads config setting and calls the constructor for the sockets and three servers.
        /// </summary>
        private ReflectorMgr()
        {
            string setting;

            if ((setting = ConfigurationManager.AppSettings[AppConfig.UICulture]) != null) {
                try {
                    cultureInfo = new CultureInfo(setting);
                }
                catch { }
            }

            if ( (setting = ConfigurationManager.AppSettings[AppConfig.UnicastPort] ) != null )
            {
                reflectorUnicastRTPListenPort = int.Parse(setting, CultureInfo.InvariantCulture);
            }

            //TimeSpan timeoutPeriod = TimeSpan.Zero;
            //if ( (setting = ConfigurationManager.AppSettings[AppConfig.TimeOutMinutes] ) != null )
            //{
            //    // Minutes
            //    timeoutPeriod = new TimeSpan(0, int.Parse(setting, CultureInfo.InvariantCulture), 0);
            //}

            if ((setting = ConfigurationManager.AppSettings[AppConfig.MultipleInterfaceSupport] ) != null)
            {
                multipleInterfaceSupport = bool.Parse(setting);
            } 

            if ((setting = ConfigurationManager.AppSettings[AppConfig.IPv4Support] ) != null)
            {
                if(bool.Parse(setting))
                {
                    enabledTrafficTypes |= TrafficType.IPv4;
                }
            } 

            if ((setting = ConfigurationManager.AppSettings[AppConfig.IPv6Support] ) != null)
            {
                if(bool.Parse(setting))
                {
                    if (Socket.OSSupportsIPv6)
                    {
                        enabledTrafficTypes |= TrafficType.IPv6;
                    }
                    else
                    {
                        eventLog.WriteEntry(Strings.IPv6NotEnabledInOs, EventLogEntryType.Warning, 
                            ReflectorEventLog.ID.Error);
                    }
                }
            }
            
            // If the user has set both IPv6 support and IPv4 support to false, we go to default 
            // config: IPv4Support only.
            if (enabledTrafficTypes < TrafficType.IPv4)
            {
                enabledTrafficTypes = TrafficType.IPv4;
            }

            if ((setting = ConfigurationManager.AppSettings[AppConfig.MCLoopbackOff] ) != null)
            {
                mcLoopbackOff = bool.Parse(setting);
            }

            if ((setting = ConfigurationManager.AppSettings[AppConfig.MulticastInterfaceRouteIndex] ) != null)
            {
                // Route print produces a hexadecimal number
                mcInterfaceRouteIndex = Convert.ToInt32(setting, 16);
            }

            if ((enabledTrafficTypes & TrafficType.IPv4) == TrafficType.IPv4)
            {

                if ( ((setting = ConfigurationManager.AppSettings[AppConfig.MulticastInterfaceIP] ) != null) 
                    && multipleInterfaceSupport)
                {
                    multicastInterfaceIP = IPAddress.Parse(setting);
                }
                else
                {
                    //Workaround: Pass a generic IPv4 multicast IP.
                    multicastInterfaceIP = Utility.GetLocalRoutingInterface(IPAddress.Parse("233.4.5.6"));
                }
            

                if (((setting = ConfigurationManager.AppSettings[AppConfig.UnicastInterfaceIP] ) != null) 
                    && multipleInterfaceSupport)
                {
                    unicastInterfaceIP = IPAddress.Parse(setting);
                }
                else
                {
                    // The interface which is routed toward the Root name servers
                    unicastInterfaceIP = Utility.GetLocalRoutingInterface(IPAddress.Parse("198.41.0.4"));
                }
            }

            if ((enabledTrafficTypes & TrafficType.IPv6) == TrafficType.IPv6)
            {

                if ( ((setting = ConfigurationManager.AppSettings[AppConfig.IPv6MulticastInterfaceIP] ) != null) 
                    && multipleInterfaceSupport)
                {
                    IPv6multicastInterfaceIP = IPAddress.Parse(setting);
                }
                else
                {
                    //Workaround: Pass a generic IPv6 multicast IP.
                    IPv6multicastInterfaceIP = Utility.GetLocalRoutingInterface(IPAddress.Parse("ff1e::1"));
                }
            

                if (((setting = ConfigurationManager.AppSettings[AppConfig.IPv6UnicastInterfaceIP] ) != null) 
                    && multipleInterfaceSupport)
                {
                    IPv6unicastInterfaceIP = IPAddress.Parse(setting);
                }
                else
                {
                    // The interface which is routed toward 6bone.net
                    IPv6unicastInterfaceIP = Utility.GetLocalRoutingInterface(IPAddress.Parse("2001:5c0:0:2::24"));
                }
            }

            try
            {
                sockets = new ReflectorSockets(enabledTrafficTypes);
            }
            catch(Exception e)
            {
                eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, (int)ReflectorEventLog.ID.Error);
                throw;
            }

            regServer = new RegistrarServer();
            UMforwarder = new UCtoUCMC(regServer,enabledTrafficTypes); 
            MUforwarder = new MCtoUC(regServer,enabledTrafficTypes);
        }
        
        #endregion Constructors

        #region Public Methods
        /// <summary>
        /// Starts three independent threads who perform reflector server's tasks.
        /// </summary>
        public void StartReflector()
        {
            try
            {
                pC = new ReflectorPC("Reflector");

                sockets.InitAll(enabledTrafficTypes);
                
                UMforwarder.StartThreads();
                MUforwarder.StartThreads();

                isRunning = true;

                eventLog.WriteEntry(Strings.ReflectorServiceStarted, EventLogEntryType.Information,
                    (int)ReflectorEventLog.ID.ServiceStarted);
            } 
            catch(Exception e)
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                    Strings.ReflectorServiceStartingException, e.ToString()), EventLogEntryType.Error,
                    (int)ReflectorEventLog.ID.ThreadStartingException);

            }
        }

        /// <summary>
        /// Stops the reflector.
        /// </summary>
        public void StopReflector()
        {
            isRunning = false;

            try
            {
                // On 64-bit machines, the call to UMforwarder.StopThreads() would seemingly
                // not stop threads and the call to MUforwarder.StopThreads() would seemingly
                // abort the thread that was stopping the service, leaving the service in an
                // uncontrollable state.  Since all the threads were created with
                // Background = true, we decided to remove the calls to stop the
                // threads.  The service works fine.  Now we get some additional
                // warnings (count varies) in the event log, because the threads
                // are sometimes stopped by the process exiting, and sometimes
                // by the socket being closed.

                //regServer.StopThreads();
                //UMforwarder.StopThreads();
                //MUforwarder.StopThreads();

                // regServer no longer has separate listening sockets
                //regServer.StopListening();

                sockets.DisposeAll();
                pC.Dispose();
                pC = null;
                eventLog.WriteEntry(Strings.ReflectorServiceStopped, EventLogEntryType.Information,
                    (int)ReflectorEventLog.ID.ServiceStopped);
            }
            // On stopping the service, avoid the AbortException written in the event viewer
            catch (ThreadAbortException) { }
            catch (Exception e)
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture,
                    Strings.ReflectorServiceStoppingException, e.ToString()), EventLogEntryType.Warning,
                    (int)ReflectorEventLog.ID.ThreadStoppingException);
            }
        }
        #endregion
    }

    /// <summary>
    /// ReflectorSockets class initalizes sockets for RTP/RTCP, IPv6/IPv4, UC/MC, keeps a reference to them 
    /// and disposes them at the end.
    /// 
    /// The Unicast socket receives unicast data, sends unicast data, and sends multicast data.
    /// The Multicast socket only receives multicast data and does not send any data.
    /// </summary>
    public class ReflectorSockets
    {
        #region private members
        /// <summary>
        /// This boolean hold the information about whether the sockets are initalized or not.
        /// </summary>
        private bool IsInitalized = false;
        #endregion

        #region Public Socket member variables
        /// <summary>
        /// A Socket class for Receiving IPv6 multicast RTP traffic.
        /// </summary>
        private Socket sockMCv6RTP  = null;

        public Socket SockMCv6RTP
        {
            get {return sockMCv6RTP;}
        }

        /// <summary>
        /// A Socket class for Receiving IPv6 unicast RTP traffic and sending to IPv6 multicast RTP traffic.
        /// </summary>
        private Socket sockUCv6RTP  = null;

        public Socket SockUCv6RTP
        {
            get {return sockUCv6RTP;}
        }

        /// <summary>
        /// A Socket class for Receiving IPv6 multicast RTCP traffic.
        /// </summary>
        private Socket sockMCv6RTCP = null;

        public Socket SockMCv6RTCP
        {
            get {return sockMCv6RTCP;}
        }

        /// <summary>
        /// A Socket class for Receiving IPv6 unicast RTCP traffic and sending to IPv6 multicast RTP traffic.
        /// </summary>
        private Socket sockUCv6RTCP = null;

        public Socket SockUCv6RTCP
        {
            get {return sockUCv6RTCP;}
        }

        /// <summary>
        /// A Socket class for Receiving IPv4 multicast RTP traffic.
        /// </summary>
        private Socket sockMCv4RTP  = null;

        public Socket SockMCv4RTP
        {
            get {return sockMCv4RTP;}
        }

        /// <summary>
        /// A Socket class for Receiving IPv4 unicast RTP traffic and sending to IPv4 multicast RTP traffic.
        /// </summary>
        private Socket sockUCv4RTP  = null;

        public Socket SockUCv4RTP
        {
            get {return sockUCv4RTP;}
        }

        /// <summary>
        /// A Socket class for Receiving IPv4 multicast RTCP traffic.
        /// </summary>
        private Socket sockMCv4RTCP = null;

        public Socket SockMCv4RTCP
        {
            get {return sockMCv4RTCP;}
        }
        /// <summary>
        /// A Socket class for Receiving IPv4 unicast RTCP traffic and sending to IPv4 multicast RTP traffic.
        /// </summary>
        private Socket sockUCv4RTCP = null;
        
        public Socket SockUCv4RTCP
        {
            get {return sockUCv4RTCP;}
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initalizes all sockets
        /// </summary>
        /// <param name="IPv4Support">Boolean indicating whether to initalize IPv4 Sockets</param>
        /// <param name="IPv6Support">Boolean indicating whether to initalize IPv6 Sockets</param>
        public ReflectorSockets(TrafficType traffTypes)
        {
            InitAll(traffTypes);
        } 
        
        #endregion

        #region public methods
        /// <summary>
        /// Initalizes all sockets
        /// </summary>
        /// <param name="traffTypes">TrafficType indicating whether to initalize IPv6/IPv4 Sockets</param>
        public void InitAll(TrafficType traffTypes)
        {

            try
            {
                lock(this)
                {
                    if (!IsInitalized)
                    {
                        if ( (traffTypes & TrafficType.IPv6) == TrafficType.IPv6)
                        {
                            InitSocket(out sockMCv6RTP, TrafficType.MCv6RTP);
                            InitSocket(out sockUCv6RTP, TrafficType.UCv6RTP);
                            InitSocket(out sockMCv6RTCP, TrafficType.MCv6RTCP);
                            InitSocket(out sockUCv6RTCP, TrafficType.UCv6RTCP);
                        }

                        if ( (traffTypes & TrafficType.IPv4) == TrafficType.IPv4)
                        {
                            InitSocket(out sockMCv4RTP, TrafficType.MCv4RTP);
                            InitSocket(out sockUCv4RTP, TrafficType.UCv4RTP);
                            InitSocket(out sockMCv4RTCP, TrafficType.MCv4RTCP);
                            InitSocket(out sockUCv4RTCP, TrafficType.UCv4RTCP);
                        }
                        IsInitalized = true;
                    }
                }
            } 
            catch(Exception e)
            {
                throw new Exception(Strings.SocketInitializationError, e);
            }
        }


        /// <summary>
        /// Calls the Socket.Close() on all sockets
        /// </summary>
        public void DisposeAll()
        {
            try
            {
                lock(this)
                {
                    if (IsInitalized)
                    {
                        IsInitalized = false;
                        if (sockMCv6RTP  != null)
                            sockMCv6RTP.Close();

                        if (sockUCv6RTP  != null)
                            sockUCv6RTP.Close();

                        if (sockMCv6RTCP  != null)
                            sockMCv6RTCP.Close();

                        if (sockUCv6RTCP  != null)
                            sockUCv6RTCP.Close();

                        if (sockMCv4RTP  != null)
                            sockMCv4RTP.Close();

                        if (sockUCv4RTP  != null)
                            sockUCv4RTP.Close();

                        if (sockMCv4RTCP  != null)
                            sockMCv4RTCP.Close();

                        if (sockUCv4RTCP  != null)
                            sockUCv4RTCP.Close();
                    }
                }
            } 
            catch (SocketException e)
            {
                throw new Exception(Strings.SocketDisposingError, e);

            }
        }

        #endregion

        #region private methods
        /// <summary>
        /// Intitalizes one socket based on the given parameters.
        /// </summary>
        /// <param name="sock">The socket class to be initalized</param>
        /// <param name="addrFamily">The address family for the socket to be initalized to (InterNetwork/InterNetworkV6)</param>
        /// <param name="isUnicast">Wether this socket is to be used for Multicast or Unicast</param>
        /// <param name="traffType">Wether this socket is to be used for RTP traffic or RTCP</param>
        private static void InitSocket(out Socket sock, TrafficType traffTypes)
        {
            int bindPort = 0; 
            SocketOptionLevel sOL = 0;
            AddressFamily addrFamily = AddressFamily.InterNetwork;
            IPAddress bindInterface = null;
     
            #region initalizing bindPort, sOL, addrFamily and BindInterface based on function prarameters
            switch (traffTypes)
            {
                case TrafficType.UCv4RTP:
                    bindInterface = ReflectorMgr.UnicastInterfaceIP;
                    sOL = SocketOptionLevel.IP;
                    bindPort = ReflectorMgr.ReflectorUnicastRTPListenPort;
                    addrFamily = AddressFamily.InterNetwork;
                    break;
                case TrafficType.UCv6RTP:
                    bindInterface = ReflectorMgr.IPv6UnicastInterfaceIP;
                    sOL = SocketOptionLevel.IPv6;
                    bindPort = ReflectorMgr.ReflectorUnicastRTPListenPort;
                    addrFamily = AddressFamily.InterNetworkV6;
                    break;
                case TrafficType.MCv4RTP:
                    bindInterface = ReflectorMgr.MulticastInterfaceIP;
                    sOL = SocketOptionLevel.IP;
                    bindPort = ReflectorMgr.ReflectorMulticastRTPListenPort;
                    addrFamily = AddressFamily.InterNetwork;
                    break;
                case TrafficType.MCv6RTP:
                    bindInterface = ReflectorMgr.IPv6MulticastInterfaceIP;
                    sOL = SocketOptionLevel.IPv6;
                    bindPort = ReflectorMgr.ReflectorMulticastRTPListenPort;
                    addrFamily = AddressFamily.InterNetworkV6;
                    break;
                case TrafficType.UCv4RTCP:
                    bindInterface = ReflectorMgr.UnicastInterfaceIP;
                    sOL = SocketOptionLevel.IP;
                    bindPort = ReflectorMgr.ReflectorUnicastRTPListenPort + 1;
                    addrFamily = AddressFamily.InterNetwork;
                    break;
                case TrafficType.UCv6RTCP:
                    bindInterface = ReflectorMgr.IPv6UnicastInterfaceIP;
                    sOL = SocketOptionLevel.IPv6;
                    bindPort = ReflectorMgr.ReflectorUnicastRTPListenPort + 1;
                    addrFamily = AddressFamily.InterNetworkV6;
                    break;
                case TrafficType.MCv4RTCP:
                    bindInterface = ReflectorMgr.MulticastInterfaceIP;
                    sOL = SocketOptionLevel.IP;
                    bindPort = ReflectorMgr.ReflectorMulticastRTPListenPort + 1;
                    addrFamily = AddressFamily.InterNetwork;
                    break;
                case TrafficType.MCv6RTCP:
                    bindInterface = ReflectorMgr.IPv6MulticastInterfaceIP;
                    sOL = SocketOptionLevel.IPv6;
                    bindPort = ReflectorMgr.ReflectorMulticastRTPListenPort + 1;
                    addrFamily = AddressFamily.InterNetworkV6;
                    break;
                default:
                    Debug.Assert(false);
                    throw new ArgumentException(Strings.InvalidTrafficTypeCombination);
            }
            #endregion

            sock = new Socket(addrFamily, SocketType.Dgram, ProtocolType.Udp);
            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 1500 * 160);

            // Multicast receive only
            if ((traffTypes & TrafficType.Multicast) == TrafficType.Multicast)
            {
                sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, -1);
            }

            // Unicast socket sends multicast data, so set some multicast sending properties on it
            if ((traffTypes & TrafficType.Unicast) == TrafficType.Unicast)
            {
                sock.SetSocketOption(sOL, SocketOptionName.MulticastTimeToLive, 128);

                // Only set this value if it is other than the default
                if(ReflectorMgr.MulticastInterfaceRouteIndex > 0)
                {
                    sock.SetSocketOption(sOL, SocketOptionName.MulticastInterface, 
                        (int)IPAddress.HostToNetworkOrder(ReflectorMgr.MulticastInterfaceRouteIndex));
                }
            }

            sock.Bind(new IPEndPoint(bindInterface, bindPort));

            if (((traffTypes & TrafficType.Multicast) == TrafficType.Multicast) && (! ReflectorMgr.MCLoopbackOff))
            {
                sock.SetSocketOption(sOL, SocketOptionName.MulticastLoopback, 1);          
            }
        }

        #endregion
    }
}
