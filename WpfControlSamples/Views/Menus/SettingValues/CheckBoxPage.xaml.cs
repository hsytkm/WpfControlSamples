﻿using System;
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
    public partial class CheckBoxPage : ContentControl
    {
        public CheckBoxPage()
        {
            InitializeComponent();

            DataContext = new CheckBoxViewModel();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e) { }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e) { }
    }

    class CheckBoxViewModel : MyBindableBase
    {
        public bool CheckFlag
        {
            get => _checkFlag;
            set => SetProperty(ref _checkFlag, value);
        }
        private bool _checkFlag = true;

        public bool? ThreeStateFlag
        {
            get => _threeStateFlag;
            set
            {
                if (SetProperty(ref _threeStateFlag, value))
                {
                    ThreeStateText = value.HasValue ? value.Value.ToString() : "null";
                }
            }
        }
        private bool? _threeStateFlag = null;

        public string ThreeStateText
        {
            get => _threeStateText;
            set => SetProperty(ref _threeStateText, value);
        }
        private string _threeStateText;


    }
}
