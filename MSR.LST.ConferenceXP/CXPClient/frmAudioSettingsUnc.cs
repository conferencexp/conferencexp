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
    public class frmAudioSettingsUnc : frmAudioSettingsBase {
        #region Form Designer

        private Button btnAudio;        
        private ComboBox cbBufferSize;
        private ComboBox cbBufferCount;
        private Label lblBufferSize;
        private Label lblBufferCount;

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
            this.btnAudio = new System.Windows.Forms.Button();
            this.cbBufferSize = new System.Windows.Forms.ComboBox();
            this.cbBufferCount = new System.Windows.Forms.ComboBox();
            this.lblBufferSize = new System.Windows.Forms.Label();
            this.lblBufferCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAudio
            // 
            this.btnAudio.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAudio.Location = new System.Drawing.Point(8, 70);
            this.btnAudio.Name = "btnAudio";
            this.btnAudio.Size = new System.Drawing.Size(173, 24);
            this.btnAudio.TabIndex = 80;
            this.btnAudio.Text = "Uncompressed Audio Format...";
            this.btnAudio.Click += new System.EventHandler(this.btnAudio_Click);
            // 
            // cbBufferSize
            // 
            this.cbBufferSize.FormattingEnabled = true;
            this.cbBufferSize.Items.AddRange(new object[] {
            "1000",
            "2000",
            "5000",
            "10000"});
            this.cbBufferSize.Location = new System.Drawing.Point(383, 162);
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
            this.cbBufferCount.Location = new System.Drawing.Point(116, 162);
            this.cbBufferCount.Name = "cbBufferCount";
            this.cbBufferCount.Size = new System.Drawing.Size(100, 21);
            this.cbBufferCount.TabIndex = 89;
            // 
            // lblBufferSize
            // 
            this.lblBufferSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblBufferSize.Location = new System.Drawing.Point(285, 165);
            this.lblBufferSize.Name = "lblBufferSize";
            this.lblBufferSize.Size = new System.Drawing.Size(95, 21);
            this.lblBufferSize.TabIndex = 90;
            this.lblBufferSize.Text = "Audio Buffer Size";
            // 
            // lblBufferCount
            // 
            this.lblBufferCount.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblBufferCount.Location = new System.Drawing.Point(8, 165);
            this.lblBufferCount.Name = "lblBufferCount";
            this.lblBufferCount.Size = new System.Drawing.Size(106, 21);
            this.lblBufferCount.TabIndex = 92;
            this.lblBufferCount.Text = "Audio Buffer Count";
            // 
            // frmAudioSettingsUnc
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(528, 312);
            this.ControlBox = false;
            this.Controls.Add(this.btnAudio);
            this.Controls.Add(this.lblBufferSize);
            this.Controls.Add(this.cbBufferSize);
            this.Controls.Add(this.lblBufferCount);
            this.Controls.Add(this.cbBufferCount);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmAudioSettingsUnc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }

        #endregion

        #region Constructor

        public frmAudioSettingsUnc(FilterInfo fi, frmAVDevices frmAV): base(fi, frmAV)
        {
            InitializeComponent();
        }
        
        #endregion Constructor

        #region Form Load

        protected override void frmAudioSettings_Load(object sender, EventArgs e) {
            base.frmAudioSettings_Load(sender, e);

            this.btnAudio.Font = UIFont.StringFont;
            this.cbBufferSize.Font = UIFont.StringFont;
            this.cbBufferCount.Font = UIFont.StringFont;
            this.lblBufferSize.Font = UIFont.StringFont;
            this.lblBufferCount.Font = UIFont.StringFont;
 
            this.btnAudio.Text = Strings.AudioFormat;
            this.lblBufferSize.Text = Strings.AudioBufferSize;
            this.lblBufferCount.Text = Strings.AudioBufferCount;

            this.Controls.Remove(this.btnAudio);
            this.gbMicAndAudio.Controls.Add(this.btnAudio);
            
            try
            {
                this.Text += " - Uncompressed";
                RestoreBufferSettings();
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

            ac.CaptureGraph.Source.GetMediaType(out mt, out fb);

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
