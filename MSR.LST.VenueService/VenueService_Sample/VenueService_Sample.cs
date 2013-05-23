using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;

using Microsoft.Win32;

using MSR.LST.ConferenceXP.VenueService;

using VS = MSR.LST.ConferenceXP.VenueService.VenueService;


namespace MSR.LST.ConferenceXP.VenueService
{
    /// <summary>
    /// This is a BVT for VenueService
    /// </summary>
    class VenueService_Sample
    {
        private EventLog eventLog;

        public VenueService_Sample()
        {
            //Set up EventLog
            if (!EventLog.SourceExists("BVT_Venue"))
                EventLog.CreateEventSource("BVT_Venue", "TestLog");
            eventLog = new EventLog("TestLog",".","BVT_Venue");
        }
        public void WriteToLog(string message, EventLogEntryType eventLogEntryType)
        {
            Console.WriteLine(message);
            eventLog.WriteEntry(message, eventLogEntryType);
        }

        [STAThread]
        static void Main(string[] args)
        {
            // Override the system UICulture
            string cultureOverride = null;
            if ((cultureOverride = ConfigurationManager.AppSettings["MSR.LST.ConferenceXP.VenueService.UICulture"]) != null) {
                try {
                    System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(cultureOverride);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                }
                catch { }
            }

            string message;
            VenueService_Sample VenueService_Sample = null;

            try
            {
                // Start by getting the venue service storage (use the app.config as an override)
                string filePath = ConfigurationManager.AppSettings["FilePath"];
                if (filePath == null || filePath == String.Empty)
                {
                    // Use the registry key
                    using(RegistryKey key = Registry.LocalMachine.OpenSubKey(StorageInstaller.LocalMachineSubkey))
                    {
                        filePath = (string)key.GetValue("DataFile");
                    }
                }
                FileStorage storage = new FileStorage(filePath);

                VenueService_Sample = new VenueService_Sample();

                message = Strings.VenueServiceSampleStartedSuccessfully;
                VenueService_Sample.WriteToLog(message, EventLogEntryType.Information);

                // Create a reference to the VenueService web service
                Console.WriteLine(Strings.CreateAReferenceToTheVenueServiceWebService);
                MSR.LST.ConferenceXP.VenueService.VenueService.VenueService vs = new MSR.LST.ConferenceXP.VenueService.VenueService.VenueService();

                // Cleanup - remove venues and particpants in case the sample was cancelled before it finished
                // Will fail silently if not present
                try
                {
                    storage.DeleteVenue("venue1@microsoft.com");
                }
                catch {}
                try
                {
                    storage.DeleteVenue("venue2@microsoft.com");
                }
                catch {}
                try
                {
                    storage.DeleteParticipant("participant1@microsoft.com");
                }
                catch {}
                try
                {
                    storage.DeleteParticipant("participant2@microsoft.com");
                }
                catch {}


                Console.WriteLine(Strings.CreateVenuesCalledV1AndV2);
                // Create the first venue called v1, and update the attributes
                // Create the security so venue1@ms and anyone at hotmail can join
                Venue v1 = new Venue();
                v1.Name = "Test Venue 1";
                v1.Identifier = "venue1@microsoft.com";
                v1.Icon = null;
                v1.IPAddress = "233.233.233.10";
                v1.Port = 1000;
                v1.AccessList = new SecurityPatterns(new String[] { @"[\w-]@hotmail.com" });
                storage.AddVenue(v1, new PrivateVenueState());

                // Create the second venue called v2, and update the attributes
                // Create the security so venue2@ms and anyone at msn can join
                Venue v2 = new Venue();
                v2.Name = "Test Venue 2";
                v2.Identifier = "venue2@microsoft.com";
                v2.Icon = null;
                v2.IPAddress = "233.233.233.11";
                v2.Port = 1000;
                v2.AccessList = new SecurityPatterns(new String[] { @"[\w-]@msn.com" });
                storage.AddVenue(v2, new PrivateVenueState());

                Console.WriteLine(Strings.TestVenueSecurity);
                // Get the list of venues for jav@hotmail and verify which venues he can see
                VS.Venue[] vList = vs.GetVenues("jay@Hotmail.Com");
                bool v1Found = false;
                bool v2Found = false;
                foreach(VS.Venue v in vList)
                {
                    if (v.Identifier == v1.Identifier)
                    {
                        v1Found = true;
                    }
                    else if(v.Identifier == v2.Identifier)
                    {
                        v2Found = true;
                    }
                }

                // Validate that only allowed people can join v1
                // positive case
                if (!v1Found)
                {
                    message = Strings.PermissionFailedSecurityError;
                    throw new Exception(message); 
                }

                // negative case
                if (v2Found)
                {
                    message = Strings.PermissionFailedNegativeCaseSecurityError;
                    throw new Exception(message); 
                }

                Console.WriteLine(Strings.UpdateTheNameAttributeOfV1);
                // Update the name attribute of v1 
                v1.Name = "Test Venue 1 Updated";
                storage.UpdateVenue(new VenueState(v1, new PrivateVenueState()));

                // Verify the name change of v1 by retriving the name from the server 
                // and compare against the local name.
                if (vs.GetVenue(v1.Identifier).Name != v1.Name)
                {
                    message = string.Format(CultureInfo.CurrentCulture, Strings.VenueNameChangeFailed, vs.GetVenue(v1.Identifier).Name.ToString(), v1.Name.ToString());
                    throw new Exception(message); 
                }

                Console.WriteLine(Strings.DeleteV1AndV2);
                // Delete v1
                storage.DeleteVenue(v1.Identifier);

                // Verify v1 is deleted
                if (vs.GetVenue(v1.Identifier) != null)
                {
                    message = Strings.FailedV1CannotBeDeleted;
                    throw new Exception(message); 
                }

                // Delete v2
                storage.DeleteVenue(v2.Identifier);

                // Verify v2 is deleted
                if (vs.GetVenue(v2.Identifier) != null)
                {
                    message = Strings.FailedV2CannotBeDeleted;
                    throw new Exception(message); 

                }

                Console.WriteLine(Strings.CreateParticipants);
                //Create the first participant called p1, and update the atrributes
                VS.Participant p1 = new VS.Participant();
                p1.Name = "Test Participant 1";
                p1.Icon = null;
                p1.Identifier = "participant1@microsoft.com";
                vs.AddParticipant(p1);

                //Create the second participant called p2, and update the attributes
                VS.Participant p2 = new VS.Participant();
                p2.Name = "Test Participant 2";
                p2.Icon = null;
                p2.Identifier="participant2@microsoft.com";
                vs.AddParticipant(p2);

                // Verify the participants (p1 and p2) are created by checking the
                // total number of participants existed
                // Just check number is >= 2 so that we don't fail on sample venues.
                VS.Participant[] pList = vs.GetParticipants();
                if (pList.Length < 2)
                {
                    message = string.Format(CultureInfo.CurrentCulture, Strings.IncorrectNumberOfParticipants, 
                        pList.Length);
                    throw new Exception(message); 

                }

                Console.WriteLine(Strings.UpdateTheNameAttributeOfP1);
                // Update the name attribute of p1
                p1.Name = "Test Participant 1 Updated";
                vs.UpdateParticipant(p1);

                // Verify the name change of p1 by retriving the name from the server 
                // and compare against the local name.
                if (vs.GetParticipant(p1.Identifier).Name != p1.Name)
                {
                    message = string.Format(CultureInfo.CurrentCulture, Strings.ParticipantNameChangeFailed, 
                        vs.GetParticipant(p1.Identifier).Name.ToString(), p1.Name.ToString());
                    throw new Exception(message); 

                }

                Console.WriteLine(Strings.DeleteP1AndP2);
                // Delete p1
                storage.DeleteParticipant(p1.Identifier);

                // Verify p1 is deleted
                if (vs.GetParticipant(p1.Identifier) != null)
                {
                    message = Strings.FailedDeleteParticipantP1;
                    throw new Exception(message); 

                }

                // Delete p2
                storage.DeleteParticipant(p2.Identifier);

                // Verify p2 is deleted
                if (vs.GetParticipant(p2.Identifier) != null)
                {
                    message = Strings.FailedDelteParticipantP2;
                    throw new Exception(message); 

                }

                message = Strings.VenueServiceSampleEndedSuccessfully;
                VenueService_Sample.WriteToLog(message, EventLogEntryType.Information);

                Console.WriteLine(Strings.PressAKeyToContinue);
                Console.Read();
                return;

            }
            catch(Exception ex)
            {
                message = ex.ToString();
                VenueService_Sample.WriteToLog(message, EventLogEntryType.Error);

                message = Strings.TestEndedAbnormally;
                VenueService_Sample.WriteToLog(message, EventLogEntryType.Information);
                return;
            }
        }

    }
}
