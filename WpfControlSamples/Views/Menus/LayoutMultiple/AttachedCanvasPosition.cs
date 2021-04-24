#nullable disable
using System;
using System.Diagnostics;
using System.Windows;
using WpfControlSamples.Extensions;

namespace WpfControlSamples.Views.Menus
{
    static class AttachedCanvasPosition
    {
        public static readonly DependencyProperty LeftTopProperty =
            DependencyProperty.RegisterAttached(
                "LeftTop", typeof(Point), typeof(AttachedCanvasPosition),
                new FrameworkPropertyMetadata(new Point(),
                     (sender, e) => OnLeftTopPropertyChanged(sender, e.OldValue, e.NewValue)),
                new ValidateValueCallback(IsValidPoint));

        public static Point GetLeftTop(UIElement element) =>
            (Point)element.GetValue(LeftTopProperty);

        public static void SetLeftTop(UIElement element, Point value) =>
            element.SetValue(LeftTopProperty, value);

        // 検証コールバックによって false が返されると、値が設定されない
        private static bool IsValidPoint(object value) =>
            value is Point p && !double.IsNaN(p.X) && !double.IsNaN(p.Y);

        private static void OnLeftTopPropertyChanged(DependencyObject sender, object oldValue, object newValue)
        {
            if (sender is not UIElement element) return;
            if (newValue is not Point leftTop) return;
            element.SetCanvasLeftTop(leftTop);
        }
    }
}
