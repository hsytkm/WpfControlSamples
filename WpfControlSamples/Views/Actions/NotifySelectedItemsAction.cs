using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WpfControlSamples.Views.Actions
{
    /* Selector.SelectedItems に setter がないので TriggerAction で対応
     *
     *  <i:Interaction.Triggers>
     *      <i:EventTrigger EventName="SelectionChanged" >
     *          <action:NotifySelectedItemsAction NotifySelectedItemsCollection="{Binding NotifySelectedItems, Mode=OneTime}" />
     *       </i:EventTrigger>
     *  </i:Interaction.Triggers>
     *
     * Generic クラスにしたいけど、WPF の x:TypeArguments はルート要素じゃないと使えないっぽい。なんでやねん
     * https://docs.microsoft.com/ja-jp/dotnet/desktop/xaml-services/generics
     */
    class NotifySelectedItemsAction : TriggerAction<Selector>
    {
        public static readonly DependencyProperty NotifySelectedItemsCollectionProperty =
            DependencyProperty.Register(
                nameof(NotifySelectedItemsCollection),
                typeof(ObservableCollection<object>),
                typeof(NotifySelectedItemsAction));

        public ObservableCollection<object> NotifySelectedItemsCollection
        {
            get => (ObservableCollection<object>)GetValue(NotifySelectedItemsCollectionProperty);
            set => SetValue(NotifySelectedItemsCollectionProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            if (parameter is not SelectionChangedEventArgs e) return;

            var selectedItems = NotifySelectedItemsCollection;
            if (selectedItems is null) return;

            if (e.AddedItems is not null)
            {
                foreach (var item in e.AddedItems)
                {
                    selectedItems.Add(item);
                }
            }
            if (e.RemovedItems is not null)
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
}
