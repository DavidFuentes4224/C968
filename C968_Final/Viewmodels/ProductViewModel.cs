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
        public ProductViewModel(Product model, PartStore partStore, ProductStore productStore, TableActions.Action action)
        {
            if (action == TableActions.Action.ADD)
                Title = "Add Product";
            else
                Title = "Update Product";

            m_errorViewModel = new ErrorViewModel();
            m_productStore = productStore;
            m_partStore = partStore;

            InputErrors = new ObservableCollection<string>();

            Id = model.ProductID.ToString();
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
            {
                m_errorViewModel.AddError(nameof(ProductParts), c_partError);
            }

            AddPartCommand = new RelayCommand<IList>(AddPart, CanAddPart);
            DeletePartCommand = new RelayCommand<IList>(RemovePart, CanRemovePart);
            SaveProductCommand = new RelayCommand<Window>(SaveProduct, CanSaveProduct);
            SearchTableItemsCommand = new RelayCommand<string>(SearchItems, CanSearch);

            UpdateErrorsList();
        }
        
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public string Id { get; set; }
        public string NameInput
        {
            get => m_name;
            set
            {
                m_name = value;
                ValidateInputs(nameof(NameInput));
            }
        }

        public string InventoryInput
        {
            get => m_inventory;
            set
            {
                m_inventory = value;
                ValidateInputs(nameof(InventoryInput));
            }
        }

        public string PriceInput
        {
            get => m_price;
            set
            {
                m_price = value;
                ValidateInputs(nameof(PriceInput));
            }
        }

        public string MaxInput
        {
            get => m_max;
            set
            {
                m_max = value;
                ValidateInputs(nameof(MaxInput));
            }
        }

        public string MinInput
        {
            get => m_min;
            set
            {
                m_min = value;
                ValidateInputs(nameof(MinInput));
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

        public string Title { get; private set; }

        public ObservableCollection<string> InputErrors { get => inputErrors; private set => inputErrors = value; }

        public PartBase SelectedAddPart { get; set; }
        public PartBase SelectedRemovePart { get; set; }

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
                m_productParts.Add(part);

            if (m_productParts.Count() > 0)
                m_errorViewModel.RemoveError(nameof(ProductParts));
            else
                m_errorViewModel.AddError(nameof(ProductParts), c_partError);

            UpdateErrorsList();

        }

        private bool CanRemovePart(object p) => true;
        private void RemovePart(IList p)
        {
            if (p == null)
                return;

            var diaglogResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (diaglogResult != MessageBoxResult.Yes)
                return;

            var selectedParts = p.Cast<PartBase>().ToList();
            for(int i = 0; i < selectedParts.Count(); i++)
            {
                m_productParts.Remove(selectedParts[i]);
            }

            if (m_productParts.Count() == 0)
                m_errorViewModel.AddError(nameof(ProductParts), c_partError);
            else
                m_errorViewModel.RemoveError(nameof(ProductParts));

            UpdateErrorsList();
        }

        private bool CanSaveProduct(object p) => !m_errorViewModel.HasErrors;
        private void SaveProduct(Window window)
        {
            if (window is null)
                return;

            var associatedParts = ProductParts.Select(p => (PartBase)p).Select(p => p.PartID);

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
            var filteredParts = allParts
                .Where(part => searchText is null 
                || part.Name.Contains(searchText)
                || part.PartID.ToString().Contains(searchText));

            foreach (var part in filteredParts)
            {
                m_candidateParts.Add(part);
            }
            OnPropertyChanged(nameof(CandidateParts));
            OnPropertyChanged(nameof(CandidatePartsTableName));
        }

        private void ValidateInputs(string propName)
        {
            switch (propName)
            {
                case nameof(NameInput):
                    {
                        m_errorViewModel.RemoveError(nameof(NameInput));
                        if (m_name is null || m_name.Trim().Length == 0)
                            m_errorViewModel.AddError(nameof(NameInput), "Name must contain a valid value.");
                        break;
                    }
                case nameof(InventoryInput):
                case nameof(MinInput):
                case nameof(MaxInput):
                    {
                        m_errorViewModel.RemoveError(nameof(InventoryInput));
                        m_errorViewModel.RemoveError(nameof(MinInput));
                        m_errorViewModel.RemoveError(nameof(MaxInput));

                        if (!int.TryParse(m_min, out var min) || min <= 0)
                            m_errorViewModel.AddError(nameof(MinInput), "Min must be at above 0.");

                        if (!int.TryParse(m_max, out var max) || max <= 0)
                            m_errorViewModel.AddError(nameof(MaxInput), "Max must be above 0.");

                        if (min > max)
                        {
                            m_errorViewModel.AddError(nameof(MinInput), "Min must be less than or equal to Max");
                            m_errorViewModel.AddError(nameof(MaxInput), "Min must be less than or equal to Max");
                        }

                        if (!int.TryParse(m_inventory, out var inventory) || inventory <= 0)
                            m_errorViewModel.AddError(nameof(InventoryInput), "Inventory must be above 0");
                        else if (!ValidationUtility.IsInRange(inventory, min, max))
                            m_errorViewModel.AddError(nameof(InventoryInput), "Inventory must be within Min and Max");

                        OnPropertyChanged(nameof(InventoryInput));
                        OnPropertyChanged(nameof(MinInput));
                        OnPropertyChanged(nameof(MaxInput));
                        break;
                    }
                case nameof(PriceInput):
                    {
                        m_errorViewModel.RemoveError(nameof(PriceInput));

                        var isDecimal = ValidationUtility.ValueIsDecimalFormat(m_price);
                        if (!isDecimal)
                            m_errorViewModel.AddError(nameof(PriceInput), "Price must be in decimal format.");
                        else if (!float.TryParse(m_price, out var price) || price <= 0)
                            m_errorViewModel.AddError(nameof(PriceInput), "Price must be above 0");
                        break;
                    }
            }

            UpdateErrorsList();
        }

        private void UpdateErrorsList()
        {
            InputErrors.Clear();
            var allErrors = m_errorViewModel.GetAllErrors();
            foreach (var error in allErrors)
                InputErrors.Add(error);
        }

        readonly ErrorViewModel m_errorViewModel;
        readonly ProductStore m_productStore;
        readonly PartStore m_partStore;

        const string c_partError = "Product requires at least 1 part.";

        ObservableCollection<TableItem> m_candidateParts;
        ObservableCollection<TableItem> m_productParts;
        ObservableCollection<string> inputErrors;

        string m_name;
        string m_inventory;
        string m_price;
        string m_max;
        string m_min;
        
    }
}
