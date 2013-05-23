using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.Win32;

namespace MSR.LST.MDShow {

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
}
