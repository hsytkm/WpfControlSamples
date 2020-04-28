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
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class ButtonPage : ContentControl
    {
        public ICommand ButtonClick => _buttonClick ??
            (_buttonClick = new MyCommand<string>(t => MessageBox.Show(t + " is pushed!", "PushPush")));
        private ICommand _buttonClick;

        #region SwitchButtons
        private bool _isEnabled;

        public ICommand StartCommand => _startCommand ??
            (_startCommand = new MyCommand(
                () => SwitchState(start: true),
                () => !_isEnabled));
        private ICommand _startCommand;

        public ICommand StopCommand => _stopCommand ??
            (_stopCommand = new MyCommand(
                () => SwitchState(start: false),
                () => _isEnabled));
        private ICommand _stopCommand;

        private void SwitchState(bool start)
        {
            _isEnabled = start;
            (StartCommand as MyCommand).ChangeCanExecute();
            (StopCommand as MyCommand).ChangeCanExecute();
        }
        #endregion

        public ButtonPage()
        {
            InitializeComponent();

            DataContext = ButtonClick;
        }
    }
}