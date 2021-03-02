using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using System.Windows.Threading;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class FluidMoveBehavior6Page : ContentControl
    {
        private readonly ObservableCollection<FluidMoveBehavior6SubItem> _viewItemsSource;

        public FluidMoveBehavior6Page()
        {
            InitializeComponent();

            _viewItemsSource = CreateViewItemsSource();
            DataContext = _viewItemsSource;
        }

        private ObservableCollection<FluidMoveBehavior6SubItem> CreateViewItemsSource()
        {
            var itemsCount = 3;
            var source = Enumerable.Range(0, itemsCount).Select(x => new FluidMoveBehavior6SubItem());
            var items = new ObservableCollection<FluidMoveBehavior6SubItem>(source);

            RenumberItemsIndex(items);

            // 初期メンは3色を指定
            items[0].Background = Brushes.LightBlue;
            items[1].Background = Brushes.LightPink;
            items[2].Background = Brushes.LightGreen;

            return items;
        }

        private static void RenumberItemsIndex(ObservableCollection<FluidMoveBehavior6SubItem> items)
        {
            var itemsCount = items.Count;
            for (int i = 0; i < itemsCount; ++i)
            {
                items[i].ItemIndex = i;
                items[i].ItemsCount = itemsCount;

                var from = i;
                var to = (from + 1) % itemsCount;
                items[i].SwapItemCommand = new MyCommand(() => SwapItemsSource(items, from, to));
            }
        }

        private static void SwapItemsSource(ObservableCollection<FluidMoveBehavior6SubItem> items, int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || items.Count <= fromIndex) return;
            if (toIndex < 0 || items.Count <= toIndex) return;
            if (fromIndex == toIndex) return;

            var item = items[fromIndex];
            items.Remove(item);
            items.Insert(toIndex, item);

            // コレクションを入れ替えたら番号を付け直す
            RenumberItemsIndex(items);
        }

        public ICommand AddItemCommand => _addItemCommand ??= new MyCommand(() =>
        {
            if (_viewItemsSource.Count >= 5) return;

            var brushes = Models.SampleData.WpfSolidColorBrushes;
            var random = new Random().Next(0, brushes.Count);
            var item = new FluidMoveBehavior6SubItem
            {
                Background = brushes[random].Brush
            };

            _viewItemsSource.Add(item);

            // コレクションの追加時に番号を付け直す
            RenumberItemsIndex(_viewItemsSource);
        });
        private ICommand _addItemCommand;

        public ICommand DeleteLastItemCommand => _deleteLastItemCommand ??= new MyCommand(() =>
        {
            if (_viewItemsSource.Count <= 0) return;

            // ◆削除することで、基準機種 RadioButton が全て OFF になるけど未対応
            _viewItemsSource.RemoveAt(_viewItemsSource.Count - 1);
            RenumberItemsIndex(_viewItemsSource);
        });
        private ICommand _deleteLastItemCommand;


        // 1つの RadioButton だけ true にする対応。ダサい。もうちょっとカッコイイ実装ない？
        private void MyItemsControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModels = _viewItemsSource.Select(x => x.DataContext)
                .OfType<FluidMoveBehavior6SubItemViewModel>().ToList();

            viewModels[0].IsReferenceModel = true;

            for (int i = 2; i < viewModels.Count - 1; ++i)
            {
                viewModels[i].IsEnabled = false;
            }
        }

    }
}
