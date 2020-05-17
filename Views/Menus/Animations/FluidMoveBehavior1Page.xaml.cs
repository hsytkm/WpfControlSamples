using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class FluidMoveBehavior1Page : ContentControl
    {
        public FluidMoveBehavior1Page()
        {
            InitializeComponent();

            DataContext = Models.SampleData.WpfColors.Select(x => new ColorListViewItem(x)).ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(FindName("myPanel") is Panel panel)) return;

            var element = panel.Children.Cast<UIElement>().Last();
            panel.Children.Remove(element);
            panel.Children.Insert(0, element);
        }
    }
}