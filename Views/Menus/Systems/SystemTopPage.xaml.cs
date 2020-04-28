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
    }

    class SystemTopViewModel : MyBindableBase
    {
        public string AppName { get; } = App.Current.SampleModelContext.AppName;
    }
}