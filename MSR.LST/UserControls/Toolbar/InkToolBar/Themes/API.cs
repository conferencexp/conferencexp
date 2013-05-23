using System;
using System.Runtime.InteropServices;


namespace NativeMethods
{
    [StructLayout(LayoutKind.Sequential)]
    internal class RECT
    {
        public int left,
            top,
            right,
            bottom;

        internal RECT(int left, int right, int top, int bottom)
        {
            this.left   = left;
            this.right  = right;
            this.top    = top;
            this.bottom = bottom;
        }
    }

    /// <summary>
    /// Wraps some methods of native UxTheme.dll.
    /// </summary>
    /// <remarks>
    /// Used primarily for drawing the custom color combobox control.
    /// </remarks>
    internal class UxTheme
    {
        #region COMBOBOX Parts & States
        // Parts
        public const int CP_DROPDOWN = 1;

        // States
        public const int CBXS_NORMAL   = 1;
        public const int CBXS_HOT      = 2;
        public const int CBXS_PRESSED  = 3;
        public const int CBXS_DISABLED = 4;
        #endregion

        #region EDIT Parts & States
        // Parts
        public const int EP_CARET    = 1;
        public const int EP_EDITTEXT = 2;

        public const int ETS_NORMAL   = 1;
        public const int ETS_HOT      = 2;
        public const int ETS_SELECTED = 3;
        public const int ETS_DISABLED = 4;
        public const int ETS_FOCUSED  = 5;
        public const int ETS_READONLY = 6;
        public const int ETS_ASSIST   = 7;
        #endregion

        [DllImport("UxTheme.dll")]
        internal static extern IntPtr CloseThemeData(IntPtr hTheme);

        [DllImport("UxTheme.dll")]
        internal static extern IntPtr DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, RECT pRect, RECT pClipRect);

        [DllImport("UxTheme.dll", CharSet=CharSet.Auto)]
        internal static extern IntPtr OpenThemeData(IntPtr hwnd, string pszClassList);
    }
}
