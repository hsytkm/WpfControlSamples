using System;
using System.Reflection;
using System.Windows.Markup;
using System.Windows.Media;

namespace WpfControlSamples.Views.MarkupExtensions
{
    //[ContentProperty(nameof(Source))]
    class EmbeddedImageResourceExtension : MarkupExtension
    {
        public string Source { set; get; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // WPFでは Imageコントロール に 埋め込みリソース画像をそのまま突っ込めるので実装していません。
            throw new NotImplementedException();

            //if (string.IsNullOrEmpty(Source))
            //{
            //    var lineInfo = (serviceProvider.GetService(typeof(IXmlLineInfoProvider)) is IXmlLineInfoProvider lineInfoProvider)
            //        ? lineInfoProvider.XmlLineInfo : new XmlLineInfo();

            //    throw new XamlParseException($"{nameof(EmbeddedImageResourceExtension)} requires {nameof(Source)} property to be set", lineInfo);
            //}

            //var assemblyName = this.GetType().GetTypeInfo().Assembly.GetName().Name;
            //return ImageSource.FromResource(assemblyName + "." + Source, typeof(EmbeddedImageResourceExtension).GetTypeInfo().Assembly);
        }

    }
}
