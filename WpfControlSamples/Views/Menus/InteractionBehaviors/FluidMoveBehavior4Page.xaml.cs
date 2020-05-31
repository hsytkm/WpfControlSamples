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
    public partial class FluidMoveBehavior4Page : ContentControl
    {
        public FluidMoveBehavior4Page()
        {
            InitializeComponent();

            var items = new ObservableCollection<MyItemType>
            {
                new MyItemType(new Uri("pack://application:,,,/WpfControlSamples;component/Resources/Images/Resource1.png")),
                new MyItemType(new Uri("pack://application:,,,/WpfControlSamples;component/Resources/Images/Resource2.png")),
                new MyItemType(new Uri("pack://application:,,,/WpfControlSamples;component/Resources/Images/Resource3.png"))
            };
            DataContext = items;
        }
    }

    class MyItemType
    {
        public ImageSource Image { get; }
        public MyItemType(Uri uri) => Image = new BitmapImage(uri);
    }
}