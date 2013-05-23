using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP.ArchiveService
{
    /// <summary>
    /// Summary description for ConferencesTreeView.
    /// </summary>
    public class ConferencesTreeView : System.Windows.Forms.TreeView
    {
        private enum ParentNode {Year, Month, Date};

        private bool canConnect = false;
        private ToolTip tips = new System.Windows.Forms.ToolTip();

        private readonly int folderImageIndex, conferenceImageIndex, participantImageIndex;
        private readonly int videoImageIndex, presentationImageIndex, soundImageIndex;


        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #region Ctor, Dispose
        public ConferencesTreeView()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            base.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(EditConference);
            base.MouseMove += new MouseEventHandler(MouseMoved);

            this.ImageList = new ImageList();
            
            Image img = Image.FromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.ArchiveService.icons.Folder.bmp"));
            this.folderImageIndex = ImageList.Images.Count;
            ImageList.Images.Add(img);

            img = Image.FromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.ArchiveService.icons.GenericVenueIcon.png"));
            this.conferenceImageIndex = ImageList.Images.Count;
            ImageList.Images.Add(img);

            img = Image.FromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.ArchiveService.icons.GenericParticipantIcon.png"));
            this.participantImageIndex = ImageList.Images.Count;
            ImageList.Images.Add(img);

            img = Image.FromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.ArchiveService.icons.VideoIcon.gif"));
            this.videoImageIndex = ImageList.Images.Count;
            ImageList.Images.Add(img);

            img = Image.FromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.ArchiveService.icons.PresentationIcon.gif"));
            this.presentationImageIndex = ImageList.Images.Count;
            ImageList.Images.Add(img);

            img = Image.FromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.ArchiveService.icons.SoundIcon.gif"));
            this.soundImageIndex = ImageList.Images.Count;
            ImageList.Images.Add(img);
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

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        #region Public Properties
        public bool Connected
        {
            get
            {
                return canConnect;
            }
        }
        #endregion

        #region Public Methods
        public void Connect()
        {
            try
            {
                // - Display wait message
                Cursor.Current = Cursors.WaitCursor;

                // - Get conferences ordered by start time
                Conference[] confs = DBHelper.GetConferences();

                // - Add items with parents: year, month, date
                TreeNode lastNode = null;
                Conference lastConf = null;

                if (confs != null && confs.Length > 0)
                {
                    this.Nodes.Clear();
                }
                foreach( Conference conf in confs )
                {
                    TreeNode newNode = new TreeNode(string.Format(CultureInfo.CurrentCulture, Strings.Conference,
                        conf.Start.ToShortTimeString(), conf.Description));
                    newNode.Tag = conf;
                    newNode.ImageIndex = conferenceImageIndex;
                    newNode.SelectedImageIndex = conferenceImageIndex;

                    TreeNode parent = null;
                    bool tripSwitch = (lastNode == null);

                    #region Add parents for this node
                    // Optimize creation of parents by tracking the last node & using the knowledge
                    //  that confs is organized by startTime (thank you, Mr. SQL, for the "order by" command.)

                    // My apologies for the lack of readability of this code.  It _was_ worse...
                    if( tripSwitch || conf.Start.Year != lastConf.Start.Year )
                    {
                        parent = this.Nodes.Add( conf.Start.Year.ToString(CultureInfo.InvariantCulture) );
                        parent.Tag = ParentNode.Year;
                        parent.ImageIndex = folderImageIndex;
                        parent.SelectedImageIndex = parent.ImageIndex;

                        if( tripSwitch != true )
                            tripSwitch = true;
                    }
                    if( tripSwitch || conf.Start.Month != lastConf.Start.Month )
                    {
                        if( !tripSwitch )
                            parent = lastNode.Parent.Parent.Parent;

                        parent = parent.Nodes.Add( conf.Start.ToString("MMMM", CultureInfo.InvariantCulture) );
                        parent.Tag = ParentNode.Month;
                        parent.ImageIndex = folderImageIndex;
                        parent.SelectedImageIndex = parent.ImageIndex;

                        if( tripSwitch != true )
                            tripSwitch = true;
                    }
                    if( tripSwitch || conf.Start.Day != lastConf.Start.Day )
                    {
                        if( !tripSwitch )
                            parent = lastNode.Parent.Parent;

                        parent = parent.Nodes.Add( conf.Start.ToLongDateString() );
                        parent.Tag = ParentNode.Date;
                        parent.ImageIndex = folderImageIndex;
                        parent.SelectedImageIndex = parent.ImageIndex;
                    }
                    else
                    {
                        parent = lastNode.Parent;
                    }
                    #endregion

                    // Pri3: Optimize getting of participants/streams.  Try to find a way to do it only after
                    //  each conference is expanded (note that a conference can't be "expanded" if it doesn't
                    //  have any children yet.  consider using a placeholder)
                    #region Get participants / streams
                    Participant[] participants = DBHelper.GetParticipants( conf.ConferenceID );
                    foreach( Participant part in participants )
                    {
                        // Create Participant node
                        TreeNode partNode = new TreeNode(string.Format(CultureInfo.CurrentCulture, 
                            Strings.Participant, part.CName));
                        partNode.Tag = part;
                        partNode.ImageIndex = participantImageIndex;
                        partNode.SelectedImageIndex = partNode.ImageIndex;

                        // Add all of the streams for this participant
                        Stream[] streams = DBHelper.GetStreams(part.ParticipantID);
                        foreach( Stream str in streams )
                        {
                            TreeNode streamNode = partNode.Nodes.Add(string.Format(CultureInfo.CurrentCulture, 
                                Strings.Stream, str.Name));
                            streamNode.Tag = str;

                            // Set the payload type icon
                            MSR.LST.Net.Rtp.PayloadType payload = (MSR.LST.Net.Rtp.PayloadType)Enum.Parse(typeof(MSR.LST.Net.Rtp.PayloadType), str.Payload);
                            if( payload == MSR.LST.Net.Rtp.PayloadType.dynamicVideo )
                                streamNode.ImageIndex = videoImageIndex;
                            else if( payload == MSR.LST.Net.Rtp.PayloadType.dynamicAudio )
                                streamNode.ImageIndex = soundImageIndex;
                            else //if( payload == MSR.LST.Net.Rtp.PayloadType.dynamicPresentation )
                                streamNode.ImageIndex = presentationImageIndex;

                            streamNode.SelectedImageIndex = streamNode.ImageIndex;
                        }

                        newNode.Nodes.Add(partNode);
                    }
                    #endregion

                    parent.Nodes.Add( newNode );
                    lastNode = newNode;
                    lastConf = conf;
                }

                foreach( TreeNode rootNode in base.Nodes )
                {
                    rootNode.Expand();
                }

                this.canConnect = true;

                // - Remove wait message
                Cursor.Current = Cursors.Default;
            }
            catch
            {
                this.canConnect = false;
            }
        }

        public void DeleteConferences(TreeNode[] nodes)
        {
            int[] confIDs = new int[nodes.Length];
            for (int cnt = 0; cnt < confIDs.Length; ++cnt)
            {
                confIDs[cnt] = ((nodes[cnt] as TreeNode).Tag as Conference).ConferenceID;
            }
            DBHelper.DeleteConferences (confIDs);
            RemoveNodes (nodes);
        }

        public void DeleteNode(TreeNode node)
        {
            if( node.Tag is Conference )
            {
                DBHelper.DeleteConferences( new int[]{(node.Tag as Conference).ConferenceID} );
            }
            else if( node.Tag is Participant )
            {
                DBHelper.DeleteParticipants( new int[]{(node.Tag as Participant).ParticipantID} );
            }
            else if( node.Tag is Stream )
            {
                DBHelper.DeleteStreams( new int[]{(node.Tag as Stream).StreamID} );
            }
            else // it's a year, month, or day, so do search for conferences
            {
                // We'll do this with a really simple recursive-style approach...
                foreach(TreeNode child in node.Nodes)
                {
                    DeleteNode(child);
                }
            }

            this.RemoveNodes(new TreeNode[]{node});
        }

        public TreeNode[] GetConferencesInRange(DateTime start, DateTime end)
        {
            ArrayList confs = new ArrayList();

            foreach( TreeNode yearNode in Nodes )
            {
                int year = Int32.Parse(yearNode.Text, CultureInfo.InvariantCulture);
                if( start.Year <= year && year <= end.Year )
                {
                    foreach( TreeNode monthNode in yearNode.Nodes )
                    {
                        // Parsing a month is annoying.  Just check them all.
                        foreach( TreeNode dateNode in monthNode.Nodes )
                        {
                            DateTime date = DateTime.Parse(dateNode.Text, CultureInfo.InvariantCulture);
                            // Because start's "time" is set, but date's "time" is 0, we have to use DayOfYear
                            if( start.DayOfYear <= date.DayOfYear && date.DayOfYear <= end.DayOfYear )
                            {
                                foreach( TreeNode confNode in dateNode.Nodes )
                                {
                                    Conference conf = confNode.Tag as Conference;
                                    if( start <= conf.Start && conf.End <= end )
                                        confs.Add(confNode);
                                }
                            }
                        }
                    }
                }
            }

            TreeNode[] confArr = new TreeNode[ confs.Count ];
            confs.CopyTo(confArr, 0);
            return confArr;
        }

        public void CleanUpDatabase()
        {
            // Find:
            // - conferences w/o participants
            // - participants w/o streams
            // - streams w/o bytes
            ArrayList confs = new ArrayList(), parts = new ArrayList(), streams = new ArrayList();
            #region Depth-first-search of tree
            // By collecting a full list of the conferences, participants, and streams to be deleted
            //  we can more effeciently delete the objects by doing quick bulk deletes from the database,
            //  rather than transacting for each individual delete.
            foreach( TreeNode yearNode in Nodes )
            {
                foreach( TreeNode monthNode in yearNode.Nodes )
                {
                    foreach( TreeNode dateNode in monthNode.Nodes )
                    {
                        foreach( TreeNode confNode in dateNode.Nodes )
                        {
                            if( !confNode.Nodes.GetEnumerator().MoveNext() ) // check for emptiness
                            {
                                confs.Add( confNode );
                            }
                            else
                            {
                                foreach( TreeNode partNode in confNode.Nodes )
                                {
                                    if( !partNode.Nodes.GetEnumerator().MoveNext() )
                                    {
                                        parts.Add( partNode );
                                    }
                                    else
                                    {
                                        foreach( TreeNode streamNode in partNode.Nodes )
                                        {
                                            if( (streamNode.Tag as Stream).Bytes <= 1 )
                                                streams.Add( streamNode );
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region Transact on DB for Delete:
            // - conferences
            int[] confIDs = new int[confs.Count];
            for( int cnt = 0; cnt < confIDs.Length; ++cnt )
            {
                confIDs[cnt] = ((confs[cnt] as TreeNode).Tag as Conference).ConferenceID;
            }
            DBHelper.DeleteConferences(confIDs);
            
            // - participants
            int[] partIDs = new int[parts.Count];
            for( int cnt = 0; cnt < partIDs.Length; ++cnt )
            {
                partIDs[cnt] = ((parts[cnt] as TreeNode).Tag as Participant).ParticipantID;
            }
            DBHelper.DeleteParticipants(partIDs);

            // - streams
            int[] streamIDs = new int[streams.Count];
            for( int cnt = 0; cnt < streamIDs.Length; ++cnt )
            {
                streamIDs[cnt] = ((streams[cnt] as TreeNode).Tag as Stream).StreamID;
            }
            DBHelper.DeleteStreams(streamIDs);
            #endregion

            // Delete from UI
            RemoveNodes(confs);
            RemoveNodes(parts);
            RemoveNodes(streams);

            // Find Conferences w/o end times
            // (for simplicity, just get them from the DB again)
            Conference[] allConfs = DBHelper.GetConferences();
            foreach(Conference conf in allConfs)
            {
                if (conf.End == DateTime.MinValue)
                {
                    DBHelper.CreateConferenceEndTime(conf.ConferenceID);
                }
            }
        }
        #endregion

        #region Private Methods
        private void EditConference(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
        {
            // Only allow renaming (re-describing) conferences only
            if( !(e.Node.Tag is Conference) )
            {
                e.CancelEdit = true;
                return;
            }
            else if( e.Label == null ) // For some reason, this means the label was edited but not changed
            {
                return;
            }

            // Display wait cursor
            Cursor.Current = Cursors.WaitCursor;

            // Get conference info
            Conference conf = e.Node.Tag as Conference;

            // Test for someone leaving the time string at the beginning & remove it, if necessary
            string time = string.Format(CultureInfo.CurrentCulture, Strings.ConferenceLower, 
                conf.Start.ToShortTimeString().ToLower(CultureInfo.InvariantCulture));
            string newLabel;
            if( e.Label.ToLower(CultureInfo.InvariantCulture).StartsWith(time) )
            {
                // it's there, so remove it.
                newLabel = e.Label.Substring( time.Length );
            }
            else
            {
                newLabel = e.Label;
            }

            // Update DB
            if( DBHelper.RenameConference( conf.ConferenceID, newLabel ) )
            {
                conf.Description = newLabel;
                e.Node.Text = string.Format(CultureInfo.CurrentCulture, Strings.Conference, 
                    conf.Start.ToShortTimeString(), newLabel);
                e.CancelEdit = true; // we do this to prevent some base code from ignoring the line just above
            }

            // Return to normal cursor
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Deletes a set of nodes from recordings, and after deleting nodes checks the parent 
        /// Year, Month, & Date nodes for emptiness, and removes them as necessary.
        /// </summary>
        /// <param name="deletedMeNodes">Nodes to delete.</param>
        private void RemoveNodes( IEnumerable deleteMeNodes )
        {
            foreach( TreeNode node in deleteMeNodes )
            {
                TreeNode parent = node.Parent;

                // This is some sort of weird limiting case.
                if( parent == null )
                    continue;
                
                node.Remove();

                while( parent != null && !parent.Nodes.GetEnumerator().MoveNext() ) // while parent has no kids
                {
                    TreeNode oldParent = parent;
                    parent = oldParent.Parent;

                    DeleteNodeContents(oldParent);
                    oldParent.Remove();
                }
            }
        }

        /// <summary>
        /// Deletes the data in the database that a node corresponds to, if any.
        /// </summary>
        private void DeleteNodeContents(TreeNode emptyNode)
        {
            if (emptyNode.Tag is Conference)
            {
                DBHelper.DeleteConferences(new int[]{(emptyNode.Tag as Conference).ConferenceID});
            }
            else if (emptyNode.Tag is Participant)
            {
                DBHelper.DeleteParticipants(new int[]{(emptyNode.Tag as Participant).ParticipantID});
            }
            else if (emptyNode.Tag is Stream)
            {
                DBHelper.DeleteStreams(new int[]{(emptyNode.Tag as Stream).StreamID});
            }
        }

        private void MouseMoved(object sender, MouseEventArgs e)
        {
            string toolTipString = string.Empty; // "" is the default

            TreeNode node = GetNodeAt(e.X, e.Y);
            if( node != null && node.Tag != null )
            {
                toolTipString = node.Tag.ToString();
            }

            // Don't change unless we have to
            if (this.tips.GetToolTip(this) != toolTipString)
                this.tips.SetToolTip(this, toolTipString);
        }
        #endregion

    }
}
