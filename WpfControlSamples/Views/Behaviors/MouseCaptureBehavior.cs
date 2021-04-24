#nullable disable
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace WpfControlSamples.Views.Behaviors
{
    // MouseCaptureAction でも同様のことができる。 個人的にはTriggerAction版の方が好み
    class MouseCaptureBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;

            base.OnDetaching();
        }

        /// <summary>
        /// マウス操作の強制キャプチャ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is IInputElement inputElement)) return;
            inputElement.CaptureMouse();
        }

        /// <summary>
        /// マウス操作の強制キャプチャを終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is IInputElement inputElement)) return;
            inputElement.ReleaseMouseCapture();
        }
    }
}
