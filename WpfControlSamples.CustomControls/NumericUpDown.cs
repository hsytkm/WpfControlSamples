using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WpfControlSamples.CustomControls
{
    #region
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfControlSamples.CustomControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfControlSamples.CustomControls;assembly=WpfControlSamples.CustomControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:NumericUpDown/>
    ///
    /// </summary>
    #endregion

    // WPF4.5入門 その54 「カスタムコントロール」 https://blog.okazuki.jp/entry/2014/09/08/221209
    public class NumericUpDown : Control
    {
        static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value), typeof(int), typeof(NumericUpDown),
                new PropertyMetadata(0, ValueChanged));

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((NumericUpDown)d).UpdateState(true);
        }

        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private void UpdateState(bool useTransition)
        {
            var stateName = (Value >= 0) ? "Positive" : "Negative";
            VisualStateManager.GoToState(this, stateName, useTransition);
        }

        // XAMLで定義されたボタン格納用変数
        private RepeatButton? upButton;
        private RepeatButton? downButton;

        // ボタンのクリックイベント
        private void UpClick(object sender, RoutedEventArgs e) => Value++;
        private void DownClick(object sender, RoutedEventArgs e) => Value--;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // 前のテンプレートのコントロールの後処理
            if (upButton != null) upButton.Click -= UpClick;
            if (downButton != null) downButton.Click -= DownClick;

            // テンプレートからコントロールの取得
            upButton = GetTemplateChild("PART_UpButton") as RepeatButton;
            downButton = GetTemplateChild("PART_DownButton") as RepeatButton;

            // イベントハンドラの登録
            if (upButton != null) upButton.Click += UpClick;
            if (downButton != null) downButton.Click += DownClick;

            // VisualStateManagerの更新
            UpdateState(false);
        }
    }
}
