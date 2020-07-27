using System;
using System.Windows;

namespace WpfControlSamples.Views.AttachedProperties
{
    // Xaml の ControlTemplate 内に 値を渡すために使用している。
    // 他にスマートな実装あれば、そちらに置き換えたい…
    static class AttachedBuiltIn
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(AttachedBuiltIn));

        public static string GetText(UIElement element) =>
            (string)element.GetValue(TextProperty);

        public static void SetText(UIElement element, string value) =>
            element.SetValue(TextProperty, value);

    }
}
