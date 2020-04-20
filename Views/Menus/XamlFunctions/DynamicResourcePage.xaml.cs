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
    public partial class DynamicResourcePage : ContentControl
    {
        private const string _colorBrushResourceKey = "MyColorBrush";
        private const string _imageBrushResourceKey = "MyImageBrush";

        public DynamicResourcePage()
        {
            InitializeComponent();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            var color = Colors.Blue;
            var uri = new Uri("pack://application:,,,/Resources/Images/Resource1.png"); // 画像は「リソース」
            SetResources(ref color, uri);
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            var color = Colors.Red;
            var uri = new Uri("pack://application:,,,/Resources/Images/Resource2.png"); // 画像は「リソース」
            SetResources(ref color, uri);
        }

        private void SetResources(ref Color color, Uri uri)
        {
            //var brush = (SolidColorBrush)this.FindResource(_colorBrushResourceKey);

            this.Resources[_colorBrushResourceKey] = color.ToFreezedSolidColorBrush();

            var bitmapImage = new BitmapImage(uri);
            bitmapImage.Freeze();
            var imageBrush = new ImageBrush(bitmapImage);
            imageBrush.Freeze();
            this.Resources[_imageBrushResourceKey] = imageBrush;
        }
    }
}