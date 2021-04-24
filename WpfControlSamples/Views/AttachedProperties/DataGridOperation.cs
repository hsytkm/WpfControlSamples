#nullable disable
using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfControlSamples.Views.AttachedProperties
{
    // DataGridでAutoGenerateColumnsが有効なままで、フォーマットをカスタムする / Qiita
    //  https://qiita.com/soi/items/d62a4742b37740139736
    class DataGridOperation
    {
        // 型毎に 添付プロパティ を定義する必要がある。shoganai

        public static readonly DependencyProperty DateTimeFormatAutoGenerateProperty =
            DependencyProperty.RegisterAttached("DateTimeFormatAutoGenerate", typeof(string), typeof(DataGridOperation),
                new PropertyMetadata(null, (d, e) => AddEventHandlerOnGenerating<DateTime>(d, e)));
        public static string GetDateTimeFormatAutoGenerate(DependencyObject obj)
            => (string)obj.GetValue(DateTimeFormatAutoGenerateProperty);
        public static void SetDateTimeFormatAutoGenerate(DependencyObject obj, string value)
            => obj.SetValue(DateTimeFormatAutoGenerateProperty, value);

        public static readonly DependencyProperty TimeSpanFormatAutoGenerateProperty =
            DependencyProperty.RegisterAttached("TimeSpanFormatAutoGenerate", typeof(string), typeof(DataGridOperation),
                new PropertyMetadata(null, (d, e) => AddEventHandlerOnGenerating<TimeSpan>(d, e)));
        public static string GetTimeSpanFormatAutoGenerate(DependencyObject obj)
            => (string)obj.GetValue(TimeSpanFormatAutoGenerateProperty);
        public static void SetTimeSpanFormatAutoGenerate(DependencyObject obj, string value)
            => obj.SetValue(TimeSpanFormatAutoGenerateProperty, value);

        private static void AddEventHandlerOnGenerating<T>(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DataGrid dGrid)) return;

            if ((e.NewValue is string format))
                dGrid.AutoGeneratingColumn += (_, e) => AddFormat_OnGenerating<T>(e, format);
        }

        private static void AddFormat_OnGenerating<T>(DataGridAutoGeneratingColumnEventArgs e, string format)
        {
            if (e.PropertyType == typeof(T))
                (e.Column as DataGridTextColumn).Binding.StringFormat = format;
        }
    }
}
