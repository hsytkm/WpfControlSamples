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
using WpfControlSamples.Infrastructures;
using WpfControlSamples.Views.Behaviors;

namespace WpfControlSamples.Views.Menus
{
    public partial class UniformGrid2Page : ContentControl
    {
        public UniformGrid2Page()
        {
            DataContext = new UniformGrid2ViewModel();
            InitializeComponent();
        }
    }

    class UniformGrid2ViewModel : MyBindableBase
    {
        public ColorArray2d? ColorMatrix
        {
            get => _colorMatrix;
            private set => SetProperty(ref _colorMatrix, value);
        }
        private ColorArray2d? _colorMatrix;

        public ICommand UpdateCommand => _updateCommand ??= new MyCommand(Update);
        private ICommand _updateCommand = default!;
        public ICommand ClearCommand => _clearCommand ??= new MyCommand(() => ColorMatrix = null);
        private ICommand _clearCommand = default!;

        #region Layout
        public double LayoutRotateAngle
        {
            get => _layoutRotateAngle;
            private set => SetProperty(ref _layoutRotateAngle, value);
        }
        private double _layoutRotateAngle;

        public ICommand LayoutRotateClockwiseCommand => _layoutRotateClockwiseCommand ??= new MyCommand(() => LayoutRotateAngle += 90d);
        private ICommand _layoutRotateClockwiseCommand = default!;
        public ICommand LayoutRotateCounterClockwiseCommand => _layoutRotateCounterClockwiseCommand ??= new MyCommand(() => LayoutRotateAngle -= 90d);
        private ICommand _layoutRotateCounterClockwiseCommand = default!;
        #endregion

        public UniformGrid2ViewModel() { }

        public void Update()
        {
            var ary2d = new Color[,]
            {
                { Colors.AliceBlue, Colors.AntiqueWhite, Colors.Aqua },
                { Colors.Bisque, Colors.Beige, Colors.BlanchedAlmond },
                { Colors.CadetBlue, Colors.Chocolate, Colors.Coral },
                { Colors.DarkBlue, Colors.DarkCyan, Colors.DarkGreen },
            };

            ColorMatrix = new ColorArray2d(ary2d);
        }
    }
}
