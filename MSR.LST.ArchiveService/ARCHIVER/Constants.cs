using System;
using System.Globalization;
using System.Reflection;
using System.Net;


namespace MSR.LST.ConferenceXP.ArchiveService
{
    /// <summary>
    /// Constants class..Static to hold all the constants for the application
    /// </summary>
    public abstract class Constants
    {
        public static string    PersistenceCName = Strings.ArchiveServiceMicrosoftCom;      // name of us as a participant
        public static string PersistenceName = Strings.ArchiveService;

        public static int       MaxDBStringSize = 255;                                   // no. of chars allowed in varchar datatypes

        public static string    SQLConnectionString = 
            "Initial Catalog='ArchiveService';Data Source=.;Integrated Security=true;Connect Timeout=120";

        public static string DiagnosticService = null;
        //Language override
        public static string UICulture = null;

        public const int        TicksPerMs = 10000;                                     // handy constant
        public static int       CommandTimeout = 60;                                    // command timeout used on commands in DBHelper

        // Recording constants
        public static int       InitialStreams = 4;                                     // number of streams we expect 
        public static int       BufferSize = 405 * 8080 / 4;                            // 1/4 of the data than an intermediate page in a SQL BLOB can hold
        public static int       BufferTimeout = -1;                                     // timeout before buffer writes to disk; turned off
        public static int       IndexCapacity = BufferSize / 1024;                      // allow for 1024 byte frames
        public static int       InitialBuffers = 2;                                     // basic # of buffers necessary under small load
        public static int       MaxBuffers = 8;                                         // under horrendous load, we can peak to this
        public static int       StopAfterVenueEmptySec = 60;                            // <x> sec after a venue is empty, we stop recording

        // Playback constants
        public static int       BuffersPerStream  = 2;                                  // One to buffer, one to send.  30 sec buffering / sender.
        public static long      LongestSleepTime = 1000 * TicksPerMs;                   // The longest time the sending thread should nap.
        public static int       PlaybackBufferInterval = 10 * 1000 * TicksPerMs;        // temporal length of buffer, in ms
                                            // intentionally equivelant to the time a recording buffer of medium-qual video
        public static int       TicksSpent = 16 * TicksPerMs;                           // a debugging constant for displaying too much time spent
        public static int       SenderCreationLeadTime = 500 * TicksPerMs;              // time before a sender is needed that is created

        // remoting stuff
        public static int       TCPListeningPort = 8082;                                // port that our remoting object listens on


        #region App.Config Overrides
        private const string baseName = "MSR.LST.ConferenceXP.ArchiveService.";

        /// <summary>
        /// This static constructor reads for every non-constant static field in the class and checks for an app.config override.
        /// </summary>
        static Constants()
        {
            Type myType = typeof(Constants);
            FieldInfo[] fields = myType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                if (field.IsLiteral) // Is Constant?
                    continue; // We can't set it

                string fullName = baseName + field.Name;
                string configOverride = System.Configuration.ConfigurationManager.AppSettings[fullName];
                if (configOverride != null && configOverride != String.Empty) {
                    Type newType = field.FieldType;
                    object newValue = Convert.ChangeType(configOverride, newType, CultureInfo.InvariantCulture);
                    field.SetValue(null, newValue);
                }
                else {
                    //If the cname is not set in app.config, attempt to create a default using the host name
                    if (field.Name.Equals("PersistenceCName")) {
                        string cname = Strings.ArchiveServiceMicrosoftCom;
                        try {
                            cname = "ArchiveService@" + Dns.GetHostEntry("").HostName;
                        }
                        catch {
                            try {
                                cname = "ArchiveService@" + Dns.GetHostName() + ".net";
                            }
                            catch {
                                cname = Strings.ArchiveServiceMicrosoftCom;
                            }
                        }
                        field.SetValue(null, cname);
                    }
                }
            }
        }
        #endregion
    }
}
