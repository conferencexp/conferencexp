using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP
{
    internal class ThumbnailListViewItem
    {
        internal Image thumbnail;
        internal Guid tag;
    }

    internal class ThumbnailListView: Panel
    {
        private const int constScrollBarWidth = 15;
        private const int constNumberSpace = 7; // the total amount of space left for the numbers
        private const int constSmallMargin = 2; // the space left between each number and edge before it

        // data storage
        private ArrayList items = new ArrayList();
        public ArrayList picBoxes = new ArrayList();

        // state variables
        private bool horizontalMode = false;
        private int selection = -1;
        private int hoverIndex = -1;

        private Size pbSize = new Size(80, 60);
        private Font numFont;

        // events
        internal event EventHandler ImageSelected;
        internal event EventHandler ImageMouseEnter;
        internal event EventHandler ImageMouseLeave;

        internal ThumbnailListView()
        {
            this.Paint += new PaintEventHandler(ThumbnailViewer_Paint);
            this.SizeChanged += new EventHandler(ThumbnailListView_SizeChanged);

            this.AutoScroll = true;
            this.BackColor = SystemColors.Window;
            this.BorderStyle = BorderStyle.FixedSingle;

            this.numFont = new Font("Arial", 9, FontStyle.Bold);
        }

        protected override void Dispose(bool disposing)
        {
            if( disposing )
            {
                if( picBoxes != null )
                {
                    foreach( PictureBox pb in picBoxes )
                    {
                        if( pb.Image != null )
                            pb.Image.Dispose();
                        pb.Dispose();
                    }
                }
            }

            base.Dispose (disposing);
        }


        internal ThumbnailListViewItem[] Items
        {
            get
            {
                return (ThumbnailListViewItem[])items.ToArray(typeof(ThumbnailListViewItem));
            }
            set
            {
                ThumbnailListViewItem selectedItem = (selection == -1 ) ? null : (ThumbnailListViewItem)items[selection];
                items = new ArrayList(value);
                this.Controls.Clear();

                int cnt = 0;
                foreach(ThumbnailListViewItem item in items)
                {
                    Image img = item.thumbnail;

                    // reuse PictureBoxes, if possible
                    if( cnt < picBoxes.Count )
                        ((PictureBox)picBoxes[cnt]).Image = img;
                    else
                    {
                        // create a new PictureBox if necessary
                        PictureBox pb = new PictureBox();
                        pb.SizeMode = PictureBoxSizeMode.StretchImage;
                        pb.Click += new EventHandler(PictureBoxClick);
                        pb.MouseEnter += new EventHandler(PictureBoxMouseEnter);
                        pb.MouseLeave += new EventHandler(PictureBoxMouseLeave);
                        pb.Image = img;
                        pb.Size = pbSize;
                        picBoxes.Add(pb);
                    }

                    ++cnt;
                }

                // due to weird positioning issues, the picture boxes get added later.  
                //   this should reduce flicker-ish stuff, too
                this.Controls.AddRange((Control[])picBoxes.ToArray(typeof(Control)));

                // remove spare PictureBoxes
                while( cnt < picBoxes.Count )
                {
                    ((Control)picBoxes[cnt]).Dispose();
                    picBoxes.RemoveAt(cnt);
                }

                this.ArrangeSlides();

                // Design choice: if the same item is still [anywhere] in the list, keep it selected
                selection = items.IndexOf(selectedItem);

                this.Invalidate();
            }
        }

        internal Size ImageSize
        {
            get 
            {
                return pbSize;
            }
        }

        internal int HoverIndex
        {
            get
            {
                return hoverIndex;
            }
        }

        internal int SelectedIndex
        {
            get
            {
                return this.selection;
            }
            set
            {
                int oldSelection = selection;
                this.selection = value;
                if( value != -1 )
                {
                    this.ScrollControlIntoView((PictureBox)picBoxes[value]);
                    
                    // try to show one slide ahead, in the same direction we're moving
                    if( oldSelection != -1 )
                    {
                        if( oldSelection > value ) // going backwards
                        {
                            if( value > 0 )
                            {
                                this.ScrollControlIntoView((PictureBox)picBoxes[value-1]);
                            }
                        }
                        else // moving forwards (or not at all)
                        {
                            if( value+1 < picBoxes.Count )
                            {
                                this.ScrollControlIntoView((PictureBox)picBoxes[value+1]);
                            }
                        }
                    }
                }
                this.Invalidate();
            }
        }

        internal bool Horizontal
        {
            get
            {
                return this.horizontalMode;
            }
            set
            {
                if( value != this.horizontalMode )
                {
                    this.horizontalMode = value;

                    ArrangeSlides();
                }
            }
        }

        internal void InsertThumbnail( Image image, string slideTitle, int index, Guid tag )
        {
            lock (this)
            {
                if( index < 0 || picBoxes.Count < index )
                    throw new ArgumentOutOfRangeException(Strings.IndexMustBeWithinTheCollection);

                Image thumb = CreateSlideThumbnail( pbSize, image, slideTitle );

                ThumbnailListViewItem item = new ThumbnailListViewItem();
                item.thumbnail = thumb;
                item.tag = tag;

                this.items.Insert(index, item);
            
                PictureBox pb = new PictureBox();
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Click += new EventHandler(PictureBoxClick);
                pb.MouseEnter += new EventHandler(PictureBoxMouseEnter);
                pb.MouseLeave += new EventHandler(PictureBoxMouseLeave);
                pb.Image = thumb;
                pb.Size = pbSize;

                this.Controls.Add(pb);
                picBoxes.Insert(index, pb);

                #region Add picture box at the appropriate place
                if( picBoxes.Count == 1 ) // this is the first picBox in the arr
                {
                    // place it at "0,0" (so to speak)
                    ArrangeSlides();  // this should do that without causing problems
                }
                else // not the only one.  Easy enough
                {
                    // place picBox @ location of the previous one at this location
                    // move all picBoxes at & after this one down "one" spot

                    // <cough>... now we play the double-pointer iteration game.
                    PictureBox crnt;
                    if( index != 0 )
                        crnt = (PictureBox)picBoxes[index-1];
                    else
                        crnt = null;

                    PictureBox next = pb;
                    for( int cnt = index+1; cnt < picBoxes.Count; ++cnt )
                    {
                        crnt = next;
                        next = (PictureBox)picBoxes[cnt];

                        crnt.Location = next.Location;
                    }

                    // finally we set the last one to a new location
                    int xLoc, yLoc;
                    if( horizontalMode )
                    {
                        xLoc = crnt.Right + 2*AutoScrollMargin.Width + constNumberSpace;
                        yLoc = (this.Height - pbSize.Height - constScrollBarWidth) / 2;
                    }
                    else
                    {
                        xLoc = constNumberSpace + (this.Width - pbSize.Width - constScrollBarWidth) / 2;
                        yLoc = crnt.Bottom + 2*AutoScrollMargin.Height;
                    }

                    next.Location = new Point(xLoc, yLoc);
                }
                #endregion

                this.Invalidate();
            }
        }

        public static Image CreateSlideThumbnail( Size size, Image original, string slideTitle )
        {
            Bitmap bmp = new Bitmap( size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb );
            using( Graphics g = Graphics.FromImage(bmp) )
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                CreateSlideThumbnail( size, original, g, slideTitle );
            }
            return bmp;
        }

        public static void CreateSlideThumbnail( Size size, Image original, Graphics g, string slideTitle )
        {
            g.Clear(Color.White);

            if( original != null )
            {
                // Center the image
                float startX = 0F, startY = 0F;

                // Try to maintain the aspect ratio of the image
                SizeF drawSize = (SizeF)size;
                if( drawSize.Width / (float)original.Width > drawSize.Height / (float)original.Height )
                {
                    // this means there's free-space on the sides
                    drawSize.Width = (float)original.Width * (drawSize.Height / (float)original.Height);
                    startX = ((float)size.Width - drawSize.Width)/2;
                }
                else
                {
                    // there may be some free-space on the top & bottom
                    drawSize.Height = (float)original.Height * (drawSize.Width / (float)original.Width);
                    startY = ((float)size.Height - drawSize.Height)/2;
                }

                g.DrawImage( original, startX, startY, drawSize.Width, drawSize.Height );
            }
        }

        /// <summary>
        /// Create dynamically a bitmap image with the text passed in parameter using GDI.NET.
        /// This function is used to create the "Page not found!" page image. This function
        /// is used in the ShowPage method.
        /// </summary>
        /// <remarks>This version has not been tested on max number of characters and displays
        /// only in one line. If the text is too long, part of the text might not
        /// be displayed.</remarks>
        /// <param name="text">Text to be display</param>
        /// <returns>The bitmap containing the generated image</returns>
        public static Bitmap GenerateBitmapAlertText(string text)
        {
            // You can create an empty bitmap...
            Bitmap bmp = new Bitmap(320, 240);
            using(Graphics g = Graphics.FromImage(bmp))
            {

                // Create a gradient background
                System.Drawing.Drawing2D.LinearGradientBrush linearGradientBrush = 
                    new System.Drawing.Drawing2D.LinearGradientBrush(
                    new Rectangle(0, 0, 320, 240),
                    Color.White, Color.LightBlue, 90f);
                g.FillRectangle(linearGradientBrush, 0, 0, 320, 240);

                // Antialiasing for better text-rendering quality
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                // Draw font
                FontFamily fontFamily = new FontFamily("Times New Roman");
                Font font = new Font(fontFamily, 28);

                // Get width of text in order to center in the image
                SizeF textWidth = g.MeasureString(text, font, Int32.MaxValue, StringFormat.GenericTypographic);

                // TODO: This version does not include word wrapping.
                // Make a "3D shadow effet" by drawing twice the font 
                // (shadow in gray, font in red offset by 2 pixels)
                g.DrawString(text, font, Brushes.Gray, (float)(320/2 - textWidth.Width/2), 
                    (float) (240/2 - textWidth.Height), StringFormat.GenericTypographic);
                g.DrawString(text, font, Brushes.Red, (float)(320/2 - textWidth.Width/2 - 2),
                    (float) (240/2 - textWidth.Height - 2), StringFormat.GenericTypographic);
            }

            return bmp;
        }

        private void ArrangeSlides()
        {
            // After the scroll bars appear, controls can be at negative locations
            //  In other words, (0,0) becomes the top left of the *visible* area, so
            //  this method should never get called unless we're at the top left
            // Call InsertThumbnail to append/add/insert thumbnails if there's any chance
            //  of having the scrollbars at hte top-/left-most position

            int yLoc = -1, xLoc = -1;
            if( horizontalMode )
                yLoc = (this.Height - ImageSize.Height - constScrollBarWidth) / 2;
            else
                xLoc = constNumberSpace + (this.Width - ImageSize.Width - constScrollBarWidth) / 2;

            PictureBox pb;
            for(int cnt = 0; cnt < picBoxes.Count; ++cnt)
            {
                pb = ((PictureBox)picBoxes[cnt]);

                if( horizontalMode )
                {
                    xLoc = (cnt == 0) ? AutoScrollMargin.Width + constNumberSpace : 
                        ((PictureBox)picBoxes[cnt-1]).Right + 2*AutoScrollMargin.Width + constNumberSpace;
                }
                else
                {
                    yLoc = (cnt == 0) ? AutoScrollMargin.Height : 
                        ((PictureBox)picBoxes[cnt-1]).Bottom + 2*AutoScrollMargin.Height;
                }

                pb.Location = new Point(xLoc, yLoc);
            }
        }

        private void ThumbnailViewer_Paint(object sender, PaintEventArgs e)
        {
            using(Graphics g = e.Graphics)
            {
                if( picBoxes.Count != 0 )
                {
                    // draw the slide # at the top left corner of the slide
                    int xLoc, yLoc; // represent the top left corner of the string
                    xLoc = yLoc = constSmallMargin;

                    for( int cnt = 0; cnt < picBoxes.Count; ++cnt )
                    {
                        PictureBox pb = ((PictureBox)picBoxes[cnt]);
                        string num = (cnt + 1).ToString(CultureInfo.InvariantCulture);

                        // Draw a pretty line around the pictureBox
                        g.DrawRectangle( Pens.Black, pb.Location.X-1, pb.Location.Y-1, pb.Width+1, pb.Height+1 );
                        // Blatent abuse of the GC?  What?  <plays coy on the issue>
                        g.DrawLines( new Pen(Color.DarkGray, 2), new Point[] {new Point(pb.Left+1, pb.Bottom+2),
                                                                                 new Point(pb.Right+2, pb.Bottom+2), new Point(pb.Right+2, pb.Top+1)} );

                        if( horizontalMode )
                        {
                            xLoc = ((PictureBox)picBoxes[cnt]).Left - constSmallMargin - constNumberSpace;
                        }
                        else
                        {
                            yLoc = ((PictureBox)picBoxes[cnt]).Top;
                        }

                        g.DrawString( num, this.numFont, Brushes.Black, xLoc, yLoc );
                    }
                }

                // draw selection rectange (blue, 2F width)
                if( selection != -1 )
                {
                    PictureBox selectedItem = ((PictureBox)picBoxes[selection]);
                    g.DrawRectangle( Pens.Blue,
                        selectedItem.Left-3, selectedItem.Top-3,
                        selectedItem.Width+6, selectedItem.Height+6 );
                }

            }
        }

        private void PictureBoxClick(object sender, EventArgs ea)
        {
            SelectedIndex = picBoxes.IndexOf(sender);

            if( this.ImageSelected != null )
                ImageSelected( this, EventArgs.Empty );
        }

        private void PictureBoxMouseEnter(object sender, EventArgs ea)
        {
            hoverIndex = picBoxes.IndexOf(sender);

            if( this.ImageMouseEnter != null )
                ImageMouseEnter( this, EventArgs.Empty );
        }

        private void PictureBoxMouseLeave(object sender, EventArgs ea)
        {
            if( this.ImageMouseLeave != null )
                ImageMouseLeave( this, EventArgs.Empty );
        }

        private void pb_LocationChanged(object sender, EventArgs e)
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Location of pb changed to: {0}", 
                ((PictureBox)sender).Location));
        }

        private void ThumbnailListView_SizeChanged(object sender, EventArgs e)
        {
            // we need to prevent re-arranging the slides before the scroll bar is not at the top/left
            //  (due to issues with the controls needing to be at negative locations)
            if( picBoxes.Count <= 1 )
                this.ArrangeSlides();
        }


    } // end of class
} // end of namespace
