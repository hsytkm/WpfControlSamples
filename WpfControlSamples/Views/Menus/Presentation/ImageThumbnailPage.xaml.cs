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
    public partial class ImageThumbnailPage : ContentControl
    {
        public ImageThumbnailPage()
        {
            InitializeComponent();
            DataContext = new ImageThumbnailViewModel();
        }
    }

    class ImageThumbnailViewModel : MyBindableBase
    {
        // 埋め込みサムネイルが存在しなかった場合に、主画像をリサイズする最大幅
        // (埋め込みサムネイルが存在しなかったことを分かりやすいように大きくしとく）
        private const double _thumbnailImageWidthMax = 1280;

        public string SelectedOpenPath
        {
            get => _selectedOpenPath;
            set
            {
                if (SetProperty(ref _selectedOpenPath, value))
                {
                    SelectedImage = value?.FilePathToBitmapSourceFromEmbeddedThumbnail(_thumbnailImageWidthMax);
                }
            }
        }
        private string _selectedOpenPath;

        public BitmapSource SelectedImage
        {
            get => _selectedImage;
            private set
            {
                if (SetProperty(ref _selectedImage, value))
                {
                    ImageSize = value is null ? new Size() : new Size(value.PixelWidth, value.PixelHeight);
                }
            }
        }
        private BitmapSource _selectedImage;

        public Size ImageSize
        {
            get => _imageSize;
            private set => SetProperty(ref _imageSize, value);
        }
        private Size _imageSize;
    }
}