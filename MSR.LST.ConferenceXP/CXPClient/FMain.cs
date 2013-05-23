using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Windows.Forms;

using Microsoft.Win32;

using MSR.LST;
using MSR.LST.ConferenceXP.ArchiveService;
using MSR.LST.MDShow;
using MSR.LST.Net.Rtp;
using System.IO;
using System.Reflection;
using MSR.LST.ConferenceXP.VenueService;
using System.Net.Sockets;


namespace MSR.LST.ConferenceXP
{
    public class FMain : System.Windows.Forms.Form
    {
        #region Windows Form Designer generated code

        #region Controls

        private System.ComponentModel.IContainer components;

        private System.Windows.Forms.MenuItem menuActionsCapabilities;
        private System.Windows.Forms.MenuItem menuActionsRecord;
        internal System.Windows.Forms.MenuItem menuActionsPlayback;
        private System.Windows.Forms.MenuItem menuActionsUnicast;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem menuActionsPresentation;
        private System.Windows.Forms.MenuItem menuActionsChat;
        private System.Windows.Forms.MenuItem menuActionsActiveCapabilities;
        private System.Windows.Forms.MenuItem menuSettingsServices;

        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenu contextParticipant;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ColumnHeader columnHeader;
        internal System.Windows.Forms.Button btnLeaveConference;
        private System.Windows.Forms.StatusBar statusBar;

        private System.Windows.Forms.Timer statusBarTimer;

        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuSettings;
        private System.Windows.Forms.MenuItem menuHelpAbout;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuHelpCommunity;
        private System.Windows.Forms.MenuItem menuMyProfile;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuHelpConferenceXP;
        private System.Windows.Forms.MenuItem menuSettingsAudioVideo2;
        private System.Windows.Forms.MenuItem menuSettingsRTDocViewer;
        private System.Windows.Forms.MenuItem menuActions;
        private System.Windows.Forms.MenuItem menuActionsWMPlayback;
        private System.Windows.Forms.MenuItem menuActionsScreenScraper;
        private System.Windows.Forms.MenuItem menuActionsSharedBrowser;
        private System.Windows.Forms.MenuItem menuActionsUWClassroomPresenter;
        private System.Windows.Forms.MenuItem menuItem7;
        private MenuItem menuItem8;
        private MenuItem menuActionsPersist;
        private System.Windows.Forms.MenuItem menuSettingsAppConfig;

        private StatusBarPanel messagePanel;
        private StatusBarPanel diagnosticPanel;

        private Icon redLight = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.RedLight.ico"));
        private MenuItem menuItem9;
        private MenuItem menuActionsDiagnostics;
        private Icon greenLight = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.GreenLight.ico"));

        #endregion

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
            this.listView = new System.Windows.Forms.ListView();
            this.columnHeader = new System.Windows.Forms.ColumnHeader();
            this.contextParticipant = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.btnLeaveConference = new System.Windows.Forms.Button();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuSettings = new System.Windows.Forms.MenuItem();
            this.menuSettingsAudioVideo2 = new System.Windows.Forms.MenuItem();
            this.menuSettingsServices = new System.Windows.Forms.MenuItem();
            this.menuSettingsRTDocViewer = new System.Windows.Forms.MenuItem();
            this.menuMyProfile = new System.Windows.Forms.MenuItem();
            this.menuSettingsAppConfig = new System.Windows.Forms.MenuItem();
            this.menuActions = new System.Windows.Forms.MenuItem();
            this.menuActionsPresentation = new System.Windows.Forms.MenuItem();
            this.menuActionsChat = new System.Windows.Forms.MenuItem();
            this.menuActionsWMPlayback = new System.Windows.Forms.MenuItem();
            this.menuActionsScreenScraper = new System.Windows.Forms.MenuItem();
            this.menuActionsSharedBrowser = new System.Windows.Forms.MenuItem();
            this.menuActionsUWClassroomPresenter = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuActionsCapabilities = new System.Windows.Forms.MenuItem();
            this.menuActionsActiveCapabilities = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuActionsRecord = new System.Windows.Forms.MenuItem();
            this.menuActionsPlayback = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuActionsDiagnostics = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuActionsUnicast = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuActionsPersist = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuHelpConferenceXP = new System.Windows.Forms.MenuItem();
            this.menuHelpCommunity = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuHelpAbout = new System.Windows.Forms.MenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.statusBarTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.BackColor = System.Drawing.SystemColors.Window;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader});
            this.listView.ContextMenu = this.contextParticipant;
            this.listView.LargeImageList = this.imageList;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(232, 280);
            this.listView.SmallImageList = this.imageList;
            this.listView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.ItemActivate += new System.EventHandler(this.listView_ItemActivate);
            this.listView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listView_MouseMove);
            // 
            // columnHeader
            // 
            this.columnHeader.Width = 80;
            // 
            // contextParticipant
            // 
            this.contextParticipant.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2});
            this.contextParticipant.Popup += new System.EventHandler(this.contextParticipant_Popup);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.Text = " ";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.Text = " ";
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageList.ImageSize = new System.Drawing.Size(48, 48);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // statusBar
            this.statusBar.Location = new System.Drawing.Point(0, 235);
            this.statusBar.Name = "statusBar";
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(232, 19);
            this.statusBar.TabIndex = 2;
            this.statusBar.Text = "Loading";
            // 
            // btnLeaveConference
            // 
            this.btnLeaveConference.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLeaveConference.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnLeaveConference.Location = new System.Drawing.Point(0, 0);
            this.btnLeaveConference.Name = "btnLeaveConference";
            this.btnLeaveConference.Size = new System.Drawing.Size(232, 24);
            this.btnLeaveConference.TabIndex = 1;
            this.btnLeaveConference.Text = "Leave Venue";
            this.btnLeaveConference.Visible = false;
            this.btnLeaveConference.Click += new System.EventHandler(this.btnLeaveConference_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuSettings,
            this.menuActions,
            this.menuItem5});
            // 
            // menuSettings
            // 
            this.menuSettings.Index = 0;
            this.menuSettings.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuSettingsAudioVideo2,
            this.menuSettingsServices,
            this.menuSettingsRTDocViewer,
            this.menuMyProfile,
            this.menuSettingsAppConfig});
            this.menuSettings.Text = "&Settings";
            // 
            // menuSettingsAudioVideo2
            // 
            this.menuSettingsAudioVideo2.Index = 0;
            this.menuSettingsAudioVideo2.Text = "&Audio/Video...";
            this.menuSettingsAudioVideo2.Click += new System.EventHandler(this.menuSettingsAudioVideo2_Click);
            // 
            // menuSettingsServices
            // 
            this.menuSettingsServices.Index = 1;
            this.menuSettingsServices.Text = "&Services...";
            this.menuSettingsServices.Click += new System.EventHandler(this.menuSettingsServices_Click);
            // 
            // menuSettingsRTDocViewer
            // 
            this.menuSettingsRTDocViewer.Index = 2;
            this.menuSettingsRTDocViewer.Text = "&Presentation Viewer";
            // 
            // menuMyProfile
            // 
            this.menuMyProfile.Index = 3;
            this.menuMyProfile.Text = "&Profile...";
            this.menuMyProfile.Click += new System.EventHandler(this.menuMyProfile_Click);
            // 
            // menuSettingsAppConfig
            // 
            this.menuSettingsAppConfig.Index = 4;
            this.menuSettingsAppConfig.Text = "App &Config...";
            this.menuSettingsAppConfig.Visible = true;
            this.menuSettingsAppConfig.Click += new System.EventHandler(this.menuSettingsAppConfig_Click);
            // 
            // menuActions
            // 
            this.menuActions.Index = 1;
            this.menuActions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuActionsPresentation,
            this.menuActionsChat,
            this.menuActionsWMPlayback,
            this.menuActionsScreenScraper,
            this.menuActionsSharedBrowser,
            this.menuActionsUWClassroomPresenter,
            this.menuItem7,
            this.menuActionsCapabilities,
            this.menuActionsActiveCapabilities,
            this.menuItem4,
            this.menuActionsRecord,
            this.menuActionsPlayback,
            this.menuItem9,
            this.menuActionsDiagnostics,
            this.menuItem6,
            this.menuActionsUnicast,
            this.menuItem8,
            this.menuActionsPersist});
            this.menuActions.Text = "&Actions";
            this.menuActions.Select += new System.EventHandler(this.menuActions_Select);
            // 
            // menuActionsPresentation
            // 
            this.menuActionsPresentation.Enabled = false;
            this.menuActionsPresentation.Index = 0;
            this.menuActionsPresentation.Text = "Start &Presentation...";
            this.menuActionsPresentation.Click += new System.EventHandler(this.menuActionsPresentation_Click);
            // 
            // menuActionsChat
            // 
            this.menuActionsChat.Enabled = false;
            this.menuActionsChat.Index = 1;
            this.menuActionsChat.Text = "Start &Chat...";
            this.menuActionsChat.Click += new System.EventHandler(this.menuActionsChat_Click);
            // 
            // menuActionsWMPlayback
            // 
            this.menuActionsWMPlayback.Enabled = false;
            this.menuActionsWMPlayback.Index = 2;
            this.menuActionsWMPlayback.Text = "Start &Windows Media Playback...";
            this.menuActionsWMPlayback.Click += new System.EventHandler(this.menuActionsWMPlayback_Click);
            // 
            // menuActionsScreenScraper
            // 
            this.menuActionsScreenScraper.Enabled = false;
            this.menuActionsScreenScraper.Index = 3;
            this.menuActionsScreenScraper.Text = "Start &Local Screen Streaming...";
            this.menuActionsScreenScraper.Click += new System.EventHandler(this.menuActionsScreenScraper_Click);
            // 
            // menuActionsSharedBrowser
            // 
            this.menuActionsSharedBrowser.Enabled = false;
            this.menuActionsSharedBrowser.Index = 4;
            this.menuActionsSharedBrowser.Text = "Start &Shared Browser...";
            this.menuActionsSharedBrowser.Click += new System.EventHandler(this.menuActionsSharedBrowser_Click);
            // 
            // menuActionsUWClassroomPresenter
            // 
            this.menuActionsUWClassroomPresenter.Enabled = false;
            this.menuActionsUWClassroomPresenter.Index = 5;
            this.menuActionsUWClassroomPresenter.Text = "Start UW Classroom Presenter 3...";
            this.menuActionsUWClassroomPresenter.Visible = false;
            this.menuActionsUWClassroomPresenter.Click += new System.EventHandler(this.menuActionsUWClassroomPresenter_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 6;
            this.menuItem7.Text = "-";
            // 
            // menuActionsCapabilities
            // 
            this.menuActionsCapabilities.Enabled = false;
            this.menuActionsCapabilities.Index = 7;
            this.menuActionsCapabilities.Text = "Start &Other Capabilities";
            // 
            // menuActionsActiveCapabilities
            // 
            this.menuActionsActiveCapabilities.Enabled = false;
            this.menuActionsActiveCapabilities.Index = 8;
            this.menuActionsActiveCapabilities.Text = "&Active Venue Capabilities";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 9;
            this.menuItem4.Text = "-";
            // 
            // menuActionsRecord
            // 
            this.menuActionsRecord.Enabled = false;
            this.menuActionsRecord.Index = 10;
            this.menuActionsRecord.Text = "&Record This Conference...";
            this.menuActionsRecord.Click += new System.EventHandler(this.menuActionsRecord_Click);
            // 
            // menuActionsPlayback
            // 
            this.menuActionsPlayback.Index = 11;
            this.menuActionsPlayback.Text = "&Play a Previously Recorded Conference...";
            this.menuActionsPlayback.Click += new System.EventHandler(this.menuActionsPlayback_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 12;
            this.menuItem9.Text = "-";
            // 
            // menuActionsDiagnostics
            // 
            this.menuActionsDiagnostics.Enabled = false;
            this.menuActionsDiagnostics.Index = 13;
            this.menuActionsDiagnostics.Text = "View Conference &Diagnostics...";
            this.menuActionsDiagnostics.Click += new System.EventHandler(this.menuActionsDiagnostics_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 14;
            this.menuItem6.Text = "-";
            // 
            // menuActionsUnicast
            // 
            this.menuActionsUnicast.Index = 15;
            this.menuActionsUnicast.Text = "Start a Two-Way Unicast Conference...";
            this.menuActionsUnicast.Click += new System.EventHandler(this.menuActionsUnicast_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 16;
            this.menuItem8.Text = "-";
            // 
            // menuActionsPersist
            // 
            this.menuActionsPersist.Checked = true;
            this.menuActionsPersist.Index = 17;
            this.menuActionsPersist.Text = "Persist Window Positions";
            this.menuActionsPersist.Click += new System.EventHandler(this.menuActionsPersist_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 2;
            this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuHelpConferenceXP,
            this.menuHelpCommunity,
            this.menuItem3,
            this.menuHelpAbout});
            this.menuItem5.Text = "&Help";
            // 
            // menuHelpConferenceXP
            // 
            this.menuHelpConferenceXP.Index = 0;
            this.menuHelpConferenceXP.Text = "&ConferenceXP Help";
            this.menuHelpConferenceXP.Click += new System.EventHandler(this.menuHelpConferenceXP_Click);
            // 
            // menuHelpCommunity
            // 
            this.menuHelpCommunity.Index = 1;
            this.menuHelpCommunity.Text = "Community &Site";
            this.menuHelpCommunity.Click += new System.EventHandler(this.menuHelpCommunity_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.Text = "-";
            // 
            // menuHelpAbout
            // 
            this.menuHelpAbout.Index = 3;
            this.menuHelpAbout.Text = "&About ConferenceXP Client";
            this.menuHelpAbout.Click += new System.EventHandler(this.menuHelpAbout_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            // 
            // statusBarTimer
            // 
            this.statusBarTimer.Interval = 750;
            this.statusBarTimer.Tick += new System.EventHandler(this.statusBarTimer_Tick);
            // 
            // FMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(232, 254);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.btnLeaveConference);
            this.Font = UIFont.FormFont;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Menu = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(160, 240);
            this.Name = "FMain";
            this.Text = "ConferenceXP";
            this.Resize += new System.EventHandler(this.FMain_Resize);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FMain_Closing);
            this.Load += new System.EventHandler(this.FMain_Load);
            this.ResumeLayout(false);

        }


        #endregion

        #region Statics

        const int archiverUnicastPort = 7004; // the default port to do a unicast playback on

        private readonly string helpurlCommunity = Strings.HelpURLCommunity;
        private readonly string helpurlConferenceXP = Strings.HelpURLConferenceXP;
        private readonly string helpurlStudentEdition = Strings.HelpURLStudentEdition;
        internal static readonly string helpurlConnectivity = Strings.HelpURLConnectivity;
        internal static readonly string helpurlServices = Strings.HelpURLServices;
        internal static readonly string helpurlNotification = Strings.HelpURLNotification;

        private static ArgumentParser arguments = null;
        private static bool bStudentMode = false;
        private static bool recordNotify = true;

        private static bool enableAudioFec = true;
        private static bool enableVideoFec = true;

        internal static bool EnableAudioFec {
            get { return enableAudioFec; }
            set { 
                enableAudioFec = value;
                cxpclientRegKey.SetValue("EnableAudioFec", enableAudioFec);
            }
        }

        internal static bool EnableVideoFec {
            get { return enableVideoFec; }
            set { 
                enableVideoFec = value;
                cxpclientRegKey.SetValue("EnableVideoFec", enableVideoFec);
            }
        }

        private static bool autoPlayRemoteAudio = true;
        private static bool autoPlayRemoteVideo = true;

        private static readonly Image lockImage =      Image.FromStream(
           System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.lock.gif"));

        private static readonly Image encryptedLockImage = Image.FromStream(
           System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.redlock.gif"));

        internal static bool AutoPlayRemoteAudio
        {
            get { return autoPlayRemoteAudio; }
            set
            {
                autoPlayRemoteAudio = value;
                AVReg.WriteValue(AVReg.RootKey, AVReg.AutoPlayRemoteAudio, value);
            }
        }

        internal static bool AutoPlayRemoteVideo
        {
            get { return autoPlayRemoteVideo; }
            set
            {
                autoPlayRemoteVideo = value;
                AVReg.WriteValue(AVReg.RootKey, AVReg.AutoPlayRemoteVideo, value);
            }
        }


        const string venueServiceSuffix = "/venueservice.asmx";

        static RegistryKey baseRegKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft Research\ConferenceXP\Client\" +
            System.Reflection.Assembly.GetExecutingAssembly().CodeBase);

        // Specify all registry keys in one place
        static RegistryKey cxpclientRegKey = baseRegKey.CreateSubKey("CXPClient");
        static RegistryKey rtdocsRegKey = baseRegKey.CreateSubKey("CapabilityViewers");
        static RegistryKey venuesRegKey = baseRegKey.CreateSubKey("VenueService");
        static RegistryKey reflectorsRegKey = baseRegKey.CreateSubKey("ReflectorService");
        static RegistryKey archiversRegKey = baseRegKey.CreateSubKey("ArchiveService");
        static RegistryKey diagnosticsRegKey = baseRegKey.CreateSubKey("DiagnosticsService");

        public static IPAddress remoteIP = null;

        [STAThread]
        static void Main(string[] args)
        {
            // Override the system UICulture
            string cultureOverride = null;
            if ((cultureOverride = ConfigurationManager.AppSettings["MSR.LST.ConferenceXP.UICulture"]) != null) {
                try {
                    CultureInfo ci = new CultureInfo(cultureOverride);
                    Thread.CurrentThread.CurrentUICulture = ci;
                }
                catch {}
            }
            
            // Name the UI thread, so it is more easily identified during debugging
            Thread.CurrentThread.Name = "CXPClient UI";

            // Parse the command line arguments
            arguments = new ArgumentParser(args); // Must keep this around so Parameters doesn't go out of scope
            InvokeSettingsArguments(arguments.Parameters);
            UnhandledExceptionHandler.Register();

            Application.EnableVisualStyles();
            Application.Run(new FMain());
        }

        private static void InvokeSettingsArguments(StringDictionary parameters)
        {
            #region Set LocalParticipant Properties
            // Note: this must be done before the static Conference.ctor is called, hence it is first
            if (parameters.ContainsKey("email") || parameters.ContainsKey("e"))
            {
                string email = parameters["email"];
                if (parameters.ContainsKey("e"))
                {
                    email = parameters["e"];
                }

                // Cannot cleanly do this because the participant properties are read only and the only ctor takes a VenueParticipant...
                // Need a clean way to change these properties on LocalParticipant...
                //Participant p = Conference.LocalParticipant;
                //p.Email = parameters["email"];
                //Conference.LocalParticipant = p;
                MSR.LST.ConferenceXP.Identity.Identifier = email;
            }
            if (parameters.ContainsKey("name") || parameters.ContainsKey("n"))
            {
                string name = parameters["name"];
                if (parameters.ContainsKey("n"))
                {
                    name = parameters["n"];
                }
                //Participant p = Conference.LocalParticipant;
                //p.Name = parameters["name"];
                //Conference.LocalParticipant = p;
                throw new NotImplementedException();
            }
            #endregion
            #region Set Venue Server
            if (parameters.ContainsKey("venueservice") || parameters.ContainsKey("vs"))
            {
                string venueService = parameters["venueservice"];
                if (parameters.ContainsKey("vs"))
                {
                    venueService = parameters["vs"];
                }

                venueService = venueService.ToLower(CultureInfo.CurrentCulture);

                if (venueService == "none")
                {
                    VenueServiceBaseUrl = null;
                }
                else
                {
                    VenueServiceBaseUrl = venueService;
                    venuesRegKey.SetValue(VenueServiceBaseUrl, "false");
                }
            }

            #endregion
            #region Set Behavior Properties

            if (parameters.ContainsKey("autoplaylocal"))
            {
                Conference.AutoPlayLocal = true;
            }
            if (parameters.ContainsKey("autoplayremote"))
            {
                Conference.AutoPlayRemote = true;
            }

            #endregion
            #region Disable Recording Notification
            if (parameters.ContainsKey("recordnotify") || parameters.ContainsKey("rn"))
            {
                string notify = parameters["recordnotify"];
                if (notify == null)
                {
                    notify = parameters["rn"];
                }

                recordNotify = bool.Parse(notify);
            }
            #endregion
        }


        #endregion Statics

        #region Members

        // Used to reference from a Device MenuItem to the Device, due to the lack of MenuItem.Tag
        public Hashtable deviceMenuItems = new Hashtable(3);
        private Hashtable contextMenuItems = new Hashtable(3);
        private Hashtable menuItemTags = new Hashtable();
        //private frmStopLight stoplight = null;
        private string archiveServiceDefault = null;
        private IArchiveServer archiver;
        private ArchiverState archiverState = ArchiverState.Stopped;
        private bool twoWayUnicast = false;

        private readonly HeartbeatMonitor monitor;

        #endregion Members

        #region Handle command line startup
        private void InvokeActionArguments(StringDictionary parameters)
        {
            #region Show Help
            if (parameters.ContainsKey("help"))
            {
                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture,
                    Strings.CmdLineParametersInstructions, 
                    "  -help                           ",
                    "  -venue VenueName                ",
                    "  -v VenueName                    ",
                    "  -capability CapabilityName      ",
                    "  -c CapabilityName               ",
                    "  -venueservice http://server/web ",
                    "  -vs http://server/web           ",
                    "  -venueservice none              ",
                    "  -vs none                        ",
                    "  -ip x.x.x.x -port xxxx          ",
                    "  -recordnotify false             ",
                    "  -rn false                       ",
                    "  -email name@domain.org          ",
                    "  -e name@domain.org              ",
                    "  -password Password              ",
                    "  -p Password                     "), 
                    Strings.CmdLineParametersTitle, MessageBoxButtons.OK, MessageBoxIcon.None, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                return;
            }
            #endregion
            #region Join Venue
            if (parameters.ContainsKey("venue") || parameters.ContainsKey("v"))
            {
                string venueName = parameters["venue"];
                if (parameters.ContainsKey("v"))
                {
                    venueName = parameters["v"];
                }

                if (!Conference.VenueServiceWrapper.Venues.ContainsKey(venueName))
                {
                    string dialogText = Strings.AvailableVenues + "\n\n";
                    foreach (Venue v in Conference.VenueServiceWrapper.Venues)
                    {
                        dialogText += "  " + v.Name + "\n";
                    }
                    RtlAwareMessageBox.Show(this, dialogText, string.Format(CultureInfo.CurrentCulture,
                        Strings.VenueNotFound, venueName), MessageBoxButtons.OK, MessageBoxIcon.Stop,
                        MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

                    Application.Exit();
                }

                if (Conference.VenueServiceWrapper.Venues[venueName].PWStatus != PasswordStatus.NO_PASSWORD) {
                    if (parameters.ContainsKey("password") || parameters.ContainsKey("p")) {
                        string password = parameters["password"];
                        if (parameters.ContainsKey("p")) {
                            password = parameters["p"];
                        }
                        Venue validatedVenue = null;
                        try {
                            validatedVenue = Conference.VenueServiceWrapper.
                                ResolvePassword(Conference.VenueServiceWrapper.Venues[venueName].Identifier, password);
                        }
                        catch { }
                        if (validatedVenue != null) {
                            if (Conference.VenueServiceWrapper.Venues[venueName].PWStatus != PasswordStatus.WEAK_PASSWORD) {
                                Conference.JoinVenue(validatedVenue);
                                AutoSendAV();
                            }
                            else if (Conference.VenueServiceWrapper.Venues[venueName].PWStatus != PasswordStatus.STRONG_PASSWORD) {
                                Conference.JoinVenue(validatedVenue, password);
                                AutoSendAV();
                            }
                        }
                        else {
                            //The password supplied is invalid
                            RtlAwareMessageBox.Show(this, Strings.InvalidPassword, string.Empty, MessageBoxButtons.OK,
                               MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                        }
                    }
                    else {
                        //No password was provided
                        RtlAwareMessageBox.Show(this, Strings.PasswordRequired, string.Empty, MessageBoxButtons.OK,
                           MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);                  
                    }
                }
                else {
                    //No password required
                    Conference.JoinVenue(Conference.VenueServiceWrapper.Venues[venueName]);
                    AutoSendAV();
                }
            }
            if (parameters.ContainsKey("ip"))
            {
                if (parameters.ContainsKey("port"))
                {
                    VenueData vd = new VenueData("Custom Venue", new IPEndPoint(IPAddress.Parse(parameters["ip"]),
                        int.Parse(parameters["port"], CultureInfo.InvariantCulture)), 127, VenueType.Custom, 
                        null, null, null);
                    Venue v = Conference.VenueServiceWrapper.AddCustomVenue(vd);
                    Conference.JoinVenue(v);
                    AutoSendAV();
                }
                else
                {
                    RtlAwareMessageBox.Show(this, Strings.IPParameterError, string.Empty, MessageBoxButtons.OK,
                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                }
            }
            #endregion
            #region Play Capability
            if (parameters.ContainsKey("capability") || parameters.ContainsKey("c"))
            {
                if (Conference.ActiveVenue == null)
                {
                    RtlAwareMessageBox.Show(this, Strings.CapabilityError, string.Empty, MessageBoxButtons.OK,
                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                }

                string capabilityName = parameters["capability"];
                if (parameters.ContainsKey("c"))
                {
                    capabilityName = parameters["c"];
                }

                ArrayList alOtherCapabilitySenders = new ArrayList(Conference.OtherCapabilitySenders);
                if (alOtherCapabilitySenders.Contains(capabilityName))
                {
                    ICapabilitySender cs = Conference.CreateCapabilitySender(capabilityName);
                }
                else
                {
                    string dialogText = Strings.AvailableOtherCapabilitySenders + "\n\n";
                    foreach (string s in Conference.OtherCapabilitySenders)
                    {
                        dialogText += "  " + s + "\n";
                    }
                    dialogText += "\n\n" + Strings.CaseSensitiveReminder;
                    RtlAwareMessageBox.Show(this, dialogText, string.Format(CultureInfo.CurrentCulture,
                        Strings.CapabilitySenderNotFound, capabilityName), MessageBoxButtons.OK, MessageBoxIcon.Stop,
                        MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                    Application.Exit();
                }
            }
            #endregion
        }
        #endregion

        #region CTor / Dispose
        public FMain()
        {
            InitializeComponent();

            diagnosticPanel = new StatusBarPanel();
    
            diagnosticPanel.Icon = redLight;
            diagnosticPanel.AutoSize = StatusBarPanelAutoSize.Contents;
            diagnosticPanel.ToolTipText = Strings.MulticastNotDetected;

            messagePanel = new StatusBarPanel();
            messagePanel.AutoSize = StatusBarPanelAutoSize.Spring;

            statusBar.Panels.Add(messagePanel);
            statusBar.Panels.Add(diagnosticPanel);

            monitor = new HeartbeatMonitor();
            monitor.ProgressChanged += new ProgressChangedEventHandler(monitor_ProgressChanged);

            archiver = null;
        }

        void monitor_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            bool isConnected = (e.ProgressPercentage == 100);
            HeartbeatMonitor monitor = (HeartbeatMonitor)sender;

            if (isConnected)
            {
                diagnosticPanel.Icon = greenLight;
                if (monitor.ReflectorEnabled) {
                    diagnosticPanel.ToolTipText = Strings.ReflectorDetected;
                }
                else {
                    diagnosticPanel.ToolTipText = Strings.MulticastDetected;
                }
            }
            else
            {
                diagnosticPanel.Icon = redLight;
                if (monitor.ReflectorEnabled) {
                    diagnosticPanel.ToolTipText = Strings.ReflectorNotDetected;
                }
                else {
                    diagnosticPanel.ToolTipText = Strings.MulticastNotDetected;
                }
            }
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

        #region UI Tasks

        private void FMain_Load(object sender, System.EventArgs e)
        {
            this.listView.Font = UIFont.StringFont;
            this.statusBar.Font = UIFont.StatusFont;
            this.btnLeaveConference.Font = UIFont.StringFont;

            this.statusBar.Text = Strings.Loading;
            this.btnLeaveConference.Text = Strings.LeaveVenue;
            this.menuSettings.Text = Strings.SettingsHotkey;
            this.menuSettingsAudioVideo2.Text = Strings.AudioVideoHotkey;
            this.menuSettingsServices.Text = Strings.ServicesHotkey;
            this.menuSettingsRTDocViewer.Text = Strings.PresentationViewerHotkey;
            //this.menuSettingsNetworkDiagnostics.Text = Strings.StartConnectivityHotkey;
            this.menuMyProfile.Text = Strings.ProfileHotkey;
            this.menuSettingsAppConfig.Text = Strings.AppConfigHotkey;
            this.menuActions.Text = Strings.ActionsHotkey;
            this.menuActionsPresentation.Text = Strings.StartPresentationHotkey;
            this.menuActionsChat.Text = Strings.StartChatHotkey;
            this.menuActionsWMPlayback.Text = Strings.StartWindowsMedia;
            this.menuActionsScreenScraper.Text = Strings.StartScreenStreaming;
            this.menuActionsSharedBrowser.Text = Strings.StartSharedBrowser;
            this.menuActionsUWClassroomPresenter.Text = Strings.StartUWPresenter;
            this.menuActionsCapabilities.Text = Strings.StartOtherCapabilitiesHotkey;
            this.menuActionsActiveCapabilities.Text = Strings.ActiveVenueCapabilitiesHotkey;
            this.menuActionsRecord.Text = Strings.RecordConferenceHotkey;
            this.menuActionsPlayback.Text = Strings.PlayRecordedConferenceHotkey;
            this.menuActionsDiagnostics.Text = Strings.ViewConferenceDiagnostics;
            this.menuActionsUnicast.Text = Strings.StartUnicastConference;
            this.menuActionsPersist.Text = Strings.PersistWindowPositions;
            this.menuItem5.Text = Strings.HelpHotkey;
            this.menuHelpConferenceXP.Text = Strings.ConferenceXPHelpHotkey;
            this.menuHelpCommunity.Text = Strings.CommunitySiteHotkey;
            this.menuHelpAbout.Text = Strings.AboutConferenceXPClientHotkey;
            this.Text = Strings.ConferenceXP;

            // display Recording Notification
            if (!System.Diagnostics.Debugger.IsAttached && recordNotify)
            {
                frmNotification notification = new frmNotification();
                notification.ShowDialog();
            }

            Cursor = Cursors.AppStarting;

            // Hook up this form to ConferenceAPI so it knows where to post back Events so they occur on the form thread.
            DisplayStatusInProgress(Strings.LoadingConferenceAPI);

            Conference.CallingForm = this;

            // Set the default Conferencing behavior
            Conference.AutoPlayLocal = true;
            Conference.AutoPlayRemote = true;
            Conference.AutoPosition = Conference.AutoPositionMode.FourWay;

            // Hook the Conference events to this form's events
            Conference.ParticipantAdded += new Conference.ParticipantAddedEventHandler(OnParticipantAdded);
            Conference.ParticipantRemoved += new Conference.ParticipantRemovedEventHandler(OnParticipantRemoved);
            Conference.CapabilityAdded += new CapabilityAddedEventHandler(OnCapabilityAdded);
            Conference.CapabilityRemoved += new CapabilityRemovedEventHandler(OnCapabilityRemoved);
            Conference.DuplicateIdentityDetected += new Conference.DuplicateIdentityDetectedEventHandler(OnDuplicateIdentityDetected);
            Conference.DiagnosticUpdate += new Conference.DiagnosticUpdateEventHandler(OnDiagnosticUpdate);

            // Make use of the internal socket exceptions to determine network status
            RtpEvents.HiddenSocketException += new RtpEvents.HiddenSocketExceptionEventHandler(HiddenSockExHandler);

            #region Load Settings from Registry
            try
            {
                // Load the form location settings from the registry
                object val;

                if ((val = cxpclientRegKey.GetValue("Top")) != null)
                {
                    this.Top = Convert.ToInt32(val, CultureInfo.InvariantCulture);
                }
                else
                {
                    this.Top = SystemInformation.WorkingArea.Top;
                }

                if ((val = cxpclientRegKey.GetValue("Left")) != null)
                {
                    this.Left = Convert.ToInt32(val, CultureInfo.InvariantCulture);
                }
                else
                {
                    this.Left = SystemInformation.WorkingArea.Right - this.Width;
                }

                if ((val = cxpclientRegKey.GetValue("Width")) != null)
                {
                    this.Width = Convert.ToInt32(val, CultureInfo.InvariantCulture);
                }

                if ((val = cxpclientRegKey.GetValue("Height")) != null)
                {
                    this.Height = Convert.ToInt32(val, CultureInfo.InvariantCulture);
                }

                if ((val = cxpclientRegKey.GetValue("AutoPlayRemote")) != null)
                {
                    Conference.AutoPlayRemote = Convert.ToBoolean(val, CultureInfo.InvariantCulture);
                }

                if ((val = cxpclientRegKey.GetValue("AutoPlayLocal")) != null)
                {
                    Conference.AutoPlayLocal = Convert.ToBoolean(val, CultureInfo.InvariantCulture);
                }

                if ((val = AVReg.ReadValue(AVReg.RootKey, AVReg.AutoPlayRemoteAudio)) != null)
                {
                    autoPlayRemoteAudio = bool.Parse((string)val);
                }

                if ((val = AVReg.ReadValue(AVReg.RootKey, AVReg.AutoPlayRemoteVideo)) != null)
                {
                    autoPlayRemoteVideo = bool.Parse((string)val);
                }

                if ((val = cxpclientRegKey.GetValue("AutoPosition")) != null)
                {
                    Conference.AutoPosition = (Conference.AutoPositionMode)Enum.Parse(Conference.AutoPosition.GetType(), val.ToString());
                }

                this.menuActionsPersist.Checked = Conference.PersistWindowPositions = false;
                if ((val = cxpclientRegKey.GetValue("PersistWindowPositions")) != null) {
                    this.menuActionsPersist.Checked = Conference.PersistWindowPositions = Convert.ToBoolean(val, CultureInfo.InvariantCulture);
                }

                if ((val = cxpclientRegKey.GetValue("EnableAudioFec")) != null) {
                    enableAudioFec = Convert.ToBoolean(val, CultureInfo.InvariantCulture);
                }

                if ((val = cxpclientRegKey.GetValue("EnableVideoFec")) != null) {
                    enableVideoFec = Convert.ToBoolean(val, CultureInfo.InvariantCulture);
                }


            }
            catch
            {
                // Set the default location of the form to the top right corner of the working area
                this.Top = SystemInformation.WorkingArea.Top;
                this.Left = SystemInformation.WorkingArea.Right - this.Width;
                // Width and Height defaults are set by the development environment
            }
            #endregion

            #region Set RTDocs Viewers, Services, and Check for Student Mode

            DisplayRTDocumentViewers();

            #region Pre-populate Reflector and Archive Services on first run
            /// If there is no existing registry configuration for services, we use whatever is in app.config
            /// to pre-populate it.  After the initial run, or until the registry configuration is removed, 
            /// the app.config is ignored.  
            
            // Pre-populate Archive service if there are no entries in the registry and one is specified in app.config
            string setting = null;
            if (archiversRegKey.ValueCount == 0)
            {
                string asKey = "MSR.LST.ConferenceXP.ArchiveService";
                if ((setting = ConfigurationManager.AppSettings[asKey]) != null)
                {
                    archiversRegKey.SetValue(setting, "False");
                }

                // The next entry in the app.config starts with the postfix 2
                // i.e. - MSR.LST.ConferenceXP.ArchiveService2
                int postfix = 2;

                while ((setting = ConfigurationManager.AppSettings[asKey + postfix]) != null)
                {
                    archiversRegKey.SetValue(setting, "False");
                    postfix++; // Move to the next entry
                }
            }

            // Pre-populate Reflector service if there are no entries in the 
            // registry and one (or more) is specified in app.config
            // Disabled by default
            if (reflectorsRegKey.ValueCount == 0)
            {
                string rsKey = "MSR.LST.ConferenceXP.ReflectorService";

                if ((setting = ConfigurationManager.AppSettings[rsKey]) != null)
                {
                    reflectorsRegKey.SetValue(setting, "False");
                }

                // The next entry in the app.config starts with the postfix 2
                // i.e. - MSR.LST.ConferenceXP.ReflectorService2
                int postfix = 2;

                while ((setting = ConfigurationManager.AppSettings[rsKey + postfix]) != null)
                {
                    reflectorsRegKey.SetValue(setting, "False");
                    postfix++; // Move to the next entry
                }
            }

            #endregion Pre-populate Services

            // Set the maximum extent of the video bitrate slider
            setting = ConfigurationManager.AppSettings["MSR.LST.ConferenceXP.MaximumVideoBitRate"];
            int vbr;
            if (int.TryParse(setting, out vbr)) {
                frmVideoSettings.MaximumVideoBitRate = vbr;
            }

            // Check if student mode has been configured, and disable appropriate menus if needed           
            setting = ConfigurationManager.AppSettings["MSR.LST.ConferenceXP.StudentMode"];
            bStudentMode = (setting != null && bool.Parse(setting));

            if (bStudentMode)
            {
                // Disable menu settings
                menuActions.Visible = false;
                menuSettingsAudioVideo2.Visible = false;
                menuSettingsServices.Visible = false;
                menuMyProfile.Visible = false;
            }
            else
            {
                DisplayStatusInProgress(Strings.LoadingVenues);
                InitVenueService();
                GetArchiveService();
                SetReflectorService();
                SetDiagnosticService();
                DisplayOtherCapabilitySenders();
                SetDefaultDevices();
            }
            #endregion

            InvokeActionArguments(arguments.Parameters);

            // load manual venue mappings from venues.txt
            LoadManualVenues();

            if (bStudentMode && Conference.ActiveVenue == null)
            {
                // Create a custom venue and enter it
                JoinVenue(AddLocalVenue(), false);
            }

            // Detect whether or not the UW Classroom Presenter Capability has been installed, and if so, display it on the Actions menu
            ArrayList alCapabilitySenders = new ArrayList(Conference.OtherCapabilitySenders);
            if (alCapabilitySenders.Contains("Classroom Presenter"))
            {
                menuActionsUWClassroomPresenter.Visible = true;
            }

            // A venue may already be entered by command line parameters
            if (Conference.ActiveVenue == null)
            {
                DisplayVenues();
            }
            else
            {
                InVenueUIState();
            }

            Cursor = Cursors.Default;
        }


        /// <summary>
        ///  Load a set of venue mappings from a "venues.txt" file.
        /// </summary>
        private void LoadManualVenues()
        {
            try
            {
                using (StreamReader sr = new StreamReader("venues.txt"))
                {
                    String line;
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("#"))
                            continue; // comment;

                        String[] toks = line.Split(new char[] { ':' });
                        if (toks.Length != 2)
                            continue;

                        String venueStr = toks[0].Trim();
                        if (venueStr.Length == 0)
                            continue;

                        String ipstr = toks[1].Trim();
                        if (ipstr.Length == 0)
                            continue;

                        VenueData vd = new VenueData(venueStr,new IPEndPoint(IPAddress.Parse(ipstr), 5004), 255, 
                            VenueType.Custom, null);

                        Conference.VenueServiceWrapper.AddCustomVenue(vd);
                        
                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        private void FMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                // Check to see if we are minimized, and if so, restore before saving settings
                // Otherwise when relaunching, the form will "appear" off screen
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Normal;
                }

                // Save the Form position information to the registry
                cxpclientRegKey.SetValue("Top", this.Top);
                cxpclientRegKey.SetValue("Left", this.Left);
                cxpclientRegKey.SetValue("Width", this.Width);
                cxpclientRegKey.SetValue("Height", this.Height);
                cxpclientRegKey.SetValue("PersistWindowPositions", this.menuActionsPersist.Checked);

                cxpclientRegKey.Flush();
            }
            catch { }

            if (this.btnLeaveConference.Visible)
            {
                this.btnLeaveConference.PerformClick();
            }

            try
            {
                //if (stoplight != null)
                //{
                //    stoplight.Dispose();
                //}
            }
            catch { }
        }

        private void FMain_Resize(object sender, System.EventArgs e)
        {
            if (Conference.ActiveVenue == null)
            {
                listView.Height = this.ClientSize.Height - this.statusBar.Height;
                listView.Top = 0;
            }
            else
            {
                listView.Height = this.ClientSize.Height - this.btnLeaveConference.Height - this.statusBar.Height;
                listView.Top = this.btnLeaveConference.Bottom;
            }
        }

        private String PromptForPassword()
        {
            frmPassword passwordForm = new frmPassword();
            DialogResult dr = passwordForm.ShowDialog();

            if (dr == DialogResult.OK)
            {
                String password = passwordForm.Password;
                if (password != null)
                    return password.Trim();
            }

            return null;
        }


        private bool CheckForNullPassword(String ps)
        {
            if (ps == null || ps.Length == 0)
            {
                RtlAwareMessageBox.Show(this, Strings.VenueRequiresPassword,
                               Strings.PasswordError, MessageBoxButtons.OK, MessageBoxIcon.Error,
                               MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

                btnLeaveConference_Click(this, null);
                return false;
            }
            else return true;
        }
            

        private void JoinVenue(Venue venueToJoin, bool sendAV)
        {
            listView.Items.Clear();
            imageList.Images.Clear();
            toolTip.RemoveAll();

            try {
                Cursor.Current = Cursors.WaitCursor;

                // if the venue requires a password, prompt the user with a modal
                // dialog box.  Contact the venue service to validate the password.
                // This returns the "true" IP address of the venue

                switch (venueToJoin.PWStatus) {
                    case PasswordStatus.WEAK_PASSWORD: {

                            String password = PromptForPassword();
                            if (!CheckForNullPassword(password))
                                return;

                            // contact the venue service to validate this password
                            // this throws an exception (caught below) if the password is not
                            // valid...
                            Venue validatedVenue = Conference.VenueServiceWrapper.
                                ResolvePassword(venueToJoin.Identifier, password);
                            Conference.JoinVenue(validatedVenue);
                            break;
                        }
                    case PasswordStatus.STRONG_PASSWORD: {
                            String password = PromptForPassword();
                            if (!CheckForNullPassword(password))
                                return;

                            Venue validatedVenue = Conference.VenueServiceWrapper.
                                ResolvePassword(venueToJoin.Identifier, password);

                            // use the password as an encryption key in this step...
                            Conference.JoinVenue(validatedVenue, password);
                            break;
                        }
                    case PasswordStatus.NO_PASSWORD: {
                            // no password for the venue...
                            Conference.JoinVenue(venueToJoin);
                            break;
                        }
                }

                if (sendAV) // in unicast archive playback, we don't want to send AV
                    AutoSendAV();

                InVenueUIState();

            }

            catch (InvalidPasswordException) {
                RtlAwareMessageBox.Show(this, Strings.InvalidPassword, Strings.PasswordError, MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

                btnLeaveConference_Click(this, null);
            }
            catch (SocketException se) {
                String message = se.ToString();

                //The socket in use is a common case, so we treat it with a more helpful error message.
                if (se.SocketErrorCode == SocketError.AddressAlreadyInUse) {
                    message = Strings.FriendlySocketInUseErrorMessage;
                }
          
                RtlAwareMessageBox.Show(this, message, Strings.JoiningVenueError, MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

                // Button is not visible, so we can't call the .PerformClick() method, but we need to
                // leave the conference anyhow to clean up properly.
                btnLeaveConference_Click(this, null);
            }
            catch (Exception ex) {
                RtlAwareMessageBox.Show(this, ex.ToString(), Strings.JoiningVenueError, MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

                // Button is not visible, so we can't call the .PerformClick() method, but we need to
                // leave the conference anyhow to clean up properly.
                btnLeaveConference_Click(this, null);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void InVenueUIState()
        {
            this.Text = string.Format(CultureInfo.CurrentCulture, Strings.FMainActiveVenue, 
                Conference.ActiveVenue.Name);

            this.FMain_Resize(this, EventArgs.Empty);

            menuMyProfile.Enabled = false;

            SetArchiverMenuStatus();
            SetDiagnosticsMenuStatus();

            menuActionsPresentation.Enabled = true;
            menuSettingsServices.Enabled = false;
            menuSettingsAudioVideo2.Enabled = false;
            menuActionsChat.Enabled = true;
            menuActionsWMPlayback.Enabled = true;
            menuActionsScreenScraper.Enabled = true;
            menuActionsSharedBrowser.Enabled = true;
            menuActionsUWClassroomPresenter.Enabled = true;

            if (menuActionsCapabilities.MenuItems.Count > 0)
            {
                menuActionsCapabilities.Enabled = true;
            }
            else
            {
                menuActionsCapabilities.Enabled = false;
            }

            menuActionsActiveCapabilities.Enabled = false;
            menuActionsUnicast.Enabled = false;

            // Only allow one to leave the venue if we are not in student mode
            if (!bStudentMode)
            {
                btnLeaveConference.Visible = true;
            }
            DisplayParticipantCount();
        }


        /// <summary>
        /// This should only be executed during the first run.
        /// To make sure it is the first run, we look for reg keys of selected
        /// devices and if they aren't there we create them for the first 
        /// device of each type
        /// </summary>
        private void SetDefaultDevices()
        {
            string[] regSelectedMics = AVReg.ValueNames(AVReg.SelectedMicrophone);
            string[] regSelectedCameras = AVReg.ValueNames(AVReg.SelectedCameras);

            // Null means the key doesn't exist
            if (regSelectedMics == null && regSelectedCameras == null)
            {
                FilterInfo[] mics = AudioSource.Sources();
                FilterInfo[] cameras = VideoSource.Sources();

                if (mics.Length > 0 && cameras.Length > 0)
                {
                    // Select the first device of each type and link them
                    AVReg.WriteValue(AVReg.SelectedMicrophone, mics[0].Moniker, mics[0].Name);
                    AVReg.WriteValue(AVReg.SelectedCameras, cameras[0].Moniker, cameras[0].Name);
                    AVReg.WriteValue(AVReg.LinkedCamera, cameras[0].Moniker, cameras[0].Name);
                }
            }
        }

        private void AutoSendAV()
        {
            // Determine if the form is shared
            string[] linkedCamera = AVReg.ValueNames(AVReg.LinkedCamera);
            if (linkedCamera != null)
            {
                Debug.Assert(linkedCamera.Length <= 1);
            }

            // Create the audio capability
            FilterInfo[] mics = AudioCapability.SelectedMicrophones();
            Debug.Assert(mics.Length <= 1);  // For now we only support 1

            AudioCapability ac = null;
            foreach (FilterInfo fi in mics)
            {
                ac = new AudioCapability(fi);
                ac.FecEnabled = enableAudioFec;
            }

            // Create the video capabilities and start sending their data
            foreach (FilterInfo fi in VideoCapability.SelectedCameras())
            {
                VideoCapability vc = new VideoCapability(fi);
                vc.FecEnabled = enableVideoFec;

                // Set the shared form ID
                if (ac != null && linkedCamera != null && linkedCamera.Length > 0)
                {
                    if (fi.Moniker == linkedCamera[0])
                    {
                        Guid sharedFormID = Guid.NewGuid();
                        ac.SharedFormID = sharedFormID;
                        vc.SharedFormID = sharedFormID;
                    }
                }

                try
                {
                    vc.ActivateCamera();
                    vc.Send();
                }
                catch (Exception)
                {
                    vc.Dispose();

                    RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture,
                        Strings.SendVideoErrorText, vc.Name), Strings.SendVideoErrorTitle, MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                }
            }

            // Start sending the audio data
            try
            {
                if (ac != null)
                {
                    ac.ActivateMicrophone();
                    ac.Send();
                }
            }
            catch (Exception)
            {
                ac.Dispose();

                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, Strings.SendAudioErrorText,
                    ac.Name), Strings.SendAudioErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
        }


        /// <summary>
        /// Gets and sets the venue service URL from Conference, adding and removing the venueServiceSuffix as necessary.
        /// </summary>
        static string VenueServiceBaseUrl
        {
            get
            {
                string url = Conference.VenueServiceWrapper.VenueServiceUrl;
                if (url == null)
                    return null;
                else
                    return url.Substring(0, url.Length - venueServiceSuffix.Length);
            }
            set
            {
                try
                {
                    if (value != null)
                        Conference.SetVenueServiceUrl(value + venueServiceSuffix);
                    else
                        Conference.SetVenueServiceUrl(null);
                }
                catch (UriFormatException)
                {
                    RtlAwareMessageBox.Show(null, string.Format(CultureInfo.CurrentCulture, Strings.InvalidURIText,
                        value), Strings.InvalidURITitle, MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

                    // The address provided was improperly formatted, get back to a clean state
                    VenueServiceBaseUrl = null;
                }

                // Always show a local venue
                AddLocalVenue();
            }
        }


        private void InitVenueService()
        {
            // If we set a venue service via the command-line, we don't need to do this
            if (arguments.Parameters.ContainsKey("venueservice") || arguments.Parameters.ContainsKey("vs"))
                return;

            // Check to see if any new Venue Services were added to the app.config
            // If so, the first new one will be the one returned (app.config wins)
            string vs = AddVenueServicesFromAppConfig();

            if (vs == null) // Nothing new in app.config
            {
                // Check registry
                string[] names = venuesRegKey.GetValueNames();
                foreach (string key in names)
                {
                    if (bool.Parse((string)venuesRegKey.GetValue(key)))
                    {
                        vs = key;
                        break;
                    }
                }

                Debug.Assert(vs != null, "Why isn't there a venue service selected?");
            }

            VenueServiceBaseUrl = vs; // retries the venue service, and creates a local venue if necessary
        }

        /// <summary>
        /// Adds a new venue service from the app.config to the list of VSs in the registry
        /// If there is one or more new VSs in the app.config, we will activate 
        /// and return the first one.
        /// </summary>
        private static string AddVenueServicesFromAppConfig()
        {
            string vsKey = "MSR.LST.ConferenceXP.VenueService";
            string newVS = null;

            // Check to see if the default venue service has changed
            string setting;
            if ((setting = ConfigurationManager.AppSettings[vsKey]) != null)
            {
                ProcessVenueService(setting, ref newVS);
            }

            // The next entry in the app.config starts with the postfix 2
            // i.e. - MSR.LST.ConferenceXP.VenueService2
            int postfix = 2;

            while ((setting = ConfigurationManager.AppSettings[vsKey + postfix]) != null)
            {
                ProcessVenueService(setting, ref newVS);

                postfix++; // Move to the next entry
            }

            return newVS;
        }

        private static void ProcessVenueService(string setting, ref string newVS)
        {
            // Trim off suffix, if it has one
            if (setting.EndsWith(venueServiceSuffix))
            {
                setting = setting.Substring(0, setting.Length - venueServiceSuffix.Length);
            }

            // If it's not already in the registry...
            if (venuesRegKey.GetValue(setting) == null)
            {
                if (newVS == null)
                {
                    // This is a new entry, clear old entries
                    string[] names = venuesRegKey.GetValueNames();
                    foreach (string key in names)
                    {
                        venuesRegKey.SetValue(key, false);
                    }

                    // Set new value in registry and return it in newVS
                    venuesRegKey.SetValue(setting, true);
                    newVS = setting;
                }
                else
                {
                    venuesRegKey.SetValue(setting, false);
                }
            }
        }

        private void DisplayVenues()
        {
            this.FMain_Resize(this, EventArgs.Empty);

            listView.Items.Clear();
            imageList.Images.Clear();
            toolTip.RemoveAll();
            int cnt = 0;

            // This set of code may be causing a very long delay in showing the UI...
            Cursor prevCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            // Check that the version is good
            if (!Conference.VenueServiceWrapper.VersionIsCompatible)
            {
                if (Conference.VenueServiceWrapper.MinimumVersion != null) {
                    //Client is too old for this VS
                    RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture,
                        Strings.VenueServiceVersionErrorText, Conference.VenueServiceWrapper.MinimumVersion),
                        Strings.VersionIncompatible, MessageBoxButtons.OK, MessageBoxIcon.Stop,
                        MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                }
                else if (Conference.VenueServiceWrapper.MaximumVersion != null) { 
                    //VS is too old for the client
                    RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture,
                        Strings.VenueServiceVersionErrorText2, Conference.VenueServiceWrapper.MaximumVersion),
                        Strings.VersionIncompatible, MessageBoxButtons.OK, MessageBoxIcon.Stop,
                        MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                }

                VenueServiceBaseUrl = null;
            }

            foreach (Venue v in Conference.VenueServiceWrapper.Venues)
            {
                // Add the venue to the list

                if (v.PWStatus == PasswordStatus.NO_PASSWORD)
                    imageList.Images.Add(GenerateThumbnail48(v.Icon));
                else if (v.PWStatus == PasswordStatus.WEAK_PASSWORD)
                    imageList.Images.Add(GenerateThumbnail48(v.Icon, lockImage));
                else // strong password (encryption)
                {
                    imageList.Images.Add(GenerateThumbnail48(v.Icon, encryptedLockImage));
                }

                ListViewItem i = new ListViewItem();
                i.Text = v.Name;
                i.Tag = v;
                i.ImageIndex = cnt++;
                listView.Items.Add(i);
            }
            Cursor.Current = prevCursor;

            menuMyProfile.Enabled = true;
            menuActionsCapabilities.Enabled = false;

            menuActionsPresentation.Enabled = false;
            menuActionsChat.Enabled = false;
            menuActionsWMPlayback.Enabled = false;
            menuActionsScreenScraper.Enabled = false;
            menuActionsSharedBrowser.Enabled = false;
            menuActionsUWClassroomPresenter.Enabled = false;
            menuActionsCapabilities.Enabled = false;
            menuActionsActiveCapabilities.Enabled = false;
            menuActionsUnicast.Enabled = true;
            menuSettingsServices.Enabled = true;
            menuSettingsAudioVideo2.Enabled = true;
            menuSettingsRTDocViewer.Enabled = true;

            SetArchiverMenuStatus();
            DisplayVenueStatus();
            SetDiagnosticsMenuStatus();
        }

        /// <summary>
        /// Find the default venue service in the registry and use it as the current VS.
        /// </summary>
        private void SetVenueService()
        {
            string currentVenue = VenueServiceBaseUrl;

            // Check to see if a new venue service has been selected
            string[] names = venuesRegKey.GetValueNames();
            string venueService = null;
            foreach (string venue in names)
            {
                string venueState = (string)venuesRegKey.GetValue(venue);
                if (Boolean.Parse(venueState))
                {
                    venueService = venue;
                    break;
                }
            }

            if (currentVenue != venueService)
            {
                VenueServiceBaseUrl = venueService;
                DisplayVenues();
            }
        }

        /// <summary>
        /// Create a custom Local venue. The TTL parameter doesn't matter since currently the only way to overide the default of ttl=255 is through app.config
        /// </summary>
        private static Venue AddLocalVenue()
        {
            if (!Conference.VenueServiceWrapper.Venues.ContainsKey("Local Venue"))
            {
                VenueData vd = new VenueData("Local Venue",
                    new IPEndPoint(IPAddress.Parse("234.9.8.7"), 5004), 255, VenueType.Custom, null);

                Conference.VenueServiceWrapper.AddCustomVenue(vd);
            }

            return Conference.VenueServiceWrapper.Venues["Local Venue"];
        }
        private void DisplayRTDocumentViewers()
        {
            string[] names = rtdocsRegKey.GetValueNames();
            foreach (string key in names)
            {
                MenuItem mi = new MenuItem(key, new EventHandler(RTDocsViewerClick));

                // check the one marked as default
                object val = rtdocsRegKey.GetValue(key);
                if (val.ToString() == "default")
                {
                    mi.Checked = true;
                }

                menuSettingsRTDocViewer.MenuItems.Add(mi);
            }
        }

        private void GetArchiveService()
        {
            // Get list of registered archivers, and select the one that is enabled, if any
            string[] keys = archiversRegKey.GetValueNames();
            archiveServiceDefault = null;

            // Find the default Archive Service, if there is one
            if (keys != null)
            {
                foreach (string key in keys)
                {
                    // See whether or not it is enabled based on the value of the key/value pair
                    if (bool.Parse((string)archiversRegKey.GetValue(key)))
                    {
                        archiveServiceDefault = key;
                        break;
                    }
                }
            }

            // If we found a default, use it
            if (archiveServiceDefault != null)
            {
                archiverState = ArchiverState.Stopped;
                GetNewArchiver();
            }
            else
            {
                archiverState = ArchiverState.Unavailable;
                // Disable the archiver menu
                SetArchiverMenuStatus();
            }
        }

        /// <summary>
        /// Creates a new IArchiveServer object to make remote calls to.
        /// </summary>
        internal void GetNewArchiver()
        {
            if (archiveServiceDefault != null)
            {
                try
                {
                    object factoryObj = Activator.GetObject(typeof(IArchiveServer), GetArchiveUri() + "/ArchiveServer");
                    this.archiver = (IArchiveServer)factoryObj;
                }
                catch (Exception ex)
                {
                    archiverState = ArchiverState.Unavailable;

                    RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                        Strings.ArchiveServiceErrorText, ex.ToString()), Strings.ArchiveServiceErrorTitle, 
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 
                        (MessageBoxOptions)0);
                }
            }

            SetArchiverMenuStatus();
        }

        /// <summary>
        /// Add "tcp://" prefix as well as port postfix if needed 
        /// </summary>
        /// <returns>formatted URI</returns>
        private string GetArchiveUri()
        {
            if (archiveServiceDefault == null)
                return null;

            string archiverUri = archiveServiceDefault;

            // Add the tcp:// prefix to the ArchiveService address if it needs it
            if (!archiverUri.StartsWith("tcp://"))
            {
                archiverUri = "tcp://" + archiverUri;
            }

            // Add port postfix, if needed
            if (archiverUri.IndexOf(':', 4) < 4)
            {
                archiverUri += ":8082"; // NOTE: hardcoded default port here
            }

            return archiverUri;
        }

        /// <summary>
        /// Accessor to enable/disable the playback menu
        /// </summary>
        internal bool EnableMenuActionsPlayback
        {
            set
            {
                menuActionsPlayback.Enabled = value;
            }
        }

        /// <summary>
        /// Accessor to enable/disable the record menu
        /// </summary>
        internal bool EnableMenuActionsRecord
        {
            set
            {
                menuActionsRecord.Enabled = value;
            }
        }

        internal void SetDiagnosticsMenuStatus() {
            if ((Conference.DiagnosticsEnabled) &&
                (Conference.ActiveVenue != null) &&
                (!twoWayUnicast)) {
                this.menuActionsDiagnostics.Enabled = true;
            }
            else {
                this.menuActionsDiagnostics.Enabled = false;
            }
        }

        internal void SetArchiverMenuStatus()
        {
            if ((archiveServiceDefault != null) && (!twoWayUnicast))
            {
                if (archiverState == ArchiverState.Unavailable)
                {
                    menuActionsRecord.Enabled = false;
                    menuActionsPlayback.Enabled = false;
                }
                else
                {
                    Venue activeVenue = Conference.ActiveVenue as Venue;
                    if (activeVenue == null)
                    {
                        menuActionsRecord.Enabled = false;
                        menuActionsPlayback.Enabled = true;

                        if (archiverState == ArchiverState.Playing)
                        {
                            menuActionsPlayback.Enabled = false;
                        }
                    }
                    else
                    {
                        menuActionsRecord.Enabled = true;
                        menuActionsPlayback.Enabled = true;

                        // TODO: Combine these to if statements since thy set the
                        //       same value
                        if (archiverState == ArchiverState.Playing)
                        {
                            menuActionsRecord.Enabled = false;
                            menuActionsPlayback.Enabled = false;
                        }
                        else if (archiverState == ArchiverState.Recording)
                        {
                            menuActionsPlayback.Enabled = false;
                            menuActionsRecord.Enabled = false;
                        }
                        else if (activeVenue.PWStatus == PasswordStatus.STRONG_PASSWORD)
                        {
                            // we currently disallow recording of encrypted venues
                            menuActionsRecord.Enabled = false;
                        }
                    }
                }
            }
            else // There is no default archive service
            {
                menuActionsPlayback.Enabled = false;
                menuActionsRecord.Enabled = false;
            }
        }

        private void SetReflectorService()
        {
            // Check to see if any reflector is marked as enabled (True)
            string[] keys = reflectorsRegKey.GetValueNames();
            string reflectorDefault = null;

            // Find the default reflector service, if there is one
            if (keys != null)
            {
                DisplayVenueStatus(); // We display venue service in the status bar when reflector is not enabled

                foreach (string key in keys)
                {
                    // See whether or not it is enabled (based on the value of the key/value pair)
                    if (bool.Parse((string)reflectorsRegKey.GetValue(key)))
                    {
                        reflectorDefault = key;
                    }
                }
            }

            Conference.ReflectorEnabled = false;

            // If we found a default, use it
            if (reflectorDefault != null)
            {
                Uri reflectorUri = MSR.LST.ConferenceXP.frmServices2.ValidateUri(reflectorDefault);
                if (reflectorUri != null)
                {
                    string portOverrideStr = ConfigurationManager.AppSettings["MSR.LST.Net.RtpReflectorPort"];
                    if (portOverrideStr != null)
                        Conference.ReflectorRtpPort = Int32.Parse(portOverrideStr, CultureInfo.InvariantCulture);
                    else { } // use hard-coded default...

                    Conference.ReflectorAddress = reflectorUri.Host;
                    Conference.ReflectorEnabled = true;

                    // set address on heartbeat monitor
                    IPAddress addr = null;
                    try
                    {
                        addr = System.Net.Dns.GetHostEntry
                            (Conference.ReflectorAddress).AddressList[0];
                    }
                    catch (Exception)
                    {
                        if (!IPAddress.TryParse(Conference.ReflectorAddress, out addr)) {
                            addr = null;
                        }
                    }

                    if (addr != null) {
                        try {
                            IPEndPoint ep = new IPEndPoint(addr, Conference.ReflectorRtpPort);
                            monitor.SetReflectorAddress(ep);
                        }
                        catch { }
                    }

                }
            }

            // set ref enabled on heartbeat monitor
            monitor.ReflectorEnabled = Conference.ReflectorEnabled;

            // Update the status bar 
            DisplayVenueStatus();
        }


        #region Set Diagnostic Services

        /// <summary>
        /// Check to see if any new Diagnostic Services were added to the app.config
        /// If so, the first new one will be the one returned. 
        /// In the process we will also add any new items from app.config to the registry.
        /// </summary>
        private void SetDiagnosticService() {
            string ds = AddDiagnosticServicesFromAppConfig();

            if (ds == null) // Nothing new in app.config
            {
                // Check registry
                string[] names = diagnosticsRegKey.GetValueNames();
                foreach (string key in names) {
                    if (bool.Parse((string)diagnosticsRegKey.GetValue(key))) {
                        ds = key;
                        break;
                    }
                }
            }

            MSR.LST.Net.Rtp.RtpSession.DiagnosticsEnabled = false;
            Conference.DiagnosticsEnabled = false;

            // If we found an activated Diagnostic Service, attempt to use it.
            if (ds != null) {
                Uri diagnosticsUri = MSR.LST.ConferenceXP.frmServices2.ValidateUri(ds);
                if (diagnosticsUri != null) {
                    //Flag Conference to enable the additional client functionality by using incoming diagnostic information.
                    Conference.DiagnosticsEnabled = true;
                    Conference.DiagnosticsWebService = diagnosticsUri;

                    //Flag the RTP code to participate by sending our diagnostic information to the server.
                    MSR.LST.Net.Rtp.RtpSession.DiagnosticsServer = diagnosticsUri.Host;
                    MSR.LST.Net.Rtp.RtpSession.DiagnosticsEnabled = true;
                }
            }

        }

        /// <summary>
        /// Adds a new Diagnostic service from the app.config to the list of services in the registry
        /// If there are one or more new services in the app.config, we will activate 
        /// and return the first one.
        /// </summary>
        private static string AddDiagnosticServicesFromAppConfig() {
            string dsKey = "MSR.LST.ConferenceXP.DiagnosticService"; //App.config key
            string newDS = null;

            // Check to see if the default service has changed in app.config
            string setting;
            if ((setting = ConfigurationManager.AppSettings[dsKey]) != null) {
                ProcessDiagnosticService(setting, ref newDS);
            }

            // The next entry in the app.config starts with the postfix 2
            // i.e. - MSR.LST.ConferenceXP.DiagnosticService2
            int postfix = 2;
            while ((setting = ConfigurationManager.AppSettings[dsKey + postfix]) != null) {
                ProcessDiagnosticService(setting, ref newDS);
                postfix++; // Move to the next entry
            }

            return newDS;
        }

        /// <summary>
        /// If the specified App.config DS is already in the registry do nothing.  
        /// Otherwise, add it to the registry.  Also, if newDS is null,
        /// make the new DS the enabled DS, disable existing entries in the registry, and 
        /// return in newDS.
        /// </summary>
        /// <param name="appConfigDS"></param>
        /// <param name="newDS"></param>
        private static void ProcessDiagnosticService(string appConfigDS, ref string newDS) {
            // If it's not already in the registry...
            if (diagnosticsRegKey.GetValue(appConfigDS) == null) {
                if (newDS == null) {
                    // This is the first new app.config entry; disable all existing entries.
                    string[] names = diagnosticsRegKey.GetValueNames();
                    foreach (string key in names) {
                        diagnosticsRegKey.SetValue(key, false);
                    }

                    // Enable new value in registry and return it in newDS
                    diagnosticsRegKey.SetValue(appConfigDS, true);
                    newDS = appConfigDS;
                }
                else {
                    // This is a new entry, but not the first new entry.  
                    // Add this one to the registry, but disable it, and don't change newDS.
                    diagnosticsRegKey.SetValue(appConfigDS, false);
                }
            }
        }

        #endregion Set Diagnostic Services

        private void DisplayOtherCapabilitySenders()
        {
            menuActionsCapabilities.MenuItems.Clear();
            foreach (string s in Conference.OtherCapabilitySenders)
            {
                if (s != "Presentation" && s != "Chat" && s != "Windows Media Playback" && s != "Local Screen Streaming"
                    && s != "Shared Browser" && s != "Classroom Presenter 3")
                {
                    MenuItem mi = new MenuItem(string.Format(CultureInfo.CurrentCulture, 
                        Strings.StartCapabilityMenuItems, s), new EventHandler(OtherCapabilitySenderClick));
                    menuActionsCapabilities.MenuItems.Add(mi);
                }
                if (s.Equals("Classroom Presenter 3")) {
                    menuActionsUWClassroomPresenter.Visible = true;
                }
            }
        }

        private void UpdateActionMenu()
        {
            menuActionsActiveCapabilities.MenuItems.Clear();

            foreach (ICapabilityViewer cv in Conference.CapabilityViewers.Values)
            {
                // Skip the CapabilityViewers associated with a participant
                if (cv.Owner != null)
                {
                    continue;
                }
                MenuItem mi = new MenuItem(cv.Name, new EventHandler(ActiveCapabilityClick));
                if (cv.IsPlaying == true)
                {
                    mi.Checked = true;
                }
                else
                {
                    mi.Checked = false;
                }

                menuActionsActiveCapabilities.MenuItems.Add(mi);
                menuItemTags.Add(mi, cv);
            }

            if (menuActionsActiveCapabilities.MenuItems.Count > 0)
            {
                menuActionsActiveCapabilities.Enabled = true;
            }
            else
            {
                menuActionsActiveCapabilities.Enabled = false;
            }
        }


        private void RefreshImages()
        {
            for (int i = 0; i < listView.Items.Count; i++)
            {
                listView.Items[i].ImageIndex = i;
                Image icon = null;
                if (listView.Items[i].Tag is Participant) {
                    icon = ((Participant)listView.Items[i].Tag).DecoratedIcon;
                }

                if (icon != null) {
                    imageList.Images[i] = GenerateThumbnail48(icon);
                }
            }
            listView.Refresh();
        }

        private Image GenerateThumbnail48(Image masterImage, Image subImage)
        {
            Bitmap icon = new Bitmap(48, 48, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(icon))
            {
                g.FillRectangle(new SolidBrush(listView.BackColor), 0, 0, 48, 48);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(masterImage, 0, 0, 48, 48);

                if (subImage != null)
                {
                    // Draw the bitmap.
                    g.DrawImage(subImage, new Point(28, 28));
                }
              }
            return icon;
        }

        private Image GenerateThumbnail48(Image masterImage)
        {
            return GenerateThumbnail48(masterImage, null);
        }

        #endregion

        #region ConferenceAPI Event Handlers

        private void OnParticipantAdded(IParticipant p)
        {
            if (Conference.ActiveVenue != null) // We may have already left the venue and this is coming in late because it's invoked on the form thread and comes in via the message loop
            {
                //If there was a missing participant in the list with this identifier, remove it.
                foreach (ListViewItem item in listView.Items) {
                    if ((item.Tag is Participant) &&
                        (((Participant)item.Tag).MissingParticipant) &&
                        (((Participant)item.Tag).Identifier.Equals(p.Identifier))) {
                        imageList.Images.RemoveAt(item.Index);
                        listView.Items.Remove(item);
                    }
                }

                ListViewItem lvi = new ListViewItem();
                lvi.Text = p.Name;
                lvi.Tag = p;
                listView.Items.Add(lvi);
                imageList.Images.Add(p.DecoratedIcon);
                RefreshImages();
                DisplayParticipantCount();
            }
        }

        private void OnParticipantRemoved(IParticipant p)
        {
            if (Conference.ActiveVenue != null) // We may have already left the venue and this is coming in late because it's invoked on the form thread and comes in via the message loop
            {
                // Remove the receiver participant from the list
                foreach (ListViewItem lvi in listView.Items)
                {
                    if (lvi.Tag == p)
                    {
                        imageList.Images.RemoveAt(lvi.Index);
                        listView.Items.Remove(lvi);
                    }
                }

                RefreshImages();
                DisplayParticipantCount();
            }
        }

        private void OnCapabilityAdded(object conference, CapabilityEventArgs cea)
        {
            if (Conference.ActiveVenue != null) // We may have already left the venue and this is coming in late because it's invoked on the form thread and comes in via the message loop
            {
                RefreshImages();

                if (cea.Capability is AudioCapability)
                {
                    ((AudioCapability)cea.Capability).AutoPlayRemote = AutoPlayRemoteAudio;
                }

                if (cea.Capability is VideoCapability)
                {
                    ((VideoCapability)cea.Capability).AutoPlayRemote = AutoPlayRemoteVideo;
                }
            }
        }

        private void OnCapabilityRemoved(object conference, CapabilityEventArgs cea)
        {
            if (Conference.ActiveVenue != null) // We may have already left the venue and this is coming in late because it's invoked on the form thread and comes in via the message loop
            {
                RefreshImages();
            }
        }

        private delegate void HSEHInvoker(object session, RtpEvents.HiddenSocketExceptionEventArgs hseea);
        private void HiddenSockExHandler(object session, RtpEvents.HiddenSocketExceptionEventArgs hseea)
        {
            if (this.InvokeRequired) {
                this.Invoke(new HSEHInvoker(HiddenSockExHandler),new object[]{session,hseea});
            }
            else {

                if (Conference.ActiveVenue != null && hseea.Session == Conference.RtpSession) {
                    btnLeaveConference.PerformClick();

                    RtlAwareMessageBox.Show(this, Strings.SocketExceptionText, Strings.SocketExceptionTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                }
            }
        }

        private void OnDuplicateIdentityDetected(object conference, Conference.DuplicateIdentityDetectedEventArgs ea)
        {
            btnLeaveConference.PerformClick();

            RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, Strings.ExitedVenueSameName,
                ea.IPAddresses[0].ToString(), ea.IPAddresses[1].ToString()), Strings.DuplicateIdentityDetected, 
                MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
        }

        void OnDiagnosticUpdate(DiagnosticMonitor.DiagnosticUpdateEventArgs args) {
            Console.WriteLine("**OnDiagnosticUpdate:" + args.ToString());
            //Update participant icons and tooltips
            switch (args.EventType) {
                case DiagnosticMonitor.DiagnosticEventType.MissingParticipantAdded:
                    AddMissingParticipant(args.Sender);
                    break;
                case DiagnosticMonitor.DiagnosticEventType.MissingParticipantRemoved:
                    RemoveMissingParticipant(args.Sender);
                    break;
                case DiagnosticMonitor.DiagnosticEventType.ThroughputWarningAdded:
                    AddThroughputWarning(args.Sender, args.Receiver);
                    break;
                case DiagnosticMonitor.DiagnosticEventType.ThroughputWarningRemoved:
                    RemoveThroughputWarning(args.Sender, args.Receiver);
                    break;
            }
        }

        private void RemoveThroughputWarning(string sender, string receiver) {
            //if there is a participant whose identifer matches sender or receiver, remove the warning information.
            if (Conference.ActiveVenue != null) {
                foreach (ListViewItem lvi in listView.Items) {
                    if (lvi.Tag is Participant) {
                        Participant p = (Participant)lvi.Tag;
                        if (p.Identifier.Equals(sender)) {
                            p.RemoveThroughputWarning(new Participant.ThroughputWarning(Participant.ThroughputWarningType.Outbound, receiver));
                        }
                        else if (p.Identifier.Equals(receiver)) {
                            p.RemoveThroughputWarning(new Participant.ThroughputWarning(Participant.ThroughputWarningType.Inbound, sender));
                        }
                    }
                }
                RefreshImages();
            }            
        }

        private void AddThroughputWarning(string sender, string receiver) {
            //if there is a participant whose identifer matches sender or receiver, add the warning icon decoration and tooltip details.
            if (Conference.ActiveVenue != null) 
            {
                foreach (ListViewItem lvi in listView.Items) {
                    if (lvi.Tag is Participant) {
                        Participant p = (Participant)lvi.Tag;
                        if (p.Identifier.Equals(sender)) {
                            p.AddThroughputWarning(new Participant.ThroughputWarning(Participant.ThroughputWarningType.Outbound, receiver));
                        }
                        else if (p.Identifier.Equals(receiver)) {
                            p.AddThroughputWarning(new Participant.ThroughputWarning(Participant.ThroughputWarningType.Inbound, sender));
                        }
                    }
                }
                RefreshImages();
            }            
        }

        private void RemoveMissingParticipant(string cname) {
            if (Conference.ActiveVenue != null) 
            {
                // Remove the missing participant from the list
                foreach (ListViewItem lvi in listView.Items) {
                    if ((lvi.Tag is Participant) && 
                        (((Participant)lvi.Tag).MissingParticipant) &&
                        (((Participant)lvi.Tag).Identifier.Equals(cname))) {
                        imageList.Images.RemoveAt(lvi.Index);
                        listView.Items.Remove(lvi);
                    }
                }

                RefreshImages();
            }
        }

        private void AddMissingParticipant(string cname) {
            if (Conference.ActiveVenue != null)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = "Missing Participant: " + cname;
                Participant mp = new Participant(cname);
                lvi.Tag = mp;
                listView.Items.Add(lvi);
                imageList.Images.Add(mp.Icon);
                RefreshImages();
            }        
        }

        #endregion

        #region Menu Handlers

        #region Settings menu
        private void menuSettingsAudioVideo2_Click(object sender, System.EventArgs e)
        {
            new frmAVDevices(cxpclientRegKey).ShowDialog();
        }

        //private void menuSettingsNetworkDiagnostics_Click(object sender, System.EventArgs e)
        //{
        //    if (stoplight != null)
        //    {
        //        stoplight.Dispose();
        //    }

        //    // Create a new one each time
        //    stoplight = new frmStopLight();
        //    stoplight.WindowState = FormWindowState.Normal;
        //    stoplight.Show();
        //}

        private void menuSettingsServices_Click(object sender, System.EventArgs e)
        {
            frmServices services = new frmServices(venuesRegKey, archiversRegKey, reflectorsRegKey, diagnosticsRegKey);
            if (services.ShowDialog() == DialogResult.OK)
            {
                SetVenueService();
                GetArchiveService();
                SetReflectorService();
                SetDiagnosticService();
            }

        }

 

        private void menuSettingsAppConfig_Click(object sender, System.EventArgs e)
        {
            // This is *really* cheap, but hey, it works!
            Process.Start("notepad.exe", Process.GetCurrentProcess().ProcessName + ".exe.config");
        }

        private void menuMyProfile_Click(object sender, System.EventArgs e)
        {
            Conference.EditProfileUI();
        }

        private void RTDocsViewerClick(object o, EventArgs ea)
        {
            MenuItem mi = (MenuItem)o;
            if (mi.Checked == true) { } // do nothing
            else
            {
                // Reset all the devices to false and check new default
                foreach (MenuItem miSib in mi.Parent.MenuItems)
                {
                    miSib.Checked = false;
                }
                mi.Checked = true;

                // Update registry with new default and set others to non-default
                string[] names = rtdocsRegKey.GetValueNames();
                foreach (string key in names)
                {
                    rtdocsRegKey.SetValue(key, "non-default");
                }
                rtdocsRegKey.SetValue(mi.Text, "default");

                // Tell user this takes effect next time the client starts
                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, Strings.PresentationViewerChangeText,
                    mi.Text), Strings.PresentationViewerChangeTitle, MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
        }
        #endregion

        #region Actions menu
        private void menuActions_Select(object sender, System.EventArgs e)
        {
            if (Conference.VenueServiceWrapper.Venues != null)
            {
                UpdateActionMenu();
            }
        }

        private void menuActionsPresentation_Click(object sender, System.EventArgs e)
        {
            ICapabilitySender cs = Conference.CreateCapabilitySender("Presentation");
        }

        private void menuActionsChat_Click(object sender, System.EventArgs e)
        {
            ICapabilitySender cs = Conference.CreateCapabilitySender("Chat");
        }

        private void menuActionsWMPlayback_Click(object sender, System.EventArgs e)
        {
            ICapabilitySender cs = Conference.CreateCapabilitySender("Windows Media Playback");
        }

        private void menuActionsScreenScraper_Click(object sender, System.EventArgs e)
        {
            ICapabilitySender cs = Conference.CreateCapabilitySender("Local Screen Streaming");
        }

        private void menuActionsSharedBrowser_Click(object sender, System.EventArgs e)
        {
            ICapabilitySender cs = Conference.CreateCapabilitySender("Shared Browser");
        }

        private void menuActionsUWClassroomPresenter_Click(object sender, System.EventArgs e)
        {
            ICapabilitySender cs = Conference.CreateCapabilitySender("Classroom Presenter 3");
        }

        private void ActiveCapabilityClick(object o, EventArgs ea)
        {
            MenuItem mi = (MenuItem)o;
            ICapabilityViewer cv = (ICapabilityViewer)menuItemTags[mi];

            try
            {
                if (mi.Checked)
                {
                    // Stop sending as well if this cv is also an ICapabilitySender
                    if (cv is ICapabilitySender)
                    {
                        ((ICapabilitySender)cv).StopSending();
                    }

                    cv.StopPlaying();
                    mi.Checked = false;
                }
                else
                {
                    //Pri2: We want to check for success here and notify the user if a failure occurs?
                    cv.Play();
                    mi.Checked = true;
                }
                UpdateActionMenu();
            }
            catch (ObjectDisposedException)
            {
                // If a channel capability is accessed after it has been closed, but before the BYE
                // packet is received we may try to access the capability after it has been
                // disposed.
            }

        }

        private void OtherCapabilitySenderClick(object o, EventArgs ea)
        {
            MenuItem mi = (MenuItem)o;
            string name = mi.Text.Substring(6);
            name = name.Remove(name.Length - 3, 3);

            ICapabilitySender cs = Conference.CreateCapabilitySender(name);
        }

        /// <summary>
        /// menuActionsRecord Click event handler. Open the record dialog box.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void menuActionsRecord_Click(object sender, System.EventArgs e)
        {
            frmRecord record = new frmRecord(this);
            record.Show();
            record.Location = new Point(
                SystemInformation.WorkingArea.Right - record.Width,
                SystemInformation.WorkingArea.Bottom - record.Height);


            // TODO: FYI: I removed code to change cursor:
            //       Begining: this.Cursor = Cursors.WaitCursor;
            //       End: this.Cursor = Cursors.Default;
            //       Check if this code needs to be added again
        }

        /// <summary>
        /// Find a playback venue to join.
        /// </summary>
        /// <returns>The venue</returns>
        private Venue GetPlayBackVenue()
        {
            Venue playbackVenue;

            // Check to make sure we aren't playing back from our machine to our machine
            Uri archiverUri = new Uri(GetArchiveUri());
            if (archiverUri.IsLoopback)
            {
                // In the odd case that we're trying to play back from a local archiver, do a multicast playback
                //  (becasue unicast isn't technically possible), in a random venue
                playbackVenue = Conference.VenueServiceWrapper.CreateRandomMulticastVenue("Playback Venue", null);
            }
            else
            {
                // Get the Archiver's IPAddress
                IPAddress archiverIP;
                try {
                    IPHostEntry archiverHE = Dns.GetHostEntry(archiverUri.Host);
                    archiverIP = archiverHE.AddressList[0];
                }
                catch {
                    //Starting with .Net 4 GetHostEntry will throw if argument is an IP address.
                    IPAddress.TryParse(archiverUri.Host, out archiverIP);
                }

                // Get the port to playback on
                int playbackPort = archiverUnicastPort;
                string portOverrideStr = ConfigurationManager.AppSettings["MSR.LST.ConferenceXP.ArchiveService.UnicastPort"];
                if (portOverrideStr != null)
                    playbackPort = Int32.Parse(portOverrideStr, CultureInfo.InvariantCulture);

                // Join to the Archiver's IPAddress as a "venue"
                VenueData ven = new VenueData("Playback Venue", new IPEndPoint(archiverIP, playbackPort), ushort.MaxValue,
                    VenueType.PrivateVenue, null);
                playbackVenue = Conference.VenueServiceWrapper.AddCustomVenue(ven);
            }
            return playbackVenue;
        }

        /// <summary>
        /// Display the playback info in the status bar
        /// </summary>
        private void DisplayPlayBackInfo(ArchiveService.Conference selectedConference)
        {
            MSR.LST.ConferenceXP.ArchiveService.Conference conf = selectedConference;

            // TODO: Put Debug.Assert instead of if ( conf != null )
            if (conf != null)
            {
                DisplayStatusMessage(string.Format(CultureInfo.CurrentCulture, Strings.PlaybackConference, 
                    conf.Description));
            }
        }

        private void menuActionsPlayback_Click(object sender, System.EventArgs e)
        {
            // Create and show the play back dialog box to select a conference
            // to play back
            // Note: we pass a reference to FMain form in the ctor so the created form
            //       can communicate with FMain

            if (sender != this && (this.archiverState == ArchiverState.Stopped))
            {
                GetNewArchiver();
                frmArchiveConf client = new frmArchiveConf(this.archiver, this);
                if (!client.IsDisposed)
                {
                    client.Show();
                    client.Location = new Point(
                        SystemInformation.WorkingArea.Right - client.Width,
                        SystemInformation.WorkingArea.Bottom - client.Height);

                }
            }
            else if (sender != this && this.archiverState == ArchiverState.Playing)
            {
                StopPlayBack();
            }
        }

        private void menuActionsUnicast_Click(object sender, System.EventArgs e)
        {
            frmNetworkUnicast unicastSession = new frmNetworkUnicast();
            if (unicastSession.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    VenueData vd = new VenueData("Unicast Venue", new System.Net.IPEndPoint(remoteIP,
                        Convert.ToInt32("5004", CultureInfo.InvariantCulture)), 127, VenueType.Custom, null, null, null);
                    Venue v = Conference.VenueServiceWrapper.AddCustomVenue(vd);
                    JoinVenue(v, true);
                    twoWayUnicast = true;
                    SetArchiverMenuStatus();
                }
                catch (Exception ex)
                {
                    RtlAwareMessageBox.Show(this, ex.Message, Strings.UnableToJoinUnicastVenue, 
                        MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 
                        (MessageBoxOptions)0);
                    twoWayUnicast = false;
                    SetArchiverMenuStatus();
                }
            }
        }

        private void menuActionsPersist_Click(object sender, EventArgs e) {
            menuActionsPersist.Checked = !menuActionsPersist.Checked;
            Conference.PersistWindowPositions = menuActionsPersist.Checked; 
        }

        private void menuActionsDiagnostics_Click(object sender, EventArgs e) {
            string diagnosticsWebServiceQuery = Conference.DiagnosticsWebService.ToString();
            if (Conference.ActiveVenue != null) {
                //Add a query string to cause the default view to go to the current venue. %23 is encoded '#'.
                string venueMoniker = System.Net.WebUtility.HtmlEncode(Conference.ActiveVenue.Name) + "%23" + Conference.ActiveVenue.EndPoint.Address.ToString();
                diagnosticsWebServiceQuery += "?venue=" + venueMoniker;
            }
            Process.Start(diagnosticsWebServiceQuery);
        }

        #endregion

        #region Help menu

        private void menuHelpAbout_Click(object sender, System.EventArgs e)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);

            // Note: adding a string such as "1.0 RC1" to AssemblyInformationalVersion in AssemblyInfo.cs will cause
            // that information to be displayed as well.
            string version = fvi.FileVersion;
            if (fvi.FileVersion != Application.ProductVersion) {
                version += " (" + Application.ProductVersion + ")";
            }

            RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, Strings.AboutCXPClientText,
                version, Conference.About), Strings.AboutCXPClientTitle, MessageBoxButtons.OK,
                MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
        }

        private void menuHelpCommunity_Click(object sender, System.EventArgs e)
        {
            Process.Start(helpurlCommunity);
        }

        private void menuHelpConferenceXP_Click(object sender, System.EventArgs e)
        {
            if (bStudentMode)
            {
                Process.Start(helpurlStudentEdition);
            }
            else
            {
                Process.Start(helpurlConferenceXP);
            }
        }

        # endregion


        #endregion Menu handlers

        #region Context handlers
        private void listView_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Point clientCoords = listView.PointToClient(Cursor.Position);
            ListViewItem lvi = listView.GetItemAt(clientCoords.X, clientCoords.Y);
            string toolTipString = string.Empty;

            if (lvi != null)
            {
                if (lvi.Tag is Participant)
                {
                    Participant p = (Participant)lvi.Tag;

                    if (p.MissingParticipant) {
                        toolTipString = "Participant " + p.Identifier + " is in the venue, but no streams are received. \n\n";
                        foreach (Participant.ThroughputWarning tw in p.ThroughputWarnings) {
                            if (tw.WarningType == Participant.ThroughputWarningType.Inbound) {
                                toolTipString += "Throughput Warning: Inbound to this participant from " + tw.OtherParticipant + "\n";
                            }
                            else if (tw.WarningType == Participant.ThroughputWarningType.Outbound) {
                                toolTipString += "Throughput Warning: Outbound from this participant to " + tw.OtherParticipant + "\n";
                            }
                        }
                    }
                    else {
                        // rtpParticpant can be null if the participant is leaving
                        if (p.RtpParticipant != null) {
                            toolTipString = string.Format(CultureInfo.CurrentCulture, Strings.NameIdentifierIPEMail +
                                "\n", p.Name, p.Identifier, p.RtpParticipant.IPAddress, p.Email);

                            foreach (ICapabilityViewer cv in p.Capabilities) {
                                toolTipString += cv.PayloadType + ": " + cv.Name;
                                if (cv.IsPlaying) {
                                    toolTipString += Strings.Playing;
                                }
                                toolTipString += "\n";
                            }

                            toolTipString += "\n";
                            foreach (Participant.ThroughputWarning tw in p.ThroughputWarnings) {
                                if (tw.WarningType == Participant.ThroughputWarningType.Inbound) {
                                    toolTipString += "Throughput Warning: Inbound to this participant from " + tw.OtherParticipant + "\n";
                                }
                                else if (tw.WarningType == Participant.ThroughputWarningType.Outbound) {
                                    toolTipString += "Throughput Warning: Outbound from this participant to " + tw.OtherParticipant + "\n";
                                }
                            }
                        }
                    }
                }
                if (lvi.Tag is Venue)
                {
                    Venue v = (Venue)lvi.Tag;

                    if (v.VenueData.VenueType == VenueType.Invalid)
                    {
                        toolTipString = Strings.VenueNotAvailable;
                    }
                    else
                    {
                        toolTipString = string.Format(CultureInfo.CurrentCulture, Strings.EnterAVenue, v.Name,
                            v.Identifier, v.EndPoint.Address.ToString(), v.EndPoint.Port);

                        // encryption status
                        Venue venue = v as Venue;
                        if (v.PWStatus == PasswordStatus.STRONG_PASSWORD)
                            toolTipString += "\n" + Strings.UsesEncryption;
                        else if (v.PWStatus == PasswordStatus.WEAK_PASSWORD)
                            toolTipString += "\n" + Strings.DoesNotUseEncryption;
                    }

                }
            }

            if (toolTip.GetToolTip(listView) != toolTipString)
                toolTip.SetToolTip(listView, toolTipString);
        }


        private void contextParticipant_Popup(object sender, System.EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
            {
                contextParticipant.MenuItems.Clear();
                return;
            }

            ListViewItem lvi = listView.SelectedItems[0];

            contextParticipant.MenuItems.Clear();

            if (lvi.Tag is Participant)
            {
                Participant p = (Participant)lvi.Tag;

                foreach (ICapabilityViewer cv in p.Capabilities)
                {
                    string menuText;
                    if (cv.IsPlaying)
                        menuText = string.Format(CultureInfo.CurrentCulture, Strings.StopNamePayloadType, cv.Name, 
                            cv.PayloadType.ToString());
                    else
                        menuText = string.Format(CultureInfo.CurrentCulture, Strings.PlayNamePayloadType, cv.Name, 
                            cv.PayloadType.ToString());

                    MenuItem mi = new MenuItem(menuText, new EventHandler(OnParticipantContextClick));
                    contextMenuItems.Add(mi, cv);
                    contextParticipant.MenuItems.Add(mi);
                }

                if (lvi.Tag is Venue)
                    return;
            }
        }


        private void OnParticipantContextClick(object sender, System.EventArgs e)
        {
            ICapabilityViewer cv = (ICapabilityViewer)contextMenuItems[sender];

            try
            {
                // Toggle the cv
                if (cv.IsPlaying)
                {
                    cv.StopPlaying();
                }
                else
                {
                    //Pri2: We want to check the result here and notify the user upon failure instead of fail silently

                    cv.Play();
                }
            }
            catch (ObjectDisposedException)
            {
                // If the context menu for a participant is selected before a capability for that
                // participant goes away, we may try to access the capability after it has been
                // disposed.
            }
        }

        internal void LeaveConference()
        {
            this.DisplayStatusInProgress(Strings.LeavingConference);

            Cursor.Current = Cursors.WaitCursor;

            // Stop recording or playing
            if (archiverState == ArchiverState.Recording)
            {
                StopRecording();
            }
            else if (archiverState == ArchiverState.Playing)
            {
                StopPlayBack();
            }

            twoWayUnicast = false;
            SetArchiverMenuStatus();

            if (Conference.ActiveVenue != null)
            {
                Conference.LeaveVenue();

                // If we just left a unicast venue, then remove it
                foreach (Venue v in Conference.VenueServiceWrapper.Venues)
                {
                    if (v.Name.StartsWith("Unicast") || v.Name.StartsWith("Playback"))
                    {
                        Conference.VenueServiceWrapper.Venues.Remove(v.Name);
                        break;
                    }
                }
            }

            btnLeaveConference.Visible = false;

            listView.Enabled = true;

            this.Text = Strings.ConferenceXP;

            DisplayVenues();

            Cursor.Current = Cursors.Default;
        }

        private void btnLeaveConference_Click(object sender, System.EventArgs e)
        {
            LeaveConference();
        }


        private void listView_ItemActivate(object sender, System.EventArgs e)
        {
            // because there is no locking to prevent the listView.SelectedItems from being cleared
            //  before this call, we have to expect the worst.  Note: this was added because I was
            //  seeing exceptions being thrown on this line in rare cases
            ListViewItem lvi;
            try
            {
                lvi = (ListViewItem)listView.SelectedItems[0];
            }
            catch
            {
                return;
            }

            if (lvi.Tag is Venue)
            {
                if (((Venue)lvi.Tag).VenueData.VenueType == VenueType.Invalid)
                {
                    RtlAwareMessageBox.Show(this, Strings.VenueJoinErrorText, Strings.VenueJoinErrorTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                }
                else
                {
                    this.DisplayStatusInProgress(Strings.JoiningVenue);
                    JoinVenue((Venue)lvi.Tag, true);
                }
            }
        }

        #endregion

        #region Status Bar Utilities
        /// <summary>Keeps something moving on the UI while a task is occuring, to prevent 
        /// the user from expecting the application is hung</summary>
        private void DisplayStatusInProgress(string message)
        {
            statusBarTimer.Stop();
            messagePanel.Text = message;
            statusBar.Font = new Font(statusBar.Font, FontStyle.Bold);
            statusBarTimer.Interval = 750;
            statusBarTimer.Start();
        }

        private void DisplayParticipantCount()
        {
            statusBarTimer.Stop();

            // Attempt to check and update participant count.
            // Throw away exception if Conference.ActiveVenue becomes null (i.e. leaving venue)
            try
            {
                int numParticipants = Conference.Participants.Length;
                DisplayStatusMessage(string.Format(CultureInfo.CurrentCulture, Strings.Participants, 
                    numParticipants));
            }
            catch { };

            statusBar.Font = new Font(statusBar.Font, FontStyle.Regular);
        }

        // Displays static status
        private void DisplayStatusMessage(string message)
        {
            statusBarTimer.Stop();
            statusBar.Font = new Font(statusBar.Font, FontStyle.Regular);
            messagePanel.Text = message;
        }

        private void DisplayVenueStatus()
        {
            statusBarTimer.Stop();
            statusBar.Font = new Font(statusBar.Font, FontStyle.Regular);

            if (Conference.ReflectorEnabled)
            {
                messagePanel.Text = string.Format(CultureInfo.CurrentCulture, Strings.ReflectorEnabled, 
                    Conference.ReflectorAddress);
            }
            else
            {
                messagePanel.Text = string.Format(CultureInfo.CurrentCulture, Strings.VenueServiceBaseURL, 
                    VenueServiceBaseUrl);
            }
        }

        private void statusBarTimer_Tick(object sender, EventArgs e)
        {
            string text = messagePanel.Text;
            if (text.EndsWith("...."))
                messagePanel.Text = text.Remove(text.Length - 3, 3);
            else
                messagePanel.Text += ".";
        }
        #endregion

        #region Internal

        /// <summary>
        /// Allows the UI to get the archiver state.
        /// </summary>
        /// <example>
        /// Used when the user move the play back slider to ensure
        /// we jump in the archive only if the archive is still playing
        /// </example>
        internal ArchiverState GetArchiverState
        {
            get
            {
                return archiverState;
            }
        }

        /// <summary>
        /// Start a new recording of a conference.
        /// </summary>
        /// <param name="ConferenceName">Name of the recorded conference</param>
        internal void StartRecording(string ConferenceName)
        {

            // For now, we disallow recording of encrypted venues
            Venue venue = Conference.ActiveVenue as Venue;
            if (venue == null)
                return; // shouldn't happen

            if (venue.PWStatus == PasswordStatus.STRONG_PASSWORD)
            {
                RtlAwareMessageBox.Show(this, Strings.NoArchivingEncryptedVenues,
                                Strings.RecordingError, MessageBoxButtons.OK, MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                return;
            }


            if (this.archiverState == ArchiverState.Stopped)
            {
                GetNewArchiver();
                archiver.Record(ConferenceName, Conference.ActiveVenue.Name, Conference.ActiveVenue.EndPoint);
                this.archiverState = ArchiverState.Recording;
            }

            SetArchiverMenuStatus();
        }

        /// <summary>
        /// Stop the current recording.
        /// </summary>
        internal void StopRecording()
        {
            try
            {
                if (this.archiverState == ArchiverState.Recording)
                {
                    if (archiver != null && Conference.ActiveVenue != null)
                    {
                        int refs = archiver.StopRecording(Conference.ActiveVenue.EndPoint);
                        archiver = null;

                        if (refs > 0)
                        {
                            RtlAwareMessageBox.Show(this, Strings.RecordingNotStoppedText,
                                Strings.RecordingNotStoppedTitle, MessageBoxButtons.OK, MessageBoxIcon.None,
                                MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                        }
                        else if (refs == 0)
                        {
                            RtlAwareMessageBox.Show(this, Strings.RecordingStoppedText, Strings.RecordingStoppedTitle,
                                MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1,
                                (MessageBoxOptions)0);
                        }
                        else // refs < 0 (i.e. error)
                        {
                            RtlAwareMessageBox.Show(this, Strings.RecordingStoppedPrematurelyText,
                                Strings.RecordingStoppedPrematurelyTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                        }
                    }

                    this.archiverState = ArchiverState.Stopped;
                }
            }
            catch (Exception ex)
            {
                archiverState = ArchiverState.Stopped;

                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                    Strings.ArchiveServiceErrorText, ex.ToString()), Strings.ArchiveServiceErrorTitle, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 
                    (MessageBoxOptions)0);
            }

            SetArchiverMenuStatus();
        }

        /// <summary>
        /// Play back an archive in the appropriate venue
        /// </summary>
        /// <param name="Streams">Streams ID to play back</param>
        /// <param name="selectedConference">Selected Conference</param>
        /// <returns>The IP adress where the archive is played back</returns>
        internal System.Net.IPEndPoint PlayArchive(int[] streams, ArchiveService.Conference selectedConference)
        {
            // TODO: Move PlayArchive out of BarUI

            // IP adresse where the archive is played back
            System.Net.IPEndPoint archivePlayBackIP = null;

            // TODO: Make sure we cannot get to an infinite loop by removing test on sender
            // if ( sender != this && (this.archiverState == ArchiverState.Stopped) )

            if (this.archiverState == ArchiverState.Stopped)
            {
                if (Conference.ActiveVenue == null) // Join a venue to playback to (generally unicast)
                {
                    // We've established a venue to join to, so do it
                    JoinVenue(GetPlayBackVenue(), false);
                }

                // We're definitely in a venue now
                Debug.Assert(Conference.ActiveVenue != null);

                // We're in a venue - playback to it
                if (MSR.LST.Net.Utility.IsMulticast(Conference.ActiveVenue.EndPoint))
                {
                    // Multicast playback - send directly to the multicast group
                    archivePlayBackIP = Conference.ActiveVenue.EndPoint;
                    archiver.Play(Conference.ActiveVenue.EndPoint, streams);
                }
                else
                {
                    // Unicast playback - send data directly to the local IP
                    archivePlayBackIP = new IPEndPoint(Conference.RtpSession.MulticastInterface,
                        Conference.ActiveVenue.EndPoint.Port);
                    archiver.Play(archivePlayBackIP, streams);
                }

                DisplayPlayBackInfo(selectedConference);

                this.archiverState = ArchiverState.Playing;
            }

            SetArchiverMenuStatus();
            return archivePlayBackIP;
        }

        /// <summary>
        /// Stop the playback.
        /// </summary>
        internal void StopPlayBack()
        {
            if (archiver != null && Conference.ActiveVenue != null)
            {
                IPEndPoint remoteVenue; // find the remote venue, which is different from out venue in unicast playback
                if (MSR.LST.Net.Utility.IsMulticast(Conference.ActiveVenue.EndPoint))
                {
                    remoteVenue = Conference.ActiveVenue.EndPoint;
                }
                else
                {
                    remoteVenue = new IPEndPoint(Conference.RtpSession.MulticastInterface,
                        Conference.ActiveVenue.EndPoint.Port);
                }

                archiver.StopPlaying(remoteVenue);
                archiver = null;
            }

            this.archiverState = ArchiverState.Stopped;

            // Reset the status message to participants
            this.DisplayParticipantCount();
        }

        #endregion Internal

    }
}
