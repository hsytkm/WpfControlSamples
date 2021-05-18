using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WpfControlSamples.ViewModels
{
    interface IUpdatableItem<T> where T : class
    {
        /// <summary>プロパティキーを比較します</summary>
        bool IsSameKey(T other);

        /// <summary>プロパティ値を更新します</summary>
        void UpdateValue(T other);
    }

    static class IUpdatableItemExtension
    {
        /// <summary>
        /// Key が同じ要素があれば Value を更新し、要素がなければ追加します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">追加/更新先のコレクション</param>
        /// <param name="items">追加/更新したいアイテム</param>
        public static void AddOrUpdateRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
            where T : class, IUpdatableItem<T>
        {
            foreach (var item in items)
                collection.AddOrUpdate(item);
        }

        /// <summary>
        /// Key が同じ要素があれば Value を更新し、要素がなければ追加します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">追加/更新先のコレクション</param>
        /// <param name="item">追加/更新したいアイテム</param>
        public static void AddOrUpdate<T>(this ObservableCollection<T> collection, T item)
            where T : class, IUpdatableItem<T>
        {
            var target = collection.FirstOrDefault(x => x.IsSameKey(item));
            if (target is null)
            {
                collection.Add(item);
            }
            else
            {
                target.UpdateValue(item);
            }
        }
    }
}
