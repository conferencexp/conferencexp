using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Microsoft.Win32;


namespace MSR.LST.MDShow
{
    [RunInstaller(true)]
    public class Installation : Installer
    {
        public override void Commit(IDictionary savedState)
        {
            MDShowEventLog.Install();

            string oldDirectory = Directory.GetCurrentDirectory();
            FileInfo fi = new FileInfo(Assembly.GetExecutingAssembly().Location);
            Directory.SetCurrentDirectory(fi.Directory.FullName);

            try
            {
                RegisterCxpRtpFilters();
            }
            catch (DllNotFoundException)
            {
                RtlAwareMessageBox.Show(null, Strings.MissingCxpRtpFiltersError, Strings.FileNotFound, 
                    MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }

            try
            {
                RegisterScreenScraperFilter();
            }
            catch (DllNotFoundException)
            {
                RtlAwareMessageBox.Show(null, Strings.MissingScreenScraperFilterError, Strings.FileNotFound, 
                    MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }

            Directory.SetCurrentDirectory(oldDirectory);            
        }

        public override void Uninstall(IDictionary savedState)
        {
            string oldDirectory = Directory.GetCurrentDirectory();
            FileInfo fi = new FileInfo(Assembly.GetExecutingAssembly().Location);
            Directory.SetCurrentDirectory(fi.Directory.FullName);

            try
            {
                UnregisterCxpRtpFilters();
            }
            catch (DllNotFoundException)
            {
                RtlAwareMessageBox.Show(null, Strings.MissingCxpRtpFiltersError, Strings.FileNotFound, 
                    MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }

            try
            {
                UnregisterScreenScraperFilter();
            }
            catch (DllNotFoundException)
            {
                RtlAwareMessageBox.Show(null, Strings.MissingScreenScraperFilterError, Strings.FileNotFound, 
                    MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }

            Directory.SetCurrentDirectory(oldDirectory);

            MDShowEventLog.Uninstall();

            if (savedState.Count != 0)
                base.Uninstall(savedState);
        }

        [DllImport("CxpRtpFilters.ax", EntryPoint="DllRegisterServer")]
        private static extern void RegisterCxpRtpFilters();

        [DllImport("CxpRtpFilters.ax", EntryPoint="DllUnregisterServer")]
        private static extern void UnregisterCxpRtpFilters();

        [DllImport("ScreenScraperFilter.ax", EntryPoint="DllRegisterServer")]
        private static extern void RegisterScreenScraperFilter();

        [DllImport("ScreenScraperFilter.ax", EntryPoint="DllUnregisterServer")]
        private static extern void UnregisterScreenScraperFilter();
    }
}
