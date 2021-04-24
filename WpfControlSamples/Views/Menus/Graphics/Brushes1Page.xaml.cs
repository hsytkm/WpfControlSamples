#nullable disable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
    public partial class Brushes1Page : ContentControl
    {
        public Brushes1Page()
        {
            InitializeComponent();

            DataContext = new Brushes1ViewModel();
        }
    }

    class Brushes1ViewModel : MyBindableBase
    {
        #region RadialGradientBrush
        public double GradientOriginX
        {
            get => _gradientOriginX;
            set
            {
                if (SetProperty(ref _gradientOriginX, value))
                    GradientOrigin = new Point(_gradientOriginX, _gradientOriginY);
            }
        }
        private double _gradientOriginX = 0.75;

        public double GradientOriginY
        {
            get => _gradientOriginY;
            set
            {
                if (SetProperty(ref _gradientOriginY, value))
                    GradientOrigin = new Point(_gradientOriginX, _gradientOriginY);
            }
        }
        private double _gradientOriginY = 0.25;

        public Point? GradientOrigin
        {
            get
            {
                if (_gradientOrigin is null)
                    _gradientOrigin = new Point(_gradientOriginX, _gradientOriginY);
                return _gradientOrigin;
            }
            private set => SetProperty(ref _gradientOrigin, value);
        }
        private Point? _gradientOrigin = null;

        #endregion

    }
}
