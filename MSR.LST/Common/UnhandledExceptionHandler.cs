using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

// Investigate using Watson reporting


namespace MSR.LST
{
    public class UnhandledExceptionHandler
    {
        public static void Register()
        {
            // Register our unhandled exception handler with the app domain
            AppDomain.CurrentDomain.UnhandledException += 
                new UnhandledExceptionEventHandler(Handler);
        }

        private static void Handler(object sender, UnhandledExceptionEventArgs args)
        {
            // Wrap it all in a try catch, so as not to generate any extra errors at this point
            try
            {
                // The information to display or log
                // Since "info" will be logged as well as displayed to the user in a message box, localize all 
                //     strings connected to building "info" CRN
                string info;

                // Special case ThreadAbortExceptions during finalization
                ThreadAbortException tae = args.ExceptionObject as ThreadAbortException;
                if(tae != null)
                {
                    if(!args.IsTerminating)
                    {
                        return;
                    }
                }

                // Retrieve the exception object from the EventArgs
                Exception e = args.ExceptionObject as Exception;

                // Determine type of exception - CLS / Non-CLS
                if(e != null)
                {
                    // CLS-compliant - can access any Exception field (StackTrace, InnerException, etc.)
                    info = e.ToString();
                }
                else
                {
                    // Non-CLS compliant - can access methods defined by Object (ToString, GetType, etc.)
                    info = string.Format(CultureInfo.CurrentCulture, Strings.NonCLSCompliantException,
                        args.ExceptionObject.GetType(), args.ExceptionObject.ToString());
                }

                // List type and string of Sender
                info += "\n\n" + string.Format(CultureInfo.CurrentCulture, Strings.SenderType, sender.GetType());
                info += "\n" + string.Format(CultureInfo.CurrentCulture, Strings.SenderString, sender.ToString());

                // Determine what type of thread caused the error
                info += "\n" + Strings.ThreadType + " ";
                if(!args.IsTerminating)
                {
                    info += Strings.PoolOrFinalizer;
                }
                else
                {
                    info += Strings.MainOrManual;
                }

                // Log version of product
                info += "\n" + string.Format(CultureInfo.CurrentCulture, Strings.ProductVersion, 
                    Application.ProductVersion.ToString());

                // Log it to the event log
                try
                {
                    EventLog eventLog = new EventLog("UnEx", ".", "MSR.LST.UnEx");
                    eventLog.WriteEntry(info, EventLogEntryType.Error, 999);
                }
                catch(Exception){}

                // Compose message to user
                string msg = string.Format(CultureInfo.CurrentCulture, Strings.ErrorMessageToUser,
                    AppDomain.CurrentDomain.FriendlyName, info);

                // Read user's preference from configuration file
                bool showMsg = true;
                if (ConfigurationManager.AppSettings[AppConfig.LST_ShowErrors] != null)
                {
                    showMsg = bool.Parse(ConfigurationManager.AppSettings[AppConfig.LST_ShowErrors]);
                }

                if(showMsg)
                {
                    RtlAwareMessageBox.Show(null, msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, 
                        MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                }
            }
            catch(Exception){}
        }
    }
}
