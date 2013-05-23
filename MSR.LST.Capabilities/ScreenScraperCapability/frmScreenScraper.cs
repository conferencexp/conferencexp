using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization; // IScreenScraper
using System.Runtime.InteropServices;
using System.Windows.Forms;

using MSR.LST.MDShow;   // FilgraphManagerClass, IFilterGraph, IGraphBuilder, CaptureGraphBuilder2
using MSR.LST.MDShow.Filters;

using Win32Util;    // Win32Window


namespace MSR.LST.ConferenceXP
{
    public class frmScreenScraper  : CapabilityForm
    {
        #region Windows Form Designer generated code

        private System.ComponentModel.Container components = null;

        private System.Windows.Forms.HScrollBar hsbFrameRate;
        private System.Windows.Forms.CheckedListBox clbWindows;
        private System.Windows.Forms.Label lblFrameRate;
        private System.Windows.Forms.Button btnRefresh;

        public frmScreenScraper()
        {
            InitializeComponent();
        }
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.hsbFrameRate = new System.Windows.Forms.HScrollBar();
            this.lblFrameRate = new System.Windows.Forms.Label();
            this.clbWindows = new System.Windows.Forms.CheckedListBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // hsbFrameRate
            // 
            this.hsbFrameRate.LargeChange = 1;
            this.hsbFrameRate.Location = new System.Drawing.Point(8, 24);
            this.hsbFrameRate.Maximum = 30;
            this.hsbFrameRate.Minimum = 1;
            this.hsbFrameRate.Name = "hsbFrameRate";
            this.hsbFrameRate.Size = new System.Drawing.Size(184, 17);
            this.hsbFrameRate.TabIndex = 0;
            this.hsbFrameRate.Value = 5;
            this.hsbFrameRate.ValueChanged += new System.EventHandler(this.hsbFrameRate_ValueChanged);
            // 
            // lblFrameRate
            // 
            this.lblFrameRate.Location = new System.Drawing.Point(8, 8);
            this.lblFrameRate.Name = "lblFrameRate";
            this.lblFrameRate.Size = new System.Drawing.Size(184, 16);
            this.lblFrameRate.TabIndex = 1;
            this.lblFrameRate.Text = "Frame Rate (1 - 30): 5 / second";
            // 
            // clbWindows
            // 
            this.clbWindows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.clbWindows.HorizontalScrollbar = true;
            this.clbWindows.Location = new System.Drawing.Point(8, 56);
            this.clbWindows.Name = "clbWindows";
            this.clbWindows.Size = new System.Drawing.Size(336, 94);
            this.clbWindows.TabIndex = 2;
            this.clbWindows.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbWindows_ItemCheck);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(208, 18);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 23);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh List";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // frmScreenScraper
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(352, 158);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.clbWindows);
            this.Controls.Add(this.lblFrameRate);
            this.Controls.Add(this.hsbFrameRate);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Name = "frmScreenScraper";
            this.Text = "Screen Scraper - please select window";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmScreenScraper_Closing);
            this.Load += new System.EventHandler(this.frmScreenScraper_Load);
            this.ResumeLayout(false);

        }

        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (components != null) 
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #endregion

        #region Members

        ScreenScraperCapability ssc = null;

        FilgraphManagerClass fgm = null;
        IGraphBuilder iGB = null;
        ICaptureGraphBuilder2 iCGB2 = null;
        uint rotID = 0;

        IScreenScraper iSS = null;

        private _AMMediaType cMT;
        private VIDEOINFOHEADER cVI = new VIDEOINFOHEADER();
        private uint cBitRate = 1024 * 1024;
        private IPin cOutputPin;
        private IBaseFilter compressor;

        #endregion Members
                                                                                             
        #region Public

        public override void AddCapability(ICapability capability)
        {
            base.AddCapability (capability);

            if(ssc == null)
            {
                ssc = (ScreenScraperCapability)capability;
                BuildFilterGraph();
            }
        }

        public override bool RemoveCapability(ICapability capability)
        {
            bool ret = base.RemoveCapability (capability);

            if(ret)
            {
                ssc = null;

                fgm.Stop();
                FilterGraph.RemoveAllFilters(fgm);
                fgm = null;
            }

            return ret;
        }

        
        #endregion Public

        #region Private

        private void frmScreenScraper_Load(object sender, System.EventArgs e)
        {
            this.lblFrameRate.Text = Strings.FrameRateTitle;
            this.btnRefresh.Text = Strings.RefreshList;
            this.Text = Strings.SelectWindow;

            // Initialize list box with active windows
            btnRefresh_Click(this, null);

            // Initialize frame rate to default
            hsbFrameRate_ValueChanged(this, null);
        }

        private void frmScreenScraper_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(ssc != null)
            {
                ssc.StopSending();
            }
        }

        
        /// <summary>
        /// Refresh the list of available windows
        /// </summary>
        private void btnRefresh_Click(object sender, System.EventArgs e)
        {
            // Retrieve the current window, so we can reset it as selected, if it still exists
            Win32Window current = (Win32Window)clbWindows.SelectedItem;

            // Re-populate
            clbWindows.Items.Clear();

            clbWindows.Items.Add(Win32Window.DesktopWindow);

            foreach(Win32Window window in Win32Window.ApplicationWindows)
            {
                clbWindows.Items.Add(window); // Uses ToString override
            }
        }

        
        private void clbWindows_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            if(e.NewValue == CheckState.Checked)
            {
                iSS.Handle(((Win32Window)clbWindows.SelectedItem).Window);
                fgm.Run();
            }
            else
            {
                fgm.Stop();
            }
        }

        private void hsbFrameRate_ValueChanged(object sender, System.EventArgs e)
        {
            iSS.FrameRate(hsbFrameRate.Value);
            lblFrameRate.Text = string.Format(CultureInfo.CurrentCulture, Strings.FrameRateText, hsbFrameRate.Minimum, 
                hsbFrameRate.Maximum, hsbFrameRate.Value);
        }


        private void BuildFilterGraph()
        {
            InitializeFgm();

            // iCGB2.RenderStream fails if you try to connect source, compressor and renderer all at 
            // once because the compressor needs to have settled on the media type before then so...

            // Start with source filter - ScreenScraper
            // Compressor takes source filter and renders with it
            // Renderer takes compressor and renders with it
            AddRtpRenderer(AddCompressor(AddScreenScraper()));
        }

        private void InitializeFgm()
        {
            fgm = new FilgraphManagerClass();
            iGB = (IGraphBuilder)fgm;

            rotID = FilterGraph.AddToRot(iGB);

            iCGB2 = CaptureGraphBuilder2Class.CreateInstance();
            iCGB2.SetFiltergraph(iGB);
        }

        private IBaseFilter AddScreenScraper()
        {
            IBaseFilter screenScraper = MDShow.ScreenScraperClass.CreateInstance();
            iGB.AddFilter(screenScraper, "Screen Scraper");

            iSS = (IScreenScraper)screenScraper;
            iSS.FrameRate(hsbFrameRate.Value);

            return screenScraper;
        }

        private IBaseFilter AddCompressor(IBaseFilter source)
        {
            compressor = Filter.CreateBaseFilterByName("MSScreen 9 encoder DMO");
            iGB.AddFilter(compressor, "MSScreen 9 encoder DMO");

            iCGB2.RenderStream(IntPtr.Zero, IntPtr.Zero, source, null, compressor);

            // Get the output pin, Configure the Video Compressor
            cOutputPin = Filter.GetPin(compressor, _PinDirection.PINDIR_OUTPUT, Guid.Empty, 
                Guid.Empty, false, 0);

            InitializeCompressorMediaType();
            ConfigureCompressor();

            return compressor;
        }
        private void InitializeCompressorMediaType()
        {
            ArrayList mts = new ArrayList();
            ArrayList ihs = new ArrayList();
            ArrayList sccs = new ArrayList();

            Pin.GetStreamConfigCaps((IAMStreamConfig)cOutputPin, out mts, out ihs, out sccs);

            for(int i = 0; i < mts.Count; i++)
            {
                Console.WriteLine(MediaType.Dump((_AMMediaType)mts[i]));
                Console.WriteLine(Pin.DebugStreamConfigCaps(sccs[i]));
            }

            // There's only one
            cMT = (_AMMediaType)mts[0];
            cMT.formattype = MediaType.FormatType.FORMAT_VideoInfo;

            // MediaTypes are local to method, so free them all
            // then reallocate just the one we want
            for(int i = 0; i < mts.Count; i++)
            {
                _AMMediaType mt = (_AMMediaType)mts[i];
                MediaType.Free(ref mt);
            }

            cMT.cbFormat = (uint)Marshal.SizeOf(typeof(VIDEOINFOHEADER));
            cMT.pbFormat = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(VIDEOINFOHEADER)));
        }

        private void ConfigureCompressor()
        {
            // Have the compressor use the same height and width settings as the device

            // Because these are structs that access their members through properties
            // some of the properties (like BitmapInfo) are copied, so we work on the 
            // copy and then restore it at the end
            BITMAPINFOHEADER bmih = cVI.BitmapInfo;
            bmih.Width = 1600;
            bmih.Height = 1200;
            bmih.Size = (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER));
            bmih.Planes = 1;
            cVI.BitmapInfo = bmih;

            RECT r = cVI.Source;
            r.Right  = 1600;
            r.Bottom = 1200;
            cVI.Source = r;

            r = cVI.Target;
            r.Right  = 1600;
            r.Bottom = 1200;
            cVI.Target = r;

            // Configure the bit rate
            cVI.BitRate = cBitRate;

            // Update the structure in memory
            Marshal.StructureToPtr(cVI, cMT.pbFormat, false);

            // Allow compressor specific configuration
            // e.g. WM9+ requires extra configuration, others may as well
            ConfigureWMScreenEncoder();

            // Use the structure in the compressor
            IAMStreamConfig iSC = (IAMStreamConfig)cOutputPin;
            Console.WriteLine(MediaType.Dump(cMT));
            iSC.SetFormat(ref cMT);

            IAMVideoCompression iVC = (IAMVideoCompression)cOutputPin;
        }

        private void ConfigureWMScreenEncoder()
        {
            //
            // Set Private Data
            //
            IWMCodecPrivateData iPD = (IWMCodecPrivateData)compressor;
            iPD.SetPartialOutputType(ref cMT);

            uint cbData = 0;
            iPD.GetPrivateData(IntPtr.Zero, ref cbData);

            if(cbData != 0)
            {
                int vihSize = Marshal.SizeOf(cVI);

                // Allocate space for video info header + private data
                IntPtr vipd = Marshal.AllocCoTaskMem(vihSize + (int)cbData);

                // Copy vih into place
                Marshal.StructureToPtr(cVI, vipd, false);

                // Fill in private data
                iPD.GetPrivateData(new IntPtr(vipd.ToInt32() + vihSize), ref cbData);

                // Clean up current media type
                MediaType.Free(ref cMT);

                // Reset it
                cMT.pbFormat = vipd;
                cMT.cbFormat = (uint)vihSize + cbData;
            }
        }


        private IBaseFilter AddRtpRenderer(IBaseFilter compressor)
        {
            IBaseFilter rtpRenderer = MDShow.RtpRendererClass.CreateInstance();
            iGB.AddFilter(rtpRenderer, "Rtp Renderer");

            ((IRtpRenderer)rtpRenderer).Initialize(ssc.RtpSender);

            iCGB2.RenderStream(IntPtr.Zero, IntPtr.Zero, compressor, null, rtpRenderer);

            return rtpRenderer;
        }

        #endregion Private
    }
}
