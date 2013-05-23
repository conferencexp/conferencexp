using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Resources;

using MSR.LST.Net.Rtp;
using System.Collections.Generic;


namespace MSR.LST.ConferenceXP
{

    public class Participant : IParticipant
    {
        #region Constructors
        /// <summary>
        /// Construct a participant from a VenueService.Participant
        /// </summary>
        /// <param name="participant">VenueService.Participant</param>
        /// <param name="venue"></param>
        internal Participant(VenueService.Participant participant)
        {
            identifier = participant.Identifier;
            name = participant.Name;
            email = participant.Email;
            icon = Utilities.ByteToBitmap(participant.Icon);
            role = ParticipantRole.Other;
            ThroughputWarnings = new List<ThroughputWarning>();
        }

        /// <summary>
        /// This constructor is for the special case of a participant that is determined to be missing
        /// by the use of Diagnostics Service backchannel communications.
        /// </summary>
        /// <param name="cname"></param>
        public Participant(string cname) {
            identifier = cname;
            name = "Missing Participant: " + cname;
            email = cname;
            icon = null;
            role = ParticipantRole.Other;
            missingParticipant = true;
            ThroughputWarnings = new List<ThroughputWarning>();
        }

        #endregion
        #region Persistent Public Properties
        /// <summary>
        /// A unique identifier for the participant, like their email address, same definition as Rtp CNAME
        /// See RFC 1889 CNAME for details.
        /// </summary>
        public string Identifier
        {
            get
            {
                return identifier;
            }
        }
        /// <summary>
        /// Friendly name, same definition as Rtp NAME.
        /// See RFC 1889 NAME for details.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }
        /// <summary>
        /// Same definition as Rtp PHONE.
        /// See RFC 1889 PHONE for details.
        /// </summary>
        public string Phone
        {
            get
            {
                return phone;
            }
        }
        /// <summary>
        /// Same definition as Rtp EMAIL.
        /// See RFC 1889 EMAIL for details.
        /// </summary>
        public string Email
        {
            get
            {
                return email;
            }
        }
        /// <summary>
        /// 96x96 image that represents the participant.  Image is reformated to PNG and resized upon submission to the Venue Service.
        /// </summary>
        public Image Icon
        {
            get
            {
                if (icon != null)
                    return icon;
                else
                {
                    System.IO.Stream streamNullIcon;
                    if (missingParticipant) {
                        streamNullIcon = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.MissingParticipantIcon.png");
                    }
                    else {
                        streamNullIcon = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.GenericParticipantIcon.png");
                    }
                    Bitmap bm = new Bitmap(streamNullIcon);
                    return Utilities.GenerateThumbnail(bm);
                }
            }
        }

        public bool MissingParticipant {
            get { return missingParticipant; }
        }

        #endregion
        #region Non-persistent Public Properties
        /// <summary>
        /// The Participant's Icon, but overlayed with graphical representations of the CapabilityViewers being sent by this Participant.
        /// Useful to provide graphical feedback to the user on who is sending what data.
        /// </summary>
        public Image DecoratedIcon
        {
            get
            {
                return Utilities.CreateDecoratedParticipantImage(this);
            }
        }
        /// <summary>
        /// What Role does the Participant play?  Used to allow a UI to programmatically distinguish between different Participants.
        /// </summary>
        public ParticipantRole Role
        {
            get
            {
                return role;
            }
        }
        /// <summary>
        /// Generic object holder for use by whatever application is consuming the Venue object.  Generally used to set a relationship between an object
        /// and a UI control that represents that object, like a ListViewItem in a ListView.
        /// </summary>
        public object Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
        }
        /// <summary>
        /// What CapabilityViewers are associated to this Participant.  A CapabilityViewer is the viewing object created for each stream
        /// of data sent by a Participant.
        /// </summary>
        public ICapability[] Capabilities
        {
            get
            {
                if (capabilityViewers.Count == 0)
                {
                    return new ICapability[0];
                }
                else
                {
                    ArrayList al = new ArrayList(capabilityViewers.Values);
                    return (ICapability[])al.ToArray(typeof(ICapability));
                }
            }
        }

        public RtpParticipant RtpParticipant
        {
            get
            {
                if (Conference.ActiveVenue == null)
                {
                    return null;
                }
                else
                {
                    return Conference.RtpSession.Participant(identifier);
                }
            }
        }


        public List<ThroughputWarning> ThroughputWarnings;

        public class ThroughputWarning {
            public string OtherParticipant;
            public ThroughputWarningType WarningType;

            public ThroughputWarning(ThroughputWarningType warningType, string otherParticipantCname) {
                WarningType = warningType;
                OtherParticipant = otherParticipantCname;
            }

            public override bool Equals(object otherObj) {
                if (otherObj is ThroughputWarning) {
                    if ((WarningType.Equals(((ThroughputWarning)otherObj).WarningType)) &&
                        (OtherParticipant.Equals(((ThroughputWarning)otherObj).OtherParticipant))) {
                        return true;
                    }
                }
                return false;
            }

            public override int GetHashCode() {
                return base.GetHashCode();
            }
        }

        public enum ThroughputWarningType { 
            Inbound,
            Outbound
        }

        #endregion
        #region Public Methods
        /// <summary>
        /// Add a new CapabilityViewer to CapabilityViewers[]
        /// </summary>
        /// <param name="iCV"></param>
        public void AddCapabilityViewer(ICapability iCV)
        {
            capabilityViewers.Add(iCV.ID, iCV);
        }
        /// <summary>
        /// Remove a CapabilityVeiwer from CapabilityViewers[]
        /// </summary>
        /// <param name="iCV"></param>
        public void RemoveCapabilityViewer(ICapability iCV)
        {
            capabilityViewers.Remove(iCV.ID);
        }

        public void AddThroughputWarning(ThroughputWarning warning) {
            if (ThroughputWarnings == null) {
                ThroughputWarnings = new List<ThroughputWarning>();
            }
            if (!ThroughputWarnings.Contains(warning)) {
                ThroughputWarnings.Add(warning);
            }
        }

        public void RemoveThroughputWarning(ThroughputWarning warning) {
            if ((ThroughputWarnings != null) &&
                (ThroughputWarnings.Contains(warning))) {
                ThroughputWarnings.Remove(warning);
            }
        }

        #endregion
        #region Private Properties
        private string identifier = null;
        private string name = null;
        private string phone = null;
        private string email = null;
        private ParticipantRole role = ParticipantRole.Other;
        private Image icon = null;
        internal ICapabilityHashtable capabilityViewers = new ICapabilityHashtable();
        private object tag = null;
        private bool missingParticipant = false;
        #endregion
        #region Overrides
        public override int GetHashCode()
        {
            return identifier.GetHashCode();
        }
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, 
                "Participant [ Identifier == {0}, CapabilityViewers.Count = {1} ]", identifier, 
                capabilityViewers.Count);
        }
        #endregion
    }
}
