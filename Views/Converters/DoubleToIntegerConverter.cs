using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(double), typeof(int))]
    class DoubleToIntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (int)Math.Round((double)value * GetParameter(parameter));

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();

        private double GetParameter(object parameter)
        {
            if (parameter is double d) return (int)Math.Round(d);
            if (parameter is int i) return i;
            if (parameter is string s) return double.Parse(s);

            throw new NotSupportedException();
        }    }
}
