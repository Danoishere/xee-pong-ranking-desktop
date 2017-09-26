using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HerbstmatchRanking
{
    public class PointListener
    {
        public event Action<HttpListenerContext> OnRequestReceived;
        public HttpListener Listener { get; set; }

        public PointListener()
        {
            Listener = new HttpListener();
            Listener.Prefixes.Add("http://" + GetLocalIPAddress() + "/");
            Console.WriteLine("Listening on " + "http://" + GetLocalIPAddress() + "/");
        }

        public async void StartListening()
        {
            try
            {
                await ListenForRequest();
                StartListening();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }  
        }

        private async Task<HttpListenerContext> ListenForRequest()
        {
            Listener.Start();
            var context = await Listener.GetContextAsync();
            var response = context.Response;
            const string responseString = "<html><body><h1>OK</h1></body></html>";

            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;

            var output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();

            Listener.Stop();
            OnRequestReceived?.Invoke(context);

            return context;
        }

        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}
