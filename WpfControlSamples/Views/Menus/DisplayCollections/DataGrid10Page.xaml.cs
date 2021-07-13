using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
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
using WpfControlSamples.Models;
using WpfControlSamples.ViewModels;
using WpfControlSamples.Views.Behaviors;

namespace WpfControlSamples.Views.Menus
{
    public partial class DataGrid10Page : ContentControl
    {
        public DataGrid10Page()
        {
            DataContext = new DataGrid10ViewModel();
            InitializeComponent();
        }
    }

    class DataGrid10ViewModel : MyBindableBase
    {
        public IReadOnlyList<NameMessageRecord> Collection
            => SampleData.WpfColors.Select(x => new NameMessageRecord(x.Name, x.Color.ToString())).ToArray();

    }

    record NameMessageRecord(string Name, string Message);
}
