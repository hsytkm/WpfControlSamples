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
        public BitmapImage MyResourceImage { get; }

        public BitmapImage MyEmbeddedImage1 { get; }

        private readonly BitmapImage _embeddedImage2;

        public BitmapSource MyEmbeddedImage2
        {
            get => _myEmbeddedImage2;
            private set => SetProperty(ref _myEmbeddedImage2, value);
        }
        private BitmapSource _myEmbeddedImage2;

        public ImageViewModel()
        {
            var uri = new Uri("pack://application:,,,/WpfControlSamples;component/Resources/Images/Resource3.png");
            using var stream = Application.GetResourceStream(uri).Stream;
            MyResourceImage = stream.ToBitmapImage();

            MyEmbeddedImage1 = LoadMyEmbeddedImage("Resources.Images.EmbeddedResource1.png");

            _embeddedImage2 = LoadMyEmbeddedImage("Resources.Images.EmbeddedResource2.png");
            MyEmbeddedImage2 = _embeddedImage2;
        }

        // 埋め込みリソース の読み出し
        private BitmapImage LoadMyEmbeddedImage(string resName)
        {
            var myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var prjName = "WpfControlSamples";
            var fullResName = prjName + "." + resName;

            using var stream = myAssembly.GetManifestResourceStream(fullResName);
            return stream.ToBitmapImage();
        }

        public ICommand ToDefaultCommand =>
            _toDefaultCommand ??= new MyCommand(() => MyEmbeddedImage2 = _embeddedImage2);
        private ICommand _toDefaultCommand;

        private int grayScaleChannelPattern;
        public ICommand ToGrayScaleCommand =>
            _toGrayScaleCommand ??= new MyCommand(() =>
            {
                // BGRchを切り替える
                grayScaleChannelPattern = (grayScaleChannelPattern + 1) % 3;
                MyEmbeddedImage2 = _embeddedImage2.ToGrayScale(grayScaleChannelPattern);
            });
        private ICommand _toGrayScaleCommand;

    }
}