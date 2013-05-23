using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

using MSR.LST.Net;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// Summary description for frmNetworkUnicast.
    /// </summary>
    public class frmNetworkUnicast : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtRemoteIP;
        private System.Windows.Forms.ComboBox cmbMyIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public frmNetworkUnicast()
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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbMyIP = new System.Windows.Forms.ComboBox();
            this.txtRemoteIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblInfo.Location = new System.Drawing.Point(8, 8);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(344, 40);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Two-way conferencing over unicast is typically used when you are having trouble w" +
                "ith multicast connectivity and cannot access a ConferenceXP Reflector Service.";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(248, 240);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(128, 240);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbMyIP);
            this.groupBox1.Controls.Add(this.txtRemoteIP);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(8, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 112);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Exchange IP addresses";
            // 
            // cmbMyIP
            // 
            this.cmbMyIP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMyIP.Location = new System.Drawing.Point(8, 32);
            this.cmbMyIP.Name = "cmbMyIP";
            this.cmbMyIP.Size = new System.Drawing.Size(322, 22);
            this.cmbMyIP.TabIndex = 3;
            // 
            // txtRemoteIP
            // 
            this.txtRemoteIP.Location = new System.Drawing.Point(8, 80);
            this.txtRemoteIP.Name = "txtRemoteIP";
            this.txtRemoteIP.Size = new System.Drawing.Size(322, 20);
            this.txtRemoteIP.TabIndex = 2;
            this.txtRemoteIP.TextChanged += new System.EventHandler(this.txtRemoteIP_TextChanged);
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(312, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Give the remote participant the IP address of your computer:";
            // 
            // label2
            // 
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(8, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(304, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Type the IP address of the remote participant\'s computer:";
            // 
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Location = new System.Drawing.Point(16, 184);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(336, 48);
            this.label3.TabIndex = 7;
            this.label3.Text = "Note: If your computer has more than one network card, it may have more than one " +
                "IP address. The IP address that appears above is the recommended IP address for " +
                "two-way conferencing.";
            // 
            // frmNetworkUnicast
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(362, 272);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblInfo);
            this.Font = UIFont.FormFont;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNetworkUnicast";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Two-Way Conference Over Unicast";
            this.Load += new System.EventHandler(this.frmNetworkUnicast_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private void txtRemoteIP_TextChanged(object sender, System.EventArgs e)
        {
            btnOK.Enabled = txtRemoteIP.TextLength >= 7;
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            // Validate unicast string
            try
            {
                IPAddress remoteIP = IPAddress.Parse(txtRemoteIP.Text);
                IPAddress myIP = IPAddress.Parse(cmbMyIP.Text);

                if (myIP.AddressFamily == remoteIP.AddressFamily)
                {
                    FMain.remoteIP = remoteIP;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    RtlAwareMessageBox.Show(this, Strings.NetworkConfigErrorText, Strings.NetworkConfigErrorTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                        (MessageBoxOptions)0);
                }
            }
            catch (Exception ex)
            {
                RtlAwareMessageBox.Show(this, ex.ToString(), Strings.NetworkConfigErrorTitle, MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void GetAndAddInterfaceForIP(string strIPAddress)
        {
            try
            {
                IPAddress ipInterface = Utility.GetLocalRoutingInterface(IPAddress.Parse(strIPAddress));

                // GetLocalRoutingInterface can return null
                if (ipInterface != null)
                {
                    cmbMyIP.Items.Add(ipInterface.ToString());
                }
            }
            // If the IPv6 or IPv4 protocol stack is not installed we skip finding an IP address for that interface.
            catch (FormatException) { }
            catch (ArgumentNullException) { }
        }

        private void frmNetworkUnicast_Load(object sender, System.EventArgs e)
        {
            this.lblInfo.Font = UIFont.StringFont;
            this.btnCancel.Font = UIFont.StringFont;
            this.btnOK.Font = UIFont.StringFont;
            this.groupBox1.Font = UIFont.StringFont;
            this.cmbMyIP.Font = new System.Drawing.Font("Courier New", UIFont.Size);
            this.txtRemoteIP.Font = new System.Drawing.Font("Courier New", UIFont.Size);
            this.label1.Font = UIFont.StringFont;
            this.label2.Font = UIFont.StringFont;
            this.label3.Font = UIFont.StringFont;

            this.lblInfo.Text = Strings.TwoWayUnicastConferencing;
            this.btnCancel.Text = Strings.Cancel;
            this.btnOK.Text = Strings.OK;
            this.groupBox1.Text = Strings.ExchangeIPAddresses;
            this.label1.Text = Strings.GiveIPAddress;
            this.label2.Text = Strings.TypeIPAddress;
            this.label3.Text = Strings.RecommendedIPAddress;
            this.Text = Strings.TwoWayUnicastConference;

            // Find the interface which is routed toward the Root name servers
            GetAndAddInterfaceForIP("198.41.0.4");
            // Find the interface which is routed toward 6bone.net
            GetAndAddInterfaceForIP("2001:5c0:0:2::24");
            // The first item added (IPv4 address) is the default item.
            cmbMyIP.SelectedText = cmbMyIP.Items[0].ToString();
            cmbMyIP.SelectedIndex = 0;

            // If remote ip has been set while the client has been running,
            // then populate fields since it is likely you want to enter the same venue
            if (FMain.remoteIP != null)
            {
                txtRemoteIP.Text = FMain.remoteIP.ToString();
                // If the remoteIP is an IPv6 address default to the local IPv6 interface
                if (FMain.remoteIP.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    // Assuming if there is an IPv6 address in the list of local interfaces
                    // it's the last one.
                    cmbMyIP.SelectedText = cmbMyIP.Items[cmbMyIP.Items.Count - 1].ToString();
                    cmbMyIP.SelectedIndex = cmbMyIP.Items.Count - 1;
                }
            }
            else
            {
                txtRemoteIP.Text = string.Empty;
            }
        }
    }
}
