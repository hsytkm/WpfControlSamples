#nullable disable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
    public partial class MouseCursor2Page : ContentControl
    {
        public MouseCursor2Page()
        {
            InitializeComponent();

            var uri = new Uri("pack://application:,,,/WpfControlSamples;component/Resources/Cursors/zoom_mag.cur");
            using var stream = Application.GetResourceStream(uri).Stream;
            /*using */var myCursor1 = new Cursor(stream, true);
            border.Cursor = myCursor1;
        }
    }
}
