#nullable disable
using System.Windows;
using System.Windows.Controls;

namespace WpfControlSamples.Views.Helpers
{
    /*
     * TreeView.SelectedItem に setter がないので 添付プロパティ で対応
     * TreeViewHelperBehavior の 添付プロパティ版
     *
     * 添付プロパティよりも Behavior の方が良い。
     *  - SelectedItem に初期値を設定しないと、イベントAttachされない
     *    (そのくせに、SelectedItem に setter がないから TwoWay バインドにならない)
     *  - 添付プロパティやとイベントの解除できへん
     */
    class TreeViewHelper : DependencyObject
    {
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.RegisterAttached(
                "SelectedItem", typeof(object), typeof(TreeViewHelper),
                new FrameworkPropertyMetadata("",
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedItemChanged));

        public static object GetSelectedItem(DependencyObject obj)
            => obj.GetValue(SelectedItemProperty);

        public static void SetSelectedItem(DependencyObject obj, object value)
            => obj.SetValue(SelectedItemProperty, value);

        public static readonly DependencyProperty IsAttachedProperty =
            DependencyProperty.RegisterAttached(
                "IsAttached", typeof(bool), typeof(TreeViewHelper),
                new FrameworkPropertyMetadata(false, (sender, e) =>
                {
                    if (!(sender is TreeView treeView)) return;

                    if ((bool)e.OldValue)
                    {
                        treeView.SelectedItemChanged -= TreeView_SelectedItemChanged;
                    }
                    if ((bool)e.NewValue)
                    {
                        treeView.SelectedItemChanged += TreeView_SelectedItemChanged;
                    }
                }));

        public static bool GetIsAttached(DependencyObject obj)
            => (bool)obj.GetValue(IsAttachedProperty);

        public static void SetIsAttached(DependencyObject obj, bool value)
            => obj.SetValue(IsAttachedProperty, value);

        private static void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!(sender is TreeView treeView)) return;
            //if (treeView.SelectedItem is null) return;

            SetSelectedItem(treeView, treeView.SelectedItem);
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TreeView treeView)) return;

            if (!GetIsAttached(treeView)) SetIsAttached(treeView, true);

            if (treeView.SelectedItem != e.NewValue)
            {
                treeView.SelectedItemChanged -= TreeView_SelectedItemChanged;
                //treeView.SelectedItem = e.NewValue;
                treeView.SelectedItemChanged += TreeView_SelectedItemChanged;
            }
        }
    }
}
