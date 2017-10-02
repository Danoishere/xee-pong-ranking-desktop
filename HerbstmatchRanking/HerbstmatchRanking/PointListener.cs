using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
            PermitUrlAccess();

            Listener = new HttpListener();
            Listener.Prefixes.Add("http://" + GetLocalIPAddress() + ":8080/");
            Console.WriteLine("Listening on " + "http://" + GetLocalIPAddress() + ":8080/");
        }

        private void PermitUrlAccess()
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("netsh http add urlacl url=http://" + GetLocalIPAddress() + ":8080/ sddl=D:(A;;GX;;;S-1-1-0) listen=yes");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
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
