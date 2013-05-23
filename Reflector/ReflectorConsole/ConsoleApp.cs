using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Net;

using MSR.LST.ConferenceXP.ReflectorService;
using MSR.LST.Net;

using ReflectorCommonLib;
using System.Collections.Generic;


namespace MSR.LST.ConferenceXP.ReflectorService
{
    /// <summary>
    /// This class provides a console executable for the Reflector.
    /// </summary>
    class ConsoleApp
    {
        private static ReflectorMgr refMgr;
        private static RegistrarServer registrar;

        [STAThread]
        static void Main(string[] args)
        {
            // Override the system UICulture
            string cultureOverride = null;
            if ((cultureOverride = ConfigurationManager.AppSettings["MSR.LST.Reflector.UICulture"]) != null) {
                try {
                    CultureInfo ci = new CultureInfo(cultureOverride);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                }
                catch { }
            }

            MSR.LST.UnhandledExceptionHandler.Register();

            refMgr = ReflectorMgr.getInstance();
            registrar = refMgr.RegServer;

            StartService();
            while (true)
            {
                Console.Write(Strings.ReflectorConsole);
                string input = Console.ReadLine().ToLower(CultureInfo.CurrentCulture);
                switch (input)
                {
                    case "p":
                        Console.WriteLine();
                        PrintTable();
                        Console.WriteLine();
                        break;
                    case "q":
                        Console.WriteLine();
                        StopService();
                        Console.WriteLine();
                        return;
                    case "h":
                        Console.WriteLine();
                        PrintHelp();
                        Console.WriteLine();
                        break;
                    case "d":
                        //Console.WriteLine();
                        //DeleteClient();
                        //Console.WriteLine();
                        break;
                    case "s":
                        Console.WriteLine();
                        StartService();
                        Console.WriteLine();
                        break;
                    case "u":
                        Console.WriteLine();
                        ServiceStatus();
                        Console.WriteLine();
                        break;
                    case "t":
                        Console.WriteLine();
                        StopService();
                        Console.WriteLine();
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Invalid Command");
                        Console.WriteLine();
                        PrintHelp();
                        Console.WriteLine();
                        break;
                }
            }
        }

        static void PrintTable()
        {
            if (refMgr.IsRunning)
            {
                Console.WriteLine(Strings.TableHeader);
                Console.WriteLine("______________________________________________________________________________________________");
                
                int i = 0;
                lock (registrar.ClientRegTable)
                {
                    ICollection<ClientEntry> values = registrar.ClientRegTable.Values;
                    foreach (ClientEntry entry in values)
                    {
                        Console.Write(i + ".\t");
                        Console.Write(entry.ClientEP + "\t" + entry.GroupEP + "\t\t\t"
                            + entry.JoinTime);
                        Console.WriteLine();
                        i++;
                    }
                }
            }
            else
            {
                Console.WriteLine(Strings.ServiceNotRunningStartTheService);
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine(Strings.Enter);
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.CmdLinePHelp, "     p"));
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.CmdLineSHelp, "     s"));
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.CmdLineTHelp, "     t"));
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.CmdLineUHelp, "     u"));
            //Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.CmdLineDHelp, "     d"));
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.CmdLineQHelp, "     q"));
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.CmdLineHHelp, "     h"));
        }

        static void StartService()
        {
            if (!refMgr.IsRunning)
            {
                refMgr.StartReflector();
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine(Strings.ServiceStartedSuccessfully);
                if ((ReflectorMgr.EnabledTrafficTypes & TrafficType.IPv4) == TrafficType.IPv4)
                {
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.UnicastSideRtpEndpoint, 
                        ReflectorMgr.Sockets.SockUCv4RTP.LocalEndPoint));
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.UnicastSideRtcpEndpoint, 
                        ReflectorMgr.Sockets.SockUCv4RTCP.LocalEndPoint));
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.MulticastSideRtpEndpoint, 
                        ReflectorMgr.Sockets.SockMCv4RTP.LocalEndPoint));
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.MulticastSideRtcpEndpoint, 
                        ReflectorMgr.Sockets.SockMCv4RTCP.LocalEndPoint));
                } 
                if ((ReflectorMgr.EnabledTrafficTypes & TrafficType.IPv6) == TrafficType.IPv6)
                {
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                        Strings.IPv6UnicastSideRtpEndpoint, ReflectorMgr.Sockets.SockUCv6RTP.LocalEndPoint));
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                        Strings.IPv6UnicastSideRtcpEndpoint, ReflectorMgr.Sockets.SockUCv6RTCP.LocalEndPoint));
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                        Strings.IPv6MulticastSideRtpEndpoint, ReflectorMgr.Sockets.SockMCv6RTP.LocalEndPoint));
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                        Strings.IPv6MulticastSideRtcpEndpoint, ReflectorMgr.Sockets.SockMCv6RTCP.LocalEndPoint));
                }
                Console.WriteLine();
                PrintHelp();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine(Strings.ServiceAlreadyStarted);
                if ((ReflectorMgr.EnabledTrafficTypes & TrafficType.IPv4) == TrafficType.IPv4)
                {
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.UnicastSideRtpEndpoint, 
                        ReflectorMgr.Sockets.SockUCv4RTP.LocalEndPoint));
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.UnicastSideRtcpEndpoint, 
                        ReflectorMgr.Sockets.SockUCv4RTCP.LocalEndPoint));
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.MulticastSideRtpEndpoint, 
                        ReflectorMgr.Sockets.SockMCv4RTP.LocalEndPoint));
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.MulticastSideRtcpEndpoint, 
                        ReflectorMgr.Sockets.SockMCv4RTCP.LocalEndPoint));
                } 
                if ((ReflectorMgr.EnabledTrafficTypes & TrafficType.IPv6) == TrafficType.IPv6)
                {
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                        Strings.IPv6UnicastSideRtpEndpoint, ReflectorMgr.Sockets.SockUCv6RTP.LocalEndPoint));
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                        Strings.IPv6UnicastSideRtcpEndpoint, ReflectorMgr.Sockets.SockUCv6RTCP.LocalEndPoint));
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                        Strings.IPv6MulticastSideRtpEndpoint, ReflectorMgr.Sockets.SockMCv6RTP.LocalEndPoint));
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                        Strings.IPv6MulticastSideRtcpEndpoint, ReflectorMgr.Sockets.SockMCv6RTCP.LocalEndPoint));
                }
            }
        }

        static void StopService()
        {
            if (refMgr.IsRunning)
            {
                refMgr.StopReflector();
                Console.WriteLine(Strings.ServiceStoppedSuccessfully);
            }
            else
            {
                Console.WriteLine(Strings.ServiceIsNotRunning);
            }
        }

        static void ServiceStatus()
        {
            if (refMgr.IsRunning)
            {
                Console.WriteLine(Strings.ServiceIsRunning);
                if ((ReflectorMgr.EnabledTrafficTypes & TrafficType.IPv4) == TrafficType.IPv4)
                {
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.UnicastSideRtpEndpoint, 
                        ReflectorMgr.Sockets.SockUCv4RTP.LocalEndPoint));
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.UnicastSideRtcpEndpoint, 
                        ReflectorMgr.Sockets.SockUCv4RTCP.LocalEndPoint));
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.MulticastSideRtpEndpoint, 
                        ReflectorMgr.Sockets.SockMCv4RTP.LocalEndPoint));
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.MulticastSideRtcpEndpoint, 
                        ReflectorMgr.Sockets.SockMCv4RTCP.LocalEndPoint));
                } 
                if ((ReflectorMgr.EnabledTrafficTypes & TrafficType.IPv6) == TrafficType.IPv6)
                {
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                        Strings.IPv6UnicastSideRtpEndpoint, ReflectorMgr.Sockets.SockUCv6RTP.LocalEndPoint));
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                        Strings.IPv6UnicastSideRtcpEndpoint, ReflectorMgr.Sockets.SockUCv6RTCP.LocalEndPoint));
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                        Strings.IPv6MulticastSideRtpEndpoint, ReflectorMgr.Sockets.SockMCv6RTP.LocalEndPoint));
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                        Strings.IPv6MulticastSideRtcpEndpoint, ReflectorMgr.Sockets.SockMCv6RTCP.LocalEndPoint));
                }
            }
            else
            {
                Console.WriteLine(Strings.ServiceIsNotRunning);
            }
        }

        static void DeleteClient()
        {
            if (refMgr.IsRunning)
            {
                Console.Write(Strings.PleaseEnterClientIPAddressToDelete + " ");
                String entry = Console.ReadLine();

                IPEndPoint ep;
                try
                {
                    String[] toks = entry.Split(new char[] { ':' });
                    IPAddress address = IPAddress.Parse(toks[0]);
                    int port = int.Parse(toks[1]);
                    ep = new IPEndPoint(address, port);
                }
                
                catch (Exception e)
                {
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.InvalidEntry, e.Message));
                    return;
                }

                if (registrar.ClientRegTable.ContainsKey(ep))
                {
                    lock (registrar.ClientRegTable)
                    {
                        registrar.ClientRegTable.Remove(ep);
                    }
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.ClientIPAddressDeleted, ep));
                }
                else
                {
                    Console.WriteLine(Strings.ClientIPAddressDoesNotExist);
                } 
            }
            else
            {
                Console.WriteLine(Strings.ServiceIsNotRunning);
            }
        }
    }
}
