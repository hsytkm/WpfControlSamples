#nullable disable
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    /// <summary>
    /// 円 (Ellipse) の 大きさに応じて、Margin を変える(センターに来るようにする)
    /// </summary>
    [ValueConversion(typeof(Size), typeof(Thickness))]
    class EllipseSizeToMarginConverter : GenericValueConverter<Size, Thickness>
    {
        public override Thickness Convert(Size size, object parameter, CultureInfo culture) =>
            new Thickness(-(size.Width / 2.0), -(size.Height / 2.0), 0, 0);

        public override Size ConvertBack(Thickness thickness, object parameter, CultureInfo culture) => default;
    }
}
