using System;

namespace MSR.LST.MDShow
{
    internal class AppConfig
    {
        private const string MDS = "MSR.LST.MDShow.";

        public const string MDS_VideoMediaType = MDS + "VideoMediaType";
        public const string MDS_VideoCompressorName = MDS + "VideoCompressorName";
        public const string MDS_AudioCompressionFormat = MDS + "AudioCompressionFormat";
        public const string MDS_RtpRendererFlags = MDS + "RtpRendererFlags";
        public const string DVSourceFilterName = MDS + "DVSourceFilterName";
        public const string DVOutputPinIndex = MDS + "DVOutputPinIndex";
        public const string DVAudioDisabled = MDS + "DVAudioDisabled";

    }
}