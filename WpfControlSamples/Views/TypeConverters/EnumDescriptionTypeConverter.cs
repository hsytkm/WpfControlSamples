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
                var s = value.ToString();
                if (s is null) return null;
                return (int)Enum.Parse(e.GetType(), s);
            }

            if (destinationType == typeof(string))
            {
                if (value is not null)
                {
                    var valueString = value.ToString() ?? throw new NullReferenceException();
                    var fieldInfo = value.GetType().GetField(valueString);
                    var attributes = fieldInfo?.GetCustomAttributes<DescriptionAttribute>(false);
                    var attribute = attributes?.FirstOrDefault();
                    var desctiption = attribute is not null ? attribute.Description : valueString;

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
