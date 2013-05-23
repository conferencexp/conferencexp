using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Principal;
using System.ServiceProcess;
using System.Windows.Forms;

using Microsoft.Win32;


namespace MSR.LST.ConferenceXP.ArchiveService
{
    [RunInstaller(true)]
    public class ArchiveWindowsServiceInstaller : System.Configuration.Install.Installer
    {
        #region Privates & Constants

        static ushort[] ports = { 8082, 5004, 5005 };
        static ProtocolType[] portTypes = { ProtocolType.Tcp, ProtocolType.Udp, ProtocolType.Udp };
        const string portMapName = "ConferenceXP Archive Service";

        private bool sqlFound;          // keep a note on this, so we can show the error if necessary
        private string sqlServiceName;  
        private string sqlInstanceName;

        #endregion

        #region CTor

        public ArchiveWindowsServiceInstaller()
        {
            // - Define and create the service installer
            ServiceInstaller archiveInstaller = new ServiceInstaller();
            archiveInstaller.StartType = ServiceStartMode.Automatic;
            archiveInstaller.ServiceName = ArchiveWindowsService.ShortName;
            archiveInstaller.DisplayName = ArchiveWindowsService.DisplayName;
            archiveInstaller.ServicesDependedOn = GetDependencies();
            archiveInstaller.Committed += new InstallEventHandler(archiveInstaller_Committed);
            Installers.Add(archiveInstaller);

            // - Define and create the process installer
            ServiceProcessInstaller processInstaller = new ServiceProcessInstaller();
            processInstaller.Account = ServiceAccount.LocalService;
            Installers.Add(processInstaller);

            // - Add the installer for Archiver to the list
            AssemblyInstaller arcInstall = new AssemblyInstaller();
            arcInstall.Assembly = Assembly.Load("Archiver");
            Installers.Add(arcInstall);
        }

        #endregion

        #region Install/Uninstall

        public override void Install(IDictionary savedState)
        {
            CheckAdministratorRole();

            #region Add the firewall punchthrough
            try
            {
                MSR.LST.Net.FirewallUtility.AddFirewallException(portMapName,
                    ports, portTypes, Context.Parameters["assemblypath"]);
            }
            catch (NotSupportedException ex)
            {
                RtlAwareMessageBox.Show(null, string.Format(CultureInfo.CurrentCulture, 
                    Strings.FirewallExceptionAddingText, ex.Message), Strings.FirewallExceptionFailedTitle, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 
                    (MessageBoxOptions)0);
            }
            #endregion

            #region Check for no SQL and show the error message here

            if (!this.sqlFound) // Show the error msg here that we couldn't show earlier
            {
                RtlAwareMessageBox.Show(null, Strings.SqlServerNotFoundText, Strings.SqlServerNotFoundTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
            else // only try to create the database if we know SQL is installed
            {
                CreateDatabase();
                try {
                    DBSetupHelper.SetConfigFiles(InstallPath, this.sqlInstanceName);
                }
                catch (Exception ex){
                    RtlAwareMessageBox.Show(null, ex.ToString(), Strings.ConfigurationFileCustomizationFailed,
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                        (MessageBoxOptions)0);

                }
            }

            #endregion

            base.Install(savedState);
        }

        /// <summary>
        /// Starts the Archive Service after the installation has committed.
        /// </summary>
        private void archiveInstaller_Committed(object sender, InstallEventArgs e)
        {
            // Start the archive service at the end of installation
            try
            {
                ServiceController sc = new ServiceController(ArchiveWindowsService.ShortName);
                sc.Start();
            }
            catch (Exception ex)
            {
                // Don't except - that would cause a rollback.  Instead, just tell the user.
                RtlAwareMessageBox.Show(null, string.Format(CultureInfo.CurrentCulture, 
                    Strings.ServiceStartFailureText, ex.ToString()), Strings.ServiceStartFailureTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1,
                    (MessageBoxOptions)0);
            }
        }

        public override void Uninstall(IDictionary savedState)
        {
            CheckAdministratorRole();

            #region Remove firewall punchthrough
            try
            {
                MSR.LST.Net.FirewallUtility.RemoveFirewallException(portMapName,
                    ports, portTypes, Context.Parameters["assemblypath"]);
            }
            catch (NotSupportedException ex)
            {
                RtlAwareMessageBox.Show(null, string.Format(CultureInfo.CurrentCulture, 
                    Strings.FirewallExceptionRemovingText, ex.Message), Strings.FirewallExceptionFailedTitle, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 
                    (MessageBoxOptions)0);
            }
            #endregion

            #region Make sure the service is stopped.
            try
            {
                ServiceController sc = new ServiceController(ArchiveWindowsService.ShortName);
                if (sc.Status != ServiceControllerStatus.Stopped)
                    sc.Stop();
            }
            catch { }
            #endregion

            if (sqlFound) // don't bother trying to remove it if we already know SQL can't be found
            {
                RemoveDatabase();
            }

            base.Uninstall(savedState);
        }
        #endregion

        #region Helper methods

        /// <summary>
        /// Returns MSSQL as a dependency if it is installed, otherwise returns null.  This allows the service to work
        /// better if SQL is installed, but also allows the service to be installed (not fail) if SQL isn't installed.
        /// (Not having SQL installed is common for proper hosted services, where SQL is in the tier-2 servers.)
        /// </summary>
        private string[] GetDependencies() {
            if (DBSetupHelper.SelectDatabaseInstance(out this.sqlInstanceName, out this.sqlServiceName)) {
                sqlFound = true;
                return new string[] { sqlServiceName };
            }
            else {
                sqlFound = false;
                return null;
            }
        }

        /// <summary>
        /// Gets the installation directory for this install.
        /// </summary>
        private string InstallPath
        {
            get
            {
                string assemblyPath = Context.Parameters["assemblypath"];
                return assemblyPath.Substring(0, assemblyPath.LastIndexOf('\\') + 1);
            }
        }

        /// <summary>
        /// Checks to make sure we're running as the administrator.  If not, exit.
        /// </summary>
        private static void CheckAdministratorRole()
        {
            WindowsPrincipal wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            if (wp.IsInRole(WindowsBuiltInRole.Administrator) == false)
            {
                RtlAwareMessageBox.Show(null, Strings.AdministratorPrivilegesRequiredText,
                    Strings.AdministratorPrivilegesRequiredTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop,
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                Application.Exit();
            }
        }

        /// <summary>
        /// Creates the database and the schema, but does not create the stored procedures.  That's done in CreateStoredProcedures().  Because
        /// wiping the schema & database would cause the user to lose all of their data, this is an optional activity.
        /// </summary>
        private void CreateDatabase()
        {
            // The installer context should tell us whether to create the database.
            // This is the value from the radio buttons in the setup dialog.
            string crdb = this.Context.Parameters["createdatabase"];
            if ((crdb != null) && crdb.Equals("YES")) {

                Console.WriteLine("\n" + Strings.PleaseWaitDatabase);

                try {
                    DBSetupHelper.InstallDatabase(InstallPath, this.sqlInstanceName, this.sqlServiceName);
                }
                catch (Exception ex) {
                    RtlAwareMessageBox.Show(null, string.Format(CultureInfo.CurrentCulture,
                        Strings.DatabaseOperationFailedText, ex.ToString()), Strings.DatabaseOperationFailedTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                        (MessageBoxOptions)0);
                }

                Console.WriteLine(Strings.DatabaseInitializationComplete + "\n");
            }
        }

 
        private void RemoveDatabase()
        {
            // Verify the user wants to delete the database
            DialogResult dr = RtlAwareMessageBox.Show(null, Strings.DeleteDatabaseText, Strings.DeleteDatabaseTitle,
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            if (dr != DialogResult.Yes)
                return;

            try
            {
                DBSetupHelper.RemoveDatabase(InstallPath);
            }
            catch (Exception ex)
            {
                RtlAwareMessageBox.Show(null, string.Format(CultureInfo.CurrentCulture, Strings.RunDropDatabaseSQL, 
                    ex.ToString()), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
        }

        #endregion
    }

}
