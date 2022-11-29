using C968_Final.Commands;
using C968_Final.Models;
using C968_Final.Stores;
using C968_Final.Utility;
using C968_Final.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace C968_Final.Viewmodels
{
    public class ProductsSectionViewModel : ViewModelBase
    {
        public ProductsSectionViewModel(Inventory inventory)
        {
            m_inventory = inventory;
            m_inventory.InventoryUpdated += OnInventoryUpdated;
            m_tableItems = new ObservableCollection<TableItem>();

            AddTableItemCommand = new RelayCommand<object>(AddItem, CanAddItem);
            EditTableItemCommand = new RelayCommand<Product>(EditItem, CanEditItem);
            DeleteTableItemCommand = new RelayCommand<Product>(DeleteItem, CanDeleteItem);
            SearchTableItemsCommand = new RelayCommand<string>(SearchItems, CanSearch);
        }


        public RelayCommand<object> AddTableItemCommand { get; set; }
        public RelayCommand<Product> EditTableItemCommand { get; set; }
        public RelayCommand<Product> DeleteTableItemCommand { get; set; }
        public RelayCommand<string> SearchTableItemsCommand { get; set; }


        public string ErrorMessage => "Product is associated with at least one part.";
        public bool ShowErrorMessage
        {
            get
            {
                var selectedProduct = (Product)m_selectedTableItem;
                if (selectedProduct is null)
                    return false;

                return !m_inventory.CanRemoveProduct(selectedProduct.ProductID);
            }
        }

        public string TableName
        {
            get
            {
                var allProductsCount = m_inventory.Products.Count();
                var filteredProductsCount = TableItems.Count();
                var hiddenPartsText = $"({allProductsCount - filteredProductsCount} hidden)";
                return $"Products ({filteredProductsCount} results) {(allProductsCount != filteredProductsCount ? hiddenPartsText : "")}";
            }
        }
        public string IdTitle => "Product ID";

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
                OnPropertyChanged(nameof(ShowErrorMessage));
            }
        }

        private bool CanAddItem(object p) => true;
        private void AddItem(object p)
        {
            var nextId = m_inventory.NextProductId;
            var item1 = new Product() { ProductID = nextId};

            var productViewModel = new ProductViewModel(item1, m_inventory, TableActions.Action.ADD);
            var productView = new AddOrModifyProduct() { DataContext = productViewModel };
            productView.ShowDialog();

            RefreshTableItems();
        }

        private bool CanEditItem(object p) => p is object;
        private void EditItem(object p)
        {
            if (!(p is Product product))
                return;

            var productViewModel = new ProductViewModel(product, m_inventory, TableActions.Action.UPDATE);
            var productView = new AddOrModifyProduct() { DataContext = productViewModel };
            productView.ShowDialog();

            RefreshTableItems();
        }
        
        private bool CanDeleteItem(object p) => p is Product product && m_inventory.CanRemoveProduct(product.ProductID);
        private void DeleteItem(object p)
        {
            if (!(p is Product product))
                return;

            var diaglogResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (diaglogResult != MessageBoxResult.Yes)
                return;

            m_inventory.RemoveProduct(product.ProductID);
            SelectedTableItem = null;
            RefreshTableItems();
        }

        private bool CanSearch(string p) => true;
        private void SearchItems(string searchText) => RefreshTableItems(searchText?.Trim().ToLower());

        private void RefreshTableItems(string searchText = null)
        {
            m_tableItems.Clear();
            var allProducts = m_inventory.Products;
            var filteredProducts = allProducts
                .Where(product => searchText is null
                || product.Name.ToLower().Contains(searchText)
                || product.ProductID.ToString().Contains(searchText));
            foreach (var part in filteredProducts)
            {
                m_tableItems.Add(part);
            }
            OnPropertyChanged(nameof(TableItems));
            OnPropertyChanged(nameof(TableName));
            OnPropertyChanged(nameof(ShowErrorMessage));
        }

        private void OnInventoryUpdated(object sender, EventArgs e) => RefreshTableItems();

        readonly ObservableCollection<TableItem> m_tableItems;
        readonly Inventory m_inventory;

        TableItem m_selectedTableItem;
    }
}