using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WpfControlSamples.Views.Behaviors
{
    /*
     *  RenderTransform だと回転時に ActualWidth/Height が更新されず、90/270度回転で縦が Panel に収まらない。
     *  LayoutTransform だと回転時に縦が収まる。（Render よりも処理が重いらしいが shoganai）
     *
     *  RenderTransform での回転は SizeChanged イベントで処理が必要なので Behavior で実装した。(RenderRotateAngleBehavior.cs)
     *  LayoutTransform は SizeChanged が必要ないので Action で実装した。(LayoutRotateAngleAction.cs)
     */
    [TypeConstraint(typeof(FrameworkElement))]
    public class RenderRotateAngleBehavior : Behavior<FrameworkElement>
    {
        private Storyboard? _storyboard;

        #region Angle
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register(nameof(Angle), typeof(double), typeof(RenderRotateAngleBehavior),
                new FrameworkPropertyMetadata(0d, (sender, e) => ((RenderRotateAngleBehavior)sender).OnAnglePropertyChanged((double)e.NewValue)));

        private void OnAnglePropertyChanged(double newAngle)
        {
            var fe = this.AssociatedObject;
            if (fe.RenderTransform is not RotateTransform rotate) return;

            rotate.CenterX = fe.ActualWidth / 2d;
            rotate.CenterY = fe.ActualHeight / 2d;

            var animation = new DoubleAnimation
            {
                Duration = TimeSpan.FromMilliseconds(200),
                From = rotate.Angle,
                To = newAngle,
            };
            Storyboard.SetTarget(animation, fe);
            Storyboard.SetTargetProperty(animation, new PropertyPath("RenderTransform.Angle"));

            _storyboard ??= new Storyboard();
            _storyboard.Children.Add(animation);
            _storyboard.Begin();
        }

        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }
        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SizeChanged += OnSizeChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SizeChanged -= OnSizeChanged;
            base.OnDetaching();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is FrameworkElement fe)
                SetRotateTransformCenterPosition(fe);

            // Render側はコレ(常に回転芯を中心にする)がないと 90度回転中の ベースPanel リサイズで　Image が変な位置に表示される
            static void SetRotateTransformCenterPosition(FrameworkElement fe)
            {
                if (fe.RenderTransform is not RotateTransform rotate) return;
                rotate.CenterX = fe.ActualWidth / 2d;
                rotate.CenterY = fe.ActualHeight / 2d;
            }
        }
    }
}
