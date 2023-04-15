using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WpfControlSamples.ViewModels;

/// <summary>
/// ObservableCollection&lt;T&gt; と SelectedItem をペアで管理します。
/// コレクション要素が変化しない場合は ReadOnlyCollectionWithSelectedItem を使用してください。
/// </summary>
public sealed class ObservableCollectionWithSelectedItem<T> : ObservableCollection<T>
    where T : class?    // struct で要素が存在しない場合の挙動を未規定なので(defaultの扱い)
{
    public T? SelectedItem
    {
        get => _selectedItem;
        set
        {
            bool needToUpdateValue = (_selectedItem, value) switch
            {
                (null, null) => false,  // 変化なく更新不要
                (not null, not null) => !EqualityComparer<T>.Default.Equals(_selectedItem, value),   // 同値なら更新不要
                _ => true               // 異なるので更新
            };

            if (needToUpdateValue)
            {
                _selectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedItem)));
            }
        }
    }
    private T? _selectedItem;

    public ObservableCollectionWithSelectedItem() : base() { }
    public ObservableCollectionWithSelectedItem(List<T> items) : base(items) { }
    public ObservableCollectionWithSelectedItem(IEnumerable<T> collection) : base(collection) { }

    protected override void ClearItems()
    {
        SelectedItem = null;
        base.ClearItems();
    }

    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);

        // 初回追加のみ選択を更新します
        SelectedItem ??= this[0];
    }

    protected override void RemoveItem(int index)
    {
        // 選択中のアイテムが削除される場合は、選択アイテムを変更します
        int selectedIndex = SelectedItem is not null ? this.IndexOf(SelectedItem) : -1;
        if (index == selectedIndex)
        {
            SelectedItem = (this.Count, selectedIndex) switch
            {
                (<= 1, _) => null,                      // 削除で候補がなくなる場合は未選択にします
                (_, > 0) => this[selectedIndex - 1],    // 選択中が先頭要素でなけば1つ前の要素を選択します
                (_, _) => this[selectedIndex + 1],      // 1つ後の要素を選択します(ここまでの条件で後方存在が保障されるはず)
            };
        }
        base.RemoveItem(index);
    }

    public void AddRange(IEnumerable<T> items)
    {
        foreach (T item in items)
            this.Add(item);
    }
}

public static class ObservableCollectionWithSelectedItem
{
    public static ObservableCollectionWithSelectedItem<T> Create<T>(List<T> items) where T : class? => new(items);
    public static ObservableCollectionWithSelectedItem<T> Create<T>(IEnumerable<T> collection) where T : class? => new(collection);
}
