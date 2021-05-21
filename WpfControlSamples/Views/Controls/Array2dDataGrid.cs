using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfControlSamples.ViewModels;

namespace WpfControlSamples.Views.Controls
{
    /// <summary>Array2d 用のコンバーター</summary>
    public class Array2dDataGridConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                ValueArray2d<int> i => ToRowsList(i),
                ValueArray2d<double> d => ToRowsList(d),
                _ => null,
            };

            static IReadOnlyList<T>[] ToRowsList<T>(ValueArray2d<T> array2d) where T : struct
            {
                var rowSourceList = new IReadOnlyList<T>[array2d.Height];
                for (int y = 0; y < rowSourceList.Length; y++)
                {
                    rowSourceList[y] = array2d.GetRowSpan(y).ToArray();
                }
                return rowSourceList;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    /// <summary>
    /// 2次元配列をBindingするDataGrid
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Array2dDataGrid<T> : DataGrid where T : struct
    {
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            if (newValue is not IReadOnlyList<T>[] rowSourceList) return;

            this.Columns.Clear();

            var style = new Style(typeof(TextBlock));
            style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));

            var columnMax = rowSourceList[0].Count;
            for (int columnIndex = 0; columnIndex < columnMax; columnIndex++)
            {
                var columnIndexString = columnIndex.ToString();

                this.Columns.Add(new DataGridTextColumn()
                {
                    Header = "X" + columnIndexString,
                    Binding = new Binding("[" + columnIndexString + "]") { Mode = BindingMode.OneTime },
                    ElementStyle = style,
                });
            }
        }
    }

    // xamlでジェネリックを指定できない(と思う)ので個別に定義する
    public sealed class Array2dIntDataGrid : Array2dDataGrid<int> { }
    public sealed class Array2dDoubleDataGrid : Array2dDataGrid<double> { }
}
