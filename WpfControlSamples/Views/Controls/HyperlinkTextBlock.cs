#nullable disable
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

// <TextBlock>
//     <Hyperlink NavigateUri="https://www.google.co.jp/"
//                RequestNavigate="Hyperlink_RequestNavigate" >
//         <Hyperlink.Inlines>
//             <TextBlock Text="https://www.google.co.jp/" />
//         </Hyperlink.Inlines>
//     </Hyperlink>
// </TextBlock>
namespace WpfControlSamples.Views.Controls
{
    /// <summary>
    /// TextBlock内にHyperlinkを持つコントロール
    ///   <ctrl:HyperlinkTextBlock Text="Google" NavigateUri="https://www.google.co.jp/" />
    /// </summary>
    class HyperlinkTextBlock : TextBlock
    {
        public static new readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(HyperlinkTextBlock));

        public new string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty NavigateUriProperty =
            DependencyProperty.Register(
                nameof(NavigateUri), typeof(Uri), typeof(HyperlinkTextBlock),
                new FrameworkPropertyMetadata(default(Uri),
                    (sender, e) => ((HyperlinkTextBlock)sender).OnNavigateUriPropertyChanged(e.OldValue, e.NewValue)));

        public Uri NavigateUri
        {
            get => (Uri)GetValue(NavigateUriProperty);
            set => SetValue(NavigateUriProperty, value);
        }

        private void OnNavigateUriPropertyChanged(object oldValue, object newValue)
        {
            if (newValue is null || newValue is not Uri uri)
            {
                foreach (var inline in Inlines)
                {
                    RemoveMyHyperlinkEvent(inline);
                }
                Inlines.Clear();
                return;
            }

            var hyperlink = CreateMyHyperlink(uri);
            Inlines.Clear();
            Inlines.Add(hyperlink);
        }
        private const string MyHyperLinkName = nameof(MyHyperLinkName);

        /// <summary>
        /// 自作Hyperlinkの作成
        /// </summary>
        /// <param name="uri"></param>
        private Hyperlink CreateMyHyperlink(Uri uri)
        {
            if (uri is null) throw new ArgumentNullException();

            var text = !string.IsNullOrEmpty(Text) ? Text : uri.AbsoluteUri;

            var hyperlink = new Hyperlink() { NavigateUri = uri };
            var textBlock = new TextBlock() { Text = text };
            hyperlink.Inlines.Add(textBlock);

            // 自作Hyperlinkのイベント追加
            hyperlink.SetValue(FrameworkElement.NameProperty, MyHyperLinkName);
            hyperlink.RequestNavigate += Hyperlink_RequestNavigate;

            return hyperlink;
        }

        /// <summary>
        /// 自作Hyperlinkのイベント削除
        /// </summary>
        /// <param name="inline"></param>
        private static void RemoveMyHyperlinkEvent(Inline inline)
        {
            if (inline is null) throw new ArgumentNullException();
            if (!(inline is Hyperlink hyperlink)) return;

            var nameProp = hyperlink.GetValue(FrameworkElement.NameProperty);
            if (nameProp is string name && name == MyHyperLinkName)
            {
                hyperlink.RequestNavigate -= Hyperlink_RequestNavigate;
            }
        }

        private static void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e) =>
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
    }
}
