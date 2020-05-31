using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class CollectionView3Page : ContentControl
    {
        public CollectionView3Page()
        {
            InitializeComponent();

            DataContext = new CollectionView3ViewModel();
        }
    }

    class CollectionView3ViewModel : MyBindableBase
    {
        public IList<ColorListViewItem> SourceColors { get; } =
            Models.SampleData.WpfColors.Select(x => new ColorListViewItem(x)).ToList();

        public ICommand FilterCommand => _filterCommand ??= new MyCommand<string>(name =>
            {
                var collectionView = CollectionViewSource.GetDefaultView(SourceColors);
                if (string.IsNullOrEmpty(name))
                {
                    collectionView.Filter = null;   // clear
                }
                else
                {
                    collectionView.Filter = x =>
                    {
                        var color = (ColorListViewItem)x;
                        return color.Name.Contains(name);
                    };
                }
            });
        private ICommand _filterCommand;

    }
}