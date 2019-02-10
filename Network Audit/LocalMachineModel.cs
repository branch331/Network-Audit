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
        /// <summary>
        /// Obtains IP address and connection information for the local machine.
        /// </summary>
        public LocalMachineModel()
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
}
