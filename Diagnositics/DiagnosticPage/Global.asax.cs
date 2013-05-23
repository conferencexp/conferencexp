using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;
using MSR.LST.Net;
using MSR.LST.Net.Rtp;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;
using MSR.LST;
using System.Text;
using edu.washington.cs.cct.cxp.utilities;
using System.Web.Services;
using System.IO;
using System.Globalization;
using Microsoft.Win32;

namespace edu.washington.cs.cct.cxp.diagnostics
{

    public class SenderData
    {
        //////////////////////////////////////////////////////////
        // Sender state 
        ////////////////////////////////////////////////////////////


        // Kb/sec
        private double dataRate = 0.0;
        private double packetRate = 0.0;

        private ulong packetsSent = 0;
        private ulong bytesSent = 0;

        // in units of "ticks"
        private long lastSendUpdate = 0;

        /// <summary>
        /// This constructor is useful for merging the data from two existing sender datas...
        /// </summary>

        public SenderData() { }

        public SenderData(SenderData s1, SenderData s2)
        {
            this.dataRate = s1.DataRate + s2.DataRate;
            this.packetRate = s1.PacketRate + s2.PacketRate;
            this.packetsSent = s1.PacketsSent + s2.PacketsSent;
            this.bytesSent = s1.BytesSent + s2.BytesSent;

            this.lastSendUpdate = Math.Max(s1.lastSendUpdate, s2.lastSendUpdate);

            this.source = s1.Source;
            this.cName = s1.CName;
        }

        public double DataRate
        {
            get { return dataRate; }
        }

        public double PacketRate
        {
            get { return packetRate; }
        }

        public DateTime LastSendUpdate
        {
            get { return new DateTime((long)lastSendUpdate); }
        }

        public ulong BytesSent
        {
            get { return bytesSent; }
        }

        public ulong PacketsSent
        {
            get { return packetsSent; }

        }

        public void updateSenderState(SenderReport report, long when)
        {

            if (when <= lastSendUpdate)
                return; // duplicate

            if (lastSendUpdate != 0)
            {
                double bytesSentThisInterval = (report.BytesSent - this.bytesSent);
                double packetsSentThisInterval = (report.PacketCount - this.packetsSent);

                double seconds = ((double)(when - lastSendUpdate)) / TimeSpan.TicksPerSecond;

                // convert to bits and kilobits
                dataRate = 8 * bytesSentThisInterval / seconds / 1000.0;
                packetRate = packetsSentThisInterval / seconds;
            }

            // update state
            this.bytesSent = report.BytesSent;
            this.packetsSent = report.PacketCount;
            this.lastSendUpdate = when;
        }

        private IPEndPoint source;

        internal IPEndPoint Source
        {
            get { return source; }
            set { source = value; }
        }


        private String cName;

        public String CName
        {
            get { return cName; }
            set { cName = value; }
        }

    }

    public class ReceiverData
    {
        /// <summary>
        /// Map from reportee (data sender) ssrc to a summary of receiver state
        /// </summary>
        private IDictionary<uint, ReceiverSummary> receiverSummaries = new Dictionary<uint, ReceiverSummary>();
        private long lastReceiverStateUpdate = 0; // units of ticks
        private readonly IPEndPoint endpoint;

        private String cName;

        public ReceiverData(IPEndPoint ipe)
        {
            endpoint = ipe;
        }

        public String CName
        {
            get { return cName; }
            set { cName = value; }
        }

        public ICollection<ReceiverSummary> ReceiverSummaries
        {
            get { return receiverSummaries.Values; }
        }

        public void updateReceiverState(ArrayList receiverReports, long when,VenueState venueState)
        {
            IDictionary<uint, ReceiverSummary> newSummaries = new Dictionary<uint, ReceiverSummary>();

            if (when <= lastReceiverStateUpdate)
                return; // duplicate information

            String timeString = DateTime.Now.ToString("s");

            foreach (ReceiverReport receiverReport in receiverReports)
            {

                ReceiverSummary summary = null;
                if (receiverSummaries.ContainsKey(receiverReport.SSRC) && lastReceiverStateUpdate > 0)
                {

                    /// We have an existing receiver summary: update session statistics 

                    summary = receiverSummaries[receiverReport.SSRC];

                    double packetsReceivedThisInterval =
                        receiverReport.ExtendedHighestSequence - summary.TotalPacketsReceived;

                    double lossesThisInterval = ((ulong) receiverReport.PacketsLost) - summary.TotalLosses;
                                       
                    double seconds = ((double)(when - lastReceiverStateUpdate)) / TimeSpan.TicksPerSecond;

                    summary.PacketRate = packetsReceivedThisInterval / seconds;
                    if (lossesThisInterval + packetsReceivedThisInterval == 0) {
                        summary.LossRate = 0.0;  //This can happen because some capabilities don't send much data.
                    }
                    else {
                        summary.LossRate = lossesThisInterval / (lossesThisInterval + packetsReceivedThisInterval);
                    }

                    if (venueState.UseLogging) {
                        SenderData senderState = venueState.GetSenderState(receiverReport.SSRC);
                        venueState.WriteLog(timeString + " " + endpoint + " " + senderState.Source + "/" + receiverReport.SSRC +
                                    " " + summary.PacketRate + " " + lossesThisInterval);
                    }
                }
                else
                {
                    /// create a new summary
                    summary = new ReceiverSummary(receiverReport.SSRC);
                }

                /// in either case, update the session statistics
                summary.TotalLosses = (ulong)receiverReport.PacketsLost;

                /// Note: we use EHS as total packets received!
                summary.TotalPacketsReceived = receiverReport.ExtendedHighestSequence;

                newSummaries[receiverReport.SSRC] = summary;
            } // for each receiver report...

            this.receiverSummaries = newSummaries;
            lastReceiverStateUpdate = when;
        }
    }

    /// <summary>
    /// Contains information specific to a sender/receiver pair
    /// </summary>
    public class ReceiverSummary
    {
        /// <summary>
        /// Merge two existing receiver summaries into one
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        public ReceiverSummary(ReceiverSummary r1, ReceiverSummary r2)
        {
            this.ssrc = 0; // this value should not be used after merging two summaries...
            this.packetRate = r1.PacketRate + r2.PacketRate;
            this.totalLosses = r1.TotalLosses + r2.TotalLosses;
            this.totalPacketsReceived = r1.TotalPacketsReceived + r2.TotalPacketsReceived;

            // XXX Is simple averaging the right thing to do?
            this.LossRate = (r1.LossRate + r2.LossRate) / 2;
        }

        /// <param name="ssrc">The ssrc of the sender</param>
        public ReceiverSummary(uint ssrc)
        {
            this.ssrc = ssrc;
        }

        /// <summary>
        /// This is the reportee's (data sender) ssrc
        /// </summary>
        private readonly uint ssrc;

        /// <summary>
        /// The below statistics pertain to a single reporting interval
        /// </summary>
        private double packetRate = 0.0; // packets/second

        public double PacketRate
        {
            get { return packetRate; }
            set { packetRate = value; }
        }
        private double lossRate = 0.0;

        public double LossRate
        {
            get { return lossRate; }
            set { lossRate = value; }
        }

        /// <summary>
        /// The statistics below are for the lifetime of a conference
        /// </summary>
        private ulong totalPacketsReceived = 0;
        private  ulong totalLosses = 0;

        public uint SSRC
        {
            get { return ssrc; }
        }
        

        public ulong TotalLosses
        {
            get { return totalLosses; }
            set { totalLosses = value; }
        }

       

        public ulong TotalPacketsReceived
        {
            get { return totalPacketsReceived; }
            set { totalPacketsReceived = value; }
        }


    }

    public class VenueState : IDisposable {
        public static readonly long HOST_STATE_TIMEOUT = 15 * 1000; // ms
        private static readonly string LOGGING_KEY = "SOFTWARE\\CCT\\DiagnosticService\\50\\LoggingEnabled\\";
        /// <summary>
        /// After this many consecutive log write failures, disable logging.
        /// </summary>
        private static readonly int LOG_FAILURE_MAX = 100;

        private readonly IDictionary<IPEndPoint,ReceiverData> receiverData =
            new TimeoutDictionary<IPEndPoint, ReceiverData>(HOST_STATE_TIMEOUT);

        private String logFileName = null;
        private String venueName;
        private bool useLogging;

        /// <summary>
        /// Consecutive log write failures
        /// </summary>
        private int logWriteFailureCounter = 0;
        
        /// <summary>
        /// Logging enabled bit
        /// </summary>
        public bool UseLogging
        {
            get { return useLogging; }
            set { 
                //Set logging bit and persist to the registry.
                useLogging = value;
                logWriteFailureCounter = 0;
                try {
                    using (RegistryKey key = Registry.LocalMachine.CreateSubKey(LOGGING_KEY)) {
                        if (key != null) {
                            if (useLogging) {
                                key.SetValue(this.venueName, "true");
                            }
                            else {
                                if (((string)key.GetValue(this.venueName,"false")) == "true")
                                    key.DeleteValue(this.venueName);
                            }
                        }
                    }
                }
                catch (Exception e) {
                    Global.WriteEventLog("Failed to persist logging state to the registry: " + e.ToString(), EventLogEntryType.Warning);
                }
            }
        }

        /// <summary>
        /// The name of the log file if logging is or was enabled.  Null otherwise.
        /// </summary>
        public String LogFileName
        {
            get { return logFileName; }
        } 
        
        public VenueState(String venueName)
        {
            this.venueName = venueName;
            this.logWriteFailureCounter = 0;
            /// Find out if logging should be enabled for this venue.  Since IIS restarts the 
            /// process under various circumstances, we persist this to the registry so that we
            /// can get more or less coninuous logging across restarts.
            this.useLogging = false;
            if (Global.LoggingEnabled) {
                try {
                    using (RegistryKey key = Registry.LocalMachine.CreateSubKey(LOGGING_KEY)) {
                        if (key != null) {
                            if (((string)key.GetValue(this.venueName, "false")) == "true") {
                                this.useLogging = true;
                            }
                        }
                    }
                }
                catch (Exception e) {
                    Global.WriteEventLog("Failed to retrieve persisted logging state from the registry: " + e.ToString(), EventLogEntryType.Warning);
                }
            }
            else {
                //If logging is globally disabled, clean out registry persistence.
                try {
                    Registry.LocalMachine.DeleteSubKey(LOGGING_KEY);
                }
                catch (Exception e) {
                    Global.WriteEventLog("Failed to clear persisted logging state in the registry: " + e.ToString(), EventLogEntryType.Warning);
                }
            }
        }


        /// <summary>
        /// If logging is enabled, write an entry to the log file.
        /// </summary>
        /// <param name="message"></param>
        public void WriteLog(string message) {
            if (this.useLogging) {
                String physicalRoot = HttpRuntime.AppDomainAppPath;

                //On first use, create the file and write a header.
                if (this.logFileName == null) {
                    try {
                        logFileName = "/CXP_DIAGNOSTIC_LOG-" + DateTime.Now.Ticks + ".txt";
                        using (StreamWriter logWriter = File.AppendText(physicalRoot + logFileName)) {
                            logWriter.WriteLine("#Time Receiver Sender PacketRate PacketsLost");
                            logWriter.Flush();
                            logWriter.Close();
                        }
                        this.logWriteFailureCounter = 0;
                    }
                    catch (Exception e) {
                        this.logWriteFailureCounter++;
                        if (this.logWriteFailureCounter == 1) { 
                            Global.WriteEventLog("Failed to write to logfile.  " + e.ToString(), EventLogEntryType.Warning);
                        }
                        if (this.logWriteFailureCounter >= VenueState.LOG_FAILURE_MAX) {
                            Global.WriteEventLog("Failed to write to logfile.  Logging will be disabled.  " + e.ToString(), EventLogEntryType.Warning);
                            useLogging = false;
                        }
                        return;
                    }
                }

                try {
                    using (StreamWriter logWriter = File.AppendText(physicalRoot + logFileName)) {
                        logWriter.WriteLine(message);
                        logWriter.Flush();
                        logWriter.Close();
                    }
                    this.logWriteFailureCounter = 0;
                }
                catch (Exception e) {
                    //Disable logging only if write failures persist for awhile.  This way we can roll the logs without
                    //causing the logging to become disabled.
                    this.logWriteFailureCounter++;
                    if (this.logWriteFailureCounter == 1) { 
                        Global.WriteEventLog("Failed to write to logfile.  " + e.ToString(), EventLogEntryType.Warning);
                    }
                    if (this.logWriteFailureCounter >= VenueState.LOG_FAILURE_MAX) {
                        Global.WriteEventLog("Failed to write to logfile.  Logging will be disabled.  " + e.ToString(), EventLogEntryType.Warning);
                        useLogging = false;
                    }
                    return;        
                }
            }
        }

        internal IDictionary<IPEndPoint, ReceiverData> ReceiverData
        {
            get { return receiverData; }
        }

        public ReceiverData GetReceiverDataWithoutCreating(IPEndPoint ep)
        {
            lock (receiverData)
            {
                if (receiverData.ContainsKey(ep))
                    return receiverData[ep];
                else return null;
            }
        }

        public ReceiverData GetReceiverData(IPEndPoint ep)
        {
            lock (receiverData)
            {
                if (receiverData.ContainsKey(ep))
                    return receiverData[ep];
                else return new ReceiverData(ep);
            }
        }

        private bool participantChange = false;
        /// <summary>
        /// Flags the web service that there is a participant change, so the connectivity matrix 
        /// should be rebuilt.
        /// </summary>
        internal bool ParticipantChange {
            get {
                if (participantChange) {
                    participantChange = false;
                    return true;
                }
                return false;
            }

            set {
                participantChange = value;
            }
        }


        /// <summary>
        /// There can be multiple RtpStream senders per host; thus, we use the ssrc to 
        /// uniquely identify the sender.
        /// </summary>
        private readonly IDictionary<uint,SenderData> senderData =
            new TimeoutDictionary<uint, SenderData>(HOST_STATE_TIMEOUT);

        public SenderData GetSenderState(uint ssrc)
        {
            lock (senderData)
            {
                if (senderData.ContainsKey(ssrc))
                    return senderData[ssrc];
                else return new SenderData();
            }
        }

        internal IDictionary<uint, SenderData> SenderData
        {
            get { return senderData; }
        }

        ~VenueState()
        {
            Dispose();
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }


    public class Global : System.Web.HttpApplication
    {

        public static int DIAGNOSTIC_PORT = 1776;

        public static readonly string EVENT_LOG_SOURCE = "DiagnosticService";

        public static readonly long VENUE_STATE_TIMEOUT = 30*1000; // ms

        /// <summary>
        /// Enable the UI to allow users to turn on logging.
        /// </summary>
        public static bool LoggingEnabled = false;

        // Map from venue name to venue-specific diagnostic state
        internal static IDictionary<String, VenueState> venueStateMap =
            new  TimeoutDictionary<String,VenueState>(VENUE_STATE_TIMEOUT);
          
        private UdpListener udpListener;
        private static Global theInstance = null;

        private volatile bool isRunning = false;

        EventLog eventLog = null;

        protected void Application_Start(object sender, EventArgs e)
        {
            try {
                if (EventLog.SourceExists(EVENT_LOG_SOURCE)) {
                    eventLog = new EventLog("Application");
                    eventLog.Source = EVENT_LOG_SOURCE;
                    eventLog.WriteEntry("Diagnostic Service starting.");
                }
            }
            catch {
                eventLog = null;
            }

            int diagPort;
            if (int.TryParse(ConfigurationManager.AppSettings["DiagnosticPort"], out diagPort)) {
                if (diagPort > 0) DIAGNOSTIC_PORT = diagPort;
            }

            bool loggingEnable;
            if (bool.TryParse(ConfigurationManager.AppSettings["LoggingEnabled"], out loggingEnable)) {
                LoggingEnabled = loggingEnable;
            }
    
           // Start a UDP socket to receive RTCP traffic
            IPAddress localAll = IPAddress.Any;
            //IPAddress multicastAdr = IPAddress.Parse(addr);
               
            IPEndPoint endPoint = new IPEndPoint(localAll,DIAGNOSTIC_PORT);

            udpListener = new UdpListener(endPoint, 0);
            isRunning = true;

            Thread thread = new Thread(Start);
            thread.IsBackground = true;
            thread.Name = "UDP receive thread";
            thread.Start();

            theInstance = this;
        }

        public static void WriteEventLog(String message, EventLogEntryType entryType) {
            if (theInstance != null) {
                theInstance.writeEventLog(message, entryType);
            }
        }


        private void writeEventLog(String message, EventLogEntryType entryType) {
            if (eventLog != null) {
                eventLog.WriteEntry(message,entryType);
            }
        }

       private void Start()
       {
            //BufferChunk chunk = new BufferChunk(2048);
            CompoundPacket compoundPacket = new CompoundPacket();
            EndPoint endPoint = null;

            while (isRunning)
            {
                try
                {
                     compoundPacket.Reset();
                   udpListener.ReceiveFrom(compoundPacket.Buffer, out endPoint);

                    compoundPacket.ParseBuffer();
                    //IPAddress ipAddress = ((IPEndPoint)endPoint).Address;
                    IPEndPoint ipEndpoint = (IPEndPoint)endPoint;

                    // The compound packet enumerator destroys its list during enumeration,
                    // so we keep track of packets that have yet to be processed
                    IList<RtcpPacket> yetToBeProcessed = new List<RtcpPacket>();


                    String venueName = null;
                    //uint ssrc = 0;
                    long when = 0; // in units of "ticks"

                    // scan through the compound packet, looking for key pieces of meta-data
                    // first, look for the app packet that specifies the venue
                    // also, obtain the ssrc and the time stamp
                    
                    foreach (RtcpPacket packet in compoundPacket)
                    {
                        if (packet.PacketType == (byte)Rtcp.PacketType.APP)
                        {
                            AppPacket appPacket = new AppPacket(packet);
                            if (appPacket.Name.Equals(Rtcp.APP_PACKET_NAME) &&
                                appPacket.Subtype == Rtcp.VENUE_APP_PACKET_SUBTYPE)
                            {

                                BufferChunk chunk = new BufferChunk(appPacket.Data);
                                when = chunk.NextInt64();
                                venueName = chunk.NextUtf8String(chunk.Length);
                                int padIndex = venueName.IndexOf((char)0);
                                if (padIndex > 0)
                                    venueName = venueName.Substring(0, padIndex);
                            }
                        }
                        else
                        {
                            yetToBeProcessed.Add(packet);
                        }
                    }

                    if (venueName == null)
                        continue; // can't do anything if we don't know the venue for this packet
                    if (when == 0)
                        continue; // need a timestamp
                    
                    VenueState venueState = null;
                    
                    // compound operations must always be locked...
                    lock (venueStateMap)
                    {
                        if (!venueStateMap.ContainsKey(venueName))
                            venueState = new VenueState(venueName);
                        else venueState = venueStateMap[venueName];
                    }


                    // scan again, this time processing the RTCP packets
                    foreach (RtcpPacket packet in yetToBeProcessed)
                    {

                        switch (packet.PacketType)
                        {
                            case (byte)Rtcp.PacketType.SR:
                                {
                                    SrPacket sr = new SrPacket(packet);
                                    
                                    SenderData senderData = venueState.GetSenderState(sr.SSRC);
                                    senderData.Source = ipEndpoint;

                                    senderData.updateSenderState(sr.SenderReport, when);

                                    // this "refreshes" the host state (so that it won't expire)
                                    venueState.SenderData[sr.SSRC] = senderData;
                                    break;
                                }

                            case (byte)Rtcp.PacketType.RR:
                                {
                                    RrPacket rr = new RrPacket(packet);
                                    ReceiverData receiverData = venueState.GetReceiverData (ipEndpoint);

                                    // currently, we replace all receiver summaries with the data
                                    // from a single RR packet
                                    receiverData.updateReceiverState(rr.ReceiverReports, when, venueState);
                                        

                                    // this "refreshes" the host state (so that it won't expire)
                                    venueState.ReceiverData[ipEndpoint] = receiverData;
                                    break;
                                }

                            case (byte)Rtcp.PacketType.SDES:
                                {
                                    SdesPacket sdp = new SdesPacket(packet);                                   

                                    foreach(SdesReport report in sdp.Reports())
                                    {
                                        SenderData senderData = venueState.GetSenderState(report.SSRC);
                                        senderData.CName = report.SdesData.CName;
                                        senderData.Source = ipEndpoint;

                                        // this "refreshes" the host state (so that it won't expire)
                                        venueState.SenderData[report.SSRC] = senderData;

                                        ReceiverData receiverData = 
                                            venueState.GetReceiverDataWithoutCreating(ipEndpoint);

                                        if (receiverData != null)
                                            receiverData.CName = report.SdesData.CName;
                                    }
                                    break;
                                }

                            case (byte)Rtcp.PacketType.BYE:
                                {
                                    //BYE packets occur when capabilities stop.  Clean out sender data only for the
                                    //ssrc's affected.  We leave receiver reports alone for now.
                                    ByePacket byePacket = new ByePacket(packet);
                                    foreach (uint ssrc in byePacket.SSRCs) {
                                        venueState.SenderData.Remove(ssrc);
                                    }
                                    //Set a flag to cause the matrix for this venue to be rebuilt on the next request.
                                    venueState.ParticipantChange = true;
                                    continue;
                                }

                            case (byte)Rtcp.PacketType.APP:
                                {
                                    // ignored

                                    break;
                                }
                        }
                     }  // foreach packet...

                    // refresh the venue state
                     venueStateMap[venueName] = venueState;
                 }
                catch (Exception e)
                {
                    Console.Out.WriteLine("Exception : " + e.ToString());

                    writeEventLog("Exception in receive thread: " + e.ToString(), EventLogEntryType.Warning);
                }
            } // loop forever...
        }

 

        //private SenderData getSenderData(VenueState venueState, uint ssrc)
        //{
        //    lock (venueState.SenderData)
        //    {
        //        if (venueState.SenderData.ContainsKey(ssrc))
        //            return venueState.SenderData[ssrc];
        //        else return new SenderData();
        //    }
        //}




        protected void Application_End(object sender, EventArgs e)
        {
            isRunning = false;
            writeEventLog("Diagnostic Service ending.", EventLogEntryType.Information);

            if (udpListener != null)
            {
                udpListener.Dispose();
                udpListener = null;
            }
        }
    }
}
