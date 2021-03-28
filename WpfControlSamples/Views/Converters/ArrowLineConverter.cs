using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfControlSamples.Views.Converters
{
    [ValueConversion(typeof(IReadOnlyList<Point>), typeof(Geometry))]
    class ArrowLineConverter : GenericValueConverter<IReadOnlyList<Point>, Geometry>
    {
        private const double _arrowAngle = 35d;
        private const double _arrowLengthMinDefault = 20d;
        private const double _startEllipseRadius = 3d;

        public override Geometry Convert(IReadOnlyList<Point> points, object parameter, CultureInfo culture)
        {
            if (points.Count < 2) return null;
            if (points[0] == points[1]) return null;
            var point1 = points[0];
            var point2 = points[1];

            var arrowLength = Math.Min(GetDistance(point1, point2), _arrowLengthMinDefault);
            var geometries = new Geometry[]
            {
                new LineGeometry(point1, point2),
                GetArrowPoint(point1, point2, arrowLength, _arrowAngle),
                GetArrowPoint(point1, point2, arrowLength, -_arrowAngle),
                new EllipseGeometry(point1, _startEllipseRadius, _startEllipseRadius)
            };

            var group = new GeometryGroup();
            foreach (var geometry in geometries)
            {
                group.Children.Add(geometry);
            }
            return group;

            static double GetDistance(in Point p1, in Point p2)
            {
                var x = p1.X - p2.X;
                var y = p1.Y - p2.Y;
                return Math.Sqrt(x * x + y * y);
            }

            static LineGeometry GetArrowPoint(in Point point1, in Point point2, double arrowLength, double angle)
            {
                static double AngleToRadian(double angle) => angle * Math.PI / 180d;

                var radian = Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
                var d = radian + AngleToRadian(angle);
                var x = point2.X - arrowLength * Math.Cos(d);
                var y = point2.Y - arrowLength * Math.Sin(d);
                return new LineGeometry(point2, new Point(x, y));
            }
        }

        public override IReadOnlyList<Point> ConvertBack(Geometry points, object parameter, CultureInfo culture) => default;
    }
}
