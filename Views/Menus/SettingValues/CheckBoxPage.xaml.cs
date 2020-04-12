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
    public partial class CheckBoxPage : ContentControl
    {
        public CheckBoxPage()
        {
            InitializeComponent();

            DataContext = new CheckBoxViewModel();
        }
    }

    class CheckBoxViewModel : MyBindableBase
    {
        public bool CheckFlag
        {
            get => _checkFlag;
            set => SetProperty(ref _checkFlag, value);
        }
        private bool _checkFlag = true;
    }
}