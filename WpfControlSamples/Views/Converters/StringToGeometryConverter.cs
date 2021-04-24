#nullable disable
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(string), typeof(Geometry))]
    class StringToGeometryConverter : GenericValueConverter<string, Geometry>
    {
        public override Geometry Convert(string text, object parameter, CultureInfo culture)
        {
            var fontFamily = GetFontName(parameter);

            var typeface = new Typeface(
                fontFamily,
                FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

            var formattedText = new FormattedText(
                text,
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface,
                emSize: 64, foreground: new SolidColorBrush(), pixelsPerDip: 1.25);

            return formattedText.BuildGeometry(new Point());
        }

        private static FontFamily GetFontName(object parameter)
        {
            if (parameter is FontFamily ff) return ff;
            var fontname = (parameter is string s) ? s : "Segoe UI";
            return new FontFamily(fontname);
        }

        public override string ConvertBack(Geometry geometry, object parameter, CultureInfo culture) => default;
    }
}
