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
    public partial class ImageClickPage : ContentControl
    {
        public ImageClickPage()
        {
            InitializeComponent();
            DataContext = new ImageClickViewModel();
        }
    }

    class ImageClickViewModel : MyBindableBase
    {
        public BitmapSource MyImage { get; }

        public Point MouseLeftButtonDownPointOnImageSource
        {
            get => _mouseLeftButtonDownPointOnImageSource;
            set => SetProperty(ref _mouseLeftButtonDownPointOnImageSource, value);
        }
        private Point _mouseLeftButtonDownPointOnImageSource;

        public Point MouseRightButtonDownPointOnImageSource
        {
            get => _mouseRightButtonDownPointOnImageSource;
            set => SetProperty(ref _mouseRightButtonDownPointOnImageSource, value);
        }
        private Point _mouseRightButtonDownPointOnImageSource;

        public ImageClickViewModel()
        {
            var uri = new Uri("pack://application:,,,/WpfControlSamples;component/Resources/Images/Picture1.jpg");
            using var stream = Application.GetResourceStream(uri).Stream;
            MyImage = stream.ToBitmapImage();

        }
    }
}
