using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;


namespace Tools
{
    public class Settings
    {
        public static string GetExecutableDirectory()
        {
            return Path.GetDirectoryName(GetExecutable());
        }

        public static string GetExecutable()
        {
            return Process.GetCurrentProcess().MainModule.FileName;
        }
    }
}