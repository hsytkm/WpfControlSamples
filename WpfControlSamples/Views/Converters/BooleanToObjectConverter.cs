using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfControlSamples.Views.Converters
{
#if true
    // GenericValueConverter で書き換え（やりたかったことが実装できた）
    [ValueConversion(typeof(bool), typeof(object))]
    class BooleanToObjectConverter : GenericValueConverter<bool, object>
    {
        public object TrueObject { set; get; }

        public object FalseObject { set; get; }

        public override object Convert(bool b, object parameter, CultureInfo culture) =>
            b ? TrueObject : FalseObject;

        public override bool ConvertBack(object value, object parameter, CultureInfo culture) =>
            (value).Equals(TrueObject);
    }

#else
    /* Xamarin.Formsのようにジェネリックのクラスにしたかったけど、
     * ジェネリックの属性ができないっぽいので断念して object型 にした（ちゃんと調べてない）
     * 
     *  Creating Generic IValueConverter to convert data types to string
     *  https://stackoverflow.com/questions/14110335/creating-generic-ivalueconverter-to-convert-data-types-to-string
     *  
     */
    [ValueConversion(typeof(bool), typeof(object))]
    class BooleanToObjectConverter_Old : IValueConverter
    {
        public object TrueObject { set; get; }

        public object FalseObject { set; get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (value is bool b && b) ? TrueObject : FalseObject;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            (value).Equals(TrueObject);
    }
#endif
}
