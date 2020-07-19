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
                nameof(IndexProperty), typeof(int), typeof(MovableRectangle2), new FrameworkPropertyMetadata(0));

        public int Index
        {
            get => (int)GetValue(IndexProperty);
            set => SetValue(IndexProperty, value);
        }

        public static readonly DependencyProperty CanvasWidthMaxProperty =
            DependencyProperty.Register(
                nameof(CanvasWidthMax), typeof(double), typeof(MovableRectangle2), new FrameworkPropertyMetadata(double.PositiveInfinity));

        public double CanvasWidthMax
        {
            get => (double)GetValue(CanvasWidthMaxProperty);
            set => SetValue(CanvasWidthMaxProperty, value);
        }

        public static readonly DependencyProperty CanvasHeightMaxProperty =
            DependencyProperty.Register(
                nameof(CanvasHeightMax), typeof(double), typeof(MovableRectangle2), new FrameworkPropertyMetadata(double.PositiveInfinity));

        public double CanvasHeightMax
        {
            get => (double)GetValue(CanvasHeightMaxProperty);
            set => SetValue(CanvasHeightMaxProperty, value);
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

            var parentCanvasPos = this.GetCanvasLeftTop();
            var oldThumbCanvasPos = thumb.GetCanvasLeftTop();

            var leftMin = -parentCanvasPos.X;
            var leftMax = CanvasWidthMax - parentCanvasPos.X;
            var newLeft = Clamp(oldThumbCanvasPos.X + e.HorizontalChange, leftMin, leftMax);

            var topMin = -parentCanvasPos.Y;
            var topMax = CanvasHeightMax - parentCanvasPos.Y;
            var newTop = Clamp(oldThumbCanvasPos.Y + e.VerticalChange, topMin, topMax);

            thumb.SetCanvasLeftTop(newLeft, newTop);

            UpdatePolygonPoints();

            e.Handled = true;
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
            private readonly Vector _baseAddress;

            public DragMove(UIElement ui) =>
                (_basePoint, _baseAddress) = (GetCurrentMousePosition(ui), (Vector)ui.GetCanvasLeftTop());

            private Point GetCurrentMousePosition(DependencyObject d) => Mouse.GetPosition(Window.GetWindow(d));

            public Vector GetNewAddress(DependencyObject d)
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

            var leftTop = _dragMove.Value.GetNewAddress(this);
            var parentCanvasPos = this.GetCanvasLeftTop();

            // 四隅座標を取得
            var cornersLeft = _cornerThumbs.Select(x => x.GetCanvasLeft()).ToArray();
            var cornerLeftMin = parentCanvasPos.X + cornersLeft.Min();
            var cornerLeftMax = parentCanvasPos.X + cornersLeft.Max();
            var cornersTop = _cornerThumbs.Select(x => x.GetCanvasTop()).ToArray();
            var cornerTopMin = parentCanvasPos.Y + cornersTop.Min();
            var cornerTopMax = parentCanvasPos.Y + cornersTop.Max();

            var leftMin = parentCanvasPos.X - cornerLeftMin;
            var leftMax = parentCanvasPos.X + (CanvasWidthMax - cornerLeftMax);
            var newLeft = Clamp(leftTop.X, leftMin, leftMax);

            var topMin = parentCanvasPos.Y - cornerTopMin;
            var topMax = parentCanvasPos.Y + (CanvasHeightMax - cornerTopMax);
            var newTop = Clamp(leftTop.Y, topMin, topMax);

            this.SetCanvasLeftTop(newLeft, newTop);
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