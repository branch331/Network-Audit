using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace Network_Audit
{
    internal class NetworkerModel : INotifyPropertyChanged
    {
        private List<NetworkerViewModel> allNetworkResources;
        private bool canBeginNetworkAudit;
        private string localIPAddress;
        private string connectedColor;
        public event PropertyChangedEventHandler PropertyChanged;
        static object lockObj = new object();

        public IEnumerable<NetworkerViewModel> ConnectedNetworkResources //Make a public list to bind to the DataGrid ItemsSource
        {
            get
            {
                if (AllNetworkResources == null)
                {
                    return Enumerable.Empty<NetworkerViewModel>();
                }
                return AllNetworkResources
                    .Where(x => x.IsOnNetwork == true);
            }
        }

        private List<NetworkerViewModel> AllNetworkResources
        {
            get { return allNetworkResources; }
            set
            {
                if (allNetworkResources != value)
                {
                    allNetworkResources = value;
                    NotifyPropertyChanged("ConnectedNetworkResources");
                }
            }

        }

        public bool CanBeginNetworkAudit
        {
            get { return canBeginNetworkAudit; }
            set
            {
                if (canBeginNetworkAudit != value)
                {
                    canBeginNetworkAudit = value;
                    NotifyPropertyChanged("CanBeginNetworkAudit");
                }
            }
        }

        public string ConnectedColor
        {
            get { return connectedColor; }
            set
            {
                if (connectedColor != value)
                {
                    connectedColor = value;
                    NotifyPropertyChanged("ConnectedColor");
                }
            }
        }

        public void StartAudit()
        {
            AllNetworkResources = new List<NetworkerViewModel>();
            bool connected;

            LocalMachineModel localobj = new LocalMachineModel();
            localIPAddress = localobj.LocalIPAddress;
            connected = localobj.Connected;

            CheckConnectionColor(connected);

            if (connected)
            {
                for (int i = 0; i < 255; i++) 
                {
                    NetworkerViewModel myObj = new NetworkerViewModel(localIPAddress, i);
                    AllNetworkResources.Add(myObj);
                }

                CheckResourcesOnNetworkAsync(AllNetworkResources);
                MessageBox.Show("Scan Complete!");
            }
            else
            {
                MessageBox.Show("Unable to scan. Network is unavailable.");
            }
        }

        public void CheckConnectionColor(bool connected)
        {
            if (connected)
            {
                ConnectedColor = "#40FF00"; //green
            }
            else
            {
                ConnectedColor = "#FF0000"; //red
            }
        }

        public async void CheckResourcesOnNetworkAsync(List<NetworkerViewModel> networkResources)
        {
            var tasks = new List<Task>();

            foreach (NetworkerViewModel x in networkResources)
            {
                var task = CheckResourcesOnNetworkTask(x);
                tasks.Add(task);
            }

            await Task
               .WhenAll(tasks)
               .ContinueWith(t => { NotifyPropertyChanged("ConnectedNetworkResources"); });

            NotifyPropertyChanged("ConnectedNetworkResources");
    }

        private async Task CheckResourcesOnNetworkTask(NetworkerViewModel model)
        {
            await model.CheckIsOnNetworkTask();
            if (model.IsOnNetwork)
            {
                model.GetHostName();
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
