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
    public partial class TransformsPage : ContentControl
    {
        public TransformsPage()
        {
            InitializeComponent();
        }

        private void ScaleCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.scaleTransform.ScaleX = 0.5;
            this.scaleTransform.ScaleY = 1.5;
        }
        private void ScaleCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.scaleTransform.ScaleX = 1;
            this.scaleTransform.ScaleY = 1;
        }

        private void RotateCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.rotateTransform.Angle = 50;
            this.rotateTransform.CenterX = 40;
            this.rotateTransform.CenterY = 100;
        }
        private void RotateCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.rotateTransform.Angle = 0;
            this.rotateTransform.CenterX = 0;
            this.rotateTransform.CenterY = 0;
        }

        private void SkewCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.skewTransform.AngleX = 40;
            this.skewTransform.AngleY = 20;
        }
        private void SkewCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.skewTransform.AngleX = 0;
            this.skewTransform.AngleY = 0;
        }

        private void TranslateCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.translateTransform.X = 50;
            this.translateTransform.Y = 40;
        }
        private void TranslateCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.translateTransform.X = 0;
            this.translateTransform.Y = 0;
        }
    }
}
