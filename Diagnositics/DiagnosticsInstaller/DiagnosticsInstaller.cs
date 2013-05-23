using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using MSR.LST.Net;

namespace DiagnosticsInstaller {
    [RunInstaller(true)]
    public partial class DiagnosticsInstaller : Installer {
        public DiagnosticsInstaller() {
            InitializeComponent();
        }

        public const string EVENT_LOG_SOURCE = "DiagnosticService";
        private int port = edu.washington.cs.cct.cxp.diagnostics.Global.DIAGNOSTIC_PORT;
        private const string FW_EXCEPTION_NAME = "CXP Diagnostic Service Listen Port";

        public override void Install(System.Collections.IDictionary stateSaver) {
            base.Install(stateSaver);
            try {
                //Set up event log
                if (!EventLog.SourceExists(EVENT_LOG_SOURCE)) {
                    EventLog.CreateEventSource(EVENT_LOG_SOURCE, "Application");
                }
            }
            catch { }

            //Create a firewall exception for the listening port
            try {
                this.addPortException();
            }
            catch { }
        }

        public override void Uninstall(System.Collections.IDictionary savedState) {
            base.Uninstall(savedState);

            try {
                //Remove event log source
                if (EventLog.SourceExists(EVENT_LOG_SOURCE)) {
                    EventLog.DeleteEventSource(EVENT_LOG_SOURCE);
                }
            }
            catch { }

            //Remove the firewall exception
            try {
                this.remPortException();
            }
            catch { }
        }

        #region Firewall

        private void addPortException() {
            if (FirewallUtility.HasVistaFirewall) {
                FirewallUtility.AddPortExceptionToVistaFirewall(FW_EXCEPTION_NAME, this.port.ToString(), NetFwTypeLib.NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP);
            }
            else if (FirewallUtility.HasSp2Firewall) {
                FirewallUtility.AddPortExceptionToSP2Firewall(FW_EXCEPTION_NAME, this.port, NetFwTypeLib.NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP);
            }
            else {
                FirewallUtility.AddOldFirewallPort(FW_EXCEPTION_NAME, (ushort)this.port, System.Net.Sockets.ProtocolType.Udp);
            }
        }

        private void remPortException() {
            if (FirewallUtility.HasVistaFirewall) {
                FirewallUtility.RemoveAppFromVistaFirewall(FW_EXCEPTION_NAME);
            }
            else if (FirewallUtility.HasSp2Firewall) {
                FirewallUtility.RemovePortExceptionFromSP2Firewall(this.port, NetFwTypeLib.NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP);
            }
            else {
                FirewallUtility.RemoveOldFirewallPort(FW_EXCEPTION_NAME, (ushort)this.port, System.Net.Sockets.ProtocolType.Udp);
            }
        }
        #endregion Firewall
    }
}