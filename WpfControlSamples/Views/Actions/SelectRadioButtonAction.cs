using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfControlSamples.Views.Actions
{
    class SelectRadioButtonAction : TargetedTriggerAction<RadioButton>
    {
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(SelectRadioButtonAction));
        public object SelectedItem
        {
            get => (object)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            if (!(Target is RadioButton radio)) return;

            var data = radio.DataContext;
            if (data is null) return;

            var type = data.GetType();
            var item = type.IsEnum ? Enum.Parse(type, data.ToString() ?? "") : data;

            // Loadedイベントのタイミングで選択されてるボタンを有効化する
            if (SelectedItem?.Equals(item) ?? false) radio.IsChecked = true;

            SelectedItem = item;
        }
    }
}
