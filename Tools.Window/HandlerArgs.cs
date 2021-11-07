using System;
using System.Runtime.InteropServices;

namespace Tools.Window
{
    [Serializable]
    [ComVisible(false)]
    public class HandlerArgs
    {
        public static readonly HandlerArgs Empty = new HandlerArgs();

        public HandlerArgs()
        {
            
        }
    }
}
