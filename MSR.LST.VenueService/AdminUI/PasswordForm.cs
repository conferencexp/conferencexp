using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace MSR.LST.ConferenceXP.VenueService
{
    public partial class PasswordForm : Form
    {

        private String password;
        private PasswordStatus passwordStatus = PasswordStatus.NO_PASSWORD;

        public PasswordStatus PWStatus
        {
            get { return passwordStatus; }
        }

        public String Password
        {
            get { return password; }
        }
        
        public PasswordForm()
        {

            this.password = "";
            InitializeComponent();
        }


        // Confirm password selection
        private void button2_Click(object sender, EventArgs e)
        {
            String password1 = textBox1.Text;
            String password2 = textBox2.Text;

            int comparison = String.Compare(password1, password2);
            if (comparison == 0)
            {
                // passwords match
                this.password = password1.Trim();
                ComputePasswordStatus();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                // passwords don't match: show error message
                DialogResult dr = RtlAwareMessageBox.Show(this, 
                    string.Format(CultureInfo.CurrentCulture, Strings.PasswordsDoNotMatch), 
                string.Format(CultureInfo.CurrentCulture,   Strings.PasswordError),
                MessageBoxButtons.OK, MessageBoxIcon.Error, 
                MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }


        }

        private void ComputePasswordStatus()
        {
            if (password == null || password.Trim().Length == 0)
                passwordStatus = PasswordStatus.NO_PASSWORD;
            else if (useEncryption.Checked)
                passwordStatus = PasswordStatus.STRONG_PASSWORD;
            else passwordStatus = PasswordStatus.WEAK_PASSWORD;
        }

        // cancel password selection; no changes result
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void PasswordForm_Load(object sender, EventArgs e)
        {
            this.button1.Font = UIFont.StringFont;
            this.button2.Font = UIFont.StringFont;
            this.label1.Font = UIFont.StringFont;
            this.label2.Font = UIFont.StringFont;
            this.textBox1.Font = UIFont.StringFont;
            this.textBox2.Font = UIFont.StringFont;
            this.useEncryption.Font = UIFont.StringFont;

            this.button1.Text = Strings.Cancel;
            this.button2.Text = Strings.Confirm;
            this.Text = Strings.VenuePassword;
            this.label1.Text = Strings.Password;
            this.label2.Text = Strings.ConfirmPassword;
            this.useEncryption.Text = Strings.UseEncryption;

            this.textBox1.Text = this.password;
            this.textBox2.Text = this.password;
        }
    }
}
