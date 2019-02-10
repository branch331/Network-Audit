using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Threading;

namespace Network_Audit
{
    internal class RemoteMachineModel
    {
        /// <summary>
        /// Contains network information for other potential hosts on the network.
        /// </summary>
        /// <param name="localIPAddress">IP address of the local machine, used for ping operations to detect the potential host.</param>
        /// <param name="ipIteration">Value from 0 - 255 to replace the last subnet of the local IP address; used for ping operations.</param>

        public RemoteMachineModel(string localIPAddress, int ipIteration)
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

            if (reply.Status == IPStatus.Success)
            {
                IsOnNetwork = true;
                PingResponseTime = reply.RoundtripTime;
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
