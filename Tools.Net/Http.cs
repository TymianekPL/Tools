using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Net
{
    public struct HttpAdress
    {
        public IPAddress IP { get; set; }
        public int PORT { get; set; }

        public override string ToString()
        {
            string IPstr = IP.ToString();
            if(IPstr == "0.0.0.0")
                IPstr = "*";
            return $"http://{IPstr}:{PORT}/";
        }
    }

    public class Http
    {
        public HttpAdress[] adresses;

        public Action<HttpAdress[]> OnStart { get; set; }

        public Http(IPAddress IP, int PORT)
        {
            Array.Resize(ref adresses, 1);
            adresses[0].PORT = PORT;
            adresses[0].IP = IP;
        }

        public Http(HttpAdress adress)
        {
            Array.Resize(ref adresses, 1);
            adresses[0] = adress;
        }

        public Http(HttpAdress[] adresses)
        {
            this.adresses = adresses;
        }

        private bool running = true;
        private HttpListener listener;

        public void Start(Action<HttpListenerResponse, HttpListenerRequest, HttpListener> callback)
        {
            if (HttpListener.IsSupported)
            {
                listener = new HttpListener();
                foreach (HttpAdress adress in adresses)
                {
                    listener.Prefixes.Add(adress.ToString());
                }
                listener.Start();
                OnStart?.Invoke(adresses);
                while (running)
                {
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    HttpListenerResponse response = context.Response;
                    Task.Run(() =>
                    {
                        callback.Invoke(response, request, listener);
                    });
                }
            }
            else
            {
                Stop();
                throw new Exception("HTTP is not supported on this device!");
            }
        }

        public void Stop()
        {
            listener.Close();
            running = false;
        }
    }
}
