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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class RotateAngleAnimationPage : ContentControl
    {
        public RotateAngleAnimationPage()
        {
            DataContext = new RotateAngleAnimationViewModel();
            InitializeComponent();
        }
    }

    class RotateAngleAnimationViewModel : MyBindableBase
    {
        public BitmapSource ImageSource { get; }
        private const double _rotateAngle = 90;

        #region Render
        public double RenderRotateAngle
        {
            get => _renderRotateAngle;
            private set => SetProperty(ref _renderRotateAngle, value);
        }
        private double _renderRotateAngle;

        public ICommand RenderRotateClockwiseCommand => _renderRotateClockwiseCommand ??=
            new MyCommand(() => RenderRotateAngle += _rotateAngle);
        private ICommand _renderRotateClockwiseCommand = default!;

        public ICommand RenderRotateCounterClockwiseCommand => _renderRotateCounterClockwiseCommand ??=
            new MyCommand(() => RenderRotateAngle -= _rotateAngle);
        private ICommand _renderRotateCounterClockwiseCommand = default!;
        #endregion

        #region Layout
        public double LayoutRotateAngle
        {
            get => _layoutRotateAngle;
            private set => SetProperty(ref _layoutRotateAngle, value);
        }
        private double _layoutRotateAngle;

        public ICommand LayoutRotateClockwiseCommand => _layoutRotateClockwiseCommand ??=
            new MyCommand(() => LayoutRotateAngle += _rotateAngle);
        private ICommand _layoutRotateClockwiseCommand = default!;

        public ICommand LayoutRotateCounterClockwiseCommand => _layoutRotateCounterClockwiseCommand ??=
            new MyCommand(() => LayoutRotateAngle -= _rotateAngle);
        private ICommand _layoutRotateCounterClockwiseCommand = default!;
        #endregion

        public Point MousePosOnImageSource
        {
            get => _mousePosOnImageSource;
            set => SetProperty(ref _mousePosOnImageSource, value);
        }
        private Point _mousePosOnImageSource;

        public RotateAngleAnimationViewModel()
        {
            var uri = new Uri("pack://application:,,,/WpfControlSamples;component/Resources/Images/Picture1.jpg");
            using var stream = Application.GetResourceStream(uri).Stream;
            ImageSource = stream.ToBitmapImage();
        }
    }
}
