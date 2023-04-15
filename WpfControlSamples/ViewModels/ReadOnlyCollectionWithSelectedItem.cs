using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WpfControlSamples.ViewModels;

/// <summary>
/// IReadOnlyList&lt;T&gt; と SelectedItem をペアで管理します。
/// コレクション要素が変化する場合は ObservableCollectionSelectedItemPair を使用してください。
/// </summary>
public sealed class ReadOnlyCollectionWithSelectedItem<T> : IReadOnlyList<T>, INotifyPropertyChanged
    where T : class?    // struct で要素が存在しない場合の挙動を未規定なので(defaultの扱い)
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly List<T> _items;

    // IEnumerable を継承しているのでxamlからインスタンスを直参照すれば動作します。
    public IReadOnlyList<T> Items => _items;

    public T? SelectedItem
    {
        get => _selectedItem;
        set
        {
            bool needToUpdateValue = (_selectedItem, value) switch
            {
                (null, null) => false,      // 変化なく更新不要
                (null, not null) => true,   // 初回更新
                (not null, null) => false,  // 新たに null は設定させない仕様なので更新不要
                (not null, not null) => !EqualityComparer<T>.Default.Equals(_selectedItem, value)    // 同値なら更新不要
            };

            if (needToUpdateValue && _items.Contains(value!))
            {
                _selectedItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
            }
        }
    }
    private T? _selectedItem;

    public int Count => _items.Count;

    public T this[int index] => _items[index];

    public ReadOnlyCollectionWithSelectedItem(List<T> items)
    {
        _items = items;
        SelectedItem = items.Count > 0 ? items[0] : null;
    }

    public ReadOnlyCollectionWithSelectedItem(IEnumerable<T> collection) : this(collection.ToList()) { }

    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public static class ReadOnlyCollectionWithSelectedItem
{
    public static ReadOnlyCollectionWithSelectedItem<T> Create<T>(List<T> items) where T : class? => new(items);
    public static ReadOnlyCollectionWithSelectedItem<T> Create<T>(IEnumerable<T> collection) where T : class? => new(collection);
}
