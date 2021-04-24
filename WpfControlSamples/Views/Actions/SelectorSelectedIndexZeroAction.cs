#nullable disable
using Microsoft.Xaml.Behaviors;
using System;
using System.Windows.Controls.Primitives;

namespace WpfControlSamples.Views.Actions
{
    /// <summary>
    /// 先頭要素を選択するAction
    /// </summary>
    class SelectorSelectedIndexZeroAction : TriggerAction<Selector>
    {
        protected override void Invoke(object parameter)
        {
            if (!(this.AssociatedObject is Selector selector)) return;
            selector.SelectedIndex = 0;
        }
    }
}
