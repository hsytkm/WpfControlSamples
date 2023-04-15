#nullable disable
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
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
        public ReadOnlyCollectionWithSelectedItem<ListBox9Item> ItemsSource { get; } =
            new(Models.SampleData.WpfColors.Select(x => new ListBox9Item(x.Name, x.Color)));
    }

    record ListBox9Item
    {
        public string Name { get; }
        public Color Color { get; }
        public ListBox9Item(string name, Color color) => (Name, Color) = (name, color);
    }
}
