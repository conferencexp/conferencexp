using System;


namespace MSR.LST.ConferenceXP
{
    [Capability.Name("Rich Text Chat")]
    [Capability.PayloadType(PayloadType.RichTextChat)]
    [Capability.FormType(typeof(RichTextChatForm))]
    [Capability.Channel(true)]
    public class RichTextChatCapability : CapabilityWithWindow, ICapabilitySender, ICapabilityViewer
    {
        // Required ctor for ICapabilitySender
        public RichTextChatCapability() : base()
        {
            // Instance name
            name = Microsoft.VisualBasic.Interaction.InputBox(Strings.EnterATopic, Strings.Topic, 
                Strings.RichTextChat, 0, 0);

            // Do something minimal for the case in which cancel is pressed
            if (name == string.Empty)
            {
                name = Strings.RichTextChat;
            }
        }

        // Required ctor for ICapabilityViewer
        public RichTextChatCapability(DynamicProperties dynaProps) : base(dynaProps) {}

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