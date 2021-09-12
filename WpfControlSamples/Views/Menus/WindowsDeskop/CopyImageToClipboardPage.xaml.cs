using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
using WpfControlSamples.Drawing;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;
using WpfControlSamples.ViewModels;

namespace WpfControlSamples.Views.Menus
{
    public partial class CopyImageToClipboardPage : ContentControl
    {
        public CopyImageToClipboardPage()
        {
            DataContext = new CopyImageToClipboardViewModel();
            InitializeComponent();
        }
    }

    class CopyImageToClipboardViewModel : MyBindableBase
    {
        private const string _uriString = "pack://application:,,,/WpfControlSamples;component/Resources/Images/Resource4.png";
        public BitmapImage ImageSource { get; } = new BitmapImage(new(_uriString));

        // 標準メソッドでSet（透過色を扱えない）
        public ICommand CopyToClipboardWithoutAlphaCommand => _copyToClipboardWithoutAlphaCommand ??= new MyCommand(() =>
        {
            System.Windows.Clipboard.SetImage(ImageSource);
        });
        private ICommand _copyToClipboardWithoutAlphaCommand = default!;

        // 自作メソッドでSet（透過色を無理やり対応）
        public ICommand CopyToClipboardWithAlphaCommand => _copyToClipboardWithAlphaCommand ??= new MyCommand(() =>
        {
            ClipboardUtils.SetTransparentImageToClipboard(ImageSource);
        });
        private ICommand _copyToClipboardWithAlphaCommand = default!;


        public BitmapSource? CopiedImage
        {
            get => _copiedImage;
            private set => SetProperty(ref _copiedImage, value);
        }
        private BitmapSource? _copiedImage;

        // 標準メソッドでGet（透過色を正しく扱えない）
        public ICommand CopyFromClipboardWithoutAlphaCommand => _copyFromClipboardWithoutAlphaCommand ??= new MyCommand(() =>
        {
            CopiedImage = System.Windows.Clipboard.GetImage();
        });
        private ICommand _copyFromClipboardWithoutAlphaCommand = default!;

        // 自作メソッドでGet（自作メソッドでSetしていたら透過色を取れる）
        public ICommand CopyFromClipboardWithAlphaCommand => _copyFromClipboardWithAlphaCommand ??= new MyCommand(() =>
        {
            CopiedImage = ClipboardUtils.GetClipboardBitmapSource();
        });
        private ICommand _copyFromClipboardWithAlphaCommand = default!;

        // 画像をクリアする（テスト用）
        public ICommand ClearImageCommand => _clearImageCommand ??= new MyCommand(() =>
        {
            CopiedImage = null;
        });
        private ICommand _clearImageCommand = default!;
        
    }
}
