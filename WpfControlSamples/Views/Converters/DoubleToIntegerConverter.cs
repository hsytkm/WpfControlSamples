using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(double), typeof(int))]
    class DoubleToIntegerConverter : GenericValueConverter<double, int>
    {
        public override int Convert(double value, object parameter, CultureInfo culture)
        {
            var ratio = parameter switch
            {
                int i => i,
                double d => (int)Math.Round(d),
                string s => double.Parse(s, CultureInfo.InvariantCulture),
                _ => 1,
            };
            return (int)Math.Round(value * ratio);
        }

        public override double ConvertBack(int value, object parameter, CultureInfo culture) => value;
    }
}
