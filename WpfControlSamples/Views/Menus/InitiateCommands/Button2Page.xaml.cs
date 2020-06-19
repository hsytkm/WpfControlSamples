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
    public partial class Button2Page : ContentControl
    {
        public Button2Page()
        {
            InitializeComponent();
            DataContext = new Button2ViewModel();
        }
    }

    class Button2ViewModel : MyBindableBase
    {
        private bool _isEnabled;

        public ICommand StartCommand =>
            _startCommand ??= new MyCommand(
                () => SwitchState(start: true),
                () => !_isEnabled);
        private ICommand _startCommand;

        public ICommand StopCommand =>
            _stopCommand ??= new MyCommand(
                () => SwitchState(start: false),
                () => _isEnabled);
        private ICommand _stopCommand;

        private void SwitchState(bool start)
        {
            _isEnabled = start;
            (StartCommand as MyCommand).ChangeCanExecute();
            (StopCommand as MyCommand).ChangeCanExecute();
        }


        public int Counter
        {
            get => _counter;
            private set => SetProperty(ref _counter, value);
        }
        private int _counter;

        public bool EnableButton
        {
            get => _enableButton;
            set
            {
                if (SetProperty(ref _enableButton, value))
                    (SwitchFlagCommand as MyCommand).ChangeCanExecute();
            }
        }
        private bool _enableButton;

        public ICommand SwitchFlagCommand => _switchFlagCommand ??= new MyCommand(
            () => Counter++,
            () => EnableButton);
        private ICommand _switchFlagCommand;
    }
}