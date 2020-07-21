using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlSamples.Extensions;

namespace WpfControlSamples.Views.Menus
{
    public partial class MovableRectangle2 : UserControl
    {
        #region DependencyProperty
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register(
                nameof(IndexProperty), typeof(int), typeof(MovableRectangle2),
                new FrameworkPropertyMetadata(0));

        public int Index
        {
            get => (int)GetValue(IndexProperty);
            set => SetValue(IndexProperty, value);
        }

        public static readonly DependencyProperty CanvasWidthMaxProperty =
            DependencyProperty.Register(
                nameof(CanvasWidthMax), typeof(double), typeof(MovableRectangle2),
                new FrameworkPropertyMetadata(double.PositiveInfinity,
                    (sender, e) => ((MovableRectangle2)sender).OnCanvasSizeMaxChanged(widthChanged: (double)e.NewValue - (double)e.OldValue)),
                new ValidateValueCallback(IsValidLength));

        public double CanvasWidthMax
        {
            get => (double)GetValue(CanvasWidthMaxProperty);
            set => SetValue(CanvasWidthMaxProperty, value);
        }

        public static readonly DependencyProperty CanvasHeightMaxProperty =
            DependencyProperty.Register(
                nameof(CanvasHeightMax), typeof(double), typeof(MovableRectangle2),
                new FrameworkPropertyMetadata(double.PositiveInfinity,
                    (sender, e) => ((MovableRectangle2)sender).OnCanvasSizeMaxChanged(heightChanged: (double)e.NewValue - (double)e.OldValue)),
                new ValidateValueCallback(IsValidLength));

        public double CanvasHeightMax
        {
            get => (double)GetValue(CanvasHeightMaxProperty);
            set => SetValue(CanvasHeightMaxProperty, value);
        }

        // 検証コールバックで正数チェック
        private static bool IsValidLength(object value) => (value is double d) && d > 0;

        // Canvasサイズの変化時の処理
        private void OnCanvasSizeMaxChanged(double widthChanged = 0, double heightChanged = 0)
        {
            if (double.IsInfinity(widthChanged) || double.IsInfinity(heightChanged)) return;

            ShiftPolygonPositionLocal();

            foreach (var thumb in _cornerThumbs)
            {
                LimitThumbPosition(thumb);
            }

            // Canvasが大きい場合は枠を水平/垂直に移動（枠移動量0を要求して更新)
            void ShiftPolygonPositionLocal() => ShiftPolygonPosition(new Vector(0, 0));

            // Canvasが小さい場合は枠を押し潰し（点移動量0を要求して更新）
            void LimitThumbPosition(Thumb thumb) => ChangeThumbPosition(thumb, horizontalChange: 0, verticalChange: 0);
        }
        #endregion

        private readonly Thumb[] _cornerThumbs;
        private bool _canControl;

        public MovableRectangle2()
        {
            InitializeComponent();

            _cornerThumbs = new[] { thumb0, thumb1, thumb2, thumb3 };
            _canControl = true;     // 操作可能で初期化

            this.Loaded += MovableRectangle2_Loaded;
        }

        private void MovableRectangle2_Loaded(object sender, RoutedEventArgs e)
        {
            // 連番をタイトルにしとく
            titleTextBlock.Text = Index.ToString();

            // Windowsのように画面ごとに初期位置をずらす
            var offset = 10.0 * (Index % 5);
            var width = 100.0;
            var height = 100.0;

            var leftTops = new[]
            {
                new Point(offset, offset),
                new Point(offset + width, offset),
                new Point(offset + width, offset + height),
                new Point(offset, offset + height),
            };

            for (int i = 0; i < leftTops.Length; ++i)
            {
                var leftTop = leftTops[i];
                var thumb = _cornerThumbs[i];

                thumb.SetCanvasLeftTop(leftTop.X, leftTop.Y);
            }

            UpdatePolygonPoints();
            UpdateCornerThumbsVisibility(_canControl);
        }

        #region Thumb
        private void CornerThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!(sender is Thumb thumb)) return;

            ChangeThumbPosition(thumb, e.HorizontalChange, e.VerticalChange);
            e.Handled = true;
        }

        private void ChangeThumbPosition(Thumb thumb, double horizontalChange, double verticalChange)
        {
            if (thumb is null) throw new ArgumentNullException(nameof(thumb));

            var parentCanvasPos = this.GetCanvasLeftTop();
            var oldThumbCanvasPos = thumb.GetCanvasLeftTop();

            var leftMin = -parentCanvasPos.X;
            var leftMax = CanvasWidthMax - parentCanvasPos.X;
            var newLeft = Clamp(oldThumbCanvasPos.X + horizontalChange, leftMin, leftMax);

            var topMin = -parentCanvasPos.Y;
            var topMax = CanvasHeightMax - parentCanvasPos.Y;
            var newTop = Clamp(oldThumbCanvasPos.Y + verticalChange, topMin, topMax);

            thumb.SetCanvasLeftTop(newLeft, newTop);

            UpdatePolygonPoints();
        }

        private void UpdatePolygonPoints()
        {
            var points = _cornerThumbs.Select(x => x.GetCanvasLeftTop());

            // ◆めくられ対策は一旦無効
            //var sortedPoints = SortQuadranglePoints(points);

            polygon.Points = new PointCollection(points);

            // タイトルの表示位置は一番左の隅を基準にする
            var leftTopCorner = _cornerThumbs.Select(x => x.GetCanvasLeftTop()).OrderBy(x => x.X).First();
            titlePanel.SetCanvasLeftTop(leftTopCorner);
        }

        // 枠点をめくられないよう並べ替える(面積が最大を採用する実装)
        private static Point[] SortQuadranglePoints(IEnumerable<Point> points)
        {
            return Enumerate(points)
                .OrderByDescending(x => CalcQuadrangleArea(x))
                .First();

            // 全ての要素を使った順列を求める
            // https://qiita.com/gushwell/items/8780fc2b71f2182f36ac
            static IEnumerable<T[]> Enumerate<T>(IEnumerable<T> items)
            {
                if (items.Count() == 1)
                {
                    yield return new T[] { items.First() };
                    yield break;
                }
                foreach (var item in items)
                {
                    var leftside = new T[] { item };
                    var unused = items.Except(leftside);
                    foreach (var rightside in Enumerate(unused))
                    {
                        yield return leftside.Concat(rightside).ToArray();
                    }
                }
            }

            // 四点から面積を求める(座標法)
            // https://ja.wikipedia.org/wiki/%E5%BA%A7%E6%A8%99%E6%B3%95
            static double CalcQuadrangleArea(Point[] ps)
            {
                if (ps.Length != 4) throw new ArgumentException(nameof(ps.Length));  // 四角形以外は想定外

                return (ps[0].X * ps[1].Y - ps[1].X * ps[0].Y
                      + ps[1].X * ps[2].Y - ps[2].X * ps[1].Y
                      + ps[2].X * ps[3].Y - ps[3].X * ps[2].Y
                      + ps[3].X * ps[0].Y - ps[0].X * ps[3].Y) / 2.0;
            }
        }
        #endregion

        #region SelfDragMove

        private struct DragMove
        {
            private readonly Point _basePoint;
            private readonly Point _baseAddress;

            public DragMove(UIElement ui) =>
                (_basePoint, _baseAddress) = (GetCurrentMousePosition(ui), ui.GetCanvasLeftTop());

            private Point GetCurrentMousePosition(DependencyObject d) => Mouse.GetPosition(Window.GetWindow(d));

            public Point GetNewAddress(DependencyObject d)
            {
                var newPoint = GetCurrentMousePosition(d);
                var shift = newPoint - _basePoint;
                return _baseAddress + shift;
            }
        }
        private DragMove? _dragMove = null;

        private void Polygon_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!_canControl) return;

            if (!(sender is FrameworkElement self)) return;
            self.CaptureMouse();
            self.Cursor = Cursors.Hand;

            _dragMove = new DragMove(this);
        }

        private void Polygon_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is FrameworkElement self)) return;
            self.ReleaseMouseCapture();
            self.Cursor = Cursors.Arrow;

            _dragMove = null;
        }

        private void Polygon_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!_dragMove.HasValue) return;

            // 新座標を移動量に換算
            var newPos = _dragMove.Value.GetNewAddress(this);
            var currentPos = this.GetCanvasLeftTop();
            var moveVector = newPos - currentPos;

            ShiftPolygonPosition(moveVector);
        }

        private void ShiftPolygonPosition(Vector moveVector)
        {
            var parentCanvasPos = this.GetCanvasLeftTop();
            var newPos = parentCanvasPos + moveVector;

            // 枠の四隅座標を親Canvas基準に変換
            var cornerRect = Rect.Offset(GetCornerRect(), (Vector)parentCanvasPos);

            var leftMin = parentCanvasPos.X - cornerRect.Left;
            var leftMax = parentCanvasPos.X + (CanvasWidthMax - cornerRect.Right);
            var newLeft = Clamp(newPos.X, leftMin, leftMax);

            var topMin = parentCanvasPos.Y - cornerRect.Top;
            var topMax = parentCanvasPos.Y + (CanvasHeightMax - cornerRect.Bottom);
            var newTop = Clamp(newPos.Y, topMin, topMax);

            this.SetCanvasLeftTop(newLeft, newTop);

            // 枠の四隅座標を取得
            Rect GetCornerRect()
            {
                var cornersLeft = _cornerThumbs.Select(x => x.GetCanvasLeft()).ToArray();
                var cornerLeftMin = cornersLeft.Min();
                var cornerLeftMax = cornersLeft.Max();
                var cornersTop = _cornerThumbs.Select(x => x.GetCanvasTop()).ToArray();
                var cornerTopMin = cornersTop.Min();
                var cornerTopMax = cornersTop.Max();

                return new Rect(new Point(cornerLeftMin, cornerTopMin), new Point(cornerLeftMax, cornerTopMax));
            }
        }
        #endregion

        private static double Clamp(double self, double min, double max) =>
            Math.Max(min, Math.Min(max, self));

        private void UpdateCornerThumbsVisibility(bool isVisible)
        {
            foreach (var thumb in _cornerThumbs)
            {
                thumb.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _canControl = !_canControl;
            UpdateCornerThumbsVisibility(_canControl);
        }

    }
}