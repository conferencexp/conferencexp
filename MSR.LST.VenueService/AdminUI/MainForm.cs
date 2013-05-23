using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

using Microsoft.Win32;


namespace MSR.LST.ConferenceXP.VenueService
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    internal class MainForm : System.Windows.Forms.Form
    {
        #region Winform Private Vars
        private System.Windows.Forms.Label venuesLbl;
        private System.Windows.Forms.Label participantsLbl;
        private MSR.LST.ConferenceXP.VenueService.VenueManager venues;
        private MSR.LST.ConferenceXP.VenueService.ParticipantManager participants;
        private System.ComponentModel.IContainer components = null;
        private MSR.LST.Services.BasicServiceButtons basicButtons;
        #endregion

        #region Private Variables
        private readonly int[] resizeConstants;
        private FileStorage storage;

        private readonly string helpUrl = Strings.HelpURLVenueService;
        #endregion

        #region Ctor, Main, Dispose
        [STAThread]
        static void Main()
        {
            // Override the system UICulture
            string cultureOverride = null;
            if ((cultureOverride = ConfigurationManager.AppSettings["MSR.LST.ConferenceXP.VenueService.UICulture"]) != null) {
                try {
                    System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(cultureOverride);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                }
                catch { }
            }

            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Get the UI constants to use for resizing
            resizeConstants = this.SpacingConstants;

            basicButtons.HelpUrl = helpUrl;
            basicButtons.AboutClicked += new EventHandler(this.aboutBtn_Click);
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
        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.venuesLbl = new System.Windows.Forms.Label();
            this.venues = new MSR.LST.ConferenceXP.VenueService.VenueManager();
            this.participants = new MSR.LST.ConferenceXP.VenueService.ParticipantManager();
            this.participantsLbl = new System.Windows.Forms.Label();
            this.basicButtons = new MSR.LST.Services.BasicServiceButtons();
            this.SuspendLayout();
            // 
            // venuesLbl
            // 
            this.venuesLbl.AutoSize = true;
            this.venuesLbl.ForeColor = System.Drawing.Color.Blue;
            this.venuesLbl.Location = new System.Drawing.Point(16, 4);
            this.venuesLbl.Name = "venuesLbl";
            this.venuesLbl.Size = new System.Drawing.Size(88, 20);
            this.venuesLbl.TabIndex = 0;
            this.venuesLbl.Text = "Manage Venues";
            this.venuesLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // venues
            // 
            this.venues.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.venues.Location = new System.Drawing.Point(8, 16);
            this.venues.Name = "venues";
            this.venues.Size = new System.Drawing.Size(464, 192);
            this.venues.TabIndex = 1;
            this.venues.VariableWidthColumns = new int[] {
        0,
        1};
            // 
            // participants
            // 
            this.participants.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.participants.Location = new System.Drawing.Point(8, 220);
            this.participants.Name = "participants";
            this.participants.Size = new System.Drawing.Size(464, 192);
            this.participants.TabIndex = 2;
            this.participants.VariableWidthColumns = new int[] {
        0,
        1,
        2};
            // 
            // participantsLbl
            // 
            this.participantsLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.participantsLbl.AutoSize = true;
            this.participantsLbl.ForeColor = System.Drawing.Color.Blue;
            this.participantsLbl.Location = new System.Drawing.Point(16, 204);
            this.participantsLbl.Name = "participantsLbl";
            this.participantsLbl.Size = new System.Drawing.Size(112, 20);
            this.participantsLbl.TabIndex = 0;
            this.participantsLbl.Text = "Manage Participants";
            this.participantsLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // basicButtons
            // 
            this.basicButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.basicButtons.HelpUrl = null;
            this.basicButtons.Location = new System.Drawing.Point(16, 428);
            this.basicButtons.Name = "basicButtons";
            this.basicButtons.Size = new System.Drawing.Size(448, 24);
            this.basicButtons.TabIndex = 7;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(480, 466);
            this.Controls.Add(this.participantsLbl);
            this.Controls.Add(this.participants);
            this.Controls.Add(this.venuesLbl);
            this.Controls.Add(this.venues);
            this.Controls.Add(this.basicButtons);
            this.Font = UIFont.FormFont;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(312, 300);
            this.Name = "MainForm";
            this.Text = "ConferenceXP Venue Service Manager";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }
        #endregion

        #region Events: Resize & Paint
        private void MainForm_Resize(object sender, System.EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                // Set the size & location of the two ItemManagers
                this.SpacingConstants = resizeConstants;

                // Also, do a refresh to re-paint the lines
                Refresh();
            }
        }

        /// <summary>
        /// A set of get/set operations that retains the spacing between components set
        /// at design-time and uses them during resize operations.
        /// </summary>
        /// <remarks>
        /// This is necessary to handle the "sophisticated" "space-sharing" components in the UI.
        /// Also, note that we ignore width and x-location here, as simply setting the Anchor property
        /// in the Form Editor handled that.
        /// </remarks>
        private int[] SpacingConstants
        {
            get
            {
                int[] constants = new int[3];
                int venuesBottom = venues.Top + venues.Height;
                // 0: the distance between the two ItemManagers (a buffer space)
                constants[0] = participants.Top - venuesBottom;
                int participantsBottom = participants.Top + participants.Height;
                // 1: the distance between the closeBtn and the bottom of participants
                constants[1] = basicButtons.Top - participantsBottom;
                // 2: the distance betwen the top of the participants label and participants
                constants[2] = participantsLbl.Top - participants.Top;

                return constants;
            }
            set
            {
                // find where the bottom of participants will be
                int participantsBottom = basicButtons.Top - value[1];
                int spaceAvailable = participantsBottom - venues.Top;
                // find the height of each ItemManager, which is half the total space minus the buffer
                int spacePerItemManager = (spaceAvailable - value[0]) / 2;
                // put the ItemManagers where they should be
                venues.Height = spacePerItemManager;
                int venuesBottom = venues.Top + venues.Height;
                participants.Top = venuesBottom + value[0];
                participants.Height = spacePerItemManager;
                // put the label at the right height
                participantsLbl.Top = participants.Top + value[2];
            }
        }

        private void MainForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // Draw in the three lines in the UI
            using (Graphics g = e.Graphics)
            {
                // Draw line across bottom of UI
                int participantsBottom = participants.Top + participants.Height;
                int lineY = participantsBottom + (basicButtons.Top - participantsBottom) / 2;
                int lineRight = basicButtons.Left + basicButtons.Width;
                int lineLeft = basicButtons.Left;
                DrawLine(g, lineY, lineLeft, lineRight);

                // Draw line next to Venues label
                lineY = venuesLbl.Top + venuesLbl.Height / 2;
                lineLeft = venuesLbl.Left + venuesLbl.Width;
                DrawLine(g, lineY, lineLeft, lineRight);

                // Draw a line next to the Participants label
                lineY = participantsLbl.Top + participantsLbl.Height / 2;
                lineLeft = participantsLbl.Left + participantsLbl.Width;
                DrawLine(g, lineY, lineLeft, lineRight);
            }
        }

        private void DrawLine(Graphics g, int lineY, int lineLeft, int lineRight)
        {
            g.DrawLine(SystemPens.ControlDark, lineLeft, lineY, lineRight, lineY);
            lineY += 1;
            g.DrawLine(SystemPens.ControlLightLight, lineLeft, lineY, lineRight, lineY);
        }
        #endregion

        #region Events: Load
        private void MainForm_Load(object sender, System.EventArgs e)
        {
            this.venuesLbl.Font = UIFont.StringFont;
            this.venues.Font = UIFont.StringFont;
            this.participants.Font = UIFont.StringFont;
            this.participantsLbl.Font = UIFont.StringFont;
            this.basicButtons.Font = UIFont.StringFont;

            this.venuesLbl.Text = Strings.ManageVenues;
            this.participantsLbl.Text = Strings.ManageParticipants;
            this.Text = Strings.ConferencexpVenueServiceManager;

            try
            {
                // Start by getting the venue service storage (use the app.config as an override)
                string filePath = ConfigurationManager.AppSettings["FilePath"];
                if (filePath == null || filePath == String.Empty)
                {
                    // Use the registry key
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(StorageInstaller.LocalMachineSubkey))
                    {
                        filePath = (string)key.GetValue("DataFile");
                    }
                }

                storage = new FileStorage(filePath);

                this.venues.StorageFile = storage;
                this.participants.StorageFile = storage;
            }
            catch (Exception ex)
            {
                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture,
                    Strings.StorageFailureText, ex.ToString()), Strings.StorageFailureTitle, MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                Close();
            }
        }
        #endregion

        #region About Button
        private void aboutBtn_Click(object sender, System.EventArgs e)
        {
            string versions =
                GetAssemblyDescription(Assembly.GetExecutingAssembly()) + "\n" +
                GetAssemblyDescription(Assembly.Load("Storage"));

            RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture,
                Strings.AboutConferencexpVenueServiceText, versions), Strings.AboutConferencexpVenueServiceTitle,
                MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
        }

        /// <summary>
        /// Stolen from Conference.dll for Help->About
        /// </summary>
        private static string GetAssemblyDescription(Assembly a)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(a.Location);
            return a.GetName().Name + " : " + fvi.FileVersion;
        }
        #endregion

    }
}
