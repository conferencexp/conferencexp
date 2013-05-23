// ------------------------------------------------------------------------------------------------
// This file supplements the types that are not available via an IDL file, i.e. they are in header
// files, either from DX or the OS.
//
// A major portion is the GUIDs from DX\Include\uuids.h and ksuuids.h for MajorTypes, FormatTypes 
// and SubTypes.  We use reflection to discover the Guids in each type, and convert the name from
// something like MEDIATYPE_Video ==> Video

// WMVideo Encoder Note: 
// WMVideo Encoder DMO (a.k.a. Windows Media 7) uses the WMV1 fourCC
// WMVideo9 Encoder DMO (a.k.a. Windows Media 9) uses the WMV3 fourCC
// WMVideo Advanced Encoder DMO (a.k.a. Windows Media 10 / 9.5 SDK) uses the WMVA fourCC
//
// IMPORTANT:
// _AMMediaType is a struct, so it gets copied by value and boxed in a collection
// Be careful about how you store it and when you free its pbFormat block
// ------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;


namespace MSR.LST.MDShow
{
    public class MediaType
    {
        /// <summary>
        /// Frees the pbFormat pointer of the _AMMediaType
        /// </summary>
        public static void Free(ref _AMMediaType mt)
        {
            if(mt.pbFormat != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(mt.pbFormat);
                mt.pbFormat = IntPtr.Zero;
                mt.cbFormat = 0;
            }
        }

        /// <summary>
        /// Frees the pbFormat pointer of the _AMMediaTypes in the collection
        /// </summary>
        public static void Free(_AMMediaType[] mts)
        {
            for(int i = 0; i < mts.Length; i++)
            {
                Free(ref mts[i]);
            }
        }

        /// <summary>
        /// Converts the unmanaged IntPtr to the _AMMediaType into the _AMMediaType struct and the
        /// format block. Frees pmt and the _AMMediaType's pbFormat pointer.
        /// Note: If there was extra data at the end of the format block, it is lost
        /// </summary>
        public static void MarshalData(ref IntPtr pmt, out _AMMediaType mt, out object formatBlock)
        {
                MarshalData(ref pmt, out mt);
                MarshalData(ref mt, out formatBlock);
        }

        /// <summary>
        /// Converts the unmanaged IntPtr to the _AMMediaType into the _AMMediaType struct.
        /// Frees pmt but not the _AMMediaType's pbFormat pointer; it is left in tact and will need 
        /// to be freed later.
        /// </summary>
        public static void MarshalData(ref IntPtr pmt, out _AMMediaType mt)
        {
            mt  = (_AMMediaType)Marshal.PtrToStructure(pmt, typeof(_AMMediaType));
            Marshal.FreeCoTaskMem(pmt);
            pmt = IntPtr.Zero;
        }

        /// <summary>
        /// Extracts the format block (pbFormat) from the _AMMediaType.
        /// Note: if there is extra data at the end of pbFormat, it is lost
        /// </summary>
        public static void MarshalData(ref _AMMediaType mt, out object formatBlock)
        {
            formatBlock = FormatType.MarshalData(mt);
            Free(ref mt);
        }

        /// <summary>
        /// Takes an _AMMediaType and a format block and reconstructs the pbFormat pointer
        /// </summary>
        public static _AMMediaType Construct(_AMMediaType mt, object formatBlock)
        {
            System.Diagnostics.Debug.Assert(mt.pbFormat == IntPtr.Zero && mt.cbFormat == 0);

            int size = Marshal.SizeOf(formatBlock);
            mt.pbFormat = Marshal.AllocCoTaskMem(size);
            mt.cbFormat = (uint)size;
            Marshal.StructureToPtr(formatBlock, mt.pbFormat, false);

            return mt;
        }

        /// <summary>
        /// Turns the _AMMediaType into a string representation
        /// </summary>
        public static string Dump(_AMMediaType mt)
        {
            string ret = "\r\nMedia Type\r\n";

            ret += string.Format(CultureInfo.InvariantCulture, "\tbFixedSizeSamples :  {0}\r\n", mt.bFixedSizeSamples);
            ret += string.Format(CultureInfo.InvariantCulture, "\tbTemporalCompression :  {0}\r\n", mt.bTemporalCompression);
            ret += string.Format(CultureInfo.InvariantCulture, "\tcbFormat :  {0}\r\n", mt.cbFormat);
            ret += string.Format(CultureInfo.InvariantCulture, "\tformattype :  {0}\r\n", FormatType.GuidToString(mt.formattype));
            ret += string.Format(CultureInfo.InvariantCulture, "\tlSampleSize :  {0}\r\n", mt.lSampleSize);
            ret += string.Format(CultureInfo.InvariantCulture, "\tmajortype :  {0}\r\n", MajorType.GuidToString(mt.majortype));
            ret += string.Format(CultureInfo.InvariantCulture, "\tpbFormat :  {0}\r\n", ((int)mt.pbFormat));
            ret += string.Format(CultureInfo.InvariantCulture, "\tsubtype :  {0}", SubType.GuidToString(mt.subtype));

            object formatBlock = FormatType.MarshalData(mt);
            if(formatBlock != null)
            {
                ret += FormatType.Dump(formatBlock);
            }

            return ret;
        }

        /// <summary>
        /// Converts the IntPtr to the _AMMediaType into a string representation
        /// </summary>
        public static string Dump(IntPtr pMT)
        {
            return Dump((_AMMediaType)Marshal.PtrToStructure(pMT, typeof(_AMMediaType)));
        }


        #region Major Types

        public class MajorType
        {
            private static FieldInfo[] fields;

            static MajorType()
            {
                fields = typeof(MajorType).GetFields();
            }


            // From uuids.h in the DX\Include folder, in the order they appear
            public static Guid MEDIATYPE_NULL           = Guid.Empty;
            public static Guid MEDIATYPE_Video          = new Guid(0x73646976, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIATYPE_Audio          = new Guid(0x73647561, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIATYPE_Text           = new Guid(0x73747874, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIATYPE_Midi           = new Guid(0x7364696D, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIATYPE_Stream         = new Guid(0xe436eb83, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIATYPE_Interleaved    = new Guid(0x73766169, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIATYPE_File           = new Guid(0x656c6966, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIATYPE_ScriptCommand  = new Guid(0x73636d64, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIATYPE_AUXLine21Data  = new Guid(0x670aea80, 0x3a82, 0x11d0, 0xb7, 0x9b, 0x0, 0xaa, 0x0, 0x37, 0x67, 0xa7);
            public static Guid MEDIATYPE_VBI            = new Guid(0xf72a76e1, 0xeb0a, 0x11d0, 0xac, 0xe4, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);
            public static Guid MEDIATYPE_Timecode       = new Guid(0x482dee3, 0x7817, 0x11cf, 0x8a, 0x3, 0x0, 0xaa, 0x0, 0x6e, 0xcb, 0x65);
            public static Guid MEDIATYPE_LMRT           = new Guid(0x74726c6d, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIATYPE_URL_STREAM     = new Guid(0x736c7275, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIATYPE_AnalogVideo    = new Guid(0x482dde1, 0x7817, 0x11cf, 0x8a, 0x3, 0x0, 0xaa, 0x0, 0x6e, 0xcb, 0x65);
            public static Guid MEDIATYPE_AnalogAudio    = new Guid(0x482dee1, 0x7817, 0x11cf, 0x8a, 0x3, 0x0, 0xaa, 0x0, 0x6e, 0xcb, 0x65);
        
            // From ksuuids.h in the DX\Include folder, in the order they appear
            public static Guid MEDIATYPE_MPEG2_PACK         = new Guid(0x36523B13, 0x8EE5, 0x11d1, 0x8C, 0xA3, 0x00, 0x60, 0xB0, 0x57, 0x66, 0x4A);
            public static Guid MEDIATYPE_MPEG2_PES          = new Guid(0xe06d8020, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);
            public static Guid MEDIATYPE_CONTROL            = new Guid(0xe06d8021, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);
            public static Guid MEDIATYPE_MPEG2_SECTIONS     = new Guid(0x455f176c, 0x4b06, 0x47ce, 0x9a, 0xef, 0x8c, 0xae, 0xf7, 0x3d, 0xf7, 0xb5);
            public static Guid MEDIATYPE_DVD_ENCRYPTED_PACK = new Guid(0xed0b916a, 0x044d, 0x11d1, 0xaa, 0x78, 0x00, 0xc0, 0x04f, 0xc3, 0x1d, 0x60);
            public static Guid MEDIATYPE_DVD_NAVIGATION     = new Guid(0xe06d802e, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);

            public static string GuidToString(Guid g)
            {
                return GuidConverter.GuidToString(g, fields);
            }

            public static Guid StringToGuid(string s)
            {
                return GuidConverter.StringToGuid(s, fields);
            }
        }
        

        #endregion

        #region Format Types

        public class FormatType
        {
            private static FieldInfo[] fields;

            static FormatType()
            {
                fields = typeof(FormatType).GetFields();
            }


            // From uuids.h in the DX\Include folder, in the order they appear
            public static Guid FORMAT_None          = new Guid(0x0F6417D6, 0xc318, 0x11d0, 0xa4, 0x3f, 0x00, 0xa0, 0xc9, 0x22, 0x31, 0x96);
            public static Guid FORMAT_VideoInfo     = new Guid(0x05589f80, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a);
            public static Guid FORMAT_VideoInfo2    = new Guid(0xf72a76A0, 0xeb0a, 0x11d0, 0xac, 0xe4, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);
            public static Guid FORMAT_WaveFormatEx  = new Guid(0x05589f81, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a);
            public static Guid FORMAT_MPEGVideo     = new Guid(0x05589f82, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a);
            public static Guid FORMAT_MPEGStreams   = new Guid(0x05589f83, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a);
            public static Guid FORMAT_DvInfo        = new Guid(0x05589f84, 0xc356, 0x11ce, 0xbf, 0x01, 0x00, 0xaa, 0x00, 0x55, 0x59, 0x5a);
            public static Guid FORMAT_AnalogVideo   = new Guid(0x482dde0, 0x7817, 0x11cf, 0x8a, 0x3, 0x0, 0xaa, 0x0, 0x6e, 0xcb, 0x65);

            // From ksuuids.h in the DX\Include folder, in the order they appear
            public static Guid FORMAT_MPEG2Video    = new Guid(0xe06d80e3, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);
            public static Guid FORMAT_DolbyAC3      = new Guid(0xe06d80e4, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);
            public static Guid FORMAT_MPEG2Audio    = new Guid(0xe06d80e5, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);
            public static Guid FORMAT_DVD_LPCMAudio = new Guid(0xe06d80e6, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);

            public static string Dump(object formatBlock)
            {
                string ret = "\r\nFormat Type\r\n";

                if(formatBlock != null)
                {
                    ret += formatBlock.ToString();
                }
                else
                {
                    ret += "\tnull";
                }

                return ret;
            }

            public static object MarshalData(_AMMediaType mt)
            {
                object formatBlock = null;

                if(mt.cbFormat > 0 && mt.pbFormat != IntPtr.Zero)
                {
                    if(mt.formattype == MediaType.FormatType.FORMAT_VideoInfo)
                    {
                        formatBlock = (VIDEOINFOHEADER)Marshal.PtrToStructure(mt.pbFormat, typeof(VIDEOINFOHEADER));
                    }
                    else if(mt.formattype == MediaType.FormatType.FORMAT_VideoInfo2)
                    {
                        formatBlock = (VIDEOINFOHEADER2)Marshal.PtrToStructure(mt.pbFormat, typeof(VIDEOINFOHEADER2));
                    }
                    else if(mt.formattype == MediaType.FormatType.FORMAT_MPEGVideo)
                    {
                        formatBlock = (MPEG1VIDEOINFO)Marshal.PtrToStructure(mt.pbFormat, typeof(MPEG1VIDEOINFO));
                    }
                    else if(mt.formattype == MediaType.FormatType.FORMAT_MPEG2Video)
                    {
                        formatBlock = (MPEG2VIDEOINFO)Marshal.PtrToStructure(mt.pbFormat, typeof(MPEG2VIDEOINFO));
                    }
                    else if(mt.formattype == MediaType.FormatType.FORMAT_DvInfo)
                    {
                        formatBlock = (DVINFO)Marshal.PtrToStructure(mt.pbFormat, typeof(DVINFO));
                    }
                    else if(mt.formattype == MediaType.FormatType.FORMAT_WaveFormatEx)
                    {
                        formatBlock = (WAVEFORMATEX)Marshal.PtrToStructure(mt.pbFormat, typeof(WAVEFORMATEX));
                    }
                }

                return formatBlock;
            }

            public static string GuidToString(Guid g)
            {
                return GuidConverter.GuidToString(g, fields);
            }

            public static Guid StringToGuid(string s)
            {
                return GuidConverter.StringToGuid(s, fields);
            }
        }

        
        #endregion
        
        #region Sub Types

        public class SubType
        {
            private static FieldInfo[] fields;

            static SubType()
            {
                fields = typeof(SubType).GetFields();
            }


            // From uuids.h in the DX\Include folder, in the order they appear
            public static Guid MEDIASUBTYPE_NULL = Guid.Empty;
            public static Guid MEDIASUBTYPE_None = new Guid(0xe436eb8e, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
        
            public static Guid MEDIASUBTYPE_CLPL = new Guid(0x4C504C43, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_YUYV = new Guid(0x56595559, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_IYUV = new Guid(0x56555949, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_YVU9 = new Guid(0x39555659, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_Y411 = new Guid(0x31313459, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_Y41P = new Guid(0x50313459, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_YUY2 = new Guid(0x32595559, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_YVYU = new Guid(0x55595659, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_UYVY = new Guid(0x59565955, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_Y211 = new Guid(0x31313259, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_CLJR = new Guid(0x524a4c43, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_IF09 = new Guid(0x39304649, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_CPLA = new Guid(0x414c5043, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_MJPG = new Guid(0x47504A4D, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_TVMJ = new Guid(0x4A4D5654, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_WAKE = new Guid(0x454B4157, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_CFCC = new Guid(0x43434643, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_IJPG = new Guid(0x47504A49, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_Plum = new Guid(0x6D756C50, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_DVCS = new Guid(0x53435644, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_DVSD = new Guid(0x44535644, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_MDVF = new Guid(0x4656444D, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

            public static Guid MEDIASUBTYPE_RGB1    = new Guid(0xe436eb78, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIASUBTYPE_RGB4    = new Guid(0xe436eb79, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIASUBTYPE_RGB8    = new Guid(0xe436eb7a, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIASUBTYPE_RGB565  = new Guid(0xe436eb7b, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIASUBTYPE_RGB555  = new Guid(0xe436eb7c, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIASUBTYPE_RGB24   = new Guid(0xe436eb7d, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIASUBTYPE_RGB32   = new Guid(0xe436eb7e, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
        
            public static Guid MEDIASUBTYPE_ARGB1555    = new Guid(0x297c55af, 0xe209, 0x4cb3, 0xb7, 0x57, 0xc7, 0x6d, 0x6b, 0x9c, 0x88, 0xa8);
            public static Guid MEDIASUBTYPE_ARGB4444    = new Guid(0x6e6415e6, 0x5c24, 0x425f, 0x93, 0xcd, 0x80, 0x10, 0x2b, 0x3d, 0x1c, 0xca);
            public static Guid MEDIASUBTYPE_ARGB32      = new Guid(0x773c9ac0, 0x3274, 0x11d0, 0xb7, 0x24, 0x0, 0xaa, 0x0, 0x6c, 0x1a, 0x1);
            public static Guid MEDIASUBTYPE_A2R10G10B10 = new Guid(0x2f8bb76d, 0xb644, 0x4550, 0xac, 0xf3, 0xd3, 0x0c, 0xaa, 0x65, 0xd5, 0xc5);
            public static Guid MEDIASUBTYPE_A2B10G10R10 = new Guid(0x576f7893, 0xbdf6, 0x48c4, 0x87, 0x5f, 0xae, 0x7b, 0x81, 0x83, 0x45, 0x67);
            public static Guid MEDIASUBTYPE_AYUV        = new Guid(0x56555941, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_AI44        = new Guid(0x34344941, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_IA44        = new Guid(0x34344149, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
        
            public static Guid MEDIASUBTYPE_YV12 = new Guid(0x32315659, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_NV12 = new Guid(0x3231564E, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_IMC1 = new Guid(0x31434D49, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_IMC2 = new Guid(0x32434D49, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_IMC3 = new Guid(0x33434D49, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_IMC4 = new Guid(0x34434D49, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_S340 = new Guid(0x30343353, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_S342 = new Guid(0x32343353, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
        
            public static Guid MEDIASUBTYPE_Overlay = new Guid(0xe436eb7f, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

            public static Guid MEDIASUBTYPE_MPEG1Packet         = new Guid(0xe436eb80, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIASUBTYPE_MPEG1Payload        = new Guid(0xe436eb81, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIASUBTYPE_MPEG1AudioPayload   = new Guid(0x00000050, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71);
            public static Guid MEDIASUBTYPE_MPEG1SystemStream   = new Guid(0xe436eb82, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIASUBTYPE_MPEG1System         = new Guid(0xe436eb84, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIASUBTYPE_MPEG1VideoCD        = new Guid(0xe436eb85, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIASUBTYPE_MPEG1Video          = new Guid(0xe436eb86, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIASUBTYPE_MPEG1Audio          = new Guid(0xe436eb87, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIASUBTYPE_Avi                 = new Guid(0xe436eb88, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIASUBTYPE_Asf                 = new Guid(0x3db80f90, 0x9412, 0x11d1, 0xad, 0xed, 0x0, 0x0, 0xf8, 0x75, 0x4b, 0x99);
            public static Guid MEDIASUBTYPE_QTMovie             = new Guid(0xe436eb89, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIASUBTYPE_QTRpza              = new Guid(0x617a7072, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_QTSmc               = new Guid(0x20636d73, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_QTRle               = new Guid(0x20656c72, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_QTJpeg              = new Guid(0x6765706a, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_PCMAudio_Obsolete   = new Guid(0xe436eb8a, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIASUBTYPE_PCM                 = new Guid(0x00000001, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71);
            public static Guid MEDIASUBTYPE_WAVE                = new Guid(0xe436eb8b, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIASUBTYPE_AU                  = new Guid(0xe436eb8c, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
            public static Guid MEDIASUBTYPE_AIFF                = new Guid(0xe436eb8d, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);
    
            public static Guid MEDIASUBTYPE_dvsd    = new Guid(0x64737664, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_dvhd    = new Guid(0x64687664, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_dvsl    = new Guid(0x6c737664, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_dv25    = new Guid(0x35327664, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_dv50    = new Guid(0x30357664, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_dvh1    = new Guid(0x31687664, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

            public static Guid MEDIASUBTYPE_Line21_BytePair     = new Guid(0x6e8d4a22, 0x310c, 0x11d0, 0xb7, 0x9a, 0x0, 0xaa, 0x0, 0x37, 0x67, 0xa7);
            public static Guid MEDIASUBTYPE_Line21_GOPPacket    = new Guid(0x6e8d4a23, 0x310c, 0x11d0, 0xb7, 0x9a, 0x0, 0xaa, 0x0, 0x37, 0x67, 0xa7);
            public static Guid MEDIASUBTYPE_Line21_VBIRawData   = new Guid(0x6e8d4a24, 0x310c, 0x11d0, 0xb7, 0x9a, 0x0, 0xaa, 0x0, 0x37, 0x67, 0xa7);
            public static Guid MEDIASUBTYPE_TELETEXT            = new Guid(0xf72a76e3, 0xeb0a, 0x11d0, 0xac, 0xe4, 0x00, 0x00, 0xc0, 0xcc, 0x16, 0xba);
            public static Guid MEDIASUBTYPE_WSS                 = new Guid(0x2791D576, 0x8E7A, 0x466F, 0x9E, 0x90, 0x5D, 0x3F, 0x30, 0x83, 0x73, 0x8B);
            public static Guid MEDIASUBTYPE_VPS                 = new Guid(0xa1b3f620, 0x9792, 0x4d8d, 0x81, 0xa4, 0x86, 0xaf, 0x25, 0x77, 0x20, 0x90);
            public static Guid MEDIASUBTYPE_DRM_Audio           = new Guid(0x00000009, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_IEEE_FLOAT          = new Guid(0x00000003, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_DOLBY_AC3_SPDIF     = new Guid(0x00000092, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_RAW_SPORT           = new Guid(0x00000240, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
            public static Guid MEDIASUBTYPE_SPDIF_TAG_241h      = new Guid(0x00000241, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

            public static Guid MEDIASUBTYPE_DssVideo            = new Guid(0xa0af4f81, 0xe163, 0x11d0, 0xba, 0xd9, 0x0, 0x60, 0x97, 0x44, 0x11, 0x1a);
            public static Guid MEDIASUBTYPE_DssAudio            = new Guid(0xa0af4f82, 0xe163, 0x11d0, 0xba, 0xd9, 0x0, 0x60, 0x97, 0x44, 0x11, 0x1a);
            public static Guid MEDIASUBTYPE_VPVideo             = new Guid(0x5a9b6a40, 0x1a22, 0x11d1, 0xba, 0xd9, 0x0, 0x60, 0x97, 0x44, 0x11, 0x1a);
            public static Guid MEDIASUBTYPE_VPVBI               = new Guid(0x5a9b6a41, 0x1a22, 0x11d1, 0xba, 0xd9, 0x0, 0x60, 0x97, 0x44, 0x11, 0x1a);
    
            // From ksuuids.h in the DX\Include folder, in the order they appear
            public static Guid MEDIASUBTYPE_ATSC_SI                 = new Guid(0xb3c7397c, 0xd303, 0x414d, 0xb3, 0x3c, 0x4e, 0xd2, 0xc9, 0xd2, 0x97, 0x33);
            public static Guid MEDIASUBTYPE_DVB_SI                  = new Guid(0xe9dd31a3, 0x221d, 0x4adb, 0x85, 0x32, 0x9a, 0xf3, 0x9, 0xc1, 0xa4, 0x8);
            public static Guid MEDIASUBTYPE_MPEG2DATA               = new Guid(0xc892e55b, 0x252d, 0x42b5, 0xa3, 0x16, 0xd9, 0x97, 0xe7, 0xa5, 0xd9, 0x95);
            public static Guid MEDIASUBTYPE_MPEG2_VIDEO             = new Guid(0xe06d8026, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);
            public static Guid MEDIASUBTYPE_MPEG2_PROGRAM           = new Guid(0xe06d8022, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);
            public static Guid MEDIASUBTYPE_MPEG2_TRANSPORT         = new Guid(0xe06d8023, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);
            public static Guid MEDIASUBTYPE_MPEG2_TRANSPORT_STRIDE  = new Guid(0x138aa9a4, 0x1ee2, 0x4c5b, 0x98, 0x8e, 0x19, 0xab, 0xfd, 0xbc, 0x8a, 0x11);
            public static Guid MEDIASUBTYPE_MPEG2_AUDIO             = new Guid(0xe06d802b, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);
            public static Guid MEDIASUBTYPE_DOLBY_AC3               = new Guid(0xe06d802c, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);
            public static Guid MEDIASUBTYPE_DVD_SUBPICTURE          = new Guid(0xe06d802d, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);
            public static Guid MEDIASUBTYPE_DVD_LPCM_AUDIO          = new Guid(0xe06d8032, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);
            public static Guid MEDIASUBTYPE_DTS                     = new Guid(0xe06d8033, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);
            public static Guid MEDIASUBTYPE_SDDS                    = new Guid(0xe06d8034, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);

            public static Guid MEDIASUBTYPE_DVD_NAVIGATION_PCI      = new Guid(0xe06d802f, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);
            public static Guid MEDIASUBTYPE_DVD_NAVIGATION_DSI      = new Guid(0xe06d8030, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);
            public static Guid MEDIASUBTYPE_DVD_NAVIGATION_PROVIDER = new Guid(0xe06d8031, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);

            // From DX90\Extras\DirectShow\Samples\C++\DirectShow\Common\wmsdkidl.h, in the order they appear
            public static Guid WMMEDIASUBTYPE_MP43      = new Guid(0x3334504D, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid WMMEDIASUBTYPE_MP4S      = new Guid(0x5334504D, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            // public static Guid WMMEDIASUBTYPE_DRM       = new Guid(0x00000009, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); // MEDIASUBTYPE_DRM_Audio
            // public static Guid WMMEDIASUBTYPE_WMAudioV7 = new Guid(0x00000161, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); // WMA9 
            // public static Guid WMMEDIASUBTYPE_WMAudioV2 = new Guid(0x00000161, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); // WMA9 
            public static Guid WMMEDIASUBTYPE_ACELPnet  = new Guid(0x00000130, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 

            // From wmcodecconst.h from the wm_avcodec_interface download, in the order they appear, minus duplicates
            // Also wmsdkidl in the windows SDK.
            public static Guid WMMEDIASUBTYPE_WMV1          = new Guid(0x31564D57, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid WMMEDIASUBTYPE_WMV2          = new Guid(0x32564D57, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid WMMEDIASUBTYPE_WMV3          = new Guid(0x33564D57, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71);
            public static Guid WMMEDIASUBTYPE_WVC1          = new Guid(0x31435657, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71);
            public static Guid WMMEDIASUBTYPE_MSS1          = new Guid(0x3153534D, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid WMMEDIASUBTYPE_MSS2          = new Guid(0x3253534D, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid WMMEDIASUBTYPE_WMVP          = new Guid(0x50564D57, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid WMMEDIASUBTYPE_MSA1          = new Guid(0x00000160, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid WMMEDIASUBTYPE_WMA9          = new Guid(0x00000161, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid WMMEDIASUBTYPE_WMA9Pro       = new Guid(0x00000162, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid WMMEDIASUBTYPE_WMA9Lossless  = new Guid(0x00000163, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid WMMEDIASUBTYPE_WMA9Voice     = new Guid(0x0000000A, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71);
            public static Guid WMMEDIASUBTYPE_I420          = new Guid(0x30323449, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

            public static Guid WMMEDIASUBTYPE_VIDEOIMAGE    = new Guid(0x1d4a45f2, 0xe5f6, 0x4b44, 0x83, 0x88, 0xf0, 0xae, 0x5c, 0x0e, 0x0c, 0x37);

            // MSDN?
            public static Guid WMMEDIASUBTYPE_WMVA  = new Guid(0x41564D57, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid WMMEDIASUBTYPE_WVP2  = new Guid(0x32505657, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
        
            // Not found in header files, found in filters
            public static Guid MEDIASUBTYPE_Y422    = new Guid(0x32323459, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71);
            public static Guid WMMEDIASUBTYPE_mp43  = new Guid(0x3334706d, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid WMMEDIASUBTYPE_mp4s  = new Guid(0x7334706D, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid MEDIASUBTYPE_cvid    = new Guid(0x64697663, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71);
            public static Guid MEDIASUBTYPE_IV32    = new Guid(0x32335649, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid MEDIASUBTYPE_IV41    = new Guid(0x31345649, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid MEDIASUBTYPE_IV50    = new Guid(0x30355649, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid MEDIASUBTYPE_MP42    = new Guid(0x3234504d, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid MEDIASUBTYPE_MPG4    = new Guid(0x3447504d, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid MEDIASUBTYPE_CRAM    = new Guid(0x4d415243, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 
            public static Guid MEDIASUBTYPE_NTN1    = new Guid(0x314e544e, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71); 

            public static string GuidToString(Guid g)
            {
                return GuidConverter.GuidToString(g, fields);
            }

            public static Guid StringToGuid(string s)
            {
                return GuidConverter.StringToGuid(s, fields);
            }
        }
        

        #endregion

        internal class GuidConverter
        {
            /// <summary>
            /// Converts a Guid to a formatted string such as MEDIATYPE_Video, unless we don't have a 
            /// matching Guid in which case we just return Guid.ToString()
            /// </summary>
            public static string GuidToString(Guid g, FieldInfo[] fields)
            {
                // Assume we won't be able to find the name in our list
                string name = g.ToString();

                // Then look for it
                foreach(FieldInfo fi in fields)
                {
                    if(fi.FieldType == typeof(Guid) && fi.IsStatic)
                    {
                        if((Guid)fi.GetValue(null) == g)
                        {
                            // Return everything after the first underscore
                            name = fi.Name.Substring(fi.Name.IndexOf("_") + 1);
                        }
                    }
                }

                return name;
            }
        
            /// <summary>
            /// Converts a formatted string such as MEDIATYPE_Video to a Guid, unless we don't have a 
            /// matching string, in which case we try to new a Guid, failing that throw an exception
            /// 
            /// This method is expensive, because it iterates through the hashtable one item at a time
            /// looking for a match.  However, this method should be needed rarely, in order to convert
            /// a human readable string MEDIATYPE_Video to a Guid.
            /// </summary>
            public static Guid StringToGuid(string search, FieldInfo[] fields)
            {
                // See if the string can be converted directly to a Guid, for the case where we didn't
                // have a formatted string to convert the Guid to in GuidToString
                try
                {
                    return new Guid(search);
                }
                catch(FormatException) {}//Ignore

                // Then look for it
                foreach(FieldInfo fi in fields)
                {
                    if(fi.FieldType == typeof(Guid) && fi.IsStatic)
                    {
                        if(fi.Name == search ||
                           fi.Name.Substring(fi.Name.IndexOf("_") + 1) == search)
                        {
                            return (Guid)fi.GetValue(null);
                        }
                    }
                }

                // We don't know how to convert this string to a Guid
                throw new FormatException(string.Format(CultureInfo.CurrentCulture, Strings.UnableToConvertToGuid, 
                    search));
            }
        }
    }
}