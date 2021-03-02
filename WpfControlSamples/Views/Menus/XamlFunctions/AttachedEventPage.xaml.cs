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
    public partial class AttachedEventPage : ContentControl
    {
        public AttachedEventPage()
        {
            InitializeComponent();
        }

        private void Border_Click(object sender, RoutedEventArgs e)
        {
            var now = DateTime.Now;
            DataContext = $"Button in Border was clicked. ({now})";
        }
    }
}
