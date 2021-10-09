using System;
using Tools;
using Tools.Diagnostic;

class Program
{
    private static bool debugMode = false;
    static void Main(string[] args)
    {
        foreach (string arg in args)
        {
            switch (arg)
            {
                case "--debug":
                    debugMode = true;
                    break;
                default:
                Console.WriteLine($"Invalid argument '{arg}'");
                Environment.Exit(1);
                break;
            }
        }
        #region Debug
        Debug debug = new(3000);
        debug.Run();
        debug.Updated += Debug_Updated;
        #endregion
        if(debugMode)
            Console.WriteLine("Debug mode enabled");
        try
        {
            TimeoutTime time = new TimeoutTime()
            {
                Hours = 789534,
                Milliseconds = 84,
                Minutes = 58,
                Seconds = 78953,
                Days = 785
            };
            Console.WriteLine(time.ToString());
            TimeSpan span = new(29,980, 10);
            time += span;
            Console.WriteLine(time.ToString());
        }
        catch
        {
        }
        if (debugMode)
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }

    #region DebugMethods
    private static void Debug_Updated(object sender, TaskUpdateEventArgs e)
    {
        if (e.Log.TaskID == TaskID.Exception || e.Log.TaskID == TaskID.UnhandlerException)
        {
            Exception ex = (Exception)e.Log.Data;
            Console.WriteLine($"[{e.Log.ID}/{ex.GetType().Name}] - {ex.Message}");
        }
        else if(debugMode && e.Log.TaskID == TaskID.Auto)
        {
            UpdateDiagnostic data = (UpdateDiagnostic)e.Log.Data;
            Console.WriteLine($"[{e.Log.ID}/{e.Log.TaskID}] - {data}");
        }
    }
    #endregion
}