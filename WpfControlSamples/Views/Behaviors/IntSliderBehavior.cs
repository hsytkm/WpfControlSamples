using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace WpfControlSamples.Views.Behaviors
{
    // ◆Behavior から TriggerAction に作り替えた方が良い気がするが、そもそも使わないので放っておく。
    class IntSliderBehavior : Behavior<Slider>
    {
        public static readonly DependencyProperty IntValueProperty =
            DependencyProperty.Register(nameof(IntValue), typeof(int), typeof(IntSliderBehavior));

        public int IntValue
        {
            get => (int)GetValue(IntValueProperty);
            set => SetValue(IntValueProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.ValueChanged += Slider_ValueChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.ValueChanged -= Slider_ValueChanged;

            base.OnDetaching();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            IntValue = (int)e.NewValue;
        }
    }
}
