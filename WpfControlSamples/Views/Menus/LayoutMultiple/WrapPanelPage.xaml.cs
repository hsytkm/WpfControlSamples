using System;
using System.Collections.Generic;
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
    public partial class WrapPanelPage : ContentControl
    {
        public WrapPanelPage()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SwitchOrientation(true);
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SwitchOrientation(false);
        }

        private void SwitchOrientation(bool isHorizontal)
        {
            DataContext = isHorizontal ? Orientation.Horizontal : Orientation.Vertical;
        }
    }
}
