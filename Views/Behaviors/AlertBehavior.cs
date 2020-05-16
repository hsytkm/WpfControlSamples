﻿using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace WpfControlSamples.Views.Behaviors
{
    // WPFのBehaviorをStyleで使う方法
    // https://blog.okazuki.jp/entry/2016/07/19/192918
    [TypeConstraint(typeof(Button))]
    public class AlertBehavior : Behavior<Button>
    {
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(AlertBehavior));

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Click += ButtonClicked;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Click -= ButtonClicked;
            base.OnDetaching();
        }

        private void ButtonClicked(object sender, RoutedEventArgs e)
            => MessageBox.Show(Message);
    }
}
