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
using System.Runtime.CompilerServices;
using Tools.Net;
using System.Net;
using System.IO;

namespace Test_of_lib
{
    class Program
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void Main(string[] args)
        {
            Debug debug = new Debug(500);
            debug.Run();
            debug.Updated += Debug_Updated;
            try
            {

                Http http = new Http(IPAddress.Parse("127.0.0.1"), 8080);
                http.Start((res, req, listener) =>
                {
                    string filename = $"../../src{req.RawUrl}";
                    byte[] bytes = { };
                    try
                    {
                        bytes = File.ReadAllBytes(filename);
                        if(filename.EndsWith(".html"))
                            res.ContentType = "text/html";
                        else if(filename.EndsWith(".css"))
                            res.ContentType = "text/css";
                        else if(filename.EndsWith(".js"))
                            res.ContentType = "text/javascript";
                    }
                    catch (FileNotFoundException)
                    {
                        res.ContentType = "text/html";
                        res.StatusCode = 404;
                        bytes = Encoding.UTF8.GetBytes("<!DOCTYPE html><html><head><title>Not found</title></head><body><h1>404 not found</h1><h4>Requested document was not found</h4></body></html>");
                    }
                    res.OutputStream.Write(bytes, 0, bytes.Length);
                    res.ContentEncoding = Encoding.UTF8;
                    res.OutputStream.Close();
                });
            }
            catch
            {

            }
            Console.ReadLine();
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
