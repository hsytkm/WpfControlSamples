using System;
using System.Windows;
using System.Windows.Markup;

namespace WpfControlSamples.Views.MarkupExtensions
{
    // How to Append text to static resource in Xamarin.Forms?
    // https://stackoverflow.com/questions/47350470/how-to-append-text-to-static-resource-in-xamarin-forms
    //[ContentProperty(nameof(StaticResourceKey))]
    class StringFormatExtension : MarkupExtension
    {
        public string StringFormat { get; set; }
        public string StaticResourceKey { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider is null)
                throw new ArgumentNullException(nameof(serviceProvider));

            string toReturn = null;
            if (StaticResourceKey != null)
            {
                var staticResourceExtension = new StaticResourceExtension { ResourceKey = StaticResourceKey };
                var value = staticResourceExtension.ProvideValue(serviceProvider);

                toReturn = (value is string s) ? s : value?.ToString();

                if (!string.IsNullOrEmpty(StringFormat))
                    toReturn = string.Format(StringFormat, toReturn);
            }
            return toReturn ?? "";
        }

    }
}