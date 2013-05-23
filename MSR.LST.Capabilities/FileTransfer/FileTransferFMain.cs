using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;


namespace MSR.LST.ConferenceXP
{
    public class FileTransferFMain : CapabilityForm
    {
        #region VS Generated
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlHeading;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnAddFiles;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader colFileName;
        private System.Windows.Forms.ColumnHeader colFileSize;
        private System.Windows.Forms.ColumnHeader colFileModifiedDate;
        private System.Windows.Forms.ColumnHeader colFilePath;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnAdvanced;
        private System.ComponentModel.IContainer components;

        public FileTransferFMain()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

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

        private FileTransferCapability fileTransferCapability = null;

        #region ICapabilityForm

        public override void AddCapability(ICapability capability)
        {
            base.AddCapability (capability);

            if (fileTransferCapability == null)
                fileTransferCapability = (FileTransferCapability)capability;
        }

        public override bool RemoveCapability(ICapability capability)
        {
            bool ret = base.RemoveCapability (capability);
            fileTransferCapability = null;
            this.Close();

            return ret;
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listView = new System.Windows.Forms.ListView();
            this.colFileName = new System.Windows.Forms.ColumnHeader();
            this.colFilePath = new System.Windows.Forms.ColumnHeader();
            this.colFileSize = new System.Windows.Forms.ColumnHeader();
            this.colFileModifiedDate = new System.Windows.Forms.ColumnHeader();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAddFiles = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAdvanced = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlHeading = new System.Windows.Forms.Panel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.pnlHeading.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView);
            this.groupBox1.Controls.Add(this.btnRemove);
            this.groupBox1.Controls.Add(this.btnAddFiles);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnAdvanced);
            this.groupBox1.Location = new System.Drawing.Point(-8, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(464, 213);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFileName,
            this.colFilePath,
            this.colFileSize,
            this.colFileModifiedDate});
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView.Location = new System.Drawing.Point(24, 40);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(320, 160);
            this.listView.TabIndex = 5;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            // 
            // colFileName
            // 
            this.colFileName.Text = "File";
            this.colFileName.Width = 79;
            // 
            // colFilePath
            // 
            this.colFilePath.Text = "Folder";
            this.colFilePath.Width = 115;
            // 
            // colFileSize
            // 
            this.colFileSize.Text = "Size";
            this.colFileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colFileSize.Width = 82;
            // 
            // colFileModifiedDate
            // 
            this.colFileModifiedDate.Text = "Last modified";
            this.colFileModifiedDate.Width = 130;
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(357, 80);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "&Remove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAddFiles
            // 
            this.btnAddFiles.Location = new System.Drawing.Point(357, 48);
            this.btnAddFiles.Name = "btnAddFiles";
            this.btnAddFiles.Size = new System.Drawing.Size(75, 23);
            this.btnAddFiles.TabIndex = 2;
            this.btnAddFiles.Text = "&Add files...";
            this.btnAddFiles.Click += new System.EventHandler(this.btnAddFiles_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Selected files:";
            // 
            // btnAdvanced
            // 
            this.btnAdvanced.Location = new System.Drawing.Point(357, 177);
            this.btnAdvanced.Name = "btnAdvanced";
            this.btnAdvanced.Size = new System.Drawing.Size(75, 23);
            this.btnAdvanced.TabIndex = 6;
            this.btnAdvanced.Text = "Ad&vanced";
            this.btnAdvanced.Click += new System.EventHandler(this.btnAdvanced_Click);
            // 
            // btnSend
            // 
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(264, 272);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "&Send";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(357, 272);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlHeading
            // 
            this.pnlHeading.BackColor = System.Drawing.Color.White;
            this.pnlHeading.Controls.Add(this.lblDescription);
            this.pnlHeading.Controls.Add(this.lblTitle);
            this.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeading.Location = new System.Drawing.Point(0, 0);
            this.pnlHeading.Name = "pnlHeading";
            this.pnlHeading.Size = new System.Drawing.Size(438, 56);
            this.pnlHeading.TabIndex = 3;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(32, 32);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(192, 13);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "Send files to multiple users via multicast";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(16, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(66, 14);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Send files";
            // 
            // FileTransferFMain
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(438, 305);
            this.Controls.Add(this.pnlHeading);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FileTransferFMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "File Transfer";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FileTransferFMain_Closing);
            this.Load += new System.EventHandler(this.FileTransferFMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlHeading.ResumeLayout(false);
            this.pnlHeading.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private void FileTransferFMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (fileTransferCapability != null && fileTransferCapability.IsSending)
                fileTransferCapability.StopSending();
        }

        private void btnSend_Click(object sender, System.EventArgs e)
        {
            this.Hide();

            foreach (ListViewItem lvItem in listView.Items)
            {
                string fileName = lvItem.SubItems[1].Text + Path.DirectorySeparatorChar + lvItem.SubItems[0].Text;

            retry:
                if (!File.Exists(fileName))
                {
                    DialogResult dlgFNFResult = RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                        Strings.FileNoLongerExists, fileName, Environment.NewLine), Strings.FileNotFound, 
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, 
                        (MessageBoxOptions)0);
                    if (dlgFNFResult == DialogResult.Yes)
                        continue;
                    else if (dlgFNFResult == DialogResult.Cancel)
                        return;
                    else goto retry;
                }

                Label lblMessage = new Label();
                lblMessage.Font = new Font("Tahoma", 10f);
                lblMessage.AutoSize = true;
                lblMessage.Text = string.Format(CultureInfo.CurrentCulture, Strings.SendingFilePleaseWait, fileName);
                lblMessage.Location = new Point(10, 5);

                Form frmMessage = new Form();
                frmMessage.ClientSize = new Size(lblMessage.Width + 20, lblMessage.Height + 10);
                frmMessage.StartPosition = FormStartPosition.CenterScreen;
                frmMessage.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                frmMessage.Text = Strings.SendingFile;
                frmMessage.Controls.Add(lblMessage);
                frmMessage.Show();
                Application.DoEvents();

                FileObject fileObject = new FileObject(fileName);
                fileTransferCapability.SendObject(fileObject);

                frmMessage.Close();
            }

            this.Close();
            RtlAwareMessageBox.Show(this, Strings.FileTransferCompleted, 
                Strings.SendDone, MessageBoxButtons.OK, MessageBoxIcon.Information, 
                MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            FileTransferFMain_Closing(null, null);
            this.Close();
        }

        private void btnAddFiles_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.CheckFileExists = true;
            openDialog.CheckPathExists = true;
            openDialog.Filter = "All files (*.*)|*.*";
            openDialog.Multiselect = true;
            openDialog.Title = Strings.SelectFiles;
            if (openDialog.ShowDialog() != DialogResult.OK)
                return;

            foreach (string fileName in openDialog.FileNames)
            {
                FileInfo fileInfo = new FileInfo(fileName);
                listView.Items.Add(new ListViewItem(
                    new string[] { fileInfo.Name, fileInfo.DirectoryName, 
                        ((double)fileInfo.Length / 1024.0).ToString("#,#0.00", CultureInfo.CurrentCulture) + 
                        " " + Strings.KB, fileInfo.LastWriteTime.ToString() }));
            }

            btnSend.Enabled = listView.Items.Count > 0;
        }

        private void btnRemove_Click(object sender, System.EventArgs e)
        {
            ListViewItem[] selectedItems = new ListViewItem[listView.SelectedItems.Count];
            listView.SelectedItems.CopyTo(selectedItems, 0);

            foreach (ListViewItem selectedFile in selectedItems)
                listView.Items.Remove(selectedFile);

            btnSend.Enabled = listView.Items.Count > 0;
        }

        private void listView_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            btnRemove.Enabled = listView.SelectedItems.Count > 0;
        }

        private void btnAdvanced_Click(object sender, System.EventArgs e)
        {
            Uri assemblyUri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            FileInfo assemblyInfo = new FileInfo(assemblyUri.LocalPath);
            string configFile = assemblyInfo.DirectoryName + Path.DirectorySeparatorChar + "FileTransfer.xml";

            string strXmlConfig = "<FileTransferCapability><Configuration><Fec enabled='" + fileTransferCapability.FecEnabled + 
                "' value='" + fileTransferCapability.FecChecksum + "' /><MaxBandwidth value='" +
                fileTransferCapability.MaximumBandwidthLimiter + "' /></Configuration></FileTransferCapability>";

            AdvancedDialog advDlg = new AdvancedDialog();

            XmlDocument xmlDocSettings = new XmlDocument();
            try
            {
                xmlDocSettings.Load(configFile);
                if (xmlDocSettings.DocumentElement.Name != "FileTransferCapability")
                    throw new Exception(Strings.ConfigurationFileCorrupt);
            }
            catch
            {
                xmlDocSettings.LoadXml(strXmlConfig);
            }

            XmlElement xmlConfiguration = xmlDocSettings.DocumentElement["Configuration"];
            System.Diagnostics.Debug.Assert(xmlConfiguration != null, "The configuration file is corrupt");

            advDlg.MaxBandwidth = int.Parse(xmlConfiguration["MaxBandwidth"].GetAttribute("value"), CultureInfo.InvariantCulture);
            advDlg.EnableFEC = bool.Parse(xmlConfiguration["Fec"].GetAttribute("enabled"));
            advDlg.FECAmount = int.Parse(xmlConfiguration["Fec"].GetAttribute("value"), CultureInfo.InvariantCulture);

            if (advDlg.ShowDialog() != DialogResult.OK)
                return;

            strXmlConfig = "<FileTransferCapability><Configuration><Fec enabled='" +
                advDlg.EnableFEC + "' value='" + advDlg.FECAmount + "' /><MaxBandwidth value='" +
                advDlg.MaxBandwidth + "' /></Configuration></FileTransferCapability>";

            xmlDocSettings.LoadXml(strXmlConfig);
            XmlTextWriter xtw = new XmlTextWriter(configFile, System.Text.Encoding.UTF8);
            xtw.Formatting = Formatting.Indented;
            xmlDocSettings.Save(xtw);
            xtw.Flush();
            xtw.Close();
        }

        private void FileTransferFMain_Load(object sender, EventArgs e)
        {
            this.colFileName.Text = Strings.File;
            this.colFilePath.Text = Strings.Folder;
            this.colFileSize.Text = Strings.Size;
            this.colFileModifiedDate.Text = Strings.LastModified;
            this.btnRemove.Text = Strings.RemoveHotkey;
            this.btnAddFiles.Text = Strings.AddFilesHotkey;
            this.label1.Text = Strings.SelectedFiles;
            this.btnAdvanced.Text = Strings.AdvancedHotkey;
            this.btnSend.Text = Strings.SendHotkey;
            this.btnCancel.Text = Strings.CancelHotkey;
            this.lblDescription.Text = Strings.SendFilesToMultipleUsers;
            this.lblTitle.Text = Strings.SendFiles;
            this.Text = Strings.FileTransfer;
        }
    }
}
