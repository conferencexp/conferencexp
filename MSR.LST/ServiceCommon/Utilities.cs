using System;
using System.Diagnostics;
using System.ServiceProcess;

using Microsoft.Win32;


namespace MSR.LST.Services
{
    /// <summary>
    /// Provides a very simple wrapper around ServiceController and the registry key associated with it,
    /// allowing the setting of various properties and performing operations from one place.
    /// </summary>
    public class SimpleServiceController
    {
        #region Private || Static Member Vars
        private const int autoStartType = 2;        // Value for the service StartType Automatic in registry
        private const int manualStartType = 3;      // Value for the service StartType Manual in registry

        public const int waitTimeoutSeconds = 5;   // The time to wait for the service to change states

        private ServiceController service;
        private RegistryKey serviceRegKey;
        #endregion

        #region Ctor
        public SimpleServiceController(string name)
        {
            service = new ServiceController(name);

            serviceRegKey = Registry.LocalMachine.OpenSubKey(
                "SYSTEM\\CurrentControlSet\\Services\\" + name, true);
        }
        #endregion

        #region Public Properties
        public ServiceController ServiceController
        {
            get{ return this.service; }
        }

        /// <summary>
        /// Indicates if the service is running.
        /// </summary>
        /// <remarks>
        /// If the service is not running, it is not necessarily stopped.  It may also be starting, stopping, or paused.
        /// </remarks>
        public bool Running
        {
            get
            {
                service.Refresh();
                return (service.Status == ServiceControllerStatus.Running); 
            }
        }

        /// <summary>
        /// Indicates if the service is stopped.
        /// </summary>
        /// <remarks>
        /// If the service is not stopped, it is not necessarily running.  It may also be starting, stopping, or paused.
        /// </remarks>
        public bool Stopped
        {
            get
            {
                service.Refresh();
                return (service.Status == ServiceControllerStatus.Stopped);
            }
        }

        public bool AutoStartService
        {
            get
            {
                return ( (int)serviceRegKey.GetValue("Start") == autoStartType );
            }
            set
            {
                if (value)
                    serviceRegKey.SetValue("Start", autoStartType);     // StartType set to Automatic
                else
                    serviceRegKey.SetValue("Start", manualStartType);   // StartType set to Manual
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Starts the service process and waits the given constant time for the service to start.
        /// </summary>
        public void StartServiceAndWait()
        {
            service.Start();
            service.WaitForStatus (ServiceControllerStatus.Running, new TimeSpan(0,0,waitTimeoutSeconds));
        }

        /// <summary>
        /// Stops the service process and waits the given constant time for the service to stop.
        /// </summary>
        public void StopServiceAndWait()
        {
            service.Stop();
            service.WaitForStatus (ServiceControllerStatus.Stopped, new TimeSpan(0,0,waitTimeoutSeconds));
        }
        #endregion
    }
}