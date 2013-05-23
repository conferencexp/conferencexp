using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Windows.Forms;

using MSR.LST.ConferenceXP.ArchiveService;


namespace MSR.LST.ConferenceXP
{
    public class frmArchiveClient : System.Windows.Forms.Form
    {
        #region Windows Form Designer generated code

        #region Controls

        private System.Windows.Forms.ColumnHeader id;
        private System.Windows.Forms.ColumnHeader time;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblConferenceSelected;
        private System.Windows.Forms.TrackBar tbPlayBack;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.Label lblEndTime;
        private System.Windows.Forms.Timer timerSliderUpdate;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.GroupBox gbHorizontalSeparation2;
        private System.Windows.Forms.Label lblConferenceDetails1;
        private MSR.LST.MediaControlButton btnPlay;
        private System.Windows.Forms.Label lblCurrentTime;
        private System.Windows.Forms.Label lblRewind;
        private System.Windows.Forms.Label lblFF;
        private System.Windows.Forms.ImageList imageListRewindFFButtons;
        private MSR.LST.MediaControlButton btnStop;
        private System.Windows.Forms.ToolTip buttonToolTips;
        private System.Windows.Forms.GroupBox gbHorizontalSeparation1;

        #endregion Controls

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmArchiveClient));
            this.id = new System.Windows.Forms.ColumnHeader();
            this.time = new System.Windows.Forms.ColumnHeader();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblConferenceHeader = new System.Windows.Forms.Label();
            this.lblConferenceSelected = new System.Windows.Forms.Label();
            this.tbPlayBack = new System.Windows.Forms.TrackBar();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.lblEndTime = new System.Windows.Forms.Label();
            this.lblConferenceDetails1 = new System.Windows.Forms.Label();
            this.timerSliderUpdate = new System.Windows.Forms.Timer(this.components);
            this.gbHorizontalSeparation1 = new System.Windows.Forms.GroupBox();
            this.gbHorizontalSeparation2 = new System.Windows.Forms.GroupBox();
            this.lblRewind = new System.Windows.Forms.Label();
            this.lblFF = new System.Windows.Forms.Label();
            this.btnPlay = new MSR.LST.MediaControlButton();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.imageListRewindFFButtons = new System.Windows.Forms.ImageList(this.components);
            this.btnStop = new MSR.LST.MediaControlButton();
            this.buttonToolTips = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.tbPlayBack)).BeginInit();
            this.SuspendLayout();
            // 
            // id
            // 
            this.id.Width = 0;
            // 
            // time
            // 
            this.time.Text = "Duration";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(288, 192);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblConferenceHeader
            // 
            this.lblConferenceHeader.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblConferenceHeader.ForeColor = System.Drawing.Color.Black;
            this.lblConferenceHeader.Location = new System.Drawing.Point(16, 12);
            this.lblConferenceHeader.Name = "lblConferenceHeader";
            this.lblConferenceHeader.Size = new System.Drawing.Size(68, 16);
            this.lblConferenceHeader.TabIndex = 3;
            this.lblConferenceHeader.Text = "Conference:";
            this.lblConferenceHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblConferenceSelected
            // 
            this.lblConferenceSelected.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblConferenceSelected.Location = new System.Drawing.Point(80, 12);
            this.lblConferenceSelected.Name = "lblConferenceSelected";
            this.lblConferenceSelected.Size = new System.Drawing.Size(304, 16);
            this.lblConferenceSelected.TabIndex = 15;
            this.lblConferenceSelected.Text = "<< No conference selected >>";
            this.lblConferenceSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbPlayBack
            // 
            this.tbPlayBack.Enabled = false;
            this.tbPlayBack.Location = new System.Drawing.Point(36, 72);
            this.tbPlayBack.Maximum = 1000;
            this.tbPlayBack.Name = "tbPlayBack";
            this.tbPlayBack.Size = new System.Drawing.Size(328, 45);
            this.tbPlayBack.TabIndex = 18;
            this.tbPlayBack.TickFrequency = 50;
            this.tbPlayBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbPlayBack_MouseDown);
            this.tbPlayBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbPlayBack_MouseUp);
            // 
            // lblStartTime
            // 
            this.lblStartTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblStartTime.Location = new System.Drawing.Point(12, 112);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(96, 16);
            this.lblStartTime.TabIndex = 19;
            this.lblStartTime.Text = "00:00:00";
            // 
            // lblEndTime
            // 
            this.lblEndTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblEndTime.Location = new System.Drawing.Point(268, 112);
            this.lblEndTime.Name = "lblEndTime";
            this.lblEndTime.Size = new System.Drawing.Size(116, 16);
            this.lblEndTime.TabIndex = 20;
            this.lblEndTime.Text = "<< End Time >>";
            this.lblEndTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblConferenceDetails1
            // 
            this.lblConferenceDetails1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblConferenceDetails1.Location = new System.Drawing.Point(16, 36);
            this.lblConferenceDetails1.Name = "lblConferenceDetails1";
            this.lblConferenceDetails1.Size = new System.Drawing.Size(368, 16);
            this.lblConferenceDetails1.TabIndex = 22;
            this.lblConferenceDetails1.Text = "<< No details >>";
            // 
            // timerSliderUpdate
            // 
            this.timerSliderUpdate.Interval = 1000;
            this.timerSliderUpdate.Tick += new System.EventHandler(this.timerSliderUpdate_Tick);
            // 
            // gbHorizontalSeparation1
            // 
            this.gbHorizontalSeparation1.Location = new System.Drawing.Point(16, 56);
            this.gbHorizontalSeparation1.Name = "gbHorizontalSeparation1";
            this.gbHorizontalSeparation1.Size = new System.Drawing.Size(368, 8);
            this.gbHorizontalSeparation1.TabIndex = 25;
            this.gbHorizontalSeparation1.TabStop = false;
            // 
            // gbHorizontalSeparation2
            // 
            this.gbHorizontalSeparation2.Location = new System.Drawing.Point(16, 168);
            this.gbHorizontalSeparation2.Name = "gbHorizontalSeparation2";
            this.gbHorizontalSeparation2.Size = new System.Drawing.Size(368, 8);
            this.gbHorizontalSeparation2.TabIndex = 26;
            this.gbHorizontalSeparation2.TabStop = false;
            // 
            // lblRewind
            // 
            this.lblRewind.Enabled = false;
            this.lblRewind.Location = new System.Drawing.Point(16, 76);
            this.lblRewind.Name = "lblRewind";
            this.lblRewind.Size = new System.Drawing.Size(14, 18);
            this.lblRewind.TabIndex = 31;
            this.lblRewind.Click += new System.EventHandler(this.lblRewind_Click);
            // 
            // lblFF
            // 
            this.lblFF.Enabled = false;
            this.lblFF.Location = new System.Drawing.Point(368, 76);
            this.lblFF.Name = "lblFF";
            this.lblFF.Size = new System.Drawing.Size(14, 18);
            this.lblFF.TabIndex = 32;
            this.lblFF.Click += new System.EventHandler(this.lblFF_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Image = ((System.Drawing.Image)(resources.GetObject("btnPlay.Image")));
            this.btnPlay.ImageType = MSR.LST.MediaControlImage.Play;
            this.btnPlay.Location = new System.Drawing.Point(16, 136);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(24, 24);
            this.btnPlay.TabIndex = 33;
            this.buttonToolTips.SetToolTip(this.btnPlay, "Play");
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblCurrentTime.Location = new System.Drawing.Point(268, 144);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(116, 16);
            this.lblCurrentTime.TabIndex = 34;
            this.lblCurrentTime.Text = "00:00:00";
            this.lblCurrentTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // imageListRewindFFButtons
            // 
            this.imageListRewindFFButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListRewindFFButtons.ImageStream")));
            this.imageListRewindFFButtons.TransparentColor = System.Drawing.Color.White;
            this.imageListRewindFFButtons.Images.SetKeyName(0, "");
            this.imageListRewindFFButtons.Images.SetKeyName(1, "");
            this.imageListRewindFFButtons.Images.SetKeyName(2, "");
            this.imageListRewindFFButtons.Images.SetKeyName(3, "");
            // 
            // btnStop
            // 
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageType = MSR.LST.MediaControlImage.Stop;
            this.btnStop.Location = new System.Drawing.Point(48, 136);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(24, 24);
            this.btnStop.TabIndex = 35;
            this.buttonToolTips.SetToolTip(this.btnStop, "Stop");
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // frmArchiveClient
            // 
            this.AcceptButton = this.btnCancel;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(402, 232);
            this.ControlBox = false;
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lblCurrentTime);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.lblFF);
            this.Controls.Add(this.lblRewind);
            this.Controls.Add(this.gbHorizontalSeparation2);
            this.Controls.Add(this.gbHorizontalSeparation1);
            this.Controls.Add(this.lblConferenceDetails1);
            this.Controls.Add(this.lblEndTime);
            this.Controls.Add(this.lblStartTime);
            this.Controls.Add(this.tbPlayBack);
            this.Controls.Add(this.lblConferenceSelected);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblConferenceHeader);
            this.Font = UIFont.FormFont;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmArchiveClient";
            this.Text = "Conference Playback";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmArchiveClient_Closing);
            this.Load += new System.EventHandler(this.frmArchiveClient_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbPlayBack)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Statics

        private enum ImageListPlayStopButtonsEnum {PlayEnabled, PlayDisabled, StopEnabled, StopDisabled};
        private enum ImageListRewindFFButtonsEnum {RewindEnabled, RewindDisabled, FFEnabled, FFDisabled};

        internal readonly string GenericPlaybackError = Strings.ArchiveServiceErrorText;
        internal readonly string GenericPlaybackErrorTitle = Strings.ArchiveServiceErrorTitle;

        #endregion Statics

        #region Members

        private IArchiveServer archiver;
        private ArchiveService.Conference conference;

        private System.Net.IPEndPoint playBackIP;

        // Reference to BarUI form used to communicate between the Play Back
        // forms and BarUI. It is set in the ctor of frmArchiveConf class.
        private FMain refFMain = null; 

        // Ditto for the frmArchiveConf form
        private frmArchiveConf refFrmArchiveConf = null;

        // Allows to store at what time the playback started to calculate the
        // position of the slider based on this information
        private long startPlayBackTicks;
        // Duration of conference
        readonly TimeSpan totalDuration = TimeSpan.Zero;
        private int sliderRange;
        private double ratioTickBySlider;

        // TODO: This is a ugly fix stop/start playback. This will be changed when we
        //       reorganize the code.
        private bool archiverStopped = false;
        private System.Windows.Forms.Label lblConferenceHeader;

        private int[] Streams;

        // Allow to handle differently cases when an server occurs after the playback
        // has been selected, but before playing back (i.e. no need to stop the play back
        // on form closing)
        private bool serverErrorOccurred = false;

        #endregion Members

        #region Ctor / Dispose

        public frmArchiveClient(IArchiveServer archiver, ArchiveService.Conference conference, int[] streams, FMain refFMain, frmArchiveConf refFrmArchiveConf)
        {
            InitializeComponent();

            // Duration of the current conference
            totalDuration = conference.End - conference.Start;

            sliderRange = tbPlayBack.Maximum - tbPlayBack.Minimum;
            ratioTickBySlider = totalDuration.Ticks / sliderRange;

            Streams = streams;

            this.archiver = archiver;
            this.conference = conference;
            this.refFMain = refFMain;
            this.refFrmArchiveConf = refFrmArchiveConf;
        }

        #endregion Ctor / Dispose

        #region UI Tasks

        /// <summary>
        /// Format the duration in [DD:][HH:]MM:SS
        /// </summary>
        /// <param name="duration">The duration</param>
        /// <returns>Formatted duration string</returns>
        private static string formatDuration(TimeSpan duration)
        {
            // This is a pretty bad way of doing it, but there doesn't appear to be a better way
            string durString = string.Empty;

            if (duration.TotalDays > 1.0) // only show the days if there are any
            {
                durString += duration.Days.ToString("000", CultureInfo.InvariantCulture) + ':';
            }

            if (duration.Hours > 0) // only show the hours if there are any
            {
                durString += duration.Hours.ToString("00", CultureInfo.InvariantCulture) + ':';
            }

            durString += duration.Minutes.ToString("00", CultureInfo.InvariantCulture) + ":" + 
                duration.Seconds.ToString("00", CultureInfo.InvariantCulture);

            return durString;
        }

        /// <summary>
        /// Uses the overall duration of the playback to determine what the start time should appear as
        /// (i.e. with or without days and with or without hours)
        /// </summary>
        private static string getStartTime(TimeSpan playbackDuration)
        {
            if (playbackDuration.TotalDays > 1.0)
                return "000:00:00:00";
            else if (playbackDuration.Hours > 0)
                return "00:00:00";
            else
                return "00:00";
        }

        private void frmArchiveClient_Load(object sender, System.EventArgs e) 
        {
            this.btnCancel.Font = UIFont.StringFont;
            this.lblConferenceHeader.Font = UIFont.StringFont;
            this.lblConferenceSelected.Font = UIFont.StringFont;
            this.lblStartTime.Font = UIFont.StringFont;
            this.lblEndTime.Font = UIFont.StringFont;
            this.lblConferenceDetails1.Font = UIFont.StringFont;
            this.lblRewind.Font = new System.Drawing.Font("Arial Narrow", UIFont.Size * 1.58F, System.Drawing.FontStyle.Bold);
            this.lblFF.Font = new System.Drawing.Font("Arial Narrow", UIFont.Size * 1.58F, System.Drawing.FontStyle.Bold);
            this.lblCurrentTime.Font = UIFont.StringFont;

            this.time.Text = Strings.Duration.ToLower(CultureInfo.CurrentCulture);
            this.btnCancel.Text = Strings.Close;
            this.lblConferenceHeader.Text = Strings.Conference;
            this.lblConferenceSelected.Text = Strings.NoConferenceSelected;
            this.lblStartTime.Text = string.Format(CultureInfo.InvariantCulture, "00:00:00");  // use string.Format to avoid CA1303 error
            this.lblEndTime.Text = Strings.EndTime;
            this.lblConferenceDetails1.Text = Strings.NoDetails;
            this.buttonToolTips.SetToolTip(this.btnPlay, Strings.Play);
            this.lblCurrentTime.Text = string.Format(CultureInfo.InvariantCulture, "00:00:00");  // use string.Format to avoid CA1303 error
            this.buttonToolTips.SetToolTip(this.btnStop, Strings.Stop);
            this.Text = Strings.ConferencePlayback;

            // Populate the information in the UI
            lblConferenceSelected.Text = conference.Description;
            lblConferenceDetails1.Text = string.Format(CultureInfo.CurrentCulture, Strings.RecordedDateTime, 
                conference.Start.ToShortDateString(),conference.Start.ToLongTimeString());
            lblStartTime.Text = getStartTime(totalDuration);
            lblEndTime.Text = formatDuration(totalDuration);

            // Set default images for the Rewind and FF buttons
            lblRewind.Image = imageListRewindFFButtons.Images[(int)ImageListRewindFFButtonsEnum.RewindDisabled];
            lblFF.Image = imageListRewindFFButtons.Images[(int)ImageListRewindFFButtonsEnum.FFDisabled];

            // Hook up the btnLeaveConference event handler from BarUI to be able
            // to close this form (if open) when the participant leaves the venue

            // TODO: Instead we could have a hastable or list of forms on FMain and forms could add their
            //       ref to it when they are created. When the participant leaves the venue,
            //       we close all the forms referred in the hashtable.
            this.refFMain.btnLeaveConference.Click += new EventHandler(btnLeaveConference_Click);
            this.refFMain.menuActionsPlayback.Click += new EventHandler(menuActionsPlayback_Click);

            // Start the playback without waiting
            StartPlayBack();
        }

        /// <summary>
        /// Disable the slider as well as the related timer
        /// </summary>
        private void disablePlayBackUI()
        {
            tbPlayBack.Enabled = false;   
            tbPlayBack.Value = 0;
            timerSliderUpdate.Enabled =  false;
        }

        /// <summary>
        /// tbPlayBack MouseUp event handler, jump within the play back.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void tbPlayBack_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                // The user is jumping back and forth, in the playback

                // Translate the cursor position to a time to jump to (in ticks)
                long deltaJump = (long)(tbPlayBack.Value * ratioTickBySlider);
                long absJump =  conference.Start.Ticks + deltaJump;
                Debug.Assert(absJump < conference.End.Ticks);

                // Jump to the right place in the playback from Archive Service
                archiver.JumpTo(playBackIP, absJump);

                // Readjust the startPlayBackTicks value
                startPlayBackTicks = DateTime.Now.Ticks - deltaJump;
                // Note: The difference should never be negative (unless 
                //       calculation error in deltaJump) since
                //       ticks are 100-nanosecond intervals that have 
                //       elapsed since 12:00 A.M., January 1, 0001
                //       There are 10 million ticks per second (TimeSpan.TicksPerSecond) 

                // Re-enable the timer (disabled on MouseDown event)
                timerSliderUpdate.Enabled = true;
            }
            catch (ArgumentException argEx)
            {
                // TODO: Find a better message
                // "There is a problem moving in the play back. Please ensure that the play back is currently running." 
                // + System.Environment.NewLine + "Message: " + 
                RtlAwareMessageBox.Show(this, argEx.Message, Strings.ArchivePlayBackTitle, MessageBoxButtons.OK, 
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                disablePlayBackUI();
            }        
            catch (Exception ex)
            {
                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                    Strings.ArchivePlayBackText, ex.ToString()), Strings.ArchivePlayBackTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 
                    (MessageBoxOptions)0);
                disablePlayBackUI();
            }
        }

        /// <summary>
        /// Update the slider position
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void timerSliderUpdate_Tick(object sender, System.EventArgs e)
        {
            // Find how far we are on the playback
            long currentPlayBackTimeTicks = DateTime.Now.Ticks - startPlayBackTicks;

            // Update the cursor position accordingly
            int cursorPosition = (int)(currentPlayBackTimeTicks * (1 / ratioTickBySlider));

            if ((cursorPosition <= tbPlayBack.Maximum) && (currentPlayBackTimeTicks <= totalDuration.Ticks))
            {
                tbPlayBack.Value = cursorPosition;
                lblCurrentTime.Text = formatDuration(new TimeSpan(currentPlayBackTimeTicks));
            } 
            else
            {
                // TODO: A better solution would be to have an event coming from
                //       the playback server when finished instead of counting on 
                //       the slider value

                timerSliderUpdate.Enabled = false;
                refFMain.StopPlayBack();
                InitPlayBackUI();
                archiverStopped = true;               
            }
        }

        /// <summary>
        /// Initialize the play back buttons.
        /// </summary>
        private void InitializePlayBackButtons()
        {
            // TODO: I have a method that only calls another method, 
            //       but there might be additional things to do here
            UpdatePlayButtons(true);
        }

        /// <summary>
        /// Update the play buttons and the Rewind/FF buttons
        /// </summary>
        /// <param name="Play">true = play, false = stop</param>
        private void UpdatePlayButtons(bool Play)
        {
            if (Play)
            {
                btnPlay.Enabled = false;
                btnStop.Enabled = true;
 
                // TODO: I implemented Rewind and FF, right after that the spec changed to not implement them. 
                //       However, I left the code and disabled Rewind and FF for now in case we decide to 
                //       renable Rewind and FF
                // lblRewind.Enabled = true;
                // lblFF.Enabled = true;
                // lblRewind.Image = imageListRewindFFButtons.Images[(int)ImageListRewindFFButtonsEnum.RewindEnabled];
                // lblFF.Image = imageListRewindFFButtons.Images[(int)ImageListRewindFFButtonsEnum.FFEnabled];
            } 
            else
            {
                btnPlay.Enabled = true;
                btnStop.Enabled = false;

                // TODO: I implemented Rewind and FF, right after that the spec changed to not implement them. 
                //       However, I left the code and disabled Rewind and FF for now in case we decide to 
                //       renable Rewind and FF
                // lblRewind.Enabled = false;
                // lblFF.Enabled = false;
                // lblRewind.Image = imageListRewindFFButtons.Images[(int)ImageListRewindFFButtonsEnum.RewindDisabled];
                // lblFF.Image = imageListRewindFFButtons.Images[(int)ImageListRewindFFButtonsEnum.FFDisabled];
            }
        }

        /// <summary>
        /// Get a new archiver if needed and start the playback.
        /// </summary>
        private void StartPlayBack()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (archiverStopped)
                {
                    refFMain.GetNewArchiver();
                }

                // Launch the Play Back in the appropriate venue
                // TODO: Move PlayArchive out of CXPClient
                playBackIP = refFMain.PlayArchive(Streams, conference);

                startPlayBackTicks = DateTime.Now.Ticks;

                // Enable the slider related UI
                tbPlayBack.Enabled = true;
                timerSliderUpdate.Enabled = true;

                UpdatePlayButtons(true);
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                ShowPlaybackError(string.Format(CultureInfo.CurrentCulture, Strings.ArchiveServiceUnavailableText, 
                    ex.Message), Strings.ArchiveServiceUnavailableTitle);
            }
            catch (ArchiveService.VenueInUseException)
            {
                ShowPlaybackError(string.Format(CultureInfo.CurrentCulture, Strings.PlaybackErrorText,
                    Strings.VenueInUseError), Strings.PlaybackErrorTitle);
            }
            catch (InvalidOperationException ex)
            {
                ShowPlaybackError(string.Format(CultureInfo.CurrentCulture, Strings.PlaybackErrorText, 
                    ex.Message), Strings.PlaybackErrorTitle);
            }
            catch (ArgumentException ex) // IPv6 error
            {
                string errorMsg = ex.Message;
                if (errorMsg.IndexOf("IPv6") > -1)
                {
                    ShowPlaybackError(errorMsg, Strings.AddressProtocolError);
                }
                else
                {
                    string error = GenericPlaybackError + ex.ToString();
                    ShowPlaybackError(error, GenericPlaybackErrorTitle);
                }
            }
            catch (Exception ex)
            {
                string error = GenericPlaybackError + ex.ToString();
                ShowPlaybackError(error, GenericPlaybackErrorTitle);
            }
            finally
            {
                this.Cursor = Cursors.Default;     
            }
        }

        private void ShowPlaybackError(string error, string title)
        {
            RtlAwareMessageBox.Show(this, error, title, MessageBoxButtons.OK, MessageBoxIcon.Error, 
                MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

            this.Close();

            serverErrorOccurred = true;
        }

        /// <summary>
        /// btnPlay click event handler. Starts the playback of the streams 
        /// selected in the UI.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void btnPlay_Click(object sender, System.EventArgs e)
        {   
            StartPlayBack();
        }

        /// <summary>
        /// btnLeaveConference Click event handler. The local participant is leaving 
        /// the venue by clicking "Leave Venue" on BarUI.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void btnLeaveConference_Click(object sender, EventArgs e)
        {
            // The participant is leaving the venue, so we can close this form            
            refFrmArchiveConf.Close();
            this.Close();
        }

        /// <summary>
        /// btnCancel Click event handler.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            refFMain.StopPlayBack();
            this.Close();
        }

        private void menuActionsPlayback_Click(object sender, EventArgs e)
        {
            // The participant is stopping archiver, so we can close this form            
            refFrmArchiveConf.Close();
            this.Close();
        }

        /// <summary>
        /// tbPlayBack MouseDown event handler, disable slider update timer.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void tbPlayBack_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Ensure we are not interrupted by the update timer during operations on the slider
            timerSliderUpdate.Enabled = false;
        }

        // TODO: Rewind is always disabled so this code is never used, but I left the code for now
        //       in case we decide to renable Rewind 
        /// <summary>
        /// lblRewind Click event handler, "rewind" the playback to the begining
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void lblRewind_Click(object sender, System.EventArgs e)
        {
            // Ensure we are not interrupted by the update timer during operations on the slider
            timerSliderUpdate.Enabled = false;
            tbPlayBack.Value = tbPlayBack.Minimum;
            lblCurrentTime.Text = lblStartTime.Text;
            // TODO: We should create an additional method used here and in the event handler instead 
            //       of calling directly an event handler
            // Note: The timer is re-enabled in tbPlayBack_MouseUp 
            tbPlayBack_MouseUp(this, null);
        }

        // TODO: FF is always disabled so this code is never used, but I left the code for now
        //       in case we decide to renable FF
        /// <summary>
        /// lblFF Click event handler, "FF" the playback to the end
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void lblFF_Click(object sender, System.EventArgs e)
        {
            // Ensure we are not interrupted by the update timer during operations on the slider
            timerSliderUpdate.Enabled = false;
            tbPlayBack.Value = tbPlayBack.Maximum;
            lblCurrentTime.Text = lblEndTime.Text;
            // TODO: We should create an additional method used here and in the event handler instead 
            //       of calling directly an event handler
            // Note: The timer is re-enabled in tbPlayBack_MouseUp 
            tbPlayBack_MouseUp(this, null);
        }

        /// <summary>
        /// Initialize the PlayBack UI to be ready to play (cursor at the begining,
        /// reset current time, play button enabled, etc.)
        /// </summary>
        private void InitPlayBackUI()
        {
            tbPlayBack.Enabled = false;
            tbPlayBack.Value = tbPlayBack.Minimum;
            timerSliderUpdate.Enabled = false;
            lblCurrentTime.Text = lblStartTime.Text;
            UpdatePlayButtons(false);
        }

        /// <summary>
        /// btnStop Click event handler.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void btnStop_Click(object sender, System.EventArgs e)
        {
            timerSliderUpdate.Enabled = false;
            refFMain.StopPlayBack();
            InitPlayBackUI();
            archiverStopped = true;
        }

        /// <summary>
        /// frmArchiveClient Closing event handler.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void frmArchiveClient_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timerSliderUpdate.Enabled = false;
            // Ensure the playback is stopped when the form is closing
            if (!serverErrorOccurred)
            {
                refFMain.StopPlayBack();
            } 

            // If we are in a unicast playback venue, then leave it
            if( Conference.ActiveVenue != null && Conference.ActiveVenue.Name.StartsWith("Playback") )
            {
                refFMain.LeaveConference();
            }
            // Reinitialize Archiver Menu Status
            this.refFMain.SetArchiverMenuStatus();
        }

        #endregion UI Tasks
    }
}
