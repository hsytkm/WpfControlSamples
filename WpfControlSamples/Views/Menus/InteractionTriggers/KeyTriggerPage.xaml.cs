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
        public static readonly DependencyProperty KeyTextProperty =
            DependencyProperty.Register(nameof(KeyText), typeof(string), typeof(KeyTriggerPage));
        public string KeyText
        {
            get => (string)GetValue(KeyTextProperty);
            set => SetValue(KeyTextProperty, value);
        }

        public KeyTriggerPage()
        {
            DataContext = new KeyTriggerViewModel();
            InitializeComponent();

            Loaded += KeyTriggerPage_Loaded;
            Unloaded += KeyTriggerPage_Unloaded;
        }

        //[How can I capture KeyDown event on a WPF Page or UserControl object? - Stack Overflow](https://stackoverflow.com/questions/347724/how-can-i-capture-keydown-event-on-a-wpf-page-or-usercontrol-object)
        private Window? _window;

        private void KeyTriggerPage_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= KeyTriggerPage_Loaded;

            _window = Window.GetWindow(this);
            if (_window is not null)
                _window.KeyDown += Window_KeyDown;
        }

        private void KeyTriggerPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= KeyTriggerPage_Unloaded;

            if (_window is not null)
                _window.KeyDown -= Window_KeyDown;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) => KeyText = e.Key.ToString();
    }

    class KeyTriggerViewModel : MyBindableBase
    {
        public string Message
        {
            get => _message;
            private set => SetProperty(ref _message, value);
        }
        private string _message = "Push any key.";

        public ICommand PushKeyCommand => _pushKeyCommand ??=
            new MyCommand<object>(x => Message = x?.ToString() ?? "");
        private ICommand? _pushKeyCommand;

        public void PushKeyF1() => Message = nameof(PushKeyF1);
    }
}
