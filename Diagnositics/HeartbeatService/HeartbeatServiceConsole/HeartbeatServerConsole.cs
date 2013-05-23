using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;

namespace MSR.LST.Net.Heartbeat
{

    public class HeartbeatServerConsole
    {
        // XXX Make these configurable!
        public static readonly string ADDRESS = "233.0.73.19";
        public static readonly int PORT = 2112;
        private static readonly int PERIOD = 1000; // ms

        // magic cookie that indicates a legitimate heartbeat message
        public static readonly uint COOKIE = 0xDECAFBAD;

        private readonly UdpSender udpSender;

        public static void Main(string[] args)
        {
            HeartbeatServerConsole mhs = new HeartbeatServerConsole(ADDRESS, PORT);
            Thread thread = new Thread(mhs.Run);
            thread.Start();
        }

        HeartbeatServerConsole(String addr, int port)
        {
            IPAddress ipaddr = IPAddress.Parse(addr);
            IPEndPoint iep = new IPEndPoint(ipaddr, port);
            udpSender = new UdpSender(iep, 32);
        }

        private void Run()
        {
            while (true)
            {
                Console.Out.WriteLine("Sending to address: " + ADDRESS);

                byte[] buffer = new byte[sizeof(uint)];
                BufferChunk packetBuffer = new BufferChunk(buffer);
                packetBuffer.SetUInt32(0, COOKIE);

                udpSender.Send(packetBuffer);

                Thread.Sleep(PERIOD);
            }
        }
    }
}
