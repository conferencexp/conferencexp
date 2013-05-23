using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;

using Microsoft.Win32;


namespace MSR.LST.ConferenceXP.ArchiveService
{
    [RunInstaller(true)]
    public class Installation : System.Configuration.Install.Installer
    {
        public const string baseCxpRegKeyName = "SOFTWARE\\Microsoft Research\\ConferenceXP";

        public Installation() : base() 
        {
            // - Add the installer for MSR.LST.Net.Rtp to the list
            AssemblyInstaller rtpInstall = new AssemblyInstaller();
            rtpInstall.Assembly = Assembly.Load("MSR.LST.Net.Rtp");
            Installers.Add(rtpInstall);
        }

        public static bool Installed
        {
            get
            {
                bool installed = false;
                try
                {
                    using (RegistryKey pcaKey = Registry.LocalMachine.OpenSubKey(baseCxpRegKeyName))
                    {
                        if (pcaKey != null)
                        {
                            object o = pcaKey.GetValue("ArchiverInstalled");
                            if (o != null)
                                installed = Convert.ToBoolean(o, CultureInfo.InvariantCulture);
                        }
                    }
                } 
                catch {}

                return installed;
            }
            set
            {
                try
                {
                    if(value)
                    {
                        using (RegistryKey pcaKey = Registry.LocalMachine.CreateSubKey(baseCxpRegKeyName))
                        {
                            pcaKey.SetValue("ArchiverInstalled", true);
                        }
                    }
                    else
                    {
                        using (RegistryKey pcaKey = Registry.LocalMachine.OpenSubKey(baseCxpRegKeyName, true))
                        {
                            pcaKey.DeleteValue("ArchiverInstalled");
                        }
                    }
                }
                catch {}
            }
        }

        /// <summary>
        /// This routine should be called automatically by the MSI during setup, but it can also be called using:
        ///     "installutil.exe Archiver.dll"
        /// </summary>
        /// <param name="savedState">State dictionary passed in by the installer code</param>
        public override void Install (IDictionary savedState)
        {
            #region Check to make sure we're in an Administrator role
            WindowsPrincipal wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            if (wp.IsInRole(WindowsBuiltInRole.Administrator) == false)
            {
                RtlAwareMessageBox.Show(null, Strings.AdministratorPrivilegesText, Strings.AdministratorPrivilegesTitle, 
                    MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                Application.Exit();
            }
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
            ArchiveServiceEventLog.Install();
            #endregion
            
            #region Create PerfCounters
            PCInstaller.Install();
            #endregion

            #region Save the fact that we're installed to the registry
            Installed = true;
            #endregion
        }

        /// <summary>
        /// This routine should be called automatically by the MSI during Remove Programs, but it can also be called using:
        ///     "installutil.exe /u Archiver.dll"
        /// </summary>
        /// <param name="savedState">State dictionary passed in by the installer code</param>
        public override void Uninstall (IDictionary savedState)
        {
            #region Check to make sure we're in an Administrator role
            WindowsPrincipal wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            if (wp.IsInRole(WindowsBuiltInRole.Administrator) == false)
            {
                RtlAwareMessageBox.Show(null, Strings.AdministratorPrivilegesText, Strings.AdministratorPrivilegesTitle, 
                    MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                Application.Exit();
            }
            #endregion

            #region Whack the Event Logs
            ArchiveServiceEventLog.Uninstall();
            #endregion

            #region Whack PerfCounters
            PCInstaller.Uninstall();
            #endregion

            #region Whack registry entry saying we're installed
            Installed = false;
            #endregion

            #region Call base.Uninstall
            if (savedState.Count != 0)
                base.Uninstall(savedState);
            #endregion
        }

    }

}
