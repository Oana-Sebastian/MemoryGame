   M S B A M L   `   `   `  ����E  AMemoryGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null&     MemoryGame.View.CustomSizeWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Set Custom Size��5   	   $	��190��6!   $	��190��6.    � WindowStartupLocation$ CenterScreen=�5   	    � WindowStyle$ 
ToolWindow=�5   	    � 
ResizeMode$ NoResize=�5   	    � DataContext 5      ;"clr-namespace:MemoryGame.ViewModelMemoryGame.ViewModel  5	      '   !MemoryGame.ViewModel.CustomSizeVM  6
   1vm"clr-namespace:MemoryGame.ViewModel     6   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��Width (2-6):��6   $��0��6'   �� 5   
   $��1��6   $	��100��6[   $
��Left=�6g   ��6   �� *Width+ �� UpdateSourceTrigger$ PropertyChanged=�6g   �� 5   
   $��Height (2-6):��6   $��2��6(   $��0,10,0,0q�65   �� 5   
   $��3��6   $	��100��6\   $
��Left=�6h   ��6   �� *Height+$ PropertyChanged=�6h   �� 5   
   $��4��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   $��0,10,0,0q�6X   .+�5      �� $��OK��6   $��75��6"   ��6-   �� *ConfirmCommand+{�6P   �� *
CanConfirm+�� 5      $��Cancel��6   $��75��6&   $��10,0,0,0q�6S   ��61   �� *CancelCommand+6S   5      5      5       