using System;


namespace MSR.LST.ConferenceXP
{
    internal class AppConfig
    {
        private const string CXP = "MSR.LST.ConferenceXP.";

        public const string CXP_AutoPlayLocal           = CXP + "AutoPlayLocal";
        public const string CXP_AutoPlayRemote          = CXP + "AutoPlayRemote";
        public const string CXP_AutoPosition            = CXP + "AutoPosition";
        public const string CXP_Capability              = CXP + "Capability."; // Note final period
        public const string CXP_LogActivity             = CXP + "LogActivity";
        public const string CXP_VenueService            = CXP + "VenueService";
        public const string CXP_VenueServiceTimeout     = CXP + "VenueServiceTimeout";
        public const string CXP_VenueService_AutoLoad = CXP_VenueService + ".AutoLoad";
        public const string CXP_RTDocumentViewerDefault = CXP + "RTDocumentViewerDefault";
        public const string CXP_ThroughputWarningThreshold = CXP + "ThroughputWarningThreshold";


        public const string CXP_Capability_Bandwidth    = CXP_Capability + ".MaxBandwidth";
        public const string CXP_Capability_Fec          = CXP_Capability + ".Fec";
        public const string CXP_Capability_FecRatio     = CXP_Capability + ".FecRatio";
        public const string CXP_Capability_Channel      = CXP_Capability + ".Channel";

        public const string CXP_ReflectorEnabled        = CXP + "Reflector.Enabled";
        public const string CXP_ReflectorIP             = CXP + "Reflector.IP";
        public const string CXP_ReflectorPort           = CXP + "Reflector.Port";

        // TODO: This is needed for a temporary fix because the code for tiling is in 
        // conference. 
        // In fact, the tiling code is different with the VMR9 UI and with the default
        // DirectShow renderer UI. In the future, we might want to rethink the tiling 
        // architecture 
        public const string CXP_VideoUI_VMR9 = CXP_Capability + "Video.VideoUI_VMR9";
    }
}
