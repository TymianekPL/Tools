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
        public static bool RestartA()
        {
            return ExitWindowsEx(0x00000002 | 0x00000004 | 0x00002000 | 0x00000003 | 0x80000000);
        }
        [DllImport("user32.dll")]
        internal static extern bool SetSuspendState(int a, int b, int c);
        public static void Hibernate()
        {
            SetSuspendState(0, 1, 0);
        }
        [DllImport("user32.dll")]
        internal static extern void Sleep(int ms);
        public static void SleepA(int ms)
        {
            Sleep(ms);
        }
    }
}