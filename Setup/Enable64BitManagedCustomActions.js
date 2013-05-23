// Enable64BitManagedCustomActions.js <msi-file>
// Performs a post-build fixup of an msi to enable it use the 64-bit version of InstallUtil
//   instead of the default 32-bit version. Using the 64-bit version enables registry
//   keys written via custom actions to be properly written to the 64-bit hive.

// Constant values from Windows Installer
var msiOpenDatabaseModeTransact = 1;

var msiViewModifyInsert         = 1
var msiViewModifyUpdate         = 2
var msiViewModifyAssign         = 3
var msiViewModifyReplace        = 4
var msiViewModifyDelete         = 6

if (WScript.Arguments.Length != 1)
{
	WScript.StdErr.WriteLine(WScript.ScriptName + " file");
	WScript.Quit(1);
}

var filespec = WScript.Arguments(0);
var installer = WScript.CreateObject("WindowsInstaller.Installer");
var database = installer.OpenDatabase(filespec, msiOpenDatabaseModeTransact);

var scriptDirectory = WScript.ScriptFullName.substr(0, WScript.ScriptFullName.length - WScript.ScriptName.length);

var sql
var view
var record

WScript.Echo("Updating the CustomAction table...");

sql =  "UPDATE `CustomAction` SET `Source`='InstallUtil64' WHERE `Source`='InstallUtil'";
view = database.OpenView(sql);
view.Execute();
view.Close();

WScript.Echo("Updating the Binary table...");

// Add the InstallUtil64 custom action to the binary table
sql = "SELECT * FROM `Binary`";
view = database.OpenView(sql);
view.Execute();
record = installer.CreateRecord(2);
record.StringData(1) = "InstallUtil64";
record.SetStream(2, scriptDirectory + "InstallUtilLib64.dll");
view.Modify(msiViewModifyInsert, record);
view.Close();

database.Commit();