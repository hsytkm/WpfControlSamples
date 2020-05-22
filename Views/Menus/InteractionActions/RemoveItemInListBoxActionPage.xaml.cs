﻿using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class RemoveItemInListBoxActionPage : ContentControl
    {
        public RemoveItemInListBoxActionPage()
        {
            InitializeComponent();

            var source = Models.SampleData.WpfColors.Select(x => new ColorListViewItem(x));
            var items = new ObservableCollection<ColorListViewItem>(source);

            DataContext = items;
        }
    }
}