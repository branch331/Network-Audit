using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Threading;

namespace Network_Audit
{
    internal class LocalMachineModel
    {
        public LocalMachineModel()
        {
            LocalIPAddress = ObtainIPAddress();
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

        public string LocalIPAddress
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

            //Connected = NetworkInterface.GetIsNetworkAvailable();
            //InternetSpeed = CalculateInternetSpeed(); // Method doesn't seem proper

            Connected = true;
            InternetSpeed = 555;
            PingResponseTime = 0;
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
        
        public double CalculateInternetSpeed()
        {
            double responseTimeSum = 0; //Sum of all internet speed responses in ms
            int packetSize = 10; 

            for (int i = 0; i < 4; i++)
            {
                responseTimeSum += PingAddress("www.google.com", packetSize).Item2;              
            }  
            return 1000*packetSize / (responseTimeSum/4); //return the average internet speed
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

        public bool Connected
        {
            get;
            private set;
        }

        public double InternetSpeed
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
