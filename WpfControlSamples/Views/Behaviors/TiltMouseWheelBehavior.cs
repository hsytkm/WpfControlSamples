using Microsoft.Xaml.Behaviors;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using WpfControlSamples.Extensions;

namespace WpfControlSamples.Views.Behaviors
{
    /*  How to horizontally scroll in WPF using mouse tilt wheel?
     *  https://stackoverflow.com/questions/4158101/how-to-horizontally-scroll-in-wpf-using-mouse-tilt-wheel
     */
    [TypeConstraint(typeof(UIElement))]
    public sealed class TiltMouseWheelBehavior : Behavior<UIElement>
    {
        private const int SCROLL_FACTOR = 4;
        private const int WM_MOUSEHWEEL = 0x20e;

        private readonly HwndSourceHook _hook;
        private ScrollViewer? _scrollViewer;
        private HwndSource? _hwndSource;

        public TiltMouseWheelBehavior() => _hook = WindowProc;

        protected override void OnAttached()
        {
            base.OnAttached();

            _scrollViewer = AssociatedObject.FindVisualChild<ScrollViewer>();
            AssociatedObject.MouseEnter += AssociatedObject_MouseEnter;
            AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseEnter -= AssociatedObject_MouseEnter;
            AssociatedObject.MouseLeave -= AssociatedObject_MouseLeave;
            _scrollViewer = null;
            _hwndSource = null;

            base.OnDetaching();
        }

        private void AssociatedObject_MouseEnter(object sender, MouseEventArgs e)
        {
            _hwndSource ??= PresentationSource.FromDependencyObject(AssociatedObject) as HwndSource;
            _hwndSource?.AddHook(_hook);
        }

        private void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
            _hwndSource?.RemoveHook(_hook);
        }

        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            static int HIWORD(IntPtr ptr)
            {
                //return (short)(((ptr.ToInt64()) >> 16) & 0xffff);
                long value = long.Parse(ptr.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                return (value > int.MaxValue) ? -1 : 1;
            }

            if (msg is WM_MOUSEHWEEL && _scrollViewer is not null)
            {
                int delta = HIWORD(wParam) > 0 ? SCROLL_FACTOR : -SCROLL_FACTOR;
                _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + delta);

                handled = true;
            }
            return IntPtr.Zero;
        }
    }
}
