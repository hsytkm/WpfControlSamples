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
    public partial class ImagePage : ContentControl
    {
        public ImagePage()
        {
            InitializeComponent();

            DataContext = new ImageViewModel();
        }
    }

    class ImageViewModel : MyBindableBase
    {
        public BitmapImage MyEmbeddedImage
        {
            get => _myEmbeddedImage;
            private set => SetProperty(ref _myEmbeddedImage, value);
        }
        private BitmapImage _myEmbeddedImage;

        public ImageViewModel()
        {
            var myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var prjName = "WpfControlSamples";
            var resName = "Resources.Images.EmbeddedResource1.png";
            var fullResName = prjName + "." + resName;
            MyEmbeddedImage = BitmapImageExtension.EmbeddedResourceNameToBitmapImage(fullResName, myAssembly);
        }
    }
}