using System.Windows.Controls;
using System.Windows.Input;
using WpfControlSamples.Infrastructures;
using static System.Net.Mime.MediaTypeNames;

namespace WpfControlSamples.Views.Menus
{
    public partial class TextBox4Page : UserControl
    {
        public TextBox4Page()
        {
            DataContext = new TextBox4ViewModel();
            InitializeComponent();
        }
    }

    class TextBox4ViewModel : MyBindableBase
    {
        public ICommand TextEnterCommand => _textEnterCommand ??=
            new MyCommand<string?>(text => UpperText1 = text?.ToUpperInvariant() ?? "");
        private ICommand? _textEnterCommand;

        public string UpperText1
        {
            get => _upperText1;
            private set => SetProperty(ref _upperText1, value);
        }
        private string _upperText1 = "";

        public string InputText2
        {
            get => _inputText2;
            set
            {
                if (SetProperty(ref _inputText2, value))
                {
                    UpperText2 = value?.ToUpperInvariant() ?? "";
                }
            }
        }
        private string _inputText2 = "";

        public string UpperText2
        {
            get => _upperText2;
            private set => SetProperty(ref _upperText2, value);
        }
        private string _upperText2 = "";
    }
}
