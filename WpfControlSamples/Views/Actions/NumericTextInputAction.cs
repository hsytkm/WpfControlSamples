using Microsoft.Xaml.Behaviors;
using System;
using System.Linq;
using System.Windows.Controls;

namespace WpfControlSamples.Views.Actions
{
    // 数値のみ入力 → コピペ で数値以外を入力できちゃうので没
    //public class NumericTextInputAction : TriggerAction<TextBox>
    //{
    //    private readonly Regex _regex = new("[0-9]");

    //    protected override void Invoke(object parameter)
    //    {
    //        if (parameter is not TextCompositionEventArgs e) return;
    //        e.Handled = !_regex.IsMatch(e.Text);
    //    }
    //}

    // 数値のみ入力のコピペ対応版
    public class NumericTextInputAction : TriggerAction<TextBox>
    {
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject is not TextBox textBox) return;
            if (parameter is not TextChangedEventArgs e) return;
            //if (e.Source is not TextBox textBox1) return;
            //if (textBox1.Text is not string source) return;
            if (textBox.Text is not string source) return;

            Span<char> cs = stackalloc char[source.Length];
            var n = 0;
            foreach (var c in source)
            {
                if ('0' <= c && c <= '9')
                    cs[n++] = c;
            }
            textBox.Text = (n == 0) ? "" : new string(cs.Slice(0, n));
        }
    }
}
