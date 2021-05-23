using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

// https://blog.magnusmontin.net/2014/08/18/right-aligned-row-numbers-datagridrowheader-wpf/
namespace WpfControlSamples.Views.Converters
{
    // DataGrid が仮想化されている場合、Index値 がずれます。
    // DataGridRowIndexAction を使いましょう。（常にそちらを使っとけば良い気もします）
    public class DataGridRowIndexConverter : IValueConverter
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
