using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using Microsoft.Win32;

using MSR.LST.MDShow.Filters;
using MSR.LST.Net.Rtp;
using System.Collections.Generic;
using System.Configuration;


namespace MSR.LST.MDShow
{
    public sealed class FilterGraph
    {
        public enum State
        {
            Running,
            Stopped
        }
  

        /// <summary>
        /// This class only provides static methods and can't be inherited
        /// or instantiated
        /// </summary>
        private FilterGraph(){}


        /// <summary>
        /// Removes all filters from a graph
        /// 
        /// Important Note:
        /// If this method goes into an infinite loop, it is most likely because you are trying to
        /// remove a filter from a different thread than the one that added the filter, and your
        /// app was launched with the STAThreaded attribute on the Main method.
        /// 
        /// Either use MTAThreaded attribute, or remove filter from the same thread it was added.
        /// </summary>
        public static void RemoveAllFilters( FilgraphManagerClass fgm)
        {
            IFilterGraph iFG = (IFilterGraph)fgm;
            IEnumFilters iEnum;
            iFG.EnumFilters(out iEnum);

            uint fetched;
            IBaseFilter iBF;
            iEnum.Next(1, out iBF, out fetched);

            while(fetched == 1)
            {
                // Remove filter from graph
                iFG.RemoveFilter(iBF);

                // Because the state of the enumerator has changed (item was removed from collection)
                iEnum.Reset();
                iEnum.Next(1, out iBF, out fetched);
            }
        }

        public static ArrayList FiltersInGraph(IFilterGraph iFG)
        {
            ArrayList ret = new ArrayList();

            IEnumFilters iEnum;
            iFG.EnumFilters(out iEnum);

            uint fetched;
            IBaseFilter iBF;
            iEnum.Next(1, out iBF, out fetched);

            while(fetched == 1)
            {
                ret.Add(iBF);
                iEnum.Next(1, out iBF, out fetched);
            }

            return ret;
        }

        public static string Debug(IFilterGraph iFG)
        {
            string ret = "\nFilters in graph\n";

            foreach(IBaseFilter iBF in FiltersInGraph(iFG))
            {
                ret += string.Format(CultureInfo.CurrentCulture, "     {0}\n", Filter.Name(iBF));
            }

            return ret;
        }

        
        #region ROT

        [DllImportAttribute("ole32.dll")]
        private static extern int CreateItemMoniker(
            [MarshalAs(UnmanagedType.LPWStr)] string delim, 
            [MarshalAs(UnmanagedType.LPWStr)] string name,
            out IMoniker ppmk);
        
        [DllImport("ole32.dll")]
        private static extern int GetRunningObjectTable(
            int reserved, out IRunningObjectTable ROT);

        public static UInt32 AddToRot(IGraphBuilder graph)
        {
            int hr;

            IRunningObjectTable rot;
            hr = GetRunningObjectTable(0, out rot);
            Marshal.ThrowExceptionForHR(hr);

            IMoniker moniker;
            hr = CreateItemMoniker("!", string.Format(CultureInfo.InvariantCulture, "FilterGraph {0:x8} pid {1:x8}", 
                new Random().Next(), System.Diagnostics.Process.GetCurrentProcess().Id), out moniker);
            Marshal.ThrowExceptionForHR(hr);

            UInt32 register;
            rot.Register(1, graph, moniker, out register);

            return register;
        }

        
        public static void RemoveFromRot(UInt32 register)
        {
            IRunningObjectTable rot;
            GetRunningObjectTable(0, out rot);

            rot.Revoke(register);
        }

        
        #endregion ROT
    }

    
    public abstract class CaptureGraph
    {
        #region Members

        protected FilgraphManagerClass fgm;
        protected IGraphBuilder iGB;
        protected IFilterGraph iFG;

        private uint rotID;

        protected SourceFilter source;
        protected Compressor compressor;
        protected Renderer renderer;
        protected byte rtpRendererFlags;

        #endregion Members

        #region Constructor, Dispose

        protected CaptureGraph(FilterInfo fiSource)
        {
            try
            {
                // Fgm initialization
                fgm = new FilgraphManagerClass();
                iFG = (IFilterGraph)fgm;
                iGB = (IGraphBuilder)fgm;
                rotID = FilterGraph.AddToRot(iGB);
        
                // Create source filter and initialize it
                source = (SourceFilter)Filter.CreateFilter(fiSource);
                iGB.AddFilter(source.BaseFilter, source.FriendlyName);
                source.AddedToGraph(fgm);

                // Pass flags to the RtpRenderer filter from the config file.
                this.rtpRendererFlags = 0;
                string setting = ConfigurationManager.AppSettings[AppConfig.MDS_RtpRendererFlags];
                if (!String.IsNullOrEmpty(setting)) {
                    if (!byte.TryParse(setting,out rtpRendererFlags)) {
                        rtpRendererFlags = 0;
                    }
                }
            }
            catch(Exception)
            {
                Cleanup();
                throw;
            }
        }


        public virtual void Dispose()
        {
            Cleanup();
        }


        #endregion Constructor, Dispose

        #region Public

        public FilgraphManagerClass FilgraphManager
        {
            get{return fgm;}
        }

        public IFilterGraph IFilterGraph
        {
            get { return iFG; }
        }

        public IGraphBuilder IGraphBuilder
        {
            get { return iGB; }
        }


        public virtual SourceFilter Source
        {
            get{return (SourceFilter)source;}
        }

        public virtual Compressor Compressor
        {
            get{return compressor;}
        }

        public virtual Renderer AudioRenderer {
            get { return renderer; }
        }

        public virtual Renderer VideoRenderer {
            get { return renderer; }
        }

        public virtual void AddCompressor(FilterInfo fiCompressor)
        {
            RemoveCompressor();

            compressor = (Compressor)Filter.CreateFilter(fiCompressor);
            iGB.AddFilter(compressor.BaseFilter, compressor.FriendlyName);
            compressor.AddedToGraph(fgm); // Chooses input pin

            try
            {
                iGB.Connect(source.OutputPin, compressor.InputPin);
            }
            catch(COMException)
            {
                RemoveCompressor();
                throw;
            }
        }

        public virtual void AddAudioRenderer(FilterInfo fiRenderer)
        {
            RemoveRenderer();

            renderer = (Renderer)Filter.CreateFilter(fiRenderer);
            iGB.AddFilter(renderer.BaseFilter, renderer.FriendlyName);
            renderer.AddedToGraph(fgm); // Chooses input pin

            IPin pin = compressor == null ? source.OutputPin : compressor.OutputPin;

            try
            {
                iGB.Connect(pin, renderer.InputPin);
            }
            catch(COMException)
            {
                RemoveRenderer();
                throw;
            }
        }

        /// <summary>
        /// Removes everything downstream of the source
        /// </summary>
        public void RemoveCompressor()
        {
            RemoveFiltersDownstreamFrom(source);
        }


        public virtual void RemoveCompressor(PayloadType payload) {
            RemoveCompressor();
        }

        
        public virtual void RemoveRenderer()
        {
            Filter start = compressor != null ? (Filter)compressor : source;
            RemoveFiltersDownstreamFrom(start);
        }

        public virtual void RemoveRenderer(PayloadType payload) {
            RemoveRenderer();
        }

        public virtual void RemoveFiltersDownstreamFromSource(PayloadType payload) {
            RemoveFiltersDownstreamFrom(source);
        }

        /// <summary>
        /// Removes all filters from the graph, starting at the end and working
        /// back to but not including "start"
        /// </summary>
        public void RemoveFiltersDownstreamFrom(Filter start)
        {
            if(start == null)
            {
                string msg = Strings.NullStartError;
                
                Debug.Fail(msg);
                throw new ArgumentNullException(Strings.Start, msg);
            }

            Stop();

            List<IBaseFilter> toRemove = EnumerateDownstreamFromFilter(start.DownstreamBaseFilter);

            foreach(IBaseFilter iBF in toRemove)
            {
                iFG.RemoveFilter(iBF);
                if ((renderer != null) && (iBF == renderer.BaseFilter)) {
                    DisposeRenderer();
                }
                if ((Compressor != null) && (iBF == Compressor.BaseFilter)) {
                    DisposeCompressor();
                }
            }

        }

        /// <summary>
        /// Find all the filters connected downstream from the specified pin,
        /// following all branches.
        /// </summary>
        /// <param name="pin"></param>
        /// <returns></returns>
        private List<IBaseFilter> EnumerateDownstreamFromPin(IPin pin) {
            //Find the input pin that the target pin is connected to.
            IPin connectedToInput;
            try {
                pin.ConnectedTo(out connectedToInput);
            }
            catch (COMException) {
                //not connected
                return new List<IBaseFilter>();
            }

            // Map that pin to the next filter.
            _PinInfo pInfo;
            connectedToInput.QueryPinInfo(out pInfo);
            IBaseFilter connectedFilter = pInfo.pFilter;

            //Add the filter to the list.
            List<IBaseFilter> returnList = new List<IBaseFilter>();
            returnList.Add(connectedFilter);

            //Enumerate output pins of the filter
            ArrayList outPins = Filter.GetPins(Filter.GetPins(connectedFilter), _PinDirection.PINDIR_OUTPUT);

            foreach (IPin p in outPins) {
                //recurse over each pin
                returnList.AddRange(EnumerateDownstreamFromPin(p));
            }

            return returnList;
        }

        /// <summary>
        /// Find all the filters connected downstream from the specified filter
        /// </summary>
        /// <param name="startFilter"></param>
        /// <returns></returns>
        private List<IBaseFilter> EnumerateDownstreamFromFilter(IBaseFilter startFilter) { 
            List<IBaseFilter> returnList = new List<IBaseFilter>();

            //Enumerate output pins of the filter
            ArrayList outPins = Filter.GetPins(Filter.GetPins(startFilter), _PinDirection.PINDIR_OUTPUT);

            foreach (IPin p in outPins) {
                //recurse over each pin
                returnList.AddRange(EnumerateDownstreamFromPin(p));
            }

            return returnList;          
        }
        
        /// <summary>
        /// Remove filters connected downstream from the specified output pin
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="outPin"></param>
        public void RemoveFiltersDownstreamFromPin(IPin outPin) {
            List<IBaseFilter> toRemove = EnumerateDownstreamFromPin(outPin);
            //Remove filters.
            foreach (IBaseFilter ibf in toRemove) {
                this.iGB.RemoveFilter(ibf);
            }

        }

        /// <summary>
        /// This filter has an input pin matching one of the pins in the list.
        /// </summary>
        /// <param name="thisFilter"></param>
        /// <param name="inputPins"></param>
        /// <returns></returns>
        private bool FilterHasInputPin(IBaseFilter thisFilter, List<IPin> inputPins) {
            if (inputPins.Count == 0) return false;

            IEnumPins enumPins;
            thisFilter.EnumPins(out enumPins);

            IPin p;
            uint fetched;
            enumPins.Next(1, out p, out fetched);
            while (fetched == 1) {
                _PinDirection pinDir;
                p.QueryDirection(out pinDir);
                if (pinDir == _PinDirection.PINDIR_INPUT) {
                    if (inputPins.Contains(p)) {
                        return true;
                    }
                }
                enumPins.Next(1, out p, out fetched);
            }
            return false;
        }

        /// <summary>
        /// This filter has the input pin specified.
        /// </summary>
        /// <param name="thisFilter"></param>
        /// <param name="inputPin"></param>
        /// <returns></returns>
        private bool FilterHasInputPin(IBaseFilter thisFilter, IPin inputPin) {
            if (inputPin == null) return false;

            IEnumPins enumPins;
            thisFilter.EnumPins(out enumPins);

            IPin p;
            uint fetched;
            enumPins.Next(1, out p, out fetched);
            while (fetched == 1) {
                _PinDirection pinDir;
                p.QueryDirection(out pinDir);
                if (pinDir == _PinDirection.PINDIR_INPUT) {
                    if (p.Equals(inputPin)) {
                        return true;
                    }
                }
                enumPins.Next(1, out p, out fetched);
            }
            return false;
        }

        public virtual void RenderLocal()
        {
            if(renderer != null)
            {
                string msg = Strings.RenderLocalError;
                System.Diagnostics.Debug.Fail(msg);
                throw new InvalidOperationException(msg);
            }

            IPin pin = compressor != null ? (IPin)compressor.OutputPin : source.OutputPin;
            iGB.Render(pin);
        }

        public virtual void RenderNetwork(RtpSender rtpSender, PayloadType payload) {
            RenderNetwork(rtpSender);
        }

        public void RenderNetwork(RtpSender rtpSender)
        {
            if(rtpSender == null)
            {
                string msg = Strings.NullRtpSenderError;
                
                Debug.Fail(msg);
                throw new ArgumentNullException(Strings.RtpSender, msg);
            }

            renderer = (Renderer)Filter.NetworkRenderer();
            ((IRtpRenderer)renderer.BaseFilter).Initialize2(rtpSender,this.rtpRendererFlags);
            iGB.AddFilter(renderer.BaseFilter, renderer.FriendlyName);
            renderer.AddedToGraph(fgm);

            // Connect last pin (device or compressor) to the network renderer
            iGB.Connect(compressor != null ? compressor.OutputPin : source.OutputPin,
                renderer.InputPin);
        }
    

        /// <summary>
        /// Start sending the data stream.
        /// </summary>
        public void Run()
        {
            if (fgm != null)
            {
                fgm.Run();
            }
        }

        /// <summary>
        /// Stop sending the data stream.
        /// </summary>
        public virtual void Stop()
        {
            if (fgm != null)
            {
                fgm.Stop();
            }
        }

    
        #endregion Public

        #region Private

        private void Cleanup()
        {
            if(fgm != null)
            {
                fgm.Stop();
                FilterGraph.RemoveAllFilters(fgm);
                FilterGraph.RemoveFromRot(rotID);
            
                iGB = null;
                iFG = null;
                fgm = null;
            }

            DisposeSource();
            DisposeCompressor();
            DisposeRenderer();
        }

        private void DisposeSource()
        {
            if(source != null)
            {
                source.Dispose();
                source = null;
            }
        }

        private void DisposeCompressor()
        {
            if(compressor != null)
            {
                compressor.Dispose();
                compressor = null;
            }
        }

        private void DisposeRenderer()
        {
            if(renderer != null)
            {
                renderer.Dispose();
                renderer = null;
            }
        }
                

        #endregion Private
    }

    public class VideoCaptureGraph : CaptureGraph, IVideoCaptureGraph
    {
        #region Constructor

        public VideoCaptureGraph(FilterInfo fiSource) : base(fiSource){}

        
        #endregion Constructor

        #region Public

        public VideoSource VideoSource
        {
            get{return (VideoSource)Source;}
        }

        public VideoCompressor VideoCompressor
        {
            get{return (VideoCompressor)Compressor;}
        }

        
        #endregion Public
    }

    public class AudioCaptureGraph : CaptureGraph, IAudioCaptureGraph
    {
        #region Constructor

        public AudioCaptureGraph(FilterInfo fiSource) : base(fiSource){}


        #endregion Constructor

        #region Public

        public AudioSource AudioSource
        {
            get{return (AudioSource)Source;}
        }

        public AudioCompressor AudioCompressor
        {
            get{return (AudioCompressor)Compressor;}
        }


        #endregion Public
    }

    /// <summary>
    /// Represents a graph for DV audio, DV video or both.  In the latter case the graph will be shared
    /// by audio and video capabilities.
    /// </summary>
    public class DVCaptureGraph : CaptureGraph, IVideoCaptureGraph, IAudioCaptureGraph {
        #region Static
        /// <summary>
        /// These graphs can be shared by audio and video capabilites.  We only create one instance per source filter.
        /// Application code should normally call this instead of using the constructor.  A side effect of calling
        /// on a running graph is that the graph will be stopped.  It is the caller's responsibility to call Run if needed.
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        public static DVCaptureGraph GetInstance(FilterInfo fi) {
            if (!instances.ContainsKey(fi)) {
                instances.Add(fi, new DVCaptureGraph(fi));
            }
            else {
                instances[fi].Stop();
                instances[fi].RefCount++;
            }
            return instances[fi];
        }

        private static Dictionary<FilterInfo, DVCaptureGraph> instances = new Dictionary<FilterInfo, DVCaptureGraph>();

        #endregion Static

        #region Members

        private Compressor audioCompressor;
        private Renderer audioRenderer;
        private Compressor videoCompressor;
        private Renderer videoRenderer;
        private FilterInfo myFilterInfo;

        private IPin splitterAudioOut;
        private IPin splitterVideoOut;
        private IBaseFilter dvSplitter;

        /// <summary>
        /// Indicates a graph with network renderers.  These need slightly different handling.
        /// </summary>
        private bool networkContext = false;

        #endregion Members

        #region Constructor

        public DVCaptureGraph(FilterInfo fiSource) : base(fiSource) {
            myFilterInfo = fiSource;
            RefCount = 1;
        }

        /// <summary>
        /// If Dispose is called on a shared graph, it will not really be disposed.  If the Paused flag was previously set
        /// then the Run method will be called.
        /// </summary>
        public override void Dispose() {
            RefCount--;

            if (RefCount == 0) {
                instances.Remove(myFilterInfo);
                base.Dispose();
            }
            else if (Paused) {
                if (!networkContext) {
                    fgm.Run();
                }
                Paused = false;
            }
        }

        #endregion Constructor

        #region Public
        /// <summary>
        /// Count of capabilities that have references to this instance.
        /// </summary>
        public int RefCount = 0;

        /// <summary>
        /// Flag to restart if the graph is shared by multiple capabilities.
        /// </summary>
        public bool Paused = false;

        public override Renderer AudioRenderer {
            get {
                return audioRenderer;
            }
        }

        public override Renderer VideoRenderer {
            get {
                return videoRenderer;
            }
        }

        public DVSource DVSource {
            get {
                return (DVSource)source;
            }
        }

        /// <summary>
        /// Finish building the Graph for local DV audio playback.  If there is already an audio compressor, just 
        /// add the renderer, and connect.  Otherwise add and connect the DV Splitter and the renderer.
        /// </summary>
        /// <param name="fiRenderer"></param>
        public override void AddAudioRenderer(FilterInfo fiRenderer) {
            RemoveAndDispose(audioRenderer);

            //Add selected renderer
            audioRenderer = (Renderer)Filter.CreateFilter(fiRenderer);
            iGB.AddFilter(audioRenderer.BaseFilter, audioRenderer.FriendlyName);
            audioRenderer.AddedToGraph(fgm);

            if (!AddDVSplitter()) {
                RemoveAndDispose(audioRenderer);
                throw new ApplicationException("Failed to add DV Splitter Filter");
            }

            IPin pin = audioCompressor == null ? splitterAudioOut : audioCompressor.OutputPin;

            //Connect
            try {
                iGB.Connect(pin, audioRenderer.InputPin);
            }
            catch (COMException) {
                RemoveAndDispose(audioRenderer);
                throw;
            }

            renderer = audioRenderer;
        }

        /// <summary>
        /// The compressor could be audio or video.  If there is already an existing compressor of the
        /// given type, remove it and any downstream filters before adding the given compressor.
        /// The graph my not yet contain a DV Splitter, in which case, add the splitter first.
        /// </summary>
        /// <param name="fiCompressor"></param>
        public override void AddCompressor(FilterInfo fiCompressor) {

            Stop();

            if (!AddDVSplitter()) {
                throw new ApplicationException("Failed to add DV Splitter Filter");
            }

            if (fiCompressor.Category.Equals(Filter.CLSID_AudioCompressorCategory)) {
                if (audioCompressor != null) {
                    RemoveFiltersDownstreamFromPin(splitterAudioOut);
                    RemoveAndDispose(audioCompressor);
                    audioCompressor = null;
                }
                audioCompressor = (Compressor)Filter.CreateFilter(fiCompressor);
                iGB.AddFilter(audioCompressor.BaseFilter, audioCompressor.FriendlyName);
                audioCompressor.AddedToGraph(fgm);
                compressor = audioCompressor;

                _AMMediaType[] mts = Pin.GetMediaTypes(splitterAudioOut);
                //Returns one DVInfo mt.  
                foreach (_AMMediaType mt in mts) {
                    Debug.WriteLine(MediaType.Dump(mt));
                }

                try {
                    iGB.Connect(this.splitterAudioOut, audioCompressor.InputPin);
                }
                catch (COMException) {
                    RemoveAndDispose(audioCompressor);
                    audioCompressor = null;
                    throw;
                }
            }
            else if (fiCompressor.Category.Equals(Filter.CLSID_VideoCompressorCategory)) {
                if (videoCompressor != null) {
                    RemoveFiltersDownstreamFromPin(splitterVideoOut);
                    RemoveAndDispose(videoCompressor);
                    videoCompressor = null;
                }
                videoCompressor = (Compressor)Filter.CreateFilter(fiCompressor);
                iGB.AddFilter(videoCompressor.BaseFilter, videoCompressor.FriendlyName);
                videoCompressor.AddedToGraph(fgm);
                compressor = videoCompressor;            
                try {
                    iGB.Connect(this.splitterVideoOut, videoCompressor.InputPin);
                }
                catch (COMException) {
                    RemoveAndDispose(videoCompressor);
                    videoCompressor = null;
                    throw;
                }            
            }
        }


        /// <summary>
        /// Most likely this will remain unused.
        /// </summary>
        public override void RemoveRenderer() {
            if (dvSplitter != null) {
                RemoveFiltersDownstreamFrom(source);
                dvSplitter = null;
                splitterAudioOut = splitterVideoOut = null;
            }
        }

        /// <summary>
        /// Remove the renderer for the branch of the graph corresponding to the specified payload
        /// </summary>
        /// <param name="payload"></param>
        public override void RemoveRenderer(PayloadType payload) {
            if (dvSplitter != null) {
                if (payload == PayloadType.dynamicAudio) {
                    if (audioCompressor == null) {
                        RemoveFiltersDownstreamFromPin(splitterAudioOut);
                    }
                    else {
                        RemoveFiltersDownstreamFrom(audioCompressor);
                    }
                }
                else if (payload == PayloadType.dynamicVideo) {
                    if (videoCompressor == null) {
                        RemoveFiltersDownstreamFromPin(splitterVideoOut);
                    }
                    else {
                        RemoveFiltersDownstreamFrom(videoCompressor);
                    }
                }
            }
        }

        /// <summary>
        /// Remove everything downstream from the DVSplitter for the specified payload type
        /// </summary>
        /// <param name="payload"></param>
        public override void RemoveCompressor(PayloadType payload) {
            RemoveFiltersDownstreamFromSplitter(payload);
        }

        /// <summary>
        /// Remove everything downstream from the DVSplitter for the specified payload type
        /// </summary>
        /// <param name="payload"></param>
        public override void RemoveFiltersDownstreamFromSource(PayloadType payload) {
            RemoveFiltersDownstreamFromSplitter(payload);
        }

        /// <summary>
        /// Stop the graph, and if it is shared by more than one capability, set a paused flag so that
        /// we can restart if needed.
        /// </summary>
        public override void Stop() {
            base.Stop();
            if (this.RefCount > 1) {
                //We are shared by more than one capability, so flag it to restart later.
                Paused = true;
            }
        }

        /// <summary>
        /// Render local video
        /// </summary>
        public override void RenderLocal() {
            if (!AddDVSplitter()) {
                throw new ApplicationException("Failed to add DV Splitter");
            }

            IPin pin = videoCompressor != null ? (IPin)videoCompressor.OutputPin : splitterVideoOut;
            iGB.Render(pin);
        }

        public DVSource VideoSource {
            get { return (DVSource)source; }
        }
        
        public AudioCompressor AudioCompressor {
            get { return (AudioCompressor)audioCompressor; }
        }

        public VideoCompressor VideoCompressor {
            get { return (VideoCompressor)videoCompressor; }
        }

        /// <summary>
        /// Connect up a branch of the graph for network sending.  Source filter should already be in the graph.
        /// DVSplitter and compressor may also already be connected, but for uncompressed cases, the Splitter
        /// may not be there yet.
        /// </summary>
        /// <param name="rtpSender"></param>
        /// <param name="payload"></param>
        public override void RenderNetwork(RtpSender rtpSender, PayloadType payload) {
            if (rtpSender == null) {
                string msg = Strings.NullRtpSenderError;
                Debug.Fail(msg);
                throw new ArgumentNullException(Strings.RtpSender, msg);
            }

            //Splitter may not yet be added in network scenarios without compression
            if (!this.AddDVSplitter()) {
                throw new ApplicationException("Failed to add DV Splitter");
            }

            networkContext = true;

            if (payload == PayloadType.dynamicVideo) { 
                videoRenderer = (Renderer)Filter.NetworkRenderer();
                ((IRtpRenderer)videoRenderer.BaseFilter).Initialize2(rtpSender, this.rtpRendererFlags);
                iGB.AddFilter(videoRenderer.BaseFilter, videoRenderer.FriendlyName);
                videoRenderer.AddedToGraph(fgm);
                iGB.Connect(videoCompressor != null ? videoCompressor.OutputPin : splitterVideoOut,
                    videoRenderer.InputPin);            
            }
            else if (payload == PayloadType.dynamicAudio) { 
                audioRenderer = (Renderer)Filter.NetworkRenderer();
                ((IRtpRenderer)audioRenderer.BaseFilter).Initialize2(rtpSender, this.rtpRendererFlags);
                iGB.AddFilter(audioRenderer.BaseFilter, audioRenderer.FriendlyName);
                audioRenderer.AddedToGraph(fgm);
                iGB.Connect(audioCompressor != null ? audioCompressor.OutputPin : splitterAudioOut,
                    audioRenderer.InputPin);
            }
        }

        public void GetVideoMediaType(out _AMMediaType mt, out object formatBlock) {
            _AMMediaType[] mts;
            object[] formats;
            //GetMediaType does not work with this pin because it cannot be cast to IAMStreamConfig.
            if (!AddDVSplitter()) {
                throw new ApplicationException("Failed to add DV Splitter");
            }
            Pin.GetMediaTypes(this.splitterVideoOut, out mts, out formats);
            //Is it safe to assume only one video media type?
            if (mts.Length > 0) {
                mt = mts[0];
                formatBlock = formats[0];
            }
            else {
                mt = new _AMMediaType();
                formatBlock = null;
            }
        }


        public void GetAudioMediaType(out _AMMediaType mt, out object formatBlock) {
            _AMMediaType[] mts;
            object[] formats;
            //GetMediaType does not work with this pin because it cannot be cast to IAMStreamConfig.
            if (!AddDVSplitter()) {
                throw new ApplicationException("Failed to add DV Splitter");
            }
            Pin.GetMediaTypes(this.splitterAudioOut, out mts, out formats);
            //Is it safe to assume one audio media type?
            if (mts.Length > 0) {
                mt = mts[0];
                formatBlock = formats[0];
            }
            else {
                mt = new _AMMediaType();
                formatBlock = null;
            }
        }

        public void GetAudioMediaTypes(out _AMMediaType[] mts, out object[] fbs) {
            if (!AddDVSplitter()) {
                throw new ApplicationException("Failed to add DV Splitter");
            }
            Pin.GetMediaTypes(this.splitterAudioOut, out mts, out fbs);
        }

        #endregion Public

        #region Private

        /// <summary>
        /// Remove the specified branch from the splitter.
        /// </summary>
        /// <param name="payload"></param>
        private void RemoveFiltersDownstreamFromSplitter(PayloadType payload) {
            if (dvSplitter != null) {
                if (payload == PayloadType.dynamicAudio) {
                    RemoveFiltersDownstreamFromPin(splitterAudioOut);
                    audioCompressor = null;
                }
                else if (payload == PayloadType.dynamicVideo) {
                    RemoveFiltersDownstreamFromPin(splitterVideoOut);
                    videoCompressor = null;
                }
            }
        }

        /// <summary>
        /// Add a DV Splitter filter and connect it to the DV source.  If the DV Splitter is already there, do nothing.
        /// </summary>
        /// <returns></returns>
        private bool AddDVSplitter() {
            if ((dvSplitter != null) &&
                (splitterAudioOut != null) &&
                (splitterVideoOut != null))
                return true;

            //Add a DVSplitter and connect it.
            try {
                dvSplitter = Filter.CreateBaseFilterByName("DV Splitter");
                iGB.AddFilter(dvSplitter, "DV Splitter");
                IPin dvSplitterInput;
                dvSplitter.FindPin("Input", out dvSplitterInput);
                iGB.Connect(source.OutputPin, dvSplitterInput);
            }
            catch (COMException) {
                dvSplitter = null;
                splitterVideoOut = splitterAudioOut = null;
                return false;
            }

            //Find output pins
            try {
                this.splitterVideoOut = Filter.GetPin(dvSplitter, _PinDirection.PINDIR_OUTPUT, Guid.Empty, Guid.Empty, true, 0);
                this.splitterAudioOut = Filter.GetPin(dvSplitter, _PinDirection.PINDIR_OUTPUT, Guid.Empty, Guid.Empty, true, 1);
            }
            catch (COMException) {
                dvSplitter = null;
                splitterVideoOut = splitterAudioOut = null;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Remove the specified filter from the graph and dispose it.
        /// </summary>
        /// <param name="fil"></param>
        private void RemoveAndDispose(Filter fil) {
            if ((fil != null) && (fil.BaseFilter != null)) {
                iFG.RemoveFilter(fil.BaseFilter);
                fil.Dispose();
            }
        }


        #endregion Private

    }

    #region Interfaces

    public interface IVideoCaptureGraph {
        VideoCompressor VideoCompressor { get; }
        void AddCompressor(FilterInfo fi);
    }

    public interface IAudioCaptureGraph {
        AudioCompressor AudioCompressor { get; }
    }

    #endregion Interfaces

}
