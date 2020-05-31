using System;
using System.Windows.Media;

namespace WpfControlSamples.Extensions
{
    static class ColorExtensions
    {
        public static SolidColorBrush ToFreezedSolidColorBrush(this Color color)
        {
            var brush = new SolidColorBrush(color);
            brush.Freeze();
            return brush;
        }

    }
}
