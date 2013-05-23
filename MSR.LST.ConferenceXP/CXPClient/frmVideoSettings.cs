using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using MSR.LST.MDShow;


namespace MSR.LST.ConferenceXP
{
    public class frmVideoSettings : System.Windows.Forms.Form
    {
        #region Windows Form Designer generated code

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox ckEnableCompression;
        private System.Windows.Forms.Label lblBitRate;
        private System.Windows.Forms.HScrollBar hsbBitRate;
        private System.Windows.Forms.Label lblKeyFrameRate;
        private System.Windows.Forms.HScrollBar hsbKeyFrameRate;
        private System.Windows.Forms.Button btnCamera;
        private System.Windows.Forms.Button btnVideo;
        private System.Windows.Forms.GroupBox gbCustomCompression;
        private System.Windows.Forms.GroupBox gbCurrentSettings;
        private System.Windows.Forms.Label lblCurrentSettings;
        private System.Windows.Forms.ComboBox cboUpstreamPropPages;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.Label lblMoreConfig;
        private System.Windows.Forms.GroupBox gbCameraAndVideo;
        private CheckBox ckDisableVideoFec;

        private System.ComponentModel.Container components = null;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.btnOK = new System.Windows.Forms.Button();
            this.cboUpstreamPropPages = new System.Windows.Forms.ComboBox();
            this.btnCamera = new System.Windows.Forms.Button();
            this.btnVideo = new System.Windows.Forms.Button();
            this.gbCurrentSettings = new System.Windows.Forms.GroupBox();
            this.lblCurrentSettings = new System.Windows.Forms.Label();
            this.gbCustomCompression = new System.Windows.Forms.GroupBox();
            this.btnDefault = new System.Windows.Forms.Button();
            this.lblBitRate = new System.Windows.Forms.Label();
            this.hsbBitRate = new System.Windows.Forms.HScrollBar();
            this.lblKeyFrameRate = new System.Windows.Forms.Label();
            this.hsbKeyFrameRate = new System.Windows.Forms.HScrollBar();
            this.btnApply = new System.Windows.Forms.Button();
            this.ckEnableCompression = new System.Windows.Forms.CheckBox();
            this.lblMoreConfig = new System.Windows.Forms.Label();
            this.gbCameraAndVideo = new System.Windows.Forms.GroupBox();
            this.ckDisableVideoFec = new System.Windows.Forms.CheckBox();
            this.gbCurrentSettings.SuspendLayout();
            this.gbCustomCompression.SuspendLayout();
            this.gbCameraAndVideo.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(424, 265);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 23);
            this.btnOK.TabIndex = 42;
            this.btnOK.Text = "Close";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cboUpstreamPropPages
            // 
            this.cboUpstreamPropPages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUpstreamPropPages.Enabled = false;
            this.cboUpstreamPropPages.Location = new System.Drawing.Point(8, 70);
            this.cboUpstreamPropPages.Name = "cboUpstreamPropPages";
            this.cboUpstreamPropPages.Size = new System.Drawing.Size(272, 21);
            this.cboUpstreamPropPages.TabIndex = 64;
            this.cboUpstreamPropPages.SelectedIndexChanged += new System.EventHandler(this.cboUpstreamPropPages_SelectedIndexChanged);
            // 
            // btnCamera
            // 
            this.btnCamera.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCamera.Location = new System.Drawing.Point(8, 20);
            this.btnCamera.Name = "btnCamera";
            this.btnCamera.Size = new System.Drawing.Size(120, 24);
            this.btnCamera.TabIndex = 65;
            this.btnCamera.Text = "Camera...";
            this.btnCamera.Click += new System.EventHandler(this.btnCamera_Click);
            // 
            // btnVideo
            // 
            this.btnVideo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnVideo.Location = new System.Drawing.Point(152, 20);
            this.btnVideo.Name = "btnVideo";
            this.btnVideo.Size = new System.Drawing.Size(128, 24);
            this.btnVideo.TabIndex = 66;
            this.btnVideo.Text = "Video Format...";
            this.btnVideo.Click += new System.EventHandler(this.btnVideo_Click);
            // 
            // gbCurrentSettings
            // 
            this.gbCurrentSettings.Controls.Add(this.lblCurrentSettings);
            this.gbCurrentSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbCurrentSettings.Location = new System.Drawing.Point(304, 8);
            this.gbCurrentSettings.Name = "gbCurrentSettings";
            this.gbCurrentSettings.Size = new System.Drawing.Size(216, 255);
            this.gbCurrentSettings.TabIndex = 68;
            this.gbCurrentSettings.TabStop = false;
            this.gbCurrentSettings.Text = "Current Settings";
            // 
            // lblCurrentSettings
            // 
            this.lblCurrentSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurrentSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblCurrentSettings.Location = new System.Drawing.Point(3, 16);
            this.lblCurrentSettings.Name = "lblCurrentSettings";
            this.lblCurrentSettings.Size = new System.Drawing.Size(210, 236);
            this.lblCurrentSettings.TabIndex = 63;
            this.lblCurrentSettings.Text = "Current Settings";
            // 
            // gbCustomCompression
            // 
            this.gbCustomCompression.Controls.Add(this.btnDefault);
            this.gbCustomCompression.Controls.Add(this.lblBitRate);
            this.gbCustomCompression.Controls.Add(this.hsbBitRate);
            this.gbCustomCompression.Controls.Add(this.lblKeyFrameRate);
            this.gbCustomCompression.Controls.Add(this.hsbKeyFrameRate);
            this.gbCustomCompression.Controls.Add(this.btnApply);
            this.gbCustomCompression.Enabled = false;
            this.gbCustomCompression.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbCustomCompression.Location = new System.Drawing.Point(8, 112);
            this.gbCustomCompression.Name = "gbCustomCompression";
            this.gbCustomCompression.Size = new System.Drawing.Size(288, 151);
            this.gbCustomCompression.TabIndex = 70;
            this.gbCustomCompression.TabStop = false;
            // 
            // btnDefault
            // 
            this.btnDefault.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDefault.Location = new System.Drawing.Point(11, 119);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(104, 23);
            this.btnDefault.TabIndex = 83;
            this.btnDefault.Text = "Restore Defaults";
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // lblBitRate
            // 
            this.lblBitRate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblBitRate.Location = new System.Drawing.Point(8, 69);
            this.lblBitRate.Name = "lblBitRate";
            this.lblBitRate.Size = new System.Drawing.Size(240, 16);
            this.lblBitRate.TabIndex = 68;
            this.lblBitRate.Text = "Bit Rate";
            // 
            // hsbBitRate
            // 
            this.hsbBitRate.LargeChange = 1;
            this.hsbBitRate.Location = new System.Drawing.Point(8, 85);
            this.hsbBitRate.Maximum = 1000;
            this.hsbBitRate.Minimum = 1;
            this.hsbBitRate.Name = "hsbBitRate";
            this.hsbBitRate.Size = new System.Drawing.Size(272, 17);
            this.hsbBitRate.TabIndex = 69;
            this.hsbBitRate.TabStop = true;
            this.hsbBitRate.Value = 1;
            this.hsbBitRate.ValueChanged += new System.EventHandler(this.hsbBitRate_ValueChanged);
            // 
            // lblKeyFrameRate
            // 
            this.lblKeyFrameRate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblKeyFrameRate.Location = new System.Drawing.Point(8, 29);
            this.lblKeyFrameRate.Name = "lblKeyFrameRate";
            this.lblKeyFrameRate.Size = new System.Drawing.Size(248, 16);
            this.lblKeyFrameRate.TabIndex = 66;
            this.lblKeyFrameRate.Text = "Key frame rate";
            // 
            // hsbKeyFrameRate
            // 
            this.hsbKeyFrameRate.LargeChange = 1;
            this.hsbKeyFrameRate.Location = new System.Drawing.Point(8, 45);
            this.hsbKeyFrameRate.Maximum = 8;
            this.hsbKeyFrameRate.Minimum = 1;
            this.hsbKeyFrameRate.Name = "hsbKeyFrameRate";
            this.hsbKeyFrameRate.Size = new System.Drawing.Size(272, 16);
            this.hsbKeyFrameRate.TabIndex = 67;
            this.hsbKeyFrameRate.Value = 1;
            this.hsbKeyFrameRate.ValueChanged += new System.EventHandler(this.hsbKeyFrameRate_ValueChanged);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnApply.Location = new System.Drawing.Point(184, 119);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(95, 23);
            this.btnApply.TabIndex = 82;
            this.btnApply.Text = "Apply";
            this.btnApply.Click += new System.EventHandler(this.btnApplyCompression_Click);
            // 
            // ckEnableCompression
            // 
            this.ckEnableCompression.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ckEnableCompression.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ckEnableCompression.Location = new System.Drawing.Point(16, 120);
            this.ckEnableCompression.Name = "ckEnableCompression";
            this.ckEnableCompression.Size = new System.Drawing.Size(217, 18);
            this.ckEnableCompression.TabIndex = 64;
            this.ckEnableCompression.Text = "Enable Video Compression";
            this.ckEnableCompression.CheckedChanged += new System.EventHandler(this.ckEnableCompression_CheckedChanged);
            // 
            // lblMoreConfig
            // 
            this.lblMoreConfig.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblMoreConfig.Location = new System.Drawing.Point(8, 54);
            this.lblMoreConfig.Name = "lblMoreConfig";
            this.lblMoreConfig.Size = new System.Drawing.Size(272, 16);
            this.lblMoreConfig.TabIndex = 80;
            this.lblMoreConfig.Text = "Additional camera configuration options (if available)";
            // 
            // gbCameraAndVideo
            // 
            this.gbCameraAndVideo.Controls.Add(this.lblMoreConfig);
            this.gbCameraAndVideo.Controls.Add(this.btnVideo);
            this.gbCameraAndVideo.Controls.Add(this.btnCamera);
            this.gbCameraAndVideo.Controls.Add(this.cboUpstreamPropPages);
            this.gbCameraAndVideo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbCameraAndVideo.Location = new System.Drawing.Point(8, 10);
            this.gbCameraAndVideo.Name = "gbCameraAndVideo";
            this.gbCameraAndVideo.Size = new System.Drawing.Size(288, 100);
            this.gbCameraAndVideo.TabIndex = 82;
            this.gbCameraAndVideo.TabStop = false;
            this.gbCameraAndVideo.Text = "Configure Camera and Video Stream";
            // 
            // ckDisableVideoFec
            // 
            this.ckDisableVideoFec.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ckDisableVideoFec.Location = new System.Drawing.Point(8, 269);
            this.ckDisableVideoFec.Name = "ckDisableVideoFec";
            this.ckDisableVideoFec.Size = new System.Drawing.Size(422, 21);
            this.ckDisableVideoFec.TabIndex = 83;
            this.ckDisableVideoFec.Text = "Disable Forward Error Correction (Recommended for Uncompressed Video)";
            this.ckDisableVideoFec.CheckedChanged += new System.EventHandler(this.ckDisableVideoFec_CheckedChanged);
            // 
            // frmVideoSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(530, 297);
            this.ControlBox = false;
            this.Controls.Add(this.gbCameraAndVideo);
            this.Controls.Add(this.ckEnableCompression);
            this.Controls.Add(this.gbCustomCompression);
            this.Controls.Add(this.gbCurrentSettings);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.ckDisableVideoFec);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmVideoSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Advanced Video Settings";
            this.Load += new System.EventHandler(this.frmVideoSettings_Load);
            this.gbCurrentSettings.ResumeLayout(false);
            this.gbCustomCompression.ResumeLayout(false);
            this.gbCameraAndVideo.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        
        #endregion

        #region Statics

        private const string TAB = "     ";

        private static int BitRateMultiplier = 10;

        /// <summary>
        /// The maximum extent of the video bitrate slider in Kbps.
        /// </summary>
        public static int MaximumVideoBitRate {
            set {
                if (value >= 1000) {
                    BitRateMultiplier = value / 1000;
                }
            }
            get {
                return BitRateMultiplier * 1000;
            }
        }

        #endregion Statics

        #region Members

        private VideoCapability vc;
        private FilterInfo fi;
        private frmAVDevices frmAV;

        /// <summary>
        /// If the capability was created when the form was constructed, this flag
        /// indicates that we also need to dispose it on close.
        /// </summary>
        private bool disposeCapability = true;

        #endregion Members

        #region Constructor

        public frmVideoSettings(FilterInfo fi, frmAVDevices frmAV)
        {
            InitializeComponent();

            Debug.Assert(frmAV != null);

            this.fi = fi;
            this.frmAV = frmAV;

            if (!frmAV.videoCapabilities.ContainsKey(fi)) {
                vc = new VideoCapability(fi);
                vc.SetLogger(new AVLogger(Log));
                vc.ActivateCamera();
                disposeCapability = true;
            }
            else {
                vc = frmAV.videoCapabilities[fi];
                disposeCapability = false;
            }
        }

        #endregion Constructor

        #region Load

        private void frmVideoSettings_Load(object sender, System.EventArgs e)
        {
            this.btnOK.Font = UIFont.StringFont;
            this.gbCurrentSettings.Font = UIFont.StringFont;
            this.gbCustomCompression.Font = UIFont.StringFont;
            this.ckEnableCompression.Font = UIFont.StringFont;
            this.gbCameraAndVideo.Font = UIFont.StringFont;
            this.ckDisableVideoFec.Font = UIFont.StringFont;

            this.btnOK.Text = Strings.Close;
            this.btnCamera.Text = Strings.CameraEllipsis;
            this.btnVideo.Text = Strings.VideoFormat;
            this.gbCurrentSettings.Text = Strings.CurrentSettings;
            this.lblCurrentSettings.Text = Strings.CurrentSettings;
            this.btnDefault.Text = Strings.RestoreDefaults;
            this.lblBitRate.Text = Strings.BitRateLabel;
            this.lblKeyFrameRate.Text = Strings.KeyFrameRateLabel;
            this.btnApply.Text = Strings.Apply;
            this.ckEnableCompression.Text = Strings.EnableVideoCompression;
            this.lblMoreConfig.Text = Strings.AdditionalCameraConfiguration;
            this.gbCameraAndVideo.Text = Strings.ConfigureCameraAndVideo;
            this.ckDisableVideoFec.Text = Strings.DisableVideoForwardErrorCorrection;

            InitializeUI();
        }

        private void InitializeUI()
        {
            try
            {
                this.Text = string.Format(CultureInfo.CurrentCulture, Strings.AdvancedVideoSettings,
                    vc.CaptureGraph.Source.FriendlyName);

                IBaseFilter[] upstreamFilters = vc.CaptureGraph.Source.UpstreamFilters;
                if (upstreamFilters != null && upstreamFilters.Length > 0) {
                    ArrayList propPages = new ArrayList();
                    foreach (IBaseFilter iBF in upstreamFilters) {
                        if (Filter.HasDialog(iBF)) {
                            propPages.Add(new PropertyPage((ISpecifyPropertyPages)iBF,
                                Filter.Name(iBF)));
                        }
                    }

                    // Populate the property page configuration combo box
                    cboUpstreamPropPages.DataSource = propPages;
                    cboUpstreamPropPages.DisplayMember = Strings.Name;
                    cboUpstreamPropPages.SelectedIndex = 0;

                    cboUpstreamPropPages.Enabled = true;
                }

                // Enable the appropriate buttons
                btnCamera.Enabled = vc.CaptureGraph.Source.HasSourceDialog;
                btnVideo.Enabled = vc.CaptureGraph.Source.HasFormatDialog;

                // Update compressor settings
                ckEnableCompression.Enabled = false;
                ckEnableCompression.Checked = vc.RegVideoCompressorEnabled;
                ckEnableCompression.Enabled = true;

                ckDisableVideoFec.Checked = !FMain.EnableVideoFec;

                // Get these setup correctly whether there is a compressor or not
                UpdateKeyFrameRateLabel();
                UpdateBitRateLabel();

                UpdateCurrentSettings();
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

        #endregion Load

        #region UI Events

        /// <summary>
        /// Show the Video Source Filter's dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCamera_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (vc.CaptureGraph.Source.HasSourceDialog) {
                    // Vfw is not as flexible as WDM, need to stop the graph first
                    bool isVfw = false;
                    if (vc.CaptureGraph.Source is VideoSource) {
                        isVfw = ((VideoSource)(vc.CaptureGraph.Source)).IsVfw;
                    }
                    if (isVfw) {
                        frmAV.RenderAndRunVideo(vc.CaptureGraph, false);
                        vc.CaptureGraph.RemoveFiltersDownstreamFrom(vc.CaptureGraph.Source);
                    }

                    vc.CaptureGraph.Source.ShowSourceDialog(this.Handle);

                    if (isVfw) {
                        vc.AddVideoCompressor();
                        frmAV.RenderAndRunVideo(vc.CaptureGraph);
                    }

                    vc.SaveCameraSettings();
                    UpdateCurrentSettings();
                }
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

        /// <summary>
        /// Show Video source filter format dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVideo_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (vc.CaptureGraph.Source.HasFormatDialog) {
                    frmAV.RenderAndRunVideo(vc.CaptureGraph, false);
                    vc.CaptureGraph.RemoveFiltersDownstreamFromSource(MSR.LST.Net.Rtp.PayloadType.dynamicVideo);

                    vc.CaptureGraph.Source.ShowFormatDialog(this.Handle);

                    vc.AddVideoCompressor();
                    frmAV.RenderAndRunVideo(vc.CaptureGraph);

                    vc.SaveVideoSettings();
                    UpdateCurrentSettings();
                }
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

        private void cboUpstreamPropPages_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (cboUpstreamPropPages.Enabled) // Flag for programmatic vs. user input
                {
                    PropertyPage pp = (PropertyPage)cboUpstreamPropPages.SelectedItem;
                    pp.Show(this.Handle);

                    // To save Crossbar info
                    vc.SaveCameraSettings();
                    UpdateCurrentSettings();
                }
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
        
        private void ckEnableCompression_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                gbCustomCompression.Enabled = ckEnableCompression.Checked;

                if (ckEnableCompression.Enabled) // Flag for programmatic / user
                {
                    frmAV.RenderAndRunVideo(vc.CaptureGraph, false);

                    // Must come before vc.AddVideoCompressor
                    vc.RegVideoCompressorEnabled = ckEnableCompression.Checked;

                    if (ckEnableCompression.Checked) {
                        vc.AddVideoCompressor();
                    }
                    else {
                        vc.CaptureGraph.RemoveCompressor(MSR.LST.Net.Rtp.PayloadType.dynamicVideo);
                    }

                    frmAV.RenderAndRunVideo(vc.CaptureGraph);

                    UpdateCurrentSettings();
                }
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

        private void hsbBitRate_ValueChanged(object sender, System.EventArgs e)
        {
            UpdateBitRateLabel();
        }

        private void hsbKeyFrameRate_ValueChanged(object sender, System.EventArgs e)
        {
            UpdateKeyFrameRateLabel();
        }

        /// <summary>
        /// Restore default settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDefault_Click(object sender, System.EventArgs e)
        {
            try
            {
                frmAV.RenderAndRunVideo(vc.CaptureGraph, false);

                vc.RegCustomCompression = false;
                vc.AddVideoCompressor();

                frmAV.RenderAndRunVideo(vc.CaptureGraph);

                UpdateCurrentSettings();
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

        private void btnApplyCompression_Click(object sender, System.EventArgs e)
        {
            try
            {
                frmAV.RenderAndRunVideo(vc.CaptureGraph, false);

                // Create the custom settings and set them
                VideoCompressorQualityInfo vcqi = VideoCompressor.DefaultQualityInfo;
                vcqi.BitRate = (uint)BitRateValue;
                vcqi.KeyFrameRate = KeyFrameRateValue;
                ((IVideoCaptureGraph)vc.CaptureGraph).VideoCompressor.QualityInfo = vcqi;

                frmAV.RenderAndRunVideo(vc.CaptureGraph);

                // Save them
                vc.SaveVideoCompressorSettings();
                vc.RegCustomCompression = true;

                UpdateCurrentSettings();
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

        private void ckDisableVideoFec_CheckedChanged(object sender, EventArgs e) {
            FMain.EnableVideoFec = !ckDisableVideoFec.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e) {
            updateOnClose();
        }

        #endregion UI Events

        #region Private
        
        private void updateOnClose() {
            frmAV.UpdateVideoBox(vc.CaptureGraph);
            if ((disposeCapability) && (vc != null)) {
                vc.Dispose();
                vc = null;
                disposeCapability = false;
            }
        }

        private void UpdateCurrentSettings()
        {
            lblCurrentSettings.Text = null;

            UpdateCustomCompressionFields();

            UpdateCameraSettings();
            UpdateVideoSettings();
            UpdateCompressorSettings();
        }

        private void UpdateCameraSettings()
        {
            if (vc.CaptureGraph.Source is VideoSource) {
                VideoSource vs = ((VideoSource)vc.CaptureGraph.Source);
                if (vs.HasVideoStandards || vs.HasPhysConns) {
                    lblCurrentSettings.Text = Strings.Camera;

                    if (vs.HasVideoStandards) {
                        lblCurrentSettings.Text += "\r\n" + string.Format(CultureInfo.InvariantCulture,
                            Strings.VideoStandard, TAB, vs.CurrentVideoStandard);
                    }

                    if (vs.HasPhysConns) {
                        lblCurrentSettings.Text += "\r\n" + string.Format(CultureInfo.InvariantCulture,
                            Strings.VideoSource, TAB, vs.CurrentPhysicalConnector);
                    }
                }
            }
            else if (vc.CaptureGraph is DVCaptureGraph) { 
                lblCurrentSettings.Text += "\r\n" + string.Format(CultureInfo.InvariantCulture,
                            Strings.VideoStandard, TAB, "DV");
            }
        }

        private void UpdateVideoSettings()
        {
            _AMMediaType mt;
            object formatBlock;
            vc.CaptureGraph.Source.GetMediaType(out mt, out formatBlock);

            VIDEOINFOHEADER vih = new VIDEOINFOHEADER();
            bool unknownFormat = false;

            if (formatBlock is VIDEOINFOHEADER) {
                vih = (VIDEOINFOHEADER)formatBlock;
            }
            else if (formatBlock is DVINFO) { 
                DVCaptureGraph dvcg = vc.CaptureGraph as DVCaptureGraph;
                if (dvcg != null) {
                    dvcg.GetVideoMediaType(out mt, out formatBlock);
                    if (formatBlock is VIDEOINFOHEADER) {
                        vih = (VIDEOINFOHEADER)formatBlock;
                    }
                    else {
                        unknownFormat = true;
                    }
                }
                else {
                    unknownFormat = true;
                }
            }
            else {
                unknownFormat = true;
            }

            if (unknownFormat) {
                lblCurrentSettings.Text += "Unknown Video Format";
                return;
            }

            BITMAPINFOHEADER bmih = vih.BitmapInfo;

            if (lblCurrentSettings.Text == String.Empty) {
                lblCurrentSettings.Text = Strings.VideoStream;
            }
            else {
                lblCurrentSettings.Text += "\r\n\r\n" + Strings.VideoStream;
            }

            lblCurrentSettings.Text += string.Format(CultureInfo.CurrentCulture, "\r\n" +
                Strings.AdvancedVideoSettingsStatus, TAB, bmih.Width, bmih.Height,
                vih.FrameRate.ToString("F2", CultureInfo.InvariantCulture), MediaType.SubType.GuidToString(mt.subtype),
                bmih.BitCount, (uint)(bmih.Width * bmih.Height * bmih.BitCount * vih.FrameRate) / 1000);
        }

        private void UpdateCompressorSettings()
        {
            if (((IVideoCaptureGraph)vc.CaptureGraph).VideoCompressor == null) {
                lblCurrentSettings.Text += string.Format(CultureInfo.CurrentCulture, "\r\n\r\n" +
                    Strings.CompressorDisabled2, TAB);
            }
            else {
                VideoCompressor vcomp = ((IVideoCaptureGraph)vc.CaptureGraph).VideoCompressor;
                VideoCompressorQualityInfo vcqi = vcomp.QualityInfo;

                lblCurrentSettings.Text += string.Format(CultureInfo.CurrentCulture, "\r\n\r\n" +
                    Strings.CompressorStatus, TAB, vcomp.FriendlyName,
                    MediaType.SubType.GuidToString(vcqi.MediaSubType), vcqi.BitRate / 1000, KeyFrameRateString);
            }
        }

        private void UpdateCustomCompressionFields()
        {
            if (((IVideoCaptureGraph)vc.CaptureGraph).VideoCompressor != null) {
                VideoCompressorQualityInfo vcqi = ((IVideoCaptureGraph)vc.CaptureGraph).VideoCompressor.QualityInfo;
                BitRateValue = (int)vcqi.BitRate;
                KeyFrameRateValue = vcqi.KeyFrameRate;
            }
        }

        private void UpdateKeyFrameRateLabel()
        {
            lblKeyFrameRate.Text = KeyFrameRateString;
        }
        private string KeyFrameRateString
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture, Strings.KeyFrameRate, hsbKeyFrameRate.Value);
            }
        }

        private void UpdateBitRateLabel()
        {
            lblBitRate.Text = string.Format(CultureInfo.CurrentCulture, Strings.BitRateKbps, BitRateValue / 1000);
        }
        

        private int BitRateValue
        {
            get
            {
                return hsbBitRate.Value * BitRateMultiplier * 1000;
            }

            set
            {
                hsbBitRate.Value = Math.Max(hsbBitRate.Minimum,Math.Min(
                    (int)(value / (BitRateMultiplier * 1000)), 
                    hsbBitRate.Maximum));
            }
        }

        private int KeyFrameRateValue
        {
            get
            {
                return hsbKeyFrameRate.Value * 1000;
            }

            set
            {
                hsbKeyFrameRate.Value = value / 1000;
            }
        }

        private void Log(string msg)
        {
            frmAV.Log(msg);
        }

        #endregion Private

    }
}
