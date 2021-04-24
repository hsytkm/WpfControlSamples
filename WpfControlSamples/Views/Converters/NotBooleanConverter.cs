#nullable disable
using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    class NotBooleanConverter : GenericValueConverter<bool, bool>
    {
        public override bool Convert(bool b, object parameter, CultureInfo culture) => !b;

        public override bool ConvertBack(bool b, object parameter, CultureInfo culture) => default;
    }
}
