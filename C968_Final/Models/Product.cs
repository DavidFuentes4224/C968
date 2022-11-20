using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C968_Final.Models
{
    public class Product : TableItem
    {
        // AssociatedParts is not used.
        // AssociatedPartIds is a better approach as changes to parts can propagate to other Products automatically.
        public BindingList<Part> AssociatedParts { get; set; }
        public List<int> AssociatedPartIds { get; set; }
        public int ProductID { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int InStock { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public string DisplayID { get => ProductID.ToString(); }

        public void AddAssociatedPart(Part part) => AssociatedPartIds.Add(part.PartID);
        public bool RemoveAssociatedPart(int partId) => AssociatedPartIds.Remove(partId);
        public Part LookupAssociatedPart(int partId) => AssociatedParts.FirstOrDefault(p => p.PartID == partId);
    }
}
