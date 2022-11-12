using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C968_Final.Models
{
    //Used as wrapper class for TableSectionItems
    public class TableItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propName) => PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }
}
