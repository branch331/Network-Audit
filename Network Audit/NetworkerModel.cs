﻿using System;
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
        public event PropertyChangedEventHandler PropertyChanged;

        private List<NetworkerViewModel> allNetworkResources;

        private bool canBeginNetworkAudit = true;
        private string connectedColor;
        private string internetSpeed;
        private int deviceCount;
        private int scanProgress;
        private double scansRemaining;

        public NetworkerModel()
        {
            CanBeginNetworkAudit = true;
        }

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

        public string InternetSpeed
        {
            get { return internetSpeed; }
            set
            {
                if (internetSpeed != value)
                {
                    internetSpeed = value;
                    NotifyPropertyChanged("InternetSpeed");
                }
            }
        }

        public int ScanProgress
        {
            get { return scanProgress; }
            set
            {
                if (scanProgress != value)
                {
                    scanProgress = value;
                    NotifyPropertyChanged("ScanProgress");
                }
            }
        }

        public int DeviceCount
        {
            get { return deviceCount; }
            set
            {
                if (deviceCount != value)
                {
                    deviceCount = value;
                }
            }
        }

        public async void StartAudit()
        {
            AllNetworkResources = new List<NetworkerViewModel>();
            string localIPAddress = "";
            bool connected = false;

            deviceCount = 0;
            scanProgress = 0;
            scansRemaining = 255;

            CanBeginNetworkAudit = false;

            LocalMachineViewModel localobj = new LocalMachineViewModel();
            localIPAddress = localobj.LocalIPAddress;
            connected = localobj.Connected;
            InternetSpeed = localobj.InternetSpeed;

            CheckConnectionColor(connected);

            if (connected)
            {
                for (int i = 0; i < 255; i++) 
                {
                    NetworkerViewModel myObj = new NetworkerViewModel(localIPAddress, i);
                    AllNetworkResources.Add(myObj);
                }

                await FindResourceHostNamesAsync(AllNetworkResources);
            }
            else
            {
                MessageBox.Show("Unable to scan. Network is unavailable.");
            }

            CanBeginNetworkAudit = true; 
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

        //Asynchronously iterate through network resources to find if they are connected, and their hostnames
        private async Task FindResourceHostNamesAsync(List<NetworkerViewModel> networkResources) 
        {
            var tasks = new List<Task>();

            foreach (NetworkerViewModel x in networkResources)
            {
                Task task = FindResourceHostNamesTask(x);
                tasks.Add(task);
            }

            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            await Task
               .WhenAll(tasks)
               .ContinueWith(t => 
               {
                   NotifyPropertyChanged("ConnectedNetworkResources");
                   NotifyPropertyChanged("DeviceCount");
               });

            MessageBox.Show("Scan Complete!\nTime Elapsed: " + timer.Elapsed.ToString("s\\.fff") + " s\nDevices Found: " + deviceCount.ToString());
        }

        private async Task FindResourceHostNamesTask(NetworkerViewModel model)
        {
            await model.CheckIsOnNetworkTask();

            if (model.IsOnNetwork)
            {
                await model.GetHostNameAsync();
                deviceCount++;
            }

            scansRemaining -= 1;
            ScanProgress = Convert.ToInt32(Math.Round(((255 - scansRemaining) / 255) * 100));
        }
        
        private async Task GetResourceHostNameTask(NetworkerViewModel model)
        {
            if (model.IsOnNetwork)
            {
                await model.GetHostNameAsync();
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
