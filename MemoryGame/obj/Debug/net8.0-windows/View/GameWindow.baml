   M S B A M L   `   `   `  ����E  AMemoryGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:MemoryGame.ViewModelMemoryGame.ViewModel  5   	        MemoryGame.View.GameWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   1vm"clr-namespace:MemoryGame.ViewModel     5   	     � Title$  Memory Game��5   	    � WindowStartupLocation$ CenterScreen=�5   	    � SizeToContent$ WidthAndHeight=�5	   	    � 
ResizeMode$ NoResize=�5
   	   ��5   	   �� *WindowHeight+��5   	   �� *WindowWidth+ �� Mode$ OneWay=�.��5      ]� .+�5   
   o� $	��Top=�6   .@�5      m� $
��File��6   .@�5      m� $��New Game��6    m� Command 6-   �� *NewGameCommand+m� 5      $��	Open Game��6    6.   �� *OpenGameCommand+m� 5      $��	Save Game��6    6.   �� *SaveGameCommand+�� 5      m� 5      $��
Statistics��6    6/   �� *StatisticsCommand+�� 5      m� 5      $��Category��6   .@�5      m� $��Drivers��6    m� CommandParameter$ Drivers��6T    60   �� *CategoryCommand+6T   m� 5      $��Legends��6   $ Legends��6T    60   �� *CategoryCommand+6T   m� 5      $��Teams��6   $ Teams��6R    6.   �� *CategoryCommand+6R   5      �� 5      m� 5      $��
Difficulty��6   .@�5      m� $
��Easy��6   $
 Easy��6S    6-   �� *DifficultyCommand+6S   m� 5      $��Normal��6   $ Normal��6U    6/   �� *DifficultyCommand+6U   m� 5      $
��Hard��6   $
 Hard��6S    6-   �� *DifficultyCommand+6S   m� 5       $��Expert��6   $ Expert��6U    6/   �� *DifficultyCommand+6U   5!      m� 5"      $
��Exit��6    6)   �� *ExitCommand+5#      m� 5$      $��Options��6   .@�5%      m� $��Standard��6    61   �� *SetStandardCommand+m� 5&      $��Custom��6    6/   �� *SetCustomCommand+5'      m� 5(      $
��Help��6   .@�5)      m� $��About��6    6.   �� *AboutCommand+5*      5+      �� 5-   
    �� Orientation$ Vertical=�6   .+�5.      �� $��
Time left:��6   $
��Bold"�6*   $��10q�6<   �� 5/      $��10q�62   ��6   �� *
TimeLeft+62   �� 51      ��6   �� *Cards+��52      �� 53      .R�55      A� $��10q�57      $��Center=�58      $��Center=�6+    A� Rows 55   &   �� *Rows+	 A� Columns	 56      �� *	Columns+58   +   59      ��5;      �� 5<      .R�5=      �� $��Center=�5>   a   $��Center=�6~   ��5=   !   �� *!DataContext.CardSelectedCommand+
 �� RelativeSource
 ��  �� AncestorType
 Window��5>   !   �� {�6>   �� *IsGameActive+.��5?      �� $	��140��6D   $	��140��6P   ��6$   �� *DisplayImage+6P   5@      5A      5C      5D      5E      5F      