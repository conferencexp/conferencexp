using System;
using System.Configuration;
using System.Globalization;

using Microsoft.Win32;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// Identity returns a string that is a unique identifier for the user logged on to this computer.  The algorithm it uses is:
    /// 
    ///  * Check for overrides.  Overrides could set the identifier from an app.config file or change the default to Windows authentication and ignore Messenger/Passport
    ///  
    ///  * If Windows Messenger is running, return LocalLogonName
    ///  * If Windows Messenger isn't running, check the registry for the last LogonName that was logged on to Messenger
    ///  * If Messenger has never logged in, get the Identity from Windows using the Username logged on and a combination of the computer name and domain name.
    /// </summary>
    public class Identity
    {

        /// <summary>
        /// Identifier override used for debuggin purposes
        /// </summary>
        static private string identifier = null;

        /// <summary>
        /// Get the Identifier of the current user in the form of an email address like "Bob@BobsWorld.Com"
        /// 
        /// Note: Identifier.set should only be used by debugging applications
        /// </summary>
        public static string Identifier
        {
            get
            {
                // First try the programmatic override
                if (identifier != null)
                    return identifier;

                string setting;

                // Second try the Application Settings Identifier override if set
                if ((setting = ConfigurationManager.AppSettings[AppConfig.ID_IdentityOverride]) != null)
                {
                    return setting;
                }

                // Third, get identifier from Windows.
                return GetIdentifierFromWindows();

                // Note: we used to get identifier from Messenger, but this technique was dropped since it causes problems
                // in Win 7 under some circumstances.
            }
            set
            {
                identifier = value;
            }
        }

        /// <summary>
        /// Get the Identifier from Windows Messenger which will return the Passport email address.
        /// If that fails, default to GetIdentifierFromWindows()
        /// </summary>
        /// <returns>string Identifier</returns>
        private static string GetIdentifierFromMessenger()
        {

            // Check Messenger for logon name
            try
            {
                // Create the messenger object
                MessengerAPI.MessengerClass msgr = new MessengerAPI.MessengerClass();

                // Assign the logon name
                return msgr.MySigninName;

            }
            catch (Exception)
            {

                RegistryKey pcaKey = null;
                try
                {
                    pcaKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\MessengerService\\ListCache\\.NET Messenger Service");
                }
                catch (Exception) {}

                using (pcaKey)
                {
                    if (pcaKey != null)
                    {
                        object o = pcaKey.GetValue("IdentityName");
                        if (o != null)
                        {
                            return Convert.ToString(o, CultureInfo.InvariantCulture);
                        }
                    }
                }

                return GetIdentifierFromWindows();
            }
        }

        /// <summary>
        /// Get the Identifier from Windows, using a combination of logic from the username, computer name, and domain name
        /// </summary>
        /// <returns>string Identifier</returns>
        private static string GetIdentifierFromWindows()
        {
            // Get the username of the Windows account logged on to this computer, I.E. "Bob"
            string userName = System.Environment.UserName;

            // Get the computer name (no DNS extension, I.E. "MyLaptop")
            string hostname = System.Net.Dns.GetHostName();

            // Get the fully qualified domain name (fqdn) for this computer, I.E. "MyLaptop.MyDomain.Com"
            // Note:  Generally the fqdn isn't set for most computers unless the computer is in a domain or the user set it manually in a very hiddent dialog box.
            // This may come back as "MyLaptop" in that case.
            string fqdn = System.Net.Dns.GetHostEntry(hostname).HostName;

            // Create a faux domain name in case the fqdn wasn't set.  For instance "MyLaptop.Dot.Net"
            string domainName = System.Environment.UserDomainName + ".ConferenceXP.Net";

            // If the fqdn did come back as more than the computer name, use it but trim the computer name.
            // I.E. fqdn == "MyLaptop.MyDomain.Com", trimmed to just "MyDomain.Com"
            if (fqdn.Length > hostname.Length)
                domainName = fqdn.Substring(hostname.Length + 1);

            // Possible outcomes:
            //   Bob is logged on to MyLaptop with fqdn set to "BobsWorld.Com"; "Bob@BobsWorld.Com"
            //   Bob is logged on to MyLaptop with no fqdn set; "Bob@MyLaptop.ConferenceXP.Net"
            return userName + "@" + domainName;
        }
    }
}
