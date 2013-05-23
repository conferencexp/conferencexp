using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using edu.washington.cs.cct.cxp.diagnostics;
using System.Net;
using System.Xml.Serialization;
using System.Configuration;
using System.Diagnostics;

namespace DiagnosisPage {
    /// <summary>
    /// WebService APIs
    /// </summary>
    [WebService(Namespace = "http://diagnostics.cxp.cct.cs.washington.edu/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class DiagnosticWebService : System.Web.Services.WebService {

        /// <summary>
        /// Cache of venue/matrix pairs.
        /// </summary>
        public static Dictionary<string,ConnectivityMatrix> ConnectivityMatrixCache = new Dictionary<string, ConnectivityMatrix>();

        /// <summary>
        /// Refresh a matrix that is more than this many seconds old.
        /// </summary>
        public static int CONNECTIVITY_MATRIX_TIMEOUT = 10;

        public DiagnosticWebService() {
            int timeout;
            //Configure timeout from web.config
            if (int.TryParse(ConfigurationManager.AppSettings["CacheTimeoutSeconds"], out timeout)) {
                if (timeout >= 0) {
                    CONNECTIVITY_MATRIX_TIMEOUT = timeout;
                }
            }
        }


        [WebMethod]
        public string GetVersion() {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Return the latest matrix for the specified venue.
        /// </summary>
        /// <param name="venue"></param>
        /// <returns></returns>
        [WebMethod]
        public ConnectivityMatrix GetMatrix(string venue) {
            if ((Global.venueStateMap!= null) && (Global.venueStateMap.ContainsKey(venue))) {
                VenueState venueState = Global.venueStateMap[venue];
                //The VenueState.ParticipantChange flag indicates that a participant left the venue since we checked last.
                if ((!venueState.ParticipantChange) && 
                    (DiagnosticWebService.ConnectivityMatrixCache.ContainsKey(venue))) {
                    //Here we have a cached value, check for timeout.
                    if (DateTime.Now - DiagnosticWebService.ConnectivityMatrixCache[venue].CreationTime < 
                        TimeSpan.FromSeconds(DiagnosticWebService.CONNECTIVITY_MATRIX_TIMEOUT)) {
                        //Debug.WriteLine("Returned Cached Matrix.");
                        return DiagnosticWebService.ConnectivityMatrixCache[venue];
                    }
                    else {
                        //cache timeout
                        DiagnosticWebService.ConnectivityMatrixCache.Remove(venue);
                    }
                }
                
                ConnectivityMatrix cm = new ConnectivityMatrix(venueState);
                if (DiagnosticWebService.ConnectivityMatrixCache.ContainsKey(venue)) {
                    DiagnosticWebService.ConnectivityMatrixCache.Remove(venue);
                }
                DiagnosticWebService.ConnectivityMatrixCache.Add(venue, cm);
                //Debug.WriteLine("Built new matrix: " + cm.ToString());
                return cm;
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// A more or less concise connectivity matrix for a venue.  Each row represents a sender.
        /// Each cell in a row represents one receiver of that sender's streams.  The connectivity metric contained in
        /// the cell is throughput differential: (sent packet rate - received packet rate).
        /// Note that there are some limitations on what we can
        /// serialize in the context of a WebMethod, so to keep it simple we stick to basic types.
        /// </summary>
        public class ConnectivityMatrix {
            /// <summary>
            /// One row per sender.
            /// </summary>
            public Row[] Rows;

            /// <summary>
            /// Used only on the server to find out when this matrix is stale so that we can create a fresh one.
            /// </summary>
            [XmlIgnore]
            public DateTime CreationTime = DateTime.MinValue;

            //Required for serialization
            public ConnectivityMatrix() { }

            /// <summary>
            /// Build a new matrix
            /// </summary>
            /// <param name="venueState"></param>
            public ConnectivityMatrix(VenueState venueState) {
                // Aggregate total sender packet rates for each unique sender cname, and 
                // compile a mapping of sender ssrc to cname which we will use for merging receiver reports.
                Dictionary<string, Row> matrix = new Dictionary<string, Row>(); 
                Dictionary<uint, string> ssrc2cname = new Dictionary<uint, string>();
                lock (venueState.SenderData) {
                    foreach (uint ssrc in venueState.SenderData.Keys) {
                        SenderData sd = venueState.SenderData[ssrc];
                        //Debug.WriteLine("  Building Matrix SenderData.cname=" + sd.CName + ";ssrc=" + ssrc.ToString());
                        if (matrix.ContainsKey(sd.CName)) {
                            matrix[sd.CName].SenderPacketRate += sd.PacketRate;
                        }
                        else {
                            matrix.Add(sd.CName, new Row(sd.CName, sd.PacketRate));
                        }
                        ssrc2cname.Add(ssrc, sd.CName);
                    }
                }

                ////Test code:  Create and remove a bogus sender to test missing participant functionality.
                //if (DateTime.Now.Second < 30) {
                //    matrix.Add("bogus@cs.washington.edu", new Row("bogus@cs.washington.edu", 50.15));
                //}
                ////End test code

                // Create the NxN matrix with row and column for each cname
                foreach (Row r in matrix.Values) {
                    r.CreateCells(matrix.Values);
                }

                lock (venueState.ReceiverData) {
                    // For each receiver summary, find the sender cname and update the appropriate sender's row.
                    foreach (ReceiverData rd in venueState.ReceiverData.Values) {
                        foreach (ReceiverSummary rs in rd.ReceiverSummaries) {
                            if (ssrc2cname.ContainsKey(rs.SSRC)) {
                                string senderCname = ssrc2cname[rs.SSRC];
                                if (matrix.ContainsKey(senderCname)) {
                                    matrix[senderCname].UpdateReceiver(rd.CName, rs.PacketRate);
                                }
                            }
                        }
                    }
                }

                //Commit: copy to serialized arrays
                foreach (Row r in matrix.Values) {
                    r.CommitRow();
                }
                Rows = new Row[matrix.Count];
                matrix.Values.CopyTo(Rows, 0);

                CreationTime = DateTime.Now;

            }

            public override string ToString() {
                if (Rows == null) {
                    return "ConnectivityMatrix is null";
                }
                else {
                    string outstr = "";
                    foreach (Row r in Rows) {
                        outstr += r.ToString() + "\r\n";
                    }
                    return outstr;
                }
            }


            /// <summary>
            /// We have one row per sender, with one cell in the row for each receiver.
            /// </summary>
            public class Row {
                public double SenderPacketRate;
                public string SenderCname;
                public Cell[] Cells;

                //The non-serialized member used to help build the row's contents.
                [XmlIgnore]
                public Dictionary<string,Cell> CellDict;

                /// <summary>
                /// Required for serialization
                /// </summary>
                public Row() { }

                public Row(string senderCname, double packetRate) {
                    SenderCname = senderCname;
                    SenderPacketRate = packetRate;
                }

                /// <summary>
                /// Create the cells across this row.  Each cell begins life with the throughput differential
                /// set to the total sender packet rate.  
                /// </summary>
                /// <param name="rowCollection"></param>
                internal void CreateCells(Dictionary<string, Row>.ValueCollection cols) {
                    CellDict = new Dictionary<string, Cell>();
                    foreach (Row col in cols) {
                        //Exclude the loopback cell
                        if (!SenderCname.Equals(col.SenderCname)) {
                            CellDict.Add(col.SenderCname, new Cell(col.SenderCname, SenderPacketRate));
                        }
                    }
                }

                /// <summary>
                /// Update the specified receiver cell with the specified received packet rate
                /// </summary>
                /// <param name="receiverCname"></param>
                /// <param name="packetRate"></param>
                internal void UpdateReceiver(string receiverCname, double packetRate) { 
                    if (CellDict.ContainsKey(receiverCname)) {
                        CellDict[receiverCname].UpdateCell(packetRate);
                    }
                }

                /// <summary>
                /// Copy all cells to the serialized array
                /// </summary>
                internal void CommitRow() {
                    Cells = new Cell[CellDict.Count];
                    CellDict.Values.CopyTo(Cells, 0);
                }

                public override string ToString() {
                    string s = "Row{sender=" + SenderCname + ";PacketsSent=" + SenderPacketRate.ToString() + ";";
                    foreach (Cell c in Cells) {
                        s += c.ToString();
                    }
                    s += "}";
                    return s;
                }
            }

            /// <summary>
            /// Data for one receiver and one sender
            /// </summary>
            public class Cell {
                public string ReceiverCname;
                public double ThroughputDifferential;

                //Required for serialization
                public Cell() { }

                public Cell(string receiverCname, double packetRate) {
                    ReceiverCname = receiverCname;
                    ThroughputDifferential = packetRate;
                }

                /// <summary>
                /// Subtract the specified packet rate from the current value
                /// </summary>
                /// <param name="receivedPacketRate"></param>
                internal void UpdateCell(double receivedPacketRate) {
                    this.ThroughputDifferential -= receivedPacketRate;
                }

                public override string ToString() {
                    return "Cell{receiver=" + ReceiverCname + ";differential=" + ThroughputDifferential.ToString() + "}";
                }
            }

        }

    }
}
