using System;
using System.Configuration;
using System.Globalization;      

// IPEndPoint
using System.Net;
using System.Windows.Forms;

// Add a reference to NetworkingBasics.dll: classes used - BufferChunk
// Add a reference to LSTCommon.dll: classes used -  UnhandledExceptionHandler
using MSR.LST;              

// Add a reference to MSR.LST.Net.Rtp.dll
// Classes used - RtpSession, RtpSender, RtpParticipant, RtpStream
using MSR.LST.Net.Rtp;

// Code Flow (CF)
// 1. Hook Rtp events:
//   a.   RtpParticipant Added / Removed
//   b.   RtpStream Added / Removed
//   c.   Hook / Unhook FrameReceived event for that stream
// 2. Join RtpSession by providing an RtpParticipant and Multicast EndPoint
// 3. Retrieve RtpSender
// 4. Send data over network
// 5. Receive data from network
// 6. Unhook events, dispose RtpSession


namespace RtpChat
{
    public class frmChat : System.Windows.Forms.Form
    {
        #region Windows Form Designer generated code

        // Form variables
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtReceive;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblReceive;
        private System.Windows.Forms.Label lblSend;
        private System.Windows.Forms.Button btnJoinLeave;
        private System.Windows.Forms.Button btnSend;

        // Required designer variable
        private System.ComponentModel.Container components = null;

        // Constructor
        public frmChat()
        {
            // Required for Windows Form Designer support
            InitializeComponent();
        }


        // Required method for Designer support - do not modify
        // the contents of this method with the code editor.
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.txtReceive = new System.Windows.Forms.TextBox();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lblReceive = new System.Windows.Forms.Label();
            this.lblSend = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnJoinLeave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtReceive
            // 
            this.txtReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReceive.CausesValidation = false;
            this.txtReceive.Location = new System.Drawing.Point(16, 64);
            this.txtReceive.Multiline = true;
            this.txtReceive.Name = "txtReceive";
            this.txtReceive.ReadOnly = true;
            this.txtReceive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReceive.Size = new System.Drawing.Size(320, 152);
            this.txtReceive.TabIndex = 3;
            this.txtReceive.Text = "";
            // 
            // txtSend
            // 
            this.txtSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSend.Enabled = false;
            this.txtSend.Location = new System.Drawing.Point(16, 240);
            this.txtSend.Multiline = true;
            this.txtSend.Name = "txtSend";
            this.txtSend.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSend.Size = new System.Drawing.Size(320, 64);
            this.txtSend.TabIndex = 2;
            this.txtSend.Text = "";
            // 
            // btnSend
            // 
            this.btnSend.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSend.CausesValidation = false;
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(16, 40);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(0, 5);
            this.btnSend.TabIndex = 2;
            this.btnSend.TabStop = false;
            this.btnSend.Text = "&Send";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblReceive
            // 
            this.lblReceive.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.lblReceive.Location = new System.Drawing.Point(16, 48);
            this.lblReceive.Name = "lblReceive";
            this.lblReceive.Size = new System.Drawing.Size(72, 16);
            this.lblReceive.TabIndex = 3;
            this.lblReceive.Text = "Receive:";
            // 
            // lblSend
            // 
            this.lblSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSend.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.lblSend.Location = new System.Drawing.Point(16, 224);
            this.lblSend.Name = "lblSend";
            this.lblSend.Size = new System.Drawing.Size(48, 16);
            this.lblSend.TabIndex = 4;
            this.lblSend.Text = "Send:";
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.lblName.Location = new System.Drawing.Point(16, 8);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(56, 16);
            this.lblName.TabIndex = 5;
            this.lblName.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.txtName.Location = new System.Drawing.Point(64, 8);
            this.txtName.MaxLength = 32;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(192, 26);
            this.txtName.TabIndex = 0;
            this.txtName.Text = "";
            // 
            // btnJoinLeave
            // 
            this.btnJoinLeave.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.btnJoinLeave.Location = new System.Drawing.Point(264, 8);
            this.btnJoinLeave.Name = "btnJoinLeave";
            this.btnJoinLeave.TabIndex = 1;
            this.btnJoinLeave.Text = "Join";
            this.btnJoinLeave.Click += new System.EventHandler(this.btnJoinLeave_Click);
            // 
            // frmChat
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(352, 318);
            this.Controls.Add(this.btnJoinLeave);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtSend);
            this.Controls.Add(this.txtReceive);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblSend);
            this.Controls.Add(this.lblReceive);
            this.Controls.Add(this.btnSend);
            this.MaximizeBox = false;
            this.Name = "frmChat";
            this.Text = "RtpChat";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmChat_Closing);
            this.Load += new System.EventHandler(this.frmChat_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

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

        #region Statics / App.Config overrides

        private static IPEndPoint ep = RtpSession.DefaultEndPoint;

        static frmChat()
        {
            string setting;

            // See if there was a multicast IP address set in the app.config
            if((setting = ConfigurationManager.AppSettings["EndPoint"]) != null)
            {
                string[] args = setting.Split(new char[]{':'}, 2);
                ep = new IPEndPoint(IPAddress.Parse(args[0]), int.Parse(args[1], CultureInfo.InvariantCulture));
            }
        }


        [STAThread]
        static void Main() 
        {
            // Make sure no exceptions escape unnoticed
            UnhandledExceptionHandler.Register();
            Application.Run(new frmChat());
        }


        #endregion Statics / App.Config overrides

        #region Members

        /// <summary>
        /// Manages the connection to a multicast address and all the objects related to Rtp
        /// </summary>
        private RtpSession rtpSession;

        /// <summary>
        /// Sends the data across the network
        /// </summary>
        private RtpSender rtpSender;

        #endregion Members

        #region Private

        private void btnJoinLeave_Click(object sender, System.EventArgs e)
        {
            if(btnJoinLeave.Text == Strings.Join)
            {
                // Need a valid name
                if(txtName.Text.Length > 0)
                {
                    HookRtpEvents(); // 1
                    JoinRtpSession(txtName.Text); // 2

                    // Change the UI
                    btnJoinLeave.Text = Strings.Leave;
                    txtName.ReadOnly = true;
                    txtSend.Enabled = true;
                    btnSend.Enabled = true;
                    txtSend.Focus();
                }
                else
                {
                    txtName.Focus();
                }
            }
            else
            {
                Cleanup(); // 6

                // Change the UI
                btnJoinLeave.Text = Strings.Join;
                txtName.ReadOnly = false;
                txtSend.Enabled = false;
                btnSend.Enabled = false;
                txtReceive.Clear();
            }
        }

        
        // CF1 Hook Rtp events
        private void HookRtpEvents()
        {
            RtpEvents.RtpParticipantAdded += new RtpEvents.RtpParticipantAddedEventHandler(RtpParticipantAdded);
            RtpEvents.RtpParticipantRemoved += new RtpEvents.RtpParticipantRemovedEventHandler(RtpParticipantRemoved);
            RtpEvents.RtpStreamAdded += new RtpEvents.RtpStreamAddedEventHandler(RtpStreamAdded);
            RtpEvents.RtpStreamRemoved += new RtpEvents.RtpStreamRemovedEventHandler(RtpStreamRemoved);
        }

        
        // CF2 Create participant, join session
        // CF3 Retrieve RtpSender
        private void JoinRtpSession(string name)
        {
            rtpSession = new RtpSession(ep, new RtpParticipant(name, name), true, true);
            rtpSender = rtpSession.CreateRtpSenderFec(name, PayloadType.Chat, null, 0, 200);
        }

        
        // CF4 Send the data  
        private void btnSend_Click(object sender, System.EventArgs e)
        {
            if(txtSend.Text.Length > 0)
            {
                // BufferChunk does an automatic conversion on text.
                rtpSender.Send((BufferChunk)txtSend.Text);
                txtSend.Clear();
            }
        }

        
        // CF5 Receive data from network
        private void RtpParticipantAdded(object sender, RtpEvents.RtpParticipantEventArgs ea)
        {
            ShowMessage(string.Format(CultureInfo.CurrentCulture, Strings.HasJoinedTheChatSession, 
                ea.RtpParticipant.Name));
        }

        private void RtpParticipantRemoved(object sender, RtpEvents.RtpParticipantEventArgs ea)
        {
            ShowMessage(string.Format(CultureInfo.CurrentCulture, Strings.HasLeftTheChatSession, 
                ea.RtpParticipant.Name));
        }

        private void RtpStreamAdded(object sender, RtpEvents.RtpStreamEventArgs ea)
        {
            ea.RtpStream.FrameReceived += new RtpStream.FrameReceivedEventHandler(FrameReceived);
        }

        private void RtpStreamRemoved(object sender, RtpEvents.RtpStreamEventArgs ea)
        {
            ea.RtpStream.FrameReceived -= new RtpStream.FrameReceivedEventHandler(FrameReceived);
        }

        private void FrameReceived(object sender, RtpStream.FrameReceivedEventArgs ea)
        {
            ShowMessage(string.Format(CultureInfo.CurrentCulture, "{0}: {1}", ea.RtpStream.Properties.Name, 
                (string)ea.Frame));
        }


        // CF6 Unhook events, dispose RtpSession
        private void Cleanup()
        {
            UnhookRtpEvents();
            LeaveRtpSession();
        }

        private void UnhookRtpEvents()
        {
            RtpEvents.RtpParticipantAdded -= new RtpEvents.RtpParticipantAddedEventHandler(RtpParticipantAdded);
            RtpEvents.RtpParticipantRemoved -= new RtpEvents.RtpParticipantRemovedEventHandler(RtpParticipantRemoved);
            RtpEvents.RtpStreamAdded -= new RtpEvents.RtpStreamAddedEventHandler(RtpStreamAdded);
            RtpEvents.RtpStreamRemoved -= new RtpEvents.RtpStreamRemovedEventHandler(RtpStreamRemoved);
        }

        private void LeaveRtpSession()
        {
            if(rtpSession != null)
            {
                // Clean up all outstanding objects owned by the RtpSession
                rtpSession.Dispose();
                rtpSession = null;
                rtpSender = null;
            }
        }

        private void frmChat_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Cleanup();
        }


        
        private void ShowMessage(string msg)
        {
            // Update the text
            txtReceive.Text += (msg + Environment.NewLine);

            // Move the cursor to the end of the text box, so the most recent text is visible
            txtReceive.SelectionStart = txtReceive.MaxLength;
            txtReceive.ScrollToCaret();
        }

        
        #endregion Private

        private void frmChat_Load(object sender, EventArgs e)
        {
            this.btnSend.Text = Strings.SendHotkey;
            this.lblReceive.Text = Strings.Receive;
            this.lblSend.Text = Strings.Send;
            this.lblName.Text = Strings.Name;
            this.btnJoinLeave.Text = Strings.Join;
        }
    }
}
