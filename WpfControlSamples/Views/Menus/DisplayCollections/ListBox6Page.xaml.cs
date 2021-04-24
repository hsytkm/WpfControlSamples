#nullable disable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class ListBox6Page : ContentControl
    {
        static IReadOnlyCollection<DataContainer> SourceItems => ListBox5Page.SourceItems
            .Select(x => new DataContainer(x)).ToArray();

        public ListBox6Page()
        {
            InitializeComponent();

            var view = CollectionViewSource.GetDefaultView(SourceItems);
            view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(DataContainer.Type)));
            //view.SortDescriptions.Add(new SortDescription(nameof(DataContainer.Type), ListSortDirection.Descending));
            //view.SortDescriptions.Add(new SortDescription(nameof(DataContainer.Value), ListSortDirection.Ascending));

            DataContext = view;
        }
    }

    class DataContainer
    {
        public Type Type { get; }
        public object Value { get; set; }
        public DataContainer(object obj)
        {
            Type = obj?.GetType();
            Value = obj;
        }
        public override string ToString() => Value?.ToString() ?? "null";
    }

    class MyDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Template1 { get; set; }
        public DataTemplate Template2 { get; set; }
        public DataTemplate Template3 { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is CollectionViewGroup group && group.Name is Type type)
            {
                // typeof は定数値じゃないので switch 使えないってさ。なんでやねん
                if (type == typeof(bool)) return Template1;
                if (type == typeof(int)) return Template2;
                if (type == typeof(string)) return Template3;
            }
            return base.SelectTemplate(item, container);
        }
    }
}
