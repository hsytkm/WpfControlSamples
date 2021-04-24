#nullable disable
using System;
using System.Globalization;
using System.Windows.Markup;

namespace WpfControlSamples.Views.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(int))]
    class ToIntExtension : MarkupExtension
    {
        public string Value { set; get; }

        public override object ProvideValue(IServiceProvider serviceProvider)
            => int.Parse(Value, CultureInfo.InvariantCulture);
    }
}
