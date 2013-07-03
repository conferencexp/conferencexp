using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using MSR.LST.MDShow;
using System.Collections.Generic;
using System.Reflection;


namespace MSR.LST.ConferenceXP
{
    public class frmAudioSettingsOpus : frmAudioSettingsBase {
        
        #region Windows Form Designer

        private System.ComponentModel.Container components = null;
        private ComboBox cbCompressionFormat;
        private ComboBox cbSignal;
        private Label lblSignal;
        private Label lblACompressionFmt;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing ) {
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
            this.cbCompressionFormat = new System.Windows.Forms.ComboBox();
            this.lblACompressionFmt = new System.Windows.Forms.Label();
            this.cbSignal = new System.Windows.Forms.ComboBox();
            this.lblSignal = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbCompressionFormat
            // 
            this.cbCompressionFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCompressionFormat.FormattingEnabled = true;
            this.cbCompressionFormat.Location = new System.Drawing.Point(8, 79);
            this.cbCompressionFormat.Name = "cbCompressionFormat";
            this.cbCompressionFormat.Size = new System.Drawing.Size(272, 21);
            this.cbCompressionFormat.TabIndex = 86;
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
            // cbSignal
            // 
            this.cbSignal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSignal.FormattingEnabled = true;
            this.cbSignal.Location = new System.Drawing.Point(54, 134);
            this.cbSignal.Name = "cbSignal";
            this.cbSignal.Size = new System.Drawing.Size(121, 21);
            this.cbSignal.TabIndex = 88;
            // 
            // lblSignal
            // 
            this.lblSignal.AutoSize = true;
            this.lblSignal.Location = new System.Drawing.Point(12, 137);
            this.lblSignal.Name = "lblSignal";
            this.lblSignal.Size = new System.Drawing.Size(36, 13);
            this.lblSignal.TabIndex = 89;
            this.lblSignal.Text = "Signal";
            // 
            // frmAudioSettingsOpus
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(528, 312);
            this.ControlBox = false;
            this.Controls.Add(this.lblSignal);
            this.Controls.Add(this.cbSignal);
            this.Controls.Add(this.lblACompressionFmt);
            this.Controls.Add(this.cbCompressionFormat);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmAudioSettingsOpus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Constructor

        public frmAudioSettingsOpus(FilterInfo fi, frmAVDevices frmAV): base(fi, frmAV) {
            InitializeComponent();
        }
        
        #endregion Constructor

        #region Form Load

        protected override void frmAudioSettings_Load(object sender, EventArgs e) {
            base.frmAudioSettings_Load(sender, e);

            this.lblACompressionFmt.Font = UIFont.StringFont;
            this.cbCompressionFormat.Font = UIFont.StringFont;

            this.lblACompressionFmt.Text = Strings.SelectAudioCompressionFormat;
            this.cbCompressionFormat.Text = Strings.SelectAudioCompressionFormat;

            // Put some controls into a groupbox of the base class.
            // Do it here so that the forms designer still works.
            this.Controls.Remove(this.lblACompressionFmt);
            this.Controls.Remove(this.cbCompressionFormat);
            this.gbMicAndAudio.Controls.Add(this.cbCompressionFormat);
            this.gbMicAndAudio.Controls.Add(this.lblACompressionFmt);

            //TODO: add the rest of the Opus settings
            this.cbSignal.DataSource = Enum.GetValues(typeof(OpusAudioCompressor.EnumSignal));

            try
            {
                this.Text += " - Opus Encoder";
                RestoreCompressionFormat();
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

        #endregion Form Load

        #region Methods

        private void RestoreCompressionFormat() {

            // Load OpusEncoder static properties from the registry.
            RegLoad();

            // TODO: populate form controls with compressor static values while validating what was loaded.
            try {
                this.cbSignal.SelectedItem = (OpusAudioCompressor.EnumSignal)OpusAudioCompressor.Signal;
            }
            catch {
                this.cbSignal.SelectedItem = OpusAudioCompressor.EnumSignal.Auto;
                OpusAudioCompressor.Signal = (int)OpusAudioCompressor.EnumSignal.Auto;
            }

            // Populate the compression format combo box and set default.
            _AMMediaType[] mts = Pin.GetMediaTypes(ac.CaptureGraph.Source.OutputPin);
            bool foundDefault = false;
            foreach (_AMMediaType mt in mts) {
                WAVEFORMATEX wfex = (WAVEFORMATEX)MediaType.FormatType.MarshalData(mt);
                if (OpusAudioCompressor.WorksWithOpus(wfex)) {
                    int i = cbCompressionFormat.Items.Add(new CompressorFmt(wfex));
                    if ((OpusAudioCompressor.Frequency == wfex.SamplesPerSec) &&
                        (OpusAudioCompressor.Channels == wfex.Channels) &&
                        (OpusAudioCompressor.Depth == wfex.BitsPerSample)) {
                        cbCompressionFormat.SelectedIndex = i;
                        foundDefault = true;
                    }
                }
            }

            if (cbCompressionFormat.Items.Count == 0) {
                throw new ApplicationException("No audio formats supported by the device are compatible with the Opus Encoder.");
            }

            if (!foundDefault) {
                cbCompressionFormat.SelectedIndex = 0;
            }

        }

        /// <summary>
        /// Attempt to read all of the Opus compressor's public statics from the registry.
        /// </summary>
        private void RegLoad() {
            // Read compressor statics from registry.
            Type myType = typeof(OpusAudioCompressor);
            FieldInfo[] fields = myType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields) {
                if (field.IsLiteral) 
                    continue;
                object val = AVReg.ReadValue(AVReg.OpusCompressorKey, field.Name);
                if (val != null) {
                    Type newType = field.FieldType;
                    object newValue = Convert.ChangeType(val, newType, System.Globalization.CultureInfo.InvariantCulture);
                    field.SetValue(null, newValue);
                }
            }
        }

        /// <summary>
        /// Save compressor properties to the registry.
        /// </summary>
        protected override void SaveSettings() {

            // TODO: validate and copy values from form controls into compressor statics.
            OpusAudioCompressor.Frequency = (int)((CompressorFmt)this.cbCompressionFormat.SelectedItem).WFEX.SamplesPerSec;
            OpusAudioCompressor.Channels = (int)((CompressorFmt)this.cbCompressionFormat.SelectedItem).WFEX.Channels;

            OpusAudioCompressor.EnumSignal signal;
            if (Enum.TryParse<OpusAudioCompressor.EnumSignal>(cbSignal.SelectedValue.ToString(), out signal)) {
                OpusAudioCompressor.Signal = (int)signal;
            }
            else {
                OpusAudioCompressor.Signal = (int)OpusAudioCompressor.EnumSignal.Auto;
            }

            // Write compressor statics to registry.
            Type myType = typeof(OpusAudioCompressor);
            FieldInfo[] fields = myType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields) {
                if (field.IsLiteral) 
                    continue;
                AVReg.WriteValue(AVReg.OpusCompressorKey, field.Name, field.GetValue(null));
            }       
        }
        
        protected override void  UpdateCurrentSettings() {
            //TODO: Show current settings
            lblCurrentSettings.Text = "Opus Encoder";
        }

        #endregion Methods

        #region CompressorFmt class

        private class CompressorFmt {
            public CompressorFmt(WAVEFORMATEX wfex) {
                this.WFEX = wfex;
            }

            public WAVEFORMATEX WFEX;

            public override string ToString() {
                float khz = (float)WFEX.SamplesPerSec / 1000f;

                string ch = (WFEX.Channels == 1) ? "Mono" : "Stereo";
                return khz.ToString() + "KHz, " + ch;
            }
        }
        
        #endregion

    }
}
