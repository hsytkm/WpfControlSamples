using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(bool[]), typeof(bool))]
    class BooleansAreAllFalseConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            => values.OfType<bool>().All(x => !x);

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
