using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(string), typeof(Uri))]
    class PathStringToUriConverter : IValueConverter
    {
        private readonly static Uri _emptyUri = new Uri("about:blank");

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // How to handle Empty Uri's for Path Binding to an Image Source
            // https://stackoverflow.com/questions/10027407/how-to-handle-empty-uris-for-path-binding-to-an-image-source
            if (value is null) return _emptyUri;

            if (value is string path) return new Uri(path);

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
