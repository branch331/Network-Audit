﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Network_Audit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NetworkerModel obj = new NetworkerModel();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = obj;
        }

        private void RunButton(object sender, RoutedEventArgs e)
        {
            obj.StartAudit();
        }

        private void TestDevices(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Total # Devices: " + obj.NetworkResources[0].NumDevices.ToString());
        }
    }
}
