using C968_Final.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace C968_Final.Views
{
    /// <summary>
    /// Interaction logic for TableSectionView.xaml
    /// </summary>
    public partial class TableSectionView : UserControl
    {
        public TableSectionView()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(TableSectionView), new PropertyMetadata(""));



        public ObservableCollection<TableItem> TableItems
        {
            get { return (ObservableCollection<TableItem>)GetValue(TableItemsProperty); }
            set { SetValue(TableItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TableItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TableItemsProperty =
            DependencyProperty.Register("TableItems", typeof(ObservableCollection<TableItem>), typeof(TableSectionView), new PropertyMetadata(null));




        private void AddBttn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ModiftyBttn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteBttn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
