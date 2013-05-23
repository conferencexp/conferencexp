using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;


namespace MSR.LST.Controls.InkToolBarControls
{
    internal enum PenMenuSize {Fine, Medium, Thick};

    abstract class ImageMenuItem : System.Windows.Forms.MenuItem
    {
        private System.Drawing.Font              m_font;
        private bool                             m_alignLeft;
        private Image                            m_image;
        private Color                            m_color;
        private System.ComponentModel.IContainer components;

        public ImageMenuItem(string text, System.Drawing.Image image, bool alignImageLeft) : base(text)
        {
            InitializeComponent();

            m_image     = image;
            m_alignLeft = alignImageLeft;

            InitializeFont();
        }

        private void InitializeFont()
        {
            m_font = SystemInformation.MenuFont;
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ImageMenuItem));
            // 
            // ImageMenuItem
            // 
            this.OwnerDraw = true;

        }
        #endregion

        internal System.Drawing.Image Image
        {
            get { return m_image;  }
            set { m_image = value; }
        }

        public System.Drawing.Color Color
        {
            set { this.m_color = value; }
            get { return this.m_color;  }
        }

        protected override void OnMeasureItem(System.Windows.Forms.MeasureItemEventArgs e)
        {
            if (this.Visible)
            {
                base.OnMeasureItem(e);

                StringFormat strfmt = new StringFormat();
                strfmt.HotkeyPrefix = HotkeyPrefix.Show;

                SizeF sizef  = e.Graphics.MeasureString(this.Text, this.m_font, 1000, strfmt);

                e.ItemWidth  = (int) Math.Ceiling(sizef.Width);
                e.ItemHeight = 17;

                // Add scale adjusted width.
                e.ItemWidth += (int) Math.Ceiling(SystemInformation.MenuCheckSize.Width * 
                    (double) e.ItemHeight / SystemInformation.MenuCheckSize.Height);

                // Comment out this line if the menu group does not contain any standard MenuItems
                //e.ItemWidth -= SystemInformation.MenuCheckSize.Width;

                e.ItemWidth += (int) Math.Ceiling(m_image.Width * (double) e.ItemHeight / m_image.Height);
            }
        }

        protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
        {
            if (this.Visible)
            {
                base.OnDrawItem(e);

                // Show/Hide the accelerator
                StringFormat strfmt     = new StringFormat();
                if ((e.State & DrawItemState.NoAccelerator) == 0)
                    strfmt.HotkeyPrefix = HotkeyPrefix.Show;
                else
                    strfmt.HotkeyPrefix = HotkeyPrefix.Hide;

                //Calculate check mark and text rectangles
                Rectangle rectCheck = e.Bounds;
                rectCheck.Width     = (int) Math.Ceiling(SystemInformation.MenuCheckSize.Width * 
                    (double) rectCheck.Height / SystemInformation.MenuCheckSize.Height);

                int buffer = (rectCheck.Width / 2);

                Rectangle rectText  = e.Bounds;
                Rectangle rectImage = e.Bounds;
                rectImage.Width = (int) Math.Ceiling(m_image.Width * (double) rectImage.Height / m_image.Height);

                if (m_alignLeft)
                {
                    rectImage.X    += rectCheck.Width;

                    rectText.X      = rectImage.X + rectImage.Width + buffer;
                    rectText.Width -= rectCheck.Width + rectImage.Width;
                }
                else
                {
                    rectText.X     += rectCheck.Width;
                    rectText.Width -= rectCheck.Width + rectImage.Width + buffer;

                    rectImage.X     = rectText.X + rectText.Width;
                    rectImage.Width = (int) Math.Ceiling(m_image.Width * (double) e.Bounds.Height / m_image.Height);
                }


                //
                // Time to draw...
                //

                //Selected item color for rendering image backgrounds
                Color foreColor, backColor;
                Brush foreBrush, backBrush;

                if ((e.State & DrawItemState.Selected) != 0)
                {
                    foreColor = SystemColors.HighlightText;
                    backColor = SystemColors.Highlight;
                }
                else
                {
                    foreColor = SystemColors.MenuText;
                    backColor = SystemColors.Menu;
                }

                foreBrush = SystemBrushes.FromSystemColor(foreColor);
                backBrush = SystemBrushes.FromSystemColor(backColor);

                //Fill background
                e.Graphics.FillRectangle(backBrush, e.Bounds);

                //Draw glyph
                if ((e.State & DrawItemState.Checked) != 0)
                    DrawMenuGlyph(e.Graphics, rectCheck, foreColor, backColor);

                //Draw text
                e.Graphics.DrawString(this.Text, m_font, foreBrush, rectText, strfmt);

                //TODO:
                //Currently the back color being swapped is White, if one of the fore colors being displayed 
                //was actually white then it would also get swapped to the background color.  Need to
                //make the color to be swapped Color.Transparent.
                DrawImage(e.Graphics, m_image, rectImage, Color.Red, m_color, Color.White, backColor);
            }
        }

        /// <summary>
        /// Draws a menu glyph with the specified colors.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect">The <c>System.Drawing.Graphics</c> object to draw on.</param>
        /// <param name="foreColor">The <c>System.Drawing.Color</c> that represents the foreground color.</param>
        /// <param name="backColor">The <c>System.Drawing.Color</c> that represents the background color.</param>
        private void DrawMenuGlyph(Graphics g, Rectangle rect, Color foreColor, Color backColor)
        {
            Graphics offscreen      = null;
            Bitmap image            = null;
            Rectangle offsceenRect  = new Rectangle(0, 0, rect.Width, rect.Height);

            //
            // Render glyph to offscreen bitmap, then swap colors
            //
            try 
            {
                image     = new Bitmap(rect.Width, rect.Height);
                offscreen = Graphics.FromImage(image);

                ControlPaint.DrawMenuGlyph(offscreen, offsceenRect, this.RadioCheck ? MenuGlyph.Bullet : MenuGlyph.Checkmark);

                DrawImage(g, image, rect, Color.White, backColor, Color.Black, foreColor);
            }
            finally 
            {
                if (offscreen != null) offscreen.Dispose();
                if (image != null) image.Dispose();
            }
        }

        /// <summary>
        /// Draws an image in the menu.
        /// </summary>
        /// <param name="g">The <c>System.Drawing.Graphics</c> object to draw on.</param>
        /// <param name="baseImage">The <c>System.Drawing.Image</c> object to draw.</param>
        /// <param name="rectDraw">The <c>System.Drawing.Rectangle</c> structure that specifies the 
        /// location and size of the drawn image.</param>
        /// <param name="oldColor1">A <c>System.Drawing.Color</c> that represents the color to be swapped with <c>newColor1</c></param>
        /// <param name="newColor1">A <c>System.Drawing.Color</c> that represents the color to replace <c>oldColor1</c></param>
        /// <param name="oldColor2">A <c>System.Drawing.Color</c> that represents the color to be swapped with <c>newColor2</c></param>
        /// <param name="newColor2">A <c>System.Drawing.Color</c> that represents the color to replace <c>oldColor2</c></param>
        private void DrawImage(Graphics g, Image baseImage, Rectangle rectDraw, Color oldColor1, Color newColor1, Color oldColor2, Color newColor2)
        {
            ImageAttributes attributes = new ImageAttributes(); 
 
            ColorMap[] colorMap = new ColorMap[2]{ new ColorMap(), new ColorMap() }; 

            //For color images, render inner color from name
            colorMap[0].OldColor = oldColor1; 
            colorMap[0].NewColor = newColor1;    

            //Render background
            colorMap[1].OldColor = oldColor2; 
            colorMap[1].NewColor = newColor2;                     
  
            attributes.SetRemapTable(colorMap);                     
  
            g.DrawImage(baseImage, rectDraw, 0, 0, rectDraw.Width, rectDraw.Height, GraphicsUnit.Pixel, attributes);
        }
    }
}
