using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using MSR.LST.Net;

using ReflectorCommonLib;
using System.Collections.Generic;
using System.Configuration;


namespace MSR.LST.ConferenceXP.ReflectorService
{
    /// <summary>
    /// This class is responsible for receiving data from Unicast side and sending it back to other Unicast
    /// clients and multicast clients.
    /// </summary>
    public class UCtoUCMC
    {

        #region Private Members

        private RegistrarServer registrar;

        /// <summary>
        /// This arraylist will provide the data required for each thread.
        /// Thread 1: RTP IPv4
        /// Thread 2: RTP IPv6
        /// Thread 3: RTCP IPv4
        /// Thread 4: RTCP IPv6
        /// </summary>
        private TrafficType [] ThreadTypeData=null;

        /// <summary>
        /// An index used to keep track of which types of threads are created. This index refers to an entry in 
        /// ThreadTypeData array. This index is also used as a lock to guarantee exclusive access to this array.
        /// That way we ensure one and only one thread is created per each entry in ThreadTypeData array.
        /// </summary>
        private int idxThreadTypeData=0;

        /// <summary>
        /// The thread variable for the multicast to unicast RTP/RTCP IPv4/IPv6 data forwarder. This variable is a member 
        /// so although the thread is created in the StartThread method, the stop routine could Interrupt 
        /// and Abort them.
        /// </summary>
        private Thread [] threadPTRs=null;

        /// <summary>
        /// Log events
        /// </summary>
        private ReflectorEventLog eventLog = new ReflectorEventLog(ReflectorEventLog.Source.UCtoUCMC);

        private bool SendMulticast = true;

        #endregion Members

        #region Constructors
        /// <summary>
        /// Initalizes a ThreadType(s) to keep the required initalization data for threads to start
        /// </summary>
        public UCtoUCMC(RegistrarServer registrar, TrafficType traffTypes)
        {
            this.registrar = registrar;

            // initialize multicast enabled flag...
            String setting = ConfigurationManager.AppSettings[AppConfig.SendMulticast];
            
            if (setting != null)
            {
                try
                {
                    SendMulticast = Boolean.Parse(setting);
                }
                catch (Exception)
                {
                    SendMulticast = true;
                }
            }

            if ( (traffTypes & TrafficType.IPv4andIPv6) == TrafficType.IPv4andIPv6)
            {
                ThreadTypeData = new TrafficType[4];
                ThreadTypeData[0] = TrafficType.IPv4RTP;
                ThreadTypeData[1] = TrafficType.IPv6RTP;
                ThreadTypeData[2] = TrafficType.IPv4RTCP;
                ThreadTypeData[3] = TrafficType.IPv6RTCP;
            } 
            else if ( ((traffTypes & TrafficType.IPv4) == TrafficType.IPv4 ) || 
                      ((traffTypes & TrafficType.IPv6) == TrafficType.IPv6 ) )
            {
                ThreadTypeData = new TrafficType[2];
                if ((traffTypes & TrafficType.IPv4) == TrafficType.IPv4)
                {
                    ThreadTypeData[0] = TrafficType.IPv4RTP;
                    ThreadTypeData[1] = TrafficType.IPv4RTCP;
                }
                else
                {
                    ThreadTypeData[0] = TrafficType.IPv6RTP;
                    ThreadTypeData[1] = TrafficType.IPv6RTCP;
                }
            } 
            else 
            {
                Debug.Assert(false);
                throw new Exception(Strings.EnableEitherIPv6OrIPv4);
            }
        }

        #endregion Constructors

        #region Public thread methods
        /// <summary>
        /// Starts an RTP and RTCP unicast to unicast/multicast forwarder threads.
        /// </summary>
        public void StartThreads() 
        {
            idxThreadTypeData = 0;

            threadPTRs = new Thread[ThreadTypeData.Length];
            for (int i=0; i<ThreadTypeData.Length; i++)
            {
                threadPTRs[i] = new Thread(new ThreadStart(Start));
                threadPTRs[i].IsBackground = true;
                if (ReflectorMgr.ReflectorCultureInfo != null) {
                    threadPTRs[i].CurrentUICulture = ReflectorMgr.ReflectorCultureInfo;
                }
                threadPTRs[i].Start();
            }
        }


        /// <summary>
        /// Stops the RTP and RTCP UCtoUCMC threads.
        /// </summary>
        public void StopThreads()
        {
            try
            {
                for (int i=0; i< threadPTRs.Length; i++)
                {
                    threadPTRs[i].Abort();
                    threadPTRs[i].Interrupt();
                    threadPTRs[i].Join(100);
                }
            }
            // On stopping the service, avoid the AbortException written in the event viewer
            catch(ThreadAbortException){}
            catch (Exception e)
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                    Strings.MCtoUCThreadTerminatingException, e.ToString()), EventLogEntryType.Warning, 
                    (int)ReflectorEventLog.ID.ThreadStoppingException);
            }
        }
        #endregion

        #region Public methods

        /// <summary>
        /// Forwards the traffic from unicast to unicast/multicast for the given trafficType
        /// </summary>
        public void Start() 
        {

            int size = 0;
            TrafficType traffTypes;

            lock (ThreadTypeData)
            {
                if (idxThreadTypeData < ThreadTypeData.Length)
                {
                    traffTypes = ThreadTypeData[idxThreadTypeData];
                    Thread.CurrentThread.Name = "Reflector_UCtoUCMC_" + traffTypes.ToString();
                    idxThreadTypeData++;
                }
                else
                {
                    throw new Exception(Strings.CreatedThreadsExceedThreadTypesDefined);
                }
            }
            

            #region Assigning appropriate sockets to "(mc/uc)(Ref/Srv)Sock"
            // The Ref(erence) socket variables are assigned to the socket protocol that this thread is not listening on 
            // but may use for inter-protocol communication. For example if mcSrvSock is an IPv4 socket, mcRefSock would be
            // an IPv6 socket and vice versa.
            Socket mcRefSock = null;
            Socket ucRefSock = null;
            Socket ucSrvSock = null;
            int ucPort = 0;
            EndPoint remoteEP = null;

            switch (traffTypes)
            {
                case TrafficType.IPv4RTCP:
                    ucPort = ReflectorMgr.ReflectorUnicastRTPListenPort + 1;
                    remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    mcRefSock = ReflectorMgr.Sockets.SockMCv6RTCP;
                    ucRefSock = ReflectorMgr.Sockets.SockUCv6RTCP;
                    ucSrvSock = ReflectorMgr.Sockets.SockUCv4RTCP;
                    break;
                case TrafficType.IPv6RTCP:
                    ucPort = ReflectorMgr.ReflectorUnicastRTPListenPort + 1;
                    remoteEP = new IPEndPoint(IPAddress.IPv6Any, 0);
                    mcRefSock = ReflectorMgr.Sockets.SockMCv4RTCP;
                    ucRefSock = ReflectorMgr.Sockets.SockUCv4RTCP;
                    ucSrvSock = ReflectorMgr.Sockets.SockUCv6RTCP;
                    break;
                case TrafficType.IPv4RTP:
                    ucPort = ReflectorMgr.ReflectorUnicastRTPListenPort;
                    remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    mcRefSock = ReflectorMgr.Sockets.SockMCv6RTP;
                    ucRefSock = ReflectorMgr.Sockets.SockUCv6RTP;
                    ucSrvSock = ReflectorMgr.Sockets.SockUCv4RTP;
                    break;
                case TrafficType.IPv6RTP:
                    ucPort = ReflectorMgr.ReflectorUnicastRTPListenPort;
                    remoteEP = new IPEndPoint(IPAddress.IPv6Any, 0);
                    mcRefSock = ReflectorMgr.Sockets.SockMCv4RTP;
                    ucRefSock = ReflectorMgr.Sockets.SockUCv4RTP;
                    ucSrvSock = ReflectorMgr.Sockets.SockUCv6RTP;
                    break;
                default:
                    Debug.Assert(false);
                    throw new ArgumentException(Strings.InvalidTrafficTypeCombination);
            }

            #endregion

            IPEndPoint groupEP = null;
            byte [] buf = new byte[1500];
            IList<IPEndPoint> members = new List<IPEndPoint>();
            
            while (true)
            {
                try
                {
                    //EndPoint ep = null;
                    size = ucSrvSock.ReceiveFrom(buf, ref remoteEP);

                    // First, check whether this is a control message (JOIN or LEAVE)
                    if (size <= 50)
                    {
                        try
                        {
                            UdpReflectorMessage message = new UdpReflectorMessage(buf, size);
                            registrar.ProcessMessage(message, remoteEP as IPEndPoint, ucSrvSock);

                            continue; // read next message
                        }
                        catch (InvalidUdpReflectorMessage) 
                        {  
                            // fall through 
                        }
                    }


                    if ((traffTypes & TrafficType.RTP) == TrafficType.RTP)
                    {
                        ReflectorMgr.PC[ReflectorPC.ID.UnicastPacketsReceived]++;
                    }
        
                    ClientEntry entry = registrar.GetEntry(remoteEP as IPEndPoint);

                    if (entry != null)
                    {

                        registrar.MarkAsActive(entry);

                        // Make sure this node isn't also sending over multicast...
                        if (registrar.IsIPAddressUsingMulticast(entry.ClientEP.Address))
                        {
                            eventLog.WriteEntry("Warning: receving both unicast and multicast from: " + entry.ClientEP.Address,
                                EventLogEntryType.Warning, (int)ReflectorEventLog.ID.UCtoUCMCException);
                            continue; // read next message without propogating further...
                        }

                        // lookup the (first) group which this client is a member of that group.
                        groupEP = entry.GroupEP;

                        // Find the other members of the group
                        registrar.MemberLookup(members, groupEP);

                        // Send the data to the Multicast side
                        if (SendMulticast)
                        {
                            if (groupEP.AddressFamily == ucSrvSock.AddressFamily)
                            {
                                ucSrvSock.SendTo(buf, 0, size, SocketFlags.None, groupEP);
                            }
                            else if ((mcRefSock != null) && (groupEP.AddressFamily == ucRefSock.AddressFamily))
                            {
                                ucRefSock.SendTo(buf, 0, size, SocketFlags.None, groupEP);
                            }
                        }

                        // Send the data to all unicast client members except the sender.
                        for (int i = 0; i < members.Count; i++)
                        {
                            if (!remoteEP.Equals(members[i]))
                            {
                                if (members[i].AddressFamily == ucSrvSock.AddressFamily)
                                {
                                    ucSrvSock.SendTo(buf, 0, size, SocketFlags.None, members[i]);
                                }
                                else if ((ucRefSock != null) && (members[i].AddressFamily == ucRefSock.AddressFamily))
                                {
                                    ucRefSock.SendTo(buf, 0, size, SocketFlags.None, members[i]);
                                }
                            }
                        }

                        if ((traffTypes & TrafficType.RTP) == TrafficType.RTP)
                        {
                            ReflectorMgr.PC[ReflectorPC.ID.UCtoUCPacketsSent] += members.Count - 1;
                        }
                    }

                } 
                // On stopping the service, avoid the AbortException written in the event viewer
                catch(ThreadAbortException){}
                catch (Exception e) // Connection reset by peer! this happens occasionally when a UC client leaves.
                {
                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.UCtoUCMCForwarderException, 
                        traffTypes, remoteEP, e.ToString()), EventLogEntryType.Warning, (int)ReflectorEventLog.ID.UCtoUCMCException);
                }
            }
        }

        #endregion

    }
}