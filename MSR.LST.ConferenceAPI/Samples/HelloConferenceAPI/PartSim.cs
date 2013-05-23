using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Threading;

using Microsoft.Win32;

using MSR.LST.MDShow;


namespace MSR.LST.ConferenceXP
{
    class PartSim
    {
        private static string venueName = null;
        private static RegistryKey settings = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft Research\ConferenceXP\Client\" + System.Reflection.Assembly.GetExecutingAssembly().CodeBase + @"\HelloConferenceAPI");

        [STAThread]
        static void Main(string[] args)
        {
            #region Set the Venue Name
            if (ConfigurationManager.AppSettings["MSR.LST.ConferenceXP.PartSim.VenueName"] != null)
            {
                venueName = ConfigurationManager.AppSettings["MSR.LST.ConferenceXP.PartSim.VenueName"];
            }
            else
            {
                if (args.Length > 0)
                {
                    #region Unparse the argument strings back into one string
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append(args[0]);
                    for (int i = 1; i < args.Length; i++)
                    {
                        sb.Append(" ");
                        sb.Append(args[i]);
                    }
                    venueName = sb.ToString();
                    #endregion
                }
                else
                {
                    #region If no venue specified as an argument, use the first venue in the list
                    IEnumerator iEnum = Conference.VenueServiceWrapper.Venues.GetEnumerator();
                    iEnum.MoveNext();
                    venueName = ((Venue)iEnum.Current).Name;
                    #endregion
                }
            }
            #endregion

            #region Hook up events
            Conference.ParticipantAdded += new Conference.ParticipantAddedEventHandler(OnParticipantAdded);
            Conference.ParticipantRemoved += new Conference.ParticipantRemovedEventHandler(OnParticipantRemoved);
            Conference.CapabilityAdded += new CapabilityAddedEventHandler(OnCapabilityAdded);
            Conference.CapabilityRemoved += new CapabilityRemovedEventHandler(OnCapabilityRemoved);
            Conference.CapabilityPlaying += new CapabilityPlayingEventHandler(OnCapabilityPlayed);
            Conference.CapabilityStoppedPlaying += new CapabilityStoppedPlayingEventHandler(OnCapabilityStoppedPlaying);
            #endregion

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
                Conference.CallingForm = this;

                #region Choose a Behavior
                if (ConfigurationManager.AppSettings["MSR.LST.ConferenceXP.PartSim.Behavior"] != null)
                {
                    string behaviorName = ConfigurationManager.AppSettings["MSR.LST.ConferenceXP.PartSim.Behavior"];
                
                    switch (behaviorName)
                    {
                        case "InAndOut":
                        {
                            InAndOut();
                            break;
                        }
                        case "CycleSendVideo":
                        {
                            CycleSendVideo();
                            break;
                        }
                        case "CyclePlayCapabilityViewers":
                        {
                            CyclePlayCapabilityViewers();
                            break;
                        }
                        case "JoinAndLeave":
                        {
                            JoinAndLeave();
                            break;
                        }
                        case "JoinAndPlay":
                        {
                            JoinAndPlay();
                            break;
                        }
                        case "JoinAndSend":
                        {
                            JoinAndSend();
                            break;
                        }

                        default:
                        {
                            JoinVenue();
                            break;
                        }
                    }
                }                
                #endregion
            }
        }

        #region Behaviors
        /// <summary>
        /// Default behavior
        /// </summary>
        static void JoinVenue()
        {

            #region Search for a Venue match, join if found
            foreach (Venue v in Conference.VenueServiceWrapper.Venues)
            {
                if (v.Name == venueName)
                {
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.JoiningVenue, venueName));
                    Conference.JoinVenue(v);
                    break;
                }
            }
                #endregion
            #region If no match found, list the available venues
            if (Conference.ActiveVenue == null)
            {
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.UnableToFindVenue, venueName));
                Console.WriteLine(Strings.AvailableVenues);
                foreach (Venue v in Conference.VenueServiceWrapper.Venues)
                    Console.WriteLine("   " + v.Name);
                Console.ReadLine();
                return;
            }
            #endregion

        }

        static void JoinAndLeave()
        {
            JoinVenue();
            Thread.Sleep(15000);
            Conference.LeaveVenue();
        }

        static void JoinAndPlay()
        {
            Conference.AutoPlayRemote = true;
            Conference.AutoPosition = Conference.AutoPositionMode.None;

            // Hook CapabilityPlaying a second time so we can autoposition the window
            Conference.CapabilityPlaying += new CapabilityPlayingEventHandler(RestoreCapabilitySavedPosition);

            Console.WriteLine(Strings.StartingJoinAndPlay);
            JoinVenue();
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.JoinedVenue0As1, 
                Conference.ActiveVenue.Name, Conference.LocalParticipant.Name));
            Console.WriteLine(Strings.PressEnterToSavePlayingWindowPosition);
            Console.ReadLine();
            if (Conference.CapabilityViewers.Count == 0)
            {
                Console.WriteLine(Strings.NoCapabilityViewerFound);
                return;
            }
            if (Conference.CapabilityViewers[0] is ICapabilityWindow)
            {
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.SavingWindowPosition, 
                    Conference.CapabilityViewers[0].Name));
                SaveCapabilityPosition((ICapabilityWindow)Conference.CapabilityViewers[0]);
            }
            else
            {
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                    Strings.CapabilityViewerDoesNotImplementICapabilityWindow, 
                    Conference.CapabilityViewers[0].Name));
            }
        }

        private static void RestoreCapabilitySavedPosition(object conference, CapabilityEventArgs cea)
        {
            // Do Window Positioning here
            ICapabilityWindow cww = null;
            if (cea.Capability is ICapabilityWindow)
            {
                cww = (ICapabilityWindow)cea.Capability;
            }
            else
            {
                return;
            }
            // Load the form location settings from the registry
            if (settings.GetValue("Top") != null)
            {
                cww.Top = Convert.ToInt32(settings.GetValue("Top"), CultureInfo.InvariantCulture);
            }
            if (settings.GetValue("Left") != null)
            {
                cww.Left = Convert.ToInt32(settings.GetValue("Left"), CultureInfo.InvariantCulture);
            }
            if (settings.GetValue("Width") != null)
            {
                cww.Width = Convert.ToInt32(settings.GetValue("Width"), CultureInfo.InvariantCulture);
            }

            if (settings.GetValue("Height") != null)
            {
                cww.Height = Convert.ToInt32(settings.GetValue("Height"), CultureInfo.InvariantCulture);
            }
        }

        private static void SaveCapabilityPosition(ICapabilityWindow cww)
        {
            settings.SetValue("Top", cww.Top);
            settings.SetValue("Left", cww.Left);
            settings.SetValue("Width", cww.Width);
            settings.SetValue("Height", cww.Height);
        }

        static void JoinAndSend()
        {
            #region Validate Inputs
            if (ConfigurationManager.AppSettings["MSR.LST.ConferenceXP.PartSim.JoinAndSend.OtherCapabilityName"] == null)
            {
                Console.WriteLine(Strings.MustSpecifyAnOtherCapabilityName);
                Console.WriteLine(Strings.ValidOtherCapabilitySenders);
                foreach(string s in Conference.OtherCapabilitySenders)
                {
                    Console.WriteLine(s);
                }
                return;
            }
            string otherCapabilitySenderName = ConfigurationManager.AppSettings["MSR.LST.ConferenceXP.PartSim.JoinAndSend.OtherCapabilityName"];

            bool found = false;
            foreach(string s in Conference.OtherCapabilitySenders)
            {
                if (s == otherCapabilitySenderName)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                    Strings.UnableToFindOtherCapabilitySender, otherCapabilitySenderName));
                Console.WriteLine(Strings.ValidOtherCapabilitySenders);
                foreach(string s in Conference.OtherCapabilitySenders)
                {
                    Console.WriteLine(s);
                }
                return;
            }
            #endregion

            // Hook CapabilitySending so we can autoposition the window
            Conference.CapabilitySending += new CapabilitySendingEventHandler(RestoreCapabilitySavedPosition);
            Conference.AutoPosition = Conference.AutoPositionMode.None;

            Console.WriteLine(Strings.StartingJoinAndSend);

            JoinVenue();
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.JoinedVenue0As1, Conference.ActiveVenue.Name, Conference.LocalParticipant.Name));
            
            ICapabilitySender cs = Conference.CreateCapabilitySender(otherCapabilitySenderName);
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.StartedOtherCapabilitySender, 
                otherCapabilitySenderName));

            Console.WriteLine(Strings.PressEnterToSaveSendingWindowPosition);
            Console.ReadLine();
            if (cs.IsSending)
            {
                if (cs is ICapabilityWindow)
                {
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.SavingWindowPosition, 
                        cs.Name));
                    SaveCapabilityPosition((ICapabilityWindow)cs);
                }
                else
                {
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                        Strings.CapabilitySenderDoesNotImplementICapabilityWindow, cs.Name));
                }
                return;
            }
        }

        static void InAndOut()
        {
            long lastMemory = 0;
            long currentMemory = 0;
            int i = 0;
            while (true)
            {
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.InAndOutTestRun + "\n", ++i));

                JoinVenue();
                Thread.Sleep(15000);
                
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.LeavingVenue, 
                    Conference.ActiveVenue.Name));
                Conference.LeaveVenue();
                
                currentMemory = GC.GetTotalMemory(true);
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.MemoryUsed,
                    currentMemory, (currentMemory - lastMemory).ToString(CultureInfo.InvariantCulture)));
                lastMemory = currentMemory;
            }
        }

        static void CycleSendVideo()
        {
            long lastMemory = 0;
            long currentMemory = 0;
            int i = 0;

            VideoCapability vc = null;
            
            FilterInfo[] cameras = VideoSource.Sources();
            if (cameras.Length > 0)
            {
                vc = new VideoCapability(cameras[0]);
                vc.ActivateCamera();
            }

            JoinVenue();
            while (vc != null)
            {
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.CycleSendVideoTestRun + 
                    "\n", ++i));

                vc.BeginSend(null, null);
                Thread.Sleep(15000);
                vc.BeginStopSending(null, null);
                Thread.Sleep(5000);
                
                currentMemory = GC.GetTotalMemory(true);
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.MemoryUsed, currentMemory,
                    (currentMemory - lastMemory).ToString(CultureInfo.InvariantCulture)));
                lastMemory = currentMemory;
            }
        }
        static void CyclePlayCapabilityViewers()
        {
            long lastMemory = 0;
            long currentMemory = 0;
            int i = 0;

            JoinVenue();
            while (true)
            {
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, 
                    Strings.CyclePlayCapabilityViewersTestRun + "\n", ++i));

                foreach (ICapabilityViewer cv in Conference.CapabilityViewers)
                {
                    cv.BeginPlay(null, null);
                }
                Thread.Sleep(15000);

                foreach (ICapabilityViewer cv in Conference.CapabilityViewers)
                {
                    cv.BeginStopPlaying(null, null);
                }
                Thread.Sleep(5000);
                
                currentMemory = GC.GetTotalMemory(true);
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.MemoryUsed, currentMemory,
                    (currentMemory - lastMemory).ToString(CultureInfo.InvariantCulture)));
                lastMemory = currentMemory;
            }
        }
        #endregion

        #region Event Handlers
        private static void OnCapabilityAdded(object conference, CapabilityEventArgs cae)
        {
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.Added, cae.Capability.ToString(), 
                cae.Capability.Name));
        }

        private static void OnCapabilityRemoved(object conference, CapabilityEventArgs cae)
        {
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.Removed, 
                cae.Capability.ToString(), cae.Capability.Name));
        }

        private static void OnCapabilityPlayed(object conference, CapabilityEventArgs cea)
        {
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.Played, 
                cea.Capability.ToString(), cea.Capability.Name));
        }

        private static void OnCapabilityStoppedPlaying(object conference, CapabilityEventArgs cea)
        {
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.StoppedPlaying, 
                cea.Capability.ToString(), cea.Capability.Name));
        }

        private static void OnParticipantAdded(IParticipant p)
        {
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.ParticipantAdded, p.Name ));
        }

        private static void OnParticipantRemoved(IParticipant p)
        {
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.ParticipantRemoved, p.Name ));
        }
        #endregion
    }
}
