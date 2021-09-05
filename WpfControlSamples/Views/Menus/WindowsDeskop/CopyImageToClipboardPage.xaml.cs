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
        private const string _uriString = "pack://application:,,,/WpfControlSamples;component/Resources/Images/Resource2.png";
        private static readonly Uri _uri = new(_uriString);
        public BitmapImage MyImage { get; } = new BitmapImage(_uri);

        public ICommand CopyToClipboardWithoutAlphaCommand => _copyToClipboardWithoutAlphaCommand ??= new MyCommand(() =>
        {
            System.Windows.Clipboard.SetImage(MyImage);
        });
        private ICommand _copyToClipboardWithoutAlphaCommand = default!;

        public ICommand CopyToClipboardWithAlphaCommand => _copyToClipboardWithAlphaCommand ??= new MyCommand(async () =>
        {
            await ClipboardUtils.SetTransparentImageToClipboardAsync(MyImage);
        });
        private ICommand _copyToClipboardWithAlphaCommand = default!;


    }
}
