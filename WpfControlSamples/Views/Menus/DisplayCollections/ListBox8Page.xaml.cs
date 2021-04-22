using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class ListBox8Page : ContentControl
    {
        public ListBox8Page()
        {
            InitializeComponent();
            DataContext = new ListBox8ViewModel();
        }
    }

    class ListBox8ViewModel : MyBindableBase
    {
        private readonly ListBox8Model _model = new();
        public ObservableCollection<ListBox8Item> ItemsSource { get; } = new();

        public ICommand UpdateCommand => _updateCommand ??= new MyCommand(() =>
        {
            _model.Update();
            var items = _model.NameValuePairs.Select(x => new ListBox8Item(x.name, x.value));
            ItemsSource.AddOrUpdateRange(items);
        });
        private ICommand _updateCommand;

        public ListBox8ViewModel() => UpdateCommand.Execute(null);
    }

    class ListBox8Item : MyBindableBase, IUpdatableItem<ListBox8Item>
    {
        public string Name { get; }
        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
        private string _value;
        public ListBox8Item(string name, string value) => (Name, Value) = (name, value);

        /// <summary>プロパティキーを比較します</summary>
        public bool IsSameKey(ListBox8Item other) => Name == other.Name;

        /// <summary>プロパティ値を更新します</summary>
        public void UpdateValue(ListBox8Item other) => Value = other.Value;
    }

    class ListBox8Model : MyBindableBase
    {
        public ObservableCollection<(string name, string value)> NameValuePairs { get; } = new();
        private int _counter;
        private readonly Random _random = new();

        public void Update()
        {
            NameValuePairs.Clear();

            var index = _random.Next(Models.SampleData.WpfColors.Count);
            NameValuePairs.Add(("Index", "0 (fix)"));
            NameValuePairs.Add(("Counter", (++_counter).ToString()));
            NameValuePairs.Add(("Random", index.ToString()));
            NameValuePairs.Add(("Color", Models.SampleData.WpfColors[index].Name));
        }
    }
}
