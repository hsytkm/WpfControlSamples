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
using WpfControlSamples.Views.Actions;

namespace WpfControlSamples.Views.Menus
{
    public partial class ImageZoomPage : ContentControl
    {
        public ImageZoomPage()
        {
            InitializeComponent();
            DataContext = new ImageZoomViewModel();
        }
    }

    class ImageZoomViewModel : MyBindableBase
    {
        public BitmapSource MyImage { get; }

        public Size MyImagePixelSize
        {
            get => _myImagePixelSize;
            set => SetProperty(ref _myImagePixelSize, value);
        }
        private Size _myImagePixelSize;

        public double ZoomRatioMin { get; } = 0.1;
        public double ZoomRatioMax { get; } = 32.0;

        public double ZoomRatio
        {
            get => _zoomRatio;
            set => SetProperty(ref _zoomRatio, value);
        }
        private double _zoomRatio = 1.0;

        public MouseWheelCtrlAction.Direction WheelDirection
        {
            get => _wheelDirection;
            set
            {
                _wheelDirection = value;
                NotifyPropertyChanged(nameof(WheelDirection));  // 値に変化がなくても通知する
            }
        }
        private MouseWheelCtrlAction.Direction _wheelDirection;

        public ICommand To1XCommand => _to1XCommand ??= new MyCommand(() => ZoomRatio = 1.0);
        private ICommand _to1XCommand;

        public Point MousePosOnImageSource
        {
            get => _mousePosOnImageSource;
            set => SetProperty(ref _mousePosOnImageSource, value);
        }
        private Point _mousePosOnImageSource;

        public Color ImageCursorColor
        {
            get => _imageCursorColor;
            set => SetProperty(ref _imageCursorColor, value);
        }
        private Color _imageCursorColor;

        public ImageZoomViewModel()
        {
            var uri = new Uri("pack://application:,,,/WpfControlSamples;component/Resources/Images/Picture1.jpg");
            using var stream = Application.GetResourceStream(uri).Stream;
            MyImage = stream.ToBitmapImage();

            UpdateImagePixelSize(ZoomRatio);

            this.PropertyChanged += (_, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(ZoomRatio):
                        UpdateImagePixelSize(ZoomRatio);
                        break;

                    case nameof(WheelDirection):
                        // 電源ON直後に enum(0) が通知されるので、渋々初期値の None を除外している。
                        // Rx使えるなら Skip(1) とかで対応したい。
                        if (WheelDirection != MouseWheelCtrlAction.Direction.None)
                        {
                            var ratioStep = 0.1;
                            var ratioPlus = (WheelDirection == MouseWheelCtrlAction.Direction.Up) ? ratioStep : -ratioStep;
                            var newRatio = ZoomRatio + ratioPlus;
                            ZoomRatio = Math.Max(ZoomRatioMin, Math.Min(ZoomRatioMax, newRatio));
                        }
                        break;

                    case nameof(MousePosOnImageSource):
                        ImageCursorColor = MyImage.GetImagePixel((int)MousePosOnImageSource.X, (int)MousePosOnImageSource.Y);
                        break;
                }
            };
        }

        private void UpdateImagePixelSize(double ratio)
        {
            if (ratio == 0) throw new ArgumentException(nameof(ratio));
            MyImagePixelSize = new Size(MyImage.PixelWidth * ratio, MyImage.PixelHeight * ratio);
        }
    }
}
