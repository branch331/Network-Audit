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
        //private IEnumerable<NetworkerViewModel> networkResources;
        private List<NetworkerViewModel> allNetworkResources;
        private bool canBeginNetworkAudit;
        private string localIPAddress;
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

            LocalMachineModel localobj = new LocalMachineModel();
            localIPAddress = localobj.LocalIPAddress;

            System.Diagnostics.Stopwatch timer2 = new System.Diagnostics.Stopwatch();
            timer2.Start();
            for (int i = 2; i < 255; i++)
            {
                NetworkerViewModel myObj = new NetworkerViewModel(localIPAddress, i);
                //string constructortime = timer2.Elapsed.ToString();
                //System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
                AllNetworkResources.Add(myObj);
                //System.Windows.MessageBox.Show(i.ToString());
            }
            timer2.Start();

            CheckResourcesOnNetworkAsync(AllNetworkResources);

            foreach (NetworkerViewModel x in ConnectedNetworkResources) //***not running
            {
                x.GetHostName();
                MessageBox.Show(x.HostName);
            }

            NotifyPropertyChanged("ConnectedNetworkResources");

            //System.Windows.MessageBox.Show("Total Time Elapsed: " + timer2.Elapsed.ToString());
        }

        public async void CheckResourcesOnNetworkAsync(List<NetworkerViewModel> networkResources)
        {
            var tasks = new List<Task>();

            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            foreach (NetworkerViewModel x in networkResources)
            {
                var task = CheckResourcesOnNetworkTask(x);
                tasks.Add(task);
            }
            //System.Windows.MessageBox.Show("Time for adding to task list: " + timer.Elapsed.ToString());
            await Task
               .WhenAll(tasks)
               .ContinueWith(t => { NotifyPropertyChanged("ConnectedNetworkResources"); });

            //foreach(NetworkerViewModel y in ConnectedNetworkResources)
            //{
            //    y.GetHostName();
            //}

            NotifyPropertyChanged("ConnectedNetworkResources");
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
