#nullable disable
using System;
using System.Windows.Markup;

namespace WpfControlSamples.Views.MarkupExtensions
{
    // https://www.youtube.com/watch?v=Bp5LFXjwtQ0
    [MarkupExtensionReturnType(typeof(Array))]
    class EnumBindingSourceExtension : MarkupExtension
    {
        public Type EnumType { get; }

        public EnumBindingSourceExtension(Type type)
        {
            if (type is null || !type.IsEnum)
                throw new ArgumentException("type is must not be null and of type Enum");

            EnumType = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(EnumType);
        }
    }
}
