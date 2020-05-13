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
    public partial class ShowWindowSubDialog : Window
    {
        public ShowWindowSubDialog()
        {
            InitializeComponent();
        }

        private void Button_Click_True(object sender, RoutedEventArgs e)
            => this.DialogResult = true;

        private void Button_Click_False(object sender, RoutedEventArgs e)
            => this.DialogResult = false;

    }
}