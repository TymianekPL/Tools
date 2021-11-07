using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Tools.Net
{
    public struct WssAdress
    {
        public IPAddress IP { get; set; }
        public int PORT { get; set; }
    }

    public class WssListener
    {
        public WssAdress wss;

        public WssListener(WssAdress wss)
        {
            this.wss = wss;
        }


        private TcpListener listener;
        private Action<Socket> callback;

        private readonly bool running = true;

        public void Start(Action<Socket> callback)
        {
            listener = new TcpListener(wss.IP, wss.PORT);
            listener.Start();
            listener.BeginAcceptTcpClient(BeginAcceptTcpClientCallback, null);
            this.callback = callback;
            while (running)
            {
            }
        }

        private void BeginAcceptTcpClientCallback(IAsyncResult result)
        {
            Socket socket = listener.EndAcceptSocket(result);
            listener.BeginAcceptTcpClient(BeginAcceptTcpClientCallback, null);
            callback.Invoke(socket);
        }
    }
}
