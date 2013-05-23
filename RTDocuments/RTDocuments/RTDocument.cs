using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;


namespace MSR.LST.RTDocuments
{

    /// <summary>
    /// The RTDocument structure is based upon the LRN specification for exchanging learning content.
    /// See http://www.imsproject.org or http://www.Microsoft.Com/eLearn for details.
    /// Based on IMS Content Packaging Information Model v1.1.2.
    /// </summary>
    [Serializable]
    public class RTDocument : ICloneable
    {
        #region Members

        /// <summary>
        /// Unique Identifier of the RTDocument. We use a Guid as the identifier, as it can
        /// be generated from any machine and still create a unique identifier for the document.
        /// </summary>
        private Guid identifier = Guid.Empty;

        #endregion Members

        #region Public

        public Metadata Metadata = new Metadata();
        public Organization Organization = new Organization();
        public Resources Resources = new Resources();

        /// <summary>
        /// Accessor of the RTDocument Identifier 
        /// </summary>
        public Guid Identifier
        {
            get{return identifier;}
            set{identifier = value;}
        }

        #endregion Public

        object ICloneable.Clone()
        {
            RTDocument rtD = new RTDocument();

            rtD.identifier = identifier;
            rtD.Metadata = (Metadata)((ICloneable)Metadata).Clone();
            // Need to be sure to clone resources first because Organization refers to Resources...
            rtD.Resources = (Resources)((ICloneable)Resources).Clone();
            rtD.Organization = (Organization)((ICloneable)Organization).Clone();

            return rtD;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, 
                "RTDocument [ RTDocument.Identifier: {0}, Metadata.Title: {1}, Organization.TableOfContents.Count: {2}, Resources.Pages.Count: {3}, Resources.ResourceList.Count: {4} ]", 
                identifier.ToString(), Metadata.Title, Organization.TableOfContents.Count, Resources.Pages.Count, 
                Resources.ResourceList.Count);
        }
    }

    /// <summary>
    /// Based on the Dublin Core and IMS Learning Resource Meta-Data Information Model.
    /// See: http://dublincore.org/documents/dces/
    /// or: http://www.imsproject.org
    /// for more information.
    /// </summary>
    [Serializable]
    public class Metadata : ICloneable
        {
        /// <summary>
        /// 
        /// Name:        Title
        /// Identifier:  Title
        /// Definition:  A name given to the resource.
        /// Comment:     Typically, a Title will be a name by which the resource is formally known.
        /// 
        /// 1000 characters max
        /// </summary>
        public string Title;
        /// <summary>
        /// 
        /// Name:        Description
        /// Identifier:  Description
        /// Definition:  An account of the content of the resource.
        /// Comment:     Description may include but is not limited to: an abstract, table of contents, reference to a graphical representation of content or a free-text account of the content.
        /// 
        /// 2000 characters max
        /// </summary>
        public string Description;
        /// <summary>
        /// 1000 characters max
        /// </summary>
        public string Keyword;
        /// <summary>
        /// 
        /// Name:        Creator
        /// Identifier:  Creator
        /// Definition:  An entity primarily responsible for making the content of the resource.
        /// Comment:     Examples of a Creator include a person, an organisation, or a service.  Typically, the name of a Creator should be used to indicate the entity.
        /// 
        /// IMSComment:  Corresponds to 
        /// 
        /// </summary>
        public string Creator;
        /// <summary>
        /// 50 characters max
        /// </summary>
        public string Version;
        /// <summary>
        /// Corresponds to an IMS entry lifecycle, contribute == creation, date == CreationDate
        /// </summary>
        public DateTime CreationDate = DateTime.UtcNow;
        /// <summary>
        /// Any application extension.  Similar in use to the Windows.Forms.Control.Tag object.
        /// </summary>
        public object Extension;

        object ICloneable.Clone()
        {
            Metadata md = new Metadata();

            md.Title = Title;
            md.Description = Description;
            md.Keyword = Keyword;
            md.Creator = Creator;
            md.Version = Version;
            md.CreationDate = CreationDate;

            if (Extension != null)
            {
                if (Extension is ICloneable)
                {
                    md.Extension = ((ICloneable)Extension).Clone();
                }
            }

            return md;
        }


        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, 
                "Metadata [ Title: {0}, Description: {1}, Keyword: {2}, Creator: {3} ]", Title, Description, 
                Keyword, Creator);
        }
    }

    /// <summary>
    /// Organization contains the organization of the content.  There may be more than one organization per document
    /// and the organization may be of different types, such as a Table of Contents or a multiple choice graph, etc.
    /// </summary>
    [Serializable]
    public class Organization : ICloneable
        {
        /// <summary>
        /// Ordered list of TableOfContents entries
        /// </summary>
        public TOCList TableOfContents = new TOCList();
        //Pri3: Should there be a common IOrganizationNode and a helper function that creates a unified hashtable across all Organization Nodes for easy lookup?
        //      Have a Get property that holds a flat index of all IOrganizationNodes and their Guid Identifiers as Key
        public object Extension;

        object ICloneable.Clone()
        {
            Organization org = new Organization();

            if (TableOfContents != null)
            {
                org.TableOfContents = (TOCList)TableOfContents.Clone();
            }

            if (Extension != null)
            {
                if (Extension is ICloneable)
                {
                    org.Extension = ((ICloneable)Extension).Clone();
                }
            }

            return org;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "Organization [ TableOfContents.Count: {0} ]", 
                TableOfContents.Count);
        }
    }


    /// <summary>
    /// The simplest organization is a Table of Contents which is just like the Table Of Contents from any book.
    /// </summary>
    [Serializable]
    public class TOCNode : ICloneable
    {
        public Guid Identifier = Guid.NewGuid();
        /// <summary>
        /// Reference to an object under Resources, such as a File or Page
        /// </summary>
        //Pri3: Should make sure that whenever Resource is set that ResourceIdentifier is set automatically
        //      This will require that every Resource implement an interface that exposes Identifier.  This will help in creating a unified lookup of all resources anyway
        public object Resource;
        /// <summary>
        /// Used for when the Resource objects are stripped from the RTDocument for transport reasons
        /// </summary>
        public Guid ResourceIdentifier;
        /// <summary>
        /// 200 characters max
        /// </summary>
        public string Title;
        /// <summary>
        /// A Thumbnail image of the page 96 pixels wide.  96x72 for 4x3 documents (like PPT) or 96x124 for 8.5"x11" documents like Acrobat, Word, etc.
        /// </summary>
        public System.Drawing.Image Thumbnail;
        public TOCList Children;
        public object Extension;

        public object Clone()
        {
            TOCNode tn = new TOCNode();

            tn.Identifier = Identifier;
            tn.Resource = Resource;
            tn.ResourceIdentifier = ResourceIdentifier;
            tn.Title = Title;
            if (Thumbnail != null)
            {
                tn.Thumbnail = (Image)Thumbnail.Clone();
            }
            if (Children != null)
            {
                tn.Children = (TOCList)Children.Clone();
            }

            if (Extension != null)
            {
                if (Extension is ICloneable)
                {
                    tn.Extension = ((ICloneable)Extension).Clone();
                }
            }

            return tn;
        }


        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, 
                "TOCNode [ Identifier: {0}, ResourceIdentifier: {1}, Title: {2}, Children.Count: {3} ]", 
                Identifier.ToString(), ResourceIdentifier.ToString(), Title, Children.Count);
        }
    }


    /// <summary>
    /// The list of Resources associated with this document.
    /// For our needs, there will generally be one File and many Pages
    /// </summary>
    [Serializable]
    public class Resources : ICloneable
    {
        public ResourceHashtable ResourceList = new ResourceHashtable();
        //Pri3: Should there be a common IResource and a helper function that creates a unified hashtable across all Resources for easy lookup?
        public PageHashtable Pages = new PageHashtable();
        public object Extension;

        object ICloneable.Clone()
        {
            Resources res = new Resources();

            if (ResourceList != null)
            {
                res.ResourceList = (ResourceHashtable)ResourceList.Clone();
            }
            if (Pages != null)
            {
                res.Pages = (PageHashtable)Pages.Clone();
            }

            if (Extension != null)
            {
                if (Extension is ICloneable)
                {
                    res.Extension = ((ICloneable)Extension).Clone();
                }
            }

            return res;
        }
    }

    /// <summary>
    /// Used to refer to the original document
    /// </summary>
    [Serializable]
    public class Resource : ICloneable
    {
        public Guid Identifier;
        /// <summary>
        /// Type of resource this is, per IMS spec.  Current only defined value for this is "webcontent".  Ignored for our needs, but part of the std.
        /// </summary>
        public ResourceType Type;
        /// <summary>
        /// URL to a remote resource.
        /// </summary>
        public string HRef;
        public File[] Files;
        /// <summary>
        /// The dependency element identifies a single resource which can act as a container for multiple files that this resource
        /// depends upon. Rather than having to list all resources item by item each time they are needed, dependency allows authors
        /// to define a container of resources and to simply refer to that dependency element instead of individual resources.
        /// The same restrictions on the values of the identifierref attribute apply to dependency as apply to item
        /// (see Section 4.4.2 if the IMS spec for further guidance). Below is an example of using dependency. 
        /// </summary>
        public Resource[] Dependencies;

        public object Extension;

        object ICloneable.Clone()
        {
            Resource clone = new Resource();

            clone.Identifier = Identifier;
            clone.Type = Type;
            clone.HRef = HRef;
            if (Files != null)
            {
                clone.Files = (File[])Files.Clone();
            }
            if (Dependencies != null)
            {
                clone.Dependencies = (Resource[])Dependencies.Clone();
            }

            if (Extension != null)
            {
                if (Extension is ICloneable)
                {
                    clone.Extension = ((ICloneable)Extension).Clone();
                }
            }

            return clone;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, 
                "Resource [ Identifier: {0}, HRef: {1}, Files.Length: {2}, Dependencies.Length: {3} ]", 
                Identifier.ToString(), HRef, Files.Length, Dependencies.Length);
        }
    }

    [Serializable]
    public class File : ICloneable
    {
        public Guid Identifier;
        /// <summary>
        /// URL to the document if stored on the network or in the filesystem
        /// </summary>
        public string HRef;
        /// <summary>
        /// Byte[]s of the document if stored in this structure
        /// </summary>
        public byte[] FileBytes;
        /// <summary>
        /// IETF MimeType
        /// </summary>
        public string MimeType;
        /// <summary>
        /// Filename extension, useful for associating files to applications when a corresponding MimeType doesn't exist.
        /// </summary>
        public object Extension;

        object ICloneable.Clone()
        {
            File clone = new File();

            clone.Identifier = Identifier;
            clone.HRef = HRef;
            if (FileBytes != null)
            {
                clone.FileBytes = (byte[])FileBytes.Clone();
            }
            clone.MimeType = MimeType;

            if (Extension != null)
            {
                if (Extension is ICloneable)
                {
                    clone.Extension = ((ICloneable)Extension).Clone();
                }
            }

            return clone;
        }


        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, 
                "File [ Identifier: {0}, HRef: {1}, FileBytes.Length: {2}, MimeType: {3} ]", 
                Identifier.ToString(), HRef, FileBytes.Length, MimeType);
        }
    }

    [Serializable]
    public class Page : ICloneable
        {
        public Guid Identifier;
        /// <summary>
        /// Supported mime types are image/png and image/x-wmf.  Other image types could go here in future
        /// </summary>
        public string MimeType;
        /// <summary>
        /// Normally a full sized WMF or large PNG
        /// </summary>
        public Image Image
        {
            get
            {
                if( rtImage == null )
                    return null;
                else
                    return rtImage.image;
            }
            set
            {
                if( value == null )
                    rtImage = null;
                else if( rtImage == null )
                    rtImage = new RTImage(value);
                else
                    rtImage.image = value;
            }
        }
        private RTImage rtImage = null;
        /// <summary>
        /// A Region map of the page that allows the page to be interacted with in smaller graphical areas
        /// </summary>
        //public Region[] Regions;
        /// <summary>
        /// This is the Private Notes field for notes viewed only on the presentation device.
        /// Property is an object so that it can be of any form (text, html, ink, image) but whatever form this takes, it should
        /// override object.ToString() to give meaninful results to support full text search.
        /// </summary>
        //public object PrivateNotes;
        /// <summary>
        /// This is the Public Notes field for notes that may be viewed in the public workspace.
        /// Property is an object so that it can be of any form (text, html, ink, image) but whatever form this takes, it should
        /// override object.ToString() to give meaninful results to support full text search.
        /// </summary>
        //public object PublicNotes;


        // Should these be part of a 'lecture' object?
        //   possibly many lectures per page?  Or is this an overoptimization

        //  Lecture
        //       or should this be associated with the RTDocument?
        //       Brown wouldn't like that, they see the same document being reused many times in many contexts
        //
        //    Prof Audio Stream SSRC
        //    Prof Video Stream SSRC

        // Transcript (per lecture?  What about Public Notes and Private Notes?)
        // Media linking
        //   Media Time Start
        //   Media Time End

        //  Lecture (new instance of RTDocument
        //     Organzation
        //       Timeline (instead of TOC)
        //         Page
        //           StartTime
        //           EndTime
        //           IdentifierRef
        //     Resources
        //       ProfessorStreams
        //         Audio SSRC
        //         Video SSRC
        //       Resource
        //         Identifier

        public object Extension;

        object ICloneable.Clone()
        {
            Page clone = new Page();

            clone.Identifier = Identifier;
            clone.MimeType = MimeType;
            if (Image != null)
            {
                clone.Image = (Image)Image.Clone();
            }

            if (Extension != null)
            {
                if (Extension is ICloneable)
                {
                    clone.Extension = ((ICloneable)Extension).Clone();
                }
            }

            return clone;
        }

        public override string ToString()
        {
            string retData = "Page " +
                "{ Identifier: " + Identifier.ToString() +
                ", MimeType: " + MimeType;
            if( Image != null )
                retData +=
                ", Image.Height: " + Image.Height +
                ", Image.Width: " + Image.Width;
            if( Extension != null )
                retData +=
                ", Extension.Type: " + Extension.GetType().ToString();
            retData += " }";
            return retData;
        }

    }

    /// <summary>
    /// An Image wrapper that doesn't bloat during serialization
    /// </summary>
    [Serializable]
    public class RTImage : ISerializable
    {
        //
        // P/Invokes
        //
        [DllImport("Gdi32.dll")]
        private static extern uint GetEnhMetaFileBits(IntPtr hMetaFile, uint size, [In,Out]byte[] pData);

        [DllImport("Gdi32.dll")]
        private static extern IntPtr SetEnhMetaFileBits(uint bufferSize, byte[] pData);

        [DllImport("Gdi32.dll")]
        private static extern bool DeleteEnhMetaFile(IntPtr hMetaFile);


        //
        // Instance members & methods
        //
        public Image image;

        public RTImage(Image val)
        {
            this.image = val;
        }

        /// <summary>
        /// Custom deserialzer
        /// </summary>
        RTImage(SerializationInfo info, StreamingContext context)
        {
            byte[] bits = (byte[])info.GetValue("image", typeof(byte[]));
            string type = info.GetString("type");
            if( type == "emf" )
            {
                IntPtr ptr = SetEnhMetaFileBits((uint)bits.Length, bits);
                Debug.Assert(ptr != IntPtr.Zero, "Failed deserialization of image!");
                image = new Metafile(ptr, false);
            }
            else // type == "Jpeg"
            {
                MemoryStream ms = new MemoryStream(bits);
                image = Image.FromStream(ms);
            }
        }

        /// <summary>
        /// Custom serializer
        /// </summary>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            byte[] bits = null;

            if( image.RawFormat.Guid == ImageFormat.Emf.Guid )
            {
                info.AddValue("type", "emf");

                // The image goes bad during serialization, so we have to clone it.
                Metafile mf = (Metafile)((Metafile)image).Clone();
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
                // The info is used in the custom deserializer
                info.AddValue("type", "Jpeg");

                // TODO: Check if an initial size of 100000 bytes is a good choice or not
                // TODO: Why not: System.IO.MemoryStream ms = new System.IO.MemoryStream(0);
                //       so we are sure it also work with jpeg images smaller than 100K
                System.IO.MemoryStream ms = new System.IO.MemoryStream(100000);
                image.Save(ms, ImageFormat.Jpeg);
                bits = ms.ToArray();
            }

            info.AddValue( "image", bits );
        }
    }

    [Serializable]
    public enum ResourceType
    {
        webcontent
    }
}
