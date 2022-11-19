using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C968_Final.Models
{
    public sealed class OutsourcedPart : PartBase
    {
        public OutsourcedPart(string companyName, PartBase part)
        {
            CompanyName = companyName;
            PartID = part.PartID;
            Name = part.Name;
            Price = part.Price;
            InStock = part.InStock;
            Min = part.Min;
            Max = part.Max;
           }

        public string CompanyName { get; set; }
    }
}
