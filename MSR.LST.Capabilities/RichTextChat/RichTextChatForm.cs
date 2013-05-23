using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP
{
    public class RichTextChatForm : CapabilityForm
    {
        #region WinForm Generated Stuff

        private System.Windows.Forms.Button btnFont;
        private System.Windows.Forms.FontDialog fontDlg;
        private System.Windows.Forms.Label lblFont;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox txtReceive;
        private System.Windows.Forms.RichTextBox txtSend;

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
            this.btnSend = new System.Windows.Forms.Button();
            this.txtReceive = new System.Windows.Forms.RichTextBox();
            this.txtSend = new System.Windows.Forms.RichTextBox();
            this.fontDlg = new System.Windows.Forms.FontDialog();
            this.btnFont = new System.Windows.Forms.Button();
            this.lblFont = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Enabled = false;
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSend.Location = new System.Drawing.Point(229, 274);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(56, 36);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "&Send";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtReceive
            // 
            this.txtReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReceive.Location = new System.Drawing.Point(0, 0);
            this.txtReceive.Name = "txtReceive";
            this.txtReceive.ReadOnly = true;
            this.txtReceive.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.txtReceive.Size = new System.Drawing.Size(288, 232);
            this.txtReceive.TabIndex = 2;
            this.txtReceive.Text = "";
            // 
            // txtSend
            // 
            this.txtSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSend.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSend.Location = new System.Drawing.Point(0, 274);
            this.txtSend.Name = "txtSend";
            this.txtSend.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.txtSend.Size = new System.Drawing.Size(224, 36);
            this.txtSend.TabIndex = 0;
            this.txtSend.Text = "";
            this.txtSend.SelectionChanged += new System.EventHandler(this.txtSend_SelectionChanged);
            this.txtSend.TextChanged += new System.EventHandler(this.txtSend_TextChanged);
            // 
            // fontDlg
            // 
            this.fontDlg.ShowColor = true;
            // 
            // btnFont
            // 
            this.btnFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFont.Location = new System.Drawing.Point(0, 240);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(80, 24);
            this.btnFont.TabIndex = 1;
            this.btnFont.Text = "Change Font";
            this.btnFont.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // lblFont
            // 
            this.lblFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFont.Location = new System.Drawing.Point(88, 240);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new System.Drawing.Size(192, 32);
            this.lblFont.TabIndex = 4;
            // 
            // RichTextChatForm
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(288, 312);
            this.Controls.Add(this.lblFont);
            this.Controls.Add(this.btnFont);
            this.Controls.Add(this.txtSend);
            this.Controls.Add(this.txtReceive);
            this.Controls.Add(this.btnSend);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Name = "RichTextChatForm";
            this.Text = "Rich Text Chat";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.RichTextChat_Closing);
            this.Load += new System.EventHandler(this.RichTextChat_Load);
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

        private RichTextChatCapability rtChat = null;

        private Font lastFont;
        private Color lastColor;

        #endregion Members

        #region Constructor

        public RichTextChatForm()
        {
            InitializeComponent();
        }


        #endregion Constructor
        
        #region ICapabilityForm

        public override void AddCapability(ICapability capability)
        {
            base.AddCapability (capability);

            if(rtChat == null)
            {
                rtChat = (RichTextChatCapability)capability;

                // Hook the ObjectReceived event so we can receive incoming data
                rtChat.ObjectReceived += new CapabilityObjectReceivedEventHandler(OnObjectReceived);
            }
        }

        public override bool RemoveCapability(ICapability capability)
        {
            bool ret = base.RemoveCapability (capability);

            if(ret)
            {
                // Remove the ObjectReceived event handler.
                // This form is going away, but the Capability may be replayed in which case we'd receive this event into a disposed form!
                rtChat.ObjectReceived -= new CapabilityObjectReceivedEventHandler(OnObjectReceived);
                rtChat = null;
            }

            return ret;
        }


        #endregion ICapabilityForm

        #region Private

        private void RichTextChat_Load(object sender, System.EventArgs e)
        {
            this.btnSend.Text = Strings.SendHotkey;
            this.btnFont.Text = Strings.ChangeFont;
            this.Text = Strings.RichTextChat;

            UpdateFont();
        }

        private void RichTextChat_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(rtChat != null)
            {
                if (rtChat.IsSending)
                {
                    rtChat.StopSending();
                }
            }
        }

        
        private void txtSend_TextChanged(object sender, System.EventArgs e)
        {
            // In case they activate the button by typing text, and then delete it
            btnSend.Enabled = !(txtSend.Text.Length == 0);
        }

        private void txtSend_SelectionChanged(object sender, System.EventArgs e)
        {
            UpdateFont();
        }

        private void btnSend_Click(object sender, System.EventArgs e)
        {
            string msg = txtSend.Rtf;
            txtSend.Clear();

            rtChat.SendObject(msg);
        }

        private void btnFont_Click(object sender, System.EventArgs e)
        {
            if(fontDlg.ShowDialog(this) == DialogResult.OK)
            {
                // Only change selected text or future text, if we have already started typing
                if(txtSend.SelectedText.Length > 0 || txtSend.SelectionStart > 0)
                {
                    txtSend.SelectionFont = fontDlg.Font;
                    txtSend.SelectionColor = fontDlg.Color;
                }
                else // Otherwise change the whole text box
                {
                    txtSend.Font = fontDlg.Font;
                    txtSend.ForeColor = fontDlg.Color;
                }

                UpdateFont();
            }

            txtSend.Focus();
        }


        private void OnObjectReceived(object o, ObjectReceivedEventArgs orea)
        {
            if(!(orea.Data is String))
            {
                string msg = string.Format(CultureInfo.CurrentCulture, 
                    "Unexpected data type for - orea.Data.  Expected type 'string' received type '{0}'", 
                    orea.Data.GetType().ToString());

                Debug.Assert(false, msg);
                throw new ArgumentException(msg);
            }

            ViewMessage(orea.Participant.Name, (string)orea.Data);
        }

        private void ViewMessage(string participant, string msg)
        {
            // Set the selection point at the end of the text box
            // just in case user was moving it around inside receive text box
            txtReceive.SelectionStart = txtReceive.TextLength;

            // Add the user's name in a default font
            txtReceive.SelectionFont = Font;
            txtReceive.AppendText(participant + ": ");

            // Add the message
            txtReceive.SelectedRtf = msg;

            // Move the cursor to the end of the text box, so the most recent text is visible
            txtReceive.SelectionStart = txtReceive.TextLength;
            txtReceive.ScrollToCaret();
        }

        private void UpdateFont()
        {
            if( lastFont == null || !EqualFont(lastFont, txtSend.SelectionFont) ||
                lastColor != txtSend.SelectionColor)
            {
                lastFont = txtSend.SelectionFont;
                lastColor = txtSend.SelectionColor;

                lblFont.Text = string.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}, {3}", 
                    lastFont.Name, lastFont.Size, lastFont.Style, lastColor);
            }
        }

        /// <summary>
        /// Font.Equal is kind of unpredictable, so do our own comparison
        /// </summary>
        private bool EqualFont(Font f1, Font f2)
        {
            return f1.Name == f2.Name &&
                   f1.Size == f2.Size &&
                   f1.Style == f2.Style;
        }
        
        
        #endregion Private
    }
}