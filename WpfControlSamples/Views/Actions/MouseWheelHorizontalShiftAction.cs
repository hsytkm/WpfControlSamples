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
        private ScrollViewer? _scrollViewer;

        protected override void OnAttached()
        {
            base.OnAttached();
            _scrollViewer = AssociatedObject;
        }

        protected override void OnDetaching()
        {
            _scrollViewer = null;
            base.OnDetaching();
        }

        protected override void Invoke(object parameter)
        {
            if (_scrollViewer is null) return;
            if (parameter is not MouseWheelEventArgs mouseWheelEventArgs) return;
            if (!((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)) return;

            // Source は、操作の発行元（ScrollViewer の子要素）のことがあるので使用しない
            //if (!(mouseWheelEventArgs.Source is ScrollViewer scrollViewer)) return;

            // MouseWheel操作でScrollViewerが動いてうざいので、水平移動を優先する
            mouseWheelEventArgs.Handled = true;

            if (mouseWheelEventArgs.Delta < 0)
            {
                _scrollViewer.LineRight();
            }
            else
            {
                _scrollViewer.LineLeft();
            }
        }
    }
}
