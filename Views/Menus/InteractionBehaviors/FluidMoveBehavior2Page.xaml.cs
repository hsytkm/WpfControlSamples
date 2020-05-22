using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
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
    public partial class FluidMoveBehavior2Page : ContentControl
    {
        public FluidMoveBehavior2Page()
        {
            InitializeComponent();

            DataContext = new FluidMoveBehavior2ViewModel();
        }
    }

    class FluidMoveBehavior2ViewModel : MyBindableBase
    {
        public ObservableCollection<FluidData> LeftItems { get; } =
            new ObservableCollection<FluidData>()
            {
                new FluidData("Data1", Brushes.LightBlue),
                new FluidData("Data2", Brushes.LightPink),
                new FluidData("Data3", Brushes.LightGreen),
                new FluidData("Data4", Brushes.LightCoral),
                new FluidData("Data5", Brushes.LightSeaGreen),
            };
        public ObservableCollection<FluidData> RightItems { get; } =
            new ObservableCollection<FluidData>();

        public FluidData LeftSelectedItem
        {
            get => _leftSelectedItem;
            set
            {
                if (SetProperty(ref _leftSelectedItem, value))
                {
                    // 左右リストの片方しか選択できないようにする
                    if (value != null) RightSelectedItem = null;
                }
            }
        }
        private FluidData _leftSelectedItem;

        public FluidData RightSelectedItem
        {
            get => _rightSelectedItem;
            set
            {
                if (SetProperty(ref _rightSelectedItem, value))
                {
                    // 左右リストの片方しか選択できないようにする
                    if (value != null) LeftSelectedItem = null;
                }
            }
        }
        private FluidData _rightSelectedItem;

        public void MoveToRight()
        {
            if (LeftSelectedItem is null) return;
            RightItems.Add(LeftSelectedItem);
            LeftItems.Remove(LeftSelectedItem);
        }

        public void MoveToLeft()
        {
            if (RightSelectedItem is null) return;
            LeftItems.Add(RightSelectedItem);
            RightItems.Remove(RightSelectedItem);
        }
    }

    class FluidData
    {
        public string Name { get; }
        public Brush Brush { get; }
        public FluidData(string n, Brush b) { Name = n; Brush = b; }
    }

}