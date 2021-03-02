using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class DynamicXamlLoadPage : ContentControl
    {
        public DynamicXamlLoadPage()
        {
            InitializeComponent();
        }

        private void Button_Click_LoadXaml(object sender, RoutedEventArgs e)
        {
            var uri = new Uri("/Views/Menus/XamlFunctions/DynamicLoadTarget.xaml", UriKind.Relative);
            using var stream = Application.GetResourceStream(uri).Stream;
            var loadedControl = XamlReader.Load(stream);
            contentControl.Content = loadedControl;
        }

        private void Button_Click_ClearXaml(object sender, RoutedEventArgs e)
        {
            contentControl.Content = null;
        }
    }
}
