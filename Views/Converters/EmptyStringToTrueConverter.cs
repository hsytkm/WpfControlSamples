using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(string), typeof(bool))]
    class EmptyStringToTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) throw new ArgumentNullException();
            if (value is string s) return string.IsNullOrEmpty(s);
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => new NotImplementedException();
    }
}
