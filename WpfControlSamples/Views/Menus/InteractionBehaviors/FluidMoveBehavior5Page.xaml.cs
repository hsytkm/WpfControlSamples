#nullable disable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
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
    public partial class FluidMoveBehavior5Page : ContentControl
    {
        public ObservableCollection<FluidMoveBehavior5Data> Items { get; } =
            new ObservableCollection<FluidMoveBehavior5Data>();

        public ICommand DeleteItemCommand =>
            _deleteItemCommand ??= new MyCommand(() =>
            {
                if (Items.Any()) Items.RemoveAt(0);
            });
        private ICommand _deleteItemCommand;

        public FluidMoveBehavior5Page()
        {
            InitializeComponent();

            var timer = new DispatcherTimer(DispatcherPriority.Normal, this.Dispatcher)
            {
                Interval = new TimeSpan(hours: 0, minutes: 0, seconds: 3)
            };
            timer.Tick += Timer_Tick;
            timer.Start();

            DataContext = this;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Items.Count >= 20) return;

            var now = DateTime.Now;
            var color = Models.SampleData.WpfColors[now.Second].Color;
            var item = new FluidMoveBehavior5Data(now.ToString(), color);
            Items.Add(item);
        }
    }

    public class FluidMoveBehavior5Data
    {
        public string Name { get; }
        public Color BgColor { get; }
        public FluidMoveBehavior5Data(string s, Color c) => (Name, BgColor) = (s, c);
    }
}
