using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

using MSR.LST.Services;


namespace MSR.LST.ConferenceXP.ReflectorService
{
    public class AdminUI : System.Windows.Forms.Form
    {
        #region Windows Form Designer generated private members

        private System.Windows.Forms.Button forceLeaveBtn;
        private System.Windows.Forms.Button refreshBtn;
        private System.Windows.Forms.ListBox tableListBox;
        private System.Windows.Forms.Label clientAddress;
        private System.Windows.Forms.Label participantsLbl;
        private MSR.LST.Services.BasicServiceButtons serviceBtns;
        
        #endregion

        #region Members

        private readonly string helpUrlReflector = Strings.HelpURLReflector;
        private Label groupAddress;
        private Label joinTime;
        private ServiceControlButtons serviceControl;
        private SimpleServiceController service;
        private IContainer components = null;
        private AdminRemoting remoteAdmin = null;

        #endregion
        
        #region Constructor, Dispose
        
        public AdminUI()
        {
            // Required for Windows Form Designer support.
            InitializeComponent();

            service = new SimpleServiceController(ReflectorMgr.ReflectorServiceName);

            serviceBtns.HelpUrl = helpUrlReflector;
            serviceBtns.AboutClicked += new EventHandler(ShowAboutMsg);
            serviceControl.ServiceName = ReflectorMgr.ReflectorServiceName;

            serviceControl.ServiceStarted += new ServiceControlButtons.ServiceStartedEventHandler(ServiceButtonsClicked);
            serviceControl.ServiceStopped += new ServiceControlButtons.ServiceStoppedEventHandler(ServiceButtonsClicked);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Override the system UICulture
            string cultureOverride = null;
            if ((cultureOverride = ConfigurationManager.AppSettings["MSR.LST.Reflector.UICulture"]) != null) {
                try {
                    CultureInfo ci = new CultureInfo(cultureOverride);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                }
                catch { }
            }

            Application.EnableVisualStyles();
            Application.Run(new AdminUI());
        }

        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose( disposing );
        }

        #endregion

        #region Windows Form Designer generated code

        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminUI));
            this.refreshBtn = new System.Windows.Forms.Button();
            this.forceLeaveBtn = new System.Windows.Forms.Button();
            this.participantsLbl = new System.Windows.Forms.Label();
            this.clientAddress = new System.Windows.Forms.Label();
            this.tableListBox = new System.Windows.Forms.ListBox();
            this.groupAddress = new System.Windows.Forms.Label();
            this.joinTime = new System.Windows.Forms.Label();
            this.serviceBtns = new MSR.LST.Services.BasicServiceButtons();
            this.serviceControl = new MSR.LST.Services.ServiceControlButtons();
            this.SuspendLayout();
            // 
            // refreshBtn
            // 
            this.refreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshBtn.Enabled = false;
            this.refreshBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.refreshBtn.Location = new System.Drawing.Point(399, 420);
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(115, 26);
            this.refreshBtn.TabIndex = 0;
            this.refreshBtn.Text = "Refresh";
            this.refreshBtn.Click += new System.EventHandler(this.refreshBtn_Click);
            // 
            // forceLeaveBtn
            // 
            this.forceLeaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.forceLeaveBtn.Enabled = false;
            this.forceLeaveBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.forceLeaveBtn.Location = new System.Drawing.Point(399, 463);
            this.forceLeaveBtn.Name = "forceLeaveBtn";
            this.forceLeaveBtn.Size = new System.Drawing.Size(115, 26);
            this.forceLeaveBtn.TabIndex = 1;
            this.forceLeaveBtn.Text = "Force Leave";
            this.forceLeaveBtn.Visible = false;
            this.forceLeaveBtn.Click += new System.EventHandler(this.forceLeaveBtn_Click);
            // 
            // participantsLbl
            // 
            this.participantsLbl.AutoSize = true;
            this.participantsLbl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.participantsLbl.ForeColor = System.Drawing.Color.Blue;
            this.participantsLbl.Location = new System.Drawing.Point(19, 4);
            this.participantsLbl.Name = "participantsLbl";
            this.participantsLbl.Size = new System.Drawing.Size(123, 15);
            this.participantsLbl.TabIndex = 9;
            this.participantsLbl.Text = "Reflector Participants";
            this.participantsLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // clientAddress
            // 
            this.clientAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.clientAddress.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clientAddress.Location = new System.Drawing.Point(19, 26);
            this.clientAddress.Name = "clientAddress";
            this.clientAddress.Size = new System.Drawing.Size(106, 27);
            this.clientAddress.TabIndex = 8;
            this.clientAddress.Text = "Client Address";
            this.clientAddress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableListBox
            // 
            this.tableListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableListBox.ItemHeight = 15;
            this.tableListBox.Location = new System.Drawing.Point(19, 52);
            this.tableListBox.Name = "tableListBox";
            this.tableListBox.Size = new System.Drawing.Size(495, 334);
            this.tableListBox.TabIndex = 7;
            // 
            // groupAddress
            // 
            this.groupAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupAddress.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupAddress.Location = new System.Drawing.Point(192, 26);
            this.groupAddress.Name = "groupAddress";
            this.groupAddress.Size = new System.Drawing.Size(142, 27);
            this.groupAddress.TabIndex = 12;
            this.groupAddress.Text = "Group Address : Port";
            this.groupAddress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // joinTime
            // 
            this.joinTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.joinTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.joinTime.Location = new System.Drawing.Point(401, 26);
            this.joinTime.Name = "joinTime";
            this.joinTime.Size = new System.Drawing.Size(106, 27);
            this.joinTime.TabIndex = 8;
            this.joinTime.Text = "Join Time";
            this.joinTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // serviceBtns
            // 
            this.serviceBtns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceBtns.HelpUrl = null;
            this.serviceBtns.Location = new System.Drawing.Point(24, 528);
            this.serviceBtns.Name = "serviceBtns";
            this.serviceBtns.Size = new System.Drawing.Size(490, 26);
            this.serviceBtns.TabIndex = 10;
            // 
            // serviceControl
            // 
            this.serviceControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceControl.Location = new System.Drawing.Point(24, 420);
            this.serviceControl.Name = "serviceControl";
            this.serviceControl.ServiceName = null;
            this.serviceControl.Size = new System.Drawing.Size(368, 69);
            this.serviceControl.TabIndex = 13;
            // 
            // AdminUI
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(524, 566);
            this.Controls.Add(this.serviceBtns);
            this.Controls.Add(this.tableListBox);
            this.Controls.Add(this.clientAddress);
            this.Controls.Add(this.participantsLbl);
            this.Controls.Add(this.forceLeaveBtn);
            this.Controls.Add(this.refreshBtn);
            this.Controls.Add(this.groupAddress);
            this.Controls.Add(this.joinTime);
            this.Controls.Add(this.serviceControl);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(540, 70577);
            this.MinimumSize = new System.Drawing.Size(540, 250);
            this.Name = "AdminUI";
            this.Text = "ConferenceXP Reflector Service Manager";
            this.Load += new System.EventHandler(this.AdminUI_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.AdminUI_Paint);
            this.Resize += new System.EventHandler(this.AdminUI_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion 

        #region Form Load, Paint, Resize & Close

        private void AdminUI_Load(object sender, System.EventArgs e)
        {
            this.refreshBtn.Font = UIFont.StringFont;
            this.forceLeaveBtn.Font = UIFont.StringFont;
            this.participantsLbl.Font = UIFont.StringFont;
            this.clientAddress.Font = UIFont.StringFont;
            this.tableListBox.Font = UIFont.StringFont;
            this.groupAddress.Font = UIFont.StringFont;
            this.joinTime.Font = UIFont.StringFont;
            this.serviceBtns.Font = UIFont.StringFont;
            this.serviceControl.Font = UIFont.StringFont;

            this.refreshBtn.Text = Strings.Refresh;
            this.forceLeaveBtn.Text = Strings.ForceLeave;
            this.participantsLbl.Text = Strings.ReflectorParticipants;
            this.clientAddress.Text = Strings.ClientAddress;
            //this.lblJoinPort.Text = Strings.ReflectorJoinPort;
            this.groupAddress.Text = Strings.GroupAddress;
            this.joinTime.Text = Strings.JoinTime;
            this.Text = Strings.ConferenceXPReflectorServiceManager;

            // Connect to service (if we can)
            if (serviceControl.ConnectToService())
            {
                SetState(serviceControl.ServiceController.Running);
            }
            else
            {
                // Report errors about both connections in one dialog
                RtlAwareMessageBox.Show(this, Strings.ErrorConnectingText, Strings.ErrorConnectingTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                Close();
            }
        }

        private void AdminUI_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            using(Graphics g = e.Graphics)
            {
                // Draw line across bottom of UI
                int forceLeaveBottom = forceLeaveBtn.Top + forceLeaveBtn.Height;
                int lineY = forceLeaveBottom + (serviceBtns.Top - forceLeaveBottom )/2;
                int lineRight = tableListBox.Left + tableListBox.Width;
                int lineLeft = tableListBox.Left;
                DrawLine(g, lineY, lineLeft, lineRight);

                // Draw line next to Reflector Participants label
                lineY = participantsLbl.Top + participantsLbl.Height/2;
                lineLeft = participantsLbl.Left + participantsLbl.Width;
                DrawLine(g, lineY, lineLeft, lineRight);
            }
        }

        private static void DrawLine(Graphics g, int lineY, int lineLeft, int lineRight)
        {
            g.DrawLine(SystemPens.ControlDark, lineLeft, lineY, lineRight, lineY);
            lineY += 1;
            g.DrawLine(SystemPens.ControlLightLight, lineLeft, lineY, lineRight, lineY);
        }

        private void AdminUI_Resize(object sender, System.EventArgs e)
        {
            Refresh();
        }

        private void AdminUI_Close(object sender, EventArgs e)
        {
            ServiceEventsDetached();
        }

        #endregion


        #region Event Handlers, UI Actions
        
        private void refreshBtn_Click(object sender, System.EventArgs e)
        {
            RefreshClientTable();
        }

        private void forceLeaveBtn_Click(object sender, System.EventArgs e)
        {
            remoteAdmin.ForceLeave((ClientEntry)tableListBox.SelectedItem);
            RefreshClientTable();
        }

        private void ShowAboutMsg(object sender, System.EventArgs e)
        {
            Assembly reflector = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(reflector.Location);
            string version = reflector.GetName().Name + " : " + fvi.FileVersion;

            RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                Strings.AboutCxpReflectorServiceText, version), Strings.AboutCxpReflectorServiceTitle, 
                MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
        }

        private void ServiceButtonsClicked(object sender, EventArgs e)
        {
            SetState(serviceControl.ServiceController.Running);
        }

        private void ServiceEventsDetached()
        {
            // Detach events
            serviceControl.ServiceStarted -= new ServiceControlButtons.ServiceStartedEventHandler(ServiceButtonsClicked);
            serviceControl.ServiceStopped -= new ServiceControlButtons.ServiceStoppedEventHandler(ServiceButtonsClicked);
        }

        #endregion

        #region Helper methods

        private void RefreshClientTable()
        {
            // Invoke a method on the remote object.
            tableListBox.DataSource = remoteAdmin.GetClientTable();
        }

        private void SetState(bool serviceRunning)
        {
            refreshBtn.Enabled = serviceRunning;
            forceLeaveBtn.Enabled = serviceRunning;

            if (serviceRunning)
            {
                // Create an instance of the remote object
                if (remoteAdmin == null)
                {
                    remoteAdmin = (AdminRemoting)Activator.GetObject(typeof(AdminRemoting),
                        "tcp://localhost:" + ConfigurationManager.AppSettings.Get(AppConfig.AdminPort) +
                        "/ReflectorAdminEndpoint");
                }

                RefreshClientTable();
            }
            else
            {
                remoteAdmin = null;
                tableListBox.DataSource = null;
                tableListBox.Items.Clear();
                //lblDynamicPort.Text = string.Empty;
            }
        }

        #endregion
    }
}
