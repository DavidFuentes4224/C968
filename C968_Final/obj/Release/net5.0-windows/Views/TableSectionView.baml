   M S B A M L   `   `   `  ???? E  AC968_Final, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:C968_Final.ViewsC968_Final.Views  '    !C968_Final.Views.TableSectionView   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:C968_Final.Views     -     >?Name$
  root??.??]? $??1??$
??True??.+??? $	??Top=? ?? Orientation$ 
Horizontal=?$??Right=?.+??? -   $??0,0,16,0q?$
??16,4q?$??Search?????? *SearchTableItemsCommand+????  ?? ElementName$ 
SearchText?? ?? Path$
 Text??? -    ??Name$ 
SearchText??$??150px??$??0q??? $	??Top=?$
??10,0q????? *	TableName+?? $??Bottom=?$ 
Horizontal=?$??Right=?$
??0,16q?.+??? $	??Add??$??16,0,0,0q????? *AddTableItemCommand+?? $??Modify??$??16,0,0,0q????? *EditTableItemCommand+???? $ 	DataItems??$ SelectedItem??? $??Delete??$??16,0,0,0q????? *DeleteTableItemCommand+???? $ 	DataItems??$ SelectedItem??? $??Bottom=?$??Right=?
???  ?????? *ErrorMessage+y??? *ShowErrorMessage+   BooleanToVisibilty ?? 	Converter# [  &   System.Windows.Controls.DataGrid  -     Name$ 	DataItems??   
IsReadOnly$
 True?????? *
TableItems+ ?? SelectedItem ?? *SelectedTableItem+	   Columns	 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn
   Width5  /System.Windows.Controls.DataGridLengthConverter$
 * 1  +System.Windows.Controls.DataGridBoundColumn   Binding ?? *	DisplayID+   Header ?? ???? *DataContext.IdTitle+ ?? RelativeSource ??  ?? AncestorType"    $
 Name??$
 *  ?? *Name+  $ 	Inventory??$
 *  ?? *	InStock+  $ Price??$
 *  ?? *Price+  $	 Min??$
 *  ?? *Min+  $	 Max??$
 *  ?? *Max+