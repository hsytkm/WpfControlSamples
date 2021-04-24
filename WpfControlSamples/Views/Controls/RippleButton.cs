#nullable disable
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WpfControlSamples.Views.Controls
{
    // WPFでクリック時に波紋を出すエフェクト(Ripple Effect)を実装する
    // https://takachan.hatenablog.com/entry/2017/12/21/234425
    //
    // ResourceDictionary の Style と合わせて使用する必要がある。
    // 面倒なので Style をコードに埋め込みたい。
    class RippleButton : Button
    {
        public static readonly DependencyProperty RippleColorProperty =
            DependencyProperty.Register(
                nameof(RippleColor), typeof(Brush), typeof(RippleButton),
                new PropertyMetadata(Brushes.White));

        public Brush RippleColor
        {
            get => (Brush)GetValue(RippleColorProperty);
            set => SetValue(RippleColorProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PreviewMouseDown += OnMouseDown;
        }

        private void OnMouseDown(object sender, RoutedEventArgs e)
        {
            // クリック位置からRippleの中心を取る
            var mousePos = (e as MouseButtonEventArgs).GetPosition(this);

            var ellipse = this.GetTemplateChild("CircleEffect") as Ellipse;

            ellipse.Margin = new Thickness(mousePos.X, mousePos.Y, 0, 0);

            // アニメーションの動作の指定
            var storyboard = (this.FindResource("RippleAnimation") as Storyboard).Clone();

            // 円の最大の大きさ -> コントロールの大きさの倍
            double effectMaxSize = Math.Max(this.ActualWidth, this.ActualHeight) * 3;

            (storyboard.Children[2] as ThicknessAnimation).From = new Thickness(mousePos.X, mousePos.Y, 0, 0);

            (storyboard.Children[2] as ThicknessAnimation).To = new Thickness(
                mousePos.X - effectMaxSize / 2, mousePos.Y - effectMaxSize / 2, 0, 0);

            (storyboard.Children[3] as DoubleAnimation).To = effectMaxSize;

            ellipse.BeginStoryboard(storyboard);
        }
    }
}
