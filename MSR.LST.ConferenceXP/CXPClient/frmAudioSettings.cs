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
    public class frmAudioSettings : System.Windows.Forms.Form
    {
        /// Notes: 
        /// * Do not set the audio pin combo box to sorted.  We use the index
        /// not the name, to determine which pin was selected.
        /// * We assume there is no frmAV AudioCapability when the form is
        /// constructed.  We always create our own capability, and dispose it
        /// when we close.  This form also has a Test button.

        #region Windows Form Designer generated code

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblInputSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAudio;
        private System.Windows.Forms.ComboBox cboMicrophonePins;
        private System.Windows.Forms.CheckBox ckUncompressedAudio;
        private System.Windows.Forms.ComboBox cboLinkedCamera;
        private System.Windows.Forms.GroupBox gbCurrentSettings;
        private System.Windows.Forms.Label lblCurrentSettings;
        private System.Windows.Forms.GroupBox gbMicAndAudio;
        private CheckBox ckDisableFec;
        private ComboBox cbBufferSize;
        private ComboBox cbBufferCount;
        private Label lblBufferSize;
        private Label lblBufferCount;
        private CheckBox ckTestAudio;
        private Label label2;
        private ComboBox cbCompressionFormat;

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
            this.gbCurrentSettings = new System.Windows.Forms.GroupBox();
            this.lblCurrentSettings = new System.Windows.Forms.Label();
            this.cboMicrophonePins = new System.Windows.Forms.ComboBox();
            this.lblInputSource = new System.Windows.Forms.Label();
            this.btnAudio = new System.Windows.Forms.Button();
            this.ckUncompressedAudio = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboLinkedCamera = new System.Windows.Forms.ComboBox();
            this.gbMicAndAudio = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbCompressionFormat = new System.Windows.Forms.ComboBox();
            this.ckDisableFec = new System.Windows.Forms.CheckBox();
            this.cbBufferSize = new System.Windows.Forms.ComboBox();
            this.cbBufferCount = new System.Windows.Forms.ComboBox();
            this.lblBufferSize = new System.Windows.Forms.Label();
            this.lblBufferCount = new System.Windows.Forms.Label();
            this.ckTestAudio = new System.Windows.Forms.CheckBox();
            this.gbCurrentSettings.SuspendLayout();
            this.gbMicAndAudio.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(437, 280);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 43;
            this.btnOK.Text = "Close";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbCurrentSettings
            // 
            this.gbCurrentSettings.Controls.Add(this.lblCurrentSettings);
            this.gbCurrentSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbCurrentSettings.Location = new System.Drawing.Point(304, 6);
            this.gbCurrentSettings.Name = "gbCurrentSettings";
            this.gbCurrentSettings.Size = new System.Drawing.Size(216, 112);
            this.gbCurrentSettings.TabIndex = 76;
            this.gbCurrentSettings.TabStop = false;
            this.gbCurrentSettings.Text = "Current Settings";
            // 
            // lblCurrentSettings
            // 
            this.lblCurrentSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurrentSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblCurrentSettings.Location = new System.Drawing.Point(3, 16);
            this.lblCurrentSettings.Name = "lblCurrentSettings";
            this.lblCurrentSettings.Size = new System.Drawing.Size(210, 93);
            this.lblCurrentSettings.TabIndex = 23;
            this.lblCurrentSettings.Text = "Current Settings";
            // 
            // cboMicrophonePins
            // 
            this.cboMicrophonePins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMicrophonePins.Location = new System.Drawing.Point(8, 36);
            this.cboMicrophonePins.Name = "cboMicrophonePins";
            this.cboMicrophonePins.Size = new System.Drawing.Size(272, 21);
            this.cboMicrophonePins.TabIndex = 64;
            this.cboMicrophonePins.SelectedIndexChanged += new System.EventHandler(this.cboMicrophonePins_SelectedIndexChanged);
            // 
            // lblInputSource
            // 
            this.lblInputSource.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblInputSource.Location = new System.Drawing.Point(8, 20);
            this.lblInputSource.Name = "lblInputSource";
            this.lblInputSource.Size = new System.Drawing.Size(264, 16);
            this.lblInputSource.TabIndex = 79;
            this.lblInputSource.Text = "Choose microphone\'s input source";
            // 
            // btnAudio
            // 
            this.btnAudio.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAudio.Location = new System.Drawing.Point(204, 130);
            this.btnAudio.Name = "btnAudio";
            this.btnAudio.Size = new System.Drawing.Size(173, 24);
            this.btnAudio.TabIndex = 80;
            this.btnAudio.Text = "Uncompressed Audio Format...";
            this.btnAudio.Click += new System.EventHandler(this.btnAudio_Click);
            // 
            // ckUncompressedAudio
            // 
            this.ckUncompressedAudio.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ckUncompressedAudio.Location = new System.Drawing.Point(8, 135);
            this.ckUncompressedAudio.Name = "ckUncompressedAudio";
            this.ckUncompressedAudio.Size = new System.Drawing.Size(160, 16);
            this.ckUncompressedAudio.TabIndex = 81;
            this.ckUncompressedAudio.Text = "Use Uncompressed Audio";
            this.ckUncompressedAudio.CheckedChanged += new System.EventHandler(this.ckAudioCompression_CheckedChanged);
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(8, 262);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 16);
            this.label1.TabIndex = 84;
            this.label1.Text = "Select a camera to associate with your microphone";
            // 
            // cboLinkedCamera
            // 
            this.cboLinkedCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLinkedCamera.Location = new System.Drawing.Point(8, 282);
            this.cboLinkedCamera.Name = "cboLinkedCamera";
            this.cboLinkedCamera.Size = new System.Drawing.Size(272, 21);
            this.cboLinkedCamera.Sorted = true;
            this.cboLinkedCamera.TabIndex = 85;
            this.cboLinkedCamera.SelectedIndexChanged += new System.EventHandler(this.cboLinkedCamera_SelectedIndexChanged);
            // 
            // gbMicAndAudio
            // 
            this.gbMicAndAudio.Controls.Add(this.label2);
            this.gbMicAndAudio.Controls.Add(this.cbCompressionFormat);
            this.gbMicAndAudio.Controls.Add(this.lblInputSource);
            this.gbMicAndAudio.Controls.Add(this.cboMicrophonePins);
            this.gbMicAndAudio.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbMicAndAudio.Location = new System.Drawing.Point(8, 6);
            this.gbMicAndAudio.Name = "gbMicAndAudio";
            this.gbMicAndAudio.Size = new System.Drawing.Size(288, 109);
            this.gbMicAndAudio.TabIndex = 87;
            this.gbMicAndAudio.TabStop = false;
            this.gbMicAndAudio.Text = "Configure Microphone and Audio Stream";
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(8, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(264, 16);
            this.label2.TabIndex = 97;
            this.label2.Text = "Select Audio Compression Format";
            // 
            // cbCompressionFormat
            // 
            this.cbCompressionFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCompressionFormat.FormattingEnabled = true;
            this.cbCompressionFormat.Location = new System.Drawing.Point(8, 82);
            this.cbCompressionFormat.Name = "cbCompressionFormat";
            this.cbCompressionFormat.Size = new System.Drawing.Size(272, 21);
            this.cbCompressionFormat.TabIndex = 96;
            this.cbCompressionFormat.SelectedIndexChanged += new System.EventHandler(this.cbCompressionFormat_SelectedIndexChanged);
            // 
            // ckDisableFec
            // 
            this.ckDisableFec.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ckDisableFec.Location = new System.Drawing.Point(8, 224);
            this.ckDisableFec.Name = "ckDisableFec";
            this.ckDisableFec.Size = new System.Drawing.Size(478, 21);
            this.ckDisableFec.TabIndex = 82;
            this.ckDisableFec.Text = "Disable Forward Error Correction (Recommended for Uncompressed Audio)";
            this.ckDisableFec.CheckedChanged += new System.EventHandler(this.ckDisableFec_CheckedChanged);
            // 
            // cbBufferSize
            // 
            this.cbBufferSize.FormattingEnabled = true;
            this.cbBufferSize.Items.AddRange(new object[] {
            "1000",
            "2000",
            "5000",
            "10000"});
            this.cbBufferSize.Location = new System.Drawing.Point(383, 177);
            this.cbBufferSize.Name = "cbBufferSize";
            this.cbBufferSize.Size = new System.Drawing.Size(100, 21);
            this.cbBufferSize.TabIndex = 88;
            // 
            // cbBufferCount
            // 
            this.cbBufferCount.FormattingEnabled = true;
            this.cbBufferCount.Items.AddRange(new object[] {
            "4",
            "5",
            "6"});
            this.cbBufferCount.Location = new System.Drawing.Point(116, 177);
            this.cbBufferCount.Name = "cbBufferCount";
            this.cbBufferCount.Size = new System.Drawing.Size(100, 21);
            this.cbBufferCount.TabIndex = 89;
            // 
            // lblBufferSize
            // 
            this.lblBufferSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblBufferSize.Location = new System.Drawing.Point(285, 180);
            this.lblBufferSize.Name = "lblBufferSize";
            this.lblBufferSize.Size = new System.Drawing.Size(95, 21);
            this.lblBufferSize.TabIndex = 90;
            this.lblBufferSize.Text = "Audio Buffer Size";
            // 
            // lblBufferCount
            // 
            this.lblBufferCount.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblBufferCount.Location = new System.Drawing.Point(8, 180);
            this.lblBufferCount.Name = "lblBufferCount";
            this.lblBufferCount.Size = new System.Drawing.Size(106, 21);
            this.lblBufferCount.TabIndex = 92;
            this.lblBufferCount.Text = "Audio Buffer Count";
            // 
            // ckTestAudio
            // 
            this.ckTestAudio.Appearance = System.Windows.Forms.Appearance.Button;
            this.ckTestAudio.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ckTestAudio.Location = new System.Drawing.Point(319, 280);
            this.ckTestAudio.Name = "ckTestAudio";
            this.ckTestAudio.Size = new System.Drawing.Size(80, 23);
            this.ckTestAudio.TabIndex = 95;
            this.ckTestAudio.Text = "Test Audio";
            this.ckTestAudio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckTestAudio.CheckedChanged += new System.EventHandler(this.ckTestAudio_CheckedChanged);
            // 
            // frmAudioSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(528, 312);
            this.ControlBox = false;
            this.Controls.Add(this.ckUncompressedAudio);
            this.Controls.Add(this.ckTestAudio);
            this.Controls.Add(this.btnAudio);
            this.Controls.Add(this.lblBufferSize);
            this.Controls.Add(this.cbBufferSize);
            this.Controls.Add(this.lblBufferCount);
            this.Controls.Add(this.gbMicAndAudio);
            this.Controls.Add(this.ckDisableFec);
            this.Controls.Add(this.gbCurrentSettings);
            this.Controls.Add(this.cbBufferCount);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cboLinkedCamera);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmAudioSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmAudioSettings_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAudioSettings_FormClosing);
            this.gbCurrentSettings.ResumeLayout(false);
            this.gbMicAndAudio.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        #region Statics

        /// <summary>
        /// A string of spaces
        /// </summary>
        private const string TAB = "     ";

        #endregion Statics

        #region Members

        private AudioCapability ac;

        /// <summary>
        /// parent form
        /// </summary>
        private frmAVDevices frmAV;

        /// <summary>
        /// Source filter to use to build the capability
        /// </summary>
        private FilterInfo sourceFilter;

        #endregion Members

        #region Constructor

        public frmAudioSettings(FilterInfo fi, frmAVDevices frmAV)
        {
            InitializeComponent();

            Debug.Assert(frmAV != null);
            this.frmAV = frmAV;
            this.sourceFilter = fi;

            ac = new AudioCapability(fi);
            ac.SetLogger(new AVLogger(Log));
            ac.ActivateMicrophone();
        }
        
        #endregion Constructor

        #region Form Load

        private void frmAudioSettings_Load(object sender, System.EventArgs e)
        {
            this.btnOK.Font = UIFont.StringFont;
            this.gbCurrentSettings.Font = UIFont.StringFont;
            this.btnAudio.Font = UIFont.StringFont;
            this.ckUncompressedAudio.Font = UIFont.StringFont;
            this.label1.Font = UIFont.StringFont;
            this.cboLinkedCamera.Font = UIFont.StringFont;
            this.gbMicAndAudio.Font = UIFont.StringFont;
            this.ckDisableFec.Font = UIFont.StringFont;
            this.cbBufferSize.Font = UIFont.StringFont;
            this.cbBufferCount.Font = UIFont.StringFont;
            this.lblBufferSize.Font = UIFont.StringFont;
            this.lblBufferCount.Font = UIFont.StringFont;
            this.ckTestAudio.Font = UIFont.StringFont;

            this.btnOK.Text = Strings.Close;
            this.gbCurrentSettings.Text = Strings.CurrentSettings;
            this.lblCurrentSettings.Text = Strings.CurrentSettings;
            this.lblInputSource.Text = Strings.MicrophoneInputSource;
            this.btnAudio.Text = Strings.AudioFormat;
            this.ckUncompressedAudio.Text = Strings.UseUncompressedAudio;
            this.label1.Text = Strings.SelectCamera;
            this.gbMicAndAudio.Text = Strings.ConfigureMicrophone;
            this.label2.Text = Strings.SelectAudioCompressionFormat;
            this.cbCompressionFormat.Text = Strings.SelectAudioCompressionFormat;
            this.ckDisableFec.Text = Strings.DisableAudioForwardErrorCorrection;
            this.lblBufferSize.Text = Strings.AudioBufferSize;
            this.lblBufferCount.Text = Strings.AudioBufferCount;
            this.ckTestAudio.Text = Strings.TestAudio;

            try
            {
                lblCurrentSettings.Text = null;
                this.Text = string.Format(CultureInfo.CurrentCulture, Strings.AdvancedAudioSettings, 
                    ac.CaptureGraph.Source.FriendlyName);

                RestoreLinkedCamera();
                RestoreInputPin();
                RestoreCompressionFormat();
                RestoreCompression();
                RestoreBufferSettings();
                RestoreFec();
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

        private void RestoreFec() {
            ckDisableFec.Enabled = false;
            ckDisableFec.Checked = !FMain.EnableAudioFec;
            ckDisableFec.Enabled = true;
        }

        private void RestoreLinkedCamera()
        {
            // List the selected cameras
            foreach (FilterInfo fi in VideoCapability.SelectedCameras()) {
                cboLinkedCamera.Items.Add(fi);
            }

            // Read from registry and select camera, if in list
            string[] linkedCamera = AVReg.ValueNames(AVReg.LinkedCamera);
            if(linkedCamera != null && linkedCamera.Length > 0)
            {
                Debug.Assert(linkedCamera.Length == 1);

                for(int i = 0; i < cboLinkedCamera.Items.Count; i++)
                {
                    FilterInfo fi = (FilterInfo)cboLinkedCamera.Items[i];
                    if(fi.Moniker == linkedCamera[0])
                    {
                        cboLinkedCamera.SelectedItem = fi;
                    }
                }
            }

            if(cboLinkedCamera.SelectedIndex == -1 && cboLinkedCamera.Items.Count > 0)
            {
                cboLinkedCamera.SelectedIndex = 0;
            }

            cboLinkedCamera.Enabled = cboLinkedCamera.Items.Count > 1;
        }

        /// <summary>
        /// Restore the selected audio input.  If it is a DV source, we have no choices.
        /// </summary>
        private void RestoreInputPin()
        {
            if (ac.CaptureGraph is AudioCaptureGraph) {
                AudioCaptureGraph acg = ac.CaptureGraph as AudioCaptureGraph;
                foreach (IPin pin in acg.AudioSource.InputPins) {
                    cboMicrophonePins.Items.Add(Pin.Name(pin));
                }
                cboMicrophonePins.Enabled = acg.AudioSource.InputPins.Count > 1;

                if (cboMicrophonePins.Items.Count > 0) {
                    cboMicrophonePins.SelectedIndex = acg.AudioSource.InputPinIndex;
                }
            }
            else if (ac.CaptureGraph is DVCaptureGraph) {
                cboMicrophonePins.Items.Add("DV Audio");
                cboMicrophonePins.Enabled = false;
                cboMicrophonePins.SelectedIndex = 0;
            }
        }

        private void RestoreCompressionFormat() {
            cbCompressionFormat.Enabled = false;
            foreach (AudioCompressor.MediaTypeIndexPair p in AudioCompressor.MediaTypeIndices) {
                //It seems that only one compression format works with DV audio.  Don't show the formats that don't work.
                if (ac.CaptureGraph is DVCaptureGraph) {
                    if (p.Index == AudioCompressor.DEFAULT_DV_COMPRESSION_INDEX) { 
                        int i = this.cbCompressionFormat.Items.Add(p);
                        if (ac.CompressionMediaTypeIndex == p.Index) {
                            cbCompressionFormat.SelectedIndex = i;
                        }               
                    }
                }
                else {
                    int i = this.cbCompressionFormat.Items.Add(p);
                    if (ac.CompressionMediaTypeIndex == p.Index) {
                        cbCompressionFormat.SelectedIndex = i;
                    }
                }
            }   
            cbCompressionFormat.Enabled = true;
        }


        private void RestoreCompression()
        {
            ckUncompressedAudio.Enabled = false;
            //ckUncompressedAudio.Checked = !ac.RegAudioCompressorEnabled;
            btnAudio.Enabled = ckUncompressedAudio.Checked;
            cbCompressionFormat.Enabled = !ckUncompressedAudio.Checked;
            ckUncompressedAudio.Enabled = true;
        }

        private void RestoreBufferSettings() {
            int size = ac.BufferSize;
            int sindex = cbBufferSize.Items.IndexOf(size.ToString());
            if (sindex == -1) {
                sindex = cbBufferSize.Items.Add(size.ToString());
            }
            cbBufferSize.SelectedIndex = sindex;

            int count = ac.BufferCount;
            int cindex = cbBufferCount.Items.IndexOf(count.ToString());
            if (cindex == -1) {
                cindex = cbBufferCount.Items.Add(count.ToString());
            }
            cbBufferCount.SelectedIndex = cindex;
        }


        #endregion Form Load

        #region UI Events

        /// <summary>
        /// Show the uncompressed audio format form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAudio_Click(object sender, System.EventArgs e)
        {
            try
            {
                // Show the form
                frmAudioFormat af = new frmAudioFormat(ac.CaptureGraph);
                if(af.ShowDialog(this) == DialogResult.OK)
                {
                    if (ckTestAudio.Checked) {
                        ckTestAudio.Checked = false;
                    }

                    // Get the media type they selected and set it
                    _AMMediaType mt;
                    object fb;
                    af.GetMediaType(out mt, out fb);

                    #region Log

                    Log("Setting media type to...");
                    Log(MediaType.Dump(mt) + MediaType.FormatType.Dump(fb));

                    #endregion Log

                    frmAV.RenderAndRunAudio(ac.CaptureGraph, false);
                    ac.CaptureGraph.RemoveFiltersDownstreamFromSource(MSR.LST.Net.Rtp.PayloadType.dynamicAudio);

                    try
                    {
                        ac.CaptureGraph.Source.SetMediaType(mt, fb);
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

                    ac.SaveAudioSettings();
                    ac.AddAudioCompressor();
                    frmAV.RenderAndRunAudio(ac.CaptureGraph);

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
        /// Input pin change.  Not applicable to DV sources
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboMicrophonePins_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            AudioCaptureGraph acg = ac.CaptureGraph as AudioCaptureGraph;
            if (acg == null) {
                return;
            }

            if (ckTestAudio.Checked) {
                ckTestAudio.Checked = false;
            }

            acg.AudioSource.InputPinIndex = cboMicrophonePins.SelectedIndex;
            ac.SaveMicrophoneSettings();
        }

        /// <summary>
        /// Checked means "Use uncompressed audio"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckAudioCompression_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (ckTestAudio.Checked) {
                    ckTestAudio.Checked = false;
                }

                btnAudio.Enabled = ckUncompressedAudio.Checked;
                cbCompressionFormat.Enabled = !ckUncompressedAudio.Checked;

                if(ckUncompressedAudio.Enabled)
                {
                    //ac.RegAudioCompressorEnabled = !ckUncompressedAudio.Checked;

                    frmAV.RenderAndRunAudio(ac.CaptureGraph, false);
                    ac.CaptureGraph.RemoveFiltersDownstreamFromSource(MSR.LST.Net.Rtp.PayloadType.dynamicAudio);

                    if(!ckUncompressedAudio.Checked)
                    {
                        ac.AddAudioCompressor();
                    }

                    frmAV.RenderAndRunAudio(ac.CaptureGraph);
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

        private void cboLinkedCamera_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            frmAV.SaveLinkedCamera((FilterInfo)cboLinkedCamera.SelectedItem);
        }

        private void btnOK_Click(object sender, EventArgs e) {
            SaveBufferConfig();
            if (ckTestAudio.Checked) {
                ckTestAudio.Checked = false;
            }
        }

        private void ckTestAudio_CheckedChanged(object sender, EventArgs e) {
            SaveBufferConfig();

            try {
                ckTestAudio.Text = ckTestAudio.Checked ? Strings.StopAudio : Strings.TestAudio;
                if (ckTestAudio.Checked) {
                    RefreshAudioCapability();
                }
                frmAV.RenderAndRunAudio(ac.CaptureGraph, ckTestAudio.Checked);
            }
            catch (COMException ex) {
                frmAV.Log(DShowError._AMGetErrorText(ex.ErrorCode));
                frmAV.Log(ex.ToString());
            }
            catch (Exception ex) {
                frmAV.Log(ex.ToString());
            }
        }

        private void ckDisableFec_CheckedChanged(object sender, EventArgs e) {
            FMain.EnableAudioFec = !ckDisableFec.Checked;
        }

        private void cbCompressionFormat_SelectedIndexChanged(object sender, EventArgs e) {
            if (ckTestAudio.Checked) {
                ckTestAudio.Checked = false;
            }

            if (cbCompressionFormat.Enabled) {
                if (ac.CompressionMediaTypeIndex != ((AudioCompressor.MediaTypeIndexPair)cbCompressionFormat.SelectedItem).Index) {
                    ac.CompressionMediaTypeIndex = ((AudioCompressor.MediaTypeIndexPair)cbCompressionFormat.SelectedItem).Index;
                    frmAV.RenderAndRunAudio(ac.CaptureGraph, false);
                    ac.CaptureGraph.RemoveFiltersDownstreamFromSource(MSR.LST.Net.Rtp.PayloadType.dynamicAudio);
                    //If adding the compressor failed last time, we want to reset this property so that we can try again with the new format:
                    //ac.RegAudioCompressorEnabled = true;
                    ac.AddAudioCompressor();
                    frmAV.RenderAndRunAudio(ac.CaptureGraph);
                    UpdateCurrentSettings();
                }
            }
        }

        private void frmAudioSettings_FormClosing(object sender, FormClosingEventArgs e) {
            ac.Dispose();
            ac = null;
        }        
        
        #endregion UI Events

        #region Private

        private void SaveBufferConfig() {
            int bcount;
            if (Int32.TryParse((cbBufferCount.Text), out bcount)) {
                if ((bcount > 0) && (bcount < 100)) {
                    ac.BufferCount = bcount;
                }
            }
            int bsize;
            if (Int32.TryParse(cbBufferSize.Text, out bsize)) {
                if ((bsize >= 100) && (bsize < 1000000)) {
                    ac.BufferSize = bsize;
                }
            }
        }        
        
        private void UpdateCurrentSettings()
        {
            _AMMediaType mt;
            object fb;

            if(!ckUncompressedAudio.Checked) //compressed
            {
                if (ac.CaptureGraph.Compressor is OpusAudioCompressor) {
                    //TODO: forms to be redesigned to support Opus
                    lblCurrentSettings.Text = "Opus Encoder";
                    return;
                }
                ac.CaptureGraph.Compressor.GetMediaType(out mt, out fb);
            }
            else //uncompressed
            {
                ac.CaptureGraph.Source.GetMediaType(out mt, out fb);
            }

            if (fb is WAVEFORMATEX) {
                WAVEFORMATEX wfe = (WAVEFORMATEX)fb;
                lblCurrentSettings.Text = string.Format(CultureInfo.CurrentCulture, Strings.AudioFormatSettings, TAB,
                    wfe.Channels, wfe.BitsPerSample, wfe.SamplesPerSec, wfe.AvgBytesPerSec * 8 / 1000);
            }
            else if (fb is DVINFO) {
                DVCaptureGraph dvcg = ac.CaptureGraph as DVCaptureGraph;
                if (dvcg != null) {
                    dvcg.GetAudioMediaType(out mt, out fb);
                    if (fb is WAVEFORMATEX) {
                        WAVEFORMATEX wfe = (WAVEFORMATEX)fb;
                        lblCurrentSettings.Text = string.Format(CultureInfo.CurrentCulture, Strings.AudioFormatSettings, TAB,
                            wfe.Channels, wfe.BitsPerSample, wfe.SamplesPerSec, wfe.AvgBytesPerSec * 8 / 1000);
                    }
                }
            }
            else {
                lblCurrentSettings.Text = "Unknown";
            }
        }

        private void Log(string msg)
        {
            frmAV.Log(msg);
        }

        private void RefreshAudioCapability() {
            //Rebuild graph.  AFAIK the only reason we need to do this is to apply changes to the buffer settings.
            ac.Dispose();
            ac = new AudioCapability(this.sourceFilter);
            ac.SetLogger(new AVLogger(Log));
            ac.ActivateMicrophone();
        }

        #endregion Private

    }
}
