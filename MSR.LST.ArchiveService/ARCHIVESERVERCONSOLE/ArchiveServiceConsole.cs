using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.IO;
using System.Reflection;


namespace MSR.LST.ConferenceXP.ArchiveService
{
    /// <summary>
    /// A console app to host the Archive Service during testing.
    /// </summary>
    class ArchiveServiceConsole
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Language override
            if (Constants.UICulture != null) {
                try {
                    CultureInfo ci = new CultureInfo(Constants.UICulture);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                }
                catch { }
            }

            Console.WriteLine(Strings.CXPArchiveServiceStarting);

            #region Hook us as a listner to Trace / Debug
            TraceListener tl = new TextWriterTraceListener(Console.Out);
            Trace.Listeners.Add( tl );
            #endregion

            #region Trace to a file
            string filename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "LOG_" + DateTime.Now.Ticks.ToString() + ".txt");
            TraceListener tlf = new TextWriterTraceListener(filename);
            Trace.Listeners.Add(tlf);
            Trace.AutoFlush = true;
            #endregion

            BaseService arc = new BaseService();
            arc.Start();

            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.CXPArchiveServiceListeningOnPort, 
                Constants.TCPListeningPort));
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.PressKeyToQuit, "q"));
            Console.WriteLine("-----------------------------------------------------------------\n");

            while (Console.ReadLine() != "q");

            arc.Stop();
        }

    }

}
