using C968_Final.Commands;
using C968_Final.Models;
using C968_Final.Stores;
using C968_Final.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C968_Final.Viewmodels
{
    public class ProductsSectionViewModel : ViewModelBase
    {
        public ProductsSectionViewModel(PartStore partStore, ProductStore productStore)
        {
            m_partStore = partStore;
            m_productStore = productStore;
            m_tableItems = new ObservableCollection<TableItem>();

            AddTableItemCommand = new RelayCommand<object>(AddItem, CanAddItem);
            EditTableItemCommand = new RelayCommand<Product>(EditItem, CanEditItem);
            DeleteTableItemCommand = new RelayCommand<Product>(DeleteItem, CanDeleteItem);
        }

        public RelayCommand<object> AddTableItemCommand { get; set; }
        public RelayCommand<Product> EditTableItemCommand { get; set; }
        public RelayCommand<Product> DeleteTableItemCommand { get; set; }
        public RelayCommand<object> AddDummyTableItemsCommand { get; set; }

        public string ErrorMessage => "";
        public string TableName => "Products";
        public string IdTitle => "Product ID";

        public IEnumerable<TableItem> TableItems => m_tableItems;

        private bool CanAddItem(object p) => true;
        private void AddItem(object p)
        {
            var item1 = new Product() { Name = "Product ABC Added" };

            var productViewModel = new ProductViewModel(item1, m_partStore, m_productStore);
            var productView = new AddOrModifyProduct() { DataContext = productViewModel };
            productView.ShowDialog();

            RefreshTableItems();
        }

        private bool CanEditItem(object p) => true;
        private void EditItem(object p)
        {
            if (!(p is Product product))
                return;

            var productViewModel = new ProductViewModel(product, m_partStore, m_productStore);
            var productView = new AddOrModifyProduct() { DataContext = productViewModel };
            productView.ShowDialog();

            RefreshTableItems();
        }

        private bool CanDeleteItem(object p) => true;
        private void DeleteItem(object p)
        {
            if (!(p is Product product))
                return;

            m_productStore.DeleteProduct(product.Id.Value);
            RefreshTableItems();
        }

        private void RefreshTableItems()
        {
            m_tableItems.Clear();
            foreach (var part in m_productStore.GetProducts())
            {
                m_tableItems.Add(part);
            }
            OnPropertyChanged(nameof(TableItems));
        }

        readonly ObservableCollection<TableItem> m_tableItems;
        readonly ProductStore m_productStore;
        readonly PartStore m_partStore;
    }
}