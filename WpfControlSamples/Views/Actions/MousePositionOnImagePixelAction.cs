#nullable disable
using Microsoft.Xaml.Behaviors;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WpfControlSamples.Views.Actions
{
    class MousePositionOnImagePixelAction : TriggerAction<Image>
    {
        public static readonly DependencyProperty MousePositionProperty =
            DependencyProperty.Register(nameof(MousePosition), typeof(Point), typeof(MousePositionOnImagePixelAction));
        public Point MousePosition
        {
            get => (Point)GetValue(MousePositionProperty);
            set => SetValue(MousePositionProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            var image = this.AssociatedObject;
            if (!(image.Source is BitmapSource bitmapSource)) return;

            var viewCursorPos = Mouse.GetPosition(image);

            var cursorX = Math.Round((viewCursorPos.X / image.ActualWidth) * bitmapSource.PixelWidth - 0.5d, 0);
            cursorX = Math.Max(0, Math.Min(bitmapSource.PixelWidth - 1, cursorX));

            var cursorY = Math.Round((viewCursorPos.Y / image.ActualHeight) * bitmapSource.PixelHeight - 0.5d, 0);
            cursorY = Math.Max(0, Math.Min(bitmapSource.PixelHeight - 1, cursorY));

            MousePosition = new Point(cursorX, cursorY);
        }
    }
}
