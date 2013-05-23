using System;
using System.Collections;
using System.Drawing;
using System.Net;

using MSR.LST.Net.Rtp;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// A Public Venue uses a multicast IP address that is routable throughout the Internet and be used to conference between organizations
    /// provided multicast connectivity exists and there are no firewalls blocking traffic.
    /// 
    /// A Private Venue uses a multicast IP address that is in the non-routable address space (233/24) and therefore should not leave the local
    /// network.  Note that this privacy is not guaranteed because cases have been found where an organization's "edge router" was configured to
    /// forward these restricted addresses.
    /// </summary>
    public enum VenueType
    {
        PrivateVenue,
        PublicVenue,
        Custom,
        Invalid
    }

    /// <summary>
    /// A Venue is a 'Virtual Meetingplace', or 'Virtual Conference Room'.  It consists of a series of persistent properties (Identifier, Name, EndPoint,
    /// Type, and Icon) stored in a database in the Venue Service, a non-persistent 'helper' property (Tag), a set of non-persistent state about who
    /// is in the Venue (Participants) and data available to be viewed (CapabilityViewers).
    /// 
    /// Additional server side functionality is available on the venue, such as an Access Control List (ACL) which determines who can see each Venue.
    /// This functionality is applied when the Venue Service is queried for Venue[], so when a Venue object is passed down to the client, the ACL
    /// matching has already been run.
    /// </summary>
    /// <remarks>
    /// (pfb on 25-Oct-04): Due to removing the Venue.Join() and Leave() logic from the Venue item itself (moved to Conference), there is really
    /// no difference between IVenue and VenueData.  This ambiguity will need to be resolved & cleaned at a later date.
    /// </remarks>
    public interface IVenue
    {
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
        /// Multicast IPEndPoint used by the Venue
        /// </summary>
        IPEndPoint EndPoint
        {
            get;
        }

        ushort TTL
        {
            get;
        }
        /// <summary>
        /// Public or Private?  Could be programmatically determined
        /// </summary>
        VenueType Type
        {
            get;
        }
        /// <summary>
        /// 96x96 Image that represents the Venue, programmatically resized by the Venue Service Administration Tool before it's uploaded to the server.
        /// </summary>
        Image Icon
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

        VenueData VenueData
        {
            get;
        }
    }


    [Serializable]
    public struct VenueData
    {
        public string Name;
        public IPEndPoint IPEndPoint;
        public ushort TTL;
        public VenueType VenueType;
        public Image Icon;
        public IPEndPoint ReflectorService;
        public IPEndPoint ArchiveService;

        public VenueData( string name, IPEndPoint ipEndPoint, ushort ttl, VenueType venueType, Image icon )
        {
            Name = name;
            IPEndPoint = ipEndPoint;
            TTL = ttl;
            VenueType = venueType;
            Icon = icon;
            ReflectorService = null;
            ArchiveService = null;
        }
        public VenueData( string name, IPEndPoint ipEndPoint, ushort ttl, VenueType venueType, Image icon, IPEndPoint reflectorService, IPEndPoint archiveService )
        {
            Name = name;
            IPEndPoint = ipEndPoint;
            TTL = ttl;
            VenueType = venueType;
            Icon = icon;
            ReflectorService = reflectorService;
            ArchiveService = archiveService;
        }

        public override string ToString()
        {
            return "VenueData {" +
                " Name := " + Name +
                " IPEndPoint := " + IPEndPoint +
                " TTL := " + TTL +
                " VenueType := " + VenueType +
                " Icon := " + Icon +
                " ReflectorService := " + ReflectorService +
                " ArchiveService := " + ArchiveService +
                " }";
        }
    }
}
