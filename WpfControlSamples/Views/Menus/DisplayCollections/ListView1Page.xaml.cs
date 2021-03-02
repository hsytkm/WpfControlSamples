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
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class ListView1Page : ContentControl
    {
        public ListView1Page()
        {
            InitializeComponent();

            DataContext = new ListView1ViewModel();
        }
    }

    class ListView1ViewModel : MyBindableBase
    {
        public IList<ColorListViewItem> SourceColors { get; } =
            Models.SampleData.WpfColors.Select(x => new ColorListViewItem(x)).ToList();

        public ColorListViewItem SelectedColor
        {
            get => _selectedColor;
            set => SetProperty(ref _selectedColor, value);
        }
        private ColorListViewItem _selectedColor;

        public ListView1ViewModel()
        {
            SelectedColor = SourceColors[3];    // Initialize
        }
    }
}
