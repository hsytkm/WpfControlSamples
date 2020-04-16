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

namespace WpfControlSamples.Views.Menus
{
    public partial class DependencyPropertyPage : ContentControl
    {
        public DependencyPropertyPage()
        {
            InitializeComponent();
        }

        private const int MinAge = 0;
        private const int MaxAge = 128;

        public static readonly DependencyProperty AgeProperty =
            DependencyProperty.Register(
                name: nameof(Age),
                propertyType: typeof(string),
                ownerType: typeof(DependencyPropertyPage),
                // 以降は省略可
                typeMetadata: new FrameworkPropertyMetadata(
                    defaultValue: "20",
                    propertyChangedCallback: (sender, e) =>
                        ((DependencyPropertyPage)sender).OnAgePropertyChanged(e.OldValue, e.NewValue),
                    coerceValueCallback: (sender, e) =>
                        ((DependencyPropertyPage)sender).CoerceAge(e)),
                validateValueCallback: new ValidateValueCallback(IsValidAge));

        public string Age
        {
            get => (string)GetValue(AgeProperty);
            set => SetValue(AgeProperty, value);
        }

        // 検証コールバックによって false が返されると、値が設定されない
        private static bool IsValidAge(object value)
        {
            var isValid = false;

            // 年齢が負数はあり得ない(値の補正はCoerceで行う)
            if (value is string s)
            {
                if (int.TryParse(s, out var age))
                {
                    isValid = (age >= 0);
                }
            }
            return isValid;
        }

        private void OnAgePropertyChanged(object oldValue, object newValue)
        {
            if (!int.TryParse(oldValue.ToString(), out var oldAge)) return;
            if (!int.TryParse(newValue.ToString(), out var newAge)) return;
            this.AppendLog($"propertyChanged: {oldAge} -> {newAge}");
        }

        // 値を補正できる ※Coerce=(人に)強制して(力ずくで)～させる
        private object CoerceAge(object value)
        {
            if (int.TryParse(value.ToString(), out var oldAge))
            {
                var newAge = Math.Max(MinAge, Math.Min(MaxAge, oldAge));

                this.AppendLog($"coerceValue: {oldAge} -> {newAge}");
                return newAge.ToString();
            }
            return value;
        }

        private readonly StringBuilder _stringBuilder = new StringBuilder();

        private void AppendLog(string message)
        {
            _stringBuilder.AppendLine(message);
            DataContext = _stringBuilder.ToString();
        }

        //private void ClearLog()
        //{
        //    _stringBuilder.Clear();
        //    DataContext = _stringBuilder.ToString();
        //}

    }
}