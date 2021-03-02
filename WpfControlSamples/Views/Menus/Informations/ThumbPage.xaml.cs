using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    public partial class ThumbPage : ContentControl
    {
        public ThumbPage()
        {
            InitializeComponent();
        }

        private void Thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (!(sender is Thumb thumb)) return;
            if (!(thumb.Template.FindName("thumbInBorder", thumb) is Border border)) return;

            border.BorderThickness = new Thickness(5);
        }

        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (!(sender is Thumb thumb)) return;
            if (!(thumb.Template.FindName("thumbInBorder", thumb) is Border border)) return;

            border.BorderThickness = new Thickness(0);
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!(sender is Thumb thumb)) return;
            if (!(thumb.Parent is Canvas canvas)) return;

            var x = Canvas.GetLeft(thumb) + e.HorizontalChange;
            var y = Canvas.GetTop(thumb) + e.VerticalChange;

            x = Math.Max(x, 0);
            y = Math.Max(y, 0);
            x = Math.Min(x, canvas.ActualWidth - thumb.ActualWidth);
            y = Math.Min(y, canvas.ActualHeight - thumb.ActualHeight);

            Canvas.SetLeft(thumb, x);
            Canvas.SetTop(thumb, y);
        }
    }
}
