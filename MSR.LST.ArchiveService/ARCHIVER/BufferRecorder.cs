using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;


namespace MSR.LST.ConferenceXP.ArchiveService
{
    /// <summary>
    /// Represents an individual write buffer for a collection of frames
    /// manages the raw data and the indexes into that data
    /// </summary>
    internal class BufferRecorder : IDisposable
    {
        #region Private Variables
        private int             number ;        // diagnostic support. so we can find out which buffer screws up
        private Index[]         indices;        // array of indices into the buffer
        private int             currentIndex;   // what's the current index
        private bool            isAvailable;    // is this buffer available to write
        private int             bufferIndex;    // where we are at in the buffer
        private byte[]          buffer;         // the buffer itself
        private int             streamID;       // the stream id for this buffer
        private AutoResetEvent  canSave;        // ready to save
        private bool            writing = false;

        private RegisteredWaitHandle waitHandle;
        public event EventHandler Overflowed;
        #endregion

        #region EventLog
        /// <summary>
        /// A singleton event log wrapper.
        /// </summary>
        private static ArchiveServiceEventLog eventLog = null;

        private static void InitEventLog()
        {
            eventLog = new ArchiveServiceEventLog( ArchiveServiceEventLog.Source.BufferRecorder);
        }
        #endregion

        #region CTor / DCtor / Dispose
        static BufferRecorder()
        {
            InitEventLog();
        }

        /// <summary>
        /// create a buffer recorder for a stream
        /// </summary>
        /// <param name="number">ordinal for diagnostics</param>
        /// <param name="streamID">the stream I'm for</param>
        public BufferRecorder( int number, int streamID )
        {
            isAvailable = true;

            this.number =  number;

            indices = new Index[Constants.IndexCapacity];
            currentIndex = 0;

            buffer = new byte[Constants.BufferSize];
            this.streamID = streamID;

            // setup the db writer thread
            canSave = new AutoResetEvent(false);
            // The BufferTimeout feature is "turned off" so that the buffer won't automatically write to disk sporadically.
            //  This is a nice safety feature we should try to turn back on.  It was turned off because it was saving data
            //  and causing frames to be written out of order in the DB.
            waitHandle = ThreadPool.RegisterWaitForSingleObject( canSave,
                new WaitOrTimerCallback(InitiateSave), 
                this, 
                Constants.BufferTimeout,
                false);

            Thread.Sleep(50); // Allows for the Registration to occur & get setup
            // Without the Sleep, if we walk into a massive session with lots of data moving, we get slammed & can't keep up.
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Flush();
        }

        ~BufferRecorder()
        {
            Flush();
        }
        #endregion

        #region Public Properties
        public bool Available
        {
            get { return isAvailable; }
        }

        public int StreamID
        {
            get { return this.streamID; }
            set { this.streamID = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Do we have room to take another frame
        /// </summary>
        /// <param name="size">size of the frame we want to add</param>
        /// <returns>true if we do, false if we don't</returns>
        public bool HasRoom(int size)
        {
            lock(this)
            {
                if( !this.isAvailable )
                    return false;

                if( this.currentIndex+1 >= indices.Length )
                    return false;

                if ( bufferIndex + size > buffer.Length )
                    return false;

                return true;
            }
        }

        /// <summary>
        /// add an incoming frame to the buffer...assumes we have room
        /// </summary>
        /// <param name="data">what to add</param>
        public void AddFrame( FrameWithTicks frame )
        {
            lock(this)
            {
                if( !HasRoom( frame.frame.Length ) )
                    throw new InvalidOperationException(Strings.AddFrameCalledError);

                long ticks = frame.ticks;

                indices[currentIndex].start     = bufferIndex;
                indices[currentIndex].end       = bufferIndex + frame.frame.Length-1;
                indices[currentIndex].timestamp = ticks;

                // (jeffb) Handle two frames within a 100ns interval (surely this doesn't happen?)
                // (pbristow) This does happen as recording fires up, as there are frames waiting on the queue while the process is "ramping up"
                // NOTE: The next note can be disregarded, as the first 'if' below fixes it (with a poor hack, but one that works, nonetheless.
                // NOTE: this breaks from one BufferRecorder to another.  If the last frame in this buffer and the first frame in the next 
                //   buffer arrive simultaneously, the timestamp could be identical.  So incredibly small of a possibility we don't deal w/ it.
                if ( currentIndex == 0 )
                    indices[currentIndex].timestamp += 1;
                else if ( indices[currentIndex].timestamp <= indices[currentIndex-1].timestamp )
                    indices[currentIndex].timestamp = indices[currentIndex-1].timestamp +1;

                Array.Copy(frame.frame.Buffer, frame.frame.Index, buffer, bufferIndex, frame.frame.Length);
                currentIndex++;
                bufferIndex += frame.frame.Length;
            }
        }

        /// <summary>
        /// mark that this buffer is ok to write to the db
        /// </summary>
        public void EnableWrite()
        {
            lock(this)
            {
                // If we're available, write.  If not, we're already writing (return "KEEP YOUR PANTS ON!" :)
                if( this.isAvailable )
                {
                    Trace.WriteLine(String.Format(CultureInfo.InvariantCulture,
                        "BufferRecorder::EnableWrite called on buffer {0}, stream {1} at {2}", 
                        number, streamID, DateTime.Now.Ticks));
                    this.isAvailable = false;
                    canSave.Set();
                }
            }
        }
        #endregion

        #region DirectWrite (public, static)
        public static void DirectWrite( FrameWithTicks frame, int streamID )
        {
            Trace.WriteLine(String.Format(CultureInfo.InvariantCulture, 
                "BufferRecorder::DirectWrite called with frame of size {0} bytes and steamID {1}.",
                frame.frame.Length, streamID));

            Index ind = new Index();
            ind.start = 0;
            ind.end = frame.frame.Length-1;
            ind.timestamp = frame.ticks;

            DBHelper.SaveBufferAndIndices(streamID, new Index[]{ind}, 1, (byte[])frame.frame);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// called when we're shutting down to flush partially filled buffers.
        /// </summary>
        private void Flush()
        {
            Trace.WriteLine(String.Format(CultureInfo.InvariantCulture, 
                "BufferRecorder::Flush called on buffer {0}, stream {1}.", number, streamID));

            if( canSave != null )
            {
                waitHandle.Unregister(canSave);
                canSave.Close();
                canSave = null;
            }

            if( isAvailable )
            {
                isAvailable = false;
                WriteData();
            }
        }

        /// <summary>
        /// Thread callback to start the save. Triggered off the cansave AutoResetEvent
        /// </summary>
        private void InitiateSave( object o, bool timedOut )
        {
            Trace.WriteLine(String.Format(CultureInfo.InvariantCulture, 
                "BufferRecorder::InitiateSave called on buffer {0}, stream {1}.", number, streamID));

            if( timedOut )
            {
                Trace.WriteLine("BufferRecorder::InitiateSave due to timeout.");
                this.isAvailable = false;
            }

            WriteData();
        }

        /// <summary>
        /// Writes the buffer to SQL and empties it.
        /// </summary>
        private void WriteData()
        {
            lock( this )
            {
                // This and the lock are to circumvent a small multithreading dillema
                if( this.Available )
                {
                    Trace.WriteLine(String.Format(CultureInfo.InvariantCulture, 
                        "BufferRecorder::WriteData called when Available at time {0}.  Returning.", 
                        DateTime.Now.Ticks));
                    return;
                }

                // Going to try a spin-wait just to see if we can resolve this multithreading problem
                while( this.writing )
                     Thread.Sleep(10);

                writing = true;
                bool overflowed;

                do
                {
                    overflowed = false;

                    try
                    {
                        // Don't try to write an empty buffer
                        if( currentIndex > 0 )
                        {
#if TIMING
                            long startTimer = DateTime.Now.Ticks;
#endif

                            // TODO: Find out why this gets called twice on one data set OR find a workaround.
                            Trace.WriteLine(String.Format(CultureInfo.InvariantCulture, 
                                "BufferRecorder::WriteData doing SBAI on buffer {0}, stream {1} at {2}", 
                                number, streamID, DateTime.Now.Ticks));
                            DBHelper.SaveBufferAndIndices( 
                                this.streamID,
                                this.indices,
                                this.currentIndex,
                                buffer );
                            
#if TIMING
                            long takenTime = DateTime.Now.Ticks - startTimer;
                            Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "TIMING: SBAI took {0} ms", 
                                (takenTime / Constants.TicksPerMs)));
#endif

                        }
                    }
                    catch(SqlException ex)
                    {
                        // Catch the overflow case, where we have more data than the 'int' type holds
                        if( ex.Message.ToLower(CultureInfo.InvariantCulture).IndexOf("overflow") >= 0 )
                        {
                            overflowed = true; // by setting this, we directly try to write the data again
                            this.Overflowed(this, EventArgs.Empty);
                        }

                        // Two exceptions are seen here:
                        //   Timeouts in SQL due to taking a *really* bad performance beating
                        //   OR Constraint violation in Frame table due to unusual multithreading problem not yet solved (mentioned above)
                        // Dont do anything, b/c failed DB ops are already event-logged.  Just move on & hope that the stream is playable.
                    }
                    catch(InvalidOperationException ex)
                    {
                        // In the worst of performance cases, we run out of pooled connections and get this exception
                        eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.DatabaseOperationFailedError,
                            ex.ToString()), EventLogEntryType.Error, ArchiveServiceEventLog.ID.DBOpFailed);

                        // Again, ignore & move on, dropping the frames.
                    }
                } // In the specific case where we've overflowed, try again immediately (to hopefully preempt other buffers from going out-of-order)
                while (overflowed);

                writing = false;
                isAvailable = true;

                bufferIndex = 0;
                currentIndex = 0;

                Trace.WriteLine("BufferRecorder::WriteData completed.");
            }
        }

        #endregion
    }
}
