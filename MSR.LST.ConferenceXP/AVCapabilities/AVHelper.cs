using System;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

using Microsoft.Win32;


namespace MSR.LST.ConferenceXP
{
    public delegate void AVLogger(string msg);

    /// <summary>
    /// Per-user (HKCU) registry settings for audio and video
    /// </summary>
    public sealed class AVReg
    {
        static AVReg()
        {
            ms = new MemoryStream();

            bf = new BinaryFormatter();
            bf.AssemblyFormat = FormatterAssemblyStyle.Simple;
        }

        public static readonly BinaryFormatter bf;
        public static readonly MemoryStream ms;

        // There is a bug in the .NET Framework 1.1 that causes RegistryKey.OpenSubKey to throw if the key name
        //  passed in is >= 255 chars.  Instead, it's supposed to check if an individual subkey name (not the full
        //  path being passed in) is >= 255 chars.
        // To work around this bug, we open the clientKey, and then open subkeys off of it.  This basically eliminates
        //  the chance of hitting the error, unless either the device moniker is excessively long, or the install path
        //  is > 200 chars to so.
        static RegistryKey clientKey = Registry.CurrentUser.CreateSubKey(
            @"Software\Microsoft Research\ConferenceXP\Client\" + 
            System.Reflection.Assembly.GetEntryAssembly().CodeBase);

        public static readonly string RootKey = "AV\\";

        public static readonly string SelectedDevices = RootKey + "SelectedDevices\\";
        public static readonly string SelectedCameras = SelectedDevices + "Cameras\\";
        public static readonly string SelectedMicrophone = SelectedDevices + "Microphone\\";
        public static readonly string SelectedSpeaker = SelectedDevices + "Speaker\\";
        public static readonly string LinkedCamera = SelectedDevices + "LinkedCamera\\";

        public static readonly string MediaType      = "MediaType";
        public static readonly string FormatBlock    = "FormatBlock";

        public static readonly string PhysicalConnectorIndex  = "PhysicalConnectorIndex";
        public static readonly string VideoStandardIndex      = "VideoStandardIndex";
        public static readonly string MicrophoneSourceIndex = "MicrophoneSourceIndex";

        public static readonly string CompressorEnabled = "CompressorEnabled";
        public static readonly string AudioCompressorEnabled = "AudioCompressorEnabled";
        public static readonly string VideoCompressorEnabled = "VideoCompressorEnabled";
        public static readonly string CustomCompression = "CustomCompression";
        public static readonly string CompressorBitRate = "CompressorBitRate";
        public static readonly string CompressorKeyFrameRate = "CompressorKeyFrameRate";
        public static readonly string CompressionMediaTypeIndex = "CompressionMediaTypeIndex";

        public static readonly string AutoPlayRemoteAudio = "AutoPlayRemoteAudio";
        public static readonly string AutoPlayRemoteVideo = "AutoPlayRemoteVideo";

        public static readonly string AudioBufferSize = "AudioBufferSize";
        public static readonly string AudioBufferCount = "AudioBufferCount";

        public static object ReadValue(string key, string valName)
        {
            object ret = null;

            using(RegistryKey rk = clientKey.OpenSubKey(key))
            {
                if (rk != null) // Settings exist
                {
                    ret = rk.GetValue(valName);
                }
            }

            return ret;
        }

        public static void DeleteValue(string key, string valName)
        {
            using(RegistryKey rk = clientKey.OpenSubKey(key, true))
            {
                if (rk != null) 
                {
                    rk.DeleteValue(valName, false);
                }
            }
        }

        public static void WriteValue(string key, string valName, object val)
        {
            using(RegistryKey rk = clientKey.CreateSubKey(key))
            {
                if (rk != null) 
                {
                    rk.SetValue(valName, val);
                }
            }
        }

        public static string[] ValueNames(string key)
        {
            string[] ret = null;

            using(RegistryKey rk = clientKey.OpenSubKey(key))
            {
                if (rk != null) 
                {
                    ret = rk.GetValueNames();
                }
            }

            return ret;
        }
    }
}