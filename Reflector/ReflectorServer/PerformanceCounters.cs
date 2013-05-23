using System;
using System.Diagnostics;

using MSR.LST;


namespace MSR.LST.ConferenceXP.ReflectorService
{
    #region Install/Uninstall

    public class PCInstaller : BasePCInstaller
    {
        private static Type[] counterTypes = new Type[]
            {
                typeof(ReflectorPC)
            };

        public static void Install()
        {
            Install(counterTypes);
        }

        public static void Uninstall()
        {
            Uninstall(counterTypes);
        }
    }
   
    #endregion

    #region Performance Counter Class

    /// <summary>
    /// Contains performance counter data for Reflector
    /// </summary>
    public class ReflectorPC : BasePC
    {
        #region Statics
    
        private const string categoryName = "Reflector";

        // Counter names, must be kept in sync with ID enum or it won't compile
        private static readonly string[] counterNames = new string[(int)ID.Count]
        {
            "Multicast Packets Received (Other)",
            "Multicast Packets Received (Self)",
            "Mutlicast-Unicast Packets Sent",
            "Participants Current",
            "Participants Total",
            "Unicast Packets Received",
            "Unicast-Unicast Packets Sent"
        };

        internal enum ID
        {
            MulticastPacketsReceivedOther,
            MulticastPacketsReceivedSelf,
            MCtoUCPacketsSent,
            CurrentParticipats,
            TotalParticipants,
            UnicastPacketsReceived,
            UCtoUCPacketsSent,

            // This one needs to be last
            Count
        }

        
        #endregion Statics
        
        #region Constructors

        internal ReflectorPC(string instanceName) : base(categoryName, counterNames, instanceName) {}
        
        #endregion Constructors

        #region Indexer

        /// <summary>
        /// Update a performance counter
        /// </summary>
        internal long this[ID id]
        {
            get{return base[(int)id];}

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
