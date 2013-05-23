using System;
using System.Diagnostics;

using Microsoft.Win32;


namespace MSR.LST.MDShow
{
    /// <summary>
    /// MDShow EventLog helper class
    /// </summary>
    internal class MDShowEventLog: MSR.LST.LstEventLog
    {
        /// <summary>
        /// Name of the EventLog log to which all entries will be written
        /// </summary>
        private static string LOG_NAME = "Filters";

        /// <summary>
        /// An enumeration of the EventLog Sources for MDShow
        /// 
        /// Count is only meant to be used to determine the length of the enum
        /// </summary>
        public enum Source
        {
            MDShow,
            RtpRenderer,
            RtpSource,
        }

        
        /// <summary>
        /// ID of the event log messages in MDShow
        /// </summary>
        public enum ID
        {
            Error,
        }

        public MDShowEventLog(Source source)
            :base(LOG_NAME, ".", source.ToString())
        { }

        public void WriteEntry(string message, EventLogEntryType type, ID eventID)
        {
            base.WriteEntry(message, type, (int)eventID);
        }

        /// <summary>
        /// Installs the EventLog log and all the sources
        /// </summary>
        public static void Install()
        {
            Install(LOG_NAME, Enum.GetNames(typeof(Source)));
        }


        /// <summary>
        /// Uninstalls the EventLog log and all the sources
        /// </summary>
        public static void Uninstall()
        {
            Uninstall(LOG_NAME, Enum.GetNames(typeof(Source)));
        }

    }

}
