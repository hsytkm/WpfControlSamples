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
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class ToggleButton1Page : ContentControl
    {
        public ToggleButton1Page()
        {
            InitializeComponent();
            DataContext = new ToggleButton1ViewModel();
        }
    }

    class ToggleButton1ViewModel : MyBindableBase
    {
        public bool ToggleFlag
        {
            get => _toggleFlag;
            private set => SetProperty(ref _toggleFlag, value);
        }
        private bool _toggleFlag;

        public ICommand OnCommand => _onCommand ??= new MyCommand(() => ToggleFlag = true);
        private ICommand _onCommand;

        public ICommand OffCommand => _offCommand ??= new MyCommand(() => ToggleFlag = false);
        private ICommand _offCommand;
    }
}