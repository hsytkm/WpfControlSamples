#nullable disable
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
    public partial class RadioButton2Page : ContentControl
    {
        public RadioButton2Page()
        {
            // InitializeComponent の前（or xaml)なら期待通りに動作する
            DataContext = new RadioButton2ViewModel();

            InitializeComponent();

            // InitializeComponent の後なら RadioButton の初期値が設定されない（Loaded で バインドを解決できない）
            //DataContext = new RadioButton2ViewModel();
        }
    }

    class RadioButton2ViewModel : MyBindableBase
    {
        public static IReadOnlyList<ColorListViewItem> SourceColors { get; } =
            Models.SampleData.WpfColors.Take(20).Select(x => new ColorListViewItem(x)).ToArray();

        public ColorListViewItem SelectedColor
        {
            get => _selectedColor;
            set => SetProperty(ref _selectedColor, value);
        }
        private ColorListViewItem _selectedColor = SourceColors.ElementAt(2);
    }
}
