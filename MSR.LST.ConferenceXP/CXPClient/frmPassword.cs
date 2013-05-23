using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MSR.LST.ConferenceXP
{
    public partial class frmPassword : Form
    {

        private String password = null;

        public String Password
        {
            get { return password; }
        }

        public frmPassword()
        {
            InitializeComponent();
        }

        // OK
        private void button1_Click(object sender, EventArgs e)
        {
            password = textBox1.Text;
            this.DialogResult = DialogResult.OK;
        }

        // cancel
        private void button2_Click(object sender, EventArgs e)
        {
            password = null;
            this.DialogResult = DialogResult.Cancel;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                password = textBox1.Text;
                this.DialogResult = DialogResult.OK;
            }
        }

        // detect carriage return
        //private void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == (char)13)
        //    {
        //        password = textBox1.Text;
        //        this.DialogResult = DialogResult.OK;
        //    }
        //}
    }
}