using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// Summary description for AdvancedDialog.
    /// </summary>
    public class AdvancedDialog : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkFEC;
        public bool EnableFEC
        {
            get { return checkFEC.Checked; }
            set { checkFEC.Checked = value; }
        }

        private System.Windows.Forms.NumericUpDown numFECAmount;
        public int FECAmount
        {
            get { return (int)numFECAmount.Value; }
            set { numFECAmount.Value = value; }
        }

        private System.Windows.Forms.Label lblMaxBW;
        private System.Windows.Forms.NumericUpDown numMaxBW;
        public int MaxBandwidth
        {
            get { return (int)numMaxBW.Value; }
            set { numMaxBW.Value = value; }
        }

        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.Label lblFECAmount;
        private System.Windows.Forms.Label label1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public AdvancedDialog()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.lblFECAmount = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPercent = new System.Windows.Forms.Label();
            this.numMaxBW = new System.Windows.Forms.NumericUpDown();
            this.lblMaxBW = new System.Windows.Forms.Label();
            this.numFECAmount = new System.Windows.Forms.NumericUpDown();
            this.checkFEC = new System.Windows.Forms.CheckBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxBW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFECAmount)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFECAmount
            // 
            this.lblFECAmount.AutoSize = true;
            this.lblFECAmount.Location = new System.Drawing.Point(128, 35);
            this.lblFECAmount.Name = "lblFECAmount";
            this.lblFECAmount.Size = new System.Drawing.Size(46, 13);
            this.lblFECAmount.TabIndex = 0;
            this.lblFECAmount.Text = "Amount:";
            this.lblFECAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblPercent);
            this.groupBox1.Controls.Add(this.numMaxBW);
            this.groupBox1.Controls.Add(this.lblMaxBW);
            this.groupBox1.Controls.Add(this.numFECAmount);
            this.groupBox1.Controls.Add(this.checkFEC);
            this.groupBox1.Controls.Add(this.lblFECAmount);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 112);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Advanced settings";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(240, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "B/s";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPercent
            // 
            this.lblPercent.AutoSize = true;
            this.lblPercent.Location = new System.Drawing.Point(242, 35);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(15, 13);
            this.lblPercent.TabIndex = 4;
            this.lblPercent.Text = "%";
            this.lblPercent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numMaxBW
            // 
            this.numMaxBW.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numMaxBW.Location = new System.Drawing.Point(120, 70);
            this.numMaxBW.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numMaxBW.Name = "numMaxBW";
            this.numMaxBW.Size = new System.Drawing.Size(120, 20);
            this.numMaxBW.TabIndex = 3;
            this.numMaxBW.ThousandsSeparator = true;
            this.numMaxBW.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            // 
            // lblMaxBW
            // 
            this.lblMaxBW.AutoSize = true;
            this.lblMaxBW.Location = new System.Drawing.Point(24, 72);
            this.lblMaxBW.Name = "lblMaxBW";
            this.lblMaxBW.Size = new System.Drawing.Size(82, 13);
            this.lblMaxBW.TabIndex = 2;
            this.lblMaxBW.Text = "Max bandwidth:";
            this.lblMaxBW.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numFECAmount
            // 
            this.numFECAmount.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numFECAmount.Location = new System.Drawing.Point(184, 33);
            this.numFECAmount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numFECAmount.Name = "numFECAmount";
            this.numFECAmount.Size = new System.Drawing.Size(56, 20);
            this.numFECAmount.TabIndex = 1;
            this.numFECAmount.ThousandsSeparator = true;
            this.numFECAmount.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // checkFEC
            // 
            this.checkFEC.Checked = true;
            this.checkFEC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkFEC.Location = new System.Drawing.Point(24, 31);
            this.checkFEC.Name = "checkFEC";
            this.checkFEC.Size = new System.Drawing.Size(104, 24);
            this.checkFEC.TabIndex = 0;
            this.checkFEC.Text = "&Enable FEC";
            this.checkFEC.CheckedChanged += new System.EventHandler(this.checkFEC_CheckedChanged);
            // 
            // btnApply
            // 
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnApply.Location = new System.Drawing.Point(120, 128);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "&OK";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(213, 128);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            // 
            // AdvancedDialog
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(296, 163);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "AdvancedDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Advanced";
            this.Load += new System.EventHandler(this.AdvancedDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxBW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFECAmount)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void checkFEC_CheckedChanged(object sender, System.EventArgs e)
        {
            lblFECAmount.Enabled = numFECAmount.Enabled = lblPercent.Enabled = checkFEC.Checked;
        }

        private void btnApply_Click(object sender, System.EventArgs e)
        {
            RtlAwareMessageBox.Show(this, Strings.TheseSettingsWillTakeEffect, Strings.Information, 
                MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 
                (MessageBoxOptions)0);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void AdvancedDialog_Load(object sender, EventArgs e)
        {
            this.lblFECAmount.Text = Strings.Amount;
            this.groupBox1.Text = Strings.AdvancedSettings;
            this.label1.Text = Strings.Bps;
            this.lblPercent.Text = Strings.PercentSign;
            this.lblMaxBW.Text = Strings.MaxBandwidth;
            this.checkFEC.Text = Strings.EnableFECHotkey;
            this.btnApply.Text = Strings.OKHotkey;
            this.btnCancel.Text = Strings.CancelHotkey;
            this.Text = Strings.Advanced;
        }
    }
}
