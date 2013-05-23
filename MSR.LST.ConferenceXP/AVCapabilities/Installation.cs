using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Security.Principal;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace MSR.LST.ConferenceXP {
    [RunInstaller(true)]
    public partial class Installation : Installer {

        public Installation() : base() { }

        public override void Install (IDictionary savedState) {

            #region Make sure we are running on Vista, Server 2008 or later
            System.OperatingSystem os = System.Environment.OSVersion;
            if (os.Version.Major < 6) {
                //MessageBox.Show("This will only install on Vista or later");
                return;
            }
            #endregion

            #region Check to make sure we're in an Administrator role
            WindowsPrincipal wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            if (wp.IsInRole(WindowsBuiltInRole.Administrator) == false) {
                //MessageBox.Show("You Must Be An Administrator To Install");
                return;
            }
            #endregion

            #region Register msadds32.ax
            //First check if it is in the system directory.  If so, we assume it is already installed.
            string axPath = Path.Combine(System.Environment.SystemDirectory, "msadds32.ax");
            if (!File.Exists(axPath)) {
                try {
                    RegisterWMACodecFilter();
                }
                catch {
                    //MessageBox.Show("Failed to register msadds32.ax: " + e.ToString());
                }
            }
            #endregion

            #region Check to see if msaud32.acm is in the system32 directory and if not, copy it there.
            try {
                string acmPath = Path.Combine(System.Environment.SystemDirectory, "msaud32.acm");
                if (!File.Exists(acmPath)) {
                    Assembly assem = Assembly.GetExecutingAssembly();
                    string acmSrcPath = Path.Combine(Path.GetDirectoryName(assem.Location), "msaud32.acm");
                    if (File.Exists(acmSrcPath)) {
                        File.Copy(acmSrcPath, acmPath);
                    }
                    else {
                        //MessageBox.Show("Can't find msaud32.acm in the source directory");
                    }
                }
            }
            catch {
                //MessageBox.Show("Exception while copying msaud32.acm " + e.ToString());
            }
            #endregion

            #region Add msaud32.acm entry to the registry
            //HKLM, "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Drivers32", "msacm.msaudio1",, "msaud32.acm" 
            string driverKeyName = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Drivers32";
            try {
                RegistryKey driverKey = Registry.LocalMachine.OpenSubKey(driverKeyName, true);
                string value = (string)driverKey.GetValue("msacm.msaudio1");
                if (value == null) {
                    driverKey.SetValue("msacm.msaudio1", "msaud32.acm");
                }
            }
            catch {
                //MessageBox.Show("Exception while adding registry key: " + e.ToString());
            }
            #endregion
        }

        public override void Uninstall(System.Collections.IDictionary savedState) {}

        [DllImport(@"msadds32.ax", EntryPoint = "DllRegisterServer")]
        private static extern void RegisterWMACodecFilter();

    }
}