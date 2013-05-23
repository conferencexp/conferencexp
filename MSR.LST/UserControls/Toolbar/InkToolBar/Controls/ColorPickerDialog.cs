using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;


namespace MSR.LST.Controls.InkToolBarControls
{
    /// <summary>
    /// Represents the drop-down window of the ColorComboBox.
    /// </summary>
    internal class ColorPicker : System.Windows.Forms.Form
    {
        private System.Windows.Forms.TabControl m_tabControl;
        private System.Windows.Forms.TabPage m_webTab;
        private System.Windows.Forms.TabPage m_systemTab;
        private ColorListBox m_webList;
        private ColorListBox m_systemList;
        private System.Windows.Forms.Button m_btnOK;
        private System.Windows.Forms.Button m_btnCancel;
        private bool m_listMouseDown;
        private Color m_color;
        private bool m_colorChanged;
        internal event EventHandler ColorChanged;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        internal ColorPicker()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitForm call
            PopulateListBoxes();
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

        private void PopulateListBoxes()
        {
            // Populate system colors list box
            System.Reflection.PropertyInfo[] pInfos = typeof(SystemColors).GetProperties();

            foreach (System.Reflection.PropertyInfo p in pInfos)
                this.m_systemList.Items.Add(p.Name);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_tabControl = new System.Windows.Forms.TabControl();
            this.m_webTab = new System.Windows.Forms.TabPage();
            this.m_webList = new MSR.LST.Controls.InkToolBarControls.ColorListBox();
            this.m_systemTab = new System.Windows.Forms.TabPage();
            this.m_systemList = new MSR.LST.Controls.InkToolBarControls.ColorListBox();
            this.m_btnOK = new System.Windows.Forms.Button();
            this.m_btnCancel = new System.Windows.Forms.Button();
            this.m_tabControl.SuspendLayout();
            this.m_webTab.SuspendLayout();
            this.m_systemTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_tabControl
            // 
            this.m_tabControl.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                       this.m_webTab,
                                                                                       this.m_systemTab});
            this.m_tabControl.HotTrack = true;
            this.m_tabControl.Multiline = true;
            this.m_tabControl.Name = "m_tabControl";
            this.m_tabControl.SelectedIndex = 0;
            this.m_tabControl.Size = new System.Drawing.Size(208, 236);
            this.m_tabControl.TabIndex = 0;
            this.m_tabControl.TabStop = false;
            this.m_tabControl.Enter += new System.EventHandler(this.OnTabControl_Enter);
            this.m_tabControl.SelectedIndexChanged += new System.EventHandler(this.OnTabControl_SelectedIndexChanged);
            // 
            // m_webTab
            // 
            this.m_webTab.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                   this.m_webList});
            this.m_webTab.Location = new System.Drawing.Point(4, 22);
            this.m_webTab.Name = "m_webTab";
            this.m_webTab.Size = new System.Drawing.Size(200, 210);
            this.m_webTab.TabIndex = 0;
            this.m_webTab.Text = Strings.Web;
            // 
            // m_webList
            // 
            this.m_webList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_webList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.m_webList.Items.AddRange(new object[] {
                                                           Strings.Black,
                                                           Strings.DimGray,
                                                           Strings.Gray,
                                                           Strings.DarkGray,
                                                           Strings.Silver,
                                                           Strings.LightGray,
                                                           Strings.Gainsboro,
                                                           Strings.WhiteSmoke,
                                                           Strings.White,
                                                           Strings.RosyBrown,
                                                           Strings.IndianRed,
                                                           Strings.Brown,
                                                           Strings.Firebrick,
                                                           Strings.LightCoral,
                                                           Strings.Maroon,
                                                           Strings.DarkRed,
                                                           Strings.Red,
                                                           Strings.Snow,
                                                           Strings.MistyRose,
                                                           Strings.Salmon,
                                                           Strings.Tomato,
                                                           Strings.DarkSalmon,
                                                           Strings.Coral,
                                                           Strings.OrangeRed,
                                                           Strings.LightSalmon,
                                                           Strings.Sienna,
                                                           Strings.SeaShell,
                                                           Strings.Chocolate,
                                                           Strings.SaddleBrown,
                                                           Strings.SandyBrown,
                                                           Strings.PeachPuff,
                                                           Strings.Peru,
                                                           Strings.Linen,
                                                           Strings.Bisque,
                                                           Strings.DarkOrange,
                                                           Strings.BurlyWood,
                                                           Strings.Tan,
                                                           Strings.AntiqueWhite,
                                                           Strings.NavajoWhite,
                                                           Strings.BlanchedAlmond,
                                                           Strings.PapayaWhip,
                                                           Strings.Moccasin,
                                                           Strings.Orange,
                                                           Strings.Wheat,
                                                           Strings.Oldlace,
                                                           Strings.FloralWhite,
                                                           Strings.DarkGoldenrod,
                                                           Strings.Cornsilk,
                                                           Strings.Gold,
                                                           Strings.Khaki,
                                                           Strings.LemonChiffon,
                                                           Strings.PaleGoldenrod,
                                                           Strings.DarkKhaki,
                                                           Strings.Beige,
                                                           Strings.LightGoldenrodYellow,
                                                           Strings.Olive,
                                                           Strings.Yellow,
                                                           Strings.LightYellow,
                                                           Strings.Ivory,
                                                           Strings.OliveDrab,
                                                           Strings.YellowGreen,
                                                           Strings.DarkOliveGreen,
                                                           Strings.GreenYellow,
                                                           Strings.Chartreuse,
                                                           Strings.LawnGreen,
                                                           Strings.DarkSeaGreen,
                                                           Strings.ForestGreen,
                                                           Strings.LimeGreen,
                                                           Strings.PaleGreen,
                                                           Strings.DarkGreen,
                                                           Strings.Green,
                                                           Strings.Lime,
                                                           Strings.Honeydew,
                                                           Strings.SeaGreen,
                                                           Strings.MediumSeaGreen,
                                                           Strings.SpringGreen,
                                                           Strings.MintCream,
                                                           Strings.MediumSpringgreen,
                                                           Strings.MediumAquaMarine,
                                                           Strings.AquaMarine,
                                                           Strings.Turquoise,
                                                           Strings.LightSeaGreen,
                                                           Strings.MediumTurquoise,
                                                           Strings.DarkSlateGray,
                                                           Strings.PaleTurquoise,
                                                           Strings.Teal,
                                                           Strings.DarkCyan,
                                                           Strings.Aqua,
                                                           Strings.LightCyan,
                                                           Strings.Azure,
                                                           Strings.DarkTurquoise,
                                                           Strings.CadetBlue,
                                                           Strings.PowderBlue,
                                                           Strings.LightBlue,
                                                           Strings.DeepSkyBlue,
                                                           Strings.SkyBlue,
                                                           Strings.LightSkyBlue,
                                                           Strings.SteelBlue,
                                                           Strings.AliceBlue,
                                                           Strings.DodgerBlue,
                                                           Strings.SlateGray,
                                                           Strings.LightSlateGray,
                                                           Strings.LightSteelBlue,
                                                           Strings.CornflowerBlue,
                                                           Strings.RoyalBlue,
                                                           Strings.MidnightBlue,
                                                           Strings.Lavender,
                                                           Strings.Navy,
                                                           Strings.DarkBlue,
                                                           Strings.MediumBlue,
                                                           Strings.Blue,
                                                           Strings.GhostWhite,
                                                           Strings.SlateBlue,
                                                           Strings.DarkSlateBlue,
                                                           Strings.MediumSlateBlue,
                                                           Strings.MediumPurple,
                                                           Strings.BlueViolet,
                                                           Strings.Indigo,
                                                           Strings.DarkOrchid,
                                                           Strings.DarkViolet,
                                                           Strings.MediumOrchid,
                                                           Strings.Thistle,
                                                           Strings.Plum,
                                                           Strings.Violet,
                                                           Strings.Purple,
                                                           Strings.DarkMagenta,
                                                           Strings.Fuchsia,
                                                           Strings.Orchid,
                                                           Strings.MediumVioletRed,
                                                           Strings.DeepPink,
                                                           Strings.HotPink,
                                                           Strings.LavenderBlush,
                                                           Strings.PaleVioletRed,
                                                           Strings.Crimson,
                                                           Strings.Pink,
                                                           Strings.LightPink});
            this.m_webList.Name = "m_webList";
            this.m_webList.Size = new System.Drawing.Size(200, 210);
            this.m_webList.TabIndex = 0;
            this.m_webList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnList_MouseDown);
            this.m_webList.SelectedIndexChanged += new System.EventHandler(this.WebList_SelectedIndexChanged);
            // 
            // m_systemTab
            // 
            this.m_systemTab.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                      this.m_systemList});
            this.m_systemTab.Location = new System.Drawing.Point(4, 22);
            this.m_systemTab.Name = "m_systemTab";
            this.m_systemTab.Size = new System.Drawing.Size(200, 210);
            this.m_systemTab.TabIndex = 1;
            this.m_systemTab.Text = Strings.System;
            // 
            // m_systemList
            // 
            this.m_systemList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_systemList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.m_systemList.Name = "m_systemList";
            this.m_systemList.Size = new System.Drawing.Size(200, 210);
            this.m_systemList.TabIndex = 0;
            this.m_systemList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnList_MouseDown);
            this.m_systemList.SelectedIndexChanged += new System.EventHandler(this.SystemList_SelectedIndexChanged);
            // 
            // m_btnOK
            // 
            this.m_btnOK.Location = new System.Drawing.Point(48, 40);
            this.m_btnOK.Name = "m_btnOK";
            this.m_btnOK.TabIndex = 2;
            this.m_btnOK.TabStop = false;
            this.m_btnOK.Text = Strings.OK;
            this.m_btnOK.Click += new System.EventHandler(this.OnBtnOK_Click);
            // 
            // m_btnCancel
            // 
            this.m_btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_btnCancel.Location = new System.Drawing.Point(48, 72);
            this.m_btnCancel.Name = "m_btnCancel";
            this.m_btnCancel.TabIndex = 2;
            this.m_btnCancel.TabStop = false;
            this.m_btnCancel.Text = Strings.Cancel;
            this.m_btnCancel.Click += new System.EventHandler(this.OnBtnCancel_Click);
            // 
            // ColorPicker
            // 
            this.AcceptButton = this.m_btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.m_btnCancel;
            this.ClientSize = new System.Drawing.Size(200, 202);
            this.ControlBox = false;
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.m_tabControl});
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorPicker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.VisibleChanged += new System.EventHandler(this.OnVisibleChanged);
            this.m_tabControl.ResumeLayout(false);
            this.m_webTab.ResumeLayout(false);
            this.m_systemTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void WebList_SelectedIndexChanged(object source, EventArgs e)
        {
            if (m_webList.SelectedIndex >= 0)
            {
                m_color        = Color.FromName((string)m_webList.SelectedItem);
                m_colorChanged = true;
                if (m_listMouseDown)
                {
                    OnColorChanged();
                    this.Hide();
                }
            }
        }

        private void SystemList_SelectedIndexChanged(object source, EventArgs e)
        {
            if (m_systemList.SelectedIndex >= 0)
            {
                this.m_color   = Color.FromName((string)m_systemList.SelectedItem);
                m_colorChanged = true;
                if (m_listMouseDown)
                {
                    OnColorChanged();
                    this.Hide();
                }
            }
        }

        internal Color Color
        {
            get { return m_color; }
            set 
            { 
                m_color         = value;
                m_colorChanged  = false;

                //Look for color in the web list, otherwise in the system list.
                m_webList.SelectedIndex = m_webList.Items.IndexOf(m_color.Name);
                if (m_webList.SelectedIndex >= 0)
                    m_tabControl.SelectedTab = m_webTab;
                else
                {
                    m_systemList.SelectedIndex = m_systemList.Items.IndexOf(m_color.Name);
                    if (m_systemList.SelectedIndex >= 0 )
                        m_tabControl.SelectedTab = m_systemTab;
                }
            }
        }

        protected void OnColorChanged()
        {
            if (ColorChanged != null)
                ColorChanged(this, new EventArgs());
        }

        protected override void OnDeactivate(System.EventArgs e)
        {
            this.Hide();
            base.OnDeactivate(e);
        }

        private void OnBtnOK_Click(object sender, System.EventArgs e)
        {
            if (m_colorChanged)
                OnColorChanged();
            this.Hide();
        }

        private void OnBtnCancel_Click(object sender, System.EventArgs e)
        {
            this.Hide();
        }

        private void OnList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            m_listMouseDown = true;
        }

        private void OnVisibleChanged(object sender, System.EventArgs e)
        {
            m_listMouseDown = false;
        }

        private void OnTabControl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (m_tabControl.SelectedIndex == m_tabControl.TabPages.IndexOf(m_webTab))
                m_webList.Focus();
            else
                m_systemList.Focus();
        }

        private void OnTabControl_Enter(object sender, System.EventArgs e)
        {
            m_webList.Focus();
        }

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                if (m_colorChanged)
                    OnColorChanged();
                this.Hide();

                e.Handled = true;
            }

            base.OnKeyDown(e);
        }
    }
}
