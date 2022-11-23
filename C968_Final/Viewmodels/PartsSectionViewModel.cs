﻿using C968_Final.Commands;
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

            AddTableItemCommand = new RelayCommand<object>(AddItem, CanAddItem);
            EditTableItemCommand = new RelayCommand<PartBase>(EditItem, CanEditItem);
            DeleteTableItemCommand = new RelayCommand<PartBase>(DeleteItem, CanDeleteItem);
            SearchTableItemsCommand = new RelayCommand<string>(SearchItems, CanSearch);
            m_inventory = inventory;
            inventory.InventoryUpdated += Inventory_InventoryUpdated;
        }

        private void Inventory_InventoryUpdated(object sender, EventArgs e)
        {
            RefreshTableItems();
            OnPropertyChanged(nameof(ShowErrorMessage));
        }

        public RelayCommand<object> AddTableItemCommand { get; set; }
        public RelayCommand<PartBase> EditTableItemCommand { get; set; }
        public RelayCommand<PartBase> DeleteTableItemCommand { get; set; }
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

        public bool ShowErrorMessage
        {
            get
            {
                var selectedPart = (PartBase)m_selectedTableItem;
                if (selectedPart is null)
                    return false;
                
                return !m_inventory.CanRemovePart(selectedPart.PartID); 
            }
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

                OnPropertyChanged(nameof(ShowErrorMessage));
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

            if(!m_inventory.CanRemovePart(part.PartID))
            {
                var messageBox = MessageBox.Show("Unable to delete. This part is associated with a Product.", "Delete Error", MessageBoxButton.OK);
                return;
            }

            var diaglogResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (diaglogResult != MessageBoxResult.Yes)
                return;

            m_inventory.DeletePart(part);
            SelectedTableItem = null;
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
            OnPropertyChanged(nameof(ShowErrorMessage));
        }

        readonly ObservableCollection<TableItem> m_tableItems;
        readonly Inventory m_inventory;

        TableItem m_selectedTableItem;
    }
}