using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization; // IScreenScraper
using System.Runtime.InteropServices;
using System.Windows.Forms;

using MSR.LST.MDShow;   // FilgraphManagerClass, IFilterGraph, IGraphBuilder, CaptureGraphBuilder2
using MSR.LST.MDShow.Filters;

using Win32Util;
using System.Configuration;    // Win32Window


namespace MSR.LST.ConferenceXP
{
    public class frmScreenScraperSimple : CapabilityForm
    {
        #region Windows Form Designer generated code
        
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblBitRate;
        private System.Windows.Forms.HScrollBar hsbBitRate;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Button btnClose;

        private System.ComponentModel.Container components = null;

        public frmScreenScraperSimple()
        {
            InitializeComponent();
        }


        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblBitRate = new System.Windows.Forms.Label();
            this.hsbBitRate = new System.Windows.Forms.HScrollBar();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblInfo.Location = new System.Drawing.Point(12, 12);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(400, 56);
            this.lblInfo.TabIndex = 0;
            // 
            // lblBitRate
            // 
            this.lblBitRate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBitRate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblBitRate.Location = new System.Drawing.Point(12, 34);
            this.lblBitRate.Name = "lblBitRate";
            this.lblBitRate.Size = new System.Drawing.Size(312, 16);
            this.lblBitRate.TabIndex = 1;
            this.lblBitRate.Visible = false;
            // 
            // hsbBitRate
            // 
            this.hsbBitRate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hsbBitRate.LargeChange = 100;
            this.hsbBitRate.Location = new System.Drawing.Point(12, 50);
            this.hsbBitRate.Maximum = 10099;
            this.hsbBitRate.Minimum = 10;
            this.hsbBitRate.Name = "hsbBitRate";
            this.hsbBitRate.Size = new System.Drawing.Size(312, 17);
            this.hsbBitRate.SmallChange = 10;
            this.hsbBitRate.TabIndex = 2;
            this.hsbBitRate.Value = 10;
            this.hsbBitRate.Visible = false;
            this.hsbBitRate.ValueChanged += new System.EventHandler(this.hsbBitRate_ValueChanged);
            // 
            // btnStartStop
            // 
            this.btnStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartStop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStartStop.Location = new System.Drawing.Point(256, 78);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(75, 23);
            this.btnStartStop.TabIndex = 3;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClose.Location = new System.Drawing.Point(340, 78);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmScreenScraperSimple
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(424, 112);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.hsbBitRate);
            this.Controls.Add(this.lblBitRate);
            this.Controls.Add(this.lblInfo);
            this.Font = UIFont.FormFont;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmScreenScraperSimple";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmScreenScraperSimple_Closing);
            this.Load += new System.EventHandler(this.frmScreenScraperSimple_Load);
            this.ResumeLayout(false);

        }

        
        #endregion

        #region Statics

        private const int FrameRateDefault = 1;
        private const string CompressorName = "MSScreen 9 encoder DMO";
        private const uint BitRateDefault = 250; // kilobits

        private static string InfoLabel = string.Format(CultureInfo.CurrentCulture, 
            Strings.ThisCapabilityInformation, FrameRateDefault, CompressorName);

        private readonly string BitRateLabel = Strings.CompressionBitRate;

        private readonly string Start = Strings.Start;
        private readonly string Stop = Strings.Stop;

        #endregion Statics

        #region Members

        private int frameRate = FrameRateDefault;
        private uint bitRate = BitRateDefault;

        ScreenScraperCapability ssc = null;

        FilgraphManagerClass fgm = null;
        IGraphBuilder iGB = null;
        ICaptureGraphBuilder2 iCGB2 = null;
        uint rotID = 0;

        IScreenScraper iSS = null;

        private IBaseFilter compressor;
        private IPin cOutputPin;
        private _AMMediaType cMT;
        private VIDEOINFOHEADER cVI = new VIDEOINFOHEADER();

        #endregion Members
                                                                                             
        #region Public

        public override void AddCapability(ICapability capability)
        {
            base.AddCapability (capability);

            if(ssc == null)
            {
                ssc = (ScreenScraperCapability)capability;
            }
        }

        public override bool RemoveCapability(ICapability capability)
        {
            bool ret = base.RemoveCapability (capability);

            if(ret)
            {
                ssc = null;

                if(fgm != null)
                {
                    fgm.Stop();
                    FilterGraph.RemoveAllFilters(fgm);
                    fgm = null;
                }
            }

            return ret;
        }

        
        #endregion Public

        private void frmScreenScraperSimple_Load(object sender, System.EventArgs e)
        {
            //Configurable settings
            //Frame rate
            string setting;
            if ((setting = ConfigurationManager.AppSettings[AppConfig.ScreenScraperFrameRate]) != null) {
                int fr;
                if (Int32.TryParse(setting, out fr)) {
                    if (fr > 0) {
                        frameRate = fr;
                        InfoLabel = string.Format(CultureInfo.CurrentCulture,
                            Strings.ThisCapabilityInformation, frameRate, CompressorName);
                    }
                }
            }

            //Bit Rate
            if ((setting = ConfigurationManager.AppSettings[AppConfig.ScreenScraperBitRate]) != null) {
                uint br;
                if (uint.TryParse(setting, out br)) {
                    if (br > 0) {
                        bitRate = br;
                    }
                }
            }

            this.lblInfo.Font = UIFont.StringFont;
            this.lblBitRate.Font = UIFont.StringFont;
            this.btnStartStop.Font = UIFont.StringFont;
            this.btnClose.Font = UIFont.StringFont;

            this.btnClose.Text = Strings.Close;

            // Static text
            lblInfo.Text = InfoLabel;
            lblBitRate.Text = BitRateLabel;
            btnStartStop.Text = Start;

            // Default values
            hsbBitRate.Value = (int)bitRate;
        }

        private void frmScreenScraperSimple_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(ssc != null)
            {
                ssc.StopSending();
            }
        }

        private void hsbBitRate_ValueChanged(object sender, System.EventArgs e)
        {
            lblBitRate.Text = BitRateLabel + hsbBitRate.Value;
        }
        private void btnStartStop_Click(object sender, System.EventArgs e)
        {
            try
            {
                if(btnStartStop.Text == Start)
                {
                    if(fgm == null)
                    {
                        BuildFilterGraph();

                        // Changing the bit rate on the fly hasn't been tested yet
                        hsbBitRate.Enabled = false;
                    }

                    fgm.Run();
                    btnStartStop.Text = Stop;
                }
                else
                {
                    // Pause, don't Stop, otherwise the media clock needs to be reset on the source
                    // filter, and that isn't implemented yet.  jasonv 3/29/2005
                    fgm.Pause();
                    btnStartStop.Text = Start;
                }
            }
            catch(Exception ex)
            {
                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, Strings.DataStreamError, 
                    ex.ToString()), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            Close();
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
            iSS.FrameRate(frameRate);
            iSS.Handle(Win32Window.DesktopWindow.Window);

            return screenScraper;
        }

        private IBaseFilter AddCompressor(IBaseFilter source)
        {
            compressor = Filter.CreateBaseFilterByName(CompressorName);
            iGB.AddFilter(compressor, CompressorName);

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

            cMT.cbFormat = (uint)Marshal.SizeOf(cVI);
            cMT.pbFormat = Marshal.AllocCoTaskMem((int)cMT.cbFormat);
        }

        private void ConfigureCompressor()
        {
            // Have the compressor use the same height and width settings as the device

            // Because these are structs that access their members through properties
            // some of the properties (like BitmapInfo) are copied, so we work on the 
            // copy and then restore it at the end
            BITMAPINFOHEADER bmih = cVI.BitmapInfo;
            bmih.Width = SystemInformation.PrimaryMonitorSize.Width;
            bmih.Height = SystemInformation.PrimaryMonitorSize.Height;
            bmih.Size = (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER));
            bmih.Planes = 1;
            cVI.BitmapInfo = bmih;

            // Configure the bit rate
            cVI.BitRate = (uint)(hsbBitRate.Value * 1024); // Change to kilobits

            // Update the structure in memory
            Marshal.StructureToPtr(cVI, cMT.pbFormat, false);

            // Allow compressor specific configuration
            // e.g. WM9+ requires extra configuration, others may as well
            ConfigureWMScreenEncoder();
            Console.WriteLine(MediaType.Dump(cMT));

            // Use the structure in the compressor
            IAMStreamConfig iSC = (IAMStreamConfig)cOutputPin;
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

    }
}
