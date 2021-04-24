#nullable disable
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

        private const int AgeMin = 0;

        #region 依存関係プロパティ (Age)
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
                var newAge = Math.Max(AgeMin, Math.Min(AgeMax, oldAge));

                this.AppendLog($"coerceValue: {oldAge} -> {newAge}");
                return newAge.ToString();
            }
            return value;
        }
        #endregion

        #region 読み取り専用の依存関係プロパティ (AgeMax)
        // WPF4.5入門 その43 「読み取り専用の依存関係プロパティ」https://blog.okazuki.jp/entry/2014/08/18/083455

        // RegisterReadOnlyメソッドでDependencyPropertyKeyを取得
        private static readonly DependencyPropertyKey AgeMaxPropertyKey =
            DependencyProperty.RegisterReadOnly(
                name: nameof(AgeMax),
                propertyType: typeof(int),
                ownerType: typeof(DependencyPropertyPage),
                typeMetadata: new PropertyMetadata(defaultValue: 128),
                validateValueCallback: new ValidateValueCallback(IsValidAgeMax));

        // DependencyPropertyは、DependencyPropertyKeyから取得する
        public static readonly DependencyProperty AgeMaxProperty = AgeMaxPropertyKey.DependencyProperty;

        // getは普段通りで、setはDependencyPropertyKeyを使って行う
        public int AgeMax
        {
            get => (int)GetValue(AgeMaxProperty);
            private set => SetValue(AgeMaxPropertyKey, value);
        }

        private static bool IsValidAgeMax(object value)
        {
            // ◆読み取り専用の値検証って何なんやろ？なぞい
            return true;
        }

        #endregion

        private readonly StringBuilder _stringBuilder = new();

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
