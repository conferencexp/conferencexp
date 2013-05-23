using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// Summary description for FileReceivedDialog.
    /// </summary>
    public class ItemReceivedDialog : System.Windows.Forms.Form
    {
        public ItemReceivedDialog(string title, string description) : this(title, description, null) {}

        public ItemReceivedDialog(string title, string description, Image msgIcon)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            this.Title = title;

            if (description != null) this.Description = description;
            if (msgIcon != null) pboxIcon.Image = msgIcon;
        }
        public string Button1Label
        {
            set { btnAccept.Text = value; }
        }
        public string Button2Label
        {
            set { btnReject.Text = value; }
        }

        private System.Windows.Forms.Panel pnlHeading;

        private System.Windows.Forms.PictureBox pboxIcon;
        public Image Image
        {
            set { pboxIcon.Image = value; }
        }

        private System.Windows.Forms.Label lblTitle;
        public string Title
        {
            set { this.Text = lblTitle.Text = value; }
        }

        private System.Windows.Forms.GroupBox groupBox1;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pboxAvatar;
        public Image Avatar
        {
            set { pboxAvatar.Image = value; }
        }
        public string AvatarTip
        {
            set 
            {
                toolTip.SetToolTip(pboxAvatar, value);
                toolTip.SetToolTip(lblFrom, value);
            }
            get
            {
                return toolTip.GetToolTip(pboxAvatar);
            }
        }

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblFrom;
        public string From
        {
            set { lblFrom.Text = value; }
        }

        private System.Windows.Forms.Label lblFile;
        public string File
        {
            set { lblFile.Text = value; }
        }

        private System.Windows.Forms.PictureBox pboxFileIcon;
        public Image FileIcon
        {
            set { pboxFileIcon.Image = value; }
        }
        public string FileIconTip
        {
            set 
            { 
                toolTip.SetToolTip(pboxFileIcon, value);
                toolTip.SetToolTip(lblFile, value);
            }
            get
            {
                return toolTip.GetToolTip(pboxFileIcon);
            }
        }

        private System.Windows.Forms.Label label5;

        private System.Windows.Forms.Label lblDescription;
        public string Description
        {
            set { lblDescription.Text = value; }
        }

        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnReject;
        private System.ComponentModel.IContainer components;

        public string Time
        {
            set { lblTime.Text = value; }
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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnReject = new System.Windows.Forms.Button();
            this.pnlHeading = new System.Windows.Forms.Panel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pboxIcon = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pboxFileIcon = new System.Windows.Forms.PictureBox();
            this.lblFile = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pboxAvatar = new System.Windows.Forms.PictureBox();
            this.lblFrom = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pnlHeading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pboxIcon)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pboxFileIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxAvatar)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAccept
            // 
            this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAccept.Location = new System.Drawing.Point(192, 176);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 23);
            this.btnAccept.TabIndex = 0;
            this.btnAccept.Text = "&Accept";
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnReject
            // 
            this.btnReject.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnReject.Location = new System.Drawing.Point(280, 176);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(75, 23);
            this.btnReject.TabIndex = 1;
            this.btnReject.Text = "&Reject";
            // 
            // pnlHeading
            // 
            this.pnlHeading.BackColor = System.Drawing.Color.White;
            this.pnlHeading.Controls.Add(this.lblDescription);
            this.pnlHeading.Controls.Add(this.lblTitle);
            this.pnlHeading.Controls.Add(this.pboxIcon);
            this.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeading.Location = new System.Drawing.Point(0, 0);
            this.pnlHeading.Name = "pnlHeading";
            this.pnlHeading.Size = new System.Drawing.Size(368, 48);
            this.pnlHeading.TabIndex = 2;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(32, 27);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(272, 16);
            this.lblDescription.TabIndex = 2;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(16, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(288, 16);
            this.lblTitle.TabIndex = 1;
            // 
            // pboxIcon
            // 
            this.pboxIcon.Location = new System.Drawing.Point(320, 8);
            this.pboxIcon.Name = "pboxIcon";
            this.pboxIcon.Size = new System.Drawing.Size(32, 32);
            this.pboxIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pboxIcon.TabIndex = 0;
            this.pboxIcon.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblTime);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.pboxFileIcon);
            this.groupBox1.Controls.Add(this.lblFile);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.pboxAvatar);
            this.groupBox1.Controls.Add(this.lblFrom);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(-8, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 125);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(72, 88);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(0, 14);
            this.lblTime.TabIndex = 7;
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(23, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 14);
            this.label5.TabIndex = 6;
            this.label5.Text = "Time:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pboxFileIcon
            // 
            this.pboxFileIcon.Location = new System.Drawing.Point(76, 56);
            this.pboxFileIcon.Name = "pboxFileIcon";
            this.pboxFileIcon.Size = new System.Drawing.Size(16, 16);
            this.pboxFileIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pboxFileIcon.TabIndex = 5;
            this.pboxFileIcon.TabStop = false;
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFile.Location = new System.Drawing.Point(104, 56);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(0, 14);
            this.lblFile.TabIndex = 4;
            this.lblFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(32, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 14);
            this.label3.TabIndex = 3;
            this.label3.Text = "File:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pboxAvatar
            // 
            this.pboxAvatar.Location = new System.Drawing.Point(72, 21);
            this.pboxAvatar.Name = "pboxAvatar";
            this.pboxAvatar.Size = new System.Drawing.Size(24, 24);
            this.pboxAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pboxAvatar.TabIndex = 2;
            this.pboxAvatar.TabStop = false;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.Location = new System.Drawing.Point(104, 24);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(0, 14);
            this.lblFrom.TabIndex = 1;
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "From:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // ItemReceivedDialog
            // 
            this.AcceptButton = this.btnAccept;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnReject;
            this.ClientSize = new System.Drawing.Size(368, 213);
            this.Controls.Add(this.pnlHeading);
            this.Controls.Add(this.btnReject);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ItemReceivedDialog";
            this.Load += new System.EventHandler(this.ItemReceivedDialog_Load);
            this.pnlHeading.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pboxIcon)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pboxFileIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxAvatar)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void btnAccept_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ItemReceivedDialog_Load(object sender, EventArgs e)
        {
            this.btnAccept.Text = Strings.AcceptHotkey;
            this.btnReject.Text = Strings.RejectHotkey;
            this.label5.Text = Strings.Time;
            this.label3.Text = Strings.FileColon;
            this.label1.Text = Strings.From;
        }
    }
}
