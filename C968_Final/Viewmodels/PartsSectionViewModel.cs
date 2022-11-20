using C968_Final.Commands;
using C968_Final.Models;
using C968_Final.Stores;
using C968_Final.Utility;
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
        public PartsSectionViewModel(Inventory inventory)
        {
            m_tableItems = new ObservableCollection<TableItem>();

            ShowErrorMessage = Visibility.Hidden;

            AddTableItemCommand = new RelayCommand<object>(AddItem, CanAddItem);
            EditTableItemCommand = new RelayCommand<PartBase>(EditItem, CanEditItem);
            DeleteTableItemCommand = new RelayCommand<PartBase>(DeleteItem, CanDeleteItem);
            AddDummyTableItemsCommand = new RelayCommand<object>(AddDummyItems, CanAddDummys);
            SearchTableItemsCommand = new RelayCommand<string>(SearchItems, CanSearch);
            m_inventory = inventory;
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
                var allPartsCount = m_inventory.AllParts.Count();
                var filteredPartsCount = TableItems.Count();
                var hiddenPartsText = $"({allPartsCount - filteredPartsCount} hidden)";
                return $"Parts ({filteredPartsCount} results) {(allPartsCount != filteredPartsCount ? hiddenPartsText : "")}";
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

                var selectedPartId = ((PartBase)m_selectedTableItem).PartID;
                ShowErrorMessage = m_inventory.CanRemovePart(selectedPartId) ? Visibility.Hidden : Visibility.Visible;
            }
        }

        private bool CanAddItem(object p) => true;
        private void AddItem(object p)
        {
            var nextId = m_inventory.NextPartId;
            var newPart = new PartBase() { PartID = nextId };

            var partViewModel = new PartViewModel(true, newPart, m_inventory, TableActions.Action.ADD);
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
            var partViewModel = new PartViewModel(isInHouse, part, m_inventory, TableActions.Action.UPDATE);
            var partView = new AddOrModifyPart() { DataContext = partViewModel };
            partView.ShowDialog();

            RefreshTableItems();
        }

        private bool CanDeleteItem(PartBase p) => p is object && m_inventory.CanRemovePart(p.PartID);
        private void DeleteItem(object p)
        {
            if (!(p is PartBase part))
                return;

            var diaglogResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (diaglogResult != MessageBoxResult.Yes)
                return;

            m_inventory.DeletePart(part);
            SelectedTableItem = null;
            RefreshTableItems();
        }

        private bool CanAddDummys(object p) => true;
        private void AddDummyItems(object p)
        {
            var partId = 0;
            var part1 = new InhousePart(1, new PartBase() { PartID = partId++, InStock = 1, Max = 5, Min = 1, Name = "Part 1", Price = 5.99M });;
            var part2 = new InhousePart(2, new PartBase() { PartID = partId++, InStock = 5, Max = 5, Min = 1, Name = "Part 2", Price = 12.99M });
            var part3 = new InhousePart(3, new PartBase() { PartID = partId++, InStock = 2, Max = 5, Min = 1, Name = "Part 3", Price = 55.99M });

            var outPart1 = new OutsourcedPart("Comp 1", new PartBase() { PartID = partId++, InStock = 1, Max = 5, Min = 1, Name = "O Part 1", Price = 9.99M });
            var outPart2 = new OutsourcedPart("Comp 2", new PartBase() { PartID = partId++, InStock = 1, Max = 5, Min = 1, Name = "O Part 2", Price = 21.99M });
            var outPart3 = new OutsourcedPart("Comp 3", new PartBase() { PartID = partId++, InStock = 1, Max = 5, Min = 1, Name = "O Part 3", Price = 38.99M });

            var partList = new List<PartBase> { part1, part2, part3, outPart1, outPart2, outPart3 };
            foreach (var part in partList)
                m_inventory.AddPart(part);

            RefreshTableItems();
        }

        private bool CanSearch(string p) => true;
        private void SearchItems(string searchText) => RefreshTableItems(searchText?.Trim().ToLower());

        private void RefreshTableItems(string searchText = null)
        {
            m_tableItems.Clear();
            var allParts = m_inventory.AllParts;
            var filteredParts = allParts
                .Where(part => searchText is null 
                || part.Name.ToLower().Contains(searchText) 
                || part.PartID.ToString().Contains(searchText));
            foreach (var part in filteredParts)
            {
                m_tableItems.Add(part);
            }
            OnPropertyChanged(nameof(TableItems));
            OnPropertyChanged(nameof(TableName));
        }

        readonly ObservableCollection<TableItem> m_tableItems;
        readonly Inventory m_inventory;

        TableItem m_selectedTableItem;
        Visibility m_showErrorMessage;
    }
}