using Accessibility;
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
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class ListView2Page : ContentControl
    {
        public ListView2Page()
        {
            InitializeComponent();
        }
    }

    class ListView2ViewModel1 : MyBindableBase
    {
        private const int ItemCountMax = 50;

        private static readonly IEnumerable<ListView2Item> _source =
            Enumerable.Range(0, ItemCountMax).Select(x => new ListView2Item(x));

        public ObservableCollection<ListView2Item> SourceItems { get; } =
            new ObservableCollection<ListView2Item>(_source);

        public IList<int> SelectableIndex { get; } = Enumerable.Range(0, ItemCountMax).ToList();

        public ICommand ToggleIsSelectedCommand =>
            _selectItemCommand ??= new MyCommand<int>(index => ToggleIsSelected(SourceItems, index));
        private ICommand _selectItemCommand;

        private static void ToggleIsSelected(IList<ListView2Item> list, int index)
        {
            if (index > list.Count - 1) return;
            list[index].IsSelected = !list[index].IsSelected;
        }
    }

    class ListView2ViewModel2 : ListView2ViewModel1
    {
        public ObservableCollection<object> NotifySelectedViewItems { get; } =
            new ObservableCollection<object>();

        public ListView2ViewModel2()
        {
            // ◆ちゃんと使う場合は Dispose で remove しましょう
            NotifySelectedViewItems.CollectionChanged += NotifySelectedViewItems_CollectionChanged;
        }

        private static void NotifySelectedViewItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (ListView2Item item in e.NewItems)
                    {
                        item.IsSelected = true;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (ListView2Item item in e.OldItems)
                    {
                        item.IsSelected = false;
                    }
                    break;
                default:
                    Debug.Assert(true, e.Action.ToString());
                    break;
            }
        }
    }

    class ListView2Item : MyBindableBase
    {
        public string Name { get; }
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
        private bool _isSelected;
        public ListView2Item(int x) => Name = $"{x:D2}";
    }
}
