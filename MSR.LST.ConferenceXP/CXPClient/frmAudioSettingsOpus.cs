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
        private Label label1;
        private ComboBox cbVBR;
        private Label label2;
        private ComboBox cbBufferMS;
        private Label label3;
        private ComboBox cbVBRConstraint;
        private Label label4;
        private ComboBox cbDTX;
        private Label label5;
        private Label label6;
        private ComboBox cbLSBDepth;
        private Label label7;
        private ComboBox cbForcedChannels;
        private Label label8;
        private ComboBox cbMaxBandwidth;
        private Label label9;
        private ComboBox cbComplexity;
        private Label label10;
        private ComboBox cbBitRate;
        private Label label11;
        private ComboBox cbApplication;
        private Label label12;
        private ComboBox cbInbandFec;
        private TextBox tbManualBitRate;
        private TextBox tbPacketLossPerc;
        private Label lblManualBitRate;
        private Label lblManualBitRateRange;
        private Label label16;
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbVBR = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbBufferMS = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbVBRConstraint = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbDTX = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbLSBDepth = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbForcedChannels = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbMaxBandwidth = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbComplexity = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbBitRate = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cbApplication = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cbInbandFec = new System.Windows.Forms.ComboBox();
            this.tbManualBitRate = new System.Windows.Forms.TextBox();
            this.tbPacketLossPerc = new System.Windows.Forms.TextBox();
            this.lblManualBitRate = new System.Windows.Forms.Label();
            this.lblManualBitRateRange = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
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
            this.cbSignal.Location = new System.Drawing.Point(75, 203);
            this.cbSignal.Name = "cbSignal";
            this.cbSignal.Size = new System.Drawing.Size(121, 21);
            this.cbSignal.TabIndex = 88;
            // 
            // lblSignal
            // 
            this.lblSignal.AutoSize = true;
            this.lblSignal.Location = new System.Drawing.Point(15, 206);
            this.lblSignal.Name = "lblSignal";
            this.lblSignal.Size = new System.Drawing.Size(36, 13);
            this.lblSignal.TabIndex = 89;
            this.lblSignal.Text = "Signal";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(304, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 91;
            this.label1.Text = "VBR";
            // 
            // cbVBR
            // 
            this.cbVBR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVBR.FormattingEnabled = true;
            this.cbVBR.Location = new System.Drawing.Point(400, 14);
            this.cbVBR.Name = "cbVBR";
            this.cbVBR.Size = new System.Drawing.Size(121, 21);
            this.cbVBR.TabIndex = 90;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 180);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 93;
            this.label2.Text = "Buffer MS";
            // 
            // cbBufferMS
            // 
            this.cbBufferMS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBufferMS.FormattingEnabled = true;
            this.cbBufferMS.Location = new System.Drawing.Point(75, 177);
            this.cbBufferMS.Name = "cbBufferMS";
            this.cbBufferMS.Size = new System.Drawing.Size(121, 21);
            this.cbBufferMS.TabIndex = 92;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(304, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 95;
            this.label3.Text = "VBR Constraint";
            // 
            // cbVBRConstraint
            // 
            this.cbVBRConstraint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVBRConstraint.FormattingEnabled = true;
            this.cbVBRConstraint.Location = new System.Drawing.Point(400, 42);
            this.cbVBRConstraint.Name = "cbVBRConstraint";
            this.cbVBRConstraint.Size = new System.Drawing.Size(121, 21);
            this.cbVBRConstraint.TabIndex = 94;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(304, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 97;
            this.label4.Text = "DTX";
            // 
            // cbDTX
            // 
            this.cbDTX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDTX.FormattingEnabled = true;
            this.cbDTX.Location = new System.Drawing.Point(400, 69);
            this.cbDTX.Name = "cbDTX";
            this.cbDTX.Size = new System.Drawing.Size(121, 21);
            this.cbDTX.TabIndex = 96;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(308, 234);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 99;
            this.label5.Text = "Packet Loss Perc";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(304, 99);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 101;
            this.label6.Text = "LSB Depth";
            // 
            // cbLSBDepth
            // 
            this.cbLSBDepth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLSBDepth.FormattingEnabled = true;
            this.cbLSBDepth.Location = new System.Drawing.Point(400, 96);
            this.cbLSBDepth.Name = "cbLSBDepth";
            this.cbLSBDepth.Size = new System.Drawing.Size(121, 21);
            this.cbLSBDepth.TabIndex = 100;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(308, 180);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 13);
            this.label7.TabIndex = 103;
            this.label7.Text = "Forced Channels";
            // 
            // cbForcedChannels
            // 
            this.cbForcedChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbForcedChannels.FormattingEnabled = true;
            this.cbForcedChannels.Location = new System.Drawing.Point(400, 177);
            this.cbForcedChannels.Name = "cbForcedChannels";
            this.cbForcedChannels.Size = new System.Drawing.Size(121, 21);
            this.cbForcedChannels.TabIndex = 102;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(308, 153);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 105;
            this.label8.Text = "Max Bandwidth";
            // 
            // cbMaxBandwidth
            // 
            this.cbMaxBandwidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMaxBandwidth.FormattingEnabled = true;
            this.cbMaxBandwidth.Location = new System.Drawing.Point(400, 150);
            this.cbMaxBandwidth.Name = "cbMaxBandwidth";
            this.cbMaxBandwidth.Size = new System.Drawing.Size(121, 21);
            this.cbMaxBandwidth.TabIndex = 104;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(308, 126);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 13);
            this.label9.TabIndex = 107;
            this.label9.Text = "Complexity";
            // 
            // cbComplexity
            // 
            this.cbComplexity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbComplexity.FormattingEnabled = true;
            this.cbComplexity.Location = new System.Drawing.Point(400, 123);
            this.cbComplexity.Name = "cbComplexity";
            this.cbComplexity.Size = new System.Drawing.Size(121, 21);
            this.cbComplexity.TabIndex = 106;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 126);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(45, 13);
            this.label10.TabIndex = 109;
            this.label10.Text = "Bit Rate";
            // 
            // cbBitRate
            // 
            this.cbBitRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBitRate.FormattingEnabled = true;
            this.cbBitRate.Location = new System.Drawing.Point(75, 123);
            this.cbBitRate.Name = "cbBitRate";
            this.cbBitRate.Size = new System.Drawing.Size(121, 21);
            this.cbBitRate.TabIndex = 108;
            this.cbBitRate.SelectedIndexChanged += new System.EventHandler(this.cbBitRate_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 233);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 13);
            this.label11.TabIndex = 113;
            this.label11.Text = "Application";
            // 
            // cbApplication
            // 
            this.cbApplication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbApplication.FormattingEnabled = true;
            this.cbApplication.Location = new System.Drawing.Point(75, 230);
            this.cbApplication.Name = "cbApplication";
            this.cbApplication.Size = new System.Drawing.Size(121, 21);
            this.cbApplication.TabIndex = 112;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(308, 207);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 13);
            this.label12.TabIndex = 111;
            this.label12.Text = "Inband FEC";
            // 
            // cbInbandFec
            // 
            this.cbInbandFec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInbandFec.FormattingEnabled = true;
            this.cbInbandFec.Location = new System.Drawing.Point(400, 204);
            this.cbInbandFec.Name = "cbInbandFec";
            this.cbInbandFec.Size = new System.Drawing.Size(121, 21);
            this.cbInbandFec.TabIndex = 110;
            // 
            // tbManualBitRate
            // 
            this.tbManualBitRate.Location = new System.Drawing.Point(100, 151);
            this.tbManualBitRate.Name = "tbManualBitRate";
            this.tbManualBitRate.Size = new System.Drawing.Size(80, 20);
            this.tbManualBitRate.TabIndex = 114;
            // 
            // tbPacketLossPerc
            // 
            this.tbPacketLossPerc.Location = new System.Drawing.Point(400, 231);
            this.tbPacketLossPerc.Name = "tbPacketLossPerc";
            this.tbPacketLossPerc.Size = new System.Drawing.Size(68, 20);
            this.tbPacketLossPerc.TabIndex = 115;
            // 
            // lblManualBitRate
            // 
            this.lblManualBitRate.AutoSize = true;
            this.lblManualBitRate.Location = new System.Drawing.Point(14, 154);
            this.lblManualBitRate.Name = "lblManualBitRate";
            this.lblManualBitRate.Size = new System.Drawing.Size(83, 13);
            this.lblManualBitRate.TabIndex = 116;
            this.lblManualBitRate.Text = "Manual Bit Rate";
            // 
            // lblManualBitRateRange
            // 
            this.lblManualBitRateRange.AutoSize = true;
            this.lblManualBitRateRange.Location = new System.Drawing.Point(182, 154);
            this.lblManualBitRateRange.Name = "lblManualBitRateRange";
            this.lblManualBitRateRange.Size = new System.Drawing.Size(70, 13);
            this.lblManualBitRateRange.TabIndex = 117;
            this.lblManualBitRateRange.Text = "(512-512000)";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(474, 234);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(40, 13);
            this.label16.TabIndex = 119;
            this.label16.Text = "(0-100)";
            // 
            // frmAudioSettingsOpus
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(528, 312);
            this.ControlBox = false;
            this.Controls.Add(this.label16);
            this.Controls.Add(this.lblManualBitRateRange);
            this.Controls.Add(this.lblManualBitRate);
            this.Controls.Add(this.tbPacketLossPerc);
            this.Controls.Add(this.tbManualBitRate);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cbApplication);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cbInbandFec);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cbBitRate);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cbComplexity);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbMaxBandwidth);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cbForcedChannels);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbLSBDepth);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbDTX);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbVBRConstraint);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbBufferMS);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbVBR);
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

            // Put some controls into a groupbox of the base class.
            // Do it here so that the forms designer still works.
            this.Controls.Remove(this.lblACompressionFmt);
            this.Controls.Remove(this.cbCompressionFormat);
            this.gbMicAndAudio.Controls.Add(this.cbCompressionFormat);
            this.gbMicAndAudio.Controls.Add(this.lblACompressionFmt);

            // Hide the current settings display since all the settings are exposed in controls
            this.gbCurrentSettings.Visible = false;

            // Increase the height of the form to fit more controls.
            this.ClientSize = new System.Drawing.Size(this.ClientSize.Width,this.ClientSize.Height + 50);

            // Establish data sources for the ComboBox controls
            this.cbSignal.DataSource = Enum.GetValues(typeof(OpusAudioCompressor.EnumSignal));
            this.cbBitRate.DataSource = Enum.GetValues(typeof(OpusAudioCompressor.EnumBitRate));
            this.cbBufferMS.DataSource = OpusAudioCompressor.ValuesBufferMS;
            this.cbComplexity.DataSource = OpusAudioCompressor.ValuesComplexity;
            this.cbDTX.DataSource = Enum.GetValues(typeof(OpusAudioCompressor.EnumDTX));
            this.cbForcedChannels.DataSource = Enum.GetValues(typeof(OpusAudioCompressor.EnumForcedChannels));
            this.cbLSBDepth.DataSource = OpusAudioCompressor.ValuesLSBDepth;
            this.cbMaxBandwidth.DataSource = Enum.GetValues(typeof(OpusAudioCompressor.EnumMaxBandwidth));
            this.cbVBR.DataSource = Enum.GetValues(typeof(OpusAudioCompressor.EnumVBR));
            this.cbVBRConstraint.DataSource = Enum.GetValues(typeof(OpusAudioCompressor.EnumVBRConstraint));
            this.cbApplication.DataSource = Enum.GetValues(typeof(OpusAudioCompressor.EnumApplication));
            this.cbInbandFec.DataSource = Enum.GetValues(typeof(OpusAudioCompressor.EnumInbandFec));

            try
            {
                this.Text += " - Opus Encoder";
                // Load OpusEncoder static properties from the registry and put them in the form
                RestoreComboBoxValues();
                // Load compression format combo box and validate compatibility with selected hardware
                RestoreCompressionFormat();
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

        #region Restore Settings

        /// <summary>
        /// Put compatible formats supported by the selected hardware device into the 
        /// compression format ComboBox.  We assume the compressor's static properties have already been
        /// pulled in from the registry.
        /// </summary>
        private void RestoreCompressionFormat() {
            _AMMediaType[] mts = Pin.GetMediaTypes(ac.CaptureGraph.Source.OutputPin);
            int defaultIndex = 0;
            // Note: GetMediaTypes appears to return the selected MT in element 0, which is a 
            // duplicate of a MT found elsewhere in the array.  That's why we are ignoring
            // element 0 here.
            for (int j=1; j< mts.Length; j++) {
                _AMMediaType mt = mts[j];
                WAVEFORMATEX wfex = (WAVEFORMATEX)MediaType.FormatType.MarshalData(mt);
                if (OpusAudioCompressor.WorksWithOpus(wfex)) {
                    int i = cbCompressionFormat.Items.Add(new CompressorFmt(wfex));
                    if ((OpusAudioCompressor.Frequency == wfex.SamplesPerSec) &&
                        (OpusAudioCompressor.Channels == wfex.Channels) &&
                        (OpusAudioCompressor.Depth == wfex.BitsPerSample)) {
                        defaultIndex = i;
                    }
                }
            }

            if (cbCompressionFormat.Items.Count == 0) {
                throw new ApplicationException("No audio formats supported by the device are compatible with the Opus Encoder.");
            }
            cbCompressionFormat.SelectedIndex = defaultIndex;
        }


        /// <summary>
        /// Populate form controls and compressor static values from the registry, while validating what was loaded.
        /// 
        /// We assume that for each control whose value needs to be persisted there is a public static int
        /// field in the compressor class, the name of which is used as a key for finding validation data.
        /// Validation data may come in the form of an enum with int as the underlying data type, an
        /// int[] containing the accepted values, or an int[] containing min/max values. We look for them in
        /// that order, stopping when the first is found.  
        /// 
        /// Once validated we put the value into the corresponding ComboBox or TextBox.  If validation fails
        /// we get a default value from the compressor and use that instead.
        /// </summary>
        private void RestoreComboBoxValues() {
            // Loop over all the public statics in the compressor type.
            Type compressorType = typeof(OpusAudioCompressor);
            FieldInfo[] fields = compressorType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields) {
                if (field.IsLiteral || field.IsInitOnly) 
                    continue; //Ignore const and readonly

                // Fetch the current value in case none is set in the registry.
                // On first use, this should be the default.
                object newValue = field.GetValue(null);

                // Read from registry
                object val = AVReg.ReadValue(AVReg.OpusCompressorKey, field.Name);
                if (val != null) {
                    newValue = val;
                    field.SetValue(null, newValue);
                }

                // Find the corresponding enum or array for validation
                Type enumType;
                int[] valuesArray;
                // Check for enum first
                if ((enumType = findEnumType(field.Name)) != null) {
                    if (!enumType.IsEnumDefined(newValue)) {
                        //If the registry value is invalid, revert to default value
                        newValue = findDefaultValue(field.Name);
                        field.SetValue(null, newValue);
                    }

                    ComboBox cb = findComboBox(field.Name);
                    if (cb != null)
                        cb.SelectedItem = Enum.ToObject(enumType, newValue);
                }
                else if ((valuesArray = findValuesArray(field.Name)) != null) {
                    // If there's no matching enum, look for the corresponding array of int.
                    int i = (int)newValue;
                    bool validated = false;
                    foreach (int j in valuesArray) {
                        if (i == j) {
                            validated = true;
                            break;
                        }
                    }
                    if (!validated) {
                        newValue = findDefaultValue(field.Name);
                        field.SetValue(null, newValue);
                    }

                    ComboBox cb = findComboBox(field.Name);
                    if (cb != null)
                        cb.SelectedItem = newValue;
                }
                else if ((valuesArray = findMinMaxArray(field.Name)) != null) {
                    // In this case the integer value is constrained by min and max, and the 
                    // control is a TextBox.
                    if ((int)newValue < valuesArray[0] || (int)newValue > valuesArray[1]) {
                        newValue = findDefaultValue(field.Name);
                        field.SetValue(null, newValue);
                    }

                    TextBox tb = findTextBox(field.Name);
                    if (tb != null) {
                        tb.Text = newValue.ToString();
                    }
                }
            }
        }

        #endregion

        #region Save Settings

        /// <summary>
        /// Save compressor settings from the form back into the static properties and persist to the registry.
        /// </summary>
        protected override void SaveSettings() {
            // Compressor format
            OpusAudioCompressor.Frequency = (int)((CompressorFmt)this.cbCompressionFormat.SelectedItem).WFEX.SamplesPerSec;
            OpusAudioCompressor.Channels = (int)((CompressorFmt)this.cbCompressionFormat.SelectedItem).WFEX.Channels;

            Type compressorType = typeof(OpusAudioCompressor);
            FieldInfo[] fields = compressorType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields) {
                if (field.IsLiteral || field.IsInitOnly)
                    continue;

                Type enumType;
                int[] valuesArray;
                bool validated = false;
                int checkedValue = 0;

                // Look for the enum type first
                if ((enumType = findEnumType(field.Name)) != null) {
                    object saveValue;
                    ComboBox cb = findComboBox(field.Name);
                    if (cb == null)
                        continue;
                    if (enumTryParse(enumType, cb.SelectedValue.ToString(), out saveValue)) {
                        checkedValue = (int)saveValue;
                        validated = true;
                    }
                }
                else if ((valuesArray = findValuesArray(field.Name)) != null) {
                    // Use a int[] of legal values
                    int uncheckedValue = (int)field.GetValue(null);
                    ComboBox cb = findComboBox(field.Name);
                    if ((cb == null) ||
                        ((cb != null) && (int.TryParse(cb.SelectedValue.ToString(), out uncheckedValue)))) {
                        foreach (int j in valuesArray) {
                            if (uncheckedValue == j) {
                                checkedValue = uncheckedValue;
                                validated = true;
                                break;
                            }
                        }
                    }
                }
                else if ((valuesArray = findMinMaxArray(field.Name)) != null) {
                    // Use a int[] with min/max values.
                    TextBox tb = findTextBox(field.Name);
                    if (tb == null)
                        continue;
                    if (int.TryParse(tb.Text, out checkedValue)) {
                        if (checkedValue >= valuesArray[0] && checkedValue <= valuesArray[1]) {
                            validated = true;
                        }
                    }
                }

                if (!validated) {
                    checkedValue = (int)findDefaultValue(field.Name);
                }
                field.SetValue(null, checkedValue);
                AVReg.WriteValue(AVReg.OpusCompressorKey, field.Name, checkedValue);
            }
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Given a name, find the corresponding enum type
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private Type findEnumType(string tag) {
            Type compressorType = typeof(OpusAudioCompressor);
            MemberInfo[] enums = compressorType.GetMember("Enum" + tag);
            if (enums.Length != 1)
                return null;
            return (Type)enums[0];
        }

        /// <summary>
        /// Get the legal values array from the compressor
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private int[] findValuesArray(string tag) {
            return findArrayField(tag, "Values");
        }

        /// <summary>
        /// Get the min/max array from the compressor
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private int[] findMinMaxArray(string tag) {
            return findArrayField(tag, "MinMax");
        }

        /// <summary>
        /// Get an array from the compressor
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private int[] findArrayField(string tag, string prefix) {
            Type compressorType = typeof(OpusAudioCompressor);
            FieldInfo fi = compressorType.GetField(prefix + tag);
            if (fi != null) {
                return (int[])fi.GetValue(null);
            }
            return null;
        }

        /// <summary>
        /// Get the default value for a property from the compressor 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private object findDefaultValue(string tag) {
            Type compressorType = typeof(OpusAudioCompressor);
            FieldInfo field = compressorType.GetField("Default" + tag);
            if (field != null) {
                return field.GetValue(null);
            }
            return null;
        }

        /// <summary>
        /// Find a textbox control on this form
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private TextBox findTextBox(string tag) {
            return findControl(tag, "tb") as TextBox;
        }

        /// <summary>
        /// Find a ComboBox control on this form.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private ComboBox findComboBox(string tag) {
            return findControl(tag, "cb") as ComboBox;
        }

        /// <summary>
        /// Find a control on this form.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private Control findControl(string tag, string prefix) {
            Type myType = typeof(frmAudioSettingsOpus);
            FieldInfo ctl = myType.GetField(prefix + tag, BindingFlags.Instance | BindingFlags.NonPublic);
            if (ctl == null)
                return null;
            return ctl.GetValue(this) as Control;
        }

        /// <summary>
        /// Simulate the generic Enum.TryParse using a reflected type.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool enumTryParse(Type enumType, string value, out object result) {
            try {
                result = Enum.Parse(enumType, value);
                return true;
            }
            catch {
                result = null;
                return false;
            }
        }

        #endregion

        #region UI Events

        private void cbBitRate_SelectedIndexChanged(object sender, EventArgs e) {
            bool manual = (this.cbBitRate.SelectedValue.ToString() == "Manual");
            this.tbManualBitRate.Enabled = manual;
            this.lblManualBitRate.Enabled = manual;
            this.lblManualBitRateRange.Enabled = manual;
        }

        #endregion       
        
        #endregion Methods

        #region CompressorFmt class

        /// <summary>
        /// Helper for compressor format ComboBox items
        /// </summary>
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
