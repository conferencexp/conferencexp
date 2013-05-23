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
    internal class ParticipantManager : MSR.LST.ConferenceXP.VenueService.ItemManager
    {
        #region Private Members
        private System.ComponentModel.IContainer components = null;

        private int defaultIconIndex;
        #endregion

        #region Ctor, Dispose
        public ParticipantManager()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Like Windows Explorer, we show the icon in the first row, and accomodate it with extra space
            base.list.Columns.Add(Strings.Name, 120 + images.ImageSize.Width, HorizontalAlignment.Left);
            base.list.Columns.Add(Strings.Identifier, 150, HorizontalAlignment.Left);
            base.list.Columns.Add(Strings.Email, 150, HorizontalAlignment.Left);

            // Store which columns we want to make fixed-width
            base.VariableWidthColumns = new int[]{0, 1, 2}; // Name & Identifier

            // And now make sure we're using up all of our column space
            base.ResizeColumns();

            // Hook the button events
            base.editBtn.Click += new EventHandler(editBtn_Click);
            base.deleteBtn.Click += new EventHandler(deleteBtn_Click);

            // Because it just doesn't make sense, we won't allow creating participants
            this.newBtn.Visible = false;
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
            Participant[] parts = StorageFile.GetParticipants();

            images.Images.Clear();
            list.Items.Clear();

            // Add the default participant icon at spot [0]
            defaultIconIndex = images.Images.Add(ParticipantEditor.defaultParticipantIcon, images.TransparentColor);

            // Becasue sorting is slow, we need to add the items all at once; do this by using AddRange
            ArrayList lvis = new ArrayList(parts.Length);
            foreach(Participant part in parts)
            {
                int imageIndex = AddIcon (part.Icon);
                lvis.Add (CreateLvi(part, imageIndex));
            }
            ListViewItem[] participantItems = (ListViewItem[])lvis.ToArray(typeof(ListViewItem));
            list.Items.AddRange(participantItems);
        }

        private ListViewItem CreateLvi(Participant part, int imageIndex)
        {
            // If the participant's name is null, show the identifier as the name
            string name = (part.Name == null || part.Name == String.Empty) ? part.Identifier : part.Name;
            string[] columns = new string[]{name, part.Identifier, part.Email};
            ListViewItem item = new ListViewItem(columns);
            item.ImageIndex = imageIndex;
            item.Tag = part;

            return item;
        }

        /// <summary>
        /// Takes an icon as a byte[], adds it to the images list, and returns its new index in the list.
        /// </summary>
        private int AddIcon(byte[] participantIcon)
        {
            Image icon = IconUtilities.BytesToImage (participantIcon);
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
            Participant selectedParticipant = (Participant)list.SelectedItems[0].Tag;
            ParticipantEditor editor = new ParticipantEditor(selectedParticipant);
            editor.Text = Strings.EditParticipant;
            DialogResult dr = editor.ShowDialog();

            if (dr == DialogResult.OK)
            {
                Participant editedParticipant = editor.GetParticipant();

                Debug.Assert(editedParticipant.Identifier == editor.OriginalParticipant.Identifier);

                StorageFile.UpdateParticipant(editedParticipant);

                // Don't remove the image; it will screw up the images for all of the other participants
                // But we will check to see if the image has changed (if not, just use the old imageIndex)
                int imageIndex;
                if (editedParticipant.Icon == selectedParticipant.Icon)
                    imageIndex = list.SelectedItems[0].ImageIndex;
                else
                    imageIndex = AddIcon(editedParticipant.Icon);

                // Remove the old item
                list.Items.RemoveAt(list.SelectedIndices[0]);

                // Create and add the new item
                list.Items.Add (CreateLvi(editedParticipant, imageIndex));
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            Participant selectedParticipant = ((Participant)list.SelectedItems[0].Tag);
            string removeStr = string.Format(CultureInfo.CurrentCulture, Strings.ConfirmParticipantDeleteText, 
                selectedParticipant.Name);
            DialogResult dr = RtlAwareMessageBox.Show(this, removeStr, Strings.ConfirmParticipantDeleteTitle, 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            if (dr == DialogResult.Yes)
            {
                StorageFile.DeleteParticipant(selectedParticipant.Identifier);
                // don't remove the image; it will screw up the images for all of the other participants
                list.Items.RemoveAt(list.SelectedIndices[0]);
            }
        }
        #endregion

    }
}

