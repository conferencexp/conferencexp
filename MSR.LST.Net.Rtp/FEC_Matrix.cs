// TODO: Add validation function
// TODO: Clean-up Gauss-Jordan elimination
using System;


namespace MSR.LST.Net.Rtp
{
    /// <summary>
    /// Overview:
    /// -------------------------------------------------------------------------------------------
    /// FEC_Matrix is a class that implements the basic matrix calculation using
    /// Galois Fields on 16 bits (GF16).
    /// 
    /// Public Methods:
    /// -------------------------------------------------------------------------------------------
    /// Invert - Invert the matrix by using Gauss-Jordan using GF16 operations
    /// CreateVandermondeMatrix - Create a Vandermonde matrix over GF16
    /// </summary>

    public class FEC_Matrix
    {
        /// <summary>
        /// Invert the matrix by using Gauss-Jordan using GF16 operations
        /// </summary>
        /// <remarks>
        /// The inversion is done by adding the identity matrix to the right
        /// and using Gauss-Jordan algorithm (elementary
        /// operations on rows) until we get the identity matrix 
        /// on the left. Then we strip the identity on the left
        /// in order to get the reverted matrix.
        /// </remarks>
        /// <param name="mtxIn_GF16">The matrix to invert</param>
        /// <returns>The inverted matrix</returns>
        public static GF16[,] Invert(GF16[,] mtxIn_GF16)
        {
            // Add the ID to the right to be able to apply Gauss-Jordan to revert the matrix
            GF16[,] mtxWithID_GF16 = FEC_Matrix.AddIdentityRight(mtxIn_GF16);

            // Apply Gauss Jordan algo to revert the matrix
            GF16[,] mtxInvert_GF16 = FEC_Matrix.GaussJordan(mtxWithID_GF16);

            // Remove the ID on the left
            GF16[,] mtxInvertStripped_GF16 = FEC_Matrix.StripIdentity(mtxInvert_GF16);

            // Return the inverted matrix
            return mtxInvertStripped_GF16;
        }

        /// <summary>
        /// Create a Vandermonde matrix of size row x column over GF16
        /// </summary>
        /// <remarks>
        /// The Vandermonde matrix is typically used to create the encoding matrix where:
        /// - The number of Columns of the matrix correspond to number of checksum 
        /// packets.
        /// - The number of Rows of the matrix correspond to number of data packets. 
        /// </remarks>
        /// <param name="columns">The number of columns of the Vandermonde matrix</param>
        /// <param name="rows">The number of rows of the Vandermode matrix</param>
        /// <returns></returns>
        public static UInt16[,] CreateVandermondeMatrix(int columns, int rows)
        {
            // TODO: Add input validation

            // maxChecksumPackets will be the max number of Columns of the encoding static matrix
            // maxDataPackets will be the max number of Rows of the encoding static matrix

            UInt16[,] vandermondeMtx = new UInt16[columns, rows]; 

            // Creation of the Vandermonde Matrix over GF16 with the following
            // (2^column)^row
 
            // As an example, a 5 x 3 Vandermonde Matrix over GF16 (5 data packets, 3 checksum packets)
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

            for (int col = 0; col < columns; col++)
            {
                // multFactor is the number to multiply to get the value in the next row
                // for a given column of the Vandermonde matrix 
                UInt16 multFactor = GF16.Power(2, (uint)col);
                for (int row = 0; row < rows; row++)
                {
                    if (row == 0)
                    { 
                        // Special case the first row (power of zero)
                        vandermondeMtx[col, row] = 1;
                    } 
                    else
                    {
                        // Each element of the Vandermonde matrix is calculated as (2^column)^row over GF16

                        // This algorithm uses the previous row to compute the next one to improve
                        // the performances (instead of recalculating (2^column)^row)
                        vandermondeMtx[col, row] = GF16.Multiply(vandermondeMtx[col, row-1], multFactor);
                    }
                }
            }
            return vandermondeMtx;
        }

        /// <summary>
        /// Add identity to the right side of a matrix
        /// </summary>
        /// <remarks>This is use when doing Jordan-Gauss elimination</remarks>
        /// <param name="matrixIn">The matrix</param>
        /// <returns>The new matrix with the identity</returns>
        private static GF16[,] AddIdentityRight (GF16[,] matrixIn)
        {
            Int32 rowLength = matrixIn.GetLength(1);
            Int32 columnLength = matrixIn.GetLength(0);

            GF16[,] matrixOut = new GF16[columnLength * 2, rowLength];

            // Add in identity portion to the top
            for (Int32 column = 0; column < columnLength; column++)
            {
                matrixOut[column + columnLength, column] = (UInt16)1;
            }

            // Copy the previous matrix to the bottom
            MatrixCopyTo(matrixIn, matrixOut);

            return matrixOut;
        }

        /// <summary>
        /// Copy a matrix to a destination matrix
        /// </summary>
        /// <param name="matrixIn">The matrix to copy</param>
        /// <param name="matrixDestination">The destination matrix</param>
        private static void MatrixCopyTo(GF16[,] matrixIn, GF16[,] matrixDestination)
        {
            int rowLength = matrixIn.GetLength(1);
            int columnLength = matrixIn.GetLength(0);

            for (int row = 0; row < rowLength; row++)
                for (int column = 0; column < columnLength; column++)
                    matrixDestination[column, row] = matrixIn[column, row];
        }

        /// <summary>
        /// Gauss - Jordan elimination.
        /// This algorithm takes a matrix and use elementary
        /// row operations to put the matrix in a row echelon form. This
        /// means that the result matrix will have the identity matrix at the
        /// begining.  
        /// This method can be used in 2 specific scenarios:
        /// - To solve a linear system numerically
        /// - To invert a matrix
        /// </summary>
        /// <example>
        /// For instance the matrix:
        /// 0   1   1   1   0   0
        /// 0   1   2   0   1   0
        /// 1   1   4   0   0   1
        /// 
        /// Will become (over int):
        /// [   ID  ]  [  rest  ]
        /// 1   0   0   2  -3   1
        /// 0   1   0   2  -1   0
        /// 0   0   1  -1   1   0
        /// </example>
        /// <remarks>
        /// For the Reed Solomon encoding, Gauss - Jordan elimination
        /// is used to invert the decoding matrix. This is needed to
        /// recover from packet loss.
        /// Important note: The input is modified
        /// </remarks>
        /// <param name="matrixIn">The matrix to transform in a row echelon form</param>
        /// <returns>The matrix in a row echelon from</returns>
        private static GF16[,] GaussJordan(GF16[,] matrixIn)
        {
            // Matrix size variables
            int rowLength = matrixIn.GetLength(1);
            int columnLength = matrixIn.GetLength(0);

            // The number of pivots needed is the min between row/column
            // For instance the matrix:
            // 0   1   1   1   0   0
            // 0   1   2   0   1   0
            // 1   1   4   0   0   1
            // has 3 pivots
            int pivots = Math.Min(rowLength, columnLength);

            // Loop through all the pivot columns
            for (int pivotColumn = 0; pivotColumn < pivots; pivotColumn++)
            {
                // TODO: Place Step 1 in a separate method

                // Step 1: Find the Pivot Row and reduce the pivot to have a factor of 1

                int pivotRow = -1;
                // Loop through all the rows to find a pivot for the pivotColumn
                for (int row = 0; row < rowLength; row++)
                {
                    // A pivot must be non-zero...
                    if (matrixIn[pivotColumn, row].Value != 0)
                    {

                        // ...and don't pivot on a row that has previous non-zero column entries

                        // Loop through columns of the row Check is the row found respect the non-zero column rule
                        bool isRowWithPreviousZero = true;
                        for (int column = 0; column < pivotColumn; column++)
                        {
                            // Check for non-zero
                            if (matrixIn[column, row].Value != 0)
                            {
                                // The row is not a pivot because it has previous non-zero entries
                                isRowWithPreviousZero = false;
                                break;
                            }
                        }

                        // If the row found has previous zero, it is a pivot row
                        if (isRowWithPreviousZero)
                        {
                            // The current row is the pivot
                            pivotRow = row;

                            GF16 factorPivot = matrixIn[pivotColumn, row];
                            // Reduce the pivot row to have a factor of 1
                            for(int column = pivotColumn; column < columnLength; column++)
                            {
                                matrixIn[column, row] /= factorPivot;
                            }
                            // We can exit the loop through rows in Step 1
                            break;
                        }
                    }
                }

                // Ensure that a pivot has been found
                if (pivotRow == -1)
                {
                    throw new ApplicationException(Strings.NoPivotRowFound);
                }

                // TODO: Place Step 2 in a separate method

                // Step 2: Do elementary operation to all rows (except the pivot row and 
                // the rows that have zero on the pivot column)
                // The final goal is to have a zero for all rows on the column corresponding to the 
                // pivot column
                
                // Loop trough the rows to do elementary operation (subtraction) 
                for (int row = 0; row < rowLength; row++)
                {
                    // Note: We skip the pivot row and the rows that has zero on the pivot
                    // column (no operation requiered)
                    if ((row != pivotRow) && (matrixIn[ pivotColumn, row].Value != 0))
                    {                     
                        // Now, determine the factor by which the pivot row must be added to the target 
                        // row to cancel out the pivot value
                        GF16 factorOperation = matrixIn[ pivotColumn, row ];

                        // Perform the operation for each column of the current row
                        for(int column = 0; column < columnLength; column++)
                        {
                            // Perform a subtraction. The final goal is to have a zero for all rows 
                            // on the column corresponding to the pivot column
                            matrixIn[column, row] -= factorOperation * matrixIn[column, pivotRow];
                        }
                    }
                }

            }

            // At this point the work manipulation is done but we might get the rows
            // out of order. 
            // For instance, the input matrix:
            // 0   1   1   1   0   0
            // 0   1   2   0   1   0
            // 1   1   4   0   0   1
            //
            // will become the following (over int operation):
            // 0   1   0   2  -1   0
            // 0   0   1  -1   1   0
            // 1   0   0   2  -3   1
            //
            // so now, we need to swap the row to place them in order to obtain the following:
            // [   ID  ]  [  rest  ]
            // 1   0   0   2  -3   1
            // 0   1   0   2  -1   0
            // 0   0   1  -1   1   0

            // TODO: Place Step 3 in a separate method

            // Step 3: Swap raw into order to have an identity matrix at the front

            for(int row = 0; row < rowLength; row++) // can use row < rowLength -1, because last row should always be in order?
            {
                // Skip rows that are in proper order
                if (matrixIn[row,row].Value != 1)
                {
                    // Not in proper order, search down through the matrix
                    bool found = false;
                    for (int searchRow = 0; searchRow < rowLength; searchRow++)
                    {
                        if (matrixIn[row, searchRow].Value == 1) // found the row we're looking for
                        {
                            found = true;
                            // Do the swap operation
                            // Save the target row to temporary swap space first
                            GF16[] swapRowTemp = new GF16[columnLength];
                            for(int swapRowColumn = 0; swapRowColumn < columnLength; swapRowColumn++)
                            {
                                swapRowTemp[swapRowColumn] = matrixIn[swapRowColumn, row];
                            }
                            // Replace the target row contents with the found row contents
                            for(int swapRowColumn = 0; swapRowColumn < columnLength; swapRowColumn++)
                            {
                                matrixIn[swapRowColumn, row] = matrixIn[swapRowColumn,searchRow];
                            }
                            // Replace the found row contents with the temporary swap space (previous target row contents)
                            for(int swapRowColumn = 0; swapRowColumn < columnLength; swapRowColumn++)
                            {
                                matrixIn[swapRowColumn,searchRow] = swapRowTemp[swapRowColumn];
                            }
                        }
                    }
                    if(!found)
                        throw new ApplicationException(Strings.SwapRowNotFound);
                }
            }

            return matrixIn;
        }   

        /// <summary>
        /// Remove the identity at the front of the matrix
        /// </summary>
        /// <param name="matrixIn">The input matrix</param>
        /// <returns>Stripped Matrix without the identity at the front</returns>
        private static GF16[,] StripIdentity(GF16[,] matrixIn)
        {
            int rowLength = matrixIn.GetLength(1);
            int columnLength = matrixIn.GetLength(0);
            GF16[,] matrixOut = new GF16[columnLength - rowLength, rowLength];

            for (int row = 0; row < rowLength; row++)
                for (int column = rowLength; column < columnLength; column++)
                    matrixOut[column - rowLength, row] = matrixIn[column, row];

            return matrixOut;
        }
    }
}
