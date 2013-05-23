using System;

using MSR.LST.ConferenceXP;


namespace MSR.LST.ConferenceXP
{
    [Capability.Name("Chat")]
    [Capability.PayloadType(PayloadType.Chat)]
    [Capability.FormType(typeof(ChatFMain))]
    [Capability.Channel(true)]
    public class ChatCapability : CapabilityWithWindow, ICapabilitySender, ICapabilityViewer
    {
        // Required ctor for ICapabilitySender
        public ChatCapability() : base()
        {
            // Instance name
            name = Microsoft.VisualBasic.Interaction.InputBox(Strings.EnterATopic, Strings.Topic, Strings.Chat, 0, 0);

            // Do something minimal for the case in which cancel is pressed
            if (name == string.Empty)
            {
                name = Strings.Chat;
            }
        }

        // Required ctor for ICapabilityViewer
        public ChatCapability(DynamicProperties dynaProps) : base(dynaProps) {}

        /// <summary>
        /// Chat is a 2 way capability (it is always a sender and receiver)
        /// So when we are initialized to Play, make sure we Send also
        /// </summary>
        public override void Play()
        {
            base.Play ();

            Send();
        }

        /// <summary>
        /// Chat is a 2 way capability (it is always a sender and receiver)
        /// So when we StopPlaying, make sure we StopSending also
        /// </summary>
        public override void StopPlaying()
        {
            base.StopPlaying ();

            StopSending();
        }
    }
}
