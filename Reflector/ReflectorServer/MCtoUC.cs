using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

using MSR.LST.Net;

using ReflectorCommonLib;
using System.Collections.Generic;


namespace MSR.LST.ConferenceXP.ReflectorService
{
    /// <summary>
    /// This class is responsible for forwarding traffic from Multicast to Unicast
    /// </summary>
    public class MCtoUC
    {
        #region Constructor
        /// <summary>
        /// Creates the threadtype table based on IPv6/IPv4 support  
        /// </summary>
        public MCtoUC(RegistrarServer registrar, TrafficType traffTypes)
        {
            this.registrar = registrar;

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

        #endregion

        #region Private Members

        private readonly RegistrarServer registrar;

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
        private ReflectorEventLog eventLog = new ReflectorEventLog(ReflectorEventLog.Source.MCtoUC);

        #endregion Private Members
        
        #region Public thread methods
        /// <summary>
        /// Starts the two threads, one for RTP forwarding and the other for RTCP forwarding
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
        /// Stops two threads, one for RTP forwarding and the other for RTCP forwarding
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
        /// This method start the .
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
                    Thread.CurrentThread.Name = "Reflector_MCtoUC_" + traffTypes.ToString();
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
            IPEndPoint ipepTmpRef = null;
            IPEndPoint ipepTmpSrv = null;
            Socket ucRefSock = null;
            Socket mcSrvSock = null;
            Socket ucSrvSock = null;
            int ucPort = 0;
            int mcPort = 0;


            switch (traffTypes)
            {
                case TrafficType.IPv4RTCP:
                    ucPort = ReflectorMgr.ReflectorUnicastRTPListenPort + 1;
                    mcPort = ReflectorMgr.ReflectorMulticastRTPListenPort + 1;
                    ipepTmpRef = new IPEndPoint(IPAddress.IPv6Any, 0);
                    ipepTmpSrv = new IPEndPoint(IPAddress.Any, 0);
                    ucRefSock = ReflectorMgr.Sockets.SockUCv6RTCP;
                    mcSrvSock = ReflectorMgr.Sockets.SockMCv4RTCP;
                    ucSrvSock = ReflectorMgr.Sockets.SockUCv4RTCP;
                    break;
                case TrafficType.IPv6RTCP:
                    ucPort = ReflectorMgr.ReflectorUnicastRTPListenPort + 1;
                    mcPort = ReflectorMgr.ReflectorMulticastRTPListenPort + 1;
                    ipepTmpSrv = new IPEndPoint(IPAddress.IPv6Any, 0);
                    ipepTmpRef = new IPEndPoint(IPAddress.Any, 0);
                    ucRefSock = ReflectorMgr.Sockets.SockUCv4RTCP;
                    mcSrvSock = ReflectorMgr.Sockets.SockMCv6RTCP;
                    ucSrvSock = ReflectorMgr.Sockets.SockUCv6RTCP;
                    break;
                case TrafficType.IPv4RTP:
                    ucPort = ReflectorMgr.ReflectorUnicastRTPListenPort;
                    mcPort = ReflectorMgr.ReflectorMulticastRTPListenPort;
                    ipepTmpRef = new IPEndPoint(IPAddress.IPv6Any, 0);
                    ipepTmpSrv = new IPEndPoint(IPAddress.Any, 0);
                    ucRefSock = ReflectorMgr.Sockets.SockUCv6RTP;
                    mcSrvSock = ReflectorMgr.Sockets.SockMCv4RTP;
                    ucSrvSock = ReflectorMgr.Sockets.SockUCv4RTP;
                    break;
                case TrafficType.IPv6RTP:
                    ucPort = ReflectorMgr.ReflectorUnicastRTPListenPort;
                    mcPort = ReflectorMgr.ReflectorMulticastRTPListenPort;
                    ipepTmpSrv = new IPEndPoint(IPAddress.IPv6Any, 0);
                    ipepTmpRef = new IPEndPoint(IPAddress.Any, 0);
                    ucRefSock = ReflectorMgr.Sockets.SockUCv4RTP;
                    mcSrvSock = ReflectorMgr.Sockets.SockMCv6RTP;
                    ucSrvSock = ReflectorMgr.Sockets.SockUCv6RTP;
                    break;
                default:
                    Debug.Assert(false);
                    throw new ArgumentException(Strings.InvalidTrafficTypeCombination);
            }
            #endregion

            IList<IPEndPoint> members = new List<IPEndPoint>();

            byte[] buf = new byte[1500];

            while (true)
            {
                try
                {
                    EndPoint sourceEP = new IPEndPoint(IPAddress.Any, 0);
                    IPPacketInformation ipPackInfo;
                    SocketFlags flags = SocketFlags.None;

                    size = mcSrvSock.ReceiveMessageFrom(buf, 0, buf.Length, ref flags, ref sourceEP,
                        out ipPackInfo);

                    IPEndPoint sourceIpe = (IPEndPoint)sourceEP;

                    registrar.MarkIPAddressAsUsingMulticast(sourceIpe.Address);

                    // If the packet's source address is the reflector's IP address then this packet
                    // was forwarded from Unicast to Multicast by the reflector. So, we shouldn't 
                    // forward it to UC again. Also, "AND" this condition with source port 
                    // equal to 7004/7005 to have the support running the reflector and CXPClient on the
                    // same machine.
                    if ((sourceIpe.Port != ucPort) || (!sourceIpe.Address.Equals(ReflectorMgr.MulticastInterfaceIP)
                        && !sourceIpe.Address.Equals(ReflectorMgr.IPv6MulticastInterfaceIP)))
                    {
                        if ((traffTypes & TrafficType.RTP) == TrafficType.RTP)
                        {
                            ReflectorMgr.PC[ReflectorPC.ID.MulticastPacketsReceivedOther]++;
                        }

                        // Lookup the members of this multicast group.
                        IPEndPoint multicastEP = new IPEndPoint(ipPackInfo.Address, mcPort);
                        registrar.MemberLookup(members, multicastEP);

                        if (members.Count != 0)
                        {
                            // Send the data to each individual.
                            for (int j = 0; j < members.Count; j++)
                            {
                                if (members[j].AddressFamily == ucSrvSock.AddressFamily)
                                {
                                    ucSrvSock.SendTo(buf, 0, size, SocketFlags.None, members[j]);
                                }
                                else if ((ucRefSock != null) && (members[j].AddressFamily == ucRefSock.AddressFamily))
                                {
                                    ucRefSock.SendTo(buf, 0, size, SocketFlags.None, members[j]);
                                }
                            }

                            if ((traffTypes & TrafficType.RTP) == TrafficType.RTP)
                            {
                                ReflectorMgr.PC[ReflectorPC.ID.MCtoUCPacketsSent] += members.Count;
                            }
                        }
                    }
                    else if ((traffTypes & TrafficType.RTP) == TrafficType.RTP)
                    {
                        ReflectorMgr.PC[ReflectorPC.ID.MulticastPacketsReceivedSelf]++;
                    }
                }
                // On stopping the service, avoid the AbortException written in the event viewer
                catch (ThreadAbortException){}
                catch (Exception e) // Connection reset by peer! this happens occasionally when a UC client leaves.
                {
                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.MCtoUCListenerException, 
                        traffTypes, e.ToString()), EventLogEntryType.Warning, (int)ReflectorEventLog.ID.MCtoUCException);
                }
            }
        }

        #endregion
    }
}