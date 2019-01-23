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

        public List<NetworkViewModel> NetworkResources //Make a public list to bind to the DataGrid ItemsSource
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
            NetworkViewModel myobj = new NetworkViewModel();
            NetworkResources.Add(myobj);
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
