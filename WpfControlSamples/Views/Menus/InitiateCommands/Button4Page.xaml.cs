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
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class Button4Page : ContentControl
    {
        public Button4Page()
        {
            DataContext = new Button4ViewModel();
            InitializeComponent();
        }
    }

    class Button4ViewModel : MyBindableBase
    {
        public int Counter
        {
            get => _counter;
            private set => SetProperty(ref _counter, value);
        }
        private int _counter;

        public ICommand CountUpCommand => _countUpCommand ??= new MyCommand(() => Counter++);
        private ICommand _countUpCommand;

    }
}
