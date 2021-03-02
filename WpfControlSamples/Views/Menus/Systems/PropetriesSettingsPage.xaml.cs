using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class PropetriesSettingsPage : ContentControl
    {
        public PropetriesSettingsPage()
        {
            InitializeComponent();
        }
    }

    class PropetriesSettingsViewModel : MyBindableBase
    {
        public string SettingString
        {
            get => _settingString;
            set => SetProperty(ref _settingString, value);
        }
        private string _settingString;

        public int SettingInt
        {
            get => _settingInt;
            set => SetProperty(ref _settingInt, value);
        }
        private int _settingInt;

        public bool SettingBool
        {
            get => _settingBool;
            set
            {
                if (SetProperty(ref _settingBool, value))
                {
                    Properties.Settings.Default.SettingSampleBool = value;
                    Properties.Settings.Default.Save();
                }
            }
        }
        private bool _settingBool;

        public PropetriesSettingsViewModel()
        {
            SettingString = Properties.Settings.Default.SettingSampleString;
            SettingInt = Properties.Settings.Default.SettingSampleInt;
            SettingBool = Properties.Settings.Default.SettingSampleBool;

            this.PropertyChanged += (_, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(SettingString):
                        Properties.Settings.Default.SettingSampleString = SettingString;
                        Properties.Settings.Default.Save();
                        break;
                    case nameof(SettingInt):
                        Properties.Settings.Default.SettingSampleInt = SettingInt;
                        Properties.Settings.Default.Save();
                        break;
                    case nameof(SettingBool):
                        // nop
                        break;
                }
            };
        }
    }
}
