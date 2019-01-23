using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Network_Audit
{
    internal class Networker : INotifyPropertyChanged
    {
        private List<NetworkViewModel> networkResources;
        public event PropertyChangedEventHandler PropertyChanged;

        public List<NetworkViewModel> NetworkResources
        {
            get { return networkResources; }
            set
            {
                if (networkResources != value)
                {
                    networkResources = value;
                    NotifyPropertyChanged("NetworkResources");
                }
            }
        }

        public void StartAudit()
        {
            NetworkResources = new List<NetworkViewModel> { };
            for (int i = 0; i < 3; i++)
            {
                NetworkViewModel myobj = new NetworkViewModel(i.ToString());
                NetworkResources.Add(myobj);
                
                //System.Windows.MessageBox.Show(myobj.IP_Address);    
            }
        }

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
