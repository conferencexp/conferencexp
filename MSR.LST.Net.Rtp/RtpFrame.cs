using System;
using System.Runtime.InteropServices;


namespace MSR.LST.Net.Rtp
{
    /// <summary>
    /// A collection of packets representing a frame
    /// 
    /// From the sending side, a BufferChunk "frame" is submitted and "packetized"
    /// From the receiving side, packets are submitted and "framed" when complete
    /// </summary>
    internal class RtpFrame
    {
        #region Statics
        
        // The growth of the frame is limited to ~= 48MB
        //
        // 1 packet (PoolInitialSize) Doubled (PoolGrowthFactor) 15 times (PoolMaxGrows)
        // 1 * 2^15 = 32768 packets * ~1.5 K per buffer ~= 48MB of data.
        
        /// <summary>
        /// Initial number of pre-allocated packets for sending data
        /// </summary>
        private const int PoolInitialSizeC = 1;

        /// <summary>
        /// Multiplier - amount of packets to grow the frame by
        /// </summary>
        private const int PoolGrowthFactorC = 2;

        /// <summary>
        /// How many times do we want the frame to grow by GrowthFactor before we consider
        /// the situation to be out of control (unbounded)
        /// </summary>
        private const int PoolGrowsMaxC = 15;

        #endregion

        #region Members

        private uint packetSize;
        private uint packetsInFrame;
        private uint lastPacketsInFrame = 0;
        private uint packetsInFramePool;
        private uint poolLength;
        private uint length;

        private RtpPacket[] pool;
        private ushort poolGrows;
        private ushort poolInitialSize = PoolInitialSizeC;
        private ushort poolGrowsMax = PoolGrowsMaxC;
        private ushort poolGrowthFactor = PoolGrowthFactorC;

        private PayloadType payloadType;
        private uint timeStamp;
        private uint streamSsrc;

        private ushort packetsReceived;
        private RtpListener.ReturnBufferHandler returnBufferHandler;

        private bool allowDuplicatePackets;

        #endregion Members

        #region Constructors

        /// <summary>
        /// Constructs a frame for Sending
        /// </summary>
        public RtpFrame(uint packetSize, PayloadType payloadType, uint timeStamp)
        {
            this.packetSize = packetSize;
            this.payloadType = payloadType;
            this.timeStamp = timeStamp;
        }

        
        
        /// <summary>
        /// Constructs a frame for Receiving data
        /// Defaults allowDuplicatePackets to false
        /// </summary>
        public RtpFrame(uint packetsInFrame, uint timeStamp, RtpListener.ReturnBufferHandler returnBufferHandler) :
            this(packetsInFrame, timeStamp, returnBufferHandler, false){}

        /// <summary>
        /// Constructs a frame for Receiving data
        /// </summary>
        public RtpFrame(uint packetsInFrame, uint timeStamp, RtpListener.ReturnBufferHandler returnBufferHandler, bool allowDuplicatePackets)
        {
            this.packetsInFrame = packetsInFrame;
            this.timeStamp = timeStamp;
            this.returnBufferHandler = returnBufferHandler;
            this.allowDuplicatePackets = allowDuplicatePackets;

            pool = new RtpPacket[packetsInFrame];
        }


        #endregion Constructors

        #region Public

        public BufferChunk Data
        {
            get
            {
                if(!Complete)
                {
                    throw new FrameIncompleteException();
                }

                BufferChunk data = new BufferChunk((int)length);

                for(int i = 0; i < packetsInFrame; i++)
                {
                    data += pool[i].Payload;
                }

                return data;
            }

            set
            {
                length = (uint)value.Length;
                Packetize(value);
            }
        }

        public void UnmanagedData(IntPtr[] ptrs, int[] lengths, bool prependLengths)
        {
            this.length = 0;

            foreach(int length in lengths)
            {
                // Zero length is OK, because it might be a placeholder ptr
                if(length < 0)
                    throw new ArgumentOutOfRangeException(Strings.LengthOfDataMustBePositive);

                this.length += (uint)(length);
                
                // Add 4 bytes for each length, since we will write the length to the frame, so the
                // data can be read back out...
                if(prependLengths)
                {
                    this.length += 4;
                }
            }

            Packetize(ptrs, lengths, prependLengths);
        }

        public RtpPacket this[int index]
        {
            get
            {
                if(index >= packetsInFrame)
                {
                    throw new IndexOutOfRangeException();
                }

                return pool[index];
            }

            set
            {
                // Validate the timestamp
                if(value.TimeStamp != timeStamp)
                {
                    throw new IncorrectTimestampException();
                }

                if(index >= packetsInFrame)
                {
                    throw new IndexOutOfRangeException();
                }

                if(pool[index] == null)
                {
                    packetsReceived++;
                    length += (uint)value.PayloadSize;
                    pool[index] = value;
                }
                else
                {
                    if(!allowDuplicatePackets)
                    {
                        throw new DuplicatePacketException();
                    }
                }
            }
        }

        public bool Complete
        {
            get{return packetsReceived == packetsInFrame;}
        }

        public void Dispose()
        {
            // Return packets back to where they came from
            for(int i = 0; i < packetsInFrame; i++)
            {
                RtpPacket packet = pool[i];

                // A packet could be null if we never received a packet for that location
                if(packet != null)
                {
                    if(returnBufferHandler != null)
                    {
                        returnBufferHandler(packet.ReleaseBuffer());
                    }

                    pool[i] = null;
                }
            }
        }

        public uint TimeStamp
        {
            set{timeStamp = value;}
        }

        public int PacketCount
        {
            get{return (int)packetsInFrame;}
        }

        public uint Ssrc
        {
            set
            {
                if( this.pool != null )
                {
                    foreach(RtpPacket packet in this.pool)
                    {
                        if( packet != null )
                            packet.SSRC = value;
                    }
                }

                this.streamSsrc = value;
            }
        }
        
        public uint Length
        {
            get{return length;}
        }
        public ushort PoolInitialSize
        {
            get{return poolInitialSize;}
            set{poolInitialSize = value;}
        }
        public ushort PoolGrowsMax
        {
            get{return poolGrowsMax;}
            set{poolGrowsMax = value;}
        }
        public ushort PoolGrowthFactor
        {
            get{return poolGrowthFactor;}
            set{poolGrowthFactor = value;}
        }

        
        #endregion Public

        #region Private

        /// <summary>
        /// Converts frame into packets
        /// 
        /// Note: This method does not currently take into account header extensions in the packet
        /// If we re-add support for optional headers, we would need to use the MaxPayloadSize
        /// property of each packet, and header extensions would have to be set before this method 
        /// is called. JVE 6/28/2004
        /// </summary>
        private void Packetize(BufferChunk frame)
        {
            // This assumes length member variable has been set already
            ApproximatePacketsInFrame();

            // We'll find out real number of packets as we build them
            packetsInFrame = 0;

            while(frame.Length > 0)
            {
                RtpPacket packet = GetNextPacket();

                // In the event that we re-add support for custom headers, add that data here 
                // before making the call to packet.MaxPayloadSize - 9/23/2004 JVE

                // Copy the MaxPayload amount of data
                packet.Payload = frame.NextBufferChunkMax(packet.MaxPayloadSize);
            }

            WritePacketsInFrame();
        }

        private void Packetize(IntPtr[] ptrs, int[] lengths, bool prependLengths)
        {
            // This assumes length member variable has been set already
            ApproximatePacketsInFrame();

            // Initialize to enter loop
            packetsInFrame = 0; // We'll find out real number of packets
            RtpPacket packet = null;
            int pktAvailable = 0;
            int ptrIndex = 0;
            int ptrAvailable = 0;
            IntPtr ptr = IntPtr.Zero;
            bool writePtrLength = false;
            int ptrsLength = ptrs.Length;

            while(true)
            {
                // If there is no data left in this ptr, get a new one
                if(ptrAvailable == 0)
                {
                    if(ptrIndex + 1 > ptrs.Length)
                    {
                        break; // while(true)
                    }

                    ptrAvailable = lengths[ptrIndex];
                    ptr = ptrs[ptrIndex];
                    ptrIndex++;

                    // Write the length of the ptr to the packet
                    if(prependLengths)
                    {
                        writePtrLength = true;

                        if(pktAvailable < 4)
                        {
                            pktAvailable = 0;  // Get a new packet
                        }
                    }
                }

                // Note:
                // pktAvailable == 0 is handled after ptrAvailable == 0 to bypass the case where
                // the final ptr fit into the packet perfectly.  We want to exit the loop at that
                // point before grabbing another packet which won't have anything put in it.  The
                // case for prepending the length of ptr still works if the previous packet didn't
                // have room for the length.

                // If there is no room in the current packet, get a new one
                if(pktAvailable == 0)
                {
                    packet = GetNextPacket();

                    // In the event that we re-add support for custom headers, add that data here 
                    // before making the call to packet.MaxPayloadSize - 9/23/2004 JVE

                    // Find out how much room is left in the packet
                    packet.Reset(); // Mimic packet.Payload = ...
                    pktAvailable = packet.MaxPayloadSize - packet.PayloadSize;
                }

                // Write length of ptr to packet
                if(writePtrLength)
                {
                    // Add 4 bytes for length
                    packet.AppendPayload(ptrAvailable);
                    pktAvailable -= 4;

                    writePtrLength = false;

                    // Place holder ptr
                    if(ptrAvailable == 0)
                    {
                        continue; // Nothing to copy
                    }
                }

                // Copy as much as the packet can hold
                if(ptrAvailable >= pktAvailable)
                {
                    packet.AppendPayload(ptr, pktAvailable);
                    // buffer.CopyFrom(ptr, pktAvailable);

                    ptr = (IntPtr)(ptr.ToInt32() + pktAvailable); // Advance pointer

                    ptrAvailable -= pktAvailable;
                    pktAvailable = 0;
                }
                else // Copy as much as the ptr can provide
                {
                    packet.AppendPayload(ptr, ptrAvailable);

                    pktAvailable -= ptrAvailable;
                    ptrAvailable = 0;
                }
            }

            WritePacketsInFrame();
        }


        /// <summary>
        /// Approximates how many packets would be in a frame of the given length.  This is a perf
        /// improvement if sending a large frame that would grow the pool multiple times
        /// 
        /// Note: This assumes length member variable has been set already
        /// </summary>
        /// <param name="length">length of frame</param>
        private void ApproximatePacketsInFrame()
        {
            // + 1 because the chances are pretty slim it will land exactly on the packet boundary
            // and it is better safe than sorry.
            uint packets = (length / packetSize) + 1;

            // Grow pool if necessary
            if(packets > poolLength)
            {
                GrowPool(packets);
            }
        }

        private void GrowPool(uint packets)
        {
            // Use a local so that we don't change the member variable until everything is committed
            uint localPoolLength = poolLength;

            // Startup condition
            if(localPoolLength == 0)
            {
                localPoolLength = poolInitialSize;
            }

            // Determine amount of growth necessary
            while(packets > localPoolLength)
            {
                poolGrows++;

                if(poolGrows > poolGrowsMax)
                {
                    throw new FrameTooLargeException();
                }

                localPoolLength *= poolGrowthFactor;
            }

            // Copy old packets
            RtpPacket[] clone = new RtpPacket[localPoolLength];

            for(int i = 0; i < packetsInFramePool; i++)
            {
                clone[i] = pool[i];
            }

            pool = clone;
            poolLength = localPoolLength;
        }

        private RtpPacket GetNextPacket()
        {
            packetsInFrame++;

            // Grow pool if necessary
            if(packetsInFrame > poolLength)
            {
                GrowPool(packetsInFrame);
            }

            RtpPacket packet = GetPacket((int)packetsInFrame - 1);
            packet.TimeStamp = timeStamp;

            return packet;
        }

        private RtpPacket GetPacket(int index)
        {
            RtpPacket rtpPacket = pool[index];

            if(rtpPacket == null)
            {
                rtpPacket = new RtpPacket((int)packetSize);
                rtpPacket.PayloadType = payloadType;
                rtpPacket.FrameIndex = (ushort)(index);
                rtpPacket.SSRC = streamSsrc;

                pool[index] = rtpPacket;

                packetsInFramePool = (uint)index + 1;
            }

            return rtpPacket;
        }


        private void WritePacketsInFrame()
        {
            if(packetsInFrame != lastPacketsInFrame)
            {
                // Assign the number of packets in a frame
                for(int i = 0; i < packetsInFrame; i++)
                {
                    pool[i].PacketsInFrame = (ushort)packetsInFrame;
                }

                lastPacketsInFrame = packetsInFrame;
            }
        }

        
        #endregion Private
    }
}
