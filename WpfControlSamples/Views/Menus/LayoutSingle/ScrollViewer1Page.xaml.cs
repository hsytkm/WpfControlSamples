#nullable disable
using System;
using System.Collections.Generic;
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
    public partial class ScrollViewer1Page : ContentControl
    {
        public ScrollViewer1Page()
        {
            InitializeComponent();

            DataContext = string.Join(Environment.NewLine,
                Models.SampleData.WpfColors.Select(x => x.Name));
        }

        private void ScrollViewer_ScrollToTop(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToTop();
        }

        private void ScrollViewer_ScrollToCenter(object sender, RoutedEventArgs e)
        {
            var extentHeight = scrollViewer.ExtentHeight;
            var viewportHeight = scrollViewer.ViewportHeight;

            // 表示範囲(Viewport)の中央がセンターになるようにする
            var offset = (extentHeight / 2d) - (viewportHeight / 2d);
            scrollViewer.ScrollToVerticalOffset(offset);
        }

        private void ScrollViewer_ScrollToBottom(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToBottom();
        }
    }
}
