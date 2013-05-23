using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Reflection;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP
{
    [RunInstaller(true)]
    public class Installer : System.Configuration.Install.Installer
    {
        public Installer() : base()
        {
            AssemblyInstaller ai = new AssemblyInstaller();
            ai.Assembly = Assembly.Load("Conference");
            Installers.Add(ai);

            AssemblyInstaller ai2 = new AssemblyInstaller();
            ai2.Assembly = Assembly.Load("LSTCommon");
            Installers.Add(ai2);

            AssemblyInstaller ai3 = new AssemblyInstaller();
            ai3.Assembly = Assembly.Load("AVCapabilities");
            Installers.Add(ai3);
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install (stateSaver);

            if (MSR.LST.Net.FirewallUtility.HasVistaFirewall) {
                MSR.LST.Net.FirewallUtility.AddAppToVistaFirewall("ConferenceXP Client",
                    Context.Parameters["assemblypath"]);
            }
            else if (MSR.LST.Net.FirewallUtility.HasSp2Firewall)
            {
                if (MSR.LST.Net.FirewallUtility.Sp2FirewallEnabled)
                {
                    MSR.LST.Net.FirewallUtility.AddAppToSP2Firewall ("ConferenceXP Client",
                        Context.Parameters["assemblypath"], "*", true);
                }
            }
            else if (MSR.LST.Net.FirewallUtility.OldFirewallEnabled)
            {
                RtlAwareMessageBox.Show(null, Strings.FirewallNotFoundText, Strings.FirewallNotFound, 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, 
                    (MessageBoxOptions)0);
            }
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall (savedState);

            if (MSR.LST.Net.FirewallUtility.HasVistaFirewall) {
                try {
                    MSR.LST.Net.FirewallUtility.RemoveAppFromVistaFirewall("ConferenceXP Client");
                }
                catch { }
            }
            else if (MSR.LST.Net.FirewallUtility.HasSp2Firewall)
            {
                if (MSR.LST.Net.FirewallUtility.Sp2FirewallEnabled) {
                    try {
                        MSR.LST.Net.FirewallUtility.RemoveAppFromSP2Firewall(Context.Parameters["assemblypath"]);
                    }
                    catch { }
                }
            }
        }
    }
}
