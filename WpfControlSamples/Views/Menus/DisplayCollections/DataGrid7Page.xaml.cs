using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;
using WpfControlSamples.ViewModels;

namespace WpfControlSamples.Views.Menus
{
    public partial class DataGrid7Page : ContentControl
    {
        public DataGrid7Page()
        {
            DataContext = new DataGrid7ViewModel();
            InitializeComponent();
        }
    }

    class DataGrid7ViewModel : MyBindableBase
    {
        public ObservableCollection<DataGridProduct> Products { get; } = new(DataGridProduct.Products);
    }
}
