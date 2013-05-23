// ------------------------------------------------------------------------------------------------
// This file supplements the types that are not available via an IDL file, i.e. they are in header
// files, either from DX or the OS.
// ------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;


namespace MSR.LST.MDShow
{
    public abstract class DShowError
    {
        [DllImportAttribute("quartz.dll")]
        private static extern uint AMGetErrorText(
            int hr,
            StringBuilder pBuffer,
            uint MaxLen);

        public static string _AMGetErrorText(int hr)
        {
            uint length = 512;
            StringBuilder sb = new StringBuilder((int)length);

            // Don't check the return value, because we don't care how many
            // characters were returned.  ToString will take care of it for us.
            AMGetErrorText(hr, sb, length);

            return sb.ToString();
        }
    }


    /// <summary>
    /// Located in Platform SDK\include\WinDefs.h, WTypes.h, WTypes.idl
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        private int left;
        private int top;
        private int right;
        private int bottom;

        public int Left
        {
            get { return left; }
            set { left = value; }
        }

        public int Top
        {
            get { return top; }
            set { top = value; }
        }

        public int Right
        {
            get { return right; }
            set { right = value; }
        }

        public int Bottom
        {
            get { return bottom; }
            set { bottom = value; }
        }


        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "L: {0}, T: {1}, R: {2}, B: {3}", 
                left, top, right, bottom);
        }

    }

    /// <summary>
    /// Located in Platform SDK\include\WinDef.h, WTypes.h
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public static bool operator ==(SIZE s1, SIZE s2)
        {
            return (s1.cx == s2.cx && s1.cy == s2.cy);
        }

        public static bool operator !=(SIZE s1, SIZE s2)
        {
            return !(s1 == s2);
        }

        private int cx;
        private int cy;

        public int CX
        {
            get { return cx; }
            set { cx = value; }
        }

        public int CY
        {
            get { return cy; }
            set { cy = value; }
        }

        // Constructor
        public SIZE(int cx, int cy)
        {
            this.cx = cx;
            this.cy = cy;
        }


        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}x{1}", cx, cy);
        }

        // Compiler warning if we don't override this
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return (this == (SIZE)obj);
        }

        // Compiler warning if we don't override this
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Located in Platform SDK\include\amvideo.h
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct MPEG1VIDEOINFO
    {
        private VIDEOINFOHEADER hdr;
        private uint startTimeCode;
        private uint cbSequenceHeader;
        private IntPtr sequenceHeader;

        public VIDEOINFOHEADER Hdr
        {
            get { return hdr; }
            set { hdr = value; }
        }

        public uint StartTimeCode
        {
            get { return startTimeCode; }
            set { startTimeCode = value; }
        }

        public uint SequenceHeaderByteCount
        {
            get { return cbSequenceHeader; }
            set { cbSequenceHeader = value; }
        }

        public IntPtr SequenceHeader
        {
            get { return sequenceHeader; }
            set { sequenceHeader = value; }
        }


        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, 
                "MPEG1VIDEOINFO - StartTimeCode: {0}  SequenceHeaderByteCount: {1}  SequenceHeader: {2}\n{3}", 
                StartTimeCode, SequenceHeaderByteCount, SequenceHeader, hdr.ToString());
        }
    }
    /// <summary>
    /// Located in Platform SDK\include\dvdmedia.h
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct MPEG2VIDEOINFO
    {
        private VIDEOINFOHEADER2 hdr;
        private uint startTimeCode;
        private uint cbSequenceHeader;
        private uint profile;
        private uint level;
        private uint flags;
        private IntPtr sequenceHeader;

        public VIDEOINFOHEADER2 Hdr
        {
            get { return hdr; }
            set { hdr = value; }
        }

        public uint StartTimeCode
        {
            get { return startTimeCode; }
            set { startTimeCode = value; }
        }

        public uint SequenceHeaderByteCount
        {
            get { return cbSequenceHeader; }
            set { cbSequenceHeader = value; }
        }

        public uint Profile
        {
            get { return profile; }
            set { profile = value; }
        }

        public uint Level
        {
            get { return level; }
            set { level = value; }
        }

        public uint Flags
        {
            get { return flags; }
            set { flags = value; }
        }

        public IntPtr SequenceHeader
        {
            get { return sequenceHeader; }
            set { sequenceHeader = value; }
        }


        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, 
                "MPEG2VIDEOINFO - StartTimeCode: {0}  SequenceHeaderByteCount: {1}  Profile: {2}  Level: {3}  Flags: {4}  SequenceHeader: {5}\n{6}", 
                StartTimeCode, cbSequenceHeader, Profile, Level, Flags, SequenceHeader.ToString(), hdr);
        }
    }
    /// <summary>
    /// Located in Platform SDK\include\Amvideo.h
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct VIDEOINFOHEADER
    {
        private RECT source;
        private RECT target;
        private uint bitRate;
        private uint bitErrorRate;
        private long avgTimePerFrame;
        private BITMAPINFOHEADER bitmapInfo;

        public RECT Source
        {
            get { return source; }
            set { source = value; }
        }

        public RECT Target
        {
            get { return target; }
            set { target = value; }
        }

        public uint BitRate
        {
            get { return bitRate; }
            set { bitRate = value; }
        }

        public uint BitErrorRate
        {
            get { return bitErrorRate; }
            set { bitErrorRate = value; }
        }

        public long AvgTimePerFrame
        {
            get { return avgTimePerFrame; }
            set { avgTimePerFrame = value; }
        }

        public BITMAPINFOHEADER BitmapInfo
        {
            get { return bitmapInfo; }
            set { bitmapInfo = value; }
        }

        public double FrameRate
        {
            get { return Math.Round(10000000D / AvgTimePerFrame, 2); }
            set { AvgTimePerFrame = (long)(10000000D / value); }
        }


        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, 
                "VIDEOINFOHEADER - Source: {0}  Target: {1}  BitRate: {2}  BitErrorRate: {3}  AvgTimePerFrame: {4}\n{5}", 
                Source.ToString(), Target.ToString(), BitRate, BitErrorRate, AvgTimePerFrame, BitmapInfo.ToString());
        }
    }
    /// <summary>
    /// Located in Platform SDK\include\dvdmedia.h
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct VIDEOINFOHEADER2
    {
        private RECT source;
        private RECT target;
        private uint bitRate;
        private uint bitErrorRate;
        private long avgTimePerFrame;
        private uint interlaceFlags;
        private uint copyProtectFlags;
        private uint pictAspectRatioX;
        private uint pictAspectRatioY;
        private uint reserved1;
        private uint reserved2;
        private BITMAPINFOHEADER bitmapInfo;

        public RECT Source
        {
            get { return source; }
            set { source = value; }
        }

        public RECT Target
        {
            get { return target; }
            set { target = value; }
        }

        public uint BitRate
        {
            get { return bitRate; }
            set { bitRate = value; }
        }

        public uint BitErrorRate
        {
            get { return bitErrorRate; }
            set { bitErrorRate = value; }
        }

        public long AvgTimePerFrame
        {
            get { return avgTimePerFrame; }
            set { avgTimePerFrame = value; }
        }

        public uint InterlaceFlags
        {
            get { return interlaceFlags; }
            set { interlaceFlags = value; }
        }

        public uint CopyProtectFlags
        {
            get { return copyProtectFlags; }
            set { copyProtectFlags = value; }
        }

        public uint PictAspectRatioX
        {
            get { return pictAspectRatioX; }
            set { pictAspectRatioX = value; }
        }

        public uint PictAspectRatioY
        {
            get { return pictAspectRatioY; }
            set { pictAspectRatioY = value; }
        }

        public uint Reserved1
        {
            get { return reserved1; }
            set { reserved1 = value; }
        }

        public uint Reserved2
        {
            get { return reserved2; }
            set { reserved2 = value; }
        }

        public BITMAPINFOHEADER BitmapInfo
        {
            get { return bitmapInfo; }
            set { bitmapInfo = value; }
        }

        public double FrameRate
        {
            get { return Math.Round(10000000D / AvgTimePerFrame, 2); }
            set { AvgTimePerFrame = (long)(10000000D / value); }
        }


        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, 
                "VIDEOINFOHEADER2 - Source: {0}  Target: {1}  BitRate: {2}  BitErrorRate: {3}  AvgTimePerFrame: {4}  InterlaceFlags: {5}  CopytProtectFlags: {6}  PictAspectRatioX: {7}  PictAspectRatioY: {8}  Reserved1: {9}  Reserved2: {10}\n{11}", 
                Source.ToString(), Target.ToString(), BitRate, BitErrorRate, AvgTimePerFrame, InterlaceFlags, 
                CopyProtectFlags, PictAspectRatioX, PictAspectRatioY, Reserved1, Reserved2, BitmapInfo.ToString());
        }
    }

    /// <summary>
    /// Located in Platform SDK\include\WinGDI.h
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFOHEADER
    {
        private uint size;
        private int width;
        private int height;
        private ushort planes;
        private ushort bitCount;
        private uint compression;
        private uint sizeImage;
        private int xPelsPerMeter;
        private int yPelsPerMeter;
        private uint clrUsed;
        private uint clrImportant;

        public uint Size
        {
            get { return size; }
            set { size = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public ushort Planes
        {
            get { return planes; }
            set { planes = value; }
        }

        public ushort BitCount
        {
            get { return bitCount; }
            set { bitCount = value; }
        }

        public uint Compression
        {
            get { return compression; }
            set { compression = value; }
        }

        public uint SizeImage
        {
            get { return sizeImage; }
            set { sizeImage = value; }
        }

        public int XPelsPerMeter
        {
            get { return xPelsPerMeter; }
            set { xPelsPerMeter = value; }
        }

        public int YPelsPerMeter
        {
            get { return yPelsPerMeter; }
            set { yPelsPerMeter = value; }
        }

        public uint ClrUsed
        {
            get { return clrUsed; }
            set { clrUsed = value; }
        }

        public uint ClrImportant
        {
            get { return clrImportant; }
            set { clrImportant = value; }
        }


        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, 
                "BITMAPINFOHEADER - Size: {0},  Width: {1},  Height: {2},  Planes: {3},  BitCount: {4},  " +
                "Compression: {5},  SizeImage: {6},  XPelsPermeter: {7},  YPelsPerMeter: {8},  ClrUsed: {9},  ClrImportant: {10}", 
                Size, Width, Height, Planes, BitCount, Compression, SizeImage, XPelsPerMeter, YPelsPerMeter, 
                ClrUsed, ClrImportant);
        }
    }

    /// <summary>
    /// Located in Platform SDK\include\MMReg.h
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct WAVEFORMATEX
    {
        private ushort formatTag;
        private ushort channels;
        private uint samplesPerSec;
        private uint avgBytesPerSec;
        private ushort blockAlign;
        private ushort bitsPerSample;
        private ushort size;

        public ushort FormatTag
        {
            get { return formatTag; }
            set { formatTag = value; }
        }

        public ushort Channels
        {
            get { return channels; }
            set { channels = value; }
        }

        public uint SamplesPerSec
        {
            get { return samplesPerSec; }
            set { samplesPerSec = value; }
        }

        public uint AvgBytesPerSec
        {
            get { return avgBytesPerSec; }
            set { avgBytesPerSec = value; }
        }

        public ushort BlockAlign
        {
            get { return blockAlign; }
            set { blockAlign = value; }
        }

        public ushort BitsPerSample
        {
            get { return bitsPerSample; }
            set { bitsPerSample = value; }
        }

        public ushort Size
        {
            get { return size; }
            set { size = value; }
        }


        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture,
                "WAVEFORMATEX -  FormatTag: {0}     Channels: {1}     SamplesPerSec: {2}     AvgBytesPerSec: {3}     " +
                "BlockAlign: {4}     BitsPerSample: {5}     Size: {6}", 
                FormatTag, Channels, SamplesPerSec, AvgBytesPerSec, BlockAlign, BitsPerSample, Size);
        }
    }

    /// <summary>
    /// Located in Platform SDK\include\strmif.h
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct VIDEO_STREAM_CONFIG_CAPS
    {
        private Guid guid;
        private uint videoStandard;
        private SIZE inputSize;
        private SIZE minCroppingSize;
        private SIZE maxCroppingSize;
        private int cropGranularityX;
        private int cropGranularityY;
        private int cropAlignX;
        private int cropAlignY;
        private SIZE minOutputSize;
        private SIZE maxOutputSize;
        private int outputGranularityX;
        private int outputGranularityY;
        private int stretchTapsX;
        private int stretchTapsY;
        private int shrinkTapsX;
        private int shrinkTapsY;
        private long minFrameInterval;
        private long maxFrameInterval;
        private int minBitsPerSecond;
        private int maxBitsPerSecond;

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public uint VideoStandard
        {
            get { return videoStandard; }
            set { videoStandard = value; }
        }

        public SIZE InputSize
        {
            get { return inputSize; }
            set { inputSize = value; }
        }

        public SIZE MinCroppingSize
        {
            get { return minCroppingSize; }
            set { minCroppingSize = value; }
        }

        public SIZE MaxCroppingSize
        {
            get { return maxCroppingSize; }
            set { maxCroppingSize = value; }
        }

        public int CropGranularityX
        {
            get { return cropGranularityX; }
            set { cropGranularityX = value; }
        }

        public int CropGranularityY
        {
            get { return cropGranularityY; }
            set { cropGranularityY = value; }
        }

        public int CropAlignX
        {
            get { return cropAlignX; }
            set { cropAlignX = value; }
        }

        public int CropAlignY
        {
            get { return cropAlignY; }
            set { cropAlignY = value; }
        }

        public SIZE MinOutputSize
        {
            get { return minOutputSize; }
            set { minOutputSize = value; }
        }

        public SIZE MaxOutputSize
        {
            get { return maxOutputSize; }
            set { maxOutputSize = value; }
        }

        public int OutputGranularityX
        {
            get { return outputGranularityX; }
            set { outputGranularityX = value; }
        }

        public int OutputGranularityY
        {
            get { return outputGranularityY; }
            set { outputGranularityY = value; }
        }

        public int StretchTapsX
        {
            get { return stretchTapsX; }
            set { stretchTapsX = value; }
        }

        public int StretchTapsY
        {
            get { return stretchTapsY; }
            set { stretchTapsY = value; }
        }

        public int ShrinkTapsX
        {
            get { return shrinkTapsX; }
            set { shrinkTapsX = value; }
        }

        public int ShrinkTapsY
        {
            get { return shrinkTapsY; }
            set { shrinkTapsY = value; }
        }

        public long MinFrameInterval
        {
            get { return minFrameInterval; }
            set { minFrameInterval = value; }
        }

        public long MaxFrameInterval
        {
            get { return maxFrameInterval; }
            set { maxFrameInterval = value; }
        }

        public int MinBitsPerSecond
        {
            get { return minBitsPerSecond; }
            set { minBitsPerSecond = value; }
        }

        public int MaxBitsPerSecond
        {
            get { return maxBitsPerSecond; }
            set { maxBitsPerSecond = value; }
        }


        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture,
                "VIDEO_STREAM_CONFIG_CAPS -  guid: {0},  VideoStandard: {1},  InputSize: {2},  " +
                "MinCroppingSize: {3},  MaxCroppingSize: {4},  CropGranularityX: {5},  CropGranularityY: {6},  " +
                "CropAlignX: {7},  CropAlignY: {8},  MinOutputSize: {9},  MaxOutputSize: {10},  " +
                "OutputGranularityX: {11},  OutputGranularityY: {12},  StretchTapsX: {13},  StretchTapsY: {14},  " +
                "ShrinkTapsX: {15},  ShrinkTapsY: {16},  MinFrameInterval: {17},  MaxFrameInterval: {18},  " +
                "MinBitsPerSecond: {19},  MaxBitsPerSecond: {20}", // Docs clearly say this is a format type
                MediaType.FormatType.GuidToString(guid), VideoStandard, InputSize.ToString(), 
                MinCroppingSize.ToString(), MaxCroppingSize.ToString(), CropGranularityX, CropGranularityY,
                CropAlignX, CropAlignY, MinOutputSize.ToString(), MaxOutputSize.ToString(), OutputGranularityX, 
                OutputGranularityY, StretchTapsX, StretchTapsY, ShrinkTapsX, ShrinkTapsY, MinFrameInterval, 
                MaxFrameInterval, MinBitsPerSecond, MaxBitsPerSecond);
        }
    }

    /// <summary>
    /// Located in Platform SDK\include\strmif.h
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct AUDIO_STREAM_CONFIG_CAPS
    {
        private Guid guid;
        private uint minimumChannels;
        private uint maximumChannels;
        private uint channelsGranularity;
        private uint minimumBitsPerSample;
        private uint maximumBitsPerSample;
        private uint bitsPerSampleGranularity;
        private uint minimumSampleFrequency;
        private uint maximumSampleFrequency;
        private uint sampleFrequencyGranularity;

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public uint MinimumChannels
        {
            get { return minimumChannels; }
            set { minimumChannels = value; }
        }

        public uint MaximumChannels
        {
            get { return maximumChannels; }
            set { maximumChannels = value; }
        }

        public uint ChannelsGranularity
        {
            get { return channelsGranularity; }
            set { channelsGranularity = value; }
        }

        public uint MinimumBitsPerSample
        {
            get { return minimumBitsPerSample; }
            set { minimumBitsPerSample = value; }
        }

        public uint MaximumBitsPerSample
        {
            get { return maximumBitsPerSample; }
            set { maximumBitsPerSample = value; }
        }

        public uint BitsPerSampleGranularity
        {
            get { return bitsPerSampleGranularity; }
            set { bitsPerSampleGranularity = value; }
        }

        public uint MinimumSampleFrequency
        {
            get { return minimumSampleFrequency; }
            set { minimumSampleFrequency = value; }
        }

        public uint MaximumSampleFrequency
        {
            get { return maximumSampleFrequency; }
            set { maximumSampleFrequency = value; }
        }

        public uint SampleFrequencyGranularity
        {
            get { return sampleFrequencyGranularity; }
            set { sampleFrequencyGranularity = value; }
        }


        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, 
                "AUDIO_STREAM_CONFIG_CAPS -  guid: {0},  MinimumChannels: {1},  MaximumChannels: {2},  " +
                "ChannelsGranularity: {3},  MinimumBitsPerSample: {4},  MaximumBitsPerSample: {5},  " +
                "BitsPerSampleGranularity: {6},  MinimumSampleFrequency: {7},  MaximumSampleFrequency: {8},  " +
                "SampleFrequencyGranularity: {9}", // Docs clearly say this is a major type
                MediaType.MajorType.GuidToString(guid), MinimumChannels, MaximumChannels, ChannelsGranularity,
                MinimumBitsPerSample, MaximumBitsPerSample, BitsPerSampleGranularity, MinimumSampleFrequency, 
                MaximumSampleFrequency, SampleFrequencyGranularity);
        }
    }


    // Property pages are shared by Filters and Pins
    // This seemed like the most logical place to put the types
    [StructLayout(LayoutKind.Sequential)]
    public struct CAUUID
    {
        private uint cElems;
        private IntPtr pElems;

        public uint ElementCount
        {
            get { return cElems; }
            set { cElems = value; }
        }

        public IntPtr Elements
        {
            get { return pElems; }
            set { pElems = value; }
        }
    }


    [ComImport]
    [Guid("B196B28B-BAB4-101A-B69C-00AA00341D07")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ISpecifyPropertyPages
    {
        int GetPages(out CAUUID pPages);
    }


    /// <summary>
    /// Class that handles displaying of Property Pages.
    /// </summary>
    public class PropertyPage
    {
        private ISpecifyPropertyPages iSPP;
        private string name;

        public PropertyPage(ISpecifyPropertyPages iSPP, string name)
        {
            this.iSPP = iSPP;
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }

        public void Show(IntPtr hwnd)
        {
            CAUUID ca;
            int hr = iSPP.GetPages(out ca);

            if (hr == 0) // S_OK
            {
                object o = iSPP;
                OleCreatePropertyFrame(hwnd, 0, 0, name, 1, ref o,
                    ca.ElementCount, ca.Elements, 0, 0, IntPtr.Zero);

                Marshal.FreeCoTaskMem(ca.Elements);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        [DllImport("oleaut32.dll", CharSet = CharSet.Unicode)]
        private static extern int OleCreatePropertyFrame(
            [In] IntPtr hwndOwner, [In] uint x, [In] uint y,
            [In] String lpszCaption, [In] uint cObjects, [In, MarshalAs(UnmanagedType.IUnknown)] ref object ppUnk,
            [In] uint cPages, [In] IntPtr pPageClsID, [In] uint LCID,
            [In] uint dwReserved, IntPtr pvReserved);
    }
}