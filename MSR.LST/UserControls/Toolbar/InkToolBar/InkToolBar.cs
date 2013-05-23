using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.Ink;

using MSR.LST.Controls.InkToolBarControls;


namespace MSR.LST.Controls
{
    // Represents a pre-constructed toolbar for Ink enabled applications.
    // 
    // The InkToolBar enables developers to drag and drop a toolbar for controlling Ink enabled 
    // applications.  It has buttons for Pen, Highlighter, Eraser and EraseAll.
    // 
    // Set the Mode property to control which tool is initially selected.  To use, subscribe and handle 
    // the SettingsChanged and ModeChanged events.  SettingsChanged is fired when a Pen or Highlighter 
    // setting changed.  ModeChanged is fired when a different tool is selected.
    // 
    // Supports Windows XP visual styles and themes when used with a manifest.
    //


    /// <summary>
    /// Specifies the selected tool.
    /// </summary>
    public enum InkToolBarMode 
    { 
        /// <summary>
        /// Represents a pen tool with opaque ink and small widths.
        /// </summary>
        Pen, 
        /// <summary>
        /// Represents a highlighter tool with transparent ink and large heights.
        /// </summary>
        Highlighter,
        /// <summary>
        /// Represents an eraser tool.
        /// </summary>
        Eraser,
    };

    /// <summary>
    /// Represents a pre-constructed toolbar for Ink enabled applications.
    /// </summary>
    [DefaultEvent("SettingsChanged")]
    [DefaultProperty("Mode")]
    [ToolboxBitmap(typeof(MSR.LST.Controls.InkToolBar), "Images.InkToolBar.ico")]
    public class InkToolBar : System.Windows.Forms.ToolBar
    {
        #region Constants
        //
        // Pen size constants
        //
        private const float penSizeFine      = 10;
        private const float penSizeMedium    = 60;
        private const float penSizeThick     = 100;

        //
        // Eraser size constants
        //
        private const int   eraserSizeSmall  = 100;
        private const int   eraserSizeMedium = 300;
        private const int   eraserSizeLarge  = 600;

        // 
        // position/width of the colored line position underneath of pencil/pen icon
        //
        private const int lineIconY = 30;
        private const int lineIconX1 = 6;
        private const int lineIconX2 = 33;
        private const int widthLineHighlighterIcon = 4;
        private const int widthLinePenIcon = 1;

        #endregion

        #region Definitions
        //
        // Mode
        //
        private InkToolBarMode m_mode;
        //
        // Highlighter transparency indicator
        //
        private bool m_useRasterOpForHighlighter;
        //
        // Settings Dialog
        //
        private SettingsDialog  m_settingsDialog;
        //
        // Attributes
        //

        private InkOverlay inkOverlay = null;


        private Microsoft.Ink.DrawingAttributes m_drawingAttributes;
        private Microsoft.Ink.DrawingAttributes m_penAttributes;
        private Microsoft.Ink.DrawingAttributes m_highlighterAttributes;
        private ErasingAttributes               m_eraserAttributes;
        //
        // ToolBar Buttons
        //
        private System.Windows.Forms.ToolBarButton m_btnPen;
        private System.Windows.Forms.ToolBarButton m_btnHighlighter;
        private System.Windows.Forms.ToolBarButton m_btnEraser;
        //
        // Drop Down Menus
        //
        private System.Windows.Forms.ContextMenu m_menuPen;
        private System.Windows.Forms.ContextMenu m_menuHighlighter;
        private System.Windows.Forms.ContextMenu m_menuEraser;
        //
        // Menu: Pen->Color
        //
        private System.Windows.Forms.MenuItem      menuItemPenColor;
        private InkToolBarControls.ColorMenuItem[] menuItemPenColors;
        private System.Windows.Forms.MenuItem      menuItemPenColorSeperator;
        private System.Windows.Forms.MenuItem      menuItemPenColorMore;
        //
        // Menu: Pen->Size
        //
        private System.Windows.Forms.MenuItem      menuItemPenSizeSeperator;
        private System.Windows.Forms.MenuItem      menuItemPenSizeMore;
        //
        // Menu: Pen->Settings...
        //
        private System.Windows.Forms.MenuItem menuItemPenSeperator;
        private System.Windows.Forms.MenuItem menuItemPenSettings;
        //
        // Menu: Highlighter->Color
        //
        private System.Windows.Forms.MenuItem      menuItemHighlighterColor;
        private InkToolBarControls.ColorMenuItem[] menuItemHighlighterColors;
        private System.Windows.Forms.MenuItem      menuItemHighlighterColorSeperator;
        private System.Windows.Forms.MenuItem      menuItemHighlighterColorMore;
        //
        // Menu: Highlighter->Settings...
        //
        private System.Windows.Forms.MenuItem menuItemHighlighterSeperator;
        private System.Windows.Forms.MenuItem menuItemHighlighterSettings;
        //
        // Image Lists
        //
        private System.Windows.Forms.ImageList m_sizeImages;
        private System.Windows.Forms.ImageList m_colorImages;
        private System.Windows.Forms.ImageList m_imagesAlpha;
        private System.Windows.Forms.ToolBarButton m_btnEraseAll;
                        
        private System.ComponentModel.IContainer components;
        #endregion

        #region Events

        //
        // Events
        //
        /// <summary>
        /// Occurs when any of the pen, highlighter or eraser settings change.
        /// </summary>
        [Category("Ink")]
        [Description("Occurs when any ink settings change.")]
        public event EventHandler SettingsChanged;
        /// <summary>
        /// Occurs when a different tool other than the currently selected tool is selected from the toolbar.
        /// </summary>
        [Category("Ink")]
        [Description("Occurs when the toolbar's Mode changes.")]
        public event EventHandler ModeChanged;
        /// <summary>
        /// Occurs when the toolbar's EraseAll eraser is clicked.
        /// </summary>
        [Category("Ink")]
        [Description("Occurs when the toolbar's EraseAll eraser is clicked.")]
        public event EventHandler EraseAllClicked;
        /// <summary>
        /// Occurs when the Settings Dialog is opened or closed.
        /// </summary>
        [Category("Ink")]
        [Description("Occurs when the Settings Dialog's visibility changes.")]
        public event EventHandler SettingsDialogVisibleChanged;

        #endregion

        /// <summary>
        /// Initializes a new instance of the InkToolBar class.
        /// </summary>
        /// <remarks>
        /// The default <c>Mode</c> is Pen.  Set <c>Mode</c> to select a different startup tool.  
        /// Subscibe and handle the <c>SettingsChanged</c> and <c>ModeChanged</c> events.
        /// </remarks>
        public InkToolBar()
        {
            InitializeComponent();
            InitializeMenus();
            InitializeModes();
        }


        // Properties
        // Property of the control

        /// <summary>
        /// Allows to pass an InkOverlay object to the InkToolBar control so
        /// the Ink management can be done inside the InkToolBar control
        /// </summary>
        public InkOverlay InkOverlay
        {
            set
            {
                inkOverlay = value;

                inkOverlay.DefaultDrawingAttributes = m_penAttributes;
                // Make sure we are not in delete mode
                inkOverlay.EditingMode = InkOverlayEditingMode.Ink;

                // Init default color to blue, set button to selected
                inkOverlay.DefaultDrawingAttributes.Color = m_penAttributes.Color;
            }
        }

        /// <summary>
        /// Allows to get the current color name of the InkOverlay object.
        /// </summary>
        public string ColorName
        {
            get
            {
                return inkOverlay.DefaultDrawingAttributes.Color.Name;
            }
        }

        /// <summary>
        /// Initializes the button drop down menus.
        /// </summary>
        private void InitializeMenus()
        {
            this.menuItemPenColor  = new System.Windows.Forms.MenuItem();
            this.menuItemPenColors = new ColorMenuItem[4];
            for (int i=0; i < this.menuItemPenColors.Length; i++)
                this.menuItemPenColors[i]  = new ColorMenuItem();
            this.menuItemPenColorSeperator = new System.Windows.Forms.MenuItem();
            this.menuItemPenColorMore      = new System.Windows.Forms.MenuItem();

            this.menuItemPenSizeSeperator = new System.Windows.Forms.MenuItem();
            this.menuItemPenSizeMore      = new System.Windows.Forms.MenuItem();

            this.menuItemPenSeperator      = new System.Windows.Forms.MenuItem();
            this.menuItemPenSettings       = new System.Windows.Forms.MenuItem();

            this.menuItemHighlighterColor  = new System.Windows.Forms.MenuItem();
            this.menuItemHighlighterColors = new ColorMenuItem[3];
            for (int i=0; i < this.menuItemHighlighterColors.Length; i++)
                this.menuItemHighlighterColors[i]  = new ColorMenuItem();
            this.menuItemHighlighterColorSeperator = new System.Windows.Forms.MenuItem();
            this.menuItemHighlighterColorMore      = new System.Windows.Forms.MenuItem();

            this.menuItemHighlighterSeperator = new System.Windows.Forms.MenuItem();
            this.menuItemHighlighterSettings  = new System.Windows.Forms.MenuItem();


            // ++++++++++++++++++++++ m_menuPen ++++++++++++++++++++++++++++

            this.m_menuPen.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                      this.menuItemPenColors[0],
                                                                                      this.menuItemPenColors[1],
                                                                                      this.menuItemPenColors[2], 
                                                                                      this.menuItemPenColors[3]});

            // menuItemPenColors[0]
            this.menuItemPenColors[0].RadioCheck = true;
            this.menuItemPenColors[0].Index = 0;
            this.menuItemPenColors[0].Text = Strings.BlueHotkey;
            this.menuItemPenColors[0].Color = Color.Blue;
            this.menuItemPenColors[0].Click += new System.EventHandler(this.MenuItemPenColors_Click); 
            // menuItemPenColors[1]
            this.menuItemPenColors[1].RadioCheck = true;
            this.menuItemPenColors[1].Index = 1;
            this.menuItemPenColors[1].Text = Strings.BlackHotkey;
            this.menuItemPenColors[1].Color = Color.Black;
            this.menuItemPenColors[1].Click += new System.EventHandler(this.MenuItemPenColors_Click); 
            // menuItemPenColors[2]
            this.menuItemPenColors[2].RadioCheck = true;
            this.menuItemPenColors[2].Index = 2;
            this.menuItemPenColors[2].Text = Strings.RedHotkey;
            this.menuItemPenColors[2].Color = Color.Red;
            this.menuItemPenColors[2].Click += new System.EventHandler(this.MenuItemPenColors_Click);
            // menuItemPenColors[3]
            this.menuItemPenColors[3].RadioCheck = true;
            this.menuItemPenColors[3].Index = 3;
            this.menuItemPenColors[3].Text = Strings.YellowHotkey;
            this.menuItemPenColors[3].Color = Color.Yellow;
            this.menuItemPenColors[3].Click += new System.EventHandler(this.MenuItemPenColors_Click);
            
            // ++++++++++++++++++++ m_menuHighlighter ++++++++++++++++++++++++++++           
            
            // m_menuHighlighter
            this.m_menuHighlighter.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                              this.menuItemHighlighterColors[0],
                                                                                              this.menuItemHighlighterColors[1],
                                                                                              this.menuItemHighlighterColors[2]});
            // menuItemHighlighterColors[0]
            this.menuItemHighlighterColors[0].RadioCheck = true;
            this.menuItemHighlighterColors[0].Index = 0;
            this.menuItemHighlighterColors[0].Text = Strings.YellowHotkey;
            this.menuItemHighlighterColors[0].Color = Color.Yellow;
            this.menuItemHighlighterColors[0].Click += new System.EventHandler(this.MenuItemHighlighterColors_Click);
            // menuItemHighlighterColors[1]
            this.menuItemHighlighterColors[1].RadioCheck = true;
            this.menuItemHighlighterColors[1].Index = 1;
            this.menuItemHighlighterColors[1].Text = Strings.LimeHotkey;
            this.menuItemHighlighterColors[1].Color = Color.Lime;
            this.menuItemHighlighterColors[1].Click += new System.EventHandler(this.MenuItemHighlighterColors_Click);
            // menuItemHighlighterColors[2]
            this.menuItemHighlighterColors[2].RadioCheck = true;
            this.menuItemHighlighterColors[2].Index = 2;
            this.menuItemHighlighterColors[2].Text = Strings.BlueHotkey;
            this.menuItemHighlighterColors[2].Color = Color.Blue;
            this.menuItemHighlighterColors[2].Click += new System.EventHandler(this.MenuItemHighlighterColors_Click);
        }

        // ++++++++++++++++ Initialize modes +++++++++++++++++++

        /// <summary>
        /// Initializes the toolbar's modes and checks the appropriate menu items.
        /// </summary>
        private void InitializeModes()
        {
            // Attributes
            m_penAttributes         = new Microsoft.Ink.DrawingAttributes();
            m_highlighterAttributes = new Microsoft.Ink.DrawingAttributes();
            m_eraserAttributes      = new ErasingAttributes();

            // Pen Attributes
            m_penAttributes.AntiAliased  = true;
            m_penAttributes.Color        = Color.Blue;
            m_penAttributes.Width        = penSizeMedium;
            m_penAttributes.Height       = m_penAttributes.Width;
            m_penAttributes.FitToCurve   = true;
            m_penAttributes.PenTip       = PenTip.Ball;
            m_penAttributes.Transparency = 0;

            //Check color menu
            foreach (ImageMenuItem item in this.menuItemPenColors)
            {
                if (m_penAttributes.Color == item.Color)
                    item.Checked = true;
                else
                    item.Checked = false;
            }

            // Select (push) the pen button
            this.Buttons[(int) InkToolBarMode.Pen].Pushed = true;

            // Highlighter Attributes
            m_highlighterAttributes.AntiAliased  = true;
            m_highlighterAttributes.Color        = Color.Yellow;
            m_highlighterAttributes.Width        = penSizeThick;
            m_highlighterAttributes.Height       = 600;
            m_highlighterAttributes.PenTip       = PenTip.Rectangle;
            m_highlighterAttributes.Transparency = Convert.ToByte(255 * 0.5); //50%
            // Set the highlighter icon
            SetLineIcon(Color.Yellow, InkToolBarMode.Highlighter, 3);

            // Check highlighter color menu
            foreach (ImageMenuItem item in this.menuItemHighlighterColors)
            {
                if (m_highlighterAttributes.Color == item.Color)
                    item.Checked = true;
                else
                    item.Checked = false;
            }
            // Eraser Attributes
            m_eraserAttributes.Mode = InkOverlayEraserMode.StrokeErase;
            m_eraserAttributes.Size = InkToolBar.eraserSizeMedium;

            // Set default attributes
            if (Mode == InkToolBarMode.Highlighter)
                SetInkDrawingAttributes(m_highlighterAttributes);
            else
                SetInkDrawingAttributes(m_penAttributes);
        }


        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(InkToolBar));
            this.m_btnPen = new System.Windows.Forms.ToolBarButton();
            this.m_menuPen = new System.Windows.Forms.ContextMenu();
            this.m_btnHighlighter = new System.Windows.Forms.ToolBarButton();
            this.m_menuHighlighter = new System.Windows.Forms.ContextMenu();
            this.m_btnEraser = new System.Windows.Forms.ToolBarButton();
            this.m_menuEraser = new System.Windows.Forms.ContextMenu();
            this.m_btnEraseAll = new System.Windows.Forms.ToolBarButton();
            this.m_sizeImages = new System.Windows.Forms.ImageList(this.components);
            this.m_colorImages = new System.Windows.Forms.ImageList(this.components);
            this.m_imagesAlpha = new System.Windows.Forms.ImageList(this.components);
            // 
            // m_btnPen
            // 
            this.m_btnPen.DropDownMenu = this.m_menuPen;
            this.m_btnPen.ImageIndex = 0;
            this.m_btnPen.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.m_btnPen.ToolTipText = Strings.Pen;
            // 
            // m_btnHighlighter
            // 
            this.m_btnHighlighter.DropDownMenu = this.m_menuHighlighter;
            this.m_btnHighlighter.ImageIndex = 1;
            this.m_btnHighlighter.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.m_btnHighlighter.ToolTipText = Strings.Highlighter;
            // 
            // m_btnEraser
            // 
            this.m_btnEraser.ImageIndex = 2;
            this.m_btnEraser.ToolTipText = Strings.Eraser;
            // 
            // m_btnEraseAll
            // 
            this.m_btnEraseAll.ImageIndex = 3;
            this.m_btnEraseAll.ToolTipText = Strings.EraseAll;
            // 
            // m_sizeImages
            // 
            this.m_sizeImages.ImageSize = new System.Drawing.Size(80, 17);
            this.m_sizeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_sizeImages.ImageStream")));
            this.m_sizeImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // m_colorImages
            // 
            this.m_colorImages.ImageSize = new System.Drawing.Size(24, 17);
            this.m_colorImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_colorImages.ImageStream")));
            this.m_colorImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // m_imagesAlpha
            // 
            this.m_imagesAlpha.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.m_imagesAlpha.ImageSize = new System.Drawing.Size(40, 40);
            this.m_imagesAlpha.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_imagesAlpha.ImageStream")));
            this.m_imagesAlpha.TransparentColor = System.Drawing.Color.Magenta;
            // 
            // InkToolBar
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
                                                                               this.m_btnPen,
                                                                               this.m_btnHighlighter,
                                                                               this.m_btnEraser,
                                                                               this.m_btnEraseAll});
            this.ButtonSize = new System.Drawing.Size(31, 30);
            this.Divider = false;
            this.ImageList = this.m_imagesAlpha;
            this.Size = new System.Drawing.Size(100, 34);
            this.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.ToolBarButtonClicked);

        }
        #endregion

        #region Overridden Base Class Properties

        /// <summary>
        /// Gets or sets the accessible role of the control.
        /// </summary>
        /// <remarks>Overridden to change the default value for the AccessibleRole property</remarks>
        [DefaultValue(AccessibleRole.ToolBar)]
        public new System.Windows.Forms.AccessibleRole AccessibleRole
        {
            get { return base.AccessibleRole;  }
            set { base.AccessibleRole = value; }
        }

        /// <summary>
        /// Gets the collection of System.Windows.Forms.ToolBarButton controls assigned to the 
        /// toolbar control.
        /// </summary>
        /// <remarks>Overridden to remove the Buttons property from the properties window</remarks>
        [Browsable(false)]
        public new ToolBar.ToolBarButtonCollection Buttons
        {
            get { return base.Buttons; }
        }

        /// <summary>
        /// Gets or sets the collection of images available to the toolbar button controls.
        /// </summary>
        /// <remarks>Overridden to remove the ImageList property from the properties window</remarks> 
        [Browsable(false)]
        public new System.Windows.Forms.ImageList ImageList
        {
            get { return base.ImageList; }
            set { base.ImageList = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether drop-down buttons on a toolbar display down arrows.
        /// </summary>
        /// <remarks>Overridden to change the default value for the DropDownArrows property</remarks>
        [DefaultValue(true)]
        public new bool DropDownArrows
        {
            get { return base.DropDownArrows;  }
            set { base.DropDownArrows = value; }
        }

        /// <summary>
        ///  Gets or sets the value that determines the appearance of a toolbar control and its buttons. 
        /// </summary>
        /// <remarks>Overridden to change the default value for the Appearance property</remarks>
        [DefaultValue(ToolBarAppearance.Flat)]
        public new ToolBarAppearance Appearance
        {
            get { return base.Appearance;  }
            set { base.Appearance = value; }
        }

        /// <summary>
        ///  Gets or sets a value indicating whether the toolbar displays a tooltip for each button. 
        /// </summary>
        /// <remarks>Overridden to change the default value for the ShowTooltips property</remarks>
        [DefaultValue(true)]
        public new bool ShowToolTips
        {
            get { return base.ShowToolTips;  }
            set { base.ShowToolTips = value; }
        }


        #endregion

        #region Event Handlers

        /// <summary>
        /// Raises the <c>EraseAllClicked</c> event.
        /// </summary>
        /// <remarks>
        /// The <c>EraseAllClicked</c> event is raised when the EraseAll button is clicked
        /// </remarks>
        protected virtual void OnEraseAllClicked()
        {
            if(EraseAllClicked != null)
            {
                EraseAllClicked(this, new EventArgs());
            }
        }

        /// <summary>
        /// Raises the <c>SettingsChanged</c> event.
        /// </summary>
        /// <remarks>
        /// The <c>SettingsChanged</c> event is raised when pen or highlighter settings change such as 
        /// color.
        /// </remarks>
        protected virtual void OnSettingsChanged()
        {
            // Fire event to any subscribers
            if (null != SettingsChanged)
            {
                SettingsChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Raises the <c>ModeChanged</c> event.
        /// </summary>
        /// <remarks>
        /// The <c>ModeChanged</c> event is raised when the currently selected tool changes.
        /// </remarks>
        protected virtual void OnModeChanged()
        {
            //
            // Fire event to any subscribers
            //
            if (ModeChanged != null)
            {
                ModeChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Raises the <c>SettingsDialogVisibilityChanged</c> event.
        /// </summary>
        /// <remarks>
        /// The <c>SettingsDialogVisibilityChanged</c> event is raised when the Settings Dialog is opened or closed.
        /// </remarks>
        protected virtual void OnSettingsDialogVisibleChanged(object sender, System.EventArgs e)
        {
            if (SettingsDialogVisibleChanged != null)
            {
                SettingsDialogVisibleChanged(sender, e);
            }
        }

        private void OnSettingsDialogVisibilityChanged(object sender, System.EventArgs e)
        {
            OnSettingsDialogVisibleChanged(sender, e);
        }


        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the pen properties.
        /// </summary>
        [Browsable(false)]
        public Microsoft.Ink.DrawingAttributes PenAttributes
        {
            get { return m_penAttributes;  }
            set 
            {
                m_penAttributes = value; 

                // Set the mode to Pen
                Mode = InkToolBarMode.Pen;

                // Push the correct button
                foreach(ToolBarButton b in this.Buttons)
                {
                    b.Pushed = false;
                }
                this.Buttons[(int) InkToolBarMode.Pen].Pushed = true;

                // Sync the color in the button icon/drop-down
                SetPen(m_penAttributes.Color, true);
                UpdatePenMenus();
            }
        }

        /// <summary>
        /// Gets or sets the highlighter properties.
        /// </summary>
        [Browsable(false)]
        public Microsoft.Ink.DrawingAttributes HighlighterAttributes
        {
            get { return m_highlighterAttributes;  }
            set 
            {
                m_highlighterAttributes = value;

                // Set the mode to Highlighter
                Mode = InkToolBarMode.Highlighter;

                // Push the correct button
                foreach(ToolBarButton b in this.Buttons)
                {
                    b.Pushed = false;
                }
                this.Buttons[(int) InkToolBarMode.Highlighter].Pushed = true;

                // Sync the color in the button icon/drop-down
                SetHighlighter(m_highlighterAttributes.Color, true);
                UpdateHighlighterMenus();
            }
        }



        /// <summary>
        /// Indicates whether highlighter should render using alpha transparency or
        /// use a MaskPen RasterOperation.
        /// </summary>
        [Category("Behavior")]
        [Description("Indicates whether highlighter should render using alpha transparency or use a MaskPen RasterOperation.")]
        [DefaultValue(false)]
        public bool UseRasterOperationForHighlighter
        {
            get { return this.m_useRasterOpForHighlighter;  }
            set 
            { 
                this.m_useRasterOpForHighlighter     = value;
                m_highlighterAttributes.Transparency = Convert.ToByte(255 * 0.7); //70%

                if (value)
                    m_highlighterAttributes.RasterOperation = RasterOperation.MaskPen;
                else
                    m_highlighterAttributes.RasterOperation = RasterOperation.CopyPen;
            }
        }

        /// <summary>
        /// Gets or sets the <c>Mode</c> of the toolbar.
        /// </summary>
        /// <value>
        /// One of the <c>InkToolBarMode</c> values.  The default is <c>InkToolBarMode.Pen</c>.
        /// </value>
        /// <remarks>
        /// The <c>Mode</c> property determines which Ink tool is selected on the toolbar.  When a
        /// <c>Mode</c> changes, the <c>ModeChanged</c> event is fired.
        /// </remarks>
        [Category("Behavior")]
        [Description("Indicates the default tool.")]
        [DefaultValue(InkToolBarMode.Pen)]
        public InkToolBarMode Mode
        {
            get { return m_mode;  }
            set 
            { 
                switch (value)
                {
                    case InkToolBarMode.Pen:
                        SetInkDrawingAttributes(this.m_penAttributes);
                        break;
                    case InkToolBarMode.Highlighter:
                        SetInkDrawingAttributes(this.m_highlighterAttributes);
                        break;
                    case InkToolBarMode.Eraser:
                    default:
                        break;
                }
                if (m_mode != value)
                {
                    m_mode = value;
                    OnModeChanged();
                }
            }
        }


        /// <summary>
        /// Sets the default drawing attributes.
        /// </summary>
        /// <param name="drawingAttributes"></param>
        private void SetInkDrawingAttributes(Microsoft.Ink.DrawingAttributes drawingAttributes)
        {
            this.m_drawingAttributes = drawingAttributes;
            OnSettingsChanged();
        }

        #endregion

        /// <summary>
        /// Switch to eraser mode.
        /// </summary>
        public void SwitchToEraserMode()
        {
            // Set the mode to Eraser
            Mode = InkToolBarMode.Eraser;

            inkOverlay.EraserMode = this.m_eraserAttributes.Mode;
            inkOverlay.EraserWidth = this.m_eraserAttributes.Size;
            inkOverlay.EditingMode = InkOverlayEditingMode.Delete;

            // Push the correct button
            foreach(ToolBarButton b in this.Buttons)
            {
                b.Pushed = false;
            }
            this.Buttons[(int) InkToolBarMode.Eraser].Pushed = true;
        }

        /// <summary>
        /// Delete all the strokes on the picture box.
        /// </summary>
        public void DeleteAllStrokes()
        {
            try
            {
                inkOverlay.Ink.DeleteStrokes(inkOverlay.Ink.Strokes);
                OnEraseAllClicked();
            }
            catch (System.ArgumentException)
            {
                RtlAwareMessageBox.Show(this, Strings.TryingToDrawError, 
                    Strings.Information, MessageBoxButtons.OK, MessageBoxIcon.Information, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                inkOverlay.Ink.DeleteStrokes(inkOverlay.Ink.Strokes);
            }
            this.Refresh();
        }

        #region Internal Event Handlers

        private void unpushToolBarButtons()
        {
            // Unpush all the buttons
            foreach(ToolBarButton b in this.Buttons)
            {
                b.Pushed = false;
            }
        }

        /// <summary>
        /// Event handler for the <c>ToolBar.ButtonClick</c> event.
        /// </summary>
        /// <param name="sender">Object that invoked the event.</param>
        /// <param name="e">A <c>ToolBarButtonClickEventArgs</c> that contains the event data.</param>
        private void ToolBarButtonClicked(Object sender, ToolBarButtonClickEventArgs e)
        {
            switch (this.Buttons.IndexOf(e.Button))
            {
                case 0:
                    // Push only the button that has been clicked
                    unpushToolBarButtons();
                    e.Button.Pushed = true;
                    inkOverlay.EditingMode = InkOverlayEditingMode.Ink;
                    SetPen(m_penAttributes.Color, true);
                    Mode = InkToolBarMode.Pen;
                    break;
                case 1:
                    // Push only the button that has been clicked
                    unpushToolBarButtons();
                    e.Button.Pushed = true;
                    inkOverlay.EditingMode = InkOverlayEditingMode.Ink;
                    SetHighlighter(m_highlighterAttributes.Color, true);
                    Mode = InkToolBarMode.Highlighter;
                    break;
                case 2:
                    // Push only the button that has been clicked
                    unpushToolBarButtons();
                    e.Button.Pushed = true;
                    Mode = InkToolBarMode.Eraser;
                    inkOverlay.EditingMode = InkOverlayEditingMode.Delete;
                    break;
                case 3:
                    e.Button.Pushed = false;
                    DeleteAllStrokes();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// This method allows to set the pen color and push the 
        /// Pen button if requested.
        /// </summary>
        /// <param name="colorName">The color</param>
        /// <param name="push">If true, the Pen button will be pushed and all the 
        /// other buttons in the InkToolBar control will be unpushed.</param>
        public void SetPen(Color color, bool push)
        {
            //Update pen attributes' color
            this.m_penAttributes.Color = color;

            inkOverlay.DefaultDrawingAttributes = m_penAttributes;
            // Make sure we are not in delete mode
            inkOverlay.EditingMode = InkOverlayEditingMode.Ink;

            SetLineIcon(color, InkToolBarMode.Pen, widthLinePenIcon);
            //Set the mode
            Mode = InkToolBarMode.Pen;

            // Push only the Pen button
            if (push)
            {
                // Unpush all the buttons
                foreach(ToolBarButton b in this.Buttons)
                {
                    b.Pushed = false;
                }
                // Push the Pen button
                this.Buttons[(int) InkToolBarMode.Pen].Pushed = true;
            }
        }

        /// <summary>
        /// Get the current pen color.
        /// </summary>
        /// <param name="imgMenuItem">The image menu item.</param>
        private Color GetCurrentColorPen(ImageMenuItem imgMenuItem)
        {
            //Get the menu item's index
            int index = imgMenuItem.Index;
            // Obtain the menu text without the shortcut ampersand: '&'.
            Color color = menuItemPenColors[index].Color;
            //Check mark the relative item
            foreach(ImageMenuItem i in menuItemPenColors)
                i.Checked = (index == i.Index);
            return color;
        }

        /// <summary>
        /// Event handler for the <c>MenuItem.Click</c> event from the Pen-Color menus.
        /// </summary>
        /// <param name="sender">Object that invoked the event.</param>
        /// <param name="e">An <c>EventArgs</c> that contains the event data.</param>
        /// <remarks>
        /// By setting the <c>Mode</c> at the end of the method.  The <c>ModeChanged</c> event will be 
        /// fired if needed.
        /// </remarks>
        private void MenuItemPenColors_Click(object sender, System.EventArgs e)
        {
            // Get the color name of the pen
            Color color = GetCurrentColorPen((ImageMenuItem)sender);
            // Set the pen
            SetPen(color, true);
        }

        /// <summary>
        /// Updates the pen menus after pen settings have changed.
        /// </summary>
        private void UpdatePenMenus()
        {
            foreach (ImageMenuItem i in menuItemPenColors)
            {
                i.Checked = (m_penAttributes.Color == i.Color);
            }
        }

        /// <summary>
        /// Get the current highlighter color.
        /// </summary>
        /// <param name="imgMenuItem">The image menu item.</param>
        private Color GetCurrentColorHighlighter(ImageMenuItem imgMenuItem)
        {
            //Get the menu item's index
            int index = imgMenuItem.Index;
            
            Color color = menuItemHighlighterColors[index].Color;
            //Check mark the relative item
            foreach(ImageMenuItem i in menuItemHighlighterColors)
                i.Checked = (index == i.Index);
            return color;
        }

        /// <summary>
        /// Set the line below the icon.
        /// </summary>
        /// <param name="colorName">The color name.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="width">The width of the line.</param>
        private void SetLineIcon(Color color, InkToolBarMode mode, int width)
        {
            // Update the icon of the highlighter with the selected color
            Image img = m_imagesAlpha.Images[(int)mode];
            Graphics g = Graphics.FromImage(img);
            // Draw a line underneath of the icon with the selected
            // color
            Pen myPen = new Pen(color);
            for (int y = lineIconY; y <= lineIconY + width -1; y++)
                g.DrawLine(myPen, lineIconX1, y, lineIconX2, y);
            myPen.Dispose();
            m_imagesAlpha.Images[(int)mode] = img;
            this.Invalidate();
        }

        /// <summary>
        /// Set the highlighter.
        /// </summary>
        /// <param name="colorName">The color of the highlighter.</param>
        /// <param name="push">Whether or not the button should 
        /// be programatically pushed</param>
        public void SetHighlighter(Color color, bool push)
        {
            //Update highlighter attributes' color
            this.m_highlighterAttributes.Color = color;
 
            inkOverlay.DefaultDrawingAttributes = m_highlighterAttributes;
            // Make sure we are not in delete mode
            inkOverlay.EditingMode = InkOverlayEditingMode.Ink;

            //Set the mode
            Mode = InkToolBarMode.Highlighter;

            if (push)
            {
                // Unpush all the buttons
                foreach(ToolBarButton b in this.Buttons)
                {
                    b.Pushed = false;
                }
                this.Buttons[(int) InkToolBarMode.Highlighter].Pushed = true;
            }

            SetLineIcon(color, InkToolBarMode.Highlighter, widthLineHighlighterIcon);
        }

        /// <summary>
        /// Event handler for the <c>MenuItem.Click</c> event from the Highlighter-Color menus.
        /// </summary>
        /// <param name="sender">Object that invoked the event.</param>
        /// <param name="e">An <c>EventArgs</c> that contains the event data.</param>
        /// <remarks>
        /// By setting the <c>Mode</c> at the end of the method.  The <c>ModeChanged</c> event will be 
        /// fired if needed. 
        /// </remarks>
        private void MenuItemHighlighterColors_Click(object sender, System.EventArgs e)
        {
            Color color = GetCurrentColorHighlighter((ImageMenuItem)sender);
            // Set the highlighter
            SetHighlighter(color, true);
        }

        /// <summary>
        /// Event handler for the <c>MenuItem.Click</c> event for opening the <c>SettingsDialog</c>.
        /// </summary>
        /// <param name="sender">Object that invoked the event.</param>
        /// <param name="e">An <c>EventArgs</c> that contains the event data.</param>
        /// <remarks>
        /// Displays the <c>SettingsDialog</c>, updating any attributes that might have changed for the 
        /// highlighter.
        /// <para>
        /// By setting the <c>Mode</c> at the end of the method.  The <c>ModeChanged</c> event will be 
        /// fired if needed.
        /// </para>
        /// </remarks>
        private void MenuItemHighlighterSettings_Click(object sender, System.EventArgs e)
        {
            //
            // Display the settings dialog, updating any attributes that might have
            // changed for the highlighter.
            //
            m_settingsDialog                 = new SettingsDialog(m_highlighterAttributes.Clone());
            m_settingsDialog.VisibleChanged += new System.EventHandler(OnSettingsDialogVisibilityChanged);
            m_settingsDialog.Text = Strings.HighlighterSettings;

            if (m_settingsDialog.ShowDialog(this.Parent) == DialogResult.OK)
            {
                m_highlighterAttributes = m_settingsDialog.InkDrawingAttributes;

                UpdateHighlighterMenus();
            }
            // Unsubscribe from the VisibleChanged event.
            m_settingsDialog.VisibleChanged -= new System.EventHandler(OnSettingsDialogVisibilityChanged);


            Mode = InkToolBarMode.Highlighter;
        }

        /// <summary>
        /// Updates the highlighter color menus after settings are changed using the <c>SettingsDialog</c>.
        /// </summary>
        private void UpdateHighlighterMenus()
        {
            foreach (ImageMenuItem i in menuItemHighlighterColors)
                i.Checked = (m_highlighterAttributes.Color == i.Color);
        }

        #endregion
    }
}
