using System;


namespace MSR.LST.ConferenceXP
{
    [Capability.Name("Local Screen Streaming")]
    [Capability.FormType(typeof(frmScreenScraperSimple))]
    [Capability.PayloadType(PayloadType.dynamicVideo)]
    [Capability.Channel(false)]
    public class ScreenScraperCapability : CapabilityWithWindow, ICapabilitySender
    {
        // Required ctor for ICapabilitySender
        public ScreenScraperCapability() : base() 
        {
            name = Conference.LocalParticipant.Name + " - " + name;
        }
    }
}