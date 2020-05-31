using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;

namespace WpfControlSamples.Views.Actions
{
    // Action のターゲット要素をその親以外にしたい場合は、
    // 基底クラスを TriggerAction でなく TargetedTriggerAction にする

    class MessageBoxShowAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty MessageProperty
            = DependencyProperty.Register(nameof(Message), typeof(string), typeof(MessageBoxShowAction));

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            var targetName = (this.AssociatedObject is FrameworkElement fe) ? $"{fe.Name}" : "";
            MessageBox.Show(targetName + " : " + Message);
        }
    }

    // Action のターゲット要素をその親以外にしたい場合は、
    // 基底クラスを TriggerAction でなく TargetedTriggerAction にする
    class TargetedMessageBoxShowAction : TargetedTriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty MessageProperty
            = DependencyProperty.Register(nameof(Message), typeof(string), typeof(TargetedMessageBoxShowAction));

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            var targetName = (this.Target is FrameworkElement fe) ? $"{fe.Name}" : "";
            MessageBox.Show(targetName + " : " + Message);
        }
    }
}
