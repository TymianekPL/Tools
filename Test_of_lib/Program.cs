using System;
using System.Collections.Generic;
using Tools;
using Tools.Window;
using Tools.Window.Components;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Math = Tools.Math;
using Debug = Tools.Diagnostic.Debug;
using System.Threading;
using Tools.Diagnostic;

namespace Test_of_lib
{
    class Program
    {
        static void Main(string[] args)
        {
            Debug debug = new Debug(500);
            debug.Run();
            debug.Updated += Debug_Updated;
            Thread.Sleep(1000);
            debug.time = 200;
            Thread.Sleep(1000);
            debug.Stop();
            var data = (UpdateDiagnostic)debug.Result.Last().Data;
            Console.WriteLine($"{data.WindowsDeviceState.WindowsPlatform}/{data.WindowsDeviceState.WindowsServicePack}/{data.WindowsDeviceState.WindowsVersion}/{data.WindowsDeviceState.Name}");
            TestWindow window = new TestWindow
            {
                Width = 1000,
                Height = 400,
                X = 500,
                Y = 200,
                Style = WindowStyle.Normal,
                Title = "Hello, world!"
            };
            window.Show();
        }

        private static void Debug_Updated(object sender, TaskUpdateEventArgs e)
        {
            if (e.Log.TaskID == TaskID.Exception || e.Log.TaskID == TaskID.UnhandlerException)
            {
                Exception ex = (Exception)e.Log.Data;
                Console.WriteLine($"[{e.Log.ID}] - Exception found: {ex.Message}\nClass: {ex.GetType().Name}");
            }
        }
    }
}
