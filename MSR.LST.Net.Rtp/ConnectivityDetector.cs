using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Windows.Forms;


namespace MSR.LST.Net.Rtp
{
    public class ConnectivityDetector
    {
        #region Statics

        private static string ipAddress = "233.4.12.3";
        private static ushort port = 5004;
        private static int updateInterval = 10000;

        static ConnectivityDetector()
        {
            string setting;

            if((setting = ConfigurationManager.AppSettings[AppConfig.CD_IPAddress]) != null)
            {
                ipAddress = setting;
            }

            if((setting = ConfigurationManager.AppSettings[AppConfig.CD_Port]) != null)
            {
                port = ushort.Parse(setting, CultureInfo.InvariantCulture);
            }

            if((setting = ConfigurationManager.AppSettings[AppConfig.CD_UpdateIntervalSeconds]) != null)
            {
                updateInterval = ushort.Parse(setting, CultureInfo.InvariantCulture) * 1000;
            }
        }

        #endregion Statics

        #region Members

        private RtpEL eventLog = new RtpEL(RtpEL.Source.ConnectivityDetector);

        private string cName; // "ConnectivityDetector@" + HostName, set in constructor
        private const string name = "CD";

        private RtpParticipant rtpParticipant;
        private RtpSession rtpSession;
        private RtpSender rtpSender;

        private bool networkReceive = false;
        private bool networkSend = false;
        private bool wellKnownHost = false;

        private ArrayList wellKnownHosts = new ArrayList();

        private System.Threading.Timer updateThread;

        private object classLock = new object();
        private bool disposed = false;

        private DateTime lastSend = DateTime.Now;

        #endregion

        #region Constructors

        public ConnectivityDetector()
        {
            try
            {
                cName = "ConnectivityDetector@" + HostName;

                InitializeWellKnownHosts();
                InitializeNetwork();

                // Wait N seconds, and only call this once
                updateThread = new System.Threading.Timer(new TimerCallback(UpdateThread), null, updateInterval, updateInterval);
            }
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 0);
            }
        }

        public void Dispose()
        {
            lock(classLock)
            {
                try
                {
                    disposed = true;

                    // Unhook events
                    RtpEvents.ReceiverReport            -= new RtpEvents.ReceiverReportEventHandler(RtpReceiverReport);
                    RtpEvents.DuplicateCNameDetected    -= new RtpEvents.DuplicateCNameDetectedEventHandler(DuplicateCNameDetected);

                    if(updateThread != null)
                    {
                        updateThread.Dispose();
                        updateThread = null;
                    }

                    if(rtpSession != null)
                    {
                        rtpSession.Dispose();
                        rtpSession = null;
                        rtpSender = null;
                    }
                }
                catch(Exception e)
                {
                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                        Strings.ErrorDisposingConnectivityDetector, e.ToString()), EventLogEntryType.Error, 0);
                }
            }
        }

        
        #endregion

        #region Public

        public string MulticastIPAddress
        {
            get{return ipAddress;}
        }
        public int MulticastPort
        {
            get{return port;}
        }
        public string LocalMulticastInterface
        {
            get{return MSR.LST.Net.Utility.GetLocalMulticastInterface().ToString();}
        }
        public string HostName
        {
            get{return Dns.GetHostEntry(Dns.GetHostName()).HostName;}
        }


        #endregion

        #region Private

        private void InitializeWellKnownHosts()
        {
            try
            {
                wellKnownHosts.Add("ConnectivityDetector@venues.conferencexp.net");
                wellKnownHosts.Add("ConnectivityDetector@disc1.cs.washington.edu");
                wellKnownHosts.Add("ConnectivityDetector@holodeck.eecs.berkeley.edu");
            }
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, (int)RtpEL.ID.Error);
            }
        }

        private void InitializeNetwork()
        {
            RtpEvents.ReceiverReport            += new RtpEvents.ReceiverReportEventHandler(RtpReceiverReport);
            RtpEvents.DuplicateCNameDetected    += new RtpEvents.DuplicateCNameDetectedEventHandler(DuplicateCNameDetected);

            // Create participant
            rtpParticipant = new RtpParticipant(cName, name);
            rtpParticipant.SetTool(true);

            // Create session with Participant and Rtp data
            rtpSession = new RtpSession(new IPEndPoint(IPAddress.Parse(ipAddress), port), rtpParticipant, true, true);

            // Create RtpSender
            rtpSender = rtpSession.CreateRtpSender(HostName, PayloadType.PipecleanerSignal, null);
        }

        private void UpdateThread(object state)
        {
            lock(classLock)
            {
                if(!disposed)
                {
                    try
                    {
                        CheckForOthers();
                        CheckForI2();

                        RaiseConnectivityEvent();

                        ClearFlags();

                        SendRtpData();
                    }
                    catch (Exception e)
                    {
                        eventLog.WriteEntry(e.ToString(),EventLogEntryType.Error, (int)RtpEL.ID.Error);
                    }
                }
            }
        }

        private void RtpReceiverReport(object sender, RtpEvents.ReceiverReportEventArgs ea)
        {
            // XXX deprecated!

            networkSend = true;
            // We'll take a receiver report from any RtpSession
            //if (ea.dataSourceSSRC == rtpSession

            //if(ea.SenderCName != cName)
            //{
            //    if (ea.SourceCName == cName)
            //    {
            //        networkSend = true;
            //    }
            //}
        }

        private void DuplicateCNameDetected (object sender, RtpEvents.DuplicateCNameDetectedEventArgs ea)
        {
            if (sender == rtpSession)
            {
                string msg = string.Format(CultureInfo.CurrentCulture, Strings.DuplicateCnameWasDetected, 
                    ea.IPAddresses[0].ToString(), ea.IPAddresses[1].ToString());

                eventLog.WriteEntry(msg, EventLogEntryType.Error, 0);
                RtlAwareMessageBox.Show(null, msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

                Dispose();
            }
        }

        private void CheckForOthers()
        {
            if (rtpSession.Participants.Count > 1)
            {
                networkReceive = true;
            }
        }
        private void CheckForI2()
        {
            // Look for well-known hosts (Internet2 hosts)
            foreach(string wkh in wellKnownHosts)
            {
                if(rtpSession.Participants.ContainsKey(wkh))
                {
                    wellKnownHost = true;
                    break;
                }
            }
        }

        private string Details()
        {
            string details = Strings.ConferencexpNodesDetected + "\n";
            foreach (RtpParticipant p in new ArrayList(rtpSession.Participants.Values))
            {
                details += string.Format(CultureInfo.CurrentCulture, "  {0},  {1}\n", p.CName, p.Tool);
            }

            return details;
        }

        private void RaiseConnectivityEvent()
        {
            try
            {
                if (Connectivity != null)
                {
                    Connectivity(networkReceive, networkSend, wellKnownHost, Details());
                }
            }
            catch (Exception e)
            {
                eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 0);
            }
        }

        private void ClearFlags()
        {
            networkReceive = false;
            networkSend = false;
            wellKnownHost = false;
        }

        private void SendRtpData()
        {
            // Only send data once per minute
            if(DateTime.Now.Subtract(lastSend).Minutes >= 1)
            {
                rtpSender.Send((BufferChunk)name);
                lastSend = DateTime.Now;
            }
        }


        #endregion

        #region Events

        public delegate void ConnectivityEventHandler(bool networkReceiveDetected, 
            bool networkSendDetected, bool wellKnownHostDetected, string details);

        public event ConnectivityEventHandler Connectivity;

        #endregion
    }
}
