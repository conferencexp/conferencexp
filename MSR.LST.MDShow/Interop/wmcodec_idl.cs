using System;
using System.Runtime.InteropServices;
using System.Text;


namespace MSR.LST.MDShow
{
    public class WM9PropList
    {
        public const string g_wszWMACAvgBytesPerSec = "AvgBytesPerSec";
        public const string g_wszWMACAvgPCMValue = "AverageLevel";
        public const string g_wszWMACFoldDownXToYChannels = "FoldDown%dTo%dChannels";
        public const string g_wszWMACFoldXToYChannelsZ = "Fold%uTo%uChannels%u";
        public const string g_wszWMACHiResOutput = "_HIRESOUTPUT";
        public const string g_wszWMACIncludeNumPasses = "_INCLUDENUMPASSES";
        public const string g_wszWMACInputFormatName = "_INPUTFORMATNAME";
        public const string g_wszWMACMixTable = "MixTable";
        public const string g_wszWMACMusicSpeechClassMode = "MusicSpeechClassMode";
        public const string g_wszWMACOriginalWaveFormat = "_ORIGINALWAVEFORMAT";
        public const string g_wszWMACPeakPCMValue = "PeakValue";
        public const string g_wszWMACSourceFormatTag = "_SOURCEFORMATTAG";
        public const string g_wszWMACSpeakerConfig = "SpeakerConfig";
        public const string g_wszWMACVoiceBuffer = "BufferWindow";
        public const string g_wszWMACVoiceEDL = "_EDL";
        public const string g_wszWMADRCAverageReference = "WMADRCAverageReference";
        public const string g_wszWMADRCAverageTarget = "WMADRCAverageTarget";
        public const string g_wszWMADRCPeakReference = "WMADRCPeakReference";
        public const string g_wszWMADRCPeakTarget = "WMADRCPeakTarget";
        public const string g_wszWMACDRCSetting = "DynamicRangeControl";
        public const string g_wszWMVCAvgBitrate = "_RAVG";
        public const string g_wszWMVCAvgFrameRate = "_AVGFRAMERATE";
        public const string g_wszWMVCBAvg = "_BAVG";
        public const string g_wszWMVCBMax = "_BMAX";
        public const string g_wszWMVCBufferFullnessInFirstByte = "_BUFFERFULLNESSINFIRSTBYTE";
        public const string g_wszWMVCCodedFrames = "_CODEDFRAMES";
        public const string g_wszWMVCComplexityEx = "_COMPLEXITYEX";
        public const string g_wszWMVCComplexityExLive = "_COMPLEXITYEXLIVE";
        public const string g_wszWMVCComplexityExMax = "_COMPLEXITYEXMAX";
        public const string g_wszWMVCComplexityExOffline = "_COMPLEXITYEXOFFLINE";
        public const string g_wszWMVCComplexityMode = "_COMPLEXITY";
        public const string g_wszWMVCCrisp = "_CRISP";
        public const string g_wszWMVCDatarate = "_DATARATE";
        public const string g_wszWMVCDecoderComplexityRequested = "_DECODERCOMPLEXITYREQUESTED";
        public const string g_wszWMVCDecoderComplexityProfile = "_DECODERCOMPLEXITYPROFILE";
        public const string g_wszWMVCDecoderDeinterlacing = "_DECODERDEINTERLACING";
        public const string g_wszWMVCDefaultCrisp = "_DEFAULTCRISP";
        public const string g_wszWMVCDXVAEnabled = "DXVAEnabled";
        public const string g_wszWMVCEndOfPass = "_ENDOFPASS";
        public const string g_wszWMVCFOURCC = "_FOURCC";
        public const string g_wszWMVCFrameCount = "_FRAMECOUNT";
        public const string g_wszWMVCFrameInterpolationEnabled = "_FRAMEINTERPOLATIONENABLED";
        public const string g_wszWMVCFrameInterpolationSupported = "_FRAMEINTERPOLATIONSUPPORTED";
        public const string g_wszWMVCInterlacedCodingEnabled = "_INTERLACEDCODINGENABLED";
        public const string g_wszWMVCKeyframeDistance = "_KEYDIST";
        public const string g_wszWMVCLiveEncode = "_LIVEENCODE";
        public const string g_wszWMVCMaxBitrate = "_RMAX";
        public const string g_wszWMVCPacketOverhead = "_ASFOVERHEADPERFRAME";
        public const string g_wszWMVCPassesRecommended = "_PASSESRECOMMENDED";
        public const string g_wszWMVCPassesUsed = "_PASSESUSED";
        public const string g_wszWMVCProduceDummyFrames = "_PRODUCEDUMMYFRAMES";
        public const string g_wszWMVCTotalFrames = "_TOTALFRAMES";
        public const string g_wszWMVCTotalWindow = "_TOTALWINDOW";
        public const string g_wszWMVCVBREnabled = "_VBRENABLED";
        public const string g_wszWMVCVBRQuality = "_VBRQUALITY";
        public const string g_wszWMVCVideoWindow = "_VIDEOWINDOW";
        public const string g_wszWMVCZeroByteFrames = "_ZEROBYTEFRAMES";


        public const string g_wszSpeechFormatCaps = "SpeechFormatCap";
        public const string g_wszWMCPCodecName = "_CODECNAME";
        public const string g_wszWMCPSupportedVBRModes = "_SUPPORTEDVBRMODES";
    }

    public enum WMT_PROP_DATATYPE
    {
        WMT_PROP_TYPE_DWORD = 0,
        WMT_PROP_TYPE_STRING    = 1,
        WMT_PROP_TYPE_BINARY    = 2,
        WMT_PROP_TYPE_BOOL  = 3,
        WMT_PROP_TYPE_QWORD = 4,
        WMT_PROP_TYPE_WORD  = 5,
        WMT_PROP_TYPE_GUID  = 6
    }

    
    [ComVisible(true), ComImport, Guid("352bb3bd-2d4d-4323-9e71-dcdcfbd53ca6"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMVideoDecoderHurryup
    {
        void SetHurryup([In] int lHurryup);
        void GetHurryup([Out] int plHurryup);
    }

    [ComVisible(true), ComImport, Guid("A7B2504B-E58A-47fb-958B-CAC7165A057D"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMCodecStrings
    {
        void GetName(
            [In][Out] ref _AMMediaType pmt,
            [In] uint cchLength,
            [Out] StringBuilder szName,
            [Out] out uint pcchLength);
        
        void GetDescription(
            [In][Out] ref _AMMediaType pmt,
            [In] uint cchLength,
            [Out] StringBuilder szDescription,
            [Out] out uint pcchLength);
    }

    [ComVisible(true), ComImport, Guid("2573e11a-f01a-4fdd-a98d-63b8e0ba9589"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMCodecProps
    {
        void GetFormatProp(
            [In][Out] ref _AMMediaType pmt,
            [In] [MarshalAs(UnmanagedType.LPWStr)] string pszName,
            [Out] out WMT_PROP_DATATYPE pType,
            [In] IntPtr pValue,
            [In][Out] ref uint pdwSize);
        
        void GetCodecProp(
            [In] uint dwFormat,
            [In] [MarshalAs(UnmanagedType.LPWStr)] string pszName,
            [Out] out WMT_PROP_DATATYPE pType,
            [In] IntPtr pValue,
            [In][Out] ref uint pdwSize);
    }

    [ComVisible(true), ComImport, Guid("A81BA647-6227-43b7-B231-C7B15135DD7D"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMCodecLeakyBucket
    {
        void SetBufferSizeBits([In] uint ulBufferSize);
        void GetBufferSizeBits([Out] out uint ulBufferSize);
        
        void SetBufferFullnessBits([In] uint ulBufferFullness);
        void GetBufferFullnessBits([Out] out uint ulBufferFullness);
    }

    [ComVisible(true), ComImport, Guid("D051ED9F-BC5C-4e83-B14E-8428485C286A"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMCodecMetaData
    {
        void GetNumberOfValues([Out] out uint pulNumValues);
        
        void GetBufferSizes(
            [In] uint ulValueIndex,
            [Out] out uint pcbData,
            [Out] out uint pcchName);
        
        void GetValueAndName(
            [In] uint ulValueIndex,
            [Out] IntPtr pbData,
            [In] uint cbData,
            [Out] out uint pcbData,
            [Out] [MarshalAs(UnmanagedType.LPWStr)] out string szName,
            [In] uint cchName,
            [Out] out uint pcchName);
    }

    [ComVisible(true), ComImport, Guid("B72ADF95-7ADC-4a72-BC05-577D8EA6BF68"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMCodecOutputTimestamp
    {
        void GetNextOutputTime([Out] out double prtTime);
    }

    [ComVisible(true), ComImport, Guid("73f0be8e-57f7-4f01-aa66-9f57340cfe0e"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IWMCodecPrivateData
    {
        void SetPartialOutputType([In] ref _AMMediaType pmt);
        
        void GetPrivateData([In] [Out] IntPtr pbData,
                            [In] [Out] ref uint pcbData);
    }
}
