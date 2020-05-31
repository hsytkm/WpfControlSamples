using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class MultiBinding2Page : ContentControl
    {
        public MultiBinding2Page()
        {
            InitializeComponent();
            DataContext = new MultiBinding2ViewModel();
        }
    }

    class MultiBinding2ViewModel : MyBindableBase
    {
        public int InputParam1
        {
            get => _inputParam1;
            set => SetProperty(ref _inputParam1, value);
        }
        private int _inputParam1;

        public int InputParam2
        {
            get => _inputParam2;
            set => SetProperty(ref _inputParam2, value);
        }
        private int _inputParam2 = 123;
    }
}