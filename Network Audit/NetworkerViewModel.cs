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
        /// <summary>
        /// Obtains IP address and connection information for the local machine.
        /// </summary>
        public LocalMachineViewModel()
        {
            LocalIPAddress = ObtainIPAddress();
            if (Connected = NetworkInterface.GetIsNetworkAvailable())
            {
                InternetSpeed = CalculateInternetSpeed().ToString("0.000");
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
        /// <summary>
        /// Contains network information for other potential hosts on the network.
        /// </summary>
        /// <param name="localIPAddress">IP address of the local machine, used for ping operations to detect the potential host.</param>
        /// <param name="ipIteration">Value from 0 - 255 to replace the last subnet of the local IP address; used for ping operations.</param>
        
        static object lockObj = new object();

        public NetworkerViewModel(string localIPAddress, int ipIteration)
        {
            IsOnNetwork = false;
            RemoteIPAddress = GetRemoteIP(localIPAddress, ipIteration);
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

            var reply = await pinger.SendPingAsync(RemoteIPAddress, 5000);

            lock (lockObj)
            {
                if (reply.Status == IPStatus.Success)
                {
                    IsOnNetwork = true;
                    PingResponseTime = reply.RoundtripTime;
                }
            }
        }

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
