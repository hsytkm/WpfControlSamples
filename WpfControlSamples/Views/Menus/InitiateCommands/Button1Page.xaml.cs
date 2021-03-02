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
    public partial class Button1Page : ContentControl
    {
        public ICommand ButtonClick =>
            _buttonClick ??= new MyCommand<string>(t => MessageBox.Show(t + " is pushed!", "PushPush"));
        private ICommand _buttonClick;

        public Button1Page()
        {
            InitializeComponent();

            DataContext = ButtonClick;
        }

        private int _counter;
        private void IncrementButton_Click(object sender, RoutedEventArgs e)
            => numberTextBlock.Text = (++_counter).ToString();

        private void DecrementButton_Click(object sender, RoutedEventArgs e)
            => numberTextBlock.Text = (--_counter).ToString();

    }
}
