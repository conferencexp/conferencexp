using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP.VenueService
{
    internal class VenueManager : MSR.LST.ConferenceXP.VenueService.ItemManager
    {
        #region Private Members
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// The index in the image list of the default venue icon
        /// </summary>
        private int defaultIconIndex;
        #endregion

        #region Ctor, Dispose
        public VenueManager()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Like Windows Explorer, we show the icon in the first row, and accomodate it with extra space
            base.list.Columns.Add(Strings.Name, 120+images.ImageSize.Width, HorizontalAlignment.Left);
            base.list.Columns.Add(Strings.Identifier, 150, HorizontalAlignment.Left);
            base.list.Columns.Add(Strings.IPAddress, 96, HorizontalAlignment.Left);
            base.list.Columns.Add(Strings.Port, 38, HorizontalAlignment.Left);

            // Store which columns we want to make fixed-width
            base.VariableWidthColumns = new int[]{0,1}; // Name & Identifier

            // And now make sure we're using up all of our column space
            base.ResizeColumns();

            // Don't allow refresh on this form
            base.refreshBtn.Visible = false;

            // Hook the button events
            base.editBtn.Click += new EventHandler(editBtn_Click);
            base.newBtn.Click += new EventHandler(newBtn_Click);
            base.deleteBtn.Click += new EventHandler(deleteBtn_Click);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (components != null) 
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }
        #endregion

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        #region Private Methods
        override protected void RefreshList()
        {
            VenueState[] venueStates = StorageFile.GetVenuesComplete();
//            Venue[] venues = StorageFile.GetVenues();

            images.Images.Clear();
            list.Items.Clear();

            // Add the default venue icon at spot [0]
            defaultIconIndex = images.Images.Add(VenueEditor.defaultVenueIcon, images.TransparentColor);

            foreach(VenueState venueState in venueStates)
            {
                int imageIndex = AddVenueIcon (venueState.Venue.Icon);
                list.Items.Add (CreateLvi(venueState,imageIndex));
            }
        }

        private ListViewItem CreateLvi(VenueState venueState, int imageIndex)
        {
            Venue venue = venueState.Venue;

            string[] columns = new string[]{venue.Name, venue.Identifier, venue.IPAddress.ToString(), 
                venue.Port.ToString(CultureInfo.InvariantCulture)};
            ListViewItem item = new ListViewItem(columns);
            item.ImageIndex = imageIndex;
            item.Tag = venueState;

            return item;
        }

        /// <summary>
        /// Takes a venue icon as a byte[], adds it to the images list, and returns its new index in the list.
        /// </summary>
        private int AddVenueIcon(byte[] venueIcon)
        {
            Image icon = IconUtilities.BytesToImage (venueIcon);
            int imageIndex;
            if( icon == null )
                imageIndex = this.defaultIconIndex;
            else
                imageIndex = images.Images.Add(icon, images.TransparentColor);

            return imageIndex;
        }
        #endregion

        #region Button Events
        private void editBtn_Click(object sender, EventArgs e)
        {
            VenueState selectedVenueState = (VenueState)list.SelectedItems[0].Tag;
            VenueEditor editor = new VenueEditor(selectedVenueState);
            editor.Text = Strings.EditVenue;
            editor.Closing += new CancelEventHandler(CheckForDupIP);
            editor.Closing += new CancelEventHandler(CheckForDupID);
            DialogResult dr = editor.ShowDialog();

            if (dr == DialogResult.OK)
            {
                VenueState newVenueState = editor.GetVenueState();
                
                if (newVenueState.Identifier != selectedVenueState.Identifier)
                {
                    // The venue identifier was edited, so we need to completely delete & re-add this venue
                    StorageFile.DeleteVenue(selectedVenueState.Identifier);
                    StorageFile.AddVenue(newVenueState);
                }
                else
                {
                    StorageFile.UpdateVenue(newVenueState);
                }

                // Don't remove the image; it will screw up the images for all of the other venues
                // But we will check to see if the image has changed (if not, just use the old imageIndex)
                int imageIndex;
                if (newVenueState.Venue.Icon == selectedVenueState.Venue.Icon)
                    imageIndex = list.SelectedItems[0].ImageIndex;
                else
                    imageIndex = AddVenueIcon(newVenueState.Venue.Icon);

                // Remove the old item
                list.Items.RemoveAt(list.SelectedIndices[0]);

                // Create and add the new item
                list.Items.Add (CreateLvi(newVenueState, imageIndex));
            }
        }

        private void newBtn_Click(object sender, EventArgs e)
        {
            VenueEditor editor = new VenueEditor(new VenueState());
            editor.Text = Strings.NewVenue;
            editor.Closing += new CancelEventHandler(CheckForDupIP);
            editor.Closing += new CancelEventHandler(CheckForDupID);
            DialogResult dr = editor.ShowDialog();

            if (dr == DialogResult.OK)
            {
                // Get the new venue
                VenueState venueState = editor.GetVenueState();

                // Store it
                StorageFile.AddVenue(venueState);

                // Create the new LVI
                int imageIndex = AddVenueIcon(venueState.Venue.Icon);

                // Add it to the list
                list.Items.Add (CreateLvi(venueState, imageIndex));
            }
        }

        /// <summary>
        /// When the edit form is closing, check to make sure the IP is unique.
        /// </summary>
        private void CheckForDupIP(object sender, CancelEventArgs e)
        {
            VenueEditor editor = (VenueEditor)sender;

            if (editor.DialogResult != DialogResult.OK)
                return;

            Venue ven = editor.GetVenue();
            IPAddress ip = IPAddress.Parse(ven.IPAddress.Trim());

            // First check to see if the IP changed from the selected venue
            //  If it changed, we have to check against all of the venue IPs
            Venue original = editor.OriginalVenue;
            if (original.IPAddress == null || !IPAddress.Parse(original.IPAddress.Trim()).Equals(ip))
            {
                Venue dupIPVenue = null;

                // The IP has changed, so check to make sure it's not a dup
                foreach(ListViewItem item in list.Items)
                {
                    VenueState currentVenueState = (VenueState)item.Tag;
                    Venue currentVen = currentVenueState.Venue;
                    if (IPAddress.Parse(currentVen.IPAddress.Trim()).Equals(ip))
                    {
                        dupIPVenue = currentVen;
                        break;
                    }
                }

                // If the IP is a duplicate, show an error and prevent the dialog from closing
                if (dupIPVenue != null)
                {
                    RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                        Strings.DuplicateIPAddressText, dupIPVenue.Name), Strings.DuplicateIpAddressTitle, 
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 
                        (MessageBoxOptions)0);
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// When the edit form is closing, check to make sure the IP is unique.
        /// </summary>
        private void CheckForDupID(object sender, CancelEventArgs e)
        {
            VenueEditor editor = (VenueEditor)sender;

            if (editor.DialogResult != DialogResult.OK)
                return;

            Venue ven = editor.GetVenue();
            string id = ven.Identifier.ToLower(CultureInfo.InvariantCulture).Trim();

            // First check to see if the ID changed from the selected venue
            //  If it changed, we have to check against all of the venue IDs
            Venue original = editor.OriginalVenue;
            if (original.Identifier == null || original.Identifier.ToLower(CultureInfo.InvariantCulture).Trim() != id)
            {
                Venue dupIDVenue = null;

                // The IP has changed, so check to make sure it's not a dup
                foreach(ListViewItem item in list.Items)
                {
                    VenueState currentVenueState = (VenueState)item.Tag;
                    Venue currentVen = currentVenueState.Venue;

                    if (currentVen.Identifier.ToLower(CultureInfo.InvariantCulture).Trim() == id)
                    {
                        dupIDVenue = currentVen;
                        break;
                    }
                }

                // If the ID is a duplicate, show an error and prevent the dialog from closing
                if (dupIDVenue != null)
                {
                    RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                        Strings.DuplicateOwnerText, dupIDVenue.Name), Strings.DuplicateOwnerTitle, 
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 
                        (MessageBoxOptions)0);
                    e.Cancel = true;
                }
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            VenueState selectedVenueState = ((VenueState)list.SelectedItems[0].Tag);
            Venue selectedVenue = selectedVenueState.Venue;
            string removeStr = string.Format(CultureInfo.CurrentCulture, Strings.ConfirmVenueDeleteText, 
                selectedVenue.Name);
            DialogResult dr = RtlAwareMessageBox.Show(this, removeStr, Strings.ConfirmVenueDeleteTitle, 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, 
                (MessageBoxOptions)0);
            if (dr == DialogResult.Yes)
            {
                StorageFile.DeleteVenue(selectedVenue.Identifier);
                // don't remove the image; it will screw up the images for all of the other venues
                list.Items.RemoveAt(list.SelectedIndices[0]);
            }
        }
        #endregion

    }
}

