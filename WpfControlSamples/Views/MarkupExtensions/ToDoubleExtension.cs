using System;
using System.Globalization;
using System.Windows.Markup;

namespace WpfControlSamples.Views.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(double))]
    class ToDoubleExtension : MarkupExtension
    {
        public string Value { set; get; }

        public override object ProvideValue(IServiceProvider serviceProvider)
            => double.Parse(Value, CultureInfo.InvariantCulture);
    }
}
