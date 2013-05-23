using System;
using System.Collections;
using System.Drawing;
using System.Net;

using MSR.LST.Net.Rtp;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// What Role does the Participant play?  Used to allow a UI to programmatically distinguish between different Participants.
    /// </summary>
    public enum ParticipantRole
    {
        Professor,
        TA,
        Student,
        Other,
        Unknown,
    }

    /// <summary>
    /// A Participant represents a person who is participating in a conference in a Venue.  A Participant has persistent properties (Identifier, Name
    /// Phone, Email, and Icon) that are stored in a database, accessible via web services from the Venue Service, and rarely updated; non-persistent
    /// state that can change based on context (Tag, Role, DecoratedIcon, CapabilityViewers, and Venue); and methods (AddCapabilityViewer,
    /// RemoveCapabilityViewer) used to maintain the state.
    /// </summary>
    public interface IParticipant
    {
        #region Persistent Properties
        /// <summary>
        /// A unique identifier like email address, same definition as Rtp CNAME.
        /// See RFC 1889 CNAME for details.
        /// </summary>
        string Identifier
        {
            get;
        }
        /// <summary>
        /// Friendly name, same definition as Rtp NAME.
        /// See RFC 1889 NAME for details.
        /// </summary>
        string Name
        {
            get;
        }
        /// <summary>
        /// Same definition as Rtp PHONE.
        /// See RFC 1889 PHONE for details.
        /// </summary>
        string Phone
        {
            get;
        }
        /// <summary>
        /// Same definition as Rtp EMAIL.
        /// See RFC 1889 EMAIL for details.
        /// </summary>
        string Email
        {
            get;
        }
        /// <summary>
        /// 96x96 image that represents the participant.  Image is reformated to PNG and resized upon submission to the Venue Service.
        /// </summary>
        Image Icon
        {
            get;
        }
        #endregion
        #region Non-persistent Properties
        /// <summary>
        /// Generic object holder for use by whatever application is consuming the Venue object.  Generally used to set a relationship between an object
        /// and a UI control that represents that object, like a ListViewItem in a ListView.
        /// </summary>
        object Tag
        {
            get;
            set;
        }
        /// <summary>
        /// What Role does the Participant play?  Used to allow a UI to programmatically distinguish between different Participants.
        /// </summary>
        ParticipantRole Role
        {
            get;
        }
        /// <summary>
        /// The Participant's Icon, but overlayed with graphical representations of the CapabilityViewers being sent by this Participant.
        /// Useful to provide graphical feedback to the user on who is sending what data.
        /// </summary>
        Image DecoratedIcon
        {
            get;
        }
        /// <summary>
        /// What Capabilities are associated to this Participant.  A Capability is the viewing object created for each stream
        /// of data sent by a Participant.
        /// </summary>
        ICapability[] Capabilities
        {
            get;
        }
        RtpParticipant RtpParticipant
        {
            get;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Add a new CapabilityViewer to CapabilityViewers[]
        /// </summary>
        /// <param name="iCV"></param>
        void AddCapabilityViewer(ICapability iCV);
        /// <summary>
        /// Remove a CapabilityVeiwer from CapabilityViewers[]
        /// </summary>
        /// <param name="iCV"></param>
        void RemoveCapabilityViewer(ICapability iCV);
        #endregion
    }

    public class IParticipantHashtable : DictionaryBase
    {
        public IParticipantHashtable(int length) : base() {}
        public IParticipantHashtable() : base() {}
        
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
            IParticipantHashtable iPHT = new IParticipantHashtable();

            foreach( DictionaryEntry de in Dictionary )
                iPHT.Add((string)de.Key, (IParticipant)de.Value);

            return iPHT;
        }

        public IParticipant this [ string identifier ]
        {
            get
            {
                return (IParticipant) Dictionary[identifier];
            }
            set
            {
                Dictionary[identifier] = value;
            }
        }

        public void Add ( string identifier, IParticipant iParticipant )
        {
            Dictionary.Add( identifier, iParticipant );
        }

        public bool Contains ( string identifier )
        {
            return Dictionary.Contains ( identifier );
        }

        public bool ContainsKey ( string identifier )
        {
            return Dictionary.Contains ( identifier );
        }

        public void Remove ( string identifier )
        {
            Dictionary.Remove ( identifier );
        }
    }
}
