using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace MSR.LST.MDShow
{
    public abstract class Pin
    {
        public static Guid PIN_CATEGORY_CAPTURE = new Guid("fb6c4281-0353-11d1-905f-0000c0cc16ba");

        public static string Name(IPin pin)
        {
            _PinInfo pi;
            pin.QueryPinInfo(out pi);
            return pi.achName;
        }


        /// <summary>
        /// Gets iSC's current _AMMediaType, without freeing pbFormat
        /// Caller should call MediaType.Free(ref _AMMediaType) when done
        /// </summary>
        public static _AMMediaType GetMediaType(IAMStreamConfig iSC)
        {
            IntPtr pmt = IntPtr.Zero;
            iSC.GetFormat(out pmt);

            _AMMediaType mt;
            MediaType.MarshalData(ref pmt, out mt); // Frees pmt
            System.Diagnostics.Debug.Assert(pmt == IntPtr.Zero);

            return mt;
        }
        /// <summary>
        /// Gets iSC's available _AMMediaTypes, without freeing pbFormat
        /// Caller should call MediaType.Free(_AMMediaType[]) when done
        /// </summary>
        public static _AMMediaType[] GetMediaTypes(IPin pin)
        {
            IEnumMediaTypes iEnum;
            pin.EnumMediaTypes(out iEnum);

            ArrayList alMTs = new ArrayList();

            IntPtr[] ptrs = new IntPtr[1];
            uint fetched;

            iEnum.Next(1, ptrs, out fetched);

            while(fetched == 1)
            {
                _AMMediaType mt = (_AMMediaType)Marshal.PtrToStructure(ptrs[0], typeof(_AMMediaType));
                alMTs.Add(mt);
                
                Marshal.FreeCoTaskMem(ptrs[0]);
                ptrs[0] = IntPtr.Zero;

                iEnum.Next(1, ptrs, out fetched);
            }

            _AMMediaType[] mts = new _AMMediaType[alMTs.Count];
            alMTs.CopyTo(mts);
            
            return mts;
        }

        /// <summary>
        /// Gets iSC's current _AMMediaType, and frees pbFormat
        /// </summary>
        public static void GetMediaType(IAMStreamConfig iSC, out _AMMediaType mt, out object formatBlock)
        {
            IntPtr pmt = IntPtr.Zero;
            iSC.GetFormat(out pmt);

            // Frees pmt and mt.pbFormat
            MediaType.MarshalData(ref pmt, out mt, out formatBlock); 

            System.Diagnostics.Debug.Assert(pmt == IntPtr.Zero);
            System.Diagnostics.Debug.Assert(mt.pbFormat == IntPtr.Zero && mt.cbFormat == 0);
        }

        /// <summary>
        /// Gets iSC's available _AMMediaTypes, and frees the pbFormats
        /// </summary>
        public static void GetMediaTypes(IPin pin, out _AMMediaType[] mediaTypes, out object[] formatBlocks)
        {
            mediaTypes = GetMediaTypes(pin);
            
            formatBlocks = new object[mediaTypes.Length];

            for(int i = 0; i < mediaTypes.Length; i++)
            {
                object formatBlock;
                MediaType.MarshalData(ref mediaTypes[i], out formatBlock); // Frees pbFormat
                formatBlocks[i] = formatBlock;
            }
        }

        /// <summary>
        /// Sets the _AMMediaType on the pin, but doesn't free it
        /// </summary>
        public static void SetMediaType(IAMStreamConfig iSC, _AMMediaType mt)
        {
            System.Diagnostics.Debug.Assert(mt.pbFormat != IntPtr.Zero && mt.cbFormat != 0);
            iSC.SetFormat(ref mt);
        }

        /// <summary>
        /// Sets the _AMMediaType on the pin, then frees it
        /// </summary>
        public static void SetMediaType(IAMStreamConfig iSC, ref _AMMediaType mt)
        {
            try
            {
                SetMediaType(iSC, mt);
            }
            finally
            {
                MediaType.Free(ref mt);
            }
        }

        /// <summary>
        /// Constructs the _AMMediaType (adds pbFormat to it), sets it, then frees it
        /// </summary>
        public static void SetMediaType(IAMStreamConfig iSC, _AMMediaType mt, object formatBlock)
        {
            System.Diagnostics.Debug.Assert(mt.pbFormat == IntPtr.Zero && mt.cbFormat == 0);

            mt = MediaType.Construct(mt, formatBlock);
            SetMediaType(iSC, ref mt);
        }
        

        public static void GetStreamConfigCaps(IAMStreamConfig iSC, 
            out ArrayList mediaTypes, out ArrayList infoHeaders, out ArrayList streamConfigCaps)
        {
            // Initialize return values
            mediaTypes = new ArrayList();
            infoHeaders = new ArrayList();
            streamConfigCaps = new ArrayList();

            // Find out how many capabilities the stream has
            int piCount, piSize;
            iSC.GetNumberOfCapabilities(out piCount, out piSize);

            IntPtr pSCC = Marshal.AllocCoTaskMem(piSize);

            try
            {
                // Iterate through capabilities
                for(int i = 0; i < piCount; i++)
                {
                    IntPtr pmt = IntPtr.Zero;
                    iSC.GetStreamCaps(i, out pmt, pSCC);

                    _AMMediaType mt;
                    object formatBlock;

                    MediaType.MarshalData(ref pmt, out mt, out formatBlock); // Frees pmt

                    mediaTypes.Add(mt);
                    infoHeaders.Add(formatBlock);
                    streamConfigCaps.Add(MarshalStreamConfigCaps(mt.majortype, pSCC));
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(pSCC);
            }
        }

        public static string DebugStreamConfigCaps(object o)
        {
            string ret = "\r\nStream capabilities\r\n";

            if(o != null)
            {
                ret += o.ToString();
            }
            else
            {
                ret += "null";
            }

            return ret;
        }

        private static object MarshalStreamConfigCaps(Guid majorType, IntPtr pSCC)
        {
            object o = null;

            if(pSCC != IntPtr.Zero)
            {
                if(majorType == MediaType.MajorType.MEDIATYPE_Video)
                {
                    o = Marshal.PtrToStructure(pSCC, typeof(VIDEO_STREAM_CONFIG_CAPS));
                }
                else if(majorType == MediaType.MajorType.MEDIATYPE_Audio)
                {
                    o = Marshal.PtrToStructure(pSCC, typeof(AUDIO_STREAM_CONFIG_CAPS));
                }
            }

            return o;
        }

                
        public static bool HasDialog(IPin iPin)
        {
            return iPin != null && iPin is ISpecifyPropertyPages;
        }

        public static void ShowDialog(IPin iPin, IntPtr hwnd)
        {
            Debug.Assert(HasDialog(iPin));

            PropertyPage pp = new PropertyPage((ISpecifyPropertyPages)iPin, Pin.Name(iPin));
            pp.Show(hwnd);
        }
    }
}