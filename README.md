# Network-Audit
This application performs an asynchronous IP ping sweep and returns network information using C#/WPF and the System.Net Dns class. 

![Alt text](demo.gif)

First, a network connection is confirmed and internet speed determined, then the table is populated with devices found on the network.

### NetworkViewModel.cs
The viewmodel for the application; this first instantiates an object for the LocalMachineModel class, and then asynchronously iterates through 
all IP addresses on its subnet using the RemoteMachineModel class to identify if the device is actually on the network.

### LocalMachineModel.cs
This class contains information for the local machine such as the local IP address, if a network is available, and internet speed.

### RemoteMachineModel.cs
This class contains information for any remote device on the same network as the local machine. It will have its own remote IP address,
and we will test if the device is on the network (with asynchronous pings), and obtain its hostname if so.


