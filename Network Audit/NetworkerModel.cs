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
        //private IEnumerable<NetworkerViewModel> networkResources;
        private List<NetworkerViewModel> allNetworkResources;
        private bool canBeginNetworkAudit;
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
         
        public void StartAudit()
        {
            AllNetworkResources = new List<NetworkerViewModel>();
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            for (int i = 2; i < 25; i++) //********** change back to 255
            {
                NetworkerViewModel myObj = new NetworkerViewModel(i);
                AllNetworkResources.Add(myObj);
                //System.Windows.MessageBox.Show(i.ToString());
            }
            CheckResourcesOnNetworkAsync(AllNetworkResources);

            System.Windows.MessageBox.Show("Total Time Elapsed: " + timer.Elapsed.ToString());
            System.Threading.Thread.Sleep(500);

            NotifyPropertyChanged("ConnectedNetworkResources");
        }

        public async void CheckResourcesOnNetworkAsync(List<NetworkerViewModel> networkResources)
        {
            var tasks = new List<Task>();

            foreach (NetworkerViewModel x in networkResources)
            {
                var task = CheckResourcesOnNetworkTask(x);
                tasks.Add(task);
                //System.Windows.MessageBox.Show("Adding task: " + x.RemoteIPAddress);
            }
            await Task
                .WhenAll(tasks)
                .ContinueWith(t => { NotifyPropertyChanged("ConnectedNetworkResources"); });
        }

        private async Task CheckResourcesOnNetworkTask(NetworkerViewModel model)
        {
            await model.CheckIsOnNetworkTask();
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
