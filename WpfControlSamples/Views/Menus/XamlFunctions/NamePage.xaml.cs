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
    public partial class NamePage : ContentControl
    {
        public NamePage()
        {
            InitializeComponent();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            // パターン1
            textBlock1.Foreground = Brushes.Red;
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            // パターン2
            if (FindName("textBlock2") is not TextBlock textBlock) return;
            textBlock.Background = Brushes.LightPink;
        }

    }
}
