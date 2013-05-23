using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP.VenueService
{
    /// <summary>
    /// Summary description for IconInput.
    /// </summary>
    internal class IconInput : System.Windows.Forms.UserControl
    {
        const int IconSize = 96;
        private System.Windows.Forms.PictureBox pictureIcon;
        private System.Windows.Forms.GroupBox iconGroupBox;
        private System.Windows.Forms.Button clearBtn;
        private System.Windows.Forms.Button loadBtn;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private Image defaultImage;


        public IconInput()
        {
            // This call is required by the Windows.Forms Form Designer.
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

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.clearBtn = new System.Windows.Forms.Button();
            this.loadBtn = new System.Windows.Forms.Button();
            this.pictureIcon = new System.Windows.Forms.PictureBox();
            this.iconGroupBox = new System.Windows.Forms.GroupBox();
            this.iconGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // clearBtn
            // 
            this.clearBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.clearBtn.Location = new System.Drawing.Point(96, 56);
            this.clearBtn.Name = "clearBtn";
            this.clearBtn.Size = new System.Drawing.Size(104, 24);
            this.clearBtn.TabIndex = 20;
            this.clearBtn.Text = Strings.RestoreDefault;
            this.clearBtn.Click += new System.EventHandler(this.cmdClearIcon_Click);
            // 
            // loadBtn
            // 
            this.loadBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.loadBtn.Location = new System.Drawing.Point(96, 24);
            this.loadBtn.Name = "loadBtn";
            this.loadBtn.Size = new System.Drawing.Size(104, 24);
            this.loadBtn.TabIndex = 19;
            this.loadBtn.Text = Strings.ChoosePicture;
            this.loadBtn.Click += new System.EventHandler(this.cmdLoadIcon_Click);
            // 
            // pictureIcon
            // 
            this.pictureIcon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureIcon.Location = new System.Drawing.Point(16, 24);
            this.pictureIcon.Name = "pictureIcon";
            this.pictureIcon.Size = new System.Drawing.Size(64, 64);
            this.pictureIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureIcon.TabIndex = 18;
            this.pictureIcon.TabStop = false;
            // 
            // iconGroupBox
            // 
            this.iconGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.iconGroupBox.Controls.Add(this.pictureIcon);
            this.iconGroupBox.Controls.Add(this.loadBtn);
            this.iconGroupBox.Controls.Add(this.clearBtn);
            this.iconGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.iconGroupBox.Location = new System.Drawing.Point(0, 0);
            this.iconGroupBox.Name = "iconGroupBox";
            this.iconGroupBox.Size = new System.Drawing.Size(216, 104);
            this.iconGroupBox.TabIndex = 21;
            this.iconGroupBox.TabStop = false;
            this.iconGroupBox.Text = Strings.Icon;
            // 
            // IconInput
            // 
            this.Controls.Add(this.iconGroupBox);
            this.Name = "IconInput";
            this.Size = new System.Drawing.Size(216, 104);
            this.iconGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        public Image Icon
        {
            get
            {
                return pictureIcon.Image;
            }
            set
            {
                pictureIcon.Image = value;
            }
        }

        public Image DefaultIcon
        {
            get
            {
                return this.defaultImage;
            }
            set
            {
                if( pictureIcon.Image == this.defaultImage )
                    pictureIcon.Image = value;
                this.defaultImage = value;
            }
        }

        public byte[] IconAsBytes
        {
            get
            {
                if( pictureIcon.Image != null && pictureIcon.Image != this.defaultImage )
                    return IconUtilities.ImageToIconBytes(pictureIcon.Image, IconSize);
                else
                    return null;
            }
            set
            {
                this.pictureIcon.Image = IconUtilities.BytesToImage(value);
                if( this.pictureIcon.Image == null )
                    this.pictureIcon.Image = this.defaultImage;
            }
        }

        private void cmdLoadIcon_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG;*.EMF;*.EXIF;*.ICO;*.TIF;*.WMF";

            DialogResult dr = dialog.ShowDialog();
            if( dr == DialogResult.OK )
            {
                pictureIcon.Image = IconUtilities.CreateSquareThumbnail(Image.FromFile(dialog.FileName), IconSize);
            }
        }

        private void cmdClearIcon_Click(object sender, System.EventArgs e)
        {
            DialogResult dr = RtlAwareMessageBox.Show(this, Strings.ConfirmDefaultIconRestoreText, 
                Strings.ConfirmDefaultIconRestoreTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, 
                MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

            if (dr == DialogResult.Yes)
                IconAsBytes = null;  // sets the default icon
        }

        private void cmdSaveIcon_Click(object sender, System.EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = "png";

            DialogResult dr = dialog.ShowDialog();
            if( dr == DialogResult.OK )
            {
                Stream file = dialog.OpenFile();
                pictureIcon.Image.Save(file, ImageFormat.Png);
                file.Close();
            }
        }
    }

    #region Helper Methods
    public class IconUtilities
    {
        public static Image BytesToImage(byte[] icon)
        {
            if (icon != null)
            {
                Stream imageStream = new System.IO.MemoryStream(icon);
                return Image.FromStream(imageStream);
            }
            else
            {
                return null;
            }
        }

        public static byte[] ImageToIconBytes(Image masterImage, int iconSize)
        {
            MemoryStream ms = new MemoryStream();
            Image thumbnail = CreateSquareThumbnail(masterImage, iconSize);
            thumbnail.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        public static Image CreateSquareThumbnail(Image masterImage, int iconSize)
        {
            // Calculate the thumbnail size
            double newWidth = 0.0;
            double newHeight = 0.0;

            if (masterImage.Width > iconSize || masterImage.Height > iconSize)
            {
                bool portrait = (masterImage.Width > masterImage.Height);

                if (portrait)
                {
                    double pct = (double)masterImage.Width / iconSize;
                    newWidth = (double)masterImage.Width / pct;
                    newHeight = (double)masterImage.Height / pct;
                }
                else
                {
                    double pct = (double)masterImage.Height / iconSize;
                    newWidth = (double)masterImage.Width / pct;
                    newHeight = (double)masterImage.Height / pct;
                }
            }
            else
            {
                newWidth = masterImage.Width;
                newHeight = masterImage.Height;
            }

            // Put the thumbnail on a square background
            Image squareThumb = new Bitmap(iconSize,iconSize);

            using(Graphics g = Graphics.FromImage(squareThumb))
            {
                int x = 0;
                int y = 0;

                x = (iconSize - (int)newWidth) / 2;
                y = (iconSize - (int)newHeight) / 2;

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(masterImage, x, y, (int)newWidth, (int)newHeight);
            }

            return squareThumb;
        }
    }
    #endregion

}
