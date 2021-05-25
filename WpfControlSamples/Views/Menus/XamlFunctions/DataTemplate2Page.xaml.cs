using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using WpfControlSamples.Models;

namespace WpfControlSamples.Views.Menus
{
    public partial class DataTemplate2Page : ContentControl
    {
        public DataTemplate2Page()
        {
            DataContext = new DataTemplate2ViewModel();
            InitializeComponent();
        }
    }

    class DataTemplate2ViewModel : MyBindableBase
    {
        private int _index;
        public ObservableCollection<DataTemplateChildViewModel> ChildViewModels { get; } = new();

        public ICommand AddViewModelCommand => _addViewModelCommand ??= new MyCommand(() =>
        {
            var source = SampleData.WpfColors[_index++];
            var vm = new DataTemplateChildViewModel(source.Name, source.Color);
            ChildViewModels.Add(vm);
        });
        private ICommand _addViewModelCommand = default!;

        public DataTemplate2ViewModel()
        {
            for (int i = 0; i < 3; i++)
                AddViewModelCommand.Execute(null);
        }

    }

    // View に渡して、自動で View を生成するための ViewModel
    class DataTemplateChildViewModel
    {
        public string Name { get; }
        public Brush BgBrush { get; }
        public DataTemplateChildViewModel(string name, Color color)
        {
            Name = name;
            BgBrush = color.ToFreezedSolidColorBrush();
        }

        // xaml用
        public DataTemplateChildViewModel() : this("Red", Colors.Red) { }
    }

}
