using System;
using System.Collections;
using System.Globalization;


namespace MSR.LST.Net.Rtp
{
    /// <summary>
    /// IFec is the root interface for dealing with encoding/decoding.  
    /// </summary>
    public interface IFec
    {
        void Encode(BufferChunk[] data, BufferChunk[] checksum);
        void Decode(BufferChunk[] data, int nbChecksumPackets, BufferChunk[] recovery);
    }

    /// <summary>
    /// Class that implements the shared functionality between the fec implementations
    /// </summary>
    public class CFec
    {
        /// <summary>
        /// Amount of overhead, in bytes, the fec code adds to the data it encodes.  This is
        /// proprietary to our implementation.  We use it to store the size of the encoded data
        /// so that we know the length on the receiving end.  This should be pushed back up the API
        /// instead of stored here. JVE 6/16/2004
        /// </summary>
        public const int SIZE_OVERHEAD = 2;
    }

    /// <summary>
    /// Overview:
    /// -------------------------------------------------------------------------------------------
    /// XOR_Fec is a class that implement encoding/decoding using XOR algorithm.
    /// XOR algorithm is faster than Reed Solomon algorithm because it has less
    /// operation. However, XOR algorithm does not allow to generate more than one
    /// checksum packet.
    /// 
    /// Public Methods:
    /// -------------------------------------------------------------------------------------------
    /// Encode - Encode a buffer chunk array using XOR algorithm and generate an 
    ///          a buffer chunk array of the checksum (one item for XOR encoding) 
    /// Decode - Decode an array of rtpPackets using Reed Solomon algorithm and
    ///          generate a byte stream
    /// </summary>

    public class XOR_Fec: IFec
    {
        // Sort BufferChunks by length, longest to shortest
        private class BufferChunkLengthSorter : IComparer
        {
            public int Compare(object a, object b)
            {
                return ((BufferChunk)b).Length - ((BufferChunk)a).Length;
            }
        }

        private BufferChunkLengthSorter sorter = new BufferChunkLengthSorter();

        private int cActiveColumns = 1;
        private BufferChunk[] activeColumns;

        public XOR_Fec()
        {
            activeColumns = new BufferChunk[cActiveColumns];
        }

        /// <summary>
        /// This method creates as many checksum buffers as exist in the checksum array
        /// </summary>
        /// <param name="bytes">Array of buffer chunk that represents the data to encode</param>
        /// <param name="checksum">Contains 1 BufferChunk, which should be Reset() before calling</param>
        /// <returns>The checksum packet</returns>
        public void Encode(BufferChunk[] data, BufferChunk[] checksum)
        {
            ValidateObjectNotNull(data);
            ValidateObjectNotNull(checksum);
            ValidateNullData(data, 0);
            ValidateNullData(checksum, 0);
            ValidateChecksumPacketCount(checksum.Length);

            // Sort the BufferChunks from longest to shortest
            ActiveColumns(data, data.Length);

            // Add 2 bytes so the checksum packet can contain the length of the missing packet
            int xorDataLength = activeColumns[0].Length + 2;
            BufferChunk xorData = checksum[0];
            int index = xorData.Index;
            
            // Store the xor'd lengths of the data
            xorData += (ushort)(XORLength() ^ xorDataLength);
            xorData.Reset(index + 2, xorDataLength - 2);

            // Populate the checksum buffer (xorData)
            XORData(xorData, xorDataLength - 2);

            // Set xorData.Index back to 0
            xorData.Reset(index, xorDataLength);
        }

        /// <summary>
        /// Decoder: This method takes an array of BufferChunk in parameter with the data
        /// and checksum received and returns the missing packets. Because it
        /// is based on XOR, it can recover only from one packet lost. This method
        /// is very similar than encoder, but differ in the validation part.
        /// </summary>
        /// <param name="bytes">Array of buffer chunk that represents the packets where
        /// you might have lost a packet</param>
        /// <param name="nbChecksumPackets">Should be always set to 1</param>
        /// <returns>The missing packet</returns>
        public void Decode(BufferChunk[] data, int nbChecksumPackets, BufferChunk[] recovery)
        {
            ValidateObjectNotNull(data);
            ValidateObjectNotNull(data[data.Length - 1]); // Checksum packet can't be null
            ValidateChecksumPacketCount(nbChecksumPackets);
            ValidateNullData(data, 1);

            // Sort the BufferChunks from longest to shortest
            ActiveColumns(data, data.Length - 1);
            
            int xorDataLength = XORLength() ^ data[data.Length - 1].NextUInt16();

            BufferChunk xorData = recovery[0];
            xorData.Reset(xorData.Index, xorDataLength);
            XORData(xorData, xorDataLength);
        }

        
        /// <summary>
        /// This method creates a buffer chunk containing the XOR of all the
        /// buffer chunks passed in parameter.
        /// The lenght of the checksum buffer is the lenght of the larger
        /// buffer chunk.
        /// This method can be used for encoding to generate the checksum packet
        /// as well as for decoding to generate the missing packet
        /// </summary>
        /// <param name="bytes">array of buffer chunk</param>
        /// <returns>array of buffer chunk containing only one item: the XOR row by row
        /// of all the others buffer chunks</returns>
        /// <remarks>
        /// This method accept null entries in the array because the decoder will
        /// certainly place an array with a missing packet to recover.
        /// There is no validation done in this method in order to have it generic
        /// because the validation rules are different on the encoder and decoder
        /// </remarks>
        private void XORData(BufferChunk xorData, int xorDataLength)
        {
            // Keep track of the tail of the array so we don't have to look it up each time
            int acTail = cActiveColumns - 1;

            // Create the XOR checksum packet
            for(int row = 0; row < xorDataLength; row += 8)
            {
                // Each BufferChunk can have a different size
                // Virtually remove all columns that are too short to continue, this improves performance
                while(activeColumns[acTail].Length <= row)
                {
                    acTail--;
                }

                UInt64 xorValue = 0;
                for(int i = 0; i <= acTail; i++) // <= because acTail has already been adjusted - 1
                {
                    // XOR operation on the current row/column
                    xorValue ^= activeColumns[i].GetPaddedUInt64(row);
                }

                // Set the value of the checksum packet for the current row
                xorData.SetPaddedUInt64(row, xorValue);
            }
        }

        private int XORLength()
        {
            int xorValue = 0;

            for(int i = 0; i < cActiveColumns; i++)
            {
                xorValue ^= activeColumns[i].Length;
            }

            return xorValue;
        }
        
        
        /// <summary>
        /// Allocates storage for and sorts data from longest to shortest
        /// </summary>
        private void ActiveColumns(BufferChunk[] data, int cData)
        {
            // Number of active columns
            cActiveColumns = cData;
            activeColumns = new BufferChunk[cActiveColumns];

            // Copy - data.Length != cData or cActiveColumns
            for(int dataIndex = 0, acIndex = 0; dataIndex < data.Length; dataIndex++)
            {
                if(data[dataIndex] != null)
                {
                    activeColumns[acIndex] = data[dataIndex];
                    acIndex++;
                }
            }

            // Sort
            if(cActiveColumns > 1)
            {
                Array.Sort(activeColumns, sorter);
            }
        }

        private void ValidateNullData(BufferChunk[] data, int expectedNullCount)
        {
            int actualNullCount = 0;

            for(int i = 0; i < data.Length; i++)
            {
                if(data[i] == null)
                {
                    actualNullCount++;
                }
            }

            if(actualNullCount != expectedNullCount)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, 
                    Strings.IncorrectNumberOfNullObjects, expectedNullCount, actualNullCount));
            }
        }

        private void ValidateChecksumPacketCount(int nbChecksumPackets)
        {
            if(nbChecksumPackets != 1)
            {
                throw new ArgumentOutOfRangeException("nbChecksumPackets", nbChecksumPackets,
                    Strings.XORCanOnlyProduce1Checksum);
            }
        }

        private void ValidateObjectNotNull(object data)
        {
            if(data == null)
            {
                throw new ArgumentNullException(Strings.Data, Strings.NullObjectsAreNotAccepted);
            }
        }
    }

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    /// <summary>
    /// Overview:
    /// -------------------------------------------------------------------------------------------
    /// RS_Fec is a class that implement encoding/decoding using Reed Solomon algorithm.
    /// Reed Solomon algorithm does allow to generate more than one checksum packet (unlike XOR FEC),
    /// and allows to recover from #checksum packet lost in set of data packets that were used to encode.
    /// 
    /// Public Methods:
    /// -------------------------------------------------------------------------------------------
    /// Encode - Encode a BufferChunk array using RS algorithm 
    ///          and place the checksum(s) in a BufferChunk array
    /// Decode - Recover the packet lost in the data BufferChunk array
    /// </summary>
    public class RS_Fec : IFec
    {
        // TODO: During integration, take these values from CXP const
        // TODO: Validate size and make the Vandermonde matrix grow if
        //       bigger

        // Warning! Do not make both of these a power of 2 (>= 1024) 
        // or encoding slows down by a factor of 3x
        public static readonly ushort MaxDataPackets = 4000; 
        public static readonly ushort MaxChecksumPackets = 1000;
 
        // Vandermonde encoding matrix
        static UInt16[,] enc;

        #region Constructor

        /// <summary>
        /// Static constructor: Create the encoding Vandermonde matrix
        /// </summary>
        static RS_Fec()
        {
            // To increase runtime encoding performance, we create a large static Vandermonde encoding matrix ahead of time
            // that can be shared

            // maxChecksumPackets will be the max number of Columns of the encoding static matrix
            // maxDataPackets will be the max number of Rows of the encoding static matrix

            // Creation of the Vandermonde Matrix over GF16 with the following
            // (2^column)^row
 
            // As an example, a 5 x 3 Vandermonde Matrix over GF4 (5 data packets, 3 checksum packets)
            // would give the following:
            //
            // 1^0 2^0 4^0
            // 1^1 2^1 4^1
            // 1^2 2^2 4^2
            // 1^3 2^3 4^3
            // 1^4 2^4 4^4
            //
            // Which gives:
            //
            // 1   1   1
            // 1   2   4
            // 1   4   16
            // 1   8   64
            // 1   16  256
            enc = FEC_Matrix.CreateVandermondeMatrix(MaxChecksumPackets, MaxDataPackets);  
        }

        /// <summary>
        /// Dynamic constructor of the RS Fec
        /// </summary>
        public RS_Fec()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #endregion Constructor

        #region Public Methods for Encoding

        /// <summary>
        /// Encode a BufferChunk array using RS algorithm 
        /// and place the checksum(s) in a BufferChunk array
        /// </summary>
        /// <param name="data">Array of BufferChunk that represents the data to encode</param>
        /// <param name="checksum">Array of BufferChunk to store the generated checksum packets</param>
        public void Encode(BufferChunk[] data, BufferChunk[] checksum)
        {
            // Encode by multiplying the data BufferChunk array
            // with the encoding Vandermonde matrix. The operations are done over
            // GF16 (Galois Field 16 bits)
            FEC_BufferChunk.EncodeRS(data, checksum, enc);
        }

        #endregion Public Methods for Encoding

        #region Public Methods for Decoding

        // write the data into the provided RtpPacket's BufferChunks in the recovery[]
        /// <summary>
        /// Decode and insert in place the recovered packet(s) lost in the data BufferChunk array
        /// </summary>
        /// <param name="data">Array of BufferChunk that represents the data received including
        /// checksum (with a null entrie for each packet lost)</param>
        /// <param name="checksum">Number of checksum used to encode</param>
        /// <param name="recovery">Array of BufferChunk to store the recovered data packets</param>
        public void Decode(BufferChunk[] data, int checksum, BufferChunk[] recovery)
        {
            // TODO: We should first validate that #null entries in data BufferChunk
            // array is equal to recovery.Length and is also equal to checksum

            // Generate a decoding matrix from the BufferChunk array of packet received
            GF16[,] mtxDecode = FEC_BufferChunk.DecodingMatrixGeneration(data, checksum);

            // Invert the decoding matrix
            // TODO: To optimize we could generate the invert matrix only with the 
            // row needed for decoding missing packets instead of the full inverted matrix
            GF16[,] mtxDecodeInverted = FEC_Matrix.Invert(mtxDecode);

            // Decode by multiplying the received data BufferChunk array
            // with the inverted decoding matrix. The operations are done over
            // GF16 (Galois Field 16 bits)
            FEC_BufferChunk.DecodeRS(checksum, data, mtxDecodeInverted, recovery);
        }

        #endregion Public Methods for Decoding

    }

}
