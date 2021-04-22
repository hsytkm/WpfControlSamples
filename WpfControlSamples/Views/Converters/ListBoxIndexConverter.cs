using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    /* How to display row numbers in a ListView?
     * https://stackoverflow.com/questions/660528/how-to-display-row-numbers-in-a-listview
     *
     * ListViewItem inherits from ListBoxItem.
     */
    [ValueConversion(typeof(ListBoxItem), typeof(int))]
    class ListBoxIndexConverter : GenericValueConverter<ListBoxItem, int>
    {
        public override int Convert(ListBoxItem item, object parameter, CultureInfo culture)
        {
            var itemsControl = ItemsControl.ItemsControlFromItemContainer(item);
            int index = itemsControl.ItemContainerGenerator.IndexFromContainer(item);
            return index + 1;
        }

        public override ListBoxItem ConvertBack(int index, object parameter, CultureInfo culture) => default;
    }
}
