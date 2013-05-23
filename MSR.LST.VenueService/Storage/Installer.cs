using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.AccessControl;
using System.Windows.Forms;

using Microsoft.Win32;


namespace MSR.LST.ConferenceXP.VenueService
{
    /// <summary>
    /// Summary description for Installer.
    /// </summary>
    [RunInstaller(true)]
    public class StorageInstaller : System.Configuration.Install.Installer
    {
        public static readonly string LocalMachineSubkey = @"Software\Microsoft Research\ConferenceXP\Venue Service\";

        static StorageInstaller()
        {
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            LocalMachineSubkey += string.Format(CultureInfo.InvariantCulture, "{0}.{1}", v.Major, v.Minor);
        }

        public StorageInstaller()
        {
        }

        private string InstallPath
        {
            get
            {
                // This is set on the custom Install action as CustomActionData
                // And is pulled from the MSI installer
                return Context.Parameters["webdir"];
            }
        }

        public override void Install(IDictionary savedState)
        {
            base.Install(savedState);

            // Create .dat file
            //
            // NOTE: during install we can't access the Web.config file, so the file name
            //  is just hard-coded here.  Given that you can't change the web.config file
            //  during install, this really shouldn't be a problem.  If someone changes the
            //  file location, then they'll have to deal with the problem of file permissions.
            //
            Console.WriteLine(Strings.CreatingVenueServiceDataFile);
            string filepath = InstallPath + @"VenueService.dat";
            if( !File.Exists(filepath) )
            {
                using (FileStorage file = new FileStorage(filepath))
                {
                    // Add two sample venues
                    file.AddVenue(new Venue("someone@hotmail.com", "234.7.13.19", 5004,
                        "Sample Venue 1", null, null), new PrivateVenueState());
                    file.AddVenue(new Venue("somebody@hotmail.com", "235.8.14.20", 5004,
                        "Sample Venue 2", null, null),new PrivateVenueState());
                }
            }
            else
            {
                Console.WriteLine(Strings.DataFileExistsSkippingCreatingFile);
            }

            // Store the location of the .dat file so the admin tool can find it
            using(RegistryKey key = Registry.LocalMachine.CreateSubKey(LocalMachineSubkey))
            {
                key.SetValue("DataFile", filepath);
            }

            try
            {
                // In Venue Service 4.0 and prior, we used impersonation and the
                // IUSR_Machine account (having troubles when the machine name was
                // changed, but not the account name to match it).  In Vista, 
                // impersonation would cause an HTTP 500 error, and so we disabled
                // impersonation.  That in turn changed the user account that was
                // being used to read / write venueservice.dat.  We added security to the
                // folder (not just the file) so that in the event the file was deleted,
                // an empty one could be reconstructed.
                DirectorySecurity folderSec = Directory.GetAccessControl(InstallPath);

                if ((Environment.OSVersion.Version.Major == 5 && 
                     Environment.OSVersion.Version.Minor >= 2) ||
                     Environment.OSVersion.Version.Major >= 6) // Win2K3 or Vista and above (Longhorn Server, etc.)
                {
                    folderSec.AddAccessRule(new FileSystemAccessRule("NETWORK SERVICE",
                        FileSystemRights.Modify,
                        InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                        PropagationFlags.None, AccessControlType.Allow));
                }
                else if (Environment.OSVersion.Version.Major == 5 && 
                         Environment.OSVersion.Version.Minor == 1) // WinXP
                {
                    folderSec.AddAccessRule(new FileSystemAccessRule("ASPNET",
                        FileSystemRights.Modify,
                        InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                        PropagationFlags.None, AccessControlType.Allow));
                }
                else // Win2K and other down level OSes
                {
                    throw new ApplicationException(Strings.OSVersion);
                }

                Directory.SetAccessControl(InstallPath, folderSec);
            }
            catch(Exception)
            {
                RtlAwareMessageBox.Show(null, Strings.FileSecurityChangesFailedText, Strings.FileSecurityChangesFailedTitle, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
        }
        
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall (savedState);

            try
            {
                // Delete the registry key
                Registry.LocalMachine.DeleteSubKeyTree(LocalMachineSubkey);
            }
            catch{}
        }
    }
}
