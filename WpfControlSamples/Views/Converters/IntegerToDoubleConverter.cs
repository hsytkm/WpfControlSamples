using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(int), typeof(double))]
    class IntegerToDoubleConverter : GenericValueConverter<int, double>
    {
        public override double Convert(int i, object parameter, CultureInfo culture) => i;
        public override int ConvertBack(double d, object parameter, CultureInfo culture) => (int)Math.Round(d);
    }
}
