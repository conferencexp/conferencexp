using System;
using System.Collections.Generic;
using System.Text;

namespace MSR.LST.Net.Rtp
{
    /// <summary>
    /// Apply a "self-xor" to packet data; this is for testing purposes.
    /// </summary>
    public class XorTransform : PacketTransform
    {
        #region DataTransform Members
        public void Encode(RtpPacket packet)
        {

            XorPacket(packet);
        }

        private static void XorPacket(RtpPacket packet)
        {
            BufferChunk chunk = packet.Payload;
            byte[] data = chunk.Buffer;
            for (int i = chunk.Index; i <= chunk.Length; i++)
            {
                data[i] ^= 0x0b;
            }
        }

        public void Decode (RtpPacket packet)
        {

            XorPacket(packet);
        }

        #endregion
    }
}
