﻿#nullable disable
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
    public partial class TextBox1Page : ContentControl
    {
        public TextBox1Page()
        {
            InitializeComponent();

            DataContext = new TextBox1ViewModel();
        }
    }

    class TextBox1ViewModel : MyBindableBase
    {
        public string InputText1
        {
            get => _inputText1;
            set => SetProperty(ref _inputText1, value);
        }
        private string _inputText1 = "abc";

        public int InputInt1
        {
            get => _inputInt1;
            set => SetProperty(ref _inputInt1, value);
        }
        private int _inputInt1 = 123;

        public string InputText2
        {
            get => _inputText2;
            private set => SetProperty(ref _inputText2, value);
        }
        private string _inputText2;

        public string InputText3
        {
            get => _inputText3;
            set => SetProperty(ref _inputText3, value);
        }
        private string _inputText3;

        public ICommand TextEnterCommand => _textEnterCommand ??=
            new MyCommand<string>(text => InputText2 = text.ToUpperInvariant());
        private ICommand _textEnterCommand;

    }
}
