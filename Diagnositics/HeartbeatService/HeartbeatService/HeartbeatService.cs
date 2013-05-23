using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

namespace MSR.LST.Net.Heartbeat {
    public partial class HeartbeatService : ServiceBase {
        public const string ShortName = "CXPHeartbeat";
        public const string DisplayName = "ConferenceXP Heartbeat Service";
        public const string Description = "Source a multicast heartbeat for ConferenceXP diagnostics.";

        public static EventLog ServiceEventLog = null;
        private HeartbeatServer m_hs;

        public HeartbeatService() {
            InitializeComponent();
            this.ServiceName = ShortName;
            this.AutoLog = true;
            this.CanPauseAndContinue = false;
            this.CanShutdown = true;
            this.CanStop = true;
            ServiceEventLog = this.EventLog;

            m_hs = new HeartbeatServer(Constants.Address, Constants.Port, Constants.Cookie, Constants.Period, Constants.Ttl);
        }

        protected override void OnStart(string[] args) {
            m_hs.Start();
        }

        protected override void OnStop() {
            m_hs.Stop();
        }

    }
}
