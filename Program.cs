using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Diagnostics;

public static class position_windows
{
    public class ApplicationInfo
    {
        public IntPtr hwnd;
        
        public String windowTitle;

        public String processName;

        public ApplicationInfo(IntPtr HWND, String ProcessName, String WindowTitle)
        {
            hwnd = HWND;
            processName = ProcessName;
            windowTitle = WindowTitle;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct TITLEBARINFO
    {
        public const int CCHILDREN_TITLEBAR = 5;
        public uint cbSize; //Specifies the size, in bytes, of the structure. 
                            //The caller must set this to sizeof(TITLEBARINFO).

        public RECT rcTitleBar; //Pointer to a RECT structure that receives the 
                                //coordinates of the title bar. These coordinates include all title-bar elements
                                //except the window menu.

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]

        //Add reference for System.Windows.Forms
        public AccessibleStates[] rgstate;
        //0    The title bar itself.
        //1    Reserved.
        //2    Minimize button.
        //3    Maximize button.
        //4    Help button.
        //5    Close button.
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        internal int left;
        internal int top;
        internal int right;
        internal int bottom;

        internal RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }
    }

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    [DllImport("user32.dll")]
    public static extern int EnumWindows(CallBack x, ArrayList y);
    public delegate bool CallBack(IntPtr hwnd, ArrayList lParam);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetTitleBarInfo(IntPtr hwnd, ref TITLEBARINFO pti);

    [DllImport("dwmapi.dll")]
    static extern int DwmGetWindowAttribute(IntPtr hWnd, DwmWindowAttribute dwAttribute, out RECT lpRect, int cbAttribute);

    private enum DwmWindowAttribute
    {
        DWMWA_NCRENDERING_ENABLED = 1,
        DWMWA_NCRENDERING_POLICY = 2,
        DWMWA_TRANSITIONS_FORCEDISABLED = 3,
        DWMWA_ALLOW_NCPAINT = 4,
        DWMWA_CAPTION_BUTTON_BOUNDS = 5,
        DWMWA_NONCLIENT_RTL_LAYOUT = 6,
        DWMWA_FORCE_ICONIC_REPRESENTATION = 7,
        DWMWA_FLIP3D_POLICY = 8,
        DWMWA_EXTENDED_FRAME_BOUNDS = 9,
        DWMWA_HAS_ICONIC_BITMAP = 10,
        DWMWA_DISALLOW_PEEK = 11,
        DWMWA_EXCLUDED_FROM_PEEK = 12,
        DWMWA_CLOAK = 13,
        DWMWA_CLOAKED = 14,
        DWMWA_FREEZE_REPRESENTATION = 15,
        DWMWA_LAST = 16
    };

    // Define the SetWindowPosFlags enumeration.
    [Flags()]
    private enum SetWindowPosFlags : uint
    {
        SynchronousWindowPosition = 0x4000,
        DeferErase = 0x2000,
        DrawFrame = 0x0020,
        FrameChanged = 0x0020,
        HideWindow = 0x0080,
        DoNotActivate = 0x0010,
        DoNotCopyBits = 0x0100,
        IgnoreMove = 0x0002,
        DoNotChangeOwnerZOrder = 0x0200,
        DoNotRedraw = 0x0008,
        DoNotReposition = 0x0200,
        DoNotSendChangingEvent = 0x0400,
        IgnoreResize = 0x0001,
        IgnoreZOrder = 0x0004,
        ShowWindow = 0x0040,
    }

    internal enum ShowWindowCommands
    {
        /// <summary>
        /// Hides the window and activates another window.
        /// </summary>
        Hide = 0,
        /// <summary>
        /// Activates and displays a window. If the window is minimized or 
        /// maximized, the system restores it to its original size and position.
        /// An application should specify this flag when displaying the window 
        /// for the first time.
        /// </summary>
        Normal = 1,
        /// <summary>
        /// Activates the window and displays it as a minimized window.
        /// </summary>
        ShowMinimized = 2,
        /// <summary>
        /// Maximizes the specified window.
        /// </summary>
        Maximize = 3, // is this the right value?
        /// <summary>
        /// Activates the window and displays it as a maximized window.
        /// </summary>       
        ShowMaximized = 3,
        /// <summary>
        /// Displays a window in its most recent size and position. This value 
        /// is similar to <see cref="Win32.ShowWindowCommand.Normal"/>, except 
        /// the window is not activated.
        /// </summary>
        ShowNoActivate = 4,
        /// <summary>
        /// Activates the window and displays it in its current size and position. 
        /// </summary>
        Show = 5,
        /// <summary>
        /// Minimizes the specified window and activates the next top-level 
        /// window in the Z order.
        /// </summary>
        Minimize = 6,
        /// <summary>
        /// Displays the window as a minimized window. This value is similar to
        /// <see cref="Win32.ShowWindowCommand.ShowMinimized"/>, except the 
        /// window is not activated.
        /// </summary>
        ShowMinNoActive = 7,
        /// <summary>
        /// Displays the window in its current size and position. This value is 
        /// similar to <see cref="Win32.ShowWindowCommand.Show"/>, except the 
        /// window is not activated.
        /// </summary>
        ShowNA = 8,
        /// <summary>
        /// Activates and displays the window. If the window is minimized or 
        /// maximized, the system restores it to its original size and position. 
        /// An application should specify this flag when restoring a minimized window.
        /// </summary>
        Restore = 9,
        /// <summary>
        /// Sets the show state based on the SW_* value specified in the 
        /// STARTUPINFO structure passed to the CreateProcess function by the 
        /// program that started the application.
        /// </summary>
        ShowDefault = 10,
        /// <summary>
        ///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread 
        /// that owns the window is not responding. This flag should only be 
        /// used when minimizing windows from a different thread.
        /// </summary>
        ForceMinimize = 11
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static implicit operator System.Drawing.Point(POINT p)
        {
            return new System.Drawing.Point(p.X, p.Y);
        }

        public static implicit operator POINT(System.Drawing.Point p)
        {
            return new POINT(p.X, p.Y);
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    internal struct WINDOWPLACEMENT
    {
        /// <summary>
        /// The length of the structure, in bytes. Before calling the GetWindowPlacement or SetWindowPlacement functions, set this member to sizeof(WINDOWPLACEMENT).
        /// <para>
        /// GetWindowPlacement and SetWindowPlacement fail if this member is not set correctly.
        /// </para>
        /// </summary>
        public int Length;

        /// <summary>
        /// Specifies flags that control the position of the minimized window and the method by which the window is restored.
        /// </summary>
        public int Flags;

        /// <summary>
        /// The current show state of the window.
        /// </summary>
        public ShowWindowCommands ShowCmd;

        /// <summary>
        /// The coordinates of the window's upper-left corner when the window is minimized.
        /// </summary>
        public POINT MinPosition;

        /// <summary>
        /// The coordinates of the window's upper-left corner when the window is maximized.
        /// </summary>
        public POINT MaxPosition;

        /// <summary>
        /// The window's coordinates when the window is in the restored position.
        /// </summary>
        public RECT NormalPosition;

        /// <summary>
        /// Gets the default (empty) value.
        /// </summary>
        public static WINDOWPLACEMENT Default
        {
            get
            {
                WINDOWPLACEMENT result = new WINDOWPLACEMENT();
                result.Length = Marshal.SizeOf(result);
                return result;
            }
        }
    }


    public static void Main()
    {
        ArrayList screens = new ArrayList();
        foreach (System.Windows.Forms.Screen screen in Screen.AllScreens)
        {
            Console.WriteLine("Device Name: " + screen.DeviceName);
            Console.WriteLine("Bounds: " +
                screen.Bounds.ToString());
            Console.WriteLine("Type: " +
                screen.GetType().ToString());
            Console.WriteLine("Working Area: " +
                screen.WorkingArea.ToString());
            Console.WriteLine("Primary Screen: " +
                screen.Primary.ToString());
        }

        ArrayList focusableApps = new ArrayList();
        CallBack myCallBack = new CallBack(position_windows.GetFocusableApps);
        EnumWindows(myCallBack, focusableApps);
        // List<RECT> locations = SplitHalfHorizontal(Screen.AllScreens[0]);
        List<RECT> locations = SplitHalfVertical(Screen.AllScreens[0]);
        ApplicationInfo app1 = focusableApps[0] as ApplicationInfo;
        ApplicationInfo app2 = focusableApps[1] as ApplicationInfo;
        ArrangeWindows(app2.hwnd, locations[1]);
        ArrangeWindows(app1.hwnd, locations[0]);

    }

    public static string GetFocusableAppsString(){
        string focusableAppsString = "";
        ArrayList focusableApps = new ArrayList();
        CallBack myCallBack = new CallBack(position_windows.GetFocusableApps);
        EnumWindows(myCallBack, focusableApps);
        foreach (ApplicationInfo app in focusableApps){
            string windowTitle = app.windowTitle;
            focusableAppsString = focusableAppsString + windowTitle + "\r\n";
        }
        Console.Write(focusableAppsString);
        return focusableAppsString;
    }

    public static bool GetFocusableApps(IntPtr hwnd, ArrayList lParam)
    {

        int size = GetWindowTextLength(hwnd);
        TITLEBARINFO tbi = new TITLEBARINFO();
        tbi.cbSize = (uint)Marshal.SizeOf(tbi);
        GetTitleBarInfo(hwnd, ref tbi);

        if (size > 0
            && IsWindowVisible(hwnd)
            && tbi.rgstate[0] != AccessibleStates.Invisible
            && tbi.rgstate[0] == AccessibleStates.Focusable)
        {

            Console.Write("Window handle is ");
            Console.Write(hwnd);
            Console.Write(" Window State is: ");
            Console.Write(tbi.rgstate[0]);
            var builder = new StringBuilder(size + 1);
            GetWindowText(hwnd, builder, builder.Capacity);
            Console.WriteLine(" Window Title " + builder.ToString());
            
            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
            string appName = (Process.GetProcessById((int)pid)).ProcessName;
            lParam.Add(new ApplicationInfo(hwnd, appName, builder.ToString()));
        }
        return true;
    }

    internal static void ArrangeWindows(IntPtr hwnd, RECT position)
    {
        WINDOWPLACEMENT winplc = new WINDOWPLACEMENT();
        winplc.Length = Marshal.SizeOf(winplc);
        winplc.ShowCmd = ShowWindowCommands.Normal;
        winplc.NormalPosition = position;
        winplc.Flags = 4;
        SetWindowPlacement(hwnd, ref winplc);

        //Wait for ASYNC Windows to update position before checking 
        //if they are in the correct location
        System.Threading.Thread.Sleep(10);

        RECT visibleWindow = new RECT();
        DwmGetWindowAttribute(hwnd, DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out visibleWindow, Marshal.SizeOf(typeof(RECT)));

        //Attempt to fit the window better taking into account the
        //invisible "glass" border in Windows Vista and forward.
        if (!RECT.Equals(visibleWindow, position))
        {
            int leftDiff = position.left - visibleWindow.left;
            int topDiff = position.top - visibleWindow.top;
            int rightDiff = position.right - visibleWindow.right;
            int bottomDiff = position.bottom - visibleWindow.bottom;
            int diffSum = leftDiff + topDiff + rightDiff + bottomDiff;

            //Correct position on the screen if the diff is less than the glass
            //border around. If diff is too big, something is wrong, do not try to
            //adjust.
            if (diffSum <= 28 && diffSum > 0)
            {
                RECT adjustedPosition = position;
                adjustedPosition.bottom += bottomDiff;
                adjustedPosition.left += leftDiff;
                adjustedPosition.right += rightDiff;
                adjustedPosition.top += topDiff;
                winplc.NormalPosition = adjustedPosition;
                SetWindowPlacement(hwnd, ref winplc);
            }
        }
    }

    static List<RECT> SplitHalfVertical(System.Windows.Forms.Screen screen)
    {
        List<RECT> splits = new List<RECT>();
        int halfWayPoint = screen.WorkingArea.Width / 2;
        RECT left = new RECT(screen.WorkingArea.X, screen.WorkingArea.Y, halfWayPoint, screen.WorkingArea.Height);
        RECT right = new RECT(halfWayPoint, screen.WorkingArea.Y, screen.WorkingArea.Width, screen.WorkingArea.Height);
        splits.Add(left);
        splits.Add(right);
        return splits;
    }

    static List<RECT> SplitHalfHorizontal(System.Windows.Forms.Screen screen)
    {
        List<RECT> splits = new List<RECT>();
        int halfWayPoint = screen.WorkingArea.Height / 2;
        RECT top = new RECT(screen.WorkingArea.X, screen.WorkingArea.Y, screen.WorkingArea.Right, halfWayPoint);
        RECT bottom = new RECT(screen.WorkingArea.X, halfWayPoint, screen.WorkingArea.Width, screen.WorkingArea.Height);
        splits.Add(top);
        splits.Add(bottom);
        return splits;
    }
}
