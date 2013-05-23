using System;


namespace MSR.LST.Net.Rtp
{
    internal class AppConfig
    {
        private const string RTP = "MSR.LST.Net.Rtp.";

        public const string RTP_TimeToLive              = RTP + "TimeToLive";
        public const string RTP_MTU = RTP + "MTU";

        private const string CD = "MSR.LST.Net.ConnectivityDetector.";

        public const string CD_UpdateIntervalSeconds = CD + "UpdateIntervalSeconds";
        public const string CD_IPAddress = CD + "IPAddress";
        public const string CD_Port = CD + "Port";
    }
}