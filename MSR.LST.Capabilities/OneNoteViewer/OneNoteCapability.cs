using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Microsoft.Ink;
using Microsoft.Win32;

using MSR.LST;
using MSR.LST.ConferenceXP;
using MSR.LST.Net.Rtp;
using MSR.LST.RTDocuments;
using MSR.LST.RTDocuments.Utilities;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// PresentationToOneNoteCapability takes notes created from Presentation or other RTDocs-compatable
    /// software and presents them in OneNote, in real time, for in-class student note-taking.
    /// </summary>
    [Capability.Name("OneNote Presentation")]
    [Capability.PayloadType(PayloadType.RTDocument)]
    [Capability.MaxBandwidth(100000)]
    [Capability.FormType(typeof(OptionsForm))]
    [Capability.Channel(true)]
    public abstract class PresentationToOneNoteCapability : Capability, ICapabilityViewer
    {
        #region Constants
        // This GUID represents the RTD identifier for a simple whiteboard.  This prevents having to
        //  ship a blank doc across the wire when Presentation starts (and all the problems that that causes).
        private const string constWhiteboardGuid = "7F41B219-481D-2D47-DAED-C39EE301F5DA";

        // The guid for the Open Remote hack
        // TODO: remove me too.
        private const string constOpenRemoteGuid = "E255521C-AFBC-DDC3-A25E-5332C03DEC32";

        const float constWidthPageSend = 960F;
        const float constHeightPageSend = 720F;

        // Pri2: generalize to handle and DPI
        const float inkSpaceToPixel = 96F / 2.54F / 1000; // @ 96 DPI (~= 53/2 ~= 0.0358)

        // This is 70% of the size of a ON page, at 1024 x 768, empirically determined, and in inches
        //  (the only way that the ON API will take it is in inches).
        const float maxONWidth = 658F;
        const float maxONHeight = 357F;

        const string notebookRegKeyPath = @"Software\Microsoft\Office\{0}\OneNote\Options\Save";
        const string notebookRegKeyName = "My Notebook path";
        #endregion

        #region Variables
        OneNote.ImportServer oneNoteServer;
        OneNote.ISimpleImporter importer;

        // Holds the guids for all the strokes stored on each page (for DeleteAllStrokes)
        //  Key: page ID (guid), Value: Arraylist of stroke IDs (guids)
        Hashtable strokesPerPage = new Hashtable();

        RTDocument rtDoc = null;
        Guid crntPage = Guid.Empty;
        SizeF slideSize = Size.Empty;

        /* Due to the fact that modal dialogs allow the context of an entire thread to be
         * swapped out during use, we need this.  The thread was allowing multiple objects to
         * be received while we're still waiting on the user to tell us what folder to put the
         * document in.  Thus, we need a synchronized way to help handle all of this :(
         */
        Queue receiveQueue = new Queue(4);

        System.CodeDom.Compiler.TempFileCollection tfc = new System.CodeDom.Compiler.TempFileCollection();

        string crntONFile = null;
        string oneNoteNotebookDir = null;

        #endregion

        #region Ctor
        protected PresentationToOneNoteCapability(DynamicProperties dynaProps, string version) : base(dynaProps)
        {
            base.isSender = false;
            base.ObjectReceived += new CapabilityObjectReceivedEventHandler(OnObjectReceived);

            // Get OneNote import server
            oneNoteServer = new OneNote.ImportServer();
            importer = (OneNote.ISimpleImporter)oneNoteServer;

            // Get current OneNote notebook directory from Registry
            using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey( 
                string.Format(CultureInfo.InvariantCulture, notebookRegKeyPath, version )))
            {
                string notebookPath = (string)regKey.GetValue( notebookRegKeyName );
            
                if( notebookPath == null || notebookPath == "My Notebook" )
                {
                    oneNoteNotebookDir = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)
                        + "\\My Notebook";
                }
                else
                {
                    oneNoteNotebookDir = notebookPath;
                }
            }

            if (!Directory.Exists(tfc.BasePath))
                Directory.CreateDirectory(tfc.BasePath);

            // Delete the old log file, if it exists
            if( System.IO.File.Exists("log.xml") )
                System.IO.File.Delete("log.xml");

            // We don't create the blank RTDoc here like Presentation does becuase the
            //  user may decide to open a document, causing the user of OneNote to have
            //  a section with a blank page in it for no purpose.
        }
        #endregion

        #region General Methods (top-level methods)
        public override void StopPlaying()
        {
            base.StopPlaying();
        }

        /// <summary>
        /// Hook the objectReceived event and process it.
        /// </summary>
        private void OnObjectReceived(object o, ObjectReceivedEventArgs orea)
        {
            // Don't receive your own messages (objects)
            if (orea.Participant != Conference.LocalParticipant)
            {
                Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Message recieved of type: {0}", 
                    orea.Data.ToString()));

                // Check to see if there are waiting messages
                lock( this.receiveQueue )
                {
                    // Messages in the queue signals that there's a thread working on a current
                    //  message, thus we don't want to start processing another message
                    receiveQueue.Enqueue( orea );

                    if( receiveQueue.Count != 1 )
                    {
                        // There are other messages waiting on the user.  Return and let the
                        //  currently processing message finish.
                        return;
                    }
                }

                // There are no messages waiting, so we're free to process this one
                while(true)
                {
                    this.ConsumeObject( orea );

                    // Check for messages added while we were processing that one.
                    lock( this.receiveQueue )
                    {
                        receiveQueue.Dequeue(); // that was this message

                        if( receiveQueue.Count != 0 )
                        {
                            orea = (ObjectReceivedEventArgs) receiveQueue.Peek();
                        }
                        else // .Count == 0
                        {
                            return;
                        }
                    }
                }
            }
        }

        private void ConsumeObject( ObjectReceivedEventArgs orea )
        {
            // Create RTDoc with one slide & put it in ON
            if (rtDoc == null && !(orea.Data is RTDocument || orea.Data is RTFrame))
            {
                // Create blank RTDocument
                rtDoc = new RTDocument();
                rtDoc.Identifier = new Guid(constWhiteboardGuid);
                rtDoc.Metadata.Title = string.Format(CultureInfo.CurrentCulture, Strings.WhiteboardSession, 
                    DateTime.Now.ToString("u", CultureInfo.CurrentCulture));
                rtDoc.Metadata.Creator = Conference.LocalParticipant.Name;

                // Add a blank page
                Page pg = new Page();
                pg.Identifier = new Guid(constWhiteboardGuid);
                TOCNode tn = new TOCNode();
                tn.Title = Strings.WhiteboardTitle;
                tn.Resource = pg;
                tn.ResourceIdentifier = pg.Identifier;
                tn.Identifier = new Guid(constWhiteboardGuid);
                rtDoc.Organization.TableOfContents.Add(tn);
                rtDoc.Resources.Pages.Add(pg.Identifier, pg);

                // Add the page to the strokes hash
                strokesPerPage.Add( pg.Identifier, new ArrayList() );

                // Init necessary vars
                this.crntPage = pg.Identifier;
                this.crntONFile = "Whiteboard - " + 
                    DateTime.Now.ToString("u", CultureInfo.InvariantCulture).Replace(":", ".").Replace("Z", string.Empty) + ".one";

                // Import the page
                this.InsertPageInON( pg, tn.Title, Guid.Empty );

                // Show first page
                System.Threading.Thread.Sleep(50);
                importer.NavigateToPage( crntONFile, crntPage.ToString("B") );
            }

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
                RTNodeChangedReceived(
                    (RTNodeChanged)orea.Data);
            } 
            else if (orea.Data is RTDocument)
            {
                RTDocumentReceived((RTDocument)orea.Data);
            }
            else if (orea.Data is RTFrame)
            {
                RTFrameReceived( (RTFrame)orea.Data );
            }
        }

        #endregion

        #region Specialized Receive methods
        /// <summary>
        /// Handle the reception of an RTDocument object. The rtDocument received
        /// does not contain the actual pages (the pages are received separately),
        /// so this function mainly handles the reception of the document structure 
        /// (TOC, number of pages, guid of pages, etc.) on the client(s).
        /// </summary>
        /// <param name="rtDocReceived">rtDocument structure received</param>
        private void RTDocumentReceived(RTDocument rtDocReceived)
        {
            // Set rtDoc only if you receive a new document (not a repeat)
            if ((rtDoc == null) || (rtDocReceived.Identifier != rtDoc.Identifier))
            {
                rtDoc = rtDocReceived;

                // Deal with blank titles
                if( rtDocReceived.Metadata.Title == null )
                {
                    rtDocReceived.Metadata.Title = "Presentation Session";
                }

                crntONFile = MakeTitleValid(rtDocReceived.Metadata.Title);

                // If this file name already exists, we append the date & time to it.
                if( System.IO.File.Exists( this.oneNoteNotebookDir + "\\" + crntONFile + ".one" ) )
                {
                    if( Options.ShowOverwriteFileQuestion(crntONFile) )
                    {
                        // Delete fails becuase the file is in use.  I guess we just plop the data
                        //  on top of the old slide, eh?
                        crntONFile += ".one";
                    }
                    else
                    {
                        crntONFile += DateTime.Now.ToString("u", 
                            CultureInfo.InvariantCulture).Replace(":", ".").Replace("Z", string.Empty) + ".one";
                    }
                }
                else
                {
                    crntONFile += ".one";
                }

                // Add all Pages to strokes hash immediately
                this.strokesPerPage.Clear();
                foreach( TOCNode tn in rtDoc.Organization.TableOfContents )
                {
                    this.strokesPerPage.Add( tn.ResourceIdentifier, new ArrayList() );
                }
            }
        }

        /// <summary>
        /// Handle the reception of a Page object.
        /// </summary>
        /// <param name="p">Page received</param>
        private void PageReceived(Page page)
        {
            // Put page in rtDoc
            TOCNode parentNode = null;
            int index;
            for(index = 0; index < rtDoc.Organization.TableOfContents.Count; ++index )
            {
                TOCNode node = rtDoc.Organization.TableOfContents[index];
                if( node.ResourceIdentifier == page.Identifier )
                {
                    node.Resource = page;
                    parentNode = node;
                    break;
                }
            }
            if( parentNode == null )
                throw new InvalidOperationException(Strings.ParentTocNodeNotFound);

            // Add page to Resources.Pages
            if( !rtDoc.Resources.Pages.ContainsKey(page.Identifier) )
            {
                rtDoc.Resources.Pages.Add( page.Identifier, page );

                // Insert Page in ON
                // Instead of finding the previous sibling we just insert it at the end
                // and assume the page is in order (which it is due to the current
                // background send implementation of Presenation).  We're avoiding using the
                // "insertAfter" attribute of DataImport becuase it add pages as subpages.
                // Other, inserted slides, such as captures and new whiteboards will get 
                // inserted as subpages, thus giving them a different look to imply they were
                // inserted & not originally in the document.
                this.InsertPageInON( page, parentNode.Title, Guid.Empty );

                // If it's the first page, display it
                Guid pFirst = rtDoc.Organization.TableOfContents[0].ResourceIdentifier;
                if (page.Identifier == pFirst)
                {
                    crntPage = pFirst;
                    importer.NavigateToPage( crntONFile, pFirst.ToString("B") );
                }
            }
        }

        /// <summary>
        /// Handle the reception of a RTPageAdd object. The
        /// RTPageAdd allows to dynamically add pages on a document
        /// on client(s).
        /// </summary>
        /// <param name="pa">Page to add</param>
        private void RTPageAddReceived(RTPageAdd pa)
        {
            // Add to stroke hash
            strokesPerPage.Add(pa.Page.Identifier, new ArrayList());

            Guid prevPageID = Guid.Empty;

            // Add TOCNode to rtDoc
            if( pa.PreviousSiblingIdentifier != Guid.Empty )
            {
                TOCNode prevSib = rtDoc.Organization.TableOfContents[pa.PreviousSiblingIdentifier];
                int prevSibIndex = rtDoc.Organization.TableOfContents.IndexOf( prevSib );
                
                if( prevSib == null )
                {
                    // If we can't find the page, just use the last one
                    prevSibIndex = rtDoc.Organization.TableOfContents.Count-1;
                    if( prevSibIndex != -1 )
                    {
                        prevSib = rtDoc.Organization.TableOfContents[prevSibIndex];
                    }
                }

                rtDoc.Organization.TableOfContents.Insert( prevSibIndex+1, pa.TOCNode );
                prevPageID = (prevSib == null) ? Guid.Empty : prevSib.ResourceIdentifier;
            }
            else
            {
                rtDoc.Organization.TableOfContents.Add(pa.TOCNode);
            }

            // Add page to rtDoc
            pa.TOCNode.Resource = pa.Page;
            pa.TOCNode.ResourceIdentifier = pa.Page.Identifier;
            rtDoc.Resources.Pages.Add(pa.Page.Identifier, pa.Page);

            // Insert Page in ON
            this.InsertPageInON( pa.Page, pa.TOCNode.Title, prevPageID );
        }

        /// <summary>
        /// Handle the reception of a RTStroke object.
        /// </summary>
        /// <param name="rtStroke">Stroke received</param>
        private void RTStrokeReceived(RTStroke rtStroke)
        {
            Page pg = rtDoc.Resources.Pages[rtStroke.PageIdentifier];
            
            if( pg == null ) // if the page is missing, ignore the stroke :(
                return;

            SizeF imageSize = GetSlideSize( pg.Image );

            // Resize the received stroke
            float xRatio = (imageSize.Width / constWidthPageSend)*inkSpaceToPixel;
            float yRatio = (imageSize.Height / constHeightPageSend)*inkSpaceToPixel;
            Rectangle bounds = rtStroke.Stroke.GetBoundingBox();
            RectangleF newBounds = new RectangleF( bounds.X*xRatio, bounds.Y*yRatio, 
                bounds.Width*xRatio, bounds.Height*yRatio );

            // Add the stroke to the page
            StringBuilder importData = new StringBuilder(2000);
            XmlTextWriter xml = CreateInitdXml(importData);

            xml.WriteStartElement( "PlaceObjects" );
            xml.WriteAttributeString( "pagePath", crntONFile );
            xml.WriteAttributeString( "pageGuid", rtStroke.PageIdentifier.ToString("B") );

            xml.WriteStartElement( "Object" );
            xml.WriteAttributeString( "guid", rtStroke.StrokeIdentifier.ToString("B") );

            xml.WriteStartElement( "Position" );
            xml.WriteAttributeString( "x", newBounds.X.ToString(CultureInfo.InvariantCulture) );
            xml.WriteAttributeString("y", newBounds.Y.ToString(CultureInfo.InvariantCulture));
            xml.WriteEndElement(); // end Position

            xml.WriteStartElement( "Ink" );
            xml.WriteAttributeString("width", newBounds.Width.ToString(CultureInfo.InvariantCulture));
            xml.WriteAttributeString("height", newBounds.Height.ToString(CultureInfo.InvariantCulture));

            Ink ink = new Ink();
            ink.AddStrokesAtRectangle( rtStroke.Strokes, rtStroke.Strokes.GetBoundingBox() );
            byte[] base64ISF_bytes = ink.Save( PersistenceFormat.Base64InkSerializedFormat );
            xml.WriteStartElement( "Data" );
            xml.WriteBase64( base64ISF_bytes, 0, base64ISF_bytes.Length );
            xml.WriteEndElement(); // end Data

            xml.WriteEndDocument();

            string finalData = importData.ToString();
            LogAsLastCommand( finalData );
            importer.Import( finalData );

            // prevents ink & objects from getting inserted too quickly after a page
            System.Threading.Thread.Sleep(50); 

            // Store the stroke ID in strokesPerPage
            ((ArrayList)strokesPerPage[rtStroke.PageIdentifier]).Add(rtStroke.StrokeIdentifier);
        }

        /// <summary>
        /// Handle the reception of an RTStrokeRemove object.
        /// </summary>
        /// <param name="rtStrokeRemove">Stroke to remove</param>
        private void RTStrokeRemoveReceived(RTStrokeRemove rtStrokeRemove)
        {
            // Remove the stroke ID from the arraylist of strokes for that page
            ((ArrayList)strokesPerPage[rtStrokeRemove.PageIdentifier]).Remove(rtStrokeRemove.StrokeIdentifier);

            // Get the remove id & call delete on it
            StringBuilder importData = new StringBuilder(2000);
            XmlTextWriter xml = CreateInitdXml(importData);

            xml.WriteStartElement( "PlaceObjects" );
            xml.WriteAttributeString( "pagePath", crntONFile );
            xml.WriteAttributeString( "pageGuid", rtStrokeRemove.PageIdentifier.ToString("B") );

            xml.WriteStartElement( "Object" );
            xml.WriteAttributeString( "guid", rtStrokeRemove.StrokeIdentifier.ToString("B") );
            xml.WriteElementString( "Delete", String.Empty );

            xml.WriteEndDocument();

            string finalData = importData.ToString();
            LogAsLastCommand( finalData );
            importer.Import( finalData );
        }

        /// <summary>
        /// Handle the reception of an RTNodeChanged object. 
        /// The RTNodeChanged allows to navigate
        /// through pages on the client(s).
        /// </summary>
        /// <param name="nav">Navigation object received</param>
        private void RTNodeChangedReceived(RTNodeChanged nav)
        {
            // Test if we are in slaved navigation mode
            TOCNode node = rtDoc.Organization.TableOfContents[nav.OrganizationNodeIdentifier];
            if( node != null && node.ResourceIdentifier != Guid.Empty && node.Resource != null )
            {
                this.crntPage = node.ResourceIdentifier;

                if( Options.AutoSyncSlides )
                    this.SyncToPage( this, EventArgs.Empty );
            }
        }

        /// <summary>
        /// Handle the reception of an EraseType. This is used to handle the eraser icon 
        /// that removes all strokes on the screen.
        /// </summary>
        /// <param name="eraseReceived">Contains the erase type. Right now there is
        /// only EraseAll, but it might be extended later on</param>
        private void RTFrameReceived(RTFrame rtFrameReceived)
        {
            if ( rtFrameReceived.ObjectTypeIdentifier == new Guid(RTDocumentHelper.constEraseAllGuid)
                && crntPage != Guid.Empty )
            {
                // Delete all strokes on this page
                ArrayList strokes = (ArrayList)this.strokesPerPage[crntPage];

                StringBuilder importData = new StringBuilder(2000);
                XmlTextWriter xml = CreateInitdXml(importData);

                xml.WriteStartElement( "PlaceObjects" );
                xml.WriteAttributeString( "pagePath", crntONFile );
                xml.WriteAttributeString( "pageGuid", crntPage.ToString("B") );

                foreach( Guid strokeID in strokes )
                {
                    xml.WriteStartElement( "Object" );
                    xml.WriteAttributeString( "guid", strokeID.ToString("B") );
                    xml.WriteElementString( "Delete", String.Empty );
                    xml.WriteEndElement(); // end Object
                }

                xml.WriteEndDocument();

                string finalData = importData.ToString();
                LogAsLastCommand( finalData );
                importer.Import( finalData );
            }
        }

        #endregion

        #region Private Utility Methods
        /// <summary>
        /// Takes a title and turns it into a valid file name, in a hack-ish fasion
        /// </summary>
        private string MakeTitleValid( string title )
        {
            title = title.Replace(":", ".");
            title = title.Replace("?", string.Empty);
            title = title.Replace("\\", string.Empty);
            title = title.Replace("/", string.Empty);
            title = title.Replace("*", string.Empty);
            title = title.Replace("<", string.Empty);
            title = title.Replace(">", string.Empty);
            title = title.Replace("\"", string.Empty);
            return title.Replace("|", string.Empty);
        }

        /// <summary>
        /// Syncs ON to the current page.
        /// </summary>
        internal void SyncToPage( object sender, EventArgs e )
        {
            if( crntPage != Guid.Empty && this.crntONFile != null )
            {
                string navTo = this.crntPage.ToString("B");
                LogAsLastCommand(string.Format(CultureInfo.CurrentCulture, 
                    "importer.NavigateToPage(\"{0}\", \"{1}\");", crntONFile, navTo));
                importer.NavigateToPage(crntONFile, navTo);
            }
        }

        /// <summary>
        /// Provides quick & dirty access to the OptionsForm form for this importer
        /// </summary>
        private OptionsForm Options
        {
            get
            {
                return (OptionsForm) base.form;
            }
        }

        /// <summary>
        /// Does all of the necessary work to create the Xml for a new page & inserts it.
        /// </summary>
        private void InsertPageInON( Page page, string title, Guid parentPage )
        {
            // Give the drawing area a background by adding an image to all pages without one
            if( page.Image == null )
            {
                page.Image = new Bitmap( (int)(constWidthPageSend / 2F), (int)(constHeightPageSend / 2F),
                    System.Drawing.Imaging.PixelFormat.Format16bppRgb555 );
                // Add a nice black line around it to make it look good.
                using( Graphics g = Graphics.FromImage(page.Image) )
                {
                    g.Clear( Color.White );
                }
            }

            // Pri3: Draw a single black line around all images
            // For unknown reasons, when trying to get a Graphics on a MWF an OutOfMemoryException
            //   is thrown.  Thus, we don't draw the black line on all images just yet.

            StringBuilder importData = new StringBuilder( (page.Image == null) ? 2000 : 1000000 );
            XmlTextWriter xml = CreateInitdXml( importData );

            // Ensure page verifies that the page exists (and creates it if it doesn't)
            xml.WriteStartElement( "EnsurePage" );
            xml.WriteAttributeString( "path", crntONFile );
            xml.WriteAttributeString( "guid", page.Identifier.ToString("B") );
            xml.WriteAttributeString( "title", title );
            // OneNote request a date in the following format "2003-10-16T17:30:00-08:00"
            // TODO: Remove the hardcoding of 08:00 and get this value from the local settings
            xml.WriteAttributeString("date", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss-08:00", 
                CultureInfo.InvariantCulture));

            if( parentPage != Guid.Empty )
            {
                xml.WriteAttributeString( "insertAfter", parentPage.ToString("B") );
            }
            xml.WriteEndElement(); // end EnsurePage

            // Write the image into the page
            if( page.Image != null )
            {
                xml.WriteStartElement( "PlaceObjects" );
                xml.WriteAttributeString( "pagePath", crntONFile );
                xml.WriteAttributeString( "pageGuid", page.Identifier.ToString("B") );

                GetImageAsXml( page.Image, xml );
                
                xml.WriteEndElement(); // end PlaceObjects
            }

            xml.WriteEndDocument();


            string finalData = importData.ToString();
            LogAsLastCommand( finalData );
            importer.Import( finalData );
        }

        /// <summary>
        /// Takes an image and writes it as an Image element to the Xml stream provided.
        /// </summary>
        private void GetImageAsXml( Image img, XmlTextWriter xml )
        {
            xml.WriteStartElement( "Object" );
            xml.WriteAttributeString( "guid", Guid.NewGuid().ToString("B") );

            xml.WriteStartElement( "Position" );
            xml.WriteAttributeString( "x", "0" );
            xml.WriteAttributeString( "y", "0" );
            xml.WriteEndElement(); // end Position

            xml.WriteStartElement( "Image" );
            xml.WriteAttributeString( "backgroundImage", "true" );

            // Set size of image
            SizeF imageSize = GetSlideSize( img );
            xml.WriteAttributeString( "width", (imageSize.Width).ToString(CultureInfo.InvariantCulture) );
            xml.WriteAttributeString( "height", (imageSize.Height).ToString(CultureInfo.InvariantCulture) );

            // Write the image in the xml as Base64
            xml.WriteStartElement( "Data" );
            byte[] bits;
            if( img.RawFormat.Equals( ImageFormat.Emf ) || img.RawFormat.Equals( ImageFormat.Wmf ) )
            {
                // The image goes bad during serialization, so we have to clone it.
                Metafile mf = (Metafile)((Metafile)img).Clone();
                IntPtr ptr = mf.GetHenhmetafile();

                Debug.Assert(ptr != IntPtr.Zero, "Failed to get pointer to image.");

                uint size = GetEnhMetaFileBits(ptr, 0, null);
                bits = new byte[size];
                uint numBits = GetEnhMetaFileBits(ptr, size, bits);

                mf.Dispose();

                Debug.Assert(size == numBits, "Improper serialization of metafile!");
            }
            else
            {
                MemoryStream imgMS = new MemoryStream();
                img.Save(imgMS, System.Drawing.Imaging.ImageFormat.Jpeg);
                bits = imgMS.ToArray();
            }

            xml.WriteBase64(bits, 0, bits.Length);
            xml.WriteEndElement(); // end Data
            xml.WriteEndElement(); // end Image
            xml.WriteEndElement(); // end Object
        }

        /// <summary>
        /// Simply gets the size, in inches, of the image to be placed in OneNote.  This automatically scales
        /// the image to fit in a specified maximum area.
        /// </summary>
        private SizeF GetSlideSize( Image pgImg )
        {
            SizeF slideSize = SizeF.Empty;

            // If the image is null, we want a 3:4 whiteboard
            float slideRatio = (pgImg == null) ? 0.75F : ( (float)pgImg.Height / (float)pgImg.Width );

            if( slideRatio*maxONWidth >= maxONHeight )
                // Width is oversize, resize by height
            {
                slideSize.Height = maxONHeight;
                slideSize.Width =  maxONHeight / slideRatio;
            }
            else
                // Height is oversize, resize by width
            {
                slideSize.Width = maxONWidth;
                slideSize.Height = maxONWidth * slideRatio;
            }

            return slideSize;
        }

        /// <summary>
        /// Creates a new XmlTextWriter from a StringBuilder, intialized properly to comply with OneNote's needs.
        /// </summary>
        /// <remarks>
        /// The StringBuilder is init'd eslewhere so that each method can define a default size
        /// (e.g. ImportPage expects images in the 500KB+ range, while importing ink should be a couple KB)
        /// </remarks>
        private XmlTextWriter CreateInitdXml( StringBuilder importData )
        {
            XmlTextWriter xml = new XmlTextWriter( new StringWriter(importData, CultureInfo.InvariantCulture) );
            xml.Formatting = Formatting.Indented;
            xml.Indentation = 4;
            xml.WriteStartDocument();

            // Pri3: Solve this hack & make sure the underlying data really is utf-8
            // replace "utf-16" with "utf-8" so that OneNote doesn't flip out.
            importData.Replace( "utf-16", "utf-8" );

            xml.WriteStartElement( "Import" );

            xml.WriteAttributeString( "xmlns", "http://schemas.microsoft.com/office/onenote/2004/import" );
            return xml;
        }

        /// <summary>
        /// Writes a string of text to the log.xml file for debugging.
        /// </summary>
        /// <remarks>
        /// Because of the size of the data, writing to a file is probably better than writing to the event log, etc.
        /// </remarks>
        private void LogAsLastCommand( string text )
        {
            if( System.Diagnostics.Debugger.IsAttached )
            {
                StreamWriter debugLog = new StreamWriter("log.xml", true, System.Text.UTF8Encoding.UTF8);
                debugLog.WriteLine(text);
                debugLog.Close();
            }
        }

        #endregion

        #region PInvoke Methods
        // These methods are used for importing and exporting files to raw EMF (Enhanced Windows MetaFiles) image types.
        // It just so happens that EMF is an outstanding format for storing vectorized PowerPoint slides.

        [DllImport("Gdi32.dll")]
        private static extern uint GetEnhMetaFileBits(IntPtr hMetaFile, uint size, [In,Out]byte[] pData);

        [DllImport("Gdi32.dll")]
        private static extern IntPtr SetEnhMetaFileBits(uint bufferSize, byte[] pData);

        [DllImport("Gdi32.dll")]
        private static extern bool DeleteEnhMetaFile(IntPtr hMetaFile);
        #endregion

        #region Static Methods

        /// <summary>
        /// Returns whether this machine meets all of the software requirements to run the OneNoteCapability
        /// </summary>
        public static new bool IsRunable(string regInstallRoot, int major, int minor, int build, 
            string warningKey, string msg)
        {
            try
            {
                string oneNoteLocation;
                using (RegistryKey oneNoteKey = Registry.LocalMachine.OpenSubKey(regInstallRoot))
                {
                    oneNoteLocation = (string)oneNoteKey.GetValue("Path") + "OneNote.exe";
                }

                if (oneNoteLocation == null)
                {
                    // OneNote is not installed, so it will not be listed
                    // as an available capability viewer
                    return false;
                }

                FileVersionInfo oneNoteVersionInfo = FileVersionInfo.GetVersionInfo(oneNoteLocation);
                if (!(oneNoteVersionInfo.ProductMajorPart == major
                    && oneNoteVersionInfo.ProductMinorPart >= minor
                    && oneNoteVersionInfo.ProductBuildPart >= build))
                {
                    // Check to make sure we haven't done this before
                    using (RegistryKey cxpKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft Research\ConferenceXP"))
                    {
                        string oneNoteWarning = (string)cxpKey.GetValue("OneNote Warning");

                        if (oneNoteWarning == null)
                        {
                            // Write the reg key to make sure we only say this once...
                            cxpKey.SetValue(warningKey, "true");

                            RtlAwareMessageBox.Show(null, msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, 
                                MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

                            return false;
                        }
                    }
                }

                // If all of that worked, we're golden
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        #endregion
    }

    [Capability.Name("OneNote 2003 Presentation")]
    public class PresentationToOneNote2003Capability : PresentationToOneNoteCapability
    {
        #region Statics
        
        /// <summary>
        /// Returns whether this machine meets all of the software requirements to run the OneNoteCapability
        /// </summary>
        public static new bool IsRunable
        {
            get
            {
                string regInstallRoot = @"SOFTWARE\Microsoft\Office\11.0\OneNote\InstallRoot";
                string warningKey = Strings.OneNote2003WarningKey;
                string msg = Strings.OneNote2003WarningMsg;

                // 11.0.6355 is OneNote 2003, with SP1 (minimum requirement)
                return PresentationToOneNoteCapability.IsRunable(regInstallRoot, 
                    11, 0, 6356, warningKey, msg);
            }
        }

        #endregion

        #region Ctor

        public PresentationToOneNote2003Capability(DynamicProperties dynaProps)
            : base(dynaProps, "11.0"){}

        #endregion
    }

    [Capability.Name("OneNote 2007 Presentation")]
    public class PresentationToOneNote2007Capability : PresentationToOneNoteCapability
    {
        #region Statics

        /// <summary>
        /// Returns whether this machine meets all of the software requirements to run the OneNoteCapability
        /// </summary>
        public static new bool IsRunable
        {
            get
            {
                string regInstallRoot = @"SOFTWARE\Microsoft\Office\12.0\OneNote\InstallRoot";
                string warningKey = Strings.OneNote2007WarningKey;
                string msg = Strings.OneNote2007WarningMsg;

                // 12.0.nnnn is OneNote 2007 RTM
                return PresentationToOneNoteCapability.IsRunable(regInstallRoot,
                    12, 0, 0, warningKey, msg);
            }
        }

        #endregion

        #region Ctor

        public PresentationToOneNote2007Capability(DynamicProperties dynaProps)
            : base(dynaProps, "12.0") { }

        #endregion
    }
}
