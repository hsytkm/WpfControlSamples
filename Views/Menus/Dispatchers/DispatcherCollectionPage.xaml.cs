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
        #region ObservableCollection<T>
        public ObservableCollection<string> Items1 { get; } =
            new ObservableCollection<string>(Enumerable.Range(0, 3).Select(x => GetData(x)).Reverse());

        public ICommand AddItem1OnUIThreadCommand =>
            _addItem1OnUIThreadCommand ??= new MyCommand(() => Items1.Insert(0, GetData(Items1.Count)));
        private ICommand _addItem1OnUIThreadCommand;

        public ICommand AddItem1OnTaskCommand =>
            _addItem1OnTaskCommand ??= new MyCommand(async () =>
            {
                // Task(UIスレッド以外)からコレクション操作するので例外が発生する
                await Task.Run(() =>
                {
                    try
                    {
                        //Items1.Insert(0, GetData(Items1.Count)); 例外が発生すると以降のコレクション操作できないので無効化
                    }
                    catch (Exception ex)
                    {
                        // System.NotSupportedException: この型の CollectionView は、
                        // Dispatcher スレッドとは異なるスレッドからその SourceCollection への変更をサポートしません。
                        Debug.WriteLine(ex);
                    }
                });
            });
        private ICommand _addItem1OnTaskCommand;

        public ICommand AddItem1OnTaskUseDispatcherCommand =>
            _addItem1OnTaskUseDispatcherCommand ??= new MyCommand(async () =>
            {
                // Task(UIスレッド以外)からコレクション操作する場合は、Dispatcher にお願いする
                var d = Dispatcher.CurrentDispatcher;
                await Task.Run(() =>
                {
                    d.Invoke(() =>
                    {
                        Items1.Insert(0, GetData(Items1.Count));
                    });
                });
            });
        private ICommand _addItem1OnTaskUseDispatcherCommand;

        #endregion

        #region DispatchObservableCollection<T>

        public DispatchObservableCollection<string> Items2 { get; } =
            new DispatchObservableCollection<string>(Enumerable.Range(0, 3).Select(x => GetData(x)).Reverse());

        public ICommand AddItem2OnUIThreadCommand =>
            _addItem2OnUIThreadCommand ??= new MyCommand(() => Items2.Insert(0, GetData(Items2.Count)));
        private ICommand _addItem2OnUIThreadCommand;

        public ICommand AddItem2OnTaskCommand =>
            _addItem2OnTaskCommand ??= new MyCommand(async () =>
            {
                // Task(UIスレッド以外)からコレクション操作するので例外が発生する
                await Task.Run(() =>
                {
                    try
                    {
                        Items2.Insert(0, GetData(Items2.Count));
                    }
                    catch (Exception ex)
                    {
                        // System.NotSupportedException: この型の CollectionView は、
                        // Dispatcher スレッドとは異なるスレッドからその SourceCollection への変更をサポートしません。
                        Debug.WriteLine(ex);
                    }
                });
            });
        private ICommand _addItem2OnTaskCommand;

        public ICommand AddItem2OnTaskUseDispatcherCommand =>
            _addItem2OnTaskUseDispatcherCommand ??= new MyCommand(async () =>
            {
                // Task(UIスレッド以外)からコレクション操作する場合は、Dispatcher にお願いする
                var d = Dispatcher.CurrentDispatcher;
                await Task.Run(() =>
                {
                    d.Invoke(() =>
                    {
                        Items2.Insert(0, GetData(Items2.Count));
                    });
                });
            });
        private ICommand _addItem2OnTaskUseDispatcherCommand;

        #endregion

        private static string GetData(int x) => "Data" + x.ToString();
    }
}