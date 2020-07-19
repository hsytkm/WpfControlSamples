using System;
using System.Collections.Generic;
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

            itemsControl.Loaded += ItemsControl_Loaded;
        }

        private static IEnumerable<MovableRectangle2> CreateMovableRectangle2(double canvasWidth, double canvasHeight)
        {
            var brushes = new[]
            {
                Brushes.Bisque,
                Brushes.SlateBlue,
                Brushes.LightGoldenrodYellow,
                Brushes.LightSalmon,
                Brushes.Transparent,
                Brushes.CornflowerBlue,
            };

            return brushes.Select((brush, index) => new MovableRectangle2()
                {
                    Index = index,
                    CanvasWidthMax = canvasWidth,
                    CanvasHeightMax = canvasHeight,
                    Background = brush,
                    BorderBrush = Brushes.Gray
                });
        }

        private void ItemsControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is ItemsControl itemsControl)) return;

            if (itemsControl.TryGetChildControl<Canvas>(out var canvas))
            {
                // Canvas の Load 完了後に子要素を追加する
                DataContext = CreateMovableRectangle2(canvas.ActualWidth, canvas.ActualHeight);
            }
        }
    }
}