#nullable disable
using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    class NotNullToTrueConverter : GenericValueConverter<object, bool>
    {
        public override bool Convert(object obj, object parameter, CultureInfo culture) => !(obj is null);

        public override object ConvertBack(bool b, object parameter, CultureInfo culture) => default;
    }
}
