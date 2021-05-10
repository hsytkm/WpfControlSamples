using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using WpfControlSamples.Extensions;
using WpfControlSamples.Models;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(HsbColor), typeof(Brush))]
    class HsbColorToBrushConverter : GenericValueConverter<HsbColor, Brush>
    {
        public override Brush Convert(HsbColor hsbColor, object parameter, CultureInfo culture)
            => new SolidColorBrush(hsbColor.ToRgbColor()).WithFreeze();

        public override HsbColor ConvertBack(Brush brush, object parameter, CultureInfo culture) => default;
    }
}
