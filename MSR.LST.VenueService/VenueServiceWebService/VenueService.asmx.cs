using System;
using System.Collections;
using System.Configuration;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;

using Microsoft.Win32;


namespace MSR.LST.ConferenceXP.VenueService
{
    /// <summary>
    /// VenueService exposes a web service interface onto the conferencing venue and
    /// participant data.
    /// </summary>
    /// <example>
    /// using MSR.LST.ConferenceXP.VenueService
    /// class Test
    /// {
    ///     public static void Main()
    ///     {
    ///         VenueService sv = new VenueService()
    ///         Venue[] venues = vs.GetVenues()
    ///     }
    /// }
    /// </example>
    [WebService(Namespace="http://research.microsoft.com/lst")]
    public class VenueService : System.Web.Services.WebService
    {
        static FileStorage storage = null;

        #region Static & Instance Ctors
        static VenueService()
        {
            // Start by getting the venue service storage (use the app.config as an override)
            string filePath = ConfigurationManager.AppSettings["FilePath"];
            if (filePath == null || filePath == String.Empty)
            {
                // Use the registry key
                using(RegistryKey key = Registry.LocalMachine.OpenSubKey(StorageInstaller.LocalMachineSubkey))
                {
                    filePath = (string)key.GetValue("DataFile");
                }
            }

            storage = new FileStorage(filePath);
        }

        /// <summary>
        /// VenueService constructor. Creates a new instance of the VenueService. 
        /// Reads the database connection string from the web.config file if present.
        /// </summary>
        /// <example>
        /// /// using MSR.LST.ConferenceXP.VenueService
        /// class Test
        /// {
        ///     public static void Main()
        ///     {
        ///         // construct a VenueService object
        ///         VenueService sv = new VenueService()
        ///         Venue[] venues = vs.GetVenues()
        ///     }
        /// }
        /// </example>
        public VenueService()
        {
            //CODEGEN: This call is required by the ASP.NET Web Services Designer
            InitializeComponent();
        }
        #endregion

        #region Component Designer generated code

        //Required by the Web Services Designer 
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if(disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Version-checking
        /// <summary>
        /// Verifies that the entry assembly's version provided matches the expected assembly
        /// version for all users using this venue service.
        /// </summary>
        /// <remarks>While it is not enforced, all applications connecting to this
        /// Venue Service should run this method before any other.</remarks>
        /// <param name="assemblyVersion">The file version of the entry assembly on the
        /// client machine as a string.</param>
        /// <param name="minVersion">Minimum version.  This will be non-null if the client 
        /// needs to be upgraded to work with the VS.</param>
        /// <param name="maxVersion">Maximum version.  This will be non-null if the VS only supports clients of 
        /// older version.</param>
        /// <returns>True if the assembly version is compatible with the users on
        /// this Venue Service.</returns>
        [WebMethod]
        public bool CheckAssemblyVersion(string assemblyVersion, out string minVersion, out string maxVersion)
        {
            minVersion = null;
            maxVersion = null;

            string version = ConfigurationManager.AppSettings["MinVersion"];
            if( version != null && version != String.Empty )
            {
                if( new Version(version) > new Version(assemblyVersion) )
                {
                    minVersion = version;
                }
            }

            version = ConfigurationManager.AppSettings["MaxVersion"];
            if( version != null && version != String.Empty )
            {
                if( new Version(version) < new Version(assemblyVersion) )
                {
                    maxVersion = version;
                }
            }

            return ((minVersion == null) && (maxVersion == null));

        }
        #endregion

        #region Venues
        /// <summary>
        /// Retrieves the specified venue from the database.
        /// Returns null if the venue does not exist
        /// </summary>
        /// <param name="venueIdentifier">the id of the venue to retrieve</param>
        /// <returns>the required venue or null</returns>
        /// /// <example>
        /// using MSR.LST.ConferenceXP.VenueService
        /// class Test
        /// {
        ///     public static void Main()
        ///     {
        ///         VenueService vs = new VenueService()
        ///         Venue v = vs.GetVenue("test@test.com");
        ///
        ///         if ( v != null)
        ///             Console.WriteLine("venue {0} has ip address {1}", v.Name, v.IPAddress);
        ///     }
        /// }
        /// </example>
        [WebMethod]
        public Venue GetVenue(string venueIdentifier)
        {
            ValidateEmail(venueIdentifier);
            return storage.GetVenue(venueIdentifier);
        }

        /// <summary>
        /// Retrieves all the venues in the database, ordered by name.
        /// Returns null if no venues currently exist.
        /// </summary>
        /// <returns>an array of venues or null if no venues exist</returns>
        /// <example>
        /// using MSR.LST.ConferenceXP.VenueService
        /// class Test
        /// {
        ///     public static void Main()
        ///     {
        ///         VenueService vs = new VenueService()
        ///         foreach ( Venue v in vs.GetVenues()
        ///         {
        ///             Console.WriteLine("Venue named {0}", v.Name);
        ///         }
        ///     }
        /// }
        /// </example>
        [WebMethod]
        public Venue[] GetVenues(string participantIdentifier)
        {
            ValidateEmail(participantIdentifier);

            Venue[] allVenues = storage.GetVenues();
            ArrayList enterable = new ArrayList();

            foreach( Venue ven in allVenues )
            {
                if (ven.AccessList == null || ven.AccessList.IsMatch(participantIdentifier))
                {
                    enterable.Add(ven);
                }
            }

            return (Venue[])enterable.ToArray(typeof( Venue));
        }
        #endregion

        #region Participants
        /// <summary>
        /// Adds a participant to the database.
        /// Throws if the participant is already present.
        /// </summary>
        /// <param name="p">the participant to add</param>
        /// <example>
        /// using MSR.LST.ConferenceXP.VenueService
        /// class Test
        /// {
        ///     public static void Main()
        ///     {
        ///         VenueService vs = new VenueService()
        ///         Participant p = new Participant();
        ///         p.Identifier = "someone@hotmail.com";
        ///         p.Name = "Joe Bloggs";
        ///         p.Phone="234 5 6 7";
        ///         p.Email = p.Identifier;
        ///
        ///         vs.AddParticipant(p);
        ///     }
        /// }
        /// </example>

        [WebMethod]
        public void AddParticipant(Participant p)
        {
            ValidateParticipant(p);
            storage.AddParticipant(p);
        }

        /// <summary>
        /// Updates the specified participant
        /// </summary>
        /// <param name="p">the participant to update</param>
        /// <example>
        /// using MSR.LST.ConferenceXP.VenueService
        /// class Test
        /// {
        ///     public static void Main()
        ///     {
        ///         VenueService vs = new VenueService()
        ///         Participant p = new Participant();
        ///         p.Identifier = "someone@hotmail.com";
        ///         p.Name = "Joe Bloggs";
        ///         p.Phone="234 5 6 7";
        ///         p.Email = p.Identifier;
        ///
        ///         vs.AddParticipant(p);
        ///
        ///         p.Email = "john@hotmail.com";
        ///         vs.UpdateParticipant(p);
        ///     }
        /// }
        /// </example>
        [WebMethod]
        public void UpdateParticipant(Participant p)
        {
            ValidateParticipant(p);
            storage.UpdateParticipant(p);
        }

        /// <summary>
        /// Retrieves the specified participant from the database.
        /// Returns null if the participant does not exist.
        /// </summary>
        /// <param name="participantIdentifier">the participant to retrieve</param>
        /// <returns>a participant or null</returns>
        /// <example>
        /// using MSR.LST.ConferenceXP.VenueService
        /// class Test
        /// {
        ///     public static void Main()
        ///     {
        ///         VenueService vs = new VenueService()
        ///         Participant p = vs.GetParticipant("someone@hotmail.com");
        ///         Console.WriteLine("Participant {0} has name {1}", p.Identifier, p.Name);
        ///     }
        /// }
        /// </example>
        [WebMethod]
        public Participant GetParticipant(string participantIdentifier)
        {
            ValidateEmail(participantIdentifier);
            return storage.GetParticipant(participantIdentifier);
        }

        /// <summary>
        /// retrieves a list of all participants in the databse ordered by name.
        /// Returns null if there are no participants present
        /// </summary>
        /// <returns>an array of participants or null</returns>
        /// <example>
        /// using MSR.LST.ConferenceXP.VenueService
        /// class Test
        /// {
        ///     public static void Main()
        ///     {
        ///         VenueService vs = new VenueService()
        ///         Participant[] parts = vs.GetParticipants();
        ///         if ( parts != null &amp;&amp; parts[0] &amp;= null )
        ///         {
        ///             parts[0].Email = "someonenew@somewhere.com";
        ///             vs.UpdateParticipant(parts[0]);
        ///         }
        ///
        ///     }
        /// }
        /// </example>
        [WebMethod]
        public Participant[] GetParticipants()
        {
            return storage.GetParticipants();
        }


        [WebMethod]
        public Venue GetVenueWithPassword(string venueIdentifier, byte [] passwordHash)
        {
            ValidateEmail(venueIdentifier);
            return storage.GetVenueWithPassword(venueIdentifier, passwordHash);
        }
        #endregion

        #region Privacy
        [WebMethod]
        public string PrivacyPolicyUrl()
        {
            return ConfigurationManager.AppSettings["PrivacyPolicyURL"];
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Private: Validates that the specified participant's properties are valid
        /// </summary>
        /// <param name="p">the participant to validate</param>
        private static void ValidateParticipant(Participant p)
        {
            ValidateEmail(p.Identifier);

            if (p.Name != null && p.Name.Length > 255)
                 throw new SoapException("VenueServiceException",
                    new XmlQualifiedName("VSNameLengthException"));

            if ( p.Email != null && p.Email.Length > 0 )
                ValidateEmail(p.Email);

            // Check that the max image size does not exceed 96x96 pixels at 32-bit color depth
            if (p.Icon != null && p.Icon.Length > 36864)
                throw new SoapException("VenueServiceException",
                    new XmlQualifiedName("VSParticipantIconSizeException"));
        }

        /// <summary>
        /// Private: validates an email address is of the correct form or is null
        /// </summary>
        /// <param name="email">the email to validate</param>
        private static void ValidateEmail( string email )
        {
            Regex reg = new Regex(SecurityPatterns.EmailValidator, RegexOptions.IgnoreCase);
            if ( !reg.IsMatch(email) )
                throw new SoapException("VenueServiceException",
                    new XmlQualifiedName("VSEmailAddressException"));
        }
        #endregion

    }
}