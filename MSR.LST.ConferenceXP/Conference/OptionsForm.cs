using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Web.Services.Protocols;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// Summary description for OptionsForm.
    /// </summary>
    internal class OptionsForm : System.Windows.Forms.Form
    {
        public enum FormState
        {
            clean,
            create,
            edit
        }

        public FormState State = FormState.clean;
        private bool initializing = true;
        
        public OptionsForm(FormState state)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            State = state;
            ShowParticipant();
            
            if (State == FormState.create)
            {
                EnableOK(this, null);
            }


            if (Conference.VenueServiceWrapper.VenueService.PrivacyPolicyUrl() != null)
            {
                linkPrivacyPolicy.Visible = true;                    
                linkPrivacyPolicy.Links.Add(0, 14, Conference.VenueServiceWrapper.VenueService.PrivacyPolicyUrl());
            }
        }

        #region Windows Form Designer generated code
        
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ImageList SmallIcons;
        private System.Windows.Forms.OpenFileDialog OpenImageFileDialog;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox EmailTextBox;
        private System.Windows.Forms.Label EmailLabel;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button SelectImageButton;
        private System.Windows.Forms.PictureBox ParticipantImage;
        private System.Windows.Forms.Button DeleteImageButton;
        private System.Windows.Forms.LinkLabel linkPrivacyPolicy;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label IDLabel;
        private System.Windows.Forms.Label label1;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.SmallIcons = new System.Windows.Forms.ImageList(this.components);
            this.OpenImageFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.linkPrivacyPolicy = new System.Windows.Forms.LinkLabel();
            this.DeleteImageButton = new System.Windows.Forms.Button();
            this.EmailTextBox = new System.Windows.Forms.TextBox();
            this.EmailLabel = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.IDLabel = new System.Windows.Forms.Label();
            this.SelectImageButton = new System.Windows.Forms.Button();
            this.ParticipantImage = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.ParticipantImage)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // SmallIcons
            // 
            this.SmallIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("SmallIcons.ImageStream")));
            this.SmallIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.SmallIcons.Images.SetKeyName(0, "");
            this.SmallIcons.Images.SetKeyName(1, "");
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(244, 240);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(244, 208);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(96, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // linkPrivacyPolicy
            // 
            this.linkPrivacyPolicy.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.linkPrivacyPolicy.Location = new System.Drawing.Point(244, 176);
            this.linkPrivacyPolicy.Name = "linkPrivacyPolicy";
            this.linkPrivacyPolicy.Size = new System.Drawing.Size(96, 20);
            this.linkPrivacyPolicy.TabIndex = 146;
            this.linkPrivacyPolicy.TabStop = true;
            this.linkPrivacyPolicy.Text = "Privacy Policy";
            this.linkPrivacyPolicy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkPrivacyPolicy.Visible = false;
            this.linkPrivacyPolicy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkPrivacyPolicy_LinkClicked);
            // 
            // DeleteImageButton
            // 
            this.DeleteImageButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteImageButton.Image")));
            this.DeleteImageButton.Location = new System.Drawing.Point(120, 64);
            this.DeleteImageButton.Name = "DeleteImageButton";
            this.DeleteImageButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.DeleteImageButton.Size = new System.Drawing.Size(32, 32);
            this.DeleteImageButton.TabIndex = 7;
            this.DeleteImageButton.Click += new System.EventHandler(this.DeleteImageButton_Click);
            // 
            // EmailTextBox
            // 
            this.EmailTextBox.Location = new System.Drawing.Point(84, 72);
            this.EmailTextBox.Name = "EmailTextBox";
            this.EmailTextBox.Size = new System.Drawing.Size(232, 20);
            this.EmailTextBox.TabIndex = 4;
            this.EmailTextBox.TextChanged += new System.EventHandler(this.EnableOK);
            // 
            // EmailLabel
            // 
            this.EmailLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.EmailLabel.Location = new System.Drawing.Point(16, 74);
            this.EmailLabel.Name = "EmailLabel";
            this.EmailLabel.Size = new System.Drawing.Size(60, 18);
            this.EmailLabel.TabIndex = 135;
            this.EmailLabel.Text = "E-mail:";
            this.EmailLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(84, 48);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(232, 20);
            this.NameTextBox.TabIndex = 1;
            this.NameTextBox.TextChanged += new System.EventHandler(this.EnableOK);
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label4.Location = new System.Drawing.Point(16, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 18);
            this.label4.TabIndex = 132;
            this.label4.Text = "Name:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // IDLabel
            // 
            this.IDLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.IDLabel.Location = new System.Drawing.Point(84, 24);
            this.IDLabel.Name = "IDLabel";
            this.IDLabel.Size = new System.Drawing.Size(232, 23);
            this.IDLabel.TabIndex = 2;
            // 
            // SelectImageButton
            // 
            this.SelectImageButton.Image = ((System.Drawing.Image)(resources.GetObject("SelectImageButton.Image")));
            this.SelectImageButton.Location = new System.Drawing.Point(120, 24);
            this.SelectImageButton.Name = "SelectImageButton";
            this.SelectImageButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SelectImageButton.Size = new System.Drawing.Size(32, 32);
            this.SelectImageButton.TabIndex = 6;
            this.SelectImageButton.Click += new System.EventHandler(this.SelectImageButton_Click);
            // 
            // ParticipantImage
            // 
            this.ParticipantImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ParticipantImage.Location = new System.Drawing.Point(8, 24);
            this.ParticipantImage.Name = "ParticipantImage";
            this.ParticipantImage.Size = new System.Drawing.Size(102, 102);
            this.ParticipantImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ParticipantImage.TabIndex = 127;
            this.ParticipantImage.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.NameTextBox);
            this.groupBox1.Controls.Add(this.IDLabel);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.EmailLabel);
            this.groupBox1.Controls.Add(this.EmailTextBox);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(16, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 104);
            this.groupBox1.TabIndex = 147;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "My information";
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 18);
            this.label1.TabIndex = 136;
            this.label1.Text = "Identity:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ParticipantImage);
            this.groupBox2.Controls.Add(this.SelectImageButton);
            this.groupBox2.Controls.Add(this.DeleteImageButton);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(16, 128);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(176, 136);
            this.groupBox2.TabIndex = 148;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "My icon";
            // 
            // OptionsForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(358, 280);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.linkPrivacyPolicy);
            this.Font = UIFont.FormFont;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "My Profile";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ParticipantImage)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
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

        private void ShowParticipant()
        {
            Participant p = Conference.LocalParticipant;

            if (p != null)
            {
                IDLabel.Text = p.Identifier;
                NameTextBox.Text = p.Name;
                EmailTextBox.Text = p.Email;
                if (p.Icon != null)
                {
                    ParticipantImage.Image = p.Icon;
                }
            }
            else
            {
                IDLabel.Text = Identity.Identifier;
            }

            initializing = false;
        }

        private void SelectImageButton_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "Image Files|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG;*.EMF;*.EXIF;*.ICO;*.TIF;*.WMF";

            if (d.ShowDialog() == DialogResult.OK && d.FileName != null)
            {
                try
                {
                    ParticipantImage.Image = Image.FromFile(d.FileName);
                    EnableOK(this, null);
                }
                catch
                {
                RtlAwareMessageBox.Show(this, Strings.FileTypeCannotBeRead, Strings.ErroneousImageFile, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                }
            }
        }

        private void DeleteImageButton_Click(object sender, System.EventArgs e)
        {
            ParticipantImage.Image = null;
            EnableOK(this, null);
        }

        private void linkPrivacyPolicy_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            Bitmap bm = null;
            if (ParticipantImage.Image != null)
            {
                MemoryStream ms = new MemoryStream();
                ParticipantImage.Image.Save(ms, ImageFormat.Png);
                bm = new Bitmap(ms);
            }

            try
            {
                if (State == FormState.create)
                {
                    Conference.AddProfileOnServer(NameTextBox.Text, null, EmailTextBox.Text, bm, false);
                }
                else
                {
                    Conference.UpdateProfileOnServer(NameTextBox.Text, null, EmailTextBox.Text, bm, false);
                }
            }
            catch (System.Net.WebException)
            {
                RtlAwareMessageBox.Show(this, Strings.ProfileCouldNotBeUpdated, 
                    Strings.ErrorConnectingToVenueServer, MessageBoxButtons.OK, MessageBoxIcon.Error, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
            catch (SoapException se)
            {
                if (se.Code.ToString() == "VSNameLengthException")
                {
                    RtlAwareMessageBox.Show(this, Strings.NameLengthErrorText,
                        Strings.NameLengthErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1, (MessageBoxOptions)0); 
                }
                else if (se.Code.ToString() == "VSParticipantIconSizeException")
                {
                    RtlAwareMessageBox.Show(this, Strings.ParticipantIconSizeErrorText,
                        Strings.ParticipantIconSizeErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                }
                else if (se.Code.ToString() == "VSEmailAddressException")
                {
                    RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                        Strings.EmailAddressInvalidText, EmailTextBox.Text),
                        Strings.EmailAddressInvalidTitle, MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                }
            }
            catch (Exception ex)
            {
                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture,
                    Strings.UnexpectedConnectionError, ex.ToString()), Strings.ErrorConnectingToVenueServer, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 
                    (MessageBoxOptions)0);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void EnableOK(object sender, System.EventArgs e)
        {
            if (!initializing)
            {
                btnOK.Enabled = true;
            }
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            this.btnCancel.Font = UIFont.StringFont;
            this.btnOK.Font = UIFont.StringFont;
            this.linkPrivacyPolicy.Font = UIFont.StringFont;
            this.groupBox1.Font = UIFont.StringFont;
            this.groupBox2.Font = UIFont.StringFont;

            this.btnCancel.Text = Strings.Cancel;
            this.btnOK.Text = Strings.OK;
            this.linkPrivacyPolicy.Text = Strings.PrivacyPolicy;
            this.EmailLabel.Text = Strings.Email;
            this.label4.Text = Strings.Name;
            this.groupBox1.Text = Strings.MyInformation;
            this.label1.Text = Strings.Identity;
            this.groupBox2.Text = Strings.MyIcon;
            this.Text = Strings.MyProfile;
        }
    }
}
