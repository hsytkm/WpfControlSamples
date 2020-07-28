using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(string), typeof(Uri))]
    class PathStringToUriConverter : GenericValueConverter<string, Uri>
    {
        private static readonly Uri _emptyUri = new Uri("about:blank");

        public override Uri Convert(string uriString, object parameter, CultureInfo culture)
        {
            // How to handle Empty Uri's for Path Binding to an Image Source
            // https://stackoverflow.com/questions/10027407/how-to-handle-empty-uris-for-path-binding-to-an-image-source

            return string.IsNullOrWhiteSpace(uriString) ? _emptyUri : new Uri(uriString);
        }

        public override string ConvertBack(Uri uri, object parameter, CultureInfo culture) => default;
    }
}
