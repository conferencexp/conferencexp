using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;


namespace MSR.LST.ConferenceXP.ReflectorService
{
    public class ReflectorService : System.ServiceProcess.ServiceBase
    {
        private System.ComponentModel.Container components = null;
        private ReflectorMgr refMgr = null;
        TcpServerChannel channel = null;
        
        public ReflectorService()
        {
            InitializeComponent();
            refMgr = ReflectorMgr.getInstance();
        }

        static void Main()
        {
            // Override the system UICulture
            string cultureOverride = null;
            if ((cultureOverride = ConfigurationManager.AppSettings["MSR.LST.Reflector.UICulture"]) != null) {
                try {
                    System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(cultureOverride);
                    Thread.CurrentThread.CurrentUICulture = ci;
                }
                catch { }
            }

            if (System.Threading.Thread.CurrentThread.Name == null ||
                System.Threading.Thread.CurrentThread.Name.Length == 0)
            {
                System.Threading.Thread.CurrentThread.Name = "Reflector Service Main";
            }

            System.ServiceProcess.ServiceBase[] ServicesToRun;

            ServicesToRun = new System.ServiceProcess.ServiceBase[] { new ReflectorService() };

            System.ServiceProcess.ServiceBase.Run(ServicesToRun);
        }

        private void InitializeComponent()
        {
        }

        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (components != null) 
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }
        
        protected override void OnStart(string[] args)
        {
            refMgr.StartReflector();

            // Register the server channel.
            IDictionary channelProperties = new Hashtable();
            channelProperties["port"] = ConfigurationManager.AppSettings.Get(AppConfig.AdminPort);
            channelProperties["rejectRemoteRequests"] = true;

            channel = new TcpServerChannel(channelProperties, null);
            ChannelServices.RegisterChannel(channel, false);

            // Expose an object for remote calls
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(AdminRemoting), 
                "ReflectorAdminEndpoint", WellKnownObjectMode.Singleton);
        }
 
        protected override void OnStop()
        {
            refMgr.StopReflector();

            // Close the server channel.
            if (channel != null)
            {
                channel.StopListening(null);
                ChannelServices.UnregisterChannel(channel);
            }
        }

        private void FormProc()
        {
            Application.EnableVisualStyles();
            Application.DoEvents();
            Application.Run();
        }
    }
}
