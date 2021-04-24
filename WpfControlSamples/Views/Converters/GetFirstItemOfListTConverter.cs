#nullable disable
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

            if (type.IsArray)
            {
                return ((Array)value).GetValue(0);
            }
            else if (type.IsGenericType)
            {
                var isGenericIEnumerable = type.GetInterfaces()
                    .Any(t => t.IsConstructedGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

                if (isGenericIEnumerable)
                    return ((IEnumerable)value).OfType<object>().FirstOrDefault();
            }

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();

    }
}
