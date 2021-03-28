using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace WpfControlSamples.Views.MarkupExtensions
{
    // WPFサンプル:Pathオブジェクトを使い、枠付きテキストを表示する
    //   http://gushwell.ldblog.jp/archives/52313757.html
    [MarkupExtensionReturnType(typeof(Geometry))]
    class OutlinedTextPathExtension : MarkupExtension
    {
        public string Text { set; get; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var typeface = new Typeface(
                new FontFamily("Segoe UI"),
                FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

            var formattedText = new FormattedText(
                Text,
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface,
                emSize: 64, foreground: new SolidColorBrush(), pixelsPerDip: 1.25);

            return formattedText.BuildGeometry(new Point(0, 0));
        }
    }
}
