#nullable disable
using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(int), typeof(int))]
    class AddIntegerConverter : GenericValueConverter<int, int>
    {
        public override int Convert(int x, object parameter, CultureInfo culture)
        {
            var y = GetParameter(parameter);
            return x + y;
        }

        private static int GetParameter(object parameter)
        {
            if (parameter is int i) return i;
            if (parameter is string s) return int.Parse(s);

            throw new NotSupportedException();
        }

        public override int ConvertBack(int brush, object parameter, CultureInfo culture) => default;
    }
}
