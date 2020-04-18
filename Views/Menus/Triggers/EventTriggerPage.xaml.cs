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
    public partial class EventTriggerPage : ContentControl
    {
        public EventTriggerPage()
        {
            InitializeComponent();

            Mouse.GetPosition((IInputElement)this);
        }
    }

    class EventTriggerViewModel : MyBindableBase
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

        public ICommand MouseMoveToPointCommand => _mouseMoveToPointCommand ??
            (_mouseMoveToPointCommand = new MyCommand<FrameworkElement>(fe =>
            {
                var point = Mouse.GetPosition((IInputElement)fe);
                MousePoint = $"MousePoint: ({point.X:f1}, {point.Y:f1})";
            }));
        private ICommand _mouseMoveToPointCommand;

        public string MousePoint
        {
            get => _mousePoint;
            private set => SetProperty(ref _mousePoint, value);
        }
        private string _mousePoint;

        public void ViewModelMethod()
        {
            MessageBox.Show("Call ViewModelMethod!!");
        }
    }
}