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
    public partial class TimerTriggerPage : ContentControl
    {
        public TimerTriggerPage()
        {
            InitializeComponent();
            DataContext = new TimerTriggerViewModel();
        }
    }

    class TimerTriggerViewModel : MyBindableBase
    {
        public int Count
        {
            get => _count;
            private set => SetProperty(ref _count, value);
        }
        private int _count;

        public void IncrementCounter() => ++Count;
    }
}