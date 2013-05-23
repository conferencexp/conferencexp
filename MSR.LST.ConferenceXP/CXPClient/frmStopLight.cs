using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using MSR.LST.Net.Rtp;


namespace MSR.LST.ConferenceXP
{
    public class frmStopLight : System.Windows.Forms.Form
    {
        #region Windows Form Designer generated code

        private System.Windows.Forms.MenuItem menuStoplightDetails;

        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.NotifyIcon stoplightNotify;
        private System.Windows.Forms.RichTextBox txtStatus;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.MenuItem menuStoplightExit;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStopLight));
            this.stoplightNotify = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.menuStoplightDetails = new System.Windows.Forms.MenuItem();
            this.menuStoplightExit = new System.Windows.Forms.MenuItem();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // stoplightNotify
            // 
            this.stoplightNotify.ContextMenu = this.contextMenu;
            this.stoplightNotify.Icon = ((System.Drawing.Icon)(resources.GetObject("stoplightNotify.Icon")));
            this.stoplightNotify.Text = "Testing network connection";
            this.stoplightNotify.Visible = true;
            this.stoplightNotify.Click += new System.EventHandler(this.stoplightNotify_Click);
            this.stoplightNotify.DoubleClick += new System.EventHandler(this.stoplightNotify_DoubleClick);
            // 
            // contextMenu
            // 
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuStoplightDetails,
            this.menuStoplightExit});
            // 
            // menuStoplightDetails
            // 
            this.menuStoplightDetails.Index = 0;
            this.menuStoplightDetails.Text = "Open";
            this.menuStoplightDetails.Click += new System.EventHandler(this.menuStoplightDetails_Click);
            // 
            // menuStoplightExit
            // 
            this.menuStoplightExit.Index = 1;
            this.menuStoplightExit.Text = "Exit";
            this.menuStoplightExit.Click += new System.EventHandler(this.menuStoplightExit_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescription.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblDescription.Location = new System.Drawing.Point(16, 16);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(452, 52);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "This utility detects whether your computer is connected to Internet2 " +
                "and to a multicast network. If your computer is not connected to a multicast network, you " +
                "can participate in ConferenceXP conferences over unicast or by using the ConferenceXP " +
                "Reflector Service.";
            // 
            // txtStatus
            // 
            this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatus.BackColor = System.Drawing.SystemColors.Window;
            this.txtStatus.Location = new System.Drawing.Point(16, 110);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(508, 370);
            this.txtStatus.TabIndex = 2;
            this.txtStatus.Text = "Determining status...";
            this.txtStatus.WordWrap = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(476, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // linkLabel1
            // 
            this.linkLabel1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.linkLabel1.Location = new System.Drawing.Point(14, 80);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(216, 25);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "About Internet2 and Multicast Connectivity";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // frmStopLight
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(540, 488);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.lblDescription);
            this.Font = UIFont.FormFont;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(420, 400);
            this.Name = "frmStopLight";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connectivity Detector";
            this.Resize += new System.EventHandler(this.frmStopLight_Resize);
            this.Load += new System.EventHandler(this.frmStopLight_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                // Dispose detector before components, since detector updates UI
                if (detector != null)
                {
                    detector.Dispose();
                    detector = null;
                }

                if (components != null) 
                {
                    components.Dispose();
                }
            }

            base.Dispose( disposing );
        }


        #endregion

        #region Members

       // private ConnectivityDetector detector = null;
        private DateTime startTime;

        private Icon redLight = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.RedLight.ico"));
        private Icon greenLight = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.GreenLight.ico"));
        private Icon yellowLight = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.YellowLight.ico"));
        private Icon whiteLight = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.WhiteLight.ico"));

        private delegate void GuiUpdateHandler(bool networkReceiveDetected,
            bool networkSendDetected, bool wellKnownHostDetected, string details);

        private GuiUpdateHandler updateGui;

        #endregion

        #region Constructor

        public frmStopLight()
        {
            InitializeComponent();

           // detector = new ConnectivityDetector();
         //   detector.Connectivity += new ConnectivityDetector.ConnectivityEventHandler(ConnectivityEvent);

            startTime = DateTime.Now;

            updateGui = new GuiUpdateHandler(UpdateGui);
        }


        #endregion Constructor

        #region Private

        private void ConnectivityEvent(bool networkReceiveDetected, bool networkSendDetected,
            bool wellKnownHostDetected, string details)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(updateGui, networkReceiveDetected, networkSendDetected,
                    wellKnownHostDetected, details);
            }
            else
            {
                UpdateGui(networkReceiveDetected, networkSendDetected,
                    wellKnownHostDetected, details);
            }
        }

        private void UpdateGui(bool networkReceiveDetected, bool networkSendDetected,
            bool wellKnownHostDetected, string details)
        {
            TimeSpan uptime = DateTime.Now.Subtract(startTime);

            bool multicastFunctional = networkReceiveDetected && networkSendDetected;
            bool i2Functional = multicastFunctional && wellKnownHostDetected;

            // ---------------------------------------------------------
            // Tooltips have to be less than 64 characters, be succinct!
            // ---------------------------------------------------------
            if (i2Functional)
            {
                stoplightNotify.Icon = greenLight;
                stoplightNotify.Text = Strings.Internet2Functional;
            }
            else if (multicastFunctional)
            {
                stoplightNotify.Icon = greenLight;
                stoplightNotify.Text = Strings.MulticastFunctional;
            }
            else if (networkReceiveDetected)
            {
                stoplightNotify.Icon = yellowLight;
                stoplightNotify.Text = Strings.ReceiveOnlyNotification;
            }
            else
            {
                if (uptime.Minutes > 2)
                {
                    stoplightNotify.Icon = redLight;
                    stoplightNotify.Text = Strings.MulticastNonfunctional;
                }
            }

            txtStatus.Text = string.Format(CultureInfo.CurrentCulture, Strings.MulticastIPStatus + "\n\n", 
                detector.MulticastIPAddress.ToString(CultureInfo.InvariantCulture), networkReceiveDetected, 
                networkSendDetected, multicastFunctional, i2Functional, uptime.Minutes, 
                uptime.Seconds.ToString("00", CultureInfo.InvariantCulture), details);
        }

        #region Menu Handlers

        private void menuStoplightDetails_Click(object sender, System.EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        private void menuStoplightExit_Click(object sender, System.EventArgs e)
        {
            Hide();
            Dispose();
        }

        #endregion
      
        private void stoplightNotify_Click(object sender, System.EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        private void stoplightNotify_DoubleClick(object sender, System.EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(FMain.helpurlConnectivity);
        }

        #endregion Private

        private void frmStopLight_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.Forms.FormWindowState.Minimized)
            {
                this.WindowState = System.Windows.Forms.FormWindowState.Normal;
                this.Hide();
            }
        }

        private void frmStopLight_Load(object sender, EventArgs e)
        {
            this.lblDescription.Font = UIFont.StringFont;
            this.txtStatus.Font = UIFont.StringFont;
            this.linkLabel1.Font = UIFont.StringFont;

            this.stoplightNotify.Text = Strings.TestingNetworkConnection;
            this.menuStoplightDetails.Text = Strings.Open;
            this.menuStoplightExit.Text = Strings.Exit;
            this.lblDescription.Text = Strings.ThisUtilityDetects;
            this.txtStatus.Text = Strings.DeterminingStatus;
            this.linkLabel1.Text = Strings.AboutInternet2AndMulticast;
            this.Text = Strings.ConnectivityDetector;
        }
    }
}
