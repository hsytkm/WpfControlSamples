using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;

namespace WpfControlSamples.Views.Triggers
{
    class MyMessageTestAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty MessageProperty
            = DependencyProperty.Register(nameof(Message), typeof(string), typeof(MyMessageTestAction), null);

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            MessageBox.Show(Message);
        }
    }
}
