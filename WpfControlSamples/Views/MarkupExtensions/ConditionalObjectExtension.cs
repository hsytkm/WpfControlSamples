using System;
using System.Windows.Markup;

namespace WpfControlSamples.Views.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(object))]
    class ConditionalObjectExtension : MarkupExtension
    {
        public object? Debug { set; get; }

        public object? Release { set; get; }

        public override object? ProvideValue(IServiceProvider serviceProvider)
        {
#if DEBUG
            return Debug;
#else
            return Release;
#endif
        }
    }
}
