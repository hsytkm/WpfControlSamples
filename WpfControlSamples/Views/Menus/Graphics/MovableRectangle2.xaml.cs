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
                nameof(Index), typeof(int), typeof(MovableRectangle2),
                new FrameworkPropertyMetadata(0));

        public int Index
        {
            get => (int)GetValue(IndexProperty);
            set => SetValue(IndexProperty, value);
        }

        public static readonly DependencyProperty BackgroundBrushProperty =
            DependencyProperty.Register(
                nameof(BackgroundBrush), typeof(Brush), typeof(MovableRectangle2),
                new FrameworkPropertyMetadata(Brushes.Red));

        public Brush BackgroundBrush
        {
            get => (Brush)GetValue(BackgroundBrushProperty);
            set => SetValue(BackgroundBrushProperty, value);
        }

        public static readonly DependencyProperty StrokeBrushProperty =
            DependencyProperty.Register(
                nameof(StrokeBrush), typeof(Brush), typeof(MovableRectangle2),
                new FrameworkPropertyMetadata(Brushes.Blue));

        public Brush StrokeBrush
        {
            get => (Brush)GetValue(StrokeBrushProperty);
            set => SetValue(StrokeBrushProperty, value);
        }

        public static readonly DependencyProperty CornerPointCollectionProperty =
            DependencyProperty.Register(
                nameof(CornerPointCollection), typeof(PointCollection), typeof(MovableRectangle2),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    (sender, e) => ((MovableRectangle2)sender).OnCornerPointsChanged((PointCollection)e.NewValue)));

        public PointCollection CornerPointCollection
        {
            get => (PointCollection)GetValue(CornerPointCollectionProperty);
            set => SetValue(CornerPointCollectionProperty, value);
        }

        // Canvasサイズの変化時の処理
        private void OnCornerPointsChanged(PointCollection points) =>
            polygon.Points = new PointCollection(points);

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

        // 検証コールバックで念のため負数除外
        private static bool IsValidLength(object value) => (value is double d) && d >= 0;

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
        private UIElement _baseControlOnCanvas;

        public MovableRectangle2()
        {
            InitializeComponent();

            _cornerThumbs = new[] { thumb0, thumb1, thumb2, thumb3 };
            _canControl = true;     // 操作可能で初期化

            this.Loaded += MovableRectangle2_Loaded;
        }

        private void MovableRectangle2_Loaded(object sender, RoutedEventArgs e)
        {
            // ItemsControl内に配置されてたら(ItemsSourceなら) ContentPresenter を操作し、
            // そうでなければ(Canvas下に直で定義されてたら)自身のCanvas.Left(Top)を操作する。
            var parent = VisualTreeHelper.GetParent(this);
            _baseControlOnCanvas = (parent is ContentPresenter presenter) ? (UIElement)presenter : (UIElement)this;

            // 連番をタイトルにしとく
            titleTextBlock.Text = Index.ToString();

            // 枠の初期値を設定
            if (CornerPointCollection != null)
            {
                for (int i = 0; i < Math.Min(_cornerThumbs.Length, CornerPointCollection.Count); ++i)
                {
                    _cornerThumbs[i].SetCanvasLeftTop(CornerPointCollection[i]);
                }
            }
            else
            {
                // 初期の枠位置が外部から指定されなかった場合の対応（動作確認用）
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

            var baseCanvasPos = _baseControlOnCanvas.GetCanvasLeftTop();
            var oldThumbCanvasPos = thumb.GetCanvasLeftTop();

            var leftMin = -baseCanvasPos.X;
            var leftMax = CanvasWidthMax - baseCanvasPos.X;
            var newLeft = Clamp(oldThumbCanvasPos.X + horizontalChange, leftMin, leftMax);

            var topMin = -baseCanvasPos.Y;
            var topMax = CanvasHeightMax - baseCanvasPos.Y;
            var newTop = Clamp(oldThumbCanvasPos.Y + verticalChange, topMin, topMax);

            thumb.SetCanvasLeftTop(newLeft, newTop);

            UpdatePolygonPoints();
        }

        private void UpdatePolygonPoints()
        {
            var points = _cornerThumbs.Select(x => x.GetCanvasLeftTop());
            CornerPointCollection = new PointCollection(points);

            // タイトルの表示位置は一番左の隅を基準にする
            var leftTopCorner = _cornerThumbs.Select(x => x.GetCanvasLeftTop()).OrderBy(x => x.X).First();
            titlePanel.SetCanvasLeftTop(leftTopCorner);
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

            _dragMove = new DragMove(_baseControlOnCanvas);
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
            var newPos = _dragMove.Value.GetNewAddress(_baseControlOnCanvas);
            var currentPos = _baseControlOnCanvas.GetCanvasLeftTop();
            var moveVector = newPos - currentPos;

            ShiftPolygonPosition(moveVector);
        }

        private void ShiftPolygonPosition(Vector moveVector)
        {
            var parentCanvasPos = _baseControlOnCanvas.GetCanvasLeftTop();
            var newPos = parentCanvasPos + moveVector;

            // 枠の四隅座標を親Canvas基準に変換
            var cornerRect = Rect.Offset(GetCornerRect(), (Vector)parentCanvasPos);

            var leftMin = parentCanvasPos.X - cornerRect.Left;
            var leftMax = parentCanvasPos.X + (CanvasWidthMax - cornerRect.Right);
            var newLeft = Clamp(newPos.X, leftMin, leftMax);

            var topMin = parentCanvasPos.Y - cornerRect.Top;
            var topMax = parentCanvasPos.Y + (CanvasHeightMax - cornerRect.Bottom);
            var newTop = Clamp(newPos.Y, topMin, topMax);

            _baseControlOnCanvas.SetCanvasLeftTop(newLeft, newTop);

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