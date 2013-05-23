using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

using MSR.LST.ConferenceXP.VenueService;
using MSR.LST.Net;


namespace MSR.LST.ConferenceXP
{

    public class InvalidPasswordException : System.Exception
    {
    }

    /// <summary>
    /// VenueBroker acts as a middleman for the venue service.  It deals with the venue service if it is inaccessible.
    /// It handles caching of venues as necessary.  It handles keeping a list of the existing venues, including those
    /// that were user-added.
    /// </summary>
    public class VenueBroker
    {
        #region Member variables
        private static int venueInUseTimeout = 2500;
        private static int serviceTimeout = 10000;

        private VenueList venues = new VenueList(); // Indexed by venue.Name
        private VenueService.VenueService service = null;

        private bool venueServiceTimedOut = false; // stores whether we've timedout during a venue service operation

        private string minVersion = null; // the minimum client version supported by the VenueService
        private string maxVersion = null; // the maxiumum client version supported by the VS.

        private bool mediateTimeouts = true;
        private ConferenceEventLog eventLog = new ConferenceEventLog(ConferenceEventLog.Source.Venue);
        #endregion

        #region Properties
        /// <summary>
        /// Whether this class masks and mediates venue service timeouts.
        /// 
        /// If true, VenueBroker will not only mask the timeouts - it will try to compensate by creating
        /// local venues when the venue list cannot be obtained and will create Participant wrappers
        /// around RtpParticipants as necessary.
        /// </summary>
        public bool MaskTimeout
        {
            get
            {
                return this.mediateTimeouts;
            }
            set
            {
                this.mediateTimeouts = value;
            }
        }

        public VenueList Venues
        {
            get
            {
                return this.venues;
            }
        }

        public VenueService.VenueService VenueService
        {
            get
            {
                return this.service;
            }
        }

        public string VenueServiceUrl
        {
            get
            {
                if (service == null)
                {
                    return null;
                }
                else
                {
                    return service.Url;
                }
            }
            set
            {
                // If we're setting the venue service url to null, set the venue service object itself 
                //   to null since the Url property itself cannot be null
                if (value == null)
                {
                    service = null;

                    this.venueServiceTimedOut = true;
                    this.venues = new VenueList();
                }
                else if(service == null || value != service.Url)
                {
                    service = new VenueService.VenueService();
                    service.Timeout = serviceTimeout;
                    service.Url = value;

                    this.venueServiceTimedOut = false;
                    this.RefreshVenues();
                }
            }
        }

        public int WebServiceTimeout
        {
            get
            {
                return serviceTimeout;
            }
            set
            {
                serviceTimeout = value;

                if( service != null )
                    service.Timeout = value;
            }
        }

        /// <summary>
        /// The venue service is down and unreachable.
        /// </summary>
        public bool IsUnreachable
        {
            get
            {
                return this.venueServiceTimedOut;
            }
        }

        /// <summary>
        /// Returns true if the venue service states that this application is compatible with the service.
        /// </summary>
        public bool VersionIsCompatible
        {
            get
            {
                return ((minVersion == null) && (maxVersion == null));
            }
        }

        public string MaximumVersion {
            get { return this.maxVersion;  }
        }

        public string MinimumVersion
        {
            get
            {
                return this.minVersion;
            }
        }

        #endregion

        #region Custom Venue Creation
        /// <summary>
        /// Creates a venue with IPAddress 234.*.*.* where each * is randomly generated.  Extremely useful
        /// in the case where a venue server is inaccessible.
        /// </summary>
        public Venue CreateRandomMulticastVenue( string name, Image icon )
        {
            IPEndPoint endPoint = null;
            bool trafficInVenue = true;
            while (trafficInVenue)
            {
                #region Randomly choose the Venue's IP address
                Random rnd = new Random();
                int randomaddr = 234;
                randomaddr = randomaddr * 256 + rnd.Next(1, 2^8 - 1);
                randomaddr = randomaddr * 256 + rnd.Next(1, 2^8 - 1);
                randomaddr = randomaddr * 256 + rnd.Next(1, 2^8 - 1);
                endPoint = new IPEndPoint(IPAddress.HostToNetworkOrder(randomaddr), 5004);
                #endregion
                #region Detect whether there is already traffic in the venue, if so change the IP address
                BufferChunk bc = new BufferChunk(MSR.LST.Net.Rtp.Rtp.MAX_PACKET_SIZE);
                UdpListener udpListener = new UdpListener(endPoint, venueInUseTimeout);
                try
                {
                    udpListener.Receive(bc);
                }
                catch (System.Net.Sockets.SocketException e)
                {
                    if (e.ErrorCode == 10060)
                    {
                        trafficInVenue = false;
                    }
                    else
                    {
                        throw;
                    }
                }
                finally
                {
                    udpListener.Dispose();
                }
                #endregion
            }

            VenueData venueData = new VenueData( name, endPoint, 5, VenueType.Custom, icon );

            return AddCustomVenue(venueData);
        }

        public Venue AddCustomVenue( string venueName, IPAddress ipAddress )
        {
            if (venues.ContainsKey(venueName))
            {
                throw new Exception(string.Format(CultureInfo.CurrentCulture, Strings.TryingToAddCustomVenue, 
                    venueName));
            }

            IPEndPoint ipEp = new IPEndPoint(ipAddress, 5004);

            return AddCustomVenue(new VenueData(venueName, ipEp, 128, VenueType.Custom, null));
        }

        public Venue AddCustomVenue( VenueData venueData )
        {
            if (venues.ContainsKey(venueData.Name))
            {
                throw new Exception(string.Format(CultureInfo.CurrentCulture, Strings.TryingToAddCustomVenue, 
                    venueData.Name));
            }

            venueData.VenueType = VenueType.Custom;

            Venue newVen = new Venue(venueData);
            venues.Add( newVen );
            
            return newVen;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Retries accessing the venueService if it timed out.
        /// </summary>
        /// <returns>
        /// Whether we timed out again.
        /// </returns>
        public bool RefreshAfterTimeout()
        {
            this.venueServiceTimedOut = false;
            this.RefreshVenues();

            return !this.venueServiceTimedOut;
        }

        public Venue ResolvePassword(String venueID, String password)
        {
            byte[] passwordHash = PasswordHasher.getInstance().HashPassword(password);

            VenueService.Venue retVal = service.GetVenueWithPassword(venueID, passwordHash);
            if (retVal == null || retVal.IPAddress == null || retVal.IPAddress.Equals(IPAddress.None))
                throw new InvalidPasswordException();
            else return new Venue(retVal);
        }

        public Participant GetParticipant(string cname)
        {
            return GetParticipant(cname, cname, null, null);
        }

        public Participant GetParticipant(string cname, string name, string email, string phone)
        {
            VenueService.Participant vp = this.GetParticipantData(cname);

            if (vp == null)
            {
                // Create a minimal participant if venue service lookup failed
                vp = new VenueService.Participant();
                vp.Identifier = cname;
            }

            // Pri3: Look for nulls & pick based on what's not null, instead of just using one or the other...

            vp.Name = name;
            vp.Email = email;

            return new Participant(vp);
        }

        /// <summary>
        /// Gets a participant without mediating a problem, should it occur.
        /// </summary>
        public Participant GetParticipantUnmediated(string cname)
        {
            VenueService.Participant vp = this.GetParticipantData(cname);

            if( vp == null )
                return null;
            else
                return new ConferenceXP.Participant(vp);
        }
        #endregion

        #region Helper Methods
        private void RefreshVenues()
        {
            venues = new VenueList();

            VenueService.Venue[] wsVenues = null;

            bool versionGood = false;

            try
            {
                string minVersion = null;
                string maxVersion = null;
                string entryAssemFile = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string fileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(entryAssemFile).ProductVersion;
                versionGood = service.CheckAssemblyVersion(fileVersion, out minVersion, out maxVersion);
                this.minVersion = this.maxVersion = null;

                if (!versionGood) {
                    this.minVersion = minVersion;
                    this.maxVersion = maxVersion;
                    return; // we aren't compatible with this venue service, so don't try to get the venues
                }

                wsVenues = service.GetVenues(Identity.Identifier);
            }
            catch (Exception e)
            {
                if (this.mediateTimeouts)
                {
                    this.venueServiceTimedOut = true;
                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                        Strings.UnableToConnectUsingDefaultVenue, e.ToString()), EventLogEntryType.Warning, 25);
                }
                else
                {
                    throw;
                }
            }

            if (wsVenues != null)
            {
                foreach (VenueService.Venue v in wsVenues)
                {
                    //We expect the future VenueService may return some types that this client version does not handle.
                    if (v.VenueType != MSR.LST.ConferenceXP.VenueService.VenueType.STATIC_SINGLE_GROUP) {
                        continue;
                    }

                    try
                    {
                        ConferenceXP.Venue cxpVenue = new ConferenceXP.Venue(v);

                        // If the venue is IPv4 or (the venue is IPv6 and IPv6 
                        // is installed on the OS)
                        if (cxpVenue.EndPoint.AddressFamily == AddressFamily.InterNetwork ||
                           (cxpVenue.EndPoint.AddressFamily == AddressFamily.InterNetworkV6 &&
                           Socket.OSSupportsIPv6))
                        {
                            venues.Add(new ConferenceXP.Venue(v));
                        }
                        else // IPv6 not installed on OS
                        {
                            CreateInvalidVenue(venues, v);
                        }
                    }
                    catch (Exception e)
                    {
                        eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.InvalidVenue, e.ToString()),
                            EventLogEntryType.Error, ConferenceEventLog.ID.Error);

                        CreateInvalidVenue(venues, v);
                    }
                }
            }
        }

        private void CreateInvalidVenue(VenueList venues, VenueService.Venue v)
        {
            eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.InvalidVenue, v.Name),
                EventLogEntryType.Warning, ConferenceEventLog.ID.Error);

            venues.Add(new ConferenceXP.Venue(v.Name, new IPEndPoint(IPAddress.Any, 0),
                VenueType.Invalid, Utilities.ByteToBitmap(v.Icon)));
        }

        private VenueService.Participant GetParticipantData(string cname)
        {
            VenueService.Participant vp = null;

            if( this.service != null && !this.venueServiceTimedOut )
            {
                try
                {
                    //Pri2: This should be an async call to the Venue Service
                    vp = service.GetParticipant(cname);

                    // Check for possible problems on the Venue Service
                    if (vp != null && vp.Identifier.ToLower(CultureInfo.InvariantCulture) != cname.ToLower(CultureInfo.InvariantCulture))
                    {
                        eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.IdentifierNotMatchingRequest, 
                            cname, vp.Identifier), EventLogEntryType.Error, ConferenceEventLog.ID.Error);

                        return null;
                    }
                }
                catch (Exception e)
                {
                    if( this.mediateTimeouts )
                    {
                        eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.UnableToConnectUsingGenericParticipant, 
                            e.ToString()), EventLogEntryType.Warning, 60);
                        this.venueServiceTimedOut = true;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return vp;
        }
        #endregion

    }

}