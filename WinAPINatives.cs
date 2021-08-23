using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{

    public class WinAPINatives
    {
        [DllImport("WinAPI.dll")]
        public static extern bool ShutdownA();
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