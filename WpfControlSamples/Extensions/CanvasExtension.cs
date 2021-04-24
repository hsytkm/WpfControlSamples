#nullable disable
using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfControlSamples.Extensions
{
    static class CanvasExtension
    {
        public static double GetCanvasLeft(this UIElement element)
        {
            var left = Canvas.GetLeft(element);
            return double.IsNaN(left) ? 0 : left;
        }

        public static double GetCanvasTop(this UIElement element)
        {
            var top = Canvas.GetTop(element);
            return double.IsNaN(top) ? 0 : top;
        }

        public static Point GetCanvasLeftTop(this UIElement element) =>
            new Point(element.GetCanvasLeft(), element.GetCanvasTop());


        public static void SetCanvasLeftTop(this UIElement element, double left, double top)
        {
            Canvas.SetLeft(element, left);
            Canvas.SetTop(element, top);
        }

        public static void SetCanvasLeftTop(this UIElement element, Point leftTop) =>
            element.SetCanvasLeftTop(leftTop.X, leftTop.Y);

        public static void ShiftCanvasLeftTop(this UIElement element, Vector shift)
        {
            var oldPos = element.GetCanvasLeftTop();
            element.SetCanvasLeftTop(oldPos + shift);
        }

    }
}
