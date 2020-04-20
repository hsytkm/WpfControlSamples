using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace WpfControlSamples.Views.Menus
{
    public partial class CollectionViewPage : ContentControl
    {
        public CollectionViewPage()
        {
            InitializeComponent();

            DataContext = new CollectionViewViewModel();
        }
    }

    class CollectionViewViewModel : MyBindableBase
    {
        public IList<ColorListViewItem> SourceColors { get; } =
            Models.SampleData.WpfColors.Select(x => new ColorListViewItem(x)).ToList();

        #region Filter
        public ICommand FilterCommand => _filterCommand ??
            (_filterCommand = new MyCommand<string>(name =>
            {
                var collectionView = CollectionViewSource.GetDefaultView(SourceColors);
                if (string.IsNullOrEmpty(name))
                {
                    collectionView.Filter = null;   // clear
                }
                else
                {
                    collectionView.Filter = x =>
                    {
                        var color = (ColorListViewItem)x;
                        return color.Name.Contains(name);
                    };
                }
            }));
        private ICommand _filterCommand;
        #endregion

        #region Sort
        public ICommand SortNameCommand => _sortNameCommand ??
            (_sortNameCommand = new MyCommand<int?>(pattern => SortDescription(nameof(ColorListViewItem.Name), pattern)));
        private ICommand _sortNameCommand;

        public ICommand SortHueCommand => _sortHueCommand ??
            (_sortHueCommand = new MyCommand<int?>(pattern => SortDescription(nameof(ColorListViewItem.Hue), pattern)));
        private ICommand _sortHueCommand;

        private void SortDescription(string name, int? pattern)
        {
            ListSortDirection? direction = null;
            if (pattern.HasValue)
                direction = (pattern.Value == 0) ? ListSortDirection.Ascending : ListSortDirection.Descending;

            var collectionView = CollectionViewSource.GetDefaultView(SourceColors);
            collectionView.SortDescriptions.Clear();    // 必ずクリア

            if (direction.HasValue)
            {
                collectionView.SortDescriptions.Add(new SortDescription(name, direction.Value));
            }
        }
        #endregion

        #region Group
        public ICommand GroupCommand => _groupCommand ??
            (_groupCommand = new MyCommand<int?>(groupPattern =>
            {
                var collectionView = CollectionViewSource.GetDefaultView(SourceColors);

                collectionView.GroupDescriptions.Clear();
                if (groupPattern.HasValue)
                {
                    // Hueプロパティの10の位の値でグルーピングする
                    collectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(ColorListViewItem.Hue), new HueGroupConverter()));
                }
            }));
        private ICommand _groupCommand;
        #endregion

    }

    class HueGroupConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hue = (int)Math.Round((double)value);
            return (hue / 10) * 10;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}