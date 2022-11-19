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
using System.Windows.Controls;

namespace C968_Final.Viewmodels
{
    public class PartViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        public PartViewModel(bool isInhouse, PartBase model, PartStore partStore, TableActions.Action action)
        {
            if (action == TableActions.Action.ADD)
                Title = "Add Part";
            else
                Title = "Update Part";

            m_partStore = partStore;
            m_errorViewModel = new ErrorViewModel();
            InputErrors = new ObservableCollection<string>();

            IsInHouse = isInhouse;

            Id = model.PartID.ToString();
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

        public string Title { get; private set; }
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

        public string MachineIdInput
        {
            get => m_machineId;
            set
            {
                m_machineId = value;
                ValidateInputs(nameof(MachineIdInput));
            }
        }
        public string CompanyNameInput
        {
            get => m_comanyName;
            set
            {
                m_comanyName = value;
                ValidateInputs(nameof(CompanyNameInput));
            }
        }

        public bool IsInHouse { get; set; }
        public Visibility InHouseVisible { get; set; }
        public Visibility OutsourcedVisible { get; set; }
        public ObservableCollection<string> InputErrors { get => inputErrors; private set => inputErrors = value; }

        public bool HasErrors => HasErrorsCore();
        public IEnumerable GetErrors(string propertyName) => m_errorViewModel.GetErrors(propertyName);

        public bool CanSavePart(Window p) => !HasErrors;
        public void SavePart(Window window)
        {
            if (window is null)
                return;

            var part = new PartBase()
            {
                Name = NameInput,
                Price = decimal.Parse(PriceInput),
                Max = int.Parse(MaxInput),
                Min = int.Parse(MinInput),
                InStock = int.Parse(InventoryInput)
            };

            // Update part.
            if (int.TryParse(Id, out var id))
            {
                part.PartID = id;

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

        private bool HasErrorsCore() => m_errorViewModel.HasErrors;

        private void ValidateInputs(string propName)
        {
            switch(propName)
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
                case nameof(CompanyNameInput):
                case nameof(MachineIdInput):
                    {
                        m_errorViewModel.RemoveError(nameof(CompanyNameInput));
                        m_errorViewModel.RemoveError(nameof(MachineIdInput));

                        if (IsInHouse)
                        {
                            if (!int.TryParse(m_machineId, out var machineId) || machineId <= 0)
                                m_errorViewModel.AddError(nameof(MachineIdInput), "Machine ID must contain be above 0.");
                        }
                        else
                        {
                            if (!(m_comanyName.Trim().Length > 0))
                                m_errorViewModel.AddError(nameof(CompanyNameInput), "Company ID must contain an valid value.");
                        }
                        break;
                    }
            }

            InputErrors.Clear();
            var allErrors = m_errorViewModel.GetAllErrors();
            foreach (var error in allErrors)
                InputErrors.Add(error);
        }

        readonly PartStore m_partStore;
        readonly ErrorViewModel m_errorViewModel;

        const string c_inHouse = "InHouse";

        string m_name;
        string m_inventory;
        string m_price;
        string m_max;
        string m_min;
        string m_machineId;
        string m_comanyName;
        private ObservableCollection<string> inputErrors;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}
