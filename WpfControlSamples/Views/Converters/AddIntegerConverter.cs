using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(int), typeof(int))]
    class AddIntegerConverter : GenericValueConverter<int, int>
    {
        public override int Convert(int value, object parameter, CultureInfo culture)
        {
            var x = parameter switch
            {
                int i => i,
                string s => int.Parse(s, CultureInfo.InvariantCulture),
                _ => throw new NotSupportedException()
            };
            return value + x;
        }

        public override int ConvertBack(int value, object parameter, CultureInfo culture) => default;
    }
}
