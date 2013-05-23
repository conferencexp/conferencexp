using System;
using System.Globalization;
using System.Runtime.InteropServices;

using MSR.LST.Net;


namespace MSR.LST
{
    /// <summary>
    /// Overview:
    /// -------------------------------------------------------------------------------------------
    /// BufferChunk is a helper class created to make network calls in DotNet easier by allowing
    /// byte[] to be passed around along with an index (or offset) and length (or chunksize).  As
    /// it was observed that many functions in this code as well as in System.Net.Sockets commonly
    /// used the parameters (byte[], index, length).
    /// 
    /// When combined with MSR.LST.Net.Sockets.Socket, you get a fully functional Socket class that
    /// accepts a BufferChunk in the Send and Receive commands.
    /// 
    /// 
    /// Members:
    /// -------------------------------------------------------------------------------------------
    /// index  - offset inside the buffer where valid data starts
    /// length - amount of valid data
    /// buffer - byte[] containing the data
    /// 
    /// Except for constructors (which set index and length member variables), when index and
    /// length are passed as parameters, they are used as offsets into the valid data, not offsets
    /// into the buffer.
    /// 
    /// 
    /// Object State:
    /// -------------------------------------------------------------------------------------------
    /// BufferChunk does not accept or return null or zero-length objects.  However, it is valid
    /// for a BufferChunk to be in a state where it has no data to manipulate i.e. length == 0
    /// 
    /// this.index + this.length cannot be > buffer.Length
    /// index + length cannot be > this.length when manipulating inside the valid data
    /// index must be >= 0
    /// length must be >= 0
    /// 
    /// Integral types:
    /// -------------------------------------------------------------------------------------------
    /// BufferChunk allows the reading and writing of integral types (Int16, Int32, Int64 and the
    /// unsigned counterparts) into the byte[].  Because this class was written to help get data
    /// onto and off of the wire, and because the Rtp spec says that integral types should be sent
    /// in BigEndian byte order, we go ahead and do that conversion here.  It is just as fast as
    /// not doing the conversion because of how we write the data (using shifts).
    /// </summary>

    [ComVisible(false)]
    public class BufferChunk: IDisposable, ICloneable
    {
        #region Statics

        private static bool littleEndian;

        static BufferChunk()
        {
            littleEndian = BitConverter.IsLittleEndian;
        }


        // Pri1: Remove Public properties (fix rtcpPacket and rtpPacket)
        // Pri1: Remove Length.set method (fix rtcpPacket)
        // Pri2: Add GetHashCode
        // Pri2: I still don't buy the need for this class to be disposed - no expensive resources
        //       and it doesn't do any of the proper dispose checks, nor can it be resurrected - JVE
        // Pri3: Befriend DotNet Framework by overriding ToString et al -- usable in Exception messages?
        // Pri3: Finish comments and examples

        public static bool Compare(byte[] obj1, byte[] obj2)
        {
            bool ret = false;

            if(obj1 == null || obj2 == null)
            {
                if(obj1 == obj2)
                {
                    ret = true;
                }
            }
            else if(obj1.Length == obj2.Length)
            {
                int i = 0;

                for(; i < obj1.Length; i++)
                {
                    if(obj1[i] != obj2[i])
                    {
                        break;
                    }
                }

                if(i == obj1.Length)
                {
                    ret = true;
                }
            }
            
            return ret;
        }


        public static byte[] Copy(byte[] source)
        {
            byte[] ret = null;

            if(source != null)
            {
                ret = new byte[source.Length];
                Array.Copy(source, 0, ret, 0, source.Length);
            }

            return ret;
        }

        
        #endregion Statics
        #region Private Properties
        
        /// <summary>
        /// For doing conversions with strings
        /// </summary>
        private static System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();

        /// <summary>
        /// Data storage
        /// </summary>
        private byte[] buffer = null;

        /// <summary>
        /// Offset where the valid data starts
        /// </summary>
        private int index = 0;

        /// <summary>
        /// Length of the valid data
        /// </summary>
        private int length = 0;

        /// <summary>
        /// Flag indicating whether the object is disposed or not
        /// </summary>
        private bool disposed = false;

        #endregion
        #region Public Properties
        /// <summary>
        /// Index points to the start of the valid data area
        /// 
        /// Note: This property may be removed going forward
        /// </summary>
        /// <example>
        /// using MSR.LST;
        /// 
        /// public int SendTo(BufferChunk bufferChunk, EndPoint endPoint)
        /// {
        ///     return SendTo(bufferChunk.Buffer, bufferChunk.Index, bufferChunk.Length,
        ///                   SocketFlags.None, endPoint);
        /// }
        /// </example>
        public int Index
        {
            get
            {
                return index;
            }
        }
        /// <summary>
        /// Length is amount of valid data in the buffer
        /// 
        /// Length should not be directly manipulated to select smaller sections of the BufferChunk
        /// because this would abandon valid data.  Instead, you should use the method 
        /// <see cref="BufferChunk.Peek"/>  to create a shallow copy new BufferChunk pointing to 
        /// just the section you want.
        /// 
        /// Note: This property may be removed going forward
        /// </summary>
        /// <example>
        /// using MSR.LST;
        /// 
        /// public int SendTo(BufferChunk bufferChunk, EndPoint endPoint)
        /// {
        ///     return SendTo(bufferChunk.Buffer, bufferChunk.Index, bufferChunk.Length,
        ///                   SocketFlags.None, endPoint);
        /// }
        /// </example>
        public int Length
        {
            get
            {
                return length;
            }

            //Pri1: Remove this method once the rewrites of Rtcp are completed
            set
            {
                ValidateNonNegative(value);
                ValidatePointerData(index, value, buffer.Length);

                length = value;
            }
        }
        /// <summary>
        /// Buffer gives you direct access to the byte[] which is storing the raw data of the 
        /// BufferChunk.  Buffer is simply a byte[] that is passed ByRef so you have easy and
        /// efficient access to the basic data.
        /// 
        /// Note: This property may be removed going forward
        /// </summary>
        /// <example>
        /// using MSR.LST;
        /// 
        /// public int SendTo(BufferChunk bufferChunk, EndPoint endPoint)
        /// {
        ///     return SendTo(bufferChunk.Buffer, bufferChunk.Index, bufferChunk.Length,
        ///                   SocketFlags.None, endPoint);
        /// }
        /// </example>
        public byte[] Buffer
        {
            get
            {
                return buffer;
            }
        }
        #endregion
        #region Constructors
        /// <summary>
        /// Constructor, create a new BufferChunk and allocate a new byte[] to hold the data.
        /// </summary>
        /// <param name="size">int size of the new byte[] to create, must be >= 1</param>
        /// <example>
        /// BufferChunk bufferChunk = new BufferChunk(2000);
        /// </example>
        public BufferChunk(int size)
        {
            ValidateNonNegative(size);
            ValidateNotZeroLength(size);

            buffer = new byte[size];
            length = 0;
        }
        /// <summary>
        /// Constructor, create a BufferChunk using an existing byte[] without performing a memcopy
        /// </summary>
        /// <param name="buffer">byte[] to be used as the data store for the BufferChunk,
        ///                      cannot be null or zero length</param>
        /// <example>
        /// byte[] buffer = new byte[2000];
        /// BufferChunk bufferChunk = new BufferChunk(buffer);
        /// </example>
        public BufferChunk(byte[] buffer)
        {
            ValidateObject(buffer);
            ValidateNotZeroLength(buffer.Length);

            this.buffer = buffer;
            length = buffer.Length;
        }
        /// <summary>
        /// Constructor, create a BufferChunk from its constituent parts
        /// </summary>
        /// <param name="buffer">byte[] to be used as the data store for the BufferChunk</param>
        /// <param name="index">offset at which the valid data starts</param>
        /// <param name="length">amount of 'valid data'</param>
        /// <example>
        /// byte[] buffer = new byte[2000];
        /// BufferChunk bufferChunk = new BufferChunk(buffer, 10, 200);
        /// </example>
        public BufferChunk(byte[] buffer, int index, int length)
        {
            ValidateObject(buffer);
            ValidateNotZeroLength(buffer.Length);
            ValidateNonNegative(index);
            ValidateNonNegative(length);
            ValidatePointerData(index, length, buffer.Length);

            this.buffer = buffer;
            this.index = index;
            this.length = length;
        }
        #endregion
        #region Private Methods
        // How much space the buffer has left (for operator+)
        private int AvailableBuffer
        {
            get
            {
                return buffer.Length - index - length;
            }
        }
        
        #region Validate Methods

        // BufferChunk does not accept or create null objects
        private static void ValidateObject(object o)
        {
            if(o == null)
            {
                throw new ArgumentNullException(Strings.BufferChunkDoesNotAcceptNull);
            }
        }

        // BufferChunk does not accept or create objects of zero length
        private static void ValidateNotZeroLength(int length)
        {
            if(length == 0)
            {
                throw new NoDataException(Strings.BufferChunkDoesNotAcceptZeroLength);
            }
        }

        // Index and Length must be >= 0
        private static void ValidateNonNegative(int val)
        {
            if(val < 0)
            {
                throw new ArgumentOutOfRangeException("val", val, Strings.AllIntegerValuesMustBePositive);
            }
        }

        // When setting pointers (Index and Length), make sure they fall within the valid
        // data area - buffer.Length or this.length (valid data)
        private static void ValidatePointerData(int index, int length, int dataLength)
        {
            if(index + length > dataLength)
            {
                throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, 
                    Strings.IndexAndLengthInvalidData, index, length, dataLength));
            }
        }

        // Make sure we have as much data as they are requesting
        private static void ValidateSufficientData(int requested, int actual)
        {
            if(requested > actual)
            {
                throw new InsufficientDataException(string.Format(CultureInfo.CurrentCulture, 
                    Strings.BufferChunkDoesNotHaveEnoughData, requested, actual));
            }
        }

        // Make sure we have as much space as they are requesting
        private static void ValidateSufficientSpace(int requested, int actual)
        {
            if(requested > actual)
            {
                throw new InsufficientSpaceException(string.Format(CultureInfo.CurrentCulture, 
                    Strings.BufferChunkDoesNotHaveEnoughBuffer, requested, actual));
            }
        }

        // Make sure it is not a null pointer
        private static void ValidateIntPtr(IntPtr ptr)
        {
            if(ptr == IntPtr.Zero)
            {
                throw new ArgumentException(Strings.NullPointersAreInvalid);
            }
        }


        #endregion

        #endregion
        #region Operators & Casts (to/from byte[] & string)
        /// <summary>
        /// Explicitly cast the valid data into a new byte[]. This function creates a copy of the 
        /// BufferChunk data and omits the bytes before the Index and after the Length from the 
        /// byte[] copy.  This is a simple way to interoperate BufferChunks with functions that 
        /// only know how to deal with byte[].
        /// </summary>
        /// <param name="source">BufferChunk</param>
        /// <returns>byte[] containing the valid data from the BufferChunk</returns>
        /// <example>
        /// using MSR.LST;
        /// using System.Net.Sockets;
        /// 
        /// Socket socket = new Socket(...);  // This standard socket only knows byte[]
        /// BufferChunk bufferChunk = new bufferChunk(500); // Create a new BufferChunk containing a 500 byte buffer
        ///
        /// socket.Send((byte[])bufferChunk, SocketFlags.None);    //Note the explicit cast from BufferChunk to byte[]
        /// </example>
        public static explicit operator byte[](BufferChunk source)
        {
            ValidateObject(source);
            ValidateNotZeroLength(source.length);

            byte[] returnBuffer = new byte[source.length];
            Array.Copy(source.buffer, source.index, returnBuffer, 0, source.length);
            return(returnBuffer);
        }
        /// <summary>
        /// Explicitly cast the valid data to a string.  Helpful for applications that want to send
        /// strings or XML over the network without worrying about the String to UTF8 logic.
        /// </summary>
        /// <param name="source">BufferChunk containing the data</param>
        /// <returns>string form of data</returns>
        /// <example>
        /// using MSR.LST;
        /// 
        /// BufferChunk bc = new BufferChunk(new byte[] {74, 97, 115, 111, 110});
        /// if((string)bc == "Jason")...
        /// </example>
        public static explicit operator string (BufferChunk source)
        {
            ValidateObject(source);
            ValidateNotZeroLength(source.length);

            lock(utf8)
            {
                return utf8.GetString((byte[])source);
            }
        }
        /// <summary>
        /// Explicitly cast a string to a BufferChunk.  Helpful for applications that want to send strings or XML over the network without worrying about the String to UTF8 logic.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static explicit operator BufferChunk (string source)
        {
            ValidateObject(source);
            ValidateNotZeroLength(source.Length);

            lock(utf8)
            {
                return new BufferChunk(utf8.GetBytes(source));
            }
        }
        /// <summary>
        /// Explicitly cast a byte[] into a BufferChunk.  Useful when you want to start acting upon a byte[] in an incremental
        /// fashion by taking advantage of the functionality a BufferChunk provides over a byte[].  For instance, this is useful for
        /// taking a large (say 500k) dataset and dividing it up into smaller (say 1.5k) chunks.
        /// 
        /// This is functionally equivalent to <c>new BufferChunk(buffer)</c>
        /// </summary>
        /// <param name="buffer">byte[] buffer containing valid data</param>
        /// <returns>BufferChunk</returns>
        public static explicit operator BufferChunk(byte[] buffer)
        {
            // Let the constructor do the checking for us
            return new BufferChunk(buffer);
        }
        /// <summary>
        /// Override + and += operator to allow appending of buffers, provided there is room in the left-most BufferChunk
        /// </summary>
        /// <param name="destination">BufferChunk destination that will be appended to</param>
        /// <param name="source">BufferChunk source</param>
        /// <returns>Reference to BufferChunk destination</returns>
        public static BufferChunk operator+ (BufferChunk destination, BufferChunk source)
        {
            ValidateObject(source);
            ValidateNotZeroLength(source.length);
            ValidateSufficientSpace(source.length, destination.AvailableBuffer);

            Array.Copy(source.buffer, source.index, destination.buffer, destination.index + destination.length, source.length);
            destination.length += source.length;

            return destination;
        }
        public static BufferChunk operator+ (BufferChunk destination, byte[] source)
        {
            ValidateObject(source);
            ValidateNotZeroLength(source.Length);
            ValidateSufficientSpace(source.Length, destination.AvailableBuffer);

            Array.Copy(source, 0, destination.buffer, destination.index + destination.length, source.Length);
            destination.length += source.Length;

            return destination;
        }
        public static BufferChunk operator+ (BufferChunk destination, byte b)
        {
            ValidateSufficientSpace(1, destination.AvailableBuffer);

            destination[destination.length++] = b;

            return destination;
        }


        public static BufferChunk operator+ (BufferChunk destination, Int16 data)
        {
            ValidateSufficientSpace(2, destination.AvailableBuffer);

            // Advance the length 2, and set the data
            destination.length += 2;
            destination._SetInt16(destination.length - 2, data);

            return destination;
        }
        public static BufferChunk operator+ (BufferChunk destination, Int32 data)
        {
            ValidateSufficientSpace(4, destination.AvailableBuffer);

            // Advance the length 4, and set the data
            destination.length += 4;
            destination._SetInt32(destination.length - 4, data);

            return destination;
        }
        public static BufferChunk operator+ (BufferChunk destination, Int64 data)
        {
            return destination += (UInt64)data;
        }
        public static BufferChunk operator+ (BufferChunk destination, UInt16 data)
        {
            return destination += (Int16)data;;
        }
        public static BufferChunk operator+ (BufferChunk destination, UInt32 data)
        {
            return destination += (Int32)data;
        }
        public static BufferChunk operator+ (BufferChunk destination, UInt64 data)
        {
            ValidateSufficientSpace(8, destination.AvailableBuffer);

            // Advance the length 8, and set the data
            destination.length += 8;
            destination ._SetUInt64(destination.length - 8, data);

            return destination;
        }


        public static BufferChunk operator+ (BufferChunk destination, string s)
        {
            ValidateObject(s);
            ValidateNotZeroLength(s.Length);

            byte[] bytes;

            lock(utf8)
            {
                bytes = utf8.GetBytes(s);
            }

            ValidateSufficientSpace(bytes.Length, destination.AvailableBuffer);

            return destination += (BufferChunk)bytes;
        }
        #endregion
        #region Indexer
        /// <summary>
        /// Indexer used to allow us to treat a BufferChunk like a byte[].  Useful when making in place modifications or reads from a BufferChunk.
        /// </summary>
        public byte this [int index]
        {
            get
            {
                ValidateNonNegative(index);
                ValidateNotZeroLength(length);
                ValidateSufficientData(1, length - index);
                
                return buffer[this.index + index];
            }
            set
            {
                ValidateNonNegative(index);
                ValidateSufficientSpace(1, length - index);

                buffer[this.index + index] = value;
            }
        }
        #endregion
        #region IClonable implementation
        /// <summary>
        /// Creates a shallow copy (new Index and Length, duplicate reference to the same Buffer) of a BufferChunk.
        /// </summary>
        /// <returns>BufferChunk instance with ref Buffer, ByVal Index, and ByVal Length</returns>
        public object Clone()
        {
            return new BufferChunk(buffer, index, length);
        }
        #endregion
        #region IDisposable implementation
        /// <summary>
        /// Disposes the internal state of the object
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                buffer = null;
                disposed = true;
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Copy the valid data section of 'this' to the destination BufferChunk
        /// overwriting dest's previous contents
        /// This method does not allow dest's valid data section to grow or shrink
        /// (i.e. treat valid data as a fixed buffer)
        /// </summary>
        /// <param name="destination">BufferChunk</param>
        /// <param name="index">offset in the destination BufferChunk's valid data</param>
        public void CopyTo(BufferChunk destination, int index)
        {
            ValidateObject(destination);
            ValidateNonNegative(index);
            ValidateNotZeroLength(this.length);
            ValidateSufficientSpace(this.length, destination.length - index);

            Array.Copy(this.buffer, this.index, destination.buffer, destination.index + index, this.length);
        }

        public void CopyTo(IntPtr dest, Int32 length)
        {
            ValidateIntPtr(dest);
            ValidateNonNegative(length);
            ValidateNotZeroLength(length);
            ValidateSufficientData(length, this.length);

            Marshal.Copy(buffer, this.index, dest, length);
            
            this.length -= length;
            this.index += length;
        }

        public void CopyFrom(IntPtr src, Int32 length)
        {
            ValidateIntPtr(src);
            ValidateNonNegative(length);
            ValidateNotZeroLength(length);
            ValidateSufficientSpace(length, AvailableBuffer);

            Marshal.Copy(src, buffer, this.index + this.length, length);
            this.length += length;
        }

        /// <summary>
        /// Reset the BufferChunk's Index and Length pointers to zero so it is ready for reuse as an empty BufferChunk.
        /// Note that the actual byte[] buffer is not reset, so the memory is not deallocated/reallocated, allowing for
        /// more efficient reuse of memory without abusing the GC
        /// </summary>
        public void Reset()
        {
            index = 0;
            length = 0;
        }

        /// <summary>
        /// Used to zero out the data area of the BufferChunk.
        /// </summary>
        public void Clear()
        {
            for (int i = index; i < index + length; i++)
                buffer[i] = 0;
        }

        /// <summary>
        /// Reset the BufferChunk's Index and Length pointers to supplied values
        /// </summary>
        public void Reset(int index, int length)
        {
            ValidateNonNegative(index);
            ValidateNonNegative(length);
            ValidatePointerData(index, length, buffer.Length);
            
            this.index = index;
            this.length = length;
        }

        /// <summary>
        /// Create a return BufferChunk containing a subset of the data from the valid data.
        /// </summary>
        /// <param name="index">int index into the valid data area</param>
        /// <param name="length">int length of the data to copy</param>
        /// <returns>BufferChunk length Length that was extracted from the source BufferChunk</returns>
        public BufferChunk Peek(int index, int length)
        {
            return new BufferChunk(this.buffer, this.index + index, length);
        }

        
        // All of the Next* methods, retrieve data starting at index, and then advance index
        #region Next*


        /// <summary>
        /// Returns a BufferChunk consisting of the next 'length' bytes of the BufferChunk instance.
        /// Automatically increments Index and decrements Length.
        /// This function is useful for iterative functions that parse through a large BufferChunk returning smaller BufferChunks
        /// </summary>
        /// <param name="length">int</param>
        /// <returns>BufferChunk</returns>
        /// <example>
        /// 
        ///     ...
        ///     frameBuffer = new BufferChunk(500000);
        ///     ...
        /// 
        ///     int packetsInFrame = (ushort)((frameBuffer.Length + RtpHeaderExtensionSize) / (MaximumPacketPayload));
        ///     if (((frameBuffer.Length + RtpHeaderExtensionSize) % (MaximumPacketPayload)) > 0)
        ///         packetsInFrame++;
        ///
        ///     for (int i = 0; i &lt; packetsInFrame; i++)
        ///     {
        ///         int sizeToCopy = (frameBuffer.Length &lt; MaximumPacketPayload) ? frameBuffer.Length : MaximumPacketPayload;
        ///         socket.Send((byte[])frameBuffer.NextBufferChunk(sizeToCopy));
        ///     }
        /// </example>
        public BufferChunk NextBufferChunk(int length)
        {
            // Peek will validate for us
            BufferChunk retBC = Peek(0, length);

            this.length -= length;
            this.index += length;

            return retBC;
        }

        /// <summary>
        /// Returns the requested amount of data, or whatever remains if length > this.length
        /// </summary>
        public BufferChunk NextBufferChunkMax(int length)
        {
            if(length > this.Length)
            {
                length = this.Length;
            }

            return NextBufferChunk(length);
        }

        public byte NextByte()
        {
            // Let GetByte do the checking
            byte ret = GetByte(0);

            length--;
            index++;

            return ret;
        }

        
        public Int16 NextInt16()
        {
            // Let GetShort do the checking
            Int16 ret = GetInt16(0);

            length -= 2;
            index += 2;

            return ret;
        }

        public Int32 NextInt32()
        {
            // Let GetInt do the checking
            Int32 ret = GetInt32(0);

            length -= 4;
            index += 4;

            return ret;
        }

        public Int64 NextInt64()
        {
            // Let GetInt do the checking
            Int64 ret = GetInt64(0);

            length -= 8;
            index += 8;

            return ret;
        }
        
        public UInt16 NextUInt16()
        {
            return (UInt16)NextInt16();
        }

        public UInt32 NextUInt32()
        {
            return (UInt32)NextInt32();
        }

        public UInt64 NextUInt64()
        {
            return (UInt64)NextInt64();
        }

        
        public string NextUtf8String(int length)
        {
            // Let GetUTFString do the checking
            string ret = GetUTF8String(0, length);

            this.length -= length;
            index += length;

            return ret;
        }

        
        #endregion Next*

        // All of the Get* methods retrieve data starting at index
        #region Get*

        /// <summary>
        /// Retrieves 1 byte from inside the BufferChunk
        /// This method is included for consistency.  It simply forwards to the indexer.
        /// </summary>
        /// <param name="index">Offset into the valid data</param>
        public byte GetByte(int index)
        {
            // Let the indexer do the checking
            return this[index];
        }


        /// <summary>
        /// Retrieves 2 bytes inside the BufferChunk
        /// </summary>
        /// <param name="index">Offset into the valid data</param>
        public Int16 GetInt16(int index)
        {
            ValidateNonNegative(index);
            ValidateNotZeroLength(this.length - index);
            ValidateSufficientData(2, this.length - index);

            return _GetInt16(index);
        }

        /// <summary>
        /// Retrieves 4 bytes inside the BufferChunk
        /// </summary>
        /// <param name="index">Offset into the valid data</param>
        public int GetInt32(int index)
        {
            ValidateNonNegative(index);
            ValidateNotZeroLength(this.length - index);
            ValidateSufficientData(4, this.length - index);

            return _GetInt32(index);
        }

        /// <summary>
        /// Retrieves 8 bytes inside the BufferChunk
        /// </summary>
        /// <param name="index">Offset into the valid data</param>
        public long GetInt64(int index)
        {
            return (Int64)GetUInt64(index);
        }
        

        /// <summary>
        /// Retrieves 2 bytes inside the BufferChunk
        /// </summary>
        /// <param name="index">Offset into the valid data</param>
        public UInt16 GetUInt16(int index)
        {
            return (UInt16)GetInt16(index);
        }

        /// <summary>
        /// Retrieves 4 bytes inside the BufferChunk
        /// </summary>
        /// <param name="index">Offset into the valid data</param>
        public UInt32 GetUInt32(int index)
        {
            return (UInt32)GetInt32(index);
        }

        /// <summary>
        /// Retrieves 8 bytes inside the BufferChunk
        /// </summary>
        /// <param name="index">Offset into the valid data</param>
        public UInt64 GetUInt64(int index)
        {
            ValidateNonNegative(index);
            ValidateNotZeroLength(this.length - index);
            ValidateSufficientData(8, this.length - index);

            return _GetUInt64(index);
        }
        
        
        public UInt16 GetPaddedUInt16(int index)
        {
            ValidateNonNegative(index);
            ValidateSufficientData(1, length - index); // We need at least 1 byte

            UInt16 ret = 0;
            
            int dataSize = 2;
            if(index + dataSize <= length) // All the data requested
            {
                ret = (UInt16)_GetInt16(index);
            }
            else // The rest of the data in the buffer
            {
                int offset = 0;
                while(index + offset < length)
                {
                    int shift;

                    if(littleEndian)
                    {
                        shift = dataSize - offset - 1;
                    }
                    else
                    {
                        shift = offset;
                    }

                    ret += (UInt16)(buffer[this.index + index + offset] << shift * 8);
                    offset++;
                }
            }

            return ret;
        }
        
        public UInt32 GetPaddedUInt32(int index)
        {
            ValidateNonNegative(index);
            ValidateSufficientData(1, length - index);

            UInt32 ret = 0;
            
            int dataSize = 4;
            if(index + dataSize < length) // All the data requested
            {
                ret = (UInt32)_GetInt32(index);
            }
            else // The rest of the data in the buffer
            {
                int offset = 0;
                while(index + offset < length)
                {
                    int shift;

                    if(littleEndian)
                    {
                        shift = dataSize - offset - 1;
                    }
                    else
                    {
                        shift = offset;
                    }

                    ret += ((UInt32)buffer[this.index + index + offset]) << shift * 8;
                    offset++;
                }
            }

            return ret;
        }

        public UInt64 GetPaddedUInt64(int index)
        {
            int dataSize = 8;
            int availableData = length - index;
            
            ValidateNonNegative(index);
            ValidateSufficientData(1, availableData);

            UInt64 ret = 0;
            
            if(dataSize < availableData) // All the data requested
            {
                ret = _GetUInt64(index);
            }
            else // The rest of the data in the buffer
            {
                int indexOffset = this.index + index;

                if(littleEndian)
                {
                    for(int offset = 0, shift = dataSize - 1; offset < availableData; offset++, shift--)
                    {
                        ret += ((UInt64)buffer[indexOffset + offset]) << shift * 8;
                    }
                }
                else
                {
                    for(int offset = 0, shift = 0; offset < availableData; offset++, shift++)
                    {
                        ret += ((UInt64)buffer[indexOffset + offset]) << shift * 8;
                    }
                }
            }

            return ret;
        }


        /// <summary>
        /// Retrieves length bytes from inside the BufferChunk and converts from UTF8 string
        /// </summary>
        /// <param name="index">Offset into the valid data</param>
        public string GetUTF8String(int index, int length)
        {
            ValidateNonNegative(index);
            ValidateNonNegative(length);
            ValidateNotZeroLength(length);
            ValidateNotZeroLength(this.length - index);
            ValidateSufficientData(length, this.length - index);

            lock(utf8)
            {
                return utf8.GetString(buffer, this.index + index, length);
            }
        }

        
        private Int16 _GetInt16(int index)
        {
            Int16 ret;
            
            // BigEndian network -> LittleEndian architecture
            if(littleEndian)
            {
                ret  = (Int16)(buffer[this.index + index + 0] << 1 * 8);
                ret += (Int16)(buffer[this.index + index + 1] << 0 * 8);
            }
            else // BigEndian network -> BigEndian architecture
            {
                ret  = (Int16)(buffer[this.index + index + 0] << 0 * 8);
                ret += (Int16)(buffer[this.index + index + 1] << 1 * 8);
            }

            return ret;
        }
        
        private Int32 _GetInt32(int index)
        {
            Int32 ret;
            
            // BigEndian network -> LittleEndian architecture
            if(littleEndian)
            {
                ret  = buffer[this.index + index + 0] << 3 * 8;
                ret += buffer[this.index + index + 1] << 2 * 8;
                ret += buffer[this.index + index + 2] << 1 * 8;
                ret += buffer[this.index + index + 3] << 0 * 8;
            }
            else // BigEndian network -> BigEndian architecture
            {
                ret  = buffer[this.index + index + 0] << 0 * 8;
                ret += buffer[this.index + index + 1] << 1 * 8;
                ret += buffer[this.index + index + 2] << 2 * 8;
                ret += buffer[this.index + index + 3] << 3 * 8;
            }

            return ret;
        }

        private unsafe UInt64 _GetUInt64(int index)
        {
            UInt64 ret;
            
            fixed(byte* pb = &buffer[this.index + index])
            {
                ret = *((UInt64*)pb);
            }

            return ret;
        }


        #endregion Get*
        
        // All of the Set* methods set data starting at index
        #region Set*

        /// <summary>
        /// Modifies 1 byte inside the BufferChunk
        /// This method is included for consistency.  It simply forwards to the indexer.
        /// </summary>
        /// <param name="index">Offset into the valid data</param>
        /// <param name="data">Value to write at index</param>
        public void SetByte(int index, byte data)
        {
            // Let the indexer do the checking
            this[index] = data;
        }


        /// <summary>
        /// Modifies 2 bytes inside the BufferChunk
        /// </summary>
        /// <param name="index">Offset into the valid data</param>
        /// <param name="data">Value to write at index</param>
        public void SetInt16(int index, Int16 data)
        {
            ValidateNonNegative(index);
            ValidateSufficientSpace(2, length - index);

            _SetInt16(index, data);
        }

        /// <summary>
        /// Modifies 4 bytes inside the BufferChunk
        /// </summary>
        /// <param name="index">Offset into the valid data</param>
        /// <param name="data">Value to write at index</param>
        public void SetInt32(int index, Int32 data)
        {
            ValidateNonNegative(index);
            ValidateSufficientSpace(4, length - index);

            _SetInt32(index, data);
        }

        /// <summary>
        /// Modifies 8 bytes inside the BufferChunk
        /// </summary>
        /// <param name="index">Offset into the valid data</param>
        /// <param name="data">Value to write at index</param>
        public void SetInt64(int index, Int64 data)
        {
            SetUInt64(index, (UInt64)data);
        }

        
        /// <summary>
        /// Modifies 2 bytes inside the BufferChunk
        /// </summary>
        /// <param name="index">Offset into the valid data</param>
        /// <param name="data">Value to write at index</param>
        public void SetUInt16(int index, UInt16 data)
        {
            SetInt16(index, (Int16)data);
        }

        /// <summary>
        /// Modifies 4 bytes inside the BufferChunk
        /// </summary>
        /// <param name="index">Offset into the valid data</param>
        /// <param name="data">Value to write at index</param>
        public void SetUInt32(int index, UInt32 data)
        {
            SetInt32(index, (Int32)data);
        }

        /// <summary>
        /// Modifies 8 bytes inside the BufferChunk
        /// </summary>
        /// <param name="index">Offset into the valid data</param>
        /// <param name="data">Value to write at index</param>
        public void SetUInt64(int index, UInt64 data)
        {
            ValidateNonNegative(index);
            ValidateSufficientSpace(8, length - index);

            _SetUInt64(index, data);
        }

        
        public void SetPaddedUInt16(int index, UInt16 data)
        {
            ValidateNonNegative(index);
            ValidateSufficientData(1, length - index);
            
            int dataSize = 2;
            if(index + dataSize < length) // All the data requested
            {
                _SetInt16(index, (Int16)data);
            }
            else // The rest of the data in the buffer
            {
                int offset = 0;
                while(index + offset < length)
                {
                    int shift;

                    if(littleEndian)
                    {
                        shift = dataSize - offset - 1;
                    }
                    else
                    {
                        shift = offset;
                    }

                    buffer[this.index + index + offset] = (byte)(data >> (shift * 8));
                    offset++;
                }
            }
        }

        
        public void SetPaddedUInt32(int index, UInt32 data)
        {
            ValidateNonNegative(index);
            ValidateSufficientData(1, length - index);
            
            int dataSize = 4;
            if(index + dataSize < length) // All the data requested
            {
                _SetInt32(index, (Int32)data);
            }
            else // The rest of the data in the buffer
            {
                int offset = 0;
                while(index + offset < length)
                {
                    int shift;

                    if(littleEndian)
                    {
                        shift = dataSize - offset - 1;
                    }
                    else
                    {
                        shift = offset;
                    }

                    buffer[this.index + index + offset] = (byte)(data >> (shift * 8));
                    offset++;
                }
            }
        }

        
        public void SetPaddedUInt64(int index, UInt64 data)
        {
            int dataSize = 8;
            int availableData = length - index;

            ValidateNonNegative(index);
            ValidateSufficientData(1, availableData);
            
            if(dataSize < availableData) // All the data requested
            {
                _SetUInt64(index, data);
            }
            else // The rest of the data in the buffer
            {
                int indexOffset = this.index + index;
                
                if(littleEndian)
                {
                    for(int offset = 0, shift = dataSize - 1; offset < availableData; offset++, shift--)
                    {
                        buffer[indexOffset + offset] = (byte)(data >> (shift * 8));
                    }
                }
                else
                {
                    for(int offset = 0, shift = 0; offset < availableData; offset++, shift++)
                    {
                        buffer[indexOffset + offset] = (byte)(data >> (shift * 8));
                    }
                }
            }
        }


        /// <summary>
        /// Modifies UTF8.GetBytes(data) bytes inside the BufferChunk
        /// </summary>
        /// <param name="index">Offset into the valid data</param>
        /// <param name="data">Value to write at index</param>
        public void SetUTF8String(int index, string data)
        {
            utf8.GetBytes(data, 0, data.Length, buffer, this.index + index);
        }

        
        private void _SetInt16(int index, Int16 data)
        {
            // LittleEndian architecture -> BigEndian network
            if(littleEndian)
            {
                buffer[this.index + index + 0] = (byte)(data >> (1 * 8));
                buffer[this.index + index + 1] = (byte)(data >> (0 * 8));
            }
            else // BigEndian architecture -> BigEndian network
            {
                buffer[this.index + index + 0] = (byte)(data >> (0 * 8));
                buffer[this.index + index + 1] = (byte)(data >> (1 * 8));
            }
        }

        private void _SetInt32(int index, Int32 data)
        {
            // LittleEndian architecture -> BigEndian network
            if(littleEndian)
            {
                buffer[this.index + index + 0] = (byte)(data >> (3 * 8));
                buffer[this.index + index + 1] = (byte)(data >> (2 * 8));
                buffer[this.index + index + 2] = (byte)(data >> (1 * 8));
                buffer[this.index + index + 3] = (byte)(data >> (0 * 8));
            }
            else // BigEndian architecture -> BigEndian network
            {
                buffer[this.index + index + 0] = (byte)(data >> (0 * 8));
                buffer[this.index + index + 1] = (byte)(data >> (1 * 8));
                buffer[this.index + index + 2] = (byte)(data >> (2 * 8));
                buffer[this.index + index + 3] = (byte)(data >> (3 * 8));
            }
        }

        private unsafe void _SetUInt64(int index, UInt64 data)
        {
            fixed(byte* pb = &buffer[this.index + index])
            {
                *((UInt64*)pb) = data;
            }
        }


        #endregion Set*

        #endregion Methods
    
        #region Custom Exceptions

        // Raised when trying to add more data than current buffer can hold
        public class InsufficientSpaceException : ApplicationException
        {
            public InsufficientSpaceException() : base() {}
            public InsufficientSpaceException(string msg) : base(msg) {}
            public InsufficientSpaceException(string msg, Exception inner) : base(msg, inner) {}
        }
        
        // Raised when requesting more data than current buffer holds
        public class InsufficientDataException : ApplicationException
        {
            public InsufficientDataException() : base() {}
            public InsufficientDataException(string msg) : base(msg) {}
            public InsufficientDataException(string msg, Exception inner) : base(msg, inner) {}
        }
        
        // Raised when requesting more data than current buffer holds
        public class NoDataException : ApplicationException
        {
            public NoDataException() : base() {}
            public NoDataException(string msg) : base(msg) {}
            public NoDataException(string msg, Exception inner) : base(msg, inner) {}
        }
        #endregion    
    }
}
