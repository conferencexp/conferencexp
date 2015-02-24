using System;
using System.Diagnostics;
using System.Collections;

using Microsoft.Win32;


namespace MSR.LST.ConferenceXP.ArchiveService
{
    /// <summary>
    /// Archive Service EventLog helper class
    /// </summary>
    internal class ArchiveServiceEventLog: MSR.LST.LstEventLog
    {
        /// <summary>
        /// Name of the EventLog log to which all entries will be written
        /// </summary>
        private static string LOG_NAME = "Archive Service";

        /// <summary>
        /// An enumeration of the EventLog Sources.
        /// </summary>
        public enum Source
        {
            Service,

            ArchiveServer,
            ConferenceRecorder,
            StreamRecorder,
            BufferRecorder,
            DBHelper,

            ConferencePlayer,
            StreamPlayer,
            BufferPlayer
        }

        
        /// <summary>
        /// ID of the event log messages.
        /// </summary>
        public enum ID
        {
            Error,
            Warning,
            Information,

            Starting,
            Stopping,

            BufferNotAvailable,
            BadStreamInDB,
            DBOpFailed,
            IndiciesFailedToSave,
            FrameDropped,
            GrowingBuffers,
            EmptyBuffersInPlayback,
            ImproperPopulateCall,
            BufferTooSmall,
            MaximumStreamSizeReached,
        }

        public ArchiveServiceEventLog(Source source)
            :base(LOG_NAME, ".", source.ToString())
        {
            //base.TickBetweenEachEntry = 1000 * Constants.TicksPerMs;
            base.TickBetweenEachEntry = -1;
        }

        public void WriteEntry(string message, EventLogEntryType type, ID eventID)
        {
            base.WriteEntry(message, type, (int)eventID);
        }

        #region Install/Uninstall
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
        #endregion

    }

}
