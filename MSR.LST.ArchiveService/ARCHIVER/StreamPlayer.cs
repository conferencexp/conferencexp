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
    /// Plays a group of streams on a dedicated thread.
    /// </summary>
    /// <remarks>
    /// This class enables you to send certain streams using one thread and other streams on another, so
    /// that the delay in, for instance, RTDocuments streams due to large sends does not affect the quality
    /// of Audio/Video streams.
    /// </remarks>
    internal class StreamsGroupPlayer
    {
        #region Private Variables
        private ArrayList players = null;               // StreamPlayers that we hold and are playing
        private Thread playbackThread = null;           // Thread calling all the playback stuff
        private long firstStoredTick = 0;               // First stored 'ticks' in all the streams
        private long startTimeTicks = 0;                // Start time of playing.
        private bool didTimeJump = false;               // Whether we did a time jump and need to skip sleeping next time
        #endregion

        #region CTor
        public StreamsGroupPlayer(ArrayList streams, long firstGlobalStoredTick, long startTime)
        {
            this.players = streams;
            this.firstStoredTick = firstGlobalStoredTick;
            this.startTimeTicks = startTime;

            // Start the thread that plays all of the streams
            this.playbackThread = new Thread(new ThreadStart(this.RunPlaybackThread));
            playbackThread.Name = "Playback";
            playbackThread.IsBackground = true;
            playbackThread.Priority = ThreadPriority.AboveNormal;

            //Language override
            if (Constants.UICulture != null) {
                try {
                    System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(Constants.UICulture);
                    playbackThread.CurrentUICulture = ci;
                }
                catch { }
            }

            playbackThread.Start();
        }
        #endregion

        #region Public Methods & Properties
        public void RemoveStream (StreamPlayer stream)
        {
            lock(players)
            {
                players.Remove(stream);
            }
        }

        public int StreamCount
        {
            get
            {
                return players.Count;
            }
        }

        public int StreamsPlaying
        {
            get
            {
                int playingCnt = 0;
                foreach (StreamPlayer player in players)
                {
                    if (!player.StreamEnded)
                        ++playingCnt;
                }

                return playingCnt;
            }
        }

        /// <summary>
        /// Jumps to a specific time in the set of streams being played.
        /// </summary>
        public void JumpTo (long timeToJumpTo)
        {
            // First, ensure that we don't waste time on the dedicated thread sleeping
            this.didTimeJump = true;

            // Stop sending and iterating through the senders
            lock (players)
            {
                foreach(StreamPlayer player in players)
                {
                    player.JumpTo(timeToJumpTo);
                }
            }

            // Change startTimeTicks to "trick" the StreamsGroupPlayer into playing from a different point
            long newElapsedTicks = timeToJumpTo - this.firstStoredTick;
            this.startTimeTicks = DateTime.Now.Ticks - newElapsedTicks;
        }

        public void Stop()
        {
            if( playbackThread != null )
            {
                playbackThread.Abort();
                playbackThread.Interrupt();
                playbackThread.Join();
                playbackThread = null;
            }

            if( players != null )
            {
                lock (players)
                {
                    foreach( StreamPlayer player in players )
                        player.Stop();
                    players = null;
                }
            }
        }
        #endregion

        #region Private Methods
        private void RunPlaybackThread()
        {
            try
            {
                while (true)
                {
                    // This is here to make it easy to add a feature to change the playback speed.
                    // NOTE: we're broken in two places in ConferencePlayer regarding this.
                    float playbackSpeed = 1.0F;

                    // Keep track of the latest time we should sleep until after this round of frame sends
                    long shortestSleepTime = Constants.LongestSleepTime;

                    lock (players)
                    {
                        foreach(StreamPlayer player in players)
                        {
                            if( player.StreamEnded )
                                continue;

                            long elapsedTicks = DateTime.Now.Ticks - startTimeTicks;
                            long movingTimeBoundary = firstStoredTick + (long)(playbackSpeed * elapsedTicks);

                            long timeUntilFrame; // the time the current player can sleep until

#if TIMING
                            long startTimer = DateTime.Now.Ticks;
#endif
                            timeUntilFrame = player.OnWakeUp(movingTimeBoundary);
#if TIMING
                            long takenTime = DateTime.Now.Ticks - startTimer;
                            if( takenTime > Constants.TicksSpent )
                            Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, 
                                "TIMING: TIME WASTED IN OnWakeUp: {0} ms", (takenTime / Constants.TicksPerMs)));
#endif

                            if ( timeUntilFrame < shortestSleepTime )
                                shortestSleepTime = timeUntilFrame;
                        }
                    }

                    // If we recently did a time jump, our sleep time info could be wrong.  Don't sleep.
                    if (!this.didTimeJump)
                    {
                        long sleepTime = shortestSleepTime / Constants.TicksPerMs;
                        sleepTime = (sleepTime >= 0) ? sleepTime : 0; // make sure we're not trying to sleep < 0 ms
                        Thread.Sleep((int)sleepTime);
                    }
                    else
                    {
                        this.didTimeJump = false;
                    }
                }
            }
            catch( ThreadAbortException )
            { }
        }
        #endregion
    }


    /// <summary>
    /// Plays an individual stream back.
    /// </summary>
    internal class StreamPlayer 
    {
        #region Private Variables
        private int streamID;
        private RtpSession rtpSession;                  // the session for the whole ConferencePlayer
        private RtpSender rtpSender;                    // the sender for this stream
        private BufferPlayer[] buffers;                 // the array of buffers for this stream
        private ConferencePlayerPC perfCounter;         // perf counter; needs to know of all buffers created.
        private Hashtable privExtns;                    // "Private extensions" for the RtpStream
        private string cname, name;                     // info about this stream;
        private PayloadType streamPayload;

        private long firstStreamTicks;                  // the first time this stream sends a frame (recorded time, not local)
        private int activeBufferIndex;                  // the index of the buffer currently sending
        private BufferPlayer activeBuffer;              // ...and the buffer itself
        private bool currentStreamEnded;                // whether we've reached the end of hte stream & stopped playing
        private bool outOfDataLoggedFlag;               // if we were out of data, last time we checked
        private bool createSenderFired;                 // whether we've fired the work item to create the sender

        private int emptyErrors;                        // total times we've run out of data waiting on DB
        private int totalFramesSent;                    // total frames sent in this stream
        private long totalBytesSent;                    // total bytes sent in this stream
        private long totalLateness;                     // sum of all of the temporal disparities in frame sends (in ms)
        #endregion

        #region EventLog
        /// <summary>
        /// A singleton event log wrapper.
        /// </summary>
        private static ArchiveServiceEventLog eventLog = null;

        private static void InitEventLog()
        {
            eventLog = new ArchiveServiceEventLog( ArchiveServiceEventLog.Source.StreamPlayer);
        }
        #endregion

        #region CTor
        static StreamPlayer()
        {
            InitEventLog();
        }

        /// <summary>
        /// get details of the stream...raw data and start tiume etc
        /// set up the stream senders associated with us. we have n, one active and the rest acting as buffers
        /// </summary>
        public StreamPlayer( RtpSession session, int newStreamID, ConferencePlayerPC cppc)
        {
            int maxFrameSize, maxFrameCount, maxBufferSize;

            // This occasionally throws due to bad data.  Let ConferencePlayer handle it.
            DBHelper.GetStreamStatistics( newStreamID, out this.firstStreamTicks, out maxFrameSize, out maxFrameCount, out maxBufferSize);

            streamID = newStreamID;
            totalFramesSent = 0;
            totalBytesSent = 0;
            totalLateness = 0;
            buffers = new BufferPlayer[Constants.BuffersPerStream];
            perfCounter = cppc;

            string payload;
            DBHelper.GetStreamAndParticipantDetails( streamID, out name, out payload, out cname, out privExtns );
            streamPayload = (PayloadType)Enum.Parse( typeof(PayloadType), payload, true );

            // for delayed buffers (late joiners), we create the rtpSender later
            this.rtpSession = session;

            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                "Playing back stream: {0}; payload: {1}; name: {2} : {3}", streamID, streamPayload, cname, name));

            // buffer n buffers worth of data
            long startingTick = this.firstStreamTicks;
            for ( int i = 0; i < buffers.Length; i++)
            { 
                buffers[i] = new BufferPlayer( streamID, maxFrameSize, maxFrameCount, maxBufferSize);
                buffers[i].Populate( startingTick );
                startingTick = buffers[i].LastTick + 1;

                perfCounter.AddInstanceForCollection(buffers[i]);
            }

            // first stream is initially active
            activeBufferIndex = 0;
            activeBuffer = buffers[activeBufferIndex];
        }
        #endregion

        #region Public Properties & Events
        /// <summary>
        /// Fires when all of the data for this stream has been played out and it's time for the stream to be disposed.
        /// </summary>
        static public event EventHandler EndOfStreamReached;

        public bool StreamEnded
        { get {return this.currentStreamEnded;} }

        public long BytesSent
        { get {return this.totalBytesSent;} }

        public int FramesSent
        { get {return this.totalFramesSent;} }

        public int BuffersEmptyErrors
        { get {return this.emptyErrors;} }

        public long LatenessSum
        { get {return this.totalLateness;} }

        public long FirstStreamsTicks
        { get {return this.firstStreamTicks;} }

        public PayloadType Payload
        { get {return this.streamPayload;} }
        #endregion

        #region Other Public Methods (OnWakeUp, JumpTo, Stop)
        /// <summary>
        /// Sends all of the frames this stream needs to send by the given time boundary.
        /// Also calls populate on the underlying buffer as necessary
        /// </summary>
        /// <returns>the time of the next frame to be sent, in ticks</returns>
        public long OnWakeUp(long timeBoundary)
        {
            if( this.currentStreamEnded )
                return long.MaxValue;

#if TIMING
            long startTimer, takenTime;
#endif

            // We must return the time until the next frame.  Keep up with it via this variable.
            long timeUntilFrame = 0;

            // Allowing the rtpSender to be created late allows the playback to properly replicate the situation seen
            // during recording.  "Late joiner" support, if you will.  This also deals with the underlying 500ms delay
            // implanted into RtpSession.CreateRtpSender.

            if( rtpSender != null )
            {
#if TIMING
                startTimer = DateTime.Now.Ticks;
#endif

                #region Send Frames
                if( !activeBuffer.Populating && !activeBuffer.Empty )
                {
                    if( this.outOfDataLoggedFlag )
                        this.outOfDataLoggedFlag = false;

                    totalFramesSent += activeBuffer.SendFrames( rtpSender, timeBoundary, out timeUntilFrame, ref totalBytesSent, ref totalLateness );
                }
                #endregion 

#if TIMING
                takenTime = DateTime.Now.Ticks - startTimer;
                if( takenTime > Constants.TicksSpent )
                Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, 
                    "TIMING: TIME WASTED SENDING FRAMES: {0} ms", (takenTime / Constants.TicksPerMs)));

                startTimer = DateTime.Now.Ticks; 
#endif

                #region Empty Buffer
                if ( activeBuffer.Empty )
                {
                    if( !activeBuffer.EndOfStream ) // not the end of the stream, so we go get new data to play out
                    {
                        // guaranteed atomic
                        BufferPlayer oldBuffer = activeBuffer;

                        // Get the next buffer
                        int newBufferIndex = ( activeBufferIndex+1 ) % Constants.BuffersPerStream;
                        BufferPlayer newBuffer = buffers[newBufferIndex];

                        if ( !newBuffer.Populating ) // we can't enable population on the old buffer until we can get the 'LastTick' from the new buffer
                        {
                            if( !newBuffer.EndOfStream ) // if we're at the end of the stream, don't enable population on the old buffer
                            {
                                // Broken here so we only work with 2 buffers
                                oldBuffer.EnablePopulation( newBuffer.LastTick + 1 );
                            }
                        }
                        else // we're in a performance rut and have run out of data
                        {
                            // the "outOfDataLoggedFlag" prevents us from event-logging too much
                            if( !outOfDataLoggedFlag )
                            {
                                // The database can't keep up; both buffers are empty!
                                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, 
                                    Strings.OnWakeUpNoDataError, streamID), EventLogEntryType.Warning, 
                                    ArchiveServiceEventLog.ID.EmptyBuffersInPlayback);

                                this.emptyErrors++;
                                this.outOfDataLoggedFlag = true;
                            }
                        }

                        timeUntilFrame = 0; // we need to come back quick to see when the first frame is in the next buffer

                        // save the "new" buffer as the "current" one
                        activeBufferIndex = newBufferIndex;
                        activeBuffer = buffers[newBufferIndex];
                    }
                    else // end of stream
                    {
                        if( !currentStreamEnded )
                        {
                            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                                "End of stream reached.  Stopping sending this stream.  ID: {0}", streamID));

                            currentStreamEnded = true;

                            DisposeSender();

                            ThreadPool.QueueUserWorkItem(new WaitCallback(FireEndOfStream), this);
                        }

                        timeUntilFrame = long.MaxValue;
                    }
                }
                #endregion
                
#if TIMING
                takenTime = DateTime.Now.Ticks - startTimer;
                if( takenTime > Constants.TicksSpent )
                Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, 
                    "TIMING: TIME WASTED ON EMPTY BUFFER: {0} ms", (takenTime / Constants.TicksPerMs)));
#endif
            }
            else // Sender isn't created yet.  Check if we need to.
            {
#if TIMING
                startTimer = DateTime.Now.Ticks;
#endif

                // Pri2: Change this to be compatable with use of the "playback speed" feature.  TimeBoundary is speed-based...
                timeUntilFrame = (firstStreamTicks - timeBoundary);

                #region Sender creation
                if( timeUntilFrame <= Constants.SenderCreationLeadTime ) // <x> ms of "prep time" to get fired up
                {
                    if( !createSenderFired )
                    {
                        createSenderFired = true;
                        Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                            "RtpSender being created for stream: {0}", streamID));
                        ThreadPool.QueueUserWorkItem(new WaitCallback(CreateSender));
                    }
                }
                else
                {
                    timeUntilFrame -= Constants.SenderCreationLeadTime;
                }
                #endregion
                
#if TIMING
                takenTime = DateTime.Now.Ticks - startTimer;
                if( takenTime > Constants.TicksSpent )
                Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, 
                    "TIMING: TIME WASTED CREATING SENDER: {0} ms", (takenTime / Constants.TicksPerMs)));
#endif
            }

            return timeUntilFrame;
        }

        /// <summary>
        /// Creates the RtpSender for this stream asynchronously.  This causes the 500ms Thread.Sleep that occurs in
        /// RtpSession.CreateRtpSender to not cause performance problems during playback.
        /// </summary>
        private void CreateSender(object stateVar)
        {
            // For multi-threading safety purposes, we create the sender before assigning the new sender to the class variable.
            RtpSender newSender = rtpSession.CreateRtpSender(string.Format(CultureInfo.CurrentCulture, 
                Strings.Playback, cname, name), this.streamPayload, this.privExtns);
            this.rtpSender = newSender;
        }

        private void DisposeSender()
        {
            // Pri2: Account for the case that the sender creation has been fired, but the sender hasn't yet been created.
            if (rtpSender != null)
            {
                this.rtpSender.Dispose();
                this.rtpSender = null;
            }

            this.createSenderFired = false;
        }

        /// <summary>
        /// Skips this stream to a given time point, repopulating or adjusting buffers as necessary.
        /// </summary>
        public void JumpTo (long timeToJumpTo)
        {
            // We may need to "restart" after this jump, so don't pretend to be stopped:
            this.currentStreamEnded = false;

            // Find out if any of the buffers contain this time point
            int usefulBuffer = -1;
            for( int cnt = 0; cnt < buffers.Length; ++cnt )
            {
                BufferPlayer buffer = buffers[0];
                if( !buffer.Populating ) // If the buffer is populating, we can't read Ticks from it
                {
                    if( buffer.FirstTick <= timeToJumpTo && timeToJumpTo <= buffer.LastTick )
                    {
                        usefulBuffer = cnt;
                        break;
                    }
                }
            }

            // Setup the active buffer
            if( usefulBuffer != -1 )
            {
                this.activeBufferIndex = usefulBuffer;
                this.activeBuffer = buffers[usefulBuffer];

                this.activeBuffer.JumpToPointInBuffer (timeToJumpTo);
            }
            else
            {
                // do this synchronously so that we can populate the next buffer
                this.activeBuffer.Populate (timeToJumpTo);
            }

            // See if we need to dispose our sender because it's not in use
            //  Double the SenderCreationLeadTime, as we don't want to dispose a sender just to create a new one momentarily
            if( timeToJumpTo < this.firstStreamTicks - 2*Constants.SenderCreationLeadTime )
            {
                DisposeSender();
            }

            // Make sure we have at least one other buffer with good data in it
            int newBufferIndex = (activeBufferIndex+1) % Constants.BuffersPerStream;
            BufferPlayer newBuffer = buffers[newBufferIndex];
            newBuffer.EnablePopulation (activeBuffer.LastTick + 1, true);
        }

        /// <summary>
        /// Asynchronously declares the end of the stream to the ConferencePlayer object via an event.
        /// </summary>
        private void FireEndOfStream(object sender)
        {
            EndOfStreamReached(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Stop the stream from sending; close buffers.
        /// </summary>
        public void Stop()
        {
            try
            {
                for ( int i = 0; i < buffers.Length; i++)
                {
                    perfCounter.RemoveInstanceForCollection(buffers[i]);
                    buffers[i].StopSending();
                }

                DisposeSender();
            }
            catch(Exception ex)
            {
                // (pbristow) Any reason to expect this to happen???
                Trace.Fail(string.Format(CultureInfo.CurrentCulture, 
                    "ERROR: Exception in StreamPlayer.Stop() - {0}", ex.ToString()));
            }
        }
        #endregion
    }
}
