using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.Win32;
using System.Collections.Specialized;
using System.Net;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// Provides a UI for editing the services locations & stores them to the registry.
    /// </summary>
    public class frmServices : System.Windows.Forms.Form
    {
        #region Private Vars
        public static bool autoSendAudio = true;
        public static bool autoSendVideo = true;
        private RegistryKey venuesRegKey = null;
        private RegistryKey archiversRegKey = null;
        private RegistryKey reflectorsRegKey = null;
        private RegistryKey diagnosticsRegKey = null;
        private bool bInitService = false;
        #endregion

        #region Private Winform Vars
        private System.Windows.Forms.GroupBox grpVenueService;
        private System.Windows.Forms.GroupBox grpArchiveService;
        private System.Windows.Forms.GroupBox grpReflector;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnReflectorServices;
        private System.Windows.Forms.Button btnArchiveServices;
        private System.Windows.Forms.Button btnVenueServices;
        private System.Windows.Forms.ComboBox cmbVenueServices;
        private System.Windows.Forms.ComboBox cmbArchiveServices;
        private System.Windows.Forms.ComboBox cmbReflectorServices;
        private System.Windows.Forms.CheckBox chkEnableArchive;
        private System.Windows.Forms.CheckBox chkEnableReflector;
        private GroupBox grpDiagnostics;
        private ComboBox cmbDiagnosticsServices;
        private CheckBox chkEnableDiagnostics;
        private Button btnDiagnosticsServices;
        private System.ComponentModel.Container components = null;
        #endregion

        #region Ctor, Dispose
        public frmServices(RegistryKey vkey, RegistryKey akey, RegistryKey rkey, RegistryKey dkey)
        {
            InitializeComponent();

            venuesRegKey = vkey;
            archiversRegKey = akey;
            reflectorsRegKey = rkey;
            diagnosticsRegKey = dkey;

            InitService(venuesRegKey, this.cmbVenueServices, null);                 
            InitService(archiversRegKey, this.cmbArchiveServices, this.chkEnableArchive);
            InitService(reflectorsRegKey, this.cmbReflectorServices, this.chkEnableReflector);
            InitService(diagnosticsRegKey, this.cmbDiagnosticsServices, this.chkEnableDiagnostics);
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
            this.grpVenueService = new System.Windows.Forms.GroupBox();
            this.btnVenueServices = new System.Windows.Forms.Button();
            this.cmbVenueServices = new System.Windows.Forms.ComboBox();
            this.grpArchiveService = new System.Windows.Forms.GroupBox();
            this.btnArchiveServices = new System.Windows.Forms.Button();
            this.cmbArchiveServices = new System.Windows.Forms.ComboBox();
            this.chkEnableArchive = new System.Windows.Forms.CheckBox();
            this.grpReflector = new System.Windows.Forms.GroupBox();
            this.cmbReflectorServices = new System.Windows.Forms.ComboBox();
            this.chkEnableReflector = new System.Windows.Forms.CheckBox();
            this.btnReflectorServices = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.grpDiagnostics = new System.Windows.Forms.GroupBox();
            this.cmbDiagnosticsServices = new System.Windows.Forms.ComboBox();
            this.chkEnableDiagnostics = new System.Windows.Forms.CheckBox();
            this.btnDiagnosticsServices = new System.Windows.Forms.Button();
            this.grpVenueService.SuspendLayout();
            this.grpArchiveService.SuspendLayout();
            this.grpReflector.SuspendLayout();
            this.grpDiagnostics.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpVenueService
            // 
            this.grpVenueService.Controls.Add(this.btnVenueServices);
            this.grpVenueService.Controls.Add(this.cmbVenueServices);
            this.grpVenueService.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpVenueService.Location = new System.Drawing.Point(8, 8);
            this.grpVenueService.Name = "grpVenueService";
            this.grpVenueService.Size = new System.Drawing.Size(368, 80);
            this.grpVenueService.TabIndex = 0;
            this.grpVenueService.TabStop = false;
            this.grpVenueService.Text = "Venue Service";
            // 
            // btnVenueServices
            // 
            this.btnVenueServices.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnVenueServices.Location = new System.Drawing.Point(190, 48);
            this.btnVenueServices.Name = "btnVenueServices";
            this.btnVenueServices.Size = new System.Drawing.Size(170, 23);
            this.btnVenueServices.TabIndex = 70;
            this.btnVenueServices.Text = "Configure Venue Services...";
            this.btnVenueServices.Click += new System.EventHandler(this.btnVenueServices_Click);
            // 
            // cmbVenueServices
            // 
            this.cmbVenueServices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVenueServices.Location = new System.Drawing.Point(8, 16);
            this.cmbVenueServices.Name = "cmbVenueServices";
            this.cmbVenueServices.Size = new System.Drawing.Size(352, 21);
            this.cmbVenueServices.TabIndex = 29;
            this.cmbVenueServices.SelectedIndexChanged += new System.EventHandler(this.cmbVenueServices_SelectedIndexChanged);
            // 
            // grpArchiveService
            // 
            this.grpArchiveService.Controls.Add(this.btnArchiveServices);
            this.grpArchiveService.Controls.Add(this.cmbArchiveServices);
            this.grpArchiveService.Controls.Add(this.chkEnableArchive);
            this.grpArchiveService.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpArchiveService.Location = new System.Drawing.Point(8, 104);
            this.grpArchiveService.Name = "grpArchiveService";
            this.grpArchiveService.Size = new System.Drawing.Size(368, 80);
            this.grpArchiveService.TabIndex = 1;
            this.grpArchiveService.TabStop = false;
            this.grpArchiveService.Text = "Archive Service";
            // 
            // btnArchiveServices
            // 
            this.btnArchiveServices.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnArchiveServices.Location = new System.Drawing.Point(190, 48);
            this.btnArchiveServices.Name = "btnArchiveServices";
            this.btnArchiveServices.Size = new System.Drawing.Size(170, 23);
            this.btnArchiveServices.TabIndex = 69;
            this.btnArchiveServices.Text = "Configure Archive Services...";
            this.btnArchiveServices.Click += new System.EventHandler(this.btnArchiveServices_Click);
            // 
            // cmbArchiveServices
            // 
            this.cmbArchiveServices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbArchiveServices.Enabled = false;
            this.cmbArchiveServices.Location = new System.Drawing.Point(8, 16);
            this.cmbArchiveServices.Name = "cmbArchiveServices";
            this.cmbArchiveServices.Size = new System.Drawing.Size(352, 21);
            this.cmbArchiveServices.TabIndex = 31;
            this.cmbArchiveServices.SelectedIndexChanged += new System.EventHandler(this.cmbArchiveServices_SelectedIndexChanged);
            // 
            // chkEnableArchive
            // 
            this.chkEnableArchive.Enabled = false;
            this.chkEnableArchive.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkEnableArchive.Location = new System.Drawing.Point(8, 40);
            this.chkEnableArchive.Name = "chkEnableArchive";
            this.chkEnableArchive.Size = new System.Drawing.Size(176, 24);
            this.chkEnableArchive.TabIndex = 30;
            this.chkEnableArchive.Text = "Enable &Archive Service";
            this.chkEnableArchive.CheckedChanged += new System.EventHandler(this.chkEnableArchive_CheckedChanged);
            // 
            // grpReflector
            // 
            this.grpReflector.Controls.Add(this.cmbReflectorServices);
            this.grpReflector.Controls.Add(this.chkEnableReflector);
            this.grpReflector.Controls.Add(this.btnReflectorServices);
            this.grpReflector.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpReflector.Location = new System.Drawing.Point(8, 192);
            this.grpReflector.Name = "grpReflector";
            this.grpReflector.Size = new System.Drawing.Size(368, 80);
            this.grpReflector.TabIndex = 2;
            this.grpReflector.TabStop = false;
            this.grpReflector.Text = "Reflector Service";
            // 
            // cmbReflectorServices
            // 
            this.cmbReflectorServices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReflectorServices.Enabled = false;
            this.cmbReflectorServices.Location = new System.Drawing.Point(8, 16);
            this.cmbReflectorServices.Name = "cmbReflectorServices";
            this.cmbReflectorServices.Size = new System.Drawing.Size(352, 21);
            this.cmbReflectorServices.TabIndex = 31;
            this.cmbReflectorServices.SelectedIndexChanged += new System.EventHandler(this.cmbReflectorServices_SelectedIndexChanged);
            // 
            // chkEnableReflector
            // 
            this.chkEnableReflector.Enabled = false;
            this.chkEnableReflector.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkEnableReflector.Location = new System.Drawing.Point(8, 48);
            this.chkEnableReflector.Name = "chkEnableReflector";
            this.chkEnableReflector.Size = new System.Drawing.Size(160, 24);
            this.chkEnableReflector.TabIndex = 32;
            this.chkEnableReflector.Text = "Enable &Reflector Service";
            this.chkEnableReflector.CheckedChanged += new System.EventHandler(this.chkEnableReflector_CheckedChanged);
            // 
            // btnReflectorServices
            // 
            this.btnReflectorServices.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnReflectorServices.Location = new System.Drawing.Point(190, 48);
            this.btnReflectorServices.Name = "btnReflectorServices";
            this.btnReflectorServices.Size = new System.Drawing.Size(170, 23);
            this.btnReflectorServices.TabIndex = 68;
            this.btnReflectorServices.Text = "Configure Reflector Services...";
            this.btnReflectorServices.Click += new System.EventHandler(this.btnReflectorServices_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(141, 378);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 23);
            this.btnOK.TabIndex = 38;
            this.btnOK.Text = "Close";
            // 
            // grpDiagnostics
            // 
            this.grpDiagnostics.Controls.Add(this.cmbDiagnosticsServices);
            this.grpDiagnostics.Controls.Add(this.chkEnableDiagnostics);
            this.grpDiagnostics.Controls.Add(this.btnDiagnosticsServices);
            this.grpDiagnostics.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpDiagnostics.Location = new System.Drawing.Point(8, 287);
            this.grpDiagnostics.Name = "grpDiagnostics";
            this.grpDiagnostics.Size = new System.Drawing.Size(368, 80);
            this.grpDiagnostics.TabIndex = 39;
            this.grpDiagnostics.TabStop = false;
            this.grpDiagnostics.Text = "Diagnostics Service";
            // 
            // cmbDiagnosticsServices
            // 
            this.cmbDiagnosticsServices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDiagnosticsServices.Enabled = false;
            this.cmbDiagnosticsServices.Location = new System.Drawing.Point(8, 16);
            this.cmbDiagnosticsServices.Name = "cmbDiagnosticsServices";
            this.cmbDiagnosticsServices.Size = new System.Drawing.Size(352, 21);
            this.cmbDiagnosticsServices.TabIndex = 31;
            this.cmbDiagnosticsServices.SelectedIndexChanged += new System.EventHandler(this.cmbDiagnosticsServices_SelectedIndexChanged);
            // 
            // chkEnableDiagnostics
            // 
            this.chkEnableDiagnostics.Enabled = false;
            this.chkEnableDiagnostics.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkEnableDiagnostics.Location = new System.Drawing.Point(8, 48);
            this.chkEnableDiagnostics.Name = "chkEnableDiagnostics";
            this.chkEnableDiagnostics.Size = new System.Drawing.Size(160, 24);
            this.chkEnableDiagnostics.TabIndex = 32;
            this.chkEnableDiagnostics.Text = "Enable &Diagnostic Service";
            this.chkEnableDiagnostics.CheckedChanged += new System.EventHandler(this.chkEnableDiagnostics_CheckedChanged);
            // 
            // btnDiagnosticsServices
            // 
            this.btnDiagnosticsServices.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDiagnosticsServices.Location = new System.Drawing.Point(190, 48);
            this.btnDiagnosticsServices.Name = "btnDiagnosticsServices";
            this.btnDiagnosticsServices.Size = new System.Drawing.Size(170, 23);
            this.btnDiagnosticsServices.TabIndex = 68;
            this.btnDiagnosticsServices.Text = "Configure Diagnostics Services...";
            this.btnDiagnosticsServices.Click += new System.EventHandler(this.btnDiagnosticsServices_Click);
            // 
            // frmServices
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(386, 413);
            this.ControlBox = false;
            this.Controls.Add(this.grpDiagnostics);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grpReflector);
            this.Controls.Add(this.grpArchiveService);
            this.Controls.Add(this.grpVenueService);
            this.Font = UIFont.FormFont;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmServices";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ConferenceXP Services";
            this.Load += new System.EventHandler(this.frmServices_Load);
            this.grpVenueService.ResumeLayout(false);
            this.grpArchiveService.ResumeLayout(false);
            this.grpReflector.ResumeLayout(false);
            this.grpDiagnostics.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Private Helper Methods
        /// <summary>
        /// Populates the comboBox and enableCheckBox based on the data in the registryKey provided.
        /// </summary>
        /// <param name="regkey">The registry key where the URLs for this service are stored.</param>
        private void InitService(RegistryKey regkey, 
            System.Windows.Forms.ComboBox serviceComboBox,
            System.Windows.Forms.CheckBox enableCheckBox)
        {
            // set a flag to prevent checkbox events from firing when we are in this method
            bInitService = true;
            serviceComboBox.Text = string.Empty;
            serviceComboBox.Items.Clear();

            if (regkey.ValueCount == 0)
            {
                serviceComboBox.Enabled = false;
                if (enableCheckBox != null)
                {
                    enableCheckBox.Checked = false;
                    enableCheckBox.Enabled = false;
                }
            }
            else
            {
                // Enable checkbox
                if (enableCheckBox != null)
                {
                    enableCheckBox.Enabled = true;
                }

                // Get the list of services from the registry
                string[] names = regkey.GetValueNames();
                bool bSelectedFound = false;
                foreach (string service in names)
                {
                    serviceComboBox.Items.Add(service);

                    // and if there is a selected service and update fields accordingly
                    object servicestate = regkey.GetValue(service);
                    if (Boolean.Parse((string) servicestate))
                    {
                        // We have a selected service, so enable the combo box and update the text field
                        serviceComboBox.Enabled = true; 
                        serviceComboBox.Text = service;
                        if (enableCheckBox != null)
                        {
                            enableCheckBox.Checked = true;
                        }
                        bSelectedFound = true;
                    }
                }

                // If no selected service was found during initialization, 
                // display the 1st one listed with unchecked checkbox
                if (!bSelectedFound) 
                {
                    serviceComboBox.Text = names[0];

                    if (enableCheckBox != null)
                    {
                        serviceComboBox.Enabled = false;
                        enableCheckBox.Checked = false;
                    }
                    else
                    {
                        // This service can't be disabled, so auto-set a default service host to use
                        EnableService(regkey, serviceComboBox);
                    }
                }
            }
            bInitService = false;
        }

        private void EnableService(RegistryKey regkey, System.Windows.Forms.ComboBox serviceComboBox)
        {
            // Combo list has already been populated, so just enable           
            serviceComboBox.Enabled = true;

            // Check if the service displayed in the text is in fact the selected one, and set it if it is not
            object servicestate = regkey.GetValue(serviceComboBox.Text);
            if (!Boolean.Parse((string) servicestate))
            { 
                regkey.SetValue(serviceComboBox.Text, "True");
            }
        }
      
        private void ChangeService(RegistryKey regkey, 
            System.Windows.Forms.ComboBox serviceComboBox,
            System.Windows.Forms.CheckBox enableCheckBox)            
        {
            if (enableCheckBox == null  || enableCheckBox.Checked)
            {
                string[] names = regkey.GetValueNames();
                foreach (string service in names)
                {
                    regkey.SetValue(service, (service == serviceComboBox.Text).ToString());
                }
            }
        }

        private void DisableService(RegistryKey regkey, 
            System.Windows.Forms.ComboBox serviceComboBox,
            System.Windows.Forms.CheckBox enableCheckBox)
        {
            serviceComboBox.Enabled = false; 

            if (regkey.ValueCount > 0)
            {
                string[] names = regkey.GetValueNames(); 
                serviceComboBox.Text = names[0];
                foreach (string service in names)
                {     
                    regkey.SetValue(service, "False");
                }
                regkey.Flush();
            }
        }
        #endregion

        #region UI Event Handlers
        private void btnVenueServices_Click(object sender, System.EventArgs e)
        {
            frmServices2 services = new frmServices2(ServicesUIText.VenueService, venuesRegKey);
            if (services.ShowDialog() == DialogResult.OK) 
            {
                InitService(venuesRegKey, this.cmbVenueServices, null);
            }            
        }

        private void btnArchiveServices_Click(object sender, System.EventArgs e)
        {
            // If the service is disabled, but there are entries in the list, note that fact
            bool explicitlyDisabled = (!chkEnableArchive.Checked) && (cmbArchiveServices.Items.Count > 0);

            frmServices2 services = new frmServices2(ServicesUIText.ArchiveService, archiversRegKey);
            if (services.ShowDialog() == DialogResult.OK) 
            {
                InitService(archiversRegKey, this.cmbArchiveServices, this.chkEnableArchive);
                
                // If the service was not explictly disabled, and now there's one entry in the list, it should be auto-enabled
                if( !explicitlyDisabled && cmbArchiveServices.Items.Count == 1 )
                {
                    cmbArchiveServices.SelectedIndex = 0;
                    chkEnableArchive.Checked = true;
                }
            }
        }

        private void btnReflectorServices_Click(object sender, System.EventArgs e)
        {
            // If the service is disabled, but there are entries in the list, note that fact
            bool explicitlyDisabled = (!chkEnableReflector.Checked) && (cmbReflectorServices.Items.Count > 0);

            frmServices2 services = new frmServices2(ServicesUIText.ReflectorService, reflectorsRegKey);
            if (services.ShowDialog() == DialogResult.OK) 
            {
                InitService(reflectorsRegKey, this.cmbReflectorServices, this.chkEnableReflector);

                // If the service was not explictly disabled, and now there's one entry in the list, it should be auto-enabled
                if( !explicitlyDisabled && cmbReflectorServices.Items.Count == 1 )
                {
                    cmbReflectorServices.SelectedIndex = 0;
                    chkEnableReflector.Checked = true;
                }
            }
        }

        private void chkEnableArchive_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!bInitService)
            {
                if (chkEnableArchive.Checked == false)
                {
                    DisableService(archiversRegKey, this.cmbArchiveServices, this.chkEnableArchive);             
                }
                else
                {                
                    EnableService(archiversRegKey, this.cmbArchiveServices);
                }
            }                        
        }

        private void chkEnableReflector_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!bInitService)
            {
                if (chkEnableReflector.Checked == false)
                {
                    DisableService(reflectorsRegKey, this.cmbReflectorServices, this.chkEnableReflector);
                }
                else
                {
                    EnableService(reflectorsRegKey, this.cmbReflectorServices);
                }
            }
        }

        private void cmbVenueServices_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (!bInitService)
            {
                ChangeService(venuesRegKey, this.cmbVenueServices, null);
            }
        }

        private void cmbArchiveServices_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (!bInitService)
            {                    
                ChangeService(archiversRegKey, this.cmbArchiveServices, this.chkEnableArchive);
            }
        }

        private void cmbReflectorServices_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (!bInitService)
            {
                ChangeService(reflectorsRegKey, this.cmbReflectorServices, this.chkEnableReflector);
            }
        }

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(FMain.helpurlServices);
        }

        private void chkEnableDiagnostics_CheckedChanged(object sender, EventArgs e) {
            if (!bInitService) {
                if (chkEnableDiagnostics.Checked == false) {
                    DisableService(diagnosticsRegKey, this.cmbDiagnosticsServices, this.chkEnableDiagnostics);
                }
                else {
                    EnableService(diagnosticsRegKey, this.cmbDiagnosticsServices);
                }
            }

        }

        private void btnDiagnosticsServices_Click(object sender, EventArgs e) {
            // If the service is disabled, but there are entries in the list, note that fact
            bool explicitlyDisabled = (!chkEnableDiagnostics.Checked) && (cmbDiagnosticsServices.Items.Count > 0);

            frmServices2 services = new frmServices2(ServicesUIText.DiagnosticsService, diagnosticsRegKey);
            if (services.ShowDialog() == DialogResult.OK) {
                InitService(diagnosticsRegKey, this.cmbDiagnosticsServices, this.chkEnableDiagnostics);

                // If the service was not explictly disabled, and now there's one entry in the list, it should be auto-enabled
                if (!explicitlyDisabled && cmbDiagnosticsServices.Items.Count == 1) {
                    cmbDiagnosticsServices.SelectedIndex = 0;
                    chkEnableDiagnostics.Checked = true;
                }
            }

        }

        private void cmbDiagnosticsServices_SelectedIndexChanged(object sender, EventArgs e) {
            if (!bInitService) {
                ChangeService(diagnosticsRegKey, this.cmbDiagnosticsServices, this.chkEnableDiagnostics);
            }
        }

        #endregion

        #region Internal UI Text
        /// <summary>
        /// Contains a static enumeration of all the information needed for the services UI.
        /// </summary>
        internal sealed class ServicesUIText
        {
            readonly internal string ServiceName;
            readonly internal string ServiceNames;
            readonly internal string ExampleText;
            readonly internal string ConnectionType;
            readonly internal string MoreInfoUrl;

            static readonly internal ServicesUIText ArchiveService;
            static readonly internal ServicesUIText VenueService;
            static readonly internal ServicesUIText ReflectorService;
            static readonly internal ServicesUIText DiagnosticsService;

            internal string ConfigText
            { get { return Strings.Configure + ServiceNames; } }
            internal string AddServiceText
            { get { return ServiceName + ConnectionType + Strings.Colon; } }
            internal string MyServicesText
            { get { return Strings.My + ServiceNames + Strings.Colon; } }

            private ServicesUIText(string name, string names, string type, string example, string infoUrl) 
            {
                this.ServiceName = name;
                this.ServiceNames = names;
                this.ExampleText = example;
                this.ConnectionType = type;
                this.MoreInfoUrl = infoUrl;
            }

            static ServicesUIText()
            {
                ArchiveService = new ServicesUIText(Strings.ArchiveService, Strings.ArchiveServices, Strings.HostNameOrIPAddress, 
                    Strings.ExampleArchiveHostNameIP, Strings.HelpURLArchiveInfo);
                VenueService = new ServicesUIText(Strings.Venue_Service, Strings.Venue_Services, Strings.URL, Strings.ExampleURL,
                    Strings.HelpURLVenuesInfo);
                ReflectorService = new ServicesUIText(Strings.ReflectorService, Strings.ReflectorServices, Strings.HostNameOrIPAddress,
                    Strings.ExampleReflectorHostNameIP, Strings.HelpURLReflectorInfo);
                DiagnosticsService = new ServicesUIText(Strings.DiagnosticService, Strings.DiagnosticServices, Strings.URL,
                    Strings.ExampleDiagnosticService, Strings.HelpURLDiagnosticInfo);
            }
        }
        #endregion

        private void frmServices_Load(object sender, EventArgs e)
        {
            this.grpVenueService.Font = UIFont.StringFont;
            this.grpArchiveService.Font = UIFont.StringFont;
            this.grpReflector.Font = UIFont.StringFont;
            this.btnOK.Font = UIFont.StringFont;
            this.grpDiagnostics.Font = UIFont.StringFont;

            this.grpVenueService.Text = Strings.Venue_Service;
            this.btnVenueServices.Text = Strings.ConfigureVenueServices;
            this.grpArchiveService.Text = Strings.ArchiveService;
            this.btnArchiveServices.Text = Strings.ConfigureArchiveServicesEllipsis;
            this.chkEnableArchive.Text = Strings.EnableArchiveServiceHotkey;
            this.grpReflector.Text = Strings.ReflectorService;
            this.chkEnableReflector.Text = Strings.EnableReflectorServiceHotkey;
            this.btnReflectorServices.Text = Strings.ConfigureReflectorServices;
            this.btnOK.Text = Strings.Close;
            this.grpDiagnostics.Text = Strings.DiagnosticService;
            this.chkEnableDiagnostics.Text = Strings.EnableDiagnosticServiceHotkey;
            this.btnDiagnosticsServices.Text = Strings.ConfigureDiagnosticServices;
            //this.linkLabel1.Text = Strings.ViewHostedServices;
            this.Text = Strings.CXPServices;
        }

    }
}
