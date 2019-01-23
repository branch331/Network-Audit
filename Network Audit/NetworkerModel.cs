using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Network_Audit
{
    internal class NetworkerModel : INotifyPropertyChanged
    {
        private List<NetworkerViewModel> networkResources;
        public event PropertyChangedEventHandler PropertyChanged;

        public List<NetworkerViewModel> NetworkResources //Make a public list to bind to the DataGrid ItemsSource
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
            NetworkResources = new List<NetworkerViewModel> { };
            NetworkerViewModel myobj = new NetworkerViewModel();
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
