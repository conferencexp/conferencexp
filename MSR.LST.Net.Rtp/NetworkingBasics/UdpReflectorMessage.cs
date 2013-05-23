using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using MSR.LST;
using System.Diagnostics;

namespace MSR.LST.Net
{
    public enum UdpReflectorMessageType { JOIN, LEAVE, PING, PING_REPLY };

    public class InvalidUdpReflectorMessage : Exception
    {
    }

    public class UdpReflectorMessage
    {

        private static readonly String headerLine = "VCP 1.0";
       
        private static readonly UTF8Encoding utf8 = new UTF8Encoding();

        private readonly IPEndPoint multicastEP;

        private readonly UdpReflectorMessageType type;

        public UdpReflectorMessageType Type
        {
            get { return type; }
        } 


      
        public IPEndPoint MulticastEP
        {
            get { return multicastEP; }
        }


        // ping messages don't need a valid ip address...
        public UdpReflectorMessage(UdpReflectorMessageType type)
        {
            this.type = type;
            this.multicastEP = new IPEndPoint(IPAddress.Loopback, 1);
        }
        // outbound reflector messages
        public UdpReflectorMessage(UdpReflectorMessageType type, IPEndPoint multicastEP)
        {
            this.type = type;
            this.multicastEP = multicastEP;
        }

        // inbound data (from the network)
        // throws InvalidUdpReflectorMessage if this is not a valid control message
        public UdpReflectorMessage(byte[] buffer,int count)
        {
            if (count > 50)
                throw new InvalidUdpReflectorMessage();
            else
            {
                String str = utf8.GetString(buffer,0,count);
                String[] lines = str.Split(new char[] { '\n' }, 2);
                if (lines.Length < 2)
                    throw new InvalidUdpReflectorMessage();
                if (!lines[0].Equals(headerLine))
                    throw new InvalidUdpReflectorMessage();
                
                String [] toks = lines[1].Split(new char [] { ':' },3);
                if (toks.Length < 3)
                    throw new InvalidUdpReflectorMessage();


                if (toks[0].Trim().Equals("JOIN", StringComparison.InvariantCultureIgnoreCase))
                    type = UdpReflectorMessageType.JOIN;
                else if (toks[0].Equals("LEAVE", StringComparison.InvariantCultureIgnoreCase))
                    type = UdpReflectorMessageType.LEAVE;
                else if (toks[0].Equals("PING", StringComparison.InvariantCultureIgnoreCase))
                    type = UdpReflectorMessageType.PING;
                else if (toks[0].Equals("PING_REPLY", StringComparison.InvariantCultureIgnoreCase))
                    type = UdpReflectorMessageType.PING_REPLY;
                else throw new InvalidUdpReflectorMessage();
                
                try 
                {
                    IPAddress addr = IPAddress.Parse(toks[1].Trim());
                    int port = int.Parse(toks[2].Trim());

                    this.multicastEP = new IPEndPoint(addr,port);
                }
                catch (Exception)
                {
                    throw new InvalidUdpReflectorMessage();
                }
            }
        }
        public BufferChunk ToBufferChunk()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(headerLine + "\n");

            if (type == UdpReflectorMessageType.JOIN)
                builder.Append("JOIN: ");
            else if (type == UdpReflectorMessageType.LEAVE)
                builder.Append("LEAVE: ");
            else if (type == UdpReflectorMessageType.PING) 
                builder.Append("PING: ");
            else if (type == UdpReflectorMessageType.PING_REPLY)
                builder.Append("PING_REPLY: ");
            else
            {
                Debug.Assert(false);
            }

            builder.Append(multicastEP.Address.ToString()  +  ":" + multicastEP.Port + "\n");

            byte[] buffer = utf8.GetBytes(builder.ToString());
            return new BufferChunk(buffer);
        }
    }
}
