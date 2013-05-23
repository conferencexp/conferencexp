using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// Summary description for frmRecord.
    /// </summary>
    public class frmRecord : System.Windows.Forms.Form
    {
        #region Windows Form Designer generated code

        #region Controls

        private System.Windows.Forms.TextBox txtConferenceName;
        private System.Windows.Forms.GroupBox gbHorizontalSeparation1;
        private System.Windows.Forms.Label lblCurrentTime;
        private MSR.LST.MediaControlButton btnStopRecording;
        private System.Windows.Forms.Timer timerUpdateTime;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblNameHeader;
        private System.Windows.Forms.ToolTip buttonToolTips;
        private MSR.LST.MediaControlButton btnStartRecording;
        private System.ComponentModel.IContainer components;

        #endregion Controls

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRecord));
            this.lblNameHeader = new System.Windows.Forms.Label();
            this.txtConferenceName = new System.Windows.Forms.TextBox();
            this.gbHorizontalSeparation1 = new System.Windows.Forms.GroupBox();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.btnStopRecording = new MSR.LST.MediaControlButton();
            this.timerUpdateTime = new System.Windows.Forms.Timer(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.buttonToolTips = new System.Windows.Forms.ToolTip(this.components);
            this.btnStartRecording = new MSR.LST.MediaControlButton();
            this.SuspendLayout();
            // 
            // lblNameHeader
            // 
            this.lblNameHeader.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblNameHeader.Location = new System.Drawing.Point(16, 12);
            this.lblNameHeader.Name = "lblNameHeader";
            this.lblNameHeader.Size = new System.Drawing.Size(40, 16);
            this.lblNameHeader.TabIndex = 0;
            this.lblNameHeader.Text = "Name:";
            // 
            // txtConferenceName
            // 
            this.txtConferenceName.Location = new System.Drawing.Point(16, 28);
            this.txtConferenceName.MaxLength = 254;
            this.txtConferenceName.Name = "txtConferenceName";
            this.txtConferenceName.Size = new System.Drawing.Size(312, 20);
            this.txtConferenceName.TabIndex = 0;
            this.txtConferenceName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtConferenceName_KeyPress);
            this.txtConferenceName.TextChanged += new System.EventHandler(this.txtConferenceName_TextChanged);
            // 
            // gbHorizontalSeparation1
            // 
            this.gbHorizontalSeparation1.Location = new System.Drawing.Point(16, 88);
            this.gbHorizontalSeparation1.Name = "gbHorizontalSeparation1";
            this.gbHorizontalSeparation1.Size = new System.Drawing.Size(312, 8);
            this.gbHorizontalSeparation1.TabIndex = 26;
            this.gbHorizontalSeparation1.TabStop = false;
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblCurrentTime.Location = new System.Drawing.Point(280, 68);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(48, 16);
            this.lblCurrentTime.TabIndex = 35;
            this.lblCurrentTime.Text = "00:00:00";
            this.lblCurrentTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnStopRecording
            // 
            this.btnStopRecording.Enabled = false;
            this.btnStopRecording.Image = ((System.Drawing.Image)(resources.GetObject("btnStopRecording.Image")));
            this.btnStopRecording.ImageType = MSR.LST.MediaControlImage.Stop;
            this.btnStopRecording.Location = new System.Drawing.Point(56, 60);
            this.btnStopRecording.Name = "btnStopRecording";
            this.btnStopRecording.Size = new System.Drawing.Size(24, 24);
            this.btnStopRecording.TabIndex = 36;
            this.buttonToolTips.SetToolTip(this.btnStopRecording, "Stop");
            this.btnStopRecording.Click += new System.EventHandler(this.btnStopRecording_Click);
            // 
            // timerUpdateTime
            // 
            this.timerUpdateTime.Interval = 1000;
            this.timerUpdateTime.Tick += new System.EventHandler(this.timerUpdateTime_Tick);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(232, 104);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 23);
            this.btnCancel.TabIndex = 37;
            this.btnCancel.Text = "Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnStartRecording
            // 
            this.btnStartRecording.Enabled = false;
            this.btnStartRecording.Image = ((System.Drawing.Image)(resources.GetObject("btnStartRecording.Image")));
            this.btnStartRecording.ImageType = MSR.LST.MediaControlImage.Record;
            this.btnStartRecording.Location = new System.Drawing.Point(16, 60);
            this.btnStartRecording.Name = "btnStartRecording";
            this.btnStartRecording.Size = new System.Drawing.Size(24, 24);
            this.btnStartRecording.TabIndex = 3;
            this.buttonToolTips.SetToolTip(this.btnStartRecording, "Record");
            this.btnStartRecording.Click += new System.EventHandler(this.btnStartRecording_Click);
            // 
            // frmRecord
            // 
            this.AcceptButton = this.btnCancel;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(346, 144);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStopRecording);
            this.Controls.Add(this.lblCurrentTime);
            this.Controls.Add(this.gbHorizontalSeparation1);
            this.Controls.Add(this.btnStartRecording);
            this.Controls.Add(this.txtConferenceName);
            this.Controls.Add(this.lblNameHeader);
            this.Font = UIFont.FormFont;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRecord";
            this.ShowInTaskbar = false;
            this.Text = "Record This Conference";
            this.Load += new System.EventHandler(this.frmRecord_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Members

        public string ConferenceName;

        // Reference to BarUI form used to communicate between the Play Back
        // forms and BarUI. It is set in the ctor of frmRecord class.
        private FMain refFMain = null; 

        // Allows to store at what time the playback started to calculate the
        // timer information
        private long startPlayBackTicks;

        #endregion Members

        #region Ctor / Dispose

        public frmRecord(FMain refFMain)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            this.refFMain = refFMain;
        }

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

        #endregion Ctor / Dispose

        #region UI Tasks

        private void txtConferenceName_TextChanged(object sender, System.EventArgs e)
        {
            btnStartRecording.Enabled = txtConferenceName.Text.Length > 0;
        }

        private void txtConferenceName_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if( (e.KeyChar == '\n' || e.KeyChar == '\r') && btnStartRecording.Enabled )
                btnStartRecording.PerformClick();
        }

        /// <summary>
        /// btnStartRecording Click event handler. Start the conference recording.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void btnStartRecording_Click(object sender, System.EventArgs e)
        {
            // Debug.Assert: In theory the condition txtConferenceName.Text.Length < 1 should never be valid because
            //               the Start Recoding button is disabled when there is no character.
            //               I also limited the MaxLenght of the txtConferenceName textbox to 254.
            Debug.Assert( 1 <= txtConferenceName.Text.Length && txtConferenceName.Text.Length < 255 );

            try
            {
                refFMain.StartRecording(txtConferenceName.Text);

                btnStartRecording.Enabled = false;
                btnStopRecording.Enabled = true;
                txtConferenceName.Enabled = false;

                // Note: Once the recording start you can not cancel it
                //       In fact, if we wanted to implement cancel here, we would 
                //       need to remove the new data on the SQL server when cancel and make
                //       we are exactly on the same state than before starting recording
                btnCancel.Enabled = false;

                startPlayBackTicks = DateTime.Now.Ticks;
                timerUpdateTime.Enabled = true;
            }
            catch (ArchiveService.PlaybackInProgressException)
            {
                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                    Strings.PlaybackErrorText, Strings.PlaybackOngoingError), Strings.PlaybackErrorTitle, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 
                    (MessageBoxOptions)0);
            }
            catch (InvalidOperationException ex)
            {
                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, Strings.PlaybackErrorText, 
                    ex.Message), Strings.PlaybackErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
            catch (ArgumentException ex) // IPv6 error
            {
                string errorMsg = ex.Message;
                if (errorMsg.IndexOf("IPv6") > -1)
                {
                    RtlAwareMessageBox.Show(this, errorMsg, Strings.AddressProtocolError, MessageBoxButtons.OK, 
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                }
                else
                {
                    string error = string.Format(CultureInfo.CurrentCulture, Strings.ArchiveServiceErrorText, 
                        ex.ToString());
                    RtlAwareMessageBox.Show(this, error, Strings.ArchiveServiceErrorTitle, MessageBoxButtons.OK, 
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                }
            }
            catch (Exception ex)
            {
                string error = string.Format(CultureInfo.CurrentCulture, Strings.ArchiveServiceErrorText, 
                    ex.ToString());
                RtlAwareMessageBox.Show(this, error, Strings.ArchiveServiceErrorTitle, MessageBoxButtons.OK, 
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
        }

        /// <summary>
        /// btnStopRecording Click event handler. Stop the recording.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void btnStopRecording_Click(object sender, System.EventArgs e)
        {
            timerUpdateTime.Enabled = false;
            refFMain.StopRecording();
            // Disable playback because we cannot playback after recording
            // TODO: Ask Patrick why we cannot playback after recording
            refFMain.EnableMenuActionsPlayback = false;
            this.Close();
        }

        /// <summary>
        /// frmRecord Load event handler.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void frmRecord_Load(object sender, System.EventArgs e)
        {
            this.lblNameHeader.Font = UIFont.StringFont;
            this.txtConferenceName.Font = UIFont.StringFont;
            this.lblCurrentTime.Font = UIFont.StringFont;
            this.btnCancel.Font = UIFont.StringFont;

            this.lblNameHeader.Text = Strings.NameColon;
            this.lblCurrentTime.Text = string.Format(CultureInfo.InvariantCulture, "00:00:00");  // use string.Format to avoid CA1303 error
            this.buttonToolTips.SetToolTip(this.btnStopRecording, Strings.Stop);
            this.btnCancel.Text = Strings.Close;
            this.buttonToolTips.SetToolTip(this.btnStartRecording, Strings.Record);
            this.Text = Strings.RecordThisConference;
            
            this.refFMain.btnLeaveConference.Click +=new EventHandler(btnLeaveConference_Click);

            // Ensure that the record (and playback) menu are disabled to avoid
            // that the user opens several forms
            this.refFMain.EnableMenuActionsPlayback = false;
            this.refFMain.EnableMenuActionsRecord =  false;
        }

        /// <summary>
        /// btnLeaveConference Click event handler. Stop the recording and close the recording
        /// dialog box.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void btnLeaveConference_Click(object sender, EventArgs e)
        {
            timerUpdateTime.Enabled = false;
            refFMain.StopRecording();
            this.Close();
        }

        /// <summary>
        /// Format the duration in HH:MM:SS
        /// </summary>
        /// <param name="duration">The duration</param>
        /// <returns>Formatted duration</returns>
        private string formatDuration(TimeSpan duration)
        {
            // TODO: return duration.ToString("HH:MM:SS");
            return duration.Hours.ToString("00", CultureInfo.InvariantCulture) + ":" + 
                duration.Minutes.ToString("00", CultureInfo.InvariantCulture) + ":" + 
                duration.Seconds.ToString("00", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// timerUpdateTime Tick event handler. Update the current time info.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void timerUpdateTime_Tick(object sender, System.EventArgs e)
        {
            // Find how far we are on the playback
            long currentPlayBackTimeTicks = DateTime.Now.Ticks - startPlayBackTicks;
            lblCurrentTime.Text = formatDuration(new TimeSpan(currentPlayBackTimeTicks));
        }

        /// <summary>
        /// btnCancel Click event handler. Reset the menu status and close the recoding form
        /// </summary>
        /// <remarks>
        /// In this version, the cancel button is disabled while recording so we don't have to 
        /// deal with rolling back what has been recorded.
        /// </remarks>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            // Reset the status of the menu and close the form
            this.refFMain.SetArchiverMenuStatus();
            this.Close();
        }

        #endregion UI Tasks
    }
}
