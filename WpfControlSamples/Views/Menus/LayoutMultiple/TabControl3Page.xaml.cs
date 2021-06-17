#nullable disable
using System;
using System.Collections.Generic;
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
    public partial class TabControl3Page : ContentControl
    {
        public TabControl3Page()
        {
            InitializeComponent();
            DataContext = new TabControl3ViewModel();
        }
    }

    class TabControl3ViewModel : MyBindableBase
    {
        public IReadOnlyList<TabItem> TabContentSource { get; }

        public TabItem SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }
        private TabItem _selectedItem;

        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public string LogMessage
        {
            get => _logMessage;
            private set => SetProperty(ref _logMessage, value);
        }
        private string _logMessage;

        public TabControl3ViewModel()
        {
            TabContentSource = new TabItem[]
            {
                new LazyTabItemForTest("Tab1", typeof(TabContent3), WriteLog),
                new LazyTabItemForTest("タブ2", typeof(TabContent3), WriteLog),
                new LazyTabItemForTest("たぶ3", typeof(TabContent3), WriteLog),
            };
            SelectedItem = TabContentSource[0];
        }

        private void WriteLog(string log)
        {
            var lineCount = _stringBuilder.ToString().Count(c => c == '\n') + 1;    // 遅いけどOK
            _stringBuilder.AppendLine($"{lineCount} : {log}");
            LogMessage = _stringBuilder.ToString();
        }
    }

    class TabContent3 : UserControl, IDisposable
    {
        public void Dispose() { }
    }

    // こちらは用意したくなかったけど、xaml から Action<string> を流し込めなかったのでshoganai
    class LazyTabItemForTest : TabItem
    {
        private readonly Type _contentType;
        private readonly Action<string> _writeLog;

        public LazyTabItemForTest(string header, Type contentType, Action<string> writeLog)
        {
            Header = header;
            _contentType = contentType;
            _writeLog = writeLog;
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            (Content as IDisposable)?.Dispose();
            Content = Activator.CreateInstance(_contentType);

            _writeLog($"LazyTabItem_Create : {Header}");

            base.OnSelected(e);
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            (Content as IDisposable)?.Dispose();
            Content = null;

            _writeLog($"LazyTabItem_Release : {Header}");

            base.OnUnselected(e);
        }
    }

    class LazyTabItem : TabItem
    {
        public Type ContentType { get; set; }

        protected override void OnSelected(RoutedEventArgs e)
        {
            (Content as IDisposable)?.Dispose();

            Content = (ContentType is not null)
                ? Activator.CreateInstance(ContentType)
                : null;

            base.OnSelected(e);
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            (Content as IDisposable)?.Dispose();
            Content = null;

            base.OnUnselected(e);
        }
    }
}
