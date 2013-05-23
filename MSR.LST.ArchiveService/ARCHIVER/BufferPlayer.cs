using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

using MSR.LST.Net.Rtp;


namespace MSR.LST.ConferenceXP.ArchiveService
{
    /// <summary>
    /// A per-stream buffer designed to refill on data (i.e. populate) when note active.
    /// </summary>
    /// <remarks>
    /// Originally, the code was setup so that the StreamPlayer could have a variable number of StreamSenders in it.
    /// After some consideration, the code was left in a state that *most* of the code could handle growing the # of
    /// senders.  However, it makes sense to have a "one sending & one buffering" model, where only two senders are
    /// needed.  If the server can't keep up, we don't attempt to buffer more data, as that's likely to only make the
    /// problem worse.
    /// </remarks>
    internal class BufferPlayer
    {
        #region Private Variables
        private int streamID;                       // the id in the db of the stream we're sending on

        private BufferChunk buffer;                 // continas the raw bytes for all the frames we hold
        private Index[] indices;                    // the indexex (frames) in the buffer
        private int indexCount;                     // the 1's based number of indexes in the buffer
        private int currentIndex;                   // the next index we should transmit
        private long startingTick;                  // the first send time in the indexes
        private long endingTick;                    // the last send time in the indexes
        private bool streamOutOfData;               // if the database returned less data than we could hold (this is the end of the rope!)

        private bool populating;                    // if we're currently populating from the db
        private bool populateOverride = false;      // if we're overriding an ongoing populate operation
        private AutoResetEvent canPopulate;         // set when we've stopped sending and need to re-fill
        private RegisteredWaitHandle waitHandle;    // waithandle of the threadpool waiting for 'canPopulate'
        private object populatingLockObj = new object();    // on object to lock on while we're populating
        #endregion

        #region EventLog
        /// <summary>
        /// A singleton event log wrapper.
        /// </summary>
        private static ArchiveServiceEventLog eventLog = null;

        private static void InitEventLog()
        {
            eventLog = new ArchiveServiceEventLog( ArchiveServiceEventLog.Source.BufferPlayer);
        }
        #endregion

        static BufferPlayer()
        {
            InitEventLog();
        }

        /// <summary>
        /// set ourselves up, we are pretty dumb so need to be told who we're sending for
        /// and where to get our data from, and when to start sending..
        /// </summary>
        /// <remarks>
        /// It seems that we don't use maxFrameSize any more.  hmm.  Remove it?
        /// </remarks>
        public BufferPlayer(int streamID, int maxFrameSize, int maxFrameCount, int maxBufferSize)
        {
            Debug.Assert( maxBufferSize >= maxFrameSize ); // just for kicks...

            buffer = new BufferChunk( maxBufferSize );
            indices = new Index[maxFrameCount+1]; // Pri3: why is this "+1" here? (DO NOT REMOVE YET)

            this.populating = false;
            this.streamOutOfData = false;
            this.streamID = streamID;

            this.currentIndex = 0;
            this.indexCount = 0;
            this.startingTick = 0;

            canPopulate = new AutoResetEvent(false);

            // pool a thread to re-populate ourselves
            waitHandle = ThreadPool.RegisterWaitForSingleObject( 
                canPopulate,
                new WaitOrTimerCallback( InitiatePopulation ), 
                this, 
                -1, // never timeout to perform a population
                false );
        }

        /// <summary>
        /// Breaks down the buffer player into a state where it can no longer be used.
        /// </summary>
        public void StopSending()
        {
            waitHandle.Unregister(null);
            canPopulate.Close();
        }

        /// <summary>
        /// Begins an asynchronous population of the buffer.
        /// </summary>
        public void EnablePopulation( long startingTick )
        {
            EnablePopulation(startingTick, false);
        }

        /// <summary>
        /// Begins an asynchronous population of the buffer.
        /// </summary>
        /// <param name="overridePendingOperations">
        /// Stops an ongoing population operation.
        /// </param>
        public void EnablePopulation( long newStartingTicks, bool overridePendingOperations )
        {
            if( !this.populating || overridePendingOperations )
            {
                this.populating = true;
                
                // signal an ongoing populate operation to end
                this.populateOverride = true; 

                // wait for an ongoing populate operation to end
                lock( this.populatingLockObj )
                {
                    this.startingTick = newStartingTicks;
                    canPopulate.Set();
                    this.populateOverride = false;
                }
            }
        }

        /// <summary>
        /// Called by the threadpool thread when canPopulate.Set is called.
        /// </summary>
        private void InitiatePopulation( object o, bool timedOut )
        {
            Debug.Assert( !timedOut ); // I don't think this ever happens, but just to be safe...

            if ( !timedOut )
            {
                if( populating == false )
                {
                    // (pbristow) Why does this happen???  (as of 15-Sep-04, I can't recall having seen this happen.  did it ever?)
                    eventLog.WriteEntry(Strings.PopulateCalledOnSenderError, EventLogEntryType.Error, 
                        ArchiveServiceEventLog.ID.ImproperPopulateCall);
                }
                else
                {
                    // lock to ensure we'e getting the correct startingTick value
                    lock( populatingLockObj )
                    {
                        // While Populate takes starting ticks as a param, we can use the instance variable we set in EnablePopulation
                        Populate(this.startingTick);
                    }
                }
            }
        }

        /// <summary>
        /// Fills the index & byte buffers with new data from the database.
        /// </summary>
        public void Populate( long newStartingTicks )
        {
            populating = true;

            // Use a lock to prevent overwriting startingTick during an override operation
            lock( populatingLockObj )
            {
#if TIMING
                long startTimer = DateTime.Now.Ticks;
#endif

                // Try to avoid expensive database operatons (both of them)
                if( populateOverride ) // RETURN and leave populate as true
                    return;

                // load the indexes from the database
                streamOutOfData = DBHelper.LoadIndices(indices, newStartingTicks, streamID, buffer.Buffer.Length, out indexCount);
                currentIndex = 0;
            
                // if there are indicies, load the raw byte data for them
                if ( indexCount > 0 )
                {
                    // Try to avoid expensive database operatons
                    if( populateOverride ) // RETURN and leave populate as true
                        return;

                    this.startingTick = indices[0].timestamp;
                    this.endingTick = indices[indexCount-1].timestamp;

                    DBHelper.LoadBuffer(streamID, indices[0].start, indices[indexCount-1].end, ref buffer);

                    Trace.WriteLine(String.Format(CultureInfo.InvariantCulture, 
                        "Loaded buffer (ID: {0}) with: {1} sec, {2} bytes, {3} frames", 
                        streamID, ((double)(endingTick - startingTick) / 10000000.0), 
                        (indices[indexCount -1].end - indices[0].start + 1), indexCount) );
                }
                else
                {
                    // Populate these two variables with some meaningful data
                    this.startingTick = newStartingTicks;
                    this.endingTick = newStartingTicks;
                }
                
#if TIMING
                long takenTime = DateTime.Now.Ticks - startTimer;
                Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "TIMING: Load buffer took {0} ms", 
                    (takenTime / Constants.TicksPerMs)));
#endif

                populating = false;
            }
        }

        /// <summary>
        /// Jumps to a time in the buffer, skipping or resending frames as necessary the next time
        /// SendFrames is called.
        /// </summary>
        public void JumpToPointInBuffer (long timeToJumpTo)
        {
            Debug.Assert (this.startingTick <= timeToJumpTo && timeToJumpTo <= this.endingTick);

            // Reset back to the beginning
            this.currentIndex = 0;

            // Use the same while loop that's in SendFrames to iterate through the frames
            while (currentIndex < indexCount && indices[currentIndex].timestamp < timeToJumpTo)
            {
                ++currentIndex;
            }
        }

        /// <summary>
        /// Send all the frames that should be sent up to this point in time.
        /// </summary>
        /// <param name="bytesSent">Number of bytes sent.</param>
        /// <param name="cumulativeLateness">Sum of temporal disparities over all frames sent.</param>
        /// <param name="firstStoredTick">The 'start' time on the first index in this whole stream.</param>
        /// <param name="sender">The RtpSender to send data on.</param>
        /// <param name="startTimeTicks">The start time of sending, in ticks.</param>
        /// <param name="timeUntilFrame">The temporal distance between the current frame and the next one to be sent, in ticks.</param>
        /// <returns>Number of frames sent.</returns>
        public int SendFrames( RtpSender sender, long timeBoundary, out long timeUntilFrame, ref long totalBytesSent, ref long cumulativeLateness )
        {
            if( this.populating )
                throw new InvalidOperationException(Strings.BufferplayerSendframesError);

            int framesSent = 0;

            try
            {
                while ( currentIndex < indexCount && indices[currentIndex].timestamp <= timeBoundary )
                {
                    long startTimer = DateTime.Now.Ticks;

                    int frameLength = 1 + indices[currentIndex].end - indices[currentIndex].start;

                    if ( frameLength > 0)
                    {
                        // Calculate how late the frame will be
                        long lateness = (timeBoundary - indices[currentIndex].timestamp) / Constants.TicksPerMs;
                        if( lateness > Constants.TicksSpent )
                            Trace.WriteLine(String.Format(CultureInfo.InvariantCulture,
                                "--- FRAME LATENESS OF: {0} ms", lateness));
                        cumulativeLateness += lateness;

                        // Send Frame
                        buffer.Reset( indices[currentIndex].start - indices[0].start, frameLength );
                        sender.Send( buffer );
                    }
                    else
                    {
                        // (pbristow) Why would this happen???
                        Debug.Fail("Frame of length zero found.");
                    }

                    totalBytesSent += frameLength;
                    ++framesSent;
                    ++currentIndex;

                    long takenTime = DateTime.Now.Ticks - startTimer;
                    if( takenTime > Constants.TicksSpent )
                        Trace.WriteLine(String.Format(CultureInfo.InvariantCulture,
                            "TIME WASTED TO SEND A FRAME: {0} ms, ID: {1}, bytes: {2}",
                            (takenTime / Constants.TicksPerMs), streamID, frameLength));
                }

                if ( currentIndex == indexCount )
                {
                    timeUntilFrame = 0;
                }
                else
                {
                    timeUntilFrame = (indices[currentIndex].timestamp - timeBoundary);
                }
            }
            catch(ObjectDisposedException)
            {
                // Sender is disposed by Stop being called
                timeUntilFrame = long.MaxValue;
            }

            return framesSent;
        }

        #region Public Properties
        /// <summary>
        /// If we've sent all of the data in our buffer;
        /// </summary>
        public bool Empty 
        { get { return (currentIndex == indexCount); } }

        /// <summary>
        /// The last 'ticks' (i.e. "end time") of the data currently loaded in our buffer.
        /// </summary>
        public long LastTick 
        { 
            get 
            {
                if( this.populating )
                    throw new InvalidOperationException(Strings.LastTicksIsTransientError);

                return this.endingTick;
            } 
        }

        /// <summary>
        /// The first 'ticks' (i.e. "start time") of the data currently loaded in our buffer.
        /// </summary>
        public long FirstTick 
        { get { return startingTick; } }

        // For debugging
        public string LastTime
        { get { return new DateTime(LastTick).ToString("T", CultureInfo.InvariantCulture); } }

        // Also for debugging
        public string FirstTime
        { get { return new DateTime(FirstTick).ToString("T", CultureInfo.InvariantCulture); } }

        /// <summary>
        /// If we're currently retreiving data from the server.
        /// </summary>
        public bool Populating 
        { get { return populating; } }

        /// <summary>
        /// If this buffer contains the last frames in the stream (not the same as containing no streams at all!)
        /// </summary>
        public bool EndOfStream
        { get { return streamOutOfData; } }
        #endregion

    }

}
