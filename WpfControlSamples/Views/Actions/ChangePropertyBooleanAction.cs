using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;

namespace WpfControlSamples.Views.Actions
{
    /*  Trigger からの bool通知に応じて Property を変更する
     *
     *  ReferenceSource
     *    https://github.com/microsoft/XamlBehaviorsWpf/blob/7c79fe973777cc261e951bb133dff1974d27cb8a/src/Microsoft.Xaml.Behaviors/Core/ChangePropertyAction.cs
     */
    class ChangePropertyBooleanAction : TargetedTriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register(nameof(PropertyName), typeof(string), typeof(ChangePropertyBooleanAction));
        public string PropertyName
        {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public static readonly DependencyProperty ValueWhenTrueProperty =
            DependencyProperty.Register(nameof(ValueWhenTrue), typeof(object), typeof(ChangePropertyBooleanAction));
        public object ValueWhenTrue
        {
            get => (object)GetValue(ValueWhenTrueProperty);
            set => SetValue(ValueWhenTrueProperty, value);
        }

        public static readonly DependencyProperty ValueWhenFalseProperty =
            DependencyProperty.Register(nameof(ValueWhenFalse), typeof(object), typeof(ChangePropertyBooleanAction));
        public object ValueWhenFalse
        {
            get => (object)GetValue(ValueWhenFalseProperty);
            set => SetValue(ValueWhenFalseProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            static bool TryGetBool(object obj, out bool value)
            {
                var (result, val) = obj switch
                {
                    bool b => (true, b),
                    DependencyPropertyChangedEventArgs e when e.NewValue is bool b => (true, b),
                    _ => (false, false)
                };

                value = val;
                return result;
            }

            if (!TryGetBool(parameter, out var b)) return;
            if (string.IsNullOrEmpty(PropertyName)) return;
            if (Target is null)
            {
                _initializeDelayParameter = b;
                return;
            }

            var newValue = b ? ValueWhenTrue : ValueWhenFalse;
            var targetType = Target.GetType();
            var propertyInfo = targetType.GetProperty(PropertyName);

            propertyInfo.SetValue(Target, newValue, null);
        }

        // 遅延用のパラメータ
        private bool? _initializeDelayParameter = null;

        protected override void OnTargetChanged(DependencyObject oldTarget, DependencyObject newTarget)
        {
            if (!_initializeDelayParameter.HasValue) return;

            // 初回の読込みで Target is null だったりするので、Target 更新時にしてみる。
            // ちょっと小手先感があるけどまぁいいや。
            var b = _initializeDelayParameter.Value;
            _initializeDelayParameter = null;
            Invoke(b);
        }

    }
}
