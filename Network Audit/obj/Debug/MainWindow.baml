   M S B A M L   `   `   `  ����H  DNetwork Audit, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Network_AuditNetwork_Audit  5   	       Network_Audit.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:Network_Audit     5   	     � Title$  Network Audit��5   	   $��458.667��6   $��409.196��60   .��5
      � .+�5   
   �� $
��Left=�6   $��1��6/    �� Padding$ 
10,10,0,50q�6<   $��20(�6Q   $��0,0,0,-0.333q�6_   Network Audit5      �� 5   
   $
��Wrap=�6   $��2��6(   $
��Left=�65   $��10(�6P   $ 10,38,20,50q�6^   $��0,0,0,-0.333q�6t   $��3��6�   $	��395��6�   �� 6�   $G�AThis application returns network information in the fields below.��6�   6�   �� 5   
   -   5        $
��Run!��5      $
��Left=�6K   $��318,397,0,0q�6f   $	��Top=�6{   $��75��6�    J� RenderTransformOrigin$ -0.524,3.117&�6�   {�6    �� *CanBeginNetworkAudit+6�   �� 5   
   $
��Left=�6   $ 
10,10,0,50q�6/   $��10(�6D   $��2,48,0,333.333q�6R   $��73��6j   $ 
0.58,0.388&�6u   
Connected?5      A� 5   
   $
��Left=�62   $��20��6M   $��20��6Y   $��	26,72,0,0q�6d   
���   �$	��Top=�6�   $ -4.167,1.875&�6�   ��6   �� *ConnectedColor+6�   �� 5   
   $��Internet Speed��6   $ -1.375,1.864&�6)   $��10(�6N   $��146,54,159.667,345.333q�6\   �� 5   
    �� TextWrapping$
 Wrap=�6    �� 
IsReadOnly . $��10(�6W   $��155,74,189.667,329.333q�6e   $ 0.585,1.075&�6�   ��6&   �� *InternetSpeed+6�   �� 5   
   $
��kB/s��6   $��206,68,146.667,330.333q�6   �� 5   
   $��Hosts Found��6   $ -1.375,1.864&�6&   $��10(�6K   $��286,55,-0.333,344.333q�6Y   �� 5   
   $
 Wrap=�6    . $��10(�6U   $��289,74,55.667,328.333q�6c   $ 0.585,1.075&�6�   ��6&   �� *DeviceCount+6�   &   System.Windows.Controls.DataGrid  5!   
      AutoGenerateColumns .  $
��Left=�5"      $	��385��5#      $��8,105,0,30.333q�5$         
IsReadOnly . ��5&      �� *ConnectedNetworkResources+   Columns 5'      0  *System.Windows.Controls.DataGridTextColumn  5(      ,  &System.Windows.Controls.DataGridColumn   Header$ 
IP Address��6%   	   MinWidth$		 100Q�6]   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 69   �� *RemoteIPAddress+6]     5)      $ HostName��6%   $	 25Q�6T      Width5  /System.Windows.Controls.DataGridLengthConverter$	 160 6b   
 67   �� *
HostName+6b     5*      $ Ping Response (ms)��6%   $	 25Q�6f   
 6A   �� *PingResponseTime+6f   5,      � 5.   
   $��4��6   $��10,399,100.667,9.333q�6B    � Value 6#   �� *ScanProgress+6B   50      51      