using Microsoft.Win32;
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

namespace WpfControlSamples.Views.Menus
{
    public partial class MediaElementPage : ContentControl
    {
        public MediaElementPage()
        {
            InitializeComponent();
        }
    }

    class MediaElementViewModel : MyBindableBase
    {
        public string MovieFilePath
        {
            get => _movieFilePath;
            set => SetProperty(ref _movieFilePath, value);
        }
        private string _movieFilePath;

    }
}