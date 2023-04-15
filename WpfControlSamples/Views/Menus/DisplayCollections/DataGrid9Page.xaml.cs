using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using WpfControlSamples.Infrastructures;
using WpfControlSamples.Models;
using WpfControlSamples.ViewModels;
using WpfControlSamples.Views.Behaviors;

namespace WpfControlSamples.Views.Menus
{
    public partial class DataGrid9Page : ContentControl
    {
        public DataGrid9Page()
        {
            DataContext = new DataGrid9ViewModel();
            InitializeComponent();
        }
    }

    class DataGrid9ViewModel : MyBindableBase
    {
        public ReadOnlyCollectionWithSelectedItem<ColorItemsPageViewModel> ColorItemsCollection
            => ReadOnlyCollectionWithSelectedItem.Create(CreateCollection());

        private static IEnumerable<ColorItemsPageViewModel> CreateCollection()
        {
            var items = new ColorItemsPageViewModel[]
            {
                FilterByName(SampleData.WpfColors, "Red"),
                FilterByName(SampleData.WpfColors, "Green"),
                FilterByName(SampleData.WpfColors, "Blue"),
            };

            var all = new ColorItemsPageViewModel("RGB", items.SelectMany(x => x.Collection));
            return items.Prepend(all);

            static ColorItemsPageViewModel FilterByName(IList<(string Name, Color Color)> itemsSource, string name)
                => new(name, itemsSource.Where(x => x.Name.Contains(name)).Select(x => new ColorItemViewModel(x.Name, x.Color)));
        }
    }

    /// <summary>DataGrid の ItemsSource になります</summary>
    record ColorItemsPageViewModel : ICompositeColoredTextCollection<ColorItemViewModel>
    {
        public string Name { get; }
        public IImmutableList<ColorItemViewModel> Collection { get; }

        public ColorItemsPageViewModel(string name, IEnumerable<ColorItemViewModel> items)
        {
            Name = name;
            Collection = ImmutableArray.CreateRange(items);
        }
    }

    sealed class ColorItemsTabControlFilterWordBehavior : TabControlFilterWordBehavior<ColorItemViewModel> { }
}
