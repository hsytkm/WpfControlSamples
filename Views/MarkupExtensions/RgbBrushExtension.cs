using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace WpfControlSamples.Views.MarkupExtensions
{
    class RgbBrushExtension : MarkupExtension
    {
        public byte R { set; get; }

        public byte G { set; get; }

        public byte B { set; get; }

        public byte A { set; get; } = byte.MaxValue;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var color = new SolidColorBrush(Color.FromArgb(A, R, G, B));
            color.Freeze();
            return color;
        }

    }
}
