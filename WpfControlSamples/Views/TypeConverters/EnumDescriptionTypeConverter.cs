#nullable disable
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace WpfControlSamples.Views.TypeConverters
{
    // How to Localize Enums in C#  https://www.youtube.com/watch?v=T1mhORJCDsY
    class EnumDescriptionTypeConverter : EnumConverter
    {
        public EnumDescriptionTypeConverter(Type type) : base(type) { }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            static int? GetEnumIndex(object value)
            {
                if (value is not Enum e) return null;
                return (int)Enum.Parse(e.GetType(), value.ToString());
            }

            if (destinationType == typeof(string))
            {
                if (value != null)
                {
                    var fieldInfo = value.GetType().GetField(value.ToString());
                    var attributes = fieldInfo?.GetCustomAttributes<DescriptionAttribute>(false);
                    var attribute = attributes?.FirstOrDefault();

                    var desctiption = attribute is null ? value.ToString() : attribute.Description;

                    // Add enum index
                    var index = GetEnumIndex(value);
                    if (index.HasValue) desctiption = $"{index.Value}: {desctiption}";

                    return desctiption;
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
