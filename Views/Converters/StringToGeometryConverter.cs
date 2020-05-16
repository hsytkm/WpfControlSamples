using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(string), typeof(Geometry))]
    class StringToGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                var typeface = new Typeface(
                    new FontFamily("Segoe UI"),
                    FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

                var formattedText = new FormattedText(
                    text,
                    CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface,
                    emSize: 64, foreground: new SolidColorBrush(), pixelsPerDip: 1.25);

                return formattedText.BuildGeometry(new Point());
            }
            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
