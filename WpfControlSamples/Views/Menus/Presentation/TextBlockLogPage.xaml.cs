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
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class TextBlockLogPage : ContentControl
    {
        public TextBlockLogPage()
        {
            InitializeComponent();
            DataContext = new TextBlockLogViewModel();
        }
    }

    class TextBlockLogViewModel : MyBindableBase
    {
        public StringBuilder EventLogBuilder { get; } = new StringBuilder();

        public ICommand AddToLogTimeNowCommand =>
            _addToLogTimeNowCommand ??= new MyCommand(() => InsertHeadLog(DateTime.Now.ToString()));
        private ICommand _addToLogTimeNowCommand;

        private void InsertHeadLog(string msg)
        {
            EventLogBuilder.Insert(0, msg + Environment.NewLine);
            NotifyPropertyChanged(nameof(EventLogBuilder));
        }

    }
}
