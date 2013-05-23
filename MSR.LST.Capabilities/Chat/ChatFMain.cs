using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP
{
    public class ChatFMain : CapabilityForm
    {
        #region WinForm Generated Stuff

        private System.Windows.Forms.TextBox textSend;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.TextBox textReceive;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.textSend = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.textReceive = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textSend
            // 
            this.textSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textSend.Location = new System.Drawing.Point(0, 245);
            this.textSend.Multiline = true;
            this.textSend.Name = "textSend";
            this.textSend.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textSend.Size = new System.Drawing.Size(221, 35);
            this.textSend.TabIndex = 1;
            this.textSend.TextChanged += new System.EventHandler(this.textSend_TextChanged);
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.Enabled = false;
            this.buttonSend.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonSend.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonSend.Location = new System.Drawing.Point(229, 245);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(66, 35);
            this.buttonSend.TabIndex = 2;
            this.buttonSend.Text = "&Send";
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // textReceive
            // 
            this.textReceive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textReceive.ForeColor = System.Drawing.SystemColors.Window;
            this.textReceive.Location = new System.Drawing.Point(0, 0);
            this.textReceive.Multiline = true;
            this.textReceive.Name = "textReceive";
            this.textReceive.ReadOnly = true;
            this.textReceive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textReceive.Size = new System.Drawing.Size(298, 243);
            this.textReceive.TabIndex = 3;
            // 
            // ChatFMain
            // 
            this.AcceptButton = this.buttonSend;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(298, 281);
            this.Controls.Add(this.textReceive);
            this.Controls.Add(this.textSend);
            this.Controls.Add(this.buttonSend);
            this.Font = UIFont.FormFont;
            this.Name = "ChatFMain";
            this.Text = "Chat";
            this.Resize += new System.EventHandler(this.FMain_Resize);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FMain_Closing);
            this.Load += new System.EventHandler(this.ChatFMain_Load);
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

        #region Members

        private ChatCapability chatCapability = null;

        #endregion Members

        #region Constructor

        public ChatFMain()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }


        #endregion Constructor
        
        #region ICapabilityForm

        public override void AddCapability(ICapability capability)
        {
            base.AddCapability (capability);

            if(chatCapability == null)
            {
                chatCapability = (ChatCapability)capability;

                // Hook the ObjectReceived event so we can receive incoming data
                chatCapability.ObjectReceived += new CapabilityObjectReceivedEventHandler(objectReceived);
            }
        }

        public override bool RemoveCapability(ICapability capability)
        {
            bool ret = base.RemoveCapability (capability);

            if(ret)
            {
                // Remove the ObjectReceived event handler.
                // This form is going away, but the Capability may be replayed in which case we'd receive this event into a disposed form!
                chatCapability.ObjectReceived -= new CapabilityObjectReceivedEventHandler(objectReceived);
                chatCapability = null;
            }

            return ret;
        }


        #endregion ICapabilityForm

        #region Private

        private void textSend_TextChanged(object sender, System.EventArgs e)
        {
            // In case they activate the button by typing text, and then delete it
            if (textSend.Text.Length == 0)
            {
                buttonSend.Enabled= false;
            }
            else
            {
                buttonSend.Enabled= true;
            }
        }

        private void buttonSend_Click(object sender, System.EventArgs e)
        {
            // Add a new line to the end of text
            string message = textSend.Text + Environment.NewLine;
            textSend.Clear();
            SendMessage(message);
        }

        private void ViewMessage(string message)
        {
            textReceive.Text += message;

            // Move the cursor to the end of the text box, so the most recent text is visible
            textReceive.SelectionStart = textReceive.MaxLength;
            textReceive.ScrollToCaret();
        }

        private void FMain_Resize(object sender, System.EventArgs e)
        {
            textReceive.Height = this.Height - textSend.Height - System.Windows.Forms.SystemInformation.CaptionHeight - 18;
        }

        private void SendMessage(string message)
        {
            chatCapability.SendObject(message);
        }

        // Hook the objectReceived event and process it
        private void objectReceived(object o, ObjectReceivedEventArgs orea)
        {
            if(orea.Data is String)
            {
                ViewMessage(orea.Participant.Name + ": " + orea.Data.ToString());
            }
        }

        private void FMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(chatCapability != null)
            {
                if (chatCapability.IsSending)
                {
                    chatCapability.StopSending();
                }
            }

            // We have to check a second time since it may have gone null in between
            if(chatCapability != null)
            {
                if (chatCapability.IsPlaying)
                {
                    chatCapability.StopPlaying();
                }
            }
        }

        #endregion Private

        private void ChatFMain_Load(object sender, EventArgs e)
        {
            this.textSend.Font = UIFont.StringFont;
            this.buttonSend.Font = new System.Drawing.Font(UIFont.Name, UIFont.Size * 1.2F, System.Drawing.FontStyle.Bold);
            this.textReceive.Font = UIFont.StringFont;

            this.buttonSend.Text = Strings.SendHotkey;
            this.Text = Strings.Chat;
            this.FMain_Resize(this, null);
        }
    }
}
