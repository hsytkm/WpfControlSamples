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
    public partial class GroupBoxPage : ContentControl
    {
        public GroupBoxPage()
        {
            InitializeComponent();

            DataContext = new GroupBoxViewModel();
        }
    }

    class GroupBoxViewModel : MyBindableBase
    {
        public bool IsEnableRadioGroup
        {
            get => _isEnableRadioGroup;
            set => SetProperty(ref _isEnableRadioGroup, value);
        }
        private bool _isEnableRadioGroup = true;

    }
}