#nullable disable
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
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class CalendarPage : ContentControl
    {
        public CalendarPage()
        {
            InitializeComponent();
        }
    }

    class CalendarViewModel : MyBindableBase
    {
        public DateTime? Date1
        {
            get => _date1;
            set => SetProperty(ref _date1, value);
        }
        private DateTime? _date1;

        public DateTime? Date2
        {
            get => _date2;
            set => SetProperty(ref _date2, value);
        }
        private DateTime? _date2;

    }
}
