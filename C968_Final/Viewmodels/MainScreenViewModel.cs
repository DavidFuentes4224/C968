using C968_Final.Models;
using C968_Final.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C968_Final.Viewmodels
{
    public class MainScreenViewModel : ViewModelBase
    {
        public MainScreenViewModel(
            PartsSectionViewModel partsViewModel, 
            ProductsSectionViewModel productsViewModel)
        {
            PartsViewModel = partsViewModel;
            ProductsViewModel = productsViewModel;
        }

        public PartsSectionViewModel PartsViewModel { get; }
        public ProductsSectionViewModel ProductsViewModel { get; }
    }
}
