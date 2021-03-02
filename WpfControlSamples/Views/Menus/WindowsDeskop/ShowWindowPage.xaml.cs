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
    public partial class ShowWindowPage : ContentControl
    {
        public ShowWindowPage()
        {
            InitializeComponent();
        }

        private void Button_ShowWindow(object sender, RoutedEventArgs e)
        {
            if (!(Window.GetWindow(this) is MainWindow parentWindow)) return;

            var dialog = new ShowWindowSub
            {
                Owner = parentWindow
            };
            dialog.Show();
        }

        private void Button_ShowDialogWindow(object sender, RoutedEventArgs e)
        {
            DataContext = "";

            var dialog = new ShowWindowSubDialog();

            // Dialog を × ボタン で閉じた場合 false が来る。
            // null が来てほしい気もするけど調べてない（きっとPrism使うので）
            var result = dialog.ShowDialog();
            DataContext = result.HasValue ? result.Value.ToString() : "null";
        }
    }
}
