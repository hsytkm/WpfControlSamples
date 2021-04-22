using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;

// This code is taken from
// https://github.com/runceel/Livet/blob/c1b5b6ed305bcad9e1c6d32a5a4001cba38bcfac/LivetCask.Behaviors/DataContextDisposeAction.cs
namespace WpfControlSamples.Views.Actions
{
    /// <summary>アタッチしたオブジェクトの DataContext が IDisposable なら Dispose します</summary>
    public class DataContextDisposeAction : TriggerAction<FrameworkElement>
    {
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject.DataContext is IDisposable disposable)
                disposable.Dispose();
        }
    }
}
