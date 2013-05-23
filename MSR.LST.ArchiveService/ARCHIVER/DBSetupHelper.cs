using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;
using System.ServiceProcess;
using System.IO;
using System.Globalization;
using System.Diagnostics;

namespace MSR.LST.ConferenceXP.ArchiveService {
    /// <summary>
    /// Constants and static methods to help set up the ArchiveService database
    /// </summary>
    public static class DBSetupHelper {
        #region Constants

        private const string SqlRegistryKeyLocation = @"SOFTWARE\Microsoft\Microsoft SQL Server";
        private const string SqlInstancesValueName = "InstalledInstances";
        private const string DefaultInstance = "MSSQLSERVER";
        private const string DBScriptSubFolder = "Templates";
        private const string DBScriptTemplatePrefix = "template_";
        private const string ArchiveAdminConfigFileName = "ArchiveAdmin.exe.config";
        private const string ArchiveServiceConfigFileName = "ArchiveWindowsService.exe.config";
        private const string ConnectionStringTemplate = "<add key=\"MSR.LST.ConferenceXP.ArchiveService.SQLConnectionString\" value=\"data source=.\\{0};initial catalog=ArchiveService;integrated security=SSPI\"/>";
        private const string NamedInstanceConfigMarker = "<!--NAMEDINSTANCE-->";
        private const string ArchiverRegistryKeyLocation = @"SOFTWARE\Microsoft Research\ConferenceXP";
        private const string ArchiverSelectedInstanceValueName = "ArchiverSQLServerInstance";
        private const string AddDBScriptFileName = "AddDatabase.sql";
        private const string AddSPScriptFileName = "AddSPs.sql";
        private const string RemoveDBScriptFileName = "DropDatabase.sql";

        #endregion Constants

        #region Public Static Methods

        public static bool SelectDatabaseInstance(out string sqlInstanceName, out string sqlServiceName) {
            //Look up SQL Server instances from the registry
            string[] sqlInstances = DBSetupHelper.GetDBInstances();
            sqlServiceName = null;
            sqlInstanceName = null;

            if (sqlInstances.Length == 0) {
                Trace.WriteLine("No DB instance found.");
                return false;
            }

            // Do sanity check on the instances.  Accept the first one for which a service exists.
            foreach (string name in sqlInstances) {
                ServiceController sqlController;
                ServiceControllerStatus sqlStatus;
                try {
                    Console.WriteLine("Checking instance name: " + name);
                    sqlController = new ServiceController(name);
                    sqlStatus = sqlController.Status; // should throw if the service does't exist
                    sqlInstanceName = name;
                    sqlServiceName = name;
                }
                catch {
                    try {
                        Console.WriteLine("Checking instance name: " + "$MSSQL$" + name);
                        sqlController = new ServiceController("MSSQL$" + name);
                        sqlStatus = sqlController.Status; // should throw if the service does't exist
                        sqlInstanceName = name;
                        sqlServiceName = "MSSQL$" + name;
                    }
                    catch {
                        continue;
                    }
                }

                Trace.WriteLine("Selected service: " + sqlServiceName + "; instance=" + sqlInstanceName);
                return true;
            }
            Trace.WriteLine("No valid DB instance found.");
            return false;
        }

        /// <summary>
        /// Install the ArchiveService on the selected instance.  We assume that the user has already confirmed
        /// to overwrite possible existing database.
        /// Throw exceptions in a variety of error conditions.
        /// </summary>
        /// <param name="installPath"></param>
        /// <param name="sqlInstanceName"></param>
        /// <param name="sqlServiceName"></param>
        public static void InstallDatabase(string installPath, string sqlInstanceName, string sqlServiceName) {

            // ensure the selected instance's SQL process is running
            StartSqlServerService(sqlServiceName);

            // run the database initialization scripts
            InitializeDatabase(installPath, sqlInstanceName);

            // Write to the registry to indicate which instance we installed on.
            SetSelectedInstanceRegistryValue(sqlInstanceName);

        }

        /// <summary>
        /// Generates config files according to 
        /// the selected instance
        /// </summary>
        /// <param name="installDir"></param>
        /// <param name="installInstance"></param>
        public static void SetConfigFiles(string installDir, string instance) {
            bool defaultInstance = instance.Equals(DefaultInstance);

            // generate file paths
            string templateFilePathPrefix = String.Format("{0}{1}{2}{1}{3}", installDir, Path.DirectorySeparatorChar,
                DBScriptSubFolder, DBScriptTemplatePrefix);
            string archiveAdminConfigTemplate = templateFilePathPrefix + ArchiveAdminConfigFileName;
            string archiveServiceConfigTemplate = templateFilePathPrefix + ArchiveServiceConfigFileName;
            string archiveAdminConfigFilePath = installDir + ArchiveAdminConfigFileName;
            string archiveServiceConfigFilePath = installDir + ArchiveServiceConfigFileName;

            // copy a fresh version of each config file into the main installDirectory
            File.Copy(archiveAdminConfigTemplate, archiveAdminConfigFilePath, true);
            File.Copy(archiveServiceConfigTemplate, archiveServiceConfigFilePath, true);

            // update the configs
            UpdateConfigFilesWithNamedInstance(archiveAdminConfigFilePath, instance, defaultInstance);
            UpdateConfigFilesWithNamedInstance(archiveServiceConfigFilePath, instance, defaultInstance);
        }

        public static void RemoveDatabase(string installPath) {
            string instance = GetInstalledInstance();
            if (instance == null) {
                return;
            }

            string removeDBFilePath = installPath + RemoveDBScriptFileName;
            try {
                RunOsqlFile(removeDBFilePath, instance);
                //Clear the registry value that indicates which instance we used.
                ClearSelectedInstanceRegistryValue();
            }
            catch { }
        }

 
        #endregion Public Static Methods

        #region Private Static Methods

        private static string GetInstalledInstance() {
            try {
                RegistryKey archiverRegistryRoot = Registry.LocalMachine.OpenSubKey(ArchiverRegistryKeyLocation, true);
                if (archiverRegistryRoot == null) {
                    return null;
                }
                return archiverRegistryRoot.GetValue(ArchiverSelectedInstanceValueName, RegistryValueKind.String) as string;
            }
            catch {
                return null;
            }
        }


        private static void InitializeDatabase(string installDir, string instance) {
            string addDBFilePath = installDir + AddDBScriptFileName;
            string addSPFilePath = installDir + AddSPScriptFileName;

            RunOsqlFile(addDBFilePath, instance);
            RunOsqlFile(addSPFilePath, instance);
        }


        /// <summary>
        /// Run the specified file with osql under
        /// the current users's credentials.
        /// </summary>
        /// <param name="file">The full path to a .sql file to run.</param>
        private static void RunOsqlFile(string file, string instance) {
            bool defaultInstance = instance.Equals(DefaultInstance);

            string osqlLocation = FindOsqlExe();

            if (osqlLocation == null)
                throw new InvalidOperationException(Strings.OsqlNotFound);

            // Call osql on our file
            string osqlArgs = " -E -i \"" + file + "\"";
            if (!defaultInstance) {
                osqlArgs = " -S .\\" + instance + " -E -i \"" + file + "\"";
            }
            Trace.WriteLine("osql command line: " + osqlLocation + osqlArgs);
            Process osql = Process.Start(osqlLocation, osqlArgs);

            // Wait for osql to finish (can be quite some time, depending on the DB init size
            osql.WaitForExit();

            // This is wishful thinking.  Read the note in AddSPs.sql to see why. (pfb)
            if (osql.ExitCode != 0)
                throw new InvalidOperationException(Strings.OsqlFailed);

        }

        /// <summary>
        /// Attempt to locate the latest version of osql.
        /// Instead should we find the one that matches the version of the database instance?
        /// </summary>
        /// <returns></returns>
        private static string FindOsqlExe() {
            int maxVersion = 20;
            int minVersion = 8;
            for (int version = maxVersion; version >= minVersion; version--) {
                string key = "SOFTWARE\\Microsoft\\Microsoft SQL Server\\" + version.ToString() + "0\\Tools\\ClientSetup";
                string wowKey = "SOFTWARE\\Wow6432Node\\Microsoft\\Microsoft SQL Server\\" + version.ToString() + "0\\Tools\\ClientSetup";
                RegistryKey rKey = Registry.LocalMachine.OpenSubKey(key);
                if (rKey == null) {
                    rKey = Registry.LocalMachine.OpenSubKey(wowKey);
                }
                if (rKey != null) {
                    string osqlLocation = (string)Registry.LocalMachine.OpenSubKey(key).GetValue("SQLPath") + "\\BINN\\osql.exe";
                    if ((osqlLocation != null) && (File.Exists(osqlLocation))) {
                        return osqlLocation;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Sets in the Archiver registry key that tells the selected database instance
        /// </summary>
        /// <param name="instance">Selected Instance</param>
        /// <returns>True if the value was set, false otherwise</returns>
        private static bool SetSelectedInstanceRegistryValue(string selectedInstance) {
            try {
                RegistryKey archiverRegistryRoot = Registry.LocalMachine.OpenSubKey(ArchiverRegistryKeyLocation, true);
                if (archiverRegistryRoot == null) {
                    return false;
                }
                archiverRegistryRoot.SetValue(ArchiverSelectedInstanceValueName, selectedInstance, RegistryValueKind.String);
            }
            catch {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Clear in the Archiver registry key that indicates the selected database instance
        /// </summary>
        /// <param name="instance">Selected Instance</param>
        /// <returns>True if the value was set, false otherwise</returns>
        private static bool ClearSelectedInstanceRegistryValue() {
            try {
                RegistryKey archiverRegistryRoot = Registry.LocalMachine.OpenSubKey(ArchiverRegistryKeyLocation, true);
                if (archiverRegistryRoot == null) {
                    return false;
                }
                archiverRegistryRoot.DeleteValue(ArchiverSelectedInstanceValueName, false);
            }
            catch {
                return false;
            }
            return true;
        }

        private static void UpdateConfigFilesWithNamedInstance(string configFile, string instanceName, bool defaultInstance) {
            FileStream file = File.Open(configFile, FileMode.Open, FileAccess.Read);
            StreamReader fileReader = new StreamReader(file);
            string fullFileText = fileReader.ReadToEnd();
            file.Close();

            if (defaultInstance) {
                // No change other than to remove the marker from the template.
                fullFileText = fullFileText.Replace(NamedInstanceConfigMarker, "");
            }
            else {
                // Replace the marker with an appropriate connection string.
                fullFileText = fullFileText.Replace(NamedInstanceConfigMarker, String.Format(ConnectionStringTemplate, instanceName));
            }

            StreamWriter fileWriter = new StreamWriter(configFile, false);
            fileWriter.Write(fullFileText);
            fileWriter.Close();
        }


        private static void StartSqlServerService(string serviceName) {
            ServiceController sqlController;
            ServiceControllerStatus sqlStatus;
            sqlController = new ServiceController(serviceName);
            sqlStatus = sqlController.Status; // should throw if the service does't exist

            if (sqlStatus != ServiceControllerStatus.Running) // start the service if it isn't already
            {
                sqlController.Start();
                sqlController.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 15)); // wait 15 seconds, in hope
            }
        }


        /// <summary>
        /// Returns a list of all installed instances. If SQL Server is not installed or no instances are found, 
        /// returns an empty list.
        /// </summary>
        /// <returns></returns>
        private static string[] GetDBInstances() {
            string[] sqlInstances = new string[0];

            // get the full name of the instance for registry lookup
            RegistryKey sqlRoot = Registry.LocalMachine.OpenSubKey(SqlRegistryKeyLocation);
            if (sqlRoot == null) {
                return sqlInstances;
            }

            //PRI1: be sure to test this with multiple instances installed
            string[] discoveredSqlInstances = sqlRoot.GetValue(SqlInstancesValueName) as string[];

            if (discoveredSqlInstances != null) {
                sqlInstances = discoveredSqlInstances;
            }

            return sqlInstances;
        }

        #endregion Private Static Methods

    }
}
