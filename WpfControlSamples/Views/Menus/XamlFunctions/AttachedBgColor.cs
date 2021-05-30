#nullable disable
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfControlSamples.Extensions;

namespace WpfControlSamples.Views.Menus
{
    static class AttachedBgColor
    {
        public static readonly DependencyProperty BgColorProperty =
            DependencyProperty.RegisterAttached(
                name: "BgColor",
                propertyType: typeof(string),
                ownerType: typeof(AttachedBgColor),
                // 以降は省略可
                defaultMetadata: new FrameworkPropertyMetadata(
                    defaultValue: "",
                    propertyChangedCallback: (sender, e) => OnBgColorPropertyChanged(sender, e.OldValue, e.NewValue),
                    coerceValueCallback: (sender, e) => CoerceBgColor(e)),
                validateValueCallback: new ValidateValueCallback(IsValidBgColor));

        public static string GetBgColor(UIElement element) =>
            (string)element.GetValue(BgColorProperty);

        public static void SetBgColor(UIElement element, string value) =>
            element.SetValue(BgColorProperty, value);

        // 検証コールバックによって false が返されると、値が設定されない
        private static bool IsValidBgColor(object value)
        {
            var isValid = (value is string);
            //Debug.WriteLine($"{nameof(AttachedBgColor)}_validateValue: {isValid}");
            return isValid;
        }

        private static void OnBgColorPropertyChanged(DependencyObject sender, object oldValue, object newValue)
        {
            if (sender is Control control && newValue is string name)
            {
                var color = GetExistColorKeyValue(name).Color;
                control.Background = color.ToSolidColorBrush();
            }
            //Debug.WriteLine($"{nameof(AttachedBgColor)}_propertyChanged: {oldValue} -> {newValue}");
        }

        // 値を補正できる ※Coerce=(人に)強制して(力ずくで)～させる
        private static object CoerceBgColor(object value)
        {
            var color = (value is string name)
                ? GetExistColorKeyValue(name) : GetDefaultBgColorKeyValue();

            //Debug.WriteLine($"{nameof(AttachedBgColor)}_coerceValue: {value} -> {color}");
            return color.Name;
        }

        private static (string Name, Color Color) GetDefaultBgColorKeyValue() =>
            (nameof(Colors.Transparent), Colors.Transparent);

        // 引数の名前が存在したらColorを返す(存在しなければデフォ色)
        private static (string Name, Color Color) GetExistColorKeyValue(string name)
        {
            var n = name.ToLower();
            var key = Models.SampleData.WpfColors
                .FirstOrDefault(x => x.Name.ToLower() == n);

            return (key != default) ? key : GetDefaultBgColorKeyValue();
        }

    }
}
