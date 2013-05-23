using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.Win32;
using System.Text.RegularExpressions;


namespace MSR.LST.MDShow
{
    public struct FilterInfo
    {
        private string moniker;
        private string name;
        private string displayName;
        private Guid category;

        public FilterInfo(string moniker, string name, Guid category)
        {
            this.moniker = moniker;
            this.name = name;
            this.category = category;
            displayName = null;
        }


        public string Moniker
        {
            get{return moniker;}
        }

        public string Name
        {
            get{return name;}
        }

        public string DisplayName
        {
            get{return (displayName != null) ? displayName : name;}
            set{displayName = value;}
        }

        public Guid Category
        {
            get{return category;}
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }


    public abstract class Filter
    {
        #region Statics
    
        public static Guid CLSID_VideoInputDeviceCategory   = new Guid("860bb310-5d01-11d0-bd3b-00a0c911ce86");
        public static Guid CLSID_AudioInputDeviceCategory   = new Guid("33d9a762-90c8-11d0-bd43-00a0c911ce86");
        public static Guid CLSID_AudioRendererCategory      = new Guid("e0f158e1-cb04-11d0-bd4e-00a0c911ce86");
        public static Guid CLSID_VideoCompressorCategory    = new Guid("33d9a760-90c8-11d0-bd43-00a0c911ce86");
        public static Guid CLSID_AudioCompressorCategory    = new Guid("33d9a761-90c8-11d0-bd43-00a0c911ce86");
        
        public static Guid IID_IPropertyBag = Marshal.GenerateGuidForType(typeof(IPropertyBag));
        public static Guid IID_IBaseFilter  = Marshal.GenerateGuidForType(typeof(IBaseFilter));
        public static Guid LOOK_UPSTREAM_ONLY = new Guid("AC798BE0-98E3-11d1-B3F1-00AA003761C5");

        public static IBaseFilter FindBaseFilterByName(FilgraphManagerClass fgm, string name)
        {
            IFilterGraph iFG = (IFilterGraph)fgm;

            IEnumFilters iEnum;
            iFG.EnumFilters(out iEnum);

            IBaseFilter iBF = null;
            uint fetched = 0;
            iEnum.Next(1, out iBF, out fetched);

            while (fetched == 1)
            {
                _FilterInfo fi;
                iBF.QueryFilterInfo(out fi);
                string filterName = fi.achName;

                if (String.Compare(name, filterName) == 0)
                {
                    return iBF;
                }

                iEnum.Next(1, out iBF, out fetched);
            }

            return null;
        }

        public static IBaseFilter CreateBaseFilterByName(string name)
        {
            IFilterMapper2 iFM2 = FilterMapper2Class.CreateInstance();
        
            IEnumMoniker iEnum;
        
            iFM2.EnumMatchingFilters(out iEnum,
                0,
                false,
                0x200000, // MERIT_DO_NOT_USE
                false,
                0,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                false,
                false,
                0,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero);
        
            uint fetched;
            IMoniker iMon;
        
            iEnum.RemoteNext(1, out iMon, out fetched);
        
            while(fetched > 0)
            {
                object oPB;
                iMon.RemoteBindToStorage(null, null, ref IID_IPropertyBag, out oPB);
                IPropertyBag iPB = (IPropertyBag)oPB;
        
                object oFriendlyName;
                iPB.RemoteRead("FriendlyName", out oFriendlyName, null, 0, null);
        
                if((string)oFriendlyName == name)
                {
                    object oBF;
                    iMon.RemoteBindToObject(null, null, ref IID_IBaseFilter, out oBF);
        
                    return (IBaseFilter)oBF;
                }
        
                iEnum.RemoteNext(1, out iMon, out fetched);
            }

            throw new Exception(string.Format(CultureInfo.CurrentCulture, Strings.FilterNameNotFound, name));
        }

        public static IBaseFilter CreateBaseFilter(FilterInfo fi)
        {
            ICreateDevEnum iDE = CreateDeviceEnumClass.CreateInstance();

            IEnumMoniker iEnum;
            Guid filterCategory = fi.Category;
            iDE.CreateClassEnumerator(ref filterCategory, out iEnum, 0);

            if (iEnum != null)
            {
                IMoniker iMon;
                uint fetched;

                // To enter loop
                iEnum.RemoteNext(1, out iMon, out fetched);

                while(fetched == 1)
                {
                    string filterMoniker;
                    iMon.GetDisplayName(null, null, out filterMoniker);
              
                    if (filterMoniker == fi.Moniker)
                    {
                        object oBaseFilter;
                        iMon.RemoteBindToObject(null, null, ref IID_IBaseFilter, out oBaseFilter);
                        return (IBaseFilter)oBaseFilter;
                    }

                    iEnum.RemoteNext(1, out iMon, out fetched);
                }
            }

            throw new Exception(string.Format(CultureInfo.CurrentCulture, Strings.FilterMonikerNotFound, fi.Moniker));
        }

        public static Filter CreateFilter(FilterInfo fi)
        {
            // Capture devices
            if(fi.Category == AudioSource.CategoryGuid) 
                return new AudioSource(fi);

            if (fi.Category == VideoSource.CategoryGuid) {
                //A special case for DV with audio
                if (DVSource.IsDVSourceWithAudio(fi)) {
                    return new DVSource(fi);
                }
                //Special case for BlackMagic hardware
                if (fi.Name == "Decklink Video Capture") {
                    return new BlackMagicSource(fi);
                }
                return new VideoSource(fi);
            }
                
            // Compressors
            if(fi.Category == AudioCompressor.CategoryGuid) 
                return new AudioCompressor(fi);

            if(fi.Category == VideoCompressor.CategoryGuid) 
                return new VideoCompressor(fi);
    
            // Renderers
            if(fi.Category == AudioRenderer.CategoryGuid) 
                return new AudioRenderer(fi);

            throw new Exception(string.Format(CultureInfo.CurrentCulture, Strings.UnknownFilterCategory, 
                fi.Category.ToString()));
        }

        public static Filter NetworkRenderer()
        {
            return new NetworkRenderer(new FilterInfo(null, "RtpRenderer", Guid.Empty));
        }

        public static Filter NetworkSource()
        {
            return new NetworkSource(new FilterInfo(null, "RtpSource", Guid.Empty));
        }


        public static FilterInfo[] EnumerateFilters(Guid filterCategory)
        {
            ArrayList filters = new ArrayList();

            IEnumMoniker iEnum;

            ICreateDevEnum iDE = CreateDeviceEnumClass.CreateInstance();
            iDE.CreateClassEnumerator(ref filterCategory, out iEnum, 0);

            if (iEnum != null)
            {
                IMoniker iMon;
                uint fetched;

                // To enter loop
                iEnum.RemoteNext(1, out iMon, out fetched);

                while(fetched == 1)
                {
                    string monikerName;
                    iMon.GetDisplayName(null, null, out monikerName);
          
                    object oPropertyBag;
                    iMon.RemoteBindToStorage(null ,null, ref IID_IPropertyBag, out oPropertyBag);
                    IPropertyBag iPB = (IPropertyBag)oPropertyBag;

                    try {
                        object oFriendlyName;
                        iPB.RemoteRead("FriendlyName", out oFriendlyName, null, 0, null);
                        filters.Add(new FilterInfo(monikerName, ((string)oFriendlyName).Trim(), filterCategory));
                    }
                    catch {
                    }

                    iEnum.RemoteNext(1, out iMon, out fetched);
                }
            }

            // Strongly type the data
            FilterInfo[] ret = new FilterInfo[filters.Count];
            filters.CopyTo(ret);
            return ret;
        }

        public static string Name(IBaseFilter filter)
        {
            _FilterInfo fi;
            filter.QueryFilterInfo(out fi);
            return fi.achName;
        }

        public static IPin GetPinByName(IBaseFilter iBF, string name)
        {
            IEnumPins iEnum;
            iBF.EnumPins(out iEnum);

            IPin pin;
            uint fetched;
            iEnum.Next(1, out pin, out fetched);

            while (fetched == 1)
            {
                if (String.Compare(name, Pin.Name(pin)) == 0)
                {
                    return pin;
                }

                iEnum.Next(1, out pin, out fetched);
            }

            return null;
        }

        public static IPin GetPin(IBaseFilter iBF, _PinDirection pinDir, Guid category, Guid type, bool unconnected, int num)
        {
            GCHandle hCatGuid = new GCHandle();
            GCHandle hTypeGuid = new GCHandle();
            IntPtr pCatGuid = IntPtr.Zero;
            IntPtr pTypeGuid = IntPtr.Zero;

            if(category != Guid.Empty)
            {
                hCatGuid = GCHandle.Alloc(category, GCHandleType.Pinned);
                pCatGuid = hCatGuid.AddrOfPinnedObject();
            }
            else if(type != Guid.Empty)
            {
                hTypeGuid = GCHandle.Alloc(type, GCHandleType.Pinned);
                pTypeGuid = hTypeGuid.AddrOfPinnedObject();
            }

            IPin pin = null;

            try
            {
                ICaptureGraphBuilder2 cgb2 = CaptureGraphBuilder2Class.CreateInstance();
                cgb2.FindPin(iBF, pinDir, pCatGuid, pTypeGuid, unconnected, num, out pin);
            }
            finally
            {
                if(pCatGuid != IntPtr.Zero)
                {
                    hCatGuid.Free();
                }

                if(pTypeGuid != IntPtr.Zero)
                {
                    hTypeGuid.Free();
                }
            }

            return pin;
        }

        public static ArrayList GetPins(IBaseFilter iBF)
        {
            ArrayList pins = new ArrayList();

            IEnumPins iEnum;
            iBF.EnumPins(out iEnum);

            IPin pin;
            uint pcFetched = 0;

            // All DShow examples seem to use 1 for cPins even though it supports an array
            // This works out well in practice because the RCW converter listed it as an out IPin
            iEnum.Next(1, out pin, out pcFetched);

            while(pcFetched == 1)
            {
                pins.Add(pin);
                iEnum.Next(1, out pin, out pcFetched);
            }

            return pins;
        }

        public static ArrayList GetPins(ArrayList pins, _PinDirection dir)
        {
            ArrayList ret = new ArrayList();

            foreach(IPin pin in pins)
            {
                _PinDirection pinDir;
                pin.QueryDirection(out pinDir);

                if(pinDir == dir)
                {
                    ret.Add(pin);
                }
            }

            return ret;
        }

        
        public static bool HasDialog(IBaseFilter iBF)
        {
            return iBF != null && iBF is ISpecifyPropertyPages;
        }

        public static void ShowDialog(IBaseFilter iBF, IntPtr hwnd)
        {
            Debug.Assert(HasDialog(iBF));

            PropertyPage pp = new PropertyPage((ISpecifyPropertyPages)iBF, Filter.Name(iBF));
            pp.Show(hwnd);
        }

        #endregion Statics
    
        #region Members
    
        private FilterInfo fi;
    
        protected IBaseFilter filter = null;
        protected FilgraphManager fgm;
    
        private ArrayList inputPins = null;
        private IPin inputPin = null;
    
        private ArrayList outputPins = null;
        private IPin outputPin = null;
    
        #endregion Members

        #region Constructor, Dispose

        public Filter(FilterInfo fi)
        {
            this.fi = fi;
            filter = InstantiateFilter();
        }

        /// <summary>
        /// Instantiates the IBaseFilter
        /// </summary>
        protected virtual IBaseFilter InstantiateFilter()
        {
             return CreateBaseFilter(this.fi);
        }


        public virtual void Dispose()
        {
            filter = null;
            fgm = null;
            inputPin = null;
            outputPin = null;

            ClearCollections();
        }


        #endregion Constructor, Dispose

        #region Public

        /// <summary>
        /// Initializes the device by instantiating the filter, adding it into the filtergraph,
        /// adding any upstream filters (like crossbars) 
        /// 
        /// If any of these actions fail, the device cleans itself up by calling Dispose.
        /// </summary>
        public virtual void AddedToGraph(FilgraphManager fgm)
        {
            // Store graph we will be a part of
            this.fgm = fgm;

            // Retrieve all pins for this device
            ArrayList pins = GetPins(filter);
            outputPins = GetPins(pins, _PinDirection.PINDIR_OUTPUT);
            inputPins = GetPins(pins, _PinDirection.PINDIR_INPUT);

            InitializeInputPin();
            InitializeOutputPin();
        }


        public string FriendlyName
        {
            get { return fi.DisplayName;}
        }

        public string Moniker
        {
            get { return fi.Moniker;}
        }

        public Guid Category
        {
            get { return fi.Category;}
        }


        public IBaseFilter BaseFilter
        {
            get{return filter;}
        }

        /// <summary>
        /// If the Filter instance actually has more than one filter, this is the 
        /// furthest downstream.
        /// </summary>
        public virtual IBaseFilter DownstreamBaseFilter {
            get { return filter; }
        }

        public ArrayList InputPins
        {
            get{return inputPins;}
        }

        public virtual IPin InputPin
        {
            get
            {
                return inputPin;
            }

            set
            {
                if(!inputPins.Contains(value))
                {
                    throw new Exception(Strings.InputPinError);
                }

                inputPin = value;
            }
        }

        public int InputPinIndex
        {
            get
            {
                return InputPins.IndexOf(InputPin);
            }

            set
            {
                if(value < 0 || value >= inputPins.Count)
                {
                    throw new IndexOutOfRangeException(string.Format(CultureInfo.CurrentCulture, Strings.InputPinCount,
                        inputPins.Count, value));
                }

                InputPin = (IPin)inputPins[value];
            }
        }


        public ArrayList OutputPins
        {
            get{return outputPins;}
        }

        public virtual IPin OutputPin
        {
            get
            {
                return outputPin;
            }

            set
            {
                if(!outputPins.Contains(value))
                {
                    throw new Exception(Strings.OutputPinError);
                }

                outputPin = value;
            }
        }

        public int OutputPinIndex
        {
            get
            {
                return OutputPins.IndexOf(OutputPin);
            }

            set
            {
                if(value < 0 || value >= outputPins.Count)
                {
                    throw new IndexOutOfRangeException(string.Format(CultureInfo.CurrentCulture, Strings.OutputPinCount,
                        outputPins.Count, value));
                }

                OutputPin = (IPin)outputPins[value];
            }
        }


        /// <summary>
        /// Gets the Output pin's current _AMMediaType, without freeing pbFormat
        /// Caller should call MediaType.Free(ref _AMMediaType) when done
        /// </summary>
        public _AMMediaType GetMediaType()
        {
            return Pin.GetMediaType((IAMStreamConfig)OutputPin);
        }

        /// <summary>
        /// Gets the Output pin's available _AMMediaTypes, without freeing pbFormat
        /// Caller should call MediaType.Free(_AMMediaType[]) when done
        /// </summary>
        public _AMMediaType[] GetMediaTypes()
        {
            return Pin.GetMediaTypes(OutputPin);
        }

        /// <summary>
        /// Gets the Output pin's current _AMMediaType, and frees pbFormat
        /// </summary>
        public virtual void GetMediaType(out _AMMediaType mt, out object formatBlock)
        {
            Pin.GetMediaType((IAMStreamConfig)OutputPin, out mt, out formatBlock);
        }

        /// <summary>
        /// Gets the Output pin's available _AMMediaTypes, and frees the pbFormats
        /// </summary>
        public void GetMediaTypes(out _AMMediaType[] mts, out object[] formatBlocks)
        {
           Pin.GetMediaTypes(OutputPin, out mts, out formatBlocks);
        }

        /// <summary>
        /// Sets the media type on the Output pin, but doesn't free it
        /// </summary>
        public void SetMediaType(_AMMediaType mt)
        {
            Pin.SetMediaType((IAMStreamConfig)OutputPin, mt);
        }

        /// <summary>
        /// Sets the media type on the Output pin, then frees it
        /// </summary>
        public void SetMediaType(ref _AMMediaType mt)
        {
            Pin.SetMediaType((IAMStreamConfig)OutputPin, ref mt);
        }

        /// <summary>
        /// Sets the media type on the Output pin, then frees it
        /// </summary>
        public void SetMediaType(_AMMediaType mt, object formatBlock)
        {
            Pin.SetMediaType((IAMStreamConfig)OutputPin, mt, formatBlock);
        }

        
        public virtual string Dump()
        {
            string ret = string.Format(CultureInfo.CurrentCulture, "\r\nDebug info for - {0}", FriendlyName);

            _AMMediaType[] mts;
            object[] fbs;

            if(InputPins.Count > 0)
            {
                ret += "\r\nInput pins";
                foreach(IPin pin in InputPins)
                {
                    ret += "\r\n\t" + Pin.Name(pin);
                }

                ret += "\r\nCurrent input pin - " + Pin.Name(InputPin);

                // Due to a bug in the WMAudio Encoder DMO which forgets to
                // terminate the enumeration, we special case it
                if(FriendlyName != "WMAudio Encoder DMO")
                {
                    try
                    {
                        Pin.GetMediaTypes(InputPin, out mts, out fbs);

                        ret += string.Format(CultureInfo.CurrentCulture, "\r\nMedia Types [{0}]...", mts.Length);
                        for(int i = 0; i < mts.Length; i++)
                        {
                            ret += string.Format(CultureInfo.CurrentCulture, "\r\nMedia Type [{0}]", i);
                            ret += MediaType.Dump(mts[i]);
                            ret += MediaType.FormatType.Dump(fbs[i]);
                            ret += "\r\n";
                        }
                    }
                    catch(COMException){} // Do nothing
                }
            }

            ret += "\r\nOutput pins";
            foreach(IPin pin in OutputPins)
            {
                ret += "\r\n\t" + Pin.Name(pin);
            }

            ret += "\r\nCurrent output pin - " + Pin.Name(OutputPin);
            
            mts = null;
            fbs = null;
            GetMediaTypes(out mts, out fbs);

            ret += string.Format(CultureInfo.CurrentCulture, "\r\nMedia Types [{0}]...", mts.Length);

            for(int i = 0; i < mts.Length; i++)
            {
                ret += string.Format(CultureInfo.CurrentCulture, "\r\nMedia Type [{0}]", i);
                ret += MediaType.Dump(mts[i]);
                ret += MediaType.FormatType.Dump(fbs[i]);
                ret += "\r\n";
            }

            return ret;
        }

        
        /// <summary>
        /// Decide on the initially active InputPin and initialize it
        /// 
        /// 1. DefaultInputPin in derived class 
        /// 2. DefaultInputPin in base class - index 0
        /// 
        /// Note: Filter is in graph
        /// </summary>
        protected virtual void InitializeInputPin()
        {
            if(inputPins.Count > 0)
            {
                InputPin = DefaultInputPin();
            }

            // No initialization work here in base
        }

        /// <summary>
        /// Allow derived classes to choose the initially active InputPin
        /// </summary>
        protected virtual IPin DefaultInputPin()
        {
            return (IPin)inputPins[0];
        }

        /// <summary>
        /// <para>
        /// Decide on the initially active OutputPin and initialize it
        /// 
        /// Note: Filter is in graph
        /// </para>
        /// <list>
        /// 1. DefaultOutputPin in derived class 
        /// 2. DefaultOutputPin in base class - capture pin
        /// </list>
        /// </summary>
        protected virtual void InitializeOutputPin()
        {
            if(outputPins.Count > 0)
            {
                OutputPin = DefaultOutputPin();
            }

            // No initialization work to do in base
        }
        
        /// <summary>
        /// Allow derived classes to choose the initially active OutputPin
        /// </summary>
        protected virtual IPin DefaultOutputPin()
        {
            return (IPin)outputPins[0];
        }

        
        public virtual bool HasSourceDialog
        {
            get{return Filter.HasDialog(BaseFilter);}
        }

        public virtual bool HasFormatDialog
        {
            get{return Pin.HasDialog(OutputPin);}
        }

        public virtual void ShowSourceDialog(IntPtr hwnd)
        {
            Filter.ShowDialog(BaseFilter, hwnd);
        }

        public virtual void ShowFormatDialog(IntPtr hwnd)
        {
            Pin.ShowDialog(OutputPin, hwnd);
        }

        #endregion Public

        #region Private

        private void ClearCollections()
        {
            ClearCollection(outputPins);
            ClearCollection(inputPins);
        }

        private void ClearCollection(IList list)
        {
            if(list != null)
            {
                list.Clear();
            }
        }


        #endregion Private
    }


    public abstract class SourceFilter : Filter
    {
        #region Statics

        public static string DumpCrossbar(IAMCrossbar iCB)
        {
            string ret = "\r\nCrossbar\r\n";

            if(iCB != null)
            {
                ArrayList pins = GetPins((IBaseFilter)iCB);
                ArrayList inputPins = GetPins(pins, _PinDirection.PINDIR_INPUT);
                ArrayList outputPins = GetPins(pins, _PinDirection.PINDIR_OUTPUT);

                int inPins, outPins;
                iCB.get_PinCounts(out outPins, out inPins);

                ret += "\tInput Pins...\r\n";
                for(int inIndex = 0; inIndex < inPins; inIndex++)
                {
                    int related, type;
                    iCB.get_CrossbarPinInfo(true, inIndex, out related, out type);

                    ret += string.Format(CultureInfo.CurrentCulture, "\t{0}, Related input pin: {1}\r\n", 
                        Pin.Name((IPin)inputPins[inIndex]), related);
                }

                ret += "\r\n\tOutput Pins...";
                for(int outIndex = 0; outIndex < outPins; outIndex++)
                {
                    int related, type, routed;
                    iCB.get_CrossbarPinInfo(false, outIndex, out related, out type);
                    iCB.get_IsRoutedTo(outIndex, out routed);

                    ret += string.Format(CultureInfo.CurrentCulture, 
                        "\r\n\t{0}, Related output pin: {1}, Routed input pin: {2}" + 
                        Environment.NewLine, Pin.Name((IPin)outputPins[outIndex]), related, routed);

                    ret += "\tSwitching Matrix (which input pins this output pin can accept): ";

                    for(int inIndex = 0; inIndex < inPins; inIndex++)
                    {
                        ret += string.Format(CultureInfo.CurrentCulture, "{0}-{1}", inIndex, 
                            (iCB.CanRoute(outIndex, inIndex) == 0) ? "Yes " : "No ");
                    }
                }
            }

            return ret;
        }


        public static string PhysicalConnectorToString(tagPhysicalConnectorType pct)
        {
            string ret = pct.ToString();
            return ret.Substring(ret.IndexOf("_") + 1);
        }


        /// <summary>
        /// Makes sure each FilterInfo.DisplayName is unique
        /// by adding a number onto the end of duplicates
        /// </summary>
        public static FilterInfo[] UniquifyDisplayNames(FilterInfo[] fis)
        {
            // The last one would have nothing to compare against
            for(int i = 0; i < fis.Length - 1; i++)
            {
                // The uniquifying number
                int id = 0;

                // No need to compare against yourself or anyone before you
                for(int j = i + 1; j < fis.Length; j++)
                {
                    if(fis[i].DisplayName == fis[j].DisplayName)
                    {
                        fis[j].DisplayName = string.Format(CultureInfo.CurrentCulture, "{0} [{1}]", fis[j].DisplayName, ++id);
                    }
                }
            }

            return fis;
        }

        #endregion Statics

        #region Members

        /// <summary>
        /// Pointer to the crossbar interface if it is present
        /// </summary>
        private IAMCrossbar iCB = null;

        /// <summary>
        /// Index of the output pin on the Crossbar filter
        /// </summary>
        private int cbOutPinIdx = -1;

        /// <summary>
        /// Physical connectors (input pins) on the crossbar filter that are 
        /// routable to the output pin the SourceFilter is connected to
        /// </summary>
        private PhysicalConnector[] physConns;

        /// <summary>
        /// A collection of filters upstream of the source
        /// </summary>
        private IBaseFilter[] upstreamFilters;

        #endregion Members

        #region Constructor

        public SourceFilter(FilterInfo fi) : base(fi){}
        

        #endregion Constructor

        #region Public

        /// <summary>
        /// Returns a pointer to the crossbar filter
        /// </summary>
        public IAMCrossbar Crossbar
        {
            get { return iCB; }
        }

        /// <summary>
        /// Whether or not this filter has a crossbar
        /// </summary>
        public bool HasPhysConns
        {
            get{return iCB != null;}
        }
        /// <summary>
        /// Returns the collection of PhysicalConnectors on the crossbar that
        /// are routable to the current output pin on the crossbar
        /// </summary>
        public PhysicalConnector[] PhysicalConnectors
        {
            get
            {
                ValidateCrossbar();
                return physConns;
            }
        }

        /// <summary>
        /// Allows you to get / set which physical connector to use
        /// from the PhysicalConnectors collection 
        /// 
        /// It always queries the device on the get
        /// </summary>
        public int PhysicalConnectorIndex
        {
            get
            {
                ValidateCrossbar();
                
                int inIdx;
                if(iCB.get_IsRoutedTo(cbOutPinIdx, out inIdx) == 0) // S_OK
                {
                    for(int i = 0; i < physConns.Length; i++)
                    {
                        if(physConns[i].Index == inIdx)
                        {
                            return i;
                        }
                    }
                }

                string msg = Strings.CrossbarOutputPinNotRouted;

                Debug.Fail(msg);
                throw new InvalidOperationException(msg);
            }

            set
            {
                ValidateCrossbar();

                if(value != PhysicalConnectorIndex)
                {
                    // Make sure index is within range
                    if(value < 0 || value >= physConns.Length)
                    {
                        throw new IndexOutOfRangeException(string.Format(CultureInfo.CurrentCulture, 
                            Strings.InvalidIndex, physConns.Length, value));
                    }

                    iCB.Route(cbOutPinIdx, value);
                }
            }
        }

        /// <summary>
        /// Returns a string representation of the current PhysicalConnector
        /// </summary>
        public string CurrentPhysicalConnector
        {
            get
            {
                ValidateCrossbar();
                return Pin.Name(PhysicalConnectors[PhysicalConnectorIndex].Pin);
            }
        }
        
        /// <summary>
        /// Returns a collection of the filters upstream from the source 
        /// </summary>
        public IBaseFilter[] UpstreamFilters
        {
            get { return upstreamFilters; }
        }


        /// <summary>
        /// A virtual method that is called once the filter is in a graph
        /// Allows the filter to configure itself.
        /// </summary>
        public override void AddedToGraph(FilgraphManager fgm)
        {
            base.AddedToGraph(fgm);

            // Add Crossbar filter or other filters upstream of this filter
            BuildUpstreamGraph();
        }
                

        /// <summary>
        /// For a SourceFilter, we always want the capture pin
        /// </summary>
        protected override IPin DefaultOutputPin()
        {
            return GetPin(filter, _PinDirection.PINDIR_OUTPUT, 
                Pin.PIN_CATEGORY_CAPTURE, Guid.Empty, false, 0);
        }


        public override string Dump()
        {
            string ret = base.Dump();

            if(HasPhysConns)
            {
                ret += SourceFilter.DumpCrossbar(Crossbar);
                ret += "\r\n";
            }

            return ret;
        }


        #endregion Public

        #region Private

        /// <summary>
        /// Attempts to insert the crossbar filter and then discover the
        /// physical connectors (input pins) on it
        /// </summary>
        private void BuildUpstreamGraph()
        {
            InsertCrossbar();

            if(iCB != null)
            {
                FindPhysicalConnectors();
                FindUpstreamFilters();
            }
        }

        
        /// <summary>
        /// Insert the crossbar and other upstream filters if the device
        /// needs them.
        /// </summary>
        private void InsertCrossbar()
        {
            // Build upstream filters (crossbar)
            GCHandle hCat = GCHandle.Alloc(LOOK_UPSTREAM_ONLY, GCHandleType.Pinned);

            try
            {
                IntPtr pCat = hCat.AddrOfPinnedObject();
                Guid g = Marshal.GenerateGuidForType(typeof(IAMCrossbar));

                ICaptureGraphBuilder2 icgb2 = CaptureGraphBuilder2Class.CreateInstance();
                icgb2.SetFiltergraph((IGraphBuilder)fgm);

                object oCB;
                icgb2.RemoteFindInterface(pCat, IntPtr.Zero, filter, ref g, out oCB);
                iCB = (IAMCrossbar)oCB;
            }
            catch(COMException ce)
            {
                if((uint)ce.ErrorCode != 0x80004005) // IAMCrossbar not found
                {
                    string msg = DShowError._AMGetErrorText(ce.ErrorCode);

                    Trace.WriteLine(msg);
                    throw new Exception(string.Format(CultureInfo.CurrentCulture, Strings.DirectshowErrortext, msg), ce);
                }
            }
            finally
            {
                hCat.Free();
            }
        }
                

        /// <summary>
        /// Find out which input pins on the crossbar can route to the output
        /// pin on the crossbar and the physical connection type of each input pin
        /// </summary>
        private void FindPhysicalConnectors()
        {
            // Retrieve all of the pins from the Crossbar filter
            ArrayList cbPins = Filter.GetPins((IBaseFilter)iCB);
            ArrayList cbInPins = Filter.GetPins(cbPins, _PinDirection.PINDIR_INPUT);
            ArrayList cbOutPins = Filter.GetPins(cbPins, _PinDirection.PINDIR_OUTPUT);

            // Find out which output pin from the crossbar filter our input pin is connected to
            IPin pin;
            InputPin.ConnectedTo(out pin);
            Debug.Assert(pin != null);

            // Identify the index of the output pin
            cbOutPinIdx = cbOutPins.IndexOf(pin);
            Debug.Assert(cbOutPinIdx != -1);

            // Find out which input pins can route to the output pin on the 
            // crossbar and the physical connection type of each input pin
            int outCount, inCount;
            iCB.get_PinCounts(out outCount, out inCount);

            Debug.Assert(outCount == cbOutPins.Count);
            Debug.Assert(inCount == cbInPins.Count);

            ArrayList physConns = new ArrayList();
            for(int i = 0; i < inCount; i++)
            {
                if(iCB.CanRoute(cbOutPinIdx, i) == 0)
                {
                    int related, physicalType;
                    iCB.get_CrossbarPinInfo(true, i, out related, out physicalType);

                    physConns.Add(new PhysicalConnector((IPin)cbInPins[i],
                        (tagPhysicalConnectorType)physicalType, i));
                }
            }

            // Strongly type the collection
            this.physConns = new PhysicalConnector[physConns.Count];
            physConns.CopyTo(this.physConns);
        }


        /// <summary>
        /// Finds all the filters upstream of the source
        /// </summary>
        private void FindUpstreamFilters()
        {
            ArrayList alUpstreamFilters = new ArrayList();

            foreach(IBaseFilter iBF in FilterGraph.FiltersInGraph((IFilterGraph)fgm))
            {
                if(iBF != BaseFilter)
                {
                    alUpstreamFilters.Add(iBF);
                }
            }

            // Strongly type, for some reason CopyTo fails with an InvalidCastException
            upstreamFilters = new IBaseFilter[alUpstreamFilters.Count];
            
            for(int i = 0; i < alUpstreamFilters.Count; i++)
            {
                upstreamFilters[i] = (IBaseFilter)alUpstreamFilters[i];
            }
        }


        /// <summary>
        /// Throws an exception if this SourceFilter doesn't have a crossbar
        /// filter upstream of it
        /// </summary>
        private void ValidateCrossbar()
        {
            if(iCB == null)
            {
                throw new ApplicationException(Strings.DeviceDoesNotSupportError);
            }
        }


        #endregion Private
    }

    public class VideoSource : SourceFilter
    {
        #region Statics

        /// <summary>
        /// Guid for video Capture Sources 
        /// </summary>
        public static Guid CategoryGuid
        {
            get {return Filter.CLSID_VideoInputDeviceCategory;}
        }

        
        /// <summary>
        /// Return the current list of video (capture) sources on the machine.  The list is 
        /// dynamically generated for each call, as PnP devices may come and go.
        /// </summary>
        public static FilterInfo[] Sources()
        {
            // Get a new list each time
            return UniquifyDisplayNames(EnumerateFilters(CategoryGuid));
        }


        /// <summary>
        /// Formats the AnalogVideoStandard string to make it shorter
        /// Changes AnalogVideo_NTSC -> NTSC
        /// </summary>
        /// <param name="av"></param>
        /// <returns></returns>
        public static string VideoStandardToString(tagAnalogVideoStandard av)
        {
            string ret = av.ToString();
            return ret.Substring(ret.IndexOf("_") + 1);
        }

        
        #endregion Statics

        #region Members

        /// <summary>
        /// Interface for supporting IAMVfwCaptureDialogs
        /// </summary>
        private IAMVfwCaptureDialogs iVfwCap;
        
        /// <summary>
        /// Interface for supporting IAMAnalogVideoDecoder (AnalogVideoStandards)
        /// </summary>
        private IAMAnalogVideoDecoder iAVD;
        
        /// <summary>
        /// Video standards supported by this filter
        /// </summary>
        private tagAnalogVideoStandard[] videoStandards;

        #endregion Members

        #region Constructor

        public VideoSource(FilterInfo fi) : base(fi)
        {
            if(fi.Category != VideoSource.CategoryGuid)
            {
                Debug.Assert(false);
                throw new ArgumentOutOfRangeException("fi.Category", fi.Category, Strings.UnexpectedFilterCategory);
            }
        }


        #endregion Constructor
                
        #region Public
        
        public override void AddedToGraph(FilgraphManager fgm)
        {
            base.AddedToGraph(fgm);

            iVfwCap = filter as IAMVfwCaptureDialogs;

            #region IAMAnalogVideoDecoder

            iAVD = filter as IAMAnalogVideoDecoder;

            if(iAVD != null)
            {
                int atvf;
                iAVD.get_AvailableTVFormats(out atvf);

                if(atvf != (int)tagAnalogVideoStandard.AnalogVideo_None)
                {
                    ArrayList videoStandards = new ArrayList();

                    foreach(tagAnalogVideoStandard avs in Enum.GetValues(typeof(tagAnalogVideoStandard)))
                    {
                        if((atvf & (int)avs) == (int)avs)
                        {
                            videoStandards.Add(avs);
                        }
                    }

                    this.videoStandards = new tagAnalogVideoStandard[videoStandards.Count];
                    videoStandards.CopyTo(0, this.videoStandards, 0, videoStandards.Count);
                }
            }

            #endregion IAMAnalogVideoDecoder
        }


        public bool HasVideoStandards
        {
            get{return iAVD != null;}
        }

        public tagAnalogVideoStandard[] VideoStandards
        {
            get
            {
                ValidateAnalogVideo();
                return videoStandards;
            }
        }
        
        public int VideoStandardIndex
        {
            get
            {
                ValidateAnalogVideo();

                int iVidStd;
                iAVD.get_TVFormat(out iVidStd);

                tagAnalogVideoStandard vidStd = (tagAnalogVideoStandard)iVidStd;

                for(int i = 0; i < videoStandards.Length; i++)
                {
                    if(VideoStandards[i] == vidStd)
                    {
                        return i;
                    }
                }

                string msg = Strings.VideoStandardDoesNotMap;

                Debug.Fail(msg);
                throw new InvalidOperationException(msg);
            }

            set
            {
                ValidateAnalogVideo();

                if(VideoStandardIndex != value)
                {
                    // Make sure index is within range
                    if(value < 0 || value >= videoStandards.Length)
                    {
                        throw new IndexOutOfRangeException(string.Format(CultureInfo.CurrentCulture, 
                            Strings.InvalidIndex, videoStandards.Length, value));
                    }

                    iAVD.put_TVFormat((int)videoStandards[value]);
                }
            }
        }

        public string CurrentVideoStandard
        {
            get
            {
                ValidateAnalogVideo();
                return VideoSource.VideoStandardToString(
                    VideoStandards[VideoStandardIndex]);
            }
        }


        public bool IsVfw
        {
            get { return iVfwCap != null; }
        }

        public override bool HasSourceDialog
        {
            get 
            {
                return base.HasSourceDialog || IsVfw && 
                    (iVfwCap.HasDialog((int)VfwCaptureDialogs.VfwCaptureDialog_Source) == 0);
            }
        }

        public override bool HasFormatDialog
        {
            get 
            {
                return base.HasFormatDialog || IsVfw && 
                    (iVfwCap.HasDialog((int)VfwCaptureDialogs.VfwCaptureDialog_Format) == 0);
            }
        }


        public override void ShowSourceDialog(IntPtr hwnd)
        {
            if(HasSourceDialog)
            {
                if(base.HasSourceDialog)
                {
                    base.ShowSourceDialog(hwnd);
                }
                else
                {
                    iVfwCap.ShowDialog((int)VfwCaptureDialogs.VfwCaptureDialog_Source, hwnd);
                }
            }
        }

        public override void ShowFormatDialog(IntPtr hwnd)
        {
            if(HasFormatDialog)
            {
                if(base.HasFormatDialog)
                {
                    base.ShowFormatDialog(hwnd);
                }
                else
                {
                    iVfwCap.ShowDialog((int)VfwCaptureDialogs.VfwCaptureDialog_Format, hwnd);
                }
            }
        }
        

        public override string Dump()
        {
            string ret = base.Dump ();

            if(iVfwCap != null)
            {
                ret += "\r\nSupports IAMVfwCaptureDialogs";
            }

            if(HasSourceDialog)
            {
                ret += "\r\nHas SourceDialog";
            }

            if(HasFormatDialog)
            {
                ret += "\r\nHas FormatDialog";
            }

            if(HasVideoStandards)
            {
                ret += "\r\nVideo Standards";
                foreach(tagAnalogVideoStandard avs in VideoStandards)
                {
                    ret += "\r\n\t" + VideoSource.VideoStandardToString(avs);
                }

                ret += "\r\nDefault Video Standard - " + CurrentVideoStandard;
            }

            return ret;
        }


        #endregion Public

        #region Private

        private void ValidateVfw()
        {
            if(iVfwCap == null)
            {
                throw new ApplicationException(Strings.DoesNotSupportIAMVfwCaptureDialogs);
            }
        }

        private void ValidateAnalogVideo()
        {
            if(iAVD == null)
            {
                throw new ApplicationException(Strings.DoesNotSupportIAMAnalogVideoDecoder);
            }
        }


        #endregion Private
    }

    public class AudioSource : SourceFilter
    {
        #region Statics

        /// <summary>
        /// Guid for Audio Capture Sources 
        /// </summary>
        public static Guid CategoryGuid
        {
            get {return Filter.CLSID_AudioInputDeviceCategory;}
        }

        
        /// <summary>
        /// Return the current list of audio (capture) sources on the machine.  The list is 
        /// dynamically generated for each call, as PnP devices may come and go.
        /// </summary>
        public static FilterInfo[] Sources()
        {
            // Get a new list each time
            // In addition to the audio source filters, also look for video DV devices that support audio.
            FilterInfo[] videoSources = EnumerateFilters(VideoSource.CategoryGuid);
            List<FilterInfo> audioSources = new List<FilterInfo>(EnumerateFilters(CategoryGuid));
            foreach (FilterInfo fi in videoSources) {
                if (DVSource.IsDVSourceWithAudio(fi)) {
                    audioSources.Add(fi);
                }
            }
            return UniquifyDisplayNames(audioSources.ToArray());
        }

        public static readonly int DEFAULT_BUFFER_SIZE = 5000;
        public static readonly int DEFAULT_BUFFER_COUNT = 5;

        #endregion Statics

        #region Members

        private int bufferSize = DEFAULT_BUFFER_SIZE;
        private int bufferCount = DEFAULT_BUFFER_COUNT;
        
        #endregion Members

        #region Constructor

        public AudioSource(FilterInfo fi) : base(fi)
        {
            if(fi.Category != AudioSource.CategoryGuid)
            {
                Debug.Assert(false);
                throw new ArgumentOutOfRangeException("fi.Category", fi.Category, Strings.UnexpectedFilterCategory);
            }
        }
        

        #endregion Constructor

        #region Public
  
        /// <summary>
        /// Gets / Sets the current Input Pin
        /// </summary>
        public override IPin InputPin
        {
            get
            {
                IPin ret = base.InputPin;

                foreach(IPin pin in InputPins)
                {
                    IAMAudioInputMixer iAIM = pin as IAMAudioInputMixer;
                    if(iAIM != null)
                    {
                        try
                        {
                            bool enabled = false;
                            iAIM.get_Enable(out enabled);

                            // Return the first pin that is enabled
                            if(enabled)
                            {
                                ret = pin;
                                break;
                            }
                        }
                        catch(NotImplementedException){}
                        catch (COMException) { }
                    }
                }
            
                return ret;
            }
            set
            {
                base.InputPin = value;

                // Even though we just set the InputPin don't use it here
                // or it will change the pin before you can set it to be enabled
                IAMAudioInputMixer iAIM = value as IAMAudioInputMixer;

                if(iAIM != null)
                {
                    try
                    {
                        iAIM.put_Enable(true);
                    }
                    catch(NotImplementedException){} // Not much we can do
                    catch (COMException) { }
                }
            }
        }


        /// <summary>
        /// Change the default buffer size and number of buffers in order to
        /// produce low latency audio.
        /// </summary>
        protected override void InitializeOutputPin()
        {
            base.InitializeOutputPin();

            SetAllocatorProperties();
        }

        private void SetAllocatorProperties() { 
            _AllocatorProperties pprop = new _AllocatorProperties();
            pprop.cbPrefix = 0;
            pprop.cbAlign = 1;
            pprop.cbBuffer = bufferSize;
            pprop.cBuffers = bufferCount;
            
            ((IAMBufferNegotiation)OutputPin).SuggestAllocatorProperties(ref pprop);            
        }
        
        /// <summary>
        /// Returns the pin that is currently enabled
        /// </summary>
        protected override IPin DefaultInputPin()
        {
            IPin ret = InputPin;

            if(ret == null)
            {
                ret = base.DefaultInputPin();
            }

            return ret;
        }

        public int BufferSize {
            set { 
                bufferSize = value;
                SetAllocatorProperties();
            }
        }

        public int BufferCount {
            set { 
                bufferCount = value;
                SetAllocatorProperties();
            }
        }

        #endregion Public
    }

    public class DVSource : SourceFilter {
        //Source filters that are known to have DV Audio:
        private static readonly string DV_FILTER_NAME = "Microsoft DV Camera and VCR";
        private static readonly string DV_FILTER_NAME2 = "AV/C Subunit";

        private static int DVOutputPinIndex = 1;
        private static List<string> DVSourceFilters;

        static DVSource() {
            string setting;
            DVSourceFilters = new List<string>();

            //Allow DV audio to be disabled.  We just don't add any source filters so that
            //IsDVSourceWithAudio always returns false.
            if ((setting = ConfigurationManager.AppSettings[AppConfig.DVAudioDisabled]) != null) {
                bool b;
                if (bool.TryParse(setting, out b)) {
                    if (b) {
                        return;
                    }
                }
            }

            DVSourceFilters.Add(DV_FILTER_NAME);
            DVSourceFilters.Add(DV_FILTER_NAME2);

            //In case the user wants to use a different DV source filter or output pin, one can be configured.
            if ((setting = ConfigurationManager.AppSettings[AppConfig.DVSourceFilterName]) != null) {
                DVSourceFilters.Add(setting);
            }

            if ((setting = ConfigurationManager.AppSettings[AppConfig.DVOutputPinIndex]) != null) {
                int i;
                if (int.TryParse(setting, out i)) {
                    if (i >= 0) {
                        DVSource.DVOutputPinIndex = i;
                    }
                }
            }
        }

        public static bool IsDVSourceWithAudio(FilterInfo fi) {
            foreach (string s in DVSourceFilters) {
                Regex re = new Regex(s);
                if (re.IsMatch(fi.Name)) {
                    return true;
                }
            }
            return false;
        }

        protected override IPin DefaultOutputPin() {
            if (this.OutputPins.Count > DVSource.DVOutputPinIndex) {
                return (IPin)OutputPins[DVSource.DVOutputPinIndex];
            }
            return base.DefaultOutputPin();
        }

        public DVSource(FilterInfo fi): base(fi) {}

        public bool HasVideoStandards {
            get {
                return false;
            }
        }

        public tagAnalogVideoStandard[] VideoStandards {
            get {
                return null;
            }
        }

        public int VideoStandardIndex {
            get { 
                return 0; 
            }
            set { }
        }

    }

    public class NetworkSource : SourceFilter
    {
        #region Constructor

        public NetworkSource(FilterInfo fi) : base(fi){}
        

        #endregion Constructor

        protected override IBaseFilter InstantiateFilter()
        {
            return RtpSourceClass.CreateInstance();
        }
    }



    public abstract class Compressor : Filter
    {
        #region Constructor

        public Compressor(FilterInfo fi) : base(fi){}
        
        
        #endregion Constructor
    }

    public class VideoCompressor : Compressor
    {
        #region Statics

        /// <summary>
        /// Collection of video compressors on the machine
        /// </summary>
        private static FilterInfo[] compressors;

        /// <summary>
        /// Guid for the media type that will be used to select the compressor 
        /// </summary>
        private static Guid DefaultMediaType = MediaType.SubType.WMMEDIASUBTYPE_WMV1;

        /// <summary>
        /// The first or only compressor that supports this media type
        /// </summary>
        private static List<AvailableDMO> availableCompressors;

        static VideoCompressor()
        {
            // Enumerate compressors on the machine and strongly type them
            compressors = EnumerateFilters(CategoryGuid);

            // See if there is an override for the video media type
            string setting = ConfigurationManager.AppSettings[AppConfig.MDS_VideoMediaType];
            if (setting != null)
            {
                try
                {
                    DefaultMediaType = MediaType.SubType.StringToGuid(setting);
                }
                catch (FormatException) { }
            }

            // Find the available compressors that support the media type
            availableCompressors = DMOEnumerator.Enumerate(
                CategoryGuid, MediaType.MajorType.MEDIATYPE_Video,
                DefaultMediaType, false);
        }

        /// <summary>
        /// Video Compressors category Guid 
        /// </summary>
        public static Guid CategoryGuid = new Guid("33d9a760-90c8-11d0-bd43-00a0c911ce86");

        public static FilterInfo DefaultFilterInfo()
        {

            //If this config option is set, use it.
            string setting = ConfigurationManager.AppSettings[AppConfig.MDS_VideoCompressorName];
            if (!String.IsNullOrEmpty(setting)) {
                foreach (FilterInfo fi in VideoCompressor.Compressors) {
                    if (fi.Name == setting) {
                        return fi;
                    }
                }
            }
            
            foreach (FilterInfo fi in VideoCompressor.Compressors)
            {
                // We will use the first compressor that supports the media type
                if (availableCompressors.Count >= 1 &&
                    fi.Name == availableCompressors[0].Name) {
                    return fi;
                }
            }

            throw new ApplicationException(string.Format(CultureInfo.CurrentCulture, 
                Strings.UnableToFindACompressor, MediaType.SubType.GuidToString(DefaultMediaType)));
        }

        public static VideoCompressorQualityInfo DefaultQualityInfo
        {
            get
            {
                return new VideoCompressorQualityInfo(DefaultMediaType, 300000, 2000);
            }
        }

        /// <summary>
        /// Compressors is a static collection, because they don't tend to change like Pnp Audio / Video devices
        /// </summary>
        public static FilterInfo[] Compressors
        {
            get { return compressors; }
        }

        /// <summary>
        /// Video compressors are "static" devices.  That is to say, they do not come and go on the
        /// machine very often (like a USB webcam might).  They are also "multi-use", meaning you
        /// may have multiple instances of a compressor running, whereas you only have 1 instance
        /// of a webcam.  Therefore we hand out new instances each time.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static VideoCompressor CreateFilter(string name)
        {
            foreach (FilterInfo fi in compressors)
            {
                if (fi.Name == name)
                {
                    return new VideoCompressor(fi);
                }
            }

            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, 
                Strings.UnableToFindVideoCompressor, name));
        }

        #endregion Statics

        #region Members

        private VideoCompressorQualityInfo vcqi = VideoCompressor.DefaultQualityInfo;
        private _AMMediaType mt;
        private VIDEOINFOHEADER vih;

        private IAMVideoCompression iVC;
        private bool getInfo = false;

        private string version;
        private string description;
        private int defaultKFR = -1;
        private int defaultPFPKF = -1;
        private double defaultQuality = -1.0;
        private int capabilities = 0;

        #endregion Members

        #region Constructor

        public VideoCompressor(FilterInfo fi) : base(fi)
        {
            if(fi.Category != VideoCompressor.CategoryGuid)
            {
                Debug.Assert(false);
                throw new ArgumentOutOfRangeException("fi.Category", fi.Category, Strings.UnexpectedFilterCategory);
            }
        }

        
        #endregion Constructor

        #region Public
  
        public override IPin OutputPin
        {
            get
            {
                return base.OutputPin;
            }
            set
            {
                base.OutputPin = value;
                GetIAMVideoCompression();
            }
        }

        public VideoCompressorQualityInfo QualityInfo
        {
            get
            {
                return vcqi;
            }
            set
            {
                // Try setting the values on the compressor
                ConfigureCompressor(value);

                // Store value after successfully setting it
                vcqi = value;
            }
        }


        public bool SupportsIAMVideoCompression
        {
            get
            {
                return iVC != null;
            }
        }

        public bool SupportsGetInfo
        {
            get
            {
                return getInfo;
            }
        }

        public string Version
        {
            get
            {
                return version;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
        }
        
        public double DefaultQuality
        {
            get
            {
                return defaultQuality;
            }
        }
        public int DefaultKeyFrameRate
        {
            get
            {
                return defaultKFR;
            }
        }

        public int DefaultPFramesPerKeyFrame
        {
            get
            {
                return defaultPFPKF;
            }
        }

        public int Capabilities
        {
            get
            {
                return capabilities;
            }
        }


        #endregion Public

        #region Private

        private void ConfigureCompressor(VideoCompressorQualityInfo vcqi)
        {
            // This method is called after the compressor is connected to the
            // source filter, so that the media types and format blocks contain
            // useable information.
            _AMMediaType[] mts;
            object[] fbs;
            GetMediaTypes(out mts, out fbs);

            for(int i = 0; i < mts.Length; i++)
            {
                if(mts[i].subtype == vcqi.MediaSubType)
                {
                    mt = mts[i];
                    vih = (VIDEOINFOHEADER)fbs[i];
                    break;
                }
            }

            if (mt.subtype != vcqi.MediaSubType) {
                //If we are using non-standard codecs, there may not be a match.  
                //PRI2: Some compressors will likely need to be configured using their own custom tools or dialogs, or will require special case subclasses.
                return;
            }

            Debug.Assert(mt.subtype == vcqi.MediaSubType);

            // Configure the bit rate - .Net makes a copy of fb
            vih.BitRate = vcqi.BitRate;

            // Update the structure in memory with what we have
            mt = MediaType.Construct(mt, vih);

            // Allow compressor specific configuration
            // e.g. WM9+ requires extra configuration, others may as well
            CompressorSpecificConfiguration(vcqi);

            // Use the structure in the compressor - this will free the format
            // block when it is done.
            try {
                //This was observed to fail for some non-standard compressors.
                SetMediaType(ref mt);
            }
            catch (Exception ex) {
                Trace.WriteLine("Failed to set video compressor MediaType: " + ex.ToString());
            }

            // Check for other video compression settings
            IAMVideoCompression iVC = OutputPin as IAMVideoCompression;
            if(iVC != null)
            {
                // WMV9 and WMVAdv don't work well if you modify them this way
                if (FriendlyName != "WMVideo8 Encoder DMO" && 
                    FriendlyName != "WMVideo9 Encoder DMO")
                {
                    try {
                        iVC.put_KeyFrameRate(vcqi.KeyFrameRate);
                        iVC.put_Quality(vcqi.Quality);
                    }
                    catch(Exception ex) {
                        Trace.WriteLine("Failed to set video compressor quality: " + ex.ToString());
                    }
                }
            }

            CompressorDiagnostics("After setting media type");
        }
        
        private void CompressorSpecificConfiguration(VideoCompressorQualityInfo vcqi)
        {
            // ------------------------------------------------------------------------------------
            // Very specific to the WM9+ codec
            // Must come after setting the VideoInfo properties in order to get the "correct" private data
            // ------------------------------------------------------------------------------------
            if (FriendlyName == "WMVideo8 Encoder DMO" || 
                FriendlyName == "WMVideo9 Encoder DMO")
            {
                ConfigureWMEncoder(vcqi);
            }

            //
            // Add your compressor specific configuration needs here
            //
        }

        private void ConfigureWMEncoder(VideoCompressorQualityInfo vcqi)
        {
            //
            // Note: Configure compressor before setting private data
            //
            IPropertyBag iPB = (IPropertyBag)filter;
            object o = 1; // 1 == Live, can be obtained from IWMCodecProps.GetCodecProp(WM9PropList.g_wszWMVCComplexityExLive)
            iPB.Write(WM9PropList.g_wszWMVCComplexityEx, ref o);  

            if(vcqi.KeyFrameRate != VideoCompressorQualityInfo.KeyFrameRateDefault)
            {
                o = vcqi.KeyFrameRate;
                iPB.Write(WM9PropList.g_wszWMVCKeyframeDistance, ref o);
            }

            //                // More configuration possibilities
            //                o = 0;
            //                iPB.Write(WM9PropList.g_wszWMVCComplexityMode, ref o);
            //                
            //                o = 0;
            //                iPB.Write(WM9PropList.g_wszWMVCCrisp, ref o);
            //
            //                o = "MP";
            //                iPB.Write(WM9PropList.g_wszWMVCDecoderComplexityRequested, ref o);
            //
            //                o = 10000;
            //                iPB.Write(WM9PropList.g_wszWMVCVideoWindow, ref o);
            //
            //                o = true;
            //                iPB.Write(WM9PropList.g_wszWMVCVBREnabled, ref o);


            //
            // Set Private Data
            //
            IWMCodecPrivateData iPD = (IWMCodecPrivateData)filter;
            iPD.SetPartialOutputType(ref mt);

            uint cbData = 0;
            iPD.GetPrivateData(IntPtr.Zero, ref cbData);

            if(cbData != 0)
            {
                int vihSize = Marshal.SizeOf(vih);

                // Allocate space for video info header + private data
                IntPtr vipd = Marshal.AllocCoTaskMem(vihSize + (int)cbData);

                // Copy vih into place
                Marshal.StructureToPtr(vih, vipd, false);

                // Fill in private data
                iPD.GetPrivateData(new IntPtr(vipd.ToInt32() + vihSize), ref cbData);

                // Reset it
                MediaType.Free(ref mt); // Clean it up, so we can reuse it
                mt.pbFormat = vipd;
                mt.cbFormat = (uint)vihSize + cbData;
            }
        }

        private void CompressorDiagnostics(string msg)
        {
#if CompressorDiagnostics

            Trace.WriteLine(msg);

            #region IAMVideoCompression

            IAMVideoCompression iVC = null;

            try
            {
                iVC = (IAMVideoCompression)cOutputPin;
            }
            catch(InvalidCastException)
            {
                Trace.WriteLine("Compressor does not support IAMVideoCompression");
            }
            
            if(iVC != null)
            {
                try
                {
                    int pcbVersion = 0;
                    int pcbDescription = 0;
                    int defaultKFR, defaultPFPK, capabilities;
                    double defaultQuality;

                    // Make the call to get the lengths of the strings
                    iVC.GetInfo(null, ref pcbVersion, null, ref pcbDescription, 
                        out defaultKFR, out defaultPFPK, out defaultQuality, out capabilities);


                    StringBuilder version = new StringBuilder(pcbVersion / 2);
                    StringBuilder description = new StringBuilder(pcbDescription / 2);
                    
                    iVC.GetInfo(version, ref pcbVersion, description, ref pcbDescription, 
                        out defaultKFR, out defaultPFPK, out defaultQuality, out capabilities);

                    Trace.WriteLine(version.ToString());
                    Trace.WriteLine(description.ToString());
                    Trace.WriteLine(defaultKFR);
                    Trace.WriteLine(defaultPFPK);
                    Trace.WriteLine(defaultQuality);
                    Trace.WriteLine(capabilities);
                }
                catch(NotImplementedException)
                {
                    Trace.WriteLine("Compressor does not support IAMVideoCompression.GetInfo");
                }

                try
                {
                    int kfr;
                    iVC.get_KeyFrameRate(out kfr);
                    Trace.WriteLine("KeyFrameRate: " + kfr);
                }
                catch(NotImplementedException)
                {
                    Trace.WriteLine("Compressor does not support IAMVideoCompression.get_KeyFrameRate");
                }

                try
                {
                    int pfpkf;
                    iVC.get_PFramesPerKeyFrame(out pfpkf);
                    Trace.WriteLine("PFramesPerKeyFrame: " + pfpkf);
                }
                catch(NotImplementedException)
                {
                    Trace.WriteLine("Compressor does not support IAMVideoCompression.get_PFramesPerKeyFrame");
                }

                try
                {
                    double q;
                    iVC.get_Quality(out q);
                    Trace.WriteLine("Quality: " + q);
                }
                catch(NotImplementedException)
                {
                    Trace.WriteLine("Compressor does not support IAMVideoCompression.get_Quality");
                }

                try
                {
                    ulong ws;
                    iVC.get_WindowSize(out ws);
                    Trace.WriteLine("WindowSize: " + ws);
                }
                catch(NotImplementedException)
                {
                    Trace.WriteLine("Compressor does not support IAMVideoCompression.get_WindowSize");
                }
            }
            
            #endregion IAMVideoCompression

            #region Native Interfaces

            #region IWMCodecLeakyBucket

            IWMCodecLeakyBucket iCLB = (IWMCodecLeakyBucket)compressor;
            uint bsb;

            try
            {
                iCLB.GetBufferSizeBits(out bsb);
                Trace.WriteLine("IWMCodecLeakyBucket.GetBufferSizeBits: " + bsb);
            }
            catch(COMException){}

            #endregion IWMCodecLeakyBucket

            // IWMCodecMetaData - not implemented by encoder in this version

            // IWMCodecOutputTimestamp - not interesting

            // IWMCodePrivateData - used when setting media type
            
            #region IWMCodecProps
            
            IWMCodecProps iCP = (IWMCodecProps)compressor;
            WMT_PROP_DATATYPE dt;
            uint expSize = 64, actSize;
            IntPtr ipData = Marshal.AllocCoTaskMem((int)expSize);

            try
            {
                actSize = expSize;
                iCP.GetCodecProp(cVI.BitmapInfo.Compression, WM9PropList.g_wszWMCPCodecName, out dt, ipData, ref actSize);
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IWMCodecProps.GetCodecProp.WMCPCodecName: {0}, DataType: {1}, ActualSize: {2}",
                    Marshal.PtrToStringUni(ipData).ToString(), dt.ToString(), actSize.ToString()));
            }
            catch(COMException){}

            actSize = expSize;
            iCP.GetCodecProp(cVI.BitmapInfo.Compression, WM9PropList.g_wszWMCPSupportedVBRModes, out dt, ipData, ref actSize);
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IWMCodecProps.GetCodecProp.WMCPSupportedVBRModes: {0}, DataType: {1}, ActualSize: {2}",
                Marshal.ReadInt32(ipData).ToString(), dt.ToString(), actSize.ToString()));

            actSize = expSize;
            iCP.GetCodecProp(cVI.BitmapInfo.Compression, WM9PropList.g_wszWMVCComplexityExLive, out dt, ipData, ref actSize);
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IWMCodecProps.GetCodecProp.WMVCComplexityExLive: {0}, DataType: {1}, ActualSize: {2}",
                Marshal.ReadInt32(ipData).ToString(), dt.ToString(), actSize.ToString()));

            actSize = expSize;
            iCP.GetCodecProp(cVI.BitmapInfo.Compression, WM9PropList.g_wszWMVCComplexityExMax, out dt, ipData, ref actSize);
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IWMCodecProps.GetCodecProp.WMVCComplexityExMax: {0}, DataType: {1}, ActualSize: {2}",
                Marshal.ReadInt32(ipData).ToString(), dt.ToString(), actSize.ToString()));

            actSize = expSize;
            iCP.GetCodecProp(cVI.BitmapInfo.Compression, WM9PropList.g_wszWMVCComplexityExOffline, out dt, ipData, ref actSize);
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IWMCodecProps.GetCodecProp.WMVCComplexityExOffline: {0}, DataType: {1}, ActualSize: {2}",
                Marshal.ReadInt32(ipData).ToString(), dt.ToString(), actSize.ToString()));

            actSize = expSize;
            iCP.GetCodecProp(cVI.BitmapInfo.Compression,WM9PropList. g_wszWMVCDefaultCrisp, out dt, ipData, ref actSize);
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IWMCodecProps.GetCodecProp.WMVCDefaultCrisp: {0}, DataType: {1}, ActualSize: {2}",
                Marshal.ReadInt32(ipData).ToString(), dt.ToString(), actSize.ToString()));

            actSize = expSize;
            iCP.GetCodecProp(cVI.BitmapInfo.Compression, WM9PropList.g_wszWMVCPassesRecommended, out dt, ipData, ref actSize);
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IWMCodecProps.GetCodecProp.WMVCPassesRecommended: {0}, DataType: {1}, ActualSize: {2}",
                Marshal.ReadInt32(ipData).ToString(), dt.ToString(), actSize.ToString()));

            try
            {
                actSize = expSize;
                iCP.GetFormatProp(ref cMT, WM9PropList.g_wszWMVCVBREnabled, out dt, ipData, ref actSize);
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IWMCodecProps.GetCodecProp.WMVCVBREnabled: {0}, DataType: {1}, ActualSize: {2}",
                    Marshal.ReadInt32(ipData).ToString(), dt.ToString(), actSize.ToString()));
            }
            catch(System.NotImplementedException){}

            try
            {
                actSize = expSize;
                iCP.GetFormatProp(ref cMT, WM9PropList.g_wszWMVCVBRQuality , out dt, ipData, ref actSize);
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IWMCodecProps.GetCodecProp.WMVCVBRQuality: {0}, DataType: {1}, ActualSize: {2}",
                    Marshal.ReadInt32(ipData).ToString(), dt.ToString(), actSize.ToString()));
            }
            catch(System.NotImplementedException){}

            Marshal.FreeCoTaskMem(ipData);

            #endregion IWMCodecProps

            #region IWMCodecStrings

            IWMCodecStrings iCS = (IWMCodecStrings)compressor;
            StringBuilder sbData = new StringBuilder(128);
            uint length;

            iCS.GetName(ref cMT, 128, sbData, out length);
            Trace.WriteLine("IWMCodecStrings.GetName: " + sbData.ToString());
            
            iCS.GetDescription(ref cMT, 128, sbData, out length);
            Trace.WriteLine("IWMCodecStrings.GetDescription: " + sbData.ToString());

            #endregion IWMCodecStrings

            // IWMVideoDecoderHurryup - is a decoder interface  :-)

            #endregion Native Interfaces

            #region PropertyBag

            IPropertyBag iPB = (IPropertyBag)compressor;
            object o;

            // VBR only
//            try
//            {
//                iPB.RemoteRead(WM9PropList.g_wszWMVCAvgBitrate, out o, null, 0, null);
//                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IPropertyBag.g_wszWMVCAvgBitrate: {0}", ((int)o).ToString()));
//            }
//            catch(COMException){}

            try
            {
                iPB.RemoteRead(WM9PropList.g_wszWMVCAvgFrameRate, out o, null, 0, null);
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IPropertyBag.g_wszWMVCAvgFrameRate: {0}", ((double)o).ToString()));
            }
            catch(COMException){}

            // VBR only
//            try
//            {
//                iPB.RemoteRead(WM9PropList.g_wszWMVCBAvg, out o, null, 0, null);
//                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IPropertyBag.g_wszWMVCBAvg: {0}", ((int)o).ToString()));
//            }
//            catch(COMException){}

            // VBR only
//            try
//            {
//                iPB.RemoteRead(WM9PropList.g_wszWMVCBMax, out o, null, 0, null);
//                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IPropertyBag.g_wszWMVCBMax: {0}", ((int)o).ToString()));
//            }
//            catch(COMException){}

            try
            {
                iPB.RemoteRead(WM9PropList.g_wszWMVCBufferFullnessInFirstByte, out o, null, 0, null);
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IPropertyBag.g_wszWMVCBufferFullnessInFirstByte: {0}", ((bool)o).ToString()));
            }
            catch(COMException){}

            try
            {
                iPB.RemoteRead(WM9PropList.g_wszWMVCCodedFrames, out o, null, 0, null);
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IPropertyBag.g_wszWMVCCodedFrames: {0}", ((int)o).ToString()));
            }
            catch(COMException){}
            
            // g_wszWMVCComplexityExLive - must use IWMCodecProps.GetCodecProp instead of IPropertyBag
            // g_wszWMVCComplexityExMax - must use IWMCodecProps.GetCodecProp instead of IPropertyBag
            // g_wszWMVCComplexityExOffline - must use IWMCodecProps.GetCodecProp instead of IPropertyBag

            try
            {
                iPB.RemoteRead(WM9PropList.g_wszWMVCDecoderComplexityProfile, out o, null, 0, null);
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IPropertyBag.g_wszWMVCDecoderComplexityProfile: {0}", o.ToString()));
            }
            catch(COMException){}

            try
            {
                iPB.RemoteRead(WM9PropList.g_wszWMVCDefaultCrisp, out o, null, 0, null);
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IPropertyBag.g_wszWMVCDefaultCrisp: {0}", o.ToString()));
            }
            catch(COMException){}

            try
            {
                iPB.RemoteRead(WM9PropList.g_wszWMVCMaxBitrate, out o, null, 0, null);
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IPropertyBag.g_wszWMVCMaxBitrate: {0}", ((int)o).ToString()));
            }
            catch(COMException){}

            try
            {
                iPB.RemoteRead(WM9PropList.g_wszWMVCPassesRecommended, out o, null, 0, null);
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IPropertyBag.g_wszWMVCPassesRecommended: {0}", ((int)o).ToString()));
            }
            catch(COMException){}

            try
            {
                iPB.RemoteRead(WM9PropList.g_wszWMVCPassesUsed, out o, null, 0, null);
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IPropertyBag.g_wszWMVCPassesUsed: {0}", ((int)o).ToString()));
            }
            catch(COMException){}

            try
            {
                iPB.RemoteRead(WM9PropList.g_wszWMVCTotalFrames, out o, null, 0, null);
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IPropertyBag.g_wszWMVCTotalFrames: {0}", ((int)o).ToString()));
            }
            catch(COMException){}

            try
            {
                iPB.RemoteRead(WM9PropList.g_wszWMVCVBREnabled, out o, null, 0, null);
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IPropertyBag.g_wszWMVCVBREnabled: {0}", ((bool)o).ToString()));
            }
            catch(COMException){}

            try
            {
                iPB.RemoteRead(WM9PropList.g_wszWMVCZeroByteFrames, out o, null, 0, null);
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "IPropertyBag.g_wszWMVCZeroByteFrames: {0}", ((int)o).ToString()));
            }
            catch(COMException){}

            #endregion PropertyBag

#endif
        }


        private void GetIAMVideoCompression()
        {
            iVC = OutputPin as IAMVideoCompression;

            if(iVC != null)
            {
                // See if it supports GetInfo
                try
                {
                    int pcbVersion = 0;
                    int pcbDescription = 0;

                    // Make the call to get the lengths of the strings
                    iVC.GetInfo(null, ref pcbVersion, null, ref pcbDescription, 
                        out defaultKFR, out defaultPFPKF, out defaultQuality, out capabilities);

                    StringBuilder version = new StringBuilder(pcbVersion / 2);
                    StringBuilder description = new StringBuilder(pcbDescription / 2);
                    
                    iVC.GetInfo(version, ref pcbVersion, description, ref pcbDescription, 
                        out defaultKFR, out defaultPFPKF, out defaultQuality, out capabilities);

                    this.version = version.ToString();
                    this.description = description.ToString();
                    
                    getInfo = true;
                }
                catch(NotImplementedException){}

                // Windows Media lies about its capabilities, so we test them 1 by 1
                if(capabilities == 0) 
                {
                    // Assume it CanCrunch
                    capabilities |= (int)CompressionCaps.CompressionCaps_CanCrunch;

                    try
                    {
                        int kfr;
                        iVC.get_KeyFrameRate(out kfr);
                        iVC.put_KeyFrameRate(kfr);

                        capabilities |= (int)CompressionCaps.CompressionCaps_CanKeyFrame;
                    }
                    catch(Exception){}

                    try
                    {
                        double quality;
                        iVC.get_Quality(out quality);
                        iVC.put_Quality(quality);

                        capabilities |= (int)CompressionCaps.CompressionCaps_CanQuality;
                    }
                    catch(Exception){}

                    try
                    {
                        int pfpkr;
                        iVC.get_PFramesPerKeyFrame(out pfpkr);
                        iVC.put_PFramesPerKeyFrame(pfpkr);

                        capabilities |= (int)CompressionCaps.CompressionCaps_CanBFrame;
                    }
                    catch(Exception){}

                    try
                    {
                        ulong windowSize;
                        iVC.get_WindowSize(out windowSize);
                        iVC.put_WindowSize(windowSize);

                        capabilities |= (int)CompressionCaps.CompressionCaps_CanWindow;
                    }
                    catch(Exception){}
                }
            }
        }


        #endregion Private
    }

    public class AudioCompressor : Compressor
    {
        #region Statics
        
        private static FilterInfo[] compressors;

        static AudioCompressor()
        {
            ConfigureCustomFormats();

            // Enumerate compressors on the machine and strongly type them
            compressors = EnumerateFilters(CategoryGuid);
        }

        /// <summary>
        /// Get custom compression formats from app.config
        /// </summary>
        private static void ConfigureCustomFormats() {
            string setting;
            if ((setting = ConfigurationManager.AppSettings[AppConfig.MDS_AudioCompressionFormat]) != null) {
                List<MediaTypeIndexPair> formats = new List<MediaTypeIndexPair>(MediaTypeIndices);
                MediaTypeIndexPair newMt = MediaTypeIndexPair.FromString(setting);
                if (newMt != null) {
                    formats.Add(newMt);
                }

                // The next entry in the app.config starts with the postfix 2
                // i.e. - MSR.LST.MDShow.AudioCompressionFormat2
                string key = AppConfig.MDS_AudioCompressionFormat;
                int postfix = 2;

                while ((setting = ConfigurationManager.AppSettings[key + postfix]) != null) {
                    newMt = MediaTypeIndexPair.FromString(setting);
                    if (newMt != null) {
                        formats.Add(newMt);
                    }
                    postfix++; // Move to the next entry
                }

                MediaTypeIndices = formats.ToArray();
            }

        }

        /// <summary>
        /// Audio Compressors, CLSID_AudioCompressorCategory 
        /// </summary>
        public static Guid CategoryGuid = new Guid("33d9a761-90c8-11d0-bd43-00a0c911ce86");

        /// <summary>
        /// Only a limited number of _AMMediaTypes actually work and have
        /// low latency, in the 'Windows Media Audio V2' encoder.  
        /// 
        /// I tried the 'WMAudio Encoder DMO' and the 'WM Speech Encoder DMO'
        /// I couldn't get anything but the default value (4 Kb) from the Speech
        /// encoder, and couldn't find any reasonabl low latency values in the
        /// WMAudio encoder.  So we are sticking with the old encoder.
        /// </summary>
        public const string DefaultName = "Windows Media Audio V2";
        
        /// <summary>
        /// Only a limited number of _AMMediaTypes actually work and have
        /// low latency, in the 'Windows Media Audio V2' encoder.  
        /// 14 is 1 channel, 22 KHz, 20 Kbps
        /// 20 is 2 channel, 22 KHz, 20 Kbps
        /// 38 is 2 channel, 44.1 KHz, 64 Kbps
        /// </summary>
        public static MediaTypeIndexPair[] MediaTypeIndices = new MediaTypeIndexPair[] { 
            new MediaTypeIndexPair(14, "22 KHz, 20 Kbps, mono"),
            new MediaTypeIndexPair(22, "32 KHz, 32 Kbps, mono"),
            new MediaTypeIndexPair(20, "22 KHz, 20 Kbps, stereo"),
            new MediaTypeIndexPair(17, "22 KHz, 32 Kbps, stereo"),
            new MediaTypeIndexPair(25, "32 KHz, 64 Kbps, stereo"),
            new MediaTypeIndexPair(38, "44.1 KHz, 64 Kbps, stereo")
        };

        public static readonly int DEFAULT_COMPRESSION_INDEX = 20;
        public static readonly int DEFAULT_DV_COMPRESSION_INDEX = 25;

        public static FilterInfo DefaultFilterInfo()
        {
            foreach(FilterInfo fi in AudioCompressor.Compressors)
            {
                if(fi.Name == DefaultName)
                {
                    return fi;
                }
            }

            throw new ApplicationException(string.Format(CultureInfo.CurrentCulture, 
                Strings.UnableToFindDefaultCompressor, DefaultName));
        }


        /// <summary>
        /// Compressors is a static collection, because they don't tend to change like Pnp Audio / Audio devices
        /// </summary>
        public static FilterInfo[] Compressors
        {
            get
            {
                return compressors;
            }
        }

        /// <summary>
        /// Audio compressors are "static" devices.  That is to say, they do not come and go on the
        /// machine very often (like a USB webcam might).  They are also "multi-use", meaning you
        /// may have multiple instances of a compressor running, whereas you only have 1 instance
        /// of a webcam.  Therefore we hand out new instances each time.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static AudioCompressor CreateFilter(string name)
        {
            foreach(FilterInfo fi in compressors)
            {
                if(fi.Name == name)
                {
                    return new AudioCompressor(fi);
                }
            }

            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, 
                Strings.UnableToFindAudioCompressor, name));
        }


        #endregion Statics

        #region Members

        /// <summary>
        /// Pre-connect _AMMediaTypes (what the filter can do before it is connected)
        /// </summary>
        private _AMMediaType[] pcMTs = null;

        #endregion Members

        #region Constructor

        public AudioCompressor(FilterInfo fi) : base(fi)
        {
            if(fi.Category != AudioCompressor.CategoryGuid)
            {
                Debug.Assert(false);
                throw new ArgumentOutOfRangeException("fi.Category", fi.Category, Strings.UnexpectedFilterCategory);
            }
        }


        public override void Dispose()
        {
            MediaType.Free(pcMTs);
            pcMTs = null;

            base.Dispose ();
        }

        #endregion Constructor

        public override void AddedToGraph(FilgraphManager fgm)
        {
            base.AddedToGraph (fgm);

            // Summary of a comment from MSDN
            // The recommended order of operations for all of the audio codecs 
            // is to set the output type before you set the input type.
            pcMTs = Pin.GetMediaTypes(OutputPin);
        }

        public _AMMediaType[] PreConnectMediaTypes
        {
            get{return pcMTs;}
        }

        public class MediaTypeIndexPair {
            public MediaTypeIndexPair(int index, string desc) {
                Index = index;
                Description = desc;
            }

            /// <summary>
            /// Convert from app.config representation, eg. "26:My custom compression format"
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public static MediaTypeIndexPair FromString(string s) {
                string[] separator = {":"};
                string[] parts = s.Split(separator,StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2) {
                    return null;
                }
                int ind = 0;
                if (int.TryParse(parts[0], out ind)) {
                    if (ind > 0) {
                        return new MediaTypeIndexPair(ind, parts[1]);
                    }
                }
                return null;
            }

            public int Index;
            public string Description;
            public override string ToString() {
                return Description;
            }
        }
    }


    public abstract class Renderer : Filter
    {
        public Renderer(FilterInfo fi) : base(fi){}
    }

    public class AudioRenderer : Renderer
    {
        #region Static

        /// <summary>
        /// Audio Renderers CLSID_AudioRendererCategory 
        /// </summary>
        public static readonly Guid CategoryGuid = new Guid("e0f158e1-cb04-11d0-bd4e-00a0c911ce86");

        /// <summary>
        /// Return the current list of audio renderer on the machine.  The list is 
        /// dynamically generated for each call, as PnP devices may come and go.
        /// </summary>
        public static FilterInfo[] Renderers()
        {
            FilterInfo[] renderers = EnumerateFilters(CategoryGuid);

            // When rendering audio, the device itself "Plantronics Headset"
            // Can only be used once.  However, the "DirectSound: Plantronics Headset"
            // can be used for multiple streams (DShow must do something magical
            // behind the scenes).  So we strip out the "intuitive" choices and leave
            // the DirectSound choices, but we modify the name to be intuitive.
            ArrayList alRenderers = new ArrayList();

            // Can't use foreach here because assignment won't compile
            for(int i = 0; i < renderers.Length; i++)
            {
                FilterInfo fi = renderers[i];

                if(fi.Name.IndexOf("DirectSound") > -1)
                {
                    if(fi.Name == DefaultName)
                    {
                        // Change "Default DirectSound Device" to...
                        fi.DisplayName = "Default System Device";
                    }
                    else
                    {
                        // Remove the leading DirectSound: to give the device an intuitive name
                        fi.DisplayName = fi.Name.Remove(0, "DirectSound: ".Length);
                    }

                    alRenderers.Add(fi);
                }
            }

            return (FilterInfo[])alRenderers.ToArray(typeof(FilterInfo));
        }


        public const string DefaultName = "Default DirectSound Device";

        public static FilterInfo DefaultFilterInfo()
        {
            foreach(FilterInfo fi in AudioRenderer.Renderers())
            {
                if(fi.Name == DefaultName)
                {
                    return fi;
                }
            }

            throw new ApplicationException(string.Format(CultureInfo.CurrentCulture, 
                Strings.UnableToFindDefaultAudioRenderer, DefaultName));
        }

        
        #endregion Static

        public AudioRenderer(FilterInfo fi) : base(fi)
        {
            if(fi.Category != AudioRenderer.CategoryGuid)
            {
                Debug.Assert(false);
                throw new ArgumentOutOfRangeException("fi.Category", fi.Category, Strings.UnexpectedFilterCategory);
            }
        }
    }
    public class NetworkRenderer : Renderer
    {
        public NetworkRenderer(FilterInfo fi) : base(fi){}

        protected override IBaseFilter InstantiateFilter()
        {
            return RtpRendererClass.CreateInstance();
        }
    }


    /// <summary>
    /// An attempt at summarizing the most important information about a video compressor media type
    /// It contains info from the OutputPin, VIDEOINFOHEADER and IAMVideoCompression 
    /// </summary>
    [Serializable]
    public class VideoCompressorQualityInfo
    {
        public static readonly Guid MediaSubTypeDefault = MediaType.SubType.WMMEDIASUBTYPE_WMV3;
        
        // The key-frame rate is the number of frames per key frame. For example, if the rate is
        // 15, then a key frame occurs every 15 frames. This setting does not work well for WMV9.
        // WMV9 prefers to receive this information as the maximum time, in milliseconds, between 
        // key frames in the codec output
        public const int    KeyFrameRateDefault = -1;
        public const double QualityDefault = -1.0;

        public Guid     MediaSubType;
        public uint     BitRate;
        public int      KeyFrameRate;
        public double   Quality;


        public VideoCompressorQualityInfo(VideoCompressorQualityInfo vcqi) : 
            this(vcqi.MediaSubType, vcqi.BitRate, vcqi.KeyFrameRate, vcqi.Quality){}
            

        public VideoCompressorQualityInfo(Guid mediaSubType, uint bitRate) :
            this(mediaSubType, bitRate, KeyFrameRateDefault, QualityDefault){}

        public VideoCompressorQualityInfo(Guid mediaSubType, uint bitRate, int keyFrameRate) :
            this(mediaSubType, bitRate, keyFrameRate, QualityDefault){}


        public VideoCompressorQualityInfo(Guid mediaSubType, uint bitRate, int keyFrameRate, double quality)
        {
            MediaSubType = mediaSubType;
            BitRate = bitRate;
            KeyFrameRate = keyFrameRate;
            Quality = quality;
        }
    }

    public class PhysicalConnector
    {
        private IPin pin;
        private tagPhysicalConnectorType pct;
        private int index;

        public PhysicalConnector(IPin pin, tagPhysicalConnectorType pct, int index)
        {
            this.pin = pin;
            this.pct = pct;
            this.index = index;
        }

        public IPin Pin
        {
            get{return pin;}
        }

        public tagPhysicalConnectorType PhysicalConnectorType
        {
            get { return pct; }
        }

        public int Index
        {
            get { return index; }
        }


        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "Name: {0}, Connection type: {1}, Index: {2}", 
                MSR.LST.MDShow.Pin.Name(Pin), pct.ToString(), index);
        }
    }
}
