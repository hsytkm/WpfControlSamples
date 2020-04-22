using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfControlSamples.Views.Triggers
{
    class GetDroppedFilePathAction : TriggerAction<DependencyObject>
    {
        #region DroppedPath
        /// <summary>
        /// ドロップされた先頭ファイルPATH
        /// </summary>
        public static readonly DependencyProperty DroppedPathProperty
            = DependencyProperty.Register(
                nameof(DroppedPath), typeof(string), typeof(GetDroppedFilePathAction), null);

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
        public static readonly DependencyProperty DroppedPathsProperty
            = DependencyProperty.Register(
                nameof(DroppedPaths), typeof(IList<string>), typeof(GetDroppedFilePathAction), null);

        public IList<string> DroppedPaths
        {
            get => (IList<string>)GetValue(DroppedPathsProperty);
            set => SetValue(DroppedPathsProperty, value);
        }
        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is UIElement element)
                element.AllowDrop = true;
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject is UIElement element)
                element.AllowDrop = false;

            base.OnDetaching();
        }

        protected override void Invoke(object parameter)
        {
            if (!(parameter is DragEventArgs e)) return;

            var paths = GetFilePaths(e.Data);
            DroppedPaths = paths;
            DroppedPath = paths.Any() ? paths[0] : null;
        }

        private static IList<string> GetFilePaths(IDataObject data)
        {
            if (data.GetDataPresent(DataFormats.FileDrop))
            {
                if (data.GetData(DataFormats.FileDrop) is string[] ss)
                    return ss;

                throw new FormatException();
            }
            else
            {
                var path = data.GetData(DataFormats.Text)?.ToString();
                return new List<string>() { path };
            }
        }
    }
}
