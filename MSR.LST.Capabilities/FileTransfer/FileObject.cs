using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;


namespace MSR.LST.ConferenceXP
{
    [Serializable]
    public class FileObject
    {
        byte[] fileData;
        public byte[] Data
        {
            get { return fileData; }
        }

        string filePath;
        public string Name
        {
            get { return filePath; }
        }

        Bitmap fileIcon;
        public Bitmap Icon
        {
            get { return fileIcon; }
        }

        public FileObject(string filePath)
        {
            this.filePath = filePath;

            SHFILEINFO shinfo = new SHFILEINFO();
            IntPtr hImgSmall = Win32.SHGetFileInfo(filePath, 0, ref shinfo, 
                (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_SMALLICON);

            try
            {
                fileIcon = Bitmap.FromHicon(shinfo.hIcon);
            }
            catch
            {
                fileIcon = null;
            }
            finally
            {
                try 
                {
                    Win32.DestroyIcon(shinfo.hIcon);
                }
                catch {}
            }

            FileStream fileStream = File.OpenRead(filePath);
            BinaryReader br = new BinaryReader(fileStream);
            fileData = br.ReadBytes((int)fileStream.Length);
            br.Close();
        }
    }
}
