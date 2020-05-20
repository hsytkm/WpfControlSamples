using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class PropertyChangedTriggerPage : ContentControl
    {
        public PropertyChangedTriggerPage()
        {
            InitializeComponent();
            DataContext = new PropertyChangedTriggerViewModel();
        }
    }

    class PropertyChangedTriggerViewModel : MyBindableBase
    {
        private readonly Random _random = new Random();

        public IList<string> Items
        {
            get => _items;
            private set => SetProperty(ref _items, value);
        }
        private IList<string> _items;

        public ICommand UpdateItemsSourceCommand => _updateItemsSourceCommand ??
            (_updateItemsSourceCommand = new MyCommand(() => UpdateItemsSource()));
        private ICommand _updateItemsSourceCommand;

        private void UpdateItemsSource()
        {
            var count = _random.Next(2, 10);
            Items = Enumerable.Range(0, count).Select(x => $"Data{x}").ToList();
        }
    }
}