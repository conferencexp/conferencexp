using System;
using System.Collections;
using System.Globalization;


namespace MSR.LST.Net.Rtp
{
    /// <summary>
    /// A field is a set of numbers with an addition/subtraction operations as well as multiplication/division operation.
    /// The result of adding/subtracting or multiplying/dividing any two numbers of the field has to
    /// be in the set as well. 
    /// A finite field contains only finitely many elements. A finite field is also called Galois Field in
    /// honor of Evariste Galois. The advantage of a finite-field arithmetic is that it can be done exactly by
    /// computer (error-correcting code). 
    /// 
    /// (*) Evariste Galois 1811 (Bourg-La-Reine, near Paris) - 1832 (Paris)
    /// </summary>
    public struct GF16
    {
        /// <summary>
        /// Size of this Galois Field 2^16
        /// </summary>
        private static Int32 GF = 65536; 

        /// <summary>
        /// Prime polynomial for GF16
        /// </summary>
        private static UInt32 PrimePolynomial = 69643;

        /// <summary>
        /// Global table for Log and Antilog.
        /// These tables are used for multiplication and
        /// division of numbers.
        /// The tables are static because we need only one Log
        /// and one ALog table (the table values are the same for all
        /// the GF8 numbers) 
        /// </summary>
        private static UInt16[] Log = new UInt16[GF];
        private static UInt16[] ALog = new UInt16[GF * 2];
        private static UInt16[] inverse = new UInt16[GF];

        private UInt16 i;

        static GF16()
        {
            GF16.fillLogTables();
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="i">Value</param>
        public GF16(UInt16 i)
        {
            this.i = i;
        }

        /// <summary>
        /// Property to get the value of the Galois Field
        /// </summary>
        public UInt16 Value
        {
            get
            {
                return i;
            }
        }

        // Operator Overloads

        /// <summary>
        /// Overload of the string operator
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return i.ToString(CultureInfo.InvariantCulture);
        }

        public static implicit operator GF16(Int32 i)
        {
            if (i < 0 || i > UInt16.MaxValue)
            {
                throw new ArgumentException();
            }

            return new GF16((UInt16)i);
        }

        public static implicit operator GF16(UInt16 i)
        {
            return new GF16(i);
        }

        public override bool Equals(object o)
        {
            return this.i == ((GF16)o).i;
        }

        // TODO: I quickly added this method to avoid a warning,
        // we should check more closely if we should implement it
        // differently
        public override int GetHashCode()
        {
            return base.GetHashCode ();
        }

        /// <summary>
        /// Overload the + operator
        /// </summary>
        /// <param name="a">First operande</param>
        /// <param name="b">Second Operand</param>
        /// <returns>Sum of the 2 Galois Fields (XOR)</returns>
        public static GF16 operator + (GF16 a, GF16 b)
        {
            return GF16.Add(a.Value, b.Value);
        }

        /// <summary>
        /// Overload the - operator
        /// </summary>
        /// <param name="a">First operande</param>
        /// <param name="b">Second operande</param>
        /// <remarks>
        /// The subtraction is the same as the addition
        /// because with Galois fields, each number is its
        /// own negative. So there is no need to have a Sub
        /// method
        /// </remarks>
        /// <returns>Subrtraction of the 2 Galois Fields (XOR)</returns>
        public static GF16 operator - (GF16 a, GF16 b)
        {
            return GF16.Add(a.Value, b.Value);
        }

        /// <summary>
        /// Overload the * operator
        /// </summary>
        /// <param name="a">First operande</param>
        /// <param name="b">Second operande</param>
        /// <returns>Multiplication of the 2 Galois Fields</returns>
        public static GF16 operator * (GF16 a, GF16 b)
        {
            return GF16.Multiply(a.Value, b.Value);
        }

        /// <summary>
        /// Overload the / operator
        /// </summary>
        /// <param name="a">First operande</param>
        /// <param name="b">Second operande</param>
        /// <returns>a div b in Galois Fields</returns>
        public static GF16 operator / (GF16 a, GF16 b)
        {
            return GF16.Divide(a.Value, b.Value);
        }

        /// <summary>
        /// Add two number in GF8
        /// </summary>
        /// <remarks>
        /// The subtraction is the same as the addition
        /// because with Galois fields, each number is its
        /// own negative. So there is no need to have a Sub
        /// method
        /// </remarks>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <returns>a + b</returns>
        public static UInt16 Add (UInt16 a, UInt16 b)
        {
            // The addition is done with a exclusive-or
            return (UInt16)(a ^ b);
        }

        /// <summary>
        /// Fill the log table. The log tables are used to facilitate
        /// multiplication and division.
        /// </summary>
        /// <example>
        /// Here is the log table that would be created with GF4 (if GF value is 16):
        /// -------------------------------------------------------------------------------------
        /// |   i     || 0 | 1 | 2 | 3 | 4 | 5 |  6 |  7 | 8 |  9 | 10 | 11 | 12 | 13 | 14 | 15 |
        /// ====================================================================================
        /// | Log[i]  || - | 0 | 1 | 4 | 2 | 8 |  5 | 10 | 3 | 14 |  9 |  7 |  6 | 13 | 11 | 12 | 
        /// ------------------------------------------------------------------------------------
        /// | ALog[i] || 1 | 2 | 4 | 8 | 3 | 6 | 12 | 11 | 5 | 10 |  7 | 14 | 15 | 13 |  9 |  - |
        /// -------------------------------------------------------------------------------------
        /// </example>
        private static void fillLogTables()
        {
            UInt32 mask = 1;
            for (UInt16 i = 0; i < UInt16.MaxValue; i++)
            {
                ALog[i] = (UInt16)mask;
                Log[mask] = i;

                mask <<= 1;
                if ((mask & GF) != 0)
                    mask = mask ^ PrimePolynomial;
            }

            // set the extended gf_exp values for fast multiply
            for (UInt32 i = 0; i < UInt16.MaxValue; i++)
                ALog[i + UInt16.MaxValue] = ALog[i];

            inverse[0] = 0;
            inverse[1] = 1;
            for (UInt32 i = 2; i <= UInt16.MaxValue; i++)
                inverse[i] = ALog[UInt16.MaxValue - Log[i]];
        }

        /// <summary>
        /// Multiply 2 operandes
        /// </summary>
        /// <param name="a">First operande</param>
        /// <param name="b">Second operande</param>
        /// <returns>a * b</returns>
        /// <example>
        /// With GF4 (if GF value is 16) and the following log table:
        /// -------------------------------------------------------------------------------------
        /// |   i     || 0 | 1 | 2 | 3 | 4 | 5 |  6 |  7 | 8 |  9 | 10 | 11 | 12 | 13 | 14 | 15 |
        /// ====================================================================================
        /// | Log[i]  || - | 0 | 1 | 4 | 2 | 8 |  5 | 10 | 3 | 14 |  9 |  7 |  6 | 13 | 11 | 12 | 
        /// ------------------------------------------------------------------------------------
        /// | ALog[i] || 1 | 2 | 4 | 8 | 3 | 6 | 12 | 11 | 5 | 10 |  7 | 14 | 15 | 13 |  9 |  - |
        /// ------------------------------------------------------------------------------------- 
        /// 
        /// We would have:
        /// 
        /// 4 * 5 = ALog[(Log[4] + Log[5]) % 15)] = ALog[2 + 8] = ALog[10] = 7
        /// 
        /// This could be also verified using polynom multiplications:
        /// 4 -> 0100 -> x^2,  5 -> 0101 -> x^2 + 1
        /// 4 * 5 -> x^2 * (x^2 + 1) = x^4 + x^2 -> 10100
        /// 
        /// </example>
        public static UInt16 Multiply (UInt16 a, UInt16 b)
        {
            if (a == 0 || b == 0) return 0;

            return ALog[unchecked(Log[a] + Log[b])];
        }

        /// <summary>
        /// a divide by b
        /// </summary>
        /// <param name="a">Numerator</param>
        /// <param name="b">Denominator</param>
        /// <returns></returns>
        public static UInt16 Divide(UInt16 numerator, UInt16 denominator)
        {
            if (denominator == 0) throw new DivideByZeroException();
            if (numerator == 0) return 0;

            return ALog[unchecked(Log[numerator] + Log[inverse[denominator]])];
        }

        /// <summary>
        /// Power is used to create teh Vandermonde matrix
        /// </summary>
        /// <param name="exponent"></param>
        /// <returns></returns>
        public GF16 Power (int exponent)
        {
            if ( Value == 0 ) return this;
            if ( exponent == 0 ) return new GF16((UInt16)1);
        
            GF16 original = this;
            for (int i = 1; i < exponent; i++)
                this *= original;

            return this;
        }

        public static UInt16 Power(UInt16 x, UInt32 exponent)
        {
            if ( x == 0 ) return 0;
            if ( exponent == 0 ) return 1;
        
            UInt16 ret = x;
            for (int i = 1; i < exponent; i++)
                ret = Multiply(ret, x);

            return ret;
        }
    }
}
