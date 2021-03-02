using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class HideControlAnimation2Page : ContentControl
    {
        public HideControlAnimation2Page()
        {
            InitializeComponent();
            DataContext = new HideControlAnimation2ViewModel();
        }
    }

    class HideControlAnimation2ViewModel : MyBindableBase
    {
        public bool IsShowControl
        {
            get => _isShowControl;
            private set => SetProperty(ref _isShowControl, value);
        }
        private bool _isShowControl;

        public ICommand SwitchShowHideCommand => _switchShowHideCommand ??=
            new MyCommand(() => IsShowControl = !IsShowControl);
        private ICommand _switchShowHideCommand;
    }
}
