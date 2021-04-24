#nullable disable
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace WpfControlSamples.Views.MarkupExtensions
{
    // WPFサンプル:DrawingBrushで枠付きテキストを表示する
    //   http://gushwell.ldblog.jp/archives/52312432.html
    // これよりも OutlinedTextPathExtension の方が良さげ。どっちも使ったことないけど…
    [MarkupExtensionReturnType(typeof(Brush))]
    class OutlinedTextBrushExtension : MarkupExtension
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

            var textGeometry = formattedText.BuildGeometry(new Point(0, 0));

            var brush = new LinearGradientBrush(
                startColor: Colors.YellowGreen, endColor: Colors.SkyBlue,
                startPoint: new Point(0.2, 0), endPoint: new Point(0.8, 1)
            );

            var geometryDrawing = new GeometryDrawing(brush: null, new Pen(brush, thickness: 2.0), textGeometry);

            return new DrawingBrush(geometryDrawing);
        }
    }
}
