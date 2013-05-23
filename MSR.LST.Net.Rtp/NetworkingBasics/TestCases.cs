using System;
using MSR.LST;
using MSR.LST.Test;

namespace MSR.LST.Net.Rtp.Test
{
    #region BufferChunk Test Cases

    #region Public Properties
   
    [TCProp("Buffer Chunk : Index(get)")]
    public class BC_IndexGet : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[5], 2, 3);

            if(bc.Index != 2)
            {
                throw new TestCaseException("unexpected index");
            }
        }
    }

    [TCProp("Buffer Chunk : Buffer(get)")]
    public class BC_BufferGet : TestCase
    {
        override public void Run()
        {
            byte[] data = new byte[] {1, 2, 3};
            BufferChunk bc = new BufferChunk(data);

            if(bc.Buffer != data)
            {
                throw new TestCaseException("buffers are not the same object");
            }
        }
    }

    [TCProp("Buffer Chunk : Length(get)")]
    public class BC_LengthGet : TestCase
    {
        override public void Run()
        {
            try
            {
                BufferChunk bc = new BufferChunk(2048);
                bc.Length = 40;

                if(bc.Length != 40)
                {
                    throw new TestCaseException("get isn't returning same value as set");
                }
            }
            catch(ArgumentOutOfRangeException){}
        }
    }


    #region Property Length
    
    [TCProp("Buffer Chunk : Length(set) : -1")]
    public class BC_LengthSet_Neg1 : TestCase
    {
        override public void Run()
        {
            try
            {
                BufferChunk bc = new BufferChunk(5);
                bc.Length = -1;
                throw new TestCaseException("length must be >= 0");
            }
            catch(ArgumentOutOfRangeException){}
        }
    }

    [TCProp("Buffer Chunk : Length(set) : 0")]
    public class BC_LengthSet_0 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(5);
            bc.Length = 0;
        }
    }

    [TCProp("Buffer Chunk : Length(set) : 1")]
    public class BC_LengthSet_1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2});
            bc.Length = 1;
        }
    }

    [TCProp("Buffer Chunk : Length(set) : < buffer.Length")]
    public class BC_LengthSet_1LTBuffer : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(5);
            bc.Length = 4;
        }
    }

    [TCProp("Buffer Chunk : Length(set) : > buffer.Length")]
    public class BC_LengthSet_EQBuffer : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(5);
            bc.Length = 5;
        }
    }

    [TCProp("Buffer Chunk : Length(set) : > buffer.Length")]
    public class BC_LengthSet_1GTBuffer : TestCase
    {
        override public void Run()
        {
            try
            {
                BufferChunk bc = new BufferChunk(5);
                bc.Length = 6;
                throw new TestCaseException("length must be <= buffer.Length");
            }
            catch(ArgumentOutOfRangeException){}
        }
    }


    #endregion Property Length
    
    #endregion Public Properties

    #region Constructors

    #region Constructor Size
    
    [TCProp("Buffer Chunk : Ctor(int size) : 0")]
    public class BC_CtorSize_NoData : TestCase
    {
        override public void Run()
        {
            try
            {
                BufferChunk bc = new BufferChunk(0);
                throw new TestCaseException("buffer length must be >= 1");
            }
            catch(BufferChunk.NoDataException){}
        }
    }

    [TCProp("Buffer Chunk : Ctor(int size) : 1")]
    public class BC_CtorSize_1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(1);
        }
    }

    [TCProp("Buffer Chunk : Ctor(int size) : 2")]
    public class BC_CtorSize_2 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(2);
        }
    }

    [TCProp("Buffer Chunk : Ctor(int size) : check index")]
    public class BC_CtorSize_GetIndex : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(1);
            if(bc.Index != 0)
            {
                throw new TestCaseException("Index not at beginning of buffer");
            }
        }
    }

    [TCProp("Buffer Chunk : Ctor(int size) : check length")]
    public class BC_CtorSize_GetLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(1);
            if(bc.Length != 0)
            {
                throw new TestCaseException("Length not zero - data is invalid");
            }
        }
    }

    [TCProp("Buffer Chunk : Ctor(int size) : check buffer length")]
    public class BC_CtorSize_GetBufferLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(5);
            if(bc.Buffer.Length != 5)
            {
                throw new TestCaseException("incorrect buffer size created");
            }
        }
    }


    #endregion Constructor Size

    #region Constructor Buffer
    
    [TCProp("Buffer Chunk : Ctor(byte[] buffer) : null")]
    public class BC_CtorBuffer_Null : TestCase
    {
        override public void Run()
        {
            try
            {
                BufferChunk bc = new BufferChunk(null);
                throw new TestCaseException("should not be able to create a BufferChunk with a null byte[]");
            }
            catch(ArgumentNullException){}
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer) : byte[0]")]
    public class BC_CtorBuffer_NoData : TestCase
    {
        override public void Run()
        {
            try
            {
                BufferChunk bc = new BufferChunk(new byte[0]);
                throw new TestCaseException("no data");
            }
            catch(BufferChunk.NoDataException){}
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer) : byte[1]")]
    public class BC_CtorBuffer_1Byte : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[1]);
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer) : check buffer")]
    public class BC_CtorBuffer_GetBuffer : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3});
            for(int i = 0; i < 3; i++)
            {
                if(bc.Buffer[i] != i + 1)
                {
                    throw new TestCaseException("bytes don't match");
                }
            }
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer) : check index")]
    public class BC_CtorBuffer_GetIndex : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[1]);
            if(bc.Index != 0)
            {
                throw new TestCaseException("Index not at beginning of buffer");
            }
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer) : check length")]
    public class BC_CtorBuffer_GetLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[1]);
            if(bc.Length != 1)
            {
                throw new TestCaseException("length is invalid");
            }
        }
    }


    #endregion Constructor Buffer

    #region Constructor Buffer, Index, Length

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : null, 0, 0")]
    public class BC_CtorBufferIndexLength_Null : TestCase
    {
        override public void Run()
        {
            byte[] data = null;

            try
            {
                BufferChunk bc = new BufferChunk(data, 0, 0);
                throw new TestCaseException("check for null parameters");
            }
            catch(ArgumentNullException){}
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : byte[0], 0, 0")]
    public class BC_CtorBufferIndexLength_NoData : TestCase
    {
        override public void Run()
        {
            try
            {
                BufferChunk bc = new BufferChunk(new byte[0], 0, 0);
                throw new TestCaseException("no data");
            }
            catch(BufferChunk.NoDataException){}
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : byte[1], -1, 0")]
    public class BC_CtorBufferIndexLength_IndexNeg1 : TestCase
    {
        override public void Run()
        {
            try
            {
                BufferChunk bc = new BufferChunk(new byte[1], -1, 0);
                throw new TestCaseException("index must be >= 0");
            }
            catch(ArgumentException){}
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : byte[1], 0, 1")]
    public class BC_CtorBufferIndexLength_Index0 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[1], 0, 1);
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : byte[3], 1, 2")]
    public class BC_CtorBufferIndexLength_Index1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[3], 1, 2);
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : byte[1], 0, -1")]
    public class BC_CtorBufferIndexLength_LengthNeg1 : TestCase
    {
        override public void Run()
        {
            try
            {
                BufferChunk bc = new BufferChunk(new byte[1], 0, -1);
                throw new TestCaseException("length must be >= 0");
            }
            catch(ArgumentException){}
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : byte[1], 0, 0")]
    public class BC_CtorBufferIndexLength_Length0 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[1], 0, 0);
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : byte[1], 0, 1")]
    public class BC_CtorBufferIndexLength_Length1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[1], 0, 1);
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : byte[5], 4, 0")]
    public class BC_CtorBufferIndexLength_Index1LTBufferLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[5], 4, 0);
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : byte[5], 5, 0")]
    public class BC_CtorBufferIndexLength_IndexEQBufferLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[5], 5, 0);
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : byte[5], 6, 0")]
    public class BC_CtorBufferIndexLength_Index1GTBufferLength : TestCase
    {
        override public void Run()
        {
            try
            {
                BufferChunk bc = new BufferChunk(new byte[5], 6, 0);
                throw new TestCaseException("index must be <= buffer.Length");
            }
            catch(ArgumentException){}
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : byte[5], 0, 4")]
    public class BC_CtorBufferIndexLength_Length1LTBufferLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[5], 0, 4);
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : byte[5], 0, 5")]
    public class BC_CtorBufferIndexLength_LengthEQBufferLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[5], 0, 5);
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : byte[5], 0, 6")]
    public class BC_CtorBufferIndexLength_Length1GTBufferLength : TestCase
    {
        override public void Run()
        {
            try
            {
                BufferChunk bc = new BufferChunk(new byte[5], 0, 6);
                throw new TestCaseException("length must be <= buffer.Length");
            }
            catch(ArgumentException){}
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : byte[5], 2, 2")]
    public class BC_CtorBufferIndexLength_IndexPlusLength1LTBufferLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[5], 2, 2);
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : byte[5], 3, 2")]
    public class BC_CtorBufferIndexLength_IndexPlusLengthEQBufferLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[5], 3, 2);
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : byte[5], 3, 3")]
    public class BC_CtorBufferIndexLength_IndexPlusLength1GTBufferLength : TestCase
    {
        override public void Run()
        {
            try
            {
                BufferChunk bc = new BufferChunk(new byte[5], 3, 3);
                throw new TestCaseException("index + length cannot be > buffer.Length");
            }
            catch(ArgumentException){}
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : check buffer")]
    public class BC_CtorBufferIndexLength_GetBuffer : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3}, 1, 2);

            for(int i = 1; i < 2; i++)
            {
                if(bc.Buffer[i] != i + 1)
                {
                    throw new TestCaseException("bytes don't match");
                }
            }
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : check index")]
    public class BC_CtorBufferIndexLength_GetIndex : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[5], 1, 2);

            if(bc.Index != 1)
            {
                throw new TestCaseException("Index is invalid");
            }
        }
    }

    [TCProp("Buffer Chunk : Ctor(byte[] buffer, int index, int length) : check length")]
    public class BC_CtorBufferIndexLength_GetLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[5], 1, 2);

            if(bc.Length != 2)
            {
                throw new TestCaseException("Length is invalid");
            }
        }
    }


    #endregion Constructor Buffer, Index, Length

    #endregion Constructors
    
    #region Operators & Casts (to/from byte[] & string)

    #region OpByteArray
    
    [TCProp("Buffer Chunk : explicit operator byte[]( BufferChunk source ) : null")]
    public class BC_OpByteArray_Null : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = null;
            
            try
            {
                byte[] bytes = (byte[])bc;
                throw new TestCaseException("argument is null");
            }
            catch(ArgumentNullException){}
        }
    }

    [TCProp("Buffer Chunk : explicit operator byte[]( BufferChunk source ) : no data")]
    public class BC_OpByteArray_NoData : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[] {1, 2, 3}, 0, 0);
            
            try
            {
                byte[] bytes = (byte[])bc;
                throw new TestCaseException("no data");
            }
            catch(BufferChunk.NoDataException){}
        }
    }

    [TCProp("Buffer Chunk : explicit operator byte[]( BufferChunk source ) : get byte[]")]
    public class BC_OpByteArray_GetByteArray : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[] {1, 2, 3}, 1, 2);
            byte[] bytes = (byte[])bc;

            // Validate the length
            if(bytes.Length != 2)
            {
                throw new TestCaseException("length should be 2");
            }

            // Validate the contents
            for(int i = 0; i < 2; i++)
            {
                if(bytes[i] != i + 2)
                {
                    throw new TestCaseException("bytes don't match");
                }
            }
        }
    }


    #endregion OpByteArray

    #region OpString
    
    [TCProp("Buffer Chunk : explicit operator string ( BufferChunk source ) : null")]
    public class BC_OpString_Null : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = null;
            
            try
            {
                string contents = (string)bc;
                throw new TestCaseException("argument is null");
            }
            catch(ArgumentNullException){}
        }
    }

    [TCProp("Buffer Chunk : explicit operator string ( BufferChunk source ) : no data")]
    public class BC_OpString_NoData : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[] {74, 97, 115, 111, 110}, 0, 0);
            
            try
            {
                string contents = (string)bc;
                throw new TestCaseException("no data");
            }
            catch(BufferChunk.NoDataException){}
        }
    }

    [TCProp("Buffer Chunk : explicit operator string ( BufferChunk source ) : part buffer")]
    public class BC_OpString_GetPartBuffer : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[] {74, 97, 115, 111, 110}, 0 , 3);
            string contents = (string)bc;

            if(contents != "Jas")
            {
                throw new TestCaseException("string should be 'Jas' but is " + contents);
            }
        }
    }

    [TCProp("Buffer Chunk : explicit operator string ( BufferChunk source ) : whole buffer")]
    public class BC_OpString_GetWholeBuffer : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[] {74, 97, 115, 111, 110});
            string contents = (string)bc;

            if(contents != "Jason")
            {
                throw new TestCaseException("string should be 'Jason' but is " + contents);
            }
        }
    }


    #endregion OpString

    #region OpBC String

    [TCProp("Buffer Chunk : explicit operator BufferChunk ( string source ) : null ")]
    public class BC_OpBC_String_Null : TestCase
    {
        override public void Run()
        {
            string data = null;

            try
            {
                BufferChunk bc = (BufferChunk)data;
                throw new TestCaseException("argument null");
            }
            catch(ArgumentNullException){}
        }
    }

    [TCProp("Buffer Chunk : explicit operator BufferChunk ( string source ) : empty ")]
    public class BC_OpBC_String_NoData : TestCase
    {
        override public void Run()
        {
            string data = "";

            try
            {
                BufferChunk bc = (BufferChunk)data;
                throw new TestCaseException("no data");
            }
            catch(BufferChunk.NoDataException){}
        }
    }

    [TCProp("Buffer Chunk : explicit operator BufferChunk ( string source ) : get BC")]
    public class BC_OpBC_String_GetBC : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = (BufferChunk)"Jason";
            byte[] bytes = new byte[]{74, 97, 115, 111, 110};

            // Validate length
            if(bc.Length != 5)
            {
                throw new TestCaseException("byte[] should be length 5");
            }

            // Validate contents
            for(int i = 0; i < 5; i++)
            {
                if(bytes[i] != bc.Buffer[i])
                {
                    throw new TestCaseException("bytes don't match");
                }
            }
        }
    }


    #endregion OpBC String

    #region OpBC ByteArray
    
    [TCProp("Buffer Chunk : operator BufferChunk(byte[] buffer) : null")]
    public class BC_OpBC_ByteArray_Null : TestCase
    {
        override public void Run()
        {
            byte[] data = null;

            try
            {
                BufferChunk bc = (BufferChunk)data;
                throw new TestCaseException("should not be able to create a BufferChunk with a null byte[]");
            }
            catch(ArgumentNullException){}
        }
    }

    [TCProp("Buffer Chunk : operator BufferChunk(byte[] buffer) : byte[0]")]
    public class BC_OpBC_ByteArray_NoData : TestCase
    {
        override public void Run()
        {
            try
            {
                BufferChunk bc = (BufferChunk)new byte[0];
                throw new TestCaseException("no data");
            }
            catch(BufferChunk.NoDataException){}
        }
    }

    [TCProp("Buffer Chunk : operator BufferChunk(byte[] buffer) : byte[1]")]
    public class BC_OpBC_ByteArray_1Byte : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = (BufferChunk)new byte[1];
        }
    }

    [TCProp("Buffer Chunk : operator BufferChunk(byte[] buffer) : check buffer")]
    public class BC_OpBC_ByteArray_GetBuffer : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = (BufferChunk)new byte[]{1, 2, 3};
            for(int i = 0; i < 3; i++)
            {
                if(bc.Buffer[i] != i + 1)
                {
                    throw new TestCaseException("bytes don't match");
                }
            }
        }
    }

    [TCProp("Buffer Chunk : operator BufferChunk(byte[] buffer) : check index")]
    public class BC_OpBC_ByteArray_GetIndex : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = (BufferChunk)new byte[1];
            if(bc.Index != 0)
            {
                throw new TestCaseException("Index not at beginning of buffer");
            }
        }
    }

    [TCProp("Buffer Chunk : operator BufferChunk(byte[] buffer) : check length")]
    public class BC_OpBC_ByteArray_GetLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = (BufferChunk)new byte[1];
            if(bc.Length != 1)
            {
                throw new TestCaseException("length is invalid");
            }
        }
    }


    #endregion OpBC ByteArray
    
    #region OpPlus BufferChunk
    
    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, BufferChunk source ) : null")]
    public class BC_OpPlus_BC_Null : TestCase
    {
        override public void Run()
        {
            BufferChunk src  = null;
            BufferChunk dest = new BufferChunk(new byte[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, 0, 5);

            try
            {
                dest = dest + src;
                throw new TestCaseException("argument can't be null");
            }
            catch(ArgumentNullException){}
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, BufferChunk source ) : no data")]
    public class BC_OpPlus_BC_NoData : TestCase
    {
        override public void Run()
        {
            BufferChunk src  = new BufferChunk(new byte[]{1, 2, 3, 4, 5}, 0, 0);
            BufferChunk dest = new BufferChunk(new byte[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, 0, 5);

            try
            {
                dest = dest + src;
                throw new TestCaseException("no data");
            }
            catch(BufferChunk.NoDataException){}
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, BufferChunk source ) : inadequate room in destination")]
    public class BC_OpPlus_BC_NoRoom : TestCase
    {
        override public void Run()
        {
            BufferChunk src  = new BufferChunk(new byte[]{5, 4, 3, 2, 1});
            BufferChunk dest = new BufferChunk(new byte[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, 0, 6);

            try
            {
                dest = dest + src;
                throw new TestCaseException("there should be inadequate room in the buffer");
            }
            catch(BufferChunk.InsufficientSpaceException){}
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, BufferChunk source ) : +, adequate room in destination")]
    public class BC_OpPlus_BC_Room : TestCase
    {
        override public void Run()
        {
            BufferChunk src  = new BufferChunk(new byte[]{5, 4, 3, 2, 1});
            BufferChunk dest = new BufferChunk(new byte[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, 0, 5);

            dest = dest + src;

            // Validate length
            if(dest.Length != 10)
            {
                throw new TestCaseException("BufferChunk should be length 10");
            }

            // Validate contents
            byte[] combo = new byte[]{1, 2, 3, 4, 5, 5, 4, 3, 2, 1};
            for(int i = 0; i < 10; i++)
            {
                if(combo[i] != dest.Buffer[i])
                {
                    throw new TestCaseException("bytes don't match");
                }
            }
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, BufferChunk source ) : +=, adequate room in destination")]
    public class BC_OpPlus_BC_PlusEqualRoom : TestCase
    {
        override public void Run()
        {
            BufferChunk src  = new BufferChunk(new byte[]{5, 4, 3, 2, 1});
            BufferChunk dest = new BufferChunk(new byte[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, 0, 5);

            dest += src;

            // Validate length
            if(dest.Length != 10)
            {
                throw new TestCaseException("BufferChunk should be length 10");
            }

            // Validate contents
            byte[] combo = new byte[]{1, 2, 3, 4, 5, 5, 4, 3, 2, 1};
            for(int i = 0; i < 10; i++)
            {
                if(combo[i] != dest.Buffer[i])
                {
                    throw new TestCaseException("bytes don't match");
                }
            }
        }
    }

    
    #endregion OpPlus BufferChunk
    
    #region OpPlus Byte
    
    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, byte b ) : 0 bytes room in destination")]
    public class BC_OpPlus_Byte_0BytesRoom : TestCase
    {
        override public void Run()
        {
            byte src = 1;
            BufferChunk dest = new BufferChunk(new byte[]{1});

            try
            {
                dest = dest + src;
                throw new TestCaseException("there should be inadequate room in the buffer");
            }
            catch(BufferChunk.InsufficientSpaceException){}
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, byte b ) : 1 byte room in destination")]
    public class BC_OpPlus_Byte_1ByteRoom : TestCase
    {
        override public void Run()
        {
            byte src = 1;
            BufferChunk dest = new BufferChunk(new byte[]{0}, 0, 0);
            dest = dest + src;
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, byte b ) : 2 bytes room in destination")]
    public class BC_OpPlus_Byte_2BytesRoom : TestCase
    {
        override public void Run()
        {
            byte src = 1;
            BufferChunk dest = new BufferChunk(new byte[]{1, 2}, 0, 0);
            dest = dest + src;
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, byte b ) : +=, adequate room in destination")]
    public class BC_OpPlus_Byte_PlusEqualRoom : TestCase
    {
        override public void Run()
        {
            byte src = 1;
            BufferChunk dest = new BufferChunk(new byte[]{1, 2, 3}, 0, 2);

            dest += src;

            // Validate length
            if(dest.Length != 3)
            {
                throw new TestCaseException("BufferChunk should be length 3");
            }

            // Validate contents
            byte[] combo = new byte[]{1, 2, 1};
            for(int i = 0; i < combo.Length; i++)
            {
                if(combo[i] != dest.Buffer[i])
                {
                    throw new TestCaseException("bytes don't match");
                }
            }
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, byte b ) : min byte")]
    public class BC_OpPlus_Byte_MinByte : TestCase
    {
        override public void Run()
        {
            BufferChunk dest = new BufferChunk(1);
            dest += byte.MinValue;

            if(dest.Buffer[0] != 0)
            {
                throw new TestCaseException("unexpected byte");
            }
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, byte b ) : max byte")]
    public class BC_OpPlus_Byte_MaxByte : TestCase
    {
        override public void Run()
        {
            BufferChunk dest = new BufferChunk(1);
            dest += byte.MaxValue;

            if(dest.Buffer[0] != 255)
            {
                throw new TestCaseException("unexpected byte");
            }
        }
    }
    
    
    #endregion OpPlus Byte
    
    #region OpPlus Short
    
    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, short s ) : 1 byte room in destination")]
    public class BC_OpPlus_Short_1ByteRoom : TestCase
    {
        override public void Run()
        {
            short src = 1;
            BufferChunk dest = new BufferChunk(new byte[]{1, 2}, 0, 1);

            try
            {
                dest = dest + src;
                throw new TestCaseException("there should be inadequate room in the buffer");
            }
            catch(BufferChunk.InsufficientSpaceException){}
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, short s ) : 2 bytes room in destination")]
    public class BC_OpPlus_Short_2BytesRoom : TestCase
    {
        override public void Run()
        {
            short src = 1;
            BufferChunk dest = new BufferChunk(new byte[]{1, 2}, 0, 0);
            dest = dest + src;
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, short s ) : 3 bytes room in destination")]
    public class BC_OpPlus_Short_3BytesRoom : TestCase
    {
        override public void Run()
        {
            short src = 1;
            BufferChunk dest = new BufferChunk(new byte[]{1, 2, 3}, 0, 0);
            dest = dest + src;
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, short s ) : +=, adequate room in destination")]
    public class BC_OpPlus_Short_PlusEqualRoom : TestCase
    {
        override public void Run()
        {
            short src = 1;
            BufferChunk dest = new BufferChunk(2);

            dest = dest + src;

            // Validate length
            if(dest.Length != 2)
            {
                throw new TestCaseException("unexpected length");
            }

            // Validate contents
            byte[] buffer = new byte[]{};
            for(int i = 0; i < buffer.Length; i++)
            {
                if(buffer[i] != dest.Buffer[i])
                {
                    throw new TestCaseException("bytes don't match");
                }
            }
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, short s ) : min byte")]
    public class BC_OpPlus_Short_MinByte : TestCase
    {
        override public void Run()
        {
            BufferChunk dest = new BufferChunk(2);
            dest += (short)byte.MinValue;
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, short s ) : max byte")]
    public class BC_OpPlus_Short_MaxByte : TestCase
    {
        override public void Run()
        {
            BufferChunk dest = new BufferChunk(2);
            dest += (short)byte.MaxValue;
        }
    }
    
    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, short s ) : min short")]
    public class BC_OpPlus_Short_MinShort : TestCase
    {
        override public void Run()
        {
            BufferChunk dest = new BufferChunk(2);
            dest += short.MinValue;
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, short s ) : max short")]
    public class BC_OpPlus_Short_MaxShort : TestCase
    {
        override public void Run()
        {
            BufferChunk dest = new BufferChunk(2);
            dest += short.MaxValue;
        }
    }
    
    
    #endregion OpPlus Short
    
    #region OpPlus Int
    
    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, int i ) : 3 bytes room in destination")]
    public class BC_OpPlus_Int_3BytesRoom : TestCase
    {
        override public void Run()
        {
            int src = 1;
            BufferChunk dest = new BufferChunk(new byte[]{1, 2, 3}, 0, 0);

            try
            {
                dest = dest + src;
                throw new TestCaseException("there should be inadequate room in the buffer");
            }
            catch(BufferChunk.InsufficientSpaceException){}
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, int i ) : 4 bytes room in destination")]
    public class BC_OpPlus_Int_4BytesRoom : TestCase
    {
        override public void Run()
        {
            int src = 1;
            BufferChunk dest = new BufferChunk(new byte[]{1, 2, 3, 4}, 0, 0);
            dest = dest + src;
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, int i ) : 5 bytes room in destination")]
    public class BC_OpPlus_Int_5BytesRoom : TestCase
    {
        override public void Run()
        {
            int src = 1;
            BufferChunk dest = new BufferChunk(new byte[]{1, 2, 3, 4, 5}, 0, 0);
            dest = dest + src;
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, int i ) : +=, adequate room in destination")]
    public class BC_OpPlus_Int_PlusEqualRoom : TestCase
    {
        override public void Run()
        {
            int src = 1;
            BufferChunk dest = new BufferChunk(4);

            dest += src;

            // Validate length
            if(dest.Length != 4)
            {
                throw new TestCaseException("unexpected length");
            }
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, int i ) : min byte")]
    public class BC_OpPlus_Int_MinByte : TestCase
    {
        override public void Run()
        {
            BufferChunk dest = new BufferChunk(4);
            dest += (int)byte.MinValue;
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, int i ) : max byte")]
    public class BC_OpPlus_Int_MaxByte : TestCase
    {
        override public void Run()
        {
            BufferChunk dest = new BufferChunk(4);
            dest += (int)byte.MaxValue;
        }
    }
    
    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, int i ) : min short")]
    public class BC_OpPlus_Int_MinShort : TestCase
    {
        override public void Run()
        {
            BufferChunk dest = new BufferChunk(4);
            dest += (int)short.MinValue;
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, int i ) : max short")]
    public class BC_OpPlus_Int_MaxShort : TestCase
    {
        override public void Run()
        {
            BufferChunk dest = new BufferChunk(4);
            dest += (int)short.MaxValue;
        }
    }
    
    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, int i ) : min int")]
    public class BC_OpPlus_Int_MinInt : TestCase
    {
        override public void Run()
        {
            BufferChunk dest = new BufferChunk(4);
            dest += int.MinValue;
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, int i ) : max int")]
    public class BC_OpPlus_Int_MaxInt : TestCase
    {
        override public void Run()
        {
            BufferChunk dest = new BufferChunk(4);
            dest += int.MaxValue;
        }
    }
    
    
    #endregion OpPlus Int
    
    #region OpPlus String

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, string s ) : null")]
    public class BC_OpPlus_String_Null : TestCase
    {
        override public void Run()
        {
            string src = null;
            BufferChunk dest = new BufferChunk(new byte[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, 0, 5);

            try
            {
                dest = dest + src;
                throw new TestCaseException("argument can't be null");
            }
            catch(ArgumentNullException){}
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, string s ) : empty")]
    public class BC_OpPlus_String_NoData : TestCase
    {
        override public void Run()
        {
            string src = "";
            BufferChunk dest = new BufferChunk(new byte[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, 0, 5);

            try
            {
                dest = dest + src;
                throw new TestCaseException("no data");
            }
            catch(BufferChunk.NoDataException){}
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, string s ) : inadequate room in destination")]
    public class BC_OpPlus_String_NoRoom : TestCase
    {
        override public void Run()
        {
            string src  = "Jason";
            BufferChunk dest = new BufferChunk(new byte[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, 0, 6);

            try
            {
                dest += src;
                throw new TestCaseException("there should be inadequate room in the buffer");
            }
            catch(BufferChunk.InsufficientSpaceException){}
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, string s ) : +, adequate room in destination")]
    public class BC_OpPlus_String_Room : TestCase
    {
        override public void Run()
        {
            string src  = "Jason";
            BufferChunk dest = new BufferChunk(new byte[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, 0, 5);
            dest = dest + src;

            // Validate length
            if(dest.Length != 10)
            {
                throw new TestCaseException("BufferChunk should be length 10");
            }

            // Validate contents
            byte[] combo = new byte[]{1, 2, 3, 4, 5, 74, 97, 115, 111, 110};
            for(int i = 0; i < 10; i++)
            {
                if(combo[i] != dest.Buffer[i])
                {
                    throw new TestCaseException("bytes don't match");
                }
            }
        }
    }

    [TCProp("Buffer Chunk : operator+ ( BufferChunk destination, string s ) : +=, adequate room in destination")]
    public class BC_OpPlus_String_PlusEqualRoom : TestCase
    {
        override public void Run()
        {
            string src  = "Jason";
            BufferChunk dest = new BufferChunk(new byte[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, 0, 5);
            dest += src;

            // Validate length
            if(dest.Length != 10)
            {
                throw new TestCaseException("BufferChunk should be length 10");
            }

            // Validate contents
            byte[] combo = new byte[]{1, 2, 3, 4, 5, 74, 97, 115, 111, 110};
            for(int i = 0; i < 10; i++)
            {
                if(combo[i] != dest.Buffer[i])
                {
                    throw new TestCaseException("bytes don't match");
                }
            }
        }
    }

    
    #endregion OpPlus String
    
    #endregion Operators & Casts (to/from byte[] & string)

    #region Indexer

    [TCProp("Buffer Chunk : Indexer(get) [int index] : no data")]
    public class BC_IndexerGet_NoData : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5}, 0, 0);

            try
            {
                byte data = bc[0];
                throw new TestCaseException("no data");
            }
            catch(IndexOutOfRangeException){}
        }
    }

    [TCProp("Buffer Chunk : Indexer(get) [int index] : -1")]
    public class BC_IndexerGet_Neg1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5});

            try
            {
                if(bc[-1] == 0){}
                throw new TestCaseException("index can not be < 0");
            }
            catch(IndexOutOfRangeException){}
        }
    }

    [TCProp("Buffer Chunk : Indexer(get) [int index] : 0")]
    public class BC_IndexerGet_0 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5});

            if(bc[0] != 1)
            {
                throw new TestCaseException("byte doesn't match");
            }
        }
    }

    [TCProp("Buffer Chunk : Indexer(get) [int index] : 1")]
    public class BC_IndexerGet_1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5});

            if(bc[1] != 2)
            {
                throw new TestCaseException("index can not be < 0");
            }
        }
    }

    [TCProp("Buffer Chunk : Indexer(get) [int index] : < length")]
    public class BC_IndexerGet_1LTDataLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 0, 0}, 0, 3);
            byte data = bc[2];
        }
    }

    [TCProp("Buffer Chunk : Indexer(get) [int index] : == length")]
    public class BC_IndexerGet_EQDataLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 0, 0}, 0 ,3);

            try
            {
                byte data = bc[3];
                throw new TestCaseException("index can not be == length");
            }
            catch(IndexOutOfRangeException){}
        }
    }

    [TCProp("Buffer Chunk : Indexer(get) [int index] : > length")]
    public class BC_IndexerGet_1GTDataLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 0, 0}, 0, 3);

            try
            {
                byte data = bc[4];
                throw new TestCaseException("index can not be > length");
            }
            catch(IndexOutOfRangeException){}
        }
    }

    [TCProp("Buffer Chunk : Indexer(get) [int index] : < buffer.Length")]
    public class BC_IndexerGet_1LTBufferLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5});
            byte data = bc[4];
        }
    }

    [TCProp("Buffer Chunk : Indexer(get) [int index] : == buffer.Length")]
    public class BC_IndexerGet_EQBufferLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5});

            try
            {
                byte data = bc[5];
                throw new TestCaseException("index can not be == buffer.Length");
            }
            catch(IndexOutOfRangeException){}
        }
    }

    [TCProp("Buffer Chunk : Indexer(get) [int index] : > buffer.Length")]
    public class BC_IndexerGet_1GTBufferLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5});

            try
            {
                byte data = bc[6];
                throw new TestCaseException("index can not be > buffer.Length");
            }
            catch(IndexOutOfRangeException){}
        }
    }

    [TCProp("Buffer Chunk : Indexer(get) [int index] : get byte")]
    public class BC_IndexerGet_GetByte : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5});

            if(bc[2] != 3)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }

    [TCProp("Buffer Chunk : Indexer(set) [int index] : no data")]
    public class BC_IndexerSet_NoData : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5}, 0, 0);

            try
            {
                bc[0] = 1;
                throw new TestCaseException("no data");
            }
            catch(IndexOutOfRangeException){}
        }
    }

    [TCProp("Buffer Chunk : Indexer(set) [int index] : -1")]
    public class BC_IndexerSet_Neg1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5});

            try
            {
                bc[-1] = 0;
                throw new TestCaseException("index can not be < 0");
            }
            catch(IndexOutOfRangeException){}
        }
    }

    [TCProp("Buffer Chunk : Indexer(set) [int index] : 0")]
    public class BC_IndexerSet_0 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5});
            bc[0] = 0;
        }
    }

    [TCProp("Buffer Chunk : Indexer(set) [int index] : 1")]
    public class BC_IndexerSet_1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5});
            bc[1] = 1;
        }
    }

    [TCProp("Buffer Chunk : Indexer(set) [int index] : < length")]
    public class BC_IndexerSet_1LTDataLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 0, 0}, 0, 3);
            bc[2] = 2;
        }
    }

    [TCProp("Buffer Chunk : Indexer(set) [int index] : == length")]
    public class BC_IndexerSet_EQDataLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 0, 0}, 0, 3);
            
            try
            {
                bc[3] = 3;
                throw new TestCaseException("index must < data length");
            }
            catch(IndexOutOfRangeException){}
        }
    }

    [TCProp("Buffer Chunk : Indexer(set) [int index] : > length")]
    public class BC_IndexerSet_1GTDataLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 0, 0}, 0, 3);

            try
            {
                bc[4] = 0;
                throw new TestCaseException("index must be < length");
            }
            catch(IndexOutOfRangeException){}
        }
    }

    [TCProp("Buffer Chunk : Indexer(set) [int index] : < buffer.Length")]
    public class BC_IndexerSet_1LTBufferLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5});
            bc[4] = 4;
        }
    }

    [TCProp("Buffer Chunk : Indexer(set) [int index] : == buffer.Length")]
    public class BC_IndexerSet_EQBufferLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5});

            try
            {
                bc[5] = 5;
                throw new TestCaseException("index must be < buffer.Length");
            }
            catch(IndexOutOfRangeException){}
        }
    }

    [TCProp("Buffer Chunk : Indexer(set) [int index] : > buffer.Length")]
    public class BC_IndexerSet_1GTBufferLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5});

            try
            {
                bc[6] = 6;
                throw new TestCaseException("index must be < buffer.Length");
            }
            catch(IndexOutOfRangeException){}
        }
    }

    [TCProp("Buffer Chunk : Indexer(set) [int index] : set byte")]
    public class BC_IndexerSet_SetByte : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3});
            bc[0] = 3;

            if(bc[0] != 3)
            {
                throw new TestCaseException("byte doesn't match");
            }
        }
    }


    #endregion Indexer
    
    #region Clone

    // Jay says its OK to return a BC with no data in the Clone call
    // [TCProp("Buffer Chunk : Clone() : no data")]

    [TCProp("Buffer Chunk : Clone() : check index")]
    public class BC_Clone_GetIndex : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3});
            BufferChunk clone = (BufferChunk)bc.Clone();

            if(clone.Index != bc.Index)
            {
                throw new TestCaseException("indexes don't match");
            }
        }
    }

    [TCProp("Buffer Chunk : Clone() : check length")]
    public class BC_Clone_GetLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3});
            BufferChunk clone = (BufferChunk)bc.Clone();

            if(clone.Length != bc.Length)
            {
                throw new TestCaseException("lengths don't match");
            }
        }
    }

    [TCProp("Buffer Chunk : Clone() : check buffer")]
    public class BC_Clone_GetBuffer : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3});
            BufferChunk clone = (BufferChunk)bc.Clone();

            if(clone.Buffer != bc.Buffer)
            {
                throw new TestCaseException("buffers don't match");
            }
        }
    }

    
    #endregion Clone

    #region Dispose / Finalizer

    // I'm not convinced this class needs a Dispose / Finalizer
    // It does not contain unmanaged resources, nor expensive resources
    // (like a database connection).  It contains POD.
    // Finalizer causes extra work for the runtime.
    // Every method would need to have a check for being disposed which is
    // useless lost perf

    #endregion Dispose / Finalizer

    #region Public Methods

    #region CopyTo
    
    [TCProp("Buffer Chunk : CopyTo(BufferChunk destination, int index) : BC null")]
    public class BC_CopyTo_Null : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3});
            BufferChunk dest = null;

            try
            {
                bc.CopyTo(dest, 1);
                throw new TestCaseException("BufferChunk can not be null");
            }
            catch(ArgumentNullException){}
        }
    }

    [TCProp("Buffer Chunk : CopyTo(BufferChunk destination, int index) : no data")]
    public class BC_CopyTo_NoData : TestCase
    {
        override public void Run()
        {
            BufferChunk src = new BufferChunk(new byte[]{1, 2, 3}, 0, 0);
            BufferChunk dest = new BufferChunk(new byte[]{1, 2, 3, 4});

            try
            {
                src.CopyTo(dest, 1);
                throw new TestCaseException("no data");
            }
            catch(BufferChunk.NoDataException){}
        }
    }

    [TCProp("Buffer Chunk : CopyTo(BufferChunk bufferChunk, int index) : index -1")]
    public class BC_CopyTo_IndexNeg1 : TestCase
    {
        override public void Run()
        {
            BufferChunk src = new BufferChunk(new byte[]{1, 2, 3});

            try
            {
                src.CopyTo(new BufferChunk(5), -1);
                throw new TestCaseException("index can not be < 0");
            }
            catch(ArgumentException){}
        }
    }

    [TCProp("Buffer Chunk : CopyTo(BufferChunk bufferChunk, int index) : index 0")]
    public class BC_CopyTo_Index0 : TestCase
    {
        override public void Run()
        {
            BufferChunk dest = new BufferChunk(new byte[]{0, 0, 0, 4, 5});
            BufferChunk src = new BufferChunk(new byte[]{1, 2, 3});
            src.CopyTo(dest, 0);

            for(int i = 0; i < dest.Length; i++)
            {
                if(dest[i] != i + 1)
                {
                    throw new TestCaseException("bytes don't match");
                }
            }
        }
    }

    [TCProp("Buffer Chunk : CopyTo(BufferChunk bufferChunk, int index) : index 1")]
    public class BC_CopyTo_Index1 : TestCase
    {
        override public void Run()
        {
            BufferChunk dest = new BufferChunk(new byte[]{1, 0, 0, 0, 5});
            BufferChunk src = new BufferChunk(new byte[]{2, 3, 4});
            src.CopyTo(dest, 1);

            for(int i = 0; i < dest.Length; i++)
            {
                if(dest[i] != i + 1)
                {
                    throw new TestCaseException("bytes don't match");
                }
            }
        }
    }

    [TCProp("Buffer Chunk : CopyTo(BufferChunk bufferChunk, int index) : < dest.Length")]
    public class BC_CopyTo_1LTDataLength : TestCase
    {
        override public void Run()
        {
            BufferChunk dest = new BufferChunk(new byte[]{1, 0, 0, 0, 5, 0}, 0, 5);
            BufferChunk src = new BufferChunk(new byte[]{2, 3, 4});
            src.CopyTo(dest, 1);

            if(dest.Length != 5)
            {
                throw new TestCaseException("unexpected length");
            }

            for(int i = 0; i < dest.Length; i++)
            {
                if(dest[i] != i + 1)
                {
                    throw new TestCaseException("bytes don't match");
                }
            }
        }
    }

    [TCProp("Buffer Chunk : CopyTo(BufferChunk bufferChunk, int index) : == dest.Length")]
    public class BC_CopyTo_EQDataLength : TestCase
    {
        override public void Run()
        {
            BufferChunk dest = new BufferChunk(new byte[]{1, 2, 0, 0, 0, 0}, 0, 5);
            BufferChunk src = new BufferChunk(new byte[]{3, 4, 5});
            src.CopyTo(dest, 2);

            if(dest.Length != 5)
            {
                throw new TestCaseException("unexpected length");
            }

            for(int i = 0; i < dest.Length; i++)
            {
                if(dest[i] != i + 1)
                {
                    throw new TestCaseException("bytes don't match");
                }
            }
        }
    }

    [TCProp("Buffer Chunk : CopyTo(BufferChunk bufferChunk, int index) : > dest.Length")]
    public class BC_CopyTo_1GTDataLength : TestCase
    {
        override public void Run()
        {
            BufferChunk dest = new BufferChunk(new byte[]{1, 2, 3, 0, 0, 0}, 0, 5);
            BufferChunk src = new BufferChunk(new byte[]{4, 5, 6});

            try
            {
                src.CopyTo(dest, 3);
                throw new TestCaseException("length cannot change during CopyTo");
            }
            catch(BufferChunk.InsufficientSpaceException){}
        }
    }

    [TCProp("Buffer Chunk : CopyTo(BufferChunk bufferChunk, int index) : > buffer.Length")]
    public class BC_CopyTo_1LTBufferLength : TestCase
    {
        override public void Run()
        {
            BufferChunk src = new BufferChunk(new byte[]{1});
            BufferChunk dest = new BufferChunk(new byte[] {1, 2, 3, 4, 5});

            src.CopyTo(dest, 4);

            if(dest.Length != 5)
            {
                throw new TestCaseException("unexpected length");
            }

            if(dest[4] != 1)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }

    [TCProp("Buffer Chunk : CopyTo(BufferChunk bufferChunk, int index) : > buffer.Length")]
    public class BC_CopyTo_EQBufferLength : TestCase
    {
        override public void Run()
        {
            BufferChunk src = new BufferChunk(new byte[]{1});
            BufferChunk dest = new BufferChunk(new byte[] {1, 2, 3, 4, 5});

            try
            {
                src.CopyTo(dest, 5);
                throw new TestCaseException("index can not be == buffer.Length");
            }
            catch(BufferChunk.InsufficientSpaceException){}
        }
    }

    [TCProp("Buffer Chunk : CopyTo(BufferChunk bufferChunk, int index) : > buffer.Length")]
    public class BC_CopyTo_1GTBufferLength : TestCase
    {
        override public void Run()
        {
            BufferChunk src = new BufferChunk(new byte[]{1});
            BufferChunk dest = new BufferChunk(new byte[] {1, 2, 3, 4, 5});

            try
            {
                src.CopyTo(dest, 6);
                throw new TestCaseException("index can not be > buffer.Length");
            }
            catch(BufferChunk.InsufficientSpaceException){}
        }
    }

    [TCProp("Buffer Chunk : CopyTo(BufferChunk bufferChunk, int index) : shorten length")]
    public class BC_CopyTo_ShortenLength : TestCase
    {
        override public void Run()
        {
            BufferChunk src = new BufferChunk(new byte[]{1, 2, 3});
            BufferChunk dest = new BufferChunk(new byte[]{0, 0, 0, 4, 5});

            src.CopyTo(dest, 0);

            if(dest.Length == 3)
            {
                throw new TestCaseException("unexpected length");
            }

            for(int i = 0; i < 3; i++)
            {
                if(dest[i] != i + 1)
                {
                    throw new TestCaseException("bytes don't match");
                }
            }
        }
    }


    #endregion CopyTo

    #region Reset

    [TCProp("Buffer Chunk : Reset()")]
    public class BC_Reset : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3}, 1, 2);
            bc.Reset();

            if(bc.Index != 0)
            {
                throw new TestCaseException("unexpected index");
            }

            if(bc.Length != 0)
            {
                throw new TestCaseException("unexpected length");
            }
        }
    }

    [TCProp("Buffer Chunk : Reset(int index, int length) : index < 0")]
    public class BC_Reset_IndexNeg1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3}, 1, 2);

            try
            {
                bc.Reset(-1, 0);
                throw new TestCaseException("index cannot be < 0");
            }
            catch(ArgumentException){}
        }
    }
    
    [TCProp("Buffer Chunk : Reset(int index, int length) : index == 0")]
    public class BC_Reset_Index0 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3}, 1, 2);
            bc.Reset(0, 0);
        }
    }
    
    [TCProp("Buffer Chunk : Reset(int index, int length) : index > 0")]
    public class BC_Reset_Index1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3}, 1, 2);
            bc.Reset(1, 0);
        }
    }
    
    [TCProp("Buffer Chunk : Reset(int index, int length) : length < 0")]
    public class BC_Reset_LengthNeg1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3}, 1, 2);

            try
            {
                bc.Reset(0, -1);
                throw new TestCaseException("length cannot be < 0");
            }
            catch(ArgumentException){}
        }
    }
    
    [TCProp("Buffer Chunk : Reset(int index, int length) : length == 0")]
    public class BC_Reset_Length0 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3}, 1, 2);
            bc.Reset(0, 0);
        }
    }
    
    [TCProp("Buffer Chunk : Reset(int index, int length) : length > 0")]
    public class BC_Reset_Length1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3}, 1, 2);
            bc.Reset(0, 1);
        }
    }
    
    [TCProp("Buffer Chunk : Reset(int index, int length) : index + length > buffer.Length")]
    public class BC_Reset_IndexPlusLengthGTBufferLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3}, 1, 2);

            try
            {
                bc.Reset(2, 2);
                throw new TestCaseException("index + length > buffer.Length");
            }
            catch(ArgumentOutOfRangeException){}
        }
    }
    
    [TCProp("Buffer Chunk : Reset(int index, int length) : get index")]
    public class BC_Reset_GetIndex : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3}, 1, 2);
            bc.Reset(2, 1);

            if(bc.Index != 2)
            {
                throw new TestCaseException("unexpected index");
            }
        }
    }
    
    [TCProp("Buffer Chunk : Reset(int index, int length) : get length")]
    public class BC_Reset_GetLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3}, 1, 2);
            bc.Reset(2, 1);

            if(bc.Length != 1)
            {
                throw new TestCaseException("unexpected length");
            }
        }
    }
    

    #endregion Reset

    #region Peek

    [TCProp("Buffer Chunk : Peek(int index, int length) : no data in object")]
    public class BC_Peek_NoData : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5}, 0, 0);

            try
            {
                BufferChunk ret = bc.Peek(0, 1);
                throw new TestCaseException("no data");
            }
            catch(BufferChunk.NoDataException){}
        }
    }
    
    [TCProp("Buffer Chunk : Peek(int index, int length) : index < 0")]
    public class BC_Peek_IndexNeg1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3});

            try
            {
                BufferChunk ret = bc.Peek(-1, 0);
                throw new TestCaseException("index cannot be < 0");
            }
            catch(ArgumentException){}
        }
    }
    
    [TCProp("Buffer Chunk : Peek(int index, int length) : index == 0")]
    public class BC_Peek_Index0 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3});
            BufferChunk ret = bc.Peek(0, 1);
        }
    }
    
    [TCProp("Buffer Chunk : Peek(int index, int length) : index > 0")]
    public class BC_Peek_Index1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3});
            BufferChunk ret = bc.Peek(1, 1);
        }
    }
    
    [TCProp("Buffer Chunk : Peek(int index, int length) : length < 0")]
    public class BC_Peek_LengthNeg1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3});

            try
            {
                BufferChunk ret = bc.Peek(0, -1);
                throw new TestCaseException("length cannot be < 0");
            }
            catch(ArgumentException){}
        }
    }
    
    [TCProp("Buffer Chunk : Peek(int index, int length) : length == 0")]
    public class BC_Peek_Length0 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3});

            try
            {
                BufferChunk ret = bc.Peek(0, 0);
                throw new TestCaseException("can not operate on no data");
            }
            catch(BufferChunk.NoDataException){}
        }
    }
    
    [TCProp("Buffer Chunk : Peek(int index, int length) : length > 0")]
    public class BC_Peek_Length1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3});
            BufferChunk ret = bc.Peek(0, 1);
        }
    }
    
    [TCProp("Buffer Chunk : Peek(int index, int length) : index + length < this.Length")]
    public class BC_Peek_1LTDataLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5}, 1, 4);
            BufferChunk ret = bc.Peek(1, 2);
        }
    }
    
    [TCProp("Buffer Chunk : Peek(int index, int length) : index + length == this.Length")]
    public class BC_Peek_EQDataLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5}, 1, 4);
            BufferChunk ret = bc.Peek(2, 2);
        }
    }
    
    [TCProp("Buffer Chunk : Peek(int index, int length) : index + length > this.Length")]
    public class BC_Peek_1GTDataLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5}, 1, 4);

            try
            {
                BufferChunk ret = bc.Peek(2, 3);
                throw new TestCaseException("index + length cannot be > this.Length");
            }
            catch(BufferChunk.InsufficientDataException){}
        }
    }
    
    [TCProp("Buffer Chunk : Peek(int index, int length) : get index")]
    public class BC_Peek_GetIndex : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5}, 1, 3);
            BufferChunk ret = bc.Peek(1, 2);

            if(ret.Index != 2)
            {
                throw new TestCaseException("unexpected index");
            }
        }
    }
    
    [TCProp("Buffer Chunk : Peek(int index, int length) : get length")]
    public class BC_Peek_GetLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5}, 1, 3);
            BufferChunk ret = bc.Peek(1, 2);

            if(ret.Length != 2)
            {
                throw new TestCaseException("unexpected length");
            }
        }
    }
    
    [TCProp("Buffer Chunk : Peek(int index, int length) : get buffer")]
    public class BC_Peek_GetBuffer : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5}, 1, 3);
            BufferChunk ret = bc.Peek(1, 2);

            if(ret.Buffer != bc.Buffer)
            {
                throw new TestCaseException("buffers don't match");
            }
        }
    }
    

    #endregion Peek
    
    #region NextBufferChunk

    [TCProp("Buffer Chunk : NextBufferChunk(int length) : no data")]
    public class BC_NextBufferChunk_NoData : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3}, 0, 0);

            try
            {
                BufferChunk ret = bc.NextBufferChunk(1);
                throw new TestCaseException("no data");
            }
            catch(BufferChunk.NoDataException){}
        }
    }
    
    [TCProp("Buffer Chunk : NextBufferChunk(int length) : length < 0")]
    public class BC_NextBufferChunk_LengthNeg1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3});

            try
            {
                BufferChunk ret = bc.NextBufferChunk(-1);
                throw new TestCaseException("length cannot be < 0");
            }
            catch(ArgumentException){}
        }
    }
    
    [TCProp("Buffer Chunk : NextBufferChunk(int length) : length == 0")]
    public class BC_NextBufferChunk_Length0 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3});

            try
            {
                BufferChunk ret = bc.NextBufferChunk(0);
                throw new TestCaseException("no valid data");
            }
            catch(BufferChunk.NoDataException){}
        }
    }
    
    [TCProp("Buffer Chunk : NextBufferChunk(int length) : length > 0")]
    public class BC_NextBufferChunk_Length1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3});
            BufferChunk ret = bc.NextBufferChunk(1);
        }
    }
    
    [TCProp("Buffer Chunk : NextBufferChunk(int length) : length < this.Length")]
    public class BC_NextBufferChunk_1LTDataLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 0, 0}, 0, 3);
            BufferChunk ret = bc.NextBufferChunk(2);
        }
    }
    
    [TCProp("Buffer Chunk : NextBufferChunk(int length) : length == this.Length")]
    public class BC_NextBufferChunk_EQDataLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 0, 0}, 0, 3);
            BufferChunk ret = bc.NextBufferChunk(3);
        }
    }
    
    [TCProp("Buffer Chunk : NextBufferChunk(int length) : length > this.Length")]
    public class BC_NextBufferChunk_1GTDataLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 0, 0}, 0, 3);

            try
            {
                BufferChunk ret = bc.NextBufferChunk(4);
                throw new TestCaseException("length cannot be > this.Length");
            }
            catch(BufferChunk.InsufficientDataException){}
        }
    }
    
    [TCProp("Buffer Chunk : NextBufferChunk(int length) : get index of return BC")]
    public class BC_NextBufferChunk_GetRetIndex : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5}, 1, 4);
            BufferChunk ret = bc.NextBufferChunk(2);

            if(ret.Index != 1)
            {
                throw new TestCaseException("unexpected index");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextBufferChunk(int length) : get length of return BC")]
    public class BC_NextBufferChunk_GetRetLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5}, 1, 4);
            BufferChunk ret = bc.NextBufferChunk(2);

            if(ret.Length != 2)
            {
                throw new TestCaseException("unexpected index");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextBufferChunk(int length) : get buffer of return BC")]
    public class BC_NextBufferChunk_GetRetBuffer : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5}, 1, 4);
            BufferChunk ret = bc.NextBufferChunk(2);

            for(int i = 0; i < 2; i++)
            {
                if(ret[i] != i + 2)
                {
                    throw new TestCaseException("bytes don't match");
                }
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextBufferChunk(int length) : get index of original BC")]
    public class BC_NextBufferChunk_GetIndex : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5}, 1, 4);
            BufferChunk ret = bc.NextBufferChunk(2);

            if(bc.Index != 3)
            {
                throw new TestCaseException("unexpected index");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextBufferChunk(int length) : get length of original BC")]
    public class BC_NextBufferChunk_GetLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5}, 1, 4);
            BufferChunk ret = bc.NextBufferChunk(2);

            if(bc.Length != 2)
            {
                throw new TestCaseException("unexpected index");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextBufferChunk(int length) : get buffer of original BC")]
    public class BC_NextBufferChunk_GetBuffer : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4, 5}, 1, 4);
            BufferChunk ret = bc.NextBufferChunk(2);

            for(int i = 0; i < 2; i++)
            {
                if(bc[i] != i + 4)
                {
                    throw new TestCaseException("bytes don't match");
                }
            }
        }
    }
    
    
    #endregion NextBufferChunk
    
    #region NextByte

    [TCProp("Buffer Chunk : NextByte() : no data left")]
    public class BC_NextByte_NoData : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1}, 0, 0);
            
            try
            {
                bc.NextByte();
                throw new TestCaseException("no data");
            }
            catch(BufferChunk.NoDataException){}
        }
    }
    
    [TCProp("Buffer Chunk : NextByte() : 1 byte left")]
    public class BC_NextByte_1Byte : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1}, 0, 1);
            bc.NextByte();
        }
    }
    
    [TCProp("Buffer Chunk : NextByte() : 2 bytes left")]
    public class BC_NextByte_2Bytes : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2}, 0, 2);
            bc.NextByte();
        }
    }
    
    [TCProp("Buffer Chunk : NextByte() : get 0")]
    public class BC_NextByte_0 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(2);
            bc += (byte)0;

            if(bc.NextByte() != 0)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextByte() : get min byte")]
    public class BC_NextByte_MinByte : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(2);
            bc += byte.MinValue;

            if(bc.NextByte() != byte.MinValue)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextByte() : get max byte")]
    public class BC_NextByte_MaxByte : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(2);
            bc += byte.MaxValue;

            if(bc.NextByte() != byte.MaxValue)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextByte() : get byte")]
    public class BC_NextByte_GetByte : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4}, 1, 2);
            
            if(bc.NextByte() != 2)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextByte() : get index")]
    public class BC_NextByte_GetIndex : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4}, 1, 2);
            byte ret = bc.NextByte();

            if(bc.Index != 2)
            {
                throw new TestCaseException("unexpected index");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextByte() : get length")]
    public class BC_NextByte_GetLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3, 4}, 1, 2);
            byte ret = bc.NextByte();

            if(bc.Length != 1)
            {
                throw new TestCaseException("unexpected length");
            }
        }
    }

    
    #endregion NextByte
    
    #region NextInt16

    [TCProp("Buffer Chunk : NextInt16() : no data left")]
    public class BC_NextInt16_NoData : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(2);
            
            try
            {
                bc.NextInt16();
                throw new TestCaseException("no data");
            }
            catch(BufferChunk.NoDataException){}
        }
    }
    
    [TCProp("Buffer Chunk : NextInt16() : 1 Byte left")]
    public class BC_NextInt16_1Byte : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[1]);

            try
            {
                bc.NextInt16();
                throw new TestCaseException("not enough data");
            }
            catch(BufferChunk.InsufficientDataException)
            {
                if(bc.Length != 1)
                {
                    throw new TestCaseException("lost data");
                }
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextInt16() : 2 Bytes left")]
    public class BC_NextInt16_2Bytes : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[2]);
            bc.NextInt16();
        }
    }
    
    [TCProp("Buffer Chunk : NextInt16() : 3 Bytes left")]
    public class BC_NextInt16_3Bytes : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[3]);
            bc.NextInt16();
        }
    }
    
    [TCProp("Buffer Chunk : NextInt16() : get 0")]
    public class BC_NextInt16_0 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(2);
            bc += (short)0;

            if(bc.NextInt16() != 0)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextInt16() : get min byte")]
    public class BC_NextInt16_MinByte : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(2);
            bc += (short)byte.MinValue;

            if(bc.NextInt16() != (short)byte.MinValue)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextInt16() : get max byte")]
    public class BC_NextInt16_MaxByte : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(2);
            bc += (short)byte.MaxValue;

            if(bc.NextInt16() != (short)byte.MaxValue)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextInt16() : get min short")]
    public class BC_NextInt16_MinShort : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(2);
            bc += short.MinValue;

            if(bc.NextInt16() != short.MinValue)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextInt16() : get max short")]
    public class BC_NextInt16_MaxShort : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(2);
            bc += short.MaxValue;

            if(bc.NextInt16() != short.MaxValue)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextInt16() : get Short")]
    public class BC_NextInt16_GetShort : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(2);
            bc += (short)1;
            
            if(bc.NextInt16() != (short)1)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextInt16() : get index")]
    public class BC_NextInt16_GetIndex : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(4);
            bc += (short)1;
            bc += (short)2;

            short ret = bc.NextInt16();

            if(bc.Index != 2)
            {
                throw new TestCaseException("unexpected index");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextInt16() : get length")]
    public class BC_NextInt16_GetLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(4);
            bc += (short)1;
            bc += (short)2;

            short ret = bc.NextInt16();

            if(bc.Length != 2)
            {
                throw new TestCaseException("unexpected index");
            }
        }
    }

    
    #endregion NextInt16
    
    #region NextInt32

    [TCProp("Buffer Chunk : NextInt32() : no data left")]
    public class BC_NextInt_NoData : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(4);
            
            try
            {
                bc.NextInt32();
                throw new TestCaseException("no data");
            }
            catch(BufferChunk.NoDataException){}
        }
    }
    
    [TCProp("Buffer Chunk : NextInt32() : 1 byte left")]
    public class BC_NextInt_3Byte : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{1, 2, 3});

            try
            {
                bc.NextInt32();
                throw new TestCaseException("not enough data");
            }
            catch(BufferChunk.InsufficientDataException)
            {
                if(bc.Length != 3)
                {
                    throw new TestCaseException("lost data");
                }
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextInt32() : 4 bytes left")]
    public class BC_NextInt_4Bytes : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[4]);
            bc.NextInt32();
        }
    }
    
    [TCProp("Buffer Chunk : NextInt32() : 5 bytes left")]
    public class BC_NextInt_5Bytes : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[5]);
            bc.NextInt32();
        }
    }
    
    [TCProp("Buffer Chunk : NextInt32() : get 0")]
    public class BC_NextInt_0 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(4);
            bc += (int)0;

            if(bc.NextInt32() != 0)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextInt32() : get min byte")]
    public class BC_NextInt_MinByte : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(4);
            bc += (int)byte.MinValue;

            if(bc.NextInt32() != (int)byte.MinValue)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextInt32() : get max byte")]
    public class BC_NextInt_MaxByte : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(4);
            bc += (int)byte.MaxValue;

            if(bc.NextInt32() != (int)byte.MaxValue)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextInt32() : get min short")]
    public class BC_NextInt_MinShort : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(4);
            bc += (int)short.MinValue;

            if(bc.NextInt32() != (int)short.MinValue)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextInt32() : get max short")]
    public class BC_NextInt_MaxShort : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(4);
            bc += (int)short.MaxValue;

            if(bc.NextInt32() != (int)short.MaxValue)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextInt32() : get min Int")]
    public class BC_NextInt_MinInt : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(4);
            bc += int.MinValue;

            if(bc.NextInt32() != int.MinValue)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextInt32() : get max Int")]
    public class BC_NextInt_MaxInt : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(4);
            bc += int.MaxValue;

            if(bc.NextInt32() != int.MaxValue)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextInt32() : get Int")]
    public class BC_NextInt_GetInt : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(4);
            bc += (int)1;
            
            if(bc.NextInt32() != 1)
            {
                throw new TestCaseException("bytes don't match");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextInt32() : get index")]
    public class BC_NextInt_GetIndex : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(8);
            bc += (int)1;
            bc += (int)2;

            int ret = bc.NextInt32();

            if(bc.Index != 4)
            {
                throw new TestCaseException("unexpected index");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextInt32() : get length")]
    public class BC_NextInt_GetLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(8);
            bc += (int)1;
            bc += (int)2;

            int ret = bc.NextInt32();

            if(bc.Length != 4)
            {
                throw new TestCaseException("unexpected index");
            }
        }
    }

    
    #endregion NextInt32
    
    #region NextUTF8String
    
    // I don't think I am qualified to test the complexities of unicode strings
    // including multibyte (Surrogate Pairs)
    // Since length could be passed in as an accidental off by 1, and we may only
    // get part of a string, or even part of a character

    [TCProp("Buffer Chunk : NextUtf8String(int length) : no data")]
    public class BC_NextUtf8String_NoData : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = new BufferChunk(new byte[]{65, 66, 67}, 0, 0);

            try
            {
                string data = bc.NextUtf8String(2);
                throw new TestCaseException("no data");
            }
            catch(BufferChunk.NoDataException){}
        }
    }
    
    // I don't think I am qualified to test the complexities of unicode strings
    // including multibyte (Surrogate Pairs)
    // Since length could be passed in as an accidental off by 1, and we may only
    // get part of a string, or even part of a character

    [TCProp("Buffer Chunk : NextUtf8String(int length) : -1")]
    public class BC_NextUtf8String_LengthNeg1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = (BufferChunk)"Jason";

            try
            {
                string data = bc.NextUtf8String(-1);
                throw new TestCaseException("invalid length");
            }
            catch(OverflowException){}
        }
    }
    
    [TCProp("Buffer Chunk : NextUtf8String(int length) : 0")]
    public class BC_NextUtf8String_Length0 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = (BufferChunk)"Jason";

            try
            {
                string data = bc.NextUtf8String(0);
                throw new TestCaseException("invalid length");
            }
            catch(BufferChunk.NoDataException){}
        }
    }
    
    [TCProp("Buffer Chunk : NextUtf8String(int length) : 1")]
    public class BC_NextUtf8String_Length1 : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = (BufferChunk)"Jason";
            string data = bc.NextUtf8String(1);

            if(data != "J")
            {
                throw new TestCaseException("unexpected data");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextUtf8String(int length) : length < datalength")]
    public class BC_NextUtf8String_1LTDataLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = (BufferChunk)"Jason";
            string data = bc.NextUtf8String(4);

            if(data != "Jaso")
            {
                throw new TestCaseException("unexpected data");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextUtf8String(int length) : length == datalength")]
    public class BC_NextUtf8String_EQDataLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = (BufferChunk)"Jason";
            string data = bc.NextUtf8String(5);

            if(data != "Jason")
            {
                throw new TestCaseException("unexpected data");
            }
        }
    }
    
    [TCProp("Buffer Chunk : NextUtf8String(int length) : length < datalength")]
    public class BC_NextUtf8String_1GTDataLength : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = (BufferChunk)"Jason";

            try
            {
                string data = bc.NextUtf8String(6);
                throw new TestCaseException("length too long");
            }
            catch(BufferChunk.InsufficientDataException){}
        }
    }
    
    [TCProp("Buffer Chunk : NextUtf8String(int length) : get string")]
    public class BC_NextUtf8String_GetString : TestCase
    {
        override public void Run()
        {
            BufferChunk bc = (BufferChunk)"Jason";
            string data = bc.NextUtf8String(5);

            if(data != "Jason")
            {
                throw new TestCaseException("unexpected data");
            }
        }
    }
    

    #endregion NextUTF8String

    #endregion Public Methods
    
    #endregion BufferChunk Test Cases

}
