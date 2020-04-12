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
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    /// <summary>
    /// ProcessingPage.xaml の相互作用ロジック
    /// </summary>
    public partial class ButtonPage : ContentControl
    {
        public ICommand ButtonClick => _buttonClick ??
            (_buttonClick = new MyCommand<string>(t => MessageBox.Show(t + " is pushed!", "PushPush")));
        private ICommand _buttonClick;

        public ButtonPage()
        {
            InitializeComponent();

            DataContext = ButtonClick;
        }
    }
}