using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using WpfControlSamples.ViewModels;

namespace WpfControlSamples.Views.Behaviors
{
    public class ColorMatrixUniformGridBehavior : Behavior<UniformGrid>
    {
        #region ColorMatrix
        public static readonly DependencyProperty ColorMatrixProperty =
            DependencyProperty.Register(nameof(ColorMatrix), typeof(ColorArray2d), typeof(ColorMatrixUniformGridBehavior),
                new FrameworkPropertyMetadata(null, OnColorMatrixChanged));

        public ColorArray2d? ColorMatrix
        {
            get => (ColorArray2d)GetValue(ColorMatrixProperty);
            set => SetValue(ColorMatrixProperty, value);
        }

        private static void OnColorMatrixChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ColorMatrixUniformGridBehavior behavior) return;
            if (behavior.AssociatedObject is not UniformGrid uniformGrid) return;
            uniformGrid.Children.Clear();

            if (e.NewValue is not ColorArray2d colors) return;
            (var columns, var rows) = (colors.Columns, colors.Rows);
            if (columns == 0 || rows == 0) return;

            if (uniformGrid.Columns != columns) uniformGrid.Columns = columns;
            if (uniformGrid.Rows != rows) uniformGrid.Rows = rows;

            var partitionWidth = uniformGrid.ActualWidth / columns;
            var partitionHeight = uniformGrid.ActualHeight / rows;
            (var borderWidth, var borderHeight, var borderThickness) = GetBorderSize(partitionWidth, partitionHeight);

            foreach (var color in colors)
            {
                // ◆Dictionary などで管理した方が良い
                var brush = new SolidColorBrush(color);
                brush.Freeze();

                var border = new Border()
                {
                    BorderBrush = brush,
                    Width = borderWidth,
                    Height = borderHeight,
                    BorderThickness = new Thickness(borderThickness),
                    Background = Brushes.Transparent,
                    IsHitTestVisible = false,
                    Opacity = 0.5,
                };
                uniformGrid.Children.Add(border);
            }
        }
        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SizeChanged += AssociatedObject_SizeChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SizeChanged -= AssociatedObject_SizeChanged;
            base.OnDetaching();
        }

        private static void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is not UniformGrid uniformGrid) return;
            if (uniformGrid.Columns == 0 || uniformGrid.Rows == 0) return;

            var partitionWidth = e.NewSize.Width / uniformGrid.Columns;
            var partitionHeight = e.NewSize.Height / uniformGrid.Rows;
            (var borderWidth, var borderHeight, var borderThickness) = GetBorderSize(partitionWidth, partitionHeight);

            foreach (var border in uniformGrid.Children.OfType<Border>())
            {
                border.Width = borderWidth;
                border.Height = borderHeight;
                border.BorderThickness = new Thickness(borderThickness);
            }
        }

        /// <summary>分割された領域のサイズから内部の Border のサイズ求めます</summary>
        private static (double width, double height, double thickness) GetBorderSize(double partitionWidth, double partitionHeight)
        {
            const double keisu = 16d;   // 根拠ないので変えても良いです
            var thickness = Math.Min(partitionWidth, partitionHeight) / keisu;
            var width = Math.Max(0d, partitionWidth - thickness);
            var height = Math.Max(0d, partitionHeight - thickness);
            return (width, height, thickness);
        }
    }

    // ◆どうせ定義済みの Brush しか使わないので、Color より Brush で管理した方が GC 減りそう
    public record ColorArray2d : ValueArray2d<Color>
    {
        public ColorArray2d(int width, int height, IEnumerable<Color> source) : base(width, height, source) { }
        public ColorArray2d(Color[,] array2d) : base(array2d) { }
    }
}
