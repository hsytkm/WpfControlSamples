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
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class RepeatButtonPage : ContentControl
    {
        public RepeatButtonPage()
        {
            InitializeComponent();

            DataContext = new RepeatButtonViewModel();
        }
    }

    class RepeatButtonViewModel : MyBindableBase
    {
        public int WaitSeconds { get; } = 6;   // 計測時間

        public int Counter
        {
            get => _counter;
            private set => SetProperty(ref _counter, value);
        }
        private int _counter;

        public ICommand IncrementCommand => _incrementCommand ??
            (_incrementCommand = new MyCommand(() => ++Counter));
        private ICommand _incrementCommand;

        public ICommand DecrementCommand => _decrementCommand ??
            (_decrementCommand = new MyCommand(() => --Counter));
        private ICommand _decrementCommand;
    }
}