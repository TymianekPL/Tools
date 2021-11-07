using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Tools;

namespace Tools.Diagnostic
{
    public class Debug
    {
        public TaskResult Result { get; private set; }
        public TaskSummary Summary { get; private set; }

        private bool running = true;

        public int time = 1000;

        public Debug(int time)
        {
            this.time = time;
        }

        public Debug()
        {
            time = 1000;
        }

        public Task Run()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, er) =>
            {
                var ex = (Exception)er.ExceptionObject;
                var log = new TaskLog
                {
                    Name = "Unhandled exception",
                    TaskID = TaskID.UnhandlerException,
                    Data = ex
                };
                Result += log;

                TaskUpdateEventArgs e = new TaskUpdateEventArgs
                {
                    Reason = "Unhandled exception found",
                    Log = log
                };
                Updated?.Invoke(this, e);
                if (er.IsTerminating)
                {
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.ToString());
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    Environment.Exit(1);
                }
            };
            AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
            {
                var log = new TaskLog
                {
                    Name = "Exception",
                    TaskID = TaskID.Exception,
                    Data = eventArgs.Exception
                };
                Result += log;

                TaskUpdateEventArgs e = new TaskUpdateEventArgs
                {
                    Reason = "Exception found",
                    Log = log
                };
                Updated?.Invoke(this, e);
            };
            Summary = new TaskSummary();
            Result = new TaskResult();
            return Task.Run(() =>
            {
                while (running)
                {
                    PCResource resource = new PCResource
                    {
                        Memory = 100
                    };
                    DeviceState deviceState = new DeviceState
                    {
                        Name = Environment.MachineName,
                        WindowsVersion = Environment.OSVersion.Version,
                        WindowsPlatform = Environment.OSVersion.Platform,
                        WindowsServicePack = Environment.OSVersion.ServicePack
                    };
                    UpdateDiagnostic diagnostic = new UpdateDiagnostic
                    {
                        Resource = resource,
                        WindowsDeviceState = deviceState
                    };

                    TaskLog log = new TaskLog
                    {
                        Name = "Auto check update",
                        TaskID = TaskID.Auto,
                        Data = diagnostic
                    };
                    Result += log;

                    TaskUpdateEventArgs e = new TaskUpdateEventArgs
                    {
                        Reason = "Automatic time update",
                        Log = log
                    };
                    Updated?.Invoke(this, e);
                    Thread.Sleep(time);
                }
            });
        }

        public void Stop()
        {
            running = false;
            Summary = new TaskSummary
            {
                ResultCount = Result.Lenght,
                Result = Result
            };
        }


        public delegate void DebugHandle(object sender, TaskUpdateEventArgs e);

        public event DebugHandle Updated;
    }

    public class DeviceState
    {
        public string Name { get; internal set; }
        public Version WindowsVersion { get; internal set; }
        public string WindowsServicePack { get; internal set; }
        public PlatformID WindowsPlatform { get; internal set; }
    }

    public class UpdateDiagnostic
    {
        public DeviceState WindowsDeviceState { get; internal set; }
        public PCResource Resource { get; internal set; }

        public override string ToString()
        {
            return $"Windows:\n" +
                $"\tVersion: {WindowsDeviceState.WindowsPlatform}/{WindowsDeviceState.WindowsVersion}\n" +
                $"Resources:\n" +
                $"\tMemory: {Resource.Memory}";
        }
    }

    public class PCResource
    {
        public long Memory { get; internal set; }
    }

    public class TaskUpdateEventArgs : TaskEventArgs
    {
        public TaskLog Log { get; internal set; }
    }

    public class TaskEventArgs
    {
        public string Reason { get; internal set; } = string.Empty;
    }

    public enum TaskID : int
    {
        Auto,
        Exception,
        UnhandlerException
    }

    public struct TaskLog
    {
        public int ID { get; internal set; } // Automatic, I hope

        public TaskID TaskID { get; internal set; }

        public string Name { get; internal set; }

        public TaskLog(string name) : this()
        {
            Name = name;
        }

        public object Data { get; internal set; }
    }

    public class TaskResult
    {
        public int Lenght
        {
            get
            {
                return list.Length;
            }
        }

        private TaskLog[] list = { };

        public static TaskResult operator +(TaskResult task, TaskLog log)
        {
            Array.Resize(ref task.list, task.list.Length + 1);
            log.ID = task.list.Length;
            task.list[task.list.Length - 1] = log;
            return task;
        }

        public TaskLog Last()
        {
            return list[list.Length - 1];
        }
    }

    public struct TaskSummary
    {
        public int ResultCount { get; internal set; }
        public TaskResult Result { get; internal set; }
    }
}
