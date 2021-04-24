#nullable disable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
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
using WpfControlSamples.Models;

namespace WpfControlSamples.Views.Menus
{
    public partial class SystemTopPage : ContentControl
    {
        public SystemTopPage()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            /* Example using Hyperlink in WPF
             * https://stackoverflow.com/questions/10238694/example-using-hyperlink-in-wpf
             */
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    }

    class SystemTopViewModel : MyBindableBase
    {
        public string AppName { get; } = App.Current.SampleModelContext.AppName;
    }
}
