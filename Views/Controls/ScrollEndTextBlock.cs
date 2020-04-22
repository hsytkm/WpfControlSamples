using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfControlSamples.Views.Controls
{
    /// <summary>
    /// Textプロパティの変更時に最終行へスクロールするTextBlock
    /// </summary>
    class ScrollEndTextBlock : ScrollViewer
    {
        /* 必要ないので追加していないが、TextBlock にあるプロパティを作れば、汎用性が上がりそう。
         *   FontSize / FontWeight / FontStyle / TextDecorations
         */
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text), typeof(string), typeof(ScrollEndTextBlock),
                typeMetadata: new FrameworkPropertyMetadata(default(string),
                    (sender, e) => ((ScrollEndTextBlock)sender).OnTextPropertyChanged(e.OldValue, e.NewValue)));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private void OnTextPropertyChanged(object oldValue, object newValue)
        {
            if (newValue is null)
            {
                Content = null;
                return;
            }

            var newString = (newValue is string) ? (string)newValue : newValue.ToString();

            if (!(Content is TextBlock textBlock))
            {
                var text = new TextBlock
                {
                    Text = newString,
                    Foreground = Foreground,
                };
                Content = text;
            }
            else
            {
                textBlock.Text = newString;
            }

            this.ScrollToEnd();
        }
    }
}
