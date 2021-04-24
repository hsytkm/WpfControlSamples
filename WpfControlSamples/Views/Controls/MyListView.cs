#nullable disable
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

            if (!(listView.View is GridView gridView)) return;
            if (!(gridView.Columns is GridViewColumnCollection columns)) return;

            // Copy Header
            var builder = new StringBuilder();
            builder.AppendLine(string.Join(_separator, columns.Select(c => c.Header.ToString())));

            var propHints = columns
                .Select(col => ((Binding)col.DisplayMemberBinding).Path.Path)
                .Select(path => GetPropertyHint(path))
                .ToArray();

            foreach (var item in sortedSelectedItems)
            {
                var values = propHints.Select(hint => GetPropValue(hint, item)?.ToString() ?? "");
                builder.AppendLine(string.Join(_separator, values));
            }

            Clipboard.SetText(builder.ToString());

            // リフレクションでプロパティを取得するための情報を取得する
            static (string name, int? index) GetPropertyHint(string pathName)
            {
                static bool IsArrayPropName(string sourceName, out string arrayName, out int index)
                {
                    var match = Regex.Match(sourceName, @"^(?<name>.+)\[(?<index>[0-9]+)\]$");
                    if (match.Success)
                    {
                        arrayName = match.Groups["name"].Value;
                        index = Convert.ToInt32(match.Groups["index"].Value);
                        return true;
                    }
                    arrayName = sourceName;
                    index = -1;
                    return false;
                }
                return IsArrayPropName(pathName, out var arrayName, out var index)
                    ? (arrayName, (int?)index) : (pathName, null);
            }

            // プロパティ名の値を取得する（配列も無理やり対応）
            static object GetPropValue((string name, int? index) hint, object obj)
            {
                var type = obj.GetType();
                var value = type.GetProperty(hint.name).GetValue(obj);

                // 配列(List<T>)の読み出し
                if (hint.index.HasValue && value.GetType().IsArray)
                    return ((Array)value).Cast<object>().ElementAt(hint.index.Value);

                return value;
            }
        }

    }
}
