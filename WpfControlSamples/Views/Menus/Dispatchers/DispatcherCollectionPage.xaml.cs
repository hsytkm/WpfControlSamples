#nullable disable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;
using WpfControlSamples.ViewModels;

namespace WpfControlSamples.Views.Menus
{
    public partial class DispatcherCollectionPage : ContentControl
    {
        public DispatcherCollectionPage()
        {
            InitializeComponent();
            DataContext = new DispatcherCollectionViewModel();
        }
    }

    class DispatcherCollectionViewModel : MyBindableBase
    {
        private static string GetData(int x) => $"Data{x}";

        private readonly static IReadOnlyList<string> _itemsSource = Enumerable.Range(0, 2).Select(x => GetData(x)).Reverse().ToArray();
        public ObservableCollection<string> Items1 { get; } = new(_itemsSource);
        public DispatchObservableCollection<string> Items2 { get; } = new(_itemsSource);
        public ObservableCollection<string> Items3 { get; } = new(_itemsSource);

        private void AddItem(int x)
        {
            var items = x switch
            {
                1 => Items1,
                2 => Items2,
                3 => Items3,
                _ => throw new NotImplementedException()
            };
            items.Insert(0, GetData(items.Count));
        }

        public ICommand AddItemOnUIThreadCommand => _addItemOnUIThreadCommand ??= new MyCommand<int>(x => AddItem(x));
        private ICommand _addItemOnUIThreadCommand;

        public ICommand AddItemOnTaskCommand => _addItemOnTaskCommand ??= new MyCommand<int>(async x =>
        {
            // Task(UIスレッド以外)からコレクション操作するので例外が発生する
            await Task.Run(() =>
            {
                try
                {
                    // 例外が発生して追加できない
                    AddItem(x);
                }
                catch (Exception ex)
                {
                    // System.NotSupportedException: この型の CollectionView は、
                    // Dispatcher スレッドとは異なるスレッドからその SourceCollection への変更をサポートしません。
                    Debug.WriteLine(ex);
                    MessageBox.Show(ex.Message);
                }
            });
        });
        private ICommand _addItemOnTaskCommand;

        public ICommand AddItemOnTaskUseDispatcherCommand => _addItemOnTaskUseDispatcherCommand ??= new MyCommand<int>(async x =>
        {
            // Task(UIスレッド以外)からコレクション操作する場合は、Dispatcher にお願いする
            var d = Dispatcher.CurrentDispatcher;   // Task外でインスタンスを取得するのがポイント
            await Task.Run(() => d.Invoke(() => AddItem(x)));
        });
        private ICommand _addItemOnTaskUseDispatcherCommand;

        public DispatcherCollectionViewModel()
        {
            // WPF 4.5の新機能「複数スレッドからのコレクションの操作」 https://blog.okazuki.jp/entry/20120520/1337503048
            // Items に対してUIスレッド以外から要素を変更しても大丈夫になる。
            // （自作の DispatchObservableCollection<T> が不要になる）
            BindingOperations.EnableCollectionSynchronization(Items3, lockObject: new object());
        }
    }
}
