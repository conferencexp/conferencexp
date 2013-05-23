using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


namespace MSR.LST.Controls.InkToolBarControls
{
    class ColorMenuItem : ImageMenuItem
    {
        public ColorMenuItem() : base(string.Empty, null, true)
        {
            InitializeComponent();
            base.Image = this.m_imageList.Images[0];
        }

        private System.Windows.Forms.ImageList m_imageList;
        private System.ComponentModel.IContainer components;

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ColorMenuItem));
            this.m_imageList = new System.Windows.Forms.ImageList(this.components);
            // 
            // m_imageList
            // 
            this.m_imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.m_imageList.ImageSize = new System.Drawing.Size(24, 17);
            this.m_imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_imageList.ImageStream")));
            this.m_imageList.TransparentColor = System.Drawing.Color.Transparent;

        }
        #endregion
    }
}
