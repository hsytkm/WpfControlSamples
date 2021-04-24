#nullable disable
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using WpfControlSamples.Extensions;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(Color), typeof(Brush))]
    class ColorToBrushConverter : GenericValueConverter<Color, Brush>
    {
        public override Brush Convert(Color color, object parameter, CultureInfo culture) =>
            new SolidColorBrush(color).WithFreeze();

        public override Color ConvertBack(Brush brush, object parameter, CultureInfo culture) => default;
    }
}
