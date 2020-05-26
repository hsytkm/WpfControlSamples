using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(int), typeof(int))]
    class AddIntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int x)
            {
                var y = GetParameter(parameter);
                return x + y;
            }

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();

        private int GetParameter(object parameter)
        {
            if (parameter is int i) return i;
            if (parameter is string s) return int.Parse(s);

            throw new NotSupportedException();
        }
    }
}
