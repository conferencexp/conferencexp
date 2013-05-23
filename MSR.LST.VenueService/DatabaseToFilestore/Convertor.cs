using System;
using System.Configuration;
using System.Diagnostics;

using Microsoft.Win32;


namespace MSR.LST.ConferenceXP.VenueService
{
    /// <summary>
    /// Converts a VenueService 1.2 database to a VenueService 2.x filestore
    /// </summary>
    class Convertor
    {
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


            #region Create the file store
            string filePath = ConfigurationManager.AppSettings["FilePath"];
            if (filePath == null || filePath == String.Empty)
            {
                // Use the registry key
                using(RegistryKey key = Registry.LocalMachine.OpenSubKey(StorageInstaller.LocalMachineSubkey))
                {
                    filePath = (string)key.GetValue("DataFile");
                }
            }

            FileStorage filestore = new FileStorage(filePath);
            #endregion

            #region Delete existing participants & venues
            Venue[] oldVenues = filestore.GetVenues();
            Participant[] oldParts = filestore.GetParticipants();
            if (oldVenues.Length == 2 && (oldParts == null || oldParts.Length == 0))
            {
                // This is the default install.  Just delete them.
                foreach(Venue oldVenue in oldVenues)
                {
                    filestore.DeleteVenue(oldVenue.Identifier);
                }
            }
            #endregion

            #region Get the connection string
            string connString = ConfigurationManager.AppSettings["SqlConnectionString"];
            DatabaseStorage database = new DatabaseStorage(connString);
            #endregion

            try
            {
                #region Move the venues
                Console.WriteLine(Strings.GettingVenuesFromDatabase);
                Venue[] venues = database.GetVenues();
                Console.WriteLine(Strings.WritingVenuesToFilestore);
                filestore.WriteCaching = true;
                foreach(Venue ven in venues)
                {
                    string[] acls = database.GetVenueSecurity(ven.Identifier);
                    ven.AccessList = new SecurityPatterns(acls);
                    filestore.AddVenue(ven, new PrivateVenueState());
                }
                filestore.WriteCaching = false;
                #endregion

                #region Move the participants
                Console.WriteLine(Strings.GettingParticipantsFromDatabase);
                Participant[] participants = database.GetParticipants();
                Console.WriteLine(Strings.WritingParticipantsToFilestore);
                filestore.WriteCaching = true;
                foreach(Participant part in participants)
                {
                    filestore.AddParticipant(part);
                }
                filestore.WriteCaching = false;
                #endregion
            }
            catch (System.Data.SqlClient.SqlException)
            {
                Console.WriteLine();
                Console.WriteLine(Strings.DatabaseCouldNotBeProperlyAccessed);
                Console.WriteLine(Strings.PressEnterToExit);
                Console.ReadLine();
            }

            filestore.Dispose();
        }
    }
}
