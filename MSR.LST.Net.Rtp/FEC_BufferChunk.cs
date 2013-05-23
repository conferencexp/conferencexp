using System;
using System.Collections; // To use queue

// TODO: Add Validation


namespace MSR.LST.Net.Rtp
{
    /// <summary>
    /// Overview:
    /// -------------------------------------------------------------------------------------------
    /// FEC_BufferChunk is a class that implements the FEC calculation using
    /// BufferChunk.
    /// 
    /// Public Methods:
    /// -------------------------------------------------------------------------------------------
    /// - EncodeRS: Encode a BufferChunk array using a GF16 encoding matrix
    /// - DecodingMatrixGeneration: Generate a decoding matrix from a BufferChunk
    ///   array of packets received
    /// - DecodeRS: Recover lost packets from a BufferChunk array of received packet
    ///   using a GF16 decoding matrix
    /// </summary>

    public class FEC_BufferChunk
    {
        #region Public

        /// <summary>
        /// Encode the packets that are in the data BufferChunk array and place the 
        /// encoded result over GF16 in the checksum packets that are in 
        /// the result BufferChunk array. This method encode the size on the first 2
        /// bytes of the checksum packet and the data after that. Every checksum BufferChunk
        /// has the same size, which is the size of the largest BufferChunk in the data BufferChunk
        /// array, plus one if this BufferChunk doesn't end on a 16 bits boundary, plus
        /// two to store the encoded length 
        /// </summary>
        /// <param name="data">The data BufferChunk array (left part of the multiplication)</param>
        /// <param name="result"></param>
        /// <param name="encode">The encoding Vandermonde GF16 matrix (right part of the multiplication)</param>
        public static void EncodeRS(BufferChunk[] data, BufferChunk[] checksum, UInt16[,] encode)
        {
            // Get the length in byte of largest BufferChunk in the data BufferChunk array 
            // (which is an array of BufferChunk that could have different sizes)
            int maxDataLength = GetMaxLength(data);

            // The checksum packet as a even number of bytes so we can stay with GF16
            // and not have to use GF8 for the last byte when length odd
            int checksumNbRowsByte = maxDataLength;
            if ((maxDataLength & 1) == 1)
            {
                checksumNbRowsByte += 1;
            }

            // Add 2 more bytes to store the length
            checksumNbRowsByte += CFec.SIZE_OVERHEAD;

            // Encode the length of the data and place that on the first 2 bytes
            // of the checksum BufferChunk
            EncodeRSLength(data, checksum, encode);

            // Start to put encoded data at index + 2, because the 2 first bytes
            // of the checksum already contain the encoded size
            for (int i = 0; i < checksum.Length; i++)
            {
                BufferChunk bc = checksum[i];
                bc.Reset(bc.Index + CFec.SIZE_OVERHEAD, checksumNbRowsByte - CFec.SIZE_OVERHEAD);
                
                // Reset all of the checksum packets so they have no data
                // TODO - Temporary workaround for column based approach - JVE 7/6/2004
                bc.Clear();
            }

            // Place all the packets in a 16bits boundary to avoid byte operation
            // Important: This suppose that the memory reserved for all the bc passed in param 
            // to this method ends on a 16bits boundary (so at least one addition availabe byte 
            // when the last item is on a even address)
            Queue paddedData = new Queue();
            AddPadding(data, 0, paddedData);

            // Encode the data and place the encoded information in the checksum BufferChunk
            EncodeRSData(data, checksum, encode, maxDataLength);

            // Earlier, we changed the boundary of all data BufferChunk
            // that was not ending on a 16 bits boundary in order to have
            // better performance during the encoding. Now we need to 
            // restore the data to their original state to be clean.
            RemovePadding(paddedData);

            // Restore the BufferChunk index in the checksum checksum BufferChunk
            // to 0 to be clean. Earlier, We changed the index in order to encode 
            // the data after the encoding length.
            for (int i=0; i < checksum.Length; i++)
            {
                BufferChunk bc = checksum[i];
                bc.Reset(bc.Index - CFec.SIZE_OVERHEAD, checksumNbRowsByte);
            }
        }

        /// <summary>
        /// Decode to retrieve the missing data packet(s) (null entries) in the data BufferChunk array and place the 
        /// decoded result over GF16 in order in the recovery BufferChunk array with the right length (every
        /// data packet can have a different length)
        /// </summary>
        /// <param name="checksum">The number of checksum packet(s) that were used to encode</param>
        /// <param name="data">The data BufferChunk array</param>
        /// <param name="decode">The decoding GF16 matrix</param>
        /// <param name="recovery">The recovery BufferChunk array to place the recovered result</param>
        public static void DecodeRS(int checksum, BufferChunk[] data, GF16[,] decode, BufferChunk[] recovery)
        {

            // TODO: Validation discussed with Jason:
            // Check in data array if: the #data lost = # checksum and if this number = recovery.length 

            // Decode the length and set it to the recovery packets
            DecodeRSLength(data, checksum, decode, recovery);

            // ... and don't forget to adjust the index of the checksum BufferChunk because 
            // we don't want to take in account the length anymore
            int dataLength = data.Length;
            for (int i = dataLength - checksum; i < dataLength; i++)
            {
                BufferChunk bc = data[i];

                if(bc != null)
                {
                    bc.Reset(bc.Index + CFec.SIZE_OVERHEAD, bc.Length - CFec.SIZE_OVERHEAD);
                }
            }

            // Adjust all of the data (and recovery) to a 16bit boundary to avoid byte operations
            // Important: This assumes the provided BufferChunks have room to grow to 16bit boundary
            Queue paddedBufferChunks = new Queue();
            AddPadding(data, checksum, paddedBufferChunks);
            AddPadding(recovery, 0, paddedBufferChunks);

            // Decode the data and set the result into the recovery packets
            DecodeRSData(data, checksum, decode, recovery);

            // Reset all the packets back to their initial length
            RemovePadding(paddedBufferChunks);

            // The caller is responsible for resetting all BufferChunks so there is no need
            // to reset the checksum packets
        }


        /// <summary>
        /// Create the decoding matrix
        /// </summary>
        /// <param name="nbDataPackets">Number of data packet</param>
        /// <param name="nbChecksumPackets">Number of checksum packet</param>
        /// <param name="rtpPackets">Array of rtpPackets</param>
        /// <returns>The decoding matrix</returns>
        /// <example>
        /// For example, if you have 3 data packets and 2 checksum
        /// packets and get the last data packet, you will get
        /// the following matrix  
        /// 0   1   1
        /// 0   1   2
        /// 1   1   4
        /// </example>
        // TODO: To be more generic, we should use System.Array instead of array of BufferChunks
        public static GF16[,] DecodingMatrixGeneration(BufferChunk[] dataReceived, int nbChecksumPackets)
        {
            // nbTotalPackets is the total # rtpPackets to create (data packets and checksum packets)
            int nbTotalPackets = dataReceived.Length;
            int nbDataPackets = nbTotalPackets - nbChecksumPackets;

            // Let's suppose that we lost the 2 first packets
            // So [E'] that should have been
            //  1   0   0   1   1
            //  0   1   0   1   2
            //  0   0   1   1   4
            // is actually
            //  0   1   1
            //  0   1   2
            //  1   1   4

            // The reverted decoding matrix will be multiplied by data received to get all the datas
            // for instance [D0..D2] = [D2 C0 C1] x 1/[E']
            // => So the number of columns should correspond to the number of data recieved + checksum received
            // and the number of rows should correspond of the number of data packet K

            // count the number of packets received
            int nbPacketsReceived = ReceivedPacketsCount(nbTotalPackets, dataReceived);

            // nbPacketsReceived columns, K rows
            // Note that the # columns is smaller than the number of packet received
            // in case the number of packet lost was smaller than the number than
            // the number of checksum packets (we short the matrix: It's like if we lost the last
            // checksum(s) packet(s)
            GF16[,] mtxEprime_GF16 = new GF16[nbPacketsReceived, nbDataPackets];
                    
            // Construct the matrix mtxEprime_GF16 from the rtpPackets received
            // Note see also Jay's Decode method

            // Loop trough the data rtpPackets and put a 1 on the row of the packet
            // received

            int foundIndex = 0;
            for (int index=0; index < nbDataPackets; index++)
            {
                if (dataReceived[index] != null) // not a packet lost
                {
                    // Generate a row in the mtxEprime_GF16 (decode matrix)
                    if (index < nbDataPackets)
                    {
                        // TODO: To be clean, we should have initialized all the other items
                        // on the row explicitely to zero or make sure their are set them to zero 
                        // when creating the new matrix
                        mtxEprime_GF16[foundIndex, index] = (UInt16)1;
                    }
                    foundIndex++;
                    //                  if (foundIndex >= nbPacketsReceived-nbAdditionalPacketsReceived)
                    //                      return mtxEprime_GF16;
                }
            }

            // Generate a Vandermonde Mtx for the other items
            // TODO: Change that to use the Vandermonde static mtx
            for (UInt32 m = 0; m < nbChecksumPackets; m++)
            {
                // Note: If a checksum packet has been lost, the column is skiped
                if (dataReceived[m+nbDataPackets] != null) // not a packet lost
                {
                    for (UInt32 k = 0; k < nbDataPackets; k++)
                    {
                        // Each element of the Vandermonde matrix is calculated as (m+1)^k over GF16
                        // mtx[column, row] = (column+1)^row 

                        // With Vandermonde: 1^x, 2^x, 3^x, ... the code was:
                        // mtxEprime_GF16[foundIndex, k] = GF16.Power((UInt16)(m+1), (UInt32)k);
                        // I experimented problems (no pivot found during invertion of Eprime)
                        // when losing 3 packets (worked fine when losing 2 packets)

                        // So I changed, to use Vandermonde matrix were mtx[column, row] = (2^column)^row
                        mtxEprime_GF16[foundIndex, k] = GF16.Power((UInt16)GF16.Power(2, (m)), (UInt32)k);

                        // TODO: Check if Jay's approach is more efficient: mtxE_GF16[m, k] = GF16.gf_exp[GF16.Modnn(k*m)];
                    }
                    foundIndex++;
                    //                  if (foundIndex >= nbPacketsReceived-nbAdditionalPacketsReceived)
                    //                      return mtxEprime_GF16;
                }
            } // for
            return mtxEprime_GF16;
        }


        #endregion Public

        #region Private

        #region Private Methods for Boundaries / BufferChunk Length

        /// <summary>
        /// Find the length of the longest BC in the array
        /// </summary>
        /// <param name="data">The BufferChunk array</param>
        /// <returns>The length (in bytes) of largest BufferChunk in the data BufferChunk array</returns>
        private static int GetMaxLength(BufferChunk[] data)
        {
            // TODO: We could do some validation here if needed (i.e. no null entries, etc.)

            // To get the number of row of the first matrix (which is an array of BufferChunk that
            // could have different sizes), we have to check the length of every BufferChunk and keep
            // the max
            int maxLen = 0;

            // Get the size of the largest row
            for (int i=0; i < data.Length; i++)
            {
                if (data[i].Length > maxLen)
                {
                    maxLen = data[i].Length;
                }
            }

            return maxLen;
        }


        /// <summary>
        /// Place all the packets in a 16bits boundary to avoid byte operation
        /// Important: This suppose that all the bc passed in param to this method
        /// ends on a 16bits boundary (so at least one addition availabe byte when the last
        /// item is on a even address)
        /// </summary>
        /// <param name="data">The buffer chunk array containing the packet to resize</param>
        /// <param name="checksum">The number of checksum packet (see remark section below)</param>
        /// <param name="paddedData">Queue containing index of packet that was not on
        /// a 16bits boundary and were the size has been incremented by one</param>
        /// <remarks>
        /// Checksum parameter is the number of checksum packet. In this implementation of the
        /// Reed Solomon algorithm, we put the data to encode in a 16bits boundary to
        /// simplify the encoding and improve the performance; we can treat all the packets as UInt16 chunks, 
        /// including the last one, so no need to tread limit case with byte and deal with big and little
        /// Indian issues. Because we encode the data, and that the checksum packets are created on
        /// a 16bits boundary, we only need to take care of the packets at the entries of the data array
        /// from index 0 to index smaller than data.Length - checksum
        /// Note that teh parameter checksum should be 0 during encoding because there 
        /// is no checksum packet in the data array at that point and should be the number
        /// of checksum entries during decoding because the data array has checksum in it.
        /// </remarks>
        private static void AddPadding(BufferChunk[] data, int checksum, Queue paddedData)
        {
            // TODO: We could add validation for the limit case, such as
            // when the data entry at the index is null or the size was 0 byte, etc.

            // Note that the checksum packets should already been in a 16bits boundary 
            // (see encoding code)

            // TODO: We could also add a validation that check if we are not decrementing
            // a buffer size that was already on a 16bits boundary

            // TODO: We could continue to loop from index data.Length - checksum
            // to index < checksum to double check that all the checksum packets
            // ends on a 16bits boundary
 
            for (int i = 0; i < data.Length - checksum; i++)
            {
                BufferChunk bc = data[i];

                if ((bc != null) && ((bc.Length & 1) == 1))
                {
                    // Put the length on a 16 bits boundary
                    bc += (byte)0;

                    // Enqueue the index where we changed the length, so we can get back to the initial
                    // state after decoding
                    paddedData.Enqueue(bc);
                }
            }
        }


        /// <summary>
        /// Restore all the packet in their original size. The queue paddedData is used
        /// to know which packet index has a packet where we have to decrement the length by one.
        /// </summary>
        /// <param name="paddedData">Queue containing index of packet where the size has to be 
        /// decrement the length by one (used as input param)</param>
        private static void RemovePadding(Queue paddedData)
        {
            // Note that the checksum packets should already been in a 16bits boundary (see encoding code)
            while (paddedData.Count > 0)
            {
                // Restore the length
                ((BufferChunk)paddedData.Dequeue()).Length--;
            }
        }


        #endregion Private Methods for Boundaries / BufferChunk Length

        #region Private Methods for RS Encoding

        /// <summary>
        /// Encode the length of the packets that are in the data BufferChunk array and place the 
        /// encoded length over GF16 in the 2 first byte of the checksum packets that are in 
        /// the result BufferChunk array  
        /// </summary>
        /// <param name="data">The BufferChunk array containing the data packets to get the size</param>
        /// <param name="checksum">The number of checksum packets to generate.
        /// This number is actually also the number of column of the Vandermonde
        /// encoding matrix</param>
        /// <param name="encode">The Vandermonde encoding matrix (size min should be
        /// Vandermonde #column: checksum, Vandermonde #row: data.Length) 
        /// </param>
        /// <param name="result">The BufferChunk checksum packets array of containing the encoded length (over GF16)
        /// in the first 2 bytes</param>
        private static void EncodeRSLength(BufferChunk[] data, BufferChunk[] result, UInt16[,] encode)
        {
            // Get the length once to avoid having the overhead of getting it again inside a inner (performance)
            // Note that this value is also used outside of this method, so we could have had a parameter too,
            // but I prefer to get it here to avoid inconsistencies
            int dataPackets = data.Length;
            int checksum = result.Length;

            // Note: The size of the Vandermonde matrix used to encode the length is
            // Vandermonde #column: checksum, Vandermonde #row: dataPackets 

            // TODO: Validate if Vandermonde # max column >= checksum and
            // Vandermonde #row >= dataPackets

            // TODO: Validate that there are no null entries in data and result

            // TODO: Validate that all the checksum packet have at least 2 bytes

            // Length encoding
            for (int encodeColumn = 0; encodeColumn < checksum; encodeColumn++)
            {
                UInt16 encodeValue = 0;

                for (int dataPacket = 0; dataPacket < dataPackets; dataPacket++)
                {
                    // TODO: performance - lengths of BC don't change between loops, store once? JVE
                    UInt16 dataValue = GF16.Multiply((UInt16) data[dataPacket].Length, encode[encodeColumn, dataPacket]);
                    encodeValue = GF16.Add(encodeValue, dataValue); 
                }

                result[encodeColumn] += encodeValue;
            }
        }

        /// <summary>
        /// Encode the packets that are in the data BufferChunk array and place the 
        /// encoded result over GF16 in the checksum packets that are in 
        /// the result BufferChunk array  
        /// </summary>
        /// <param name="data">The BufferChunk array containing the data packets</param>
        /// <param name="checksum">The number of checksum packets to generate.
        /// This number is actually also the number of column of the Vandermonde
        /// encoding matrix</param>
        /// <param name="checksum">The BufferChunk checksum packets array of containing the encoded data (over GF16)
        /// after the first 2 bytes (the first 2 bytes are used to encode the length)</param>
        /// <param name="encode">The Vandermonde encoding matrix (size min should be
        /// Vandermonde #column: checksum, Vandermonde #row: data.Length) 
        /// </param>
        /// <param name="checksumRowsInt16">The number of row (in int 16 chunks) of the checksum packets</param>
        private static void EncodeRSData(BufferChunk[] data, BufferChunk[] checksum, UInt16[,] encode, int maxDataLength)
        {
            // Note: The size of the Vandermonde matrix used to encode the length is
            // Vandermonde #column: checksumLength, Vandermonde #row: dataPackets 

            // TODO: Validate if Vandermonde # max column >= checksum and
            // Vandermonde #row >= dataPackets

            // TODO: Validate that there are no null entries in data and checksum

            // Get the length once to avoid having the overhead of getting it again inside a inner (performance)
            int dataPackets = data.Length;
            int checksumLength = checksum.Length;

            // Note that we scan column after column, so we generate the checksum packets
            // one after the other
            for (int checksumColumn = 0; checksumColumn < checksumLength; checksumColumn++)
            {
                // For each column fill out the checksum mtx row after row
                for (int checksumRow = 0; checksumRow < maxDataLength; checksumRow += 2)
                {
                    for (int encodeRow = 0; encodeRow < dataPackets; encodeRow++)
                    {
                        // If we pass the size of the current data packet, so no operation are required
                        // 
                        if ((checksumRow) < data[encodeRow].Length)
                        {
                            UInt16 currentValue = GF16.Multiply(data[encodeRow].GetUInt16(checksumRow), 
                                encode[checksumColumn, encodeRow]);
                            currentValue = GF16.Add(checksum[checksumColumn].GetUInt16(checksumRow), currentValue); 
                            checksum[checksumColumn].SetUInt16(checksumRow, currentValue);
                        }
                    }
                }
            }
        }


        #endregion Private Methods for RS Encoding

        #region Private Methods for RS Decoding

        /// <summary>
        /// Count the number of packet actually received from the network
        /// </summary>
        /// <param name="nbTotalPackets">Number of packets (data + checksum)</param>
        /// <param name="rtpPackets">The array of rtpPackets received</param>
        /// <returns>The count of packets received</returns>
        private static int ReceivedPacketsCount(int nbTotalPackets, BufferChunk[] dataReceived)
        {

            // count the number of packets received
            int nbPacketsReceived = 0;
            for (int index=0; index < nbTotalPackets; index++)
            {
                if (dataReceived[index] != null) // not a packet lost
                {
                    nbPacketsReceived++;
                }
            }
            return nbPacketsReceived;
        }

        /// <summary>
        /// Decode to retrieve the length of the missing data packet(s) (null entries) in the 
        /// data BufferChunk array and set the length of the packets in the recovery BufferChunk array.
        /// We have to do that because every data packet could have a different length.
        /// </summary>
        /// <param name="data">The data BufferChunk array</param>
        /// <param name="checksum">The number of checksum packet(s) that were used to encode</param>
        /// <param name="decode">The decoding GF16 matrix</param>
        /// <param name="recovery">The recovery BufferChunk array to set the length of the recovered packets</param>
        private static void DecodeRSLength(BufferChunk[] data, int checksum, GF16[,] decode, BufferChunk[] recovery)
        {
            // Important! For now I assume the following in the firstMatrix:
            // - The array is always of size data + checksum
            // - #checksum not null entries = #packet lost
            // - recovery.length is exactly the number of data packet lost
            // - checksum param is # checksum packets that were used to encode


            // Get the length once to avoid having the overhead of getting it again inside a inner (performance)
            // Note that this value is also used outside of this method, so we could have had a parameter too,
            // but I prefer to get it here to avoid inconsitencies
            int dataLength = data.Length;

            int recoveryColumn = 0;

            for (int dataColumn = 0; dataColumn < dataLength - checksum; dataColumn++)
            {
                // recover only the missing column
                if (data[dataColumn] == null)
                {
                    // Inverted matrix row index
                    int imRowIndex = 0;

                    // length of the recovered buffer 
                    UInt16 currentLength = 0;

                    for (int dataIndex = 0; dataIndex < dataLength; dataIndex++)
                    {
                        // Perform the core operation mult with add in GF16
                        // TODO: We could crunch the column so we avoid this test
                        if (data[dataIndex] != null)
                        {
                            UInt16 length = 0;

                            if (dataIndex < dataLength - checksum) // For the data part, we get the length of the BufferChunk
                            {
                                length = (UInt16) data[dataIndex].Length;
                            } 
                            else // For the checksum part, the encoded length is inside the first 2 bytes
                            {
                                length = data[dataIndex].GetUInt16(0);
                            }

                            UInt16 currentValue = GF16.Multiply(length, decode[dataColumn, imRowIndex].Value);
                            currentLength = GF16.Add(currentLength, currentValue); 

                            imRowIndex++;
                        } // if
                    } // for column (do elementary operations)

                    BufferChunk bc = recovery[recoveryColumn];
                    bc.Reset(bc.Index, currentLength);
                    
                    // Reset all of the recovery packets so they have no data
                    // TODO - Temporary workaround for column based approach - JVE 7/6/2004
                    bc.Clear();

                    recoveryColumn++;
                } // if
            } // for dataColumn
        }

        /// <summary>
        /// Decode to retrieve the missing data packet(s) (null entries) in the data BufferChunk array and place the 
        /// decoded result over GF16 in order in the recovery BufferChunk array
        /// </summary>
        /// <param name="data">The data BufferChunk array</param>
        /// <param name="checksum">The number of checksum packet(s) that were used to encode</param>
        /// <param name="decode">The decoding GF16 matrix</param>
        /// <param name="recovery">The recovery BufferChunk array to place the recovered result</param>
        private static void DecodeRSData(BufferChunk[] data, int checksum, GF16[,] decode, BufferChunk[] recovery)
        {
            // Important! For now I assume the following in the firstMatrix:
            // - The array is always of size data + checksum
            // - #checksum not null entries = #packet lost
            // - recovery.length is exactly the number of data packet lost
            // - checksum param is # checksum packets that were used to encode

            // Reconstruct data

            // Get the length once to avoid having the overhead of getting it again inside a inner (performance)
            // Note that this value is also used outside of this method, so we could have had a parameter too,
            // but I prefer to get it here to avoid inconsitencies
            int dataLength = data.Length;

            // Scan column after column
            int recoveryColumn = 0;
            for (int dataColumn = 0; dataColumn < dataLength - checksum; dataColumn++)
            {
                // recover only the missing column
                if (data[dataColumn] == null)
                {
                    int recoveryRowLength = recovery[recoveryColumn].Length;

                    // For each column fill out the output mtx row after row
                    for (int recoveryRow = 0; recoveryRow < recoveryRowLength; recoveryRow += 2)
                    {
                        int imRowIndex = 0;

                        // nnDataColumn - not null data index
                        for (int nnDataColumn = 0; nnDataColumn < dataLength; nnDataColumn++)
                        {
                            // Perform the core operation mult with add in GF16
                            // TODO: We could crunch the column so we avoid this test
                            if (data[nnDataColumn] != null)
                            {
                                if(recoveryRow < data[nnDataColumn].Length)
                                {
                                    UInt16 currentValue = GF16.Multiply(data[nnDataColumn].GetUInt16(recoveryRow), 
                                        decode[dataColumn, imRowIndex].Value);
                                    currentValue = GF16.Add(recovery[recoveryColumn].GetUInt16(recoveryRow), currentValue); 
                                    recovery[recoveryColumn].SetUInt16(recoveryRow, currentValue);
                                }
                                imRowIndex++;
                            }
                        }
                    }

                    recoveryColumn++;
                }
            }
        }


        #endregion Private Methods for RS Decoding

        #endregion Private
    }
}
