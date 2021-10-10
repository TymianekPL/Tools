using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Tools;
using Tools.Net;

class Program
{
    [DebuggerNonUserCode]
    static void Main(string[] args)
    {
        HttpAdress[] addresses = new HttpAdress[]
        {
            new HttpAdress()
            {
                IP = IPAddress.Any,
                PORT = 80
            },
        };
        int lenght = 0;
        foreach (string arg in args)
        {
            switch (arg)
            {
                case "--port":
                    if (args.Length > lenght + 1)
                    {
                        if (int.TryParse(args[lenght + 1], out int port))
                        {
                            Array.Resize(ref addresses, addresses.Length + 1);
                            addresses[addresses.Length - 1] = new HttpAdress()
                            {
                                IP = IPAddress.Any,
                                PORT = port
                            };
                        }
                        else
                        {
                            Console.WriteLine($"'{args[lenght + 1]}' is not valid number!\nusage: --port <number>");
                            Console.Write("Press any key to continue...");
                            Console.ReadKey();
                            Environment.Exit(1);
                        }
                    }
                    else
                    {
                        Console.WriteLine("usage: --port <number>");
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                        Environment.Exit(1);
                    }
                    break;
            }
            lenght++;
        }
        while (true)
        {
            Http http = new(addresses);
            http.OnStart = (adresses) =>
            {
                if (adresses.Length > 1)
                {
                    Console.Write("Server started on ");
                    int num = 0;
                    foreach (HttpAdress adress in adresses)
                    {
                        if (num >= adresses.Length - 1)
                        {
                            Console.WriteLine($"and {adress}");
                        }
                        else if (num >= adresses.Length - 2)
                        {
                            Console.Write($"{adress} ");
                        }
                        else
                        {
                            Console.Write($"{adress}, ");
                        }
                        num++;
                    }
                }
                else
                {
                    Console.WriteLine($"Server started on {adresses[0]}");
                }
            };
            http.Start((res, req, listener) =>
            {
                ServerConfig config = new();
                res.StatusCode = (int)HttpStatusCode.OK;
                res.ContentType = "application/json";
                byte[] response;
                string filename = $@"..\..\src{req.Url.AbsolutePath}".Replace("/", "\\");
                if (File.Exists(@"..\..\src\.serverconfig"))
                {
                    string[] lines = File.ReadAllLines(@"..\..\src\.serverconfig");
                    foreach (string line in lines)
                    {
                        string[] words = line.Split(':');
                        string name = words[0];
                        string value = words[1];
                        switch (name)
                        {
                            case "header":
                                if (File.Exists(@"..\..\src\" + value.Replace("/", "\\")))
                                    config.Header = value;
                                break;
                            case "footer":
                                if (File.Exists(@"..\..\src\" + value.Replace("/", "\\")))
                                    config.Footer = value;
                                break;
                            case "404":
                                if (File.Exists(@"..\..\src\" + value.Replace("/", "\\")))
                                    config.NotFound = value;
                                break;
                        }
                    }
                }
                try
                {
                    string text = File.ReadAllText(filename);
                    if (config.Header != null)
                        text = text.Replace("{{header}}", File.ReadAllText(@"..\..\src\" + config.Header.Replace("/", "\\")));
                    if (config.Footer != null)
                        text = text.Replace("{{footer}}", File.ReadAllText(@"..\..\src\" + config.Footer.Replace("/", "\\")));
                    response = Encoding.UTF8.GetBytes(text);
                    res.ContentType = MimeMapping.GetMimeMapping(filename);
                }
                catch (UnauthorizedAccessException e)
                {
                    res.StatusCode = (int)HttpStatusCode.Forbidden;
                    response = Encoding.UTF8.GetBytes("{\"error\":403,\"message\":\"" + e.Message + "\"}");
                }
                catch (Exception e)
                {
                    if (e is FileNotFoundException || e is DirectoryNotFoundException)
                    {
                        if (filename.EndsWith("\\") && File.Exists($"{filename}index.html"))
                        {
                            string text = File.ReadAllText($"{filename}index.html");
                            if (config.Header != null)
                                text = text.Replace("{{header}}", File.ReadAllText(@"..\..\src\" + config.Header.Replace("/", "\\")));
                            if (config.Footer != null)
                                text = text.Replace("{{footer}}", File.ReadAllText(@"..\..\src\" + config.Footer.Replace("/", "\\")));
                            response = Encoding.UTF8.GetBytes(text);
                            res.ContentType = MimeMapping.GetMimeMapping($"{filename}index.html");
                        }
                        else if (File.Exists($"{filename}\\index.html"))
                        {
                            string text = File.ReadAllText($"{filename}\\index.html");
                            if (config.Header != null)
                                text = text.Replace("{{header}}", File.ReadAllText(@"..\..\src\" + config.Header.Replace("/", "\\")));
                            if (config.Footer != null)
                                text = text.Replace("{{footer}}", File.ReadAllText(@"..\..\src\" + config.Footer.Replace("/", "\\")));
                            response = Encoding.UTF8.GetBytes(text);
                            res.ContentType = MimeMapping.GetMimeMapping($"{filename}\\index.html");
                        }
                        else
                        {
                            if (config.NotFound == null)
                            {
                                res.StatusCode = (int)HttpStatusCode.NotFound;
                                response = Encoding.UTF8.GetBytes("{\"error\":404,\"message\":\"File '" + req.Url.AbsolutePath + "' was not found on this server\"}");
                            }
                            else
                            {
                                res.StatusCode = (int)HttpStatusCode.NotFound;
                                response = File.ReadAllBytes(@"..\..\src\" + config.NotFound.Replace("/", "\\"));
                                res.ContentType = MimeMapping.GetMimeMapping(@"..\..\src\" + config.NotFound.Replace("/", "\\"));
                            }
                        }
                    }
                    else
                    {
                        res.StatusCode = (int)HttpStatusCode.InternalServerError;
                        response = Encoding.UTF8.GetBytes($@"{{""error"":500,""message"":""{e.Message}"",""class"":""{e.GetType()}""}}");
                    }
                }
                res.ContentLength64 = response.Length;
                res.OutputStream.Write(response, 0, response.Length);
                res.Close();
            });
            Console.WriteLine("Server crashed/stopped. Restarting!");
        }
    }
    public struct ServerConfig
    {
        public string NotFound { get; internal set; }
        public string Header { get; internal set; }
        public string Footer { get; internal set; }
    }
}