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
    public partial class DispatcherTimerPage : ContentControl
    {
        private readonly DispatcherTimer _dispatcherTimer;
        private int _secondCounter;

        public DispatcherTimerPage()
        {
            InitializeComponent();

            var timer = new DispatcherTimer(DispatcherPriority.Normal, this.Dispatcher)
            {
                Interval = new TimeSpan(hours: 0, minutes: 0, seconds: 1)
            };
            timer.Tick += Timer_Tick;
            //timer.Start();
            _dispatcherTimer = timer;

            UpdateDataContext(0);
        }

        private void Timer_Tick(object sender, EventArgs e)
            => UpdateDataContext(++_secondCounter);

        private void UpdateDataContext(int counter) => DataContext = counter;

        public ICommand StartTimerCommand =>
            _startTimerCommand ??= new MyCommand(
                () => SwitchTimer(start: true),
                () => !_dispatcherTimer.IsEnabled);
        private ICommand _startTimerCommand;

        public ICommand StopTimerCommand =>
            _stopTimerCommand ??= new MyCommand(
                () => SwitchTimer(start: false),
                () => _dispatcherTimer.IsEnabled);
        private ICommand _stopTimerCommand;

        private void SwitchTimer(bool start)
        {
            var timer = _dispatcherTimer;
            if (start)
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }
            (StartTimerCommand as MyCommand).ChangeCanExecute();
            (StopTimerCommand as MyCommand).ChangeCanExecute();
        }
    }
}