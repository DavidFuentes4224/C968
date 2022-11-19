﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C968_Final.Models
{
    public class Product : TableItem
    {
        public List<int> AssociatedParts { get; set; }
        public int ProductID { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int InStock { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public string DisplayID { get => ProductID.ToString(); }
    }
}
