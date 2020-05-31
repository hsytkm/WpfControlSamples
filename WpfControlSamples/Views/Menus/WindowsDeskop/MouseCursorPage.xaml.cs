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
    public partial class MouseCursorPage : ContentControl
    {
        private readonly static IList<Cursor> _mouseCursors =
            typeof(Cursors).GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Select(x => (Cursor)x.GetValue(null))
                .ToList();

        public ICommand RadioButtonChangedCommand => _radioButtonChangedCommand ??
            (_radioButtonChangedCommand = new MyCommand<Cursor>(cursor => previewArea.Cursor = cursor));
        private ICommand _radioButtonChangedCommand;

        public MouseCursorPage()
        {
            InitializeComponent();
            DataContext = _mouseCursors;
        }
    }
}