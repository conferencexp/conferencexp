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
    public class frmAudioSettingsWMA : frmAudioSettingsBase {
        #region Windows Forms Designer

        private ComboBox cbBufferSize;
        private ComboBox cbBufferCount;
        private Label lblBufferSize;
        private Label lblBufferCount;
        private ComboBox cbCompressionFormat;
        private Label lblACompressionFmt;

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
        private void InitializeComponent()
        {
            this.cbBufferSize = new System.Windows.Forms.ComboBox();
            this.cbBufferCount = new System.Windows.Forms.ComboBox();
            this.lblBufferSize = new System.Windows.Forms.Label();
            this.lblBufferCount = new System.Windows.Forms.Label();
            this.cbCompressionFormat = new System.Windows.Forms.ComboBox();
            this.lblACompressionFmt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbBufferSize
            // 
            this.cbBufferSize.FormattingEnabled = true;
            this.cbBufferSize.Items.AddRange(new object[] {
            "1000",
            "2000",
            "5000",
            "10000"});
            this.cbBufferSize.Location = new System.Drawing.Point(383, 167);
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
            this.cbBufferCount.Location = new System.Drawing.Point(116, 167);
            this.cbBufferCount.Name = "cbBufferCount";
            this.cbBufferCount.Size = new System.Drawing.Size(100, 21);
            this.cbBufferCount.TabIndex = 89;
            // 
            // lblBufferSize
            // 
            this.lblBufferSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblBufferSize.Location = new System.Drawing.Point(285, 170);
            this.lblBufferSize.Name = "lblBufferSize";
            this.lblBufferSize.Size = new System.Drawing.Size(95, 21);
            this.lblBufferSize.TabIndex = 90;
            this.lblBufferSize.Text = "Audio Buffer Size";
            // 
            // lblBufferCount
            // 
            this.lblBufferCount.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblBufferCount.Location = new System.Drawing.Point(8, 170);
            this.lblBufferCount.Name = "lblBufferCount";
            this.lblBufferCount.Size = new System.Drawing.Size(106, 21);
            this.lblBufferCount.TabIndex = 92;
            this.lblBufferCount.Text = "Audio Buffer Count";
            // 
            // cbCompressionFormat
            // 
            this.cbCompressionFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCompressionFormat.FormattingEnabled = true;
            this.cbCompressionFormat.Location = new System.Drawing.Point(8, 79);
            this.cbCompressionFormat.Name = "cbCompressionFormat";
            this.cbCompressionFormat.Size = new System.Drawing.Size(272, 21);
            this.cbCompressionFormat.TabIndex = 86;
            this.cbCompressionFormat.SelectedIndexChanged += new System.EventHandler(this.cbCompressionFormat_SelectedIndexChanged);
            // 
            // lblACompressionFmt
            // 
            this.lblACompressionFmt.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblACompressionFmt.Location = new System.Drawing.Point(8, 63);
            this.lblACompressionFmt.Name = "lblACompressionFmt";
            this.lblACompressionFmt.Size = new System.Drawing.Size(264, 16);
            this.lblACompressionFmt.TabIndex = 87;
            this.lblACompressionFmt.Text = "Select Audio Compression Format";
            // 
            // frmAudioSettingsWMA
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(528, 312);
            this.ControlBox = false;
            this.Controls.Add(this.lblACompressionFmt);
            this.Controls.Add(this.lblBufferSize);
            this.Controls.Add(this.cbCompressionFormat);
            this.Controls.Add(this.cbBufferSize);
            this.Controls.Add(this.lblBufferCount);
            this.Controls.Add(this.cbBufferCount);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmAudioSettingsWMA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }

        #endregion

        #region Constructor

        public frmAudioSettingsWMA(FilterInfo fi, frmAVDevices frmAV): base(fi, frmAV) {
            InitializeComponent();
        }
        
        #endregion Constructor

        #region Form Load

        protected override void frmAudioSettings_Load(object sender, System.EventArgs e) {
            base.frmAudioSettings_Load(sender, e);

            this.cbBufferSize.Font = UIFont.StringFont;
            this.cbBufferCount.Font = UIFont.StringFont;
            this.lblBufferSize.Font = UIFont.StringFont;
            this.lblBufferCount.Font = UIFont.StringFont;
            this.lblACompressionFmt.Font = UIFont.StringFont;
            this.cbCompressionFormat.Font = UIFont.StringFont;

            this.lblACompressionFmt.Text = Strings.SelectAudioCompressionFormat;
            this.cbCompressionFormat.Text = Strings.SelectAudioCompressionFormat;
            
            this.lblBufferSize.Text = Strings.AudioBufferSize;
            this.lblBufferCount.Text = Strings.AudioBufferCount;

            // Put some controls into a groupbox of the base class.
            // Do it here so that the forms designer still works.
            this.Controls.Remove(this.lblACompressionFmt);
            this.Controls.Remove(this.cbCompressionFormat);
            this.gbMicAndAudio.Controls.Add(this.cbCompressionFormat);
            this.gbMicAndAudio.Controls.Add(this.lblACompressionFmt);
            
            try
            {
                this.Text += " - WM Audio V2";

                RestoreCompressionFormat();
                RestoreBufferSettings();
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
                    ac.RegAudioCompressorEnabled = true;
                    ac.AddAudioCompressor();
                    frmAV.RenderAndRunAudio(ac.CaptureGraph);
                    UpdateCurrentSettings();
                }
            }
        }
        
        #endregion UI Events

        #region Methods

        protected override void SaveSettings() {
            SaveBufferConfig();
        }

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
        
        protected override void UpdateCurrentSettings() {
            _AMMediaType mt;
            object fb;

            ac.CaptureGraph.Compressor.GetMediaType(out mt, out fb);
 
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

        #endregion Methods

    }
}
