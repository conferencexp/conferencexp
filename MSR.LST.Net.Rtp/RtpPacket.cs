using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;


namespace MSR.LST.Net.Rtp
{
    #region Public Enumerations

    
    /// <summary>
    /// Enumeration for Rtp PayloadTypes using values per RFC 1890.  PayloadType was previously used to tightly couple the data stream to an
    /// exact data type, but this has been falling out of favor as payload types diverge and systems such as DirectShow and QuickTime carry
    /// the media type in much greater detail in band with the data.
    /// 
    /// We use the terms 'dynamicVideo' and 'dynamicAudio', specified at the end of the dynamic band of PayloadTypes and then include a
    /// DirectShow AM_MEDIA_TYPE structure in the Rtp header extension of the first packet in a frame.  See the packet format on the website
    /// for detailed information on how this is transmitted.
    /// </summary>
    /// <example>
    ///       PT         encoding      audio/video    clock rate    channels
    ///                  name          (A/V)          (Hz)          (audio)
    ///             _______________________________________________________________
    ///             0          PCMU          A              8000          1
    ///             1          1016          A              8000          1
    ///             2          G721          A              8000          1
    ///             3          GSM           A              8000          1
    ///             4          unassigned    A              8000          1
    ///             5          DVI4          A              8000          1
    ///             6          DVI4          A              16000         1
    ///             7          LPC           A              8000          1
    ///             8          PCMA          A              8000          1
    ///             9          G722          A              8000          1
    ///             10         L16           A              44100         2
    ///             11         L16           A              44100         1
    ///             12         unassigned    A
    ///             13         unassigned    A
    ///             14         MPA           A              90000        (see text)
    ///             15         G728          A              8000          1
    ///             16--23     unassigned    A
    ///             24         unassigned    V
    ///             25         CelB          V              90000
    ///             26         JPEG          V              90000
    ///             27         unassigned    V
    ///             28         nv            V              90000
    ///             29         unassigned    V
    ///             30         unassigned    V
    ///             31         H261          V              90000
    ///             32         MPV           V              90000
    ///             33         MP2T          AV             90000
    ///             34--71     unassigned    ?
    ///             72--76     reserved      N/A            N/A           N/A
    ///             77--95     unassigned    ?
    ///             96--127    dynamic       ?
    /// </example>
    public enum PayloadType : byte
    {
        PCMU = 0, PT1016, G721, GSM,
        DVI4 = 5,
        LPC = 7, PCMA, G722, L16,
        MPA = 14, G728,
        CelB = 25, JPEG,
        nv = 28,
        H261 = 31, MPV, MP2T,
        // 96-127 are intended for dynamic assignment
        Chat = 96,
        xApplication2,
        xApplication3,
        xApplication4,
        xApplication5,
        xApplication6,
        xApplication7,
        xApplication8,
        xApplication9,
        xApplication10,
        Venue1 = 106,
        Venue2,
        Venue3,
        Venue4,
        Venue5,
        Venue6,
        Venue7,
        Venue8,
        Venue9,
        GroupChat = 115,
        FileTransfer = 116,
        ManagedDirectX = 117,
        Whiteboard = 118,
        SharedBrowser = 119,
        RichTextChat = 120,
        RTDocument = 121,               // Serialization of an RTDocument object, network protocol TBD
        PipecleanerSignal = 122,        // Diagnostic signal used by the Pipecleaner applications to test connectivity between nodes
        Test = 123,                     // Used for test cases
        FEC = 124,                      // Identifies a packet as containing Forward Error Correction information
        dynamicPresentation = 125,      // Obsolete, being replaced by RTDocument -- lifetime TBD
        dynamicVideo = 126,             // A video signal.  The format of the video signal is embedded in the data stream itself
        dynamicAudio = 127              // An audio signal.  The format of the audio signal is embedded in the data stream itself
    }
    #endregion

    #region RtpPacketBase

    /// <summary>
    /// The standard Rtp packet
    /// 
    /// TODO - we may need to re-add support for CSRCs and HeaderExtensions, although header
    /// extensions could just as easily be implemented as payload headers
    /// </summary>
    public class RtpPacketBase
    {
        #region Statics
        
        internal const int RTP_HEADER_SIZE = SSRC_INDEX + SSRC_SIZE;
        internal const int MAX_CRYPTO_BLOCK_SIZE = 8;

        private const int VPXCC_SIZE = 1;
        private const int MPT_SIZE = 1;
        private const int SEQ_SIZE = 2;
        private const int TS_SIZE = 4;
        protected const int SSRC_SIZE = Rtp.SSRC_SIZE;

        private const int VPXCC_INDEX = 0;
        private const int MPT_INDEX = VPXCC_INDEX + VPXCC_SIZE;
        private const int SEQ_INDEX = MPT_INDEX + MPT_SIZE;
        protected const int TS_INDEX = SEQ_INDEX + SEQ_SIZE;
        protected const int SSRC_INDEX = TS_INDEX + TS_SIZE;


        /// <summary>
        ///  Cast operator for forming a BufferChunk from an RtpPacketBase.
        /// </summary>
        public static explicit operator BufferChunk(RtpPacketBase packet)
        {
            Debug.Assert( packet.buffer.Length == (packet.HeaderSize+packet.PayloadSize) );
            return packet.buffer;
        }


        /// <summary>
        ///  Cast operator for forming a BufferChunk from an RtpPacketBase.
        /// </summary>
        public static explicit operator RtpPacketBase(BufferChunk buffer)
        {
            return new RtpPacketBase(buffer);
        }
        
        
        #endregion Statics
        
        #region Members

        /// <summary>
        ///  Buffer to contain the raw data
        /// </summary>
        protected BufferChunk buffer;

        /// <summary>
        /// How much space to reserve for padding.  This is required for encryption.
        /// 
        /// XXX this default pesimistically reserves space for all packets,
        /// regardless of whether encryption is actually used...
        /// </summary>
        private int reservedPaddingBytes = MAX_CRYPTO_BLOCK_SIZE;

        public int ReservedPaddingBytes
        {
            get { return reservedPaddingBytes; }
            set { reservedPaddingBytes = value; }
        }

        #endregion Members

        #region Constructors

        /// <summary>
        /// Creates a max size packet
        /// </summary>
        internal RtpPacketBase() : this(Rtp.MAX_PACKET_SIZE) {}

        /// <summary>
        /// Creates a packet of the given size
        /// </summary>
        internal RtpPacketBase(int packetSize)
        {
            buffer = new BufferChunk(new byte[packetSize]);
            Reset();
        }

        /// <summary>
        /// Create a packet from an existing buffer
        /// </summary>
        /// <param name="buffer"></param>
        internal RtpPacketBase(BufferChunk buffer)
        {
            ValidateBuffer(buffer);

            this.buffer = buffer;
        }


        /// <summary>
        /// Create a packet from an existing packet
        /// </summary>
        /// <param name="packet"></param>
        internal RtpPacketBase(RtpPacketBase packet)
        {
            buffer = packet.buffer;
        }


        #endregion

        #region Internal
        
        /// <summary>
        /// Marker reserved for payload/protocol specific information.
        /// </summary>
        internal bool Marker 
        {
            get{return ((buffer[MPT_INDEX] & 128) == 128);}
            
            set
            {
                if(value)
                {
                    // Set it
                    buffer[MPT_INDEX] |= (byte)(128);
                }
                else
                {
                    // Clear the bit
                    buffer[MPT_INDEX] ^= (byte)(buffer[MPT_INDEX] & 128);
                }
            }
        }

        /// <summary>
        /// The type of data contained in the packet
        /// </summary>
        internal PayloadType PayloadType
        {
            get{return (PayloadType)(buffer[MPT_INDEX] & 127);}
            
            set
            {
                if ((int)value > 127)
                {
                    throw new ArgumentOutOfRangeException(Strings.PayloadTypeIsASevenBitStructure);
                }

                // Preserve most significant bit
                buffer[MPT_INDEX] = (byte)(buffer[MPT_INDEX] & 128);
                buffer[MPT_INDEX] += (byte)value;
            }
        }

        /// <summary>
        /// Sequence number of the packet, used to keep track of the order packets were sent in
        /// 
        /// public because it is used by NetworkDumper
        ///</summary>
        public ushort Sequence
        {
            get{return buffer.GetUInt16(SEQ_INDEX);}
            set{buffer.SetUInt16(SEQ_INDEX, value);}
        }

        /// <summary>
        /// According to the spec - timestamp is the sampling instant for the first octet of the
        /// media data in a packet, and is used to schedule playout of the media data.
        /// 
        /// In our implementation, it is an incrementing counter used to group packets into a frame
        /// </summary>
        internal virtual uint TimeStamp
        {
            get{return buffer.GetUInt32(TS_INDEX);}
            set{buffer.SetUInt32(TS_INDEX, value);}
        }

        /// <summary>
        /// Synchronization source used to identify streams within a session
        /// 
        /// public because it is used by NetworkDumper
        /// </summary>
        public uint SSRC
        {
            get{return buffer.GetUInt32(SSRC_INDEX);}
            set{buffer.SetUInt32(SSRC_INDEX, value);}
        }

        
        /// <summary>
        /// Payload data of the RtpPacket
        /// </summary>
        internal BufferChunk Payload
        {
            set
            {
                // Make sure they haven't tried to add more data than we can handle
                if(value.Length > MaxPayloadSize)
                {
                    throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, 
                        Strings.ValueMaximumPayload, value.Length, MaxPayloadSize));
                }

                // Reset Buffer to just after the header because packets are re-used and so that
                // operator+ works properly when copying the payload
                buffer.Reset(0, HeaderSize);
                buffer += value;
            }
            get
            {
                return buffer.Peek(HeaderSize, PayloadSize);
            }
        }

        /// <summary>
        /// Set the payload; this version allows the payload to occupy the space
        /// reserved for packet padding.  This is used by encryption protocols.
        /// </summary>
        /// <param name="chunk"></param>
        internal void SetPaddedPayload(BufferChunk chunk)
        {
            if (chunk.Length > (MaxPayloadSize + ReservedPaddingBytes))
            {
                throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture,
                        Strings.ValueMaximumPayload, chunk.Length, MaxPayloadSize));
            }

            // Reset Buffer to just after the header because packets are re-used and so that
            // operator+ works properly when copying the payload
            buffer.Reset(0, HeaderSize);
            buffer += chunk;
        }

        internal void AppendPayload(Int32 data)
        {
            buffer += data;
        }

        internal void AppendPayload(BufferChunk data)
        {
            buffer += data;
        }

        internal void AppendPayload(IntPtr ptr, int length)
        {
            buffer.CopyFrom(ptr, length);
        }

        /// <summary>
        /// How much payload data can this packet accept
        /// 
        /// Be sure and set all of the header information before making this call otherwise it will
        /// be incorrect.
        /// </summary>
        internal int MaxPayloadSize
        {
            get
            {
                return buffer.Buffer.Length - HeaderSize - ReservedPaddingBytes;
            }
        }

        /// <summary>
        /// Release the BufferChunk held by this packet so it can be reused outside the scope of this packet.
        /// </summary>
        /// <returns></returns>
        internal BufferChunk ReleaseBuffer()
        {
            BufferChunk ret = buffer;
            buffer = null;

            return ret;
        }


        internal virtual int HeaderSize
        {
            get{return RTP_HEADER_SIZE;}
        }

        internal BufferChunk Buffer
        {
            get{return buffer;}
        }

        
        internal virtual int PayloadSize
        {
            get
            {
                int size = buffer.Length - HeaderSize;

                Debug.Assert(size >= 0);

                return size;
            }
            set
            {
                buffer.Reset(0, HeaderSize + value);
            }
        }
        
        
        internal virtual void Reset()
        {
            buffer.Reset(0, HeaderSize);

            // Initialize the first byte: V==2, P==0, X==0, CC==0
            buffer[VPXCC_INDEX] = (byte)(Rtp.VERSION << 6);
        }

        #endregion Internal
        
        #region Private

        /// <summary>
        /// Make sure the provided buffer might be a real Rtp Packet (version == 2)
        /// </summary>
        private void ValidateBuffer(BufferChunk buffer)
        {
            int version = buffer[VPXCC_INDEX] >> 6;

            if (version != Rtp.VERSION)
                throw new InvalidRtpPacketException(string.Format(CultureInfo.CurrentCulture, 
                    Strings.InvalidVersion, version, Rtp.VERSION));
        }

        #endregion Private

    }

    
    #endregion RtpPacketBase

    #region RtpPacket

    /// <summary>
    /// RtpPacket is based on RFC 1889.  This class knows how to form a byte array for sending out over the network and how to turn a byte array into Rtp fields and a payload.
    /// It is mean to be used as a translation mechanism from bytes to structure and vice versa.  This is a lower level class exposed only for use by
    /// applications that want intiment details about an individual Rtp packet or who want to provide their own transport mechanism.  Applications
    /// that simply want to send/receive real time data over IP Multicast should instead use RtpSender / RtpListener which handles all
    /// aspects of network transport and framing (AKA breaking/assembling large datasets into packet sized chunks).
    /// 
    /// There is a small amount of Rtp protocol intelligence in the class when you use the Next methods.  The Next methods assume you are working
    /// on RtpPackets in a series and will perform helper functions such as compare Sequence numbers for linearness and NextPayload increments
    /// the Sequence number between new packets.
    /// 
    /// This implementation has no support for CSRC identifiers.
    /// 
    /// 
    ///       The Rtp header has the following format:
    ///
    ///0                   1                   2                   3
    ///0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    ///+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///|V=2|P|X|  CC   |M|     PT      |       sequence number         |
    ///+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///|                           timestamp                           |
    ///+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///|           synchronization source (SSRC) identifier            |
    ///+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+
    ///|            contributing source (CSRC) identifiers             |
    ///|                             ....                              |
    ///+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    /// </summary>
    public class RtpPacket : RtpPacketBase
    {
        #region Statics

        /// <summary>
        /// We use a fixed size header extension
        /// </summary>
        private const int HEADER_EXTENSIONS_SIZE = PACKETS_IN_FRAME_SIZE +
                                                   FRAME_INDEX_SIZE +
                                                   FEC_INDEX_SIZE;

        private const int PACKETS_IN_FRAME_SIZE = 2;
        private const int FRAME_INDEX_SIZE = 2;
        private const int FEC_INDEX_SIZE = 2;

        #endregion Statics

        #region Constructors

        public RtpPacket() : base() {}

        public RtpPacket(int packetSize) : base(packetSize){}

        public RtpPacket(BufferChunk buffer) : base(buffer){}

        public RtpPacket(RtpPacketBase packet) : base(packet){}

        
        #endregion

        #region Internal

        internal ushort PacketsInFrame
        {
            get{return Buffer.GetUInt16(PacketsInFrame_Index);}
            set{Buffer.SetUInt16(PacketsInFrame_Index, value);}
        }

        internal ushort FrameIndex
        {
            get
            {
                return Buffer.GetUInt16(FrameIndex_Index);
            }
            set
            {
                Buffer.SetUInt16(FrameIndex_Index, value);
            }
        }

        internal ushort FecIndex
        {
            get
            {
                return Buffer.GetUInt16(FecIndex_Index);
            }
            set
            {
                Buffer.SetUInt16(FecIndex_Index, value);
            }
        }


        internal override int HeaderSize
        {
            get
            {
                return base.HeaderSize + HEADER_EXTENSIONS_SIZE;
            }
        }


        #endregion Internal

        #region Private

        private int PacketsInFrame_Index
        {
            get{return base.HeaderSize;}
        }

        private int FrameIndex_Index
        {
            get{return PacketsInFrame_Index + PACKETS_IN_FRAME_SIZE;}
        }

        private int FecIndex_Index
        {
            get{return FrameIndex_Index + FRAME_INDEX_SIZE;}
        }

        
        #endregion Private

   
    }

    #endregion RtpPacket

    #region RtpPacketFec

    /// <summary>
    /// RtpPacketFec is a forward error correction packet.  It is used to provide error correction
    /// for data packets that may become lost.
    /// 
    /// It has a fixed payload type PayloadType.FEC
    /// The normal Rtp Timestamp has been repurposed in order to save bytes.  It is split into...
    /// 
    /// FecIndex - the index of this packet within the fec packet[].  The size of the fec packet[]
    /// is either determined by the constant fec ratio, or the percent coverage across a frame.
    /// 
    /// DataRangeMin - the starting data packet sequence number for which this packet provides
    /// coverage.
    /// 
    /// PacketsInFrame - how many packets are in a frame.  Used in the event that no data packets
    /// are received, but enough fec packets arrive to recover the data.
    /// 
    /// 0                   1                   2                   3
    /// 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    ///+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///|V=2|P|X|  CC   |M|    PT.FEC   |       sequence number         |
    ///+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///|          DataRangeMin         |         PacketsInFrame        |
    ///+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///|           synchronization source (SSRC) identifier            |
    ///+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    ///|           FecIndex            |
    ///+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+
    /// </summary>
    internal class RtpPacketFec : RtpPacketBase
    {
        #region Statics

        // This is a hack.  The general idea is that we don't want to fragment an Fec Packet.
        // In order to prevent fragmentation, we have to limit an Rtp Packet to a known size, so
        // there is room for the Fec Packet overhead.  When we add support for CSRCs or Header
        // extensions, this value will be incorrect.  And we won't have allocated enough space for
        // the Fec Packet.  JVE 6/16/2004
        internal const int HEADER_SIZE_HACK = RtpPacketBase.RTP_HEADER_SIZE + 
                                              HEADER_OVERHEAD_SIZE          +
                                              CFec.SIZE_OVERHEAD;

        /// <summary>
        /// The 4 bytes of Timestamp have been re-purposed, but we needed 2 extra bytes for header
        /// </summary>
        private const int HEADER_OVERHEAD_SIZE = 2;

        #endregion Statics

        #region Constructors

        internal RtpPacketFec() : base()
        {
            PayloadType = PayloadType.FEC;
        }

        internal RtpPacketFec(RtpPacketBase packet) : base(packet)
        {
            if(PayloadType != PayloadType.FEC)
            {
                throw new ArgumentException(Strings.PacketIsNotAnFECPacket);
            }
        }

        
        #endregion Constructors

        #region Internal

        internal override int HeaderSize
        {
            get
            {
                int size = base.HeaderSize;

                // See HEADER_SIZE_HACK comments
                Debug.Assert(size == RtpPacketBase.RTP_HEADER_SIZE);

                size += HEADER_OVERHEAD_SIZE;

                return size;
            }
        }

        
        internal override uint TimeStamp
        {
            get
            {
                throw new InvalidOperationException(Strings.RtpPacketFecDoesNotSupportTimestamp);
            }
            set
            {
                throw new InvalidOperationException(Strings.RtpPacketFecDoesNotSupportTimestamp);
            }
        }


        internal override void Reset()
        {
            buffer.Clear();
            base.Reset ();
            PayloadType = PayloadType.FEC;
        }


        internal ushort DataRangeMin
        {
            get{return Buffer.GetUInt16(DataRangeMin_Index);}
            set{Buffer.SetUInt16(DataRangeMin_Index, value);}
        }


        internal ushort PacketsInFrame
        {
            get{return Buffer.GetUInt16(PacketsInFrame_Index);}
            set{Buffer.SetUInt16(PacketsInFrame_Index, value);}
        }

        
        internal ushort FecIndex
        {
            get{return Buffer.GetUInt16(FecIndex_Index);}
            set{Buffer.SetUInt16(FecIndex_Index, value);}
        }

        
        #endregion Internal

        #region Private

        private int DataRangeMin_Index
        {
            get{return TS_INDEX;}
        }

        private int PacketsInFrame_Index
        {
            get{return DataRangeMin_Index + 2;}
        }

        private int FecIndex_Index
        {
            get{return SSRC_INDEX + SSRC_SIZE;}
        }
        
        #endregion Private
    }

    #endregion RtpPacketFec

    #region Exception Classes
    /// <summary>
    /// OutOfOrder exception is thrown when issues are found with the Sequence or TimeStamp where they don't match up with the expected values an
    /// individual packet in a stream of Rtp packets should have.
    /// 
    /// Note that this exception is also thrown by the RtpPacket class when using the RtpPacket.Next() method.
    /// </summary>
    public class PacketOutOfSequenceException: ApplicationException
    {
        public int LostPackets = 0;
        public PacketOutOfSequenceException()
        {
        }
        public PacketOutOfSequenceException(string message)
            : base(message)
        {
        }
        public PacketOutOfSequenceException(string message, int lostPackets)
            : base(message)
        {
            LostPackets = lostPackets;
        }
        public PacketOutOfSequenceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    /// <summary>
    /// InvalidRtpPacket exception is thrown when an Rtp packet has invalid contents either due to an invalid Rtp header or due to unexpected
    /// data in a stream such as a HeaderExtension where none should be present or an invalid TimeStamp value in an Rtp Frame.
    /// </summary>
    /// <remarks>
    /// This can be caused by other traffic than just Rtp on an IPEndPoint or Rtp traffic from another sending program that doesn't follow the same
    /// framing rules.  It shouldn't be caused by packet data corruption on UDP streams since each UDP packet is CRC32 validated before being accepted
    /// and passed up by System.Net.Sockets.  It also should only be an issue if there is SSRC &amp; PayloadType collision between different sending
    /// applications, which should be rare if SSRCs are chosen according to the RFC 1889 specification.
    /// 
    /// Perhaps we should rename this to be consistent with some form of 'streaming/framing error'.  If we get a true 'invalid Rtp Packet' error, it's
    /// probably due to non-Rtp data being on the IP address and this should be filtered rather than Excepted.  Perhaps we should republish this an
    /// event like was done with OutOfOrder so that non-Rtp traffic could be detected and logged.
    /// </remarks>
    public class InvalidRtpPacketException: ApplicationException
    {
        public InvalidRtpPacketException()
        {
        }
        public InvalidRtpPacketException(string message)
            : base(message)
        {
        }
        public InvalidRtpPacketException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    #endregion
}
