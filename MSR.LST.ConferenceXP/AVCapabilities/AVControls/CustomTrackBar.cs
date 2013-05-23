using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// Summary description for CustomTrackBar.
    /// </summary>
    public class CustomTrackBar : System.Windows.Forms.UserControl
    {
        private System.Windows.Forms.PictureBox pbCursor;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CustomTrackBar));
            this.pbCursor = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // pbCursor
            // 
            this.pbCursor.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pbCursor.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbCursor.BackgroundImage")));
            this.pbCursor.Location = new System.Drawing.Point(0, 0);
            this.pbCursor.Name = "pbCursor";
            this.pbCursor.Size = new System.Drawing.Size(12, 13);
            this.pbCursor.TabIndex = 0;
            this.pbCursor.TabStop = false;
            this.pbCursor.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbCursor_MouseUp);
            this.pbCursor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbCursor_MouseMove);
            this.pbCursor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbCursor_MouseDown);
            // 
            // CustomTrackBar
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Controls.Add(this.pbCursor);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Name = "CustomTrackBar";
            this.Size = new System.Drawing.Size(74, 14);
            this.ResumeLayout(false);

        }
        #endregion

        #region Statics

        private const int cursorPosMin = 0;
        private const int cursorPosMax = 60;
        private const int normCursorMax = 100; // normalization max value

        #endregion Statics

        #region Members

        private bool isDragging = false;

        // Declare a form-level public event named Scroll
        // TODO: We could declare an event with object sender and EventArgs e
        //       instead of using the value and count on the accessor to get
        //       the value once the event is fired
        public delegate void ScrollHandler(int value);
        public new event ScrollHandler Scroll;

        #endregion Members

        #region Constructors

        public CustomTrackBar()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call
        }

        #endregion Constructors

        #region Public

        // Note: No need to create an accessor for the background image of the control: 
        //       it's already existing

        /// <summary>
        /// Allow to change the image of the cursor
        /// </summary>
        /// <remarks>
        /// In ConferenceXP we use that to dynamically change the color of the UI
        /// using GDI.NET
        /// </remarks>
        public Image CursorImage
        {
            get
            {
                return pbCursor.BackgroundImage;
            }
            set
            {
                pbCursor.BackgroundImage = value;
            }
        }

        /// <summary>
        /// Get or set the position of the cursor [0, 100]
        /// </summary>
        public int Value
        {
            get
            {
                // Return a normilized value [0, 100]
                return (int)(pbCursor.Left * normCursorMax/cursorPosMax);
            }
            set
            {
                ValidatePosition(value);
                // Set the cursor position from a normilized value [0, 100]
                pbCursor.Left = (int)(value * cursorPosMax/normCursorMax);
            }
        }

        #endregion Public

        #region Protected

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

        /// <summary>
        /// Ensure that the psoition is within the range [0, 100]
        /// </summary>
        /// <param name="volume">The position to validate</param>
        protected void ValidatePosition(int position)
        {
            if (position < cursorPosMin || position > normCursorMax)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, 
                    Strings.CursorPositionValueShouldBe, cursorPosMin, normCursorMax));
            }
        }

        #endregion Protected

        #region Private

        /// <summary>
        /// pbCursor MouseDown event handler. This will start the 
        /// move process.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void pbCursor_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Set the dragging flag
            isDragging = true;
        }

        /// <summary>
        /// pbCursor MouseUp event handler. This will end the 
        /// move process.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void pbCursor_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            isDragging = false;
        }

        /// <summary>
        /// pbCursor MouseMove event handler. This is during
        /// the move process of the cursor. It is used to send the 
        /// position of the picture box.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void pbCursor_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (isDragging == true)
            {
                int position = e.X + pbCursor.Left - pbCursor.Width/2;

                // Avoid that the cursor is moved outside of the permitted range
                if (position < cursorPosMin)
                {
                    position = cursorPosMin;
                }
                else if (position > cursorPosMax)
                {
                    position = cursorPosMax;
                }
                pbCursor.Left = position;

                // Fire an event with the new position
                if (Scroll != null)
                {
                    // Raise a form-level event and pass a normilized value
                    Scroll((int) (pbCursor.Left * normCursorMax/cursorPosMax));
                    this.Refresh();
                }
            }
        }

        #endregion Private
    }
}
