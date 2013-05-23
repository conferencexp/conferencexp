using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using MSR.LST.ConferenceXP.DiagnosticWebService;
using System.Net;
using System.Configuration;

namespace MSR.LST.ConferenceXP {
    /// <summary>
    /// Start a thread to make a periodic query to the Diagnostic WebService. 
    /// Examine the query result (connectivity matrix) and compare with current
    /// participant set.  Raise events to indicate the onset and resolution of problems.
    /// </summary>
    public class DiagnosticMonitor {
        /// <summary>
        /// The threshold at which to raise throughput warning events.  
        /// The number we receive from the Diagnostic server is:
        ///     (sender packet rate - receiver packet rate)
        /// Before comparing with the threshold, we calculate the absolute value of:
        ///     (sender packet rate - receiver packet rate) / sender packet rate
        /// If the weighted average exceeds the threshold, we raise warning events.
        /// </summary>
        public static double ThroughputThreshold = 0.10;

        private DiagnosticWebService.DiagnosticWebService m_Service;
        private string m_Venue;
        private IPEndPoint m_MulticastEndpoint;
        private string m_LocalCname;
        private int m_Timeout = 10000; //Make configurable?
        private DiagnosticMonitorThread m_Thread = null;

        /// <summary>
        /// Currently known venue participants
        /// </summary>
        private List<string> m_Participants;
        
        /// <summary>
        /// The participants noted as missing last time we checked
        /// </summary>
        private List<string> m_PreviousMissingParticipants;
        
        /// <summary>
        /// The throughput numbers over the warning threshold last time we checked.
        /// </summary>
        private List<SenderReceiverPair> m_PreviousThroughputWarnings;

        /// <summary>
        /// Weighted averages of throughput numbers.
        /// </summary>
        private Dictionary<SenderReceiverPair,double> m_ThroughputAverages;

        public string Venue {
            get { return m_Venue; }
        }

        public IPEndPoint MulticastEndpoint {
            get { return m_MulticastEndpoint; }
        }

        public DiagnosticMonitor(string venue, IPEndPoint mcastEndpoint, string localCname) {
            String setting;
            if ((setting = ConfigurationManager.AppSettings[AppConfig.CXP_ThroughputWarningThreshold]) != null) {
                double warningThreshold;
                if (Double.TryParse(setting, out warningThreshold)) {
                    if (warningThreshold > 0) {
                        ThroughputThreshold = warningThreshold;
                    }
                }
            }

            if (!Conference.DiagnosticsEnabled) {
                throw new ApplicationException("DiagnosticMonitor requires that Diagnostics are enabled.");
            }
            m_Participants = new List<string>();
            m_Venue = venue;
            m_MulticastEndpoint = mcastEndpoint;
            m_LocalCname = localCname;
            m_PreviousMissingParticipants = new List<string>();
            m_PreviousThroughputWarnings = new List<SenderReceiverPair>();
            m_ThroughputAverages = new Dictionary<SenderReceiverPair, double>();
            m_Service = new DiagnosticWebService.DiagnosticWebService();
            m_Service.Url = Path.Combine(Conference.DiagnosticsWebService.ToString(), "DiagnosticWebService.asmx");
            m_Service.Timeout = m_Timeout;
        }

        public void Start() {
            if (m_Thread != null) {
                Stop();
            }
            m_Thread = new DiagnosticMonitorThread(m_Service,this);
        }

        public void Stop() {
            if (m_Thread != null) {
                m_Thread.Stop();
                m_Thread = null;
            }
        }

        /// <summary>
        /// Compare the latest Connectivity Matrix from the Diagnostic Service against the known set of participants.
        /// Raise events when changes are detected.
        /// </summary>
        /// <param name="callingThread"></param>
        private void CheckMatrix(DiagnosticMonitorThread callingThread) {
            if (!callingThread.Equals(m_Thread)) {
                return;
            }

            List<DiagnosticUpdateEventArgs> events = new List<DiagnosticUpdateEventArgs>();

            lock (this) {
                //Check for senders (rows) in the matrix that don't correspond to known participants.
                //If the missing participants list changed since last time we checked, queue events.
                CheckMissingParticipants(callingThread.ConnectivityMatrix, events);

                //Look for cells in the matrix with throughput metric above the threshold.
                //If this list changed since last time we checked, queue events.
                CheckThroughput(callingThread.ConnectivityMatrix, events);
            }

            //Raise events
            if (DiagnosticUpdate != null) {
                foreach (DiagnosticUpdateEventArgs args in events) {
                    DiagnosticUpdate(args);
                }
            }
        }

        /// <summary>
        /// Check each cell in the connectivity matrix for throughput numbers that are above the warning threshold.
        /// If the list changed since last time we checked, queue events.
        /// </summary>
        /// <param name="connectivityMatrix"></param>
        /// <param name="events"></param>
        private void CheckThroughput(ConnectivityMatrix connectivityMatrix, List<DiagnosticUpdateEventArgs> events) {
            /// Note: The sender and receiver numbers have been observed occasionally to be off by up to 20% 
            /// when there are no apparent throughput problems.  This is probably just due to the fact that
            /// the reporting intervals are not aligned. It seems rare however for the numbers to be off by
            /// more than 10% on consecutive intervals.  Given the inaccuracy, probably the best approach will
            /// be to use a weighted average.  The simple approach taken here is to average the current value
            /// with the previous average.

            List<SenderReceiverPair> currentPairs = new List<SenderReceiverPair>();

            //Update throughput averages
            foreach (Row r in connectivityMatrix.Rows) {
                foreach (Cell c in r.Cells) {
                    SenderReceiverPair srp = new SenderReceiverPair(r.SenderCname, c.ReceiverCname);
                    currentPairs.Add(srp);
                    if (!m_ThroughputAverages.ContainsKey(srp)) {
                        //Give every new SenderReceiverPair a zero to start and ignore the first set of data
                        //since we can sometimes get big throughput differentials during the join.
                        m_ThroughputAverages.Add(srp, 0.0);
                        //Trace.WriteLine("Created a new throughput average for this pair.  Ignoring current interval. " + srp.Sender + "/" + srp.Receiver  );
                        continue;
                    }

                    if (c.ThroughputDifferential < 0) {
                        //Negative ThroughputDifferential means more packets were received than sent.  Count it as zero.
                        m_ThroughputAverages[srp] = (m_ThroughputAverages[srp] / 2.0); 
                        //Trace.WriteLine("Differential for this interval is a negative number; counting as zero");
                        //Trace.WriteLine("Throughput average for " + srp.Sender + "/" + srp.Receiver + " is " + m_ThroughputAverages[srp].ToString());
                        continue;
                    }

                    if (r.SenderPacketRate == 0) {
                        //Trace.WriteLine("Sender packet rate is zero.");
                        continue;
                    }
                    
                    //Average current and previous values
                    double normalizedThroughputDifferential = c.ThroughputDifferential / r.SenderPacketRate;
                    m_ThroughputAverages[srp] = (m_ThroughputAverages[srp] + normalizedThroughputDifferential) / 2.0;
                    //Trace.WriteLine("Differential for this interval is " + normalizedThroughputDifferential.ToString());
                    //Trace.WriteLine("Throughput average for " + srp.Sender + "/" + srp.Receiver + " is " + m_ThroughputAverages[srp].ToString());
                }
            }

            //Purge averages for pairs that do not appear in the current matrix.  These probably left the venue.
            Dictionary<SenderReceiverPair, double> newAverages = new Dictionary<SenderReceiverPair, double>();
            foreach (SenderReceiverPair srp in m_ThroughputAverages.Keys) {
                if (currentPairs.Contains(srp)) {
                    newAverages.Add(srp, m_ThroughputAverages[srp]);
                }
                else {
                    //Trace.WriteLine("Purging pair: " + srp.Sender + "/" + srp.Receiver);
                }
            }
            m_ThroughputAverages = newAverages;


            //Look for averages above the warning threshold, and add warning events.
            List<SenderReceiverPair> currentThroughputWarnings = new List<SenderReceiverPair>();
            foreach (SenderReceiverPair srp in m_ThroughputAverages.Keys) {
                if (m_ThroughputAverages[srp] > DiagnosticMonitor.ThroughputThreshold) {
                    currentThroughputWarnings.Add(srp);
                    //Add event only if it is not already in the warning state
                    if (!m_PreviousThroughputWarnings.Contains(srp)) {
                        events.Add(new DiagnosticUpdateEventArgs(DiagnosticEventType.ThroughputWarningAdded, srp.Sender, srp.Receiver));
                    }                                     
                }
            }

            //Look for pairs in the previous list but not in the current list and add event to remove the warning.
            foreach (SenderReceiverPair srp in m_PreviousThroughputWarnings) {
                if (!currentThroughputWarnings.Contains(srp)) {
                    events.Add(new DiagnosticUpdateEventArgs(DiagnosticEventType.ThroughputWarningRemoved, srp.Sender, srp.Receiver));
                }
            }

            //Save the current list for next time.
            m_PreviousThroughputWarnings = currentThroughputWarnings;

        }

        private class SenderReceiverPair {
            public string Sender;
            public string Receiver;

            public SenderReceiverPair(string sender, string receiver) {
                Sender = sender;
                Receiver = receiver;
            }

            public override bool Equals(object otherobj) {
                SenderReceiverPair other = (SenderReceiverPair)otherobj;
                return (this.Sender.Equals(other.Sender) && this.Receiver.Equals(other.Receiver));
            }

            public override int GetHashCode() {
                String sr = Sender + Receiver;
                return sr.GetHashCode();
            }

        }

        /// <summary>
        /// Compare the participants in the connectivity matrix with known venue participants.
        /// Compare the missing participants with those that were missing last time we checked, and
        /// queue events for any changes.
        /// </summary>
        /// <param name="connectivityMatrix"></param>
        /// <param name="events"></param>
        private void CheckMissingParticipants(ConnectivityMatrix connectivityMatrix, List<DiagnosticUpdateEventArgs> events) {
            //Make the current list of missing participants.  
            List<string> missingParticipants = new List<string>();
            foreach (Row r in connectivityMatrix.Rows) {
                if ((!r.SenderCname.Equals(m_LocalCname)) &&
                    (!m_Participants.Contains(r.SenderCname))) {
                    missingParticipants.Add(r.SenderCname);
                }
            }

            //Compare missingParticipants with m_PreviousMissingParticipants
            foreach (string s in missingParticipants) {
                if (!m_PreviousMissingParticipants.Contains(s)) {
                    //Queue event for new missing participant
                    events.Add(new DiagnosticUpdateEventArgs(DiagnosticEventType.MissingParticipantAdded, s));
                }
            }
            foreach (string s in m_PreviousMissingParticipants) {
                if (!missingParticipants.Contains(s)) { 
                    //Queue event for resolution of missing participant
                    events.Add(new DiagnosticUpdateEventArgs(DiagnosticEventType.MissingParticipantRemoved, s));
                }
            }

            //Save the current list of missing participants for next time.
            m_PreviousMissingParticipants = missingParticipants;
        }

        /// <summary>
        /// Inform the DiagnosticMonitor that a new participant joined.
        /// </summary>
        /// <param name="cname"></param>
        public void AddParticipant(string cname) {
            lock (this) {
                if (!m_Participants.Contains(cname)) {
                    m_Participants.Add(cname);
                }
            }
        }

        /// <summary>
        /// Inform the DiagnosticMonitor that a participant left.
        /// </summary>
        /// <param name="cname"></param>
        public void RemoveParticipant(string cname) {
            lock (this) {
                if (m_Participants.Contains(cname)) {
                    m_Participants.Remove(cname);
                }
            }
        }

        public delegate void DiagnosticUpdateEventHandler(DiagnosticUpdateEventArgs e);
        /// <summary>
        /// Event to raise when Diagnostic Monitor events should be reflected in the UI
        /// </summary>
        public event DiagnosticUpdateEventHandler DiagnosticUpdate;



        /// <summary>
        /// A thread to do a periodic Diagnostic WebService query along with start and stop mechanics.
        /// </summary>
        private class DiagnosticMonitorThread {
            public bool m_StopThread;
            private Thread m_Thread;
            private DiagnosticWebService.ConnectivityMatrix m_Matrix;
            private DiagnosticWebService.DiagnosticWebService m_Service;
            private DiagnosticMonitor m_Monitor;

            public DiagnosticWebService.ConnectivityMatrix ConnectivityMatrix {
                get { return m_Matrix; }
            }

            public DiagnosticMonitorThread(DiagnosticWebService.DiagnosticWebService service, DiagnosticMonitor monitor) {
                m_Service = service;
                m_Monitor = monitor;
                m_Matrix = null;
                m_Thread = new Thread(new ThreadStart(ThreadMethod));
                m_Thread.Name = "Diagnostic Monitor Thread";
                m_StopThread = false;
                m_Thread.Start();
            }

            public void Stop() {
                m_StopThread = true;
                if (m_Thread != null) {
                    //Spawn a thread to kill the work thread to avoid delay.
                    ThreadPool.QueueUserWorkItem(new WaitCallback(StopThreadThread));
                }
            }

            private void StopThreadThread(object state) {
                if (m_Thread != null) {
                    if (!m_Thread.Join(10000)) {
                        m_Thread.Abort();
                    }
                    m_Thread = null;
                }
            }

            public void ThreadMethod() {
                //Trace.WriteLine("*** Diagnostic Monitor service thread starting: " + m_Service.Url);
                try {
                    while (!m_StopThread) {
                        //Make the webservice call, and update the Connectivity Matrix
                        if (UpdateMatrix()) {
                            if (!m_StopThread) {
                                //If successful, check against current state and raise UI events.
                                m_Monitor.CheckMatrix(this);
                            }
                        }
                
                        //Sleep for 10 seconds;
                        for (int i = 0; i < 100 && !m_StopThread; i++) { 
                            Thread.Sleep(100);
                        }
                    }
                }
                catch (ThreadAbortException) { 
                    //ignore
                }
            }

            private bool UpdateMatrix() {
                try {
                    m_Matrix = m_Service.GetMatrix(m_Monitor.Venue + "#" + m_Monitor.MulticastEndpoint.Address.ToString());
                }
                catch (Exception e) {
                    Trace.WriteLine(e.ToString());
                    m_Matrix = null;
                }

                if (m_Matrix == null) {
                    return false;
                }
                return true;
            }
        }


    public class DiagnosticUpdateEventArgs {
        public string Sender;
        public string Receiver;
        public DiagnosticEventType EventType;

        /// <summary>
        /// Constructor for add/remove missing participant
        /// </summary>
        /// <param name="type"></param>
        /// <param name="missingParticipant"></param>
        public DiagnosticUpdateEventArgs(DiagnosticEventType type, string missingParticipant) {
            EventType = type;
            Sender = Receiver = missingParticipant;
        }

        /// <summary>
        /// Constructor for add/remove throughput warning
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        public DiagnosticUpdateEventArgs(DiagnosticEventType type, string sender, string receiver) {
            EventType = type;
            Sender = sender;
            Receiver = receiver;
        }

        public override string ToString() {
            return "DiagnosticUpdateEventArgs: " + Enum.GetName(typeof(DiagnosticEventType),EventType) +
                ";sender=" + Sender + ";receiver=" + Receiver;
        }

    }

    public enum DiagnosticEventType {
        MissingParticipantAdded,
        MissingParticipantRemoved,
        ThroughputWarningAdded,
        ThroughputWarningRemoved
    }

}

}
