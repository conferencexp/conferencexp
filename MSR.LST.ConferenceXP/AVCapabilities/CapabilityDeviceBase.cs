using System;
using System.Configuration;
using System.Globalization;
using System.Runtime.InteropServices;

using Microsoft.Win32;

using MSR.LST.MDShow;
using MSR.LST.Net.Rtp;


namespace MSR.LST.ConferenceXP
{
    public class CapabilityDeviceWithWindow : CapabilityDevice, ICapabilityWindow
    {
        #region Constructors
        public CapabilityDeviceWithWindow(DynamicProperties dynaProps) : base(dynaProps) {}
        public CapabilityDeviceWithWindow(FilterInfo fi) : base(fi) {}
        #endregion

        #region Implementation of ICapabilityWindow
        public int Height
        {
            get
            {
                ValidateForm();
                return form.Height;
            }
            set
            {
                ValidateForm();
                form.Height = value;
            }
        }

        public int Left
        {
            get
            {
                ValidateForm();
                return form.Left;
            }
            set
            {
                ValidateForm();
                form.Left = value;
            }
        }

        public string Caption
        {
            get
            {
                ValidateForm();
                return form.Text;
            }
            set
            {
                ValidateForm();
                form.Text = value;
            }
        }

        public bool Visible
        {
            get
            {
                ValidateForm();
                return form.Visible;
            }
            set
            {
                ValidateForm();
                form.Visible = value;
            }
        }

        public int Width
        {
            get
            {
                ValidateForm();
                return form.Width;
            }
            set
            {
                ValidateForm();
                form.Width = value;
            }
        }

        public int Top
        {
            get
            {
                ValidateForm();
                return form.Top;
            }
            set
            {
                ValidateForm();
                form.Top = value;
            }
        }

        public System.Drawing.Rectangle Rectangle
        {
            get
            {
                ValidateForm();
                return new System.Drawing.Rectangle(Left, Top, Width, Height);
            }
            set
            {
                ValidateForm();
                Left = value.Left;
                Top = value.Top;
                Width = value.Width;
                Height = value.Height;
            }
        }

        public virtual System.Drawing.Point Location
        {
            set
            {
                ValidateForm();
                form.Location = value;
            }
        }

        public virtual System.Drawing.Size Size
        {
            set
            {
                ValidateForm();

                ((FAudioVideo)form).TrimBlackFromVideo(value);
            }
        }
        #endregion
    }

    public class CapabilityDevice : Capability, ICapabilitySender
    {
        #region Members

        #region Sending side / Capture graph

        protected CaptureGraph cg = null;
        protected FilterInfo fi;
        protected AVLogger log = null;

        #endregion Sending side / Capture graph

        #region Receiving side / Playing graph

        protected FilgraphManagerClass fgm = null;
        protected object fgmLock = new object();
        protected FilterGraph.State fgmState = FilterGraph.State.Stopped;
        protected uint rotID = 0;

        protected RtpStream rtpStream;

        internal int uIBorderWidth;
        internal int uIBorderHeight;
        protected int uiState = 0;

        #endregion Receiving side / Playing graph

        #endregion Members

        public CapabilityDevice(DynamicProperties dynaProps) : base(dynaProps) {}
        public CapabilityDevice(FilterInfo fi) : base() 
        {
            this.fi = fi;
            name = Conference.LocalParticipant.Name + " - " + fi.DisplayName;
        }
        
        /// <summary>
        /// Used for logging troubleshooting information in our UI.
        /// Set this value before calling ActivateCamera() / ActivateMicrophone()
        /// </summary>
        public void SetLogger(AVLogger log)
        {
            this.log = log;
        }


        /// <summary>
        /// Reads / Writes to the registry, whether compression is enabled or not
        /// </summary>
        public bool RegCompressorEnabled {
            get {
                bool enabled = true;

                object setting = AVReg.ReadValue(DeviceKey(), AVReg.CompressorEnabled);
                if (setting != null) {
                    enabled = bool.Parse((string)setting);
                }

                return enabled;
            }

            set {
                AVReg.WriteValue(DeviceKey(), AVReg.CompressorEnabled, value);
            }
        }

        /// <summary>
        /// Reads / Writes to the registry, whether compression is enabled or not
        /// </summary>
        public bool RegAudioCompressorEnabled {
            get {
                bool enabled = true;

                object setting = AVReg.ReadValue(DeviceKey(), AVReg.AudioCompressorEnabled);
                if (setting != null) {
                    enabled = bool.Parse((string)setting);
                }

                return enabled;
            }

            set {
                AVReg.WriteValue(DeviceKey(), AVReg.AudioCompressorEnabled, value);
            }
        }

        /// <summary>
        /// Reads / Writes to the registry, whether compression is enabled or not
        /// </summary>
        public bool RegVideoCompressorEnabled {
            get {
                bool enabled = true;

                object setting = AVReg.ReadValue(DeviceKey(), AVReg.VideoCompressorEnabled);
                if (setting != null) {
                    enabled = bool.Parse((string)setting);
                }

                return enabled;
            }

            set {
                AVReg.WriteValue(DeviceKey(), AVReg.VideoCompressorEnabled, value);
            }
        }
        
        /// <summary>
        /// Provide device's registry path
        /// </summary>
        protected string DeviceKey()
        {
            return AVReg.RootKey + cg.Source.FriendlyName;
        }

        /// <summary>
        /// Save the device's current settings to the registry
        /// </summary>
        protected void SaveDeviceSettings()
        {
            if(cg.Source.HasPhysConns)
            {
                AVReg.WriteValue(DeviceKey(), AVReg.PhysicalConnectorIndex, cg.Source.PhysicalConnectorIndex);
            }
        }

        /// <summary>
        /// Save the stream's current settings to the registry
        /// </summary>
        protected void SaveStreamSettings()
        {
            _AMMediaType mt = cg.Source.GetMediaType();

            // Copy the pbFormat block into a byte array
            if(mt.pbFormat != IntPtr.Zero && mt.cbFormat > 0)
            {
                byte[] pbFormat = new byte[mt.cbFormat];
                Marshal.Copy(mt.pbFormat, pbFormat, 0, (int)mt.cbFormat);
                
                Marshal.FreeCoTaskMem(mt.pbFormat);
                mt.pbFormat = IntPtr.Zero;
                // Don't adjust cbFormat, will use on restore

                AVReg.WriteValue(DeviceKey(), AVReg.FormatBlock, pbFormat);
            }

            AVReg.ms.Position = 0;
            AVReg.bf.Serialize(AVReg.ms, mt);
            AVReg.WriteValue(DeviceKey(), AVReg.MediaType, AVReg.ms.ToArray());
        }

        
        protected void LogCurrentMediaType(Filter f)
        {
            if(f != null)
            {
                _AMMediaType mt;
                object fb;
                if (f is MSR.LST.MDShow.OpusAudioCompressor) { 
                    //TODO: log something useful
                }
                else {
                    try {
                        f.GetMediaType(out mt, out fb);
                        Log(string.Format(CultureInfo.CurrentCulture, "\r\nCurrent media type for {0}...",
                            f.FriendlyName) + MediaType.Dump(mt) + MediaType.FormatType.Dump(fb));

                    }
                    catch { 
                        //TODO: log something useful
                    }
                }
            }
        }

        protected void Log(string msg)
        {
            if(log != null)
            {
                log(msg);
            }
        }
    }
}