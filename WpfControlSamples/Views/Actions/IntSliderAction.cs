using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfControlSamples.Views.Actions
{
    public class IntSliderAction : TriggerAction<Slider>
    {
        public static readonly DependencyProperty IntValueProperty =
            DependencyProperty.Register(nameof(IntValue), typeof(int), typeof(IntSliderAction));
        public int IntValue
        {
            get => (int)GetValue(IntValueProperty);
            set => SetValue(IntValueProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            if (parameter is not RoutedPropertyChangedEventArgs<double> args) return;
            IntValue = (int)Math.Round(args.NewValue);
        }
    }
}
