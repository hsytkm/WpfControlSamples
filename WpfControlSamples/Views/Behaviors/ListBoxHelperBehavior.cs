#nullable disable
using Microsoft.Xaml.Behaviors;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace WpfControlSamples.Views.Behaviors
{
    // NotifySelectedItemsAction.cs に変更
    // (Behavior より Action の方が何してるか分かりやすそうなので)
#if false
    // ListBox.SelectedItems に setter がないので Behavior で対応
    //
    // <i:Interaction.Behaviors>
    //     <vb:ListBoxHelperBehavior NotifySelectedItems="{Binding NotifySelectedColors, Mode=OneWay}" />
    // </i:Interaction.Behaviors>
    class ListBoxHelperBehavior : Behavior<ListBox>
    {
        public static readonly DependencyProperty NotifySelectedItemsProperty =
            DependencyProperty.Register(
                nameof(NotifySelectedItems),
                typeof(ObservableCollection<object>),
                typeof(ListBoxHelperBehavior));

        public ObservableCollection<object> NotifySelectedItems
        {
            get => (ObservableCollection<object>)GetValue(NotifySelectedItemsProperty);
            set => SetValue(NotifySelectedItemsProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectionChanged += ListBox_SelectionChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= ListBox_SelectionChanged;

            base.OnDetaching();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItems = NotifySelectedItems;
            if (selectedItems is null) return;

            if (e.AddedItems != null)
            {
                foreach (var item in e.AddedItems)
                {
                    selectedItems.Add(item);
                }
            }
            if (e.RemovedItems != null)
            {
                foreach (var item in e.RemovedItems)
                {
                    if (selectedItems.Contains(item))
                    {
                        selectedItems.Remove(item);
                    }
                }
            }
        }
    }
#endif
}
