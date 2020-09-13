using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace WpfControlSamples.Views.Controls
{
    /// <summary>Ctrl+Cに対応したListView</summary>
    class MyListView : ListView
    {
        private const char _separator = '\t';

        public MyListView()
        {
            this.KeyDown += MyListView_KeyDown;
        }

        /// <summary>Ctrl+C 判定</summary>
        private static void MyListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is ListView listView)) return;

            // Ctrl + C
            var isCtrlKey = (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None;
            if (!(isCtrlKey && e.Key == Key.C)) return;

            CopyGridViewColumnsToClipboard(listView);

            e.Handled = true;
        }

        /// <summary>
        /// ListView.GridView の表示順序で要素をクリップボードにコピー
        /// </summary>
        private static void CopyGridViewColumnsToClipboard(ListView listView)
        {
            // SelectedItems
            var selectedItems = listView.SelectedItems.OfType<object>();
            if (!selectedItems.Any()) return;
            var itemsType = selectedItems.First().GetType();

            // Sort SelectedItems by index
            var itemsSource = listView.ItemsSource.OfType<object>();
            var sortedSelectedItems = selectedItems.OrderBy(x => itemsSource.ToList().IndexOf(x));

            // Ctrl + C
            if (!(listView.View is GridView gridView)) return;
            if (!(gridView.Columns is GridViewColumnCollection columns)) return;

            // Copy Header
            var builder = new StringBuilder();
            builder.AppendLine(string.Join(_separator, columns.Select(c => c.Header.ToString())));

            // Copy Values
            var bindingProperties = columns
                .Select(col => ((Binding)col.DisplayMemberBinding).Path.Path)
                .Select(name => itemsType.GetProperty(name))
                .ToArray();

            foreach (var item in sortedSelectedItems)
            {
                var values = bindingProperties
                    .Select(prop => prop.GetValue(item)?.ToString() ?? "");
                builder.AppendLine(string.Join(_separator, values));
            }

            Clipboard.SetText(builder.ToString());
        }

    }
}
