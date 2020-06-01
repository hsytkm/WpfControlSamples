using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
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

namespace WpfControlSamples.Views.Menus
{
    public partial class FolderDialogPage : ContentControl
    {
        public FolderDialogPage()
        {
            InitializeComponent();
        }

        private void FolderOpenSingleButton_Click(object sender, RoutedEventArgs e)
        {
            var path = FileDialogLibrary.FolderOpener.OpenSingle();

            if (path is null) path = "null";
            if (string.IsNullOrEmpty(path)) path = "empty";

            DataContext = path;
        }

        private void FolderOpenMultiButton_Click(object sender, RoutedEventArgs e)
        {
            var dialogTitle = "複数のフォルダが選択できますよ";
            var paths = FileDialogLibrary.FolderOpener.OpenMulti(dialogTitle);

            string text;
            if (paths is null)
            {
                text = "null";
            }
            else if (!paths.Any())
            {
                text = "zero";
            }
            else
            {
                text = string.Join(Environment.NewLine, paths);
            }
            DataContext = text;
        }
    }
}