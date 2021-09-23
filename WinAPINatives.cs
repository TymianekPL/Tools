using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{

    public class WinAPINatives
    {
        [DllImport("user32.dll")]
        private static extern bool ExitWindowsEx(uint flags);
        public static bool ShutdownA()
        {
            return ExitWindowsEx(0x00000001 | 0x00000004 | 0x00002000 | 0x00000003 | 0x80000000);
        }
        [DllImport("WinAPI.dll")]
        public static extern bool RestartA();
        [DllImport("WinAPI.dll")]
        public static extern bool HibernateA();
        [DllImport("WinAPI.dll")]
        public static extern long CrashA();
        [DllImport("WinAPI.dll")]
        public static extern bool SleepA(int ms);
    }
}