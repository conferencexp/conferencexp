using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using MSR.LST.MDShow;
using MSR.LST.MDShow.Filters;
using MSR.LST.Net.Rtp;


namespace MSR.LST.ConferenceXP
{
    public class frmWMFile : CapabilityForm
    {
        #region Windows Form Designer generated code

        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.CheckBox ckRepeat;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnClose;
        private MSR.LST.MediaControlButton btnPlay;
        private MSR.LST.MediaControlButton btnStop;
        private MSR.LST.MediaControlButton btnPause;
        private System.Windows.Forms.ToolTip buttonToolTips;
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWMFile));
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.btnSelect = new System.Windows.Forms.Button();
            this.ckRepeat = new System.Windows.Forms.CheckBox();
            this.btnPlay = new MSR.LST.MediaControlButton();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnStop = new MSR.LST.MediaControlButton();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPause = new MSR.LST.MediaControlButton();
            this.buttonToolTips = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelect.Location = new System.Drawing.Point(16, 48);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(96, 24);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.Text = "Selected file: {0}";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // ckRepeat
            // 
            this.ckRepeat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ckRepeat.Location = new System.Drawing.Point(264, 48);
            this.ckRepeat.Name = "ckRepeat";
            this.ckRepeat.Size = new System.Drawing.Size(56, 24);
            this.ckRepeat.TabIndex = 1;
            this.ckRepeat.Text = "Repeat";
            // 
            // btnPlay
            // 
            this.btnPlay.Enabled = false;
            this.btnPlay.Image = ((System.Drawing.Image)(resources.GetObject("btnPlay.Image")));
            this.btnPlay.ImageType = MSR.LST.MediaControlImage.Play;
            this.btnPlay.Location = new System.Drawing.Point(144, 48);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(24, 24);
            this.btnPlay.TabIndex = 2;
            this.buttonToolTips.SetToolTip(this.btnPlay, "Play");
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblInfo.Location = new System.Drawing.Point(16, 8);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(312, 30);
            this.lblInfo.TabIndex = 5;
            this.lblInfo.Text = "Click \"Select File\" to specify the Windows Media file you want to play.";
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageType = MSR.LST.MediaControlImage.Stop;
            this.btnStop.Location = new System.Drawing.Point(208, 48);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(24, 24);
            this.btnStop.TabIndex = 6;
            this.buttonToolTips.SetToolTip(this.btnStop, "Stop");
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClose.Location = new System.Drawing.Point(248, 88);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 24);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPause
            // 
            this.btnPause.Enabled = false;
            this.btnPause.Image = ((System.Drawing.Image)(resources.GetObject("btnPause.Image")));
            this.btnPause.ImageType = MSR.LST.MediaControlImage.Pause;
            this.btnPause.Location = new System.Drawing.Point(176, 48);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(24, 24);
            this.btnPause.TabIndex = 2;
            this.buttonToolTips.SetToolTip(this.btnPause, "Pause");
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // frmWMFile
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(338, 128);
            this.ControlBox = false;
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.ckRepeat);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPause);
            this.Font = UIFont.FormFont;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmWMFile";
            this.Text = "Windows Media Playback";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmWMV_Closing);
            this.Load += new System.EventHandler(this.frmWMFile_Load);
            this.ResumeLayout(false);

        }


        #endregion

        #region Members

        /// <summary>
        /// The file that this capability is supposed to be playing
        /// </summary>
        private string file;

        /// <summary>
        /// Instance of the capability that launched this form
        /// </summary>
        private WMFileCapability wmf = null;

        /// <summary>
        /// Instance of the DShow FilgraphManager for controlling filtergraphs
        /// </summary>
        private FilgraphManagerClass fgm = null;

        /// <summary>
        /// Interface used to Add / Remove filters from the graph
        /// </summary>
        private IGraphBuilder iGB = null;

        /// <summary>
        /// Interface used to Connect filters together inside the graph
        /// </summary>
        private ICaptureGraphBuilder2 iCGB2 = null;

        /// <summary>
        /// Instance of a class that provides a thread for monitoring events for the fgm
        /// Raises events when something intersting happens
        /// </summary>
        private FgmEventMonitor fgmEventMonitor = null;

        /// <summary>
        /// An identifier to keep track of the fgm in the Running Object Table
        /// Needed so we can unregister when done
        /// </summary>
        private uint rotID = 0;

        /// <summary>
        /// Instance of the Windows Media ASF Reader filter, for reading WM files
        /// </summary>
        private IBaseFilter wmASFReader = null;

        /// <summary>
        /// Instance of the RtpRenderer filter for sending data across the network
        /// </summary>
        private IBaseFilter videoRenderer = null;

        /// <summary>
        /// Instance of the RtpRenderer filter for sending data across the network
        /// </summary>
        private IBaseFilter audioRenderer = null;

        /// <summary>
        /// Instance of an RtpSender for sending data across the network - provided by the capability
        /// </summary>
        private RtpSender videoSender;

        /// <summary>
        /// Instance of an RtpSender for sending data across the network - provided by the capability
        /// </summary>
        private RtpSender audioSender;

        #endregion Members

        #region Public

        public frmWMFile()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        public override void AddCapability(ICapability capability)
        {
            base.AddCapability(capability);

            if (wmf == null)
            {
                wmf = (WMFileCapability)capability;
            }
        }

        public override bool RemoveCapability(ICapability capability)
        {
            bool ret = base.RemoveCapability(capability);

            if (ret)
            {
                if (fgm != null)
                {
                    fgm.Stop();
                    FilterGraph.RemoveFromRot(rotID);
                    FilterGraph.RemoveAllFilters(fgm);
                    fgm = null;
                }

                if (fgmEventMonitor != null)
                {
                    fgmEventMonitor.FgmEvent -= new FgmEventMonitor.FgmEventHandler(FgmEvent);
                    fgmEventMonitor.Dispose();
                    fgmEventMonitor = null;
                }

                wmf = null;
            }

            return ret;
        }


        public void RtpSenders(RtpSender audioSender, RtpSender videoSender)
        {
            this.audioSender = audioSender;
            this.videoSender = videoSender;
        }

        /// <summary>
        /// Special method that allows the capability to be auto-started and
        /// loop on a video which acts as a signal for testing purposes
        /// </summary>
        /// <param name="path">Path to the wmv file</param>
        public void AutoSend(string path)
        {
            file = path;

            ckRepeat.Checked = true;
            btnPlay.Enabled = (file != null);
            btnPlay.PerformClick(); // play the new file (just ignores if disabled)
        }

        #endregion Public

        #region Private UI Events

        /// <summary>
        /// Choose the files to make available for playing, puts them all in the 'selected' state
        /// </summary>
        private void btnSelect_Click(object sender, System.EventArgs e)
        {
            // Type of files supported
            ofd.Filter = "WM Files (*.wmv;*.wma)|*.wmv;*.wma";

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                file = ofd.FileName;

                if (file != null)
                {
                    string exn = System.IO.Path.GetExtension(file).ToLower(CultureInfo.InvariantCulture);

                    if (exn != ".wmv" && exn != ".wma")
                    {
                        RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                            Strings.UnexpectedFileExtension, exn, file), string.Empty, MessageBoxButtons.OK, 
                            MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

                        file = null;
                    }
                    else
                    {
                        btnStop.PerformClick(); // stop the old file

                        lblInfo.Text = string.Format(CultureInfo.CurrentCulture, Strings.SelectedFile, file);
                    }
                }

                btnPlay.Enabled = (file != null);
                btnPlay.PerformClick(); // play the new file (just ignores if disabled)
            }
        }


        /// <summary>
        /// Starts / Stops the video playing process (always starts with the first video)
        /// Toggles the Play / Stop button
        /// </summary>
        private void btnPlay_Click(object sender, System.EventArgs e)
        {
            if (!btnPause.Enabled && btnStop.Enabled) // we're paused, not stopped
            {
                Debug.Assert(fgm != null);

                fgm.Run();
            }
            else
            {
                PlayFile(file);
            }

            btnPlay.Enabled = false;
            btnStop.Enabled = true;
            btnPause.Enabled = true;

            // Until we support changing the media type on the fly don't allow choosing another
            // file after the first one is started jasonv 4.22.2005
            btnSelect.Enabled = false;
        }


        /// <summary>
        /// Pauses / Resumes the current WM File
        /// </summary>
        private void btnStop_Click(object sender, System.EventArgs e)
        {
            fgm.Stop();

            btnPlay.Enabled = true;
            btnPause.Enabled = false;
            btnStop.Enabled = false;
        }


        private void btnPause_Click(object sender, System.EventArgs e)
        {
            fgm.Pause();

            btnPlay.Enabled = true;
            btnPause.Enabled = false;
            // stop remains enabled
        }


        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// If the form is closing tell the capability we are done sending
        /// </summary>
        private void frmWMV_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (wmf != null)
            {
                wmf.StopSending();
            }
        }

        #endregion

        #region Private Utility Methods

        /// <summary>
        /// Plays the provided file
        /// </summary>
        private void PlayFile(string fileName)
        {
            InitializeFgm();
            CreateWMASFReader(fileName);
            HookUpAudio();
            HookUpVideo();

            fgm.Run();
        }


        /// <summary>
        /// Creates and initializes the Fgm, retrieves interfaces from it, starts the monitoring 
        /// thread etc. the first time.  Stops it subsequent times.
        /// </summary>
        private void InitializeFgm()
        {
            if (fgm == null)
            {
                fgm = new FilgraphManagerClass();
                iGB = (IGraphBuilder)fgm;
                iCGB2 = MDShow.CaptureGraphBuilder2Class.CreateInstance();
                iCGB2.SetFiltergraph(iGB);

                rotID = FilterGraph.AddToRot(iGB);

                // Initialize class that will monitor events on the fgm
                fgmEventMonitor = new FgmEventMonitor((IMediaEvent)fgm);
                fgmEventMonitor.FgmEvent += new FgmEventMonitor.FgmEventHandler(FgmEvent);
            }
            else
            {
                fgm.Stop();
            }
        }


        /// <summary>
        /// Loads the provided file into a Windows Media ASF Filter for reading
        /// If the filter already exists, remove it from the graph, because...
        /// 
        /// The WM ASF Reader only allows you to Load 1 file per instance.  So re-create each time 
        /// through.  See IFileSourceFilter documentation.
        /// </summary>
        private void CreateWMASFReader(string fileName)
        {
            if (wmASFReader != null)
            {
                iGB.RemoveFilter(wmASFReader);
            }

            wmASFReader = Filter.CreateBaseFilterByName("WM ASF Reader");
            iGB.AddFilter(wmASFReader, "WM ASF Reader");

            _AMMediaType wmvmt = new _AMMediaType();
            ((IFileSourceFilter)wmASFReader).Load(fileName, ref wmvmt);
        }


        /// <summary>
        /// Hooks up the audio pin of the file to the audio renderer
        /// </summary>
        private void HookUpAudio()
        {
            HookUpFilter(ref audioRenderer, MediaType.MajorType.MEDIATYPE_Audio, audioSender, "RtpRenderer Audio");
        }


        /// <summary>
        /// Hooks up the video pin of the file to the video renderer
        /// </summary>
        private void HookUpVideo()
        {
            HookUpFilter(ref videoRenderer, MediaType.MajorType.MEDIATYPE_Video, videoSender, "RtpRenderer Video");
        }


        /// <summary>
        /// Hooks up the provided renderer to a pin of the provided media type on the file
        /// </summary>
        /// <param name="renderer">The filter to render data to</param>
        /// <param name="mediaType">The type of pin to connect to this renderer</param>
        /// <param name="rtpSender">RtpSender that will be used by the renderer</param>
        /// <param name="filterName">The name of the render filter in the graph</param>
        private void HookUpFilter(ref IBaseFilter renderer, Guid mediaType, RtpSender rtpSender,
            string filterName)
        {
            // Pin the Guid, so we can pass it as an IntPtr
            GCHandle hGuid = GCHandle.Alloc(mediaType, GCHandleType.Pinned);

            try
            {
                // Only create the filter once
                if (renderer == null)
                {
                    // ConferenceXP's custom filter for sending data over network
                    renderer = MDShow.RtpRendererClass.CreateInstance();

                    // Initialize the filter with an RtpSender to use
                    ((IRtpRenderer)renderer).Initialize(rtpSender);

                    // Add filter to graph
                    iGB.AddFilter(renderer, filterName);
                }

                // The WM file may not have a pin of the desired category, so this may fail
                // e.g. Audio only file won't have video, video may not have audio, etc.
                iCGB2.RenderStream(IntPtr.Zero, hGuid.AddrOfPinnedObject(), wmASFReader, null, renderer);
            }
            catch (COMException ce)
            {
                Console.WriteLine(ce.ToString());
            }
            finally
            {
                // Always unpin the Guid
                hGuid.Free();
            }
        }


        /// <summary>
        /// The callback method once the file is done playing.
        /// 
        /// Important:  This method is called from a non-UI thread.  Due to COM rules, the fgm, 
        /// its interfaces, filters, etc. are not really accessible from this thread.  One solution
        /// is to use this.Invoke in order to get execution back onto the UI thread (example below).
        /// The alternative approach is to have the thread(s) created in the MTA (multithreaded 
        /// apartment).  This is most easily accomplished by changing the STAThreaded attribute in
        /// Form.Main to use MTAThreaded.
        /// 
        /// Parameters are unused but available due to the paradigm
        /// </summary>
        private void FgmEvent(object sender, FgmEventMonitor.FgmEventArgs ea)
        {
            switch (ea.EventCode)
            {
                case 1: // EC_COMPLETE
                    {
                        // If MTAThread set on Form.Main, no need to Invoke
                        // FileFinished();

                        if (InvokeRequired)
                        {
                            // Manipulate all DShow and COM objects from UI thread
                            // Should always take this path during the callback
                            Invoke(new MethodInvoker(FileFinished), null);
                        }
                        else // STA && InvokeRequired
                        {
                            FileFinished();
                        }

                        break;
                    }

                // Besides EC_COMPLETE, these are the only other events I have seen fire

                case 0x0D: // EC_CLOCK_CHANGED 
                case 0x0E: // EC_PAUSED
                    break;

                default:
                    {
                        Console.WriteLine(Strings.EventCodeStatus, ea.EventCode, ea.Param1, ea.Param2);
                        break;
                    }
            }

        }


        /// <summary>
        /// Does the real work for FileFinished
        /// Matches the MethodInvoker delegate signature
        /// </summary>
        private void FileFinished()
        {
            if (ckRepeat.Checked)
            {
                PlayFile(file);
            }
            else
            {
                btnStop.PerformClick();
            }
        }


        /// <summary>
        /// This class monitors events on the fgm and raises the event to be handled elsewhere
        /// </summary>
        private class FgmEventMonitor
        {
            /// <summary>
            /// EventArgs of the FgmEvent
            /// 
            /// Important: Do not free these elsewhere.  Will be freed here after event is fired.
            /// </summary>
            public class FgmEventArgs : EventArgs
            {
                public int EventCode;
                public int Param1;
                public int Param2;

                public FgmEventArgs(int eventCode, int param1, int param2)
                {
                    EventCode = eventCode;
                    Param1 = param1;
                    Param2 = param2;
                }
            }


            /// <summary>
            /// Signature of FgmEvent, and its handler(s)
            /// </summary>
            public delegate void FgmEventHandler(object sender, FgmEventArgs ea);

            /// <summary>
            /// Event to indicate something interesting happened in the fgm
            /// 
            /// Important: Do not free parameters elsewhere.  Will be freed here after event is fired.
            /// </summary>
            public event FgmEventHandler FgmEvent;

            /// <summary>
            /// Interface for monitoring fgm events on
            /// </summary>
            private IMediaEvent iME = null;

            /// <summary>
            /// Thread used to monitor events
            /// </summary>
            private Thread eventThread = null;

            /// <summary>
            /// Constructor - provides the fgm to monitor
            /// </summary>
            public FgmEventMonitor(IMediaEvent iME)
            {
                this.iME = iME;

                // Kick off a thread to monitor events in the filtergraph
                eventThread = new Thread(new ThreadStart(Monitor));
                eventThread.IsBackground = true;
                eventThread.Name = "Fgm Event Monitor";
                eventThread.Start();
            }


            /// <summary>
            /// Used to clean up the internal thread
            /// </summary>
            public void Dispose()
            {
                eventThread.Abort();
            }


            /// <summary>
            /// Thread which monitors events and raises them
            /// </summary>
            private void Monitor()
            {
                int eventCode, param1, param2;

                while (true)
                {
                    try
                    {
                        // Check for an event
                        iME.GetEvent(out eventCode, out param1, out param2, -1);

                        // Raise the event if anyone is interested
                        if (FgmEvent != null)
                        {
                            FgmEvent(this, new FgmEventArgs(eventCode, param1, param2));
                        }

                        //ParseEvent(eventCode, param1, param2);
                        iME.FreeEventParams(eventCode, param1, param2);
                    }
                    catch (COMException e) // Timedout waiting for event
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
        }


        #endregion Private

        private void frmWMFile_Load(object sender, EventArgs e)
        {
            this.btnSelect.Font = UIFont.StringFont;
            this.ckRepeat.Font = UIFont.StringFont;
            this.lblInfo.Font = UIFont.StringFont;
            this.btnClose.Font = UIFont.StringFont;

            this.btnSelect.Text = Strings.SelectFile;
            this.ckRepeat.Text = Strings.Repeat;
            this.buttonToolTips.SetToolTip(this.btnPlay, Strings.Play);
            this.lblInfo.Text = Strings.ClickSelectFileToSpecify;
            this.buttonToolTips.SetToolTip(this.btnStop, Strings.Stop);
            this.btnClose.Text = Strings.Close;
            this.buttonToolTips.SetToolTip(this.btnPause, Strings.Pause);
            this.Text = Strings.WindowsMediaPlayback;
        }
    }
}
