using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


namespace Win32Util
{
    /// <summary>
    /// Encapsulates window functions that aren't in the framework.
    /// NOTE: This class is not thread-safe. 
    /// </summary>
    public class Win32Window
    {
        #region Static Members

        /// <summary>
        /// Turn this window into a tool window, so it doesn't show up in the Alt-tab list...
        /// </summary>
        /// 
        const int GWL_EXSTYLE = -20;
        const int WS_EX_TOOLWINDOW = 0x00000080;
        const int WS_EX_APPWINDOW = 0x00040000;

        const int SRCCOPY = 0x00CC0020;  // dest = source 

        private static ArrayList topLevelWindows = null;
        private static ArrayList applicationWindows = null;
        private static Image myImage = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
            Screen.PrimaryScreen.Bounds.Height);

        #endregion Static Members

        #region Static Public

        /// <summary>
        /// All top level windows 
        /// </summary>
        public static ArrayList TopLevelWindows
        {
            get
            {
                topLevelWindows = new ArrayList();
                EnumWindows(new EnumWindowsProc(EnumerateTopLevelProc), 0);
                ArrayList top = topLevelWindows;
                topLevelWindows = null;
                return top;
            }
        }

        public static ArrayList ApplicationWindows
        {
            get
            {
                applicationWindows = new ArrayList();
                EnumWindows(new EnumWindowsProc(EnumerateApplicationCallback), 0);
                ArrayList apps = applicationWindows;
                applicationWindows = null;
                return apps;
            }
        }

        /// <summary>
        /// Return all windows of a given thread
        /// </summary>
        /// <param name="threadId">The thread id</param>
        /// <returns></returns>
        public static ArrayList GetThreadWindows(int threadId)
        {
            topLevelWindows = new ArrayList();
            EnumThreadWindows(threadId, new EnumWindowsProc(EnumerateThreadProc), 0);
            ArrayList windows = topLevelWindows;
            topLevelWindows = null;
            return windows;
        }

        /// <summary>
        /// The deskop window
        /// </summary>
        public static Win32Window DesktopWindow
        {
            get
            {
                Win32Window window = new Win32Window(GetDesktopWindow());
                window.Name = "Desktop";
                return window;
            }
        }

        /// <summary>
        /// The current foreground window
        /// </summary>
        public static Win32Window ForegroundWindow
        {
            get
            {
                return new Win32Window(GetForegroundWindow());
            }
        }


        /// <summary>
        /// Find a window by name or class
        /// </summary>
        /// <param name="className">Name of the class, or null</param>
        /// <param name="windowName">Name of the window, or null</param>
        /// <returns></returns>
        public static Win32Window FindWindow(string className, string windowName)
        {
            return new Win32Window(FindWindowWin32(className, windowName));
        }

        /// <summary>
        /// Tests whether one window is a child of another
        /// </summary>
        /// <param name="parent">Parent window</param>
        /// <param name="window">Window to test</param>
        /// <returns></returns>
        public static bool IsChild(Win32Window parent, Win32Window window)
        {
            return IsChild(parent.window, window.window);
        }


        public static Image DesktopAsBitmap
        {
            get
            {
                Graphics gr1 = Graphics.FromImage(myImage);
                IntPtr dc1 = gr1.GetHdc();
                IntPtr dc2 = GetWindowDC(GetDesktopWindow());
                BitBlt(dc1, 0, 0, Screen.PrimaryScreen.Bounds.Width,
                    Screen.PrimaryScreen.Bounds.Height, dc2, 0, 0, SRCCOPY); 
                gr1.ReleaseHdc(dc1);
                gr1.Dispose();
                return myImage;
            }
        }


        #endregion Static Public

        #region Static Private

        private delegate bool EnumWindowsProc(IntPtr window, int i);

        private static bool EnumerateThreadProc(IntPtr window, int i)
        {
            topLevelWindows.Add(new Win32Window(window));
            return(true);
        }

        private static bool EnumerateApplicationCallback(IntPtr pWindow, int i)
        {
            Win32Window window = new Win32Window(pWindow);
            
            if (window.Parent.window != IntPtr.Zero) return(true);
            if (window.Visible != true) return(true);
            if (window.Text == string.Empty) return(true);
            if (window.ClassName.Substring(0, 8) == "IDEOwner") return(true); // Skip invalid VS.Net 2003 windows

            applicationWindows.Add(window);
            return(true);
        }

        private static bool EnumerateTopLevelProc(IntPtr window, int i)
        {
            topLevelWindows.Add(new Win32Window(window));
            return(true);
        }


        #endregion Static Private

        #region Static Externs Private

        [DllImport("user32.dll")]
        private static extern bool BringWindowToTop(IntPtr window);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr parent, IntPtr childAfter, string className, string windowName);

        [DllImport("user32.dll", EntryPoint="FindWindow")]
        private static extern IntPtr FindWindowWin32(string className, string windowName);

        [DllImport("user32.dll", EntryPoint="GetClassName")]
        private static extern int GetClassName(IntPtr hwnd, [In][Out] StringBuilder text, int maxBytes);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr window, int message, int wparam, int lparam);

        [DllImport("user32.dll")]
        private static extern int PostMessage(IntPtr window, int message, int wparam, int lparam);

        [DllImport("user32.dll")]
        private static extern IntPtr GetParent(IntPtr window);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetLastActivePopup(IntPtr window);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr window, [In][Out] StringBuilder text, int copyCount);

        [DllImport("user32.dll")]
        private static extern bool SetWindowText(IntPtr window, [MarshalAs(UnmanagedType.LPTStr)] string text);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr window);

        [DllImport("user32.dll")]
        private static extern int GetWindowClassNameLength(IntPtr window);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr window, int index, int value);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr window, int index);

        [DllImport("user32.dll")]
        private static extern int IsWindowVisible(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowsProc callback, int i);

        [DllImport("user32.dll")]
        private static extern bool EnumThreadWindows(int threadId, EnumWindowsProc callback, int i);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc callback, int i);

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr window, ref int processId);

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr window, IntPtr ptr);

        [DllImport("user32.dll")]
        private static extern bool GetWindowPlacement(IntPtr window, ref WindowPlacement position);

        [DllImport("user32.dll")]
        private static extern bool IsChild(IntPtr parent, IntPtr window);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr window);

        [DllImport("user32.dll")]
        private static extern bool IsZoomed(IntPtr window);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hwnd, ref Rect rectangle);

        [DllImport("gdi32.dll")]
        private static extern UInt64 BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight,
            IntPtr hSrcDC, int xSrc, int ySrc, System.Int32 dwRop);

        [DllImport("user32.dll")]
        private static extern bool GetWindowInfo(IntPtr hwnd, ref WindowInfo info);

        [DllImport("User32.dll")]
        private static extern bool PrintWindow(IntPtr windowHandle, IntPtr deviceContextHandle, UInt32 flags);

        #endregion Externs

        #region Members

        private IntPtr window;
        private ArrayList windowList = null;

        private string name = null;

        #endregion Members

        #region Public

        public string Name
        {
            get{return name;}
            set{name = value;}
        }

        public override string ToString()
        {
            string name = this.name;

            if(name == null)
            {
                name = Text;
            }
                
            return name;
        }

        /// <summary>
        /// Create a Win32Window
        /// </summary>
        /// <param name="window">The window handle</param>
        public Win32Window(IntPtr window)
        {
            this.window = window;
        }

        /// <summary>
        /// Extract the window handle 
        /// </summary>
        public IntPtr Window
        {
            get
            {
                return window;
            }
        }

        /// <summary>
        /// Return true if this window is null
        /// </summary>
        public bool IsNull
        {
            get
            {
                return window == IntPtr.Zero;
            }
        }

        /// <summary>
        /// The children of this window, as an ArrayList
        /// </summary>
        public ArrayList Children
        {
            get
            {
                windowList = new ArrayList();
                EnumChildWindows(window, new EnumWindowsProc(EnumerateChildProc), 0);
                ArrayList children = windowList;
                windowList = null;
                return children;
            }
        }

        /// <summary>
        /// Bring a window to the top
        /// </summary>
        public void BringWindowToTop()
        {
            BringWindowToTop(window);
            System.Threading.Thread.Sleep(500);
        }

        /// <summary>
        /// Find a child of this window
        /// </summary>
        /// <param name="className">Name of the class, or null</param>
        /// <param name="windowName">Name of the window, or null</param>
        /// <returns></returns>
        public Win32Window FindChild(string className, string windowName)
        {
            return new Win32Window(
                FindWindowEx(window, IntPtr.Zero, className, windowName));
        }
        /// <summary>
        /// Send a windows message to this window
        /// </summary>
        /// <param name="message"></param>
        /// <param name="wparam"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        public int SendMessage(int message, int wparam, int lparam)
        {
            return SendMessage(window, message, wparam, lparam);
        }

        /// <summary>
        /// Post a windows message to this window
        /// </summary>
        /// <param name="message"></param>
        /// <param name="wparam"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        public int PostMessage(int message, int wparam, int lparam)
        {
            return PostMessage(window, message, wparam, lparam);
        }

        /// <summary>
        /// Get the parent of this window. Null if this is a top-level window
        /// </summary>
        public Win32Window Parent
        {
            get
            {
                return new Win32Window(GetParent(window));
            }
        }

        /// <summary>
        /// Get the last (topmost) active popup
        /// </summary>
        public Win32Window LastActivePopup
        {
            get
            {
                IntPtr popup = GetLastActivePopup(window);
                if (popup == window)
                    return new Win32Window(IntPtr.Zero);
                else
                    return new Win32Window(popup);
            }
        }

        /// <summary>
        /// The text in this window
        /// </summary>
        public string Text
        {
            get
            {
                int length = GetWindowTextLength(window);
                StringBuilder sb = new StringBuilder(length + 1);
                GetWindowText(window, sb, sb.Capacity);
                return sb.ToString();
            }
            set
            {
                SetWindowText(window, value);
            }
        }

        public string ClassName
        {
            get
            {
                int length = 254;
                StringBuilder sb = new StringBuilder(length + 1);
                GetClassName(window, sb, sb.Capacity);
                sb.Length = length;
                return sb.ToString();
            }
        }

        /// <summary>
        /// Get a long value for this window. See GetWindowLong()
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetWindowLong(int index)
        {
            return GetWindowLong(window, index);
        }

        /// <summary>
        /// Set a long value for this window. See SetWindowLong()
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int SetWindowLong(int index, int value)
        {
            return SetWindowLong(window, index, value);
        }

        /// <summary>
        /// The id of the thread that owns this window
        /// </summary>
        public int ThreadId
        {
            get
            {
                return GetWindowThreadProcessId(window, IntPtr.Zero );
            }
        }

        /// <summary>
        /// The id of the process that owns this window
        /// </summary>
        public int ProcessId
        {
            get
            {
                int processId = 0;
                GetWindowThreadProcessId(window, ref processId);
                return processId;
            }
        }

        /// <summary>
        /// The placement of this window
        /// </summary>
        public WindowPlacement WindowPlacement
        {
            get
            {
                WindowPlacement placement = new WindowPlacement();
                GetWindowPlacement(window, ref placement);
                return placement;
            }
        }

        /// <summary>
        /// Whether the window is minimized
        /// </summary>
        public bool Minimized
        {
            get
            {
                return IsIconic(window);
            }
        }

        public bool Visible
        {
            get
            {
                return IsWindowVisible(window) != 0 ? true : false;
            }
        }

        /// <summary>
        /// Whether the window is maximized
        /// </summary>
        public bool Maximized
        {
            get
            {
                return IsZoomed(window);
            }
        }


        public void MakeToolWindow()
        {
            int windowStyle = GetWindowLong(GWL_EXSTYLE);
            SetWindowLong(GWL_EXSTYLE, windowStyle | WS_EX_TOOLWINDOW);
        }

        public Image WindowAsBitmap
        {
            get
            {
                if (IsNull)
                    return null;

                this.BringWindowToTop();

                Rect rect = new Rect();
                if (!GetWindowRect(window, ref rect))
                    return null;

                WindowInfo windowInfo = new WindowInfo();
                windowInfo.size = Marshal.SizeOf(typeof(WindowInfo));
                if (!GetWindowInfo(window, ref windowInfo))
                    return null;

                Image myImage = new Bitmap(rect.Width, rect.Height);
                Graphics gr1 = Graphics.FromImage(myImage);
                IntPtr dc1 = gr1.GetHdc();
                IntPtr dc2 = GetWindowDC(window);
                BitBlt(dc1, 0, 0, rect.Width, rect.Height, dc2, 0, 0, SRCCOPY);
                gr1.ReleaseHdc(dc1);
                return myImage;
            }
        }

        public Image WindowClientAsBitmap
        {
            get
            {
                if (IsNull)
                    return null;

                this.BringWindowToTop();

                Rect rect = new Rect();
                if (!GetClientRect(window, ref rect))
                    return null;

                WindowInfo windowInfo = new WindowInfo();
                windowInfo.size = Marshal.SizeOf(typeof(WindowInfo));
                if (!GetWindowInfo(window, ref windowInfo))
                    return null;

                int xOffset = windowInfo.client.X - windowInfo.window.X;
                int yOffset = windowInfo.client.Y - windowInfo.window.Y;

                Image myImage = new Bitmap(rect.Width, rect.Height);
                Graphics gr1 = Graphics.FromImage(myImage);
                IntPtr dc1 = gr1.GetHdc();
                IntPtr dc2 = GetWindowDC(window);
                BitBlt(dc1, 0, 0, rect.Width, rect.Height, dc2, xOffset, yOffset, SRCCOPY); 
                gr1.ReleaseHdc(dc1);
                return myImage;
            }
        }


        #endregion Public

        private bool EnumerateChildProc(IntPtr window, int i)
        {
            windowList.Add(new Win32Window(window));
            return(true);
        }


        struct WindowInfo
        {
            public int size;
            public Rectangle window;
            public Rectangle client;
            public int style;
            public int exStyle;
            public int windowStatus;
            public uint xWindowBorders;
            public uint yWindowBorders;
            public short atomWindowtype;
            public short creatorVersion;
        }
    }
}
// like the Win32 rect type. Can't use Rectangle because it uses a different format...
public struct Rect
{
    public int left;
    public int top;
    public int right;
    public int bottom;

    public int Width
    {
        get{return right - left;}
    }

    public int Height
    {
        get{return bottom - top;}
    }
}

public struct WindowPlacement
{
    public int length;
    public int flags;
    public int showCmd;
    public Point minPosition;
    public Point maxPosition;
    public Rectangle normalPosition;
}