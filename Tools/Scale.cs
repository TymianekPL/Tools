using System.Text.RegularExpressions;

namespace Tools
{
    public static class Scale
    {
        public static string LongBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (bytes >= 1024 && order < sizes.Length - 1)
            {
                order++;
                bytes /= 1024;
            }
            return string.Format("{0:0.##} {1}", bytes, sizes[order]);
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
