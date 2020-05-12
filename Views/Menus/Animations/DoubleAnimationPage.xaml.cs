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
using System.Windows.Threading;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class DoubleAnimationPage : ContentControl
    {
        public DoubleAnimationPage()
        {
            InitializeComponent();

            // 100msecごとに水平座標を更新
            var timer = new DispatcherTimer(DispatcherPriority.Normal, this.Dispatcher)
            {
                Interval = new TimeSpan(0, 0, 0, 0, milliseconds: 100)
            };
            timer.Tick += (_, __) => DataContext = Canvas.GetLeft(rect1);
            timer.Start();
        }
    }
}