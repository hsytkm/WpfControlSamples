using System;
using System.Collections.Generic;
using System.Configuration;
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
    public partial class KeyTriggerPage : ContentControl
    {
        public KeyTriggerPage()
        {
            InitializeComponent();
            DataContext = new KeyTriggerViewModel();
        }
    }

    class KeyTriggerViewModel : MyBindableBase
    {
        public string Message
        {
            get => _message;
            private set => SetProperty(ref _message, value);
        }
        private string _message = "Push any key.";

        public ICommand PushKeyCommand => _pushKeyCommand ??
            (_pushKeyCommand = new MyCommand<object>(x => Message = x.ToString()));
        private ICommand _pushKeyCommand;

        public void PushKeyF1() => Message = nameof(PushKeyF1);

    }
}