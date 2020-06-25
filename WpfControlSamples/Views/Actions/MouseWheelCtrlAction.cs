using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Input;

namespace WpfControlSamples.Views.Actions
{
    /// <summary>
    /// マウスホイールの Up/Down を通知するTriggerAction
    /// </summary>
    class MouseWheelCtrlAction : TriggerAction<DependencyObject>
    {
        // 電源ON直後に enum(0) が通知されるので、渋々 None を追加した。
        // Rx使えるなら Skip(1) とかで対応したい。
        public enum Direction { None, Up, Down };

        /// <summary>
        /// マウスホイール方向の通知
        /// </summary>
        public static readonly DependencyProperty WheelDirectionProperty
            = DependencyProperty.Register(
                nameof(WheelDirection), typeof(Direction), typeof(MouseWheelCtrlAction));

        public Direction WheelDirection
        {
            get => (Direction)GetValue(WheelDirectionProperty);
            set => SetValue(WheelDirectionProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            if (!(parameter is MouseWheelEventArgs mouseWheelEventArgs)) return;

            if (!((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)) return;

            // MouseWheel操作でScrollViewerが動いてうざいので、Ctrl押下中はズームを優先する
            mouseWheelEventArgs.Handled = true;

            WheelDirection = mouseWheelEventArgs.Delta > 0 ? Direction.Up : Direction.Down;
        }
    }
}
