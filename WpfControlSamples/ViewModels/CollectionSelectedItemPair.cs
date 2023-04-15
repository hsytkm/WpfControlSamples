#if false   // ReadOnlyCollectionWithSelectedItem を使用する方が xaml がスッキリすると思います。
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;

namespace WpfControlSamples.ViewModels
{
    /// <summary>
    /// IImmutableList&lt;T&gt; と SelectedItem をペアで管理する。
    /// コレクションが変化しない場合に使用する（変化する場合は ObservableCollectionSelectedItemPair）
    /// </summary>
    sealed class CollectionSelectedItemPair<T> : INotifyPropertyChanged
    {
        public IImmutableList<T> Collection { get; }

        public T? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem is null && value is null) return;
                if (_selectedItem?.Equals(value) ?? false) return;
                if (value is not null && Collection.FirstOrDefault<T>(x => EqualityComparer<T>.Default.Equals(value, x)) is null) return;

                _selectedItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
            }
        }
        private T? _selectedItem;

        public event PropertyChangedEventHandler? PropertyChanged;

        public CollectionSelectedItemPair(IEnumerable<T> items)
        {
            Collection = ImmutableArray.CreateRange(items);

            if (Collection.Count == 0) throw new ArgumentException("items is empty.");
            SelectedItem = Collection[0];
        }
    }

    static class CollectionSelectedItemPair
    {
        public static CollectionSelectedItemPair<T> Create<T>(IEnumerable<T> items) => new(items);
    }
}
#endif
