using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP.VenueService
{
    internal class ParticipantEditor : System.Windows.Forms.Form
    {
        #region Private Winform Vars
        private System.Windows.Forms.TextBox nameInput;
        private System.Windows.Forms.Label nameLbl;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button okBtn;
        private MSR.LST.ConferenceXP.VenueService.IconInput iconInput;
        private System.ComponentModel.Container components = null;
        #endregion

        #region Private Variables
        // The set of all controls that currently have invalid inputs
        ArrayList invalidInputs = new ArrayList();

        public static readonly Image defaultParticipantIcon;
        private System.Windows.Forms.Label identifierLbl;
        private System.Windows.Forms.Label emailLbl;
        private System.Windows.Forms.TextBox emailInput;
        private System.Windows.Forms.TextBox identifierInput;
        private readonly Participant original;
        #endregion

        #region Ctor, Dispose
        static ParticipantEditor()
        {
            // Load the default venue icon
            Stream resource = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.VenueService.GenericParticipantIcon.png");
            defaultParticipantIcon = Image.FromStream(resource);
        }

        public ParticipantEditor(Participant participantToEdit)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            iconInput.DefaultIcon = defaultParticipantIcon;

            // Show the venue information in the UI
            this.nameInput.Text = participantToEdit.Name;
            this.identifierInput.Text = participantToEdit.Identifier;
            this.emailInput.Text = participantToEdit.Email;
            this.iconInput.IconAsBytes = participantToEdit.Icon;

            this.original = participantToEdit;
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
        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.identifierLbl = new System.Windows.Forms.Label();
            this.identifierInput = new System.Windows.Forms.TextBox();
            this.nameInput = new System.Windows.Forms.TextBox();
            this.nameLbl = new System.Windows.Forms.Label();
            this.emailInput = new System.Windows.Forms.TextBox();
            this.emailLbl = new System.Windows.Forms.Label();
            this.iconInput = new MSR.LST.ConferenceXP.VenueService.IconInput();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // identifierLbl
            // 
            this.identifierLbl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.identifierLbl.Location = new System.Drawing.Point(16, 12);
            this.identifierLbl.Name = "identifierLbl";
            this.identifierLbl.Size = new System.Drawing.Size(100, 16);
            this.identifierLbl.TabIndex = 0;
            this.identifierLbl.Text = "Identifier:";
            this.identifierLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // identifierInput
            // 
            this.identifierInput.Enabled = false;
            this.identifierInput.Location = new System.Drawing.Point(16, 28);
            this.identifierInput.Name = "identifierInput";
            this.identifierInput.Size = new System.Drawing.Size(240, 20);
            this.identifierInput.TabIndex = 0;
            this.identifierInput.Text = "jay@tailspintoys.com";
            // 
            // nameInput
            // 
            this.nameInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameInput.Location = new System.Drawing.Point(16, 72);
            this.nameInput.Name = "nameInput";
            this.nameInput.Size = new System.Drawing.Size(240, 20);
            this.nameInput.TabIndex = 1;
            this.nameInput.Text = "Jay Jones";
            // 
            // nameLbl
            // 
            this.nameLbl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.nameLbl.Location = new System.Drawing.Point(16, 56);
            this.nameLbl.Name = "nameLbl";
            this.nameLbl.Size = new System.Drawing.Size(100, 16);
            this.nameLbl.TabIndex = 0;
            this.nameLbl.Text = "Name:";
            this.nameLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // emailInput
            // 
            this.emailInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.emailInput.Location = new System.Drawing.Point(16, 116);
            this.emailInput.Name = "emailInput";
            this.emailInput.Size = new System.Drawing.Size(240, 20);
            this.emailInput.TabIndex = 2;
            this.emailInput.Text = "jay@tailspintoys.com";
            // 
            // emailLbl
            // 
            this.emailLbl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.emailLbl.Location = new System.Drawing.Point(16, 100);
            this.emailLbl.Name = "emailLbl";
            this.emailLbl.Size = new System.Drawing.Size(100, 16);
            this.emailLbl.TabIndex = 0;
            this.emailLbl.Text = "E-Mail:";
            this.emailLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // iconInput
            // 
            this.iconInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.iconInput.DefaultIcon = null;
            this.iconInput.Icon = null;
            this.iconInput.IconAsBytes = null;
            this.iconInput.Location = new System.Drawing.Point(16, 148);
            this.iconInput.Name = "iconInput";
            this.iconInput.Size = new System.Drawing.Size(240, 104);
            this.iconInput.TabIndex = 5;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cancelBtn.Location = new System.Drawing.Point(184, 268);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 4;
            this.cancelBtn.Text = "Cancel";
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okBtn.Location = new System.Drawing.Point(96, 268);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 4;
            this.okBtn.Text = "OK";
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // ParticipantEditor
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(274, 304);
            this.ControlBox = false;
            this.Controls.Add(this.iconInput);
            this.Controls.Add(this.identifierInput);
            this.Controls.Add(this.nameInput);
            this.Controls.Add(this.emailInput);
            this.Controls.Add(this.identifierLbl);
            this.Controls.Add(this.nameLbl);
            this.Controls.Add(this.emailLbl);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Font = UIFont.FormFont;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ParticipantEditor";
            this.ShowInTaskbar = false;
            this.Text = "Participant Editor";
            this.Load += new System.EventHandler(this.ParticipantEditor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    
        #region Public Properties & Methods
        /// <summary>
        /// Gets the original participant this editor started with (unedited).
        /// </summary>
        public Participant OriginalParticipant
        {
            get
            {
                return original;
            }
        }

        /// <summary>
        /// Compiles all of the data on the screen and returns it as a venue.
        /// </summary>
        public Participant GetParticipant()
        {
            Participant part = new Participant(this.identifierInput.Text, this.nameInput.Text, 
                this.emailInput.Text, this.iconInput.IconAsBytes);
            return part;
        }
        #endregion

        #region Input Validation
        private void okBtn_Click(object sender, System.EventArgs e)
        {
            bool idValid = ValidateIdentifierInput();
            bool emailValid = ValidateEmailInput();

            if (idValid && emailValid)
            {
                // The input is valid.  Just return.
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // Just show one message for one bad input & send the user back to fix the one input
                string error = null;
                if (!idValid)
                    error = Strings.InvalidIdentifierInput;
                else if (!emailValid)
                    error = Strings.InvalidEmailInput;
                
                RtlAwareMessageBox.Show(this, error, Strings.InvalidInput, MessageBoxButtons.OK, MessageBoxIcon.Error, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
        }

        private bool ValidateIdentifierInput()
        {
            Regex reg = new Regex(SecurityPatterns.EmailValidator, RegexOptions.IgnoreCase);
            bool valid = reg.IsMatch(identifierInput.Text);

            return valid;
        }

        private bool ValidateEmailInput()
        {
            bool valid = true;
            // Unlike the identifier, we can accept a null input here
            if (emailInput.Text != null && emailInput.Text != string.Empty)
            {
                Regex reg = new Regex(SecurityPatterns.EmailValidator, RegexOptions.IgnoreCase);
                valid = reg.IsMatch(identifierInput.Text);
            }
            
            return valid;
        }
        #endregion

        private void ParticipantEditor_Load(object sender, EventArgs e)
        {
            this.identifierLbl.Font = UIFont.StringFont;
            this.identifierInput.Font = UIFont.StringFont;
            this.nameInput.Font = UIFont.StringFont;
            this.nameLbl.Font = UIFont.StringFont;
            this.emailInput.Font = UIFont.StringFont;
            this.emailLbl.Font = UIFont.StringFont;
            this.iconInput.Font = UIFont.StringFont;
            this.cancelBtn.Font = UIFont.StringFont;
            this.okBtn.Font = UIFont.StringFont;

            this.identifierLbl.Text = Strings.IdentifierColon;
            this.nameLbl.Text = Strings.NameColon;
            this.emailLbl.Text = Strings.EmailColon;
            this.cancelBtn.Text = Strings.Cancel;
            this.okBtn.Text = Strings.OK;
            this.Text = Strings.ParticipantEditor;
        }

    }
}
