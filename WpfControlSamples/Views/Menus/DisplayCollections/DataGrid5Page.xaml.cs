using System;
using System.Collections;
using System.Collections.Generic;
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
using WpfControlSamples.ViewModels;

namespace WpfControlSamples.Views.Menus
{
    public partial class DataGrid5Page : ContentControl
    {
        public DataGrid5Page()
        {
            DataContext = new DataGrid5ViewModel();
            InitializeComponent();
        }
    }

    class DataGrid5ViewModel : MyBindableBase
    {
        public ColoredValueArray2d<int> SourceArray2d
        {
            get => _sourceArray2d;
            private set => SetProperty(ref _sourceArray2d, value);
        }
        private ColoredValueArray2d<int> _sourceArray2d = default!;

        public ICommand UpdateDataCommand => _updateDataCommand ??= new MyCommand(UpdateData);
        private ICommand _updateDataCommand = default!;

        public DataGrid5ViewModel()
        {
            var ary2d = new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            var ruleSource = new ColoringRule<int>[]
            {
                new(6, Colors.RoyalBlue),
                new(4, Colors.LightSkyBlue),
                new(2, Colors.AliceBlue),
            };
            var rules = new ColoringRules<int>(ruleSource, isDescendingOrder: true);

            SourceArray2d = new ColoredValueArray2d<int>(ary2d, rules);
        }

        private readonly Random _random = new();
        private void UpdateData()
        {
            var rows = _random.Next(1, 10);
            var columns = _random.Next(1, 10);
            var array = new int[columns * rows];

            var maxValue = 100;
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = _random.Next(maxValue);
            }

            var ruleSource = new ColoringRule<int>[]
            {
                new(){ Threshold = maxValue / 4     , Color = Colors.AliceBlue    },
                new(){ Threshold = maxValue * 2 / 4 , Color = Colors.LightSkyBlue },
                new(){ Threshold = maxValue * 3 / 4 , Color = Colors.DodgerBlue   },
                new(){ Threshold = maxValue         , Color = Colors.RoyalBlue    },
            };
            var rules = new ColoringRules<int>(ruleSource, isDescendingOrder : false);

            SourceArray2d = new ColoredValueArray2d<int>(columns, rows, array, rules);
        }
    }
}
