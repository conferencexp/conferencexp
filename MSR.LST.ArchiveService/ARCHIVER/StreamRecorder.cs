using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;

using MSR.LST.Net.Rtp;


namespace MSR.LST.ConferenceXP.ArchiveService
{
    /// <summary>
    /// manages a single stream..holds a number of buffers to send the frames out efficiently....
    /// </summary>
    internal class StreamRecorder
    {
        #region Private Variables
        private int                     streamID;               // ourselves from a db pov
        private RtpStream               rtpStream;              // the rtp stream
        private int                     partID;                 // participant ID of the recording participant
        private BufferRecorder[]         buffers;                // the buffers we use
        private int                     bufferCount;            // and how many we currenly have
        private int                     curBufIndex;            // and the one we're on now's index
        private BufferRecorder           currentBuffer;          // and the buffer itself
        private DateTime                lastOverflow = DateTime.Now; // the last time we overflowed

        private ConferenceRecorderPC    perfCounter;

        // Peformance data
        private int                     writtenBuffers = 0;
        private int                     receivedFrames = 0;
        private int                     droppedFrames = 0;
        private int                     writtenFrames = 0;
        private long                    receivedBytes = 0;
        #endregion

        #region EventLog
        /// <summary>
        /// A singleton event log wrapper.
        /// </summary>
        private static ArchiveServiceEventLog eventLog = null;

        private static void InitEventLog()
        {
            eventLog = new ArchiveServiceEventLog( ArchiveServiceEventLog.Source.StreamRecorder);
        }
        #endregion

        #region CTor / DCtor / Dispose
        static StreamRecorder()
        {
            InitEventLog();
        }

        /// <summary>
        /// setup the initial buffers and write the basic stream data to the db
        /// </summary>
        /// <param name="participant">for this participant</param>
        /// <param name="stream">using this rtp stream</param>
        public StreamRecorder( int participantID, RtpStream stream, ConferenceRecorderPC conferencePerfCounter )
        {
            this.rtpStream = stream;
            this.perfCounter = conferencePerfCounter;
            this.partID = participantID;

            // We do *not* re-use existing streams if a stream goes away and comes back.  This decision was made
            //  becuase it causes the client not to see the Rtcp data die & come back during playback, though it
            //  did during recording.  This inconsistency was viewed to cause problems, and was removed.
            InitStream();

            buffers = new BufferRecorder[Constants.MaxBuffers];
            bufferCount = Constants.InitialBuffers;

            for ( int i = 0; i < bufferCount; i++)
            {
                buffers[i] = new BufferRecorder( i, streamID );
                buffers[i].Overflowed += new EventHandler(this.OnOverflowException);

                perfCounter.AddInstanceForCollection(buffers[i]);
            }

            curBufIndex = 0;
            currentBuffer = buffers[curBufIndex];

            rtpStream.FrameReceived += new MSR.LST.Net.Rtp.RtpStream.FrameReceivedEventHandler(FrameReceived);
        }

        /// <remarks>
        /// This single call is temporarily in a new method because it is called from two places
        /// </remarks>
        private void InitStream()
        {
            streamID = DBHelper.CreateStream( partID, rtpStream.Properties.Name, rtpStream.PayloadType.ToString(), 
                rtpStream.Properties.GetPrivateExtensions() );
        }
        #endregion

        private void FrameReceived(object sender, RtpStream.FrameReceivedEventArgs frea)
        {
            FrameWithTicks frame = new FrameWithTicks(frea.Frame);

            /* A number of considerations caused this design...
             * 
             * Due to wanting the most accurate timestamp possible, the frame is stamped with the time
             * (by putting it in the FrameWithTicks structure), and then the ProcessFrame method is
             * called from the ThreadPool.
             * 
             * However, this caused problems because processing the frames and saving them to disk were
             * contending for time on the threadpool.  Due to the nature of the design of Archiver, it's
             * important that the frames are processed with higher priority than saving them to disk
             * (this minimizes the overhead of exceptions (cpu utilization) during high-stress periods,
             * causing the frames to be lost with the least amount of hiccups on the server.
             * 
             * So here we choose to process the frame on this thread only if we know that it won't get
             * immediate attention on the threadpool.
             */

            int workerThreads, ioThreads;
            ThreadPool.GetAvailableThreads(out workerThreads, out ioThreads);

            if( workerThreads > 0 )
            {
                ThreadPool.QueueUserWorkItem( new WaitCallback(ProcessFrame), frame );
            }
            else
            {
                Trace.WriteLine("No threads available.  Processing frame on EventThrower thread.");
                ProcessFrame(frame);
            }
        }

        private void ProcessFrame(object argument)
        {
            lock(this)
            {
                FrameWithTicks frame = (FrameWithTicks)argument;

                this.receivedBytes = this.receivedBytes + (long)frame.frame.Length;
                ++receivedFrames;

                // Verify the frame fits in a buffer.  If not, write it to the database directly.
                if( frame.frame.Length < Constants.BufferSize )
                {
                    // if there is no room in the current buffer
                    if (!currentBuffer.HasRoom(frame.frame.Length))
                    {
                        // With bufferAvailable we try to prevent writing the dropped frame event to the EventLog too often
                        //  Incidentally, that means we'll be writing once per every time we use up the buffer pool.  If we
                        //  have a sudden flood of data, that means it'll only get written once or twice, which helps perf.
                        bool bufferAvailable = false;

                        if( currentBuffer.Available )
                        {
                            currentBuffer.EnableWrite();
                            ++writtenBuffers;
                            bufferAvailable = true;
                        }

                        if (!IncrementBuffer())
                        {
                            // Discard the packet by doing nothing with it

                            if( bufferAvailable )
                            {
                                // Warn the user
                                eventLog.WriteEntry(Strings.DroppingAFrame, EventLogEntryType.Error, 
                                    ArchiveServiceEventLog.ID.FrameDropped );
                            }

                            ++droppedFrames;
                            return;
                        }
                    }

                    currentBuffer.AddFrame(frame);
                }
                else
                {
                    Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                        "Writing frame directly to disk.  Frame size: {0}", frame.frame.Length));

                    // Force pending data to be written to disk
                    if( currentBuffer.Available )
                    {
                        currentBuffer.EnableWrite();
                        ++writtenBuffers;
                    }

                    // Get our new buffer
                    IncrementBuffer();

                    // And write this frame directly to disk
                    BufferRecorder.DirectWrite(frame, this.streamID);
                }

                ++writtenFrames;
            }
        }

        private bool IncrementBuffer()
        {
            // increment the buffer we use....
            int bufIndex = (curBufIndex+1) % bufferCount;

            while (bufIndex != curBufIndex)
            {
                if (buffers[bufIndex].Available == true) 
                {
                    curBufIndex = bufIndex;
                    currentBuffer = buffers[curBufIndex];
                    return true;
                }

                bufIndex = ( bufIndex+1 ) % bufferCount;
            }

            // no buffers available so grow...
            if (bufferCount < buffers.Length)
            {
                eventLog.WriteEntry(string.Format(CultureInfo.CurrentCulture, Strings.GrowingBuffers, streamID, 
                    (bufferCount + 1)), EventLogEntryType.Warning, ArchiveServiceEventLog.ID.GrowingBuffers );

                bufIndex = bufferCount;
                bufferCount++;
                buffers[bufIndex] = new BufferRecorder (bufIndex, streamID);
                buffers[bufIndex].Overflowed += new EventHandler(this.OnOverflowException);
                perfCounter.AddInstanceForCollection( buffers[bufIndex] );

                curBufIndex = bufIndex;
                currentBuffer = buffers[curBufIndex];
                return true;
            }
            else
            {
                // Uut of buffers logged in return.  Also, we return w/o having changed which buffer is the current
                //  one b/c we expect the next buffer to be the first one to become available.
                return false;
            }
        }

        private void OnOverflowException(object sender, EventArgs ea)
        {
            lock(this)
            {
                // We've run out of space in the SQL row for this stream.  Stop using this row and then create a new stream
                //   and change the stream id in all of the buffers.  This is analagous to creating a whole new StreamRecorder
                //   for this stream, but we do it inside StreamRecorder for two reasons:
                //   - to minimize time spent not handling our incoming data (and potentially causing poor playback)
                //   - because this class manages the stream being recorded

                // If we recently overflowed, we just need this stream to re-try writing, now that
                //   its internal streamID has been changed.
                // Else we need to handle the overflow
                if( (DateTime.Now - this.lastOverflow).TotalSeconds > 2*Constants.CommandTimeout )
                {
                    Trace.WriteLine("OVERFLOW: stream overflowed 2GB of data.  Creating new stream to hold data.");

                    InitStream();

                    foreach( BufferRecorder buf in this.buffers )
                    {
                        if( buf != null )
                        {
                            buf.StreamID = this.streamID;
                        }
                    }

                    this.lastOverflow = DateTime.Now;
                }
            }
        }

        public void StopListening()
        {
            if( rtpStream != null )
            {
                rtpStream.FrameReceived -= new RtpStream.FrameReceivedEventHandler(FrameReceived);
                this.rtpStream = null;
            }

            int curIndex = curBufIndex;

            if( buffers != null )
            {
                do
                {
                    BufferRecorder buf = buffers[curBufIndex];
                    perfCounter.RemoveInstanceForCollection(buf);
                    buf.Dispose();

                    curBufIndex = (curBufIndex + 1) % bufferCount;
                }
                while ( curBufIndex != curIndex);

                this.buffers = null;
            }
        }

        #region Performance Properties

        public int BuffersWritten
        { get{return this.writtenBuffers;}  }

        public int FramesReceived
        { get{return this.receivedFrames;} }

        public int FramesWritten
        { get{return this.writtenFrames;} }

        public int FramesDropped
        { get{return this.droppedFrames;} }

        public long BytesReceived
        { get{return this.receivedBytes;} }

        #endregion
    }

    #region FrameWithTicks class
    internal class FrameWithTicks
    {
        internal FrameWithTicks(BufferChunk chunk)
        { frame = chunk; }

        internal BufferChunk frame;
        internal long ticks = DateTime.Now.Ticks;
    }
    #endregion

}
