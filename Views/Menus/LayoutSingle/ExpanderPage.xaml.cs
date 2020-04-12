using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class ExpanderPage : ContentControl
    {
        public ExpanderPage()
        {
            InitializeComponent();

            DataContext = new ExpanderViewModel();
        }
    }

    class ExpanderViewModel : MyBindableBase
    {
        private static IList<(string Name, Color Color)> Colors =>
            Models.SampleData.WpfColors;

        public string BlueName
        {
            get => _blueName;
            set => SetProperty(ref _blueName, value);
        }
        private string _blueName;

        public string BlueColors
        {
            get => _blueColors;
            set => SetProperty(ref _blueColors, value);
        }
        private string _blueColors;

        public string RedName
        {
            get => _redName;
            set => SetProperty(ref _redName, value);
        }
        private string _redName;

        public string RedColors
        {
            get => _redColors;
            set => SetProperty(ref _redColors, value);
        }
        private string _redColors;

        public string GreenName
        {
            get => _greenName;
            set => SetProperty(ref _greenName, value);
        }
        private string _greenName;

        public string GreenColors
        {
            get => _greenColors;
            set => SetProperty(ref _greenColors, value);
        }
        private string _greenColors;

        public ExpanderViewModel()
        {
            BlueName = "Blue Colors";
            BlueColors = string.Join(Environment.NewLine,
                Models.SampleData.WpfColors
                    .Where(x => x.Name.Contains("Blue"))
                    .Select(x => x.Name));

            RedName = "Red Colors";
            RedColors = string.Join(Environment.NewLine,
                Models.SampleData.WpfColors
                    .Where(x => x.Name.Contains("Red"))
                    .Select(x => x.Name));

            GreenName = "Green Colors";
            GreenColors = string.Join(Environment.NewLine,
                Models.SampleData.WpfColors
                    .Where(x => x.Name.Contains("Green"))
                    .Select(x => x.Name));
        }
    }
}