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


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// Class that handles installation for ConferenceAPI and its dependencies.
    /// </summary>
    [RunInstaller(true)]
    public class Installation : Installer
    {
        public const string baseCXPRegKeyName = "SOFTWARE\\Microsoft Research\\ConferenceXP";

        #region Installed?
        public static bool Installed
        {
            get
            {
                bool installed = false;
                try
                {
                    using (RegistryKey pcaKey = Registry.LocalMachine.OpenSubKey(baseCXPRegKeyName))
                    {
                        if (pcaKey != null)
                        {
                            object o = pcaKey.GetValue("ConferenceAPIInstalled");
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
                        using (RegistryKey pcaKey = Registry.LocalMachine.CreateSubKey(baseCXPRegKeyName))
                        {
                            pcaKey.SetValue("ConferenceAPIInstalled", true);
                        }
                    }
                    else
                    {
                        using (RegistryKey pcaKey = Registry.LocalMachine.OpenSubKey(baseCXPRegKeyName, true))
                        {
                            pcaKey.DeleteValue("ConferenceAPIInstalled");
                        }
                    }
                }
                catch {}
            }
        }
        #endregion

        public Installation() : base() 
        {
            #region Install on MSR.LST.Net.Rtp (a necessary dependency)
            AssemblyInstaller ai = new AssemblyInstaller();
            ai.UseNewContext = true;
            ai.Assembly = Assembly.Load("MSR.LST.Net.Rtp");
            Installers.Add(ai);
            #endregion
            
            #region Install MDShowManager (if it is in the same directory)
            FileInfo fi = new FileInfo(Assembly.GetExecutingAssembly().Location);
            FileInfo[] foundFiles = fi.Directory.GetFiles("MDShowManager.dll");
            if (foundFiles.Length == 1)
            {
                ai = new AssemblyInstaller();
                ai.UseNewContext = true;
                ai.Path = foundFiles[0].FullName;
                Installers.Add(ai);
            }
            #endregion

            #region Install Pipecleaner Agent Service (if it is in the same directory)
            fi = new FileInfo(Assembly.GetExecutingAssembly().Location);
            foundFiles = fi.Directory.GetFiles("Pipecleaner Agent Service.exe");
            if (foundFiles.Length == 1)
            {
                ai = new AssemblyInstaller();
                ai.UseNewContext = true;
                ai.Path = foundFiles[0].FullName;
                Installers.Add(ai);
            }
            #endregion
        }

        public override void Install (IDictionary savedState)
        {
            #region Check to make sure we're in an Administrator role
            WindowsPrincipal wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            if (wp.IsInRole(WindowsBuiltInRole.Administrator) == false)
            {
                RtlAwareMessageBox.Show(null, Strings.YouMustBeAnAdministratorToInstall, 
                    Strings.AdministratorPrivilegesRequired, MessageBoxButtons.OK, MessageBoxIcon.Stop, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                Application.Exit();
            }
            #endregion
            #region Basic Installer stuff
            base.Install(savedState);
            #endregion
            #region Install the Event Logs
            ConferenceEventLog.Install();
            #endregion
            #region Save the fact that we're installed to the registry
            Installed = true;
            #endregion
        }

        public override void Uninstall (IDictionary savedState)
        {
            #region Check to make sure we're in an Administrator role
            WindowsPrincipal wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            if (wp.IsInRole(WindowsBuiltInRole.Administrator) == false)
            {
                RtlAwareMessageBox.Show(null, Strings.YouMustBeAnAdministratorToUninstall, 
                    Strings.AdministratorPrivilegesRequired, MessageBoxButtons.OK, MessageBoxIcon.Stop, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                Application.Exit();
            }
            #endregion
            #region Basic Installer stuff
            base.Uninstall(savedState);
            #endregion
            #region Whack the Event Logs
            ConferenceEventLog.Uninstall();
            #endregion
            #region Whack registry entries
            try
            {
                using (RegistryKey pcaKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft Research\\ConferenceXP", true))
                {
                    if (pcaKey != null)
                        pcaKey.DeleteSubKeyTree("Client");
                }
            }
            catch {}

            try
            {
                using (RegistryKey pcaKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft Research\\ConferenceXP", true))
                {
                    if (pcaKey != null)
                        pcaKey.DeleteSubKeyTree("Client");
                }
            }
            catch {}

            Installed = false;
            #endregion
        }
    }
}
