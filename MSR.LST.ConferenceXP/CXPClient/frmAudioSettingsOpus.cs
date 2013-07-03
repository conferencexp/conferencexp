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
    public class frmAudioSettingsOpus : frmAudioSettingsBase {
        
        #region Windows Form Designer

        private System.ComponentModel.Container components = null;

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
            this.SuspendLayout();
            // 
            // frmAudioSettingsOpus
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(528, 312);
            this.ControlBox = false;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmAudioSettingsOpus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

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

            // TODO: Populate a list of candidate media types for the current source filter 
            // and restore default selection.
         
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
            //TODO: Restore Opus Encoder settings
        }

        protected override void SaveSettings() {
           //TODO: save Opus settings 
        }
        
        protected override void  UpdateCurrentSettings() {
            //TODO: Show current settings
            lblCurrentSettings.Text = "Opus Encoder";
        }

        #endregion Methods

    }
}
