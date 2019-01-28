using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Threading;

namespace Network_Audit
{
    internal class NetworkerViewModel
    {
        public NetworkerViewModel(int ipIteration)
        {
            IsOnNetwork = false;
            LocalIPAddress = ObtainIPAddress();
            RemoteIPAddress = GetRemoteIP(LocalIPAddress, ipIteration);
            Connected = NetworkInterface.GetIsNetworkAvailable();
            InternetSpeed = CalculateInternetSpeed(); // Method doesn't seem proper
            //if (IsOnNetwork = CheckIsOnNetwork2(RemoteIPAddress, ipIteration))
            //{

            //}
            CheckIsOnNetworkAsync();
            HostName = GetHostName(RemoteIPAddress);
        }

        public async void CheckIsOnNetworkAsync()
        {
            await CheckIsOnNetworkTask(RemoteIPAddress);
            HostName = GetHostName(RemoteIPAddress);
            //System.Windows.MessageBox.Show(NumDevices.ToString());
        }
        /*
public async void CalculateNumberDevicesAsync(string ipAddress)
{
    NumDevices = 0;

    var tasks = new List<Task>();

    for (int i = 1; i < 255; i++)
    {
        Ping pinger = new Ping();
        string[] split_IP = ipAddress.Split('.');
        string addressToPing = split_IP[0] + "." + split_IP[1] + "." + split_IP[2] + "." + i;
        var task = CalculateNumberDevicesTask(pinger, addressToPing);
        tasks.Add(task);
    }

    await Task.WhenAll(tasks)
        .ContinueWith(t => { System.Windows.MessageBox.Show(NumDevices.ToString()); });
}

private async Task CalculateNumberDevicesTask(Ping pinger, string ip_Address)
{
    var reply = await pinger.SendPingAsync(ip_Address, 100);

    if (reply.Status == IPStatus.Success)
    {
        lock(lockObj)
        {
            NumDevices++;
        }
    }
}


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
*/
        public async Task CheckIsOnNetworkTask(string remoteIPAddress)
        {
            Ping pinger = new Ping();

            //await PingAddress(remoteIPAddress, 1);

            var reply = await pinger.SendPingAsync(remoteIPAddress, 100);

            lock(lockObj)
            {
                if (reply.Status == IPStatus.Success)
                {
                    IsOnNetwork = true;
                }
            }
        }

        public string ObtainIPAddress()
        {
            string ip_Address = "";

            foreach (NetworkInterface x in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (x.OperationalStatus == OperationalStatus.Up)
                {
                    foreach(GatewayIPAddressInformation y in x.GetIPProperties().GatewayAddresses)
                    {
                        ip_Address = y.Address.ToString();
                    }
                }
            }

            return ip_Address;
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

        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

        public int CalculateNumberDevices2(string ipAddress) //non-asynchronous
        {
            bool pingable;
            int numDevices = 0;
            string[] split_IP = ipAddress.Split('.');

            for (int i = 1; i < 255; i++)
            {
                //Assumes subnet mask of 255.255.255.0
                string addressToPing = split_IP[0] + "." + split_IP[1] + "." + split_IP[2] + "." + i;
                pingable = PingAddress(addressToPing, 1).Item1;
                if (pingable)
                {
                    numDevices += 1;
                    //System.Windows.MessageBox.Show(i.ToString());
                }
            }

            return numDevices;
        }

        static object lockObj = new object();
        /*
        public async void CalculateNumberDevicesAsync(string ipAddress)
        {
            NumDevices = 0;

            var tasks = new List<Task>();

            for (int i = 1; i < 255; i++)
            {
                Ping pinger = new Ping();
                string[] split_IP = ipAddress.Split('.');
                string addressToPing = split_IP[0] + "." + split_IP[1] + "." + split_IP[2] + "." + i;
                var task = CalculateNumberDevicesTask(pinger, addressToPing);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks)
                .ContinueWith(t => { System.Windows.MessageBox.Show(NumDevices.ToString()); });
        }

        private async Task CalculateNumberDevicesTask(Ping pinger, string ip_Address)
        {
            var reply = await pinger.SendPingAsync(ip_Address, 100);

            if (reply.Status == IPStatus.Success)
            {
                lock(lockObj)
                {
                    NumDevices++;
                }
            }
        }
        */

        public string GetRemoteIP(string localIPAddress, int ipIteration)
        {
            string[] split_IP = localIPAddress.Split('.');
            return split_IP[0] + "." + split_IP[1] + "." + split_IP[2] + "." + ipIteration;
        }
       
        private bool CheckIsOnNetwork2(string ipAddress, int ipIteration) //Non-async method 
        {
            string[] split_IP = ipAddress.Split('.');
            //Assumes subnet mask of 255.255.255.0
            string addressToPing = split_IP[0] + "." + split_IP[1] + "." + split_IP[2] + "." + ipIteration;
            return PingAddress(addressToPing, 1).Item1;
        }

        public string GetHostName(string remoteIPAddress)
        {
            try
            {
                return System.Net.Dns.GetHostEntry(remoteIPAddress).HostName;
            }
            catch (System.Net.Sockets.SocketException)
            {
                return "Unavailable";
            }
        }

        public bool IsOnNetwork
        {
            get;
            private set;
        }

        public string LocalIPAddress
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
