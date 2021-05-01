#nullable disable
using System;
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
using WpfControlSamples.Infrastructures;
using WpfControlSamples.ViewModels;

namespace WpfControlSamples.Views.Menus
{
    public partial class ListBox9Page : ContentControl
    {
        public ListBox9Page()
        {
            DataContext = new ListBox9ViewModel();
            InitializeComponent();
        }
    }

    class ListBox9ViewModel : MyBindableBase
    {
        public CollectionSelectedItemPair<ListBox9Item> ItemsSource { get; } =
            new(Models.SampleData.WpfColors.Select(x => new ListBox9Item(x.Name, x.Color)));
    }

    record ListBox9Item
    {
        public string Name { get; }
        public Color Color { get; }
        public ListBox9Item(string name, Color color) => (Name, Color) = (name, color);
    }
}
