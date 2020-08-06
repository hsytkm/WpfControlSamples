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
using System.Windows.Threading;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class CustomTriggerPage : ContentControl
    {
        private int _counter;

        public CustomTriggerPage()
        {
            InitializeComponent();

            var timer = new DispatcherTimer(DispatcherPriority.Normal, this.Dispatcher)
            {
                Interval = new TimeSpan(hours: 0, minutes: 0, seconds: 2)
            };

            timer.Tick += (_, __) =>
            {
                ++_counter;
                (IntervalCommand as MyCommand).ChangeCanExecute();
            };

            timer.Start();
        }

        public ICommand IntervalCommand => _intervalCommand ??= new MyCommand(
            () => Debug.WriteLine("Execute IntervalCommand."),
            () => (_counter & 1) == 1);
        private ICommand _intervalCommand;
    }
}