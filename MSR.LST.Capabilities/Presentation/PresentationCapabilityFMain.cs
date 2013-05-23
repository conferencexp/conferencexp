using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.Remoting.Messaging; // AsyncResult
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

// The Ink namespace, which contains the Tablet PC Platform API
// Note: must add a ref to:Microsoft Tablet PC Platform SDK\Include\Microsoft.Ink.dll
using Microsoft.Ink;

using MSR.LST;

// The MSR.LST.Controls is modified code that originally comes from the 
// InkToolbar controls created by Leszynski Group Inc.
using MSR.LST.Controls;

// To use RTDocument, RTDocumentHelper, RTStroke, etc.
// Note: must add a ref to:RTDocuments.dll and RTDocumentUtilities.dll
using MSR.LST.RTDocuments;
using MSR.LST.RTDocuments.Utilities;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// PresentationCapabilityFMain is the main form for the CXP Presentation capability.
    /// CXP Presentation capability is a simple distributed powerpoint application example for ConferenceXP
    /// </summary>
    public class PresentationCapabilityFMain : CapabilityForm
    {
        #region VS generated

        private System.Windows.Forms.PictureBox pbRTDoc;
        private System.Windows.Forms.Panel pnlInk;
        private MSR.LST.Controls.InkToolBar inkToolBar;
        private System.Windows.Forms.MenuItem miFile;
        private System.Windows.Forms.MenuItem miOpen;
        private System.Windows.Forms.MenuItem miClose;
        private System.Windows.Forms.MenuItem miBlueInk;
        private System.Windows.Forms.MenuItem miBlackInk;
        private System.Windows.Forms.MenuItem miRedInk;
        private System.Windows.Forms.MenuItem miYellowHighlighter;
        private System.Windows.Forms.MenuItem miLimeHighlighter;
        private System.Windows.Forms.MenuItem miBlueHighlighter;
        private System.Windows.Forms.MenuItem miErase;
        private System.Windows.Forms.MenuItem miEraseAllInk;
        private System.Windows.Forms.MenuItem miSaveAsImages;
        private System.Windows.Forms.ImageList imageListFileMenu;
        private System.Windows.Forms.Panel pnlFile;
        private System.Windows.Forms.ToolBar tbFile;
        private System.Windows.Forms.ToolBarButton tbbOpenFile;
        private System.Windows.Forms.ToolBarButton tbbResend;
        private System.Windows.Forms.Panel pnlUI;
        private System.Windows.Forms.ImageList imageListSlides;
        private System.Windows.Forms.ToolBar tbInsertSlides;
        private System.Windows.Forms.ToolBarButton tbbInsertSlide;
        private System.Windows.Forms.ToolBarButton tbbSnapshot;
        private System.Windows.Forms.ImageList imageListNavigation;
        private System.Windows.Forms.ToolBar tbNavigate;
        private System.Windows.Forms.ToolBarButton tbbPrevious;
        private System.Windows.Forms.ToolBarButton tbbNext;
        private System.Windows.Forms.MenuItem miBlankSlide;
        private System.Windows.Forms.MenuItem miSnapshot;
        private System.Windows.Forms.MenuItem miPreviousSlide;
        private System.Windows.Forms.MenuItem miNextSlide;
        private MSR.LST.ConferenceXP.PresentationStatusBar statusBar;
        private MSR.LST.ConferenceXP.ThumbnailListView thumbnailsView;
        private System.Windows.Forms.ContextMenu ctxtMenuSnapshotApps;
        private System.Windows.Forms.MenuItem miTools;
        private System.Windows.Forms.MenuItem miSlide;
        private System.Windows.Forms.MenuItem miHelp;
        private System.Windows.Forms.MenuItem miYellowInk;
        private System.Windows.Forms.MenuItem miInsertSnapshot;
        private System.Windows.Forms.MenuItem miSlideShow;
        private System.Windows.Forms.PictureBox pbSlidePreview;
        private System.Windows.Forms.PictureBox pbShadowSlidePreview;
        private System.Windows.Forms.ImageList imageListVideo;
        private System.Windows.Forms.Panel pnlVideo;
        private System.Windows.Forms.MenuItem miResendCurrentSlide;
        private System.Windows.Forms.MenuItem miOpenRemote;
        private System.Windows.Forms.MenuItem miPresentationHelp;
        private System.Windows.Forms.MenuItem miHelpAbout;
        private System.Windows.Forms.Panel pnlNavigate;
        private System.Windows.Forms.Panel pnlInsert;
        private System.Windows.Forms.ToolBarButton tbbPen;
        private System.Windows.Forms.ToolBarButton tbbHighlighter;
        private System.Windows.Forms.ToolBarButton tbbEraser;
        private System.Windows.Forms.ToolBarButton tbbEraseAll;
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem miSeparatorPenHighlighter;
        private System.Windows.Forms.MenuItem miSeparatorHighlighterEraser;

        private System.ComponentModel.IContainer components;
        
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PresentationCapabilityFMain));
            this.pbRTDoc = new System.Windows.Forms.PictureBox();
            this.pnlUI = new System.Windows.Forms.Panel();
            this.pnlNavigate = new System.Windows.Forms.Panel();
            this.tbNavigate = new System.Windows.Forms.ToolBar();
            this.tbbPrevious = new System.Windows.Forms.ToolBarButton();
            this.tbbNext = new System.Windows.Forms.ToolBarButton();
            this.imageListNavigation = new System.Windows.Forms.ImageList(this.components);
            this.pnlInsert = new System.Windows.Forms.Panel();
            this.tbInsertSlides = new System.Windows.Forms.ToolBar();
            this.tbbInsertSlide = new System.Windows.Forms.ToolBarButton();
            this.tbbSnapshot = new System.Windows.Forms.ToolBarButton();
            this.ctxtMenuSnapshotApps = new System.Windows.Forms.ContextMenu();
            this.imageListSlides = new System.Windows.Forms.ImageList(this.components);
            this.pnlInk = new System.Windows.Forms.Panel();
            this.inkToolBar = new MSR.LST.Controls.InkToolBar();
            this.tbbPen = new System.Windows.Forms.ToolBarButton();
            this.tbbHighlighter = new System.Windows.Forms.ToolBarButton();
            this.tbbEraser = new System.Windows.Forms.ToolBarButton();
            this.tbbEraseAll = new System.Windows.Forms.ToolBarButton();
            this.pnlFile = new System.Windows.Forms.Panel();
            this.tbFile = new System.Windows.Forms.ToolBar();
            this.tbbOpenFile = new System.Windows.Forms.ToolBarButton();
            this.tbbResend = new System.Windows.Forms.ToolBarButton();
            this.imageListFileMenu = new System.Windows.Forms.ImageList(this.components);
            this.pnlVideo = new System.Windows.Forms.Panel();
            this.imageListVideo = new System.Windows.Forms.ImageList(this.components);
            this.mainMenu = new System.Windows.Forms.MainMenu();
            this.miFile = new System.Windows.Forms.MenuItem();
            this.miOpen = new System.Windows.Forms.MenuItem();
            this.miOpenRemote = new System.Windows.Forms.MenuItem();
            this.miResendCurrentSlide = new System.Windows.Forms.MenuItem();
            this.miSlideShow = new System.Windows.Forms.MenuItem();
            this.miClose = new System.Windows.Forms.MenuItem();
            this.miTools = new System.Windows.Forms.MenuItem();
            this.miBlueInk = new System.Windows.Forms.MenuItem();
            this.miBlackInk = new System.Windows.Forms.MenuItem();
            this.miRedInk = new System.Windows.Forms.MenuItem();
            this.miYellowInk = new System.Windows.Forms.MenuItem();
            this.miSeparatorPenHighlighter = new System.Windows.Forms.MenuItem();
            this.miYellowHighlighter = new System.Windows.Forms.MenuItem();
            this.miLimeHighlighter = new System.Windows.Forms.MenuItem();
            this.miBlueHighlighter = new System.Windows.Forms.MenuItem();
            this.miSeparatorHighlighterEraser = new System.Windows.Forms.MenuItem();
            this.miErase = new System.Windows.Forms.MenuItem();
            this.miEraseAllInk = new System.Windows.Forms.MenuItem();
            this.miSaveAsImages = new System.Windows.Forms.MenuItem();
            this.miSlide = new System.Windows.Forms.MenuItem();
            this.miBlankSlide = new System.Windows.Forms.MenuItem();
            this.miSnapshot = new System.Windows.Forms.MenuItem();
            this.miInsertSnapshot = new System.Windows.Forms.MenuItem();
            this.miPreviousSlide = new System.Windows.Forms.MenuItem();
            this.miNextSlide = new System.Windows.Forms.MenuItem();
            this.miHelp = new System.Windows.Forms.MenuItem();
            this.miPresentationHelp = new System.Windows.Forms.MenuItem();
            this.miHelpAbout = new System.Windows.Forms.MenuItem();
            this.statusBar = new MSR.LST.ConferenceXP.PresentationStatusBar();
            this.thumbnailsView = new MSR.LST.ConferenceXP.ThumbnailListView();
            this.pbSlidePreview = new System.Windows.Forms.PictureBox();
            this.pbShadowSlidePreview = new System.Windows.Forms.PictureBox();
            this.pnlUI.SuspendLayout();
            this.pnlNavigate.SuspendLayout();
            this.pnlInsert.SuspendLayout();
            this.pnlInk.SuspendLayout();
            this.pnlFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbRTDoc
            // 
            this.pbRTDoc.BackColor = System.Drawing.Color.White;
            this.pbRTDoc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbRTDoc.Location = new System.Drawing.Point(168, 72);
            this.pbRTDoc.Name = "pbRTDoc";
            this.pbRTDoc.Size = new System.Drawing.Size(672, 560);
            this.pbRTDoc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbRTDoc.TabIndex = 3;
            this.pbRTDoc.TabStop = false;
            this.pbRTDoc.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbRTDoc_MouseDown);
            // 
            // pnlUI
            // 
            this.pnlUI.Controls.Add(this.pnlNavigate);
            this.pnlUI.Controls.Add(this.pnlInsert);
            this.pnlUI.Controls.Add(this.pnlInk);
            this.pnlUI.Controls.Add(this.pnlFile);
            this.pnlUI.Controls.Add(this.pnlVideo);
            this.pnlUI.Location = new System.Drawing.Point(0, 0);
            this.pnlUI.Name = "pnlUI";
            this.pnlUI.Size = new System.Drawing.Size(600, 56);
            this.pnlUI.TabIndex = 27;
            // 
            // pnlNavigate
            // 
            this.pnlNavigate.Controls.Add(this.tbNavigate);
            this.pnlNavigate.Location = new System.Drawing.Point(504, 8);
            this.pnlNavigate.Name = "pnlNavigate";
            this.pnlNavigate.Size = new System.Drawing.Size(96, 56);
            this.pnlNavigate.TabIndex = 31;
            // 
            // tbNavigate
            // 
            this.tbNavigate.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.tbNavigate.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.tbNavigate.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
                                                                                          this.tbbPrevious,
                                                                                          this.tbbNext});
            this.tbNavigate.Divider = false;
            this.tbNavigate.DropDownArrows = true;
            this.tbNavigate.ImageList = this.imageListNavigation;
            this.tbNavigate.Location = new System.Drawing.Point(0, 0);
            this.tbNavigate.Name = "tbNavigate";
            this.tbNavigate.ShowToolTips = true;
            this.tbNavigate.Size = new System.Drawing.Size(96, 50);
            this.tbNavigate.TabIndex = 1;
            this.tbNavigate.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbNavigate_ButtonClick);
            // 
            // tbbPrevious
            // 
            this.tbbPrevious.ImageIndex = 0;
            this.tbbPrevious.ToolTipText = "Previous Slide";
            // 
            // tbbNext
            // 
            this.tbbNext.ImageIndex = 1;
            this.tbbNext.ToolTipText = "Next Slide";
            // 
            // imageListNavigation
            // 
            this.imageListNavigation.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageListNavigation.ImageSize = new System.Drawing.Size(40, 40);
            this.imageListNavigation.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListNavigation.ImageStream")));
            this.imageListNavigation.TransparentColor = System.Drawing.Color.Magenta;
            // 
            // pnlInsert
            // 
            this.pnlInsert.Controls.Add(this.tbInsertSlides);
            this.pnlInsert.Location = new System.Drawing.Point(360, 8);
            this.pnlInsert.Name = "pnlInsert";
            this.pnlInsert.Size = new System.Drawing.Size(112, 56);
            this.pnlInsert.TabIndex = 30;
            // 
            // tbInsertSlides
            // 
            this.tbInsertSlides.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.tbInsertSlides.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.tbInsertSlides.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
                                                                                              this.tbbInsertSlide,
                                                                                              this.tbbSnapshot});
            this.tbInsertSlides.Divider = false;
            this.tbInsertSlides.DropDownArrows = true;
            this.tbInsertSlides.ImageList = this.imageListSlides;
            this.tbInsertSlides.Location = new System.Drawing.Point(0, 0);
            this.tbInsertSlides.Name = "tbInsertSlides";
            this.tbInsertSlides.ShowToolTips = true;
            this.tbInsertSlides.Size = new System.Drawing.Size(112, 50);
            this.tbInsertSlides.TabIndex = 1;
            this.tbInsertSlides.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbInsertSlides_ButtonClick);
            // 
            // tbbInsertSlide
            // 
            this.tbbInsertSlide.ImageIndex = 0;
            this.tbbInsertSlide.ToolTipText = "Insert Blank Slide";
            // 
            // tbbSnapshot
            // 
            this.tbbSnapshot.DropDownMenu = this.ctxtMenuSnapshotApps;
            this.tbbSnapshot.ImageIndex = 1;
            this.tbbSnapshot.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.tbbSnapshot.ToolTipText = "Insert Screen Shot";
            // 
            // ctxtMenuSnapshotApps
            // 
            this.ctxtMenuSnapshotApps.Popup += new System.EventHandler(this.ctxtMenuSnapshotApps_Popup);
            // 
            // imageListSlides
            // 
            this.imageListSlides.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageListSlides.ImageSize = new System.Drawing.Size(40, 40);
            this.imageListSlides.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSlides.ImageStream")));
            this.imageListSlides.TransparentColor = System.Drawing.Color.Magenta;
            // 
            // pnlInk
            // 
            this.pnlInk.Controls.Add(this.inkToolBar);
            this.pnlInk.Location = new System.Drawing.Point(120, 8);
            this.pnlInk.Name = "pnlInk";
            this.pnlInk.Size = new System.Drawing.Size(220, 46);
            this.pnlInk.TabIndex = 28;
            // 
            // inkToolBar
            // 
            this.inkToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
                                                                                          this.tbbPen,
                                                                                          this.tbbHighlighter,
                                                                                          this.tbbEraser,
                                                                                          this.tbbEraseAll});
            this.inkToolBar.ButtonSize = new System.Drawing.Size(31, 30);
            this.inkToolBar.Divider = false;
            this.inkToolBar.Location = new System.Drawing.Point(0, 0);
            this.inkToolBar.Name = "inkToolBar";
            this.inkToolBar.Size = new System.Drawing.Size(220, 96);
            this.inkToolBar.TabIndex = 0;
            this.inkToolBar.ModeChanged += new System.EventHandler(this.inkToolBar_ModeChanged);
            this.inkToolBar.EraseAllClicked += new System.EventHandler(this.inkToolBar_EraseAllClicked);
            // 
            // tbbPen
            // 
            this.tbbPen.ImageIndex = 0;
            this.tbbPen.Pushed = true;
            this.tbbPen.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.tbbPen.ToolTipText = "Pen";
            // 
            // tbbHighlighter
            // 
            this.tbbHighlighter.ImageIndex = 1;
            this.tbbHighlighter.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.tbbHighlighter.ToolTipText = "Highlighter";
            // 
            // tbbEraser
            // 
            this.tbbEraser.ImageIndex = 2;
            this.tbbEraser.ToolTipText = "Eraser";
            // 
            // tbbEraseAll
            // 
            this.tbbEraseAll.ImageIndex = 3;
            this.tbbEraseAll.ToolTipText = "EraseAll";
            // 
            // pnlFile
            // 
            this.pnlFile.Controls.Add(this.tbFile);
            this.pnlFile.Location = new System.Drawing.Point(0, 8);
            this.pnlFile.Name = "pnlFile";
            this.pnlFile.Size = new System.Drawing.Size(96, 56);
            this.pnlFile.TabIndex = 29;
            // 
            // tbFile
            // 
            this.tbFile.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.tbFile.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.tbFile.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
                                                                                      this.tbbOpenFile,
                                                                                      this.tbbResend});
            this.tbFile.ButtonSize = new System.Drawing.Size(31, 30);
            this.tbFile.Divider = false;
            this.tbFile.DropDownArrows = true;
            this.tbFile.ImageList = this.imageListFileMenu;
            this.tbFile.Location = new System.Drawing.Point(0, 0);
            this.tbFile.Name = "tbFile";
            this.tbFile.ShowToolTips = true;
            this.tbFile.Size = new System.Drawing.Size(96, 50);
            this.tbFile.TabIndex = 1;
            this.tbFile.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbFile_ButtonClick);
            // 
            // tbbOpenFile
            // 
            this.tbbOpenFile.ImageIndex = 0;
            this.tbbOpenFile.ToolTipText = "Open";
            // 
            // tbbResend
            // 
            this.tbbResend.ImageIndex = 1;
            this.tbbResend.ToolTipText = "Resend Current Slide";
            // 
            // imageListFileMenu
            // 
            this.imageListFileMenu.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageListFileMenu.ImageSize = new System.Drawing.Size(40, 40);
            this.imageListFileMenu.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListFileMenu.ImageStream")));
            this.imageListFileMenu.TransparentColor = System.Drawing.Color.Magenta;
            // 
            // pnlVideo
            // 
            this.pnlVideo.Location = new System.Drawing.Point(472, 8);
            this.pnlVideo.Name = "pnlVideo";
            this.pnlVideo.Size = new System.Drawing.Size(56, 56);
            this.pnlVideo.TabIndex = 31;
            // 
            // imageListVideo
            // 
            this.imageListVideo.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageListVideo.ImageSize = new System.Drawing.Size(40, 40);
            this.imageListVideo.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListVideo.ImageStream")));
            this.imageListVideo.TransparentColor = System.Drawing.Color.Magenta;
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                     this.miFile,
                                                                                     this.miTools,
                                                                                     this.miSlide,
                                                                                     this.miHelp});
            // 
            // miFile
            // 
            this.miFile.Index = 0;
            this.miFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                   this.miOpen,
                                                                                   this.miOpenRemote,
                                                                                   this.miResendCurrentSlide,
                                                                                   this.miSlideShow,
                                                                                   this.miSaveAsImages,
                                                                                   this.miClose});
            this.miFile.Text = "&File";
            // 
            // miOpen
            // 
            this.miOpen.Index = 0;
            this.miOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.miOpen.Text = "&Open";
            this.miOpen.Click += new System.EventHandler(this.miOpen_Click);
            // 
            // miOpenRemote
            // 
            this.miOpenRemote.Index = 1;
            this.miOpenRemote.Text = "Open Remote...";
            this.miOpenRemote.Click += new System.EventHandler(this.miOpenRemote_Click);
            // 
            // miResendCurrentSlide
            // 
            this.miResendCurrentSlide.Index = 2;
            this.miResendCurrentSlide.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.miResendCurrentSlide.Text = "Resend Current Slide";
            this.miResendCurrentSlide.Click += new System.EventHandler(this.miResendCurrentSlide_Click);
            // 
            // miSlideShow
            // 
            this.miSlideShow.Index = 3;
            this.miSlideShow.Text = "Slide Show";
            this.miSlideShow.Click += new System.EventHandler(this.miSlideShow_Click);
            // 
            // miSaveAsImages
            // 
            this.miSaveAsImages.Index = 4;
            this.miSaveAsImages.Text = "&Save As Images...";
            this.miSaveAsImages.Click += new System.EventHandler(this.miSaveAsImages_Click);
            // 
            // miClose
            // 
            this.miClose.Index = 5;
            this.miClose.Text = "&Close";
            this.miClose.Click += new System.EventHandler(this.miClose_Click);
            // 
            // miTools
            // 
            this.miTools.Index = 1;
            this.miTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                    this.miBlueInk,
                                                                                    this.miBlackInk,
                                                                                    this.miRedInk,
                                                                                    this.miYellowInk,
                                                                                    this.miSeparatorPenHighlighter,
                                                                                    this.miYellowHighlighter,
                                                                                    this.miLimeHighlighter,
                                                                                    this.miBlueHighlighter,
                                                                                    this.miSeparatorHighlighterEraser,
                                                                                    this.miErase,
                                                                                    this.miEraseAllInk});
            this.miTools.Text = "&Tools";
            this.miTools.Popup += new System.EventHandler(this.miTools_Popup);
            // 
            // miBlueInk
            // 
            this.miBlueInk.Index = 0;
            this.miBlueInk.Text = "Blue Pen";
            this.miBlueInk.Click += new System.EventHandler(this.miBlueInk_Click);
            // 
            // miBlackInk
            // 
            this.miBlackInk.Index = 1;
            this.miBlackInk.Text = "Black Pen";
            this.miBlackInk.Click += new System.EventHandler(this.miBlackInk_Click);
            // 
            // miRedInk
            // 
            this.miRedInk.Index = 2;
            this.miRedInk.Text = "Red Pen";
            this.miRedInk.Click += new System.EventHandler(this.miRedInk_Click);
            // 
            // miYellowInk
            // 
            this.miYellowInk.Index = 3;
            this.miYellowInk.Text = "Yellow Pen";
            this.miYellowInk.Click += new System.EventHandler(this.miYellowInk_Click);
            // 
            // miSeparatorPenHighlighter
            // 
            this.miSeparatorPenHighlighter.Index = 4;
            this.miSeparatorPenHighlighter.Text = "-";
            // 
            // miYellowHighlighter
            // 
            this.miYellowHighlighter.Index = 5;
            this.miYellowHighlighter.Text = "Yellow Highlighter";
            this.miYellowHighlighter.Click += new System.EventHandler(this.miYellowHighlighter_Click);
            // 
            // miLimeHighlighter
            // 
            this.miLimeHighlighter.Index = 6;
            this.miLimeHighlighter.Text = "Lime Highlighter";
            this.miLimeHighlighter.Click += new System.EventHandler(this.miLimeHighlighter_Click);
            // 
            // miBlueHighlighter
            // 
            this.miBlueHighlighter.Index = 7;
            this.miBlueHighlighter.Text = "Blue Highlighter";
            this.miBlueHighlighter.Click += new System.EventHandler(this.miBlueHighlighter_Click);
            // 
            // miSeparatorHighlighterEraser
            // 
            this.miSeparatorHighlighterEraser.Index = 8;
            this.miSeparatorHighlighterEraser.Text = "-";
            // 
            // miErase
            // 
            this.miErase.Index = 9;
            this.miErase.Shortcut = System.Windows.Forms.Shortcut.CtrlE;
            this.miErase.Text = "Eraser";
            this.miErase.Click += new System.EventHandler(this.miErase_Click);
            // 
            // miEraseAllInk
            // 
            this.miEraseAllInk.Index = 10;
            this.miEraseAllInk.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
            this.miEraseAllInk.Text = "Erase All";
            this.miEraseAllInk.Click += new System.EventHandler(this.miEraseAllInk_Click);
            // 
            // miSlide
            // 
            this.miSlide.Index = 2;
            this.miSlide.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                    this.miBlankSlide,
                                                                                    this.miSnapshot,
                                                                                    this.miInsertSnapshot,
                                                                                    this.miPreviousSlide,
                                                                                    this.miNextSlide});
            this.miSlide.Text = "&Slide";
            this.miSlide.Popup += new System.EventHandler(this.miSlide_Popup);
            // 
            // miBlankSlide
            // 
            this.miBlankSlide.Index = 0;
            this.miBlankSlide.Shortcut = System.Windows.Forms.Shortcut.CtrlM;
            this.miBlankSlide.Text = "Insert Blank Slide";
            this.miBlankSlide.Click += new System.EventHandler(this.miBlankSlide_Click);
            // 
            // miSnapshot
            // 
            this.miSnapshot.Index = 1;
            this.miSnapshot.Shortcut = System.Windows.Forms.Shortcut.CtrlI;
            this.miSnapshot.Text = "Select Screen Shot";
            // 
            // miInsertSnapshot
            // 
            this.miInsertSnapshot.Enabled = false;
            this.miInsertSnapshot.Index = 2;
            this.miInsertSnapshot.Shortcut = System.Windows.Forms.Shortcut.CtrlI;
            this.miInsertSnapshot.Text = "Insert Screen Shot";
            this.miInsertSnapshot.Click += new System.EventHandler(this.miInsertSnapshot_Click);
            // 
            // miPreviousSlide
            // 
            this.miPreviousSlide.Index = 3;
            this.miPreviousSlide.Text = "&Previous Slide";
            this.miPreviousSlide.Click += new System.EventHandler(this.miPreviousSlide_Click);
            // 
            // miNextSlide
            // 
            this.miNextSlide.Index = 4;
            this.miNextSlide.Text = "&Next Slide";
            this.miNextSlide.Click += new System.EventHandler(this.miNextSlide_Click);
            // 
            // miHelp
            // 
            this.miHelp.Index = 3;
            this.miHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                   this.miPresentationHelp,
                                                                                   this.miHelpAbout});
            this.miHelp.Text = "Help";
            // 
            // miPresentationHelp
            // 
            this.miPresentationHelp.Index = 0;
            this.miPresentationHelp.Text = "Presentation Help";
            this.miPresentationHelp.Click += new System.EventHandler(this.miPresentationHelp_Click);
            // 
            // miHelpAbout
            // 
            this.miHelpAbout.Index = 1;
            this.miHelpAbout.Text = "About";
            this.miHelpAbout.Click += new System.EventHandler(this.miHelpAbout_Click);
            // 
            // statusBar
            // 
            this.statusBar.CurrentPage = 0;
            this.statusBar.Location = new System.Drawing.Point(0, 641);
            this.statusBar.Name = "statusBar";
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(848, 19);
            this.statusBar.StatusMessage = "";
            this.statusBar.TabIndex = 0;
            // 
            // thumbnailsView
            // 
            this.thumbnailsView.AutoScroll = true;
            this.thumbnailsView.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.thumbnailsView.BackColor = System.Drawing.SystemColors.Window;
            this.thumbnailsView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.thumbnailsView.Location = new System.Drawing.Point(5, 64);
            this.thumbnailsView.Name = "thumbnailsView";
            this.thumbnailsView.Size = new System.Drawing.Size(152, 568);
            this.thumbnailsView.TabIndex = 28;
            // 
            // pbSlidePreview
            // 
            this.pbSlidePreview.BackColor = System.Drawing.Color.White;
            this.pbSlidePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbSlidePreview.Location = new System.Drawing.Point(160, 72);
            this.pbSlidePreview.Name = "pbSlidePreview";
            this.pbSlidePreview.Size = new System.Drawing.Size(260, 195);
            this.pbSlidePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbSlidePreview.TabIndex = 29;
            this.pbSlidePreview.TabStop = false;
            this.pbSlidePreview.Visible = false;
            // 
            // pbShadowSlidePreview
            // 
            this.pbShadowSlidePreview.BackColor = System.Drawing.Color.LightGray;
            this.pbShadowSlidePreview.Location = new System.Drawing.Point(168, 80);
            this.pbShadowSlidePreview.Name = "pbShadowSlidePreview";
            this.pbShadowSlidePreview.Size = new System.Drawing.Size(260, 195);
            this.pbShadowSlidePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbShadowSlidePreview.TabIndex = 30;
            this.pbShadowSlidePreview.TabStop = false;
            this.pbShadowSlidePreview.Visible = false;
            // 
            // PresentationCapabilityFMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(848, 660);
            this.Controls.Add(this.pbSlidePreview);
            this.Controls.Add(this.pbShadowSlidePreview);
            this.Controls.Add(this.pnlUI);
            this.Controls.Add(this.thumbnailsView);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.pbRTDoc);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Menu = this.mainMenu;
            this.Name = "PresentationCapabilityFMain";
            this.Text = "PresentationCapabilityFMain";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PresentationCapabilityFMain_KeyDown);
            this.Resize += new System.EventHandler(this.PresentationCapabilityFMain_Resize);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.PresentationCapabilityFMain_Closing);
            this.Load += new System.EventHandler(this.PresentationCapabilityFMain_Load);
            this.pnlUI.ResumeLayout(false);
            this.pnlNavigate.ResumeLayout(false);
            this.pnlInsert.ResumeLayout(false);
            this.pnlInk.ResumeLayout(false);
            this.pnlFile.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        
        
        #endregion VS generated

        #region Member Variables

        // In order to hook object received events
        private PresentationCapability presentationCapability = null;

        #region Constants

        // Hard-coded constants to interop with UW and Brown
        // In order to scale ink to this page size
        // Pri3: See with Jay if there is a way to get these values from the sent document
        // directly
        private const int constWidthPageSend = 960;
        private const int constHeightPageSend = 720;

        // Constants for form Layout.  Due to the Form Designer, this is duplicate data, somewhat
        private const int constLayoutBuffer = 8;
        private const int constThumbnailsPortraitHeight = 116;
        private const int constThumbnailsLandscapeWidth = 138;

        // Constants for the slide preview position
        private const int constPosAdjustPreview = 20;

        // Constant for minimum width of the picture box pbRTDoc
        // To prevent the ink on the pb from scaling to zero
        private const int constPbWidthMin = 110;

        // This GUID represents the RTD identifier for a simple whiteboard.  This prevents having to
        //  ship a blank doc across the wire when Presentation starts (and all the problems that that causes).
        private const string constWhiteboardGuid = "7F41B219-481D-2D47-DAED-C39EE301F5DA";

        // This GUID represents the identifier for RTFrame to open remote file
        // Pri1: This is a temporary solution
        private const string constOpenRemoteGuid = "E255521C-AFBC-DDC3-A25E-5332C03DEC32";

        // JCB: Removed
        // This GUID represents the identifier for RTFrame to exit remote capa
        // when the initiator exits.
        // Pri1: This will be taking care at the networking level
        private const string constExitRemoteGuid = "B5371EF5-44B4-5421-4FFE-2FC5F03DF54F";
 
        // Url of the help
        // Note: We could centralize the url because the same constant is delared in FMain of BarUI project
        private const string helpurlPresentation = "http://cct.cs.washington.edu/project-wiki/index.php/ConferenceXP_Client_User_Guide";

        #endregion Constants
        
        #region RTDocument related

        private RTDocument rtDocument = null;
        private RTDocumentHelper rtDocumentHelper = null;

        private Guid pageShowing = Guid.Empty;

        // Page index in the doc (PPT) - only good for flat TOC
        private int index = 0;

        // How many pages from the doc have been received over the wire
        private int pageReceivedCounter = 0;

        // A storage place for pages received while we don't have the TOC and are requesting it
        private Queue cachedPages = null;

        #endregion RTDocument related

        #region Mode

        private bool debugMode = false;

        // For the slide show mode
        private enum displayType {Collaboration, SlideShow};
        private displayType displayMode = displayType.Collaboration;

        #endregion Mode

        #region Ink related

        // Ink
        private InkOverlay inkOverlay = null;

        // Hastable used to store PageId/Ink
        private Hashtable hashtablePageStrokes = null;

        // Mode
        private InkToolBarMode mode = InkToolBarMode.Pen;
 
        #endregion Ink related

        #region Top-of-pen / Stroke erasing

        private enum CursorID {Mouse = 1, Pen, ToOfPen};

        #endregion Top-of-pen / Stroke erasing

        #region Strokes resizing

        // Ratio y:x of the picture box pbRTDoc
        private double pbRatio = double.NaN;

        // Variables used for: Strokes scaling when resizing pbRTDoc
        private Size prevPbSize = Size.Empty;

        #endregion Strokes resizing

        #region Snapshot related

        private Win32Util.Win32Window applicationWindow = null;
        private Win32Util.Win32Window window = null;
        private Hashtable windowsInMenu = new Hashtable();

        #endregion Snapshot related
        
        #endregion Member Variables

        #region From ctor 

        /// <summary>
        /// Construct a new form for CXP Presenter capability.
        /// </summary>
        public PresentationCapabilityFMain()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Make sure the window fits on the display
            Rectangle screenArea = Screen.GetWorkingArea(this);
            if (this.Height > screenArea.Height)
                this.Height = screenArea.Height;
            if (this.Width > screenArea.Width)
                this.Width = screenArea.Width;

            // Finally, if we're filling the screen, just maximize 'cause it looks good
            if (this.Width == screenArea.Width && this.Height == screenArea.Height)
                this.WindowState = FormWindowState.Maximized;
        }

        
        #endregion

        #region Dispose method

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
                if(inkOverlay != null)
                {
                    inkOverlay.Dispose();
                }
                if(rtDocument != null)
                {
                    foreach( Page pg in rtDocument.Resources.Pages.Values )
                    {
                        if( pg.Image != null )
                            pg.Image.Dispose();
                    }
                }
            }

            // Call Dispose on your base class
            base.Dispose( disposing );
        }

        #endregion

        #region Form initialization and closing code

        /// <summary>
        /// Form_Load event handler of the main capability form. This event
        /// handler initialize the main global variable, the InkOverlay and the UI.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void PresentationCapabilityFMain_Load(object sender, System.EventArgs e)
        {
            this.statusBar.Font = UIFont.StatusFont;

            this.tbbPrevious.ToolTipText = Strings.PreviousSlide;
            this.tbbNext.ToolTipText = Strings.NextSlide;
            this.tbbInsertSlide.ToolTipText = Strings.InsertBlankSlide;
            this.tbbSnapshot.ToolTipText = Strings.InsertScreenShot;
            this.tbbPen.ToolTipText = Strings.Pen;
            this.tbbHighlighter.ToolTipText = Strings.Highlighter;
            this.tbbEraser.ToolTipText = Strings.Eraser;
            this.tbbEraseAll.ToolTipText = Strings.EraseAll;
            this.tbbOpenFile.ToolTipText = Strings.Open;
            this.tbbResend.ToolTipText = Strings.ResendCurrentSlide;
            this.miFile.Text = Strings.FileHotkey;
            this.miOpen.Text = Strings.OpenHotkey;
            this.miOpenRemote.Text = Strings.OpenRemote;
            this.miResendCurrentSlide.Text = Strings.ResendCurrentSlide;
            this.miSlideShow.Text = Strings.SlideShow;
            this.miClose.Text = Strings.CloseHotkey;
            this.miTools.Text = Strings.ToolsHotkey;
            this.miBlueInk.Text = Strings.BluePen;
            this.miBlackInk.Text = Strings.BlackPen;
            this.miRedInk.Text = Strings.RedPen;
            this.miYellowInk.Text = Strings.YellowPen;
            this.miYellowHighlighter.Text = Strings.YellowHighlighter;
            this.miLimeHighlighter.Text = Strings.LimeHighlighter;
            this.miBlueHighlighter.Text = Strings.BlueHighlighter;
            this.miErase.Text = Strings.Eraser;
            this.miEraseAllInk.Text = Strings.EraseAll;
            this.miSlide.Text = Strings.SlideHotkey;
            this.miBlankSlide.Text = Strings.InsertBlankSlide;
            this.miSnapshot.Text = Strings.SelectScreenShot;
            this.miInsertSnapshot.Text = Strings.InsertScreenShot;
            this.miPreviousSlide.Text = Strings.PreviousSlideHotkey;
            this.miNextSlide.Text = Strings.NextSlideHotkey;
            this.miHelp.Text = Strings.Help;
            this.miPresentationHelp.Text = Strings.PresentationHelp;
            this.miHelpAbout.Text = Strings.About;
            this.Text = Strings.PresentationCapabilityFMain;

            #region Initialize InkOverlay

            inkOverlay = new InkOverlay(pbRTDoc.Handle);

            // Hook up stroke added event
            inkOverlay.Stroke += new InkCollectorStrokeEventHandler(inkOverlay_Stroke);
            inkOverlay.StrokesDeleting += new InkOverlayStrokesDeletingEventHandler(inkOverlay_StrokesDeleting);

            // Hook up the NewInAirPackets event
            // Note: An in-air packet occurs when a user moves a pen near the tablet and the cursor 
            // is within the InkOverlay objects window or the user moves a mouse within the 
            // InkOverlay objects associated window. 
            // This will allow us to see if the pen is inverted meaning that the user is useing the eraser pen
            inkOverlay.CursorInRange += new InkCollectorCursorInRangeEventHandler(inkOverlay_CursorInRange);

            // Ink is actually the default editing mode, so the line
            // below is not required
            inkOverlay.EditingMode = InkOverlayEditingMode.Ink;

            // We're now set to go, so turn on tablet input
            inkOverlay.Enabled = true;

            // Link the inkOverlay of the inkToolBar control
            inkToolBar.InkOverlay = inkOverlay;

            #endregion Initialize InkOverlay

            #region Initialize UI
            
            // Hide/Show buttons and menus depending on the configuration settings
            // and Initiator capability or not

            // By default Open remote menu is invisible
            miOpenRemote.Visible = false;
            
            // If I am not the initiator, I won't be able to open a file
            if (!presentationCapability.IsSender)
            {
                // Remote user can not send anything other than ink and nav
                EnableSlideSend(false);
            } 
            else // Initiator
            {
                string setting;
                if ((setting = ConfigurationManager.AppSettings[AppConfig.PRES_OpenRemote]) != null)
                {
                    miOpenRemote.Visible = bool.Parse(setting);
                }
            }

            #endregion Initialize UI

            // Create the hashtable for page strokes
            hashtablePageStrokes = new Hashtable();

            this.pbRatio = (double)constHeightPageSend / (double)constWidthPageSend;

            // Disable the navigation button because we start in whiteboard mode
            // with ust one page
            // Pri2: Put all the UI init in one fct
            disableNavigationButtons();

            this.PerformDynamicLayout();

            // Set event handler for future orientation changes
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged +=
                new System.EventHandler( displaySettingsChanged );

            // Set cursor to default and clear status message
            statusBar.SetReadyStatusMessage();
        }

        /// <summary>
        /// Form_Closing event handler of the main capability form. This event
        /// handler ensure that the capability stops sending and playing.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void PresentationCapabilityFMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Make sure to dispose the RTDocumentHelper, as it could be performing a background operation such as BeginSendAllSlides()
            if (rtDocumentHelper != null)
            {
                rtDocumentHelper.Dispose();
            }

            if(presentationCapability != null)
            {
                if (presentationCapability.IsSending)
                {
                    string setting;
                    if (presentationCapability.IsSender && (setting = ConfigurationManager.AppSettings[AppConfig.PRES_ExitRemote]) != null)
                    {
                        if (bool.Parse(setting))
                        {
                            // Request the remote capa to exit
                            presentationCapability.SendObject(
                                new RTFrame(new Guid(constExitRemoteGuid), null));
                        }
                    }

                    presentationCapability.StopSending();
                }
            }

            // We have to check a second time since it may have gone null in between
            if(presentationCapability != null)
            {
                if (presentationCapability.IsPlaying)
                {
                    presentationCapability.StopPlaying();
                }
            }
        }

        #endregion

        #region General ObjectReceive method

        /// <summary>
        /// Hook the objectReceived event and process it.
        /// </summary>
        /// <param name="o">The object received</param>
        /// <param name="orea">The object event arguments</param>
        private void ObjectReceived(object o, ObjectReceivedEventArgs orea)
        {
            Log("ObjectReceived,");
            Log("Object orea.Data:" + orea.Data.ToString());

            try
            {
                // Don't receive your own messages (objects)
                if (orea.Participant != Conference.LocalParticipant)
                {
                    Log("Not a local participant,");

                    if (orea.Data is RTStrokeRemove)
                    {
                        RTStrokeRemoveReceived((RTStrokeRemove)orea.Data);
                    } 
                    else if (orea.Data is RTStroke)
                    {
                        RTStrokeReceived((RTStroke)orea.Data);
                    } 
                    else if (orea.Data is Page)
                    {
                        PageReceived((Page)orea.Data);
                    } 
                    else if (orea.Data is RTPageAdd)
                    {
                        RTPageAddReceived((RTPageAdd)orea.Data);
                    } 
                    else if (orea.Data is RTNodeChanged)
                    {
                        RTNodeChangedReceived( (RTNodeChanged)orea.Data);
                    } 
                    else if (orea.Data is RTDocument)
                    {
                        RTDocumentReceived((RTDocument)orea.Data);
                    }
                    else if (orea.Data is RTFrame)
                    {
                        RTFrameReceived((RTFrame)orea.Data);
                    }
                }
            }
            catch ( Exception e)
            {
                Log(string.Format(CultureInfo.CurrentCulture, "Exception: {0}", e.ToString()));
            }
        }

        #endregion

        #region Specialized Receive methods

        /// <summary>
        /// Handle the reception of a RTStroke object.
        /// </summary>
        /// <param name="rtStroke">Stroke received</param>
        private void RTStrokeReceived(RTStroke rtStroke)
        {
            Log("It's an RTStroke object,");

            Log(string.Format(CultureInfo.CurrentCulture, "Stroke ID: rtStroke.StrokeIdentifier.ToString()={0}", 
                rtStroke.StrokeIdentifier.ToString()));

            // Pri2: Check to make sure we have the right page & document

            // If the stroke already exists, delete the existing one and replace it
            RemoveStroke(rtStroke.StrokeIdentifier);

            // Resize the received stroke
            Log("Resize the stroke,");
            DisplaySizeInfo();
            float ratioWidthReceive = (float)pbRTDoc.Width / (float)constWidthPageSend;
            float ratioHeightReceive = (float)pbRTDoc.Height / (float)constHeightPageSend;
            for (int i = 0; i < rtStroke.Strokes.Count; i++)
            {
                rtStroke.Strokes[i].Scale(
                    ratioWidthReceive, ratioHeightReceive);
            }

            // Draw the stroke
            inkOverlay.Ink.AddStrokesAtRectangle(rtStroke.Strokes, rtStroke.Strokes.GetBoundingBox());
            
            // Refresh is needed if your stroke is inside a control like here in the picture box: pbRTDoc
            Refresh();
        }

        /// <summary>
        /// Handle the reception of an RTStrokeRemove object.
        /// </summary>
        /// <param name="rtStrokeRemove">Stroke to remove</param>
        private void RTStrokeRemoveReceived(RTStrokeRemove rtStrokeRemove)
        {
            Log("StrokeRemove ID:" + rtStrokeRemove.StrokeIdentifier.ToString());
            
            // Get the id of the stroke to remove
            // Lookup in inkOverlay.Ink.Strokes if there is a stroke with this id
            // if so, delete it programatically
            Guid rtStrokeRemoveGuid = rtStrokeRemove.StrokeIdentifier;
            if (RemoveStroke(rtStrokeRemoveGuid))
            {
                this.Refresh();
            }

            // Pri3: else log it / except / assert (or something)
        }
        
        private bool RemoveStroke(Guid strokeID)
        {
            Guid exPSI = RTStroke.ExtendedPropertyStrokeIdentifier;

            foreach (Stroke s in inkOverlay.Ink.Strokes)
            {
                // It is possible that a remote user will trigger this method,
                // after the local user has commenced a stroke but before
                // completing it.  In that case, the stroke has already been
                // added to the inkOverlay.Ink.Strokes collection, but it is
                // incomplete.  It doesn't have its ExtendedProperties, so we 
                // can't compare against it.  Handle the exception and carry on.
                try
                {
                    if (strokeID.Equals(new Guid((string)s.ExtendedProperties[exPSI].Data)))
                    {
                        inkOverlay.Ink.DeleteStroke(s);
                        return true;
                    }
                }
                catch (ArgumentException) { } // Ignore
            }

            return false;
        }

        /// <summary>
        /// Handle the reception of a Page object.
        /// </summary>
        /// <param name="p">Page received</param>
        private void PageReceived(Page page)
        {
            Log("It's a Page object,");

            // Store the page object to show later
            // If it is a "random" page, add it to the RTDoc
            Log("Structure Before AddPageToRTDocument:");
            DisplayRTDocumentStructureInfo();
        
            // Attempt to add the page to the RTDoc
            try
            {
                rtDocumentHelper.AddPageToRTDocument( page );
            }
            catch(InvalidOperationException)
            {
                // TOC not received.  Cache the page and request the TOC if we haven't already.
                if( this.cachedPages == null )
                {
                    this.cachedPages = Queue.Synchronized(new Queue());

                    // Pri2: Do we need to send this more often than just once?  If so, what should our solution be?
                    rtDocumentHelper.SendTocRequest();
                }

                this.cachedPages.Enqueue(page);
                return;
            }
        
            // Indicate that you are receiving a page from a RTDocument
            // Note: The pages are not necessarily sent in order of the pages in the doc
            ++pageReceivedCounter;
            if( pageReceivedCounter < rtDocument.Organization.TableOfContents.Count )
            {
                statusBar.StatusMessage = string.Format(CultureInfo.CurrentCulture, Strings.ReceivingPage, 
                    (pageReceivedCounter+1), rtDocument.Organization.TableOfContents.Count.ToString(CultureInfo.InvariantCulture)); 
            }
            else
            {
                // On last page, remove receiving message
                statusBar.SetReadyStatusMessage();
            }

            Log(string.Format(CultureInfo.CurrentCulture, "I just added a Page object with the id: {0},", 
                page.Identifier));
            Log(string.Format(CultureInfo.CurrentCulture, "The TOC Node identifier is: {0},", 
                rtDocumentHelper.TOCNodeIdentifierToPageIdentifier(page.Identifier)));

            Log("Structure After AddPageToRTDocument:");
            DisplayRTDocumentStructureInfo();

            // Insert the thumbnail for the new page
            TOCNode node = rtDocumentHelper.PageToTOCNode(page);
            int pageIndex = rtDocument.Organization.TableOfContents.IndexOf( node );

            // If previous slides haven't been recieved, just shove it at the end
            if( pageIndex > thumbnailsView.Items.Length )
                pageIndex = thumbnailsView.Items.Length;

            thumbnailsView.InsertThumbnail( page.Image, node.Title, pageIndex, node.Identifier );

            // If it's the first page, display it
            // Pri3: Doesn't this fail with a NullRefException if this isn't the first page??
            Guid pFirst = rtDocument.Organization.TableOfContents[0].ResourceIdentifier;
            if( page.Identifier.Equals(pFirst) )
            {
                ShowPage(rtDocument.Organization.TableOfContents[0].Identifier);
                index = 0;
            }
            UpdateNavigationButtonState();

        }

        /// <summary>
        /// Handle the reception of a RTPageAdd object. The
        /// RTPageAdd allows to dynamically add pages on a document
        /// on client(s).
        /// </summary>
        /// <param name="pa">Page to add</param>
        private void RTPageAddReceived(RTPageAdd pa)
        {
            Log("It's a RTPageAdd object,");

            // Pri2: verify that this new page belongs to this document
            if (rtDocumentHelper == null)
            {
                // Pri3: Send an RTDocumentRequest
                return;
            }

            pa.TOCNode.Resource = pa.Page;
            if( pa.PreviousSiblingIdentifier != Guid.Empty )
            {
                TOCNode prevSib = rtDocument.Organization.TableOfContents[pa.PreviousSiblingIdentifier];
                int prevSibIndex = rtDocument.Organization.TableOfContents.IndexOf( prevSib );
                rtDocument.Organization.TableOfContents.Insert( prevSibIndex+1, pa.TOCNode );
            }
            else
            {
                rtDocument.Organization.TableOfContents.Add( pa.TOCNode );
            }
            rtDocument.Resources.Pages.Add( pa.Page.Identifier, pa.Page );

            int pageIndex = rtDocument.Organization.TableOfContents.IndexOf( pa.TOCNode );
            thumbnailsView.InsertThumbnail( pa.Page.Image, pa.TOCNode.Title, pageIndex, pa.TOCNode.Identifier );
            statusBar.SetMaxPage( index+1, rtDocument.Organization.TableOfContents.Count );
            UpdateNavigationButtonState();
            return;
        }

        /// <summary>
        /// Handle the reception of an RTNodeChanged object. 
        /// The RTNodeChanged allows to navigate
        /// through pages on the client(s).
        /// </summary>
        /// <param name="nav">Navigation object received</param>
        private void RTNodeChangedReceived(RTNodeChanged nav)
        {
            try
            {
                Log("It's an RTNodeChanged object,");

                // Move to the correct page
                Log(string.Format(CultureInfo.CurrentCulture, 
                    "RTNodeChanged object id: {0 }pageid that will be displayed: {1},", 
                    nav.OrganizationNodeIdentifier.ToString(), 
                    rtDocumentHelper.TOCNodeIdentifierToPageIdentifier(nav.OrganizationNodeIdentifier)));

                int newIndex = rtDocument.Organization.TableOfContents.IndexOf(
                    rtDocument.Organization.TableOfContents[ nav.OrganizationNodeIdentifier ] );
                this.navigateToIndex( newIndex, false );
            }
            catch
            {
                RtlAwareMessageBox.Show(this, Strings.TryResendAllSlidesButton, string.Empty, MessageBoxButtons.OK, 
                    MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                ShowPage( pageShowing );
            }
        }

        /// <summary>
        /// Handle the reception of an RTDocument object. The rtDocument received
        /// does not contain the actual pages (the pages are received separately),
        /// so this function mainly handles the reception of the document structure 
        /// (TOC, number of pages, guid of pages, etc.) on the client(s).
        /// </summary>
        /// <param name="rtDocReceived">rtDocument structure received</param>
        private void RTDocumentReceived(RTDocument rtDocReceived)
        {
            Log("It's an RTDocument object,");

            // Set the rtDocument only if you receive a new document (not a repeat)
            if( (rtDocument == null) || (!rtDocReceived.Identifier.Equals(rtDocument.Identifier)) )
            {
                // Clean up the screen
                DeleteAllStrokes();

                Image tempImg = pbRTDoc.Image;
                pbRTDoc.Image = null;
                if( tempImg != null )
                    tempImg.Dispose();

                Log("The document received is different than the one already open!");
                rtDocument = rtDocReceived;

                Log(string.Format(CultureInfo.CurrentCulture, "rtDocID in RTDocumentReceived method: {0}", 
                    rtDocument.Identifier.ToString()));

                Log("Info before creating RTDocHelper,");
                DisplayRTDocumentOnlyStructureInfo();

                rtDocumentHelper = new RTDocumentHelper( presentationCapability, rtDocument );

                // Pri2: is crntPage == 0 correct??
                statusBar.SetMaxPage( 0, rtDocument.Organization.TableOfContents.Count );

                statusBar.StatusMessage = string.Format(CultureInfo.CurrentCulture, Strings.ReceivingPage_of_, 
                    rtDocument.Organization.TableOfContents.Count.ToString(CultureInfo.InvariantCulture)); 

                thumbnailsView.Items = new ThumbnailListViewItem[0];

                // Update the title bar of the form 
                this.Text = rtDocument.Metadata.Title + " - " + presentationCapability.Name;

                Log("Doc organization received:");
                DisplayRTDocumentStructureInfo();

                // Initialize the received page counter
                pageReceivedCounter = 0;

                if( this.cachedPages != null )
                {
                    // In the odd case that we have pages waiting, and the wrong TOC is received,
                    //  we'll try to add all the pages, fail, and then just discard them all.
                    for( int cnt = cachedPages.Count; cnt > 0; --cnt )
                    {
                        this.PageReceived( (Page)cachedPages.Dequeue() );
                    }

                    this.cachedPages = null;
                }
            }
        }

        /// <summary>
        /// Handle the reception of an rtFrame object. This is used to handle the erase all 
        /// message that removes all strokes on the screen.
        /// </summary>
        private void RTFrameReceived(RTFrame rtFrameReceived)
        {
            if (rtFrameReceived.ObjectTypeIdentifier != Guid.Empty)
            {
                // If receive a delete all stroke message
                if (rtFrameReceived.ObjectTypeIdentifier == new Guid(RTDocumentHelper.constEraseAllGuid))
                {
                    // Delete all stroke
                    DeleteAllStrokes();
                } 
                    // This code is to request remote capa to open a a local rtd file
                    // rtFrameReceived.object contains the string path to the file to open
                    // Pri1: This code is temporary
                else if ((!presentationCapability.IsSending) &&
                    (rtFrameReceived.ObjectTypeIdentifier == new Guid(constOpenRemoteGuid)))
                {
                    openLocalFile((string)rtFrameReceived.Object);
                }
                    // JCB: Removed
                    // This code is to exits the remote capability viewer when the initiator exits
                    // Pri1: This feature and is going to be removed, because it will be taking care at the networking level
                else if ((!presentationCapability.IsSender) &&
                    (rtFrameReceived.ObjectTypeIdentifier == new Guid(constExitRemoteGuid)))
                {
                    // Exit the viewer capability
                    // Pri1: This will be taking care at the networking level
                    this.Close();
                }
                else if(rtFrameReceived.ObjectTypeIdentifier == new Guid(RTDocumentHelper.constResendTocGuid))
                {
                    rtDocumentHelper.SendDocumentBody();
                }
            }
        }

        #endregion

        #region ShowPage method

        /// <summary>
        /// Display the page related to the tocNodeIdentifier in the picture box
        /// pbRTDoc. If the page is not found (tocNodeIdentifier correspond to a page
        /// guid that has not been uploaded), a "page not found!" image is displayed
        /// on the picture box.
        /// </summary>
        /// <param name="tocNodeIdentifier">TOC node identifier of the page to display</param>
        private void ShowPage(Guid tocNodeIdentifier)
        {
            Log("ShowPage,");

            #region Sanity Checks
            // Sanity is still overrated...
            if (rtDocument == null)
            {
                Log("rtDocument == null,");
                throw new Exception(Strings.RtDocumentIsNull);
            }

            if (rtDocumentHelper == null)
            {
                Log("rtDocumentHelper == null,");
                return; // Pri3: Send an RTDocumentRequest
            }

            if (tocNodeIdentifier == Guid.Empty)
            {
                Log("tocNodeIdentifier == Guid.Empty,");
                return;
            }
            #endregion

            #region Page Lookup
            // Attempt a lookup on the TOCNode for its page
            Guid pageIdentifier = rtDocumentHelper.TOCNodeIdentifierToPageIdentifier(tocNodeIdentifier);

            if (pageIdentifier == Guid.Empty)
            {
                Log("pageIdentifier == Guid.Empty");
                throw new Exception(Strings.UnableToFindPage);
            }
            if (pageShowing == pageIdentifier)
            {
                Log("pageShowing == pageIdentifier,");
                return;
            }
            if (!rtDocument.Resources.Pages.ContainsKey(pageIdentifier))
            {
                Image tempImg = pbRTDoc.Image;
                pbRTDoc.Image = ThumbnailListView.GenerateBitmapAlertText(Strings.PageNotFound);
                if( tempImg != null )
                    tempImg.Dispose();
                return;
            }
            #endregion

            // Change selected image in thumbnailsView
            // Then find and select the correct thumbnail
            int cnt = 0;  // for loops are too much work.  the compiler accounts for laziness, right?
            foreach( ThumbnailListViewItem item in thumbnailsView.Items )
            {
                if( item.tag == tocNodeIdentifier )
                {
                    thumbnailsView.SelectedIndex = cnt;
                    break;
                }
                cnt++;
            }

            // Let helper know which page we are showing
            rtDocumentHelper.CurrentOrganizationNodeIdentifier = tocNodeIdentifier;

            // get the current page, then display it
            int pageIndex = 1 + rtDocument.Organization.TableOfContents.IndexOf(
                rtDocument.Organization.TableOfContents[tocNodeIdentifier]);

            // due to the possibilities of weird networking, it's best to just account
            // for the case that we don't know a max page val, even if it's with a hack
            // like this.
            if( statusBar.MaxPageVal < pageIndex )
                statusBar.SetMaxPage( pageIndex, pageIndex );
            else
                statusBar.CurrentPage = pageIndex;

            Page p = rtDocument.Resources.Pages[pageIdentifier];
            Log(string.Format(CultureInfo.CurrentCulture, "start displaying the page id: {0}", p.Identifier));

            // Note: the PictureBox property SizeMode has been set
            // to StretchImage in the design view, so the slide
            // size fits the picture box size

            lock(this)
            {
                // Show the image associated with the page
                Image temp = pbRTDoc.Image;
                pbRTDoc.Image = (p.Image != null) ? (Image)p.Image.Clone() : null;
                if( temp != null )
                    temp.Dispose();

                // Compute the ratio of the document
                if( p.Image != null )
                {
                    pbRatio = (double)p.Image.Height/(double)p.Image.Width;
                }
                else // p.Image == null --> whiteboard slide
                {
                    pbRatio = (double)constHeightPageSend / (double)constWidthPageSend;
                }
            }

            // Reinitialize the pbRTDoc size to fit on the form taking in account the ratio
            ScaleAndPositionPbImage();

            pageShowing = pageIdentifier;
        }

        #endregion

        #region Stroke related event handler

        /// <summary>
        /// Stroke event handler fired when a stroke is just finished.
        /// </summary>
        /// <remarks>Notice that even if we use a InkOverlay object and not a InkCollector
        /// object, the second input parameter is of type InkCollectorStrokeEventArgs</remarks>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments (type InkCollectorStrokeEventArgs)</param>
        private void inkOverlay_Stroke(object sender, InkCollectorStrokeEventArgs e)
        {
            if (e.Stroke.DrawingAttributes.Transparency != 255)
            {
                Log("A stroke has been drawn!");

                // Resize the stroke before sending
                Log("Resize the stroke,");
                DisplaySizeInfo();
                Microsoft.Ink.Strokes strokes = inkOverlay.Ink.CreateStrokes();
                strokes.Add(e.Stroke);
                Microsoft.Ink.Ink newInk = new Microsoft.Ink.Ink();
                newInk.AddStrokesAtRectangle(strokes, strokes.GetBoundingBox());
                Microsoft.Ink.Stroke copiedStroke = newInk.Strokes[0];
                // Note: rtDocument.Resources.Pages[pageShowing].Image.Width cannot be 
                // used here because it's 5760 when open a PPT document
                // Ditto for rtDocument.Resources.Pages[pageShowing].Image.Height, 
                // the value is 4320
                float ratioWidthSend = (float)constWidthPageSend/(float)pbRTDoc.Width;
                float ratioHeightSend = (float)constHeightPageSend/(float)pbRTDoc.Height;
                Log(string.Format(CultureInfo.CurrentCulture, "RatioWidthSend={0},  RatioHeightSend={1}, ", 
                    ratioWidthSend.ToString(CultureInfo.InvariantCulture), 
                    ratioHeightSend.ToString(CultureInfo.InvariantCulture)));
                copiedStroke.Scale(ratioWidthSend, ratioHeightSend);

                // Send the resized stroke
                Log("Send a resized stroke,");
                rtDocumentHelper.SendStrokeAdd(copiedStroke);

                // Copy the guid in the extendend property generated inside copieStroke 
                // by rtDocumentHelper.BeginSendStrokeAdd in the extended property of the
                // local stroke
                // Note: it is very important to do that because when the user
                // delete a stroke, the stroke guid of the local and remote
                // stroke must match (copieStroke is the stroke that is sent,
                // e.Stroke is the local stroke)
                e.Stroke.ExtendedProperties.Add(RTStroke.ExtendedPropertyStrokeIdentifier, 
                    copiedStroke.ExtendedProperties[RTStroke.ExtendedPropertyStrokeIdentifier].Data);
            }
        }

        /// <summary>
        /// Stroke event handler fired when a stroke is deleting.
        /// </summary>
        /// <remarks>The input parameter e contains a collection of strokes 
        /// e.StrokesToDelete because the user might delete several strokes
        /// at once</remarks>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void inkOverlay_StrokesDeleting(object sender, InkOverlayStrokesDeletingEventArgs e)
        {
            Log("Stroke deletion...");

            // Loop through all the strokes that just has been deleted
            foreach(Stroke s in e.StrokesToDelete)
            {
                Log(string.Format(CultureInfo.CurrentCulture, "Inside foreach Guid of the stroke to delete: {0}", 
                    s.ExtendedProperties[RTStroke.ExtendedPropertyStrokeIdentifier].Data.ToString()));
                try 
                {
                    rtDocumentHelper.SendStrokeRemove(s);
                }
                catch (Exception ex)
                {
                    RtlAwareMessageBox.Show(this, ex.ToString(), string.Empty, MessageBoxButtons.OK, 
                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                }
            }
        }

        #endregion

        #region Error message box functions

        /// <summary>
        /// Error message dialog box used in case you try to navigate
        /// through pages when no document are open (whiteboard mode).
        /// </summary>
        private void DocumentNotLoadedMessage()
        {
            RtlAwareMessageBox.Show(this, Strings.YouMustFirstOpenADocument, Strings.Error, MessageBoxButtons.OK, 
                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
        }

        #endregion

        #region RTDocument Trace functions

        /// <summary>
        /// Logs the header info of the rtDocument.
        /// </summary>
        private void DisplayHeaderInfo()
        {
            Log(string.Format(CultureInfo.CurrentCulture, "rtDoc.Identifier: {0},", rtDocument.Identifier));
            Log(string.Format(CultureInfo.CurrentCulture, "rtDoc.Metadata.Title: {0},", rtDocument.Metadata.Title));
            Log(string.Format(CultureInfo.CurrentCulture, "rtDoc.Organization.TableOfContents.Count: {0},", 
                rtDocument.Organization.TableOfContents.Count));
            Log(string.Format(CultureInfo.CurrentCulture, "rtDoc.Resource.Pages.Count: {0},", 
                rtDocument.Resources.Pages.Count));
        }

        /// <summary>
        /// Logs the page info of the rtDocument.
        /// </summary>
        private void DisplayPageInfo()
        {
            foreach (Page p in rtDocument.Resources.Pages.Values)
            {
                Log(string.Format(CultureInfo.CurrentCulture, "PAGE_ID: {0},", p.Identifier.ToString()));
            }
        }

        /// <summary>
        /// Logs the structure of the rtDocument with the page identifiers.
        /// </summary>
        private void DisplayRTDocumentStructureInfo()
        {
            Log("******* BEGIN Display Structure **********");
            DisplayHeaderInfo();

            for (int i = 0; i < rtDocument.Organization.TableOfContents.Count; i++)
            {
                Log(string.Format(CultureInfo.CurrentCulture, "TOC_ID[{0}]: {1},", i, 
                    rtDocument.Organization.TableOfContents[i].Identifier.ToString()));
                Log(string.Format(CultureInfo.CurrentCulture, 
                    "rtDocumentHelper.TOCNodeIdentifierToPageIdentifier(TOC_ID): {0},",
                    rtDocumentHelper.TOCNodeIdentifierToPageIdentifier(rtDocument.Organization.
                    TableOfContents[i].Identifier)));
            }
            DisplayPageInfo();
            Log("******* END Display Structure **********,");
        }

        /// <summary>
        /// Logs the structure info of the rtDocument without the pag identifiers.
        /// </summary>
        private void DisplayRTDocumentOnlyStructureInfo()
        {
            Log("******* BEGIN Display Structure Of RTDoc Only **********");
            DisplayHeaderInfo();
            for (int i = 0; i < rtDocument.Organization.TableOfContents.Count; i++)
            {
                Log(string.Format(CultureInfo.CurrentCulture, "TOC_ID[{0}]: {1},", i, 
                    rtDocument.Organization.TableOfContents[i].Identifier.ToString()));
            }
            DisplayPageInfo();
            Log("******* END Display Structure **********,");
        }

        /// <summary>
        /// Logs the page size info of the rtDocument and the picture box.
        /// </summary>
        private void DisplaySizeInfo()
        {
            
            // TODO: Decide on debug for ship
            Log(string.Format(CultureInfo.CurrentCulture, "Size Info: pbRTDoc.Width = {0} bRTDoc.Height = {1},", 
                pbRTDoc.Width.ToString(CultureInfo.InvariantCulture),
                pbRTDoc.Height.ToString(CultureInfo.InvariantCulture)));
        }

        #endregion

        #region UI buttons event handlers

        // UI buttons event handlers

        /// <summary>
        /// Delete all the strokes on the picture box.
        /// </summary>
        private void DeleteAllStrokes()
        {
            // Iterate backwards because we are deleting from a collection
            for (int i = inkOverlay.Ink.Strokes.Count - 1; i >= 0; i--)
            {
                try
                {
                    inkOverlay.Ink.DeleteStroke(inkOverlay.Ink.Strokes[i]);
                }
                catch (ArgumentException)
                {
                    // It is possible that a remote user will trigger this method,
                    // after the local user has commenced a stroke but before
                    // completing it.  In that case, the stroke has already been
                    // added to the inkOverlay.Ink.Strokes collection, but it is
                    // incomplete.  Carry on.
                }
            }

            this.Refresh();
        }

        /// <summary>
        /// Stores all the Ink in a hashtable. The key in the hashtable
        /// is the pageIdentifier. The value in the hastable is a Ink
        /// object corresponding to the Ink on the screen.
        /// </summary>
        /// <remarks>This function is used to save Ink when you navigate pages.
        /// See navigationBar_NavigationClicked event handler.</remarks>
        /// <param name="pageIdentifier">Guid of the page corresponding to the Ink
        /// to save</param>
        private void StoreInk(Guid pageIdentifier)
        {
            // Important: Must Clone to duplicate the Ink object, otherwise
            // the object that will be stored in the hashtable will
            // point to inkOverlay.Ink, so all the strokes will be
            // remove when you call inkOverlay.Ink.DeleteStrokes()
            Ink savedInk = inkOverlay.Ink.Clone();

            // Save the strokes along with the page size
            // Note: we need to store the picture box size in order to be able to resize 
            // the strokes correctly on restore (if the picture box size has changed in the meantime)
            ScaledInk scaledInk = new ScaledInk(savedInk, pbRTDoc.Width, pbRTDoc.Height);

            hashtablePageStrokes[ pageIdentifier.ToString() ] = scaledInk;
        }

        /// <summary>
        /// Retrieve all the Ink from a hashtable. The key in the hashtable
        /// is the pageIdentifier. The value in the hastable is a Ink
        /// object corresponding to the Ink on the screen that has been saved
        /// for the given page.
        /// </summary>
        /// <remarks>This function is used to retrieve Ink when you navigate pages.
        /// See navigationBar_NavigationClicked event handler.</remarks>
        /// <param name="pageIdentifier">Guid of the page corresponding to the Ink
        /// to retreive</param>
        private void RetrieveInk(Guid pageIdentifier)
        {
            if ( hashtablePageStrokes.ContainsKey(pageIdentifier.ToString()) )
            {
                inkOverlay.Enabled = false; // In order to set the ink

                ScaledInk scaledInk = (ScaledInk)hashtablePageStrokes[pageIdentifier.ToString()];
                inkOverlay.Ink = scaledInk.PageInk;

                // Scale all strokes on the current screen
                float scaleX = (float)pbRTDoc.Size.Width/(float)scaledInk.PageWidth;
                float scaleY = (float)pbRTDoc.Size.Height/(float)scaledInk.PageHeight;

                // Rescale the strokes
                inkOverlay.Ink.Strokes.Scale(scaleX, scaleY);

                prevPbSize = pbRTDoc.Size;

                inkOverlay.Enabled = true;
            }
        }

        /// <summary>
        /// Show the page corresponding to page number stored in the global 
        /// variable index. Also send a page change message to all the clients to
        /// make sure they navigate to the same page. 
        /// </summary>
        /// <remarks>This function is used when you navigate pages.
        /// See navigationBar_NavigationClicked event handler.</remarks>
        private void UpdateCurrentPage()
        {
            // tocNodeIdentifier corresponding to the new index
            // Pri3: Flat TOC in this version of the capability, but we should take in account hierchical TOC
            Guid tocNodeIdentifier = rtDocument.Organization.TableOfContents[index].Identifier;

            // Display the new page
            ShowPage(tocNodeIdentifier);

            // Send the new selected page (this must be called after the document body has been sent)
            Guid pageIdentifier = rtDocumentHelper.TOCNodeIdentifierToPageIdentifier(tocNodeIdentifier);
            Page p = rtDocument.Resources.Pages[pageIdentifier];
            rtDocumentHelper.SendPageChange(p);

            Log(string.Format(CultureInfo.CurrentCulture, "{0}-> Page ID sent: {1},", DateTime.Now.ToString(), 
                p.Identifier.ToString()));

            Log("Structure Info on SendPageChange,");
            DisplayRTDocumentStructureInfo();
        }


        /// <summary>
        /// This function disable the navigation buttons
        /// </summary>
        private void disableNavigationButtons()
        {
            // Disable the button on the toolbar
            this.miPreviousSlide.Enabled = this.tbbPrevious.Enabled = false;
            this.miNextSlide.Enabled = this.tbbNext.Enabled = false;
            // Disable the menu on the menubar
            this.miPreviousSlide.Enabled = this.miPreviousSlide.Enabled = false;
            this.miNextSlide.Enabled = this.miNextSlide.Enabled = false;
        }

        /// <summary>
        /// This function disable the navigation button in the direction(s)
        /// that you cannot move anymore. 
        /// </summary>
        private void UpdateNavigationButtonState()
        {
            this.miPreviousSlide.Enabled = this.tbbPrevious.Enabled = (index > 0);
            this.miNextSlide.Enabled = this.tbbNext.Enabled = (index < rtDocument.Organization.TableOfContents.Count - 1);
        }

        /// <summary>
        /// Move forward.
        /// For every page change, the Ink is saved and the Ink of the next page
        /// is restored. Also an page navigation message is sent to the remote(s)
        /// capability so they can be on the same page 
        /// </summary>
        private void navigateForward()
        {
            if (index < rtDocument.Organization.TableOfContents.Count - 1)
            {
                this.navigateToIndex( index+1, true );
            }
        }

        /// <summary>
        /// Move forward.
        /// For every page change, the Ink is saved and the Ink of the next page
        /// is restored. Also an page navigation message is sent to the remote(s)
        /// capability so they can be on the same page 
        /// </summary>
        private void navigateBackward()
        {
            if (index > 0)
            {
                this.navigateToIndex( index-1, true );
            }
        }

        /// <summary>
        /// Moves to page with the index specified, updating clients (if desired), 
        /// thumbnailView, ink, and navigation buttons.
        /// </summary>
        /// <param name="navToIndex">The index to move to.</param>
        /// <param name="synchronize">Whether to synchronize clients with this page change.</param>
        private void navigateToIndex(int navToIndex, bool synchronize)
        {
            // It is possible that the local user is drawing a stroke when a 
            // page change arrives.  Loop a little waiting for the stroke to 
            // complete.  It is possible for them to start a new stroke before
            // the following actions complete (so this is weak), but I don't
            // see any other way to handle an "in-process" ink stroke since we
            // don't know when it starts, so we can't lock. jasonv 10/11/2006
            bool collecting = inkOverlay.CollectingInk;
            while (inkOverlay.CollectingInk)
            {
                Thread.Sleep(100);
            }

            if ( (0 <= navToIndex) && (navToIndex < rtDocument.Organization.TableOfContents.Count) 
                && (navToIndex != index) )
            {
                try
                {
                    // Store the Ink of the page
                    // unless we were writing when a page change came in, in 
                    // which case we want to delete the last stroke first
                    // since nobody got it
                    if (collecting)
                    {
                        inkOverlay.Ink.DeleteStroke(inkOverlay.Ink.Strokes[inkOverlay.Ink.Strokes.Count - 1]);
                    }

                    StoreInk(pageShowing);

                    // Change the current page index
                    // TODO: eliminate (or find a reason for the existence of) 'this.pageShowing'
                    this.index = navToIndex;

                    // Clean up the screen
                    DeleteAllStrokes();

                    // Update the Backward/Forward buttons (push the navigation button in the 
                    // direction(s)that you cannot move anymore)
                    UpdateNavigationButtonState();

                    if (synchronize)
                    {
                        // Display the new page on the sender and viewer
                        UpdateCurrentPage();
                    }
                    else
                    {
                        // Display the new page only on the sender
                        Guid tocNodeIdentifier = rtDocument.Organization.TableOfContents[index].Identifier;
                        ShowPage(tocNodeIdentifier);
                    }

                    // Display the Ink related to the page that just showed-up
                    RetrieveInk(pageShowing);
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());

                    // Pri2: Catch different types of exception to have more precise messages
                    // instead of one message for all
                    RtlAwareMessageBox.Show(this, Strings.ErrorOccuredDuringNavigation, Strings.Error, 
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 
                        (MessageBoxOptions)0);
                }
            }
        }

        /// <summary>
        /// This method allows the initiator to open a .ppt or .rtd file.
        /// After the file is open, all the slides are sent to the client(s) and the
        /// client(s) are positionned to the same page than the initiator.
        /// </summary>
        /// <remarks>If you open a .ppt file, a corresponding .rtd file is automatically 
        /// created with the same name and saved in the same folder. If there is an 
        /// existing .rtd file with the same name in  this folder, it will be overwrited</remarks>
        /// <param name="fileMenu">This parameter is for extensibility. The only value so far
        /// is OpenSlideDeck</param>
        private void openFile()
        {
            System.Windows.Forms.OpenFileDialog cdlg = new OpenFileDialog();
            cdlg.Filter = "All Presentation Documents (*.ppt, *.pptx, *.rtd)|" +
                "*.ppt;*.pptx;*.rtd|PowerPoint document files (*.ppt,*.pptx)|*.ppt;*.pptx|RTDocument files (*.rtd)|*.rtd" ;
            DialogResult dr = cdlg.ShowDialog();

            try
            {
                if (dr == DialogResult.OK)
                {
                    string fileName = cdlg.FileName;
                    string extension = System.IO.Path.GetExtension(fileName).ToLower(CultureInfo.InvariantCulture);

                    if (extension == ".ppt" || extension == ".pptx")
                    {
                        #region Open a PPT file
                        lock(this)
                        {
                            statusBar.SetWaitStatusMessage(Strings.ImportingPowerpointDocument);

                            rtDocument = PPTConversion.PPT2RTDocument(fileName);

                            // Autosave the PPT document a RTD document in the same directory as PPT
                            string outFileName = fileName.Substring(0, 
                                fileName.Length - extension.Length) + ".rtd";

                            FileStream fs = new FileStream(outFileName, FileMode.Create);
                            BinaryFormatter bf = new BinaryFormatter();
                            bf.Serialize(fs, rtDocument);
                            fs.Close();

                            statusBar.SetReadyStatusMessage();
                        }
                        #endregion
                    }
                    else if (extension == ".rtd")
                    {
                        #region Open a RTD file
                        lock(this)
                        {
                            BinaryFormatter myBinaryFormat = new BinaryFormatter();
                            try 
                            {
                                FileStream myInputStream = System.IO.File.OpenRead(fileName);
                                rtDocument = (RTDocument) myBinaryFormat.Deserialize(myInputStream);
                            }
                            catch (Exception)
                            {
                                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                                    Strings.ErrorInOpeningVersion, fileName), string.Empty, MessageBoxButtons.OK, 
                                    MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                                return;
                            }
                        }
                        #endregion
                    } 
                    else
                    {
                        RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                            Strings.ErrorInOpeningFileType, fileName), string.Empty, MessageBoxButtons.OK, 
                            MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                        return;
                    }

                    Log(string.Format(CultureInfo.CurrentCulture, "rtDocID = {0}", rtDocument.Identifier.ToString()));

                    // Update the title bar of the form 
                    // with the document name followed by the capability name
                    this.Text = rtDocument.Metadata.Title + " - " + this.presentationCapability.Name;

                    // Clean up the screen
                    DeleteAllStrokes();

                    Log("On open doc: info before creating RTDocHelper,");
                    DisplayRTDocumentOnlyStructureInfo();
                    rtDocumentHelper = new RTDocumentHelper(this.presentationCapability, rtDocument);

                    Log("Structure Info on open after creating RTDocHelper,");
                    DisplayRTDocumentStructureInfo();

                    #region Populate thumbnails
                    ArrayList thumbnails = new ArrayList();

                    using (Bitmap bp = new Bitmap( thumbnailsView.ImageSize.Width, thumbnailsView.ImageSize.Height, 
                               System.Drawing.Imaging.PixelFormat.Format24bppRgb))
                    {
                        using (Graphics g = Graphics.FromImage(bp))
                        {
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                            foreach( TOCNode tn in rtDocument.Organization.TableOfContents )
                            {
                                Page p = (Page)tn.Resource;

                                ThumbnailListView.CreateSlideThumbnail( thumbnailsView.ImageSize,
                                    p.Image, g, tn.Title );

                                ThumbnailListViewItem item = new ThumbnailListViewItem();
                                item.thumbnail = ((Bitmap)bp.Clone());
                                item.tag = tn.Identifier;
                                thumbnails.Add(item);
                            }
                        }
                    }

                    thumbnailsView.Items = (ThumbnailListViewItem[])thumbnails.ToArray(typeof(ThumbnailListViewItem));
                    #endregion

                    // Initialize the index on the first slide
                    index = 0;

                    // Show the current page
                    statusBar.SetMaxPage( 1, rtDocument.Organization.TableOfContents.Count );
                    ShowPage(rtDocument.Organization.TableOfContents[index].Identifier);

                    if( presentationCapability.IsSender )
                    {
                        EnableSlideSend(false);
                        statusBar.StatusMessage = Strings.SendingSlides;

                        rtDocumentHelper.BeginSendAllSlides(new AsyncCallback(SlidesSent), null);
                    }

                    // Update the Backward/Forward buttons (push the navigation button in the 
                    // direction(s)that you cannot move anymore)
                    UpdateNavigationButtonState();
                }
            }
            catch(Exception ex) 
            {
                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, Strings.ErrorInOpening, 
                    cdlg.FileName, ex.Message.ToString()), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                return;
            }
        }

        /// <summary>
        /// This method allows the initiator to resend all slides.
        /// </summary>
        private void resendSlides()
        {
            try
            {
                EnableSlideSend(false);
                statusBar.StatusMessage = Strings.ResendingSlides;

                rtDocumentHelper.BeginSendAllSlides(new AsyncCallback(SlidesSent), null);
                // Display the current page on the sender and viewer
                UpdateCurrentPage();
            }
            catch (Exception ex)
            {
                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                    Strings.ErrorDuringCommunication, ex.Message.ToString()), string.Empty, MessageBoxButtons.OK, 
                    MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
        }

        private void SlidesSent(IAsyncResult ar)
        {
            Debug.Assert(InvokeRequired);
            this.Invoke(new AsyncCallback(_SlidesSent), new object[] { ar });
        }

        private void _SlidesSent(IAsyncResult ar)
        {
            try
            {
                EnableSlideSend(true);
                statusBar.StatusMessage = string.Empty;

                AsyncResult aResult = (AsyncResult)ar;
                RTDocumentHelper.SendAllSlidesHandler aDel = (RTDocumentHelper.SendAllSlidesHandler)aResult.AsyncDelegate;
                aDel.EndInvoke(ar);
            }
            catch (Exception e)
            {
                RtlAwareMessageBox.Show(this, e.ToString(), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
        }

        private void CurrentSlideSent(IAsyncResult ar)
        {
            Debug.Assert(InvokeRequired);
            this.Invoke(new AsyncCallback(_CurrentSlideSent), new object[] { ar });
        }

        private void _CurrentSlideSent(IAsyncResult ar)
        {
            try
            {
                EnableSlideSend(true);
                statusBar.StatusMessage = string.Empty;

                AsyncResult aResult = (AsyncResult)ar;
                Capability.SendObjectHandler aDel = (Capability.SendObjectHandler)aResult.AsyncDelegate;
                aDel.EndInvoke(ar);
            }
            catch (Exception e)
            {
                RtlAwareMessageBox.Show(this, e.ToString(), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
        }

        private void EnableSlideSend(bool enable)
        {
            tbbOpenFile.Enabled = enable;
            tbbResend.Enabled = enable;

            miOpen.Enabled = enable;
            miResendCurrentSlide.Enabled = enable;
        }

        #endregion

        #region Debug/Log functions

        /// <summary>
        /// Log a text.
        /// </summary>
        /// <param name="logText">String containing the text to be logged</param>
        private void Log(string logText)
        {
            if (debugMode)
            {
                System.Diagnostics.Debug.WriteLine(logText);
            }
        }

        #endregion

        #region InsertSnapshot / InsertWhiteboard code

        // Pri1: Cleaning Snapshot code
        // - window and applicationWindow variables: changed internal to 
        // private or maybe public for test automation
        // - Get rid of buttonSnapshot event handler
        // - Get rid of FSelectApplication.cs

        /// <summary>
        /// Handle the InsertSlides toolbar event. This toolbar conatins
        /// a button to insert blank slide and a button to insert a snapshot
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void tbInsertSlides_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            if (e.Button == tbbSnapshot)
            {
                if (isAppSelected())
                {
                    // Take a snapshot
                    window = applicationWindow;
                    InsertSnapshot();
                } 
                else
                {
                    // Display the drop down list to allow user to select an app for the snashop
                    // Pri2: Investigate if there is an easier way to position the menu
                    ctxtMenuSnapshotApps.Show(
                        tbInsertSlides,
                        new Point(tbInsertSlides.Location.X + (int) (imageListSlides.ImageSize.Width * 1.5f), 
                        tbInsertSlides.Location.X + imageListSlides.ImageSize.Height));
                }
            }
            else if (e.Button == tbbInsertSlide)
            {
                InsertWhiteboard();
            }
        }

        /// <summary>
        /// This allows initiator to take a snapshot of an application and 
        /// send to all the client(s).
        /// If a document is open, the snapshot(s) will be append at the 
        /// end of the document.
        /// </summary>
        /// <remarks>
        /// This method is declared as public so we can use it for test automation.
        /// </remarks>
        private void InsertSnapshot()
        {
            if( window.Minimized )
            {
                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                    Strings.SnapshotsOfMinimizedWindows, Environment.NewLine), string.Empty, MessageBoxButtons.OK, 
                    MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                return;
            }

            #region Convert the snapshot from an uncompressed bitmap to a Png compressed bitmap
            MemoryStream ms = new MemoryStream();
            try
            {
                window.WindowAsBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            }  
            catch
            {
                RtlAwareMessageBox.Show(this, Strings.ErrorWhileTakingSnapshot, string.Empty, MessageBoxButtons.OK, 
                    MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                return;
            }
            ms.Position = 0;
            Image img = Image.FromStream(ms);
            #endregion

            #region Bring the PresentationCapability back to the front of the z-order and display the snapshot on the PresentationCapability
            this.BringToFront();

            // Free up the memory of the image if needed and set the 
            //  pbRTDoc.Image property with the new snapshot image
            Image tempImg = this.pbRTDoc.Image;
            pbRTDoc.Image = (Image)img.Clone();
            if( tempImg != null )
                tempImg.Dispose();
            #endregion
            
            #region Create a new RTDocument Page from the snapshot
            Page page = new Page();
            page.Identifier = Guid.NewGuid();
            page.Image = img;
            page.MimeType = "image/png";
            #endregion

            // Send the snapshot page to insert on the remote capabilities
            TOCNode tn = rtDocumentHelper.SendPageInsert(page, string.Format(CultureInfo.CurrentCulture, 
                Strings.SnapshotOf, window.Text), index);

            statusBar.SetMaxPage( 1, rtDocument.Organization.TableOfContents.Count );

            // Add page to thumbnails
            thumbnailsView.InsertThumbnail( page.Image, tn.Title, index+1, tn.Identifier );

            this.navigateToIndex( index+1, true );
        }


        /// <summary>
        /// ctxtMenuSnapshotApps Popup event handler that will be fired just before
        /// the drop-down menu related to the snapshot button appears.
        /// </summary>
        /// <remarks>
        /// If one of the active application correspond to the application previousily
        /// selected, a check mark is added for this item
        /// </remarks>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void ctxtMenuSnapshotApps_Popup(object sender, System.EventArgs e)
        {
            // Clear all the items
            ctxtMenuSnapshotApps.MenuItems.Clear();
            this.windowsInMenu.Clear();

            // Add the one item by active application
            foreach (Win32Util.Win32Window window in Win32Util.Win32Window.ApplicationWindows)
            {
                // Note: we hook the menu item to the event handler: OnctxtMenuSnapshotAppsClick
                MenuItem mi = new MenuItem(window.Text, new EventHandler(OnctxtMenuSnapshotAppsClick));
                ctxtMenuSnapshotApps.MenuItems.Add(mi);
                windowsInMenu.Add(mi, window);
                if (applicationWindow != null)
                {
                    // Select the item corresponding to applicationWindow
                    if ( window.ThreadId == applicationWindow.ThreadId 
                        && window.Text == applicationWindow.Text )
                    {
                        // Add a check mark for this item
                        mi.Checked = true;
                    }
                }
            }
        }

        /// <summary>
        /// ctxtMenuSnapshotApps event handler that will be when an item of 
        /// the drop-down menu related to the snapshot button is clicked.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void OnctxtMenuSnapshotAppsClick(object sender, System.EventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            applicationWindow = (Win32Util.Win32Window)windowsInMenu[mi];

            // Enable the menu "Insert Screen Shot"
            // Note: this menu is disabled by default (design time)
            miInsertSnapshot.Enabled = true;

            // Take the 1st snapshot directly
            window = applicationWindow;
            InsertSnapshot();
        }

        /// <summary>
        /// This method allows to know if a applivation has been selected for snapshot
        /// </summary>
        /// <remarks>
        /// The snapshot button contains a dropdown list of application to select.
        /// The role of this function is to be able to know whether or not an application
        /// has been selected for snapshot, so we can display the drop-down list to
        /// the user if he/she tries to click the snapshot button when no application are
        /// selected 
        /// </remarks>
        /// <returns>boolean that tells if an application is selected for snapshot</returns>
        private bool isAppSelected()
        {
            // Search for 1st selected application
            // Note: we should not have more than one applicattion selected
            //       if it is the case for some reason, this method will take the
            //       1st one found
            foreach (Win32Util.Win32Window windowTmp in Win32Util.Win32Window.ApplicationWindows)
            {
                if (applicationWindow != null)
                {
                    // Select the item corresponding to applicationWindow
                    if (windowTmp.Text == applicationWindow.Text)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Inserts a blank slide immediately after the current slide, syncing any client.
        /// </summary>
        private void InsertWhiteboard()
        {
            Page page = new Page();
            page.Identifier = Guid.NewGuid();

            TOCNode tn = rtDocumentHelper.SendPageInsert(page, string.Format(CultureInfo.CurrentCulture, 
                Strings.Whiteboard, (index + 2)), index);

            statusBar.SetMaxPage( 1, rtDocument.Organization.TableOfContents.Count );

            // Add page to thumbnails
            thumbnailsView.InsertThumbnail( page.Image, tn.Title, index+1, tn.Identifier );
            
            this.navigateToIndex( index+1, true );
        }

        #endregion

        #region Pen device events

        /// <summary>
        /// Allows the top-the-pen scenario. This allows Tablet PC users to 
        /// use the top-of-pen to erease Ink.
        /// </summary>
        private void inkOverlay_CursorInRange(object sender, InkCollectorCursorInRangeEventArgs e)
        {
            // Ignore the case of the mouse cursor entering the control
            if (e.Cursor.Id != (int)CursorID.Mouse)
            {
                // Check to see if the pen cursor is inverted or the toolbar is in erase mode
                if(e.Cursor.Inverted || inkToolBar.Mode == InkToolBarMode.Eraser)        
                {
                    if(inkOverlay.EditingMode != InkOverlayEditingMode.Delete)
                    {
                        inkOverlay.EditingMode = InkOverlayEditingMode.Delete;
                    }
                }   
                else // Ink or highlighter
                {   
                    if(inkOverlay.EditingMode != InkOverlayEditingMode.Ink)
                    {
                        inkOverlay.EditingMode = InkOverlayEditingMode.Ink;
                    }
                }
            }
        }


        #endregion Pen device events

        #region Dynamic Form Layout & Design

        /// <summary>
        /// Simplifies determining what mode we're in without having a seperate instance variable just for this.
        /// </summary>
        private bool PortraitMode
        {
            get
            {
                Rectangle screenRect = Screen.GetBounds(this);
                if( screenRect.Height > screenRect.Width )
                    return true;
                else
                    return false;
            }
        }
   
        /// <summary>
        /// Event handler for the system's DisplaySettingsChanged event.
        /// Detect and then compare the height and width of the screen.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void displaySettingsChanged(object sender, EventArgs e)
        {
            this.PerformDynamicLayout();
        }

        /// <summary>
        /// Form resizing event handler. Scale the pbRTDoc picture box given the new
        /// size of the form and center it
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void PresentationCapabilityFMain_Resize(object sender, System.EventArgs e)
        {
            this.PerformDynamicLayout();
        }

        /// <summary>
        /// Perform a dynamic positioning/sizing of the UI (slide picture box, 
        /// thumbnails list view and toolbar)
        /// </summary>
        private void PerformDynamicLayout()
        {
            /* Algo:
             * 
             * Set form MinSize & resize form if necessary (does this happen automatically?  that could cause issues.)
             * 
             * if portraitMode
             *   Position thumbnailsView horiz.
             * else
             *   Position thumbnailsView vert.
             * 
             * Position toolbar (ie. pnlUI)
             * 
             * Scale & Position pbRTDoc (note thumbnailsView can be horiz. on vert.)
             */

            // Sanity checks  (these return rather than except, becuase if Form.Resize 
            //   is called before Form.Load, this actually happens.
            if( pbRatio == double.NaN )
                return;
            if( inkOverlay == null )
                return;
            // Sanity checks to avoid performing dynamic layout on minimize
            if(this.WindowState == FormWindowState.Minimized)
                return;

            // in Slide Show mode the slides takes the maximum of 
            // screen real estate
            if (displayMode == displayType.SlideShow)
                return;

            #region Set Form MinimumSize
            // Start with the extra size needed for the form border, etc.
            Size minSize = this.Size - this.ClientSize;

            if( PortraitMode )
            {
                // Compute the minimum Size allowed for the form in this mode
                minSize.Width += pnlUI.Width + 2 * constLayoutBuffer;
                minSize.Height += pnlUI.Height + constLayoutBuffer + (int)(pbRatio*constPbWidthMin)
                    + constLayoutBuffer + constThumbnailsPortraitHeight + constLayoutBuffer + statusBar.Height;
            }
            else // Landscape mode
            {
                // Compute the minimum Size allowed for the form in this mode
                minSize.Width += constLayoutBuffer + pnlUI.Width + constLayoutBuffer;
                minSize.Height += pnlUI.Height + constLayoutBuffer + (int)(pbRatio*constPbWidthMin)
                    + constLayoutBuffer + statusBar.Height;
            }
            this.MinimumSize = minSize;
            #endregion

            #region Position ThumbnailsListView (aka thumbnailsView)
            if( PortraitMode )
            {
                this.thumbnailsView.Location = new System.Drawing.Point( constLayoutBuffer,
                    ClientSize.Height - statusBar.Height - constLayoutBuffer - constThumbnailsPortraitHeight );
                this.thumbnailsView.Size = new System.Drawing.Size( ClientSize.Width - 2*constLayoutBuffer,
                    constThumbnailsPortraitHeight );
            }
            else // Landscape mode
            {
                this.thumbnailsView.Location = new System.Drawing.Point( constLayoutBuffer, 
                    pnlUI.Bottom + constLayoutBuffer );
                this.thumbnailsView.Size = new System.Drawing.Size( constThumbnailsLandscapeWidth, 
                    ClientSize.Height - thumbnailsView.Top - constLayoutBuffer - statusBar.Height );
            }
            // this line must come after changing the size, else they end up in weird places
            thumbnailsView.Horizontal = this.PortraitMode;
            #endregion

            ScaleAndPositionPbImage();
        }

        /// <summary>
        /// Scale and position the slides picture box (pbRTDoc) taking in
        /// account the current form size, margins, and the fact
        /// that we want to preserve the existing pbRTDoc Y/X ratio.
        /// </summary>
        private void ScaleAndPositionPbImage()
        {
            // Pri1: This method has a lot of isolated if statements
            // check if you can combine them or have else if statements
            // and clean this code

            Size pbNewSize = Size.Empty;
            Size availableSpace = Size.Empty;

            // Get the size of the available space for the pbRTDoc
            if (displayMode == displayType.SlideShow)
            {
                availableSpace.Width = ClientSize.Width;
                availableSpace.Height = ClientSize.Height;

                // Use all the screen area for displaying the slide
                pbRTDoc.Left = 0;
                pbRTDoc.Top = 0;
            }
            else
            {
                if( this.PortraitMode )
                {
                    availableSpace.Width = ClientSize.Width - 2*constLayoutBuffer;
                    availableSpace.Height = thumbnailsView.Top - constLayoutBuffer - pnlUI.Bottom - constLayoutBuffer;
                }
                else // Landscape mode
                {
                    availableSpace.Width = ClientSize.Width - 2*constLayoutBuffer - thumbnailsView.Right;
                    availableSpace.Height = ClientSize.Height - constLayoutBuffer - pnlUI.Bottom - constLayoutBuffer - statusBar.Height;
                }

                // Center the pbRTDoc
                pbRTDoc.Left = (PortraitMode) ? (constLayoutBuffer) : (thumbnailsView.Right + constLayoutBuffer);
                pbRTDoc.Top = pnlUI.Bottom + constLayoutBuffer;
            }

            if( pbRatio*availableSpace.Width >= availableSpace.Height )
                // Width is oversize, resize by height
            {
                pbNewSize.Height = availableSpace.Height;
                pbNewSize.Width =  (int)(availableSpace.Height / pbRatio);
            }
            else
                // Height is oversize, resize by width
            {
                pbNewSize.Width = availableSpace.Width;
                pbNewSize.Height = (int)(availableSpace.Width * pbRatio);
            }

            // Check for size too small
            if( pbNewSize.Width < constPbWidthMin )
            {
                pbNewSize.Width = constPbWidthMin;
                pbNewSize.Height = (int)(constPbWidthMin * pbRatio);
            }

            // Assign the local value to the object
            pbRTDoc.Size = pbNewSize;

            // Display the new ratio that might be slighly different due to rounding
            double newRatio = (double)pbRTDoc.Height / (double)pbRTDoc.Width;
            Log(string.Format(CultureInfo.CurrentCulture, "New ratio for pbRTDoc = {0}", 
                newRatio.ToString(CultureInfo.InvariantCulture)));

            if( prevPbSize != Size.Empty ) // this eliminates the startup case
            {
                // Scale all strokes on the current screen
                float scaleX = (float)pbRTDoc.Size.Width/(float)prevPbSize.Width;
                float scaleY = (float)pbRTDoc.Size.Height/(float)prevPbSize.Height;
                inkOverlay.Ink.Strokes.Scale(scaleX, scaleY);
            }

            // Pri1: Combine this code with the other 
            // if (displayMode == displayType.SlideShow) above at the 
            // begining ScaleAndPositionPbImage()
            if (displayMode == displayType.SlideShow)
            {
                // Use all the screen area for displaying the slide
                pbRTDoc.Left = (availableSpace.Width - pbRTDoc.Width)/2;
                pbRTDoc.Top = (availableSpace.Height - pbRTDoc.Height)/2;
            }

            // Save sizes for the next scaling procedure
            prevPbSize = pbRTDoc.Size;
        }

        #endregion

        #region Common Methods for ToolBar/Menu/Keyboard 

        // Pri2: Investigate if there is a simpler way to show and hide the menu
        // without having to hide all the top menus
        /// <summary>
        /// Display or hide the menu
        /// </summary>
        /// <param name="Visibility">The value false will hise the menu, 
        /// the value true will show the menu</param>
        private void menuDisplay(bool Visibility)
        {
            this.miFile.Visible = this.miTools.Visible = 
                this.miSlide.Visible = this.miHelp.Visible = Visibility;
        }

        /// <summary>
        /// Switch to the slide show mode.
        /// </summary>
        /// <remarks>
        /// In slide show mode, the slide is display
        /// on the full screen and UI is hided
        /// </remarks>
        private void switchToSlideShow()
        {
            displayMode = displayType.SlideShow;

            this.pnlUI.Visible = false;
            this.statusBar.Visible = false;
            this.thumbnailsView.Visible = false;

            menuDisplay(false);

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            // Changed the background color to black in order to
            // avoid having a gray border in case the slide does not
            // over the entier screen
            this.BackColor = Color.Black;
            this.ScaleAndPositionPbImage();
        } 

        /// <summary>
        /// Switch to the collaboration mode.
        /// </summary>
        private void switchToCollaboration()
        {
            displayMode = displayType.Collaboration;

            this.pnlUI.Visible = true;
            this.statusBar.Visible = true;
            this.thumbnailsView.Visible = true;

            menuDisplay(true);

            this.BackColor = System.Drawing.SystemColors.Control;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Normal;
            this.PerformDynamicLayout();
            this.ScaleAndPositionPbImage();
        }

        #endregion Common Methods for ToolBar/Menu/Keyboard

        #region Menu Event Handlers

        /// <summary>
        /// Allow to set the InkOverlay accordingly to the mode (Pen or 
        /// Highlighter) with the color passed in parameter. 
        /// </summary>
        /// <param name="mode">The mode: InkToolBarMode.Pen or InkToolBarMode.Highlighter</param>
        /// <param name="color">The pen or highlighter color</param>
        private void setInkFromMenu(InkToolBarMode mode, Color color)
        {
            // Ensure we are in Ink mode
            inkOverlay.EditingMode = InkOverlayEditingMode.Ink;

            if (mode == InkToolBarMode.Pen)
            {
                // Get the current pen attribute (size of pen / shape)
                inkOverlay.DefaultDrawingAttributes = inkToolBar.PenAttributes;

                // Push the button and sync the color in the InkToolBar control

                // Set the new color
                inkOverlay.DefaultDrawingAttributes.Color = color;

                // Update the control attributes and push the pen button
                inkToolBar.PenAttributes = inkOverlay.DefaultDrawingAttributes;
            } 
            else if (mode == InkToolBarMode.Highlighter)
            {
                // Get the current highlighter attributes (size of highlighter / shape / transparency)
                inkOverlay.DefaultDrawingAttributes = inkToolBar.HighlighterAttributes;

                // Push the button and sync the color in the InkToolBar control

                // Set the new color
                inkOverlay.DefaultDrawingAttributes.Color = color;

                // Update the control attributes and push the pen button
                inkToolBar.HighlighterAttributes = inkOverlay.DefaultDrawingAttributes;
            }
        }

        /// <summary>
        /// Menu item miOpen_Click event handler. 
        /// Opens a .ppt or .rtd document or resend it. It also allow
        /// clients to open a .rtd file
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miOpen_Click(object sender, System.EventArgs e)
        {
            // Open File
            openFile();
        }


        // Pri1: miResendCurrentSlide_Click might be remove before we ship 2.4.1
        /// <summary>
        /// Menu item miResendCurrentSlide_Click event handler. Resends a slide.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miResendCurrentSlide_Click(object sender, System.EventArgs e)
        {
            ResendCurrentSlide();
        }

        private void ResendCurrentSlide()
        {
            EnableSlideSend(false);
            statusBar.StatusMessage = Strings.ResendingCurrentSlide;

            Page currentPage = (Page)rtDocument.Organization.TableOfContents[index].Resource;
            this.presentationCapability.BeginSendObjectBackground(currentPage, new AsyncCallback(CurrentSlideSent), null);
        }

        /// <summary>
        /// Menu item miSlideShow_Click event handler. 
        /// Switchs to slideshow mode
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miSlideShow_Click(object sender, System.EventArgs e)
        {
            if (displayMode == displayType.Collaboration)
            {  
                switchToSlideShow();
            }
        }

        /// <summary>
        /// Menu item miClose_Click event handler. Closes all the capabilities
        /// if we are on the initiator capability, else close the
        /// just the local capability.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miClose_Click(object sender, System.EventArgs e)
        {
            // Note: The close will fire a closing event that
            // will be handled in PresentationCapabilityFMain_Closing
            // were to StopSending and StopPlaying code is
            this.Close();
        }

        /// <summary>
        /// Menu item miPreviousSlide_Click event handler. Previous slide is clicked from the menu.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miPreviousSlide_Click(object sender, System.EventArgs e)
        {
            // Navigate backward
            navigateBackward();
        }

        /// <summary>
        /// Menu item miNextSlide_Click event handler. Next slide is clicked from the menu.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miNextSlide_Click(object sender, System.EventArgs e)
        {
            // Navigate forward
            navigateForward();
        }

        /// <summary>
        /// Menu item miTools_Popup event handler. This event is fired
        /// when the Tools menu appears. This is used to check the
        /// right item before the menu item appears.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miTools_Popup(object sender, System.EventArgs e)
        {
            // Uncheck all
            foreach (MenuItem item in this.miTools.MenuItems)
            {
                item.Checked = false;
            }

            string color = inkOverlay.DefaultDrawingAttributes.Color.Name;

            // Pri3: Find a more extensible/flexible way to do that 
            // (for instance using enum for color in the menu, and use index)
            // Set the pen color to the selected color

            // Check the item is currently selected
            if (mode == InkToolBarMode.Pen)
            {
                switch(color)       
                {         
                    case "Blue":   
                        miBlueInk.Checked = true;
                        break;                  
                    case "Black":            
                        miBlackInk.Checked = true;
                        break;           
                    case "Red":            
                        miRedInk.Checked = true;
                        break;         
                    case "Yellow":            
                        miYellowInk.Checked = true;
                        break;           
                    default:
                        throw new Exception(string.Format(CultureInfo.CurrentCulture, Strings.UnexpectedPenColor, 
                            color.ToString()));
                }
            }
            else if (mode == InkToolBarMode.Highlighter)
            {
                switch(color)       
                {         
                    case "Yellow":   
                        miYellowHighlighter.Checked = true;
                        break;                  
                    case "Lime":            
                        miLimeHighlighter.Checked = true;
                        break;           
                    case "Blue":            
                        miBlueHighlighter.Checked = true;
                        break;              
                    default:
                        throw new Exception(string.Format(CultureInfo.CurrentCulture, Strings.UnexpectedHighlighterColor, 
                            color.ToString()));
                }
            } 
            else if (mode == InkToolBarMode.Eraser)
            {
                miErase.Checked = true;
            }
        }

        /// <summary>
        /// Menu item miBlueInk_Click event handler. Blue Ink pen is clicked from the menu.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miBlueInk_Click(object sender, System.EventArgs e)
        {
            setInkFromMenu(InkToolBarMode.Pen, Color.Blue);
        }

        /// <summary>
        /// Menu item miBlackInk_Click event handler. Black Ink pen is clicked from the menu.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miBlackInk_Click(object sender, System.EventArgs e)
        {
            setInkFromMenu(InkToolBarMode.Pen, Color.Black);
        }

        /// <summary>
        /// Menu item miRedInk_Click event handler. Red Ink pen is clicked from the menu.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miRedInk_Click(object sender, System.EventArgs e)
        {
            setInkFromMenu(InkToolBarMode.Pen, Color.Red);
        }

        /// <summary>
        /// Menu item miYellowInk_Click event handler. Yellow Ink pen is clicked from the menu.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miYellowInk_Click(object sender, System.EventArgs e)
        {
            setInkFromMenu(InkToolBarMode.Pen, Color.Yellow);
        }

        /// <summary>
        /// Menu item miYellowHighlighter_Click event handler. Yellow Highlighter is clicked from the menu.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miYellowHighlighter_Click(object sender, System.EventArgs e)
        {
            setInkFromMenu(InkToolBarMode.Highlighter, Color.Yellow);
        }

        /// <summary>
        /// Menu item miLimeHighlighter_Click event handler. Lime Highlighter is clicked from the menu.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miLimeHighlighter_Click(object sender, System.EventArgs e)
        {
            setInkFromMenu(InkToolBarMode.Highlighter, Color.Lime);
        }

        /// <summary>
        /// Menu item miBlueHighlighter_Click event handler. Blue Highlighter is clicked from the menu.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miBlueHighlighter_Click(object sender, System.EventArgs e)
        {
            setInkFromMenu(InkToolBarMode.Highlighter, Color.Blue);
        }

        /// <summary>
        /// Menu item miErase_Click event handler. Erase is clicked from the menu.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miErase_Click(object sender, System.EventArgs e)
        {   
            // Switch the mode to eraser
            inkToolBar.SwitchToEraserMode();
        }

        /// <summary>
        /// Menu item miEraseAllInk_Click event handler. Erase All Ink is clicked from the menu.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miEraseAllInk_Click(object sender, System.EventArgs e)
        {
            // Remove all the strokes
            inkToolBar.DeleteAllStrokes();
            // Refresh the display
            this.Refresh();
            // Send a erase all ink message for the others
            rtDocumentHelper.SendEraseAllInk();
        }

        /// <summary>
        /// Menu item miBlankSlide_Click event handler. Insert a blank slide (whiteboard)
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miBlankSlide_Click(object sender, System.EventArgs e)
        {
            InsertWhiteboard();
        }

        /// <summary>
        /// Menu item miSlide_Popup event handler. This event occurs as soon as the
        /// slide menu list pops up. Populates the list of applications 
        /// for the snapshot.
        /// </summary>
        /// <remarks>
        /// Populating the list of applications for the snapshot has to be done
        /// here because we want to see an arrow that shows an extended
        /// menu from the Select Snapshot menu.
        /// </remarks>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miSlide_Popup(object sender, System.EventArgs e)
        {
            // Populate the snapshot list
            miSnapshot.MenuItems.Clear();
            windowsInMenu.Clear();

            // Add the one item by active application
            foreach (Win32Util.Win32Window window in Win32Util.Win32Window.ApplicationWindows)
            {
                // Note: we hook the menu item to the event handler: OnctxtMenuSnapshotAppsClick
                MenuItem mi = new MenuItem(window.Text, new EventHandler(OnctxtMenuSnapshotAppsClick));
                miSnapshot.MenuItems.Add(mi);
                windowsInMenu.Add(mi, window);
                if (applicationWindow != null)
                {
                    // Select the item corresponding to applicationWindow
                    if ( window.ThreadId == applicationWindow.ThreadId 
                        && window.Text == applicationWindow.Text )
                    {
                        // Add a check mark for this item
                        mi.Checked = true;
                    }
                }
            }
        }

        /// <summary>
        /// Menu item miInsertSnapshot_Click event handler. This event allows user
        /// to take a snapshot from the menu.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miInsertSnapshot_Click(object sender, System.EventArgs e)
        {
            if (isAppSelected())
            {
                // Take a snapshot
                window = applicationWindow;
                InsertSnapshot();
            } 
            else
            {
                RtlAwareMessageBox.Show(this, Strings.YouMustFirstSelectAScreenShot, Strings.ScreenShotInformation, 
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 
                    (MessageBoxOptions)0);
            }
        }

        /// <summary>
        /// Menu item miPresentationHelp_Click event handler.
        /// </summary>
        /// <remarks>
        /// The help is online on the ConferenceXP Web site
        /// </remarks>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miPresentationHelp_Click(object sender, System.EventArgs e)
        {
            // Open the help url in a browser 
            Process.Start(helpurlPresentation);
        }

        /// <summary>
        /// Menu item miHelpAbout_Click event handler.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miHelpAbout_Click(object sender, System.EventArgs e)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);

            RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, Strings.ForMoreInformationSee, 
                fvi.FileVersion, Conference.About), Strings.ConferencexpPresentation, MessageBoxButtons.OK, 
                MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
        }

        private void miSaveAsImages_Click(object sender, EventArgs e) {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = System.Environment.SpecialFolder.Desktop;
            if (fbd.ShowDialog() == DialogResult.OK) {
                //Confirm possible overwrite, if there are png files in the selected path
                if (Directory.Exists(fbd.SelectedPath)) {
                    string[] pngfiles = Directory.GetFiles(fbd.SelectedPath, "*.png");
                    if (pngfiles.Length > 0) {
                        DialogResult dr = RtlAwareMessageBox.Show(this, Strings.ConfirmImageOverwriteMessage,
                            Strings.ConfirmImageOverwriteCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2,
                            (MessageBoxOptions)0);
                        if (dr != DialogResult.Yes) {
                            return;
                        }
                    }
                }
                //Save all RTDoc pages to images
                saveRTDocument(rtDocument, fbd.SelectedPath);
            }
        }    
      
        #endregion  Menu Event Handlers

        #region ToolBar Event Handlers

        /// <summary>
        /// tbFile_ButtonClick event handler. This allows initiator to 
        /// open a .ppt or .rtd document or resend it. It also allow
        /// clients to open a .rtd file
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void tbFile_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            if (e.Button == tbbOpenFile)
            {
                // Open File
                openFile();
            }
            else if (e.Button == tbbResend)
            {
                ResendCurrentSlide();
            }
        }

        /// <summary>
        /// inkToolBar_ModeChanged event handler. This event is fired
        /// when the user clicks changed mode: Pen, Highlighter, Eraser.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void inkToolBar_ModeChanged(object sender, System.EventArgs e)
        {
            // update the mode
            mode = inkToolBar.Mode;
        }

        /// <summary>
        /// inkToolBar_EraseAllClicked event handler. This event is fired
        /// when the user clicks the EraseAll button
        /// </summary>
        private void inkToolBar_EraseAllClicked(object sender, System.EventArgs e)
        {
            // Refresh the display
            // Note: The remove strokes code is insde the InkTollBar ctrl
            this.Refresh();
            // Send a erase all ink message for the others
            rtDocumentHelper.SendEraseAllInk();
        }

        /// <summary>
        /// tbNavigate_ButtonClick event handler. Allow users to navigate
        /// to the Previous/Next slide.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void tbNavigate_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            if (e.Button == tbbPrevious)
            {
                // Navigate backward
                navigateBackward();
            }
            else if (e.Button == tbbNext)
            {
                // Navigate forward
                navigateForward();
            }
        }

        #endregion ToolBar Event Handlers

        #region ThumbnailsView Event Handlers

        /// <summary>
        /// thumbnailsView_SelectedImageChanged event handler. Previous slide is clicked from the menu.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void thumbnailsView_SelectedImageChanged(object sender, System.EventArgs e)
        {
            if( thumbnailsView.SelectedIndex != -1 )
            {
                this.navigateToIndex( thumbnailsView.SelectedIndex, true );
            }
        }

        #endregion ThumbnailsView Event Handlers

        #region Keyboard Event Handlers (Keyboard shortcuts related)

        /// <summary>
        /// PresentationCapabilityFMain_KeyDown event handler. This is used
        /// to handle page navigation keyboard shortcuts.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void PresentationCapabilityFMain_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // Page navigation Keyboard Shortcuts handling
            if ((e.KeyCode == Keys.N) ||
                (e.KeyCode == Keys.PageDown) ||
                (e.KeyCode == Keys.Right) ||
                (e.KeyCode == Keys.Down) ||
                (e.KeyCode == Keys.Space) ||  
                (e.KeyCode == Keys.Enter))
            {
                // Navigate forward
                navigateForward();
            } 
            else 
                if ((e.KeyCode == Keys.P) || 
                (e.KeyCode == Keys.PageUp) ||
                (e.KeyCode == Keys.Left) ||
                (e.KeyCode == Keys.Up) ||
                (e.KeyCode == Keys.Back))
            {
                // Navigate backward
                navigateBackward();
            } 
            else
                if ((e.KeyCode == Keys.F5) && (displayMode == displayType.Collaboration))
            {
                // Switch to Slide View
                this.switchToSlideShow();
                this.PerformDynamicLayout();
            }
            else
                if ((e.KeyCode == Keys.Escape) && (displayMode == displayType.SlideShow))
            {
                this.switchToCollaboration();
            } 
        }

        #endregion Keyboard Event Handlers (Keyboard shortcuts related)

        #region Mouse Event Handler (Mouse shortcuts related)

        /// <summary>
        /// pbRTDoc_MouseDown event handler. This is used to handle mouse event
        /// in SlideShow mode. In fact, the right click is used to 
        /// go from the slide show mode to the collaborative mode. 
        /// Right click also correspond to the button on the tablet PC pen (default
        /// tablet PC settings).
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void pbRTDoc_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Right) && (displayMode == displayType.SlideShow))
            {
                this.switchToCollaboration(); 
            }
        }

        #endregion Mouse Event Handler (Mouse shortcuts related)

        #region Slide Preview Event Handlers

        /// <summary>
        /// thumbnailsView_ImageMouseEnter event handler. This is used
        /// to display the picture box containing the slide preview when a slide
        /// in the thumbnail list view is hovered.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void thumbnailsView_ImageMouseEnter(object sender, EventArgs e)
        {
            // Set the page in the preview picture box
            Guid tocID = thumbnailsView.Items[ thumbnailsView.HoverIndex ].tag;
            Image previewImage = ((Page)rtDocument.Organization.TableOfContents[tocID].Resource).Image;

            if ((thumbnailsView.SelectedIndex != -1) && (previewImage != null))
            {
                // Reset the size of hte picture box (the shadow box size get set below)
                this.pbSlidePreview.Size = new System.Drawing.Size(260, 195);

                int previewXPos = 0;
                int previewYPos = 0;

                // Compute the location of the preview picture box
                if( PortraitMode )
                {
                    // The preview picture box will be placed on the 
                    // top of the thumbnail list view
                    previewXPos = ((PictureBox)thumbnailsView.picBoxes[thumbnailsView.HoverIndex]).Location.X;
                    previewYPos =  thumbnailsView.Location.Y - pbSlidePreview.Height;

                    // In portrait mode, the ThumbnailsListView is horizontal, so we have to ensure that:
                    // the preview picture box is not truncated horizontally on the right side
                    if ((previewXPos + pbSlidePreview.Width + constLayoutBuffer) > (this.ClientSize.Width))
                    {
                        previewXPos = this.ClientSize.Width - pbSlidePreview.Width - constLayoutBuffer;
                    } 
                        // ditto on the left side
                    else if ((previewXPos - constLayoutBuffer) < 0)
                    {
                        previewXPos = constLayoutBuffer;
                    } 
                } 
                else // Landscape Mode
                {
                    // The preview picture box will be placed on the 
                    // left side of the thumbnail list view
                    previewXPos = thumbnailsView.Location.X + thumbnailsView.Size.Width - constPosAdjustPreview;
                    previewYPos = ((PictureBox)thumbnailsView.picBoxes[thumbnailsView.HoverIndex]).Location.Y;
                   
                    // In landscape mode, the ThumbnailsListView is vertical, so we have to ensure that:
                    // the preview picture box is not truncated vertically at the bottom
                    if ((previewYPos + pbSlidePreview.Height + constLayoutBuffer) > this.ClientSize.Height)
                    {
                        previewYPos = this.ClientSize.Height - pbSlidePreview.Height - constLayoutBuffer;
                    }
                        // ditto at the top
                    else if ((previewYPos - constLayoutBuffer) < 0)
                    {
                        previewYPos = constLayoutBuffer;
                    } 
                }

                // Position the Slide Preview picture box
                this.pbSlidePreview.Location = 
                    new System.Drawing.Point(
                    previewXPos,
                    previewYPos);
                // ... and the shadow picture box
                this.pbShadowSlidePreview.Location = 
                    new System.Drawing.Point(
                    previewXPos+2,
                    previewYPos+2);

                // Try to maintain the aspect ratio of the image
                SizeF drawSize = (SizeF)pbSlidePreview.Size;
                if( drawSize.Width / (float)previewImage.Width > drawSize.Height / (float)previewImage.Height )
                {
                    // this means there's free-space on the sides
                    drawSize.Width = (float)previewImage.Width * (drawSize.Height / (float)previewImage.Height);
                }
                else
                {
                    // there may be some free-space on the top & bottom
                    drawSize.Height = (float)previewImage.Height * (drawSize.Width / (float)previewImage.Width);
                }

                pbSlidePreview.Size = new Size((int)drawSize.Width, (int)drawSize.Height);
                pbShadowSlidePreview.Size = pbSlidePreview.Size;

                // Display the picture box
                pbSlidePreview.Image = previewImage;
                pbSlidePreview.Visible = true;
                pbShadowSlidePreview.Visible = true;
            }
        }

        /// <summary>
        /// thumbnailsView_ImageMouseLeave event handler. This is used
        /// to hide the picture box containing the slide preview when the
        /// mouse is going out of the current slide.
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void thumbnailsView_ImageMouseLeave(object sender, EventArgs e)
        {
            pbSlidePreview.Visible = false;
            pbShadowSlidePreview.Visible = false;
        }

        #endregion Slide Preview Event Handlers

        #region Remote open slides

        // Pri2: All the code in this region is temporary

        // The code in this region allows to request remote capability to open a 
        // rtd file located in the same place than the one open by the initiator

        // The Open Remote menu has to be explicitely activated using the 
        // application configuration key
        // MSR.LST.ConferenceXP.Capability.Presentation.OpenRemote

        /// <summary>
        /// Menu item miOpenRemote_Click event handler. Open a remote rtd file.
        /// When you open a file using this menu, the exact same rtd file has to be
        /// at at the same location on every remote machine 
        /// </summary>
        /// <param name="sender">The event sender object</param>
        /// <param name="e">The event arguments</param>
        private void miOpenRemote_Click(object sender, System.EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog cdlg = new OpenFileDialog();
            cdlg.Filter = "RTDocument files (*.rtd)|*.rtd" ;
            DialogResult dr = cdlg.ShowDialog();

            try
            {
                if (dr == DialogResult.OK)
                {
                    string fileName = cdlg.FileName;

                    if (System.IO.Path.GetExtension(fileName).ToLower(CultureInfo.InvariantCulture) == ".rtd")
                    {
                        // Open a RTD file
                        lock(this)
                        {
                            rtDocument = LoadRtdFile(fileName);
                        }
                        if (rtDocument == null)
                        {
                            return;
                        }
                    } 
                    else
                    {
                        RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, 
                            Strings.ErrorInOpeningFileType, fileName), string.Empty, MessageBoxButtons.OK, 
                            MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                        return;
                    }

                    Log(string.Format(CultureInfo.CurrentCulture, "rtDocID = {0}", 
                        rtDocument.Identifier.ToString()));

                    // Update the title bar of the form 
                    // with the document name followed by the capability name
                    this.Text = rtDocument.Metadata.Title + " - " + this.presentationCapability.Name;

                    // Clean up the screen
                    DeleteAllStrokes();

                    Log("On open doc: info before creating RTDocHelper,");
                    DisplayRTDocumentOnlyStructureInfo();
                    rtDocumentHelper = new RTDocumentHelper(this.presentationCapability, rtDocument);

                    Log("Structure Info on open after creating RTDocHelper,");
                    DisplayRTDocumentStructureInfo();

                    #region Populate thumbnails
                    ArrayList thumbnails = new ArrayList();

                    using (Bitmap bp = new Bitmap( thumbnailsView.ImageSize.Width, thumbnailsView.ImageSize.Height, 
                               System.Drawing.Imaging.PixelFormat.Format24bppRgb))
                    {
                        using (Graphics g = Graphics.FromImage(bp))
                        {
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                            foreach( TOCNode tn in rtDocument.Organization.TableOfContents )
                            {
                                Page p = (Page)tn.Resource;

                                ThumbnailListView.CreateSlideThumbnail( thumbnailsView.ImageSize,
                                    p.Image, g, tn.Title );

                                ThumbnailListViewItem item = new ThumbnailListViewItem();
                                item.thumbnail = ((Bitmap)bp.Clone());
                                item.tag = tn.Identifier;
                                thumbnails.Add(item);
                            }
                        }
                    }

                    thumbnailsView.Items = (ThumbnailListViewItem[])thumbnails.ToArray(typeof(ThumbnailListViewItem));
                    #endregion

                    // Initialize the index on the first slide
                    index = 0;

                    // Show the current page
                    statusBar.SetMaxPage( 1, rtDocument.Organization.TableOfContents.Count );
                    ShowPage(rtDocument.Organization.TableOfContents[index].Identifier);

                    // Send a message to request the remote users to open the rtd file
                    // fileName
                    SendRemoteFileOpen(fileName);

                    // Update the Backward/Forward buttons (push the navigation button in the 
                    // direction(s)that you cannot move anymore)
                    UpdateNavigationButtonState();

                    // Display the current page on the sender and viewer
                    UpdateCurrentPage();
                }
            }
            catch(Exception ex) 
            {
                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, Strings.ErrorInOpening, 
                    cdlg.FileName, ex.Message.ToString()), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                return;
            }
        }

        /// <summary>
        /// Send a message to request the remote users to open the rtd file fileName
        /// </summary>
        /// <param name="filename">filename (path + filename)</param>
        private void SendRemoteFileOpen(string filename)
        {
            presentationCapability.SendObject(
                new RTFrame(new Guid(constOpenRemoteGuid), filename));
        }

        /// <summary>
        /// Open a file in local rtd file name.
        /// </summary>
        /// <param name="fileName">filename (path + filename)</param>
        // Pri3: There is a lot of duplication of code between openLocalFile
        // and miOpenRemote_Click method that could be written just once
        // If this code is going to stay in long term we should change that
        private void openLocalFile(string fileName)
        {
            try
            {
                // Open a RTD file
                lock(this)
                {
                    rtDocument = LoadRtdFile(fileName);
                }
                if (rtDocument == null)
                {
                    return;
                }

                // Update the title bar of the form 
                // with the document name followed by the capability name
                this.Text = rtDocument.Metadata.Title + " - " + this.presentationCapability.Name;

                DisplayRTDocumentOnlyStructureInfo();
                rtDocumentHelper = new RTDocumentHelper(this.presentationCapability, rtDocument);

                DisplayRTDocumentStructureInfo();

                #region Populate thumbnails
                ArrayList thumbnails = new ArrayList();

                using (Bitmap bp = new Bitmap( thumbnailsView.ImageSize.Width, thumbnailsView.ImageSize.Height, 
                           System.Drawing.Imaging.PixelFormat.Format24bppRgb))
                {
                    using (Graphics g = Graphics.FromImage(bp))
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                        foreach( TOCNode tn in rtDocument.Organization.TableOfContents )
                        {
                            Page p = (Page)tn.Resource;

                            ThumbnailListView.CreateSlideThumbnail( thumbnailsView.ImageSize,
                                p.Image, g, tn.Title );

                            ThumbnailListViewItem item = new ThumbnailListViewItem();
                            item.thumbnail = ((Bitmap)bp.Clone());
                            item.tag = tn.Identifier;
                            thumbnails.Add(item);
                        }
                    }
                }

                thumbnailsView.Items = (ThumbnailListViewItem[])thumbnails.ToArray(typeof(ThumbnailListViewItem));
                #endregion

                // Initialize the index on the first slide
                index = 0;

                // Show the current page
                statusBar.SetMaxPage( 1, rtDocument.Organization.TableOfContents.Count );
                ShowPage(rtDocument.Organization.TableOfContents[index].Identifier);

                // Display the current page on the sender and viewer
                UpdateCurrentPage();
                UpdateNavigationButtonState();
            }
            catch(Exception ex) 
            {
                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, Strings.ErrorInOpening, 
                    fileName,ex.Message.ToString()), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                return;
            }
        }

        /// <summary>
        /// Read in from Rtd files.
        /// </summary>
        /// <param name="fileName">filename (path + filename) to read</param>
        /// <returns>RTDocument containing the file read</returns>
        public RTDocument LoadRtdFile(string fileName) 
        {
            BinaryFormatter myBinaryFormat = new BinaryFormatter();
            try 
            {
                FileStream myInputStream = System.IO.File.OpenRead(fileName);
                RTDocument rtDocument = (RTDocument) myBinaryFormat.Deserialize(myInputStream);
                return rtDocument;
            }
            catch (Exception)
            {
                RtlAwareMessageBox.Show(this, string.Format(CultureInfo.CurrentCulture, Strings.ErrorInOpeningVersion, 
                    fileName), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                return null;
            }
        }

        #endregion Remote open slides

        #region Save As Images

        /// <summary>
        /// Save all pages in the RTDocument to PNG files at the specified path.  Existing files will be overwritten.
        /// </summary>
        /// <param name="rtd"></param>
        /// <param name="path"></param>
        private void saveRTDocument(RTDocument rtd, string path) {
            if (rtd == null) {
                Debug.WriteLine("RTDocument is null");
                return;
            }

            if (rtd.Organization.TableOfContents.Length <= 0) {
                Debug.WriteLine("RTDocument contains zero pages");
                return;
            }

            //Make sure ink on the current page is stored before we proceed
            if ((inkOverlay != null) && (inkOverlay.Ink.Strokes.Count > 0)) {
                StoreInk(pageShowing);
            }

            //Create directory.  Does nothing if the directory exists.
            Directory.CreateDirectory(path);

            int pagecount = rtd.Organization.TableOfContents.Length;
            int i = 1;
            foreach (TOCNode n in rtd.Organization.TableOfContents) {
                Page p = rtd.Resources.Pages[n.ResourceIdentifier];
                //Make a file name for the current image.
                String pngfile = Path.Combine(path, "page" + i.ToString() + ".png");

                Image img = null;
                if ((p != null) && (p.Image != null)) {
                    //Slide image or screen shot: Scale to size with maximum dimension to 960.
                    int w = 960;
                    int h = 960;
                    double ar = ((double)p.Image.Width) / ((double)p.Image.Height);
                    if (ar >= 1.0) { 
                        //width is the larger dimension.
                        h = (int)Math.Round(960.0 / ar);
                    }
                    else { 
                        //height is the larger dimension.
                        w = (int)Math.Round(960.0 * ar);
                    }
                    img = new Bitmap(p.Image, new Size(w,h));
                }
                else {
                    //Whiteboard: make a white 4x3 white image
                    img = new Bitmap(960,720);
                    Graphics g = Graphics.FromImage(img);
                    SolidBrush whiteBrush = new SolidBrush(Color.White);
                    g.FillRectangle(whiteBrush, 0, 0, img.Width, img.Height);
                    whiteBrush.Dispose();
                    g.Dispose();
                }                

                //Add ink and save
                AddInkOverlay(img, p.Identifier);
                img.Save(pngfile, ImageFormat.Png);
                img.Dispose();
                i++;
            }
        }

        /// <summary>
        /// Draw ink strokes on top of the given image. Using DibGraphicsBuffer preserves 
        /// ink transparency and anti-aliasing.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="pageIdentifier"></param>
        private void AddInkOverlay(Image img, Guid pageIdentifier) {
            ScaledInk scaledInk = (ScaledInk)hashtablePageStrokes[pageIdentifier.ToString()];
            if ((scaledInk != null) && (scaledInk.PageInk.Strokes.Count > 0)) {
                //Draw the slide data on a temporary graphics object in a temporary form
                System.Windows.Forms.Form tempForm = new System.Windows.Forms.Form();
                Graphics screenGraphics = tempForm.CreateGraphics();
                DibGraphicsBuffer dib = new DibGraphicsBuffer();
                Graphics tempGraphics = dib.RequestBuffer(screenGraphics, img.Width, img.Height);
                
                //Draw the existing image
                tempGraphics.DrawImage(img, 0, 0);

                //Scale the ink relative to the image
                Microsoft.Ink.Renderer renderer = new Microsoft.Ink.Renderer();
                Matrix transformation = new Matrix();
                renderer.GetViewTransform(ref transformation);
                transformation.Scale(((float)img.Width / scaledInk.PageWidth), 
                    ((float)img.Height / scaledInk.PageHeight));
                renderer.SetViewTransform(transformation);

                //Draw the ink
                renderer.Draw(tempGraphics, scaledInk.PageInk.Strokes);

                //Paint the results back to the original image
                Graphics toSave = Graphics.FromImage(img);
                dib.PaintBuffer(toSave, 0, 0);

                //Cleanup
                toSave.Dispose();
                tempGraphics.Dispose();
                dib.Dispose();
                screenGraphics.Dispose();
                tempForm.Dispose();
            }
        }

        #endregion Save As Images

        #region ICapabilityForm

        public override void AddCapability(ICapability capability)
        {
            base.AddCapability (capability);

            if(presentationCapability == null)
            {
                presentationCapability = (PresentationCapability)capability;
                presentationCapability.ObjectReceived += new CapabilityObjectReceivedEventHandler(ObjectReceived);

                // Init StatusBar
                statusBar.SetBusyStatusMessage(Strings.Loading);

                // Init ThumbnailsListView
                this.thumbnailsView.ImageSelected += new System.EventHandler(this.thumbnailsView_SelectedImageChanged);
                this.thumbnailsView.ImageMouseEnter += new EventHandler(thumbnailsView_ImageMouseEnter);
                this.thumbnailsView.ImageMouseLeave +=new EventHandler(thumbnailsView_ImageMouseLeave);
                Size pbSize = new Size(thumbnailsView.ClientRectangle.Width - 55, 0);
                pbSize.Height = (int)( ((float)pbSize.Width)*(3F/4F) );

                // Create blank RTDocument
                this.rtDocument = new RTDocument();
                rtDocument.Identifier = new Guid(constWhiteboardGuid);
                rtDocument.Metadata.Title = Strings.WhiteboardSession;
                rtDocument.Metadata.Creator = Conference.LocalParticipant.Name;
            
                // Add a blank page
                Page pg = new Page();
                pg.Identifier = new Guid(constWhiteboardGuid);
                TOCNode tn = new TOCNode();
                tn.Title = Strings.Whiteboard1;
                tn.Resource = pg;
                tn.ResourceIdentifier = pg.Identifier;
                tn.Identifier = new Guid(constWhiteboardGuid);
                rtDocument.Organization.TableOfContents.Add(tn);
                rtDocument.Resources.Pages.Add(pg.Identifier, pg);

                // Wrap the RTD with a helper & init vars
                rtDocumentHelper = new RTDocumentHelper( presentationCapability, rtDocument );
                rtDocumentHelper.CurrentOrganizationNodeIdentifier = tn.Identifier;
                statusBar.SetMaxPage( 1, rtDocument.Organization.TableOfContents.Count );
                thumbnailsView.InsertThumbnail(null, tn.Title, 0, tn.Identifier);
                thumbnailsView.SelectedIndex = 0;
                this.pageShowing = pg.Identifier;
            }
        }

        public override bool RemoveCapability(ICapability capability)
        {
            bool ret = base.RemoveCapability(capability);

            if(ret)
            {
                // Remove the ObjectReceived event handler.
                // This form is going away, but the Capability may be replayed in which case we'd receive this event into a disposed form!
                presentationCapability.ObjectReceived -= new CapabilityObjectReceivedEventHandler(ObjectReceived);
                presentationCapability = null;
            }

            return ret;
        }
 

        #endregion ICapabilityForm
    }
}
