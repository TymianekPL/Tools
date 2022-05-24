using System.Diagnostics;
using System.Reflection;

namespace Tools.Diagnostics
{
    public class Logger
    {
        public static bool IsDebugEnabled { get; set; }

        private StreamWriter file;

        public string Name { get; private set; }
        public string Path { get; private set; }
        public Logger(string name)
        {
            // get start time of the application
            DateTime startTime = Process.GetCurrentProcess().StartTime;
            Name = name;
            Path = @$"logs\{name}\{startTime:yyyy-MM-dd HH-mm-ss}.log";

            if (!Directory.Exists($"logs\\{Name}"))
            {
                _ = Directory.CreateDirectory($"logs\\{Name}");
            }


            { file = new(Path, true) { AutoFlush = true }; }
        }

        public void Log(string message)
        {
            if (!Directory.Exists($"logs\\{Name}"))
            {
                _ = Directory.CreateDirectory("logs");
            }
            if (file is null)
            { { file = new(Path, true) { AutoFlush = true }; } file.AutoFlush = true; }
            file.WriteLine($"({DateTime.Now.ToLocalTime()}) [{Name}] {message}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"[{DateTime.Now.ToLocalTime()}]");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($" [{Name}/Log]");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {message}");
            Console.ResetColor();
        }

        public void Log(string message, params object[] args)
        {
            Log(string.Format(message, args));
        }

        public void Warning(string message)
        {
            if (!Directory.Exists($"logs\\{Name}"))
            {
                _ = Directory.CreateDirectory("logs");
            }
            if (file is null)
            { file = new(Path, true) { AutoFlush = true }; }
            file.WriteLine($"({DateTime.Now.ToLocalTime()}) [{Name}/Warn] {message}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"[{DateTime.Now.ToLocalTime()}]");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($" [{Name}/Warn]");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {message}");
            Console.ResetColor();
        }

        public void Warning(string message, params object[] args)
        {
            Warning(string.Format(message, args));
        }

        public void Error(string message)
        {
            if (!Directory.Exists($"logs\\{Name}"))
            {
                _ = Directory.CreateDirectory("logs");
            }
            if (file is null)
            {
                file = new(Path, true) { AutoFlush = true };
            }
            file.WriteLine($"({DateTime.Now.ToLocalTime()}) [{Name}/Error] {message}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"[{DateTime.Now.ToLocalTime()}]");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($" [{Name}/Error]");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {message}");
            Console.ResetColor();
        }

        public void Error(string message, params object[] args)
        {
            Error(string.Format(message, args));
        }

        public void Success(string message)
        {
            if (!Directory.Exists($"logs\\{Name}"))
            {
                _ = Directory.CreateDirectory("logs");
            }
            if (file is null)
            {
                file = new(Path, true) { AutoFlush = true };
            }
            file.WriteLine($"({DateTime.Now.ToLocalTime()}) [{Name}] [SUCCESS] {message}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"[{DateTime.Now.ToLocalTime()}]");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($" [{Name}/Success]");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {message}");
            Console.ResetColor();
        }

        public void Success(string message, params object[] args)
        {
            Success(string.Format(message, args));
        }

        public void Debug(string message)
        {
            if (IsDebugEnabled)
            {
                if (!Directory.Exists($"logs\\{Name}"))
                {
                    _ = Directory.CreateDirectory("logs");
                }
                if (file is null)
                {
                    {
                        file = new(Path, true) { AutoFlush = true };
                    }
                    file.AutoFlush = true;
                }
                file.WriteLine($"({DateTime.Now.ToLocalTime()}) [{Name}] [DEBUG] {message}");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write($"[{DateTime.Now.ToLocalTime()}]");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($" [{Name}/Debug]");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($" {message}");
                Console.ResetColor();
            }
        }

        public void Debug(string message, params object[] args)
        {
            Debug(string.Format(message, args));
        }

        public void Exception(Exception ex)
        {
            if (!Directory.Exists($"logs\\{Name}"))
            {
                _ = Directory.CreateDirectory("logs");
            }
            if (file is null)
            {
                file = new(Path, true) { AutoFlush = true };
            }
            file.WriteLine($"({DateTime.Now.ToLocalTime()}) [{Name}] [EXCEPTION] {ex.Message}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"[{DateTime.Now.ToLocalTime()}]");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($" [{Name}/Exception]");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {ex.Message}");
            Console.ResetColor();
        }

        public void Crash(Exception ex, CrashBehaviour behaviour)
        {
            if (!Directory.Exists($"logs\\{Name}"))
            {
                _ = Directory.CreateDirectory("logs");
            }
            if (file is null)
            {
                file = new(Path, true) { AutoFlush = true };
            }

            file.WriteLine($"=== Uh oh! The program has crashed! ({DateTime.Now.ToLocalTime()}) ===");
            file.WriteLine($"({DateTime.Now.ToLocalTime()}) [{Name}] [CRASH] {ex.Message}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"[{DateTime.Now.ToLocalTime()}]");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($" [{Name}/Crash]");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {ex.Message}");
            Console.ResetColor();

            if (behaviour == CrashBehaviour.Exit)
            {
                Environment.Exit(1);
            }
            else if (behaviour == CrashBehaviour.Restart)
            {
                _ = Process.Start(Assembly.GetEntryAssembly().Location, Process.GetCurrentProcess().StartInfo.Arguments);
                Environment.Exit(1);
            }
            else if (behaviour == CrashBehaviour.Severe)
            {
                Error("=== FATAL ERROR ===");

                Dictionary<string, string> data = new()
                {
                    { "Version", Assembly.GetEntryAssembly().GetName().Version.ToString() },
                    { "OS", Environment.OSVersion.ToString() },
                    { "Architecture", Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit" },
                    { "Runtime", Environment.Version.ToString() },
                    { "CommandLine", Environment.CommandLine },
                    { "MachineName", Environment.MachineName },
                    { "UserName", Environment.UserName },
                    { "UserDomainName", Environment.UserDomainName },
                    { "CurrentDirectory", Environment.CurrentDirectory },
                    { "CurrentProcessPath", Environment.ProcessPath ?? "Unknown" },

                    { "Exception", ex.Message },
                    { "StackTrace", ex.StackTrace ?? "Unknown" },
                };

                Error("=== SYSTEM INFO ===");
                Table(data);

                Error("=== FATAL ERROR ===");

                Console.WriteLine($"Log file: {System.IO.Path.GetFullPath(Path)}");

                Environment.Exit(1);
            }
        }

        public void Table(string[] entries, bool vetical)
        {
            if (!Directory.Exists($"logs\\{Name}"))
            {
                _ = Directory.CreateDirectory("logs");
            }
            if (file is null)
            {
                file = new(Path, true) { AutoFlush = true };
            }

            if (vetical)
            {
                for (int i = 0; i < entries.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {entries[i]}");
                    file.WriteLine($"{i + 1}. {entries[i]}");
                }
            }
            else
            {
                string line = entries[0];
                for (int i = 1; i < entries.Length - 1; i++)
                {
                    line += $" | {entries[i]} | ";
                }
                line += entries[^1];
                file.WriteLine(line);
                for (int i = 0; i < entries.Length; i++)
                {
                    Console.Write($"{i + 1}. {entries[i]}");
                    // check if console width is enough
                    if (Console.WindowWidth - Console.CursorLeft < entries[i].ToString().Length)
                    {
                        Console.WriteLine();
                    }
                }
            }
        }

        public void Table(string[] entries)
        {
            Table(entries, false);
        }

        public void Table(Dictionary<string, string> entries)
        {
            string[] entriesArray = new string[entries.Count];
            int i = 0;
            foreach (KeyValuePair<string, string> entry in entries)
            {
                entriesArray[i] = entry.Key + ": " + entry.Value;
                i++;
            }
            Table(entriesArray, true);
        }

        public static void Log(string message, string name)
        {
            Logger logger = new(name);
            logger.Log(message);
        }

        public static void Log(string message, string name, params object[] args)
        {
            Logger logger = new(name);
            logger.Log(message, args);
        }

        public static void Warning(string message, string name)
        {
            Logger logger = new(name);
            logger.Warning(message);
        }

        public static void Warning(string message, string name, params object[] args)
        {
            Logger logger = new(name);
            logger.Warning(message, args);
        }

        public static void Error(string message, string name)
        {
            Logger logger = new(name);
            logger.Error(message);
        }

        public static void Error(string message, string name, params object[] args)
        {
            Logger logger = new(name);
            logger.Error(message, args);
        }

        public static void Debug(string message, string name)
        {
            Logger logger = new(name);
            logger.Debug(message);
        }

        public static void Debug(string message, string name, params object[] args)
        {
            Logger logger = new(name);
            logger.Debug(message, args);
        }

        public static void Exception(Exception ex, string name)
        {
            Logger logger = new(name);
            logger.Exception(ex);
        }

        public static void Crash(Exception ex, CrashBehaviour behaviour, string name)
        {
            Logger logger = new(name);
            logger.Crash(ex, behaviour);
        }

        public static void Table(string[] entries, bool vetical, string name)
        {
            Logger logger = new(name);
            logger.Table(entries, vetical);
        }

        public static void Table(string[] entries, string name)
        {
            Logger logger = new(name);
            logger.Table(entries);
        }

        public static void Table(Dictionary<string, string> entries, string name)
        {
            Logger logger = new(name);
            logger.Table(entries);
        }

        public static void Table(string[] entries, bool vetical, string name, params string[] args)
        {
            Logger logger = new(name);
            // add args to entries
            string[] entriesWithArgs = new string[entries.Length + args.Length];
            entries.CopyTo(entriesWithArgs, 0);
            args.CopyTo(entriesWithArgs, entries.Length);
            logger.Table(entriesWithArgs, vetical);
        }

        public static void Table(string[] entries, string name, params string[] args)
        {
            Logger logger = new(name);
            // add args to entries
            string[] entriesWithArgs = new string[entries.Length + args.Length];
            entries.CopyTo(entriesWithArgs, 0);
            args.CopyTo(entriesWithArgs, entries.Length);
            logger.Table(entriesWithArgs);
        }

        public static void Table(Dictionary<string, string> entries, string name, params object[] args)
        {
            Logger logger = new(name);
            // add args to entries
            Dictionary<string, string> entriesWithArgs = new(entries);
            foreach (string arg in args)
            {
                entriesWithArgs.Add(arg.ToString(), arg.ToString());
            }
            logger.Table(entriesWithArgs);
        }
        public static void Success(string message, string name)
        {
            Logger logger = new(name);
            logger.Success(message);
        }

        public static void Success(string message, string name, params object[] args)
        {
            Logger logger = new(name);
            logger.Success(message, args);
        }
    }

    public enum CrashBehaviour
    {
        Log = 0,
        Restart,
        Exit,
        Severe,
    }
}
