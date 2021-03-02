using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfControlSamples.Views.Behaviors
{
    // ドラッグ操作でゴーストを表示する http://yujiro15.net/blog/index.php?id=152
    class GhostAdornerHelper
    {
        /// <summary>
        /// IsEnable 添付プロパティの定義
        /// </summary>
        public static readonly DependencyProperty IsEnableProperty =
            DependencyProperty.RegisterAttached(
                "IsEnable", typeof(bool), typeof(GhostAdornerHelper),
                new PropertyMetadata(false, OnIsEnablePropertyChanged));

        /// <summary>
        /// IsEnable 添付プロパティを取得します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <returns>取得した値を返します。</returns>
        public static bool GetIsEnable(DependencyObject target)
            => (bool)target.GetValue(IsEnableProperty);

        /// <summary>
        /// IsEnable 添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetIsEnable(DependencyObject target, bool value)
            => target.SetValue(IsEnableProperty, value);

        /// <summary>
        /// IsEnable 添付プロパティ変更イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private static void OnIsEnablePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is UIElement element)) return;

            if (GetIsEnable(element))
            {
                element.PreviewMouseLeftButtonDown += Element_PreviewMouseLeftButtonDown;
                element.PreviewMouseMove += Element_PreviewMouseMove;
                element.PreviewMouseLeftButtonUp += Element_PreviewMouseLeftButtonUp;
            }
        }

        /// <summary>
        /// 装飾用コントロール
        /// </summary>
        private static GhostAdorner _adorner;

        /// <summary>
        /// PreviewMouseLeftButtonDown イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private static void Element_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is FrameworkElement adornedElement)) return;

            var parent = e.OriginalSource is FrameworkElement originalElement
                ? FindAncestor<Panel>(originalElement) : null;
            if (parent is null) return;

            var pt = e.GetPosition(adornedElement);
            var offset = new Point(-pt.X, -pt.Y);
            _adorner = new GhostAdorner(parent, adornedElement, pt, offset);

            adornedElement.CaptureMouse();
        }

        /// <summary>
        /// PreviewMouseLeftButtonUp イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private static void Element_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_adorner is null) return;

            _adorner.AdornedElement.ReleaseMouseCapture();
            _adorner.Detach();
            _adorner = null;
        }

        /// <summary>
        /// PreviewMouseMove イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private static void Element_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_adorner is null) return;

            if (_adorner.AdornedElement.IsMouseCaptured && (e.LeftButton == MouseButtonState.Pressed))
            {
                var pt = e.GetPosition(_adorner.AdornedElement);
                _adorner.CurrentPoint = pt;
            }
        }

        /// <summary>
        /// 指定された型の親要素を探します。
        /// </summary>
        /// <typeparam name="T">親要素の型を指定します。</typeparam>
        /// <param name="element">探索を開始する要素を指定します。</param>
        /// <returns>親要素を返します。</returns>
        private static T FindAncestor<T>(FrameworkElement element) where T : FrameworkElement
        {
            do
            {
                element = VisualTreeHelper.GetParent(element) as FrameworkElement;
                if (element is T t) return t;

            } while (element != null);
            return null;
        }
    }

    /// <summary>
    /// ゴーストを表示する装飾用コントロールを表します。
    /// </summary>
    class GhostAdorner : Adorner
    {
        /// <summary>
        /// 装飾層
        /// </summary>
        private readonly AdornerLayer _layer;

        /// <summary>
        /// アタッチされているかどうか
        /// </summary>
        private bool _isAttached;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="visual">装飾する要素を指定します。</param>
        /// <param name="adornedElement">装飾に用いる要素を指定します。</param>
        /// <param name="point">装飾を表示する位置を、装飾する要素に対する相対位置として指定します。</param>
        /// <param name="offset">装飾を表示する位置に対するオフセットを指定します。</param>
        public GhostAdorner(Visual visual, UIElement adornedElement, Point point, Point offset)
            : base(adornedElement)
        {
            _layer = AdornerLayer.GetAdornerLayer(visual);
            CurrentPoint = point;
            Offset = offset;

            Attach();
        }

        /// <summary>
        /// CurrentPoint 依存関係プロパティの定義
        /// </summary>
        public static readonly DependencyProperty CurrentPointProperty =
            DependencyProperty.Register(
                nameof(CurrentPoint), typeof(Point), typeof(GhostAdorner),
                new FrameworkPropertyMetadata(default(Point), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// ゴーストの表示位置を取得または設定します。
        /// </summary>
        public Point CurrentPoint
        {
            get => (Point)GetValue(CurrentPointProperty);
            set => SetValue(CurrentPointProperty, value);
        }

        /// <summary>
        /// Offset 依存関係プロパティの定義
        /// </summary>
        public static readonly DependencyProperty OffsetProperty =
            DependencyProperty.Register(
                nameof(Offset), typeof(Point), typeof(GhostAdorner),
                new FrameworkPropertyMetadata(default(Point), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// ゴーストの表示位置のオフセットを取得または設定します。
        /// </summary>
        public Point Offset
        {
            get => (Point)GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }

        /// <summary>
        /// アタッチします。
        /// </summary>
        public void Attach()
        {
            if (_layer is null) return;
            if (_isAttached) return;

            _layer.Add(this);
            _isAttached = true;
        }

        /// <summary>
        /// デタッチします。
        /// </summary>
        public void Detach()
        {
            if (_layer is null) return;
            if (!_isAttached) return;

            _layer.Remove(this);
            _isAttached = false;
        }

        /// <summary>
        /// 描画処理のオーバーライド
        /// </summary>
        /// <param name="drawingContext">描画先のコンテキストを指定します。</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            var pt = new Point(CurrentPoint.X + Offset.X, CurrentPoint.Y + Offset.Y);
            var rect = new Rect(pt, AdornedElement.RenderSize);
            var brush = new VisualBrush(AdornedElement)
            {
                Opacity = 0.3
            };

            drawingContext.DrawRectangle(brush, null, rect);
        }
    }
}
