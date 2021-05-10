#nullable disable
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using WpfControlSamples.Extensions;
using WpfControlSamples.Models;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(Color), typeof(Brush))]
    class ColorToBrushConverter : GenericValueConverter<Color, Brush>
    {
        public override Brush Convert(Color color, object parameter, CultureInfo culture) =>
            new SolidColorBrush(color).WithFreeze();

        public override Color ConvertBack(Brush brush, object parameter, CultureInfo culture) => default;
    }

    /// <summary>
    /// 指定背景色に対する読みやすい文字色（B/W）を返します
    /// </summary>
    [ValueConversion(typeof(Color), typeof(Brush))]
    class ColorToForegroundBrushConverter : GenericValueConverter<Color, Brush>
    {
        public override Brush Convert(Color color, object parameter, CultureInfo culture) => color.GetForegroundBrush();

        public override Color ConvertBack(Brush brush, object parameter, CultureInfo culture) => default;
    }
}
