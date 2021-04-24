#nullable disable
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
    public partial class SliderPage : ContentControl
    {
        public SliderPage()
        {
            InitializeComponent();

            DataContext = new SliderViewModel();
        }
    }

    class SliderViewModel : MyBindableBase
    {
        public double SliderMin => -5;
        public double SliderMax => 5;

        public double SliderValue
        {
            get => _sliderValue;
            set => SetProperty(ref _sliderValue, value);
        }
        private double _sliderValue;

        public int SliderIntValue
        {
            get => _sliderIntValue;
            set => SetProperty(ref _sliderIntValue, value);
        }
        private int _sliderIntValue;

    }
}
