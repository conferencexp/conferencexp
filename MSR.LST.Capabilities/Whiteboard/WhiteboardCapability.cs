using System;

using MSR.LST.ConferenceXP;


namespace MSR.LST.ConferenceXP
{
    [Capability.Name("Whiteboard")]
    [Capability.PayloadType(PayloadType.Whiteboard)]
    [Capability.FormType(typeof(WhiteboardForm))]
    [Capability.Channel(true)]
    public class WhiteboardCapability : CapabilityWithWindow, ICapabilitySender, ICapabilityViewer
    {
        // Required ctor for ICapabilitySender
        public WhiteboardCapability() : base()
        {
            // Instance name
            name = Microsoft.VisualBasic.Interaction.InputBox(Strings.EnterATopic, Strings.Topic, 
                Strings.Whiteboard, 0, 0);

            // Do something minimal for the case in which cancel is pressed
            if (name == string.Empty)
            {
                name = Strings.Whiteboard;
            }
        }

        // Required ctor for ICapabilityViewer
        public WhiteboardCapability(DynamicProperties dynaProps) : base(dynaProps) {}

        /// <summary>
        /// Whiteboard is a 2 way capability (it is always a sender and receiver)
        /// So when we are initialized to Play, make sure we Send also
        /// </summary>
        public override void Play()
        {
            base.Play ();

            Send();
        }

        /// <summary>
        /// Whiteboard is a 2 way capability (it is always a sender and receiver)
        /// So when we StopPlaying, make sure we StopSending also
        /// </summary>
        public override void StopPlaying()
        {
            base.StopPlaying ();

            StopSending();
        }
    }
}