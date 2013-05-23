using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using MSR.LST.MDShow;
using MSR.LST.Net.Rtp;
using System.Collections.Generic;

// Explanantions about InvalidCastException catch in ICapabilityWindow and 
// ICapabilityWindowExtended implementation:
// It could be that the iVW is invalid, since the interface is added and
// removed to FilterGraphManager as a Video Render filter is created and
// displayed, so we have to watch for an InvalidCastException if the
// video window has disappeared and therefore the interface has been removed.


namespace MSR.LST.ConferenceXP
{
    [Capability.Name("Video")]
    [Capability.PayloadType(PayloadType.dynamicVideo)]
    [Capability.FormType(typeof(FAudioVideo))]
    [Capability.Channel(false)]
    [Capability.Fec(true)]
    [Capability.FecRatio(0, 1)]
    public class VideoCapability : CapabilityDeviceWithWindow, ICapabilitySender, ICapabilityViewer, ICapabilityWindowExtended, ICapabilityWindowAutoSize
    {
        #region Statics
        
        /// <summary>
        /// Check the registry and compare to the current list of cameras on 
        /// the machine (in case there is a mismatch).  
        /// </summary>
        /// <returns>
        /// Return the FilterInfo for cameras that were selected and still exist
        /// </returns>
        public static FilterInfo[] SelectedCameras()
        {
            ArrayList selectedCameras = new ArrayList();

            string[] regSelectedCameras = AVReg.ValueNames(AVReg.SelectedCameras);
            if(regSelectedCameras != null)
            {
                FilterInfo[] cameras = VideoSource.Sources();

                for(int i = 0; i < cameras.Length; i++)
                {
                    for(int j = 0; j < regSelectedCameras.Length; j++)
                    {
                        if(cameras[i].Moniker == regSelectedCameras[j])
                        {
                            selectedCameras.Add(cameras[i]);
                            break;
                        }
                    }
                }
            }

            return (FilterInfo[])selectedCameras.ToArray(typeof(FilterInfo));
        }

        
        /// <summary>
        /// Disable DirectX Video Acceleration.  
        /// </summary>
        /// <param name="fgm"></param>
        public static void DisableDXVA(FilgraphManagerClass fgm)
        {
            // Retrieve WM decoder so we can turn off DXVA
            IBaseFilter decoder = Filter.FindBaseFilterByName(fgm, "WMVideo Decoder DMO");
            if(decoder == null)
            {
                return;
            }

            // Remove the renderer and everything back to the decoder
            IFilterGraph iFG = (IFilterGraph)fgm;
            IEnumFilters iEnum;
            iFG.EnumFilters(out iEnum);

            uint fetched;
            IBaseFilter iBF;
            iEnum.Next(1, out iBF, out fetched);
            List<IBaseFilter> toRemove = new List<IBaseFilter>();

            /// A base assumption is that the first items returned by the enumerator
            /// are the video renderer and other filters upstream to the video decoder.
            /// This should be true if these filters were the most recently added.
            /// Otherwise it would break in some contexts, including that of graphs with multiple branches.
            while(fetched == 1 && (Filter.Name(iBF) != Filter.Name(decoder)))
            {
                toRemove.Add(iBF);
                iEnum.Next(1, out iBF, out fetched);
            }

            foreach (IBaseFilter ibf in toRemove) {
                iFG.RemoveFilter(ibf);
            }

            // Try turning off DXVA
            try
            {
                IPropertyBag iPB = (IPropertyBag)decoder;
                object o = false;
                iPB.Write(WM9PropList.g_wszWMVCDXVAEnabled, ref o);
            }
            catch(Exception e){Console.WriteLine(e.ToString());} // Might be WM7 instead of WM9

            // Render again
            ((IGraphBuilder)fgm).Render(Filter.GetPin(decoder, _PinDirection.PINDIR_OUTPUT, 
                Guid.Empty, Guid.Empty, false, 0));
        }

        
        #endregion Statics    

        #region Members

        // The ratio of height to width of the video.  Set to -1 initially to indicate
        //  that we should ignore this until we know the video size.
        internal double vidSrcRatio = -1.0; // srcHeight/srcWidth

        private IVideoWindow iVW = null;
        private IBasicVideo iBV = null;

        private IntPtr videoWindowHandle = IntPtr.Zero;
        private IntPtr videoMessageDrainHandle = IntPtr.Zero;
        private int videoWindowHeight = 0;
        private int videoWindowWidth = 0;

        #endregion

        #region Constructors

        public VideoCapability(FilterInfo fi) : base(fi)
        {
            RtpStream.FirstFrameReceived += new RtpStream.FirstFrameReceivedEventHandler(RtpStream_FirstFrameReceived);
        }

        public VideoCapability(DynamicProperties dynaProps) : base(dynaProps)
        {
            RtpStream.FirstFrameReceived += new RtpStream.FirstFrameReceivedEventHandler(RtpStream_FirstFrameReceived);
        }
        
         
        public override void Dispose()
        {
            base.Dispose ();

            RtpStream.FirstFrameReceived -= new RtpStream.FirstFrameReceivedEventHandler(RtpStream_FirstFrameReceived);

            DisposeFgm();
            DeactivateCamera();
        }

        
        #endregion

        #region Public
        
        #region Sending side / Capture graph

        /// <summary>
        /// Get the CaptureGraph for this capability which could be a VideoCaptureGraph or a DVCaptureGraph
        /// </summary>
        public CaptureGraph CaptureGraph {
            get { return cg; }
        }

        /// <summary>
        /// Creates the camera (capture graph)
        /// </summary>
        public void ActivateCamera()
        {
            Log(string.Format(CultureInfo.CurrentCulture, "\r\nInitializing camera - {0}, {1}", 
                fi.DisplayName, fi.Moniker));

            // Get camera up and running
            CreateVideoGraph(fi);
            RestoreCameraSettings();
            RestoreVideoSettings();
            LogCurrentMediaType(cg.Source);

            // Add compressor if necessary
            AddVideoCompressor();

            try {
                //This was observed to fail for some non-standard compressors
                LogCurrentMediaType(cg.Compressor);
            }
            catch (Exception ex) {
                Log("Failed to find compressor current media type: " + ex.Message);
            }

            // Log all the filters in the graph
            Log(FilterGraph.Debug(cg.IFilterGraph));
        }

        /// <summary>
        /// Adds a video compressor if needed by checking the registry
        /// </summary>
        public void AddVideoCompressor()
        {
            if(RegVideoCompressorEnabled)
            {
                // Add compressor to the graph and dump its features
                vcg.AddCompressor(VideoCompressor.DefaultFilterInfo());
                Log(vcg.VideoCompressor.Dump());

                if(RegCustomCompression)
                {
                    RestoreVideoCompressorSettings();
                }
                else
                {
                    DefaultVideoCompressorSettings();
                }
            }
        }

        /// <summary>
        /// Disposes the camera (capture graph)
        /// </summary>
        public void DeactivateCamera()
        {
            if(cg != null)
            {
                Log(string.Format(CultureInfo.CurrentCulture, "\r\nDisposing graph - {0}", 
                    cg.Source.FriendlyName));

                /// Extra cleanup for a shared DV capture graph
                /// Remove everything on the video branch from DV Splitter.  If the graph is shared with
                /// an audio capability, it should be able to run again.
                if (cg is DVCaptureGraph) {
                    cg.Stop();
                    cg.RemoveFiltersDownstreamFromSource(MSR.LST.Net.Rtp.PayloadType.dynamicVideo);
                }

                cg.Dispose();
                cg = null;
            }
        }


        #region Registry

        public bool RegCustomCompression
        {
            get
            {
                bool custom = false;

                object setting = AVReg.ReadValue(DeviceKey(), AVReg.CustomCompression);
                if(setting != null)
                {
                    custom = bool.Parse((string)setting);
                }

                return custom;
            }

            set
            {
                AVReg.WriteValue(DeviceKey(), AVReg.CustomCompression, value);
            }
        }


        /// <summary>
        /// Save the camera's current settings to the registry
        /// </summary>
        public void SaveCameraSettings()
        {
            if (cg is VideoCaptureGraph) {
                VideoCaptureGraph vcg = (VideoCaptureGraph)cg;
                if (vcg.VideoSource.HasVideoStandards) {
                    AVReg.WriteValue(DeviceKey(), AVReg.VideoStandardIndex, vcg.VideoSource.VideoStandardIndex);
                }
            }
            else if (cg is DVCaptureGraph) { 
                DVCaptureGraph dvcg = (DVCaptureGraph)cg;
                if (dvcg.VideoSource.HasVideoStandards) {  //Always false for DV devices?
                    AVReg.WriteValue(DeviceKey(), AVReg.VideoStandardIndex, dvcg.VideoSource.VideoStandardIndex);
                }               
            }

            SaveDeviceSettings();
        }

        /// <summary>
        /// Save the video stream's current settings to the registry
        /// </summary>
        public void SaveVideoSettings()
        {
            SaveStreamSettings();
        }

        public void SaveVideoCompressorSettings()
        {
            VideoCompressorQualityInfo vcqi = vcg.VideoCompressor.QualityInfo;

            // Cast to int, otherwise it gets stored as a string
            AVReg.WriteValue(DeviceKey(), AVReg.CompressorBitRate, (int)vcqi.BitRate);
            AVReg.WriteValue(DeviceKey(), AVReg.CompressorKeyFrameRate, vcqi.KeyFrameRate);
        }


        #endregion Registry

        /// <summary>
        /// Shows the form's camera configuration dialog
        /// </summary>
        public void ShowCameraConfiguration()
        {
            if (cg is VideoCaptureGraph) {
                VideoCaptureGraph vcg = (VideoCaptureGraph)cg;
                if (vcg.VideoSource.IsVfw) {
                    RtlAwareMessageBox.Show(null, Strings.CanNotConfigureCamera, string.Empty, MessageBoxButtons.OK,
                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                    return;
                }

                if (cg != null && vcg.VideoSource.HasSourceDialog) {
                    vcg.VideoSource.ShowSourceDialog(form.Handle);
                }
            }
        }
        
        
        public override void Send()
        {
            lock(Conference.ActiveVenue)
            {
                lock(this)
                {
                    try
                    {
                        if (!isSending)
                        {
                            base.Send();

                            if(cg.VideoRenderer == null)
                            {
                                cg.RenderNetwork(RtpSender,MSR.LST.Net.Rtp.PayloadType.dynamicVideo);
                            }

                            cg.Run();

                            // TODO: We shouldn't have to deal with adding capabilities
                            //       in the capabilitySenders collection here. This code should
                            //       be removed from here when Conference has been cleaned-up
                            if (!Conference.CapabilitySenders.ContainsKey(ID))
                            {
                                Conference.AddCapabilitySender(this);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        eventLog.WriteEntry(e.ToString(), System.Diagnostics.EventLogEntryType.Error, 99);

                        StopSending();

                        throw;
                    }
                }
            }
        }

        public override void StopSending()
        {
            lock(this)
            {
                if (isSending)
                {
                    // We must shut down the device before calling base, since base will dispose
                    // the RtpSender that device is using.
                    cg.Stop();

                    base.StopSending();
                }
            }
        }

        public void StopSendingVideo()
        {
            cg.Stop();

            uiState |= (int)FAudioVideo.UIState.LocalVideoSendStopped;
            ((FAudioVideo)form).UpdateVideoUI(uiState);
        }

        public void ResumeSendingVideo()
        {
            cg.Run();

            uiState &= ~(int)FAudioVideo.UIState.LocalVideoSendStopped;
            ((FAudioVideo)form).UpdateVideoUI(uiState);
        }


        #endregion Sending side / Capture graph

        #region Receiving side / Playing graph

        public FilgraphManagerClass PlayGraph
        {
            get{return fgm;}
        }

        /// <summary>
        /// Overridden because we don't want to hook the frame received event
        /// </summary>
        /// <param name="rtpStream"></param>
        public override void StreamAdded(RtpStream rtpStream)
        {
            if(this.rtpStream != null)
            {
                this.rtpStream.DataStarted -= new MSR.LST.Net.Rtp.RtpStream.DataStartedEventHandler(rtpStream_DataStarted);
                this.rtpStream.DataStopped -= new MSR.LST.Net.Rtp.RtpStream.DataStoppedEventHandler(rtpStream_DataStopped);
            }

            lock(this)
            {
                this.rtpStream = rtpStream;
                this.rtpStreams.Add(rtpStream);
            }

            rtpStream.IsUsingNextFrame = true;

            // We only need to hook these events if we are remote players, not senders
            if(!IsSender)
            {
                rtpStream.DataStarted += new MSR.LST.Net.Rtp.RtpStream.DataStartedEventHandler(rtpStream_DataStarted);
                rtpStream.DataStopped += new MSR.LST.Net.Rtp.RtpStream.DataStoppedEventHandler(rtpStream_DataStopped);
            }
        }

        
        // Important note: 
        // We do not call base.Play() because:
        //    - We want to control when ShowForm is called
        //    - We don't want to hook streams FrameReceived events
        public override void Play()
        {
            lock(Conference.ActiveVenue)
            {
                lock(this)
                {
                    if (disposed)
                    {
                        throw new ObjectDisposedException(name);
                    }

                    if(!isPlaying)
                    {
                        isPlaying = true;

                        CreateForm();
                        ShowForm();

                        Conference.RaiseCapabilityPlaying(this);
                        
                        ResumePlayingVideo();

                        // Capability state and fgm state could be different
                        ((FAudioVideo)form).UpdateVideoUI(uiState);
                    }
                }
            }
        }

        public override void StopPlaying()
        {
            lock(this)
            {
                if (isPlaying)
                {
                    StopPlayingVideo();
                    base.StopPlaying();

                    // Capability state and fgm state could be different
                    // And capability / form state are different between Sender and Receiver
                    if(form != null)
                    {
                        ((FAudioVideo)form).UpdateVideoUI(uiState);
                    }
                }
            }
        }

        public void StopPlayingVideo()
        {
            lock(fgmLock)
            {
                if(fgmState != FilterGraph.State.Stopped)
                {
                    fgmState = FilterGraph.State.Stopped;

                    if (fgm != null)
                    {
                        // We need to manually unblock the stream in case there is no data flowing
                        if(rtpStream != null)
                        {
                            rtpStream.UnblockNextFrame();
                        }

                        // Due to some interesting bugs(?) in DShow, it is necessary for us to keep
                        // track of our state and re-initialize iBV and iVW
                        // We set the Owner to zero, otherwise when a window goes away and a new one
                        // takes its place we can't control it.
                        fgm.Stop();

                        if(iVW != null)
                        {
                            iVW.Visible = 0;
                            iVW.Owner = 0;
                            iVW = null;
                        }

                        iBV = null;
                    }

                    uiState |= (int)FAudioVideo.UIState.LocalVideoPlayStopped;
                    ((FAudioVideo)form).UpdateVideoUI(uiState);
                }
            }
        }

        public void ResumePlayingVideo()
        {
            Debug.Assert(IsPlaying);

            lock(fgmLock)
            {
                if(fgmState != FilterGraph.State.Running)
                {
                    fgmState = FilterGraph.State.Running;

                    if (fgm != null)
                    {
                        // Due to some interesting bugs(?) in DShow, it is necessary for us to keep
                        // track of our state and re-initialize iBV and iVW
                        iBV = (IBasicVideo)fgm;
                        iVW = (IVideoWindow)fgm;
                        iVW.Owner = videoWindowHandle.ToInt32();
                        iVW.MessageDrain = videoMessageDrainHandle.ToInt32();
                        iVW.Visible = -1;

                        ResizeVideoStream(videoWindowHeight, videoWindowWidth);

                        // We need to manually block the stream to reset its state in case it
                        // became inconsistent during shutdown
                        if(rtpStream != null)
                        {
                            rtpStream.BlockNextFrame();
                        }

                        fgm.Run();
                    }

                    uiState &= ~(int)FAudioVideo.UIState.LocalVideoPlayStopped;
                    ((FAudioVideo)form).UpdateVideoUI(uiState);
                }
            }
        }


        /// <summary>
        /// Allows the AV form to set the handle to the picture box
        /// where the video will be displayed
        /// </summary>
        public IntPtr VideoWindowHandle
        {
            set
            {
                videoWindowHandle = value;

                if(iVW != null)
                {
                    iVW.Owner = videoWindowHandle.ToInt32();
                }
            }
        }

        /// <summary>
        /// Allows the AV form to set the handle to the control that 
        /// receives mouse events on the video
        /// </summary>
        public IntPtr VideoWindowMessageDrain {
            set {
                videoMessageDrainHandle = value;
                if (iVW != null) {
                    iVW.MessageDrain = this.videoMessageDrainHandle.ToInt32();
                }
            }
        }

        /// <summary>
        /// Resize the destination video using scaling
        /// </summary>
        public void ResizeVideoStream(int height, int width)
        {
            // Store the values passed in, in case this is called before stream added
            videoWindowHeight = height;
            videoWindowWidth = width;

            // Note: We need to check if height and width are greater then zero
            // because iBV.DestinationHeight iBV.DestinationWidth must be greater than
            // zero. Typical case when height and width are zero is when the user minimize
            // the window.
            if((iBV != null) && (height > 0 && width > 0))
            {
                // Ratio y:x of the video source
                double vidSrcRatio = (double)(iBV.SourceHeight)/(double)(iBV.SourceWidth);

                if(vidSrcRatio * width >= height)
                    // Width is oversize, resize by height
                {                
                    // The video height takes the full height of the display area
                    iBV.DestinationTop = 0;
                    iBV.DestinationHeight = height;

                    // Resize dest width - left = margin, right = margin + new width
                    int newWidth = (int)(height / vidSrcRatio);
                    iBV.DestinationLeft = (int)(width - newWidth)/2;
                    iBV.DestinationWidth = newWidth;
                }
                else // Height is oversize, resize by width
                { 
                    // The video width takes the full width of the display area
                    iBV.DestinationLeft = 0;
                    iBV.DestinationWidth = width;

                    // Resize dest height - top = margin, bottom = margin + new height
                    int newHeight = (int)(width * vidSrcRatio);
                    iBV.DestinationTop = (int)(height - newHeight)/2;
                    iBV.DestinationHeight = newHeight;
                }
            }

            if(iVW != null)
            {
                // Set the position/size of the default renderer window inside the
                // CXP video UI (and removed the borders)
                iVW.SetWindowPosition(-3, -3, width + 6, height + 6);
            }
        }

        /// <summary>
        /// Allows the AV form to set the width of the UI.
        /// </summary>
        /// <remarks>
        /// This is then used in the override Size accessor to calulate
        /// the actual size of the AV form given the video ratio and the
        /// size of the UI.
        /// </remarks>
        public int UIBorderWidth
        {
            set{uIBorderWidth = value;}
        }

        /// <summary>
        /// Allows the AV form to set the height of the UI.
        /// </summary>
        /// <remarks>
        /// This is then used in the override Size accessor to calulate
        /// the actual size of the AV form given the video ratio and the
        /// size of the UI.
        /// </remarks>
        public int UIBorderHeight
        {
            set{uIBorderHeight = value;}
        }

        
        public int BorderColor
        {
            get { return iVW.BorderColor; }
            set { iVW.BorderColor = value; }
        }

        public bool CursorHidden
        {
            get
            {
                int hidden = 0;
                iVW.IsCursorHidden(out hidden);
                return (hidden == -1) ? true : false;
            }
            set
            {
                if (value == true)
                {
                    iVW.HideCursor(-1);
                }
                else
                {
                    iVW.HideCursor(0);
                }
            }
        }

        public Int32 WindowOwner
        {
            get { return iVW.Owner; }
            set { iVW.Owner = value; }
        }

        public int WindowState
        {
            get { return iVW.WindowState; }
            set { iVW.WindowState = value; }
        }

        public int WindowStyle 
        { 
            get { return iVW.WindowStyle; }
            set { iVW.WindowStyle = value; }
        }


        #endregion Receiving side / Playing graph
       
        #endregion Public

        #region Private

        #region Sending side / Capture graph

        private IVideoCaptureGraph vcg {
            get { return (IVideoCaptureGraph)cg; }
        }

        
        /// <summary>
        /// Creates the actual FilgraphManager with the chosen camera
        /// </summary>
        private void CreateVideoGraph(FilterInfo fi)
        {
            Debug.Assert(cg == null);

            // Create the graph, which creates the source filter
            if (DVSource.IsDVSourceWithAudio(fi)) {
                cg = DVCaptureGraph.GetInstance(fi);
                Log(((DVCaptureGraph)cg).VideoSource.Dump());
            }
            else {
                cg = new VideoCaptureGraph(fi);
                Log(((VideoCaptureGraph)cg).VideoSource.Dump());
            }
        }

        /// <summary>
        /// By default, we use the WMVideo9 Encoder DMO, WMV1 media type 
        /// 
        /// We support 3 basic default bit rates for video
        /// 100 Kbps for video &lt; 320 x 240
        /// 300 Kbps for video &gte; 320 x 240 && &lt; 640 x 480
        /// 1   Mbps for video &gte; 640 x 480
        /// </summary>
        private void DefaultVideoCompressorSettings()
        {
            // Read camera's current media type
            _AMMediaType mt;
            object fb;
            cg.Source.GetMediaType(out mt, out fb);

            int bmpRes = 640 * 480;
            if (fb is VIDEOINFOHEADER) {
                VIDEOINFOHEADER vih = (VIDEOINFOHEADER)fb;
                bmpRes = vih.BitmapInfo.Height * vih.BitmapInfo.Width;
            }
            else if (fb is DVINFO) {
                DVCaptureGraph dvcg = cg as DVCaptureGraph;
                if (dvcg != null) {
                    dvcg.GetVideoMediaType(out mt, out fb);
                    if (fb is VIDEOINFOHEADER) {
                        VIDEOINFOHEADER vih = (VIDEOINFOHEADER)fb;
                        bmpRes = vih.BitmapInfo.Height * vih.BitmapInfo.Width;
                    }
                }
            }
            else {
                Debug.Fail(string.Format(CultureInfo.CurrentCulture,
                    "We were expecting a DVINFO or VIDEOINFOHEADER format block, not a {0}",
                    MediaType.FormatType.GuidToString(mt.formattype)));
                return;
            }

            // Construct the compressor settings
            VideoCompressorQualityInfo vcqi = DefaultQualityInfo();

            if(bmpRes < 320 * 240)
            {
                vcqi.BitRate = 100000;
            }
            else if(bmpRes < 640 * 480)
            {
                vcqi.BitRate = 300000;
            }
            else
            {
                vcqi.BitRate = 1000000;
            }

            // Set the Video Compressor's Quality
            vcg.VideoCompressor.QualityInfo = vcqi;
        }

        private VideoCompressorQualityInfo DefaultQualityInfo()
        {
            return VideoCompressor.DefaultQualityInfo;
        }
        
        #region Registry

        /// <summary>
        /// Restore the camera's last settings from the registry
        /// </summary>
        private void RestoreCameraSettings()
        {
            object setting;

            if (cg is VideoCaptureGraph) {
                VideoCaptureGraph vcg = (VideoCaptureGraph)cg;
                if (vcg.VideoSource.HasVideoStandards) {
                    if ((setting = AVReg.ReadValue(DeviceKey(), AVReg.VideoStandardIndex)) != null) {
                        vcg.VideoSource.VideoStandardIndex = (int)setting;
                    }
                }
            }
            else if (cg is DVCaptureGraph) { 
                DVCaptureGraph dvcg = (DVCaptureGraph)cg;
                if (dvcg.VideoSource.HasVideoStandards) {
                    if ((setting = AVReg.ReadValue(DeviceKey(), AVReg.VideoStandardIndex)) != null) {
                        dvcg.VideoSource.VideoStandardIndex = (int)setting;
                    }
                }               
            }
            if (cg.Source.HasPhysConns) {
                if ((setting = AVReg.ReadValue(DeviceKey(), AVReg.PhysicalConnectorIndex)) != null) {
                    cg.Source.PhysicalConnectorIndex = (int)setting;
                }
            }
        }

        /// <summary>
        /// Restore the video stream's last settings from the registry
        /// </summary>
        private void RestoreVideoSettings()
        {
            // Read media type from registry
            byte[] bytes = (byte[])AVReg.ReadValue(DeviceKey(), AVReg.MediaType);

            if(bytes != null)
            {
                AVReg.ms.Position = 0;
                AVReg.ms.Write(bytes, 0, bytes.Length);

                AVReg.ms.Position = 0;
                _AMMediaType mt = (_AMMediaType)AVReg.bf.Deserialize(AVReg.ms);
                
                // Read format block from registry
                if(mt.cbFormat != 0)
                {
                    bytes = (byte[])AVReg.ReadValue(DeviceKey(), AVReg.FormatBlock);
                    Debug.Assert(bytes.Length == mt.cbFormat);

                    mt.pbFormat = Marshal.AllocCoTaskMem((int)mt.cbFormat);
                    Marshal.Copy(bytes, 0, mt.pbFormat, (int)mt.cbFormat);

                    Log("Restoring stream settings...");
                    Log(MediaType.Dump(mt));

                    try
                    {
                        // Set and free
                        cg.Source.SetMediaType(ref mt);
                    }
                    catch(COMException ex)
                    {
                        Log(DShowError._AMGetErrorText(ex.ErrorCode));
                        Log(ex.ToString());
                    }
                    catch(Exception ex)
                    {
                        Log(ex.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Restore the video compressor's last settings from the registry
        /// </summary>
        private void RestoreVideoCompressorSettings()
        {
            VideoCompressorQualityInfo vcqi = DefaultQualityInfo();
            vcqi.KeyFrameRate = (int)AVReg.ReadValue(DeviceKey(), AVReg.CompressorKeyFrameRate);
            vcqi.BitRate = (uint)((int)AVReg.ReadValue(DeviceKey(), AVReg.CompressorBitRate));

            vcg.VideoCompressor.QualityInfo = vcqi;
        }


        #endregion Registry

        #endregion Sending side / Capture graph

        #region Receiving side / Playing graph

        private void DisposeFgm()
        {
            lock(fgmLock)
            {
                if (fgm != null)
                {
                    // We need to manually unblock the stream in case there is no data flowing
                    if(rtpStream != null)
                    {
                        rtpStream.UnblockNextFrame();
                    }

                    FilterGraph.RemoveFromRot(rotID);
                    fgm.Stop();
                    FilterGraph.RemoveAllFilters(fgm);
                    fgm = null;
                    iVW = null;
                    iBV = null; 
                }
            }
        }

        private void RtpStream_FirstFrameReceived(object sender, EventArgs ea)
        {
            // It is safe to render the graph, because there is a frame available
            if(sender == rtpStream)
            {
                // Creation of the fgm and the adding / removing of filters needs to happen on the
                // same thread.  So make sure it all happens on the UI thread.
                Conference.FormInvoke(new System.Windows.Forms.MethodInvoker(_RtpStream_FirstFrameReceived), null);
            }
        }

        /// <summary>
        /// Creation of the fgm and the adding / removing of filters needs to happen on the
        /// same thread.  So make sure it all happens on the UI thread.
        /// </summary>
        private void _RtpStream_FirstFrameReceived()
        {
            lock(fgmLock)
            {
                DisposeFgm();

                Debug.Assert(fgm == null);

                // Create the DirectShow filter graph manager
                fgm = new FilgraphManagerClass();
                IGraphBuilder iGB = (IGraphBuilder)fgm;            
                rotID = FilterGraph.AddToRot((IGraphBuilder)fgm);

                IBaseFilter bfSource = RtpSourceClass.CreateInstance();
                ((MSR.LST.MDShow.Filters.IRtpSource)bfSource).Initialize(rtpStream);

                iGB.AddFilter(bfSource, "RtpSource");

                iGB.Render(Filter.GetPin(bfSource, _PinDirection.PINDIR_OUTPUT, Guid.Empty, 
                    Guid.Empty, false, 0));

                DisableDXVA(fgm);

                // Render the video inside of the form
                iVW = (IVideoWindow)fgm;

                // Get the correct ratio to use for the video stretching
                //  I would expect the fgm to always be castable to this, but I simply don't trust DShow
                IBasicVideo iBV = fgm as IBasicVideo;
                if (iBV != null)
                {
                    int vidWidth, vidHeight;
                    iBV.GetVideoSize(out vidWidth, out vidHeight);
                    vidSrcRatio = (double)vidHeight / (double)vidWidth;
                }

                // Remove the border from the default DShow renderer UI
                int ws = WindowStyle;
                ws = ws & ~(0x00800000); // Remove WS_BORDER
                ws = ws & ~(0x00400000); // Remove WS_DLGFRAME
                WindowStyle = ws;

                iVW = null;

                uiState &= ~(int)FAudioVideo.UIState.RemoteVideoStopped;

                if(form != null)
                {
                    ((FAudioVideo)form).UpdateVideoUI(uiState);
                }

                // FirstFrameReceived interprets fgmState as the *desired* state for the fgm
                // Because ResumePlayingVideo won't actually start if the state is already 
                // Running, we change it to Stopped so that it will start
                if(IsPlaying && fgmState == FilterGraph.State.Running)
                {
                    fgmState = FilterGraph.State.Stopped;
                    ResumePlayingVideo();
                }
            }
        }

        private void rtpStream_DataStarted(object sender, EventArgs ea)
        {
            // Let the form know data has started flowing again
            uiState &= ~(int)FAudioVideo.UIState.RemoteVideoStopped;
            ((FAudioVideo)form).UpdateVideoUI(uiState);
        }


        private void rtpStream_DataStopped(object sender, EventArgs ea)
        {
            // Let the form know data has stopped flowing again
            uiState |= (int)FAudioVideo.UIState.RemoteVideoStopped;
            ((FAudioVideo)form).UpdateVideoUI(uiState);
        }


        #endregion Receiving side / Playing graph


        #endregion
    }
}