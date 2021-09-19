using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Windows.Threading;

namespace WpfControlSamples.ViewModels
{
    /* マルチスレッド環境下でのコレクションの操作
     *  https://blog.okazuki.jp/entry/20100112/1263267397
     * 
     * 本クラスを使用しなくても以下でも対応できる。
     * WPF 4.5の新機能「複数スレッドからのコレクションの操作」
     *  https://blog.okazuki.jp/entry/20120520/1337503048
     */

    class DispatchObservableCollection<T> : ObservableCollection<T>
    {
        // CollectionChangedイベントを発行するときに使用するディスパッチャ
        public Dispatcher EventDispatcher { get; }

        public DispatchObservableCollection()
        {
            // インスタンスが作られた時のDispatcherを取得
            EventDispatcher = Dispatcher.CurrentDispatcher;
        }

        public DispatchObservableCollection(IEnumerable<T> collection)
            : base(collection)
        {
            // インスタンスが作られた時のDispatcherを取得
            EventDispatcher = Dispatcher.CurrentDispatcher;
        }

        public DispatchObservableCollection(List<T> list)
            : base(list)
        {
            // インスタンスが作られた時のDispatcherを取得
            EventDispatcher = Dispatcher.CurrentDispatcher;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (IsValidAccess())
            {
                // UIスレッドならそのまま実行
                base.OnCollectionChanged(e);
            }
            else
            {
                // UIスレッドじゃなかったらDispatcherにお願いする
                Action<NotifyCollectionChangedEventArgs> changed = OnCollectionChanged;
                EventDispatcher.Invoke(changed, e);
            }
        }

        // UIスレッドからのアクセスかどうかを判定する
        private bool IsValidAccess()
        {
            // Dispatcherが設定されていないときは、どうしようもないのでOKにしとく
            // Dispatcherが設定されていたら、今のスレッドとDispatcherのスレッドを見比べる
            return EventDispatcher is null || EventDispatcher.Thread == Thread.CurrentThread;
        }
    }
}
