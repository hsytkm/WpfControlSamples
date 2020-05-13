using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WpfControlSamples.Views.Menus
{
    public partial class DispatcherUnhandledExceptionPage : ContentControl
    {
        public DispatcherUnhandledExceptionPage()
        {
            InitializeComponent();
        }

        private void Button_Click_0(object sender, RoutedEventArgs e)
            => App.Current.RemoveEvent_DispatcherUnhandledException();

        private void Button_Click_1(object sender, RoutedEventArgs e)
            => throw new NotImplementedException("未実装なので例外出しまーす1");

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                throw new NotImplementedException("未実装なので例外出しまーす2");
            }
            catch (NotImplementedException) { }
        }

    }
}