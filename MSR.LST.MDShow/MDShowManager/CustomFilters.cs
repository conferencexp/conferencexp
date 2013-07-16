using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;
using System.Reflection;

namespace MSR.LST.MDShow {

    #region BlackMagicSource

    /// <summary>
    /// A special case to handle Blackmagic DeckLink hardware.
    /// Tested with Intensity Pro version 8.0.1.
    /// The DeckLink capture filter produces a media type that can't be 
    /// encoded or rendered without first adding a Blackmagic Decoder filter.  The
    /// subclass handles connecting the filter, and making the two filters appear to the caller
    /// as a normal source filter.
    /// </summary>
    class BlackMagicSource : VideoSource {

        private IPin decoderOutput;
        private IPin captureOutput;
        private IBaseFilter bfDecoder;

        public BlackMagicSource(FilterInfo fi) : base(fi) { }

        public override void AddedToGraph(FilgraphManager fgm) {
            IGraphBuilder gb = (IGraphBuilder)fgm;

            //Add the Blackmagic Decoder filter and connect it.
            try {
                bfDecoder = Filter.CreateBaseFilterByName("Blackmagic Design Decoder (DMO)");
                gb.AddFilter(bfDecoder, "Blackmagic Design Decoder (DMO)");
                IPin decoderInput;
                bfDecoder.FindPin("in0", out decoderInput);
                bfDecoder.FindPin("out0", out decoderOutput);
                captureOutput = GetPin(filter, _PinDirection.PINDIR_OUTPUT, Pin.PIN_CATEGORY_CAPTURE, Guid.Empty, false, 0);
                gb.Connect(captureOutput, decoderInput);
            }
            catch {
                throw new ApplicationException("Failed to add the BlackMagic Decoder filter to the graph");
            }

            base.AddedToGraph(fgm);
        }

        /// <summary>
        /// This is needed so that the caller can remove filters downstream from the decoder, and not remove the decoder.
        /// </summary>
        public override IBaseFilter DownstreamBaseFilter {
            get {
                return this.bfDecoder;
            }
        }

        /// <summary>
        /// The override returns media types on capture output pin, not the decoder output.
        /// Attempts to get the MediaType of the decoder output fail because the IAMStreamConfig interface is not supported.
        /// </summary>
        /// <param name="mt"></param>
        /// <param name="formatBlock"></param>
        public override void GetMediaType(out _AMMediaType mt, out object formatBlock) {
            Pin.GetMediaType((IAMStreamConfig)captureOutput, out mt, out formatBlock);
        }

        /// <summary>
        /// Connect downstream filters to the decoder output pin.
        /// </summary>
        /// <returns></returns>
        protected override IPin DefaultOutputPin() {
            return decoderOutput;
        }

        public override IPin OutputPin {
            get {
                return decoderOutput;
            }
            set {
                //ignore
            }
        }
    }

    #endregion BlackMagicSource

    #region OpusAudioCompressor

    public class OpusAudioCompressor : AudioCompressor {

        #region Constructor

        public OpusAudioCompressor(FilterInfo fi) : base(fi) { }

        #endregion

        #region Static

        #region Configuration Properties

        public static int Frequency = DefaultFrequency;
        public static int Channels = DefaultChannels;
        public const int Depth = DefaultDepth;
        public static int BufferMS = DefaultBufferMS;
        public static int Signal = DefaultSignal;
        public static int BitRate = DefaultBitRate;
        public static int ManualBitRate = DefaultManualBitRate;
        public static int Complexity = DefaultComplexity;
        public static int VBR = DefaultVBR;
        public static int VBRConstraint = DefaultVBRConstraint;
        public static int ForcedChannels = DefaultForcedChannels;
        public static int MaxBandwidth = DefaultMaxBandwidth;
        public static int DTX = DefaultDTX;
        public static int PacketLossPerc = DefaultPacketLossPerc;
        public static int LSBDepth = DefaultLSBDepth;
        public static int Application = DefaultApplication;
        public static int InbandFec = DefaultInbandFec;

        #endregion

        #region Property Defaults

        public const int DefaultFrequency = 48000;
        public const int DefaultChannels = 2;
        public const int DefaultDepth = 16;
        public const int DefaultBufferMS = 20;
        public const int DefaultSignal = 3002;
        public const int DefaultBitRate = -1000;
        public const int DefaultManualBitRate = 512;
        public const int DefaultComplexity = 10;
        public const int DefaultVBR = 1;
        public const int DefaultVBRConstraint = 1;
        public const int DefaultForcedChannels = -1000;
        public const int DefaultMaxBandwidth = 1105;
        public const int DefaultDTX = 0;
        public const int DefaultPacketLossPerc = 0;
        public const int DefaultLSBDepth = 24;
        public const int DefaultApplication = 2049;
        public const int DefaultInbandFec = 0;

        #endregion

        #region Property Constraints

        // Property values may be constrained either by enums, or by one of the following int arrays.
        public static readonly int[] ValuesFrequency = new[] { 8000, 12000, 16000, 24000, 48000 };
        public static readonly int[] ValuesChannels = new[] { 1, 2 };
        public static readonly int[] ValuesBufferMS = new[] { 5, 10, 20, 40, 60 };
        public static readonly int[] ValuesComplexity = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        public static readonly int[] ValuesLSBDepth = new[] { 8, 16, 24 };
        public static readonly int[] MinMaxPacketLossPerc = new[] { 0, 100 };
        public static readonly int[] MinMaxManualBitRate = new[] { 500, 512000 };

        #endregion

        #region Static Methods

        /// <summary>
        /// The encoder requires a very specific buffer size
        /// </summary>
        /// <returns></returns>
        public static int GetBufferSize() {
            return (Frequency / 1000) * BufferMS * Channels * (Depth / 8);
        }

        /// <summary>
        /// Determine if a particular format is compatible with the encoder.
        /// </summary>
        /// <returns></returns>
        public static bool WorksWithOpus(WAVEFORMATEX wfex) {
            if (wfex.BitsPerSample != 16)
                return false;

            if (wfex.Channels != 1 && wfex.Channels != 2) {
                return false;
            }

            foreach (int f in ValuesFrequency) {
                if (wfex.SamplesPerSec == f) {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #endregion

        #region Enums

        public enum EnumBitRate { Auto = -1000, Max = -1 , Manual = 0 };
        public enum EnumVBR { CBR = 0, VBR = 1 };
        public enum EnumVBRConstraint { Unconstrained = 0, Constrained = 1 };
        public enum EnumForcedChannels { Auto = -1000, ForcedMono = 1, ForcedStereo = 2 };
        public enum EnumMaxBandwidth { Narrow_4kHz = 1101, Medium_6kHz = 1102, Wide_8kHz = 1103, SuperWide_12kHz = 1104, Full_20kHz = 1105 };
        public enum EnumSignal { Auto = -1000, Voice = 3001, Music = 3002 };
        public enum EnumApplication { VoIP = 2048, Audio = 2049, LowDelay = 2051 };
        public enum EnumInbandFec { Disable = 0, Enable = 1 };
        public enum EnumDTX { Disable = 0, Enable = 1 };

        #endregion

        #region Public Methods

        /// <summary>
        /// Configuration that occurs after the filter is added to 
        /// the graph, but before it is connected
        /// to the source filter.
        /// Here we override the buffer size with the computed value
        /// that the Opus encoder requires.
        /// </summary>
        /// <param name="source"></param>
        public override void PreConnectConfig(Dictionary<string, object> args) {
            SourceFilter source = args["SourceFilter"] as SourceFilter;
            ((AudioSource)source).BufferSize = OpusAudioCompressor.GetBufferSize();
        }

        /// <summary>
        /// Configure the Opus encoder filter
        /// after the encoder is in the graph and connected
        /// to the source.
        /// 
        /// Warning: This might not work for a DVCaptureGraph.
        /// </summary>
        /// <param name="cg"></param>
        /// <param name="j"></param>
        public override void PostConnectConfig(Dictionary<string, Object> args) {
            CaptureGraph cg = args["CaptureGraph"] as CaptureGraph;
            IAudioCaptureGraph iacg = cg as IAudioCaptureGraph;

            _AMMediaType[] mts = Pin.GetMediaTypes(cg.Source.OutputPin);
            bool mtSet = false;
            foreach (_AMMediaType mt in mts) {
                WAVEFORMATEX wfex = (WAVEFORMATEX)MediaType.FormatType.MarshalData(mt);
                if ((wfex.SamplesPerSec == Frequency) && (wfex.Channels == Channels) && (wfex.BitsPerSample == Depth)) {
                    cg.Source.SetMediaType(mt);
                    mtSet = true;
                    break;
                }
            }

            if (!mtSet) {
                throw new ApplicationException("The audio device doesn't support the configured values of SamplesPerSec/Channels/BitsPerSample.");
            }

            IOpusEncoderCtl iOpus = (IOpusEncoderCtl)iacg.AudioCompressor.BaseFilter;
            iOpus.SetSignal(Signal); 
            int br = BitRate;
            if (br == 0)
                br = ManualBitRate;
            iOpus.SetBitRate(br);
            iOpus.SetComplexity(Complexity); 
            iOpus.SetMaxBandwidth(MaxBandwidth); 
            iOpus.SetVbr(VBR); 
            iOpus.SetVbrConstraint(VBRConstraint); 
            iOpus.SetDtx(DTX);
            iOpus.SetPacketLossPerc(PacketLossPerc);
            iOpus.SetLsbDepth(LSBDepth);
            iOpus.SetForcedChannels(ForcedChannels); 
        }


        #endregion

        #region Opus Encoder Interop
        [ComConversionLoss, Guid("F749E87F-5536-4D73-BF44-8179A46C4B61"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComImport]
        public interface IOpusEncoderCtl {

            void GetComplexity([In, Out] ref Int32 x);
            void SetComplexity([In] Int32 x);

            void GetBitRate([In, Out] ref Int32 x);
            void SetBitRate([In] Int32 x);

            void GetVbr([In, Out] ref Int32 x);
            void SetVbr([In] Int32 x);

            void GetVbrConstraint([In, Out] ref Int32 x);
            void SetVbrConstraint([In] Int32 x);

            void GetForcedChannels([In, Out] ref Int32 x);
            void SetForcedChannels([In] Int32 x);

            void GetMaxBandwidth([In, Out] ref Int32 x);
            void SetMaxBandwidth([In] Int32 x);

            void GetSignal([In, Out] ref Int32 x);
            void SetSignal([In] Int32 x);

            void SetApplication([In] Int32 x);
            void GetApplication([In, Out] ref Int32 x);

            void GetSampleRate([In, Out] ref Int32 x);

            void GetLookAhead([In, Out] ref Int32 x);

            void GetInbandFec([In, Out] ref Int32 x);
            void SetInbandFec([In] Int32 x);

            void GetPacketLossPerc([In, Out] ref Int32 x);
            void SetPacketLossPerc([In] Int32 x);

            void GetDtx([In, Out] ref Int32 x);
            void SetDtx([In] Int32 x);

            void GetLsbDepth([In, Out] ref Int32 x);
            void SetLsbDepth([In] Int32 x);

            void GetLastPacketDuration([In, Out] ref Int32 x);

        }
        #endregion

    }

    #endregion OpusAudioCompressor

}