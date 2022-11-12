using C968_Final.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C968_Final.Viewmodels
{
    class MainScreenViewModel : ViewModelBase
    {
        public MainScreenViewModel()
        {
            PartsViewModel = new TableSectionViewModel() { TableName = "Parts" };
            ProductsViewModel = new TableSectionViewModel();
        }

        public TableSectionViewModel PartsViewModel { get; }
        public TableSectionViewModel ProductsViewModel { get; }
    }
}
