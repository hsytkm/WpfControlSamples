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
    /// <summary>
    /// ProcessingPage.xaml の相互作用ロジック
    /// </summary>
    public partial class ListViewPage : ContentControl
    {
        public ListViewPage()
        {
            InitializeComponent();

            DataContext = new ListViewViewModel();
        }
    }

    class ListViewViewModel : MyBindableBase
    {
        public IList<ColorListViewItem> SourceColors { get; } =
            Models.SampleData.WpfColors.Select(x => new ColorListViewItem(x)).ToList();

        public ColorListViewItem SelectedColor
        {
            get => _selectedColor;
            set => SetProperty(ref _selectedColor, value);
        }
        private ColorListViewItem _selectedColor;

        public ListViewViewModel()
        {
            SelectedColor = SourceColors[3];    // Initialize
        }
    }
}