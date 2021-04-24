#nullable disable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    public partial class ListBox5Page : ContentControl
    {
        // 以下の雑多なコレクションをリストに表示する
        internal static IReadOnlyCollection<object> SourceItems { get; } =
            new object[]
            {
                false,
                true,
                123,
                "Hello",
                null,
                4321,
                "こんにちわ",
                false,
                0,
            };

        public ListBox5Page()
        {
            InitializeComponent();
            DataContext = SourceItems;
        }
    }

    class MyItemContaierStyleSelector : StyleSelector
    {
        public Style Style1 { get; set; }
        public Style Style2 { get; set; }
        public Style Style3 { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
            => item switch
            {
                bool _ => Style1,
                int _ => Style2,
                string _ => Style3,
                _ => base.SelectStyle(item, container)
            };
    }
}
