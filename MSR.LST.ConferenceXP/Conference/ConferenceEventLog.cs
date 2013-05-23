using System;
using System.Diagnostics;

using Microsoft.Win32;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// Rtp EventLog helper class
    /// 
    /// EL was preferred over EventLog because it is shorter
    ///  <ahem>C# design guidelines specificly say not to do this... (pfb)</ahem>
    /// </summary>
    public class ConferenceEventLog: MSR.LST.LstEventLog
    {
        /// <summary>
        /// Name of the EventLog log to which all entries will be written
        /// </summary>
        private static string LOG_NAME = "ConferenceAPI";

        /// <summary>
        /// An enumeration of the EventLog Sources for Rtp
        /// 
        /// Count is only meant to be used to determine the length of the enum
        /// </summary>
        public enum Source
        {
            Capability,
            Conference,
            Venue,
        }

        
        /// <summary>
        /// ID of the event log messages in Rtp
        /// </summary>
        public enum ID
        {
            Error,
            // Pri2: Make use of the ID enum in ConferenceEventLog
        }

        public ConferenceEventLog(Source source)
            :base(LOG_NAME, ".", source.ToString())
        { }

        public void WriteEntry(string message, EventLogEntryType type, ID eventID)
        {
            base.WriteEntry(message, type, (int)eventID);
        }

        /// <summary>
        /// Installs the EventLog log and all the sources
        /// </summary>
        internal static void Install()
        {
            Install(LOG_NAME, Enum.GetNames(typeof(Source)));
        }

        /// <summary>
        /// Uninstalls the EventLog log and all the sources
        /// </summary>
        internal static void Uninstall()
        {
            Uninstall(LOG_NAME, Enum.GetNames(typeof(Source)));
        }

    }

}
