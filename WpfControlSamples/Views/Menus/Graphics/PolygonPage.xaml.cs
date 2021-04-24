using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class PolygonPage : ContentControl
    {
        public PointCollection PointCollection { get; } = new PointCollection(
           new[]
           {
                new Point(10,110),
                new Point(110,110),
                new Point(110,10),
           });

        public IReadOnlyList<Point> PointArray { get; } =
            new[]
            {
                new Point(110,10),
                new Point(10,110),
                new Point(110,110),
            };

        public PolygonPage()
        {
            InitializeComponent();
        }
    }
}
