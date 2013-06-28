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
    class BlackMagicSource: VideoSource {

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
        /// <summary>
        /// Use the static constructor and reflection to set public static fields from app.config
        /// </summary>
        static OpusAudioCompressor() {
            Type myType = typeof(OpusAudioCompressor);
            FieldInfo[] fields = myType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields) {
                if (field.IsLiteral) // Is Constant?
                    continue; // We can't set it
                string fullName = field.Name;
                string configOverride = System.Configuration.ConfigurationManager.AppSettings["MSR.LST.MDShow.OpusAudioCompressor." + fullName];
                if (configOverride != null && configOverride != String.Empty) {
                    Type newType = field.FieldType;
                    object newValue = Convert.ChangeType(configOverride, newType, System.Globalization.CultureInfo.InvariantCulture);
                    field.SetValue(null, newValue);
                }
            }
        }

        public OpusAudioCompressor(FilterInfo fi) : base(fi) { }
        #endregion

        #region Static

        #region Configuration Properties
        
        public static int Frequency = 48000;
        public static int Channels = 2;
        public static int Depth = 16;
        public static int BufferMS = 20;
        public static int Signal = 3002;
        public static int BitRate = -1000;
        public static int Complexity = 10;
        public static int VBR = 1;
        public static int VBRConstraint = 1;
        public static int ForcedChannels = -1000;
        public static int MaxBandwith = 1105;
        public static int DTX = 0;
        public static int PacketLossPerc = 0;
        public static int LSBDepth = 24;

        #endregion

        /// <summary>
        /// The encoder requires a very specific buffer size
        /// </summary>
        /// <returns></returns>
        public static int GetBufferSize() {
            return (Frequency / 1000) * BufferMS * Channels * (Depth / 8);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determine if a particular format is compatible with the encoder.
        /// </summary>
        /// <returns></returns>
        public bool WorksWithOpus(WAVEFORMATEX wfex) {
            if (wfex.BitsPerSample != 16)
                return false;

            if (wfex.Channels != 1 && wfex.Channels != 2) {
                return false;
            }

            if ((wfex.SamplesPerSec != 8000) &&
                (wfex.SamplesPerSec != 12000) &&
                (wfex.SamplesPerSec != 16000) &&
                (wfex.SamplesPerSec != 24000) &&
                (wfex.SamplesPerSec != 48000)) {
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Configuration that occurs after the filter is added to 
        /// the graph, but before it is connected
        /// to the source filter.
        /// Here we override the buffer size with the computed value
        /// that the Opus encoder requires.
        /// </summary>
        /// <param name="source"></param>
        public override void PreConnectConfig(Dictionary<string,object> args) {
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
        public override void PostConnectConfig(Dictionary<string,Object> args) {
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
            iOpus.SetSignal(Signal); //3002 is music
            iOpus.SetBitRate(BitRate);
            iOpus.SetComplexity(Complexity); //default 10
            iOpus.SetMaxBandwidth(MaxBandwith); //1105 is fullband
            iOpus.SetVbr(VBR); //default 1
            iOpus.SetVbrConstraint(VBRConstraint); //default 1
            iOpus.SetDtx(DTX);
            iOpus.SetPacketLossPerc(PacketLossPerc);
            iOpus.SetLsbDepth(LSBDepth);
            iOpus.SetForcedChannels(ForcedChannels); //2 = stereo; -1000 = auto
        }


        #endregion

    }

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

    #endregion OpusAudioCompressor

}
