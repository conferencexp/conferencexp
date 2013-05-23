using System;
using System.Collections.Generic;
using System.Text;
using MSR.LST.Net;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.ComponentModel;

namespace MSR.LST.ConferenceXP
{
    class HeartbeatMonitor : BackgroundWorker
    {

        private static readonly int TIMEOUT = 4000;
        private static readonly int DEFAULT_PORT = 2112;

        private bool reflectorEnabled = false;

        private readonly Socket reflectorSocket;
        private IPEndPoint ReflectorAddress = null;
        private readonly UdpListener multicastListener;

        private DateTime lastSuccessfulReading = DateTime.Now;

        public HeartbeatMonitor()
        {
            // XXX make this config parameters
            IPAddress addr = IPAddress.Parse("233.0.73.19");
            IPEndPoint ep = new IPEndPoint(addr,DEFAULT_PORT);

            try {
                multicastListener = new UdpListener(ep, TIMEOUT);
            }
            catch (SocketException) { 
                //Exception here probably means there is no network.  
                //In this case we just leave the heartbeat monitor off.
                return;
            }

            reflectorSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            reflectorSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout,TIMEOUT);   

            this.WorkerSupportsCancellation = false;
            this.WorkerReportsProgress = true;
            this.DoWork += new DoWorkEventHandler(HeartbeatMonitor_DoWork);

            this.RunWorkerAsync();
        }

        public bool ReflectorEnabled {
            get { return reflectorEnabled; }
            set { reflectorEnabled = value; }
        }

        public void SetReflectorAddress(IPEndPoint reflectorAddr)
        {
            this.ReflectorAddress = reflectorAddr;
        }

        private void HeartbeatMonitor_DoWork(object sender, DoWorkEventArgs e)
        {         
            while (true)
            {
                try
                {
                    if (!reflectorEnabled)
                        CheckForMulticast();
                    else CheckForReflector();

                    TimeSpan duration = DateTime.Now.Subtract(this.lastSuccessfulReading);
                    if (duration.TotalMilliseconds > TIMEOUT)
                        this.ReportProgress(0);
                }
                catch (Exception)
                {
                    this.ReportProgress(0);
                }
            }
        }

        private void CheckForReflector()
        {

            // send a ping
            UdpReflectorMessage ping = new UdpReflectorMessage(UdpReflectorMessageType.PING);
            BufferChunk buffer = ping.ToBufferChunk();
            reflectorSocket.SendTo(buffer.Buffer, buffer.Index, buffer.Length, SocketFlags.None, ReflectorAddress);

            // wait for response
            byte [] byteBuffer = new byte[512];

            int count =  reflectorSocket.Receive(byteBuffer);

            UdpReflectorMessage pingReply = new UdpReflectorMessage(byteBuffer,count);
            if (pingReply.Type == UdpReflectorMessageType.PING_REPLY)
            {
                this.ReportProgress(100);
                this.lastSuccessfulReading = DateTime.Now;
            }

            // wait a bit...
            Thread.Sleep(1500);
        }

        private void CheckForMulticast()
        {
            BufferChunk buffer = new BufferChunk(1000);
            multicastListener.Receive(buffer);
            if (buffer.Length >= 4)
            {
                uint val = buffer.GetUInt32(0);
                if (val == 0xdecafbad)
                {
                    this.ReportProgress(100);
                    this.lastSuccessfulReading = DateTime.Now;
                }
            }
        }
    }
}