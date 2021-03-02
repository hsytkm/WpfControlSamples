using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;
using WpfControlSamples.Views.Adorners;

namespace WpfControlSamples.Views.Menus
{
    public partial class ResizeHandleAdornerPage : ContentControl
    {
        public ResizeHandleAdornerPage()
        {
            InitializeComponent();
        }

        // リサイズハンドルのイベント処理
        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!(sender is Thumb thumb)) return;

            //サイズ変更の対象要素を取得する
            if (!(AdornedBy.GetAdornedElementFromTemplateChild(thumb) is FrameworkElement adored)) return;

            var lengthMin = 20.0;
            var widthMin = lengthMin;
            var heightMin = lengthMin;

            // サイズ変更処理(横)
            if (thumb.HorizontalAlignment == HorizontalAlignment.Left)
            {
                Canvas.SetLeft(adored, Canvas.GetLeft(adored) + e.HorizontalChange);
                adored.Width = Math.Max(widthMin, adored.Width - e.HorizontalChange);
            }
            else
            {
                adored.Width = Math.Max(widthMin, adored.Width + e.HorizontalChange);
            }

            // サイズ変更処理(縦)
            if (thumb.VerticalAlignment == VerticalAlignment.Top)
            {
                Canvas.SetTop(adored, Canvas.GetTop(adored) + e.VerticalChange);
                adored.Height = Math.Max(heightMin, adored.Height - e.VerticalChange);
            }
            else
            {
                adored.Height = Math.Max(heightMin, adored.Height + e.VerticalChange);
            }
            e.Handled = true;
        }
    }
}
