using C968_Final.Commands;
using C968_Final.Models;
using C968_Final.Stores;
using C968_Final.Views;
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
    public class PartsSectionViewModel : ViewModelBase
    {
        public PartsSectionViewModel(PartStore partStore, ProductStore productStore)
        {
            m_partStore = partStore;
            m_productStore = productStore;
            m_tableItems = new ObservableCollection<TableItem>();

            ShowErrorMessage = Visibility.Hidden;

            AddTableItemCommand = new RelayCommand<object>(AddItem, CanAddItem);
            EditTableItemCommand = new RelayCommand<PartBase>(EditItem, CanEditItem);
            DeleteTableItemCommand = new RelayCommand<PartBase>(DeleteItem, CanDeleteItem);
            AddDummyTableItemsCommand = new RelayCommand<object>(AddDummyItems, CanAddDummys);
            SearchTableItemsCommand = new RelayCommand<string>(SearchItems, CanSearch);
        }

        public RelayCommand<object> AddTableItemCommand { get; set; }
        public RelayCommand<PartBase> EditTableItemCommand { get; set; }
        public RelayCommand<PartBase> DeleteTableItemCommand { get; set; }
        public RelayCommand<object> AddDummyTableItemsCommand { get; set; }
        public RelayCommand<string> SearchTableItemsCommand { get; set; }

        public string IdTitle => "Part ID";
        public string TableName 
        {
            get
            {
                var allPartsCount = m_partStore.GetParts().Count();
                var filteredPartsCount = TableItems.Count();
                var hiddenPartsText = $"({allPartsCount - filteredPartsCount} hidden)";
                return $"Parts ({filteredPartsCount}) {(allPartsCount != filteredPartsCount ? hiddenPartsText : "")}";
            }
        }
        public string ErrorMessage => "Part is associated with at least one product.";

        public Visibility ShowErrorMessage
        {
            get { return m_showErrorMessage; }
            set { m_showErrorMessage = value; OnPropertyChanged(nameof(ShowErrorMessage)); }
        }

        public IEnumerable<TableItem> TableItems => m_tableItems;

        public TableItem SelectedTableItem 
        {
            get
            {
                return m_selectedTableItem;
            }
            set
            {
                m_selectedTableItem = value;
                if (value is null)
                    return;

                var selectedPartId = ((PartBase)m_selectedTableItem).Id.Value;
                ShowErrorMessage = m_productStore.CanRemovePart(selectedPartId) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private bool CanAddItem(object p) => true;
        private void AddItem(object p)
        {
            var item1 = new PartBase() { Name = "PartABC Added" };

            var partViewModel = new PartViewModel(true, item1, m_partStore);
            var partView = new AddOrModifyPart() { DataContext = partViewModel };
            partView.ShowDialog();

            RefreshTableItems();
        }

        private bool CanEditItem(PartBase p) => p is object;
        private void EditItem(PartBase part)
        {
            if (part is null)
                return;

            var isInHouse = part is InhousePart;
            var partViewModel = new PartViewModel(isInHouse, part, m_partStore);
            var partView = new AddOrModifyPart() { DataContext = partViewModel };
            partView.ShowDialog();

            RefreshTableItems();
        }

        private bool CanDeleteItem(PartBase p) => p is object && m_productStore.CanRemovePart(p.Id.Value);
        private void DeleteItem(object p)
        {
            if (!(p is PartBase part))
                return;

            m_partStore.DeletePart(part.Id.Value);
            SelectedTableItem = null;
            RefreshTableItems();
        }

        private bool CanAddDummys(object p) => true;
        private void AddDummyItems(object p)
        {
            var part1 = new InhousePart(1, new PartBase() { InStock = 1, Max = 5, Min = 1, Name = "Part 1", Price = 5.99f });
            var part2 = new InhousePart(2, new PartBase() { InStock = 5, Max = 5, Min = 1, Name = "Part 2", Price = 12.99f });
            var part3 = new InhousePart(3, new PartBase() { InStock = 2, Max = 5, Min = 1, Name = "Part 3", Price = 55.99f });

            var outPart1 = new OutsourcedPart("Comp 1", new PartBase() { InStock = 1, Max = 5, Min = 1, Name = "O Part 1", Price = 9.99f });
            var outPart2 = new OutsourcedPart("Comp 2", new PartBase() { InStock = 1, Max = 5, Min = 1, Name = "O Part 2", Price = 21.99f });
            var outPart3 = new OutsourcedPart("Comp 3", new PartBase() { InStock = 1, Max = 5, Min = 1, Name = "O Part 3", Price = 38.99f });

            var partList = new List<PartBase> { part1, part2, part3, outPart1, outPart2, outPart3 };
            foreach (var part in partList)
                m_partStore.AddPart(part);

            RefreshTableItems();
        }

        private bool CanSearch(string p) => true;
        private void SearchItems(string searchText) => RefreshTableItems(searchText?.Trim());

        private void RefreshTableItems(string searchText = null)
        {
            m_tableItems.Clear();
            var allParts = m_partStore.GetParts();
            var filteredParts = allParts.Where(part => searchText is null || part.Name.Contains(searchText));
            foreach (var part in filteredParts)
            {
                m_tableItems.Add(part);
            }
            OnPropertyChanged(nameof(TableItems));
            OnPropertyChanged(nameof(TableName));
        }

        readonly ObservableCollection<TableItem> m_tableItems;
        readonly PartStore m_partStore;
        readonly ProductStore m_productStore;
        
        TableItem m_selectedTableItem;
        Visibility m_showErrorMessage;

    }
}