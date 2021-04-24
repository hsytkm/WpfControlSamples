#nullable disable
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    // 参考のために残しているが、BooleansAreAllFalseConverter で代替できる。
    // 詳細は MultiBindingPage.xaml を参照

    [ValueConversion(typeof(bool[]), typeof(Visibility))]
    class BooleansAreAllTrueVisibleConverter : IMultiValueConverter
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
