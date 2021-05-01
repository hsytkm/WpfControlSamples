using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WpfControlSamples.ViewModels
{
    /// <summary>
    /// IReadOnlyList と SelectedItem をペアで管理する。
    /// コレクションが変化しない場合に使用する（変化する場合は ObservableCollectionSelectedItemPair）
    /// </summary>
    public sealed class CollectionSelectedItemPair<T> : INotifyPropertyChanged
    {
        public IReadOnlyList<T> Collection { get; }

        public T? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem is null && value is null) return;
                if (_selectedItem?.Equals(value) ?? false) return;

                _selectedItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
            }
        }
        private T? _selectedItem;

        public event PropertyChangedEventHandler? PropertyChanged;

        public CollectionSelectedItemPair(IEnumerable<T> items)
        {
            Collection = items.ToArray();

            if (Collection.Count == 0) throw new ArgumentException("items is empty.");
            SelectedItem = Collection[0];
        }
    }
}
