   M S B A M L   `   `   `  ����E  AMemoryGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:MemoryGame.ViewModelMemoryGame.ViewModel  5   	        MemoryGame.View.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   1vm"clr-namespace:MemoryGame.ViewModel     5   	     � Title$  Memory Game - Sign In��5   	   $	��500��5   	   $	��550��6    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � DataContext 5	      !   MemoryGame.ViewModel.MainVM  5
   
   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��0,0,0,10q�6.   .+�5      �� $��	Username:��6   $��Center=�6(   �� 5      $	��200��6   $��5,0,0,0q�6"   $��32��6u   ��63   �� *NewUsername+ �� UpdateSourceTrigger$ PropertyChanged=�6u   �� 5      $	��←��6   $��5,0,0,0q�6Q   $��32��6b   $��32��6n   ��6(   �� *PreviousImageCommand+6n   �� 5      $	��161��64   $	��107��6@   $��5,0,0,0q�6M   ��6   �� *CurrentImage+6M   �� 5      $	��→��6   $��5,0,0,0q�6M   $��32��6^   $��32��6j   ��6(   �� *NextImageCommand+6j   5      �� 5   
   $��1��6   ��6   �� *Users+ �� SelectedItem 6=   �� *SelectedUser+��5      �� 5      .R�5      �� $ 
Horizontal=�6!   .+�5       �� $��50��6=   $��50��6H   $��0,0,5,0q�6T   ��6    �� *	ImagePath+6T   �� 5!      $��Center=�6>   ��6$   �� *
Username+6>   5"      5#      5%      �� 5'   
   $��2��6   $ 
Horizontal=�6"   $��Center=�6;   $��0,10,0,0q�6X   .+�5(      �� $��Add User��6   $��5q�6K   ��6(   �� *AddUserCommand+6K   �� 5)      $��Delete User��6   $��5q�6Q   ��6+   �� *DeleteUserCommand+{�6\   �� *IsUserSelected+�� 5*      $
��Play��6   $��5q�6D   ��6$   �� *PlayCommand+{�6O   �� *IsUserSelected+�� 5+      $
��Exit��6   $��5q�6D   ��6$   �� *ExitCommand+6D   5,      5-      5.      