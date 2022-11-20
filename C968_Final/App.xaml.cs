using C968_Final.Models;
using C968_Final.Stores;
using C968_Final.Viewmodels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace C968_Final
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            m_inventory = new Inventory(new ProductStore(), new PartStore());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var partsSection = new PartsSectionViewModel(m_inventory);
            var productsSection = new ProductsSectionViewModel(m_inventory);

            MainWindow = new MainWindow(){ DataContext = new MainScreenViewModel(partsSection, productsSection) };
            MainWindow.Show();
            base.OnStartup(e);
        }

        readonly Inventory m_inventory;
    }
}
