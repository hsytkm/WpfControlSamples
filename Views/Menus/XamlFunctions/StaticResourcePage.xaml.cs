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
    public partial class StaticResourcePage : ContentControl
    {
        public StaticResourcePage()
        {
            InitializeComponent();

            // structはボクシングされるっぽい
            var resourceName = "myResourceInt1";
            var x = this.Resources[resourceName] as int?;

            var ratio = 100;
            DataContext = x.HasValue ? $"{resourceName} * {ratio} = \"{x.Value * ratio}\"" : null;
        }
    }
}