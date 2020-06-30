using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    // How to display row numbers in a ListView?
    // https://stackoverflow.com/questions/660528/how-to-display-row-numbers-in-a-listview

    [ValueConversion(typeof(ListViewItem), typeof(string))]
    class ListViewIndexConverter : GenericValueConverter<ListViewItem, string>
    {
        public override string Convert(ListViewItem item, object parameter, CultureInfo culture)
        {
            var listView = ItemsControl.ItemsControlFromItemContainer(item) as ListView;
            int index = listView.ItemContainerGenerator.IndexFromContainer(item);
            return (index + 1).ToString();
        }

        public override ListViewItem ConvertBack(string s, object parameter, CultureInfo culture) => default;
    }
}
