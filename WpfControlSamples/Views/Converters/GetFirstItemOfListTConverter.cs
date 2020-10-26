using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    /// <summary>return first item of ListT (for Selector.SelectedItems)</summary>
    [ValueConversion(typeof(IEnumerable<object>), typeof(object))]
    class GetFirstItemOfListTConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = value.GetType();
            if (type.IsGenericType)
            {
                var geneType = type.GetGenericTypeDefinition();
                if (geneType == typeof(List<>))
                    return ((IEnumerable)value).OfType<object>().First();
            }

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();

    }
}
