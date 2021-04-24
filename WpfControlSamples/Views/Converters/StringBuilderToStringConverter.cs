#nullable disable
using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(StringBuilder), typeof(string))]
    class StringBuilderToStringConverter : GenericValueConverter<StringBuilder, string>
    {
        public override string Convert(StringBuilder sb, object parameter, CultureInfo culture) => sb.ToString();

        public override StringBuilder ConvertBack(string s, object parameter, CultureInfo culture) => default;
    }
}
