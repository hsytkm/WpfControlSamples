using Microsoft.Xaml.Behaviors;
using System;
using System.Windows.Controls;

namespace WpfControlSamples.Views.Actions
{
    /// <summary>ListBox の SelectedItem が表示されるようにスクロールを移動させます</summary>
    public class ListBoxScrollIntoViewAction : TriggerAction<ListBox>
    {
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject is not ListBox listBox) return;
            if (parameter is not SelectionChangedEventArgs args) return;
            if (args.AddedItems.Count <= 0) return;

            var item = args.AddedItems[0];
            listBox.ScrollIntoView(item);
        }
    }
}
