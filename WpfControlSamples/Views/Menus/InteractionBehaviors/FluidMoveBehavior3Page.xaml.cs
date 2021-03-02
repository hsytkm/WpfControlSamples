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
    public partial class FluidMoveBehavior3Page : ContentControl
    {
        public FluidMoveBehavior3Page()
        {
            InitializeComponent();
        }
        private void OnMoveToRight(object sender, RoutedEventArgs e)
        {
            MoveChildren(leftPanel, rightPanel);
        }

        private void OnMoveToLeft(object sender, RoutedEventArgs e)
        {
            MoveChildren(rightPanel, leftPanel);
        }

        private void MoveChildren(Panel from, Panel to)
        {
            var elements = from.Children.OfType<UIElement>().ToList();
            from.Children.Clear();

            foreach (var element in elements)
            {
                to.Children.Add(element);
            }
        }
    }
}
