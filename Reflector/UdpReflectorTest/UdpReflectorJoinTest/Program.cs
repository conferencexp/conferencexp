using System;
using System.Collections.Generic;
using System.Text;
using MSR.LST.Net;
using System.Net;
using MSR.LST;
using System.Threading;

namespace UdpReflectorJoinTest
{
    class TestProgram
    {
        private static readonly String multicastAddr = "233.0.73.18";
        private static readonly int multicastPort = 5004;

        private static readonly String reflectorAddr = "128.208.1.129"; // teseney
        private static readonly int reflectorPort = 7004;

        private UdpListener listener ;
        private UdpSender sender;

        static void Main(string[] args)
        {

            TestProgram tp;

            if (args.Length > 0)
            {
                Console.Out.WriteLine("Multicast mode");
                tp = new TestProgram(true);
            }
            else
            {
                Console.Out.WriteLine("Unicast reflector mode");
                tp = new TestProgram(false); // use reflector
            }
        }

        public TestProgram(bool useMulticast)
        {
            IPEndPoint multicastEP = new IPEndPoint(IPAddress.Parse(multicastAddr), multicastPort);
            IPEndPoint reflectorEP = new IPEndPoint(IPAddress.Parse(reflectorAddr), reflectorPort);

            if (useMulticast)
            {
                listener = new UdpListener(multicastEP, 0);
                sender = new UdpSender(multicastEP, 64);
            }
            else
            {
                // reflector
                //listener = new UdpListener(multicastEP, reflectorEP, 0);
                sender = new UdpSender(reflectorEP, 64);
            }

            Thread thread1 = new Thread(SendSomeStuff);
            thread1.Start();

            Thread thread2 = new Thread(ReceiveSomeStuff);
            thread2.Start();
        }

        private void SendSomeStuff()
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            Random random = new Random();
            int rnd = random.Next();

            while (true)
            {
                try
                {
                    byte[] buffer = utf8.GetBytes("My random number is: " + rnd);
                    sender.Send(new BufferChunk(buffer));

                    Thread.Sleep(3000);
                }
                catch { }
            }
        }

        private void ReceiveSomeStuff()
        {
            UTF8Encoding utf8 = new UTF8Encoding();

            while (true)
            {
                try
                {
                    BufferChunk chunk = new BufferChunk(1500);
                    EndPoint source;

                    listener.ReceiveFrom(chunk, out source);

                    Console.Out.WriteLine("Received message from: " + source.ToString());

                    String message = utf8.GetString(chunk.Buffer, 0, chunk.Length);

                    Console.Out.WriteLine("===>  " + message);


                }
                catch { }
            }
        }


    }
}
