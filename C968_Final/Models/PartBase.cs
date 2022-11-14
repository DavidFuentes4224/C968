using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C968_Final.Models
{
    public class PartBase : TableItem
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int InStock { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
