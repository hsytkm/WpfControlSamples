using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
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
        private const string _colorBrushResourceKey = "MyColorBrushKey";
        private const string _imageBrushResourceKey = "MyImageBrushKey";
        private const string _myMessageResourceKey = "MyMessageKey";

        public DynamicResourcePage()
        {
            InitializeComponent();

            SwitchResource(isEnableResource1: true);
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            SwitchResource(isEnableResource1: true);
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            SwitchResource(isEnableResource1: false);
        }

        private void SwitchResource(bool isEnableResource1)
        {
            // SolidColorBrush
            this.Resources[_colorBrushResourceKey] = isEnableResource1 ? Brushes.Blue : Brushes.Red;

            // ImageBrush
            var uriString = isEnableResource1
                ? "pack://application:,,,/Resources/Images/Resource1.png"   // 画像は「リソース」
                : "pack://application:,,,/Resources/Images/Resource2.png";  // 画像は「リソース」
            var bitmapImage = new BitmapImage(new Uri(uriString)).WithFreeze();
            var imageBrush = new ImageBrush(bitmapImage).WithFreeze();
            this.Resources[_imageBrushResourceKey] = imageBrush;

            // String
            this.Resources[_myMessageResourceKey] = isEnableResource1 ? "リソース１" : "Resource2";

            DataContext = isEnableResource1;
        }
    }

    [ValueConversion(typeof(SolidColorBrush), typeof(string))]
    class SolidColorBrushToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return null;
            if (value is SolidColorBrush brush)
            {
                return Models.SampleData.WpfSolidColorBrushes
                    .FirstOrDefault(x => x.Brush.Color == brush.Color).Name;
            }

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
