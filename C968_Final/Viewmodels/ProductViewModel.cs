using C968_Final.Commands;
using C968_Final.Models;
using C968_Final.Stores;
using C968_Final.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace C968_Final.Viewmodels
{
    public class ProductViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        public ProductViewModel(Product model, PartStore partStore, ProductStore productStore)
        {
            m_errorViewModel = new ErrorViewModel();
            m_productStore = productStore;
            m_partStore = partStore;

            Id = model.Id?.ToString();
            NameInput = model.Name?.ToString();
            InventoryInput = model.InStock.ToString();
            var priceString = model.Price.ToString();
            if (priceString is object)
                priceString = String.Format("{0:0.00}", model.Price);
            PriceInput = priceString;
            MaxInput = model.Max.ToString();
            MinInput = model.Min.ToString();


            var allParts = partStore.GetParts();
            m_candidateParts = new ObservableCollection<TableItem>(allParts);

            var productParts = partStore.GetParts(model.AssociatedParts);
            m_productParts = new ObservableCollection<TableItem>(productParts);

            if (productParts.Count == 0)
                m_errorViewModel.AddError(nameof(ProductParts), "Product must contain at least 1 part.");

            AddPartCommand = new RelayCommand<IList>(AddPart, CanAddPart);
            DeletePartCommand = new RelayCommand<IList>(RemovePart, CanRemovePart);
            SaveProductCommand = new RelayCommand<Window>(SaveProduct, CanSaveProduct);
            SearchTableItemsCommand = new RelayCommand<string>(SearchItems, CanSearch);
        }

        public string Id { get; set; }
        public string NameInput
        {
            get => m_name;
            set
            {
                m_name = value;

                m_errorViewModel.RemoveError(nameof(NameInput));

                if (!(m_name.Trim().Length > 0))
                    m_errorViewModel.AddError(nameof(NameInput), "Name must contain a value");
            }
        }

        public string InventoryInput
        {
            get => m_inventory;
            set
            {
                m_inventory = value;

                m_errorViewModel.RemoveError(nameof(InventoryInput));
                int inventory = 0;
                if (!int.TryParse(m_inventory, out inventory) || inventory <= 0)
                    m_errorViewModel.AddError(nameof(InventoryInput), "Inventory must be above 0");
            }
        }

        public string PriceInput
        {
            get => m_price;
            set
            {
                m_price = value;

                var isDecimal = ValidationUtility.ValueIsDecimalFormat(m_price);

                m_errorViewModel.RemoveError(nameof(PriceInput));
                if (!isDecimal)
                    m_errorViewModel.AddError(nameof(PriceInput), "Price must be in decimal format.");
                if (!float.TryParse(m_price, out var price) || price <= 0)
                    m_errorViewModel.AddError(nameof(PriceInput), "Price must be above 0");
            }
        }

        public string MaxInput
        {
            get => m_max;
            set
            {
                m_max = value;

                m_errorViewModel.RemoveError(nameof(MaxInput));
                if (!int.TryParse(m_max, out var max) || max < 0)
                    m_errorViewModel.AddError(nameof(MaxInput), "");

                ValidateMinMax();
            }
        }

        public string MinInput
        {
            get => m_min;
            set
            {
                m_min = value;

                m_errorViewModel.RemoveError(nameof(MinInput));
                if (!int.TryParse(m_min, out var min) || min < 0)
                    m_errorViewModel.AddError(nameof(MinInput), "");

                ValidateMinMax();
            }
        }

        public IEnumerable<TableItem> CandidateParts => m_candidateParts;
        public IEnumerable<TableItem> ProductParts => m_productParts;

        public string CandidatePartsTableName
        {
            get
            {
                var allPartsCount = m_partStore.GetParts().Count();
                var filteredPartsCount = CandidateParts.Count();
                var hiddenPartsText = $"({allPartsCount - filteredPartsCount} hidden)";
                return $"Candidate Parts ({filteredPartsCount}) {(allPartsCount != filteredPartsCount ? hiddenPartsText : "")}";
            }
        }

        public PartBase SelectedAddPart { get; set; }
        public PartBase SelectedRemovePart { get; set; }

        public Visibility MinMaxMessageVisible { get; set; }

        public RelayCommand<IList> AddPartCommand { get; private set; }
        public RelayCommand<IList> DeletePartCommand { get; private set; }
        public RelayCommand<Window> SaveProductCommand { get; private set; }
        public RelayCommand<string> SearchTableItemsCommand { get; set; }

        public bool HasErrors => m_errorViewModel.HasErrors;

        public IEnumerable GetErrors(string propertyName) => m_errorViewModel.GetErrors(propertyName);

        private bool CanAddPart(object p) => true;
        private void AddPart(IList p)
        {
            if (p == null)
                return;

            var selectedParts = p.Cast<PartBase>();
            foreach(var part in selectedParts)
            {
                m_productParts.Add(part);
            }

            if (m_productParts.Count() > 0)
                m_errorViewModel.RemoveError(nameof(ProductParts));
        }

        private bool CanRemovePart(object p) => true;
        private void RemovePart(IList p)
        {
            if (p == null)
                return;

            var selectedParts = p.Cast<PartBase>().ToList();
            // fix 
            for(int i = 0; i < selectedParts.Count(); i++)
            {
                m_productParts.Remove(selectedParts[i]);
            }

            if (m_productParts.Count() == 0)
                m_errorViewModel.AddError(nameof(ProductParts), "Product must contain at least 1 part.");
        }

        private bool CanSaveProduct(object p) => !m_errorViewModel.HasErrors;
        private void SaveProduct(Window window)
        {
            if (window is null)
                return;

            var associatedParts = ProductParts.Select(p => (PartBase)p)
                .Where(p => p.Id.HasValue)
                .Select(p => p.Id.Value);

            var product = new Product()
            {
                Name = NameInput,
                Price = float.Parse(PriceInput),
                Max = int.Parse(MaxInput),
                Min = int.Parse(MinInput),
                InStock = int.Parse(InventoryInput),
                AssociatedParts = associatedParts.ToList()
            };

            if (int.TryParse(Id, out var id))
            {
                m_productStore.UpdateProduct(id, product);
            }
            else
            {
                m_productStore.AddProduct(product);
            }

            window.Close();
        }

        private bool CanSearch(string p) => true;
        private void SearchItems(string searchText) => RefreshTableItems(searchText?.Trim());

        private void RefreshTableItems(string searchText = null)
        {
            m_candidateParts.Clear();
            var allParts = m_partStore.GetParts();
            var filteredParts = allParts.Where(part => searchText is null || part.Name.Contains(searchText));
            foreach (var part in filteredParts)
            {
                m_candidateParts.Add(part);
            }
            OnPropertyChanged(nameof(CandidateParts));
            OnPropertyChanged(nameof(CandidatePartsTableName));
        }

        private void ValidateMinMax()
        {
            var shouldShowMessage = !int.TryParse(m_min, out var min)
                || !int.TryParse(m_max, out var max)
                || min > max;

            MinMaxMessageVisible = shouldShowMessage ? Visibility.Visible : Visibility.Collapsed;
            OnPropertyChanged(nameof(MinMaxMessageVisible));
        }


        readonly ErrorViewModel m_errorViewModel;
        readonly ProductStore m_productStore;
        readonly PartStore m_partStore;

        ObservableCollection<TableItem> m_candidateParts;
        ObservableCollection<TableItem> m_productParts;

        string m_name;
        string m_inventory;
        string m_price;
        string m_max;
        string m_min;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}
