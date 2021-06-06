using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
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
using WpfControlSamples.Models;
using WpfControlSamples.ViewModels;
using WpfControlSamples.Views.Behaviors;

namespace WpfControlSamples.Views.Menus
{
    public partial class DataGrid8Page : ContentControl
    {
        public DataGrid8Page()
        {
            DataContext = new DataGrid8ViewModel();
            InitializeComponent();
        }
    }

    class DataGrid8ViewModel : MyBindableBase
    {
        public IReadOnlyList<ColorItemViewModel> ColorItems
            => SampleData.WpfColors.Select(x => new ColorItemViewModel(x.Name, x.Color)).ToArray();
    }

    /// <summary>DataGrid の Row になります</summary>
    record ColorItemViewModel : CompositeColoredTextBase
    {
        public ColoredText Name { get; }
        public ColoredText Hex { get; }
        protected override IEnumerable<ColoredText> GetColoredTextProperties()
        {
            yield return Name;
            yield return Hex;
        }

        public ColorItemViewModel(string name, Color color)
        {
            Name = new ColoredText(name);
            Hex = new ColoredText(color.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
    }

    sealed class ColorItemDataGridFilterWordBehavior : DataGridFilterWordBehavior<ColorItemViewModel> { }
}
