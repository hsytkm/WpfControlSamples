#nullable disable
using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(double), typeof(int))]
    class DoubleToIntegerConverter : GenericValueConverter<double, int>
    {
        public override int Convert(double d, object parameter, CultureInfo culture)
        {
            var param = GetParameter(parameter);
            return (int)Math.Round(d * param);
        }

        public override double ConvertBack(int i, object parameter, CultureInfo culture) => default;

        private static double GetParameter(object parameter)
        {
            if (parameter is double d) return (int)Math.Round(d);
            if (parameter is int i) return i;
            if (parameter is string s) return double.Parse(s);

            throw new NotSupportedException();
        }
    }
}
