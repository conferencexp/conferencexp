using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

using Microsoft.Win32;

using MSR.LST;


namespace MSR.LST.ConferenceXP.ArchiveService
{
    /// <summary>
    /// ArchiveServer is a .NET TCP Remoting Singleton SAO (Server Activated Object) designed 
    /// to handle all starting/stopping of recording and playback.
    /// </summary>
    public class ArchiveServer : MarshalByRefObject, IArchiveServer
    {
        #region EventLog
        /// <summary>
        /// A singleton event log wrapper.
        /// </summary>
        private static ArchiveServiceEventLog eventLog = null;

        private static void InitEventLog()
        {
            eventLog = new ArchiveServiceEventLog( ArchiveServiceEventLog.Source.ArchiveServer);
        }
        #endregion

        #region Ctor / Init / Install
        static ArchiveServer()
        {
            Install();
            InitEventLog();
            if (Constants.DiagnosticService != null) {
                //Flag the RTP code to participate by sending our diagnostic information to the server.
                MSR.LST.Net.Rtp.RtpSession.DiagnosticsServer = Constants.DiagnosticService;
                MSR.LST.Net.Rtp.RtpSession.DiagnosticsEnabled = true;
            }
        }

        public ArchiveServer()
        {
            Trace.WriteLine("Creating an ArchiveServer object.");

            // Hook the end-of-playback event in ConferencePlayer
            ConferencePlayer.EndOfConferenceReached += new EventHandler(OnPlaybackStopped);
        }

        /// <summary>
        /// Ensure that ArchiveService is properly installed on each run, attempting to make ".NET File Copy Deployment" a reality.
        /// </summary>
        /// <remarks>
        /// This was stolen from MSR.LST.Net.Rtp.  See Notes on bugs in this design in MSR.LST.Net.Rtp.RtpSession.Install
        /// </remarks>
        private static void Install()
        {
            // Get the installed state out of the registry -- if we're already installed, we don't have to reinstall
            if (Installation.Installed == false)
            {
                // Install myself
                IDictionary state = new Hashtable();
                state.Clear();
                Installation inst = new Installation();
                inst.Install(state);
                inst.Commit(state);
            }
        }
        #endregion

        #region Get Conferences/Participants/Streams

        public Version GetVersion() {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }

        public Conference[] GetConferences()
        {
            // Get the full list of conferences, and remove those that do not have proper end times
            Conference[] fullList = DBHelper.GetConferences();
            ArrayList confsWithEndTimes = new ArrayList(fullList.Length);
            foreach (Conference conf in fullList)
            {
                if (conf.End != DateTime.MinValue)
                    confsWithEndTimes.Add(conf);
            }

            // Return an array of those conferences
            Conference[] confsWithEndTimesArray = (Conference[])confsWithEndTimes.ToArray(typeof(Conference));
            return confsWithEndTimesArray;
        }

        public Participant[] GetParticipants(int conferenceID)
        {
            return DBHelper.GetParticipants( conferenceID );
        }

        public Stream[] GetStreams(int sessionID)
        {
            return DBHelper.GetStreams( sessionID );
        }
        #endregion

        #region Play / Record
        /// <summary>
        /// Records the conference.
        /// </summary>
        /// <param name="conferenceDescription">A description of the conference being recorded.</param>
        /// <param name="venueIdentifier">Venue name for the conference.</param>
        /// <param name="venue">Venue to record from.</param>
        /// <returns>The number of ongoing requests to record this conference.</returns>
        public int Record( string conferenceDescription, string venueIdentifier, IPEndPoint venue )
        {
            try
            {
                lock(this)
                {
                    CheckForIPv6(venue);

                    ConferenceRecorder ongoingRecording = CheckVenueForPlayback(venue);

                    int refs;
                    object refsObj = references[venue];
                    if( refsObj == null )
                        refs = 0;
                    else
                        refs = (int)refsObj;

                    // If a recording exists, the references to it should not equal zero
                    Debug.Assert(ongoingRecording == null || refs != 0);

                    ConferenceRecorder recorder = null;
                    if( refs == 0 )
                    {
                        // Start a ConferenceRecorder
                        eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.RecordingANewConference, 
                            conferenceDescription, venueIdentifier, venue), EventLogEntryType.Information, 
                            ArchiveServiceEventLog.ID.Starting);

                        recorder = new ConferenceRecorder();
                        recorder.RecordConference(conferenceDescription, venueIdentifier, venue);
                
                        sessions.Add(venue, recorder);
                    }
                    else
                    {
                        recorder = ongoingRecording;
                    }

                    ++refs;
                    references[venue] = refs;

                    return refs;
                }
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error, ArchiveServiceEventLog.ID.Error);
                throw;
            }
        }

        /// <summary>
        /// Plays back a conference, or at least a series of streams.
        /// </summary>
        /// <param name="venue">Venue to play back into (may be a unicast location).</param>
        /// <param name="streams">Database IDs of the streams to record.</param>
        /// <param name="playbackStopped">An Eventhandler to call when playback has 
        /// stopped due to reaching the end of all the streams.</param>
        public void Play( IPEndPoint venue, int[] streams )
        {
            try
            {
                lock(this)
                {
                    CheckForIPv6(venue);

                    CheckVenueNotInUse(venue);

                    // Start a ConferencePlayer
                    eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.PlayingARecordedConference, 
                        venue, streams.Length), EventLogEntryType.Information, ArchiveServiceEventLog.ID.Starting);

                    ConferencePlayer player = new ConferencePlayer();

                    // Unicast playback sessions shouldn't receive the inbound data.  The reason for this is that
                    //   multiple sessions would conflict at the port level, because they're all trying to receive
                    //   data on the same port.  
                    bool multicast = MSR.LST.Net.Utility.IsMulticast(venue);
                    player.Play(venue, streams, multicast);
            
                    sessions.Add(venue, player);
                }
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error, ArchiveServiceEventLog.ID.Error);
                throw;
            }
        }

        private void CheckForIPv6(IPEndPoint venue)
        {
            if (venue.AddressFamily == AddressFamily.InterNetworkV6 &&
                !Socket.OSSupportsIPv6)
            {
                throw new ArgumentException(Strings.VenueAddressProtocolError);
            }
        }

        private ConferenceRecorder CheckVenueForPlayback(IPEndPoint venue)
        {
            object sessionObj = sessions[venue];
            if( sessionObj is ConferencePlayer )
            {
                throw new PlaybackInProgressException(Strings.PlaybackOngoingError);
            }
            // else...
            
            return (ConferenceRecorder) sessionObj;
        }

        private void CheckVenueNotInUse(IPEndPoint venue)
        {
            object sessionObj = sessions[venue];
            if( sessionObj != null )
            {
                throw new VenueInUseException(Strings.RecordingOngoingError);
            }
        }

        private void OnPlaybackStopped(object sender, EventArgs ea)
        {
            lock(this)
            {
                // Ensure hte conferencePlayer has stopped
                ConferencePlayer player = (ConferencePlayer)sender;
                player.StopPlaying();
                sessions.Remove(player.Venue);
            }
        }

        /// <summary>
        /// Jumps to a specific time in a recording.
        /// </summary>
        public void JumpTo(IPEndPoint venue, long timeToJumpTo)
        {
            lock(this)
            {
                ConferencePlayer player = (ConferencePlayer)sessions[venue];
                if( player == null )
                    throw new ArgumentException(Strings.VenueDoesNotCorrelate);

                player.JumpTo(timeToJumpTo);
            }
        }

        /// <summary>
        /// Gets the current point in time, in ticks, of where playback is in the course of the recording.
        /// </summary>
        public long GetCurrentTime(IPEndPoint venue)
        {
            lock(this)
            {
                ConferencePlayer player = (ConferencePlayer)sessions[venue];
                if( player == null )
                    throw new ArgumentException(Strings.VenueDoesNotCorrelate);

                return player.CurrentTime;
            }
        }

        public void StopPlaying(IPEndPoint venue)
        {
            lock(this)
            {
                ConferencePlayer player = (ConferencePlayer)sessions[venue];
                if( player != null )
                {
                    player.StopPlaying();
                    sessions.Remove(venue);
                }
            }
        }

        public int StopRecording(IPEndPoint venue)
        {
            lock(this)
            {
                ConferenceRecorder recorder = (ConferenceRecorder)sessions[venue];
                if( recorder != null && recorder.IsRecording )
                {
                    int refs = (int)references[venue];

                    // Decrease our reference count
                    --refs;

                    // Only stop recording if no one wants this recording
                    if( refs <= 0 ) 
                    {
                        // Stop recording
                        recorder.StopRecording();
                        references.Remove(venue);
                        sessions.Remove(venue);
                    }
                    else
                    {
                        references[venue] = refs;
                    }

                    return refs;
                }
                else // if we aren't recording any more, return a -1, signifying "references were already 0!"
                {
                    return -1; // Consider this an "error code", of sorts.
                }
            }
        }
        #endregion

        /// <summary>
        /// Singletons should live forever.  This accomplishes that.
        /// </summary>
        public override object InitializeLifetimeService()
        {
            return null;
        }

        #region Private Variables
        // Holds a "session" (ConferencePlayer or ConferenceRecorder), keyed by IPEndPoint
        private Hashtable sessions = new Hashtable();

        // Holds the number of recording requests per ongoing recording session
        private Hashtable references = new Hashtable();
        #endregion

    }
}
