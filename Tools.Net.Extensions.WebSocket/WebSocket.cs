using System.Net;
using System.Net.Sockets;

namespace Tools.Net.Extensions.WebSockets
{
    public struct WssAdress
    {
        public IPAddress IP { get; set; }
        public int PORT { get; set; }
        public WssAdress()
        {
            IP = IPAddress.Any;
            PORT = 80;
        }
        public WssAdress(IPAddress iP)
        {
            IP = iP;
            PORT = 80;
        }
        public WssAdress(int PORT)
        {
            this.PORT = PORT;
            IP = IPAddress.Any;
        }
        public WssAdress(IPAddress iP, int PORT)
        {
            IP = iP;
            this.PORT = PORT;
        }
    }

    public class WssListener
    {
        public WssAdress wss;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public WssListener(WssAdress wss)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            listener = new TcpListener(wss.IP, wss.PORT);
            this.wss = wss;
        }


        private readonly TcpListener listener;
        private Action<Socket> callback;

        private readonly bool running = true;

        public void Start(Action<Socket> callback)
        {
            listener.Start();
            _ = listener.BeginAcceptTcpClient(BeginAcceptTcpClientCallback, null);
            this.callback = callback;
            while (running)
            {
            }
        }

        private void BeginAcceptTcpClientCallback(IAsyncResult result)
        {
            Socket socket = listener.EndAcceptSocket(result);
            object? x = result.AsyncState;
            _ = listener.BeginAcceptTcpClient(BeginAcceptTcpClientCallback, null);
            callback.Invoke(socket);
        }
    }
}
