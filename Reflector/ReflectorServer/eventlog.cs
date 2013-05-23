using System;
using System.Diagnostics;

using Microsoft.Win32;


namespace MSR.LST.ConferenceXP.ReflectorService
{
    /// <summary>
    /// Helper class for EventLog
    /// </summary>
    public class ReflectorEventLog: MSR.LST.LstEventLog
    {
        /// <summary>
        /// Name of the EventLog log to which all entries will be written
        /// </summary>
        private static string LOG_NAME = "CXPReflectorService";

        /// <summary>
        /// An enumeration of the EventLog Sources
        /// </summary>
        public enum Source
        {
            MCtoUC,
            ReflectorManager,
            ReflectorServiceInstaller,
            RegistrarServer,
            UCtoUCMC
        }

        
        /// <summary>
        /// ID of the event log messages
        /// </summary>
        public enum ID
        {
            Error,

            ServiceInstalled,
            ServiceStarted,
            
            ThreadStoppingException,
            ThreadStartingException,
            MCtoUCException,
            UCtoUCMCException,
            JoinLeave,
            TimeOut,

            ServiceUninstalled = 10 + ServiceInstalled,
            ServiceStopped = 10 + ServiceStarted
        }
        
        public ReflectorEventLog(Source source)
            :base(LOG_NAME, ".", source.ToString())
        {}

        public void WriteEntry(string message, EventLogEntryType type, ID eventID)
        {
            base.WriteEntry(message, type, (int)eventID);
        }

        #region Install/Uninstall
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
        #endregion
    }
}
