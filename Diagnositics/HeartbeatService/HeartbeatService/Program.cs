using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace MSR.LST.Net.Heartbeat {
    static class Program {
        static void Main() {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new HeartbeatService() };
            ServiceBase.Run(ServicesToRun);
        }
    }
}