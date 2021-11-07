#nullable disable
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
    public partial class ContextMenu1Page : ContentControl
    {
        public ContextMenu1Page()
        {
            InitializeComponent();
        }
    }

    class ContextMenu1ViewModel : MyBindableBase
    {
        public ICommand ClickCommand => _clickCommand ??
            (_clickCommand = new MyCommand<object>(prm => Message = prm.ToString()));
        private ICommand _clickCommand;

        public string Message
        {
            get => _message;
            private set => SetProperty(ref _message, value);
        }
        private string _message;

        public bool CheckFlag
        {
            get => _checkFlag;
            set => SetProperty(ref _checkFlag, value);
        }
        private bool _checkFlag = true;

        public bool IsOpenContextMenu
        {
            get => _isOpenContextMenu;
            set => SetProperty(ref _isOpenContextMenu, value);
        }
        private bool _isOpenContextMenu;

    }
}
