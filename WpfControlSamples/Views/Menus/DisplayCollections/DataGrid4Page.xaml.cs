﻿using System;
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
    public partial class DataGrid4Page : ContentControl
    {
        public DataGrid4Page()
        {
            DataContext = new DataGrid4ViewModel();
            InitializeComponent();
        }
    }

    class DataGrid4ViewModel : MyBindableBase
    {
        public ValueArray2d<int> SourceArray2d
        {
            get => _sourceArray2d;
            private set => SetProperty(ref _sourceArray2d, value);
        }
        private ValueArray2d<int> _sourceArray2d = default!;

        public ICommand UpdateDataCommand => _updateDataCommand ??= new MyCommand(UpdateData);
        private ICommand _updateDataCommand = default!;

        public DataGrid4ViewModel()
        {
            var ary2d = new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            SourceArray2d = new ValueArray2d<int>(ary2d);
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

            SourceArray2d = new ValueArray2d<int>(columns, rows, array);
        }
    }
}
