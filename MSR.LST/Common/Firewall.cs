using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Net.Sockets;
using System.Security.Principal;
using System.Windows.Forms;

using NetFwTypeLib;

using NETCONLib;


namespace MSR.LST.Net
{
    /// <remarks>
    /// WARNING: This class does not inform the user that the firewall punchthrough is being added.  Applications
    /// should always inform the user when adding punchthroughs to the firewall, for security reasons.
    /// </remarks>
    public abstract class FirewallUtility
    {
        #region Constants
        // Constants for accessing the SP2 firewall through COM
        const string INetFwMgrGuid = "{304CE942-6E39-40D8-943A-B913C40C9CD4}";
        const string INetFwAuthorizedApplicationGuid = "{EC9846B3-2762-4A6B-A214-6ACB603462D2}";
        const string INetFwOpenPortGuid = "{0CA545C6-37AD-4A6C-BF92-9F7610067EF5}";

        // Constants from the Win32 API for the Internet Connection Firewall
        const byte NAT_PROTOCOL_TCP = 6;
        const byte NAT_PROTOCOL_UDP = 17;
        #endregion

        #region Any-Version Firewall Methods
        /// <summary>
        /// Adds an exception(s) to either a 2003 or XP SP2 firewall.
        /// </summary>
        /// <param name="exceptionName">The name of the exception to be added for both types of firewalls.</param>
        /// <param name="ports">The ports to be added to Win2003 firewalls.</param>
        /// <param name="protocols">An array of ProtocolTypes for each port.</param>
        /// <param name="appPath">The path of the application being added (for new, SP2 firewalls).</param>
        public static void AddFirewallException (string exceptionName, ushort[] ports, ProtocolType[] protocols, string appPath)
        {
            if (HasVistaFirewall) {
                AddAppToVistaFirewall(exceptionName, appPath);                
            }
            else if (HasSp2Firewall)
            {
                if (Sp2FirewallEnabled)
                {
                    AddAppToSP2Firewall(exceptionName, appPath, "*", true);
                }
            }
            else
            {
                if (ports.Length != protocols.Length)
                    throw new ArgumentException(Strings.PortsAndPortTypesSameLength);
                
                for (int cnt = 0; cnt < ports.Length; ++cnt)
                {
                    AddOldFirewallPort(exceptionName, ports[cnt], protocols[cnt]);
                }
            }
        }



        /// <summary>
        /// Removes an exception(s) from firewall.
        /// </summary>
        /// <param name="exceptionName">The name of the exception to be removed for any type of firewall.</param>
        /// <param name="ports">The ports to be removed to Win2003 firewalls.</param>
        /// <param name="protocols">An array of ProtocolTypes for each port.</param>
        /// <param name="appPath">The path of the application being removed (for new, SP2 firewalls).</param>
        public static void RemoveFirewallException (string exceptionName, ushort[] ports, ProtocolType[] protocols, string appPath)
        {
            if (HasVistaFirewall) {
                RemoveAppFromVistaFirewall(exceptionName);
            }
            else if (HasSp2Firewall)
            {
                if (Sp2FirewallEnabled)
                {
                    RemoveAppFromSP2Firewall(appPath);
                }
            }
            else
            {
                if (ports.Length != protocols.Length)
                    throw new ArgumentException(Strings.PortsAndPortTypesSameLength);

                for (int cnt = 0; cnt < ports.Length; ++cnt)
                {
                    RemoveOldFirewallPort(exceptionName, ports[cnt], protocols[cnt]);
                }
            }
        }

 
        #endregion

        #region Vista and later Firewall Methods

        /// <summary>
        /// Add the exception rule for all profiles and all protocols.
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="appPath"></param>
        /// <returns>false if the firewall type isn't available, true for successful completion.</returns>
        public static bool AddAppToVistaFirewall(string ruleName, string appPath) {
            Type fwPolicy2Type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            if (fwPolicy2Type == null) {
                return false;
            }

            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)System.Activator.CreateInstance(fwPolicy2Type);
            INetFwRule fwRule = (INetFwRule)System.Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwRule"));
            fwRule.Name = ruleName;
            fwRule.ApplicationName = appPath;
            fwRule.Enabled = true;
            fwPolicy2.Rules.Add(fwRule);
            return true;
        }

        /// <summary>
        /// Add a ports/protocol exception rule for all profiles for Vista or later firewall
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="appPath"></param>
        /// <returns>false if the firewall type isn't available, true for successful completion.</returns>
        public static bool AddPortExceptionToVistaFirewall(string ruleName, string ports, NET_FW_IP_PROTOCOL_ protocol) {
            Type fwPolicy2Type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            if (fwPolicy2Type == null) {
                return false;
            }

            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)System.Activator.CreateInstance(fwPolicy2Type);
            INetFwRule fwRule = (INetFwRule)System.Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwRule"));
            fwRule.Name = ruleName;
            fwRule.Protocol = (int)protocol;
            fwRule.LocalPorts = ports;
            fwRule.Enabled = true;
            fwPolicy2.Rules.Add(fwRule);
            return true;
        }

        public static void RemoveAppFromVistaFirewall(string ruleName) {
            Type fwPolicy2Type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            if (fwPolicy2Type == null) {
                return;
            }

            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)System.Activator.CreateInstance(fwPolicy2Type);
            fwPolicy2.Rules.Remove(ruleName);            
        }        
        
        public static bool HasVistaFirewall {
            get {
                try {
                    Type fwPolicy2Type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
                    if (fwPolicy2Type == null) {
                        return false;
                    }
                    INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)System.Activator.CreateInstance(fwPolicy2Type);
                }
                catch {
                    return false;
                }
                return true;
            }
        }

        #endregion Vista and later Firewall Methods

        #region XP SP2-specific Firewall Methods
        /// <summary>
        /// Attempts to determine if the computer has a Windows XP SP2 compatible firewall.
        /// </summary>
        public static bool HasSp2Firewall
        {
            get
            {
                try
                {
                    INetFwMgr fwMgr = (INetFwMgr) Activator.CreateInstance(
                        Type.GetTypeFromCLSID(new Guid(INetFwMgrGuid)), true);
                    INetFwAuthorizedApplication fwAA = (INetFwAuthorizedApplication) Activator.CreateInstance(
                        Type.GetTypeFromCLSID(new Guid(INetFwAuthorizedApplicationGuid)), true);
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Returns true if the comptuer's SP2 firewall is enabled.
        /// </summary>
        /// <remarks>
        /// Throws if this computer does not have an SP2 firewall.
        /// </remarks>
        public static bool Sp2FirewallEnabled
        {
            get
            {
                try
                {
                    INetFwMgr fwMgr = (INetFwMgr) Activator.CreateInstance(
                        Type.GetTypeFromCLSID(new Guid(INetFwMgrGuid)), true);
                    return fwMgr.LocalPolicy.CurrentProfile.FirewallEnabled;
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    // This exception occurrs on a machine not running the service for the firewall, but only on
                    //  machines that have the firewall installed.  Therefore, the firewall is installed but not
                    //  enabled, so we just return false.  This happens by default after you install 2003 SP1 (bug?).
                    // Expected exception msg: "There are no more endpoints available from the endpoint mapper"...
                    return false;
                }
            }
        }

        /// <summary>
        /// Adds an application with specified parameters to a XP SP2-compatible firewall exception list. 
        /// </summary>
        /// <param name="name">Title of the rule</param>
        /// <param name="imageName">Full path of the image</param>
        /// <param name="strLocalSubnet">Space seperated network addresses permitted to access the application
        /// (e.g. "LocalSubnet", "*", "192.168.10.0/255.255.255.0")</param>
        /// <param name="enabled">If the exception rule should be enabled</param>
        /// <remarks>
        /// WARNING: This method does not inform the user that the firewall punchthrough is being added.  Applications
        /// should always inform the user when adding punchthroughs to the firewall, for security reasons.
        /// </remarks>
        public static void AddAppToSP2Firewall(String name, String imageName, String strLocalSubnet, bool enabled) 
        {
            // Instantiating the HNetCfg.NetFwMgr object to get "LocalPolicy" and then "CurrentProfile"
            INetFwMgr fwMgr = (INetFwMgr) Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(INetFwMgrGuid)), true);                
            INetFwPolicy fwPolicy = fwMgr.LocalPolicy;
            INetFwProfile fwProfile = fwPolicy.CurrentProfile;

            // Checking got skipped since the entry gets update if exist and inserted if not 
            // (No check necessary); Check if the entry already exists. "System.IO.FileNotFoundException"
            // will be thrown if entry doesn't exist.
            // fwAA = fwProfile.AuthorizedApplications.Item(imageName);

            // Instantiating the HNetCfg.NetFwAuthorizedApplication object
            INetFwAuthorizedApplication fwAA = (INetFwAuthorizedApplication) Activator.CreateInstance(
                Type.GetTypeFromCLSID(new Guid(INetFwAuthorizedApplicationGuid)), true);

            // Assigning values to the AuthorizedApplication to be added to the firewall permission list.
            // Make this entry Enabled/Disabled
            fwAA.Enabled = enabled;

            // The friendly name for this "Exception" rule
            fwAA.Name = name;

            // Whether only the local subnet can access this application or not
            fwAA.RemoteAddresses = strLocalSubnet;

            // The image name full path
            fwAA.ProcessImageFileName = imageName;
            
            // Adding AuthorizedApplication to the Exception List
            fwProfile.AuthorizedApplications.Add(fwAA);
        }

        /// <summary>
        /// Removes an application from an XP SP2-compatible firewall exception list.
        /// </summary>
        /// <param name="imageName">Full name of image to be removed from FW exception list</param>
        /// <remarks>
        /// WARNING: This method does not inform the user that the firewall punchthrough is being added.  Applications
        /// should always inform the user when adding punchthroughs to the firewall, for security reasons.
        /// </remarks>
        public static void RemoveAppFromSP2Firewall(String imageName) 
        {
            // Instantiating the HNetCfg.NetFwMgr object to get "LocalPolicy" and then "CurrentProfile"
            INetFwMgr fwMgr = (INetFwMgr) Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(INetFwMgrGuid)), true);                
            INetFwPolicy fwPolicy = fwMgr.LocalPolicy;
            INetFwProfile fwProfile = fwPolicy.CurrentProfile;

            // Remove application from exception rule list
            fwProfile.AuthorizedApplications.Remove(imageName);
        }

        public static void AddPortExceptionToSP2Firewall(string name, int port, NET_FW_IP_PROTOCOL_ protocol) {
            // Instantiating the HNetCfg.NetFwMgr object to get "LocalPolicy" and then "CurrentProfile"
            INetFwMgr fwMgr = (INetFwMgr)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(INetFwMgrGuid)), true);
            INetFwPolicy fwPolicy = fwMgr.LocalPolicy;
            INetFwProfile fwProfile = fwPolicy.CurrentProfile;

            INetFwOpenPort fwOpenPort = (INetFwOpenPort)Activator.CreateInstance(
                Type.GetTypeFromCLSID(new Guid(INetFwOpenPortGuid)), true);

            fwOpenPort.Name = name;
            fwOpenPort.Port = port;
            fwOpenPort.Protocol = protocol;
            fwOpenPort.Enabled = true;
            fwProfile.GloballyOpenPorts.Add(fwOpenPort);
        }

        public static void RemovePortExceptionFromSP2Firewall(int port, NET_FW_IP_PROTOCOL_ protocol) { 
            // Instantiating the HNetCfg.NetFwMgr object to get "LocalPolicy" and then "CurrentProfile"
            INetFwMgr fwMgr = (INetFwMgr) Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(INetFwMgrGuid)), true);                
            INetFwPolicy fwPolicy = fwMgr.LocalPolicy;
            INetFwProfile fwProfile = fwPolicy.CurrentProfile;

            // Remove application from exception rule list
            fwProfile.GloballyOpenPorts.Remove(port, protocol);
        }

        #endregion

        #region Pre-SP2 or 2003 Firewall Methods
        /// <summary>
        /// Returns true if this computer has a Pre-SP2 firewall enabled on any connection.
        /// </summary>
        /// <remarks>
        /// This value may or may not be true, even if the computer has an SP2 firewall.  If this
        /// computer has an SP2-compatible firewall, this value should be ignored.
        /// </remarks>
        public static bool OldFirewallEnabled
        {
            get
            {
                INetSharingManager mgr = new NetSharingManagerClass();
      
                // Iterate through all of the available connections
                foreach(INetConnection iCon in mgr.EnumEveryConnection)
                {
                    INetSharingConfiguration iShareConfig = mgr.get_INetSharingConfigurationForINetConnection(iCon);

                    if( iShareConfig.InternetFirewallEnabled ) // skip this connection if the firewall is disabled
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Checks to find whether the Windows XP/2003 Firewall is enabled on adapters and if so it opens ports.
        /// </summary>
        /// <param name="portMapName">Name of the Port Map to look for</param>
        /// <param name="port">Port  Number</param>
        /// <param name="protocolIsTcp">true if TCP, false if UDP</param>
        /// <remarks>
        /// WARNING: This method does not inform the user that the firewall punchthrough is being added.  Applications
        /// should always inform the user when adding punchthroughs to the firewall, for security reasons.
        /// </remarks>
        public static void AddOldFirewallPort(string portMapName, ushort port, ProtocolType protocol)
        {
            ValidateForOldCompatibleFirewall();
            ValidateAdministrator();

            // Get the protocolAsByte ICF constant
            byte protocolAsByte = ConvertAndValidateProtocol(protocol);

            INetSharingManager mgr = new NetSharingManagerClass();

            // Iterate through all of the available connections
            foreach(INetConnection iCon in mgr.EnumEveryConnection)
            {
                INetSharingConfiguration iShareConfig = mgr.get_INetSharingConfigurationForINetConnection(iCon);

                if( iShareConfig.InternetFirewallEnabled ) // skip this connection if the firewall is disabled
                {
                    // Make sure that this firewall doesn't already have a port map for the same port
                    bool portMapExists = false;
                    foreach(INetSharingPortMapping portMap in iShareConfig.get_EnumPortMappings(tagSHARINGCONNECTION_ENUM_FLAGS.ICSSC_ENABLED))
                    {
                        if ((ushort)(portMap.Properties.ExternalPort) == port && portMap.Properties.IPProtocol == protocolAsByte)
                        {
                            portMapExists = true;
                            break;
                        }
                    }

                    if (!portMapExists)
                    {
                        // Finally, add & enable the new port map
                        INetSharingPortMapping newPortMap = iShareConfig.AddPortMapping(portMapName, protocolAsByte, port, port, 0, SystemInformation.ComputerName, tagICS_TARGETTYPE.ICSTT_NAME);
                        newPortMap.Enable();
                    }
                }
            }
        }

        /// <summary>
        /// Checks to find whether the Windows XP/2003 Firewall is enabled on adapters and if so it opens ports.
        /// </summary>
        /// <param name="portMapName">Name of the Port Map to look for (must be the same as when it was added)</param>
        /// <param name="port">Port  Number</param>
        /// <param name="protocolIsTcp">true if TCP, false if UDP</param>
        /// <remarks>
        /// WARNING: This method does not inform the user that the firewall punchthrough is being added.  Applications
        /// should always inform the user when adding punchthroughs to the firewall, for security reasons.
        /// </remarks>
        public static void RemoveOldFirewallPort(string portMapName, ushort port, ProtocolType protocol)
        {
            ValidateForOldCompatibleFirewall();
            ValidateAdministrator();

            byte protocolAsByte = ConvertAndValidateProtocol(protocol);

            INetSharingManager mgr = new NetSharingManagerClass();
      
            // Iterate through all of the available connections
            foreach(INetConnection iCon in mgr.EnumEveryConnection)
            {
                INetSharingConfiguration iShareConfig = mgr.get_INetSharingConfigurationForINetConnection(iCon);

                if( iShareConfig.InternetFirewallEnabled ) // skip this connection if the firewall is disabled
                {
                    foreach(INetSharingPortMapping portMap in iShareConfig.get_EnumPortMappings(tagSHARINGCONNECTION_ENUM_FLAGS.ICSSC_ENABLED))
                    {
                        // Remove this port mapping only if the name & port match
                        if ((ushort)(portMap.Properties.ExternalPort) == port && portMap.Properties.IPProtocol == protocolAsByte)
                        {
                            if (String.Compare(portMap.Properties.Name, portMapName) == 0)
                            {
                                iShareConfig.RemovePortMapping(portMap);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Private Helper Methods
        private static void ValidateForOldCompatibleFirewall()
        {
            // Check the OS version and return if not Windows XP or greater
            OperatingSystem os = System.Environment.OSVersion;
            if (os.Version.Major < 5) // earlier than Win2K
                throw new NotSupportedException(Strings.OSMustBeWindowsXPSP2OrGreater);
            if (os.Version.Major == 5 && os.Version.Minor < 1) // Win2K but not XP
                throw new NotSupportedException(Strings.OSMustBeWindowsXPSP2OrGreater);

            if( HasSp2Firewall ) // Skip computers with firewall == SP2's firewall (i.e. NOT XP pre-SP2 & Win2K3)
                throw new NotSupportedException(Strings.DoNotCallAddPortToOldFirewall);
        }

        private static void ValidateAdministrator()
        {
            // Check to make sure we're in an Administrator role, as Limited accounts don't have access to read or write the Firewall settings
            WindowsPrincipal wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            if (wp.IsInRole(WindowsBuiltInRole.Administrator) == false)
                throw new InvalidOperationException(Strings.YouMustBeAnAdministratorToModify);
        }

        private static byte ConvertAndValidateProtocol(ProtocolType protocol)
        {
            switch(protocol)
            {
                case ProtocolType.Tcp:
                    return FirewallUtility.NAT_PROTOCOL_TCP;
                case ProtocolType.Udp:
                    return FirewallUtility.NAT_PROTOCOL_UDP;
                default:
                    throw new NotSupportedException(Strings.ProtocolIsNotSupported);
            }
        }
        #endregion

    }
}
