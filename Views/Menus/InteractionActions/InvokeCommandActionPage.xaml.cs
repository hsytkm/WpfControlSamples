using Microsoft.Xaml.Behaviors;
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
    public partial class InvokeCommandActionPage : ContentControl
    {
        public InvokeCommandActionPage()
        {
            InitializeComponent();
            DataContext = new InvokeCommandActionViewModel();
        }
    }

    class InvokeCommandActionViewModel : MyBindableBase
    {
        public ICommand MessageCommand => _messageCommand ??
            (_messageCommand = new MyCommand<string>(msg => Message = msg));
        private ICommand _messageCommand;

        public string Message
        {
            get => _message;
            private set => SetProperty(ref _message, value);
        }
        private string _message;

        public ICommand MouseMoveToPointCommand1 => _mouseMoveToPointCommand1 ??
            (_mouseMoveToPointCommand1 = new MyCommand<FrameworkElement>(fe =>
            {
                var point = Mouse.GetPosition((IInputElement)fe);
                MousePoint1 = $"MousePoint1: ({point.X:f1}, {point.Y:f1})";
            }));
        private ICommand _mouseMoveToPointCommand1;

        public string MousePoint1
        {
            get => _mousePoint1;
            private set => SetProperty(ref _mousePoint1, value);
        }
        private string _mousePoint1;

        public ICommand MouseMoveToPointCommand2 => _mouseMoveToPointCommand2 ??
            (_mouseMoveToPointCommand2 = new MyCommand<FrameworkElement>(fe =>
            {
                var point = Mouse.GetPosition((IInputElement)fe);
                MousePoint2 = $"MousePoint2: ({point.X:f1}, {point.Y:f1})";
            }));
        private ICommand _mouseMoveToPointCommand2;

        public string MousePoint2
        {
            get => _mousePoint2;
            private set => SetProperty(ref _mousePoint2, value);
        }
        private string _mousePoint2;
    }
}