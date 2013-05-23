using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging; // To be able to dynamically change the color of the UI
using System.Globalization;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// Summary description for FAudioVideo.
    /// </summary>
    public class FAudioVideo : CapabilityForm
    {
        #region Windows Form Designer generated code
        
        private System.Windows.Forms.PictureBox pbVideo;
        private System.Windows.Forms.PictureBox pbMiddleFill;
        private System.ComponentModel.IContainer components;

        private System.Windows.Forms.PictureBox pbVideoButton;
        private System.Windows.Forms.PictureBox pbAudioButton;
        private System.Windows.Forms.Label lblInfo;
        private CustomTrackBar tbVolume;
        private System.Windows.Forms.Panel pnlVideo;
        private System.Windows.Forms.Panel pnlAudio;
        private System.Windows.Forms.ToolTip toolTipAudioVideo;
        private System.Windows.Forms.Panel pnlLocalVideo;
        private System.Windows.Forms.PictureBox pbLocalVideoButton;
        private System.Windows.Forms.PictureBox pbVideoSeparation;
        private System.Windows.Forms.Panel pnlLocalAudio;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pbLocalAudioButton;
        private System.Windows.Forms.ImageList imageListConfig;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel pnlLocalVideoConfig;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pbVideoTab;
        private System.Windows.Forms.Panel pnlSeparatorSender;
        private System.Windows.Forms.Panel pnlSeparatorPlayer;
        private System.Windows.Forms.PictureBox pictureBox4;        
        private System.Windows.Forms.PictureBox pbVideoButtonExt;
        private System.Windows.Forms.ImageList imageListSendButtons;
        private System.Windows.Forms.ImageList imageListPlayButtons;
        
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FAudioVideo));
            this.pbVideo = new System.Windows.Forms.PictureBox();
            this.pbMiddleFill = new System.Windows.Forms.PictureBox();
            this.pbVideoButton = new System.Windows.Forms.PictureBox();
            this.pbAudioButton = new System.Windows.Forms.PictureBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.imageListSendButtons = new System.Windows.Forms.ImageList(this.components);
            this.tbVolume = new MSR.LST.ConferenceXP.CustomTrackBar();
            this.pnlVideo = new System.Windows.Forms.Panel();
            this.pbVideoButtonExt = new System.Windows.Forms.PictureBox();
            this.pbVideoSeparation = new System.Windows.Forms.PictureBox();
            this.pnlAudio = new System.Windows.Forms.Panel();
            this.toolTipAudioVideo = new System.Windows.Forms.ToolTip(this.components);
            this.pbLocalVideoButton = new System.Windows.Forms.PictureBox();
            this.pbLocalAudioButton = new System.Windows.Forms.PictureBox();
            this.pbVideoTab = new System.Windows.Forms.PictureBox();
            this.pnlLocalVideo = new System.Windows.Forms.Panel();
            this.pnlLocalAudio = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.imageListConfig = new System.Windows.Forms.ImageList(this.components);
            this.pnlSeparatorSender = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pnlLocalVideoConfig = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pnlSeparatorPlayer = new System.Windows.Forms.Panel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.imageListPlayButtons = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbVideo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMiddleFill)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideoButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAudioButton)).BeginInit();
            this.pnlVideo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideoButtonExt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideoSeparation)).BeginInit();
            this.pnlAudio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLocalVideoButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLocalAudioButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideoTab)).BeginInit();
            this.pnlLocalVideo.SuspendLayout();
            this.pnlLocalAudio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlSeparatorSender.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.pnlLocalVideoConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.pnlSeparatorPlayer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // pbVideo
            // 
            this.pbVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbVideo.BackColor = System.Drawing.Color.Black;
            this.pbVideo.Location = new System.Drawing.Point(0, 0);
            this.pbVideo.Name = "pbVideo";
            this.pbVideo.Size = new System.Drawing.Size(380, 285);
            this.pbVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbVideo.TabIndex = 0;
            this.pbVideo.TabStop = false;
            this.pbVideo.Resize += new System.EventHandler(this.pbVideo_Resize);
            this.pbVideo.MouseDown += new MouseEventHandler(pbVideo_MouseDown);
            // 
            // pbMiddleFill
            // 
            this.pbMiddleFill.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbMiddleFill.Image = ((System.Drawing.Image)(resources.GetObject("pbMiddleFill.Image")));
            this.pbMiddleFill.Location = new System.Drawing.Point(0, 286);
            this.pbMiddleFill.Name = "pbMiddleFill";
            this.pbMiddleFill.Size = new System.Drawing.Size(400, 24);
            this.pbMiddleFill.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMiddleFill.TabIndex = 65;
            this.pbMiddleFill.TabStop = false;
            // 
            // pbVideoButton
            // 
            this.pbVideoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbVideoButton.Image = ((System.Drawing.Image)(resources.GetObject("pbVideoButton.Image")));
            this.pbVideoButton.Location = new System.Drawing.Point(0, 2);
            this.pbVideoButton.Name = "pbVideoButton";
            this.pbVideoButton.Size = new System.Drawing.Size(22, 23);
            this.pbVideoButton.TabIndex = 66;
            this.pbVideoButton.TabStop = false;
            this.toolTipAudioVideo.SetToolTip(this.pbVideoButton, "Play Video");
            this.pbVideoButton.MouseLeave += new System.EventHandler(this.pbVideoButton_MouseLeave);
            this.pbVideoButton.Click += new System.EventHandler(this.pbVideoButton_Click);
            this.pbVideoButton.MouseEnter += new System.EventHandler(this.pbVideoButton_MouseEnter);
            // 
            // pbAudioButton
            // 
            this.pbAudioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbAudioButton.Image = ((System.Drawing.Image)(resources.GetObject("pbAudioButton.Image")));
            this.pbAudioButton.Location = new System.Drawing.Point(0, 2);
            this.pbAudioButton.Name = "pbAudioButton";
            this.pbAudioButton.Size = new System.Drawing.Size(23, 23);
            this.pbAudioButton.TabIndex = 68;
            this.pbAudioButton.TabStop = false;
            this.toolTipAudioVideo.SetToolTip(this.pbAudioButton, "Mute Audio");
            this.pbAudioButton.MouseLeave += new System.EventHandler(this.pbAudioButton_MouseLeave);
            this.pbAudioButton.Click += new System.EventHandler(this.pbAudioButton_Click);
            this.pbAudioButton.MouseEnter += new System.EventHandler(this.pbAudioButton_MouseEnter);
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblInfo.Font = new System.Drawing.Font("Franklin Gothic Demi", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblInfo.Image = ((System.Drawing.Image)(resources.GetObject("lblInfo.Image")));
            this.lblInfo.Location = new System.Drawing.Point(360, 292);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(928, 18);
            this.lblInfo.TabIndex = 72;
            this.lblInfo.Text = "Status: Normal";
            this.toolTipAudioVideo.SetToolTip(this.lblInfo, "Status Information");
            // 
            // imageListSendButtons
            // 
            this.imageListSendButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSendButtons.ImageStream")));
            this.imageListSendButtons.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSendButtons.Images.SetKeyName(0, "");
            this.imageListSendButtons.Images.SetKeyName(1, "");
            this.imageListSendButtons.Images.SetKeyName(2, "");
            this.imageListSendButtons.Images.SetKeyName(3, "");
            this.imageListSendButtons.Images.SetKeyName(4, "");
            this.imageListSendButtons.Images.SetKeyName(5, "");
            this.imageListSendButtons.Images.SetKeyName(6, "");
            this.imageListSendButtons.Images.SetKeyName(7, "");
            // 
            // tbVolume
            // 
            this.tbVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbVolume.BackColor = System.Drawing.SystemColors.Control;
            this.tbVolume.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tbVolume.BackgroundImage")));
            this.tbVolume.CursorImage = ((System.Drawing.Image)(resources.GetObject("tbVolume.CursorImage")));
            this.tbVolume.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.tbVolume.Location = new System.Drawing.Point(32, 6);
            this.tbVolume.Name = "tbVolume";
            this.tbVolume.Size = new System.Drawing.Size(74, 14);
            this.tbVolume.TabIndex = 73;
            this.toolTipAudioVideo.SetToolTip(this.tbVolume, "Volume");
            this.tbVolume.Value = 0;
            this.tbVolume.Visible = false;
            this.tbVolume.Scroll += new MSR.LST.ConferenceXP.CustomTrackBar.ScrollHandler(this.tbVolume_Scroll);
            // 
            // pnlVideo
            // 
            this.pnlVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlVideo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlVideo.BackgroundImage")));
            this.pnlVideo.Controls.Add(this.pbVideoButtonExt);
            this.pnlVideo.Controls.Add(this.pbVideoSeparation);
            this.pnlVideo.Controls.Add(this.pbVideoButton);
            this.pnlVideo.Location = new System.Drawing.Point(168, 286);
            this.pnlVideo.Name = "pnlVideo";
            this.pnlVideo.Size = new System.Drawing.Size(56, 24);
            this.pnlVideo.TabIndex = 74;
            this.pnlVideo.Visible = false;
            // 
            // pbVideoButtonExt
            // 
            this.pbVideoButtonExt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbVideoButtonExt.Image = ((System.Drawing.Image)(resources.GetObject("pbVideoButtonExt.Image")));
            this.pbVideoButtonExt.Location = new System.Drawing.Point(24, 2);
            this.pbVideoButtonExt.Name = "pbVideoButtonExt";
            this.pbVideoButtonExt.Size = new System.Drawing.Size(22, 23);
            this.pbVideoButtonExt.TabIndex = 73;
            this.pbVideoButtonExt.TabStop = false;
            this.toolTipAudioVideo.SetToolTip(this.pbVideoButtonExt, "Pause Video");
            this.pbVideoButtonExt.MouseLeave += new System.EventHandler(this.pbVideoButtonExt_MouseLeave);
            this.pbVideoButtonExt.Click += new System.EventHandler(this.pbVideoButtonExt_Click);
            this.pbVideoButtonExt.MouseEnter += new System.EventHandler(this.pbVideoButtonExt_MouseEnter);
            // 
            // pbVideoSeparation
            // 
            this.pbVideoSeparation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbVideoSeparation.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbVideoSeparation.BackgroundImage")));
            this.pbVideoSeparation.Location = new System.Drawing.Point(56, 2);
            this.pbVideoSeparation.Name = "pbVideoSeparation";
            this.pbVideoSeparation.Size = new System.Drawing.Size(32, 24);
            this.pbVideoSeparation.TabIndex = 72;
            this.pbVideoSeparation.TabStop = false;
            // 
            // pnlAudio
            // 
            this.pnlAudio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlAudio.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlAudio.BackgroundImage")));
            this.pnlAudio.Controls.Add(this.pbAudioButton);
            this.pnlAudio.Controls.Add(this.tbVolume);
            this.pnlAudio.Location = new System.Drawing.Point(232, 286);
            this.pnlAudio.Name = "pnlAudio";
            this.pnlAudio.Size = new System.Drawing.Size(112, 24);
            this.pnlAudio.TabIndex = 75;
            this.pnlAudio.Visible = false;
            // 
            // pbLocalVideoButton
            // 
            this.pbLocalVideoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbLocalVideoButton.Image = ((System.Drawing.Image)(resources.GetObject("pbLocalVideoButton.Image")));
            this.pbLocalVideoButton.Location = new System.Drawing.Point(5, 2);
            this.pbLocalVideoButton.Name = "pbLocalVideoButton";
            this.pbLocalVideoButton.Size = new System.Drawing.Size(23, 21);
            this.pbLocalVideoButton.TabIndex = 73;
            this.pbLocalVideoButton.TabStop = false;
            this.toolTipAudioVideo.SetToolTip(this.pbLocalVideoButton, "Stop Sending My Video");
            this.pbLocalVideoButton.MouseLeave += new System.EventHandler(this.pbLocalVideoButton_MouseLeave);
            this.pbLocalVideoButton.Click += new System.EventHandler(this.pbLocalVideoButton_Click);
            this.pbLocalVideoButton.MouseEnter += new System.EventHandler(this.pbLocalVideoButton_MouseEnter);
            // 
            // pbLocalAudioButton
            // 
            this.pbLocalAudioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbLocalAudioButton.Image = ((System.Drawing.Image)(resources.GetObject("pbLocalAudioButton.Image")));
            this.pbLocalAudioButton.Location = new System.Drawing.Point(0, 0);
            this.pbLocalAudioButton.Name = "pbLocalAudioButton";
            this.pbLocalAudioButton.Size = new System.Drawing.Size(23, 21);
            this.pbLocalAudioButton.TabIndex = 68;
            this.pbLocalAudioButton.TabStop = false;
            this.toolTipAudioVideo.SetToolTip(this.pbLocalAudioButton, "Stop Sending My Audio");
            this.pbLocalAudioButton.MouseLeave += new System.EventHandler(this.pbLocalAudioButton_MouseLeave);
            this.pbLocalAudioButton.Click += new System.EventHandler(this.pbLocalAudioButton_Click);
            this.pbLocalAudioButton.MouseEnter += new System.EventHandler(this.pbLocalAudioButton_MouseEnter);
            // 
            // pbVideoTab
            // 
            this.pbVideoTab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbVideoTab.Image = ((System.Drawing.Image)(resources.GetObject("pbVideoTab.Image")));
            this.pbVideoTab.Location = new System.Drawing.Point(2, 2);
            this.pbVideoTab.Name = "pbVideoTab";
            this.pbVideoTab.Size = new System.Drawing.Size(74, 21);
            this.pbVideoTab.TabIndex = 71;
            this.pbVideoTab.TabStop = false;
            this.toolTipAudioVideo.SetToolTip(this.pbVideoTab, "Camera Settings");
            this.pbVideoTab.Visible = false;
            this.pbVideoTab.MouseLeave += new System.EventHandler(this.pbVideoTab_MouseLeave);
            this.pbVideoTab.Click += new System.EventHandler(this.pbVideoTab_Click);
            this.pbVideoTab.MouseEnter += new System.EventHandler(this.pbVideoTab_MouseEnter);
            // 
            // pnlLocalVideo
            // 
            this.pnlLocalVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlLocalVideo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlLocalVideo.BackgroundImage")));
            this.pnlLocalVideo.Controls.Add(this.pbLocalVideoButton);
            this.pnlLocalVideo.Location = new System.Drawing.Point(0, 286);
            this.pnlLocalVideo.Name = "pnlLocalVideo";
            this.pnlLocalVideo.Size = new System.Drawing.Size(32, 24);
            this.pnlLocalVideo.TabIndex = 76;
            this.pnlLocalVideo.Visible = false;
            // 
            // pnlLocalAudio
            // 
            this.pnlLocalAudio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlLocalAudio.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlLocalAudio.BackgroundImage")));
            this.pnlLocalAudio.Controls.Add(this.pictureBox1);
            this.pnlLocalAudio.Controls.Add(this.pbLocalAudioButton);
            this.pnlLocalAudio.Location = new System.Drawing.Point(32, 288);
            this.pnlLocalAudio.Name = "pnlLocalAudio";
            this.pnlLocalAudio.Size = new System.Drawing.Size(23, 24);
            this.pnlLocalAudio.TabIndex = 76;
            this.pnlLocalAudio.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.Location = new System.Drawing.Point(-57, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(25, 24);
            this.pictureBox1.TabIndex = 74;
            this.pictureBox1.TabStop = false;
            // 
            // imageListConfig
            // 
            this.imageListConfig.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListConfig.ImageStream")));
            this.imageListConfig.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListConfig.Images.SetKeyName(0, "");
            this.imageListConfig.Images.SetKeyName(1, "");
            // 
            // pnlSeparatorSender
            // 
            this.pnlSeparatorSender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlSeparatorSender.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlSeparatorSender.BackgroundImage")));
            this.pnlSeparatorSender.Controls.Add(this.pictureBox2);
            this.pnlSeparatorSender.Location = new System.Drawing.Point(152, 286);
            this.pnlSeparatorSender.Name = "pnlSeparatorSender";
            this.pnlSeparatorSender.Size = new System.Drawing.Size(13, 24);
            this.pnlSeparatorSender.TabIndex = 77;
            this.pnlSeparatorSender.Visible = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.BackgroundImage")));
            this.pictureBox2.Location = new System.Drawing.Point(13, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(32, 24);
            this.pictureBox2.TabIndex = 72;
            this.pictureBox2.TabStop = false;
            // 
            // pnlLocalVideoConfig
            // 
            this.pnlLocalVideoConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlLocalVideoConfig.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlLocalVideoConfig.BackgroundImage")));
            this.pnlLocalVideoConfig.Controls.Add(this.pictureBox3);
            this.pnlLocalVideoConfig.Controls.Add(this.pbVideoTab);
            this.pnlLocalVideoConfig.Location = new System.Drawing.Point(60, 286);
            this.pnlLocalVideoConfig.Name = "pnlLocalVideoConfig";
            this.pnlLocalVideoConfig.Size = new System.Drawing.Size(80, 24);
            this.pnlLocalVideoConfig.TabIndex = 78;
            this.pnlLocalVideoConfig.Visible = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox3.BackgroundImage")));
            this.pictureBox3.Location = new System.Drawing.Point(80, 2);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(32, 24);
            this.pictureBox3.TabIndex = 72;
            this.pictureBox3.TabStop = false;
            // 
            // pnlSeparatorPlayer
            // 
            this.pnlSeparatorPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlSeparatorPlayer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlSeparatorPlayer.BackgroundImage")));
            this.pnlSeparatorPlayer.Controls.Add(this.pictureBox4);
            this.pnlSeparatorPlayer.Location = new System.Drawing.Point(344, 286);
            this.pnlSeparatorPlayer.Name = "pnlSeparatorPlayer";
            this.pnlSeparatorPlayer.Size = new System.Drawing.Size(13, 24);
            this.pnlSeparatorPlayer.TabIndex = 79;
            this.pnlSeparatorPlayer.Visible = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox4.BackgroundImage")));
            this.pictureBox4.Location = new System.Drawing.Point(13, 2);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(32, 24);
            this.pictureBox4.TabIndex = 72;
            this.pictureBox4.TabStop = false;
            // 
            // imageListPlayButtons
            // 
            this.imageListPlayButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListPlayButtons.ImageStream")));
            this.imageListPlayButtons.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListPlayButtons.Images.SetKeyName(0, "");
            this.imageListPlayButtons.Images.SetKeyName(1, "");
            this.imageListPlayButtons.Images.SetKeyName(2, "");
            this.imageListPlayButtons.Images.SetKeyName(3, "");
            this.imageListPlayButtons.Images.SetKeyName(4, "");
            this.imageListPlayButtons.Images.SetKeyName(5, "");
            this.imageListPlayButtons.Images.SetKeyName(6, "");
            this.imageListPlayButtons.Images.SetKeyName(7, "");
            this.imageListPlayButtons.Images.SetKeyName(8, "");
            this.imageListPlayButtons.Images.SetKeyName(9, "");
            // 
            // FAudioVideo
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(380, 310);
            this.Controls.Add(this.pnlSeparatorPlayer);
            this.Controls.Add(this.pnlLocalVideoConfig);
            this.Controls.Add(this.pnlSeparatorSender);
            this.Controls.Add(this.pnlLocalAudio);
            this.Controls.Add(this.pnlLocalVideo);
            this.Controls.Add(this.pnlAudio);
            this.Controls.Add(this.pnlVideo);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.pbMiddleFill);
            this.Controls.Add(this.pbVideo);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FAudioVideo";
            this.Load += new System.EventHandler(this.FAudioVideo_Load);
            this.SizeChanged += new System.EventHandler(this.FAudioVideo_SizeChanged);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FAudioVideo_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FAudioVideo_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pbVideo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMiddleFill)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideoButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAudioButton)).EndInit();
            this.pnlVideo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbVideoButtonExt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideoSeparation)).EndInit();
            this.pnlAudio.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLocalVideoButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLocalAudioButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbVideoTab)).EndInit();
            this.pnlLocalVideo.ResumeLayout(false);
            this.pnlLocalAudio.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlSeparatorSender.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.pnlLocalVideoConfig.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.pnlSeparatorPlayer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);

        }
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (components != null) 
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }


        #endregion

        #region Statics

        internal enum UIState
        {
            LocalVideoSendStopped = 1, // Stop local sending
            LocalVideoPlayStopped = 2, // Stop local or remote playing

            LocalAudioSendStopped = 4, // Stop local sending
            LocalAudioPlayStopped = 8, // Stop local or remote playing

            RemoteVideoStopped = 16, // Remote sending stopped
            RemoteAudioStopped = 32  // Remote sending stopped
        }

        private readonly string Status = Strings.Status;
        private readonly string StatusNormal = Strings.StatusNormal;

        private readonly string LocalAudioSendStoppedMsg = Strings.AudioIsNotBeingSent;
        private readonly string LocalVideoSendStoppedMsg = Strings.VideoIsNotBeingSent;

        private readonly string LocalAudioPlayStoppedMsg = Strings.IncomingAudioIsMuted;
        private readonly string LocalVideoPlayStoppedMsg = Strings.IncomingVideoIsPaused;

        private readonly string RemoteVideoSendStoppedMsg = Strings.IncomingVideoIsNotBeingSent;
        private readonly string RemoteAudioSendStoppedMsg = Strings.IncomingAudioIsNotBeingSent;

        // images in the order they are in the imageListSendButtons of AV Buttons
        // Note: xxxOver means icon state when the mouse is over the button 
        public enum AVSendButtonState 
        {
            VideoSend, VideoSendOver, VideoStopSending, VideoStopSendingOver,
            AudioSend, AudioSendOver, AudioStopSending, AudioStopSendingOver
        };

        // images in the order they are in the imageListPlayButtons of AV Buttons
        // Note: xxxOver means icon state when the mouse is over the button
        //       VideoExtyyy means the second button of the video player (it has two buttons that toggle) 
        public enum AVPlayButtonState 
        {
            VideoPlay, VideoExtPlay, VideoPlayOver, VideoExtPlayOver, VideoStopPlaying, VideoExtStopPlaying,
            AudioPlay, AudioPlayOver, AudioStopPlaying, AudioStopPlayingOver
        };


        public enum ConfigButtonState {VideoConfig, VideoConfigOver};        

        #endregion Statics

        #region Members

        // This form is shared with video and audio capability
        private VideoCapability videoCapability = null;
        private AudioCapability audioCapability = null;

        // By default the video plays when the capability is lauched
        // Boolean that tells whether or not the button video playing is pushed
        private bool isVideoButtonPushed = true;
        private bool isLocalVideoButtonPushed = true;

        // By default the audio plays when the capability is lauched
        // Boolean that tells whether or not the button audio playing is pushed
        private bool isAudioButtonPushed = true;
        private bool isLocalAudioButtonPushed = true;

        private delegate void UpdateUIStateHandler(int uiState);

        private string videoInfo = null;
        private string audioInfo = null;

        #endregion Members

        #region Constructors

        /// <summary>
        /// Shared Form Constructor
        /// </summary>
        public FAudioVideo()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            MinSize();
        }

        #endregion Constructors

        #region Public

        /// <summary>
        /// Position the volume slider according to the current volume
        /// </summary>
        /// <remarks>
        /// - AudioCapability MethodInvoke this method in RtpStream_FirstFrameReceived.
        /// - There is no parameter. The volume parameter is actually taken from the
        /// volume accessor in AudioCapability.
        /// </remarks>
        public void PositionVolumeSlider()
        {
            if(InvokeRequired)
            {
                Invoke(new MethodInvoker(_PositionVolumeSlider));
            }
            else
            {
                _PositionVolumeSlider();
            }
        }

        #endregion Public

        #region Private

        /// <summary>
        /// Filter the input image with a green color
        /// </summary>
        /// <remarks>
        /// The color supported so far are green and red
        /// </remarks>
        /// <param name="imageIn">Original image</param>
        /// <returns>Recolored image</returns>
        private Image ColorChange(Image imageIn, Color color)
        {
            // TODO: I was planning to use the red color filter for paused AV button.
            //       I finally decided to use an image list with all the states because 
            //       it looks better.
            //       I left the red color in case we see an use later on, otherwise
            //       the red color can be removed to decrease the amount of code.

            if (imageIn != null)
            {
                Image imageOut = (Image) imageIn.Clone();
                Graphics g = Graphics.FromImage(imageOut);

                ColorMatrix colorMatrix = null;

                // TODO: Find a better way to set the color matrix
                if (color == Color.Green)
                {
                    // Create a matrix that will convert the image to a green color
                    float[][] matrix = {
                                           new float[] {0.9f, 0, 0, 0, 0},
                                           new float[] {0, 0.9f, 0, 0, 0},
                                           new float[] {0, 0, 0.9f, 0, 0},
                                           new float[] {0, 0, 0, 0.9f, 0},
                                           new float[] {0, 0.15f, 0, 0, 0.9f}};

                    colorMatrix = new ColorMatrix(matrix);
                } 
                else if (color == System.Drawing.Color.Red)
                {
                    // Create a matrix that will convert the image to a red color
                    float[][] matrix = {
                                           new float[] {0.9f, 0, 0, 0, 0},
                                           new float[] {0, 0.9f, 0, 0, 0},
                                           new float[] {0, 0, 0.9f, 0, 0},
                                           new float[] {0, 0, 0, 0.9f, 0},
                                           new float[] {0.9f, 0, 0, 0, 0.9f}};

                    colorMatrix = new ColorMatrix(matrix);
                } 
                else
                {
                    throw new Exception(Strings.ColorNotSupported);
                }

                ImageAttributes imgAtt = new ImageAttributes();
                imgAtt.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                TextureBrush tb = new TextureBrush(
                    imageOut,
                    new Rectangle(0, 0, imageOut.Width, imageOut.Height),
                    imgAtt);

                g.FillRectangle(tb, 0, 0, imageOut.Width, imageOut.Height);
                return imageOut;
            } 
            else
            {
                return null;
            }
        }

        private void InitVideoCapability(ICapability capability)
        {
            if (videoCapability == null)
            {
                videoCapability = (VideoCapability)capability;

                this.Text = videoCapability.Name;

                // Unintuitively causes us to look at the vidSrcRatio and use it if it's been set
                TrimBlackFromVideo(base.Size);

                // TODO: Add a condition to not call InitVideoUI() if isFormLoaded is false to prevent
                // the form to show to early
                InitVideoUI();

                videoCapability.VideoWindowHandle = pbVideo.Handle;
                videoCapability.VideoWindowMessageDrain = pbVideo.Handle;
                videoCapability.ResizeVideoStream(pbVideo.Height, pbVideo.Width);

                // Set the UI borders so the video capability can calculate the
                // AV form size given some constrains due to the video ratio
                videoCapability.UIBorderWidth = UIBorderWidth;
                videoCapability.UIBorderHeight = UIBorderHeight;
            }
        }

        private void InitAudioCapability(ICapability capability)
        {
            if (audioCapability == null)
            {
                audioCapability = (AudioCapability)capability;

                // We want always to have the name of the video capability
                // if the form is shared with a video capability
                // => If the audio capability come after the video capability,
                // we want to overwrite with the name of the video capability.
                if (videoCapability != null)
                {
                    this.Text = videoCapability.Name;
                }

                // TODO: Add a condition to not call InitVideoUI() if isFormLoaded is false to prevent
                // the form to show to early
                InitAudioUI();

                // Set the UI borders so the audio capability can calculate the
                // AV form size given some constrains due to the video ratio
                audioCapability.UIBorderWidth = UIBorderWidth;
                audioCapability.UIBorderHeight = UIBorderHeight;
            }
        }

        /// <summary>
        /// This method reposition the AV buttons given the visibility of the tabs
        /// </summary>
        /// <remarks>
        /// The method assumes the following order:
        /// pnlLocalVideo, pnlLocalAudio, pnlLocalVideoConfig, pnlSeparatorSender
        /// pnlVideo, pnlAudio, pnlSeparatorPlayer, lblInfo
        /// </remarks>
        private void RepositionAVButtons()
        {
            // Note: The code below is also positioning invisible panel which is fine (otherwise we need 
            //       to add if statements)

            // Check if we need a separator after the Send buttons
            // Note: The separator is invisible by default
            // TODO: We only check about visibility of buttons before the separation, but not after
            //       We should consider this case too
            pnlSeparatorSender.Visible = ((pnlLocalVideo.Visible) || (pnlLocalAudio.Visible) || (pnlLocalVideoConfig.Visible));

            // Check if we need a separator after the Play buttons
            // Note: The separator is invisible by default
            // TODO: We only check about visibility of buttons before the separation, but not after
            //       We should consider this case too
            pnlSeparatorPlayer.Visible = ((pnlVideo.Visible) || (pnlAudio.Visible));

            // No work needed for pnlLocalVideo: it is already placed correctly (1st one)

            // Sender's buttons
            pnlLocalAudio.Left = pnlLocalVideo.Left + (pnlLocalVideo.Visible ? pnlLocalVideo.Width : 0);
            pnlLocalVideoConfig.Left = pnlLocalAudio.Left + (pnlLocalAudio.Visible ? pnlLocalAudio.Width : 0);
            pnlSeparatorSender.Left = pnlLocalVideoConfig.Left + (pnlLocalVideoConfig.Visible ? pnlLocalVideoConfig.Width : 0);

            // Player's buttons
            pnlVideo.Left = pnlSeparatorSender.Left + (pnlSeparatorSender.Visible ? pnlLocalAudio.Width : 0);
            pnlAudio.Left = pnlVideo.Left + (pnlVideo.Visible ? pnlVideo.Width : 0);
            pnlSeparatorPlayer.Left = pnlAudio.Left + (pnlAudio.Visible ?  pnlAudio.Width : 0);

            // General Info label
            lblInfo.Left = pnlSeparatorPlayer.Left + (pnlSeparatorPlayer.Visible ?  pnlSeparatorPlayer.Width : 0);
        }

        /// <summary>
        /// Initialize the video specific part of the UI when the video capability is added
        /// </summary>
        private void InitVideoUI()
        {
            // Show the video play/stop button and the video tab if sender
            pnlVideo.Visible = true;

            if(videoCapability != null)
            {
                if (!videoCapability.IsPlaying)
                {
                    // Do not show controls for managing playing video if we're not playing
                    pnlVideo.Visible = false;
                }

                if(videoCapability.IsSender)
                {
                    pbVideoTab.Visible = true;     
                    pnlLocalVideo.Visible = true;
                    pnlLocalVideoConfig.Visible = true;
                } 
                else
                {
                    // Since there is no local video buttons, place the remote
                    // video control on the left
                    pnlVideo.Left = 0;
                }

                // Test if the video capability is sharing a form with a audio capability
                if (!videoCapability.UsesSharedForm)
                {
                    // Reposition the info label and display a message
                    pnlVideo.Left = 0;
                    lblInfo.Left = pnlVideo.Left + pnlVideo.Width;
                }

            }

            // Needs to be called after setting the visibility of the panels
            RepositionAVButtons();
        }

        /// <summary>
        /// Initialize the audio specific part of the UI when the audio capability is added
        /// </summary>
        private void InitAudioUI()
        {           
            // TODO: This code below could be removed or replaced by just showing the control disabled
            //       In fact, PositionVolumeSlider is already taking care of showing/enabling the trackbar control
            //       at the right time when you actually can control the volume... Enabling right here might be too early
            //       in some case. => Remove this code below and see if it still work correctly. 
            if (audioCapability.IsSender)
            {
                pnlLocalAudio.Visible = true;
            }

            if (audioCapability.IsPlaying)
            {
                tbVolume.Visible = true;
                // Show the speaker on/off button and volume slider
                pnlAudio.Visible = true; 
            }

            // Test if the audio capability is sharing a form with a video capability
            if (!audioCapability.UsesSharedForm)
            {
                // We have audio only so we can fix the form size to a small one
                this.MinimumSize = new Size(200, pbMiddleFill.Height);
                this.MaximumSize = new Size(SystemInformation.WorkingArea.Width, pbMiddleFill.Height + Height - ClientSize.Height);

                // Reposition the info label and display a message
                pnlAudio.Left = 0;

            }
            else // We are in sharing a form with video, so we need to leave room for the video
                // button that has to be located on the left
            {
                // TODO: This does not cover all the starting cases and needs to be cleaned-up
                pnlAudio.Left = pnlLocalVideo.Width;
                
                if ((videoCapability != null) && (!videoCapability.IsSender))
                {
                    pnlAudio.Left = pnlVideo.Width;
                } 
            }
            
            lblInfo.Left = pnlAudio.Left + pnlAudio.Width;

            // Needs to be called after setting the visibility of the panels
            RepositionAVButtons();
        }

        /// <summary>
        /// Uninitialize the video specific part of the UI when the video capability is removed
        /// </summary>
        private void UninitVideoUI()
        {
            // Hide the video play/stop button and the video tab if displayed
            pbVideoTab.Visible = false;
            pnlVideo.Visible = false;
            pnlLocalVideo.Visible = false;
            pnlLocalVideoConfig.Visible = false;

            // Needs to be called after setting the visibility of the panels
            RepositionAVButtons();
        }

        /// <summary>
        /// Uninitialize the audio specific part of the UI when the audio capability is removed
        /// </summary>
        private void UninitAudioUI()
        {
            // Hide the speaker on/off button and volume slider
            tbVolume.Visible = false;
            pnlAudio.Visible = false;
            pnlLocalAudio.Visible = false;

            // Needs to be called after setting the visibility of the panels
            RepositionAVButtons();
        }
        
        /// <summary>
        /// Form load event handler. Initialize the UI.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void FAudioVideo_Load(object sender, System.EventArgs e)
        {
            this.lblInfo.Text = Strings.StatusNormal;
            this.toolTipAudioVideo.SetToolTip(this.pbVideoButton, Strings.PlayVideo);
            this.toolTipAudioVideo.SetToolTip(this.pbAudioButton, Strings.MuteAudio);
            this.toolTipAudioVideo.SetToolTip(this.lblInfo, Strings.StatusInformation);
            this.toolTipAudioVideo.SetToolTip(this.tbVolume, Strings.Volume);
            this.toolTipAudioVideo.SetToolTip(this.pbVideoButtonExt, Strings.PauseVideo);
            this.toolTipAudioVideo.SetToolTip(this.pbLocalVideoButton, Strings.StopSendingMyVideo);
            this.toolTipAudioVideo.SetToolTip(this.pbLocalAudioButton, Strings.StopSendingMyAudio);
            this.toolTipAudioVideo.SetToolTip(this.pbVideoTab, Strings.CameraSettings);

            if ((videoCapability != null && videoCapability.IsSender) ||
                (audioCapability != null && audioCapability.IsSender))
                // Initiator UI
            {
                // Dynamically re-coloring the UI in green using GDI.NET
                pbMiddleFill.Image = ColorChange(pbMiddleFill.Image, Color.Green);

                // Video UI
                pnlVideo.BackgroundImage = ColorChange(pnlVideo.BackgroundImage, Color.Green);
                pbVideoButton.Image = ColorChange(pbVideoButton.Image, Color.Green);
                pbVideoButtonExt.Image = ColorChange(pbVideoButtonExt.Image, Color.Green);
                pbVideoTab.Image = ColorChange(pbVideoTab.Image, Color.Green);
                pnlLocalVideo.BackgroundImage = ColorChange(pnlLocalVideo.BackgroundImage, Color.Green);
                pnlLocalVideoConfig.BackgroundImage = ColorChange(pnlLocalVideoConfig.BackgroundImage, Color.Green);
                pbLocalVideoButton.Image = ColorChange(pbLocalVideoButton.Image, Color.Green);
                pbVideoSeparation.BackgroundImage = ColorChange(pbVideoSeparation.BackgroundImage, Color.Green);
                pnlSeparatorSender.BackgroundImage = ColorChange(pnlSeparatorSender.BackgroundImage, Color.Green);;

                // Audio UI
                pnlAudio.BackgroundImage = ColorChange(pnlAudio.BackgroundImage, Color.Green);
                pbAudioButton.Image = ColorChange(pbAudioButton.Image, Color.Green);
                tbVolume.BackgroundImage = ColorChange(tbVolume.BackgroundImage, Color.Green);
                tbVolume.CursorImage = ColorChange(tbVolume.CursorImage, Color.Green);
                pbLocalAudioButton.Image = ColorChange(pbLocalAudioButton.Image, Color.Green);
                pnlSeparatorPlayer.BackgroundImage = ColorChange(pnlSeparatorPlayer.BackgroundImage, Color.Green);

                // Info UI
                lblInfo.Image = ColorChange(lblInfo.Image, Color.Green);

                // Set tooltips that will change depending on state
                toolTipAudioVideo.SetToolTip(this.pbLocalVideoButton, Strings.StopSendingMyVideo);
                toolTipAudioVideo.SetToolTip(this.pbLocalAudioButton, Strings.StopSendingMyAudio);
                toolTipAudioVideo.SetToolTip(this.pbAudioButton, Strings.MuteAudio);

                // TODO: Don't forget to add code like the following later on when we add imageLists
            }
            else // Remote user UI
            {
                // TODO: Add code if needed to customize remote user UI (such as button invisible)
            }

            // TODO: Discuss form load with Jason during code review
            // TODO: So far InitVideoUI and InitAudioUI might be called twice if called from AddCapa... fix that
            if (videoCapability != null)
            {
                InitVideoUI();
            }
            if (audioCapability != null)
            {
                InitAudioUI();
            }

            // The launch process is the following: 
            //         - Create Form
            //         - Prepare the UI
            //         - Show Form
            // InitUI methods access the form which causes the form to show if not displayed
            // This would make the form appear too early before all the UI is ready (bad user experience)
            // To prevent that we need to have a flag that indicates that the form has been loaded
            // so a capabability added doesn't directly call the Init UI code if the form has not been
            // loaded
            // TODO: Code to add: isFormLoaded = true;

        }

        /// <summary>
        /// Assumes the provided size is the largest size the window can be, and then uses
        /// the aspect ratio of the video to trim "black space" from the edges of the video.
        /// </summary>
        /// <param name="intendedSize">The size we have to fit in.</param>
        internal void TrimBlackFromVideo(System.Drawing.Size intendedSize)
        {
            // If we don't yet know the aspect ratio of the video, just keep what the system
            //  gave us.  We'll come back and resize later when we know the video aspect ratio.
            if (videoCapability == null || videoCapability.vidSrcRatio <= 0)
            {
                if (base.Size != intendedSize)
                    base.Size = intendedSize;
                return;
            }

            // videoWidth/videoHight represents the width and height of the video inside the form
            //   which is constraint by the source ratio

            // formWidth/formHeight represents the width and height of the AV form that comtains
            //   the video and also additional UI element of size uIBorderWidth/uIBorderHeight

            // Let's first assume that if we keep the height allowed (value.Height) and that the 
            //   form width generated will be smaller than the width allowed (value.Width)
            int videoHeight = intendedSize.Height - videoCapability.uIBorderHeight;
            int videoWidth =  (int)(videoHeight / videoCapability.vidSrcRatio);
            int formWidth = videoWidth + videoCapability.uIBorderWidth;
            int formHeight = intendedSize.Height;

            // Check if the form width generated is greater than the width allowed (value.Width)
            if (formWidth > intendedSize.Width)
            {
                // If so, keep the width allowed (value.Width) and resize the height
                videoWidth = intendedSize.Width - videoCapability.uIBorderWidth;
                videoHeight = (int)(videoWidth * videoCapability.vidSrcRatio);
                formHeight = videoHeight + videoCapability.uIBorderHeight;
                formWidth = intendedSize.Width;
            }

            // Set the new size that takes in account the video ratio constraints
            base.Size = new System.Drawing.Size(formWidth, formHeight); 
        }

        /// <summary>
        /// Any key event escapes out of maximized WindowState.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FAudioVideo_KeyDown(object sender, KeyEventArgs e) {
            if (this.WindowState == FormWindowState.Maximized) {
                this.WindowState = FormWindowState.Normal;
            }
        }

        /// <summary>
        /// Mouse down event on the video window escapes out of maximized window state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pbVideo_MouseDown(object sender, MouseEventArgs e) {
            if (this.WindowState == FormWindowState.Maximized) {
                this.WindowState = FormWindowState.Normal;
            }
        }

        /// <summary>
        /// Form closing event handler.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void FAudioVideo_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // TODO: 
            // If the form is Closed with X (the sender is the form)
            //   - Stop Playing the capability
            //   - form.Close
            // Else
            //   - continue to close the form

            // Call StopPlaying of all the capability objects not referring this shared form
            // anymore
            if (videoCapability != null)
            {
                if (videoCapability.IsSender)
                {
                    videoCapability.StopSending();
                }
                else
                {
                    videoCapability.StopPlaying();
                }
            }
            if (audioCapability != null)
            {
                if (audioCapability.IsSender)
                {
                    audioCapability.StopSending();
                }
                else
                {
                    audioCapability.StopPlaying();
                }
            }
        }

        /// <summary>
        /// VideoButton click event handler (Playing side)
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void pbVideoButton_Click(object sender, System.EventArgs e)
        {
            // Notes: 
            //  - UI updates are handled from the capability in Stop/Resume
            //  - The video player has 2 buttons that toggle: pbVideoButton and pbVideoButtonExt

            if (!isVideoButtonPushed)
            {
                // update the state of the button
                isVideoButtonPushed = !isVideoButtonPushed;

                // Restart Playing
                if (videoCapability.IsSender)
                {
                    pbVideoButton.Image = ColorChange(imageListPlayButtons.Images[(int)AVPlayButtonState.VideoPlay], Color.Green);
                    pbVideoButtonExt.Image = ColorChange(imageListPlayButtons.Images[(int)AVPlayButtonState.VideoExtPlay], Color.Green);
                }
                else
                {
                    pbVideoButton.Image = imageListPlayButtons.Images[(int)AVPlayButtonState.VideoPlay];
                    pbVideoButtonExt.Image = imageListPlayButtons.Images[(int)AVPlayButtonState.VideoExtPlay];
                }
                
                videoCapability.ResumePlayingVideo();
            }
        }

        private void pbVideoButtonExt_Click(object sender, System.EventArgs e)
        {
            // Notes: 
            //  - UI updates are handled from the capability in Stop/Resume
            //  - The video player has 2 buttons that toggle: pbVideoButton and pbVideoButtonExt

            if (isVideoButtonPushed)
            {
                // update the state of the button
                isVideoButtonPushed = !isVideoButtonPushed;

                // Display the play button since we are in pause mode
                if (videoCapability.IsSender)
                {
                    pbVideoButton.Image = ColorChange(imageListPlayButtons.Images[(int)AVPlayButtonState.VideoStopPlaying], Color.Green); 
                    pbVideoButtonExt.Image = ColorChange(imageListPlayButtons.Images[(int)AVPlayButtonState.VideoExtStopPlaying], Color.Green);     
                }
                else
                {
                    pbVideoButton.Image = imageListPlayButtons.Images[(int)AVPlayButtonState.VideoStopPlaying];
                    pbVideoButtonExt.Image = imageListPlayButtons.Images[(int)AVPlayButtonState.VideoExtStopPlaying];
                }

                videoCapability.StopPlayingVideo();
            }
        }

        /// <summary>
        /// LocalVideoButton click event handler (Sending side)
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void pbLocalVideoButton_Click(object sender, System.EventArgs e)
        {
            // Thus button will appear only on the local participant AV form
            if (!isLocalVideoButtonPushed)
            {
                // Restart Sending / Playing
                pbLocalVideoButton.Image = ColorChange(
                    imageListSendButtons.Images[(int)AVSendButtonState.VideoSendOver], Color.Green);
                videoCapability.ResumeSendingVideo(); 
                toolTipAudioVideo.SetToolTip(this.pbLocalVideoButton,Strings.StopSendingMyVideo);
            }
            else
            {
                // Display the play button since we are in pause mode

                // Display the appropriate pause message
                pbLocalVideoButton.Image = ColorChange(
                    imageListSendButtons.Images[(int)AVSendButtonState.VideoStopSendingOver], Color.Green);
                videoCapability.StopSendingVideo();
                toolTipAudioVideo.SetToolTip(this.pbLocalVideoButton, Strings.SendMyVideo);
            }

            // update the state of the button
            isLocalVideoButtonPushed = !isLocalVideoButtonPushed;
        }

        /// <summary>
        /// AudioButton click event handler (Playing side)
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void pbAudioButton_Click(object sender, System.EventArgs e)
        {
            //
            //Note: UI updates are handled from the capability in Stop/Resume
            //

            if (!isAudioButtonPushed)
            {
                // Restart Playing
                if(sender == pbAudioButton)
                {
                    audioCapability.ResumePlayingAudio();
                }

                if (audioCapability.IsSender)
                {
                    pbAudioButton.Image = ColorChange(
                        imageListPlayButtons.Images[(int)AVPlayButtonState.AudioPlay], Color.Green);
                } 
                else
                {
                    pbAudioButton.Image = imageListPlayButtons.Images[(int)AVPlayButtonState.AudioPlay];
                }

                toolTipAudioVideo.SetToolTip(this.pbAudioButton,Strings.MuteAudio);

                // update the state of the button
                isAudioButtonPushed = true;
            }
            else
            {
                // Stop playing
                if(sender == pbAudioButton)
                {
                    audioCapability.StopPlayingAudio();
                }

                if (audioCapability.IsSender)
                {
                    pbAudioButton.Image = ColorChange(
                        imageListPlayButtons.Images[(int)AVPlayButtonState.AudioStopPlaying], Color.Green);
                } 
                else
                {
                    pbAudioButton.Image = imageListPlayButtons.Images[(int)AVPlayButtonState.AudioStopPlaying];
                }

                toolTipAudioVideo.SetToolTip(this.pbAudioButton, Strings.PlayAudio);

                // update the state of the button
                isAudioButtonPushed = false;
            }
        }

        /// <summary>
        /// LocalAudioButton click event handler (Sending side)
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void pbLocalAudioButton_Click(object sender, System.EventArgs e)
        {
            // Thus button will appear only on the local participant AV form
            if (!isLocalAudioButtonPushed)
            {
                // Restart Sending
                pbLocalAudioButton.Image = ColorChange(
                    imageListSendButtons.Images[(int)AVSendButtonState.AudioSendOver], Color.Green);
                audioCapability.ResumeSendingAudio();
                toolTipAudioVideo.SetToolTip(this.pbLocalAudioButton,Strings.StopSendingMyAudio);
            }
            else
            {
                // Display the play button since we are in pause mode

                // Display the appropriate pause message
                pbLocalAudioButton.Image = ColorChange(
                    imageListSendButtons.Images[(int)AVSendButtonState.AudioStopSendingOver], Color.Green);
                audioCapability.StopSendingAudio();
                toolTipAudioVideo.SetToolTip(this.pbLocalAudioButton, Strings.SendMyAudio);
            }

            // update the state of the button
            isLocalAudioButtonPushed = !isLocalAudioButtonPushed;
        }


        #region Mouse Enter/Leave events handler

        // TODO: We could create custums control and embed this behavior inside
        //       the control instead of inside the form (reusability, shorter code in the form, etc.)

        // TODO: We should change the way we manage the two color UI
        //       For instance have a method that check if sender or not to change
        //       the color appropriately instead of have the if statement and double code 
        //       all over the place ;-)

        private void pbLocalVideoButton_MouseEnter(object sender, System.EventArgs e)
        {
            if (!isLocalVideoButtonPushed)
            {
                pbLocalVideoButton.Image = ColorChange(
                    imageListSendButtons.Images[(int)AVSendButtonState.VideoStopSendingOver], Color.Green);
            }
            else
            {
                pbLocalVideoButton.Image = ColorChange(
                    imageListSendButtons.Images[(int)AVSendButtonState.VideoSendOver], Color.Green);
            }
        }

        private void pbLocalVideoButton_MouseLeave(object sender, System.EventArgs e)
        {
            if (!isLocalVideoButtonPushed)
            {
                pbLocalVideoButton.Image = ColorChange(
                    imageListSendButtons.Images[(int)AVSendButtonState.VideoStopSending], Color.Green);
            }
            else
            {
                pbLocalVideoButton.Image = ColorChange(
                    imageListSendButtons.Images[(int)AVSendButtonState.VideoSend], Color.Green);
            }
        }

        private void pbLocalAudioButton_MouseEnter(object sender, System.EventArgs e)
        {
            if (!isLocalAudioButtonPushed)
            {
                pbLocalAudioButton.Image = ColorChange(
                    imageListSendButtons.Images[(int)AVSendButtonState.AudioStopSendingOver], Color.Green);
            }
            else
            {
                pbLocalAudioButton.Image = ColorChange(
                    imageListSendButtons.Images[(int)AVSendButtonState.AudioSendOver], Color.Green);
            }
        }

        private void pbLocalAudioButton_MouseLeave(object sender, System.EventArgs e)
        {
            if (!isLocalAudioButtonPushed)
            {
                pbLocalAudioButton.Image = ColorChange(
                    imageListSendButtons.Images[(int)AVSendButtonState.AudioStopSending], Color.Green);
            }
            else
            {
                pbLocalAudioButton.Image = ColorChange(
                    imageListSendButtons.Images[(int)AVSendButtonState.AudioSend], Color.Green);
            }
        }

        private void pbVideoTab_MouseEnter(object sender, System.EventArgs e)
        {
            pbVideoTab.Image = ColorChange(
                    imageListConfig.Images[(int)ConfigButtonState.VideoConfigOver], Color.Green);
        }

        private void pbVideoTab_MouseLeave(object sender, System.EventArgs e)
        {
            pbVideoTab.Image = ColorChange(
                imageListConfig.Images[(int)ConfigButtonState.VideoConfig], Color.Green);
        }

        private void pbVideoButton_MouseEnter(object sender, System.EventArgs e)
        {
            if ((videoCapability != null) && (!isVideoButtonPushed))
            {
                if (videoCapability.IsSender)
                {
                    pbVideoButton.Image = ColorChange(imageListPlayButtons.Images[(int)AVPlayButtonState.VideoPlayOver], Color.Green);
                }
                else
                {
                    pbVideoButton.Image = imageListPlayButtons.Images[(int)AVPlayButtonState.VideoPlayOver];
                }
            }
        }

        private void pbVideoButton_MouseLeave(object sender, System.EventArgs e)
        {
            if ((videoCapability != null) && (!isVideoButtonPushed))
            {
                if (videoCapability.IsSender)
                {
                    pbVideoButton.Image = ColorChange(imageListPlayButtons.Images[(int)AVPlayButtonState.VideoStopPlaying], Color.Green);
                }
                else
                {
                    pbVideoButton.Image = imageListPlayButtons.Images[(int)AVPlayButtonState.VideoStopPlaying];
                }
            }
        }

        private void pbVideoButtonExt_MouseEnter(object sender, System.EventArgs e)
        {
            if ((videoCapability != null) && (isVideoButtonPushed))
            {
                if (videoCapability.IsSender)
                {
                    pbVideoButtonExt.Image = ColorChange(imageListPlayButtons.Images[(int)AVPlayButtonState.VideoExtPlayOver], Color.Green);
                }
                else
                {
                    pbVideoButtonExt.Image = imageListPlayButtons.Images[(int)AVPlayButtonState.VideoExtPlayOver];
                }
            }
        }

        private void pbVideoButtonExt_MouseLeave(object sender, System.EventArgs e)
        {
            if ((videoCapability != null) && (isVideoButtonPushed))
            {
                if (videoCapability.IsSender)
                {
                    pbVideoButtonExt.Image = ColorChange(imageListPlayButtons.Images[(int)AVPlayButtonState.VideoExtPlay], Color.Green);
                }
                else
                {
                    pbVideoButtonExt.Image = imageListPlayButtons.Images[(int)AVPlayButtonState.VideoExtPlay];
                }
            }
        }

        private void pbAudioButton_MouseEnter(object sender, System.EventArgs e)
        {
            if (audioCapability != null)
            {
                if (isAudioButtonPushed)
                {
                    if (audioCapability.IsSender)
                    {
                        pbAudioButton.Image = ColorChange(
                            imageListPlayButtons.Images[(int)AVPlayButtonState.AudioPlayOver], Color.Green);
                    } 
                    else
                    {
                        pbAudioButton.Image = imageListPlayButtons.Images[(int)AVPlayButtonState.AudioPlayOver];
                    }
                }
                else
                {
                    if (audioCapability.IsSender)
                    {
                        pbAudioButton.Image = ColorChange(
                            imageListPlayButtons.Images[(int)AVPlayButtonState.AudioStopPlayingOver], Color.Green);
                    } 
                    else
                    {
                        pbAudioButton.Image = imageListPlayButtons.Images[(int)AVPlayButtonState.AudioStopPlayingOver];
                    }
                }
            }
        }

        private void pbAudioButton_MouseLeave(object sender, System.EventArgs e)
        {
            if (audioCapability != null)
            {
                if (isAudioButtonPushed)
                {
                    if (audioCapability.IsSender)
                    {
                        pbAudioButton.Image = ColorChange(
                            imageListPlayButtons.Images[(int)AVPlayButtonState.AudioPlay], Color.Green);
                    } 
                    else
                    {
                        pbAudioButton.Image = imageListPlayButtons.Images[(int)AVPlayButtonState.AudioPlay];
                    }
                }
                else
                {
                    if (audioCapability.IsSender)
                    {
                        pbAudioButton.Image = ColorChange(
                            imageListPlayButtons.Images[(int)AVPlayButtonState.AudioStopPlaying], Color.Green);
                    } 
                    else
                    {
                        pbAudioButton.Image = imageListPlayButtons.Images[(int)AVPlayButtonState.AudioStopPlaying];
                    }
                }
            }
        }

        #endregion Mouse Enter/Leave events handler

        /// <summary>
        /// Video picture box resize event.
        /// </summary>
        /// <remarks>
        /// This picture box is anchored (top, bottom, left, and right) to the AV form,
        /// so it get resized when the AV form is resized
        /// </remarks>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void pbVideo_Resize(object sender, System.EventArgs e)
        {
            if(videoCapability != null)
            {
                videoCapability.ResizeVideoStream(pbVideo.Height, pbVideo.Width);
            }
        }

        private void FAudioVideo_SizeChanged(object sender, EventArgs e) {
            if (videoCapability != null) {

                //In Maximized mode, remove the window frame and the controls
                if ((this.WindowState == FormWindowState.Maximized) &&
                    (this.FormBorderStyle != FormBorderStyle.None)) {
                    
                    this.SizeChanged -= new System.EventHandler(this.FAudioVideo_SizeChanged);
                    
                    //Hide controls by making pbVideo big enough to cover them
                    this.pbVideo.BringToFront();
                    this.pbVideo.Height = this.Size.Height;
                    this.FormBorderStyle = FormBorderStyle.None;
                    //Trace.WriteLine("Form Size changed: Set maximized");
                    
                    this.SizeChanged += new System.EventHandler(this.FAudioVideo_SizeChanged);
                }

                //Coming out of maximized mode, restore the frame and controls
                if ((this.WindowState != FormWindowState.Maximized) &&
                    (this.FormBorderStyle == FormBorderStyle.None)) {

                    this.SizeChanged -= new System.EventHandler(this.FAudioVideo_SizeChanged);
                    
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    //A tricky thing here: putting the border back doesn't set the client size back to the original.  Do it manually.
                    this.Size = this.ClientSize;
                    //Unhide controls by reducing the size of pbVideo.
                    this.pbVideo.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - this.pbMiddleFill.Height);
                    //Trace.WriteLine("Form Size Changed: unset maximized: " + this.Size.ToString());
                    
                    this.SizeChanged += new System.EventHandler(this.FAudioVideo_SizeChanged);
                }
            }
        }

       
        /// <summary>
        /// Video tab button event handler
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        /// <remarks></remarks>
        private void pbVideoTab_Click(object sender, System.EventArgs e)
        {
            // TODO: For now this button is visible only on the sender AV forms
            // because the configuration is related to sender only. This might be
            // changed in the future if we need to also control some part of
            // the playing video
            
            // Open the video configuration form
            videoCapability.ShowCameraConfiguration();
        }

        /// <summary>
        /// Handle the trackbar changed event to set the new volume level 
        /// </summary>
        /// <param name="value">Volume value [0, 100]</param>
        private void tbVolume_Scroll(int value)
        {
            audioCapability.SetVolume(value);
        }

        private int UIBorderWidth
        {
            get{return Width - ClientSize.Width;}
        }

        private int UIBorderHeight
        {
            get{return pbMiddleFill.Height + Height - ClientSize.Height;}
        }

        private void MinSize()
        {
            this.MinimumSize = new Size(80 + UIBorderWidth, 60 + UIBorderHeight);
        }

        
        private void _PositionVolumeSlider()
        {
            // Note: It gets the value from the audio capability
            // TODO: See if there is a better way to do that (i.e. Method Invoking with param)
            tbVolume.Value = audioCapability.Volume;
        }


        
        public void UpdateVideoUI(int uiState)
        {
            if(InvokeRequired)
            {
                Invoke(new UpdateUIStateHandler(_UpdateVideoUI), new object[]{uiState});
            }
            else
            {
                _UpdateVideoUI(uiState);
            }
        }
        private void _UpdateVideoUI(int uiState)
        {
            // Default state if audio is playing
            videoInfo = null;

            // Remote video stopped
            if((uiState & (int)UIState.RemoteVideoStopped) == (int)UIState.RemoteVideoStopped)
            {
                videoInfo = RemoteVideoSendStoppedMsg;
            }

            // Because local video comes last, it will win

            // Video play stopped
            if((uiState & (int)UIState.LocalVideoPlayStopped) == (int)UIState.LocalVideoPlayStopped)
            {
                videoInfo = LocalVideoPlayStoppedMsg;

                pbVideoButtonExt_Click(this, null);
            }
            else
            {
                pbVideoButton_Click(this, null);
            }

            // Enable / Disable the Stop and Play buttons by capability.IsPlaying
            // (You can't Stop or Play video in the UI if the capability isn't Playing it)
            Debug.Assert(videoCapability != null);

            pnlVideo.Visible = videoCapability.IsPlaying;

            // Local video send stopped
            if((uiState & (int)UIState.LocalVideoSendStopped) == (int)UIState.LocalVideoSendStopped)
            {
                Debug.Assert(videoCapability.IsSender);
                
                videoInfo = LocalVideoSendStoppedMsg;
            }

            RepositionAVButtons();

            UpdateInfo();

            TrimBlackFromVideo(base.Size);
        }

        
        public void UpdateAudioUI(int uiState)
        {
            if(InvokeRequired)
            {
                Invoke(new UpdateUIStateHandler(_UpdateAudioUI), new object[]{uiState});
            }
            else
            {
                _UpdateAudioUI(uiState);
            }
        }

        private void _UpdateAudioUI(int uiState)
        {
            // Default state if audio is playing
            audioInfo = null;

            // Remote audio
            if((uiState & (int)UIState.RemoteAudioStopped) == (int)UIState.RemoteAudioStopped)
            {
                audioInfo = RemoteAudioSendStoppedMsg;
            }

            // Because local audio comes last, it will win

            // Local audio play stopped
            if((uiState & (int)UIState.LocalAudioPlayStopped) == (int)UIState.LocalAudioPlayStopped)
            {
                // Don't inform the Sender they stopped playing their own audio
                if(!audioCapability.IsSender)
                {
                    audioInfo = LocalAudioPlayStoppedMsg;
                }
            }

            // Enable / Disable the Stop and Play buttons by capability.IsPlaying
            // (You can't Stop or Play video in the UI if the capability isn't Playing it)
            Debug.Assert(audioCapability != null);

            pnlAudio.Visible = audioCapability.IsPlaying;
            tbVolume.Visible = audioCapability.IsPlaying;

            if (tbVolume.Visible)
            {
                PositionVolumeSlider();
            }

            isAudioButtonPushed = !audioCapability.IsPlaying;
            pbAudioButton_Click(this, null);

            // Local audio send stopped
            if((uiState & (int)UIState.LocalAudioSendStopped) == (int)UIState.LocalAudioSendStopped)
            {
                Debug.Assert(audioCapability.IsSender);

                audioInfo = LocalAudioSendStoppedMsg;
            }

            RepositionAVButtons();

            UpdateInfo();
        }
        

        private void UpdateInfo()
        {
            string info = Status;

            if(videoInfo == null && audioInfo == null)
            {
                info = StatusNormal;
            }
            else
            {
                if(videoInfo != null)
                {
                    info += videoInfo;
                }

                if(audioInfo != null)
                {
                    // Separator between messages
                    if(videoInfo != null)
                    {
                        info += " -- ";
                    }

                    info += audioInfo;
                }
            }

            lblInfo.Text = info;
        }

        #endregion Private

        #region ICapabilityForm

        /// <summary>
        /// Add a capability object to the list of capability objects referring to this shared form
        /// </summary>
        /// <param name="capability">The capability object to add</param>
        public override void AddCapability(ICapability capability)
        {
            base.AddCapability(capability);

            if (capability is VideoCapability)
            {
                InitVideoCapability(capability);
            }
            else if (capability is AudioCapability)
            {
                InitAudioCapability(capability);
            }
        }

        /// <summary>
        /// Remove a capability object to the list of capability objects referring to this shared form
        /// </summary>
        /// <param name="capability">The capability object to remove</param>
        /// <returns>true if there is no more capability of this type, false otherwise</returns>
        public override bool RemoveCapability(ICapability capability)
        {
            bool lastCapability = base.RemoveCapability(capability);

            if(lastCapability)
            {

                // TODO - we need to keep track of how many instances of a capability are in use
                // so that we shut down when the last instance goes away, not the first
                if (capability is VideoCapability)
                {
                    UninitVideoUI();
                    videoCapability = null;
                }
                else if (capability is AudioCapability)
                {
                    UninitAudioUI();
                    audioCapability = null;
                }
            }

            return lastCapability;
        }


        #endregion ICapabilityForm


    }
}
