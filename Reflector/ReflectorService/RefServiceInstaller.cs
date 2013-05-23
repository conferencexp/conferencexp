using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Net.Sockets;
using System.Reflection;
using System.ServiceProcess;
using System.Security.Principal;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP.ReflectorService
{
    [RunInstaller(true)]
    public class RefServiceInstaller : System.Configuration.Install.Installer
    {
        #region Members

        private System.ServiceProcess.ServiceProcessInstaller serviceRefProcessInstaller;
        private System.ServiceProcess.ServiceInstaller serviceRefInstaller;
        private System.ComponentModel.Container components = null;

        // Ports to be opened/removed. True:TCP, False:UDP
        private static readonly ushort[] ports = {2334, 5004, 5005, 7004, 7005};
        private static readonly ProtocolType[] portTypes = {ProtocolType.Tcp, 
            ProtocolType.Udp, ProtocolType.Udp, ProtocolType.Udp, ProtocolType.Udp};

        // Port map name to be used
        private static readonly string portMapName = "ConferenceXP Reflector Service";

        #endregion 

        public RefServiceInstaller()
        {
            InitializeComponent();
        }

        protected override void OnCommitted(IDictionary savedState)
        {
            base.OnCommitted (savedState);

            // Setting the "Allow Interact with Desktop" option for this service.
            ConnectionOptions connOpt = new ConnectionOptions();
            connOpt.Impersonation = ImpersonationLevel.Impersonate;
            ManagementScope mgmtScope = new ManagementScope(@"root\CIMv2", connOpt);
            mgmtScope.Connect();
            ManagementObject wmiService = new ManagementObject("Win32_Service.Name='" + ReflectorMgr.ReflectorServiceName + "'");
            ManagementBaseObject inParam = wmiService.GetMethodParameters("Change");
            inParam["DesktopInteract"] = true;
            ManagementBaseObject outParam = wmiService.InvokeMethod("Change", inParam, null);

            #region Start the reflector service immediately
            try
            {
                ServiceController sc = new ServiceController("ConferenceXP Reflector Service");
                sc.Start();
            }
            catch (Exception ex)
            {
                // Don't except - that would cause a rollback.  Instead, just tell the user.
                RtlAwareMessageBox.Show(null, string.Format(CultureInfo.CurrentCulture, Strings.ServiceStartFailureText, 
                    ex.ToString()), Strings.ServiceStartFailureTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
            #endregion
        }

        public override void Install(IDictionary stateSaver)
        {
            CheckAdministratorRole();

            OpenFirewallPorts();

            base.Install(stateSaver);

            // Install the event log
            ReflectorEventLog.Install();

            // Install performance counters
            PCInstaller.Install();
        }


        public override void Uninstall(IDictionary savedState)
        {
            CheckAdministratorRole();

            // Stop the service first if it is still running
            ServiceController sc = new ServiceController(ReflectorMgr.ReflectorServiceName);
        
            if (sc.Status == ServiceControllerStatus.Running)
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
            }

            RemoveFirewallPorts();
            
            base.Uninstall(savedState);

            // Uninstall the event log
            ReflectorEventLog.Uninstall();

            // Uninstall performance counters
            PCInstaller.Uninstall();
        }


        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }


        #region Component Designer generated code
        private void InitializeComponent()
        {
            this.serviceRefProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceRefInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceRefProcessInstaller
            // 
            this.serviceRefProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceRefInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            
            // 
            // serviceRefInstaller
            // 
            this.serviceRefInstaller.DisplayName = ReflectorMgr.ReflectorServiceName;
            this.serviceRefInstaller.ServiceName = ReflectorMgr.ReflectorServiceName;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] 
                { 
                    this.serviceRefProcessInstaller,
                    this.serviceRefInstaller
                }
                );

        }
        #endregion

       
        private void OpenFirewallPorts()
        {
            try
            {
                MSR.LST.Net.FirewallUtility.AddFirewallException (portMapName,
                    ports, portTypes, Context.Parameters["assemblypath"]);
            }
            catch (NotSupportedException ex)
            {
                RtlAwareMessageBox.Show(null, string.Format(CultureInfo.CurrentCulture, 
                    Strings.FirewallExceptionFailedAddingApplication, ex.Message), 
                    Strings.FirewallExceptionFailedTitle, MessageBoxButtons.OK, MessageBoxIcon.Error, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
        }


        private void RemoveFirewallPorts()
        {
            try
            {
                MSR.LST.Net.FirewallUtility.RemoveFirewallException (portMapName,
                    ports, portTypes, Context.Parameters["assemblypath"]);
            }
            catch (NotSupportedException ex) 
            {
                RtlAwareMessageBox.Show(null, string.Format(CultureInfo.CurrentCulture, 
                    Strings.FirewallExceptionFailedRemovingApplication, ex.Message), 
                    Strings.FirewallExceptionFailedTitle, MessageBoxButtons.OK, MessageBoxIcon.Error, 
                    MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
            }
        }
            
        private void CheckAdministratorRole()
        {
            WindowsPrincipal wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            if (wp.IsInRole(WindowsBuiltInRole.Administrator) == false)
            {
                RtlAwareMessageBox.Show(null, Strings.AdministratorRoleIsRequired, Strings.ConferencexpReflector, 
                    MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                Application.Exit();
            }
        }
    }
}
