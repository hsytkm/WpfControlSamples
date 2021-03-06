﻿#nullable disable
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
    public partial class FileDialogPage : ContentControl
    {
        public FileDialogPage()
        {
            InitializeComponent();
        }

        private void FileOpenButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "ファイルを開く",
                Filter = "全てのファイル(*.*)|*.*"
            };

            var ret = dialog.ShowDialog();
            openPathTextBlock.Text = (ret.HasValue && ret.Value) ? dialog.FileName : "キャンセルされました";
        }

        private void FileSaveButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = "ファイルを保存",
                Filter = "テキストファイル|*.txt"
            };

            var ret = dialog.ShowDialog();
            savePathTextBlock.Text = (ret.HasValue && ret.Value) ? dialog.FileName : "キャンセルされました";
        }

        public ICommand ShowMessageBoxCommand =>
            _showMessageBoxCommand ??= new MyCommand<string>(message => MessageBox.Show(message, "MyCommand", MessageBoxButton.OK));
        private ICommand _showMessageBoxCommand;
    }

    class FileDialogViewModel : MyBindableBase
    {
        public string SelectedOpenPath
        {
            get => _selectedOpenPath;
            set => SetProperty(ref _selectedOpenPath, value);
        }
        private string _selectedOpenPath;

        public string SelectedSavePath
        {
            get => _selectedSavePath;
            set => SetProperty(ref _selectedSavePath, value);
        }
        private string _selectedSavePath;

    }
}
