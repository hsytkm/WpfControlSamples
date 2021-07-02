using Microsoft.Xaml.Behaviors;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfControlSamples.Views.Actions;

namespace WpfControlSamples.Views.Behaviors
{
    /// <summary>
    /// TextBox専用の DropファイルPath取得
    /// 本当は GetDroppedFilePathAction を使いたいけど、Actionの実行タイミングだと DataObjectPastingEventArgs.FormatToApply で
    /// Exception が発生して対処できなかったので、専用の Behavior で対応する。 shoganai
    /// </summary>
    public sealed class TextBoxDroppedFilePathBehaviors : Behavior<TextBox>
    {
        public static readonly DependencyProperty DroppedPathProperty =
            DependencyProperty.Register(nameof(DroppedPath), typeof(string), typeof(TextBoxDroppedFilePathBehaviors),
                new FrameworkPropertyMetadata(""));

        public string DroppedPath
        {
            get => (string)GetValue(DroppedPathProperty);
            set => SetValue(DroppedPathProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.AllowDrop = true;
            AssociatedObject.PreviewDragOver += TextBox_PreviewDragOver;
            AssociatedObject.PreviewDrop += AssociatedObject_PreviewDrop;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.AllowDrop = false;
            AssociatedObject.PreviewDragOver -= TextBox_PreviewDragOver;
            AssociatedObject.PreviewDrop -= AssociatedObject_PreviewDrop;

            base.OnDetaching();
        }

        private void TextBox_PreviewDragOver(object sender, DragEventArgs e) => e.Handled = true;

        private void AssociatedObject_PreviewDrop(object sender, DragEventArgs e)
        {
            DroppedPath = GetDroppedFilePathAction.GetFilePaths(e.Data).FirstOrDefault() ?? "";
            e.Handled = true;
        }
    }
}
