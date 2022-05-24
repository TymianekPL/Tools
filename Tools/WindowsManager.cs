using System.Runtime.InteropServices;

namespace Tools
{
    public class WindowsManager
    {
        [DllImport("ntdll.dll")]
        private static extern void RtlAdjustPrivilege(uint x, bool y, bool z, out bool a);
        [DllImport("ntdll.dll")]
        private static extern void NtRaiseHardError(ulong x, ulong y, ulong z, ulong c, ulong a, out ulong b);
        public static void InvokeBSOD()
        {
            RtlAdjustPrivilege(19, true, false, out _);
            NtRaiseHardError(0xC0000420L, 0, 0, 0, 6, out _);
        }
    }
}
