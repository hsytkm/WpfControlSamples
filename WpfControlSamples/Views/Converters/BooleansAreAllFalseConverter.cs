#nullable disable
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
        {
            // All(false) より、!Contains(true) の方が早そう（時間未計測）
            // 時間気にするなら LINQ 使うなって話やけど…
            //return values.OfType<bool>().All(x => !x);

            return !values.OfType<bool>().Contains(true);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
