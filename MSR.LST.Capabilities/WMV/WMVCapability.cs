using System;
using System.Configuration;

using MSR.LST.Net.Rtp;


namespace MSR.LST.ConferenceXP
{
    [Capability.Name("Windows Media Playback")]
    [Capability.FormType(typeof(frmWMFile))]
    [Capability.PayloadType(PayloadType.dynamicVideo)] // Necessary, but unused here
    [Capability.Channel(false)]
    public class WMFileCapability : CapabilityWithWindow, ICapabilitySender
    {
        private RtpSenderProperties videoProps;
        private RtpSenderProperties audioProps;

        private RtpSender videoSender;
        private RtpSender audioSender;

        // Required ctor for ICapabilitySender
        public WMFileCapability()
            : base()
        {
            // Audio and video should go to the same form
            Guid sharedFormID = Guid.NewGuid();

            // RtpSender properties for Video
            videoProps = new Capability.RtpSenderProperties();
            videoProps.Channel = Channel;
            videoProps.OwnedByLocalParticipant = true;
            videoProps.DelayBetweenPackets = delayBetweenPackets;
            videoProps.FecChecksum = capProps.FecChecksum;
            videoProps.FecData = capProps.FecData;
            videoProps.ID = Guid.NewGuid();
            videoProps.Name = "Windows Media Playback";
            videoProps.PayloadType = MSR.LST.Net.Rtp.PayloadType.dynamicVideo;
            videoProps.SharedFormID = sharedFormID;

            // RtpSender properties for Audio
            audioProps = new Capability.RtpSenderProperties();
            audioProps.Channel = Channel;
            audioProps.OwnedByLocalParticipant = true;
            audioProps.DelayBetweenPackets = delayBetweenPackets;
            audioProps.FecChecksum = capProps.FecChecksum;
            audioProps.FecData = capProps.FecData;
            audioProps.ID = Guid.NewGuid();
            audioProps.Name = "Windows Media Playback";
            audioProps.PayloadType = MSR.LST.Net.Rtp.PayloadType.dynamicAudio;
            audioProps.SharedFormID = sharedFormID;
        }


        // Override of Capability base class
        public override void Send()
        {
            lock (this)
            {
                if (!IsSending)
                {
                    base.Send();

                    // Pass RtpSenders to form
                    ((frmWMFile)form).RtpSenders(audioSender, videoSender);

                    // Check app.config for auto-send file 
                    string setting = ConfigurationManager.AppSettings["MSR.LST.ConferenceXP.WMPlaybackAutoSendFile"];
                    if (setting != null)
                    {
                        ((frmWMFile)form).AutoSend(setting);
                    }
                }
            }
        }

        protected override void CreateRtpSenders()
        {
            //
            // Don't call base.CreateRtpSenders, because we need to customize the 
            // creation of our RtpSenders
            //
            videoSender = CreateRtpSender(videoProps);
            audioSender = CreateRtpSender(audioProps);
        }
    }
}
