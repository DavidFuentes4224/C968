using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C968_Final.Models
{
    class InhousePart : PartBase
    {
        public InhousePart(int machineId, PartBase part)
        {
            MachineId = machineId;
            Id = part.Id;
            Name = part.Name;
            Price = part.Price;
            InStock = part.InStock;
            Min = part.Min;
            Max = part.Max;
        }

        public int MachineId { get; set; }
    }
}
