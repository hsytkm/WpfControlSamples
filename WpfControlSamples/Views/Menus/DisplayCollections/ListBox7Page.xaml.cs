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
    public partial class ListBox7Page : ContentControl
    {
        static IReadOnlyCollection<DataContainer> SourceItems => ListBox5Page.SourceItems
            .Select(x => new DataContainer(x)).ToArray();

        public ListBox7Page()
        {
            InitializeComponent();

            var view = CollectionViewSource.GetDefaultView(SourceItems);
            view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(DataContainer.Type)));
            //view.SortDescriptions.Add(new SortDescription(nameof(DataContainer.Type), ListSortDirection.Descending));
            //view.SortDescriptions.Add(new SortDescription(nameof(DataContainer.Value), ListSortDirection.Ascending));

            DataContext = view;
        }
    }

    class MyGroupStyleSelector : StyleSelector
    {
        public Style Style1 { get; set; }
        public Style Style2 { get; set; }
        public Style Style3 { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is CollectionViewGroup group && group.Name is Type type)
            {
                // typeof は定数値じゃないので switch 使えないってさ。なんでやねん
                if (type == typeof(bool)) return Style1;
                if (type == typeof(int)) return Style2;
                if (type == typeof(string)) return Style3;
            }
            return base.SelectStyle(item, container);
        }
    }

}
