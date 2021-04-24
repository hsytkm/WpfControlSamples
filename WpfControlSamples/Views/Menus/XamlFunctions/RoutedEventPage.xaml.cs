#nullable disable
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class RoutedEventPage : ContentControl
    {
        public RoutedEventPage()
        {
            InitializeComponent();

            parentCat.Nyaan += (sender, e) =>
            {
                var name = ((CatButton)e.Source).Name;
                PushMessage(name + " は、にゃーん と鳴いた");
            };
        }

        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private void PushMessage(string s)
        {
            _stringBuilder.AppendLine(s);
            DataContext = _stringBuilder.ToString();
        }

        private void CatButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is UIElement element)
            {
                // Clickイベントが親に行かないように止める
                e.Handled = true;

                element.RaiseEvent(new RoutedEventArgs(CatButton.NyaanEvent));
            }
        }
    }

    class CatButton : Button
    {
        // イベント名Eventの命名規約のstaticフィールドに格納する
        // 一般的にトンネルイベントの名前は "Preview*" になります。
        public static RoutedEvent NyaanEvent =
            EventManager.RegisterRoutedEvent(
                name: nameof(Nyaan),
                routingStrategy: RoutingStrategy.Bubble,
                handlerType: typeof(RoutedEventHandler),
                ownerType: typeof(CatButton));

        // CLRのイベントのラッパー
        public event RoutedEventHandler Nyaan
        {
            add => AddHandler(NyaanEvent, value);
            remove => RemoveHandler(NyaanEvent, value);
        }

        // 子を追加するメソッド
        //public void AddChild(CatButton child) => AddLogicalChild(child);
    }

}
