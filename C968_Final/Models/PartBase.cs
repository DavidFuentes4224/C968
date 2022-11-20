using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C968_Final.Models
{
    public class PartBase : Part
    {
        public string DisplayID { get => PartID.ToString(); }
    }
}
