using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfControlSamples.Views.AttachedProperties
{
    // [【WPF】カラムを３回押したときにソートを解除できるようにする - Qiita](https://qiita.com/denpadokei/items/838cc83269cf703aecbd)
    public sealed class DataGridRefreshSorting
    {
        public static readonly DependencyProperty CanRefreshSortingProperty =
            DependencyProperty.RegisterAttached("CanRefreshSorting", typeof(bool), typeof(DataGridRefreshSorting),
                new PropertyMetadata(OnCanRefreshSprtingChanged));

        public static bool GetCanRefreshSorting(DependencyObject dependencyObject) =>
            (bool)dependencyObject.GetValue(CanRefreshSortingProperty);

        public static void SetCanRefreshSorting(DependencyObject dependencyObject, bool value) =>
            dependencyObject.SetValue(CanRefreshSortingProperty, value);

        private static void OnCanRefreshSprtingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is not DataGrid dataGrid) return;
            if (e.NewValue is not bool newValue) return;
            if (e.OldValue is not bool oldValue) return;
            if (oldValue == newValue) return;

            if (newValue)
            {
                dataGrid.Sorting += OnDataGridSorting;
            }
            else
            {
                dataGrid.Sorting -= OnDataGridSorting;
            }
        }

        private static void OnDataGridSorting(object sender, DataGridSortingEventArgs e)
        {
            if (sender is not DataGrid dataGrid) return;

            if (e.Column.SortDirection is ListSortDirection.Descending)
            {
                e.Handled = true;
                e.Column.SortDirection = null;
                dataGrid.Items.SortDescriptions.Clear();
            }
        }
    }
}
