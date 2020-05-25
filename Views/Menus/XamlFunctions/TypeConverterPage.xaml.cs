using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class TypeConverterPage : ContentControl
    {
        public TypeConverterPage()
        {
            InitializeComponent();
        }
    }

    [TypeConverter(typeof(CharactorProfileTypeConverter))]
    class CharactorProfile
    {
        public string FirstName { get; }
        public string LastName { get; }

        public CharactorProfile() => (FirstName, LastName) = ("No", "Name");
        public CharactorProfile(string fn, string ln) => (FirstName, LastName) = (fn, ln);

        public override string ToString() => FirstName + " " + LastName;

        public static CharactorProfile Giogio { get; } = new CharactorProfile("Giorno", "Giovana");
    }

    class CharactorProfileTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (!(value is string s))
                return base.ConvertFrom(context, culture, value);

            return s.ToLower() switch
            {
                "jojo1" => new CharactorProfile("Jonathan", "Jostar"),
                "jojo2" => new CharactorProfile("Joseph", "Jostar"),
                "jojo3" => new CharactorProfile("Jotaro", "Kujo"),
                _ => new CharactorProfile()
            };
        }
    }

    class CharactorProfileCapsule
    {
        public CharactorProfile Profile { get; set; }
    }

    class StandUserProfile : CharactorProfile
    {
    }
}