using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    // How to display row numbers in a ListView?
    // https://stackoverflow.com/questions/660528/how-to-display-row-numbers-in-a-listview
    class ListViewIndexConverter : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {
            var item = (ListViewItem)value;
            var listView = ItemsControl.ItemsControlFromItemContainer(item) as ListView;
            int index = listView.ItemContainerGenerator.IndexFromContainer(item);
            return (index + 1).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
