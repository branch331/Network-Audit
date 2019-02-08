using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Threading;

namespace Network_Audit
{
    internal class LocalMachineViewModel
    {
        public LocalMachineViewModel()
        {
            LocalIPAddress = ObtainIPAddress();
            if (Connected = NetworkInterface.GetIsNetworkAvailable())
            {
                InternetSpeed = CalculateInternetSpeed().ToString("0.000");
                //CalculateInternetSpeedAsync();
            }
            else
            {
                InternetSpeed = "N/A";
            }
        }

        public string ObtainIPAddress()
        {
            string ip_Address = "";

            foreach (NetworkInterface x in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (x.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (GatewayIPAddressInformation y in x.GetIPProperties().GatewayAddresses)
                    {
                        ip_Address = y.Address.ToString();
                    }
                }
            }

            return ip_Address;
        }

        public double CalculateInternetSpeed()
        {
            System.Net.WebClient webclient = new System.Net.WebClient();

            DateTime t1 = DateTime.Now;
            byte[] data = webclient.DownloadData("http://www.google.com");
            DateTime t2 = DateTime.Now;

            return ((data.Length / 1024) / (t2 - t1).TotalSeconds); //Convert to kB/s
        }

        public async void CalculateInternetSpeedAsync()
        {
            await CalculateInternetSpeedTask();
        }

        public Task CalculateInternetSpeedTask()
        {
            return Task.Run(() =>
            {
                System.Net.WebClient webclient = new System.Net.WebClient();

                DateTime t1 = DateTime.Now;
                byte[] data = webclient.DownloadData("http://www.google.com");
                DateTime t2 = DateTime.Now;

                InternetSpeed = ((data.Length / 1024) / ((t2 - t1).TotalSeconds)).ToString("0.000");
        });
        }

        

        public string LocalIPAddress
        {
            get;
            private set;
        }

        public bool Connected
        {
            get;
            private set;
        }

        public string InternetSpeed
        {
            get;
            private set;
        }
    }

    internal class NetworkerViewModel
    {
        public NetworkerViewModel(string localIPAddress, int ipIteration)
        {
            IsOnNetwork = false;
            //LocalIPAddress = ObtainIPAddress();
            RemoteIPAddress = GetRemoteIP(localIPAddress, ipIteration);
            //InternetSpeed = CalculateInternetSpeed(); // Method doesn't seem proper
            PingResponseTime = 0;
            HostName = "test";
        }

        public async void CheckIsOnNetworkAsync()
        {
            await CheckIsOnNetworkTask();
        }

        public async Task CheckIsOnNetworkTask()
        {
            Ping pinger = new Ping();

            var reply = await pinger.SendPingAsync(RemoteIPAddress, 200);

            lock(lockObj)
            {
                if (reply.Status == IPStatus.Success)
                {
                    IsOnNetwork = true;
                    PingResponseTime = reply.RoundtripTime;
                }
            }
        }

        public Tuple<bool, double> PingAddress(string address, int packetSize) //return if address is pingable, and the time (ms) to receive a response
        {
            bool pingSuccess = false;
            double responseTime = 0;
            byte[] packet = new byte[packetSize]; 

            Ping pinger = new Ping();

            try
            {
                PingReply reply = pinger.Send(address, 50, packet); //Ping address with 15 ms timeout
                if (reply.Status == IPStatus.Success)
                {
                    pingSuccess = true;
                    responseTime = reply.RoundtripTime; 
                }
            }
            catch (PingException) 
            {

            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return Tuple.Create(pingSuccess, responseTime);
        }

        static object lockObj = new object();

        public string GetRemoteIP(string localIPAddress, int ipIteration)
        {
            string[] split_IP = localIPAddress.Split('.');
            return split_IP[0] + "." + split_IP[1] + "." + split_IP[2] + "." + ipIteration;
        }

        public void GetHostName()
        {
            try
            {
                HostName = System.Net.Dns.GetHostEntry(RemoteIPAddress).HostName;
            }
            catch (System.Net.Sockets.SocketException)
            {
                HostName = "Unavailable";
            }
        }

        public async Task GetHostNameAsync()
        {
            HostName = await GetHostNameTask();
        }

        public async Task<string> GetHostNameTask()
        {
            string hostName = "";
            await Task.Run(() =>
            { 
                try
                {
                    hostName = System.Net.Dns.GetHostEntry(RemoteIPAddress).HostName;
                }
                catch (System.Net.Sockets.SocketException)
                {
                    hostName = "Unavailable";
                }
            });

            return hostName;
        }

        public bool IsOnNetwork
        {
            get;
            private set;
        }

        public double PingResponseTime
        {
            get;
            private set;
        }

        public string RemoteIPAddress
        {
            get;
            private set;
        }

        public string HostName
        {
            get;
            private set;
        }

        public int PingCounter
        {
            get;
            private set;
        }
    }
}
