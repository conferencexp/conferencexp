using System;
using System.Collections;
using System.Globalization;


namespace MSR.LST.Net.Rtp
{
    public struct GF8
    {
        #region Reference Tables
        public static byte[] gf_log = new byte[byte.MaxValue + 1];
        public static byte[] gf_exp = new byte[(byte.MaxValue + 1) * 2];
        public static byte[] inverse = new byte[byte.MaxValue + 1];

        private static GF8 empty;

        static GF8()
        {
            GF8.empty = new GF8(0);
            empty.i = -1;

            #region Initialize Reference Tables
            UInt32 primitive_polynomial = 285;

            UInt32 mask = 1;
            gf_exp[8] = 0;

            for (byte i = 0; i < byte.MaxValue; i++)
            {
                gf_exp[i] = (byte)mask;
                gf_log[mask] = (byte)i;

                mask <<= 1;
                if ((mask & 256) != 0)
                    mask = mask ^ primitive_polynomial;
            }

            // set the extended gf_exp values for fast multiply
            for (UInt32 i = 0 ; i < byte.MaxValue ; i++)
                gf_exp[i + byte.MaxValue] = gf_exp[i] ;

            inverse[0] = 0;
            inverse[1] = 1;

            for (UInt32 i=2; i <= byte.MaxValue; i++)
                inverse[i] = gf_exp[byte.MaxValue - gf_log[i]];
            #endregion
        }

        #endregion
        #region Instance ctor, Methods, & Properties
        public GF8(byte i)
        {
            this.i = i;
        }
        public byte Value
        {
            get
            {
                return (byte)i;
            }
        }
        private Int32 i;
        public GF8 Power (byte exponent)
        {
            if ( Value == 0 ) return this;
            if ( exponent == 0 ) this = 1;
        
            GF8 original = this;
            for (int i = 1; i < exponent; i++)
                this *= original;

            return this;
        }

        #endregion
        #region Operator Overloads
        public override string ToString()
        {
            if (i == -1)
                return "GF8.Empty";
            else
                return i.ToString(CultureInfo.InvariantCulture);
        }

        public static implicit operator GF8(Int32 i)
        {
            if (i < 0 || i > byte.MaxValue)
                throw new ArgumentException();

            return new GF8((byte)i);
        }

        public static implicit operator GF8(byte i)
        {
            return new GF8(i);
        }

        public static GF8 operator + (GF8 a, GF8 b)
        {
            return GF8.Add(a.Value, b.Value);
        }

        public static GF8 operator - (GF8 a, GF8 b)
        {
            return GF8.Add(a.Value, b.Value);
        }

        public static GF8 operator * (GF8 a, GF8 b)
        {
            return GF8.Multiply(a.Value, b.Value);
        }

        public static GF8 operator / (GF8 a, GF8 b)
        {
            return GF8.Divide(a.Value, b.Value);
        }
        #endregion
        #region Static Methods
        public static GF8 Empty
        {
            get
            {
                return GF8.empty;
            }
        }
        public static byte Add (byte a, byte b)
        {
            return (byte)(a ^ b);
        }

        public static byte Multiply (byte a, byte b)
        {
            if (a == 0 || b == 0 ) return 0;

            return gf_exp[unchecked(gf_log[a] + gf_log[b])];
        }

        public static byte Divide (byte numerator, byte denominator)
        {
            if (denominator == 0) throw new DivideByZeroException();
            if (numerator == 0) return 0;

            return gf_exp[unchecked(gf_log[numerator] + gf_log[inverse[denominator]])];
        }

        public static byte Modnn(Int32 x)
        {
            while (x >= byte.MaxValue) 
            {
                x -= byte.MaxValue;
                x = (byte)((x >> 8) + (x & byte.MaxValue));
            }
            return (byte)x;
        }

        public static byte Power(byte x, UInt32 exponent)
        {
            if ( x == 0 ) return 0;
            if ( exponent == 0 ) return 1;
        
            byte ret = x;
            for (int i = 1; i < exponent; i++)
                ret = Multiply(ret, x);

            return ret;
        }
        #endregion
    }
}