using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using WpfControlSamples.Infrastructures;
using WpfControlSamples.Views.Controls;

namespace WpfControlSamples.Views.Menus
{
    public partial class BulletinTextPage : ContentControl
    {
        public BulletinTextPage()
        {
            DataContext = new BulletinTextViewModel();
            InitializeComponent();
        }
    }

    class BulletinTextViewModel : MyBindableBase
    {
        public ObservableTextQueue TextLines { get; } = new();

        public string InputText
        {
            get => _inputText;
            set => SetProperty(ref _inputText, value);
        }
        private string _inputText = "Message";

        public ICommand AddTextCommand => _addTextCommand ??= new MyCommand(() =>
        {
            TextLines.Enqueue(InputText);   // 解説表示用
            NotifyText = InputText;         // 専用UserControl用
            InputText = "";                 // Viewをクリア
        });
        private ICommand _addTextCommand = default!;

        public string NotifyText
        {
            get => _notifyText;
            set => SetProperty(ref _notifyText, value);
        }
        private string _notifyText = "";
    }
}
