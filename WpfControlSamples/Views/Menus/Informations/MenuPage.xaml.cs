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
    public partial class MenuPage : ContentControl
    {
        public MenuPage()
        {
            InitializeComponent();

            DataContext = new MenuViewModel();
        }
    }

    class MenuViewModel : MyBindableBase
    {
        public ICommand MenuItemSelectCommand => _menuItemSelectCommand ??
            (_menuItemSelectCommand = new MyCommand<string>(prm => Message = prm));
        private ICommand _menuItemSelectCommand;

        public string Message
        {
            get => _message;
            private set => SetProperty(ref _message, value);
        }
        private string _message;

    }
}
