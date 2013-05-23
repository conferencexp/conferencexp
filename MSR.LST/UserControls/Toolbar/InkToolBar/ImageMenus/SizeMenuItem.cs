using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


namespace MSR.LST.Controls.InkToolBarControls
{
    /// <summary>
    /// Base class for the pen size menus.  This is a workaround due to the fact that if a client wants to
    /// merge the toolbar menus into their form's main menu the use of Menu.MergeMenu() does not persist
    /// dervied class properties.  So the only way around is to make a concrete class for each seperate size.
    /// </summary>
    abstract class SizeMenuItem : ImageMenuItem
    {
        private System.Windows.Forms.ImageList m_imageList;

        internal SizeMenuItem(PenMenuSize size) : base(string.Empty, null, false)
        {
            InitializeComponent();
            switch (size)
            {
                case PenMenuSize.Fine:
                    base.Image = m_imageList.Images[0];
                    break;
                case PenMenuSize.Medium:
                    base.Image = m_imageList.Images[1];
                    break;
                case PenMenuSize.Thick:
                    base.Image = m_imageList.Images[2];
                    break;
                default:
                    break;
            }
        }

        #region Component Designer generated code
        private System.ComponentModel.IContainer components;

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SizeMenuItem));
            this.m_imageList = new System.Windows.Forms.ImageList(this.components);
            // 
            // m_imageList
            // 
            this.m_imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.m_imageList.ImageSize = new System.Drawing.Size(80, 17);
            this.m_imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_imageList.ImageStream")));
            this.m_imageList.TransparentColor = System.Drawing.Color.Transparent;

        }
        #endregion
    }

    class FineSizeMenuItem   : SizeMenuItem
    {
        public FineSizeMenuItem() : base(PenMenuSize.Fine)
        {
        }
    }

    class MediumSizeMenuItem : SizeMenuItem
    {
        public MediumSizeMenuItem() : base(PenMenuSize.Medium)
        {
        }
    }

    class ThickSizeMenuItem  : SizeMenuItem
    {
        public ThickSizeMenuItem() : base(PenMenuSize.Thick)
        {
        }
    }
}
