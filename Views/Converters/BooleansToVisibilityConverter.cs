using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(bool[]), typeof(Visibility))]
    public class BooleansToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // 配列が全てbool型のtrueならVisible
            foreach (var value in values)
            {
                if (!(value is bool b) || !b) return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
