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
    public partial class ListBox3Page : ContentControl
    {
        public ListBox3Page()
        {
            InitializeComponent();
            DataContext = new ListBox3ViewModel();
        }
    }

    class ListBox3ViewModel : MyBindableBase
    {
        private const int DefaultItemCount = 8;

        /// <summary>表示コレクションの元情報</summary>
        public static readonly IList<ColorListViewItem> _sourceColors =
            Models.SampleData.WpfColors.Select(x => new ColorListViewItem(x)).ToList();

        private int _referenceCounter = DefaultItemCount;
        private ColorListViewItem GetColorListViewItem()
        {
            _referenceCounter = (_referenceCounter + 1) % _sourceColors.Count;
            return _sourceColors[_referenceCounter];
        }

        /// <summary>View表示用コレクション</summary>
        public ObservableCollection<ColorListViewItem> Items { get; } =
            new ObservableCollection<ColorListViewItem>(_sourceColors.Take(DefaultItemCount));

        /// <summary>View選択要素の通知用コレクション</summary>
        public ObservableCollection<object> NotifySelectedColors { get; } = new ObservableCollection<object>();

        /// <summary>先頭に新規要素を追加する</summary>
        public ICommand AddItemHeadCommand =>
            _addItemHeadCommand ??= new MyCommand(() => Items.Insert(0, GetColorListViewItem()));
        private ICommand _addItemHeadCommand;

        /// <summary>末尾に新規要素を追加する</summary>
        public ICommand AddItemTailCommand =>
            _addItemTailCommand ??= new MyCommand(() => Items.Add(GetColorListViewItem()));
        private ICommand _addItemTailCommand;

        /// <summary>選択中の要素をコレクションから削除する</summary>
        public ICommand DeleteSelectedItemsCommand =>
            _deleteSelectedItemsCommand ??= new MyCommand(() =>
            {
                // 削除中にコレクションの変更通知が来てコレクション要素数変わっちゃうので配列化しとく
                var deleteItems = NotifySelectedColors.Cast<ColorListViewItem>().ToArray();
                foreach (var item in deleteItems)
                {
                    if (Items.Contains(item))
                        Items.Remove(item);
                }
            });
        private ICommand _deleteSelectedItemsCommand;

    }
}
