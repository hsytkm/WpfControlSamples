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

        // 選択項目の追加/削除の通知
        public ObservableCollection<object> NotifySelectedColors { get; } =
            new ObservableCollection<object>();

        // 選択中のアイテムリスト
        private readonly IList<ColorListViewItem> SelectingColors = new List<ColorListViewItem>();

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
                        SelectingColors.Add(item);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (ColorListViewItem item in e.OldItems)
                    {
                        if (SelectingColors.Contains(item))
                            SelectingColors.Remove(item);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    SelectingColors.Clear();
                    break;
                default:
                    Debug.Assert(true, e.Action.ToString());
                    break;
            }
            SelectedColors = string.Join(", ", SelectingColors.Select(x => x.Name));
        }

    }
}
