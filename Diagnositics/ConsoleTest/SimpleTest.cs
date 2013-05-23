using System;
using System.Data;
using System.Configuration;

using System.Net;
using System.Threading;

using MSR.LST.Net.Rtp;
using MSR.LST.Net;

namespace DiagnosisPage
{
    public class SimpleTest
    {

        // XXX config constants
        //private string addr = "233.31.135.60";
        private string addr = "233.0.73.18";

        private int rtpPort = 5004;

        private readonly RtpSession rtpSession;

        public static void Main()
        {


            SimpleTest st = new SimpleTest();
            st.go();
        }

        public SimpleTest()
        {
            // Create participant
            RtpParticipant rtpParticipant = null;
            //rtpParticipant.SetTool(true);

            // Create session with Participant and Rtp data
            rtpSession = new RtpSession(new IPEndPoint(IPAddress.Parse(addr), rtpPort),
                rtpParticipant, false, true);
        }

        private void go() {
            Console.Out.WriteLine("Starting RTCP event handlers");

            RtpEvents.ReceiverReport += new RtpEvents.ReceiverReportEventHandler(RtpReceiverReport);
            RtpEvents.SenderReport += new RtpEvents.SenderReportEventHandler(RtpSenderReport);
            RtpEvents.RtpParticipantAdded += new RtpEvents.RtpParticipantAddedEventHandler(RtpEvents_RtpParticipantAdded);
            RtpEvents.RtpParticipantDataChanged += new RtpEvents.RtpParticipantDataChangedEventHandler(RtpEvents_RtpParticipantDataChanged);
            RtpEvents.RtpParticipantRemoved += new RtpEvents.RtpParticipantRemovedEventHandler(RtpEvents_RtpParticipantRemoved);
            RtpEvents.RtpStreamAdded += new RtpEvents.RtpStreamAddedEventHandler(RtpEvents_RtpStreamAdded);
            RtpEvents.RtpStreamRemoved += new RtpEvents.RtpStreamRemovedEventHandler(RtpEvents_RtpStreamRemoved);
            RtpEvents.RtpStreamTimeout += new RtpEvents.RtpStreamTimeoutEventHandler(RtpEvents_RtpStreamTimeout);
            RtpEvents.RtpParticipantTimeout += new RtpEvents.RtpParticipantTimeoutEventHandler(RtpEvents_RtpParticipantTimeout);
            // RtpEvents.DuplicateCNameDetected += new RtpEvents.DuplicateCNameDetectedEventHandler(DuplicateCNameDetected);

    
            while (true)
            {
                Thread.Sleep(10000);
            }

        }

        void RtpEvents_RtpParticipantTimeout(object sender, RtpEvents.RtpParticipantEventArgs ea)
        {
            Console.Out.WriteLine("Participant timeout\n");
        }

        void RtpEvents_RtpStreamTimeout(object sender, RtpEvents.RtpStreamEventArgs ea)
        {
            Console.Out.WriteLine("Stream timeout\n");
        }

        void RtpEvents_RtpStreamRemoved(object sender, RtpEvents.RtpStreamEventArgs ea)
        {
            Console.Out.WriteLine("Stream removed\n");
        }

        void RtpEvents_RtpStreamAdded(object sender, RtpEvents.RtpStreamEventArgs ea)
        {
            Console.Out.WriteLine("Stream added\n");
        }

        void RtpEvents_RtpParticipantRemoved(object sender, RtpEvents.RtpParticipantEventArgs ea)
        {
            Console.Out.WriteLine("Participant removed\n");
        }

        void RtpEvents_RtpParticipantDataChanged(object sender, RtpEvents.RtpParticipantEventArgs ea)
        {
            Console.Out.WriteLine("Participant data changed: {0}", ea.RtpParticipant.SSRC);
            //Console.Out.WriteLine("Participant changed: " + ea.RtpParticipant.ToString());
            Console.Out.WriteLine("");
        }

        void RtpEvents_RtpParticipantAdded(object sender, RtpEvents.RtpParticipantEventArgs ea)
        {
            Console.Out.WriteLine("Participant added {0}" ,ea.RtpParticipant.SSRC);
            Console.Out.WriteLine("");
        }


        private void RtpSenderReport(object sender, RtpEvents.SenderReportEventArgs ea)
        {
            Console.Out.WriteLine("Got an SR message from {0}", ea.ssrc);
            //Console.Out.WriteLine("Message source CNAME: " + rtpSession.Partcipant(ea.ssrc));
          //  Console.Out.WriteLine("");
        }

        private void RtpReceiverReport(object sender, RtpEvents.ReceiverReportEventArgs ea)
        {
            Console.Out.WriteLine("Got an RR message from {0} regarding {1} ", ea.rrSSRC, ea.dataSourceSSRC);
            //Console.Out.WriteLine("Message source CNAME: " + rtpSession.Partcipant(ea.rrSSRC));
           // Console.Out.WriteLine("Data source cNAME: " + rtpSession.Partcipant(ea.dataSourceSSRC));
            //Console.Out.WriteLine("");
        }
    }
}
