using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfControlSamples.Views.Menus
{
    class ObjectContainerTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Template1 { get; set; }
        public DataTemplate Template2 { get; set; }
        public DataTemplate Template3 { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IObjectContainer data)
            {
                if (data.Data is bool) return Template1;
                if (data.Data is int) return Template2;
                if (data.Data is string) return Template3;
            }
            return base.SelectTemplate(item, container);
        }
    }
}
