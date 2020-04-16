using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
    /* Xamarin.Formsのようにジェネリックのクラスにしたかったけど、
     * ジェネリックの属性ができないっぽいので断念して object型 にした（ちゃんと調べてない）
     * 
     *  Creating Generic IValueConverter to convert data types to string
     *  https://stackoverflow.com/questions/14110335/creating-generic-ivalueconverter-to-convert-data-types-to-string
     *  
     */

    [ValueConversion(typeof(bool), typeof(object))]
    class BooleanToObjectConverter : IValueConverter
    {
        public object TrueObject { set; get; }

        public object FalseObject { set; get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (value is bool b && b) ? TrueObject : FalseObject;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            (value).Equals(TrueObject);
    }
}
