using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;

using MSR.LST.Net.Rtp;


namespace MSR.LST.ConferenceXP.ArchiveService
{
    /// <summary>
    /// Responsible for playing back a conference.  Manages a number of stream players.
    /// </summary>
    public class ConferencePlayer
    {
        #region Private Variables
        private IPEndPoint venueIPE = null;             // Venue we're playing back to
        private RtpSession rtpSession = null;           // Outgoing data socket via RTP API
        private StreamsGroupPlayer avPlayer = null;     // Plays A/V streams (with small frame sizes)
        private StreamsGroupPlayer otherPlayer = null;  // Plays 'other' streams (with supposedly large frame sizes)
        private ConferencePlayerPC perfCounter;         // Performance counter for this instance

        private bool playing = false;                   // Are we playing?
        private long firstStoredTick = 0;               // First stored 'ticks' in all the streams
        private long startTime = 0;                     // Start time in ticks (local, current, time) of when we started playing

        public static event EventHandler EndOfConferenceReached;
        #endregion

        #region EventLog
        /// <summary>
        /// A singleton event log wrapper.
        /// </summary>
        private static ArchiveServiceEventLog eventLog = null;

        private static void InitEventLog()
        {
            eventLog = new ArchiveServiceEventLog( ArchiveServiceEventLog.Source.ConferencePlayer);
        }
        #endregion

        #region Constructors
        static ConferencePlayer()
        {
            InitEventLog();
        }
        #endregion

        #region Public Methods
        public void Play( IPEndPoint venue, int[] streams, bool receiveData )
        {
            if( this.playing )
                throw new InvalidOperationException(Strings.PlayerAlreadyPlayingError);
            if( streams == null )
                throw new ArgumentNullException(Strings.StreamsCannotBeNull);

            venueIPE = venue;

            // Create an RtpSession
            RtpParticipant me = new RtpParticipant(Constants.PersistenceCName, 
                string.Format(CultureInfo.CurrentCulture, Strings.Playing, Constants.PersistenceName));
            rtpSession = new RtpSession(venue, me, true, receiveData);

            // andrew: connect to diagnostic server
            rtpSession.VenueName = "Archived: " + venue.ToString();

            // Hook the static stream-ended event
            StreamPlayer.EndOfStreamReached += new EventHandler(EndOfStreamReached);

            // Create a new perf counter for this ConferencePlayer instance
            this.perfCounter = new ConferencePlayerPC(venue.ToString());

            // Keep track of the streamPlayers
            ArrayList avStreams = new ArrayList();
            ArrayList otherStreams = new ArrayList();

            // Find the first stored ticks of all the streams while creating them...
            this.firstStoredTick = long.MaxValue;

            // Create all of our StreamPlayers (one per stream...)
            for ( int i = 0; i < streams.Length; i++)
            {
                StreamPlayer newStream = null;

                try
                {
                    newStream = new StreamPlayer(rtpSession, streams[i], perfCounter);
                }
                catch( Exception ex )
                {
                    eventLog.WriteEntry(String.Format(CultureInfo.CurrentCulture, Strings.StreamWithBadDataReached, 
                        streams[i], ex.ToString()), EventLogEntryType.Warning, 
                        ArchiveServiceEventLog.ID.BadStreamInDB);
                }

                if( newStream != null )
                {
                    perfCounter.AddInstanceForCollection( newStream );

                    if( newStream.FirstStreamsTicks < this.firstStoredTick )
                        this.firstStoredTick = newStream.FirstStreamsTicks;

                    // Add the new StreamPlayer to the right collection
                    // Pri3: Consider other methods here.  Maybe a smarter detection of large frame sizes,
                    //  or initializing the stream so it has enough packets ahead of time?
                    if( newStream.Payload == PayloadType.dynamicAudio || newStream.Payload == PayloadType.dynamicVideo )
                        avStreams.Add(newStream);
                    else // RTDocs stream or other large-payload stream, most likely
                        otherStreams.Add(newStream);
                }
            }

            // Start the StreamsGroupPlayers
            // Pri2: Change this to be compatable with use of the "playback speed" feature.
            long startTime = DateTime.Now.Ticks + 1500*Constants.TicksPerMs; // we'll start in 1.5 seconds (for init time)
            
            // Create the StreamsGroupPlayer(s)
            avPlayer = new StreamsGroupPlayer(avStreams, this.firstStoredTick, startTime);
            otherPlayer = new StreamsGroupPlayer(otherStreams, this.firstStoredTick, startTime);

            this.playing = true;
        }

        /// <summary>
        /// Jumps to a specific time in the recording.
        /// </summary>
        public void JumpTo(long timeToJumpTo)
        {
            if( timeToJumpTo < this.startTime )
                throw new ArgumentException(Strings.TimeToJumpToLessThanStartTime);

            avPlayer.JumpTo(timeToJumpTo);
            otherPlayer.JumpTo(timeToJumpTo);
        }

        /// <summary>
        /// Gets the current point in time, in ticks, of where playback is in the course of the recording.
        /// </summary>
        public long CurrentTime
        {
            get
            {
                // Pri2: Change this to be compatible with the use of the "playback speed" feature.
                long elapsedTime = DateTime.Now.Ticks - this.startTime;
                return this.firstStoredTick + elapsedTime;
            }
        }

        public void StopPlaying()
        {
            if( playing == false )
                return;

            if( avPlayer != null )
            {
                avPlayer.Stop();
                avPlayer = null;
            }

            if( otherPlayer != null )
            {
                otherPlayer.Stop();
                otherPlayer = null;
            }

            if( this.perfCounter != null )
            {
                perfCounter.Dispose();
            }

            lock(this)
            {
                if( rtpSession != null )
                {
                    rtpSession.Dispose();
                    rtpSession = null;
                }

                playing = false;
            }
        }
        #endregion

        #region Public Properties
        public IPEndPoint Venue
        {
            get
            {
                return venueIPE;
            }
        }
        #endregion

        #region EventHandlers
        private void EndOfStreamReached(object sender, EventArgs e)
        {
            if( playing == false )
                return;

            // If all of the streams have ended, then the entire conference is done playing.
            if( this.playing && avPlayer.StreamsPlaying == 0 && otherPlayer.StreamsPlaying == 0 )
            {
                // Stop ourselves
                this.StopPlaying();

                // Tell the Archiver object we're stopped (and thereby the client process, hopefully
                if( EndOfConferenceReached != null )
                    EndOfConferenceReached(this, EventArgs.Empty);
            }
        }
        #endregion
    }

}
