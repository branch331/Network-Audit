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
        public NetworkerViewModel()
        {
            IP_Address = ObtainIPAddress();
            Connected = NetworkInterface.GetIsNetworkAvailable();
            InternetSpeed = CalculateInternetSpeed(); // Method doesn't seem proper
            CalculateNumberDevicesAsync(IP_Address);
            //System.Windows.MessageBox.Show(NumDevices.ToString());
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
        
        public void PingAddressAsync(string address, int packetSize)
        {
            byte[] packet = new byte[packetSize];
            AutoResetEvent waiter = new AutoResetEvent(false);

            Ping pinger = new Ping();
            pinger.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

            PingOptions options = new PingOptions(64, true);

            pinger.SendAsync(address, 100, packet, options, waiter);
        }

        private void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            PingReply reply = e.Reply;

            PingCounter += 1;

            if (reply.Status == IPStatus.Success)
            {
                NumDevices += 1;
            } 

            ((AutoResetEvent)e.UserState).Set();
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
        
        public void CalculateNumberDevices(string ip_Address)
        {
            //bool pingable;
            string[] split_IP = ip_Address.Split('.');

            for (int i = 1; i < 255; i++)
            {
                //Assumes subnet mask of 255.255.255.0
                string addressToPing = split_IP[0] + "." + split_IP[1] + "." + split_IP[2] + "." + i;
                PingAddressAsync(addressToPing, 1);
            }
        }

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

        public string IP_Address
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

        public int NumDevices
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
