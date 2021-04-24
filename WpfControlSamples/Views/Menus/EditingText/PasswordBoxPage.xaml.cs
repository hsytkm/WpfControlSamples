#nullable disable
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
    public partial class PasswordBoxPage : ContentControl
    {
        public PasswordBoxPage()
        {
            InitializeComponent();

            DataContext = new PasswordBoxViewModel();
        }
    }

    class PasswordBoxViewModel : MyBindableBase
    {
        public string UserPassword
        {
            get => _userPassword;
            set => SetProperty(ref _userPassword, value);
        }
        private string _userPassword;

        public ICommand EnterOkCommand => _enterOkCommand ??
            (_enterOkCommand = new MyCommand(() =>
                MessageBox.Show("Password is " + UserPassword, "Title")));
        private ICommand _enterOkCommand;

    }
}
