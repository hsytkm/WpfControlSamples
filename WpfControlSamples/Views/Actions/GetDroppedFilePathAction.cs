using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace WpfControlSamples.Views.Actions
{
    public sealed class GetDroppedFilePathAction : TriggerAction<DependencyObject>
    {
        #region DroppedPath
        /// <summary>
        /// ドロップされた先頭ファイルPATH
        /// </summary>
        public static readonly DependencyProperty DroppedPathProperty =
            DependencyProperty.Register(nameof(DroppedPath), typeof(string), typeof(GetDroppedFilePathAction),
                new FrameworkPropertyMetadata(""));

        public string DroppedPath
        {
            get => (string)GetValue(DroppedPathProperty);
            set => SetValue(DroppedPathProperty, value);
        }
        #endregion

        #region DroppedPaths
        /// <summary>
        /// ドロップされた全ファイルPATH
        /// </summary>
        public static readonly DependencyProperty DroppedPathsProperty =
            DependencyProperty.Register(nameof(DroppedPaths), typeof(ObservableCollection<string>), typeof(GetDroppedFilePathAction));

        public ObservableCollection<string> DroppedPaths
        {
            get => (ObservableCollection<string>)GetValue(DroppedPathsProperty);
            set => SetValue(DroppedPathsProperty, value);
        }
        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is UIElement element)
                element.AllowDrop = true;

            if (AssociatedObject is System.Windows.Controls.TextBox)
            {
                /* TextBox に Attach する場合は、TextBoxDroppedFilePathBehaviors を使いましょう。
                 * 本当は this(GetDroppedFilePathAction) を使いたいけど、Action での実装だと
                 * 実行時に DataObjectPastingEventArgs.FormatToApply で Exception が発生して対処できなかった。
                 * shoganai ので専用の Behavior 実装でで対応しました。
                 */
                throw new NotSupportedException($"Use {nameof(Behaviors.TextBoxDroppedFilePathBehaviors)}!");
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject is UIElement element)
                element.AllowDrop = false;

            base.OnDetaching();
        }

        private void TextBox_PreviewDragOver(object sender, DragEventArgs e) => e.Handled = true;

        protected override void Invoke(object parameter)
        {
            if (parameter is not DragEventArgs e) return;
            var paths = GetFilePaths(e.Data).ToArray();

            DroppedPath = paths.Length > 0 ? paths[0] : "";

            // Ctrl押下じゃなければ既登録分を削除
            var isPressCtrlKey = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            if (!isPressCtrlKey) DroppedPaths.Clear();

            foreach (var path in paths)
            {
                // 重複登録のチェックが必要
                if (!DroppedPaths.Contains(path))
                    DroppedPaths.Add(path);
            }
        }

        internal static IEnumerable<string> GetFilePaths(IDataObject data)
        {
            if (data.GetDataPresent(DataFormats.FileDrop))
            {
                if (data.GetData(DataFormats.FileDrop) is string[] ss)
                {
                    foreach (var s in ss)
                        yield return s;
                }
                else
                {
                    throw new FormatException();
                }
            }
            else
            {
                var path = data.GetData(DataFormats.Text)?.ToString() ?? "";
                yield return path;
            }
        }
    }
}
