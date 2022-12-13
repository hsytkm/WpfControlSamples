#nullable disable
using System.Windows.Controls;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class TextBox1Page : ContentControl
    {
        public TextBox1Page()
        {
            InitializeComponent();

            DataContext = new TextBox1ViewModel();
        }
    }

    class TextBox1ViewModel : MyBindableBase
    {
        public string InputText1
        {
            get => _inputText1;
            set => SetProperty(ref _inputText1, value);
        }
        private string _inputText1 = "abc";

        public int InputInt1
        {
            get => _inputInt1;
            set => SetProperty(ref _inputInt1, value);
        }
        private int _inputInt1 = 123;

        public string InputText2
        {
            get => _inputText2;
            set => SetProperty(ref _inputText2, value);
        }
        private string _inputText2;
    }
}
