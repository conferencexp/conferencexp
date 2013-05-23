using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Text;


namespace MSR.LST.Net.Rtp
{
    /// <summary>
    /// -------------------------------------------------------------------------------------------
    /// RTCP:
    /// -------------------------------------------------------------------------------------------
    /// RTCP is the Real Time Control Protocol
    /// 
    /// RTCP provides for the periodic reporting of reception quality, participant identification 
    /// and other source description information, notification on changes in session membership, 
    /// and the information needed to synchronize media streams.
    /// 
    /// -------------------------------------------------------------------------------------------
    /// RTCP packet / Compound packet:
    /// -------------------------------------------------------------------------------------------
    /// The data contained in an RTCP packet is actually an amalgamation of multiple well-known
    /// RTCP "packet" types.  From here forward, the packet that actually gets delivered across the
    /// network will be known as a *compound* packet, and the well-known RTCP packet types will be 
    /// called simply packets.
    /// 
    /// -------------------------------------------------------------------------------------------
    /// Well-known RTCP packets:
    /// -------------------------------------------------------------------------------------------
    /// SR      Sender Report       Sent by participants who have recently sent RTP data
    /// RR      Receiver Report     Sent by participants who receive data
    /// SDES    Source Description  Participant identification
    /// BYE     Participant Membership  Indicates a participant is leaving the session
    /// APP     Application         Application defined data
    /// 
    /// -------------------------------------------------------------------------------------------
    /// Implementation details:
    /// -------------------------------------------------------------------------------------------
    /// Each RtpSender and RtpListener instance sends its own set of Rtcp packets.  In the case of
    /// an RtpSender, it sends SDES packets to describe the RtpStream of data it's sending out, SR
    /// packets with statistics on what is being sent, and BYE when the RtpStream is destroyed.  
    /// In the case of an RtpListener, it sends SDES packets describing who is listening, RR 
    /// packets with statistics on every RtpStream received by that RtpListener, and BYE packets 
    /// when the RtpListener is destroyed.
    /// 
    /// Since Rtcp packets may be sent over an unreliable transport such as UDP, Rtcp packets are 
    /// not guaranteed to arrive.  Therefore, timeouts are used in addition to listening for BYE 
    /// packets.
    /// 
    /// Note: Interop with other RTCP implementations is not a primary goal
    /// </summary>
    
    /// <summary>
    /// This enumerator dequeues the items from a Queue as it iterates over them
    /// </summary>
    internal class DequeueEnumerator : IEnumerator
    {
        private int index = -1;
        private Queue q;

        #region Constructor

        public DequeueEnumerator(Queue q)
        {
            this.q = q;
        }

            
        #endregion Constructor

        #region IEnumerator Interface

        object IEnumerator.Current
        {
            get
            {
                if(index == -1)
                {
                    throw new InvalidOperationException(Strings.CallMovenextBeforeCallingCurrent);
                }

                return q.Peek();
            }
        }

        bool IEnumerator.MoveNext()
        {
            bool ret = true;

            if(q.Count > 0)
            {
                // Don't dequeue on the first call
                if(index == -1)
                {
                    index = 0;
                }
                else
                {
                    q.Dequeue();
                }
            }

            if(q.Count == 0)
            {
                ret = false;
            }
                
            return ret;
        }

        void IEnumerator.Reset()
        {
            index = -1;
        }

            
        #endregion IEnumerator Interface
    }
        

    internal class CompoundPacketBuilder : IEnumerable
    {
        #region Members

        private Queue compoundPackets = new Queue();

        // Packets that can be added to the compound packet
        private Queue senderReportPackets = new Queue();
        private Queue appPackets = new Queue();

        // Reports that can be added to the compound packet, used to build sub-packets
        private Queue receiverReports = new Queue();
        private Queue byeReports = new Queue();
        private Stack sdesReports = new Stack();

        /// <summary>
        /// The participant for the local session
        /// </summary>
        private RtpParticipant participant;

        private int space;
        private bool cpComplete;

        #endregion Members

        #region Constructors

        // Allocate the storage to receive data into off the wire
        // TODO - do we need to do this when building a packet to send? JVE
        public CompoundPacketBuilder()
        {
        }

        #endregion Constructors

        #region Methods
        
        #region Add Report / Packet

        public void Add_BYEReport(uint ssrc)
        {
            byeReports.Enqueue(ssrc);
        }

        public void Add_SenderReport(uint ssrc, SenderReport sr)
        {
            senderReportPackets.Enqueue(new SrPacket(ssrc, sr));
        }

        public void Add_ReceiverReport(ReceiverReport rr)
        {
            receiverReports.Enqueue(rr);
        }

        public void Add_SDESReport(uint ssrc, SdesData sdes)
        {
            sdesReports.Push(new SdesReport(ssrc, sdes));
        }

        public void Add_APPReport(uint ssrc, string name, byte subtype, byte[] data)
        {
            this.appPackets.Enqueue(new AppPacket(ssrc, name, subtype, data));
        }

        
        public void ParticipantData(RtpParticipant participant)
        {
            this.participant = participant;
        }

        
        public short PacketCount
        {
            get{return (short)compoundPackets.Count;}
        }


        #endregion Add Report / Packet
        
        internal void BuildCompoundPackets()
        {
            // Add the local participant / session info
           sdesReports.Push(new SdesReport(participant.SSRC, participant));

            while(DataRemains())
            {
                CompoundPacket cp = new CompoundPacket();
                space = cp.Size;
                cpComplete = false;

                // All compound packets must start with a SenderReportPacket or a
                // ReceiverReportPacket.  If we don't have real ones, add an empty 
                // ReceiverReportPacket.
                if(senderReportPackets.Count == 0 && receiverReports.Count == 0)
                {
                    AddEmptyReceiverReportPacket(cp);
                }
                else
                {
                    AddSenderReportPackets(cp);
                    AddReceiverReports(cp);
                }

                AddSdesReports(cp);
                AddAppPackets(cp);
                AddByeReports(cp);

                compoundPackets.Enqueue(cp);
            }
        }

        private void AddEmptyReceiverReportPacket(CompoundPacket cp)
        {
            RrPacket rr = new RrPacket();
            rr.SSRC = participant.SSRC;

            int start = cp.Buffer.Length;
            cp.AddPacket(rr);
            int end = cp.Buffer.Length;

            space -= rr.Size;

            Debug.Assert( (end - start) == rr.Size ); // math check
        }
        
        private void AddSenderReportPackets(CompoundPacket cp)
        {
            while(!cpComplete && senderReportPackets.Count > 0)
            {
                SrPacket sr = (SrPacket)senderReportPackets.Peek();
        
                if(space >= sr.Size)
                {
                    int start = cp.Buffer.Length;
                    cp.AddPacket(sr);
                    int end = cp.Buffer.Length;

                    space -= sr.Size;
                    senderReportPackets.Dequeue();

                    Debug.Assert( (end - start) == sr.Size ); // math check
                }
                else
                {
                    cpComplete = true;
                }
            }
        }
        
        private void AddReceiverReports(CompoundPacket cp)
        {
            while(!cpComplete && receiverReports.Count > 0)
            {
                bool rrpComplete = false;
                bool addedReport = false;

                RrPacket rrp = new RrPacket();
                rrp.SSRC = participant.SSRC;
        
                // Remove the size of the header + ssrc
                if(space >= rrp.Size)
                {
                    space -= rrp.Size;

                    // Add the rest
                    while(space >= ReceiverReport.SIZE && receiverReports.Count > 0)
                    {
                        try
                        {
                            ReceiverReport rr = (ReceiverReport)receiverReports.Peek();
                            rrp.AddReceiverReport(rr);
                            receiverReports.Dequeue();
                            
                            space -= ReceiverReport.SIZE;
                            addedReport = true;
                        }
                        catch(RtcpPacket.InsufficientItemSpaceException)
                        {
                            // No more room in rrp for reports
                            rrpComplete = true;
                            break;
                        }
                    }
                }

                // We broke out of the loop for one of 3 reasons
                // 1. There were no more ReceiverReports
                // 2. There was no more room in rrp
                // 3. There was no more room in cp (cpComplete)

                // If we added a report to rrp, add rrp to cp
                if(addedReport)
                {
                    int start = cp.Buffer.Length;
                    cp.AddPacket(rrp);
                    int end = cp.Buffer.Length;

                    Debug.Assert( (end - start) == rrp.Size ); // math check
                }

                // Figure out if we exited because cp is complete
                if(receiverReports.Count > 0 && rrpComplete == false)
                {
                    cpComplete = true;
                }
            }
        }
        
        private void AddSdesReports(CompoundPacket cp)
        {
            while(!cpComplete && sdesReports.Count > 0)
            {
                bool sdespComplete = false;
                bool addedReport = false;

                SdesPacket sdesp = new SdesPacket();
        
                if(space >= sdesp.Size)
                {
                    space -= sdesp.Size;

                    while(sdesReports.Count > 0)
                    {
                        SdesReport report = (SdesReport)sdesReports.Peek();

                        if( space >= report.Size )
                        {
                            try
                            {
                                sdesp.AddReport(report);
                                sdesReports.Pop();

                                space -= report.Size;
                                addedReport = true;
                            }
                            catch(RtcpPacket.InsufficientItemSpaceException)
                            {
                                // No more room in rrp for reports
                                sdespComplete = true;
                                break;
                            }
                        }
                        else
                        {
                            break; // while loop
                        }
                    }
                }

                // We broke out of the loop for one of 3 reasons
                // 1. There were no more sdesReports
                // 2. There was no more room in sdesp
                // 3. There was no more room in cp (cpComplete)

                // If we added a report to sdesp, add sdesp to cp
                if(addedReport)
                {
                    int start = cp.Buffer.Length;
                    cp.AddPacket(sdesp);
                    int end = cp.Buffer.Length;

                    Debug.Assert( (end-start) == sdesp.Size ); // math check
                }

                // Figure out if we exited because cp is complete
                if(sdesReports.Count > 0 && sdespComplete == false)
                {
                    cpComplete = true;
                }
            }
        }
        
        private void AddAppPackets(CompoundPacket cp)
        {
            while(!cpComplete && appPackets.Count > 0)
            {
                AppPacket app = (AppPacket)appPackets.Peek();
        
                if(space >= app.Size)
                {
                    int start = cp.Buffer.Length;
                    cp.AddPacket(app);
                    int end = cp.Buffer.Length;

                    space -= app.Size;
                    appPackets.Dequeue();

                    Debug.Assert( (end-start) == app.Size ); // math check
                }
                else
                {
                    cpComplete = true;
                }
            }
        }
        
        private void AddByeReports(CompoundPacket cp)
        {
            while(!cpComplete && byeReports.Count > 0)
            {
                bool byepComplete = false;
                bool addedReport = false;

                ByePacket byep = new ByePacket();
        
                // Remove the size of the header + ssrc
                if(space >= byep.Size)
                {
                    space -= byep.Size;

                    // Add the rest
                    while(space >= Rtp.SSRC_SIZE && byeReports.Count > 0)
                    {
                        try
                        {
                            uint ssrc = (uint)byeReports.Peek();
                            byep.AddSSRC(ssrc);
                            byeReports.Dequeue();
                            
                            space -= Rtp.SSRC_SIZE;
                            addedReport = true;
                        }
                        catch(RtcpPacket.InsufficientItemSpaceException)
                        {
                            // No more room in bp for reports
                            byepComplete = true;
                            break;
                        }
                    }
                }

                // We broke out of the loop for one of 3 reasons
                // 1. There were no more byeReports
                // 2. There was no more room in bp
                // 3. There was no more room in cp (cpComplete)

                // If we added a report to bp, add bp to cp
                if(addedReport)
                {
                    int start = cp.Buffer.Length;
                    cp.AddPacket(byep);
                    int end = cp.Buffer.Length;

                    Debug.Assert( (end-start) == byep.Size ); // math check
                }

                // Figure out if we exited because cp is complete
                if(byeReports.Count > 0 && byepComplete == false)
                {
                    cpComplete = true;
                }
            }
        }
        
        
        private bool DataRemains()
        {
            int itemCount = 0;

            itemCount += senderReportPackets.Count;
            itemCount += receiverReports.Count;
            itemCount += appPackets.Count;
            itemCount += sdesReports.Count;
            itemCount += byeReports.Count;

            return itemCount > 0;
        }

        
        #endregion Methods

        #region IEnumerable Implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new DequeueEnumerator(compoundPackets);
        }


        #endregion IEnumerable Implementations
    }

    /// <summary>
    /// A compound Rtcp packet contains multiple Rtcp packets
    /// 
    /// It is public for use by the NetworkDumper diagnostic tool
    /// </summary>
    public class CompoundPacket : IEnumerable
    {
        #region Members

        private BufferChunk buffer;
        private Queue packets = new Queue();
        
        private static int size = Rtp.MAX_PACKET_SIZE;

        #endregion Members

        #region Constructors

        // Allocate the storage to receive data into off the wire
        public CompoundPacket()
        {
            buffer = new BufferChunk(size);
        }

        public CompoundPacket(BufferChunk buffer)
        {
            this.buffer = buffer;
            ParseBuffer();
        }

        
        #endregion Constructors

        #region Methods

        internal void AddPacket(RtcpPacket packet)
        {
            packet.WriteToBuffer(buffer);
        }

        public BufferChunk Buffer
        {
            get{return buffer;}
        }

        internal BufferChunk Data
        {
            get{return buffer.Peek(buffer.Index, buffer.Length);}
        }

        
        public void Reset()
        {
            packets.Clear();
            buffer.Reset(0,0);
        }

        
        /// <summary>
        /// Called immediately after the buffer is populated by the network listener
        /// It parses RtcpPackets and rigorously validates them
        /// </summary>
        public void ParseBuffer()
        {
            // See if the packet ends on a 32 bit boundary as it should
            if(buffer.Length % 4 != 0)
            {
                throw new CompoundPacketException(Strings.CompoundPacketsMustEndOnBoundary);
            }

            // Parse all data in the buffer
            while(buffer.Length > 0)
            {
                // Retrieve the next packet from the buffer
                // RTCP Version will be validated by RtcpHeader in the generic RtcpPacket
                RtcpPacket packet = new RtcpPacket(buffer);

                // Check a couple of facts if the padding bit is set
                if(packet.Padding == true)
                {
                    // Can't be set on the first packet
                    if(packets.Count == 0)
                    {
                        throw new CompoundPacketException(Strings.YouCantSetThePaddingBit);
                    }
                    else
                    {
                        // There must be as many padding octets as the the last octet claims
                        int octets = 0;
                        byte octet = 0;

                        while(buffer.Length > 0)
                        {
                            octet = buffer.NextByte();
                            octets++;
                        }

                        // Last octet contains the count of padding octets
                        if(octet != octets)
                        {
                            throw new CompoundPacketException(string.Format(CultureInfo.CurrentCulture, 
                                Strings.PaddingBytesDiscrepancy, octets, octet));
                        }
                    }
                }

                packets.Enqueue(packet);
            }

            // It must be a compound packet
            if(packets.Count <= 1)
            {
                throw new CompoundPacketException(string.Format(CultureInfo.CurrentCulture, 
                    Strings.MustContainMoreThan1Packet, packets.Count));
            }

            // First packet must be an SR or RR
            byte pt = ((RtcpPacket)packets.Peek()).PacketType;
            if((pt != (byte)Rtcp.PacketType.SR) && (pt != (byte)Rtcp.PacketType.RR))
            {
                throw new CompoundPacketException(string.Format(CultureInfo.CurrentCulture, 
                    Strings.FirstPacketMustBeSROrRR, (byte)Rtcp.PacketType.SR, (byte)Rtcp.PacketType.RR, pt));
            }
        }

        
        internal int Size
        {
            get{return size;}
        }

        
        #endregion Methods

        #region IEnumerable Implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new DequeueEnumerator(packets);
        }


        #endregion IEnumerable Implementations
        
        #region Exceptions

        /// <summary>
        /// InvalidRtcpPacketExeption is thrown if RtcpPacket.CheckForValidRtcpPacket() fails
        /// </summary>
        public class CompoundPacketException : ApplicationException
        {
            public CompoundPacketException(string msg) : base(msg) {}
        }

        
        #endregion Exceptions
    }


    /// <summary>
    /// -------------------------------------------------------------------------------------------
    /// RtcpHeader class purpose:
    /// -------------------------------------------------------------------------------------------
    /// Manipulates the RTCP header (first 32 bits (4 bytes)) of the buffer (byte[]) provided in 
    /// its constructor
    /// 
    /// -------------------------------------------------------------------------------------------
    /// RTCP Header description:
    /// -------------------------------------------------------------------------------------------
    /// An RtcpHeader is always 32 bits long and has the following format
    /// 
    ///  0                   1                   2                   3
    ///  0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    /// |V=2|P|    IC   |      PT       |             Length            |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    /// 
    /// V       version
    ///             always 2
    /// 
    /// P       padding bit
    ///             The padding bit is set on the last packet of the compound 
    ///             packet if padding is used after the packet
    ///             
    /// IC      item count between 0 and 31 (2^5 - 1)
    ///             For SR,   IC == RC, number of receiver report blocks
    ///             For RR,   IC == RC, number of receiver report blocks
    ///             For SDES, IC == SC, number of SDES list items
    ///             For BYE,  IC == RC, number of SSRC headers
    ///             For APP,  IC == Subtype
    ///             
    ///             For ACK / NACK, IC == FMT, feedback message type
    ///             
    /// PT      payload type
    ///             SR   = 200
    ///             RR   = 201
    ///             SDES = 202
    ///             BYE  = 203
    ///             APP  = 204
    ///             
    ///             ACK  = ?
    ///             NACK = ?
    ///             
    /// Length  number of 32 bit words in the packet not including the header (data)
    /// </summary>
    internal class RtcpHeader : ICloneable, IRtcpData
    {
        #region Static
        
        /// <summary>
        ///  Size of the RTCP header in bytes - 4
        /// </summary>
        public const int SIZE = 4;

        public static readonly byte[] PlaceHolder = new byte[SIZE];

        #region Bit Masks

        /// <summary>
        /// 3 most significant bit
        /// </summary>
        private static byte PADDING_MASK = 0x20; // 0010 0000

        /// <summary>
        /// 3 most significant bit
        /// </summary>
        private static byte ITEMCOUNT_MASK = 0x1F; // 0001 1111

        #endregion Bit Masks

        #endregion Static

        #region Members

        // No 'version' member, just use constant VERSION

        private bool padding = false;
        private int itemCount = 0;
        private byte packetType = 0;
        private short length = 0;

        #endregion Members

        #region Constructors

        /// <summary>
        /// Must create a packet of a derived type if building the packet from scratch
        /// </summary>
        public RtcpHeader(byte packetType)
        {
            this.packetType = packetType;
        }
        
        
        /// <summary>
        /// Constructor which converts buffer into member variables
        /// Expects buffer to be 4 bytes long
        /// </summary>
        /// <param name="buffer">Data to be manipulated</param>
        public RtcpHeader(BufferChunk buffer)
        {
            if(buffer.Length != SIZE)
            {
                throw new RtcpHeaderException(string.Format(CultureInfo.CurrentCulture, Strings.InvalidBufferLength, 
                    buffer.Length, RtcpHeader.SIZE));
            }

            ReadDataFromBuffer(buffer);
        }

        
        #endregion Constructors

        #region Methods

        #region Public

        /// <summary>
        /// Manipulates bits 0 and 1 of byte 1 of the RTCP header
        /// Version should always be the current version of RTCP (a.k.a. 2)
        /// </summary>
        public int Version
        {
            get{return Rtp.VERSION;}
        }

        
        /// <summary>
        /// Property get/set which manipulates bit 2 of byte 1 of the RTCP header
        /// Indicates whether the RTCP packet contains padding at the end of the packet
        /// </summary>
        public bool Padding
        {
            get{return padding;}
            set{padding = value;}
        }

        
        /// <summary>
        /// Property get/set which manipulates bits 3-7 (5 bits) of byte 1 of the RTCP header
        /// Indicates how many "items" (packet type dependent) are in the packet
        /// Valid values are between 0 and 31 inclusive (min and max values of 5 bits)
        /// </summary>
        public int ItemCount
        {
            get{return itemCount;}

            set
            {
                // Count must be between 0 and 31
                if(value < 0 || value > 31)
                {
                    throw new RtcpHeaderException(string.Format(CultureInfo.CurrentCulture, Strings.InvalidItemCount, value));
                }

                itemCount = value;
            }
        }

        
        /// <summary>
        /// Property get/set which manipulates byte 2 of the RTCP header
        /// Indicates what type of RTCP packet follows - SR, RR, SDES, BYE, APP, etc.
        /// 
        /// A byte is used, rather than the enum, in order to be compatible
        /// with as of yet unknown types (as recommended in Colin's book)
        /// </summary>
        public byte PacketType
        {
            get{return packetType;}

            set
            {
                // Colin's book recommends *not* validating the PacketType in order to be forward
                // compatible with as of yet unknown types
                packetType = value;
            }
        }

        
        /// <summary>
        /// Property get/set which manipulates bytes 3 and 4 of the RTCP header
        /// Length is the count of 32 bit words that follow the header (packet data)
        /// </summary>
        public short Length
        {
            get{return length;}

            set
            {
                if(value < 0 || value > (Rtp.MAX_PACKET_SIZE / 4) - 1) // 32 bit words, minus header
                {
                    throw new RtcpHeaderException(string.Format(CultureInfo.CurrentCulture, Strings.InvalidLength,
                        value, (Rtp.MAX_PACKET_SIZE / 4) - 1));
                }

                length = value;
            }
        }


        #region IRtcpData

        /// <summary>
        /// Size of the RtcpHeader, in bytes
        /// </summary>
        public int Size
        {
            get{return SIZE;}
        }

        
        /// <summary>
        /// Converts member data to byte[]
        /// </summary>
        /// <param name="buffer"></param>
        public void WriteDataToBuffer(BufferChunk buffer)
        {
            // "Clear" the byte by setting the Version
            // 2 (10) becomes 128 (10000000) when you shift it into place in the header
            buffer[0] = Rtp.VERSION << 6;

            // Padding
            if(Padding == true)
            {
                buffer[0] |= PADDING_MASK;
            }

            // ItemCount
            buffer[0] += (byte)(ItemCount); // Add in the new value

            // PacketType
            buffer[1] = PacketType;

            // Length
            buffer.SetInt16(2, Length);
        }


        /// <summary>
        /// Converts the first 4 bytes of the buffer into member variables
        /// </summary>
        /// <param name="buffer"></param>
        public void ReadDataFromBuffer(BufferChunk buffer)
        {
            int version = buffer[0] >> 6;
            
            if(version != Rtp.VERSION)
            {
                throw new RtcpHeaderException(string.Format(CultureInfo.CurrentCulture, Strings.InvalidVersion, 
                    version, Rtp.VERSION));
            }

            Padding = Convert.ToBoolean(buffer[0] & PADDING_MASK);
            ItemCount = buffer[0] & ITEMCOUNT_MASK;
            PacketType = buffer[1];
            Length = buffer.GetInt16(2);
        }


        #endregion IRtcpData

        #region ICloneable Members

        public object Clone()
        {
            RtcpHeader clone = new RtcpHeader(PacketType);

            clone.ItemCount = ItemCount;
            clone.Length = Length;
            clone.Padding = Padding;

            return clone;
        }

        
        #endregion

        #endregion Public
        
        #endregion Methods

        #region Exceptions

        /// <summary>
        /// This exception is thrown if anything unexpected happens while manipulating an RTCP header
        /// </summary>
        public class RtcpHeaderException : ApplicationException
        {
            public RtcpHeaderException(string msg) : base(msg) {}
        }

        
        #endregion Exceptions
    }

    
    /// <summary>
    /// -------------------------------------------------------------------------------------------
    /// RtcpPacket class purpose:
    /// -------------------------------------------------------------------------------------------
    /// Provides the common members of a generic RTCP packet (header and data) and provides the
    /// interface for accessing that data from all specialized RTCP packets
    /// 
    /// The well-known packet types SR, RR, SDES, BYE, APP, (etc.) will inherit from it and 
    /// override or implement their own specific functionality
    /// 
    /// It is public for use by the NetworkDumper diagnostic tool
    /// </summary>
    public class RtcpPacket : IRtcpData
    {
        #region Static
        
        /// <summary>
        /// All text in an RtcpPacket is encoded in UTF8
        /// Please lock on this object before using it
        /// </summary>
        protected static System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();


        /// <summary>
        /// Adds padding to make sure buffer ends on a 32 bit boundary
        /// </summary>
        /// <param name="cbBuffer">Size of buffer in bytes</param>
        protected static int AddPadding(BufferChunk buffer)
        {
            // We use the negative to take the 2's complement
            // and & 3 to retain the first 2 bits (0-3).
            int padding = GetNeededPadding(buffer.Length);
                
            if(padding > 0)
            {
                buffer += new byte[padding];
            }

            return padding;
        }

        /// <summary>
        /// Does the math for how much padding we need.  Also called from Size.
        /// </summary>
        /// <param name="bufferSize">The size of the buffer to pad.</param>
        /// <returns>The number of padding bytes to add.</returns>
        protected static int GetNeededPadding(int bufferSize)
        {
            return -bufferSize & 3;
        }


        /// <summary>
        /// Removes padding to make sure buffer ends on a 32 bit boundary
        /// </summary>
        /// <param name="cbBuffer">Size of buffer in bytes</param>
        protected static void RemovePadding(BufferChunk buffer)
        {
            int padding = buffer.Length % 4;
                
            if(padding > 0)
            {
                buffer.NextBufferChunk(padding);
            }
        }

        
        /// <summary>
        /// Make sure the ItemCount (of a specific packet type) has room for another item
        /// Should be called before adding an item to a collection
        /// </summary>
        /// <param name="itemCount">current ItemCount</param>
        protected static void ValidateItemSpace(int itemCount)
        {
            if(itemCount == 31)
            {
                throw new InsufficientItemSpaceException(Strings.TooManyItems);
            }
        }

        
        /// <summary>
        /// Make sure the packet passed to specialized packet constructors is of the correct type
        /// </summary>
        /// <param name="expected">Expected packet type (of the specialized packet class)</param>
        /// <param name="actual">Actual packet type (as found in the header)</param>
        protected static void ValidatePacketType(byte expected, byte actual)
        {
            if(actual != expected)
            {
                throw new RtcpPacketException(string.Format(CultureInfo.CurrentCulture, 
                    Strings.ActualPacketsDifferFromExpected, (Rtcp.PacketType)expected, expected, actual));
            }
        }

        
        /// <summary>
        /// Make sure buffer ends on a 32 bit boundary
        /// </summary>
        /// <param name="cbBuffer">Size of buffer in bytes</param>
        protected static void ValidateBufferBoundary(int cbBuffer)
        {
            if(cbBuffer % 4 != 0)
            {
                throw new RtcpPacketException(string.Format(CultureInfo.CurrentCulture, Strings.BufferMustEndOnBoundary, 
                    cbBuffer, cbBuffer % 4));
            }
        }


        #endregion Static

        #region Members

        private RtcpHeader header = null;
        private BufferChunk buffer = null;
        private uint ssrc = 0;

        #endregion Members
        
        #region Constructors
        
        /// <summary>
        /// Constructor which initializes header for use by derived packets
        /// </summary>
        protected RtcpPacket(Rtcp.PacketType type)
        {
            header = new RtcpHeader((byte)type);
        }

       
        /// <summary>
        /// Constructor which creates a generic RtcpPacket from an existing buffer
        /// </summary>
        /// <param name="buffer"></param>
        public RtcpPacket(BufferChunk buffer)
        {
            header = new RtcpHeader(buffer.NextBufferChunk(RtcpHeader.SIZE));
            this.buffer = buffer.NextBufferChunk(Header.Length * 4);
        }

        
        #endregion Constructors
        
        #region Methods
        
        #region Protected
        
        public void WriteToBuffer(BufferChunk buffer)
        {
            // Grab space for the header
            buffer += RtcpHeader.PlaceHolder;
            BufferChunk hBuffer = buffer.Peek(buffer.Length - RtcpHeader.SIZE, RtcpHeader.SIZE);

            // Write the data to the buffer
            WriteDataToBuffer(buffer);

            // Leave the buffer 32 bit aligned
            int padding = AddPadding(buffer);

            // Update the Header's length
            Debug.Assert((Size) % 4 == 0);
            header.Length = (short)((Size - RtcpHeader.SIZE) / 4);
            
            // Write the header to the buffer
            header.WriteDataToBuffer(hBuffer);
        }

        /// <summary>
        /// Converts member data into buffer 
        /// 
        /// This is meant to be overridden in derived classes, but can't be marked virtual
        /// unless the whole class is marked virtual, which is not desired.
        /// </summary>
        public virtual void WriteDataToBuffer(BufferChunk buffer){}
        
        
        /// <summary>
        /// Converts buffer into member data 
        /// 
        /// This is meant to be overridden in derived classes, but can't be marked virtual
        /// unless the whole class is marked virtual, which is not desired.
        /// </summary>
        public virtual void ReadDataFromBuffer(BufferChunk buffer){}

        
        /// <summary>
        /// Converts packet's data into derived class member data
        /// </summary>
        /// <param name="packet"></param>
        protected void ProcessPacket(RtcpPacket packet)
        {
            ValidatePacketType(Header.PacketType, packet.Header.PacketType);

            // Store the header
            header = packet.Header;

            // Do the real work
            ReadDataFromBuffer(packet.buffer);

            // Leave the buffer 32 bit aligned
            RemovePadding(packet.buffer);
        }

        
        internal RtcpHeader Header
        {
            get{return header;}
            set{header = value;}
        }
        
        #endregion Protected


        #region Public

        public virtual uint SSRC
        {
            get{return ssrc;}
            set{ssrc = value;}
        }

        public int Version
        {
            get{return Header.Version;}
        }

        public int ItemCount
        {
            get{return Header.ItemCount;}
        }

        public byte PacketType
        {
            get{return Header.PacketType;}
        }

        public short Length
        {
            get{return Header.Length;}
        }

        public bool Padding
        {
            get{return header.Padding;}
        }

        
        public virtual int Size
        {
            get { throw new RtcpPacketException(Strings.MethodMeantToBeOverridden); }
        }
        
        
        #endregion Public

        #endregion Methods

        #region Exceptions
        
        /// <summary>
        /// InvalidRtcpPacketExeption is thrown if RtcpPacket.CheckForValidRtcpPacket() fails
        /// </summary>
        public class RtcpPacketException : ApplicationException
        {
            public RtcpPacketException(string msg) : base(msg) {}
        }
        
        
        /// <summary>
        /// InvalidRtcpPacketExeption is thrown if RtcpPacket.CheckForValidRtcpPacket() fails
        /// </summary>
        public class InsufficientItemSpaceException : ApplicationException
        {
            public InsufficientItemSpaceException(string msg) : base(msg) {}
        }

        
        #endregion Exceptions
    }

    
    /// <summary>
    /// Derived class from RtcpPacket, SourceDescription (SDES).  This packet type provides property information for Rtp streams such as CNAME,
    /// NAME, EMAIL, etc.
    /// </summary>
    public class SdesPacket : RtcpPacket, IRtcpData
    {
        #region Members

        /// <summary>
        /// A queue of object[] containing {SSRC, SdesData}
        /// </summary>
        private Queue sdesReports = new Queue();

        #endregion Members

        #region Constructors

        /// <summary>
        /// Construct an empty SDES packet.
        /// </summary>
        public SdesPacket() : base(Rtcp.PacketType.SDES) {}


        /// <summary>
        /// Construct an SDES packet from existing properties
        /// </summary>
        public SdesPacket(uint ssrc, SdesData props) : base(Rtcp.PacketType.SDES)
        {
            AddReport(new SdesReport(ssrc, props));
        }

        
        /// <summary>
        /// Construct an SDES packet from an existing RtcpPacket
        /// </summary>
        public SdesPacket(RtcpPacket packet) : base(Rtcp.PacketType.SDES)
        {
            ProcessPacket(packet);
        }

        
        #endregion
        
        #region Object Overrides
        
        public override string ToString()
        {
            string ret = "SdesPacket { ";

            foreach(SdesReport report in sdesReports)
            {
                // Add ssrc
                ret += report.SSRC;

                // Add packet properties
                ret += PacketType;
                ret += ItemCount;

                // Add SDES properties
                ret += report.SdesData.ToString();
            }

            ret += " }";

            return ret;
        }

        
        #endregion

        #region Methods

        #region Public

        public void AddReport(SdesReport report)
        {
            ValidateItemSpace(sdesReports.Count);

            sdesReports.Enqueue(report);
        }

        public Queue Reports()
        {
            return LSTQueue.Clone(sdesReports);
        }

        
        public override uint SSRC
        {
            set
            {
                throw new ApplicationException(Strings.SDESPacketMayContainMoreThan1SSRC);
            }
        }
        
        
        public override int Size
        {
            get
            {
                int size = RtcpHeader.SIZE;

                foreach(SdesReport report in sdesReports)
                {
                    size += report.Size;
                }

                return size + GetNeededPadding(size);
            }
        }


        /// <summary>
        /// Converts buffer into member data
        /// </summary>
        public override void ReadDataFromBuffer(BufferChunk buffer)
        {
            for(int report = 0; report < Header.ItemCount; report++)
            {
                ReadReportFromBuffer(buffer);
            }
        }

        
        /// <summary>
        /// Converts member data into buffer
        /// </summary>
        public override void WriteDataToBuffer(BufferChunk buffer)
        {
            // Add each (SSRC, SdesData) pair to the buffer
            foreach(SdesReport report in sdesReports)
            {
                WriteReportToBuffer(buffer, report.SSRC, report.SdesData);
            }

            // Update Header
            Header.ItemCount = sdesReports.Count;
        }
        
        
        #endregion Public

        #region Private 
        
        /// <summary>
        /// Write a report (ssrc + SdesData) to the buffer
        /// </summary>
        /// <param name="ssrc">uint was cast to int because that's how BufferChunk writes 32 bits</param>
        /// <param name="props"></param>
        private void WriteReportToBuffer(BufferChunk buffer, uint ssrc, SdesData props)
        {
            buffer += ssrc;
            props.WriteDataToBuffer(buffer);
        }

        private void ReadReportFromBuffer(BufferChunk buffer)
        {
            uint ssrc = buffer.NextUInt32();
            SdesData props = new SdesData(buffer);

            AddReport(new SdesReport(ssrc, props));
        }

        
        #endregion Private

        #endregion Methods
    }

    
    /// <summary>
    /// Derived class from RtcpPacket, Bye.  This packet tells applications when someone is leaving.
    /// </summary>
    public class ByePacket : RtcpPacket
    {
        #region Members

        private ArrayList ssrcs = new ArrayList();
        private byte[] reason = null; // UTF8 string

        #endregion Members

        #region Constructors
        
        /// <summary>
        /// Construct an empty Bye packet
        /// </summary>
        public ByePacket() : base(Rtcp.PacketType.BYE) {}
        
        
        /// <summary>
        /// Constructor used to process an existing BYE packet
        /// </summary>
        public ByePacket(RtcpPacket packet) : base(Rtcp.PacketType.BYE)
        {
            ProcessPacket(packet);
        }

        
        #endregion Constructors

        #region Methods
        
        #region Public

        public override string ToString()
        {
            // TODO - I don't think ToString should be accessing the buffer, since buffer is JIT'd
            StringBuilder ret =  new StringBuilder();
            ret.Append("ByePacket {");
            ret.Append(" PacketType := " + PacketType);
            ret.Append(" ItemCount := " + ItemCount);
            ret.Append(" SSRCs := ");

            foreach(uint ssrc in ssrcs)
            {
                ret.Append(ssrc.ToString(CultureInfo.InvariantCulture) + " ");
            }

            ret.Append(" Reason := " + Reason);

            ret.Append(" }");

            return ret.ToString();
        }

        
        /// <summary>
        /// Add an SSRC to the list
        /// </summary>
        /// <param name="ssrc"></param>
        public void AddSSRC(uint ssrc)
        {
            ValidateItemSpace(ssrcs.Count);

            // TODO - Should we check to see if the ssrc already exists in our list? JVE
            ssrcs.Add(ssrc);
        }

        
        /// <summary>
        /// Property get - return a collection clone of all the SSRCs
        /// </summary>
        public ArrayList SSRCs
        {
            get{return (ArrayList)ssrcs.Clone();}
        }

       
        /// <summary>
        /// Property get/set - the reason for leaving
        /// </summary>
        public string Reason
        {
            get
            {
                string ret;

                if(reason == null)
                {
                    ret = null;
                }
                else
                {
                    lock(utf8)
                    {
                        ret = utf8.GetString(reason);
                    }
                }

                return ret;
            }
            set
            {
                byte[] reason;

                if(value == null || value == String.Empty)
                {
                    reason = null;
                }
                else
                {
                    lock(utf8)
                    {
                        reason = utf8.GetBytes(value);
                    }

                    if(reason.Length > 255)
                    {
                        throw new ApplicationException(string.Format(CultureInfo.CurrentCulture, 
                            Strings.BYEReasonBytesExceeded, 255));
                    }
                }

                this.reason = reason;
            }
        }

        
        public override uint SSRC
        {
            set
            {
                throw new ApplicationException(Strings.BYEPacketMayContainMoreThan1SSRC);
            }
        }
        
        
        public override int Size
        {
            get
            {
                return RtcpHeader.SIZE + Rtp.SSRC_SIZE * ssrcs.Count; // no padding needed, by design
            }
        }

        
        /// <summary>
        /// Converts buffer into member data
        /// </summary>
        /// <param name="packet"></param>
        public override void ReadDataFromBuffer(BufferChunk buffer)
        {
            // Read SSRCs
            for(int i = 0; i < Header.ItemCount; i++)
            {
                ssrcs.Add(buffer.NextUInt32());
            }

            // Determine if there is a reason...
            // SSRCs are 32 bits each, Length is number of 32 bit words in packet data
            if(Header.Length > Header.ItemCount)
            {
                reason = (byte[])buffer.NextBufferChunk(buffer.NextByte());
            }
        }
        
       
        /// <summary>
        /// Converts member data into buffer
        /// </summary>
        /// <param name="packet"></param>
        public override void WriteDataToBuffer(BufferChunk buffer)
        {
            // Add SSRCs
            foreach(uint ssrc in ssrcs)
            {
                buffer += ssrc;
            }

            // Add reason
            if(reason != null)
            {
                buffer += (byte)reason.Length;
                buffer += reason;
            }

            // Update Header
            Header.ItemCount = ssrcs.Count;
        }

        
        #endregion Public
        
        #endregion Methods
    }

    
    /// <summary>
    /// Used to carry application-specific control data that is not meant to be understood by other applications
    /// </summary>
    public class AppPacket : RtcpPacket
    {
        #region Static

        private const int NAME_SIZE = 4;

        #endregion Static

        #region Members

        private byte[] name = null;
        private byte[] data = null;

        #endregion Members

        #region Constructors
        
        public AppPacket(uint ssrc, string name, byte subtype, byte[] data) : base(Rtcp.PacketType.APP)
        {
            SSRC = ssrc;
            Subtype = subtype;
            Name = name;
            Data = data;
        }

        
        /// <summary>
        /// Construct an APP packet from an existing RtcpPacket
        /// </summary>
        /// <param name="packet">Packet to process</param>
        public AppPacket(RtcpPacket packet) : base(Rtcp.PacketType.APP)
        {
            ProcessPacket(packet);
        }

        
        #endregion Constructors

        #region Methods

        #region Public
        
        /// <summary>
        /// A number from 0 to 31 that can be used in addition to the four character name to determine the exact type of the APP packet.
        /// </summary>
        public byte Subtype
        {
            get
            {
                return (byte)Header.ItemCount;
            }
            set
            {
                // ItemCount validates value
                Header.ItemCount = value;
            }
        }
        
        
        /// <summary>
        /// A four character (ASCII) name that identifies this type of APP packet to the application
        /// </summary>
        public string Name
        {
            get
            {
                string ret = null;

                if(name != null)
                {
                    ret = new ASCIIEncoding().GetString(name);
                }

                return ret;
            }

            set
            {
                byte[] name;
                
                // Name is required
                if(value == null || value == String.Empty)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.NameIsRequired, 
                        NAME_SIZE));
                }
                else
                {
                    name = new ASCIIEncoding().GetBytes(value);

                    if (name.Length != NAME_SIZE)
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, 
                            Strings.NameLengthIncorrect, NAME_SIZE));
                    }
                }

                this.name = name;
            }
        }

       
        /// <summary>
        /// Application-specific data
        /// 
        /// RFC 3550 says that this can be up to 256k, RFC3550 doesn't have a limit specified
        /// But we only support a maximum packet size of @1.5K, and it the spec doesn't address
        /// how to "chunk" larger data into smaller packets
        /// </summary>
        public byte[] Data
        {
            get
            {
                return data;
            }

            set
            {
                byte[] data = null;

                if(value != null)
                {
                    ValidateBufferBoundary(value.Length);

                    // We only generate 1.5K packet sizes
                    if (value.Length > Rtp.MAX_PACKET_SIZE - RtcpHeader.SIZE - Rtp.SSRC_SIZE - NAME_SIZE)
                    {
                        throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, 
                            Strings.DataLengthIncorrect, Rtp.MAX_PACKET_SIZE, RtcpHeader.SIZE, Rtp.SSRC_SIZE, 
                            NAME_SIZE));
                    }

                    data = value;
                }

                this.data = data;
            }
        }


        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "RtcpAppPacket [ SSRC := {0} PacketType := {1} " +
                "Subtype := {2} Name := {3} Data Length := {4} ]", this.SSRC, PacketType, this.Subtype, 
                this.Name, this.Data.Length);
        }


        public override int Size
        {
            get
            {
                // andrew: no idea where the + 1 came from...
                //int size = RtcpHeader.SIZE + Rtp.SSRC_SIZE + NAME_SIZE + 1;
                int size = RtcpHeader.SIZE + Rtp.SSRC_SIZE + NAME_SIZE;

                if(data != null)
                {
                    size += data.Length;
                }

                return size + GetNeededPadding(size);
            }
        }


        public override void ReadDataFromBuffer(BufferChunk buffer)
        {
            SSRC = buffer.NextUInt32();
            name = (byte[])buffer.NextBufferChunk(4);

            if(buffer.Length != 0)
            {
                data = (byte[])buffer.NextBufferChunk(buffer.Length);
            }
        }

        public override void WriteDataToBuffer(BufferChunk buffer)
        {
            buffer += SSRC;
            buffer += (BufferChunk)name;

            // Data is not required per RFC 3550 
            if(data != null)
            {
                buffer += (BufferChunk)data;
            }
        }

        
        #endregion

        #endregion Methods
    }

    
    /// <summary>
    /// This packet represents a ReceiverReport and contains statistics on how well RtpStream data has been received.
    /// </summary>
    public class RrPacket : RtcpPacket
    {
        #region Members
        
        /// <summary>
        /// Array of ReceiverReport structs
        /// </summary>
        private ArrayList receiverReports = new ArrayList();
        
        #endregion Members
        
        #region Constructors
        
        /// <summary>
        /// Constructs an empty ReceiverReport packet
        /// </summary>
        public RrPacket() : base(Rtcp.PacketType.RR) {}
 
        
        /// <summary>
        /// Construct a ReceiverReport packet from an existing RtcpPacket
        /// </summary>
        /// <param name="packet">Packet to process</param>
        public RrPacket(RtcpPacket packet) : base(Rtcp.PacketType.RR)
        {
            ProcessPacket(packet);
        }
       
        
        #endregion
        
        #region Methods

        #region Public
        
        public override int Size
        {
            get
            {
                int size = RtcpHeader.SIZE + Rtp.SSRC_SIZE;

                if(receiverReports != null)
                {
                    size += receiverReports.Count * ReceiverReport.SIZE;
                }

                return size + GetNeededPadding(size);
            }
        }

        public ArrayList ReceiverReports
        {
            get
            {
                // Pri2: Need to expose this better so that rr_packet.ReceiverReports.Add fails
                return receiverReports;
            }
        }

        
        /// <summary>
        /// Used to allow one ReceiverReportPacket to be reused between reporting iterations
        /// </summary>
        public void ClearReceiverReports()
        {
            receiverReports.Clear();
        }

        
        /// <summary>
        /// Add a ReceiverReport entry to the ReceiverReportPacket
        /// </summary>
        /// <param name="rr"></param>
        public void AddReceiverReport(ReceiverReport rr)
        {
            ValidateItemSpace(receiverReports.Count);

            receiverReports.Add(rr);
        }

        
        public override string ToString()
        {
            string str = string.Format(CultureInfo.CurrentCulture, "ReceiverReportPacket [ SSRC := {0} " +
                "PacketType := {1} ItemCount := {2}", this.SSRC, PacketType, this.ItemCount);

            foreach(ReceiverReport rr in this.ReceiverReports)
                str += string.Format(CultureInfo.CurrentCulture, " ReceiverReport := {0}", rr.ToString());

            str += " ]";

            return str;
        }
        

        public override void ReadDataFromBuffer(BufferChunk buffer)
        {
            // Make sure ItemCount and Data length agree
            if(Header.Length * 4 != Header.ItemCount * ReceiverReport.SIZE + Rtp.SSRC_SIZE)
            {
                Debug.Assert(false, "Header length and item count disagree!");
                throw new RtcpPacketException(Strings.HeaderLengthAndItemCountDisagree);
            }

            // Store Reporter SSRC
            SSRC = buffer.NextUInt32();

            // Process Reports
            for(int rrCount = 0; rrCount < Header.ItemCount; rrCount++)
            {
                ReceiverReport rr = new ReceiverReport();
                rr.ReadDataFromBuffer(buffer);

                receiverReports.Add(rr);
            }
        }

        public override void WriteDataToBuffer(BufferChunk buffer)
        {
            // Add Reporter SSRC
            buffer += SSRC;

            // Add all the reports
            foreach(ReceiverReport rr in receiverReports)
            {
                rr.WriteDataToBuffer(buffer);
            }

            // Update Header
            Header.ItemCount = receiverReports.Count;
        }
        
        
        #endregion Public
        
        #endregion Methods
    }

    
    /// <summary>
    /// -------------------------------------------------------------------------------------------
    /// SrPacket class purpose:
    /// -------------------------------------------------------------------------------------------
    /// A SenderReportPacket is used to send statistics information out on the network so that 
    /// reporting applications can compare what each RtpSender is putting to the wire to what 
    /// RtpListeners are receiving (reported via ReceiverReportPacket(s) )
    /// 
    /// -------------------------------------------------------------------------------------------
    /// SenderReportPacket description:
    /// -------------------------------------------------------------------------------------------
    /// 0                   1                   2                   3
    /// 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+--------------------------
    /// |V=2|P|    RC   |   PT=SR=200   |             length            |   Header
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+--------------------------
    /// |                         SSRC of sender                        |   Sender SSRC
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+--------------------------
    /// |              NTP timestamp, most significant word             |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    /// |             NTP timestamp, least significant word             |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   Sender Report Block
    /// |                         RTP timestamp                         |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    /// |                     sender's packet count                     |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    /// |                      sender's octet count                     |
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+--------------------------
    /// |                 SSRC_1 (SSRC of first source)                 |
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+   Receiver Report Blocks
    /// :                               ...                             :
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+--------------------------
    /// |                  profile-specific extensions                  |   Extension
    /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+--------------------------
    /// 
    /// -------------------------------------------------------------------------------------------
    /// Comments:
    /// -------------------------------------------------------------------------------------------
    /// According to Colin's book, pg 108, paragraph 1 - Receiver report blocks are present when 
    /// the sender is also a receiver.  In our code, this is not the case, but we should be capable
    /// of parsing packets that are.
    /// </summary>
    public class SrPacket : RtcpPacket
    {
        #region Members

        private SenderReport sr = null;
        
        // Receiver reports, in case this sender is also a receiver
        private ArrayList receiverReports = null;

        #endregion Members
        
        #region Constructors
        
        public SrPacket() : base(Rtcp.PacketType.SR) {}
        
        public SrPacket(uint ssrc) : base(Rtcp.PacketType.SR)
        {
            SSRC = ssrc;
        }
        
        public SrPacket(uint ssrc, SenderReport sr) : base(Rtcp.PacketType.SR)
        {
            if(sr == null)
            {
                throw new RtcpPacketException(Strings.NeedARealSenderReport);
            }

            SSRC = ssrc;
            this.sr = sr;
        }

        /// <summary>
        /// Construct a SenderReport packet from an existing RtcpPacket
        /// </summary>
        /// <param name="packet">Packet to process</param>
        public SrPacket(RtcpPacket packet) : base(Rtcp.PacketType.SR)
        {
            sr = new SenderReport();
            ProcessPacket(packet);
        }

        
        #endregion

        #region Methods
        
        #region Public
        
        public override int Size
        {
            get
            {
                int size = RtcpHeader.SIZE + Rtp.SSRC_SIZE + SenderReport.SIZE;

                if(receiverReports != null)
                {
                    size += receiverReports.Count * ReceiverReport.SIZE;
                }

                return size + GetNeededPadding(size);
            }
        }


        /// <summary>
        /// setSenderReport synchronizes the SenderReport to the buffer
        /// </summary>
        public SenderReport SenderReport
        {
            get{return sr;}
            set{sr = value;}
        }

        
        public ArrayList ReceiverReports
        {
            get{return receiverReports;}
        }

        
        /// <summary>
        /// Add a ReceiverReport entry to the ReceiverReportPacket
        /// </summary>
        /// <param name="rr"></param>
        public void AddReceiverReport(ReceiverReport rr)
        {
            ValidateItemSpace(receiverReports.Count);

            receiverReports.Add(rr);
        }

        
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "SenderReportPacket [ SSRC := {0} PacketType := {1} " +
                "ItemCount := {2} SenderReport := {3} ]", this.SSRC, PacketType, this.ItemCount, 
                this.SenderReport.ToString());
        }
        

        public override void ReadDataFromBuffer(BufferChunk buffer)
        {
            // Make sure ItemCount and Data length agree
            if(Header.Length * 4 != Rtp.SSRC_SIZE + SenderReport.SIZE + 
                ReceiverReport.SIZE * Header.ItemCount)
            {
                Debug.Assert(false, "Header length and item count disagree!");
                throw new RtcpPacketException(Strings.HeaderLengthAndItemCountDisagree);
            }

            // Read ssrc
            SSRC = buffer.NextUInt32();

            // Parse SenderReport
            sr.ReadDataFromBuffer(buffer);

            // Process Reports
            if(Header.ItemCount > 0)
            {
                receiverReports = new ArrayList();

                for(int rrCount = 0; rrCount < Header.ItemCount; rrCount++)
                {
                    ReceiverReport rr = new ReceiverReport();
                    rr.ReadDataFromBuffer(buffer);

                    receiverReports.Add(rr);
                }
            }
        }

        public override void WriteDataToBuffer(BufferChunk buffer)
        {
            // Add the SSRC
            buffer += SSRC;

            // Add the Sender Report
            sr.WriteDataToBuffer(buffer);

            // Add the receiver reports
            int reportCount = 0;

            if(receiverReports != null)
            {
                foreach(ReceiverReport rr in receiverReports)
                {
                    rr.WriteDataToBuffer(buffer);
                }

                reportCount = receiverReports.Count;
            }

            // Update Header
            Header.ItemCount = reportCount;
        }
        
        
        #endregion Public
        
        #endregion Methods
    }
}
