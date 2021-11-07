using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Tools.Win32;

namespace Tools
{
    public class Win32NonNative
    {
        public unsafe static IntPtr GetAdjustedToken(IntPtr hSrcToken)
        {
            IntPtr admin = new();
            IntPtr hTarToken;
            uint dw = 0;
            if (GetTokenInformation(hSrcToken, TOKEN_INFORMATION_CLASS.TokenPrivileges, admin, (uint)sizeof(IntPtr), out dw))
            {
                hTarToken = admin;
            }
            else
            {
                //DuplicateTokenEx(hSrcToken, MAX, null, SecurityIdentification, TokenPrimary, &hTarToken);
                SECURITY_ATTRIBUTES a = new();
                DuplicateTokenEx(hSrcToken, UInt32.Parse("10000000", System.Globalization.NumberStyles.HexNumber), ref a, SECURITY_IMPERSONATION_LEVEL.SecurityIdentification, TOKEN_TYPE.TokenPrimary, out hTarToken);
            }
            return hTarToken;
        }

        public static void Run(string commandline, IntPtr token)
        {
            PROCESS_INFORMATION pi;
            STARTUPINFO si = new();
            SECURITY_ATTRIBUTES sa = new();

            // Initialize structs
            si.cb = Marshal.SizeOf(si);
            // Create structs
            SECURITY_ATTRIBUTES saThreadAttributes = new();
            IntPtr hToken = GetAdjustedToken(token);
            if (!CreateProcessAsUser(hToken, commandline, @"C:\Windows\system32",
     ref sa, ref saThreadAttributes, false, 0, IntPtr.Zero, "0", ref si, out pi))
            {
                // Throw exception
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }
}
