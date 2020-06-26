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
using WpfControlSamples.Views.Controls;

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

        public ICommand FolderOpenDialogCommand =>
            _folderOpenDialogCommand ??= new MyCommand(() => CallFolderBrowserDialog(this));
        private ICommand _folderOpenDialogCommand;

        private void PInvokeFolderOpenSingleButton_Click(object sender, RoutedEventArgs e) =>
            CallFolderBrowserDialog(sender);

        private void CallFolderBrowserDialog(object sender)
        {
            var result = FolderBrowserDialog.Result.None;
            var browser = new FolderBrowserDialog("P/Invoke フォルダを選択してください");

            // ウィンドウが取得できるときは設定する
            if (sender is DependencyObject dep)
            {
                var window = Window.GetWindow(dep);
                if (window != null) result = browser.ShowDialog(window);
            }
            else
            {
                result = browser.ShowDialog();
            }

            if (result == FolderBrowserDialog.Result.OK)
            {
                // CommandParameter に IReactiveProperty<string> を指定して、VMに渡したりとか。
                DataContext = browser.SelectedPath ?? "";
            }
        }

    }
}