using System;
using System.Windows;

namespace WpfControlSamples.Views.Helpers;

/* Use IValueConverter with DynamicResource?
 *  https://stackoverflow.com/questions/4805351/use-ivalueconverter-with-dynamicresource
 *
 * [WPF] HOW TO BIND TO DATA WHEN THE DATACONTEXT IS NOT INHERITED
 *  https://thomaslevesque.com/2011/03/21/wpf-how-to-bind-to-data-when-the-datacontext-is-not-inherited/
 */
public sealed class BindingProxy : Freezable
{
    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy));

    public object? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    protected override Freezable CreateInstanceCore() => new BindingProxy();
}
