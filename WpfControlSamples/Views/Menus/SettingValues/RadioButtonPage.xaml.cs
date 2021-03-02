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
    public partial class RadioButtonPage : ContentControl
    {
        public RadioButtonPage()
        {
            InitializeComponent();

            DataContext = new RadioButtonViewModel();
        }
    }

    class RadioButtonViewModel : MyBindableBase
    {
        public bool CheckFlag1
        {
            get => _checkFlag1;
            set => SetProperty(ref _checkFlag1, value);
        }
        private bool _checkFlag1;

        public bool CheckFlag2
        {
            get => _checkFlag2;
            set => SetProperty(ref _checkFlag2, value);
        }
        private bool _checkFlag2 = true;

        public bool CheckFlag3
        {
            get => _checkFlag3;
            set => SetProperty(ref _checkFlag3, value);
        }
        private bool _checkFlag3;

        public bool IsEnableRadioGroup
        {
            get => _isEnableRadioGroup;
            set => SetProperty(ref _isEnableRadioGroup, value);
        }
        private bool _isEnableRadioGroup = true;

        public enum SampleType
        {
            TYPE1,
            TYPE2,
            TYPE3
        };

        public SampleType Sample
        {
            get => _sample;
            set => SetProperty(ref _sample, value);
        }
        private SampleType _sample;

    }
}
