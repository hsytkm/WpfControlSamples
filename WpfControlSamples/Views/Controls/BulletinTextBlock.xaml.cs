using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfControlSamples.Views.Controls
{
    /// <summary>
    /// 電光掲示板のようなテキスト表示コントロール
    /// </summary>
    public partial class BulletinTextBlock : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(BulletinTextBlock),
                new FrameworkPropertyMetadata("", (sender, e) =>((BulletinTextBlock)sender).OnTextPropertyChanged((string)e.NewValue)));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private void OnTextPropertyChanged(string newText) => ((BulletinTextBlockViewModel)DataContext).TextQueue.Enqueue(newText);

        public BulletinTextBlock()
        {
            DataContext = new BulletinTextBlockViewModel();
            InitializeComponent();
        }
    }

    /// <summary>ObservableCollection通知用のViewModel</summary>
    class BulletinTextBlockViewModel : INotifyPropertyChanged
    {
#pragma warning disable CS0067
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore

        public ObservableTextQueue TextQueue { get; } = new();
    }

    /// <summary>電光掲示板のような文字列表示用のコレクションキュー</summary>
    sealed class ObservableTextQueue : ObservableCollection<string>
    {
        // 3要素の中央を View に表示する想定
        private const int DefaultCapacity = 3;
        private readonly int _capacity;

        public ObservableTextQueue(int capacity = DefaultCapacity)
        {
            if (capacity <= 1) throw new ArgumentException("capacity is 2 or over.", nameof(capacity));
            _capacity = capacity;

            // 空文字でキューを埋めておく
            for (int i = 0; i < capacity; i++) this.Add("");
        }

        /// <summary>
        /// 表示文字列を追加する
        /// <para>
        /// コレクションに新規追加した文字列は FluidMoveBehavior がアニメーションしてくれず、
        /// またコレクションから削除される文字列も FluidMoveBehavior がアニメーションしてくれない。
        /// 仕方ないので、既存後方キューに文字列をを上書きしてからエンキューにより押し込んでアニメーションさせる。
        /// </para>
        /// </summary>
        /// <param name="text"></param>
        public void Enqueue(string text)
        {
            if (this.Count != _capacity) throw new NotSupportedException();

            // 最後方のキュー値を入力文字列で更新
            this[_capacity - 1] = text;

            // デキュー（先頭要素の削除）により入力文字列を移動させる
            this.RemoveAt(0);

            // 空文字をエンキューして capacity まで要素を保持させる
            this.Add("");
        }
    }

    // BulletinTextPage (ObservableTextQueue) 専用
    [ValueConversion(typeof(double), typeof(Thickness))]
    class HeightToMinusHeightMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not double d) throw new NotSupportedException();
            return new Thickness(0, -d, 0, 0);      // 引数の長さ分、上にめり込ませる
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
