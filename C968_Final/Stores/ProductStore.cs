using C968_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C968_Final.Stores
{
    public sealed class ProductStore
    {
        public ProductStore()
        {
            m_productId = 0;

            m_productByProductId = new Dictionary<int, Product>();
        }

        public int NextId { get => m_productId; }

        public void AddProduct(Product newProduct)
        {
            newProduct.ProductID = m_productId;
            m_productByProductId.Add(m_productId, newProduct);
            m_productId++;
        }

        public void UpdateProduct(int id, Product newProduct) => m_productByProductId[id] = newProduct;

        public bool DeleteProduct(int id) => m_productByProductId.Remove(id);

        public Product GetProduct(int productId) =>
            m_productByProductId.TryGetValue(productId, out var product) ? product : null;

        public bool CanRemoveProduct(int productid) => !m_productByProductId[productid].AssociatedPartIds.Any();

        public IReadOnlyList<Product> GetProducts() => m_productByProductId.Values.ToList();

        public void RemovePart(int partID)
        {
            foreach (var product in GetProducts())
                product.RemoveAssociatedPart(partID);
        }

        Dictionary<int, Product> m_productByProductId;
        int m_productId;
    }
}
