using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WpfControlSamples.ViewModels
{
    /// <summary>
    /// ObservableCollection&lt;T&gt; と SelectedItem をペアで管理する
    /// （where T : struct の動作が自信ない。 T? をちゃんと理解してない）
    /// </summary>
    sealed class ObservableCollectionSelectedItemPair<T> : ObservableCollection<T>
    {
        public ObservableCollection<T> Collection => this;

        public T? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem is null && value is null) return;
                if (_selectedItem?.Equals(value) ?? false) return;

                _selectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedItem)));
            }
        }
        private T? _selectedItem;

        public ObservableCollectionSelectedItemPair() { }
        public ObservableCollectionSelectedItemPair(IEnumerable<T> items) => AddRange(items);

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                base.Add(item);
        }

        protected override void ClearItems()
        {
            SelectedItem = default;
            base.ClearItems();
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);

            if (SelectedItem is null)
                SelectedItem = this[0];
        }

        protected override void RemoveItem(int index)
        {
            if (SelectedItem is not null)
            {
                var selectedIndex = IndexOf(SelectedItem);
                if (index == selectedIndex)
                {
                    SelectedItem = Count <= 1
                        ? default
                        : (0 < selectedIndex)
                            ? this[selectedIndex - 1]
                            : this[selectedIndex + 1];
                }
            }
            base.RemoveItem(index);
        }
    }

    static class ObservableCollectionSelectedItemPair
    {
        public static ObservableCollectionSelectedItemPair<T> Create<T>(IEnumerable<T> items) => new(items);
    }
}
