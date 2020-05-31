using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    // 「何れかが true」は「全て false の not」で済むので不要。
    // DataTrigger で BooleansAreAllFalseConverter の False とすればよい。
    // 使い方は MultiBindingPage.xaml を参照

    //[ValueConversion(typeof(bool[]), typeof(bool))]
    //class BooleansAreAnyTrueConverter : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        return values.OfType<bool>().Contains(true);
    //    }

    //    public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture) =>
    //        throw new NotImplementedException();
    //}
}
