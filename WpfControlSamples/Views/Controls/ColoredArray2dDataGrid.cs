﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using WpfControlSamples.Extensions;
using WpfControlSamples.Models;
using WpfControlSamples.ViewModels;

namespace WpfControlSamples.Views.Controls
{
    /// <summary>ColoredValueArray2d 用のコンバーター</summary>
    public class ColoredValueArray2dConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                ColoredValueArray2d<int> i => ToRowsList(i),
                ColoredValueArray2d<double> d => ToRowsList(d),
                _ => null,
            };

            static IReadOnlyList<ValueColorPair<T>>[] ToRowsList<T>(ColoredValueArray2d<T> array2d) where T : struct, IComparable<T>
            {
                var rowSourceList = new IReadOnlyList<ValueColorPair<T>>[array2d.Height];
                for (int y = 0; y < rowSourceList.Length; y++)
                {
                    var pairs = new ValueColorPair<T>[array2d.Width];
                    for (int x = 0; x < array2d.Width; x++)
                    {
                        pairs[x] = new ValueColorPair<T>(array2d.GetValueColorPair(x, y));
                    }
                    rowSourceList[y] = pairs;
                }
                return rowSourceList;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    /// <summary>Converter から Control に渡す情報</summary>
    public record ValueColorPair<T> where T : struct, IComparable<T>
    {
        public T Value { get; }
        public Color Color { get; }
        private ValueColorPair(T value, Color color) => (Value, Color) = (value, color);
        public ValueColorPair((T value, Color color) x) : this(x.value, x.color) { }
        public Brush BackgroundBrush
        {
            get
            {
                var brush = new SolidColorBrush(Color);
                brush.Freeze();
                return brush;
            }
        }
        public Brush ForegroundBrush => Color.GetForegroundBrush();
    }

    /// <summary>
    /// Excelのように値に応じて色付けするDataGrid
    /// DataGrid を継承するのダサいけど、OnItemsSourceChanged() にアクセスする手段がパっとなかった。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ColoredArray2dDataGrid<T> : DataGrid where T : struct, IComparable<T>
    {
        public static readonly DependencyProperty CellTextBlockStyleProperty =
            DependencyProperty.Register(nameof(CellTextBlockStyle), typeof(Style), typeof(ColoredArray2dDataGrid<T>));
        public Style CellTextBlockStyle
        {
            get => (Style)GetValue(CellTextBlockStyleProperty);
            set => SetValue(CellTextBlockStyleProperty, value);
        }

        public ColoredArray2dDataGrid()
        {
            // 列ヘッダのクリックで全列を選択する対応
            this.Resources.Add(typeof(DataGridColumnHeader), DataGridColumnHeaderStyles.SelectAllColumnsStyle());
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            if (newValue is not IReadOnlyList<ValueColorPair<T>>[] rowSourceList) return;

            this.Columns.Clear();

            var columnMax = rowSourceList[0].Count;
            for (int columnIndex = 0; columnIndex < columnMax; columnIndex++)
            {
                var columnIndexString = columnIndex.ToString();
                var bindingTarget = "[" + columnIndexString + "].";
                var bindingFore = new Binding(bindingTarget + nameof(ValueColorPair<T>.ForegroundBrush)) { Mode = BindingMode.OneTime };
                var bindingBack = new Binding(bindingTarget + nameof(ValueColorPair<T>.BackgroundBrush)) { Mode = BindingMode.OneTime };

                var cellStyle = new Style(typeof(DataGridCell));
                cellStyle.Setters.Add(new Setter(DataGridCell.ForegroundProperty, bindingFore));
                cellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, bindingBack));

                // セル選択時の文字色
                // see -> https://github.com/hsytkm/LumE2pTextViewer/blob/trunk/src/LumE2pTextViewer.Mvvm/Controls/ColoredArray2dDataGrid.cs
                //var trigger = new Trigger() { Property = DataGridCell.IsSelectedProperty, Value = true };
                //trigger.Setters.Add(new Setter(ForegroundProperty, binding));
                //cellStyle.Triggers.Add(trigger);

                Columns.Add(new DataGridTextColumn()
                {
                    Header = "X" + columnIndexString,
                    Binding = new Binding(bindingTarget + nameof(ValueColorPair<T>.Value)) { Mode = BindingMode.OneTime },
                    CellStyle = cellStyle,
                    ElementStyle = CellTextBlockStyle,
                });
            }
        }
    }

    // xamlでジェネリックを指定できない(と思う)ので個別に定義する
    public sealed class ColoredArray2dIntDataGrid : ColoredArray2dDataGrid<int> { }
    public sealed class ColoredArray2dDoubleDataGrid : ColoredArray2dDataGrid<double> { }
}
