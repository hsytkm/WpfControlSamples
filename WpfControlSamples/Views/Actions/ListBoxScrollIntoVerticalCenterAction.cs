using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace WpfControlSamples.Views.Actions
{
    /// <summary>
    /// ListBox の SelectedItem が "できるだけ" センターに表示されるようにスクロールします。
    /// 標準の ListBox.ScrollIntoView() を使用した場合は SelectedItem がドセンターに表示されません。
    /// </summary>
    public class ListBoxScrollIntoVerticalCenterAction : TriggerAction<ListBox>
    {
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject is not ListBox listBox) return;

            if (parameter is not SelectionChangedEventArgs args) return;
            if (args.AddedItems.Count <= 0) return;
            var selectedItem = args.AddedItems[0];
            if (selectedItem is null) return;

            var peer = UIElementAutomationPeer.CreatePeerForElement(listBox);
            if (peer.GetPattern(PatternInterface.Scroll) is not IScrollProvider scrollProvider) return;

            // ItemsSource 内の SelectedItem の割合(%)を求めます
            var verticalPercent = GetItemIndexPercent(listBox.ItemsSource.OfType<object>(), selectedItem);

            // %指定で垂直スクロールします(水平スクロールは現位置)
            scrollProvider.SetScrollPercent(scrollProvider.HorizontalScrollPercent, verticalPercent);
        }

        // コレクション内のアイテムの位置を割合(%)で返します。
        private static double GetItemIndexPercent(IEnumerable<object> source, object item)
        {
            int index = -1, count = 0;
            foreach (var value in source)
            {
                if (Equals(item, value)) index = count;
                count++;
            }
            return (index < 0 || count <= 1) ? 0d : (index * 100) / (double)(count - 1);
        }
    }
}
