﻿#nullable disable
using Microsoft.Xaml.Behaviors;
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
    public partial class MouseDragElementBehaviorPage : ContentControl
    {
        public MouseDragElementBehaviorPage()
        {
            InitializeComponent();
            DataContext = new MouseDragElementBehaviorViewModel();
        }
    }

    class MouseDragElementBehaviorViewModel : MyBindableBase
    {
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

    }
}
