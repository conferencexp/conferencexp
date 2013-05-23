using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Net;

using MSR.LST.Net;
using MSR.LST.Net.Rtp;
using System.Drawing;
using System.Diagnostics;

namespace edu.washington.cs.cct.cxp.diagnostics
{

    public partial class _Default : System.Web.UI.Page
    {

        public readonly int REFRESH_INTERVAL = 10;
        public readonly double LOSS_HIGHLIGHT_THRESHOLD = .01; // percent loss

        private DataTable CreateEmptySenderTable()
        {

            DataTable senderTable = new DataTable("Sender Table");
            DataColumn ssrc = senderTable.Columns.Add("Sender", Type.GetType("System.Int32"));
            ssrc.Unique = true;
            senderTable.Columns.Add("IP Addr", Type.GetType("System.String"));
            senderTable.Columns.Add("CName", Type.GetType("System.String"));

            senderTable.Columns.Add("Packets Sent", Type.GetType("System.Int64"));
            senderTable.Columns.Add("Bytes Sent", Type.GetType("System.Int64"));
            senderTable.Columns.Add("Data Rate", Type.GetType("System.String"));
            senderTable.Columns.Add("Packet Rate", Type.GetType("System.String"));
            senderTable.Columns.Add("Last update*", Type.GetType("System.String"));

            return senderTable;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!this.IsPostBack)
            {
                //Check for optional initial venue to display as given by the query string
                string initialVenue = CheckQueryString();

                // initialize the drop-down list of venues
                BuildVenueList(initialVenue);
                BuildTables();
            }

           // Response.AddHeader("Refresh", REFRESH_INTERVAL.ToString());
        }

        private string CheckQueryString() {
            if ((this.ClientQueryString != null) && (this.ClientQueryString != "")) {
                string[] tokens = this.ClientQueryString.Split(new char[] { '=', '&' });
                if ((tokens.Length == 2) &&
                    (tokens[0].Equals("venue"))) {
                    string decodedVenueName = HttpUtility.UrlDecode(tokens[1]);
                    //Match the venue moniker either including both name and multicast address, or with just the name.
                    //This is because some clients failed to properly escape the # and the second case will still work for them.
                    if (Global.venueStateMap.Keys.Contains(decodedVenueName)) {
                        return decodedVenueName;
                    }
                    foreach (string k in Global.venueStateMap.Keys) {
                        if (k.StartsWith(decodedVenueName + "#")) {
                            return k;
                        }
                    }
                }
            }
            return null;
        }

        private void BuildVenueList(string initialValue)
        {
            DropDownList1.DataSource = Global.venueStateMap;
            DropDownList1.DataTextField = "Key";
            DropDownList1.DataValueField = "Key";
            this.DataBind();
            if (initialValue != null) {
                DropDownList1.SelectedValue = initialValue;
            }
        }


           
        private void BuildAdvancedReceiverTables(VenueState venueState,out DataTable thptTable,out DataTable lossTable)
        {
            thptTable = new DataTable();
            lossTable = new DataTable();

            thptTable.Columns.Add("Receiver IP", Type.GetType("System.String"));
            thptTable.Columns.Add("CName", Type.GetType("System.String"));
            lossTable.Columns.Add("Receiver IP", Type.GetType("System.String"));
            lossTable.Columns.Add("CName", Type.GetType("System.String"));

            // keep track of the packet sending rate for each sender; this avoids having
            // to hold multiple locks at the same time...

            IDictionary<uint,double> sendingRateMap = new Dictionary<uint,double>();

            lock (venueState.SenderData)
            {
                // add a column for each sender... Keep the key set sorted so that elements don't
                // bounce around between rows
       
                foreach (uint ssrc in getSortedSenders(venueState))
                {
                    thptTable.Columns.Add(ssrc.ToString(), Type.GetType("System.String"));
                    lossTable.Columns.Add(ssrc.ToString(), Type.GetType("System.String"));

                    sendingRateMap[ssrc] = venueState.SenderData[ssrc].PacketRate;
                }
            }

            lock (venueState.ReceiverData)
            {

                // add a row for each receiver
         
                foreach (IPEndPoint endPoint in getSortedReceivers(venueState))
                {

                    ReceiverData receiverData = venueState.ReceiverData[endPoint];
                    if (receiverData == null)
                        continue; // shouldn't happen...

                    // process receiver summaries
                    ICollection<ReceiverSummary> summaries = receiverData.ReceiverSummaries;
                    if (summaries == null || summaries.Count == 0)
                        continue;

                    DataRow thptRow = thptTable.NewRow();
                    DataRow lossRow = lossTable.NewRow();

                    thptRow["Receiver IP"] = endPoint.ToString();
                    thptRow["CName"] = receiverData.CName;
                    
                    lossRow["Receiver IP"] = endPoint.ToString();
                    lossRow["CName"] = receiverData.CName;


                    // initialize sender columns...
                    foreach (uint ssrc in getSortedSenders(venueState))
                    {

                        lossRow[ssrc.ToString()] = "no data";
                        thptRow[ssrc.ToString()] = "no data";
                    }

                    // process receiver summaries
                    //ICollection<ReceiverSummary> summaries = hostState.ReceiverSummaries;
                    foreach (ReceiverSummary summary in summaries)
                    {
                        uint reporteeSSRC = summary.SSRC;
                        if (thptTable.Columns.Contains(reporteeSSRC.ToString()))
                        {
                            double throughputDifferential = summary.PacketRate - sendingRateMap[reporteeSSRC];
                            thptRow[reporteeSSRC.ToString()] = String.Format("{0:N} pk/sec", throughputDifferential);

                            lossRow[reporteeSSRC.ToString()] = String.Format("{0:P}", summary.LossRate);
                        }
                    } // for each receiver summary...

                    thptTable.Rows.Add(thptRow);
                    lossTable.Rows.Add(lossRow);

                }  // for each receiver in the venue
            } // venue.receiverData lock
        }

        private DataTable BuildAdvancedSenderTable(VenueState venueState)
        {
            DataTable senderTable = CreateEmptySenderTable();

            lock (venueState.SenderData)
            {
                foreach (uint ssrc in getSortedSenders(venueState))
                {

                    SenderData senderData = venueState.SenderData[ssrc];

                    if (senderData == null)
                        continue;

                    DataRow row = senderTable.NewRow();

                    row["Sender"] = ssrc;
                    row["IP Addr"] = senderData.Source.ToString();
                    row["CName"] = senderData.CName;

                    row["Packets Sent"] = senderData.PacketsSent;
                    row["Bytes Sent"] = senderData.BytesSent;

                    double kb = senderData.DataRate;
                    row["Data rate"] = kb.ToString("N") + " Kb/sec";
                    row["Packet rate"] = senderData.PacketRate.ToString("N") + " pk/sec";

                    if (senderData.LastSendUpdate.Equals(DateTime.MinValue)) { 
                        row["Last update*"] = "No sender data received."; 
                    }
                    else {
                        row["Last update*"] = senderData.LastSendUpdate.ToString();
                    }

                    senderTable.Rows.Add(row);
                } // for each host in a the venue...
            } // unlock

            return senderTable;
        }


        private void BuildBasicReceiverTables(VenueState venueState,out DataTable thptTable,out DataTable lossTable,
            IDictionary<IPEndPoint,SenderData> senderSummaries)
        {
            thptTable = new DataTable();
            lossTable = new DataTable();

            thptTable.Columns.Add("Receiver IP", Type.GetType("System.String"));
            thptTable.Columns.Add("CName", Type.GetType("System.String"));
            lossTable.Columns.Add("Receiver IP", Type.GetType("System.String"));
            lossTable.Columns.Add("CName", Type.GetType("System.String"));

            // add a column for each sender...
            foreach (IPEndPoint endPoint in senderSummaries.Keys)  
            {
                
                thptTable.Columns.Add(endPoint.ToString(), Type.GetType("System.String"));
                lossTable.Columns.Add(endPoint.ToString(), Type.GetType("System.String"));   
            }


            lock (venueState.ReceiverData)
            {
                // add a row for each receiver
                foreach (IPEndPoint receiverEndpoint in getSortedReceivers(venueState))
                {

                    ReceiverData receiverData = venueState.ReceiverData[receiverEndpoint];
                    if (receiverData == null)
                        continue; // shouldn't happen...

                    // process receiver summaries
                    ICollection<ReceiverSummary> summaries = receiverData.ReceiverSummaries;
                    if (summaries == null || summaries.Count == 0)
                        continue;

                    DataRow thptRow = thptTable.NewRow();
                    DataRow lossRow = lossTable.NewRow();

                    thptRow["Receiver IP"] = receiverEndpoint.ToString();
                    thptRow["CName"] = receiverData.CName;

                    lossRow["Receiver IP"] = receiverEndpoint.ToString();
                    lossRow["CName"] = receiverData.CName;

                    // initialize sender columns...
                    foreach (IPEndPoint endPoint in senderSummaries.Keys)
                    {

                        lossRow[endPoint.ToString()] = "no data";
                        thptRow[endPoint.ToString()] = "no data";
                    }

                    // process receiver summaries, while merging data from co-located data streams
                    // thus, we should end up with one column per sending host, regardless of its
                    // number of output streams...
                    IDictionary<IPEndPoint, ReceiverSummary> mergedReceiverSummaries =
                        new Dictionary<IPEndPoint, ReceiverSummary>();

                    // this is the merge phase...
                    foreach (ReceiverSummary summary in summaries)
                    {
                        SenderData senderData;
                        if (!venueState.SenderData.TryGetValue(summary.SSRC, out senderData))
                            continue; // no sender data for this receiver summary


                        IPEndPoint senderEndpoint = senderData.Source;
                        if (senderEndpoint == null)
                            continue;

                        if (mergedReceiverSummaries.ContainsKey(senderEndpoint))
                        {
                            // merge the summary data with existing data from this sender
                            ReceiverSummary existingSummary = mergedReceiverSummaries[senderEndpoint];
                            ReceiverSummary updatedSummary = new ReceiverSummary(existingSummary,summary);
                            mergedReceiverSummaries[senderEndpoint] = updatedSummary;
                        }
                        else 
                        {
                            // otherwise, create a new summary entry for this EndPoint
                            mergedReceiverSummaries[senderEndpoint] = summary;
                        }
                    }

                    // At this point, we have merged data from co-located data streams.  Populate
                    // the columns corresponding to source IP addreses.
                    foreach (IPEndPoint senderEndpoint in mergedReceiverSummaries.Keys)
                    {
                        if (thptTable.Columns.Contains(senderEndpoint.ToString()))
                        {
                            ReceiverSummary summary = mergedReceiverSummaries[senderEndpoint];
                            SenderData senderData = senderSummaries[senderEndpoint];

                            double throughputDifferential = summary.PacketRate - senderData.PacketRate;

                            try
                            {
                                thptRow[senderEndpoint.ToString()] = String.Format("{0:N} pk/sec", throughputDifferential);

                                //String.Format("{1:N} - {2:N} = {0:N} pk/sec", throughputDifferential,
                                //summary.PacketRate , senderData.PacketRate);

                                lossRow[senderEndpoint.ToString()] = String.Format("{0:P}", summary.LossRate);
                            }
                            catch (Exception)
                            {
                                // these operations can fail when sender and receiver tables are out of sync
                            }
                        }
                    }

                    // mark "loopback" data as such
                    try
                    {
                        thptRow[receiverEndpoint.ToString()] = "loopback";
                        lossRow[receiverEndpoint.ToString()] = "loopback";
                    }
                    catch (Exception)
                    {
                        // these operations can fail if sender, receiver tables are out of sync...
                    }

                    thptTable.Rows.Add(thptRow);
                    lossTable.Rows.Add(lossRow);

                }  // for each receiver in the venue
            } // venue.receiverData lock
        }


        /// <summary>
        /// Merge all streams from a given IP,Port pair.  Return the summarized sender data in a dictionary,
        /// which can be used to build the BasicReceiverTable
        /// </summary>
        /// <param name="venueState"></param>
        /// <returns></returns>
        private DataTable BuildBasicSenderTable(VenueState venueState,
            out IDictionary<IPEndPoint,SenderData> senderSummaries)
        {
            DataTable senderTable = new DataTable();

            senderTable.Columns.Add("IP Addr", Type.GetType("System.String"));
            senderTable.Columns.Add("CName", Type.GetType("System.String"));

            senderTable.Columns.Add("Packets Sent", Type.GetType("System.Int64"));
            senderTable.Columns.Add("Bytes Sent", Type.GetType("System.Int64"));
            senderTable.Columns.Add("Data Rate", Type.GetType("System.String"));
            senderTable.Columns.Add("Packet Rate", Type.GetType("System.String"));
            senderTable.Columns.Add("Last update*", Type.GetType("System.String"));

            senderSummaries = new Dictionary<IPEndPoint, SenderData>();

            lock (venueState.SenderData)
            {
                foreach (uint ssrc in getSortedSenders(venueState))
                {

                    SenderData senderData = venueState.SenderData[ssrc];
                    if (senderData == null)
                        continue;


                    IPEndPoint endPoint = senderData.Source;

                    if (endPoint == null)
                        continue;

                    if (senderSummaries.ContainsKey(endPoint))
                    {
                        // merge new data with old...
                        SenderData existingData = senderSummaries[endPoint];

                        SenderData mergedData = new SenderData(senderData, existingData);
                        senderSummaries[endPoint] = mergedData;
                    }
                    else 
                    {
                        senderSummaries[endPoint] = senderData;
                    }

                }
            }

            foreach (IPEndPoint endPoint in senderSummaries.Keys)
            {
                DataRow row = senderTable.NewRow();
                SenderData senderData = senderSummaries[endPoint];

                row["IP Addr"] = senderData.Source.ToString();
                row["CName"] = senderData.CName;

                row["Packets Sent"] = senderData.PacketsSent;
                row["Bytes Sent"] = senderData.BytesSent;

                double kb = senderData.DataRate;
                row["Data rate"] = kb.ToString("N") + " Kb/sec";
                row["Packet rate"] = senderData.PacketRate.ToString("N") + " pk/sec";

                if (senderData.LastSendUpdate.Equals(DateTime.MinValue)) {
                    row["Last update*"] = "No sender data received.";
                }
                else {
                    row["Last update*"] = senderData.LastSendUpdate.ToString();
                }

                senderTable.Rows.Add(row);
            }
            return senderTable;
        }

        private VenueState CurrentVenueState()
        {
            if (DropDownList1.SelectedItem == null)
                return null;

            String venueName = DropDownList1.SelectedItem.Text;

            lock (Global.venueStateMap)
            {
                if (Global.venueStateMap.ContainsKey(venueName))
                    return Global.venueStateMap[venueName];
                else return null;
            }
        }

        private void BuildTables()
        {

            Label1.Text = "Last updated: " + System.DateTime.Now;

            if (DropDownList1.SelectedIndex < 0)
            {
                HyperLink1.Text = "";
                return;
            }

            bool useAdvanedView = CheckBox1.Checked;
            
            VenueState venueState = CurrentVenueState();
            if (venueState == null)
            {
                HyperLink1.Text = "";
                return;
            }

            if (Global.LoggingEnabled) {
                if (venueState.LogFileName != null) {
                    HyperLink1.NavigateUrl = HttpRuntime.AppDomainAppVirtualPath + venueState.LogFileName;
                    HyperLink1.Text = venueState.LogFileName;
                    HyperLink1.Visible = true;
                    Label2.Visible = true;
                }
                else {
                    HyperLink1.Visible = false;
                    Label2.Visible = false;
                }
                CheckBox2.Checked = venueState.UseLogging;
            }
            else {
                CheckBox2.Visible = false;
                HyperLink1.Visible = false;
                Label2.Visible = false;
            }

            DataTable senderTable;
            DataTable receiverTable;
            DataTable lossTable;

            if (CheckBox1.Checked)
            {
                senderTable = BuildAdvancedSenderTable(venueState);
                BuildAdvancedReceiverTables(venueState, out receiverTable, out lossTable);
            }
            else
            {
                IDictionary<IPEndPoint, SenderData> senderSummaries;
                senderTable = BuildBasicSenderTable(venueState, out senderSummaries);
                BuildBasicReceiverTables(venueState, out receiverTable, out lossTable, senderSummaries);
            }


            //////////////////////////////////////////////
            //////////////////////////////////////
            // views and binding
            DataView view = new DataView(senderTable);
            view.AllowEdit = false;
            view.AllowNew = false;
            view.AllowDelete = false;

            GridView1.DataSource = view;

            DataView view2 = new DataView(receiverTable);
            view2.AllowEdit = false;
            view2.AllowNew = false;
            view2.AllowDelete = false;

            GridView2.DataSource = view2;

            DataView view3 = new DataView(lossTable);
            view3.AllowEdit = false;
            view3.AllowNew = false;
            view3.AllowDelete = false;
            GridView3.DataSource = view3;

            this.DataBind();
        }

  
 
 
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildTables();
        }

        private void highlightCell(TableCell cell)
        {
            cell.BorderStyle = BorderStyle.Double;
            cell.BorderColor = Color.Tomato;
        }

        /// <summary>
        /// A row has been added to the throughput/receiver table; this is where we do any necessary 
        /// highlighting of row cells.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType != DataControlRowType.DataRow)
                return;
  
            TableCellCollection cells = e.Row.Cells;
        
            // throughput data starts in column #2
            for (int i = 2; i < cells.Count; i++)
            {
                if (cells[i].Text.Equals("no data"))
                {
                    cells[i].Text = "<i>" + cells[i].Text + "</i>";
                    highlightCell(cells[i]);

                }
            }
        }

        /// <summary>
        /// Perform highlighting for the loss rate table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

          //  GridView gridView = (GridView)sender;
           // DataTable table = (DataTable) gridView.DataSource;

            TableCellCollection cells = e.Row.Cells;
        
            //  data starts in column #2
            for (int i = 2; i < cells.Count; i++)
            {
                String text = cells[i].Text;
                if (text == null)
                    continue;

                // highlight cells with no data
                if (cells[i].Text.Equals("no data"))
                {
                    cells[i].Text = "<i>" + cells[i].Text + "</i>";
                    highlightCell(cells[i]);
                    continue;
                }

                // highlight cells with a loss rate above the threshold
                try
                {
                    String[] toks = text.Split(null); // whitespace split
                    double rate = double.Parse(toks[0]);
                    if (rate > LOSS_HIGHLIGHT_THRESHOLD)
                        highlightCell(cells[i]);
                }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// Advanced view checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            BuildTables();
        }

        private void Refresh()
        {
            if (DropDownList1.SelectedIndex < 0)
                BuildVenueList(null);
            else  BuildTables();
        }
        protected void RefreshButton_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        protected void RefreshButton2_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        internal class MyComparer : Comparer<IPEndPoint>
        {
            public override int Compare(IPEndPoint x, IPEndPoint y)
            {
                return x.ToString().CompareTo(y.ToString());
            }
        }

        private static readonly MyComparer myComparer = new MyComparer();

        private static ICollection<IPEndPoint> getSortedReceivers(VenueState venueState)
        {
            List<IPEndPoint> list = new List<IPEndPoint>(venueState.ReceiverData.Keys);
            list.Sort(myComparer);
            return list;
        }

        private static ICollection<uint> getSortedSenders(VenueState venueState)
        {
            List<uint> list = new List<uint>(venueState.SenderData.Keys);
            list.Sort();
            return list;
        }

        //Logging enable/disable
        protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = CheckBox2.Checked;

            VenueState vs = CurrentVenueState();
            if (vs != null)
            {               
                vs.UseLogging = isChecked;
            }
        }

    }
}
