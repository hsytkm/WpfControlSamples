using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    class NotBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b) return !b;
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b) return !b;
            throw new NotImplementedException();
        }
    }
}
