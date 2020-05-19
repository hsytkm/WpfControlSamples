using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
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
    public partial class EventTriggerAction3Page : ContentControl
    {
        public EventTriggerAction3Page()
        {
            InitializeComponent();
            DataContext = new EventTriggerAction3ViewModel();
        }
    }

    class EventTriggerAction3ViewModel : MyBindableBase
    {
        #region PropertyChangedTrigger
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
        #endregion

        #region TranslateZoomRotateBehavior
        public double MoveOffsetX
        {
            get => _offsetX;
            private set => SetProperty(ref _offsetX, value);
        }
        private double _offsetX;

        public double MoveOffsetY
        {
            get => _offsetY;
            private set => SetProperty(ref _offsetY, value);
        }
        private double _offsetY;

        public ICommand MatrixCommand => _matrixCommand ??
            (_matrixCommand = new MyCommand<Matrix>(matrix =>
            {
                MoveOffsetX = matrix.OffsetX;
                MoveOffsetY = matrix.OffsetY;
            }));
        private ICommand _matrixCommand;
        #endregion

    }
}