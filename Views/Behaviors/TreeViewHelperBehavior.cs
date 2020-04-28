using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace WpfControlSamples.Views.Behaviors
{
    /*
     * TreeView.SelectedItem に setter がないので Behavior で対応
     * TreeViewHelper の Behavior版（添付プロパティよりも Behavior の方が優れてる）
     */
    class TreeViewHelperBehavior : Behavior<TreeView>
    {
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(TreeViewHelperBehavior));

        public object SelectedItem
        {
            get => (object)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectedItemChanged += TreeView_SelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectedItemChanged -= TreeView_SelectedItemChanged;

            base.OnDetaching();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!(sender is TreeView tree)) return;
            if (tree.SelectedItem is null) return;

            SelectedItem = tree.SelectedItem;
        }
    }
}
