using System;
using System.Diagnostics;

// This file contains all of the performance counter constants and initialization / install
// (uninitialization / uninstall) methods.
// 
// In order to add new performance counters to an existing class, simply add an entry in the
// ID enum and a string to the counterNames array (keeping the position relative).
// 
// In order to add a new class of performance counters use an existing class as a model
// (inherit from BasePC, add a unique categoryName, ID enum, counterNames, constructor and indexer)
// and then add it to the PerformanceCounters.counters collection.
// 
// Note: the ID enum, and the counterNames string[] must be kept in sync or the code won't compile
// 
// It was very difficult to decide whether or not the constants should be moved into this 
// file (which causes a duplication of class names - RtpStream, RtpSender, etc.) or left in
// their "parent" class.  There are pros and cons either way, but because they are incidental
// to the "parent" class and take up a fair amount of space we decided to move them here.


namespace MSR.LST.Net.Rtp
{
    #region Install/Uninstall

    internal class PCInstaller: BasePCInstaller
    {
        private static Type[] counterTypes = new Type[]
            {
                typeof(RtpListenerPC),
                typeof(RtpSenderPC),
                typeof(RtpStreamPC),
                typeof(RtpSessionPC),
                typeof(RtcpSenderPC)
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
   
    #endregion Install / Uninstall

    #region Performance Counter Classes

    /// <summary>
    /// Contains the static performance counter data for the RtpStream class
    /// </summary>
    internal class RtpStreamPC : BasePC
    {
        #region Statics
    
        private const string categoryName = "Rtp Stream";

        // Counter names, must be kept in sync with ID enum or it won't compile
        private static readonly string[] counterNames = new string[(int)ID.Count]
            {
                "_Bytes",
                "_Bytes/sec",

                "_Frames",
                "_Frames Lost",
                "_Frames Lost/10K",
                "_Frames/sec",

                "_Invalid Packets In Frame",
                
                "_Packets",
                "_Packets Late",
                "_Packets Lost",
                "_Packets Lost/10K",
                "_Packets/sec",

                "f_Data Bytes",
                "f_Data Packets",
                "f_Data Packets Late",
                "f_Data Packets Lost",
                "f_Decode Path",
                "f_Fec Bytes",
                "f_Fec Packets",
                "f_Fec Packets Late",
                "f_Fec Packets Lost",
                "f_Ratio Checksum",
                "f_Ratio Data",
                "f_Type",
                "f_Undecodable",
                
                "SSRC"
            };

        internal enum ID
        {
            Bytes,
            BytesPerSecond,

            Frames,
            FramesLost,
            FramesLostPer10000,
            FramesPerSecond,

            PacketsInvalid,

            Packets,
            PacketsLate,
            PacketsLost,
            PacketsLostPer10000,
            PacketsPerSecond,
            
            f_DataBytes,
            f_DataPackets,
            f_DataPacketsLate,
            f_DataPacketsLost,
            f_DecodePath,
            f_FecBytes,
            f_FecPackets,
            f_FecPacketsLate,
            f_FecPacketsLost,
            f_RatioChecksum,
            f_RatioData,
            f_Type,
            f_Undecodable,

            Ssrc,

            // This one needs to be last
            Count
        }

        
        #endregion Statics
        
        #region Constructors

        internal RtpStreamPC(string instanceName) : base(categoryName, counterNames, instanceName) {}
        
        #endregion Constructors

        #region Indexer

        /// <summary>
        /// Update a performance counter
        /// </summary>
        internal long this[ID id]
        {
            set{base[(int)id] = value;}
        }
        
        #endregion Indexer

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
    /// Contains the static performance counter data for the RtpSender class
    /// </summary>
    internal class RtpSenderPC : BasePC
    {
        #region Statics

        private const string categoryName = "Rtp Sender";

        // Counter names, must be kept in sync with ID enum or it won't compile
        private static readonly string[] counterNames = new string[(int)ID.Count]
            {
                "_Bytes",
                "_Bytes/Frame Avg",
                "_Bytes/Frame Current",
                "_Bytes/Frame Max",
                "_Bytes/Frame Min",
                "_Bytes/sec",
                "_Frames",
                "_Frames/sec",
                "_Packets",
                "_Packets/sec",
                "_Px/Frame Avg",
                "_Px/Frame Current",
                "_Px/Frame Max",
                "_Px/Frame Min",

                "f_Bytes",
                "f_Packets",
                "f_Ratio Checksum",
                "f_Ratio Data",
                "f_Type",
            
                "SSRC"
            };

        internal enum ID
        {
            Bytes,
            BytesPerFrameAvg,
            BytesPerFrameCurrent,
            BytesPerFrameMax,
            BytesPerFrameMin,
            BytesPerSecond,
            Frames,
            FramesPerSecond,
            Packets,
            PacketsPerSecond,
            PxPerFrameAvg,
            PxPerFrameCurrent,
            PxPerFrameMax,
            PxPerFrameMin,

            f_Bytes,
            f_Packets,
            f_RatioChecksum,
            f_RatioData,
            f_Type,

            Ssrc,

            // This one needs to be last
            Count
        }

        #endregion Statics

        #region Constructors

        internal RtpSenderPC(string instanceName) : base(categoryName, counterNames, instanceName) {}

        #endregion Constructors

        #region Indexer

        /// <summary>
        /// Update a performance counter
        /// </summary>
        internal long this[ID id]
        {
            set{base[(int)id] = value;}
        }
        

        #endregion Indexer

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
    /// Contains the static performance counter data for the RtpListener class
    /// </summary>
    internal class RtpListenerPC : BasePC
    {
        #region Statics

        private const string categoryName = "Rtp Listener";

        // Counter names, must be kept in sync with ID enum or it won't compile
        private static readonly string[] counterNames = new string[(int)ID.Count]
            {
                "Buffer Pool Size",
                "Buffers Free",
                "Buffers In Use",

                #if FaultInjection
                "Dropped _Bytes",
                "Dropped _Packets",
                "Dropped Data Bytes",
                "Dropped Data Packets",
                "Dropped Fec Bytes",
                "Dropped Fec Packets",
                #endif

                "Growth Factor",
                "Initial Buffers",
                "Number of Grows",
                "Packets",
                "Streamless Packets"
            }; 

        internal enum ID
        {
            BufferPoolSize,
            BuffersFree,
            BuffersInUse,

            #if FaultInjection
            DroppedBytes,
            DroppedPackets,
            DroppedDataBytes,
            DroppedDataPackets,
            DroppedFecBytes,
            DroppedFecPackets,
            #endif

            GrowthFactor,
            InitialBuffers,
            GrowthCount,
            Packets,
            StreamlessPackets,

            // This one needs to be last
            Count
        }

        
        #endregion Statics
        
        #region Constructors

        internal RtpListenerPC(string instanceName) : base(categoryName, counterNames, instanceName) {}
        
        #endregion Constructors

        #region Indexer

        /// <summary>
        /// Update a performance counter
        /// </summary>
        internal long this[ID id]
        {
            set{base[(int)id] = value;}
        }
        

        #endregion Indexer

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
    /// Contains the static performance counter data for the RtpSession class
    /// </summary>
    internal class RtpSessionPC : BasePC
    {
        #region Statics

        private const string categoryName = "Rtp Session";

        // Counter names, must be kept in sync with ID enum or it won't compile
        private static readonly string[] counterNames = new string[(int)ID.Count]
            {
                "Invalid Packet Events",
                "Network Timeout Events",
                "Participants",
                "Rtp Streams",
                "RtpStream Timeout Events",
                "EventThrower Queue Length",
                "Peak Event Queue Length",
            
                "SSRC"
            }; 

        internal enum ID
        {
            InvalidPacketEvents,
            NetworkTimeoutEvents,
            Participants,
            RtpStreams,
            RtpStreamTimeoutEvents,
            EventThrowerQueueLength,
            PeakEventQueueLength,

            Ssrc,

            // This one needs to be last
            Count
        }

        
        #endregion Statics
        
        #region Constructors

        internal RtpSessionPC(string instanceName) : base(categoryName, counterNames, instanceName) {}
        
        #endregion Constructors

        #region Indexer

        /// <summary>
        /// Update a performance counter
        /// </summary>
        internal long this[ID id]
        {
            set{base[(int)id] = value;}
        }
        

        #endregion Indexer

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
    /// Contains the static performance counter data for the RtpSession class
    /// </summary>
    internal class RtcpSenderPC : BasePC
    {
        #region Statics

        private const string categoryName = "Rtcp Sender";

        // Counter names, must be kept in sync with ID enum or it won't compile
        private static readonly string[] counterNames = new string[(int)ID.Count]
            {
                "Bandwidth (Bytes/sec) Avg",

                "Bytes",
                "Bytes / Packet Avg",
                "Bytes / Packet Max",
                "Bytes / Packet Min",
                
                "Packets",
                "Packets / Interval Max",
                
                "Interval Avg",
                "Interval Forced",
                "Interval Max",
                "Interval Min",
                "Intervals",

                "SSRC"
            }; 

        internal enum ID
        {
            BandwidthAvg,

            Bytes,
            BytesPerPacketAvg,
            BytesPerPacketMax,
            BytesPerPacketMin,
            
            Packets,
            PacketsPerIntervalMax,
            
            IntervalAvg,
            IntervalForced,
            IntervalMax,
            IntervalMin,
            Intervals,

            Ssrc,

            // This one needs to be last
            Count
        }

        
        #endregion Statics
        
        #region Constructors

        internal RtcpSenderPC(string instanceName) : base(categoryName, counterNames, instanceName) {}
        
        #endregion Constructors

        #region Indexer

        /// <summary>
        /// Update a performance counter
        /// </summary>
        internal long this[ID id]
        {
            set{base[(int)id] = value;}
        }
        

        #endregion Indexer

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