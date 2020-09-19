using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class ListView4Page : ContentControl
    {
        public ListView4Page()
        {
            InitializeComponent();
            DataContext = new ListView4ViewModel();
        }
    }

    class ListView4ViewModel : MyBindableBase
    {
        public IList<ColorListViewItem> SourceColors1 { get; } =
            Models.SampleData.WpfColors.Select(x => new ColorListViewItem(x)).ToList();

        public IList<ListView4ColorData> SourceColors2 { get; } =
            Models.SampleData.WpfColors.Select(x => new ListView4ColorData(x.Name, x.Color)).ToList();
    }

    class ListView4ColorData
    {
        public string Name { get; }
        public byte[] Colors { get; }
        public ListView4ColorData(string n, Color c)
        {
            Name = n;
            Colors = new[] { c.R, c.G, c.B };
        }
    }
}