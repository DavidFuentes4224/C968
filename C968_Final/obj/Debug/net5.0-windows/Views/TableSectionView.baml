   M S B A M L   `   `   `  ????E  AC968_Final, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:C968_Final.ViewsC968_Final.Views  5      '    !C968_Final.Views.TableSectionView   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:C968_Final.Views     5      -   5          >?Name$
  root??5	      .??5
      ]? $??1??6   $
??True??6   .+?5   
   ?? $	??Top=?6    ?? Orientation$ 
Horizontal=?6*   $??Right=?6C   .+?5      ?? -   5        $??0,0,16,0q?5   ?   $
??16,4q?6?   $??Search??6?   ??6   ?? *SearchTableItemsCommand+??6A   ??  ?? ElementName$ 
SearchText?? ?? Path$
 Text?6?   ?? 5      -   5         ??Name$ 
SearchText??5      $??150px??6*   $??0q?68   5      ?? 5   
   $	??Top=?6.   $
??10,0q?6C   ??6   ?? *	TableName+6C   ?? 5   
   $??Bottom=?6   $ 
Horizontal=?6-   $??Right=?6F   $
??0,16q?6b   .+?5      ?? $	??Add??6   $??16,0,0,0q?6#   ??65   ?? *AddTableItemCommand+?? 5      $??Modify??6   $??16,0,0,0q?6&   ??68   ?? *EditTableItemCommand+??6a   ?? $ 	DataItems??$ SelectedItem??? 5      $??Delete??6   $??16,0,0,0q?6&   ??68   ?? *DeleteTableItemCommand+??6c   ?? $ 	DataItems??$ SelectedItem?5      ?? 5   
   $??Bottom=?5      $??Right=?5      
???  ????5      ?? *ErrorMessage+y?5      ?? *ShowErrorMessage+   BooleanToVisibilty ?? 	Converter# [  5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ 	DataItems??5         
IsReadOnly$
 True??6t   ??6&   ?? *
TableItems+ ?? SelectedItem 6I   ?? *SelectedTableItem+	   Columns	 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 * 6C   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   ?? *	DisplayID+   Header 5      ?? 5      ??6$   ?? *DataContext.IdTitle+ ?? RelativeSource ??  ?? AncestorType"  5         5!      $
 Name??6%   $
 * 6L    63   ?? *Name+6L     5"      $ 	Inventory??6%   $
 * 6T    68   ?? *	InStock+6T     5#      $ Price??6%   $
 * 6N    64   ?? *Price+6N     5$      $	 Min??6%   $
 * 6J    62   ?? *Min+6J     5%      $	 Max??6%   $
 * 6J    62   ?? *Max+6J   5'      5(      5*      