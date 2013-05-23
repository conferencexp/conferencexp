using System;
using System.Collections;
using System.Globalization;
using System.Threading;


namespace MSR.LST.ConferenceXP
{
    class HelloConferenceAPI
    {
        [STAThread]
        static void Main(string[] args)
        {

            string venueName = null;

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
                // Don't connect to a Venue Service; create a local venue
                Conference.VenueServiceWrapper.VenueServiceUrl = null;
                Venue localVenue = Conference.VenueServiceWrapper.CreateRandomMulticastVenue("Local Venue", null);
                venueName = localVenue.Name;
                #endregion
            }

            #region Hook up automatic functionality
            Conference.AutoPlayLocal = true;
            Conference.AutoPlayRemote = true;
            //Conference.AutoSend = true;
            #endregion

            #region Hook up events
            Conference.ParticipantAdded += new Conference.ParticipantAddedEventHandler(OnParticipantAdded);
            Conference.ParticipantRemoved += new Conference.ParticipantRemovedEventHandler(OnParticipantRemoved);
            Conference.CapabilityAdded += new CapabilityAddedEventHandler(OnCapabilityAdded);
            Conference.CapabilityRemoved += new CapabilityRemovedEventHandler(OnCapabilityRemoved);
            #endregion

            //JoinAVenue();
            Standard(venueName);

            Console.ReadLine();

        }

        static void Standard(string venueName)
        {

            #region Search for a Venue match, join if found
            foreach (Venue v in Conference.VenueServiceWrapper.Venues)
            {
                if (v.Name == venueName) // BugBug, spaces parsing
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

        static void JoinAVenue()
        {
            long lastMemory = 0;
            long currentMemory = 0;
            for (int i = 0; i < 25; i++)
            {
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.TestRun + "\n", i));
                Standard("Virtual Cubicle");
                Thread.Sleep(15000);
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.LeavingVenue, Conference.ActiveVenue.Name));
                Conference.LeaveVenue();
                currentMemory = GC.GetTotalMemory(true);
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.MemoryUsed, currentMemory, 
                    (currentMemory - lastMemory).ToString(CultureInfo.InvariantCulture)));
                lastMemory = currentMemory;
            }
        }

        #region Event Handlers
        private static void OnCapabilityAdded(object conference, CapabilityEventArgs cae)
        {
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.Added, 
                cae.Capability.ToString(), cae.Capability.Name));
        }

        private static void OnCapabilityRemoved(object conference, CapabilityEventArgs cae)
        {
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.Removed, 
                cae.Capability.ToString(), cae.Capability.Name));
        }

        private static void OnParticipantAdded(IParticipant p)
        {
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.ParticipantAdded, p.Name));
        }

        private static void OnParticipantRemoved(IParticipant p)
        {
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.ParticipantRemoved, p.Name));
        }
        #endregion
    }
}
