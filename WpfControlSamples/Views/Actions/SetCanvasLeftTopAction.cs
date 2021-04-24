using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using WpfControlSamples.Extensions;

namespace WpfControlSamples.Views.Actions
{
    /// <summary>
    /// Canvas の Left/Top を設定する
    /// </summary>
    class SetCanvasLeftTopAction : TriggerAction<UIElement>
    {
        protected override void Invoke(object parameter)
        {
            if (parameter is not DependencyPropertyChangedEventArgs args) return;
            if (args.NewValue is not Point point) return;

            this.AssociatedObject.SetCanvasLeftTop(point);
        }
    }
}
