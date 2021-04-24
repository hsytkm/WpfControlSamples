#nullable disable
using System;
using System.Collections.Generic;
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
using WpfControlSamples.ViewModels;

namespace WpfControlSamples.Views.Menus
{
    public partial class ComboBox3Page : ContentControl
    {
        public ComboBox3Page()
        {
            InitializeComponent();
            DataContext = new ComboBox3ViewModel();
        }
    }

    class ComboBox3ViewModel : MyBindableBase
    {
        public IReadOnlyList<string> Colors { get; } = Models.SampleData.WpfColors.Select(x => x.Name).ToArray();

        public ICommand ShowValueCommand => new MyCommand<string>(x => MessageBox.Show($"Value = {x ?? "null"}"));

        public ObservableCollectionSelectedItemPair<string> CollectionItemPair { get; } = new();

        private int _counter;
        public ICommand AddFirstItemCommand => _addFirstItemCommand ??= new MyCommand(() =>
        {
            var color = Models.SampleData.WpfColors[_counter++ % Models.SampleData.WpfColors.Count];
            CollectionItemPair.Add(color.Name);
        });
        private ICommand _addFirstItemCommand;

        public ICommand DeleteFirstItemCommand => _deleteFirstItemCommand ??= new MyCommand(() =>
        {
            if (CollectionItemPair.Count <= 0) return;
            CollectionItemPair.RemoveAt(0);
        });
        private ICommand _deleteFirstItemCommand;

        public ICommand DeleteLastItemCommand => _deleteLastItemCommand ??= new MyCommand(() =>
        {
            if (CollectionItemPair.Count <= 0) return;
            CollectionItemPair.RemoveAt(CollectionItemPair.Count - 1);
        });
        private ICommand _deleteLastItemCommand;
        public ICommand DeleteAllItemsCommand => _deleteAllItemsCommand ??= new MyCommand(() =>
        {
            if (CollectionItemPair.Count <= 0) return;
            CollectionItemPair.Clear();
        });
        private ICommand _deleteAllItemsCommand;

    }
}
