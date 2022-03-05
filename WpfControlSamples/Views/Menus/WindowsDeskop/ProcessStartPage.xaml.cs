using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class ProcessStartPage : ContentControl
    {
        public ProcessStartPage()
        {
            InitializeComponent();
        }

        private void HomeDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            var exeBodyPath = Environment.GetCommandLineArgs()[0];
            var homeDir = Path.GetDirectoryName(exeBodyPath);

            if (!string.IsNullOrEmpty(homeDir))
            {
                using var process = System.Diagnostics.Process.Start("explorer.exe", homeDir);
            }
        }
    }
}
