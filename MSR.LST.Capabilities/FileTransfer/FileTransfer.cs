using System;
using System.Globalization;
using System.IO;
using System.Xml;

using MSR.LST.ConferenceXP;


namespace MSR.LST.ConferenceXP
{
    [Capability.Name("File Transfer")]
    [Capability.PayloadType(PayloadType.FileTransfer)]
    [Capability.FormType(typeof(FileTransferFMain))]
    [Capability.Channel(false)]
    [Capability.MaxBandwidth(100000)]
    [Capability.Fec(true)]
    [Capability.FecRatio(0, 300)]
    public class FileTransferCapability : CapabilityWithWindow, ICapabilitySender, ICapabilityViewer
    {
        public FileTransferCapability() : base()
        {
            Uri assemblyUri = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            FileInfo assemblyInfo = new FileInfo(assemblyUri.LocalPath);
            string configFile = assemblyInfo.DirectoryName + Path.DirectorySeparatorChar + "FileTransfer.xml";

            XmlDocument xmlDocSettings = new XmlDocument();
            try
            {
                xmlDocSettings.Load(configFile);
                if (xmlDocSettings.DocumentElement.Name != "FileTransferCapability")
                    throw new Exception(Strings.ConfigurationFileCorrupt);

                XmlElement xmlConfiguration = xmlDocSettings.DocumentElement["Configuration"];
                if (xmlConfiguration == null)
                {
                    File.Delete(configFile);
                }
                else
                {
                    this.MaximumBandwidthLimiter = int.Parse(xmlConfiguration["MaxBandwidth"].GetAttribute("value"),
                        CultureInfo.InvariantCulture);
                    capProps.Fec = bool.Parse(xmlConfiguration["Fec"].GetAttribute("enabled"));
                    capProps.FecChecksum = ushort.Parse(xmlConfiguration["Fec"].GetAttribute("value"),
                        CultureInfo.InvariantCulture);
                }
            }
            catch {}

            name = Strings.FileTransfer;
        }

        public FileTransferCapability(DynamicProperties dp) : base(dp)
        {
            formType = typeof(FileTransferClient);
        }
    }
}
