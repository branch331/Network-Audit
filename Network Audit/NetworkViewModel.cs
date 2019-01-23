using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network_Audit
{
    internal class NetworkViewModel
    {
        public NetworkViewModel(string a)
        {
            IP_Address = a;
            Connected = a;
            InternetSpeed = a;
            Num_devices = a;
        }

        public string IP_Address
        {
            get;
            private set;
        }

        public string Connected
        {
            get;
            private set;
        }

        public string InternetSpeed
        {
            get;
            private set;
        }

        public string Num_devices
        {
            get;
            private set;
        }
    }
}
