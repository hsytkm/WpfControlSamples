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
    public partial class ComboBoxPage : ContentControl
    {
        public ComboBoxPage()
        {
            InitializeComponent();

            DataContext = new ComboBoxViewModel();
        }
    }

    class ComboBoxViewModel : MyBindableBase
    {
        private static IList<(string Name, Color Color)> Colors =>
            Models.SampleData.WpfColors;

        public IList<string> ItemsSource { get; } =
            Colors.Select(x => x.Name).ToList();

        public string SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (SetProperty(ref _selectedItem, value))
                    SelectedColor = Colors.First(x => x.Name == value).Color;
            }
        }
        private string _selectedItem;

        public Color SelectedColor
        {
            get => _selectedColor;
            set => SetProperty(ref _selectedColor, value);
        }
        private Color _selectedColor;

        public ComboBoxViewModel()
        {
            SelectedItem = Colors.First().Name;
        }
    }
}