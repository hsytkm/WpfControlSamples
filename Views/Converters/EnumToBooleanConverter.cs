using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    // C#のWPFでRadioButtonのIsCheckedに列挙型をバインドする
    // https://araramistudio.jimdo.com/2016/12/27/wpf%E3%81%A7radiobutton%E3%81%AEischecked%E3%81%AB%E5%88%97%E6%8C%99%E5%9E%8B%E3%82%92%E3%83%90%E3%82%A4%E3%83%B3%E3%83%89%E3%81%99%E3%82%8B/
    class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!Enum.IsDefined(value.GetType(), value))
                return DependencyProperty.UnsetValue;

            if (!(parameter is string parameterString))
                return DependencyProperty.UnsetValue;

            var parameterValue = Enum.Parse(value.GetType(), parameterString);
            return parameterValue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string parameterString))
                return DependencyProperty.UnsetValue;

            return true.Equals(value)
                ? Enum.Parse(targetType, parameterString)
                : DependencyProperty.UnsetValue;
        }
    }
}
