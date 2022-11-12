using C968_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C968_Final.Viewmodels
{
    class ProductViewModel
    {
        public ProductViewModel(Product model)
        {
            m_model = model;
        }

        private Product m_model;
    }
}
