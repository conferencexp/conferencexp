using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Collections;
using System.Security.Principal;
using System.Diagnostics;

namespace MSR.LST.Net.Heartbeat {
    [RunInstaller(true)]
    public partial class HeartbeatServiceInstaller : Installer {
        public HeartbeatServiceInstaller() {
            InitializeComponent();

            // - Define and create the service installer
            ServiceInstaller heartbeatInstaller = new ServiceInstaller();
            heartbeatInstaller.StartType = ServiceStartMode.Automatic;
            heartbeatInstaller.ServiceName = HeartbeatService.ShortName;
            heartbeatInstaller.DisplayName = HeartbeatService.DisplayName;
            heartbeatInstaller.Description = HeartbeatService.Description;
            heartbeatInstaller.Committed += new InstallEventHandler(installer_Committed);
            Installers.Add(heartbeatInstaller);

            // - Define and create the process installer
            ServiceProcessInstaller processInstaller = new ServiceProcessInstaller();
            processInstaller.Account = ServiceAccount.LocalService;
            Installers.Add(processInstaller);

        }

        #region Install/Uninstall
        public override void Install(IDictionary savedState) {
            base.Install(savedState);
        }

        protected override void OnCommitted(IDictionary savedState) {
            base.OnCommitted(savedState);
            try {
                ServiceController sc = new ServiceController(HeartbeatService.ShortName);
                sc.Start();
            }
            catch (Exception ex) {
                // Don't except - that would cause a rollback.  Instead, just tell the user.
                Debug.WriteLine("The Heartbeat service failed to start.");
            }
        }


        /// <summary>
        /// Starts the Service after the installation has committed.
        /// </summary>
        private void installer_Committed(object sender, InstallEventArgs e) {
            // Start the service at the end of installation
            try {
                ServiceController sc = new ServiceController(HeartbeatService.ShortName);
                sc.Start();
            }
            catch (Exception ex) {
                // Don't except - that would cause a rollback.  Instead, just tell the user.
                Debug.WriteLine("The Heartbeat service failed to start.");
            }
        }

        public override void Uninstall(IDictionary savedState) {
            // Make sure the service is stopped.
            try {
                ServiceController sc = new ServiceController(HeartbeatService.ShortName);
                if (sc.Status != ServiceControllerStatus.Stopped)
                    sc.Stop();
            }
            catch { }

            base.Uninstall(savedState);
        }
        #endregion

    }
}