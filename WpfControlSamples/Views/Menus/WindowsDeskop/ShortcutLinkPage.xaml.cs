using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
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
using WpfControlSamples.ViewModels;

namespace WpfControlSamples.Views.Menus
{
    public partial class ShortcutLinkPage : ContentControl
    {
        public ShortcutLinkPage()
        {
            DataContext = new ShortcutLinkViewModel();
            InitializeComponent();
        }
    }

    class ShortcutLinkViewModel : MyBindableBase
    {
        public string SelectedPath
        {
            get => _selectedPath;
            set
            {
                if (SetProperty(ref _selectedPath, value))
                {
                    var shortcut = ShortcutRecord.Create(_selectedPath);
                    FullPath = shortcut?.TargetPath ?? "";
                }
            }
        }
        private string _selectedPath = "";

        public string FullPath
        {
            get => _fullPath;
            private set => SetProperty(ref _fullPath, value);
        }
        private string _fullPath = "";

    }
}
