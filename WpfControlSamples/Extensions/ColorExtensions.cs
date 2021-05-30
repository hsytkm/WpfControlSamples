using System;
using System.Windows.Media;

namespace WpfControlSamples.Extensions
{
    static class ColorExtensions
    {
        public static SolidColorBrush ToSolidColorBrush(this Color color, bool isFrozen = true)
        {
            var brush = new SolidColorBrush(color);
            if (isFrozen) brush.Freeze();
            return brush;
        }

    }
}
