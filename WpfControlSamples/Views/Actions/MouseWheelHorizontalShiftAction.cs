using Microsoft.Xaml.Behaviors;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfControlSamples.Views.Actions
{
    /// <summary>
    /// Shift + MouseWheel で水平方向にスクロールバーを移動
    /// </summary>
    class MouseWheelHorizontalShiftAction : TriggerAction<ScrollViewer>
    {
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject is not ScrollViewer scrollViewer) return;
            if (parameter is not MouseWheelEventArgs mouseWheelEventArgs) return;
            if (!((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)) return;

            // MouseWheel操作でScrollViewerが動いてうざいので、水平移動を優先する
            mouseWheelEventArgs.Handled = true;

            if (mouseWheelEventArgs.Delta < 0)
            {
                scrollViewer.LineRight();
            }
            else
            {
                scrollViewer.LineLeft();
            }
        }
    }
}
