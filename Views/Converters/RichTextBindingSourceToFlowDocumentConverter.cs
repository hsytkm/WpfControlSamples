using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfControlSamples.Views.Converters
{
    // WPFのRichTextBoxにBindingする https://qiita.com/soi/items/ac970626af21aa5a362f
    class RichTextBindingSource
    {
        public string Text { get; set; }
        public Color Color { get; set; }
        public double FontSize { get; set; } = 12;
    }

    class RichTextBindingSourceToFlowDocumentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is RichTextBindingSource bindingSource))
                return Binding.DoNothing;

            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run("RichTextBox3 : "));
            paragraph.Inlines.Add(new Run()
            {
                Text = bindingSource.Text,
                Foreground = new SolidColorBrush(bindingSource.Color),
                FontSize = bindingSource.FontSize,
                //FontWeight = FontWeights.UltraBold,
            });
            return new FlowDocument(paragraph);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
