using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class PowerManagement
    {
        public static void Set(PowerAction action)
        {
            switch (action)
            {
                case PowerAction.Hibernate:
                if (!WinAPINatives.HibernateA())
                {
                    throw new Exception("There was error while trying to hibernate windows.");
                }
                break;
                case PowerAction.Shutdown:
                if (!WinAPINatives.ShutdownA())
                {
                    throw new Exception("There was error while trying to shutdown windows.");
                }
                break;
                case PowerAction.Restart:
                if (!WinAPINatives.RestartA())
                {
                    throw new Exception("There was error while trying to restart windows.");
                }
                break;
                default:
                throw new Exception($"{action} is not valid action!");
            }
        }
    }

    public enum PowerAction
    {
        Hibernate,
        Shutdown,
        Restart
    }
}
