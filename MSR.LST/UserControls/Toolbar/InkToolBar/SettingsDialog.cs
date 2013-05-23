using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.Ink;


namespace MSR.LST.Controls.InkToolBarControls
{
    /// <summary>
    /// Represents a dialog box for modifying pen or highlighter settings for the <c>InkToolBar</c> 
    /// control.
    /// </summary>
    /// <remarks>
    /// The <c>SettingsDialog</c> class is a modal form invoked from the drop down menus of the 
    /// <c>InkToolBar</c> control.  It enables the user to adjust attributes like color, size, smoothness,
    /// transparency and tip.  It also contains an <c>InkPicture</c> test area to try out settings
    /// before saving them to the main application.
    /// </remarks>
    internal class SettingsDialog : System.Windows.Forms.Form
    {
        private Microsoft.Ink.InkPicture inkPicture;
        private System.Windows.Forms.CheckBox m_fitToCurve;
        private System.Windows.Forms.CheckBox m_antialias;
        private System.Windows.Forms.RadioButton m_ballRadio;
        private System.Windows.Forms.RadioButton m_rectangleRadio;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button m_clearBtn;
        private System.Windows.Forms.Button m_cancelBtn;
        private System.Windows.Forms.Button m_okBtn;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown m_heightUpDown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown m_widthUpDown;
        private System.Windows.Forms.CheckBox m_pressureCheck;
        private static int[] m_customColors;
        private System.Windows.Forms.Button m_customBtn;
        private System.Windows.Forms.TrackBar m_transparencyBar;
        private MSR.LST.Controls.InkToolBarControls.PercentUpDown m_percentUpDown;
        private System.Windows.Forms.PictureBox iconTip;
        private System.Windows.Forms.PictureBox iconTransparency;
        private System.Windows.Forms.PictureBox iconColor;
        private MSR.LST.Controls.InkToolBarControls.ColorComboBox m_colorBox;
        private System.Windows.Forms.CheckBox m_optimizeForHighlighter;
        private System.Windows.Forms.Label m_transpLabel;
        private System.Windows.Forms.Label m_smoothLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Initializes a new instance of the SettingsDialog class.
        /// </summary>
        /// <param name="da">Used to initilize the form's settings.</param>
        public SettingsDialog(Microsoft.Ink.DrawingAttributes da)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            InitializeCustomControls();

            this.inkPicture.DefaultDrawingAttributes = da;
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SettingsDialog));
            this.m_customBtn = new System.Windows.Forms.Button();
            this.m_fitToCurve = new System.Windows.Forms.CheckBox();
            this.inkPicture = new Microsoft.Ink.InkPicture();
            this.m_ballRadio = new System.Windows.Forms.RadioButton();
            this.m_rectangleRadio = new System.Windows.Forms.RadioButton();
            this.m_antialias = new System.Windows.Forms.CheckBox();
            this.m_clearBtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.m_transparencyBar = new System.Windows.Forms.TrackBar();
            this.m_cancelBtn = new System.Windows.Forms.Button();
            this.m_okBtn = new System.Windows.Forms.Button();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.iconTip = new System.Windows.Forms.PictureBox();
            this.iconTransparency = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.m_transpLabel = new System.Windows.Forms.Label();
            this.m_smoothLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_pressureCheck = new System.Windows.Forms.CheckBox();
            this.m_heightUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.m_widthUpDown = new System.Windows.Forms.NumericUpDown();
            this.iconColor = new System.Windows.Forms.PictureBox();
            this.m_colorBox = new MSR.LST.Controls.InkToolBarControls.ColorComboBox();
            this.m_optimizeForHighlighter = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.m_transparencyBar)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_heightUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_widthUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // m_customBtn
            // 
            this.m_customBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_customBtn.Location = new System.Drawing.Point(264, 11);
            this.m_customBtn.Name = "m_customBtn";
            this.m_customBtn.TabIndex = 1;
            this.m_customBtn.Text = "Custom...";
            this.m_customBtn.Click += new System.EventHandler(this.CustomBtn_Click);
            // 
            // m_fitToCurve
            // 
            this.m_fitToCurve.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_fitToCurve.Location = new System.Drawing.Point(136, 284);
            this.m_fitToCurve.Name = "m_fitToCurve";
            this.m_fitToCurve.Size = new System.Drawing.Size(80, 24);
            this.m_fitToCurve.TabIndex = 10;
            this.m_fitToCurve.Text = "Fit to curve";
            this.m_fitToCurve.CheckedChanged += new System.EventHandler(this.FitToCurve_CheckedChanged);
            // 
            // inkPicture
            // 
            this.inkPicture.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.inkPicture.BackColor = System.Drawing.SystemColors.Window;
            this.inkPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inkPicture.Location = new System.Drawing.Point(8, 38);
            this.inkPicture.MarginX = -2147483648;
            this.inkPicture.MarginY = -2147483648;
            this.inkPicture.Name = "inkPicture";
            this.inkPicture.Size = new System.Drawing.Size(236, 252);
            this.inkPicture.TabIndex = 7;
            this.inkPicture.Stroke += new Microsoft.Ink.InkCollectorStrokeEventHandler(this.OnInkPicture_Stroke);
            // 
            // m_ballRadio
            // 
            this.m_ballRadio.Checked = true;
            this.m_ballRadio.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_ballRadio.Location = new System.Drawing.Point(136, 132);
            this.m_ballRadio.Name = "m_ballRadio";
            this.m_ballRadio.Size = new System.Drawing.Size(72, 24);
            this.m_ballRadio.TabIndex = 4;
            this.m_ballRadio.TabStop = true;
            this.m_ballRadio.Text = "Ball";
            this.m_ballRadio.CheckedChanged += new System.EventHandler(this.RoundRadio_CheckedChanged);
            // 
            // m_rectangleRadio
            // 
            this.m_rectangleRadio.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_rectangleRadio.Location = new System.Drawing.Point(136, 152);
            this.m_rectangleRadio.Name = "m_rectangleRadio";
            this.m_rectangleRadio.Size = new System.Drawing.Size(72, 24);
            this.m_rectangleRadio.TabIndex = 5;
            this.m_rectangleRadio.TabStop = true;
            this.m_rectangleRadio.Text = "Rectangle";
            this.m_rectangleRadio.CheckedChanged += new System.EventHandler(this.RectangleRadio_CheckedChanged);
            // 
            // m_antialias
            // 
            this.m_antialias.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_antialias.Location = new System.Drawing.Point(136, 262);
            this.m_antialias.Name = "m_antialias";
            this.m_antialias.Size = new System.Drawing.Size(80, 24);
            this.m_antialias.TabIndex = 9;
            this.m_antialias.Text = "Antialiased";
            this.m_antialias.CheckedChanged += new System.EventHandler(this.Antialias_CheckedChanged);
            // 
            // m_clearBtn
            // 
            this.m_clearBtn.BackColor = System.Drawing.SystemColors.ControlDark;
            this.m_clearBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_clearBtn.Location = new System.Drawing.Point(169, 8);
            this.m_clearBtn.Name = "m_clearBtn";
            this.m_clearBtn.TabIndex = 0;
            this.m_clearBtn.Text = "Clear";
            this.m_clearBtn.Click += new System.EventHandler(this.ClearBtn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox3.Location = new System.Drawing.Point(7, 248);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(339, 8);
            this.groupBox3.TabIndex = 51;
            this.groupBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox4.Location = new System.Drawing.Point(7, 118);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(339, 8);
            this.groupBox4.TabIndex = 52;
            this.groupBox4.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox5.Location = new System.Drawing.Point(7, 39);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(339, 8);
            this.groupBox5.TabIndex = 53;
            this.groupBox5.TabStop = false;
            // 
            // m_transparencyBar
            // 
            this.m_transparencyBar.BackColor = System.Drawing.SystemColors.Control;
            this.m_transparencyBar.LargeChange = 10;
            this.m_transparencyBar.Location = new System.Drawing.Point(126, 75);
            this.m_transparencyBar.Maximum = 100;
            this.m_transparencyBar.Name = "m_transparencyBar";
            this.m_transparencyBar.Size = new System.Drawing.Size(136, 45);
            this.m_transparencyBar.TabIndex = 3;
            this.m_transparencyBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.m_transparencyBar.ValueChanged += new System.EventHandler(this.TransparencyBar_ValueChanged);
            // 
            // m_cancelBtn
            // 
            this.m_cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_cancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_cancelBtn.Location = new System.Drawing.Point(537, 312);
            this.m_cancelBtn.Name = "m_cancelBtn";
            this.m_cancelBtn.Size = new System.Drawing.Size(72, 23);
            this.m_cancelBtn.TabIndex = 12;
            this.m_cancelBtn.Text = "&Cancel";
            // 
            // m_okBtn
            // 
            this.m_okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_okBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_okBtn.Location = new System.Drawing.Point(454, 312);
            this.m_okBtn.Name = "m_okBtn";
            this.m_okBtn.TabIndex = 13;
            this.m_okBtn.Text = "&OK";
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.Image = ((System.Drawing.Bitmap)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(16, 273);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(24, 24);
            this.pictureBox4.TabIndex = 47;
            this.pictureBox4.TabStop = false;
            // 
            // iconTip
            // 
            this.iconTip.BackColor = System.Drawing.Color.Transparent;
            this.iconTip.BackgroundImage = ((System.Drawing.Bitmap)(resources.GetObject("iconTip.BackgroundImage")));
            this.iconTip.Location = new System.Drawing.Point(16, 140);
            this.iconTip.Name = "iconTip";
            this.iconTip.Size = new System.Drawing.Size(24, 24);
            this.iconTip.TabIndex = 48;
            this.iconTip.TabStop = false;
            // 
            // iconTransparency
            // 
            this.iconTransparency.BackColor = System.Drawing.Color.Transparent;
            this.iconTransparency.Image = ((System.Drawing.Bitmap)(resources.GetObject("iconTransparency.Image")));
            this.iconTransparency.Location = new System.Drawing.Point(16, 85);
            this.iconTransparency.Name = "iconTransparency";
            this.iconTransparency.Size = new System.Drawing.Size(24, 24);
            this.iconTransparency.TabIndex = 50;
            this.iconTransparency.TabStop = false;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label5.Location = new System.Drawing.Point(48, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 23);
            this.label5.TabIndex = 45;
            this.label5.Text = "Tip :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label1.Location = new System.Drawing.Point(48, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Color :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_transpLabel
            // 
            this.m_transpLabel.BackColor = System.Drawing.SystemColors.Control;
            this.m_transpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.m_transpLabel.Location = new System.Drawing.Point(48, 86);
            this.m_transpLabel.Name = "m_transpLabel";
            this.m_transpLabel.Size = new System.Drawing.Size(80, 23);
            this.m_transpLabel.TabIndex = 55;
            this.m_transpLabel.Text = "Transparency :";
            this.m_transpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_smoothLabel
            // 
            this.m_smoothLabel.BackColor = System.Drawing.SystemColors.Control;
            this.m_smoothLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.m_smoothLabel.Location = new System.Drawing.Point(48, 273);
            this.m_smoothLabel.Name = "m_smoothLabel";
            this.m_smoothLabel.Size = new System.Drawing.Size(80, 23);
            this.m_smoothLabel.TabIndex = 49;
            this.m_smoothLabel.Text = "Smoothness :";
            this.m_smoothLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 23);
            this.label3.TabIndex = 8;
            this.label3.Text = "Test";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right);
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                 this.m_clearBtn,
                                                                                 this.inkPicture,
                                                                                 this.label3});
            this.panel1.Location = new System.Drawing.Point(357, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(252, 298);
            this.panel1.TabIndex = 11;
            // 
            // m_pressureCheck
            // 
            this.m_pressureCheck.Checked = true;
            this.m_pressureCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_pressureCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_pressureCheck.Location = new System.Drawing.Point(232, 140);
            this.m_pressureCheck.Name = "m_pressureCheck";
            this.m_pressureCheck.TabIndex = 6;
            this.m_pressureCheck.Text = "Ignore pressure";
            this.m_pressureCheck.CheckedChanged += new System.EventHandler(this.PressureCheck_CheckedChanged);
            // 
            // m_heightUpDown
            // 
            this.m_heightUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_heightUpDown.DecimalPlaces = 1;
            this.m_heightUpDown.Increment = new System.Decimal(new int[] {
                                                                             1,
                                                                             0,
                                                                             0,
                                                                             65536});
            this.m_heightUpDown.Location = new System.Drawing.Point(188, 194);
            this.m_heightUpDown.Maximum = new System.Decimal(new int[] {
                                                                           10,
                                                                           0,
                                                                           0,
                                                                           0});
            this.m_heightUpDown.Minimum = new System.Decimal(new int[] {
                                                                           1,
                                                                           0,
                                                                           0,
                                                                           65536});
            this.m_heightUpDown.Name = "m_heightUpDown";
            this.m_heightUpDown.Size = new System.Drawing.Size(49, 20);
            this.m_heightUpDown.TabIndex = 7;
            this.m_heightUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.m_heightUpDown.Value = new System.Decimal(new int[] {
                                                                         10,
                                                                         0,
                                                                         0,
                                                                         0});
            this.m_heightUpDown.ValueChanged += new System.EventHandler(this.HeightUpDown_ValueChanged);
            this.m_heightUpDown.Leave += new System.EventHandler(this.UpDown_Leave);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(136, 193);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 23);
            this.label6.TabIndex = 44;
            this.label6.Text = "Height";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(136, 221);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 23);
            this.label7.TabIndex = 43;
            this.label7.Text = "Width";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(7, 175);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(339, 8);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox5.Image = ((System.Drawing.Bitmap)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(16, 205);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(24, 24);
            this.pictureBox5.TabIndex = 41;
            this.pictureBox5.TabStop = false;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label8.Location = new System.Drawing.Point(48, 205);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 23);
            this.label8.TabIndex = 40;
            this.label8.Text = "Size :";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_widthUpDown
            // 
            this.m_widthUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_widthUpDown.DecimalPlaces = 1;
            this.m_widthUpDown.Increment = new System.Decimal(new int[] {
                                                                            1,
                                                                            0,
                                                                            0,
                                                                            65536});
            this.m_widthUpDown.Location = new System.Drawing.Point(188, 222);
            this.m_widthUpDown.Maximum = new System.Decimal(new int[] {
                                                                          10,
                                                                          0,
                                                                          0,
                                                                          0});
            this.m_widthUpDown.Minimum = new System.Decimal(new int[] {
                                                                          1,
                                                                          0,
                                                                          0,
                                                                          65536});
            this.m_widthUpDown.Name = "m_widthUpDown";
            this.m_widthUpDown.Size = new System.Drawing.Size(49, 20);
            this.m_widthUpDown.TabIndex = 8;
            this.m_widthUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.m_widthUpDown.Value = new System.Decimal(new int[] {
                                                                        10,
                                                                        0,
                                                                        0,
                                                                        0});
            this.m_widthUpDown.ValueChanged += new System.EventHandler(this.WidthUpDown_ValueChanged);
            this.m_widthUpDown.Leave += new System.EventHandler(this.UpDown_Leave);
            // 
            // iconColor
            // 
            this.iconColor.BackColor = System.Drawing.Color.Transparent;
            this.iconColor.Image = ((System.Drawing.Bitmap)(resources.GetObject("iconColor.Image")));
            this.iconColor.Location = new System.Drawing.Point(16, 10);
            this.iconColor.Name = "iconColor";
            this.iconColor.Size = new System.Drawing.Size(24, 24);
            this.iconColor.TabIndex = 38;
            this.iconColor.TabStop = false;
            // 
            // m_colorBox
            // 
            this.m_colorBox.BackColor = System.Drawing.SystemColors.Window;
            this.m_colorBox.Location = new System.Drawing.Point(136, 12);
            this.m_colorBox.Name = "m_colorBox";
            this.m_colorBox.Size = new System.Drawing.Size(123, 21);
            this.m_colorBox.TabIndex = 0;
            this.m_colorBox.ColorChanged += new System.EventHandler(this.ColorCombo_ColorChanged);
            // 
            // m_optimizeForHighlighter
            // 
            this.m_optimizeForHighlighter.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_optimizeForHighlighter.Location = new System.Drawing.Point(136, 50);
            this.m_optimizeForHighlighter.Name = "m_optimizeForHighlighter";
            this.m_optimizeForHighlighter.Size = new System.Drawing.Size(181, 24);
            this.m_optimizeForHighlighter.TabIndex = 2;
            this.m_optimizeForHighlighter.Text = "Optimize for highlighting text";
            this.m_optimizeForHighlighter.CheckedChanged += new System.EventHandler(this.OnOptimizeForHighlighter_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(241, 196);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 56;
            this.label2.Text = "mm";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(241, 224);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 13);
            this.label4.TabIndex = 57;
            this.label4.Text = "mm";
            // 
            // SettingsDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.m_cancelBtn;
            this.ClientSize = new System.Drawing.Size(616, 342);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.label4,
                                                                          this.label2,
                                                                          this.m_optimizeForHighlighter,
                                                                          this.m_colorBox,
                                                                          this.iconColor,
                                                                          this.m_transpLabel,
                                                                          this.label8,
                                                                          this.pictureBox5,
                                                                          this.groupBox1,
                                                                          this.label7,
                                                                          this.label6,
                                                                          this.m_widthUpDown,
                                                                          this.m_heightUpDown,
                                                                          this.m_pressureCheck,
                                                                          this.label5,
                                                                          this.label1,
                                                                          this.pictureBox4,
                                                                          this.iconTip,
                                                                          this.m_smoothLabel,
                                                                          this.iconTransparency,
                                                                          this.m_transparencyBar,
                                                                          this.m_fitToCurve,
                                                                          this.m_antialias,
                                                                          this.groupBox3,
                                                                          this.groupBox4,
                                                                          this.groupBox5,
                                                                          this.m_customBtn,
                                                                          this.panel1,
                                                                          this.m_rectangleRadio,
                                                                          this.m_ballRadio,
                                                                          this.m_cancelBtn,
                                                                          this.m_okBtn});
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.ShowInTaskbar = false;
            this.Text = "Ink Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.m_transparencyBar)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_heightUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_widthUpDown)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// Initializes the custom controls.
        /// </summary>
        /// <remarks>
        /// The custom controls if included in the standard initialize component method are subject to
        /// vanishing if that method is regenerated by the system.
        /// </remarks>
        private void InitializeCustomControls()
        {
            this.m_percentUpDown = new MSR.LST.Controls.InkToolBarControls.PercentUpDown();
            // 
            // m_percentUpDown
            // 
            this.m_percentUpDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_percentUpDown.Location = new System.Drawing.Point(264, 87);
            this.m_percentUpDown.Name = "m_percentUpDown";
            this.m_percentUpDown.Size = new System.Drawing.Size(75, 20);
            this.m_percentUpDown.TabIndex = 3;
            this.m_percentUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.m_percentUpDown.ValueChanged += new System.EventHandler(this.PercentUpDown_ValueChanged);
            this.m_percentUpDown.Leave += new System.EventHandler(UpDown_Leave);
            this.Controls.Add(m_percentUpDown);
        }

        /// <summary>
        /// Gets the drawing attributes for the adusted settings.
        /// </summary>
        /// <value>
        /// The adjusted <c>Microsoft.Ink.DrawingAttributes</c> that contain settings for the Pen or Highlighter.
        /// </value>
        /// <remarks>
        /// This property is accessed after closing the <c>SettingsDialog</c> form.
        /// </remarks>
        /// <example>
        /// <code>
        /// SettingsDialog s = new SettingsDialog(m_penAttributes.Clone());
        ///
        /// if (s.ShowDialog() == DialogResult.OK)
        /// {
        ///     m_penAttributes = s.InkDrawingAttributes;
        /// }
        /// </code>
        /// </example>
        public Microsoft.Ink.DrawingAttributes InkDrawingAttributes
        {
            get { return this.inkPicture.DefaultDrawingAttributes; }
        }

        /// <summary>
        /// Event handler for the <c>Form.Load</c> event.
        /// </summary>
        /// <param name="sender">Object that invoked the event.</param>
        /// <param name="e">An <c>EventArgs</c> that contains the event data.</param>
        /// <remarks>
        /// Initializes the form's controls to represent the <c>DrawingAttributes</c> passed in to the
        /// constructor.
        /// </remarks>
        private void Settings_Load(object sender, System.EventArgs e)
        {
            this.m_customBtn.Text = Strings.Custom;
            this.m_fitToCurve.Text = Strings.FitToCurve;
            this.m_ballRadio.Text = Strings.Ball;
            this.m_rectangleRadio.Text = Strings.Rectangle;
            this.m_antialias.Text = Strings.Antialiased;
            this.m_clearBtn.Text = Strings.Clear;
            this.m_cancelBtn.Text = Strings.CancelHotkey;
            this.m_okBtn.Text = Strings.OKHotkey;
            this.label5.Text = Strings.Tip;
            this.label1.Text = Strings.Color;
            this.m_transpLabel.Text = Strings.Transparency;
            this.m_smoothLabel.Text = Strings.Smoothness;
            this.label3.Text = Strings.Test;
            this.m_pressureCheck.Text = Strings.IgnorePressure;
            this.label6.Text = Strings.Height;
            this.label7.Text = Strings.Width;
            this.label8.Text = Strings.Size;
            this.m_optimizeForHighlighter.Text = Strings.OptimizeForHighlightingText;
            this.label2.Text = Strings.MillimeterAbbreviation;
            this.label4.Text = Strings.MillimeterAbbreviation;
            this.Text = Strings.InkSettings;

            m_colorBox.Color          = InkDrawingAttributes.Color;
            m_optimizeForHighlighter.Checked = (InkDrawingAttributes.RasterOperation == RasterOperation.MaskPen);
            m_transparencyBar.Value   = (int) Math.Round(((double)100 * InkDrawingAttributes.Transparency / 255), 0);
            m_ballRadio.Checked       = InkDrawingAttributes.PenTip == PenTip.Ball;
            m_rectangleRadio.Checked  = InkDrawingAttributes.PenTip == PenTip.Rectangle;
            m_heightUpDown.Value      = Convert.ToDecimal(InkDrawingAttributes.Height) / 100;
            m_widthUpDown.Value       = Convert.ToDecimal(InkDrawingAttributes.Width) / 100;
            m_pressureCheck.Checked   = InkDrawingAttributes.IgnorePressure;
            m_antialias.Checked       = InkDrawingAttributes.AntiAliased;
            m_fitToCurve.Checked      = InkDrawingAttributes.FitToCurve;

            //Disable height if tip is ball
            if (this.m_ballRadio.Checked)
                this.m_heightUpDown.Enabled = false;
        }

        /// <summary>
        /// Event handler for m_transparencyBar's <c>TrackBar.ValueChanged</c> event.
        /// </summary>
        /// <param name="sender">Object that invoked the event.</param>
        /// <param name="e">An <c>EventArgs</c> that contains the event data.</param>
        /// <remarks>
        /// Sets the <c>Transparency</c> property of the <c>DrawingAttributes</c> and updates the 
        /// <c>PercentUpDown</c> control.
        /// </remarks>
        private void TransparencyBar_ValueChanged(object sender, System.EventArgs e)
        {
            decimal nom   = this.m_transparencyBar.Value   - this.m_transparencyBar.Minimum;
            decimal denom = this.m_transparencyBar.Maximum - this.m_transparencyBar.Minimum;
            this.m_percentUpDown.Value = Convert.ToInt32(100 * nom/denom);

            this.inkPicture.DefaultDrawingAttributes.Transparency = Convert.ToByte(255 * (nom/denom));
        }

        /// <summary>
        /// Event handler for the m_clearBtn's <c>Button.Click</c> event.
        /// </summary>
        /// <param name="sender">Object that invoked the event.</param>
        /// <param name="e">An <c>EventArgs</c> that contains the event data.</param>
        /// <remarks>
        /// Clear the test area of Ink strokes.
        /// </remarks>
        private void ClearBtn_Click(object sender, System.EventArgs e)
        {
            this.inkPicture.Ink.DeleteStrokes();
            this.inkPicture.Refresh();
        }

        /// <summary>
        /// Event handler for the m_antialias's <c>CheckBox.CheckChanged</c> event.
        /// </summary>
        /// <param name="sender">Object that invoked the event.</param>
        /// <param name="e">An <c>EventArgs</c> that contains the event data.</param>
        /// <remarks>
        /// Sets the AntiAliased property.
        /// </remarks>
        private void Antialias_CheckedChanged(object sender, System.EventArgs e)
        {
            this.inkPicture.DefaultDrawingAttributes.AntiAliased = this.m_antialias.Checked;
        }

        /// <summary>
        /// Event handler for the m_fitToCurve's <c>CheckBox.CheckChanged</c> event.
        /// </summary>
        /// <param name="sender">Object that invoked the event.</param>
        /// <param name="e">An <c>EventArgs</c> that contains the event data.</param>
        /// <remarks>
        /// Sets the FitToCurve property.
        /// </remarks>
        private void FitToCurve_CheckedChanged(object sender, System.EventArgs e)
        {
            this.inkPicture.DefaultDrawingAttributes.FitToCurve = this.m_fitToCurve.Checked;
        }

        /// <summary>
        /// Event handler for m_ballRadio's <c>RadioButton.CheckChanged</c> event.
        /// </summary>
        /// <param name="sender">Object that invoked the event.</param>
        /// <param name="e">An <c>EventArgs</c> that contains the event data.</param>
        /// <remarks>
        /// Sets the PenTip property.
        /// </remarks>
        private void RoundRadio_CheckedChanged(object sender, System.EventArgs e)
        {
            this.inkPicture.DefaultDrawingAttributes.PenTip = this.m_ballRadio.Checked ? PenTip.Ball : PenTip.Rectangle;
        }

        /// <summary>
        /// Event handler for m_pressureCheck's <c>CheckBox.CheckChanged</c> event.
        /// </summary>
        /// <param name="sender">Object that invoked the event.</param>
        /// <param name="e">An <c>EventArgs</c> that contains the event data.</param>
        /// <remarks>
        /// Sets the IgnorePressure property.
        /// </remarks>
        private void PressureCheck_CheckedChanged(object sender, System.EventArgs e)
        {
            this.inkPicture.DefaultDrawingAttributes.IgnorePressure = this.m_pressureCheck.Checked;
        }

        /// <summary>
        /// Event handler for m_heightUpDown's <c>NumericUpDown.ValueChanged</c> event.
        /// </summary>
        /// <param name="sender">Object that invoked the event.</param>
        /// <param name="e">An <c>EventArgs</c> that contains the event data.</param>
        /// <remarks>
        /// Updates the Height property.
        /// </remarks>
        private void HeightUpDown_ValueChanged(object sender, System.EventArgs e)
        {
            this.inkPicture.DefaultDrawingAttributes.Height = 100 * Convert.ToSingle(this.m_heightUpDown.Value);
        }

        /// <summary>
        /// Event handler for m_widthUpDown's <c>NumericUpDown.ValueChanged</c> event.
        /// </summary>
        /// <param name="sender">Control that invoked the event.</param>
        /// <param name="e">An <c>EventArgs</c> that contains the event data.</param>
        /// <remarks>
        /// Updates the Width property.
        /// </remarks>
        private void WidthUpDown_ValueChanged(object sender, System.EventArgs e)
        {
            this.inkPicture.DefaultDrawingAttributes.Width = 100 * Convert.ToSingle(this.m_widthUpDown.Value);
        }

        /// <summary>
        /// Event handler for the <c>Control.Leave</c> event for all <c>NumericUpDown</c> controls.
        /// </summary>
        /// <param name="sender">Control that invoked the event.</param>
        /// <param name="e">An <c>EventArgs</c> that contains the event data.</param>
        /// <remarks>
        /// Ensures the control's value is updated when input focus is lost. e.g. tab or click
        /// outside the control.
        /// </remarks>
        private void UpDown_Leave(object sender, System.EventArgs e)
        {
            NumericUpDown nud = (NumericUpDown)sender;
            nud.Value = nud.Value;
        }

        /// <summary>
        /// Event handler for m_rectangleRadio's <c>RadioButton.CheckChanged</c> event.
        /// </summary>
        /// <param name="sender">Control that invoked the event.</param>
        /// <param name="e">An <c>EventArgs</c> that contains the event data.</param>
        /// <remarks>
        /// Enables or disables m_heightUpDown (Size height control).
        /// </remarks>
        private void RectangleRadio_CheckedChanged(object sender, System.EventArgs e)
        {
            this.m_heightUpDown.Enabled = this.m_rectangleRadio.Checked;
        }

        /// <summary>
        /// Event handler for m_customBtn's <c>Button.Click</c> event.
        /// </summary>
        /// <param name="sender">Control that invoked the event.</param>
        /// <param name="e"></param>
        /// <remarks>
        /// Opens a modal <c>ColorDialog</c> for choosing custom colors.  It updates the
        /// color combo box custom control.
        /// </remarks>
        private void CustomBtn_Click(object sender, System.EventArgs e)
        {
            ColorDialog colorDlg = new ColorDialog();

            //Load any custom colors
            if (m_customColors != null)
                colorDlg.CustomColors = m_customColors;

            //Initialize with current color
            colorDlg.Color = this.m_colorBox.Color;

            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                //Update color
                this.m_colorBox.Color = colorDlg.Color;
                this.inkPicture.DefaultDrawingAttributes.Color = this.m_colorBox.Color;

                //Save any custom colors
                if (colorDlg.CustomColors != null)
                    m_customColors = colorDlg.CustomColors;
            }
        }

        /// <summary>
        /// Event handler for m_colorBox's <c>ColorComboBox.ColorChanged</c> event.
        /// </summary>
        /// <param name="sender">Control that invoked the event.</param>
        /// <param name="e">An <c>EventArgs</c> that contains the event data.</param>
        /// <remarks>
        /// Updates the Color property.
        /// </remarks>
        private void ColorCombo_ColorChanged(object sender, System.EventArgs e)
        {
            this.inkPicture.DefaultDrawingAttributes.Color = this.m_colorBox.Color;
        }

        /// <summary>
        /// Event handler for the m_percentUpDown's <c>NumericUpDown.ValueChanged</c> event.
        /// </summary>
        /// <param name="sender">Control that invoked the event.</param>
        /// <param name="e">An <c>EventArgs</c> that contains the event data.</param>
        /// <remarks>
        /// Updates the transparency <c>TrackBar</c>'s value.
        /// </remarks>
        private void PercentUpDown_ValueChanged(object sender, System.EventArgs e)
        {
            decimal d = this.m_percentUpDown.Value / 100;
            int range = this.m_transparencyBar.Maximum - this.m_transparencyBar.Minimum;
            this.m_transparencyBar.Value = Convert.ToInt32(this.m_transparencyBar.Minimum + (d * range));
        }

        private void OnOptimizeForHighlighter_CheckedChanged(object sender, System.EventArgs e)
        {
            bool enable = !m_optimizeForHighlighter.Checked;

            inkPicture.DefaultDrawingAttributes.RasterOperation = enable ? RasterOperation.CopyPen 
                                                                         : RasterOperation.MaskPen;
            m_transpLabel.Enabled     = enable;
            m_transparencyBar.Enabled = enable;
            m_percentUpDown.Enabled   = enable;
            m_antialias.Enabled       = enable;
        }

        /// <summary>
        /// Invalidate the stroke if FitToCurve is set or there is transparency.
        /// </summary>
        private void OnInkPicture_Stroke(object sender, Microsoft.Ink.InkCollectorStrokeEventArgs e)
        {
            if ((inkPicture.EditingMode == InkOverlayEditingMode.Ink) && 
                (inkPicture.DefaultDrawingAttributes.FitToCurve ||
                ((inkPicture.DefaultDrawingAttributes.Transparency > 0) && !m_optimizeForHighlighter.Checked)))
            {
                using (Graphics g = inkPicture.CreateGraphics())
                {
                    inkPicture.Invalidate(InkSpaceToPixel(g, e.Stroke.GetBoundingBox())); 
                }
            }
        }

        private Rectangle InkSpaceToPixel(Graphics g, Rectangle inkSpaceRect)
        {
            Point location = inkSpaceRect.Location;
            Point size     = new Point(inkSpaceRect.Width, inkSpaceRect.Height);
            Point[] pts    = new Point[]{location, size};

            Renderer r = new Renderer();
            r.InkSpaceToPixel(g, ref pts);
            
            return new Rectangle(pts[0].X, pts[0].Y, pts[1].X, pts[1].Y);
        }
    }
}
