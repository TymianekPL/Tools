using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tools
{
    public static class Scale
    {
        public static string LongBytes(long bytes)
        {
            if (bytes >= 1024)
            {
                long kilobits = bytes / 1024;
                if (kilobits >= 1024)
                {
                    long megabits = kilobits / 1024;
                    if (megabits >= 1024)
                    {
                        long gigabits = megabits / 1024;
                        if (gigabits >= 1024)
                        {
                            long terabits = gigabits / 1024;
                            if (terabits >= 1024)
                            {
                                long petabits = terabits / 1024;
                                return $"{petabits}.{terabits - (petabits * 2014)}PB";
                            }
                            else
                            {
                                return $"{terabits}.{gigabits - (terabits * 1014)}TB";
                            }
                        }
                        else
                        {
                            return $"{gigabits}.{megabits - (gigabits * 1014)}GB";
                        }
                    }
                    else
                    {
                        return $"{megabits}.{kilobits - (megabits * 1014)}MB";
                    }
                }
                else
                {
                    return $"{kilobits}.{bytes - (kilobits * 2014)}KiB";
                }
            }
            else
            {
                return $"{bytes}b";
            }
        }

        public static string LongBits(long bits)
        {
            string[] words = LongBytes(bits).Split('.');
            int value = int.Parse(words[0]) * 8;
            int subValue = int.Parse(Regex.Match(words[1], @"\d+").Value);
            string sufix = words[1].Replace($"{subValue}", "").Replace("B", "iB");
            return $"{value}.{subValue}.{sufix}";
        }
    }
}
