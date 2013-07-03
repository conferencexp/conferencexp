using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Microsoft.Win32;

using MSR.LST.MDShow;
using System.Collections.Generic;


namespace MSR.LST.ConferenceXP
{
    public class frmAVDevices : System.Windows.Forms.Form
    {
        #region Windows Form Designer generated code

        private System.Windows.Forms.CheckBox chkAutoPlayVideo;
        private System.Windows.Forms.RadioButton rdbtnWindowTiled;
        private System.Windows.Forms.RadioButton rdbtnWindowFourWay;
        private System.Windows.Forms.RadioButton rdbtnWindowFullScreen;
        private System.Windows.Forms.Label lblVideoCameras;
        private System.Windows.Forms.Label lblMicrophone;
        private System.Windows.Forms.Button btnAdvancedVideoSettings;
        private System.Windows.Forms.Label lblSpeaker;
        private System.Windows.Forms.Button btnAdvancedAudioSettings;
        private System.Windows.Forms.CheckBox ckPlayAudio;
        private System.Windows.Forms.Label lblTestAudio;
        private System.Windows.Forms.CheckBox ckPlayVideo;
        private System.Windows.Forms.Label lblVideoInfo;
        private System.Windows.Forms.Button btnTroubleshooting;
        private System.Windows.Forms.CheckedListBox clbCameras;
        private System.Windows.Forms.ComboBox cboMicrophones;
        private System.Windows.Forms.ComboBox cboSpeakers;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Timer tmrPerf;
        private System.Windows.Forms.Label lblPerf;
        private System.Windows.Forms.CheckBox chkAutoPlayRemoteAudio;
        private System.Windows.Forms.CheckBox chkAutoPlayRemoteVideo;
        private System.Windows.Forms.GroupBox gbAutoPlay;
        private System.Windows.Forms.GroupBox gbAudioDevices;
        private System.Windows.Forms.GroupBox gbVideoDevices;
        private System.Windows.Forms.GroupBox gbWindowLayout;
        private System.Windows.Forms.GroupBox gbPerf;
        private Label label1;
        private ComboBox cboACompressor;
        private System.ComponentModel.IContainer components;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnClose = new System.Windows.Forms.Button();
            this.chkAutoPlayRemoteAudio = new System.Windows.Forms.CheckBox();
            this.gbAudioDevices = new System.Windows.Forms.GroupBox();
            this.btnAdvancedAudioSettings = new System.Windows.Forms.Button();
            this.cboSpeakers = new System.Windows.Forms.ComboBox();
            this.lblSpeaker = new System.Windows.Forms.Label();
            this.lblMicrophone = new System.Windows.Forms.Label();
            this.cboMicrophones = new System.Windows.Forms.ComboBox();
            this.ckPlayAudio = new System.Windows.Forms.CheckBox();
            this.lblTestAudio = new System.Windows.Forms.Label();
            this.gbVideoDevices = new System.Windows.Forms.GroupBox();
            this.clbCameras = new System.Windows.Forms.CheckedListBox();
            this.btnAdvancedVideoSettings = new System.Windows.Forms.Button();
            this.lblVideoCameras = new System.Windows.Forms.Label();
            this.ckPlayVideo = new System.Windows.Forms.CheckBox();
            this.lblVideoInfo = new System.Windows.Forms.Label();
            this.chkAutoPlayVideo = new System.Windows.Forms.CheckBox();
            this.rdbtnWindowFourWay = new System.Windows.Forms.RadioButton();
            this.rdbtnWindowFullScreen = new System.Windows.Forms.RadioButton();
            this.rdbtnWindowTiled = new System.Windows.Forms.RadioButton();
            this.gbWindowLayout = new System.Windows.Forms.GroupBox();
            this.gbPerf = new System.Windows.Forms.GroupBox();
            this.lblPerf = new System.Windows.Forms.Label();
            this.btnTroubleshooting = new System.Windows.Forms.Button();
            this.tmrPerf = new System.Windows.Forms.Timer(this.components);
            this.chkAutoPlayRemoteVideo = new System.Windows.Forms.CheckBox();
            this.gbAutoPlay = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboACompressor = new System.Windows.Forms.ComboBox();
            this.gbAudioDevices.SuspendLayout();
            this.gbVideoDevices.SuspendLayout();
            this.gbWindowLayout.SuspendLayout();
            this.gbPerf.SuspendLayout();
            this.gbAutoPlay.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClose.Location = new System.Drawing.Point(480, 296);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 23);
            this.btnClose.TabIndex = 37;
            this.btnClose.Text = "Close";
            // 
            // chkAutoPlayRemoteAudio
            // 
            this.chkAutoPlayRemoteAudio.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkAutoPlayRemoteAudio.Location = new System.Drawing.Point(8, 40);
            this.chkAutoPlayRemoteAudio.Name = "chkAutoPlayRemoteAudio";
            this.chkAutoPlayRemoteAudio.Size = new System.Drawing.Size(133, 16);
            this.chkAutoPlayRemoteAudio.TabIndex = 53;
            this.chkAutoPlayRemoteAudio.Text = "Others\' audio streams";
            // 
            // gbAudioDevices
            // 
            this.gbAudioDevices.Controls.Add(this.label1);
            this.gbAudioDevices.Controls.Add(this.cboACompressor);
            this.gbAudioDevices.Controls.Add(this.btnAdvancedAudioSettings);
            this.gbAudioDevices.Controls.Add(this.cboSpeakers);
            this.gbAudioDevices.Controls.Add(this.lblSpeaker);
            this.gbAudioDevices.Controls.Add(this.lblMicrophone);
            this.gbAudioDevices.Controls.Add(this.cboMicrophones);
            this.gbAudioDevices.Controls.Add(this.ckPlayAudio);
            this.gbAudioDevices.Controls.Add(this.lblTestAudio);
            this.gbAudioDevices.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbAudioDevices.Location = new System.Drawing.Point(296, 8);
            this.gbAudioDevices.Name = "gbAudioDevices";
            this.gbAudioDevices.Size = new System.Drawing.Size(280, 220);
            this.gbAudioDevices.TabIndex = 52;
            this.gbAudioDevices.TabStop = false;
            this.gbAudioDevices.Text = "Audio Settings";
            // 
            // btnAdvancedAudioSettings
            // 
            this.btnAdvancedAudioSettings.Enabled = false;
            this.btnAdvancedAudioSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdvancedAudioSettings.Location = new System.Drawing.Point(8, 157);
            this.btnAdvancedAudioSettings.Name = "btnAdvancedAudioSettings";
            this.btnAdvancedAudioSettings.Size = new System.Drawing.Size(117, 23);
            this.btnAdvancedAudioSettings.TabIndex = 71;
            this.btnAdvancedAudioSettings.Text = "Advanced Settings...";
            this.btnAdvancedAudioSettings.Click += new System.EventHandler(this.btnAdvancedAudioSettings_Click);
            // 
            // cboSpeakers
            // 
            this.cboSpeakers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSpeakers.Location = new System.Drawing.Point(8, 36);
            this.cboSpeakers.Name = "cboSpeakers";
            this.cboSpeakers.Size = new System.Drawing.Size(264, 21);
            this.cboSpeakers.Sorted = true;
            this.cboSpeakers.TabIndex = 47;
            this.cboSpeakers.SelectedIndexChanged += new System.EventHandler(this.cboSpeakers_SelectedIndexChanged);
            // 
            // lblSpeaker
            // 
            this.lblSpeaker.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblSpeaker.Location = new System.Drawing.Point(8, 20);
            this.lblSpeaker.Name = "lblSpeaker";
            this.lblSpeaker.Size = new System.Drawing.Size(88, 16);
            this.lblSpeaker.TabIndex = 46;
            this.lblSpeaker.Text = "Sound playback:";
            // 
            // lblMicrophone
            // 
            this.lblMicrophone.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblMicrophone.Location = new System.Drawing.Point(8, 66);
            this.lblMicrophone.Name = "lblMicrophone";
            this.lblMicrophone.Size = new System.Drawing.Size(96, 16);
            this.lblMicrophone.TabIndex = 45;
            this.lblMicrophone.Text = "Sound recording:";
            // 
            // cboMicrophones
            // 
            this.cboMicrophones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMicrophones.Location = new System.Drawing.Point(8, 82);
            this.cboMicrophones.Name = "cboMicrophones";
            this.cboMicrophones.Size = new System.Drawing.Size(264, 21);
            this.cboMicrophones.Sorted = true;
            this.cboMicrophones.TabIndex = 28;
            this.cboMicrophones.SelectedIndexChanged += new System.EventHandler(this.cboMicrophones_SelectedIndexChanged);
            // 
            // ckPlayAudio
            // 
            this.ckPlayAudio.Appearance = System.Windows.Forms.Appearance.Button;
            this.ckPlayAudio.Enabled = false;
            this.ckPlayAudio.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ckPlayAudio.Location = new System.Drawing.Point(192, 157);
            this.ckPlayAudio.Name = "ckPlayAudio";
            this.ckPlayAudio.Size = new System.Drawing.Size(80, 24);
            this.ckPlayAudio.TabIndex = 82;
            this.ckPlayAudio.Text = "Test Audio";
            this.ckPlayAudio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckPlayAudio.CheckedChanged += new System.EventHandler(this.ckPlayAudio_CheckedChanged);
            // 
            // lblTestAudio
            // 
            this.lblTestAudio.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblTestAudio.Location = new System.Drawing.Point(8, 185);
            this.lblTestAudio.Name = "lblTestAudio";
            this.lblTestAudio.Size = new System.Drawing.Size(264, 28);
            this.lblTestAudio.TabIndex = 81;
            // 
            // gbVideoDevices
            // 
            this.gbVideoDevices.Controls.Add(this.clbCameras);
            this.gbVideoDevices.Controls.Add(this.btnAdvancedVideoSettings);
            this.gbVideoDevices.Controls.Add(this.lblVideoCameras);
            this.gbVideoDevices.Controls.Add(this.ckPlayVideo);
            this.gbVideoDevices.Controls.Add(this.lblVideoInfo);
            this.gbVideoDevices.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbVideoDevices.Location = new System.Drawing.Point(8, 8);
            this.gbVideoDevices.Name = "gbVideoDevices";
            this.gbVideoDevices.Size = new System.Drawing.Size(280, 188);
            this.gbVideoDevices.TabIndex = 51;
            this.gbVideoDevices.TabStop = false;
            this.gbVideoDevices.Text = "Video Settings";
            // 
            // clbCameras
            // 
            this.clbCameras.HorizontalScrollbar = true;
            this.clbCameras.Location = new System.Drawing.Point(8, 36);
            this.clbCameras.Name = "clbCameras";
            this.clbCameras.Size = new System.Drawing.Size(256, 64);
            this.clbCameras.Sorted = true;
            this.clbCameras.TabIndex = 68;
            this.clbCameras.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbCameras_ItemCheck);
            this.clbCameras.SelectedIndexChanged += new System.EventHandler(this.clbCameras_SelectedIndexChanged);
            // 
            // btnAdvancedVideoSettings
            // 
            this.btnAdvancedVideoSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdvancedVideoSettings.Location = new System.Drawing.Point(8, 108);
            this.btnAdvancedVideoSettings.Name = "btnAdvancedVideoSettings";
            this.btnAdvancedVideoSettings.Size = new System.Drawing.Size(117, 23);
            this.btnAdvancedVideoSettings.TabIndex = 66;
            this.btnAdvancedVideoSettings.Text = "Advanced Settings...";
            this.btnAdvancedVideoSettings.Click += new System.EventHandler(this.btnAdvancedVideoSettings_Click);
            // 
            // lblVideoCameras
            // 
            this.lblVideoCameras.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblVideoCameras.Location = new System.Drawing.Point(8, 20);
            this.lblVideoCameras.Name = "lblVideoCameras";
            this.lblVideoCameras.Size = new System.Drawing.Size(104, 16);
            this.lblVideoCameras.TabIndex = 68;
            this.lblVideoCameras.Text = "Select camera(s):";
            // 
            // ckPlayVideo
            // 
            this.ckPlayVideo.Appearance = System.Windows.Forms.Appearance.Button;
            this.ckPlayVideo.Enabled = false;
            this.ckPlayVideo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ckPlayVideo.Location = new System.Drawing.Point(184, 108);
            this.ckPlayVideo.Name = "ckPlayVideo";
            this.ckPlayVideo.Size = new System.Drawing.Size(80, 24);
            this.ckPlayVideo.TabIndex = 75;
            this.ckPlayVideo.Text = "Test Video";
            this.ckPlayVideo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckPlayVideo.CheckedChanged += new System.EventHandler(this.ckPlayVideo_CheckedChanged);
            // 
            // lblVideoInfo
            // 
            this.lblVideoInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblVideoInfo.Location = new System.Drawing.Point(8, 140);
            this.lblVideoInfo.Name = "lblVideoInfo";
            this.lblVideoInfo.Size = new System.Drawing.Size(260, 42);
            this.lblVideoInfo.TabIndex = 84;
            // 
            // chkAutoPlayVideo
            // 
            this.chkAutoPlayVideo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkAutoPlayVideo.Location = new System.Drawing.Point(8, 20);
            this.chkAutoPlayVideo.Name = "chkAutoPlayVideo";
            this.chkAutoPlayVideo.Size = new System.Drawing.Size(128, 16);
            this.chkAutoPlayVideo.TabIndex = 65;
            this.chkAutoPlayVideo.Text = "My video stream(s)";
            // 
            // rdbtnWindowFourWay
            // 
            this.rdbtnWindowFourWay.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdbtnWindowFourWay.Location = new System.Drawing.Point(8, 40);
            this.rdbtnWindowFourWay.Name = "rdbtnWindowFourWay";
            this.rdbtnWindowFourWay.Size = new System.Drawing.Size(69, 16);
            this.rdbtnWindowFourWay.TabIndex = 70;
            this.rdbtnWindowFourWay.Text = "Four-way";
            // 
            // rdbtnWindowFullScreen
            // 
            this.rdbtnWindowFullScreen.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdbtnWindowFullScreen.Location = new System.Drawing.Point(8, 60);
            this.rdbtnWindowFullScreen.Name = "rdbtnWindowFullScreen";
            this.rdbtnWindowFullScreen.Size = new System.Drawing.Size(72, 16);
            this.rdbtnWindowFullScreen.TabIndex = 71;
            this.rdbtnWindowFullScreen.Text = "Full screen";
            // 
            // rdbtnWindowTiled
            // 
            this.rdbtnWindowTiled.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rdbtnWindowTiled.Location = new System.Drawing.Point(8, 20);
            this.rdbtnWindowTiled.Name = "rdbtnWindowTiled";
            this.rdbtnWindowTiled.Size = new System.Drawing.Size(48, 16);
            this.rdbtnWindowTiled.TabIndex = 72;
            this.rdbtnWindowTiled.Text = "Tiled";
            // 
            // gbWindowLayout
            // 
            this.gbWindowLayout.Controls.Add(this.rdbtnWindowFullScreen);
            this.gbWindowLayout.Controls.Add(this.rdbtnWindowFourWay);
            this.gbWindowLayout.Controls.Add(this.rdbtnWindowTiled);
            this.gbWindowLayout.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbWindowLayout.Location = new System.Drawing.Point(176, 204);
            this.gbWindowLayout.Name = "gbWindowLayout";
            this.gbWindowLayout.Size = new System.Drawing.Size(112, 80);
            this.gbWindowLayout.TabIndex = 73;
            this.gbWindowLayout.TabStop = false;
            this.gbWindowLayout.Text = "Window Layout";
            // 
            // gbPerf
            // 
            this.gbPerf.Controls.Add(this.lblPerf);
            this.gbPerf.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbPerf.Location = new System.Drawing.Point(296, 234);
            this.gbPerf.Name = "gbPerf";
            this.gbPerf.Size = new System.Drawing.Size(280, 46);
            this.gbPerf.TabIndex = 74;
            this.gbPerf.TabStop = false;
            this.gbPerf.Text = "Resource Utilization";
            // 
            // lblPerf
            // 
            this.lblPerf.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblPerf.Location = new System.Drawing.Point(8, 15);
            this.lblPerf.Name = "lblPerf";
            this.lblPerf.Size = new System.Drawing.Size(264, 30);
            this.lblPerf.TabIndex = 85;
            this.lblPerf.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnTroubleshooting
            // 
            this.btnTroubleshooting.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnTroubleshooting.Location = new System.Drawing.Point(296, 296);
            this.btnTroubleshooting.Name = "btnTroubleshooting";
            this.btnTroubleshooting.Size = new System.Drawing.Size(128, 24);
            this.btnTroubleshooting.TabIndex = 75;
            this.btnTroubleshooting.Text = "Troubleshooting Log...";
            this.btnTroubleshooting.Click += new System.EventHandler(this.btnTroubleshooting_Click);
            // 
            // tmrPerf
            // 
            this.tmrPerf.Interval = 2000;
            this.tmrPerf.Tick += new System.EventHandler(this.tmrPerf_Tick);
            // 
            // chkAutoPlayRemoteVideo
            // 
            this.chkAutoPlayRemoteVideo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkAutoPlayRemoteVideo.Location = new System.Drawing.Point(8, 60);
            this.chkAutoPlayRemoteVideo.Name = "chkAutoPlayRemoteVideo";
            this.chkAutoPlayRemoteVideo.Size = new System.Drawing.Size(133, 16);
            this.chkAutoPlayRemoteVideo.TabIndex = 76;
            this.chkAutoPlayRemoteVideo.Text = "Others\' video streams";
            // 
            // gbAutoPlay
            // 
            this.gbAutoPlay.Controls.Add(this.chkAutoPlayVideo);
            this.gbAutoPlay.Controls.Add(this.chkAutoPlayRemoteAudio);
            this.gbAutoPlay.Controls.Add(this.chkAutoPlayRemoteVideo);
            this.gbAutoPlay.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbAutoPlay.Location = new System.Drawing.Point(8, 204);
            this.gbAutoPlay.Name = "gbAutoPlay";
            this.gbAutoPlay.Size = new System.Drawing.Size(152, 80);
            this.gbAutoPlay.TabIndex = 77;
            this.gbAutoPlay.TabStop = false;
            this.gbAutoPlay.Text = "Auto-play";
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(8, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 16);
            this.label1.TabIndex = 84;
            this.label1.Text = "Audio Compressor:";
            // 
            // cboACompressor
            // 
            this.cboACompressor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboACompressor.Location = new System.Drawing.Point(8, 128);
            this.cboACompressor.Name = "cboACompressor";
            this.cboACompressor.Size = new System.Drawing.Size(264, 21);
            this.cboACompressor.TabIndex = 83;
            this.cboACompressor.SelectedIndexChanged += new System.EventHandler(this.cboACompressor_SelectedIndexChanged);
            // 
            // frmAVDevices
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(586, 328);
            this.ControlBox = false;
            this.Controls.Add(this.gbAutoPlay);
            this.Controls.Add(this.btnTroubleshooting);
            this.Controls.Add(this.gbPerf);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gbWindowLayout);
            this.Controls.Add(this.gbAudioDevices);
            this.Controls.Add(this.gbVideoDevices);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAVDevices";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Audio/Video Settings";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmAVDevices_Closing);
            this.Load += new System.EventHandler(this.frmAVDevices_Load);
            this.gbAudioDevices.ResumeLayout(false);
            this.gbVideoDevices.ResumeLayout(false);
            this.gbWindowLayout.ResumeLayout(false);
            this.gbPerf.ResumeLayout(false);
            this.gbAutoPlay.ResumeLayout(false);
            this.ResumeLayout(false);

        }

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

        #endregion

        #region Members

        private RegistryKey cxpclientRegKey = null;
        internal Dictionary<FilterInfo, VideoCapability> videoCapabilities;
        internal AudioCapability audioCapability = null;

        private PerformanceCounter pcCPU;
        private PerformanceCounter pcMem;

        private frmAVTroubleshooting trouble;

        #endregion Members

        #region Constructor

        public frmAVDevices(RegistryKey cxpclientkey)
        {
            InitializeComponent();

            videoCapabilities = new Dictionary<FilterInfo, VideoCapability>();

            cxpclientRegKey = cxpclientkey;

            if (BasePC.PerformanceCounterWrapper.HasUserPermissions())
            {
                if (PerformanceCounterCategory.Exists("Processor") &&
                    PerformanceCounterCategory.CounterExists("% Processor Time", "Processor") &&
                    PerformanceCounterCategory.InstanceExists("_Total", "Processor"))
                {
                    pcCPU = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                }

                if (PerformanceCounterCategory.Exists("Process") &&
                    PerformanceCounterCategory.CounterExists("Working Set", "Process") &&
                    PerformanceCounterCategory.InstanceExists(
                        System.Reflection.Assembly.GetEntryAssembly().GetName(false).Name, "Process"))
                {
                    pcMem = new PerformanceCounter("Process", "Working Set",
                        System.Reflection.Assembly.GetEntryAssembly().GetName(false).Name);
                }
            }
        }

        #endregion Constructor

        #region Load

        private void frmAVDevices_Load(object sender, System.EventArgs e) {
            //Initialize text fonts and strings
            InitText();

            //Initialize play local/remote and video window tiling checkboxes
            InitializeCXPValues();

            // Perf thread: Update memory/CPU display every couple of seconds.
            UpdatePerfLabel();
            if(pcCPU != null || pcMem != null)
            {
                tmrPerf.Start();
            }

            // Create the form we will be logging to
            trouble = new frmAVTroubleshooting();

            try
            {
                // Populate device lists
                DiscoverDevices();

                // Show the form before initializing devices.  The idea is that since we need
                // to build a graph to get the video properties, it might take some time.  In
                // practice I don't see it taking long at all for cameras I have tried.
                this.Text = Strings.InitializingAVDevices;
                this.Cursor = Cursors.WaitCursor;
                Show();
                Refresh();

                // Restore any previously selected devices
                RestoreAudio();
                RestoreVideo();
            }
            finally
            {
                this.Text = Strings.AudioVideoSettings;
                this.Cursor = Cursors.Default;
            }
        }

        private void frmAVDevices_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();


            if(audioCapability != null)
            {
                audioCapability.Dispose();
            }

            foreach (VideoCapability vc in videoCapabilities.Values) {
                vc.Dispose();
            }

            if (cboMicrophones.SelectedIndex != 0) {
                // If a camera hasn't been linked, link the first one
                if (clbCameras.CheckedItems.Count > 0) {
                    bool goodLink = false;

                    // Read from registry and select camera, if in list
                    string[] linkedCamera = AVReg.ValueNames(AVReg.LinkedCamera);
                    if (linkedCamera != null && linkedCamera.Length > 0) {
                        Debug.Assert(linkedCamera.Length == 1);

                        for (int i = 0; i < clbCameras.CheckedItems.Count; i++) {
                            FilterInfo fi = (FilterInfo)clbCameras.CheckedItems[i];
                            if (fi.Moniker == linkedCamera[0]) {
                                goodLink = true;
                                break;
                            }
                        }
                    }

                    if (!goodLink) {
                        SaveLinkedCamera((FilterInfo)clbCameras.CheckedItems[0]);
                    }
                }
            }

            if(ckPlayVideo.Checked)
            {
                // To clean up any video windows
                GC.Collect();
            }

            tmrPerf.Stop();
            trouble.Close();
        }

        /// <summary>
        /// Initialize Fonts and Strings on the form
        /// </summary>
        private void InitText()
        {
            this.btnClose.Font = UIFont.StringFont;
            this.gbAudioDevices.Font = UIFont.StringFont;
            this.gbVideoDevices.Font = UIFont.StringFont;
            this.gbWindowLayout.Font = UIFont.StringFont;
            this.gbPerf.Font = UIFont.StringFont;
            this.btnTroubleshooting.Font = UIFont.StringFont;
            this.gbAutoPlay.Font = UIFont.StringFont;

            this.btnClose.Text = Strings.Close;
            this.chkAutoPlayRemoteAudio.Text = Strings.OthersAudioStreams;
            this.gbAudioDevices.Text = Strings.AudioSettings;
            this.btnAdvancedAudioSettings.Text = Strings.AdvancedSettings;
            this.lblSpeaker.Text = Strings.SoundPlayback;
            this.lblMicrophone.Text = Strings.SoundRecording;
            this.ckPlayAudio.Text = Strings.TestAudio;
            this.gbVideoDevices.Text = Strings.VideoSettings;
            this.btnAdvancedVideoSettings.Text = Strings.AdvancedSettings;
            this.lblVideoCameras.Text = Strings.SelectCameras;
            this.ckPlayVideo.Text = Strings.TestVideo;
            this.chkAutoPlayVideo.Text = Strings.MyVideoStreams;
            this.rdbtnWindowFourWay.Text = Strings.FourWay;
            this.rdbtnWindowFullScreen.Text = Strings.FullScreen;
            this.rdbtnWindowTiled.Text = Strings.Tiled;
            this.gbWindowLayout.Text = Strings.WindowLayout;
            this.gbPerf.Text = Strings.ResourceUtilization;
            this.btnTroubleshooting.Text = Strings.TroubleshootingLog;
            this.chkAutoPlayRemoteVideo.Text = Strings.OthersVideoStreams;
            this.gbAutoPlay.Text = Strings.AutoPlay;
            this.Text = Strings.AudioVideoSettings;
            this.lblTestAudio.Text = Strings.TestingAudioWarning;
        }

        /// <summary>
        /// Initialize the play local/remote and video window tiling checkboxes
        /// </summary>
        private void InitializeCXPValues()
        {
            // Load previously saved or default settings
            chkAutoPlayVideo.Checked = Conference.AutoPlayLocal;
            chkAutoPlayRemoteAudio.Checked = FMain.AutoPlayRemoteAudio;
            chkAutoPlayRemoteVideo.Checked = FMain.AutoPlayRemoteVideo;

            // Only one of these will be true
            rdbtnWindowFourWay.Checked = (Conference.AutoPosition == Conference.AutoPositionMode.FourWay);
            rdbtnWindowTiled.Checked = (Conference.AutoPosition == Conference.AutoPositionMode.Tiled);
            rdbtnWindowFullScreen.Checked = (Conference.AutoPosition == Conference.AutoPositionMode.FullScreen);
        }

        /// <summary>
        /// Populate the UI elements displaying video and audio sources, and audio output devices
        /// </summary>
        private void DiscoverDevices()
        {
            Log("Cameras");
            foreach (FilterInfo fi in VideoSource.Sources())
            {
                clbCameras.Items.Add(fi);
                Log(string.Format(CultureInfo.CurrentCulture, "{0}, {1}", fi.DisplayName, fi.Moniker));
            }

            Log("\r\nMicrophones");
            // Add a blank so they can unselect the microphone
            cboMicrophones.Items.Add(Strings.None);
            foreach(FilterInfo fi in AudioSource.Sources())
            {
                cboMicrophones.Items.Add(fi);
                Log(string.Format(CultureInfo.CurrentCulture, "{0}, {1}", fi.DisplayName, fi.Moniker));
            }

            Log("\r\nSpeakers");
            foreach (FilterInfo fi in AudioRenderer.Renderers()) {
                cboSpeakers.Items.Add(fi);
                Log(string.Format(CultureInfo.CurrentCulture, "{0}, {1}", fi.DisplayName, fi.Moniker));
            } 
            
            Log("\r\nAudio Compressors");
            foreach (FilterInfo fi in AudioCompressor.EnabledCompressors) {
                cboACompressor.Items.Add(fi);
                Log(string.Format(CultureInfo.CurrentCulture, "{0}, {1}", fi.DisplayName, fi.Moniker));
            }
            cboACompressor.Items.Add(new FilterInfo("Uncompressed", "Uncompressed", Guid.Empty));
        }

        /// <summary>
        /// Set up checkbox state for video source devices
        /// </summary>
        private void RestoreVideo()
        {
            // Get the UI set up correctly in the event there are no selected cameras
            ClearVideoBox();

            int idx = -1;
            foreach(FilterInfo fi in VideoCapability.SelectedCameras())
            {
                idx = clbCameras.Items.IndexOf(fi);
                clbCameras.SetItemChecked(idx, true);
            }

            //The last checked item is selected.
            if (idx != -1) {
                clbCameras.SetSelected(idx, true);
            }
        }

        /// <summary>
        /// Restore checkbox state for currently selected audio source and playback device
        /// </summary>
        private void RestoreAudio()
        {
            foreach(FilterInfo fi in AudioCapability.SelectedMicrophones())
            {
                cboMicrophones.SelectedIndex = cboMicrophones.Items.IndexOf(fi);
            }

            if(cboMicrophones.SelectedIndex == -1)
            {
                cboMicrophones.SelectedIndex = 0;
            }

            cboSpeakers.SelectedIndex = cboSpeakers.Items.IndexOf(AudioCapability.SelectedSpeaker());

            cboACompressor.SelectedIndex = cboACompressor.Items.IndexOf(AudioCapability.SelectedCompressor());
        }

        #endregion Load

        #region Perf

        private void tmrPerf_Tick(object sender, System.EventArgs e)
        {
            UpdatePerfLabel();
        }

        /// <summary>
        /// Update the current memory and CPU values
        /// </summary>
        private void UpdatePerfLabel()
        {
            string cpu = Strings.Unknown;
            if(pcCPU != null)
            {
                cpu = string.Format(CultureInfo.CurrentCulture, Strings.CPU, 
                    ((uint)pcCPU.NextValue()).ToString(CultureInfo.CurrentCulture));
            }

            string mem = Strings.Unknown;
            if(pcMem != null)
            {
                mem = ((uint)(pcMem.NextValue() / (1024 * 1024))).ToString(CultureInfo.CurrentCulture);
            }

            lblPerf.Text = string.Format(CultureInfo.CurrentCulture, Strings.MemoryCPUStatus, cpu, mem);
        }

        #endregion Perf

        #region Video

        /// <summary>
        /// Set buttons to initial state assuming no video cameras
        /// </summary>
        private void ClearVideoBox()
        {
            //The test video button is enabled when at least one item is checked.
            ckPlayVideo.Enabled = this.clbCameras.CheckedItems.Count > 0;
            btnAdvancedVideoSettings.Enabled = false;
            lblVideoInfo.Enabled = false;

            lblVideoInfo.Text = Strings.ResolutionHeading;
        }

        /// <summary>
        /// Create a VideoCapability for the specified camera, grab the data from the 
        /// capture graph, then dispose the capability.
        /// </summary>
        /// <param name="fi"></param>
        private void UpdateVideoBox(FilterInfo fi) {
            if (videoCapabilities.ContainsKey(fi)) {
                //The capability might exist if there is a video test underway.
                UpdateVideoBox(videoCapabilities[fi].CaptureGraph);
            }
            else {
                VideoCapability vc = new VideoCapability(fi);
                vc.SetLogger(new AVLogger(Log));
                vc.ActivateCamera();
                UpdateVideoBox(vc.CaptureGraph);
                vc.Dispose();
            }
        }

        /// <summary>
        /// Display statistics for the specified video capture graph.  It might be a DV graph.
        /// </summary>
        /// <param name="vcg"></param>
        internal void UpdateVideoBox(CaptureGraph vcg)
        {
            btnAdvancedVideoSettings.Enabled = true;
            lblVideoInfo.Enabled = true;
            ckPlayVideo.Enabled = true;

            // Update video info about the camera
            _AMMediaType mt;
            object formatBlock;
            vcg.Source.GetMediaType(out mt, out formatBlock);

            string info = null;
            if (formatBlock is VIDEOINFOHEADER) {
                VIDEOINFOHEADER vih = (VIDEOINFOHEADER)formatBlock;
                BITMAPINFOHEADER bmih = vih.BitmapInfo;
                info = string.Format(CultureInfo.CurrentCulture, Strings.ResolutionStatus,
                    bmih.Width, bmih.Height, vih.FrameRate.ToString("F2", CultureInfo.InvariantCulture));
            }
            else if (formatBlock is DVINFO) {
                info = "DV Video";
                DVCaptureGraph dvcg = vcg as DVCaptureGraph;
                if (dvcg != null) {
                    dvcg.GetVideoMediaType(out mt, out formatBlock);
                    if (formatBlock is VIDEOINFOHEADER) { 
                        VIDEOINFOHEADER vih = (VIDEOINFOHEADER)formatBlock;
                        BITMAPINFOHEADER bmih = vih.BitmapInfo;
                        info = string.Format(CultureInfo.CurrentCulture, Strings.ResolutionStatus,
                            bmih.Width, bmih.Height, vih.FrameRate.ToString("F2", CultureInfo.InvariantCulture));                       
                    }
                }
            }


            if(vcg.Compressor == null)
            {
                info += string.Format(CultureInfo.CurrentCulture, "\r\n" + Strings.CompressorDisabled);
            }
            else
            {
                if (vcg is VideoCaptureGraph) {
                    info += string.Format(CultureInfo.CurrentCulture, "\r\n" + Strings.CompressedBitRate,
                        ((VideoCaptureGraph)vcg).VideoCompressor.QualityInfo.BitRate / 1000);
                }
                else if (vcg is DVCaptureGraph) {
                    if (((DVCaptureGraph)vcg).VideoCompressor == null) { 
                        info += string.Format(CultureInfo.CurrentCulture, "\r\n" + Strings.CompressorDisabled);                        
                    }
                    else {
                        info += string.Format(CultureInfo.CurrentCulture, "\r\n" + Strings.CompressedBitRate,
                            ((DVCaptureGraph)vcg).VideoCompressor.QualityInfo.BitRate / 1000);
                    }
                }
            }

            lblVideoInfo.Text = info;
        }

        private void clbCameras_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //Show the status for the selected camera.
            try {
                ClearVideoBox();
                
                if (clbCameras.GetItemChecked(clbCameras.SelectedIndex)) {
                    FilterInfo fi = (FilterInfo)clbCameras.Items[clbCameras.SelectedIndex];
                    UpdateVideoBox(fi);
                }
            }
            catch (COMException ex) {
                Log(DShowError._AMGetErrorText(ex.ErrorCode));
                Log(ex.ToString());
            }
            catch (Exception ex) {
                Log(ex.ToString());
            }
        }

        private void clbCameras_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            try
            {
                FilterInfo fi = (FilterInfo)clbCameras.Items[e.Index];
                if(e.NewValue == CheckState.Checked)
                {
                    //We used to check to make sure we could activate the camera before
                    //saving.  Changed this to do the validation on video test or close
                    //in order to simplify logic and improve UI performance.
                    if (ckPlayVideo.Checked) {
                        TestVideo(fi, true);
                    }
                    AVReg.WriteValue(AVReg.SelectedCameras, fi.Moniker, fi.Name);
                }
                else if(e.NewValue == CheckState.Unchecked)
                {
                    if (ckPlayVideo.Checked) {
                        //Stop a video test underway
                        TestVideo(fi, false);
                        if (this.clbCameras.CheckedItems.Count == 1) {
                            //Last camera in the video test is about to uncheck: stop and disable the test.
                            ckPlayVideo.Checked = false;
                            ckPlayVideo.Enabled = false;
                        }
                    }
                    // Remove the camera from the registry
                    AVReg.DeleteValue(AVReg.SelectedCameras, fi.Moniker);
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

        private void btnAdvancedVideoSettings_Click(object sender, System.EventArgs e)
        {
            // Todo: Here is a set of circumstances where we don't support concurrent 
            // audio test with the advanced video form. This could be fixed.
            if ((this.ckPlayAudio.Checked) && 
                (!this.ckPlayVideo.Checked) && 
                (audioCapability != null) &&
                (audioCapability.CaptureGraph is DVCaptureGraph) &&
                (DVSource.IsDVSourceWithAudio((FilterInfo)clbCameras.Items[clbCameras.SelectedIndex])))
            {
                this.ckPlayAudio.Checked = false;
            }

            new frmVideoSettings((FilterInfo)clbCameras.Items[clbCameras.SelectedIndex], this).ShowDialog(this);
        }

        private void ckPlayVideo_CheckedChanged(object sender, System.EventArgs e)
        {
            ckPlayVideo.Text = ckPlayVideo.Checked ? Strings.StopVideo : Strings.TestVideo;
            for (int i = 0; i < clbCameras.CheckedItems.Count; i++) {
                FilterInfo fi = (FilterInfo)clbCameras.CheckedItems[i];
                try {
                    TestVideo(fi, ckPlayVideo.Checked);
                }
                catch (COMException ex) {
                    Log(DShowError._AMGetErrorText(ex.ErrorCode));
                    Log(ex.ToString());
                }
                catch (Exception ex) {
                    Log(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Turn the specified video test on or off
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="start"></param>
        private void TestVideo(FilterInfo fi, bool start) {
            if (start) {
                VideoCapability vc = new VideoCapability(fi);
                videoCapabilities.Add(fi, vc);
                vc.SetLogger(new AVLogger(Log));
                vc.ActivateCamera();
                RenderAndRunVideo(vc.CaptureGraph);
            }
            else {
                if (videoCapabilities.ContainsKey(fi)) {
                    VideoCapability vc = videoCapabilities[fi];
                    videoCapabilities.Remove(fi);
                    RenderAndRunVideo(vc.CaptureGraph, false);
                    vc.Dispose();
                }
            }
        }
        
        public void RenderAndRunVideo(CaptureGraph vcg)
        {
            RenderAndRunVideo(vcg, ckPlayVideo.Checked);
        }


        public void RenderAndRunVideo(CaptureGraph vcg, bool playIt)
        {
            if(playIt)
            {
                Log("Playing video (render and run graph) - " + vcg.Source.FriendlyName);

                vcg.RenderLocal();

                if (vcg is VideoCaptureGraph) {
                    // This is not working for DV graphs but doesn't seem critical in this context
                    VideoCapability.DisableDXVA(vcg.FilgraphManager);
                }

                // Set device name in the video window and turn off the system menu
                IVideoWindow iVW = (IVideoWindow)vcg.FilgraphManager;
                iVW.Caption = vcg.Source.FriendlyName;
                iVW.WindowStyle &= ~0x00080000; // WS_SYSMENU

                vcg.Run();
            }
            else
            {
                Log("Stop video (stop and unrender graph) - " + vcg.Source.FriendlyName);

                vcg.Stop();
                vcg.RemoveRenderer(MSR.LST.Net.Rtp.PayloadType.dynamicVideo);
                
                // I have no idea why the video window stays up but this fixes it
                GC.Collect();
            }
            
            Log(FilterGraph.Debug(vcg.IFilterGraph));
        }

        #endregion Video

        #region Audio

        private void cboSpeakers_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try {
                // Delete previous value
                string[] selectedSpeaker = AVReg.ValueNames(AVReg.SelectedSpeaker);
                if (selectedSpeaker != null) {
                    Debug.Assert(selectedSpeaker.Length == 1);
                    AVReg.DeleteValue(AVReg.SelectedSpeaker, selectedSpeaker[0]);
                }

                // Store current value
                FilterInfo fi = (FilterInfo)cboSpeakers.SelectedItem;
                AVReg.WriteValue(AVReg.SelectedSpeaker, fi.Moniker, fi.Name);

            }
            catch (COMException ex) {
                Log(DShowError._AMGetErrorText(ex.ErrorCode));
                Log(ex.ToString());
            }
            catch (Exception ex) {
                Log(ex.ToString());
            }

        }

        private void cboMicrophones_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try {
                string[] selectedMicrophone = AVReg.ValueNames(AVReg.SelectedMicrophone);
                if ((selectedMicrophone != null) && (selectedMicrophone.Length > 0)) {
                    Debug.Assert(selectedMicrophone.Length == 1);
                    AVReg.DeleteValue(AVReg.SelectedMicrophone, selectedMicrophone[0]);
                }

                //Remember that microphone index zero is <none>.
                if (cboMicrophones.SelectedIndex != 0) {  
                    FilterInfo fi = (FilterInfo)cboMicrophones.SelectedItem;
                    AVReg.WriteValue(AVReg.SelectedMicrophone, fi.Moniker, fi.Name);
                }

                btnAdvancedAudioSettings.Enabled = cboMicrophones.SelectedIndex > 0;
                ckPlayAudio.Enabled = cboMicrophones.SelectedIndex > 0;
                lblTestAudio.Enabled = cboMicrophones.SelectedIndex > 0;
            }
            catch (COMException ex) {
                Log(DShowError._AMGetErrorText(ex.ErrorCode));
                Log(ex.ToString());
            }
            catch (Exception ex) {
                Log(ex.ToString());
            }
        }


        private void cboACompressor_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                FilterInfo fi = (FilterInfo)cboACompressor.SelectedItem;
                AVReg.WriteValue(AVReg.SelectedDevices, AVReg.AudioCompressor, fi.Moniker);
                if (fi.Moniker == "Uncompressed") {

                }
                else {

                }
                    
            }
            catch (Exception ex) {
                Log(ex.ToString());
            }
        }

        private void btnAdvancedAudioSettings_Click(object sender, System.EventArgs e)
        {
            if (this.ckPlayAudio.Checked) {
                //If the test audio is running, stop it and dispose the audio capability.
                this.ckPlayAudio.Checked = false;
            }

            FilterInfo compressor = (FilterInfo)this.cboACompressor.SelectedItem;
            if (compressor.Name == "Uncompressed") {
                new frmAudioSettingsUnc((FilterInfo)this.cboMicrophones.SelectedItem, this).ShowDialog(this);
            }
            else if (compressor.Name == "Windows Media Audio V2") {
                new frmAudioSettingsWMA((FilterInfo)this.cboMicrophones.SelectedItem, this).ShowDialog(this);
            }
            else if (compressor.Name == "Opus Encoder") {
                new frmAudioSettingsOpus((FilterInfo)this.cboMicrophones.SelectedItem, this).ShowDialog(this);
            }
            else {
                throw new ApplicationException("Configuration form does not exist for selected compressor");
            }

        }

        private void ckPlayAudio_CheckedChanged(object sender, System.EventArgs e)
        {
            ckPlayAudio.Text = ckPlayAudio.Checked ? Strings.StopAudio : Strings.TestAudio;
            try {
                if (ckPlayAudio.Checked) {
                    FilterInfo fi = (FilterInfo)cboMicrophones.SelectedItem;
                    audioCapability = new AudioCapability(fi);
                    audioCapability.SetLogger(new AVLogger(Log));
                    audioCapability.ActivateMicrophone();
                    RenderAndRunAudio(audioCapability.CaptureGraph, true);
                }
                else { 
                    RenderAndRunAudio(audioCapability.CaptureGraph, false);
                    audioCapability.Dispose();
                    audioCapability = null;
                }
            }
            catch (COMException ex) {
                Log(DShowError._AMGetErrorText(ex.ErrorCode));
                Log(ex.ToString());
            }
            catch (Exception ex) {
                Log(ex.ToString());
            }
        }

        public void RenderAndRunAudio(CaptureGraph cg) {
            RenderAndRunAudio(cg, ckPlayAudio.Checked);
        }

        public void RenderAndRunAudio(CaptureGraph cg, bool playIt)
        {
            if(cg == null)
            {
                throw new ArgumentNullException(Strings.CantRenderAudioGraph);
            }

            if(playIt)
            {
                Log("Playing audio (render and run graph) - " + cg.Source.FriendlyName);

                // Re-add the renderer in case they changed it since the last 
                // time they played the audio
                cg.AddAudioRenderer((FilterInfo)cboSpeakers.SelectedItem);
                cg.Run();
            }
            else
            {
                Log("Stop audio (stop and unrender graph) - " + cg.Source.FriendlyName);

                cg.Stop();
                cg.RemoveRenderer(MSR.LST.Net.Rtp.PayloadType.dynamicAudio);
            }

            Log(FilterGraph.Debug(cg.IFilterGraph));
        }
        
        #endregion Audio

        #region Save

        public void SaveLinkedCamera(FilterInfo fi)
        {
            // Delete previous link value
            string[] linkedCamera = AVReg.ValueNames(AVReg.LinkedCamera);
            if(linkedCamera != null && linkedCamera.Length > 0)
            {
                Debug.Assert(linkedCamera.Length == 1);
                AVReg.DeleteValue(AVReg.LinkedCamera, linkedCamera[0]);
            }

            // Set the new value
            AVReg.WriteValue(AVReg.LinkedCamera, fi.Moniker, fi.Name);
        }

        private void btnTroubleshooting_Click(object sender, System.EventArgs e)
        {
             trouble.Show();
        }

        /// <summary>
        /// Save the auto-play and window tiling settings
        /// </summary>
        private void SaveSettings()
        {
            Conference.AutoPlayLocal = chkAutoPlayVideo.Checked;
            FMain.AutoPlayRemoteAudio = chkAutoPlayRemoteAudio.Checked;
            FMain.AutoPlayRemoteVideo = chkAutoPlayRemoteVideo.Checked;

            cxpclientRegKey.SetValue("AutoPlayLocal", Conference.AutoPlayLocal);
            cxpclientRegKey.SetValue("AutoPlayRemote", Conference.AutoPlayRemote);

            // Auto-position settings
            if (rdbtnWindowFourWay.Checked)
            {
                Conference.AutoPosition = Conference.AutoPositionMode.FourWay;
            }
            else if (rdbtnWindowTiled.Checked)
            {
                Conference.AutoPosition = Conference.AutoPositionMode.Tiled;
            }
            else if (rdbtnWindowFullScreen.Checked)
            {
                Conference.AutoPosition = Conference.AutoPositionMode.FullScreen;
            }
         
            cxpclientRegKey.SetValue("AutoPosition", Conference.AutoPosition);

            cxpclientRegKey.Flush();

        }

        #endregion Save

        #region Log

        public void Log(string msg)
        {
            if(trouble != null)
            {
                trouble.Log(msg);
            }

            Trace.WriteLine(msg);
        }

        #endregion Log
    }
}
