using System;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using MSR.LST.Test;

namespace MSR.LST.Net.Rtp.Test
{
    [TCProp("Test creation and sending from the various types of RtpSenders")]
    public class RtpSenderTest : TestCase
    {
        byte[] data = new byte[6000];
        Random rnd = new Random();
        AutoResetEvent are = new AutoResetEvent(false);
        int timeout = 2000;
        int frameCount = 0;

        RtpSender rtpSender;

        override public void Run()
        {
            RtpEvents.RtpStreamAdded += new MSR.LST.Net.Rtp.RtpEvents.RtpStreamAddedEventHandler(RtpStreamAdded);
            RtpEvents.RtpStreamRemoved += new MSR.LST.Net.Rtp.RtpEvents.RtpStreamRemovedEventHandler(RtpStreamRemoved);

            RtpSession session = new RtpSession(RtpSession.DefaultEndPoint, 
                new RtpParticipant("RtpSenderCreation", "RtpSenderCreation"), true, true);

            // Plain old RtpSender
            rtpSender = session.CreateRtpSender("RtpSenderCreation", PayloadType.Test, null);
            Thread.Sleep(timeout);
            SendData();
            SendData();
            rtpSender.Dispose();

            // Constant FEC sender
            rtpSender = session.CreateRtpSenderFec("RtpSenderCreation", PayloadType.Test, null, 1, 1);
            SendData();
            SendData();
            rtpSender.Dispose();

            // Frame FEC sender
            rtpSender = session.CreateRtpSenderFec("RtpSenderCreation", PayloadType.Test, null, 0, 50);
            SendData();
            SendData();
            rtpSender.Dispose();

            // RtpSenders with the fec private extensions already set
            Hashtable priExns = new Hashtable();
            
            // Plain old RtpSender
            rtpSender = session.CreateRtpSender("RtpSenderCreation", PayloadType.Test, priExns);
            SendData();
            SendData();
            rtpSender.Dispose();

            // Constant FEC sender
            priExns[Rtcp.PEP_FEC] = "1:1";
            rtpSender = session.CreateRtpSender("RtpSenderCreation", PayloadType.Test, priExns);
            SendData();
            SendData();
            rtpSender.Dispose();
            
            // Frame FEC sender
            priExns[Rtcp.PEP_FEC] = "0:50";
            rtpSender = session.CreateRtpSender("RtpSenderCreation", PayloadType.Test, priExns);
            SendData();
            SendData();
            rtpSender.Dispose();

            session.Dispose();
        }

        private void SendData()
        {
            frameCount++;

            rtpSender.Send(RandomData());
            
//            Console.WriteLine("Press any key to continue");
//            Console.ReadLine();

            if(!are.WaitOne(timeout, false))
            {
                Trace.WriteLine("Timeout waiting for frame - " + frameCount);
            }
        }

        private void FrameReceived(object sender, RtpStream.FrameReceivedEventArgs ea)
        {
            BufferChunk frame = ea.Frame;

            if(frame.Length != data.Length)
            {
                throw new TestCaseException(string.Format("Lengths don't match! lengthSent: {0}, lengthRecv'd: {1}",
                    data.Length, frame.Length));
            }

            for(int i = 0; i < frame.Length; i++)
            {
                if(frame[i] != data[i])
                {
                    throw new TestCaseException(string.Format("Bytes don't match! Index:{0}, byteSent: {1} + byteRecv'd: {2}",
                        i.ToString(), data[i].ToString(), frame[i].ToString()));
                }
            }

            are.Set();
        }

        private void RtpStreamAdded(object sender, RtpEvents.RtpStreamEventArgs ea)
        {
            ea.RtpStream.FrameReceived += new MSR.LST.Net.Rtp.RtpStream.FrameReceivedEventHandler(FrameReceived);
        }

        private void RtpStreamRemoved(object sender, RtpEvents.RtpStreamEventArgs ea)
        {
            ea.RtpStream.FrameReceived -= new MSR.LST.Net.Rtp.RtpStream.FrameReceivedEventHandler(FrameReceived);
        }

        private byte[] RandomData()
        {
            rnd.NextBytes(data);
            return data;
        }
    }

    [TCProp("Test performance counter instance names")]
    public class PCNamesTest : TestCase
    {
        override public void Run()
        {
            // 61 characters should be trimmed to 60 + _NNN for duplicates
            string name = "1234567890123456789012345678901234567890123456789012345678901";

            RtpSessionPC[] pcs = new RtpSessionPC[10];

            // Test trimming
            RtpSessionPC pc = new RtpSessionPC(name);
            if(pc.Name.Length != 60){throw new TestCaseException("Unexpected pc length");}
            pcs[0] = pc;

            // Create duplicates
            for(int i = 1; i < pcs.Length; i++)
            {
                pcs[i] = new RtpSessionPC(name);
            }

            // Validate duplicates
            for(int i = 1; i < pcs.Length; i++)
            {
                string iName = ((RtpSessionPC)pcs[i]).Name;

                if(iName.Length != 62){throw new TestCaseException("Unexpected pc length");}
                if(!iName.EndsWith(i.ToString())){throw new TestCaseException("Unexpected pc name");}
            }

            // Dispose all
            for(int i = 0; i < pcs.Length; i++)
            {
                ((RtpSessionPC)pcs[i]).Dispose();
            }
        }
    }

    [TCProp("Test Sdes class")]
    public class SdesTest : TestCase
    {
        override public void Run()
        {
            SdesData sdes = new SdesData("cName", "name");
            sdes.Email = "email";
            sdes.Location = "location";
            sdes.Note = "note";
            sdes.Phone = "phone";
            sdes.SetTool(true);
            sdes.SetPrivateExtension("prefix", "data");

            // Validate the properties we set
            if(sdes.CName != "cName"){throw new TestCaseException("Unexpected cName");}
            if(sdes.Name != "name"){throw new TestCaseException("Unexpected name");}
            if(sdes.Email != "email"){throw new TestCaseException("Unexpected email");}
            if(sdes.Location != "location"){throw new TestCaseException("Unexpected location");}
            if(sdes.Note != "note"){throw new TestCaseException("Unexpected note");}
            if(sdes.Phone != "phone"){throw new TestCaseException("Unexpected phone");}
            if(sdes.GetPrivateExtension("prefix") != "data"){throw new TestCaseException("Unexpected private extension");}

            // Serialize / Deserialize the data
            BufferChunk bc = new BufferChunk(Rtp.MAX_PACKET_SIZE);
            sdes.WriteDataToBuffer(bc);
            SdesData sdes2 = new SdesData(bc);

            if(sdes2.CName != "cName"){throw new TestCaseException("Unexpected cName");}
            if(sdes2.Name != "name"){throw new TestCaseException("Unexpected name");}
            if(sdes2.Email != "email"){throw new TestCaseException("Unexpected email");}
            if(sdes2.Location != "location"){throw new TestCaseException("Unexpected location");}
            if(sdes2.Note != "note"){throw new TestCaseException("Unexpected note");}
            if(sdes2.Phone != "phone"){throw new TestCaseException("Unexpected phone");}
            if(sdes2.GetPrivateExtension("prefix") != "data"){throw new TestCaseException("Unexpected private extension");}

            // Update the data
            sdes2.Name = "name2";
            if(!sdes.UpdateData(sdes2)){throw new TestCaseException("Update expected");}
            
            sdes2.SetPrivateExtension("prefix2", "data2");
            if(!sdes.UpdateData(sdes2)){throw new TestCaseException("Update expected");}

            Console.WriteLine(sdes.ToString());
        }
    }


    [TCProp("Test new BufferChunk operators for short & int")]
    public class BufferChunkOperatorOverloads : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(7);
            short s = 25000;
            int i = 545434;
            byte b = 56;

            bc += s;
            bc += i;
            bc += b;

            if (bc.NextInt16() != s)
                throw new TestCaseException("Short failed for " + s);
            if (bc.NextInt32() != i)
                throw new TestCaseException("Int failed for " + i);
            if (bc.NextByte() != b)
                throw new TestCaseException("Byte failed for " + b);

            bc.Reset();

            s = short.MaxValue;
            i = int.MaxValue;
            b = byte.MaxValue;

            bc += s;
            bc += i;
            bc += b;

            if (bc.NextInt16() != s)
                throw new TestCaseException("Short failed for " + s);
            if (bc.NextInt32() != i)
                throw new TestCaseException("Int failed for " + i);
            if (bc.NextByte() != b)
                throw new TestCaseException("Byte failed for " + b);

            bc.Reset();

            s = short.MinValue;
            i = int.MinValue;
            b = byte.MinValue;

            bc += s;
            bc += i;
            bc += b;

            if (bc.NextInt16() != s)
                throw new TestCaseException("Short failed for " + s);
            if (bc.NextInt32() != i)
                throw new TestCaseException("Int failed for " + i);
            if (bc.NextByte() != b)
                throw new TestCaseException("Byte failed for " + b);

        }
    }
}
