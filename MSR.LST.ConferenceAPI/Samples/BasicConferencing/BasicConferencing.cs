using System;

using MSR.LST.ConferenceXP;
using MSR.LST.MDShow;


class BasicConferencing
{
    [STAThread]
    static void Main(string[] args)
    {
        System.Windows.Forms.Application.Run(new FHelloCXP());
    }

    public class FHelloCXP : System.Windows.Forms.Form
    {
        public FHelloCXP()
        {
            this.Text = Strings.HelloConferencexp;
            this.Load += new System.EventHandler(this.FHelloCXP_Load);
        }

        private void FHelloCXP_Load(object sender, System.EventArgs e)
        {
            // ConferenceAPI to autoplay any incoming streams
            Conference.AutoPlayLocal = true;
            Conference.AutoPlayRemote = true;

            // Tell ConferenceApi who the calling form is so it knows how to send events to the form on the correct thread
            Conference.CallingForm = this;

            // ConferenceAPI to autoposition any new windows
            Conference.AutoPosition = Conference.AutoPositionMode.Tiled;

            // Don't connect to a Venue Service; create a local venue and join it.
            Conference.VenueServiceWrapper.VenueServiceUrl = null;
            Venue localVenue = Conference.VenueServiceWrapper.CreateRandomMulticastVenue("Local Venue", null);
            Conference.JoinVenue( localVenue );

            // Send audio and video streams
            FilterInfo[] cameras = VideoSource.Sources();

            if (cameras.Length > 0)
            {
                VideoCapability vc = new VideoCapability(cameras[0]);
                vc.ActivateCamera();
                vc.Send();
            }
        }
    }
}