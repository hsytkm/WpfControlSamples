﻿using System;
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
    public partial class ToolTipPage : ContentControl
    {
        public ToolTipPage()
        {
            InitializeComponent();
            DataContext = 1.23456789;
        }

        private void TextBlock_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            Debug.WriteLine("ToolTipOpening");
        }

        private void TextBlock_ToolTipClosing(object sender, ToolTipEventArgs e)
        {
            Debug.WriteLine("ToolTipClosing");
        }
    }
}
