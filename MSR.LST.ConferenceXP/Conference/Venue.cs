using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

using MSR.LST;
using MSR.LST.Net;
using MSR.LST.Net.Rtp;
using MSR.LST.ConferenceXP.VenueService;

//Pri2: Move TTL to the Venue definition and inside VenueData


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// A Venue is a 'Virtual Meetingplace', or 'Virtual Conference Room'.  It consists of a series of persistent properties (Identifier, Name, EndPoint,
    /// Type, and Icon) stored in a database in the Venue Service, a non-persistent 'helper' property (Tag), a set of non-persistent state about who
    /// is in the Venue (Participants) and data available to be viewed (CapabilityViewers).
    /// 
    /// A Venue supports two simple methods, Join and Leave.  When not Joined to a venue, the venue state (Participants and CapabilityViewers) is empty.
    /// 
    /// Additional server side functionality is available on the venue, such as an Access Control List (ACL) which determines who can see each Venue.
    /// This functionality is applied when the Venue Service is queried for Venue[], so when a Venue object is passed down to the client, the ACL
    /// matching has already been run.
    /// </summary>
    public class Venue : IVenue
    {
        #region Constructors
        internal Venue(VenueService.Venue v)
        {
            identifier = v.Identifier;
            name = v.Name;
            icon = Utilities.ByteToBitmap(v.Icon);

            // Trim the IPAddress, because whitespace isn't handled in .NET 2.0, but was in .NET 1.1
            string trimmedIP = v.IPAddress.Trim();
            endPoint = new IPEndPoint(IPAddress.Parse(trimmedIP), v.Port);
            pwStatus = v.PWStatus;
            compatibilityVenueType = v.VenueType;
        }
        internal Venue( string name, IPEndPoint endPoint, VenueType venueType, Image icon)
        {
            identifier = null;
            this.name = name;
            this.endPoint = endPoint;
            this.type = venueType;
            this.icon = icon;
        }
        internal Venue (VenueData venueData)
        {
            identifier = null;
            this.name = venueData.Name;
            this.endPoint = venueData.IPEndPoint;
            this.ttl = venueData.TTL;
            this.type = venueData.VenueType;
            this.icon = venueData.Icon;
        }
        #endregion
        #region Public Properties
        /// <summary>
        /// A unique identifier like email address, same definition as Rtp CNAME.
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
        /// Multicast IPEndPoint used by the Venue
        /// </summary>
        public IPEndPoint EndPoint
        {
            get
            {
                return endPoint;
            }
        }
        public ushort TTL
        {
            get
            {
                return ttl;
            }
        }
        /// <summary>
        /// Public or Private?  Could be programmatically determined
        /// </summary>
        public VenueType Type
        {
            get
            {
                return type;
            }
        }
        /// <summary>
        /// 96x96 Image that represents the Venue, programmatically resized by the Venue Service Administration Tool before it's uploaded to the server.
        /// </summary>
        public Image Icon
        {
            get
            {
                if (icon != null)
                    return icon;
                else
                {
                    System.IO.Stream streamNullIcon = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.GenericVenueIcon.png");
                    Bitmap bm = new Bitmap(streamNullIcon);
                    return Utilities.GenerateThumbnail(bm);
                }
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

        public VenueData VenueData
        {
            get
            {
                return new VenueData(name, endPoint, ttl, type, icon );
            }
        }

        public PasswordStatus PWStatus
        {
            get { return pwStatus; }
        }

        public MSR.LST.ConferenceXP.VenueService.VenueType CompatibilityVenueType {
            get { return compatibilityVenueType; }
        }

        #endregion

        #region Private Properties
        private string identifier = null;
        private string name = null;
        private IPEndPoint endPoint = null;
        private ushort ttl = 128;
        private VenueType type = VenueType.PrivateVenue;
        private Image icon = null;

        private object tag = null;

        private PasswordStatus pwStatus;
        private MSR.LST.ConferenceXP.VenueService.VenueType compatibilityVenueType;
   

        #endregion
    }

    /// <summary>
    /// A typed hashtable with ( Venue.Name, Venue ) pairs
    /// </summary>
    public class VenueHashtable : DictionaryBase
    {

        public VenueHashtable(int length) : base() {}
        public VenueHashtable() : base() {}
        
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

        public Venue this [ string venueName ]
        {
            get
            {
                return (Venue) Dictionary[venueName];
            }
            set
            {
                Dictionary[venueName] = value;
            }
        }

        public void Add ( string venueName, Venue venue )
        {
            Dictionary.Add( venueName, venue );
        }

        public bool Contains ( string venueName )
        {
            return Dictionary.Contains ( venueName );
        }

        public bool ContainsKey ( string venueName )
        {
            return Dictionary.Contains ( venueName );
        }

        public void Remove ( string venueName )
        {
            Dictionary.Remove ( venueName );
        }

    }

    [Serializable]
    public class VenueList : ICollection, IEnumerable, ISerializable, ICloneable
    {
        #region State holders for list
        private ArrayList arrayList = new ArrayList();
        private Hashtable hashTable = new Hashtable();
        #endregion
        #region CTors
        public VenueList() {}
        public VenueList(SerializationInfo info, StreamingContext context)
        {
            Venue[] venues = null;

            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while( enumerator.MoveNext()) 
            {
                switch( enumerator.Name) 
                {
                    case "Venues":
                        venues = (Venue[])info.GetValue("Venues", typeof(Venue[]));
                        break;
                }
            }

            if (venues == null)
                throw new SerializationException();

            arrayList.Clear();
            hashTable.Clear();

            for (int i = 0; i < venues.Length; i++)
            {
                arrayList.Add(venues[i]);
                hashTable.Add(venues[i].Name, venues[i]);
            }
        }

        #endregion
        #region IEnumerable Members
        public IEnumerator GetEnumerator()
        {
            return arrayList.GetEnumerator();
        }

        #endregion
        #region "IList" Members (modified for strongly typed)
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public Venue this[int index]
        {
            get
            {
                return (Venue)arrayList[index];
            }
            set
            {
                arrayList[index] = value;
                hashTable[value.Name] = value;
            }
        }

        public Venue this[string name]
        {
            get
            {
                return (Venue)hashTable[name];
            }
            set
            {
                if (!arrayList.Contains(value))
                {
                    arrayList.Add(value);
                }
                hashTable[name] = value;
            }
        }
        public void RemoveAt(int index)
        {
            Venue venue = (Venue)arrayList[index];
            hashTable.Remove(venue.Name);
            arrayList.RemoveAt(index);
        }

        public void Insert(int index, Venue value)
        {
            arrayList.Insert(index, value);
            hashTable.Add(value.Name, value);
        }

        public void Remove(Venue value)
        {
            arrayList.Remove(value);
            hashTable.Remove(value.Name);
        }
        public void Remove(string name)
        {
            Venue venue = (Venue)hashTable[name];
            arrayList.Remove(venue);
            hashTable.Remove(name);
        }
        public bool Contains(Venue value)
        {
            return arrayList.Contains(value);
        }

        public bool ContainsKey(string name)
        {
            return hashTable.ContainsKey(name);
        }

        public bool ContainsValue(Venue value)
        {
            return hashTable.ContainsValue(value);
        }

        public void Clear()
        {
            arrayList.Clear();
            hashTable.Clear();
        }

        public int IndexOf(Venue value)
        {
            return arrayList.IndexOf(value);
        }

        public int Add(Venue value)
        {
            hashTable.Add(value.Name, value);
            return arrayList.Add(value);
        }

        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public int Length
        {
            get
            {
                return arrayList.Count;
            }
        }



        #endregion
        #region ICollection Members
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public int Count
        {
            get
            {
                return arrayList.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            arrayList.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return null;
            }
        }

        #endregion
        #region ISerializable Members
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Venue[] venues = (Venue[])arrayList.ToArray(typeof(Venue));
            info.AddValue("Venues", venues, typeof(Venue[]));
        }
        #endregion
        public object Clone()
        {
            VenueList venueListNew = new VenueList();

            foreach(Venue v in arrayList)
            {
                venueListNew.Add(v);
            }
            return venueListNew;
        }
    }
}
