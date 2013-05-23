// $Id: DibGraphicsBuffer.cs 775 2005-09-21 20:42:02Z shoat $

//-------------------------------------------------------------------------
//
//  This is part of the Microsoft Tablet PC Platform SDK
//  Copyright (C) 2002 Microsoft Corporation
//  All rights reserved.
//
//  This source code is only intended as a supplement to the
//  Microsoft Tablet PC Platform SDK Reference and related electronic 
//  documentation provided with the Software Development Kit.
//  See these sources for more detailed information. 
//
//  File: DibGraphicsBuffer.cs
//  Printing Ink Sample Application
//
//  This file contains a class that implements a fast Device Independent
//  Bitmap (DIB) bugger.  It's used to be able to print ink so that strokes
//  are antialiased and can have transparency.
//
//--------------------------------------------------------------------------
namespace MSR.LST.ConferenceXP {
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Drawing;
    using System.Drawing.Text;
    using System.Diagnostics;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;

    /// <summary>
    /// This class is for a fast Device Independent Bitmap (DIB) buffer.  
    /// It's very quick, but requires unsafe code blocks since it calls 
    /// directly into native code.
    /// </summary>
    public class DibGraphicsBuffer : IDisposable {
        private int dib;
        private IntPtr compatDC;
        private int oldBitmap;
        private int bufferWidth;
        private int bufferHeight;
        private Graphics buffer;

        /// <summary>
        /// Creates a new Graphics buffer from a compatible Graphics
        /// </summary>
        /// <param name="destinationGraphics">The destination Graphics that will be eventually painted on</param>
        /// <param name="width">The width of the new buffer</param>
        /// <param name="height">The height of the new buffer</param>
        /// <returns>The new buffer</returns>
        public Graphics RequestBuffer(Graphics destinationGraphics, int width, int height) {
            return CreateBuffer(destinationGraphics, width, height);
        }

        /// <summary>
        /// Paints from the sourceGraphics to the destination Graphics
        /// </summary>
        /// <param name="sourceGraphics">The source graphics to paint from</param>
        /// <param name="x">The x location to start painting</param>
        /// <param name="y">The y location to start painting</param>
        public void PaintBuffer(Graphics sourceGraphics, int x, int y) {
            int rop = 0xcc0020; // RasterOp.SOURCE.GetRop();
            IntPtr sourceDc = sourceGraphics.GetHdc();
            try {
                // Copy pixels (BitBlt) from source to destination
                NativeMethods.BitBlt(sourceDc.ToInt32(), 
                    x, y, bufferWidth, bufferHeight, 
                    compatDC.ToInt32(), 0, 0, 
                    rop); 
            }
            finally {
                sourceGraphics.ReleaseHdc(sourceDc);
            }
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose() {
            CleanNative();
        }

        /// <summary>
        /// Clean up from native code
        /// </summary>
        private void CleanNative() {
            if (buffer != null) {
                buffer.Dispose();
                buffer = null;
            }
            if (oldBitmap != (int)0) {
                NativeMethods.SelectObject((int)compatDC, oldBitmap);
                oldBitmap = (int)0;
            }
            if (compatDC != new IntPtr(0)) {
                NativeMethods.DeleteDC((int)compatDC);
                compatDC = new IntPtr(0);
            }
            if (dib != (int)0) {
                NativeMethods.DeleteObject(dib);
                dib = (int)0;
            }
        }

        /// <summary>
        /// Create a Graphics buffer from a compatible Graphics
        /// </summary>
        /// <param name="compat">Compatible Graphics</param>
        /// <param name="width">Width of the buffer</param>
        /// <param name="height">Height of the buffer</param>
        /// <returns></returns>
        private Graphics CreateBuffer(Graphics compat, int width, int height) {
            // If the last buffer we created works, then just use it.
            if (width == bufferWidth && height == bufferHeight && buffer != null) {
                return buffer;
            }

            // Clean up from any previous buffers
            CleanNative();

            // Create new buffer
            IntPtr src = compat.GetHdc();
            try {
                int pvbits = NativeMethods.Nullint;
                dib = CreateCompatibleDIB((int)src, width, height, ref pvbits);
                compatDC = new IntPtr(NativeMethods.CreateCompatibleDC(src.ToInt32()));
            }
            finally {
                compat.ReleaseHdc(src);
            }

            // Save old bitmap for later clean-up
            oldBitmap = NativeMethods.SelectObject((int)compatDC, dib);

            // Save new buffer
            buffer = Graphics.FromHdc(compatDC);
            bufferWidth = width;
            bufferHeight = height;

            return buffer;
        }

        /// <summary>
        /// Call into native code to create compatible Device Independent Bitmap
        /// </summary>
        /// <param name="hdc">Source hdc</param>
        /// <param name="ulWidth">Bitmap width</param>
        /// <param name="ulHeight">Bitmap height</param>
        /// <param name="ppvBits">Returned bits</param>
        /// <returns>The handle to the new bitmap</returns>
        private int CreateCompatibleDIB(int hdc, int ulWidth, int ulHeight, ref int ppvBits) {
            int hbmRet = NativeMethods.Nullint;
            NativeMethods.BITMAPINFO_FLAT pbmi = new NativeMethods.BITMAPINFO_FLAT();

            //
            // Validate hdc.
            //
            if (hdc == NativeMethods.Nullint) {
                throw new ArgumentNullException("hdc");
            }
            if (NativeMethods.GetObjectType(hdc) != NativeMethods.OBJ_DC ) {
                throw new ArgumentException("hdc", "HDC must be screen DC!");
            }

            if (FillBitmapInfo(hdc, ref pbmi)) {
                //
                // Change bitmap size to match specified dimensions.
                //

                pbmi.bmiHeader_biWidth = ulWidth;
                pbmi.bmiHeader_biHeight = ulHeight;
                if (pbmi.bmiHeader_biCompression == NativeMethods.BI_RGB) {
                    pbmi.bmiHeader_biSizeImage = 0;
                }
                else {
                    if ( pbmi.bmiHeader_biBitCount == 16 )
                        pbmi.bmiHeader_biSizeImage = ulWidth * ulHeight * 2;
                    else if ( pbmi.bmiHeader_biBitCount == 32 )
                        pbmi.bmiHeader_biSizeImage = ulWidth * ulHeight * 4;
                    else
                        pbmi.bmiHeader_biSizeImage = 0;
                }
                pbmi.bmiHeader_biClrUsed = 0;
                pbmi.bmiHeader_biClrImportant = 0;

                //
                // Create the DIB section.  Let Win32 allocate the memory and return
                // a pointer to the bitmap surface.
                //

                hbmRet = NativeMethods.CreateDIBSection(hdc, ref pbmi, NativeMethods.DIB_RGB_COLORS, ref ppvBits, NativeMethods.Nullint, 0);

                if ( hbmRet == NativeMethods.Nullint ) {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }

            return hbmRet;
        }

        /// <summary>
        /// Gets the bitmap information
        /// </summary>
        /// <param name="hdc">Source hdc</param>
        /// <param name="pbmi">Pointer to bitmap information</param>
        /// <returns>True if successful; otherwise, false</returns>
        private bool FillBitmapInfo(int hdc, ref NativeMethods.BITMAPINFO_FLAT pbmi) {
            int hbm = NativeMethods.Nullint;
            bool bRet = false;
            try {
                //
                // Create a dummy bitmap from which we can query color format info
                // about the device surface.
                //

                hbm = NativeMethods.CreateCompatibleBitmap(hdc, 1, 1);

                if (hbm == NativeMethods.Nullint) {
                    throw new Exception("failed to create bitmap for query");
                }

                pbmi.bmiHeader_biSize = Marshal.SizeOf(typeof(NativeMethods.BITMAPINFOHEADER));
                pbmi.bmiColors = new byte[NativeMethods.BITMAPINFO_MAX_COLORSIZE*4];

                //
                // Call first time to fill in BITMAPINFO header.
                //

                int diRet = NativeMethods.GetDIBits(hdc, 
                    hbm, 
                    0, 
                    0, 
                    NativeMethods.Nullint, 
                    ref pbmi, 
                    NativeMethods.DIB_RGB_COLORS);

                if ( pbmi.bmiHeader_biBitCount <= 8 ) {
                    bRet = FillColorTable(hdc, ref pbmi);
                }
                else {
                    if ( pbmi.bmiHeader_biCompression == NativeMethods.BI_BITFIELDS ) {
                        //
                        // Call a second time to get the color masks.
                        // It's a GetDIBits Win32 "feature".
                        //

                        NativeMethods.GetDIBits(hdc, 
                            hbm, 
                            0, 
                            pbmi.bmiHeader_biHeight, 
                            NativeMethods.Nullint, 
                            ref pbmi,
                            NativeMethods.DIB_RGB_COLORS);
                    }

                    bRet = true;
                }
            }
            finally {
                if (hbm != NativeMethods.Nullint) {
                    NativeMethods.DeleteObject(hbm);
                    hbm = NativeMethods.Nullint;
                }
            }

            return bRet;
        }

        /// <summary>
        /// Fills color table for bit map information
        /// </summary>
        /// <param name="hdc">Source hdc</param>
        /// <param name="pbmi">Pointer to bitmap information</param>
        /// <returns>True if successful; otherwise, false</returns>
        private unsafe bool FillColorTable(int hdc, ref NativeMethods.BITMAPINFO_FLAT pbmi) {
            bool bRet = false;
            byte[] aj = new byte[sizeof(NativeMethods.PALETTEENTRY) * 256];
            int i, cColors;

            fixed (byte* prgb = &pbmi.bmiColors[0]) {
                fixed (byte* lppe = &aj[0]) {
                    cColors = 1 << pbmi.bmiHeader_biBitCount;
                    if ( cColors <= 256 ) {

                        int palRet = NativeMethods.GetSystemPaletteEntries(hdc, 0, cColors, aj);
                        if ( palRet != 0 ) {
                            for (i = 0; i < cColors; i++) {
                                ((NativeMethods.RGBQUAD*)prgb)[i].rgbRed      = ((NativeMethods.PALETTEENTRY*)lppe)[i].peRed;
                                ((NativeMethods.RGBQUAD*)prgb)[i].rgbGreen    = ((NativeMethods.PALETTEENTRY*)lppe)[i].peGreen;
                                ((NativeMethods.RGBQUAD*)prgb)[i].rgbBlue     = ((NativeMethods.PALETTEENTRY*)lppe)[i].peBlue;
                                ((NativeMethods.RGBQUAD*)prgb)[i].rgbReserved = 0;
                            }

                            bRet = true;
                        }
                        else {
                            Debug.WriteLine("bFillColorTable: MyGetSystemPaletteEntries failed\n");
                        }
                    }
                }
            }

            return bRet;
        }
    }


    /// <summary>
    /// This class encapsulates all of the calls to native code.
    /// </summary>
    class NativeMethods {
        // Constants
        public static readonly int Nullint = (int)0;
        public const int
            BITMAPINFO_MAX_COLORSIZE = 256,
            DIB_RGB_COLORS = 0,
            DIB_PAL_COLORS = 1,
            OBJ_PEN = 1,
            OBJ_BRUSH = 2,
            OBJ_DC = 3,
            OBJ_METADC = 4,
            OBJ_PAL = 5,
            OBJ_FONT = 6,
            OBJ_BITMAP = 7,
            OBJ_REGION = 8,
            OBJ_METAFILE = 9,
            OBJ_MEMDC = 10,
            OBJ_EXTPEN = 11,
            OBJ_ENHMETADC = 12,
            OBJ_ENHMETAFILE = 13,
            BI_RGB = 0,
            BI_RLE8 = 1,
            BI_RLE4 = 2,
            BI_BITFIELDS = 3,
            PLANES = 14,
            BITSPIXEL = 12;

        /// <summary>
        /// Red, green, blue, reserved byte structure
        /// </summary>
        public struct RGBQUAD {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }

        /// <summary>
        /// Holds all bitmap information, except colors
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
            public class BITMAPINFOHEADER {
            public int      biSize = Marshal.SizeOf(typeof(BITMAPINFOHEADER));
            public int      biWidth = 0;
            public int      biHeight = 0;
            public short    biPlanes = 0;
            public short    biBitCount = 0;
            public int      biCompression = 0;
            public int      biSizeImage = 0;
            public int      biXPelsPerMeter = 0;
            public int      biYPelsPerMeter = 0;
            public int      biClrUsed = 0;
            public int      biClrImportant = 0;
        }

        /// <summary>
        /// Holds all bitmap information
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
            public class BITMAPINFO {
            public BITMAPINFOHEADER bmiHeader = new BITMAPINFOHEADER();

            [MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst=BITMAPINFO_MAX_COLORSIZE*4)]
            public byte[] bmiColors = null; // RGBQUAD structs... Blue-Green-Red-Reserved, repeat...
        }

        /// <summary>
        /// Palette entry structure
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
            public struct PALETTEENTRY {
            public byte peRed;
            public byte peGreen;
            public byte peBlue;
            public byte peFlags;
        }

        /// <summary>
        /// Holds all bitmap information in a flat structure (contains no other classes or structs)
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
            public struct BITMAPINFO_FLAT {
            public int      bmiHeader_biSize;// = Marshal.SizeOf(typeof(BITMAPINFOHEADER));
            public int      bmiHeader_biWidth;
            public int      bmiHeader_biHeight;
            public short    bmiHeader_biPlanes;
            public short    bmiHeader_biBitCount;
            public int      bmiHeader_biCompression;
            public int      bmiHeader_biSizeImage;
            public int      bmiHeader_biXPelsPerMeter;
            public int      bmiHeader_biYPelsPerMeter;
            public int      bmiHeader_biClrUsed;
            public int      bmiHeader_biClrImportant;

            [MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst=BITMAPINFO_MAX_COLORSIZE*4)]
            public byte[] bmiColors; // RGBQUAD structs... Blue-Green-Red-Reserved, repeat...
        }

        // Native methods
        [DllImport("gdi32")]
        public static extern int GetPaletteEntries(int hpal, int iStartIndex, int nEntries, byte[] lppe);
        [DllImport("gdi32")]
        public static extern int GetSystemPaletteEntries(int hdc, int iStartIndex, int nEntries, byte[] lppe);
        [DllImport("gdi32")]
        public static extern int CreateDIBSection(int hdc, ref NativeMethods.BITMAPINFO_FLAT bmi, int iUsage, ref int ppvBits, int hSection, int dwOffset);
        [DllImport("gdi32")]
        public static extern int GetObjectType(int hobject);
        [DllImport("gdi32")]
        public static extern int CreateCompatibleDC(int hDC);
        [DllImport("gdi32")]
        public static extern int CreateCompatibleBitmap(int hDC, int width, int height);
        [DllImport("gdi32")]
        public static extern int GetDIBits(int hdc, int hbm, int arg1, int arg2, int arg3, NativeMethods.BITMAPINFOHEADER bmi, int arg5);
        [DllImport("gdi32")]
        public static extern int GetDIBits(int hdc, int hbm, int arg1, int arg2, int arg3, ref NativeMethods.BITMAPINFO_FLAT bmi, int arg5);
        [DllImport("gdi32")]
        public static extern int SelectObject(int hdc, int obj);
        [DllImport("gdi32")]
        public static extern bool DeleteObject(int hObject);
        [DllImport("gdi32")]
        public static extern bool DeleteDC(int hDC);
        [DllImport("gdi32")]
        public static extern bool BitBlt(int hDC, int x, int y, int nWidth, int nHeight,
            int hSrcDC, int xSrc, int ySrc, int dwRop);
        [DllImport("gdi32")]
        public static extern int GetDeviceCaps(int hDC, int nIndex);
    }
}