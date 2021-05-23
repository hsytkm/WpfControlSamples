using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;

// Adding right-aligned row numbers to a DataGridRowHeader in WPF | Magnus Montin
// https://blog.magnusmontin.net/2014/08/18/right-aligned-row-numbers-datagridrowheader-wpf/
namespace WpfControlSamples.Views.Actions
{
    // DataGrid が仮想化されていても意図通りに Index を付与できます。
    // DataGridRowIndexConverter は仮想化に対応していませんので、こちらを使いましょう。
    public class DataGridRowIndexAction : TriggerAction<DataGrid>
    {
        public static readonly DependencyProperty StringFormatProperty =
            DependencyProperty.Register(nameof(StringFormat), typeof(string), typeof(DataGridRowIndexAction));

        public string StringFormat
        {
            get => (string)GetValue(StringFormatProperty);
            set => SetValue(StringFormatProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            if (parameter is not DataGridRowEventArgs args) return;

            var index = args.Row.GetIndex();  // index start zero.

            args.Row.Header = string.IsNullOrEmpty(StringFormat)
                ? index.ToString()
                : string.Format(StringFormat, index);
        }
    }
}
