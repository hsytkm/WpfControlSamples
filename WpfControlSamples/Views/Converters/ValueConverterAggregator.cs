using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    /* 複数の Converter を 1つの Converter にまとめます。
     * （DataTrigger か何かを使った他の実装があった気もするけど…）
     * 
     *  Is there a way to chain multiple value converters in XAML?
     *  https://stackoverflow.com/questions/2607490/is-there-a-way-to-chain-multiple-value-converters-in-xaml
     */
    public sealed class ValueConverterAggregator : List<IValueConverter>, IValueConverter
    {
        public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            return this.Aggregate(value, (current, converter)
                => converter.Convert(current, targetType, parameter, culture));
        }

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
