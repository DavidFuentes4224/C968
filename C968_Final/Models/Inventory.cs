using C968_Final.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C968_Final.Models
{
    public sealed class Inventory
    {

        public Inventory(ProductStore productStore, PartStore partStore)
        {
            this.productStore = productStore;
        }

        public List<Product> Products { get; set; }
        public List<PartBase> AllParts { get; set; }

        public void AddProduct(Product productToAdd)
        {
            throw new NotImplementedException();
        }

        public void RemoveProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public Product LookupProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(int productId, Product productToUpdate)
        {
            throw new NotImplementedException();
        }

        public void AddPart(PartBase partToAdd)
        {
            throw new NotImplementedException();
        }

        public void DeletePart(PartBase partToRemove)
        {
            throw new NotImplementedException();
        }

        public PartBase LookupPart(PartBase partId)
        {
            throw new NotImplementedException();
        }

        public void UpdatePart(int partId, PartBase partToUpdate)
        {
            throw new NotImplementedException();
        }

        private readonly ProductStore productStore;
    }
}
