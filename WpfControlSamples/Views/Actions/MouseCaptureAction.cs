using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Input;

namespace WpfControlSamples.Views.Actions
{
    /// <summary>
    /// MouseCaptureBehavior の TriggerAction 版
    /// EventTrigger の MouseButtonDown/Up で発火させる
    /// </summary>
    class MouseCaptureAction : TriggerAction<FrameworkElement>
    {
        enum CaptureButton { None, Left, Right, Middle }
        static CaptureButton _captureButton = CaptureButton.None;

        protected override void Invoke(object parameter)
        {
            if (!(parameter is MouseEventArgs e)) return;
            if (!(AssociatedObject is IInputElement inputElement)) return;

            if (_captureButton == CaptureButton.None)
            {
                var button = e switch
                {
                    { LeftButton: MouseButtonState.Pressed } => CaptureButton.Left,
                    { RightButton: MouseButtonState.Pressed } => CaptureButton.Right,
                    { MiddleButton: MouseButtonState.Pressed } => CaptureButton.Middle,
                    _ => CaptureButton.None,
                };

                if (button != CaptureButton.None)
                {
                    _captureButton = button;
                    inputElement.CaptureMouse();
                }
            }
            else
            {
                var release = _captureButton switch
                {
                    CaptureButton.Left when e.LeftButton == MouseButtonState.Released => true,
                    CaptureButton.Right when e.RightButton == MouseButtonState.Released => true,
                    CaptureButton.Middle when e.MiddleButton == MouseButtonState.Released => true,
                    _ => false,
                };

                if (release)
                {
                    inputElement.ReleaseMouseCapture();
                    _captureButton = CaptureButton.None;
                }
            }
        }
    }
}
