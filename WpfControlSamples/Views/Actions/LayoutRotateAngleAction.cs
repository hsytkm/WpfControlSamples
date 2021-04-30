using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WpfControlSamples.Views.Actions
{
    /*
     *  RenderTransform だと回転時に ActualWidth/Height が更新されず、90/270度回転で縦が Panel に収まらない。
     *  LayoutTransform だと回転時に縦が収まる。（Render よりも処理が重いらしいが shoganai）
     *
     *  RenderTransform での回転は SizeChanged イベントで処理が必要なので Behavior で実装した。(RenderRotateAngleBehavior.cs)
     *  LayoutTransform は SizeChanged が必要ないので Action で実装した。(LayoutRotateAngleAction.cs)
     */
    class LayoutRotateAngleAction : TargetedTriggerAction<FrameworkElement>
    {
        private Storyboard? _storyboard;

        protected override void Invoke(object parameter)
        {
            if (AssociatedObject is not FrameworkElement fe) return;
            if (fe.LayoutTransform is not RotateTransform rotate) return;
            if (parameter is not DependencyPropertyChangedEventArgs args) return;
            if (args.NewValue is not double newAngle) return;

            rotate.CenterX = fe.ActualWidth / 2d;
            rotate.CenterY = fe.ActualHeight / 2d;

            var animation = new DoubleAnimation
            {
                Duration = TimeSpan.FromMilliseconds(200),
                From = rotate.Angle,
                To = newAngle,
            };
            Storyboard.SetTarget(animation, fe);
            Storyboard.SetTargetProperty(animation, new PropertyPath("LayoutTransform.Angle"));

            _storyboard ??= new Storyboard();
            _storyboard.Children.Add(animation);
            _storyboard.Begin();
        }
    }
}
