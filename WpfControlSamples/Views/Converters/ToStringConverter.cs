#nullable disable
using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(object), typeof(string))]
    class ToStringConverter : GenericValueConverter<object, string>
    {
        public override string Convert(object value, object parameter, CultureInfo culture)
            => (parameter is string format) ? string.Format(format, value) : value.ToString();

        public override object ConvertBack(string value, object parameter, CultureInfo culture)
            => default;
    }
}
