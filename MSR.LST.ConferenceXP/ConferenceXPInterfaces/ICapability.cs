using System;
using System.Collections;

// Not using this namespace explicitly due to ambiguous name for PayloadType that is being
// chosen by the compiler from MSR.LST.Net.Rtp instead of MSR.LST.ConferenceXP
//using MSR.LST.Net.Rtp;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// ICapability is the root interface for dealing with data streams at a high level within ConferenceAPI.  A capability has static properties
    /// (PayloadType, Icon), instance properties (Identifier, Name, SSRC, tag), state (IsPlaying, Participant), methods (Stop, -- Play is defined
    /// in derived interfaces), and events (Playing, Stopping).
    /// 
    /// The derived interfaces, ICapabilityViewer and ICapabilitySender, are the interfaces that should be inherited from by classes.  In the case of
    /// ConferenceXP's CapabilitySenders, an additional interface -- IVideoCapabilitySender or IAudioCapabilitySender are used only to help identify
    /// objects by category.
    /// </summary>
    public interface ICapability
    {
        /// <summary>
        /// Descriptive name, human readible, equivalent to an Rtp NAME.
        /// See RFC 1889's NAME definition for detail.
        /// </summary>
        string Name
        {
            get;
            set;
        }

        PayloadType PayloadType
        {
            get;
        }

        bool Channel
        {
            get;
        }

        bool Owned
        {
            get;
        }

        bool UsesSharedForm
        {
            get;
        }
        Guid SharedFormID
        {
            get;
            set;
        }

        Guid ID
        {
            get;
        }

        /// <summary>
        /// Visual representation of this Capability, used by Participant.DecoratedIcon, 32x24 or so
        /// </summary>
        System.Drawing.Image Icon
        {
            get;
        }

        /// <summary>
        /// Generic object holder for use by whatever application is consuming the Venue object.  Generally used to set a relationship between an object
        /// and a UI control that represents that object, like a ListViewItem in a ListView.
        /// </summary>
        object Tag
        {
            get;
            set;
        }

        MSR.LST.Net.Rtp.RtpStream[] RtpStreams
        {
            get;
        }

        IParticipant[] Participants
        {
            get;
        }

        void StreamAdded(MSR.LST.Net.Rtp.RtpStream rtpStream);
        void StreamRemoved(MSR.LST.Net.Rtp.RtpStream rtpStream);
        void Dispose();
    }

    public interface ICapabilitySender : ICapability
    {
        bool IsSending
        {
            get;
        }

        int MaximumBandwidthLimiter
        {
            get;
            set;
        }

        MSR.LST.Net.Rtp.RtpSender RtpSender
        {
            get;
        }

        void Send();
        void StopSending();

        /// <summary>
        /// Note:  We're currently investigating thread safety problems around the Begin* methods
        /// </summary>
        IAsyncResult BeginSend(AsyncCallback callback, object state);
        /// <summary>
        /// Note:  We're currently investigating thread safety problems around the Begin* methods
        /// </summary>
        IAsyncResult BeginStopSending(AsyncCallback callback, object state);

        ICapabilityViewer CapabilityViewer
        {
            get;
        }

    }
    public interface ICapabilityViewer : ICapability
    {
        /// <summary>
        /// If Conference.AutoPlayLocal == true, should ICapabilityViewer.Play() be called after Conference.CapabilityAdded
        /// if Capability.Owner == Conference.LocalParticipant
        /// </summary>
        bool AutoPlayLocal
        {
            get;
        }

        /// <summary>
        /// If Conference.AutoPlayRemote == true, should ICapabilityViewer.Play() be called after Conference.CapabilityAdded
        /// if Capability.Owner != Conference.LocalParticipant
        /// </summary>
        bool AutoPlayRemote
        {
            get;
        }

        /// <summary>
        /// Is the Capability currently active?
        /// </summary>
        bool IsPlaying
        {
            get;
        }

        /// <summary>
        /// What participant is the Owner of, AKA is originating, this Capability?
        /// </summary>
        IParticipant Owner
        {
            get;
            set;
        }

        /// <summary>
        /// Play this Capability.  Show the CapabilityForm if the property has been set
        /// </summary>
        void Play();

        /// <summary>
        /// Stop playing this Capability
        /// </summary>
        void StopPlaying();

        /// <summary>
        /// Note:  We're currently investigating thread safety problems around the Begin* methods
        /// </summary>
        IAsyncResult BeginPlay(AsyncCallback callback, object state);
        /// <summary>
        /// Note:  We're currently investigating thread safety problems around the Begin* methods
        /// </summary>
        IAsyncResult BeginStopPlaying(AsyncCallback callback, object state);
    }

    #region ICapabilityQuality
    /// <summary>
    /// Can the Quality of the Capability be adjusted?  Used for setting things such as frame rates and compression ratios at a high level.
    /// </summary>
    public interface ICapabilityQuality
    {
        Quality Quality
        {
            get;
            set;
        }
    }


    /// <summary>
    /// High level enums for Quality, meaning to be interpreted on a per Capability basis.
    /// </summary>
    public enum Quality
    {
        VeryLow,
        Low,
        Medium,
        MediumHigh,
        High,
        Custom
    }
    #endregion
    #region ICapabilityWindow
    /// <summary>
    /// Implemented to support simple windowing symantecs if the Capability displays a Window upon Play.
    /// </summary>
    public interface ICapabilityWindow   // modeled on DShow IVideoWindow, simple version
    {
        string Caption { get; set; }    // Get/Set the window caption
        int Height { get; set; }
        int Left { get; set; }
        int Top { get; set; }
        bool Visible { get; set; }
        int Width { get; set; }
        System.Drawing.Rectangle Rectangle { get; set; }
        System.Drawing.Size Size { set; }
        System.Drawing.Point Location { set; }
    }

    /// <summary>
    /// Extended window symantecs to offer more control such as setting the Owner or parent window.
    /// </summary>
    public interface ICapabilityWindowExtended  : ICapabilityWindow // modeled on DShow IVideoWindow, optional attributes for more control
    {
        int BorderColor { get; set; }
        bool CursorHidden { get; set; }
        int WindowOwner { get; set; }
        int WindowState { get; set; }
        int WindowStyle { get; set; }
    }

    //TODO: Follow standards for bitflag enums
    public enum WindowState  // modeled on DShow IVideoWindow, optional attributes for more control
    {
        Hide,
        Show,
        Maximized,
        Minimized
    }

    //TODO: Follow standards for bitflag enums
    public enum WindowStyle  // modeled on DShow IVideoWindow, optional attributes for more control
    {
        Border, // AKA ThinFrame AKA not resizable
        Caption,
        Disabled,
        Dialog,
        HorizontalScroll,
        MaximizeBox,
        MinimizeBox,
        ThickFrame, // AKA Resizable
        VerticalScroll
    }

    #endregion

    /// <summary>
    /// When this interface is inherited by a class, the window will be autosized in
    /// addition to being autopositionned
    /// </summary>
    public interface ICapabilityWindowAutoSize {} 
    public class ICapabilityHashtable : DictionaryBase
    {
        public ICapabilityHashtable(int length) : base() {}
        public ICapabilityHashtable() : base() {}
        
        public ICollection Keys  
        {
            get  
            {
                return( Dictionary.Keys );
            }
        }

        public ICollection Values  
        {
            get  
            {
                return( Dictionary.Values );
            }
        }

        public object Clone()
        {
            ICapabilityHashtable iCHT = new ICapabilityHashtable();

            foreach( DictionaryEntry de in Dictionary )
                iCHT.Add((Guid)de.Key, (ICapability)de.Value);

            return iCHT;
        }

        public ICapability this [ Guid capabilityKey ]
        {
            get
            {
                return (ICapability) Dictionary[capabilityKey];
            }
            set
            {
                if ( value.GetType() != Type.GetType("ICapability") )
                {
                    throw new ArgumentException(Strings.ValueMustBeICapability, Strings.Value);
                }

                Dictionary[capabilityKey] = value;
            }
        }

        public ICapability this [ int index ]
        {
            get
            {
                ArrayList al = new ArrayList(Dictionary.Values);
                return (ICapability)al[index];
            }
            set
            {
                ArrayList al = new ArrayList(Dictionary.Values);
                if (al.Count < index)
                {
                    throw new IndexOutOfRangeException();
                }
                if (index < 0)
                {
                    throw new IndexOutOfRangeException();
                }
                al[index] = value;
            }
        }

        public void Add ( Guid capabilityKey, ICapability iCapability )
        {
            Dictionary.Add( capabilityKey, iCapability );
        }

        public bool Contains ( Guid capabilityKey )
        {
            return Dictionary.Contains ( capabilityKey );
        }

        public bool ContainsKey ( Guid capabilityKey )
        {
            return Dictionary.Contains ( capabilityKey );
        }

        public void Remove ( Guid capabilityKey )
        {
            Dictionary.Remove ( capabilityKey );
        }
    }

    #region Public Enums
    public enum PayloadType : byte
    {
        Unknown = 0,
        Chat = 96,
        xApplication2,
        xApplication3,
        xApplication4,
        xApplication5,
        xApplication6,
        xApplication7,
        xApplication8,
        xApplication9,
        xApplication10,
        Venue1 = 106,
        Venue2,
        Venue3,
        Venue4,
        Venue5,
        Venue6,
        Venue7,
        Venue8,
        Venue9,
        GroupChat = 115,
        FileTransfer = 116,
        ManagedDirectX = 117,
        Whiteboard = 118,
        SharedBrowser = 119,
        RichTextChat = 120,
        RTDocument = 121,               // Serialization of an RTDocument object, network protocol TBD
        PipecleanerSignal = 122,        // Diagnostic signal used by the Pipecleaner applications to test connectivity between nodes
        Test = 123,                     // Used for test cases
        FEC = 124,                      // Identifies a packet as containing Forward Error Correction information
        dynamicPresentation = 125,      // Obsolete, being replaced by RTDocument -- lifetime TBD
        dynamicVideo = 126,             // A video signal.  The format of the video signal is embedded in the data stream itself
        dynamicAudio = 127              // An audio signal.  The format of the audio signal is embedded in the data stream itself
    }

    /// <summary>
    /// SharedFormType enum used for form sharing (see Capability class)
    /// </summary>
    /// <remarks>
    /// Only SharedFormType.None and SharedFormType.CNAME are implemented yet.
    /// </remarks>
    public enum SharedFormType
    {
        None,      // No form sharing, default
        CName,     // Sharing on CName: typically useful to have audio and video of a participant 
                   // in a single form 
        StreamType // Sharing on Stream Payload type: typically useful to have all the 
                   // audio control in a single form
    }

    #endregion Public Enums

    #region EventArgs & Delegates
    public class ObjectReceivedEventArgs : EventArgs
    {
        public object Data = null;
        public IParticipant Participant = null;

        public ObjectReceivedEventArgs ( IParticipant participant, object data )
        {
            Data = data;
            Participant = participant;
        }
    }

    public class BytesReceivedEventArgs : EventArgs
    {
        public byte[] Data = null;
        public IParticipant Participant = null;

        public BytesReceivedEventArgs ( IParticipant participant, byte[] data )
        {
            Data = data;
            Participant = participant;
        }
    }

    public class CapabilityEventArgs : EventArgs
    {
        public ICapability Capability = null;

        public CapabilityEventArgs ( ICapability capability )
        {
            Capability = capability;
        }
    }

    public class ParticipantEventArgs : EventArgs
    {
        public IParticipant Participant = null;

        public ParticipantEventArgs ( IParticipant participant )
        {
            Participant = participant;
        }
    }

    // Capability events
    public delegate void CapabilityAddedEventHandler                ( object conference, CapabilityEventArgs ea );
    public delegate void CapabilityRemovedEventHandler              ( object conference, CapabilityEventArgs ea );
    public delegate void CapabilityTimeoutEventHandler              ( object conference, CapabilityEventArgs ea );

    public delegate void CapabilityPlayingEventHandler              ( object conference, CapabilityEventArgs ea );
    public delegate void CapabilityStoppedPlayingEventHandler       ( object conference, CapabilityEventArgs ea );

    public delegate void CapabilitySendingEventHandler              ( object conference, CapabilityEventArgs ea );
    public delegate void CapabilityStoppedSendingEventHandler       ( object conference, CapabilityEventArgs ea );

    //Pri2: Should change to object conference and create a CapabilityParticipantEventArgs that has both Capability and Participant
    public delegate void CapabilityParticipantAddedEventHandler     ( object capability, ParticipantEventArgs ea );
    public delegate void CapabilityParticipantRemovedEventHandler   ( object capability, ParticipantEventArgs ea );


    //Pri2: This is on the capability object instance rather than on Conference...
    public delegate void CapabilityObjectReceivedEventHandler       ( object capability, ObjectReceivedEventArgs ea );

    #endregion
}
