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
    public partial class ItemsControl1Page : ContentControl
    {
        public ItemsControl1Page()
        {
            InitializeComponent();

            DataContext = new ItemsControl1ViewModel();
        }
    }

    class ItemsControl1ViewModel : MyBindableBase
    {
        public IList<ColorListViewItem> SourceColors { get; } =
            Models.SampleData.WpfColors.Select(x => new ColorListViewItem(x)).ToList();

        public ColorListViewItem SelectedColor
        {
            get => _selectedColor;
            private set => SetProperty(ref _selectedColor, value);
        }
        private ColorListViewItem _selectedColor;

        public ICommand RadioButtonChangedCommand => _radioButtonChangedCommand ??=
            (_radioButtonChangedCommand = new MyCommand<ColorListViewItem>(item => SelectedColor = item));
        private ICommand _radioButtonChangedCommand;

    }
}
