using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using Tools.Window.Components;

namespace Tools.Window
{
    public interface IWindow
    {
        public struct WindowLocation
        {
            public int X { set; get; }
            public int Y { set; get; }
        }

        public struct WindowSize
        {
            public int Height { set; get; }
            public int Width { set; get; }
        }

        public WindowLocation Location { set; get; }
        public WindowSize Size { set; get; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public string Title { get; set; }
    }

    public class WindowBase : Component, IWindowBase
    {
        private WindowBase? _instance;
        public WindowBase Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = this;
                }
                return _instance;
            }
            set
            {
                if(value != null)
                {
                    throw new ArgumentException("New value must be NULL", nameof(value));
                }
                if(_instance == null)
                {
                    return;
                }
                _instance.Dispose();
            }
        }

        public int X { get; set; } = 60;
        public int Y { get; set; } = 20;
        public int Width { get; set; } = 1000;
        public int Height { get; set; } = 800;
        public WindowStyle Style { get; set; } = WindowStyle.Default;
        public string Title { get; set; } = "My app";
        public override string Name { get; internal set; } = nameof(Window);

        delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [System.Runtime.InteropServices.StructLayout(
            System.Runtime.InteropServices.LayoutKind.Sequential,
           CharSet = System.Runtime.InteropServices.CharSet.Unicode
        )]
        struct WNDCLASS
        {
            public uint style;
            public IntPtr lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
            public string lpszMenuName;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
            public string lpszClassName;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern System.UInt16 RegisterClassW(
            [System.Runtime.InteropServices.In] ref WNDCLASS lpWndClass
        );
        [DllImport("WinAPI.dll")]
        internal static extern void Create();
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr CreateWindowExW(
           UInt32 dwExStyle,
           [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
       string lpClassName,
           [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
       string lpWindowName,
           UInt32 dwStyle,
           Int32 x,
           Int32 y,
           Int32 nWidth,
           Int32 nHeight,
           IntPtr hWndParent,
           IntPtr hMenu,
           IntPtr hInstance,
           IntPtr lpParam
        );

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern System.IntPtr DefWindowProcW(
            IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam
        );

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern bool DestroyWindow(
            IntPtr hWnd
        );
        private const int ERROR_CLASS_ALREADY_EXISTS = 1410;

#pragma warning disable CS0649
        private readonly bool m_disposed;
#pragma warning restore CS0649
        private IntPtr m_hwnd;

        public void Dispose()
        {
            Dispose(true);
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
            GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        }

        private void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                }

                // Dispose unmanaged resources
                if (m_hwnd != IntPtr.Zero)
                {

                    DestroyWindow(m_hwnd);
                    m_hwnd = IntPtr.Zero;
                }
                else
                {
                    MessageBox.Show("NULL");
                }

            }
        }

        ~WindowBase()
        {
            Dispose();
        }

        public IntPtr CustomWindow(string class_name)
        {

            if (class_name == null) throw new Exception("class_name is null");
            if (class_name == String.Empty) throw new Exception("class_name is empty");

            m_wnd_proc_delegate = CustomWndProc;

            // Create WNDCLASS
            WNDCLASS wind_class = new()
            {
                lpszClassName = class_name,
                lpfnWndProc = Marshal.GetFunctionPointerForDelegate(m_wnd_proc_delegate)
            };

            UInt16 class_atom = RegisterClassW(ref wind_class);

            int last_error = Marshal.GetLastWin32Error();

            if (class_atom == 0 && last_error != ERROR_CLASS_ALREADY_EXISTS)
            {
                throw new Exception("Could not register window class");
            }

            CLASS_NAME = class_name;

            // Create window
            m_hwnd = CreateWindowExW(
                0x00040000,
                class_name,
                Title,
                0,
                X,
                Y,
                Width,
                Height,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero
            );
            return m_hwnd;
        }

        private enum GWL
        {
            GWL_WNDPROC = (-4),
            GWL_HINSTANCE = (-6),
            GWL_HWNDPARENT = (-8),
            GWL_STYLE = (-16),
            GWL_EXSTYLE = (-20),
            GWL_USERDATA = (-21),
            GWL_ID = (-12)
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        private static IntPtr CustomWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            return DefWindowProcW(hWnd, msg, wParam, lParam);
        }

        private WndProc? m_wnd_proc_delegate;

        internal string? CLASS_NAME;

        public void Close()
        {
            if(!DestroyWindow(m_hwnd))
            {
                System.ComponentModel.Win32Exception ex = new(Marshal.GetLastWin32Error());
                MessageBox.Show($"An error occured. Parameters: {ex.Message}, 0x{ex.NativeErrorCode}", "Critical error", MessageBoxButtons.Ok, MessageBoxIcon.Error);
            }
        }

        internal override IntPtr Implement(IntPtr parent)
        {
            return parent;
        }
    }

    public interface IWindowBase
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public WindowStyle Style { get; set; }

        public string Title { get; set; }
    }

    public enum WindowStyle : int
    {
        Hidden = 0,
        Normal = 1,
        Minimized = 2,
        Maximized = 3,
        NoActive = 4,
        Show = 5,
        Minimize = 6,
        ShowMinNoActive = 7,
        ShowNa = 8,
        Restore = 9,
        Default = 10,
        ForceMinimize = 11
    }

    public class Window : WindowBase, IWindow
    {
        public IWindow.WindowLocation Location { get; set; }
        public IWindow.WindowSize Size { get; set; }

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        static extern bool UpdateWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool TranslateMessage([In] ref MSG lpMsg);
        [DllImport("user32.dll")]
        static extern IntPtr DispatchMessage([In] ref MSG lpmsg);
        [DllImport("user32.dll")]
        static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
   uint wMsgFilterMax);

        public struct MSG
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public POINT pt;
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

        public delegate void StartHandler(object sender, StartHandlerArgs e);  // delegate
        public delegate void ShowedHandler(object sender, ShowedHandlerArgs e);  // delegate
        public event StartHandler? Start;
        public event ShowedHandler? Showed;
        internal IntPtr window;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, StringBuilder lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, ref IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

        public void InitializeComponents()
        {
            foreach (Component component in components)
            {
                IntPtr cmp = component.Implement(window);
                SendMessage(cmp, 0x000C, 0, "Command link");
            }
        }

        public class StartHandlerArgs : HandlerArgs
        {

        }

        public class ShowedHandlerArgs : HandlerArgs
        {

        }

        public void Show()
        {
            if (CLASS_NAME == null)
                CLASS_NAME = $"{Title}Class";
            Start?.Invoke(this, new StartHandlerArgs());
            window = CustomWindow(CLASS_NAME);
            ShowWindow(window, (int)Style);
            Showed?.Invoke(this, new ShowedHandlerArgs());
            UpdateWindow(window);
            MSG msg = new();
            while (GetMessage(out msg, window, 0, 0) != -1)
            {
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }
        }
    }
}