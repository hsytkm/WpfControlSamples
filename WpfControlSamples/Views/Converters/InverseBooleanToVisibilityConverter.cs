#nullable disable
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    /* DataTrigger + NotBooleanConverter で代替できるはずなので無効化
     * 
     * 上のコメントがどういう記述をさしてるのか分からんくなった。多分以下のイメージやと思う。
     *  <DataTrigger Binding="{Binding IsSelected, Mode=OneWay}" Value="False" >
     *    <Setter Property="Visibility" Value="Collapsed" />
     *  </DataTrigger>
     */

    //[ValueConversion(typeof(bool), typeof(Visibility))]
    //class InverseBooleanToVisibilityConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
    //        (value is bool b && !b) ? Visibility.Visible : Visibility.Collapsed;

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
    //        throw new NotImplementedException();
    //}
}
