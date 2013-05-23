using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using MSR.LST.Controls;

using NativeMethods;


namespace MSR.LST.Controls.InkToolBarControls
{
    /// <summary>
    /// Represents a <c>ComboBox</c> that display colors.
    /// </summary>
    public class ColorComboBox : System.Windows.Forms.UserControl
    {
        private ColorPicker cp;
        private State m_state;
        private System.Windows.Forms.Button m_btn;
        private System.Windows.Forms.Panel  m_colorBox;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Event is raised when Color property has changed.
        /// </summary>
        public event EventHandler ColorChanged;

        enum State
        {
            Normal, Hot, Pressed, Disabled
        };

        /// <summary>
        /// ComboBox that display systema and web colors.
        /// </summary>
        public ColorComboBox()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // Subscibe to ColorPicker's events
            cp = new ColorPicker();
            cp.ColorChanged += new EventHandler(ColorPicker_ColorChanged);
            cp.Deactivate   += new EventHandler(ColorPicker_Deactivate);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if( components != null )
                    components.Dispose();
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
            this.m_btn = new System.Windows.Forms.Button();
            this.m_colorBox = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // m_btn
            // 
            this.m_btn.BackColor = System.Drawing.SystemColors.Control;
            this.m_btn.Location = new System.Drawing.Point(105, 1);
            this.m_btn.Name = "m_btn";
            this.m_btn.Size = new System.Drawing.Size(17, 19);
            this.m_btn.TabIndex = 2;
            this.m_btn.TabStop = false;
            this.m_btn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnBtnMouseUp);
            this.m_btn.Paint += new System.Windows.Forms.PaintEventHandler(this.OnBtnPaint);
            this.m_btn.MouseEnter += new System.EventHandler(this.OnBtnMouseEnter);
            this.m_btn.MouseLeave += new System.EventHandler(this.OnBtnMouseLeave);
            this.m_btn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnBtnMouseDown);
            // 
            // m_colorBox
            // 
            this.m_colorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_colorBox.Location = new System.Drawing.Point(3, 3);
            this.m_colorBox.Name = "m_colorBox";
            this.m_colorBox.Size = new System.Drawing.Size(101, 15);
            this.m_colorBox.TabIndex = 3;
            // 
            // ColorComboBox
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.m_colorBox,
                                                                          this.m_btn});
            this.Name = "ColorComboBox";
            this.Size = new System.Drawing.Size(123, 21);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// Gets or sets the selected color on the control.
        /// </summary>
        public Color Color
        {
            get { return m_colorBox.BackColor; }
            set 
            { 
                if (this.Enabled)
                    this.m_colorBox.BackColor = value; 
                else
                    this.m_colorBox.BackColor = SystemColors.ControlLight;
            }
        }

        private void ResetColor()
        {
            Color = SystemColors.Window;
        }

        private bool ShouldSerializeColor()
        {
            return Color != SystemColors.Window;
        }

        /// <summary>
        /// Raises the ColorChanged event.
        /// </summary>
        protected virtual void OnColorChanged()
        {
            if (ColorChanged != null)
                ColorChanged(this, new EventArgs());
        }

        private void ColorPicker_ColorChanged(object sender, EventArgs e)
        {
            ColorPicker picker = (ColorPicker) sender;

            if (picker.Visible)
            {
                m_colorBox.BackColor = picker.Color;
                OnColorChanged();
            }
        }

        private void ColorPicker_Deactivate(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void OnBtnMouseEnter(object sender, System.EventArgs e)
        {
            m_state = State.Hot;
            m_btn.Invalidate();
        }

        private void OnBtnMouseLeave(object sender, System.EventArgs e)
        {
            m_state = State.Normal;
            m_btn.Invalidate();
        }

        private void OnBtnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.m_state = State.Pressed;
            m_btn.Invalidate();

            DropDown();
        }

        private void DropDown()
        {
            //Display color picker
            if (!cp.Visible)
            {
                Point pt     = new Point(this.Left,this.Bottom + 1);
                cp.Location  = this.Parent.PointToScreen(pt);

                cp.Color = m_colorBox.BackColor;
                cp.Show();
            }
        }

        private void OnBtnMouseUp(object sender,  System.Windows.Forms.MouseEventArgs e)
        {
            m_state = State.Hot;
            m_btn.Invalidate();
        }

        /// <summary>
        /// Overrides OnPaint to draw the ComboBox.
        /// </summary>
        /// <param name="e">System.Windows.Forms.PaintEventArgs</param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawBorder(e.Graphics);
        }

        private void DrawFocusRect(Graphics g)
        {
            Pen p = new Pen(SystemColors.Window);

            Rectangle r = m_colorBox.Bounds;
            r.Width  += 1;
            r.Height += 1;
            r.Offset(-1, -1);

            if (this.ContainsFocus)
                p.Color = SystemColors.Highlight;
            else
                System.Diagnostics.Debug.WriteLine("Drawing Lost Focus");

            g.DrawRectangle(p, r);
        }

        private void DrawBorder(System.Drawing.Graphics g)
        {
            IntPtr hTheme = UxTheme.OpenThemeData(this.Handle, "EDIT");

            // Makes sure Windows XP is running and 
            //  a .manifest file exists for the EXE.
            if (Environment.OSVersion.Version.Major >= 5 
                && Environment.OSVersion.Version.Minor > 0 
                && System.IO.File.Exists(Application.ExecutablePath + ".manifest")
                && (hTheme != IntPtr.Zero))
            {
                //Get DC
                IntPtr hDC    = g.GetHdc();

                int state = (int) UxTheme.ETS_NORMAL;
                switch (this.m_state)
                {
                    case State.Disabled :
                        state = (int) UxTheme.ETS_DISABLED;
                        break;
                    default:
                        break;
                }

                try
                {
                    RECT r = new RECT(this.ClientRectangle.Left, this.ClientRectangle.Right, this.ClientRectangle.Top, this.ClientRectangle.Bottom);

                    //Render button
                    IntPtr hr = UxTheme.DrawThemeBackground( hTheme, hDC, UxTheme.EP_EDITTEXT, state, r,  null);
                }
                finally
                {
                    //Release DC
                    g.ReleaseHdc(hDC);
                    UxTheme.CloseThemeData(hTheme);
                }
            }
            else
            {
                using (Graphics y = this.CreateGraphics())
                {
                    ControlPaint.DrawBorder(y, ClientRectangle, SystemColors.Control, ButtonBorderStyle.Inset);
                }
            }
        }

        /// <summary>
        /// Overriden to update internal state.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnabledChanged(System.EventArgs e)
        {
            base.OnEnabledChanged(e);

            if (this.Enabled)
                this.m_state = State.Normal;
            else
                this.m_state = State.Disabled;
        }

        private void OnBtnPaint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            IntPtr hTheme = UxTheme.OpenThemeData(this.Handle, "COMBOBOX");

            // Makes sure Windows XP is running and 
            //  a .manifest file exists for the EXE.
            if (Environment.OSVersion.Version.Major >= 5 
                & Environment.OSVersion.Version.Minor > 0 
                & System.IO.File.Exists(Application.ExecutablePath + ".manifest")
                & (hTheme != IntPtr.Zero))
            {
                //Grab DC
                IntPtr hDC = e.Graphics.GetHdc();

                int state = UxTheme.CBXS_NORMAL;
                switch (this.m_state)
                {
                    case State.Hot :
                        state = UxTheme.CBXS_HOT;
                        break;
                    case State.Pressed :
                        state = UxTheme.CBXS_PRESSED;
                        break;
                    case State.Disabled :
                        state = UxTheme.CBXS_DISABLED;
                        break;
                    default:
                        break;
                }

                try
                {
                    RECT r = new RECT(e.ClipRectangle.Left, e.ClipRectangle.Right, e.ClipRectangle.Top, e.ClipRectangle.Bottom);

                    //Render button
                    UxTheme.DrawThemeBackground(hTheme, hDC, UxTheme.CP_DROPDOWN, state, r, null);
                }
                finally
                {
                    //Release DC
                    e.Graphics.ReleaseHdc(hDC);
                    UxTheme.CloseThemeData(hTheme);
                }
            }
            else
            {
                ButtonState state = ButtonState.Normal;
                switch (this.m_state)
                {
                    case State.Pressed :
                        state = ButtonState.Pushed;
                        break;
                    case State.Disabled :
                        state = ButtonState.Inactive;
                        break;
                    default:
                        break;
                }

                ControlPaint.DrawComboButton(e.Graphics, e.ClipRectangle, state);
            }
        }

        // Since UserControls don't have a KeyPreview property, we have to override
        // ProcessKeyPreview.

        /// <summary>
        /// Overridden to see if any child controls receive a WM_KEYDOWN message.
        /// </summary>
        /// <param name="m">Windows Message</param>
        /// <returns></returns>
        /// <remarks>
        /// Since UserControls don't have a KeyPreview property, we have to override ProcessKeyPreview.
        /// </remarks>
        protected override bool ProcessKeyPreview(ref System.Windows.Forms.Message m)
        {
            bool handled = false;

            if (m.Msg == WM_KEYDOWN)
            {
                switch((int)m.WParam)
                {
                    case 115: //F4
                        DropDown();
                        handled = true;
                        break;
                    default:
                        break;
                }
            }

            if (handled)
                return true;
            else
                return base.ProcessKeyPreview(ref m);
        }

        private const int WM_KEYDOWN = 0x0100;

        /// <summary>
        /// Overridden to catch the F4 key which opens and closes a combo dropdown.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                e.Handled = true;
                DropDown();
            }

            base.OnKeyDown(e);
        }
    }
}
