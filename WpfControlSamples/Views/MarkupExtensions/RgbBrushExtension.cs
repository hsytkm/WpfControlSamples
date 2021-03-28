using System;
using System.Windows.Markup;
using System.Windows.Media;
using WpfControlSamples.Extensions;

namespace WpfControlSamples.Views.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(Brush))]
    class RgbBrushExtension : MarkupExtension
    {
        public byte R { set; get; }

        public byte G { set; get; }

        public byte B { set; get; }

        public byte A { set; get; } = byte.MaxValue;

        public override object ProvideValue(IServiceProvider serviceProvider) =>
            new SolidColorBrush(Color.FromArgb(A, R, G, B)).WithFreeze();
    }
}
