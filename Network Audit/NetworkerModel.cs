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
            for (int i = 2; i < 255; i++)
            {
                NetworkerViewModel myObj = new NetworkerViewModel(i);
                AllNetworkResources.Add(myObj); //makes linq operation ("where isconnected") redundant
            }
            System.Windows.MessageBox.Show("Total Time Elapsed: " + timer.Elapsed.ToString());
            System.Threading.Thread.Sleep(500);

            //ConnectedNetworkResources = AllNetworkResources
              //  .Where(x => x.IsOnNetwork == true);

            NotifyPropertyChanged("ConnectedNetworkResources");
            
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
