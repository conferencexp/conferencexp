using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Microsoft.Ink;


namespace MSR.LST.RTDocuments 
{
    /// <summary>
    /// Common ConferenceXP ink specification
    /// </summary>
    [Serializable]
    public class RTStroke : ISerializable
    {
        public readonly static Guid ExtendedPropertyStrokeIdentifier = new Guid("{179222D6-BCC1-4570-8D8F-7E8834C1DD2A}");

        private Ink ink = null;
        private byte[] inkData = null;
        
        [NonSerialized()]
        private Guid strokeIdentifier = Guid.Empty;

        /// <summary>
        /// The ID of the document where this ink was created.  By default, it is set to Guid.Empty and the current document
        /// of the Capability channel is assumed.
        /// </summary>
        public Guid DocumentIdentifier = Guid.Empty;
        
        /// <summary>
        /// Page where this stroke resides.  This is set by default to the current page identifier as determined from RTObjectStatics
        /// <summary>
        public Guid PageIdentifier = Guid.Empty;

        /// <summary>
        /// This is an object you can stick additional information into to store your own extensions to this structure.
        /// 
        /// Note: Object was used rather than Hashtable to be consistent with the rest of the Extension tags contained in
        /// RTDocument.  The Object can always contain a name/value pair hashtable.
        /// <summary>
        public object Extension = null;

        /// <summary>
        /// The identifier of the stroke. Unique and persistent to each stroke.  This is automatically set upon construction.
        /// </summary>
        public Guid StrokeIdentifier
        {
            get
            {
                if (strokeIdentifier != Guid.Empty)
                {
                    return strokeIdentifier;
                }

                if (Stroke == null)
                {
                    return Guid.Empty;
                }

                if (this.Stroke.ExtendedProperties.DoesPropertyExist(ExtendedPropertyStrokeIdentifier))
                {
                    return new Guid((string)Stroke.ExtendedProperties[ExtendedPropertyStrokeIdentifier].Data);
                } 
                else
                {
                    return Guid.Empty;
                }

            }
        }

        public Stroke Stroke
        {
            get
            {
                if (ink == null)
                {
                    if (inkData == null)
                    {
                        return null;
                    }

                    ink = new Ink();
                    ink.Load(inkData);
                }

                if (ink.Strokes.Count == 0)
                {
                    return null;
                }
                
                Trace.Assert(ink.Strokes.Count == 1);

                checkStrokeForStrokeIdentifier(ink.Strokes[0]);

                return ink.Strokes[0];
            }
        }

        public Strokes Strokes
        {
            get
            {
                if (ink == null)
                {
                    if (inkData == null)
                    {
                        return null;
                    }

                    ink = new Ink();
                    ink.Load(inkData);
                }

                if (ink.Strokes.Count == 0)
                {
                    return null;
                }
                
                Trace.Assert(ink.Strokes.Count == 1);

                checkStrokeForStrokeIdentifier(ink.Strokes[0]);

                return ink.Strokes;
            }
        }

        #region Custom Serialization code
        protected RTStroke(SerializationInfo info, StreamingContext context) 
        {
            ink = new Ink();
            ink.Load((byte[])info.GetValue("Ink", typeof(byte[])));
            DocumentIdentifier = new Guid(info.GetString("DocumentIdentifier"));
            PageIdentifier = new Guid(info.GetString("PageIdentifier"));
            Extension = info.GetValue("Extension", typeof(object));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) 
        {
            info.AddValue("Ink", InkData);
            info.AddValue("DocumentIdentifier", DocumentIdentifier.ToString());
            info.AddValue("PageIdentifier", PageIdentifier.ToString());
            info.AddValue("Extension", Extension);
        }
        #endregion

        public RTStroke(Guid documentIdentifier, Guid pageIdentifier, Stroke stroke, object extension ) 
        {
            DocumentIdentifier = documentIdentifier;
            PageIdentifier = pageIdentifier;
            Extension = extension;

            if (! stroke.ExtendedProperties.DoesPropertyExist(ExtendedPropertyStrokeIdentifier))
            {
                stroke.ExtendedProperties.Add(ExtendedPropertyStrokeIdentifier, Guid.NewGuid().ToString());
            }
            
            int[] strokeIds = new int[1];
            strokeIds[0] = stroke.Id;
            Strokes ourStroke = stroke.Ink.CreateStrokes(strokeIds);

            Ink fromInk = stroke.Ink;
            ink = fromInk.ExtractStrokes(ourStroke, ExtractFlags.CopyFromOriginal);
        }

        /// <remarks>
        /// Remarkable Texts constructor, which creates an RTStroke by pulling the byte[] from a stroke that was stored in the DB to send over the wire
        /// </remarks>
        public RTStroke(Guid documentIdentifier, Guid pageIdentifier, Guid strokeIdentifier, byte[] inkData, object extension )
        {
            DocumentIdentifier = documentIdentifier;
            PageIdentifier = pageIdentifier;
            Extension = extension;

            this.strokeIdentifier = strokeIdentifier;
            this.inkData = inkData;
        }

        private void checkStrokeForStrokeIdentifier(Stroke stroke)
        {
            // We store the StrokeIdentifier in the stroke object itself so it can be used as a lookup later for
            // Move or Removed commands by examining the Ink.Strokes objects directly without needing to have a separate
            // lookup hashtable just for this functionality.
            if (! stroke.ExtendedProperties.DoesPropertyExist(ExtendedPropertyStrokeIdentifier))
            {
                if (strokeIdentifier != Guid.Empty)
                {
                    stroke.ExtendedProperties.Add(ExtendedPropertyStrokeIdentifier, strokeIdentifier.ToString());
                }
                else
                {
                    throw new Exception(Strings.StrokeFoundWithNoStrokeIdentifierSet);
                }
            }
        }

        public byte[] InkData
        {
            get
            {
                if (inkData == null)
                {
                    if (ink == null)
                    {
                        return null;
                    }

                    inkData = ink.Save(PersistenceFormat.InkSerializedFormat);
                }
                return inkData;
            }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, 
                "RTStroke [ DocumentIdentifier: {0}, PageIdentifier: {1}, StrokeIdentifier: {2}, InkData Size: {3} ]", 
                DocumentIdentifier.ToString(), PageIdentifier.ToString(), StrokeIdentifier.ToString(), 
                InkData.Length);
        }
    }

    [Serializable]
    public class RTStrokeAdd : RTStroke, ISerializable
    {
        /// <summary>
        /// This is the time the stroke was created. You only need to set this field if you resend strokes (rather than only sending newly created strokes).
        /// </summary>
        public DateTime CreationTime;

        /// <summary>
        /// Has the stroke been completed? (ie, there will be no more additions.)  This defaults to true.
        /// </summary>
        public bool StrokeFinished = true;

        #region Custom Serialization code
        protected RTStrokeAdd( SerializationInfo info, StreamingContext context ) : base( info, context )
        {
            CreationTime = info.GetDateTime("CreationTime");
        }

        public override void GetObjectData( SerializationInfo info, StreamingContext context )
        {
            info.AddValue("CreationTime", CreationTime);
            base.GetObjectData ( info, context );
        }
        #endregion

        public RTStrokeAdd( DateTime creationTime, Guid documentIdentifier, Guid pageIdentifier, bool strokeFinished, Stroke stroke, object extension )
            : base( documentIdentifier, pageIdentifier, stroke, extension )
        {
            StrokeFinished = strokeFinished;
            CreationTime = creationTime;
        }

        /// <remarks>
        /// Remarkable Texts Constructor
        /// </remarks>
        public RTStrokeAdd( DateTime creationTime, Guid documentIdentifier, Guid pageIdentifier, bool strokeFinished, Guid strokeIdentifier, byte[] inkData, object extension )
            : base( documentIdentifier, pageIdentifier, strokeIdentifier, inkData, extension )
        {
            CreationTime = creationTime;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, 
                "RTStrokeAdd [ DocumentIdentifier: {0}, PageIdentifier: {1}, StrokeIdentifier: {2}, StrokeFinished: {3}, InkData Size: {4} ]", 
                DocumentIdentifier.ToString(), PageIdentifier.ToString(), StrokeIdentifier.ToString(), 
                StrokeFinished.ToString(), InkData.Length);
        }
    }

    /// <summary>
    /// </summary>
    /// <remarks>
    /// There can be multiple removals for a single stroke, if it is deleted simultaneously by different people.
    /// </remarks>
    [Serializable]
    public class RTStrokeRemove
    {
        public int      SequenceNumber;
        public DateTime DeletionTime;
        /// <summary>
        /// </summary>
        /// <remarks>
        /// This is the StrokeIdentifier of the original RTStrokeAdd, not a new identifier generated for this removal.
        /// </remarks>
        public Guid StrokeIdentifier;
        public object Extension = null;

        /// <summary>
        /// The ID of the document where this ink was created.  By default, it is set to Guid.Empty and the current document
        /// of the Capability channel is assumed.
        /// </summary>
        public Guid     DocumentIdentifier = Guid.Empty;
        /// <summary>
        /// Page where this stroke resides.  This is set by default to the current page identifier as determined from RTObjectStatics
        /// <summary>
        public Guid     PageIdentifier = Guid.Empty;

        /// <remarks>
        /// If an app stores the RTStroke, it can remove it easily using this ctor
        /// </remarks>
        public RTStrokeRemove ( DateTime deletionTime, RTStroke rtStroke, object extension )
        {
            DeletionTime = deletionTime;
            DocumentIdentifier = rtStroke.DocumentIdentifier;
            PageIdentifier = rtStroke.PageIdentifier;
            StrokeIdentifier = rtStroke.StrokeIdentifier;
            Extension = extension;
        }

        public RTStrokeRemove ( DateTime deletionTime, Guid documentIdentifier, Guid pageIdentifier, Guid strokeIdentifier, object extension )
        {
            DeletionTime = deletionTime;
            DocumentIdentifier = documentIdentifier;
            PageIdentifier = pageIdentifier;
            StrokeIdentifier = strokeIdentifier;
            Extension = extension;
        }

        public RTStrokeRemove ( DateTime deletionTime, Guid documentIdentifier, Guid pageIdentifier, Stroke stroke, object extension )
        {
            if (! stroke.ExtendedProperties.DoesPropertyExist(RTStroke.ExtendedPropertyStrokeIdentifier))
            {
                throw new ArgumentException(Strings.StrokeDoesNotContainAnExtendedProperty);
            }

            DeletionTime = deletionTime;
            DocumentIdentifier = documentIdentifier;
            PageIdentifier = pageIdentifier;
            StrokeIdentifier = new Guid((string)stroke.ExtendedProperties[RTStroke.ExtendedPropertyStrokeIdentifier].Data);
            Extension = extension;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, 
                "RTStrokeRemove [ DocumentIdentifier: {0}, PageIdentifier: {1}, StrokeIdentifier: {2}, SequenceNumber: {3} ]", 
                DocumentIdentifier.ToString(), PageIdentifier.ToString(), StrokeIdentifier.ToString(), 
                SequenceNumber);
        }
    }
}
