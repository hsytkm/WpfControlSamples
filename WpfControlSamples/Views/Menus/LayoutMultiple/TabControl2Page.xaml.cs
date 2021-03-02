using System;
using System.Collections.Generic;
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
    public partial class TabControl2Page : ContentControl
    {
        public TabControl2Page()
        {
            InitializeComponent();

            DataContext = new TabControl2ViewModel();
        }
    }

    class TabControl2ViewModel : MyBindableBase
    {
        public string[] TabContentSource { get; } =
            new[]
            {
                "Tab1 (This is First Content)" ,
                "Tab2 (This is Second Content)" ,
                "Tab3 (This is Third Content)" ,
                "Tab4" ,
                "Tab5" ,
                "Tab6 : long name" ,
                "Tab7 : long long name" ,
                "Tab8 : long long long name" ,
                "Tab9 : long long long long name" ,
            };

        public string SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }
        private string _selectedItem;

        public TabControl2ViewModel()
        {
            SelectedItem = TabContentSource[0];
        }
    }
}
