using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Input;

namespace WpfControlSamples.Views.Actions
{
    /// <summary>
    /// フルスクリーン表示の切り替え(Escキーで元の画面表示に戻す)
    /// </summary>
    class WindowFullScreenAction : TriggerAction<FrameworkElement>
    {
        private bool _isAttached;
        private (WindowStyle style, WindowState state) _saveWindow;

        protected override void Invoke(object parameter)
        {
            if (_isAttached) return;
            if (!(Window.GetWindow(AssociatedObject) is Window window)) return;

            window.PreviewKeyDown += new KeyEventHandler(OnKeyDown);
            _saveWindow = (window.WindowStyle, window.WindowState);

            // タイトルバーと境界線を表示しない + 最大化表示
            (window.WindowStyle, window.WindowState) = (WindowStyle.None, WindowState.Maximized);

            _isAttached = true;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is Window window)) return;
            if (e.Key != Key.Escape) return;

            window.PreviewKeyDown -= new KeyEventHandler(OnKeyDown);
            (window.WindowStyle, window.WindowState) = _saveWindow;

            _isAttached = false;
        }
    }
}
