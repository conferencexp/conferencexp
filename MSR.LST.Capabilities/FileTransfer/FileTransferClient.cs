using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using MSR.LST;
using MSR.LST.ConferenceXP;


namespace MSR.LST.ConferenceXP
{
    public class FileTransferClient : CapabilityForm
    {
        public FileTransferClient() {}

        // This prevents the form from showing up
        protected override void SetVisibleCore(bool value) {}

        private FileTransferCapability fileTransferCapability = null;

        #region ICapabilityForm

        public override void AddCapability(ICapability capability)
        {
            base.AddCapability (capability);

            if (fileTransferCapability == null)
            {
                fileTransferCapability = (FileTransferCapability)capability;
                fileTransferCapability.ObjectReceived += 
                    new CapabilityObjectReceivedEventHandler(ObjectReceived);
            }
        }

        public override bool RemoveCapability(ICapability capability)
        {
            bool ret = base.RemoveCapability (capability);

            if (ret)
            {
                fileTransferCapability.ObjectReceived -= 
                    new CapabilityObjectReceivedEventHandler(ObjectReceived);
                fileTransferCapability = null;
            }

            return ret;
        }

        #endregion

        private Type GetCapability(byte[] fileData)
        {
            Type[] types = null;

            try
            {
                types = Assembly.Load(fileData).GetTypes();
            }
            catch { return null; }

            foreach (Type tBaseType in types)
            {
                Type tICapability = tBaseType.GetInterface("MSR.LST.ConferenceXP.ICapability");
                if (tICapability != null)
                {
                    try
                    {
                        Capability.PayloadTypeAttribute attrPT = (Capability.PayloadTypeAttribute)
                            Attribute.GetCustomAttribute(tBaseType, typeof(Capability.PayloadTypeAttribute));

                        if (attrPT == null)
                            continue;

                        return tBaseType;
                    }
                    catch (Exception e)
                    {
                        RtlAwareMessageBox.Show(this, e.StackTrace, e.Message, MessageBoxButtons.OK, 
                            MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                    }
                }
            }

            return null;
        }

        private void ObjectReceived(object capability, ObjectReceivedEventArgs ea)
        {
            if (ea.Data is FileObject)
            {
                FileObject fileObject = ea.Data as FileObject;
                FileInfo fileInfo = new FileInfo(fileObject.Name);

                Type tCapability = null;
                ItemReceivedDialog dlgFileReceived = null;

                if (tCapability != null)
                {
                    Capability.NameAttribute attrName = (Capability.NameAttribute)
                        Attribute.GetCustomAttribute(tCapability, typeof(Capability.NameAttribute));

                    Capability.PayloadTypeAttribute attrPT = (Capability.PayloadTypeAttribute)
                        Attribute.GetCustomAttribute(tCapability, typeof(Capability.PayloadTypeAttribute));

                    dlgFileReceived = new ItemReceivedDialog(Strings.CapabilityReceived, 
                        Strings.UserSentYouCapability);
                    dlgFileReceived.Button1Label = Strings.InstallHotkey;
                    dlgFileReceived.FileIconTip = string.Format(CultureInfo.CurrentCulture, 
                        Strings.CapabilityNamePayloadType, attrName.Name, attrPT.PayloadType.ToString());
                }
                else
                {
                    dlgFileReceived = new ItemReceivedDialog(Strings.FileReceived, Strings.UserSentYouFile);
                }

                dlgFileReceived.Image = Image.FromStream(
                    System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MSR.LST.ConferenceXP.Images.FileReceived.gif"));
                dlgFileReceived.From = ea.Participant.Name;
                dlgFileReceived.Avatar = ea.Participant.Icon;
                dlgFileReceived.AvatarTip = Strings.AvatarTipText; 
                dlgFileReceived.File = fileInfo.Name;
                dlgFileReceived.FileIcon = fileObject.Icon;
                dlgFileReceived.FileIconTip += string.Format(CultureInfo.CurrentCulture, Strings.FileSize, 
                    ((double)fileObject.Data.Length / 1024.0).ToString("#,#.00", CultureInfo.CurrentCulture));
                dlgFileReceived.Time = DateTime.Now.ToString("h:mm:ss tt", CultureInfo.CurrentCulture);
                if (dlgFileReceived.ShowDialog() != DialogResult.OK)
                    return;

                if (tCapability != null)
                {
                    RtlAwareMessageBox.Show(this, Strings.NotYetImplemented, string.Empty, MessageBoxButtons.OK, 
                        MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
                }
                else
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.FileName = fileInfo.Name;
                    if (Directory.Exists(fileInfo.DirectoryName))
                        saveDialog.InitialDirectory = fileInfo.DirectoryName;
                    saveDialog.Filter = "All files (*.*)|*.*";
                    saveDialog.CheckPathExists = true;
                    saveDialog.OverwritePrompt = true;
                    saveDialog.RestoreDirectory = true;
                    if (saveDialog.ShowDialog() != DialogResult.OK)
                        return;

                    FileStream fileStream = File.OpenWrite(saveDialog.FileName);
                    fileStream.Write(fileObject.Data, 0, fileObject.Data.Length);
                    fileStream.Close();
                }
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing (e);

            if (fileTransferCapability != null && fileTransferCapability.IsPlaying)
                fileTransferCapability.StopPlaying();
        }

        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FileTransferClient
            // 
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Name = "FileTransferClient";
            this.ResumeLayout(false);

        }


    }
}
