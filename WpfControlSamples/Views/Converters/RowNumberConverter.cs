using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

// https://blog.magnusmontin.net/2014/08/18/right-aligned-row-numbers-datagridrowheader-wpf/
namespace WpfControlSamples.Views.Converters
{
    public class RowNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not DataGridRow row)
                throw new InvalidOperationException("This converter class can only be used with DataGridRow elements.");

            var index = row.GetIndex();  // index start zero.

            if (parameter is string format)
                return string.Format(format, index);

            return index;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}