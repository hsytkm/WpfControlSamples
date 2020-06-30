using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(string), typeof(bool))]
    class EmptyStringToTrueConverter : GenericValueConverter<string, bool>
    {
        public override bool Convert(string s, object parameter, CultureInfo culture)
        {
            if (s is null) throw new ArgumentNullException();
            return string.IsNullOrEmpty(s);
        }

        public override string ConvertBack(bool b, object parameter, CultureInfo culture) => default;
    }
}
