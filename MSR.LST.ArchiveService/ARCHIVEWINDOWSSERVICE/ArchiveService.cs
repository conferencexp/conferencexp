using System;
using System.Diagnostics;
using System.ServiceProcess;


namespace MSR.LST.ConferenceXP.ArchiveService
{
    public class ArchiveWindowsService : System.ServiceProcess.ServiceBase
    {
        #region Public Constants
        // Important constants with the service's name & description.
        // WARNING: The "shortName" here must be duplicated in the Admin app.config file so it can start/stop the service!
        public const string ShortName = "CXPArchiver";
        public const string DisplayName = "ConferenceXP Archive Service";

        #endregion

        #region Private Variables
        /// <summary>
        /// A base wrapper for an ArchiveService
        /// </summary>
        private BaseService arcServ;
        #endregion

        static void Main()
        {
            //Language override
            if (Constants.UICulture != null) {
                try {
                    System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(Constants.UICulture);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                }
                catch { }
            }

            // Tell the ServiceProcess base to run the ArchvieWindowsService
            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[] { new ArchiveWindowsService() };
            ServiceBase.Run(servicesToRun);
        }

        public ArchiveWindowsService()
        {
            this.ServiceName = ShortName;

            this.AutoLog = false;
            this.CanPauseAndContinue = false;
            this.CanShutdown = true;
            this.CanStop = true;
        }

        /// <summary>
        /// Set things in motion so your service can do its work.
        /// </summary>
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            arcServ = new BaseService();
            arcServ.Start();
        }

        /// <summary>
        /// Stop this service.
        /// </summary>
        protected override void OnStop()
        {
            arcServ.Stop();
            base.OnStop();
        }
    }
}
