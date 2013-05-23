using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

using Microsoft.Ink;

using MSR.LST.ConferenceXP;


namespace MSR.LST.RTDocuments.Utilities
{
    public class RTDocumentHelper : IDisposable
    {
        #region Public Static Members
        ///<summary>
        /// This GUID represents the identifier for RTFrame erase all message.
        /// </summary>
        public static string constEraseAllGuid = "E3472EA5-29BC-A4FE-25F3-B135F03C134D";

        ///<summary>
        /// This GUDI represents the identifier to request a re-send of the TOC
        ///</summary>
        public static string constResendTocGuid = "4C475E9A-38FB-41EE-90C9-377B3E649DCC";

        #endregion
        #region Private Instance Members
        private bool disposed = false;

        private Capability capability = null;
        private RTDocument rtDocument = null;

        private Guid rtDocumentIdentifier = Guid.Empty;
        #endregion
        #region Public Properties
        public RTDocument RTDocument
        {
            get
            {
                if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);
                return rtDocument;
            }
        }
        #endregion
        #region Ctors
        public RTDocumentHelper ( Capability capability, RTDocument rtDocument )
        {
            this.capability = capability;

            this.rtDocument = rtDocument;
            rtDocumentIdentifier = rtDocument.Identifier;

            // Set up delegate that is used to send the slides on a background thread
            beginSendAllSlidesDelegate = new SendAllSlidesHandler(SendAllSlides);
        }
        public RTDocumentHelper ( Capability capability )
        {
            this.capability = capability;

            // Set up delegate that is used to send the slides on a background thread
            beginSendAllSlidesDelegate = new SendAllSlidesHandler(SendAllSlides);
        }

        #endregion
        #region IDisposable Members
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                GC.SuppressFinalize(this);
            }
        }
        #endregion
        #region Object Sending Helpers
        //Pri2: Should have an object receiving helper that changes the CurrentOrganizationNodeIdentifier when a PageChange object is received too
        public void SendPageChange ( Page page )
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            TOCNode tocNode = PageToTOCNode ( page );
            RTNodeChanged rtNodeChanged = new RTNodeChanged ( tocNode.Identifier, DateTime.UtcNow, null );
            capability.SendObject ( rtNodeChanged );
            CurrentOrganizationNodeIdentifier = tocNode.Identifier;
        }
        public IAsyncResult BeginSendPageChange ( Page page, AsyncCallback callback, object state )
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            TOCNode tocNode = PageToTOCNode ( page );
            RTNodeChanged rtNodeChanged = new RTNodeChanged ( tocNode.Identifier, DateTime.UtcNow, null );
            IAsyncResult iar = capability.BeginSendObject ( rtNodeChanged, callback, state );
            CurrentOrganizationNodeIdentifier = tocNode.Identifier;
            return iar;
        }

        public void SendPageAdd ( Page page )
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            RTPageAdd pa = new RTPageAdd( page );

            AddTOCNodeToRTDocument( pa.TOCNode );
            AddPageToRTDocument( pa.Page );

            capability.SendObjectBackground(pa);
        }
        public IAsyncResult BeginSendPageAdd ( Page page, AsyncCallback callback, object state )
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            RTPageAdd pa = new RTPageAdd( page );

            AddTOCNodeToRTDocument( pa.TOCNode );
            AddPageToRTDocument( pa.Page );

            return capability.BeginSendObjectBackground(pa, callback, state);
        }

        //Pri2: This does not follow pattern because it doesn't return IAsyncResult -- needed to get the TOCNode out ASAP due to UI logic
        public TOCNode BeginSendPageInsert ( Page page, string title, int index, AsyncCallback callback, object state ) 
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            RTPageAdd pa = new RTPageAdd( page );
            pa.TOCNode.Title = title;
            pa.PreviousSiblingIdentifier = rtDocument.Organization.TableOfContents[index].Identifier;

            AddTOCNodeToRTDocument( index, pa.TOCNode );
            AddPageToRTDocument( pa.Page );

            capability.BeginSendObjectBackground(pa, callback, state);

            return pa.TOCNode;
        }

        public TOCNode SendPageInsert ( Page page, string title, int index ) 
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            //Pri2: There is a lot of code duplication with BeginSendPageInsert, we might want
            //to add a commun method for SendPageInsert and BeginSendPageInsert
            RTPageAdd pa = new RTPageAdd( page );
            pa.TOCNode.Title = title;
            pa.PreviousSiblingIdentifier = rtDocument.Organization.TableOfContents[index].Identifier;

            AddTOCNodeToRTDocument( index, pa.TOCNode );
            AddPageToRTDocument( pa.Page );

            capability.SendObject(pa);

            return pa.TOCNode;
        }

        public void AddTOCNodeToRTDocument( TOCNode tocNode )
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            rtDocument.Organization.TableOfContents.Add( tocNode );
        }

        public void AddTOCNodeToRTDocument( int index, TOCNode tocNode )
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            //Pri3: This assumes a flat TOC -- does not take parent, or calculate using siblings
            rtDocument.Organization.TableOfContents.Insert(index+1, tocNode);
        }

        public void AddPageToRTDocument( Page page )
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            TOCNode tn = PageToTOCNode( page );
            if (tn == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, 
                    Strings.TOCNodeNotFoundForPage, page.Identifier));
            }

            rtDocument.Resources.Pages.Add(page.Identifier, page);
            tn.Resource = page;
        }

        public void SendStrokeAdd ( Stroke stroke )
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            RTStrokeAdd rtStrokeAdd = new RTStrokeAdd ( DateTime.UtcNow, rtDocumentIdentifier, CurrentPageIdentifier, true, stroke, null );
            capability.SendObject ( rtStrokeAdd );
        }
        public IAsyncResult BeginSendStrokeAdd ( Stroke stroke, AsyncCallback callback, object state )
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            RTStrokeAdd rtStrokeAdd = new RTStrokeAdd ( DateTime.UtcNow, rtDocumentIdentifier, CurrentPageIdentifier, true, stroke, null );
            return capability.BeginSendObject ( rtStrokeAdd, callback, state );
        }

        public void SendStrokeRemove( Stroke stroke )
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            RTStrokeRemove rtStrokeRemove = new RTStrokeRemove( DateTime.UtcNow, rtDocumentIdentifier, CurrentPageIdentifier, stroke, null );
            capability.SendObject ( rtStrokeRemove );
        }
        public IAsyncResult BeginSendStrokeRemove( Stroke stroke, AsyncCallback callback, object state )
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            RTStrokeRemove rtStrokeRemove = new RTStrokeRemove ( DateTime.UtcNow, rtDocumentIdentifier, CurrentPageIdentifier, stroke, null );
            return capability.BeginSendObject ( rtStrokeRemove, callback, state );
        }

        public void SendDocumentBody ()
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            capability.SendObject(RTDocumentBodyOnly());
        }        
        public IAsyncResult BeginSendDocumentBody(AsyncCallback callback, object state)
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            return capability.BeginSendObject(RTDocumentBodyOnly(), callback, state);
        }

        public void SendAllSlides ()
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            SendDocumentBody();

            // Send all the pages in order
            for (int i=0; i<rtDocument.Organization.TableOfContents.Count; i++)
            {
                if (!disposed) // Could have been disposed while the slide transmission is in progress
                {
                    capability.SendObjectBackground((Page)rtDocument.Organization.TableOfContents[i].Resource);
                }
            }
        }

        public void SendEraseAllInk()
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            capability.SendObject(
                new RTFrame(new Guid(constEraseAllGuid), null));
        }

        public void SendTocRequest()
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            capability.SendObject(
                new RTFrame(new Guid(constResendTocGuid), null));
        }

        #endregion
        #region Organization/Resource lookup helpers
        public TOCNode PageToTOCNode( Page page )
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            return WalkTOCNodes ( rtDocument.Organization.TableOfContents, page );
        }
        private TOCNode WalkTOCNodes ( TOCList tocNodes, Page page )
        {
            foreach ( TOCNode tocNode in tocNodes )
            {
                if ( tocNode.ResourceIdentifier == page.Identifier )
                {
                    return tocNode;
                }

                if ( tocNode.Children != null )
                {
                    TOCNode tocNodeFromChildren = WalkTOCNodes ( tocNode.Children, page );

                    if (tocNodeFromChildren != null)
                    {
                        return tocNodeFromChildren;
                    }
                }
            }

            return null;
        }

        public Page PageIDToPage( Guid pageID )
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            return WalkTocNodesForPageID( rtDocument.Organization.TableOfContents, pageID );
        }

        private Page WalkTocNodesForPageID( TOCList nodes, Guid pageID )
        {
            foreach ( TOCNode tocNode in nodes )
            {
                if ( tocNode.ResourceIdentifier == pageID )
                {
                    return (Page)tocNode.Resource;
                }

                if ( tocNode.Children != null )
                {
                    Page pageFromChildren = WalkTocNodesForPageID( tocNode.Children, pageID );

                    if (pageFromChildren != null)
                    {
                        return pageFromChildren;
                    }
                }
            }

            return null;
        }

        public Guid TOCNodeIdentifierToPageIdentifier ( Guid tocNodeIdentifier )
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            TOCNode tocNode = rtDocument.Organization.TableOfContents[tocNodeIdentifier];

            if (tocNode == null)
            {
                return Guid.Empty;
            }

            if (tocNode.ResourceIdentifier != Guid.Empty)
            {
                return tocNode.ResourceIdentifier;
            }

            if (tocNode.Resource != null)
            {
                if (tocNode.Resource is Page)
                {
                    return ((Page)tocNode.Resource).Identifier;
                }
                throw new Exception(Strings.TOCNodeResourcePointingToObjectNotPage);
            }

            Page page = (Page)tocNode.Resource;

            if (page == null)
            {
                throw new Exception(Strings.PageNotFoundForTOCNode);
            }

            return page.Identifier;
        }
        //Pri2: Need a function to walk a non-flat TOC to find the identifier
        #endregion
        #region SequenceNumber & OrganizationNode management
        private Mutex mutex = new Mutex();
        private Guid currentOrganizationNodeIdentifier = Guid.Empty;

        public Guid CurrentOrganizationNodeIdentifier
        {
            get
            {
                if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

                mutex.WaitOne();
                Guid id = currentOrganizationNodeIdentifier;
                mutex.ReleaseMutex();
                return id;
            }
            set
            {
                if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

                mutex.WaitOne();

                currentOrganizationNodeIdentifier = value;

                mutex.ReleaseMutex();
            }
        }
        public Guid CurrentPageIdentifier
        {
            get
            {
                if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

                if (rtDocument == null)
                {
                    return Guid.Empty;
                }
                else
                {
                    return ((Page)rtDocument.Organization.TableOfContents[CurrentOrganizationNodeIdentifier].Resource).Identifier;
                }
            }
        }
        #endregion
        #region RTDocument Body Only
        public RTDocument RTDocumentBodyOnly()
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            RTDocument rtDocBody = (RTDocument)((ICloneable)rtDocument).Clone();

            rtDocBody.Resources.ResourceList.Clear();
            rtDocBody.Resources.Pages.Clear();
            rtDocBody.Resources.Extension = null;

            // Go through TOCNodes and strip Resource object refs
            if (rtDocBody.Organization.TableOfContents != null)
            {
                for (int i= 0 ; i < rtDocBody.Organization.TableOfContents.Count; i++)
                {
                    StripResourceObjectFromTocNode(rtDocBody.Organization.TableOfContents[i]);
                }
            }

            return rtDocBody;
        }

        private void StripResourceObjectFromTocNode(TOCNode toc)
        {
            if (toc.Children != null)
            {
                for (int i= 0 ; i < toc.Children.Count; i++)
                {
                    StripResourceObjectFromTocNode(toc.Children[i]);
                }
            }

            toc.Resource = null;
        }
        #endregion
        #region Image Thumbnail Helpers
        private const float ThumbnailWidth = 96F;
        public static Image GetThumbnailFromImage(Image image)
        {
            float height = image.Width / ThumbnailWidth;
            Bitmap bg = new Bitmap((int)ThumbnailWidth, (int)height);
            Graphics g = Graphics.FromImage(bg);
            g.DrawImage(image, 0, 0, (int)ThumbnailWidth, (int)height);  // Go from the metafile to Thumbnail
            return bg;
        }

        public static Image CreateThumbnailPng(Image image, int x, int y)
        {
            MemoryStream ms = new MemoryStream();
            ms.Position = 0;
            image = image.GetThumbnailImage(x, y, myCallback, IntPtr.Zero);
            image.Save(ms,ImageFormat.Png);
            return new Bitmap(ms);
        }

        private static Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
        private static bool ThumbnailCallback()
        {
            return false;
        }

        #endregion

        public delegate void SendAllSlidesHandler();
        private SendAllSlidesHandler beginSendAllSlidesDelegate = null;
        public IAsyncResult BeginSendAllSlides(AsyncCallback callback, object state)
        {
            if (disposed) throw new ObjectDisposedException(Strings.RTDocumentHelperHasBeenDisposed);

            return beginSendAllSlidesDelegate.BeginInvoke(callback, state);
        }

    }
}
