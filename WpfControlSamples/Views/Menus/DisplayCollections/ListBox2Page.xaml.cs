#nullable disable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
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

namespace WpfControlSamples.Views.Menus
{
    public partial class ListBox2Page : ContentControl
    {
        public ListBox2Page()
        {
            InitializeComponent();

            DataContext = new ListBox2ViewModel();
        }

        private void SelectAllExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            listBox.SelectAll();
        }

        private void UnselectAllExecuted(object sender, RoutedEventArgs e)
        {
            listBox.UnselectAll();
        }
    }

    class ListBox2ViewModel : MyBindableBase
    {
        public IList<ColorListViewItem> SourceColors { get; } =
            Models.SampleData.WpfColors.Select(x => new ColorListViewItem(x)).ToList();

        public string SelectedColors
        {
            get => _selectedColors;
            private set => SetProperty(ref _selectedColors, value);
        }
        private string _selectedColors;

        public bool IsSelectionModeMultiple
        {
            get => _isSelectionModeMultiple;
            set => SetProperty(ref _isSelectionModeMultiple, value);
        }
        private bool _isSelectionModeMultiple = true;

        // 選択項目の追加/削除の通知
        public ObservableCollection<object> NotifySelectedColors { get; } =
            new ObservableCollection<object>();

        // 選択中のアイテムリスト
        private readonly IList<ColorListViewItem> _selectingColors = new List<ColorListViewItem>();

        public ListBox2ViewModel()
        {
            // ◆ちゃんと使う場合は Dispose で remove しましょう
            NotifySelectedColors.CollectionChanged += SelectedColors_CollectionChanged;
        }

        private void SelectedColors_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (ColorListViewItem item in e.NewItems)
                    {
                        _selectingColors.Add(item);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (ColorListViewItem item in e.OldItems)
                    {
                        if (_selectingColors.Contains(item))
                            _selectingColors.Remove(item);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    _selectingColors.Clear();
                    break;
                default:
                    Debug.Assert(true, e.Action.ToString());
                    break;
            }
            SelectedColors = string.Join(", ", _selectingColors.Select(x => x.Name));
        }

    }
}
