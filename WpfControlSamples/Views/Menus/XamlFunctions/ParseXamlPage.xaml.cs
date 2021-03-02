using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class ParseXamlPage : ContentControl
    {
        public ParseXamlPage()
        {
            InitializeComponent();
        }
    }

    class ParseXamlViewModel : MyBindableBase
    {
        public FrameworkElement ControlFromXamlText
        {
            get => _controlFromXamlText;
            private set => SetProperty(ref _controlFromXamlText, value);
        }
        private FrameworkElement _controlFromXamlText;

        public ICommand ParseCommand => _parseCommand ??= new MyCommand<string>(
            xaml =>
            {
                using var sr = new MemoryStream(Encoding.ASCII.GetBytes(xaml));

                var pc = new ParserContext();
                pc.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                pc.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");

                try
                {
                    if (XamlReader.Load(sr, pc) is FrameworkElement fe)
                        ControlFromXamlText = fe;
                }
                catch (XamlParseException)
                {
                    ControlFromXamlText = null;
                }
            });
        private ICommand _parseCommand;

    }
}
