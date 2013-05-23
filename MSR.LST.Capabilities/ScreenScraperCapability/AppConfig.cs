using System;

namespace MSR.LST.ConferenceXP {
    internal class AppConfig {
        private const string BASE = "MSR.LST.ConferenceXP.Capability.ScreenScraperCapability.";

        // Target frame rate for the screen scraper.  
        public const string ScreenScraperFrameRate = BASE + "FrameRate";

        // Bit rate in kbps
        public const string ScreenScraperBitRate = BASE + "BitRate";
    }
}