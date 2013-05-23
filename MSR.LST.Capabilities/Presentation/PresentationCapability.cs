using System;
using System.Threading;
using System.Windows.Forms;

// To use RTDocument
using MSR.LST.ConferenceXP;
using MSR.LST.RTDocuments;
using MSR.LST.RTDocuments.Utilities;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// PresentationCapability is a capability example that contains slides and ink.
    /// </summary>

    [Capability.Name("Presentation")]
    [Capability.PayloadType(PayloadType.RTDocument)]
    [Capability.MaxBandwidth(100000)]
    [Capability.FormType(typeof(PresentationCapabilityFMain))]
    [Capability.Channel(false)]
    [Capability.BackgroundSender(true)]
    public class PresentationCapability: CapabilityWithWindow, ICapabilitySender, ICapabilityViewer
    {
        // Required ctor for ICapabilitySender
        public PresentationCapability():base()
        {
            // Instance name
            name = Strings.ConferencexpPresentation;
        }

        // Required ctor for ICapabilityViewer
        public PresentationCapability(DynamicProperties dynaProps) : base(dynaProps)
        {
            // When initialized as a player, this capability disables the sending of slides
            // so there is no need for a background senders
            capProps.BackgroundSender = false;
        }

        /// <summary>
        /// Presentation is a 2 way capability (it is always a sender and receiver)
        /// So when we are initialized to Play, make sure we Send also
        /// </summary>
        public override void Play()
        {
            base.Play ();

            Send();
        }

        /// <summary>
        /// Presentation is a 2 way capability (it is always a sender and receiver)
        /// So when we StopPlaying, make sure we StopSending also
        /// </summary>
        public override void StopPlaying()
        {
            base.StopPlaying ();

            StopSending();
        }
    }
}
