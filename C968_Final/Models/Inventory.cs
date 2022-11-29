using C968_Final.Stores;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C968_Final.Models
{
    public sealed class Inventory
    {
        public Inventory(ProductStore productStore, PartStore partStore)
        {
            m_productStore = productStore;
            m_partStore = partStore;
        }

        public event EventHandler InventoryUpdated;
        private void OnInventoryUpdated(EventArgs e)
        {
            InventoryUpdated?.Invoke(this, e);
        }

        public BindingList<Product> Products
        {
            get => new BindingList<Product>(m_productStore.GetProducts().ToList());
            set
            {
                foreach (var product in value)
                    AddProduct(product);
            }
        }
        public BindingList<Part> AllParts
        {
            get => new BindingList<Part>(m_partStore.GetParts().Select(p => (Part)p).ToList());
            set
            {
                foreach (var part in value)
                    AddPart(part);
            }
        }
        public int NextPartId => m_partStore.NextId;
        public int NextProductId => m_productStore.NextId;

        public void AddProduct(Product productToAdd)
        {
            m_productStore.AddProduct(productToAdd);
            OnInventoryUpdated(new EventArgs());
        }

        public bool RemoveProduct(int productId)
        {
            var wasSuccessful = m_productStore.DeleteProduct(productId);
            OnInventoryUpdated(new EventArgs());
            return wasSuccessful;
        }

        public void UpdateProduct(int productId, Product productToUpdate)
        {
            productToUpdate.ProductID = productId;
            m_productStore.UpdateProduct(productId, productToUpdate);
            OnInventoryUpdated(new EventArgs());
        }

        public Product LookupProduct(int productId) => m_productStore.GetProduct(productId);
        
        public bool CanRemoveProduct(int productId) => m_productStore.CanRemoveProduct(productId);

        public void AddPart(Part partToAdd)
        {
            m_partStore.AddPart((PartBase)partToAdd);
            OnInventoryUpdated(new EventArgs());
        }

        public bool DeletePart(Part partToRemove)
        {
            var wasSuccessful = m_partStore.DeletePart(partToRemove.PartID);
            m_productStore.RemovePart(partToRemove.PartID);
            OnInventoryUpdated(new EventArgs());
            return wasSuccessful;
        }

        public void UpdatePart(int partId, Part partToUpdate)
        {
            partToUpdate.PartID = partId;
            m_partStore.UpdatePart(partId, (PartBase)partToUpdate);
            OnInventoryUpdated(new EventArgs());
        }

        public Part LookupPart(int partId) => m_partStore.GetPart(partId);

        public List<Part> LookupParts(List<int> partIds) => m_partStore.GetParts(partIds).Select(p => (Part) p).ToList();

        readonly ProductStore m_productStore;
        readonly PartStore m_partStore;
    }
}
