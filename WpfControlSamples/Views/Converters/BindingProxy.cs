using System;
using System.Windows;

namespace WpfControlSamples.Views.Converters
{
    /* Use IValueConverter with DynamicResource?
     *  https://stackoverflow.com/questions/4805351/use-ivalueconverter-with-dynamicresource
     *
     * [WPF] HOW TO BIND TO DATA WHEN THE DATACONTEXT IS NOT INHERITED
     *  https://thomaslevesque.com/2011/03/21/wpf-how-to-bind-to-data-when-the-datacontext-is-not-inherited/
     */
    class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore() => new BindingProxy();

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(
                nameof(Data), typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        public object Data
        {
            get => (object)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
    }
}
