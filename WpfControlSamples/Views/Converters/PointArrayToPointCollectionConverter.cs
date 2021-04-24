#nullable disable
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(Point[]), typeof(PointCollection))]
    class PointArrayToPointCollectionConverter : GenericValueConverter<Point[], PointCollection>
    {
        public override PointCollection Convert(Point[] points, object parameter, CultureInfo culture) =>
            new PointCollection(points);

        public override Point[] ConvertBack(PointCollection points, object parameter, CultureInfo culture) =>
            points.ToArray();
    }
}
