   M S B A M L   `   `   `  ����J  FHotelIES_Grupo3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:HotelIES_Grupo3HotelIES_Grupo3  5   	        HotelIES_Grupo3.UpdateUser   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:HotelIES_Grupo3     5   	     � WindowStyle$
  None=�5   	    � 
ResizeMode$ NoResize=�5	   	    � AllowsTransparency$
 True��5
   	    � WindowStartupLocation$ CenterScreen=�5   	   
������  � Title$ 
UpdateUser��5   	   $	��720��6   $
��1280��6)    � 	Resources 5      (  "System.Windows.Controls.DatePicker �� 
TargetType �� Property �� Value	 �� 
TargetType
 �� CornerRadius � ColumnDefinitions:  4System.Windows.Controls.Primitives.DatePickerTextBox  Name ��Name �� 
IsReadOnly �� RelativeSource �� Path �� Mode ��Name �Name � AllowsTransparency&   System.Windows.Controls.Calendar  Name ��Name �� SelectionBoxItemTemplate �� SelectionBoxItem d�Name d� 	IsChecked �� IsDropDownOpen J� SnapsToDevicePixels��%@	  '        '�� �    �� 5   
   "  6   .�5      ��  � �� 5      �� 5      "	  6&   .R�5      �� 
�������
�������$��1q�5      $
 10��5      ._�5      �  5   "   �� 5   &   $��1* �67   �� 5   &   $��35 �67   .+�5   "     $ PART_TextBox��64   
������ $��
5, 0, 0, 0q�5   $   .��68   ��5    &   �� 5!   *   .R�5"   .   �� #��z��65   #��z��5#   !   #��z��5$   !   ._�5%   2   �� $ PART_TextBoxInner��6:   $
 True��5&   %   $��Center=�5'   %   
������ $��0q�5)   %   $
��Left=�5+   %   $��5q�5,   %   ��5*   %   ��  �� *TemplatedParent+$
 Text�$ TwoWay=�5,   %   5-   /   5.   +   .��5/   B   50   #   �� 52   "   $ PART_Button��6)   $��1��53      
������ 
�������$��0q�56      $��Center=�57      $��Center=�58      $��25��5:      $��25��5;      $��5q�5<      .��5=   &   �� $��/calendar.png��6,   $��20��5>       $��20��5?       $��Uniform=�5@       5A   #   � 5C   "   $ 
PART_Popup��6(   $��Bottom=�5D      $
 True��5E      .��5F   &   �� 
�������
�������$��1q�5H      $
 5��5I      ._�5J   *     $ PART_Calendar��63   5K   '   5L   #   5M      5N      5O      5Q      5S      �� 5U   
   " ��6   .�5V      ��  � �� 5W      �� 5X      "	 ��6&   .R�5Y      �� 
�������
�������$��1q�5[   	   $
 5��5\   	   ._�5]      �  5^   "   �� 5_   &   $��1* �67   �� 5`   &   $��35 �67   .+�5c   "   �� $ ContentSite��63   $��
5, 0, 0, 0q�5d      $��Center=�5e      $
��Left=�5f      #��z 5g      #��z 5h      d� 5j   "   $ DropDownToggle��6/   $��1��5k      
������ 
�������$��0q�5n      $��Center=�5o      $��Center=�5p      $��25��5q      $��25��5r      $��5q�5s       5t      �� *IsDropDownOpen+ �� *TemplatedParent+.��5u   &   �� $��/dropdown.png��6,   $��20��5v      $��20��5w      $��Uniform=�5x      5y   #   � 5{   "   $ 
PART_Popup��6(   $��Bottom=�5|      $
 True��5}      #��z 5~      $|�False��5      .��5�   &   �� 
�������
�������$��1q�5�      $	��130��5�      $
 5��5�      ._�5�   *   �� $��0q�67   $
 True��5�      .��5�   .   �� 5�   +   5�   '   5�   #   5�      5�      5�      5�      5�      5�      .��5�      �� 
�������$
 20��6%   $��2q�67   
���   �._�5�   
   � 
������ $��20q�6(    � RowDefinitions 5�      �� 5�      6"   �� 5�      6"   �� 5�      6"    5�      �� 5�      $	��65* �6#   61   �� 5�      $	��38* �6#   61   .+�5�      ]� $��	0,8,0,147q�6   .+�5�      �� $��Actualizar usuario��6   $��48(�66   ���#1f3b91$��Medium"�6Y   $��Center=�6m   5�      �� 5�      -   5        $��Right=�5�      $��1��61   $	��Top=�6A   ��5�      �� 5�      .R�5�      �� $��50��6    $��/cerrar.png��6+   5�      5�      � 5�      $��0��6   $��	0,100,0,0q�6%   .+�5�      �� $��0,0,40,0q�6   .+�5�      �� $	��DNI��6    $��25(�6+   $��0 0 0 15q�69   �� 5�      
�������$
 10��60   
�������$��1q�6U   $��50��6i   ._�5�      �� -   5        $ txtDni��5�   "   $��0q�62   
������ $��15(�6_   $��Center=�5�      $	��200��60   $
 True��5�      ${�False��6    5�      5�      �� 5�      $��0,0,40,0q�6   .+�5�      �� $	��Rol��6    $��25(�6+   $��0 0 0 15q�69   �� 5�      
�������$
 10��60   
�������$��1q�6U   $��50��6i   $	��200��6u   ._�5�      �� -   5         ��Name$ rolSeleccionado��5�   #   
������ $��Stretch=�5�      $��Stretch=�5�      $��0q�5�      .@�5�      �� .��6+   	Cliente64   �� 5�      .��6+   Administrador6:   �� 5�      .��6+   
Empleado65   5�      5�      5�      �� 5�      $��0,0,40,0q�6   .+�5�      �� $��Fecha de nacimiento��6    $��25(�6;   $��0 0 0 15q�6I   �� 5�      
�������$
 10��60   
�������$��1q�6U   $��50��6i   $	��200��6u   ._�5�        -   5          Name$ txtFecha_nac��5�   %   $��0q�6;   
������ $��15(�5�      $��Center=�5�      $��Stretch=�5�      $��Stretch=�5�      ${�False��5�      $|�False��6#   5�      5�      5�      � 5�      $��1��6   $��
0,40,0,225q�6%   $��2��69   .+�5�      �� $��0,0,40,0q�6   .+�5�      �� $��Nombre��6    $��25(�6.   $��0 0 0 15q�6<   �� 5�      
�������$
 10��60   
�������$��1q�6U   $��50��6i   ._�5�      �� -   5        $ 	txtNombre��5�   "   $��0q�65   
������ $��15(�6b   $��Center=�6p   $	��200��6�   5�      5�      �� 5�      $��0,0,40,0q�6   .+�5�      �� $��Email��6    $��25(�6-   $��0 0 0 15q�6;   �� 5�      
�������$
 10��60   
�������$��1q�6U   $��50��6i   ._�5�      �� -   5        $ txtEmail��5�   "   $��0q�64   
������ $��15(�6a   $��Center=�6o   $	��200��6�   5�      5�      �� 5�      $��0,0,40,0q�6   .+�5�      �� $��Ciudad��6    $��25(�6.   $��0 0 0 15q�6<   �� 5�      
�������$
 10��60   
�������$��1q�6U   $��50��6i   ._�5�      �� -   5        $ 	txtCiudad��5�   "   $��0q�65   
������ $��15(�6b   $��Center=�6p   $	��200��6�   5�      5�      5�      � 5�      $��2��6   .+�5�      �� $��0,0,40,0q�6   .+�5�      �� $��	Apellidos��6    $��25(�61   $��0 0 0 15q�6?   �� 5�      
�������$
 10��60   
�������$��1q�6U   $��50��6i   ._�5�      �� -   5        $ txtApellido��5�   "   $��0q�67   
������ $��15(�6d   $��Center=�6r   $	��200��6�   5�      5�      �� 5�      $��0,0,40,0q�6   .+�5�      �� $��Contraseña��6    $��25(�62   $��0 0 0 15q�6@   �� 5�      
�������$
 10��60   
�������$��1q�6U   $��50��6i   ._�5�      G� -	   5          G�Name$  txtPassword��5�   &   $��0q�6;   
������ $��15(�6h   $��Center=�6v   $	��200��6�   5�      5�      �� 5      $��0,0,40,0q�6   .+�5     �� $
��Sexo��6    $��25(�6,   $��0 0 0 15q�6:   �� 5     .+�5     	� -
   5        ! 	�Name$! hombre��5  &   $��Hombre��66   $��18(�6G   " 	� 	GroupName$
" sexo��6W   $��Center=�6h   	� 5     -   5        $! mujer��5  &   $��Mujer��65   $��18(�6E   $
" sexo��6T   $��Center=�6e   	� 5     -   5        $! indeterminado��5  &   $��Indeterminado��6=   $��18(�6U   $
" sexo��6d   $��Center=�6u   5     5     5     �� 5     $��1��6   $��0��6)   $��3��66   $��Center=�6G   $	��Top=�6d   $��	0,103,0,0q�6|   .+�5     � .+�5     �� .+�5     �� $"��Seleccionar imagen de perfil��6$   $��25(�6H   $��0 0 0 15q�6V   �� 5     
�������$
 10��64   
�������$��1q�6Y   $	��440��6m   ._�5     � .+�5  "   �� -   5        # ��Name$# 
imgPreview��5  A   $	��490��6(   $	��310��65   $
��Fill=�6U   �� 5  "   -   5        
������ 
������ $��Center=�5     $��Stretch=�5     $��Stretch=�5     
������ $��0q�5     5     5      5!     5"     5#     �� 5%     .+�5&     �� $	��300��6   $��40��6)   ���#1f3b91���#1f3b91$
 20��6`   $��0,10,0,0q�6r   ._�5'     �� -   5        
������ 
�������
������ $��18(�5'  g   .��6�   
ACTUALIZAR5)     5*     5+     5,     5-     5.     5/     