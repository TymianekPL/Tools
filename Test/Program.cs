using Tools;
using Tools.Diagnostic;
using Tools.Window;
using System;
using Tools.Net.Extensions.WebSockets;
using Tools.Net;

class Program
{
    static void Main(string[] args)
    {
        Display display = new();
        Http http = new(System.Net.IPAddress.Any, 10);
        Debug debug = new();
        Window window = new();
        WssListener wss = new(new());
    }
}