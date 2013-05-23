using System;
using System.Collections.Generic;
using System.Text;
using MSR.LST.Net;
using System.Net;
using MSR.LST.Net.Rtp;
using MSR.LST;
using System.Security.Cryptography;
using System.IO;
using System.Threading;

namespace EncodeTest
{
    class Program
    {
        private static readonly IPEndPoint ip = new IPEndPoint(IPAddress.Parse("224.0.0.4"), 7120);

        static void Main(string[] args)
        {
            RtpParticipant part = new RtpParticipant("foo@bar.com", "SENDER");

            RtpSession session = new RtpSession(ip, part, true, true);
            session.PacketTransform = new EncryptionTransform("You are a big freak!");
            //session.PacketTransform = new XorTransform();


            RtpSender sender = session.CreateRtpSender("My sender", PayloadType.Test, null);

            Stream fs = File.OpenRead("data.txt");
            int length = (int)fs.Length;
            Console.Out.WriteLine("Opening file of length: " + length);

            byte[] buffer = new byte[length];

            int bytesRead = 0;
            while (bytesRead < length)
            {
                bytesRead += fs.Read(buffer, bytesRead, Math.Min(16384, (length - bytesRead)));
            }

            for (int i = 0; i < 5; i++)
            {
                Console.Out.WriteLine("Sending buffer to address: " + ip);

                sender.Send(buffer);

                Thread.Sleep(1000);
            }

        }
    }
}
