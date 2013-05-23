using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

using Microsoft.Win32;


namespace MSR.LST
{
    [RunInstaller(true)]
    public class Installation : Installer
    {
        #region Statics
        private const string unExLogName = "UnEx";
        private static string[] unExLogSources = {"MSR.LST.UnEx"};
        #endregion

        public Installation() : base() {}

        /// <summary>
        /// This routine should be called automatically by the MSI during setup, but it can also be called using:
        ///     "installutil.exe UnhandledExceptionHandler.dll"
        /// </summary>
        /// <param name="savedState">State dictionary passed in by the installer code</param>
        public override void Install (IDictionary savedState)
        {
            #region Check to make sure we're in an Administrator role
            VerifyAdministrator();
            #endregion

            #region Uninstall in case we weren't uninstalled cleanly before
            IDictionary state = new Hashtable();
            Uninstall(state);
            if (state.Count != 0)
                Commit(state);
            state = null;
            #endregion

            #region Call base.Install
            base.Install(savedState);
            #endregion

            #region Install the Event Logs
            LstEventLog.Install(unExLogName, unExLogSources);
            #endregion
        }

        /// <summary>
        /// This routine should be called automatically by the MSI during Remove Programs, but it can also be called using:
        ///     "installutil.exe /u UnhandledExceptionHandler.dll"
        /// </summary>
        /// <param name="savedState">State dictionary passed in by the installer code</param>
        public override void Uninstall (IDictionary savedState)
        {
            #region Check to make sure we're in an Administrator role
            VerifyAdministrator();
            #endregion

            #region Whack the Event Log
            LstEventLog.Uninstall(unExLogName, unExLogSources);
            #endregion
            
            #region Call base.Uninstall
            if (savedState.Count != 0)
                base.Uninstall(savedState);
            #endregion
        }

        private static void VerifyAdministrator()
        {
            WindowsPrincipal wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            if (wp.IsInRole(WindowsBuiltInRole.Administrator) == false)
            {
                RtlAwareMessageBox.Show(null, Strings.YouMustBeAnAdministratorToInstall, 
                    Strings.AdministratorPrivilegesRequired, MessageBoxButtons.OK, MessageBoxIcon.Stop, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                Application.Exit();
            }
        }
    }
}
