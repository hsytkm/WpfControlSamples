#nullable disable
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
    public partial class PropetriesResourcesPage : ContentControl
    {
        public PropetriesResourcesPage()
        {
            InitializeComponent();
        }
    }

    class PropetriesResourcesViewModel : MyBindableBase
    {
        public string ResourceMyString { get; } = Properties.Resources.MyString;

        public bool? ResourceMyBoolean
        {
            get => Properties.Resources.MyBoolean.ToLowerInvariant() switch
            {
                "true" => true,
                "false" => false,
                _ => null,
            };
        }

        public int? ResourceMyInteger
        {
            get => int.TryParse(Properties.Resources.MyInteger, out var i) ? i : default(int?);
        }

        public double? ResourceMyDouble
        {
            get => double.TryParse(Properties.Resources.MyDouble, out var i) ? i : default(double?);
        }

    }
}
