using System;


namespace MSR.LST.Net.Rtp
{
    /// <summary>
    /// This class contains statics, constants and enums used throughout the Rtcp code
    /// </summary>
    public class Rtp
    {
        /// <summary>
        ///  Current version of RTP / RTCP - 2
        /// </summary>
        public const byte VERSION = 2;
    
        /// <summary>
        /// Size, in bytes, of an SSRC
        /// </summary>
        public const int SSRC_SIZE = 4;
    
        /// <summary>
        /// Maximum size, in bytes, of an Rtp/RtcpPacket - 1452
        /// 
        /// RFC3550, Colin's book, our network staff and the netmon properties of the local netcard
        /// suggest a standard MTU (Maximum Transmission Unit) of 1500 bytes for a typical router.
        /// 
        /// They also suggest 28 bytes for the overhead of a standard IPv4 implementation of the IP
        /// (20) and UDP (8) headers.  1500 - 28 = 1472.
        /// 
        /// There is a 14 byte overhead for the Ethernet layer, but apparently that is safe to
        /// ignore in this calculation.
        /// 
        /// In order to be IPv6 ready, we are adding an additional 20 byte overhead (or 40 for the 
        /// IP layer).  1500 - 48 = 1452.
        /// 
        /// Because Rtcp requires that all packets end on a 32 bit (4 byte) boundary, we adjust 
        /// to the closest such boundary, which is still 1452.
        /// 
        /// TODO - this should be calculated at runtime by occasionally sending a packet with the
        /// DF (do not fragment) flag set.  This will cause a router to return a packet indicating
        /// the maximum packet size it can handle without fragmenting the data.  For now it is a
        /// constant. JVE
        /// </summary>
        /// 
        private static int mtu = Default_Mtu;
        /// <summary>
        /// Typically 1500;  Possibly 9000 for jumbo frames.  May be set in app.config.
        /// </summary>
        public static int MTU {
            get { return mtu; }
            set { 
                mtu = value; 
                maxPacketSize = mtu - IPv6_Header - UDP_Header;
            }
        }

        private static int maxPacketSize = Default_Mtu - IPv6_Header - UDP_Header;
        public static int MAX_PACKET_SIZE {
            get { return maxPacketSize; }
        }

        private const int IPv6_Header = 40;
        private const int UDP_Header = 8;
        private const int Default_Mtu = 1500;

    }

    public class Rtcp
    {
        /// <summary>
        /// SDES private extension prefix (PEP) - Source
        /// 
        /// This was added to distinguish between streams from participants vs senders
        /// </summary>
        public const string PEP_SOURCE = "Src";

        /// <summary>
        /// SDES private extension data (PED) - Participant
        /// </summary>
        public const string PED_PARTICIPANT = "P";

        /// <summary>
        /// SDES private extension data (PED) - Stream
        /// </summary>
        public const string PED_STREAM = "S";

        /// <summary>
        /// SDES private extension prefix (PEP) - PayloadType
        /// 
        /// This was added in order to be able to create an RtpStream from Rtcp data
        /// </summary>
        public const string PEP_PAYLOADTYPE = "PT";

        /// <summary>
        /// SDES private extension prefix (PEP) - FEC
        /// 
        /// This was added in order to be able to know the FEC characteristics of a stream
        /// </summary>
        public const string PEP_FEC = "FEC";

        /// <summary>
        /// SDES private extension prefix (PEP) - REC
        /// 
        /// This was added in order to indicate the stream is reliable (Reliable Error Correction)
        /// </summary>
        public const string PEP_REC = "REC";

        /// <summary>
        /// SDES private extension prefix (PEP) - DBP
        /// 
        /// This was added in order to be able to know the bandwidth throttle at the receiving side, for playback purposes.
        /// </summary>
        public const string PEP_DBP = "DBP";

        /// <summary>
        /// This value identifies the "venue name" subtype, which is included in RTCP APP packets
        /// </summary>
        public const int VENUE_APP_PACKET_SUBTYPE = 22;

        public const String APP_PACKET_NAME = "cxp!";

        #region Enumerations
    
        /// <summary>
        /// Rtcp Packet Types, per RFC 3550 spec
        /// </summary>
        public enum PacketType : byte
        {
            SR = 200,   // Sender Report        stats on the Rtp packets have I sent
            RR = 201,   // Receiver Report      stats on the Rtp packets I have received
            SDES = 202, // Session Description   associated properties to an RtpStream
            BYE = 203,  // Bye                  this stream has gone away
            APP = 204   // App                  application defined control packet
        }

    
        /// <summary>
        /// Rtcp SDES Types, per RFC 3550 spec
        /// </summary>
        public enum SDESType : byte
        {
            END = 0,
            CNAME, NAME, EMAIL, PHONE, LOC, TOOL, NOTE, PRIV
        }


        /// <summary>
        /// When to send an Rtcp packet
        /// 
        /// Now or at the next scheduled interval
        /// </summary>
        public enum RtcpInterval
        {
            Now,
            Next
        }

    
        #endregion Enumerations
    }
}
