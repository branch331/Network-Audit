using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace Network_Audit
{
    internal class NetworkerViewModel
    {
        public NetworkerViewModel()
        {
            IP_Address = ObtainIPAddress();
            Connected = NetworkInterface.GetIsNetworkAvailable();
            InternetSpeed = 5;
            Num_devices = NetworkInterface.GetAllNetworkInterfaces().Length; //Not the correct call
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

        public double CalculateInternetSpeed()
        {
            double internetSpeed = 5;
            return internetSpeed;
        }

        public double CalculateNumberDevices()
        {

            double num_devices = 5;
            return num_devices;
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

        public double Num_devices
        {
            get;
            private set;
        }
    }
}
