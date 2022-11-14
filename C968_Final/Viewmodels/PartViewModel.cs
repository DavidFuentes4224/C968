using C968_Final.Commands;
using C968_Final.Models;
using C968_Final.Stores;
using C968_Final.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace C968_Final.Viewmodels
{
    public class PartViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        public PartViewModel(bool isInhouse, PartBase model, PartStore partStore)
        {
            m_partStore = partStore;
            m_errorViewModel = new ErrorViewModel();
            
            IsInHouse = isInhouse;

            Id = model.Id.ToString();
            NameInput = model.Name;
            InventoryInput = model.InStock.ToString();
            var priceString = model.Price.ToString();
            if (priceString is object)
                priceString = String.Format("{0:0.00}", model.Price);
            PriceInput = priceString;
            MaxInput = model.Max.ToString();
            MinInput = model.Min.ToString();

            if (model is InhousePart inhousePart)
                MachineIdInput = inhousePart.MachineId.ToString();
            else
                MachineIdInput = "";

            if (model is OutsourcedPart outsourcedPart)
                CompanyNameInput = outsourcedPart.CompanyName;
            else
                CompanyNameInput = "";

            var converter = new BooleanToVisibilityConverter();
            InHouseVisible = isInhouse ? Visibility.Visible : Visibility.Hidden;
            OutsourcedVisible = isInhouse ? Visibility.Hidden : Visibility.Visible;

            SavePartCommand = new RelayCommand<Window>(SavePart, CanSavePart);
            RadioCheckedCommand = new RelayCommand<string>(OnRadioChecked, CanCheckRadio);
        }

        public RelayCommand<Window> SavePartCommand { get; private set; }
        public RelayCommand<string> RadioCheckedCommand { get; private set; }

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

        public string MachineIdInput
        {
            get => m_machineId;
            set
            {
                m_machineId = value;

                m_errorViewModel.RemoveError(nameof(MachineIdInput));
                if (IsInHouse)
                {
                    if (!int.TryParse(m_machineId, out var machineId) || machineId <= 0)
                        m_errorViewModel.AddError(nameof(MachineIdInput), "Machine ID must contain an int.");
                }
            }
        }
        public string CompanyNameInput
        {
            get => m_comanyName;
            set
            {
                m_comanyName = value;

                m_errorViewModel.RemoveError(nameof(CompanyNameInput));
                if (!(m_comanyName.Trim().Length > 0))
                    m_errorViewModel.AddError(nameof(CompanyNameInput), "Company ID must contain an int.");

            }
        }


        public bool IsInHouse { get; set; }
        public Visibility InHouseVisible { get; set; }
        public Visibility OutsourcedVisible { get; set; }
        public Visibility MinMaxMessageVisible { get; set; }

        public bool HasErrors => HasErrorsCore();

        private bool HasErrorsCore()
        {
            var erroredProperties = m_errorViewModel.GetErroredProperties();

            // As only one of these fields on this form are visible at a a time, the other will be in error state.
            if (IsInHouse && erroredProperties.Where(p => p != nameof(CompanyNameInput)).Any())
                return true;
            else if (!IsInHouse && erroredProperties.Where(p => p != nameof(MachineIdInput)).Any())
                return true;

            return false ;
        }

            public bool CanSavePart(Window p)
        {
            // Add some save prevention logic
            return !this.HasErrors;
        }
        public void SavePart(Window window)
        {
            if (window is null)
                return;

            var part = new PartBase()
            {
                Name = NameInput,
                Price = float.Parse(PriceInput),
                Max = int.Parse(MaxInput),
                Min = int.Parse(MinInput),
                InStock = int.Parse(InventoryInput)
            };

            // Update part.
            if (int.TryParse(Id, out var id))
            {
                part.Id = id;

                if (int.TryParse(MachineIdInput, out var machineId))
                    m_partStore.UpdatePart(id, new InhousePart(machineId, part));
                else
                    m_partStore.UpdatePart(id, new OutsourcedPart(CompanyNameInput, part));
            }
            else
            {
                if (int.TryParse(MachineIdInput, out var machineId))
                    m_partStore.AddPart(new InhousePart(machineId, part));
                else
                    m_partStore.AddPart(new OutsourcedPart(CompanyNameInput, part));
            }

            window.Close();
        }

        public bool CanCheckRadio(string p) => true;
        public void OnRadioChecked(string radioValue)
        {
            if (radioValue is null)
                return;

            IsInHouse = radioValue == c_inHouse;
            InHouseVisible = radioValue == c_inHouse ? Visibility.Visible : Visibility.Hidden;
            OutsourcedVisible = radioValue == c_inHouse ? Visibility.Hidden : Visibility.Visible;
            OnPropertyChanged(nameof(InHouseVisible));
            OnPropertyChanged(nameof(OutsourcedVisible));

        }

        public IEnumerable GetErrors(string propertyName) => m_errorViewModel.GetErrors(propertyName);

        private void ValidateMinMax()
        {
            var shouldShowMessage = !int.TryParse(m_min, out var min)
                || !int.TryParse(m_max, out var max)
                || min > max;

            MinMaxMessageVisible = shouldShowMessage ? Visibility.Visible : Visibility.Collapsed;
            OnPropertyChanged(nameof(MinMaxMessageVisible));
        }

        readonly PartStore m_partStore;
        readonly ErrorViewModel m_errorViewModel;

        const string c_inHouse = "InHouse";
        const string c_outsource = "Outsourced";

        string m_name;
        string m_inventory;
        string m_price;
        string m_max;
        string m_min;
        string m_machineId;
        string m_comanyName;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}
