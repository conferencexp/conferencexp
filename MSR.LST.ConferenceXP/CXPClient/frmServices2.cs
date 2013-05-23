using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using Microsoft.Win32;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// Provides editing of a list of services.
    /// </summary>
    internal class frmServices2 : System.Windows.Forms.Form
    {
        #region Private Vars
        private ArrayList currentServices;
        private frmServices.ServicesUIText serviceText;
        private RegistryKey serviceRegKey = null;
        #endregion

        #region Private Winform Vars
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelServiceList;
        private System.Windows.Forms.Label lblAddService;
        private System.Windows.Forms.Label lblExample;
        private System.Windows.Forms.ListBox listBoxServices;
        private System.Windows.Forms.TextBox textAddService;
        private System.Windows.Forms.LinkLabel moreInfoLink;
        private System.ComponentModel.Container components = null;
        #endregion

        #region Ctor, Dispose
        public frmServices2(frmServices.ServicesUIText uiText, RegistryKey key)
        {
            InitializeComponent();

            this.serviceText = uiText;
            serviceRegKey = key; 
           
            this.Text = uiText.ConfigText;
            this.labelServiceList.Text = uiText.MyServicesText;
            this.lblAddService.Text = uiText.AddServiceText;
            this.lblExample.Text = uiText.ExampleText;
            
            // Add all of the existing service hosts - lower case (case insensitive)
            string[] serviceHosts = serviceRegKey.GetValueNames();
            currentServices = new ArrayList();
            foreach (string host in serviceHosts)
            {
                string lowerHost = host.ToLower(CultureInfo.CurrentCulture);
                currentServices.Add(lowerHost);
                listBoxServices.Items.Add(lowerHost);
            }
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
            this.labelServiceList = new System.Windows.Forms.Label();
            this.lblAddService = new System.Windows.Forms.Label();
            this.lblExample = new System.Windows.Forms.Label();
            this.listBoxServices = new System.Windows.Forms.ListBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.textAddService = new System.Windows.Forms.TextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.moreInfoLink = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // labelServiceList
            // 
            this.labelServiceList.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelServiceList.Location = new System.Drawing.Point(8, 80);
            this.labelServiceList.Name = "labelServiceList";
            this.labelServiceList.Size = new System.Drawing.Size(224, 16);
            this.labelServiceList.TabIndex = 33;
            this.labelServiceList.Text = "My Archive Services:";
            this.labelServiceList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAddService
            // 
            this.lblAddService.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblAddService.Location = new System.Drawing.Point(8, 8);
            this.lblAddService.Name = "lblAddService";
            this.lblAddService.Size = new System.Drawing.Size(312, 16);
            this.lblAddService.TabIndex = 34;
            this.lblAddService.Text = "Archive Service Host Name:";
            this.lblAddService.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblExample
            // 
            this.lblExample.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblExample.Location = new System.Drawing.Point(8, 48);
            this.lblExample.Name = "lblExample";
            this.lblExample.Size = new System.Drawing.Size(312, 32);
            this.lblExample.TabIndex = 35;
            this.lblExample.Text = "For example: my.archiveservice.com";
            this.lblExample.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listBoxServices
            // 
            this.listBoxServices.HorizontalScrollbar = true;
            this.listBoxServices.Location = new System.Drawing.Point(8, 96);
            this.listBoxServices.Name = "listBoxServices";
            this.listBoxServices.Size = new System.Drawing.Size(312, 121);
            this.listBoxServices.TabIndex = 8;
            this.listBoxServices.SelectedIndexChanged += new System.EventHandler(this.listBoxServices_SelectedIndexChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOK.Location = new System.Drawing.Point(128, 272);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(88, 23);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "&OK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Enabled = false;
            this.buttonDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonDelete.Location = new System.Drawing.Point(240, 224);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(80, 23);
            this.buttonDelete.TabIndex = 4;
            this.buttonDelete.Text = "&Delete";
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // textAddService
            // 
            this.textAddService.AcceptsReturn = true;
            this.textAddService.Location = new System.Drawing.Point(8, 24);
            this.textAddService.Name = "textAddService";
            this.textAddService.Size = new System.Drawing.Size(312, 20);
            this.textAddService.TabIndex = 1;
            this.textAddService.TextChanged += new System.EventHandler(this.textAddService_TextChanged);
            this.textAddService.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textAddService_KeyDown);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Enabled = false;
            this.buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonAdd.Location = new System.Drawing.Point(64, 224);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(80, 23);
            this.buttonAdd.TabIndex = 2;
            this.buttonAdd.Text = "&Add";
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Enabled = false;
            this.buttonEdit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonEdit.Location = new System.Drawing.Point(152, 224);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(80, 23);
            this.buttonEdit.TabIndex = 3;
            this.buttonEdit.Text = "&Replace";
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonCancel.Location = new System.Drawing.Point(232, 272);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(88, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // moreInfoLink
            // 
            this.moreInfoLink.Location = new System.Drawing.Point(216, 63);
            this.moreInfoLink.Name = "moreInfoLink";
            this.moreInfoLink.Size = new System.Drawing.Size(104, 16);
            this.moreInfoLink.TabIndex = 36;
            this.moreInfoLink.TabStop = true;
            this.moreInfoLink.Text = "More Information";
            this.moreInfoLink.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.moreInfoLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.moreInfoLink_LinkClicked);
            // 
            // frmServices2
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(330, 304);
            this.ControlBox = false;
            this.Controls.Add(this.moreInfoLink);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.textAddService);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.listBoxServices);
            this.Controls.Add(this.lblExample);
            this.Controls.Add(this.lblAddService);
            this.Controls.Add(this.labelServiceList);
            this.Font = UIFont.FormFont;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmServices2";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configure Archive Services";
            this.Load += new System.EventHandler(this.frmServices2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
     
        #region UI Event Handlers
        private void textAddService_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (buttonAdd.Enabled)
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                {
                    buttonAdd.PerformClick();
                }
            }
        }
        
        private void buttonAdd_Click(object sender, System.EventArgs e)
        {
            string key = textAddService.Text.ToLower(CultureInfo.CurrentCulture);

            if ( ValidateUriAndShowError(key) && ValidateUniqueHost(key))
            {
                listBoxServices.Items.Add(key);

                textAddService.Text = null;
            }
        }

        private void buttonEdit_Click(object sender, System.EventArgs e)
        {
            string oldService = listBoxServices.SelectedItem.ToString();
            string editedService = textAddService.Text.ToLower(CultureInfo.CurrentCulture);

            if ( ValidateUriAndShowError(editedService) && ValidateUniqueHost(editedService))
            {
                DialogResult dr = RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                    Strings.ConfirmReplaceServiceHost, oldService, editedService), Strings.ConfirmReplace,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, 
                    (MessageBoxOptions)0);

                if (dr == DialogResult.OK)
                {
                    listBoxServices.Items.Add(editedService);
                    listBoxServices.Items.Remove(oldService);

                    textAddService.Text = null;
                }
            }
        }

        private void buttonDelete_Click(object sender, System.EventArgs e)
        {
            string key = listBoxServices.SelectedItem.ToString();

            DialogResult dr = RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                Strings.ConfirmDeleteServiceHost, key), Strings.ConfirmDelete, MessageBoxButtons.OKCancel, 
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            if (dr == DialogResult.OK)
            {
                listBoxServices.Items.Remove(key);
            }
        }

        private void listBoxServices_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // While this is here to handle deselecting any items, interestingly, you can't deselect after you've selected...
            if (listBoxServices.SelectedItem != null)
            {
                textAddService.Text = listBoxServices.SelectedItem.ToString();

                buttonDelete.Enabled = true;
            }
            else
            {
                buttonDelete.Enabled = false;
                buttonEdit.Enabled = false;
            }
        }

        private void textAddService_TextChanged(object sender, System.EventArgs e)
        {
            if (textAddService.Text != String.Empty && textAddService.Text != null)
            {
                buttonAdd.Enabled = true;
                buttonEdit.Enabled = (this.listBoxServices.SelectedItem != null);
            }
            else // the string is empty or null
            {
                buttonAdd.Enabled = false;
                buttonEdit.Enabled = false;
            }
        }

        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            SaveHosts();
            this.Close();
        }

        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void moreInfoLink_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(serviceText.MoreInfoUrl);
        }
        #endregion

        #region Helper Methods
        private void SaveHosts()
        {
            // This 2*n^2 operation looks pretty nasty, but it's a lot better than the alternative.
            //  The other option is to check to make sure an item hasn't been deleted & later added, or
            //  added & later deleted, or added and then replaced, etc, etc.  It's just easier this way.

            // Add hosts that are in the new list but not the old
            foreach (string key in this.listBoxServices.Items)
            {
                if (!this.currentServices.Contains(key))
                    this.serviceRegKey.SetValue(key, "False");
            }

            // Delete hosts that are in the old list but not in the new
            foreach (string key in this.currentServices)
            {
                if (!this.listBoxServices.Items.Contains(key))
                    this.serviceRegKey.DeleteValue(key, false);
            }

            serviceRegKey.Flush();
        }

        private bool ValidateUniqueHost(string host)
        {
            if(this.listBoxServices.Items.Contains(host))
            {
                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, Strings.DuplicateHostNameText,
                    serviceText.ConnectionType), Strings.DuplicateHostNameTitle, MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                return false;
            }

            return true;
        }

        private bool ValidateUriAndShowError(string input)
        {
            input = input.ToLower(CultureInfo.CurrentCulture);
            Uri validatedUri = ValidateUri(input);
            bool valid = (validatedUri != null);

            if (valid)
            {
                System.Diagnostics.Debug.WriteLine(validatedUri.ToString());

                // The Reflector service does not accept a protocol scheme (e.g., http://) or folders (e.g., .../MyFolder/...).
                if (this.serviceText == frmServices.ServicesUIText.ReflectorService)
                {
                    if (input.IndexOf('/') >= 0)
                        valid = false;
                }
                    // The Archiver service does not accept a protocol scheme or IPv6 addresses
                else if (this.serviceText == frmServices.ServicesUIText.ArchiveService)
                {
                    if (input.IndexOf("://") >= 0 || validatedUri.Host.IndexOf('[') >= 0)
                        valid = false;
                }
                    // The Venue service only accepts a Uri begining with http:// or https://
                else if (this.serviceText == frmServices.ServicesUIText.VenueService)
                {
                    if ( !(input.StartsWith("http://") || input.StartsWith("https://")) )
                        valid = false;
                }
            }

            if (!valid)
            {
                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                    Strings.EnterValidConnectionType, serviceText.ConnectionType), 
                    string.Format(CultureInfo.CurrentCulture, Strings.InvalidConnectionType, 
                    serviceText.ConnectionType), MessageBoxButtons.OK, MessageBoxIcon.Warning, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }

            return valid;
        }

        /// <summary>
        /// Takes a user-inputted URI and attempts to validate it, accounting for what the user isn't
        /// required to enter (i.e. the protocol scheme and the IPv6 square brackets).
        /// </summary>
        /// <param name="input">A string to be converted to a Uri</param>
        /// <returns>Returns a Uri if the input could be converted to a valid Uri and null otherwise.</returns>
        public static Uri ValidateUri(string input)
        {
            string fixedUriStr = input;

            // Add the protocol scheme, if it's not present
            if (fixedUriStr.IndexOf("://") < 0)
            {
                fixedUriStr = "tcp://" + fixedUriStr;
            }

            // Try the new URI, and RETURN it if it's good.
            Uri possibleValidUri = TryUri(fixedUriStr);
            if (possibleValidUri != null)
                return possibleValidUri;

            // IPv6 addresses parsed by Uri require square brackets around the IP address,
            //  so try adding those here and see if it'll then parse.
            // NOTE: We can't parse IPv6 addresses w/o both ports && sq. brackets (and that's probly why Uri
            //  requires sq. brackets, so to specify a port, you have to enter the sq. brackets around the port
            bool hasSquareBrackets = ((fixedUriStr.IndexOf("[") >= 0) || (fixedUriStr.IndexOf("]") >= 0));
            if (!hasSquareBrackets)
            {
                int doubleSlashIndex = fixedUriStr.IndexOf("//");

                fixedUriStr = fixedUriStr.Insert(doubleSlashIndex + 2, "[");

                int thirdSlashIndex = fixedUriStr.IndexOf('/', doubleSlashIndex + 2);
                if (thirdSlashIndex < 0) 
                    thirdSlashIndex = fixedUriStr.Length;

                fixedUriStr = fixedUriStr.Insert(thirdSlashIndex, "]");
            }

            return TryUri(fixedUriStr);
        }

        private static Uri TryUri(string uriToTest)
        {
            Uri parsedUri;
            try
            {
                parsedUri = new Uri(uriToTest);
                return parsedUri;
            }
            catch (UriFormatException)
            {
                return null;
            }
        }
        #endregion

        private void frmServices2_Load(object sender, EventArgs e)
        {
            this.labelServiceList.Font = UIFont.StringFont;
            this.lblAddService.Font = UIFont.StringFont;
            this.lblExample.Font = UIFont.StringFont;
            this.listBoxServices.Font = UIFont.StringFont;
            this.buttonOK.Font = UIFont.StringFont;
            this.buttonDelete.Font = UIFont.StringFont;
            this.textAddService.Font = UIFont.StringFont;
            this.buttonAdd.Font = UIFont.StringFont;
            this.buttonEdit.Font = UIFont.StringFont;
            this.buttonCancel.Font = UIFont.StringFont;
            this.moreInfoLink.Font = UIFont.StringFont;

            this.buttonOK.Text = Strings.OKHotkey;
            this.buttonDelete.Text = Strings.DeleteHotkey;
            this.buttonAdd.Text = Strings.AddHotkey;
            this.buttonEdit.Text = Strings.ReplaceHotkey;
            this.buttonCancel.Text = Strings.CancelHotkey;
            this.moreInfoLink.Text = Strings.MoreInformation;
        }

    }
}
