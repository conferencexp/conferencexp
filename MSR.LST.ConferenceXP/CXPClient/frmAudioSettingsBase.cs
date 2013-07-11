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
    public class frmAudioSettingsBase : System.Windows.Forms.Form
    {
        /// Notes: 
        /// * Do not set the audio pin combo box to sorted.  We use the index
        /// not the name, to determine which pin was selected.
        /// * We assume there is no frmAV AudioCapability when the form is
        /// constructed.  We always create our own capability, and dispose it
        /// when we close.

        #region Windows Forms Generated Code

        protected System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblInputSource;
        private System.Windows.Forms.Label label1;
        protected System.Windows.Forms.ComboBox cboMicrophonePins;
        private System.Windows.Forms.ComboBox cboLinkedCamera;
        protected System.Windows.Forms.GroupBox gbCurrentSettings;
        protected System.Windows.Forms.Label lblCurrentSettings;
        protected System.Windows.Forms.GroupBox gbMicAndAudio;
        private CheckBox ckDisableFec;
        protected CheckBox ckTestAudio;

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
        //[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.btnOK = new System.Windows.Forms.Button();
            this.gbCurrentSettings = new System.Windows.Forms.GroupBox();
            this.lblCurrentSettings = new System.Windows.Forms.Label();
            this.cboMicrophonePins = new System.Windows.Forms.ComboBox();
            this.lblInputSource = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboLinkedCamera = new System.Windows.Forms.ComboBox();
            this.gbMicAndAudio = new System.Windows.Forms.GroupBox();
            this.ckDisableFec = new System.Windows.Forms.CheckBox();
            this.ckTestAudio = new System.Windows.Forms.CheckBox();
            this.gbCurrentSettings.SuspendLayout();
            this.gbMicAndAudio.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
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
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(8, 262);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(280, 16);
            this.label1.TabIndex = 84;
            this.label1.Text = "Select a camera to associate with your microphone";
            // 
            // cboLinkedCamera
            // 
            this.cboLinkedCamera.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
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
            this.gbMicAndAudio.Controls.Add(this.lblInputSource);
            this.gbMicAndAudio.Controls.Add(this.cboMicrophonePins);
            this.gbMicAndAudio.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbMicAndAudio.Location = new System.Drawing.Point(8, 6);
            this.gbMicAndAudio.Name = "gbMicAndAudio";
            this.gbMicAndAudio.Size = new System.Drawing.Size(288, 112);
            this.gbMicAndAudio.TabIndex = 97;
            this.gbMicAndAudio.TabStop = false;
            this.gbMicAndAudio.Text = "Configure Microphone and Audio Stream";
            // 
            // ckDisableFec
            // 
            this.ckDisableFec.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ckDisableFec.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ckDisableFec.Location = new System.Drawing.Point(8, 224);
            this.ckDisableFec.Name = "ckDisableFec";
            this.ckDisableFec.Size = new System.Drawing.Size(478, 21);
            this.ckDisableFec.TabIndex = 82;
            this.ckDisableFec.Text = "Disable Forward Error Correction (Recommended for Uncompressed Audio)";
            this.ckDisableFec.CheckedChanged += new System.EventHandler(this.ckDisableFec_CheckedChanged);
            // 
            // ckTestAudio
            // 
            this.ckTestAudio.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
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
            // frmAudioSettingsBase
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(528, 312);
            this.ControlBox = false;
            this.Controls.Add(this.ckTestAudio);
            this.Controls.Add(this.ckDisableFec);
            this.Controls.Add(this.gbCurrentSettings);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cboLinkedCamera);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gbMicAndAudio);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmAudioSettingsBase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAudioSettings_FormClosing);
            this.Load += new System.EventHandler(this.frmAudioSettings_Load);
            this.gbCurrentSettings.ResumeLayout(false);
            this.gbMicAndAudio.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        #region Statics

        /// <summary>
        /// A string of spaces
        /// </summary>
        protected const string TAB = "     ";

        #endregion Statics

        #region Members

        protected AudioCapability ac;

        /// <summary>
        /// parent form
        /// </summary>
        protected frmAVDevices frmAV;

        /// <summary>
        /// Source filter to use to build the capability
        /// </summary>
        protected FilterInfo sourceFilter;

        #endregion Members

        #region Constructor

        private frmAudioSettingsBase() {}

        public frmAudioSettingsBase(FilterInfo fi, frmAVDevices frmAV)
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

        protected virtual void frmAudioSettings_Load(object sender, System.EventArgs e)
        {
            this.btnOK.Font = UIFont.StringFont;
            this.gbCurrentSettings.Font = UIFont.StringFont;
            this.label1.Font = UIFont.StringFont;
            this.cboLinkedCamera.Font = UIFont.StringFont;
            this.gbMicAndAudio.Font = UIFont.StringFont;
            this.ckDisableFec.Font = UIFont.StringFont;
            this.ckTestAudio.Font = UIFont.StringFont;

            this.btnOK.Text = Strings.Close;
            this.gbCurrentSettings.Text = Strings.CurrentSettings;
            this.lblInputSource.Text = Strings.MicrophoneInputSource;
            this.label1.Text = Strings.SelectCamera;
            this.gbMicAndAudio.Text = Strings.ConfigureMicrophone;
            this.ckDisableFec.Text = Strings.DisableAudioForwardErrorCorrection;
            this.ckTestAudio.Text = Strings.TestAudio;
            this.lblCurrentSettings.Text = null;

            try
            {
                this.Text = string.Format(CultureInfo.CurrentCulture, Strings.AdvancedAudioSettings, 
                    this.sourceFilter.Name);

                RestoreLinkedCamera();
                RestoreInputPin();
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


        private void cboLinkedCamera_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            frmAV.SaveLinkedCamera((FilterInfo)cboLinkedCamera.SelectedItem);
        }

        private void btnOK_Click(object sender, EventArgs e) {
            SaveSettings();
            if (ckTestAudio.Checked) {
                ckTestAudio.Checked = false;
            }
        }

        private void ckTestAudio_CheckedChanged(object sender, EventArgs e) {
            SaveSettings();

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

        private void frmAudioSettings_FormClosing(object sender, FormClosingEventArgs e) {
            ac.Dispose();
            ac = null;
        }        
        
        #endregion UI Events

        #region Methods

        /// <summary>
        /// Override to save anything needing saving before the form is closed
        /// or before an audio test begins.
        /// </summary>
        protected virtual void SaveSettings() {}

        /// <summary>
        /// Override to update the contents of the Current Settings text area
        /// </summary>
        protected virtual void UpdateCurrentSettings() {}

        protected void Log(string msg)
        {
            frmAV.Log(msg);
        }

        protected void RefreshAudioCapability() {
            //Rebuild graph.  AFAIK the only reason we need to do this is to apply changes to the buffer settings.
            ac.Dispose();
            ac = new AudioCapability(this.sourceFilter);
            ac.SetLogger(new AVLogger(Log));
            ac.ActivateMicrophone();
        }

        #endregion Methods

    }
}
