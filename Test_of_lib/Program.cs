using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Web;
using Tools;
using static Test_of_lib.Win32API;

class Program
{
    [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
    public static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall,
    ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);
    [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
    internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

    [DllImport("advapi32.dll", SetLastError = true)]
    internal static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TokPriv1Luid
    {
        public int Count;
        public long Luid;
        public int Attr;
    }

    internal const int SE_PRIVILEGE_ENABLED = 0x00000002;
    internal const int SE_PRIVILEGE_DISABLED = 0x00000000;
    internal const int TOKEN_QUERY = 0x00000008;
    internal const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;

    public static bool EnablePrivilege(IntPtr processHandle, string privilege, bool disable)
    {
        bool retVal;
        TokPriv1Luid tp;
        IntPtr hproc = processHandle;
        IntPtr htok = IntPtr.Zero;
        retVal = OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok);
        tp.Count = 1;
        tp.Luid = 0;

        if (disable)
        {
            tp.Attr = SE_PRIVILEGE_DISABLED;
        }
        else
        {
            tp.Attr = SE_PRIVILEGE_ENABLED;
        }

        retVal = LookupPrivilegeValue(null, privilege, ref tp.Luid);
        retVal = AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
        return retVal;
    }
    static void Main(string[] args)
    {
        IntPtr hToken = IntPtr.Zero;
        IntPtr dupeTokenHandle = IntPtr.Zero;
        // For simplicity I'm using the PID of System here
        Process proc = Process.GetProcessById(8480);
        Console.WriteLine(proc.ProcessName);
        if (!EnablePrivilege(Process.GetCurrentProcess().Handle, "SeDebugPrivilege", false))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
        var h = Win32.OpenProcess((uint)Win32.ProcessAccessFlags.All, false, proc.Id);
        if (h == IntPtr.Zero)
        {
            Console.WriteLine("Error");
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
        var r = OpenProcessToken(h,
        Win32.TOKEN_QUERY | Win32.TOKEN_IMPERSONATE | Win32.TOKEN_DUPLICATE,
        ref hToken);
        if (hToken == IntPtr.Zero)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
        if (r)
        {
            WindowsIdentity newId = new(hToken);
            StringBuilder result = new();
            if (!CreateProcessAsUser(args[0], @"C:\Windows", out result, hToken))
            {
                Console.WriteLine(result.ToString());
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
        else
        {
            Console.WriteLine("Error");
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
        Console.ReadLine();
    }
}