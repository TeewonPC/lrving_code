   M S B A M L   `   `   `  ����  AnimalEnclosure     AnimalEnclosure.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  动物围场��5   	   $
��1050��6   $	��650��6#   .��5      � -   5         �Name$ 
LayoutRoot��5       � ColumnDefinitions 5   
   �� 5      6!   �� 5      $	��250 �6   6-   .+�5   
   �� $��0��6   
�����F�$��1q�69   $��5q�6M   ._�5      � .+�5      �� -   5         ��Name$ canvasBackground��5      $��50��63   ��5      �� 5       j� Stretch$
 Fill=�6%    �� ImageSource$1 +pack://application:,,,/Images/背景-01.jpg��64   5      5      5      �� 5   
   $��1��6   
�����F�$��1q�69   $��5q�6M   ��5      �� 5      $
 Fill=�6   $1 +pack://application:,,,/Images/边-01-01.jpg��6,   ._�5      �� $��15q�6   $��Center=�6%   $��Center=�6@   .+�5      �� -   5         ��Name$ 	startGame��5      $��5q�6,   $	��168��68   $��48��6D   $
��0,30q�6P   ��5      �� 5      $
 Fill=�6%   $7 1pack://application:,,,/Images/开始游戏-01.jpg��64   5      �� 5       -   5         ��Name$ tbBloodPoint��5       $��0,30,0,0q�62   $
��Wrap=�6D   $��Arial)�6X   $��14(�6k   $��玩家血量：10000点��6y   6�   �� 5!      -   5        $ tbKilledAnimals��5!      $
��0,30q�65   $
��Wrap=�6C   $��Arial)�6W   $��14(�6j   $��击杀动物：0个��6x   6�   �� 5"      -   5        $ tbEscapedAnimals��5"      $
��0,30q�66   $
��Wrap=�6D   $��Arial)�6X   $��14(�6k   $��逃跑动物：0个��6y   6�   5&      5'      5(      5)      