#nullable disable
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
            if (!(parameter is DependencyPropertyChangedEventArgs args)) return;
            if (!(args.NewValue is Point point)) return;

            this.AssociatedObject.SetCanvasLeftTop(point);
        }
    }
}
