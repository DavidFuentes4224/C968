using C968_Final.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace C968_Final.Viewmodels
{
    public class TableSectionViewModel : ViewModelBase
    {
        public TableSectionViewModel()
        {
            m_tableItems = new ObservableCollection<TableItem>();
            var item1 = new PartBase() { Name = "PartABC" };
            m_tableItems.Add(item1);

            AddTableItemCommand = 
        }

        public ICommand AddTableItemCommand { get; }

        private string tableName;
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        private readonly ObservableCollection<TableItem> m_tableItems;
        public IEnumerable<TableItem> TableItems => m_tableItems;

        
        private void AddItem()
        {
            var item1 = new PartBase() { Name = "PartABC Added" };
            m_tableItems.Add(item1);
        }
    }
}