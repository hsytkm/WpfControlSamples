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
    public partial class ComboBox3Page : ContentControl
    {
        public ComboBox3Page()
        {
            InitializeComponent();
            DataContext = new ComboBox3ViewModel();
        }
    }

    class ComboBox3ViewModel : MyBindableBase
    {
        public IList<string> Colors { get; } = Models.SampleData.WpfColors.Select(x => x.Name).ToList();

        public ICommand ShowValueCommand => new MyCommand<string>(x => MessageBox.Show($"Value = {x ?? "null"}"));
    }
}
