using System;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;


namespace MSR.LST.ConferenceXP.ArchiveService
{
    /// <summary>
    /// Contains all of the basic methods to start an Archive Service.
    /// </summary>
    public class BaseService
    {
        #region EventLog
        /// <summary>
        /// A singleton event log wrapper.
        /// </summary>
        private static ArchiveServiceEventLog eventLog = null;

        private static void InitEventLog()
        {
            eventLog = new ArchiveServiceEventLog( ArchiveServiceEventLog.Source.Service);
        }

        static BaseService()
        {
            InitEventLog();
        }
        #endregion

        TcpServerChannel channel = null;

        public void Start()
        {
            try
            {
                // Register TCP Channel
                channel = new TcpServerChannel( "Archiver", Constants.TCPListeningPort );
                System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(channel, false);

                // Register the SAO
                RemotingConfiguration.RegisterWellKnownServiceType( typeof(ArchiveServer),
                    "ArchiveServer", WellKnownObjectMode.Singleton );

                string configFileName = System.Reflection.Assembly.GetEntryAssembly().Location + ".config";
                if (System.IO.File.Exists(configFileName))
                {
                    RemotingConfiguration.Configure(configFileName, false);
                }

                eventLog.WriteEntry(Strings.ArchiveServiceStarted, EventLogEntryType.Information, 
                    ArchiveServiceEventLog.ID.Information);
            }
            catch
            {
                if ( channel != null )
                {
                    channel.StopListening(null);
                    ChannelServices.UnregisterChannel(channel);
                }
            }
        }

        public void Stop()
        {
            if ( channel != null )
            {
                channel.StopListening(null);
                ChannelServices.UnregisterChannel(channel);
            }

            eventLog.WriteEntry(Strings.ArchiveServiceStopped, EventLogEntryType.Information, 
                ArchiveServiceEventLog.ID.Information);
        }

    }

}
