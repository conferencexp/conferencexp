using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

using MSR.LST;


namespace MSR.LST.ConferenceXP.ArchiveService
{
    #region Install/Uninstall
    internal abstract class PCInstaller: BasePCInstaller
    {
        private static Type[] counterTypes = new Type[]
        {
            typeof(ConferenceRecorderPC),
            typeof(ConferencePlayerPC)
        };

        internal static void Install()
        {
            Install(counterTypes);
        }

        internal static void Uninstall()
        {
            Uninstall(counterTypes);
        }
    }
    #endregion

    #region Performance Counter Classes

    /// <summary>
    /// Contains performance counter data for the ConferenceRecorder class, as well as doing the data collection
    /// </summary>
    /// <remarks>
    /// This class was modified from its predecessor in Msr.Lst.Net.Rtp such that it does the data collection,
    /// abstracting that away from the classes it collects data from.
    /// </remarks>
    internal class ConferenceRecorderPC : BasePC, IDisposable
    {
        #region Statics
    
        private const string categoryName = "Archive Recorder";

        // Counter names, must be kept in sync with ID enum or it won't compile
        private static readonly string[] counterNames = new string[(int)ID.Count]
            {
                "Bytes Recv'd",
                "Bytes Recv'd/sec",
                "Frames Recv'd",
                "Frames Recv'd/sec",
                "Frames Written",
                "Frames Written/sec",
                "Frames Dropped",
                "Frames Dropped/sec",
                "Buffers Written",
                "Buffers Written/sec",
                "Buffers Created/sec",
                "Buffers, Total",
                "Buffers Available",
                "Buffers Available %"
            };

        internal enum ID
        {
            BytesRecvd,
            BytesRecvdPerSec,
            FramesRecvd,
            FramesRecvdPerSec,
            FramesWritten,
            FramesWrittenPerSec,
            FramesDropped,
            FramesDroppedPerSec,
            BuffersWritten,
            BuffersWrittenPerSec,
            BuffersCreatedPerSec,
            TotalBuffers,
            BuffersAvailable,
            PercentBufAvailable,

            // This one needs to be last
            Count
        }

        
        #endregion Statics
        
        #region Constructors

        // Called by ConferenceRecorder
        internal ConferenceRecorderPC(string instanceName) : base(categoryName, counterNames, instanceName) 
        {
            this.collectionTimer = new Timer(new TimerCallback(this.PerformDataCollection), null, (int)collectionPeriod, (int)collectionPeriod);
        }
        
        #endregion Constructors

        #region Data Collection
        
        #region Methods
        /// <summary>
        /// Accepts an instance of a class on which data is collected for this performance counter category.
        /// </summary>
        internal void AddInstanceForCollection(BufferRecorder bm)
        {
            lock(buffers)
            {
                if( !buffers.Contains(bm) )
                    buffers.Add(bm);
            }
        }

        internal void AddInstanceForCollection(StreamRecorder sm)
        {
            lock(streams)
            {
                if( !streams.Contains(sm) )
                    streams.Add(sm);
            }
        }

        //
        // If data is collected from ConferenceRecorder, or some other class,
        // add an overloaded AddInstanceForCollection method here.
        //
        // Note: You could, if you wanted, make AddInstanceForCollection accept an object,
        // but that would be less performant (and this is a *Performance*Counter... :)
        //

        /// <summary>
        /// Remove an instance from the set of classes on which data is collected.
        /// </summary>
        internal void RemoveInstanceForCollection(BufferRecorder bm)
        {
            lock(buffers)
            {
                if( buffers.Contains(bm) )
                    buffers.Remove(bm);
            }
        }

        internal void RemoveInstanceForCollection(StreamRecorder sm)
        {
            lock(streams)
            {
                if( streams.Contains(sm) )
                    streams.Remove(sm);
            }
        }

        private void PerformDataCollection(object state)
        {
            try
            {
                float interval; // time since the last data collection was performed, in seconds
                if( this.lastPerfCounterTicks == 0 )
                    interval = collectionPeriod / 1000F;
                else
                    interval = (DateTime.Now.Ticks - lastPerfCounterTicks) / (1000F * Constants.TicksPerMs);
                lastPerfCounterTicks = DateTime.Now.Ticks;

                #region Collect on buffers
                int cntBuffers = 0;
                int cntBufAvailable = 0;

                lock(buffers)
                {
                    foreach( BufferRecorder bm in buffers )
                    {
                        if( bm.Available )
                            ++cntBufAvailable;
                    }

                    cntBuffers = buffers.Count;
                }

                base[(int)ID.TotalBuffers] = cntBuffers;
                base[(int)ID.BuffersCreatedPerSec] = (int)( (cntBuffers - lastTotalBuffers) / interval );
                this.lastTotalBuffers = cntBuffers;

                base[(int)ID.BuffersAvailable] = cntBufAvailable;

                if( cntBuffers != 0 )
                    base[(int)ID.PercentBufAvailable] = (int)(100 * cntBufAvailable / cntBuffers);
                else
                    base[(int)ID.PercentBufAvailable] = 100;
                #endregion

                #region Collect on streams
                int cntBuffersWritten = 0;
                int cntFramesWritten = 0;
                int cntFramesRecvd = 0;
                int cntFramesDropped = 0;
                long cntBytesRecvd = 0;

                lock(streams)
                {
                    foreach(StreamRecorder sm in streams)
                    {
                        cntBuffersWritten += sm.BuffersWritten;
                        cntFramesWritten += sm.FramesWritten;
                        cntFramesDropped += sm.FramesDropped;
                        cntFramesRecvd += sm.FramesReceived;
                        cntBytesRecvd += sm.BytesReceived;
                    }
                }

                base[(int)ID.BuffersWritten] = cntBuffersWritten;
                base[(int)ID.BuffersWrittenPerSec] = (int)( (cntBuffersWritten - lastBuffersWritten) / interval );
                this.lastBuffersWritten = cntBuffersWritten;

                base[(int)ID.FramesWritten] = cntFramesWritten;
                base[(int)ID.FramesWrittenPerSec] = (int)( (cntFramesWritten - lastFramesWritten) / interval );
                this.lastFramesWritten = cntFramesWritten;
            
                base[(int)ID.FramesRecvd] = cntFramesRecvd;
                base[(int)ID.FramesRecvdPerSec] = (int)( (cntFramesRecvd - lastFramesRecvd) / interval );
                this.lastFramesRecvd = cntFramesRecvd;
            
                base[(int)ID.FramesDropped] = cntFramesDropped;
                base[(int)ID.FramesDroppedPerSec] = (int)( (cntFramesDropped - lastFramesDropped) / interval );
                this.lastFramesDropped = cntFramesDropped;
            
                base[(int)ID.BytesRecvd] = cntBytesRecvd;
                base[(int)ID.BytesRecvdPerSec] = (int)( (cntBytesRecvd - lastBytesRecvd) / interval );
                this.lastBytesRecvd = cntBytesRecvd;
                #endregion
            }
            catch (Exception ex)
            {
                if( this.collectionTimer != null )
                    throw ex;
            }
        }
        #endregion

        #region Variables
        // "Last" variables - the last data collected on certain performance datas
        int lastTotalBuffers = 0;
        int lastBuffersWritten = 0;
        int lastFramesWritten = 0;
        int lastFramesRecvd = 0;
        int lastFramesDropped = 0;
        long lastBytesRecvd = 0;

        long lastPerfCounterTicks = 0;

        /// <summary>
        /// The timer that calls PerformDataCollection every collectionPerdiod milliseconds.
        /// </summary>
        private Timer collectionTimer;

        /// <summary>
        /// How often data collection is performed, in milliseconds.
        /// </summary>
        private const int collectionPeriod = 1000;

        /// <summary>
        /// All the BufferRecorders we're collecting data on.
        /// </summary>
        /// <remarks>No need to use a synchronized ArrayList b/c we lock all of our operations</remarks>
        private ArrayList buffers = new ArrayList(Constants.InitialStreams*Constants.InitialBuffers);

        /// <summary>
        /// All the StreamRecorders we're collecting data on.
        /// </summary>
        /// <remarks>No need to use a synchronized ArrayList b/c we lock all of our operations</remarks>
        private ArrayList streams = new ArrayList(Constants.InitialStreams);
        #endregion

        #endregion

        #region IDisposable Members

        new public void Dispose()
        {
            if( this.collectionTimer != null )
                this.collectionTimer.Dispose();

            base.Dispose();
        }

        #endregion

        #region Install/Uninstall

        public static void Install()
        {
            Install(categoryName, counterNames);
        }

        public static void Uninstall()
        {
            Uninstall(categoryName);
        }

        #endregion
    }

    /// <summary>
    /// Contains performance counter data for the ConferenceRecorder class, as well as doing the data collection
    /// </summary>
    /// <remarks>
    /// This class was modified from its predecessor in Msr.Lst.Net.Rtp such that it does the data collection,
    /// abstracting that away from the classes it collects data from.
    /// </remarks>
    internal class ConferencePlayerPC : BasePC, IDisposable
    {
        #region Statics
    
        private const string categoryName = "Archive Player";

        // Counter names, must be kept in sync with ID enum or it won't compile
        private static readonly string[] counterNames = new string[(int)ID.Count]
            {
                "Bytes Sent",
                "Bytes Sent/sec",
                "Frames Sent",
                "Frames Sent/sec",
                "Streams",
                "Buffers",
                "Buffers Populating",
                "Buffers Populating %",
                "Stream Buffers Empty Errors",
                "Stream Bufs Empty Errs Per Sec",
                "Average Frame Lateness"
            };

        internal enum ID
        {
            BytesSent,
            BytesSentPerSec,
            FramesSent,
            FramesSentPerSec,
            Streams,
            Buffers,
            BuffersPopulating,
            BuffersPopPercent,
            StreamBuffersEmptyErrors,
            StreamBuffersEmptyErrorsPerSec,
            AverageFrameLateness,

            // This one needs to be last
            Count
        }

        
        #endregion Statics
        
        #region Constructors

        // Called by ConferenceRecorder
        internal ConferencePlayerPC(string instanceName) : base(categoryName, counterNames, instanceName) 
        {
            this.collectionTimer = new Timer(new TimerCallback(this.PerformDataCollection), null, (int)collectionPeriod, (int)collectionPeriod);
        }
        
        #endregion Constructors

        #region Data Collection
        
        #region Methods
        /// <summary>
        /// Accepts an instance of a class on which data is collected for this performance counter category.
        /// </summary>
        internal void AddInstanceForCollection(BufferPlayer ss)
        {
            lock(buffers)
            {
                if( !buffers.Contains(ss) )
                    buffers.Add(ss);
            }
        }

        internal void AddInstanceForCollection(StreamPlayer sp)
        {
            lock(streams)
            {
                if( !streams.Contains(sp) )
                    streams.Add(sp);
            }
        }

        //
        // If data is collected from ConferenceRecorder, or some other class,
        // add an overloaded AddInstanceForCollection method here.
        //
        // Note: You could, if you wanted, make AddInstanceForCollection accept an object,
        // but that would be less performant (and this is a *Performance*Counter... :)
        //

        /// <summary>
        /// Remove an instance from the set of classes on which data is collected.
        /// </summary>
        internal void RemoveInstanceForCollection(BufferPlayer ss)
        {
            lock(buffers)
            {
                if( buffers.Contains(ss) )
                    buffers.Remove(ss);
            }
        }

        internal void RemoveInstanceForCollection(StreamPlayer sp)
        {
            lock(streams)
            {
                if( streams.Contains(sp) )
                    streams.Remove(sp);
            }
        }

        private void PerformDataCollection(object state)
        {
            try
            {
                float interval; // time since the last data collection was performed, in seconds
                if( this.lastPerfCounterTicks == 0 )
                    interval = collectionPeriod / 1000F;
                else
                    interval = (DateTime.Now.Ticks - lastPerfCounterTicks) / 10000000F;
                lastPerfCounterTicks = DateTime.Now.Ticks;

                #region Collect on buffers
                int cntBuffers = 0;
                int cntBufPop = 0;

                lock(buffers)
                {
                    foreach( BufferPlayer ss in buffers )
                    {
                        if( ss.Populating )
                            ++cntBufPop;
                    }

                    cntBuffers = buffers.Count;
                }

                base[(int)ID.Buffers] = cntBuffers;
                base[(int)ID.BuffersPopulating] = cntBufPop;

                if( cntBuffers != 0 )
                    base[(int)ID.BuffersPopPercent] = (int)(100 * cntBufPop / cntBuffers);
                else
                    base[(int)ID.BuffersPopPercent] = 100;
                #endregion

                #region Collect on streams
                int cntStreams = 0;
                int cntFramesSent = 0;
                long cntBytesSent = 0;
                int cntErrors = 0;
                long cntLateness = 0;

                lock(streams)
                {
                    foreach(StreamPlayer sp in streams)
                    {
                        cntFramesSent += sp.FramesSent;
                        cntBytesSent += sp.BytesSent;
                        cntErrors += sp.BuffersEmptyErrors;
                        cntLateness += sp.LatenessSum;
                    }

                    cntStreams = streams.Count;
                }

                base[(int)ID.Streams] = cntStreams;

                base[(int)ID.BytesSent] = cntBytesSent;
                base[(int)ID.BytesSentPerSec] = (int)( (cntBytesSent - lastBytesSent) / interval );
                this.lastBytesSent = cntBytesSent;

                base[(int)ID.FramesSent] = cntFramesSent;
                base[(int)ID.FramesSentPerSec] = (int)( (cntFramesSent - lastFramesSent) / interval );
                this.lastFramesSent = cntFramesSent;
            
                base[(int)ID.StreamBuffersEmptyErrors] = cntErrors;
                base[(int)ID.StreamBuffersEmptyErrorsPerSec] = (int)( (cntErrors - lastErrors) / interval );
                this.lastErrors = cntErrors;

                if( cntFramesSent != 0 )
                    base[(int)ID.AverageFrameLateness] = (cntLateness - lastLateness) / cntFramesSent;
                 this.lastLateness = cntLateness;
                #endregion
            }
            catch (Exception ex)
            {
                if( this.collectionTimer != null )
                    throw ex;
            }
        }
        #endregion

        #region Variables
        // "Last" variables - the last data collected on certain performance datas
        int lastFramesSent = 0;
        long lastBytesSent = 0;
        int lastErrors = 0;
        long lastLateness = 0;

        long lastPerfCounterTicks = 0;

        /// <summary>
        /// The timer that calls PerformDataCollection every collectionPerdiod milliseconds.
        /// </summary>
        private Timer collectionTimer;

        /// <summary>
        /// How often data collection is performed, in milliseconds.
        /// </summary>
        private const int collectionPeriod = 1000;

        /// <summary>
        /// All the BufferRecorders we're collecting data on.
        /// </summary>
        /// <remarks>No need to use a synchronized ArrayList b/c we lock all of our operations</remarks>
        private ArrayList buffers = new ArrayList(Constants.InitialStreams*Constants.InitialBuffers);

        /// <summary>
        /// All the StreamRecorders we're collecting data on.
        /// </summary>
        /// <remarks>No need to use a synchronized ArrayList b/c we lock all of our operations</remarks>
        private ArrayList streams = new ArrayList(Constants.InitialStreams);
        #endregion

        #endregion

        #region IDisposable Members

        new public void Dispose()
        {
            if( this.collectionTimer != null )
                this.collectionTimer.Dispose();

            base.Dispose();
        }

        #endregion

        #region Install/Uninstall

        public static void Install()
        {
            Install(categoryName, counterNames);
        }

        public static void Uninstall()
        {
            Uninstall(categoryName);
        }

        #endregion
    }

    #endregion Performance Counter Classes
}