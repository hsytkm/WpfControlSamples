using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
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
    public partial class MovableRectangle2Page : ContentControl
    {
        public MovableRectangle2Page()
        {
            InitializeComponent();
            DataContext = new MovableRectangle2ViewModel();
        }
    }

    class PolygonRectangle : MyBindableBase
    {
        public int Index { get; }
        public Color ThemeColor1 { get; }
        public Color ThemeColor2 { get; }
        public Point[] CornerPoints
        {
            get => _cornerPoints;
            set => SetProperty(ref _cornerPoints, value);
        }
        private Point[] _cornerPoints;
        public PolygonRectangle(int index, Color themeColor)
        {
            Index = index;
            ThemeColor1 = themeColor;
            ThemeColor2 = Colors.Gray;
            CornerPoints = GetDefaultCornerPoints(index);
        }
        private static Point[] GetDefaultCornerPoints(int index)
        {
            var offset = 20.0 * (index % 5);
            var width = 100.0;
            var height = 60.0;
            return new[]
            {
                new Point(offset, offset),
                new Point(offset + width, offset),
                new Point(offset + width, offset + height),
                new Point(offset, offset + height),
            };
        }
    }

    class MovableRectangle2ViewModel : MyBindableBase
    {
        public ObservableCollection<PolygonRectangle> PolygonRectangles { get; } =
            new ObservableCollection<PolygonRectangle>();

        public MovableRectangle2ViewModel()
        {
            var themeColors = new[]
            {
                Colors.Bisque,
                Colors.SlateBlue,
                Colors.LightGoldenrodYellow,
                Colors.LightSalmon,
                Colors.Transparent,
                Colors.CornflowerBlue,
            };

            for (int i = 0; i < themeColors.Length; ++i)
            {
                PolygonRectangles.Add(new PolygonRectangle(i, themeColors[i]));
            }
        }
    }
}
