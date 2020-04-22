using System;
using System.Collections.Generic;
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
using WpfControlSamples.Views.Converters;

namespace WpfControlSamples.Views.Menus
{
    public partial class RichTextBoxPage : ContentControl
    {
        public RichTextBoxPage()
        {
            InitializeComponent();
        }
    }

    class RichTextBoxViewModel : MyBindableBase
    {
        #region Binding1
        // RichTextBoxの中身全体をBindingするのではなくて、その一部だけをBindingしてしまう、という方法です
        public string TextSimple
        {
            get => _textSimple;
            set => SetProperty(ref _textSimple, value);
        }
        private string _textSimple = "This is Simple Text.";

        public Color TextColor
        {
            get => _textColor;
            private set => SetProperty(ref _textColor, value);
        }
        private Color _textColor = Colors.Red;
        #endregion

        #region Binding2
        // 柔軟に、RichTextBoxの中身のFlowDocumentをまるごと変更したいということであれば、この方法を使用します
        // ただしViewModelにゴリゴリのView情報（RichTextBox）が入っているのでMVVM的にはイマイチです
        public FlowDocument Document
        {
            get => _document;
            private set => SetProperty(ref _document, value);
        }
        private FlowDocument _document = CreateFlowDoc("FlowDocument in VM");

        private static FlowDocument CreateFlowDoc(string innerText)
        {
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run("RichTextBox2 : "));
            paragraph.Inlines.Add(new Run(innerText) { Foreground = Brushes.BlueViolet });
            return new FlowDocument(paragraph);
        }
        #endregion

        #region Binding3
        // 柔軟に変更したいが、ViewModelにViewの情報を入れたくない/適度に抽象化したい
        public RichTextBindingSource RichTextSource
        {
            get => _richTextSource;
            private set => SetProperty(ref _richTextSource, value);
        }

        private RichTextBindingSource _richTextSource =
            new RichTextBindingSource()
            {
                Text = "Original Text in VM",
                Color = Colors.Blue,
                FontSize = 18,
            };
        #endregion

    }
}