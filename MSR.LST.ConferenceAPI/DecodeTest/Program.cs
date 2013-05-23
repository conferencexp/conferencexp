using System;
using System.Collections.Generic;
using System.Text;

using MSR.LST.Net;
using System.Net;
using MSR.LST.Net.Rtp;
using MSR.LST;
using System.Threading;
using System.Security.Cryptography;

namespace DecodeTest
{
    class Program
    {
        private static readonly IPEndPoint ip = new IPEndPoint(IPAddress.Parse("224.0.0.4"), 7120);

        private static readonly byte [] oracleHash = {0x5d,0x2b,0xc1,0x6e,0x83,0xd0,0xa4,0x59,0xda,0x5a,0x89,
            0xa0,0xcf,0x3a,0xe1,0x2c}; 

       // private static readonly UTF8Encoding utf8 = new UTF8Encoding();

        private static RtpSession session = null;
        static void Main(string[] args)
        {
            RtpEvents.RtpStreamAdded += new RtpEvents.RtpStreamAddedEventHandler(OnNewRtpStream);


            RtpParticipant part = new RtpParticipant("bam@zag.com", "Receiver");

            session = new RtpSession(ip, part, true, true);
            //session.PacketTransform = new XorTransform();
            session.PacketTransform = new EncryptionTransform("You are a big freak!");

            // make sure this thing doesn't terminate
            while (true)
            {
                Thread.Sleep(10000);
            }
        }

        static void OnNewRtpStream(object sender, RtpEvents.RtpStreamEventArgs ea)
        {
            Console.Out.WriteLine("New RTP stream");

            if (session != sender)
                return;

            RtpStream stream = ea.RtpStream;
            stream.FrameReceived += new RtpStream.FrameReceivedEventHandler(stream_FrameReceived);
        }

        static void stream_FrameReceived(object sender, RtpStream.FrameReceivedEventArgs ea)
        {
            Console.Out.WriteLine("Frame received of length: " + ea.Frame.Length);

            byte[] hash = MD5.Create().ComputeHash(ea.Frame.Buffer, ea.Frame.Index, ea.Frame.Length);

            // compare arrays:
            for (int i = 0; i < oracleHash.Length; i++)
            {
                if (oracleHash[i] != hash[i])
                {
                    Console.Out.WriteLine("Hash values not equal!");
                    return;
                }
            }
            Console.Out.WriteLine("Hash value is good!");
        }
                  
    }
}
