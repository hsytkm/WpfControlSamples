#nullable disable
using System;
using System.Collections.Generic;
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
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class WatermarkTextBox : UserControl
    {
        // ウォーターマーク文字列
        public static readonly DependencyProperty WatermarkTextProperty =
            DependencyProperty.Register(nameof(WatermarkText), typeof(string), typeof(WatermarkTextBox));
        public string WatermarkText
        {
            get => (string)GetValue(WatermarkTextProperty);
            set => SetValue(WatermarkTextProperty, value);
        }

        // 入力文字列
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(WatermarkTextBox));
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public WatermarkTextBox()
        {
            InitializeComponent();
        }
    }
}
