using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP.VenueService
{
    /// <summary>
    /// Summary description for ItemManager.
    /// </summary>
    internal abstract class ItemManager : System.Windows.Forms.UserControl
    {
        private FileStorage storage;
        private int[] variableColumns = new int[0];
        private float[] columnPercents;

        protected System.Windows.Forms.Button deleteBtn;
        protected System.Windows.Forms.ListView list;
        protected System.Windows.Forms.Button editBtn;
        protected System.Windows.Forms.Button newBtn;
        protected System.Windows.Forms.ImageList images;
        protected System.Windows.Forms.Button refreshBtn;
        private System.ComponentModel.IContainer components;

        #region Ctor, Dispose
        public ItemManager()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // Make sure we've hooked resize
            list.Resize += new EventHandler(list_Resize);
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
            this.components = new System.ComponentModel.Container();
            this.deleteBtn = new System.Windows.Forms.Button();
            this.list = new System.Windows.Forms.ListView();
            this.images = new System.Windows.Forms.ImageList(this.components);
            this.editBtn = new System.Windows.Forms.Button();
            this.newBtn = new System.Windows.Forms.Button();
            this.refreshBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // deleteBtn
            // 
            this.deleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteBtn.Enabled = false;
            this.deleteBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.deleteBtn.Location = new System.Drawing.Point(384, 152);
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.TabIndex = 6;
            this.deleteBtn.Text = Strings.Delete;
            // 
            // list
            // 
            this.list.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.list.GridLines = true;
            this.list.Location = new System.Drawing.Point(8, 8);
            this.list.MultiSelect = false;
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(448, 136);
            this.list.SmallImageList = this.images;
            this.list.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.list.TabIndex = 3;
            this.list.View = System.Windows.Forms.View.Details;
            this.list.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.list_ColumnClick);
            this.list.SelectedIndexChanged += new System.EventHandler(this.list_SelectedIndexChanged);
            // 
            // images
            // 
            this.images.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.images.ImageSize = new System.Drawing.Size(16, 16);
            this.images.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // editBtn
            // 
            this.editBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.editBtn.Enabled = false;
            this.editBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.editBtn.Location = new System.Drawing.Point(296, 152);
            this.editBtn.Name = "editBtn";
            this.editBtn.TabIndex = 5;
            this.editBtn.Text = Strings.Edit;
            // 
            // newBtn
            // 
            this.newBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.newBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.newBtn.Location = new System.Drawing.Point(208, 152);
            this.newBtn.Name = "newBtn";
            this.newBtn.TabIndex = 4;
            this.newBtn.Text = Strings.New;
            // 
            // refreshBtn
            // 
            this.refreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.refreshBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.refreshBtn.Location = new System.Drawing.Point(8, 152);
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(80, 23);
            this.refreshBtn.TabIndex = 4;
            this.refreshBtn.Text = Strings.Refresh;
            this.refreshBtn.Click += new System.EventHandler(this.refreshBtn_Click);
            // 
            // ItemManager
            // 
            this.Controls.Add(this.deleteBtn);
            this.Controls.Add(this.list);
            this.Controls.Add(this.editBtn);
            this.Controls.Add(this.newBtn);
            this.Controls.Add(this.refreshBtn);
            this.Name = "ItemManager";
            this.Size = new System.Drawing.Size(464, 184);
            this.ResumeLayout(false);

        }
        #endregion

        #region Public Properties
        internal virtual FileStorage StorageFile
        {
            get
            {
                return this.storage;
            }
            set
            {
                this.storage = value;

                this.RefreshList();
            }
        }
        #endregion

        #region Refresh
        protected abstract void RefreshList();

        private void refreshBtn_Click(object sender, System.EventArgs e)
        {
            RefreshList();
        }
        #endregion

        #region Custom ListView Column Resizing
        // Ideally, this would actually be added to a custom control extending a
        //  ListView, but that just isn't worth it the time...

        public int[] VariableWidthColumns
        {
            get
            {
                return (int[])variableColumns.Clone();
            }
            set
            {
                this.variableColumns = value;
                StoreColumnWidths();
            }
        }

        public void StoreColumnWidths()
        {
            // Sore the width, in percent of available space, of all of the non-fixed columns
            columnPercents = new float[variableColumns.Length];
            // Get the total width of the variable columns
            int variableColTotal = 0;
            foreach (int colNumber in variableColumns)
            {
                variableColTotal += list.Columns[colNumber].Width;
            }
            for (int cnt = 0; cnt < variableColumns.Length; ++cnt)
            {
                int colNumber = this.variableColumns[cnt];
                ColumnHeader col = list.Columns[colNumber];
                columnPercents[cnt] = ((float)col.Width)/((float)variableColTotal);
            }
        }

        public void ResizeColumns()
        {
            ArrayList varCols = new ArrayList(this.variableColumns);
            int extraSpace = list.Width - 6 - images.ImageSize.Width; // get all of the space not occupied by fixed-width columns
            foreach (ColumnHeader col in list.Columns)
            {
                if (!varCols.Contains(col.Index))
                {
                    extraSpace -= col.Width;
                }
            }
            
            if( extraSpace <= 0 ) // this can happen before variableColumns is set
                return;

            for (int cnt = 0; cnt < variableColumns.Length; ++cnt)
            {
                int colNumber = this.variableColumns[cnt];
                float percentAllowed = this.columnPercents[cnt];
                Debug.Assert(percentAllowed > 0F && percentAllowed < 1F);

                int newSize = (int)(percentAllowed * ((float)extraSpace));
                list.Columns[colNumber].Width = newSize;
            }
        }

        private void list_Resize(object sender, EventArgs e)
        {
            this.ResizeColumns();
        }
        #endregion

        #region Selected item changed
        private void list_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(list.SelectedIndices.Count > 0)
            {
                this.deleteBtn.Enabled = true;
                this.editBtn.Enabled = true;
            }
            else
            {
                this.deleteBtn.Enabled = false;
                this.editBtn.Enabled = false;
            }
        }
        #endregion

        #region sorting feature stolen from MSDN docs example (verbatim cut&paste)
        // so, umm, that's like (c) Microsoft 2000 (2002? '04? <shrug> ... (thanks!)  :-)
        // ColumnClick event handler.
        private void list_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            // Set the ListViewItemSorter property to a new ListViewItemComparer 
            // object. Setting this property immediately sorts the 
            // ListView using the ListViewItemComparer object.
            this.list.ListViewItemSorter = new ListViewItemComparer(e.Column);
        }

        // Implements the manual sorting of items by columns.
        class ListViewItemComparer : IComparer
        {
            private int col;
            public ListViewItemComparer()
            {
                col = 0;
            }
            public ListViewItemComparer(int column)
            {
                col = column;
            }
            public int Compare(object x, object y)
            {
                return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
            }
        }
        #endregion

    }
}
