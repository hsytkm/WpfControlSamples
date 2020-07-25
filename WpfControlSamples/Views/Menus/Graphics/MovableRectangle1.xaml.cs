using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WpfControlSamples.Views.Adorners;

namespace WpfControlSamples.Views.Menus
{
    public partial class MovableRectangle1 : Border
    {
        #region DependencyProperty
        public static readonly DependencyProperty CanvasWidthMaxProperty =
            DependencyProperty.Register(
                nameof(CanvasWidthMax), typeof(double), typeof(MovableRectangle1),
                new FrameworkPropertyMetadata(double.PositiveInfinity));

        public double CanvasWidthMax
        {
            get => (double)GetValue(CanvasWidthMaxProperty);
            set => SetValue(CanvasWidthMaxProperty, value);
        }

        public static readonly DependencyProperty CanvasHeightMaxProperty =
            DependencyProperty.Register(
                nameof(CanvasHeightMax), typeof(double), typeof(MovableRectangle1),
                new FrameworkPropertyMetadata(double.PositiveInfinity));

        public double CanvasHeightMax
        {
            get => (double)GetValue(CanvasHeightMaxProperty);
            set => SetValue(CanvasHeightMaxProperty, value);
        }
        #endregion

        public MovableRectangle1()
        {
            InitializeComponent();
        }

        private static double Clamp(double self, double min, double max) =>
            Math.Max(min, Math.Min(max, self));

        #region Thumb_Resize
        // リサイズハンドルのイベント処理
        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!(sender is Thumb thumb)) return;

            //サイズ変更の対象要素を取得する
            if (!(AdornedBy.GetAdornedElementFromTemplateChild(thumb) is FrameworkElement owner)) return;

            // サイズ変更(横)
            if (thumb.HorizontalAlignment != HorizontalAlignment.Center)
            {
                if (thumb.HorizontalAlignment == HorizontalAlignment.Left)
                {
                    var widthMin = owner.MinWidth;
                    var horiChange = Math.Min(e.HorizontalChange, owner.Width - widthMin);

                    var leftMax = CanvasWidthMax - widthMin;
                    var oldLeft = owner.GetCanvasLeft();
                    var newLeft = Clamp(oldLeft + horiChange, 0, leftMax);
                    Canvas.SetLeft(owner, newLeft);

                    var widthMax = Math.Min(CanvasWidthMax - newLeft, owner.MaxWidth);
                    owner.Width = Clamp(owner.Width - (newLeft - oldLeft), widthMin, widthMax);
                }
                else
                {
                    var widthMax = Math.Min(CanvasWidthMax - owner.GetCanvasLeft(), owner.MaxWidth);
                    owner.Width = Clamp(owner.Width + e.HorizontalChange, owner.MinWidth, widthMax);
                }
            }

            // サイズ変更(縦)
            if (thumb.VerticalAlignment != VerticalAlignment.Center)
            {
                if (thumb.VerticalAlignment == VerticalAlignment.Top)
                {
                    var heightMin = owner.MinHeight;
                    var vertChange = Math.Min(e.VerticalChange, owner.Height - heightMin);

                    var topMax = CanvasHeightMax - heightMin;
                    var oldTop = owner.GetCanvasTop();
                    var newTop = Clamp(oldTop + vertChange, 0, topMax);
                    Canvas.SetTop(owner, newTop);

                    var heightMax = Math.Min(CanvasHeightMax - newTop, owner.MaxHeight);
                    owner.Height = Clamp(owner.Height - (newTop - oldTop), heightMin, heightMax);
                }
                else
                {
                    var heightMax = Math.Min(CanvasHeightMax - owner.GetCanvasTop(), owner.MaxHeight);
                    owner.Height = Clamp(owner.Height + e.VerticalChange, owner.MinHeight, heightMax);
                }
            }

            //if (owner.RenderTransform is RotateTransform rotate)
            //{
            //    rotate.CenterX = owner.Width / 2.0;
            //    rotate.CenterY = owner.Height / 2.0;
            //}

            //Debug.WriteLine($"T={Canvas.GetTop(self):f2}, L={Canvas.GetLeft(self):f2}, W={self.Width:f2}, H={self.Height:f2}");

            e.Handled = true;
        }
        #endregion

        #region Thumb_Rotate
        private Point? _rotateStartPoint = null;
        private void RotateThumb_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is FrameworkElement thumb)) return;
            thumb.CaptureMouse();

            if (!(AdornedBy.GetAdornedElementFromTemplateChild(thumb) is FrameworkElement owner)) return;

            if (!(owner.RenderTransform is RotateTransform))
            {
                owner.RenderTransform = new RotateTransform();
            }

            var canvas = owner.FindVisualParent<Canvas>();
            if (canvas is null) return;
            _rotateStartPoint = Mouse.GetPosition(canvas);
        }

        private void RotateThumb_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is FrameworkElement thumb)) return;
            thumb.ReleaseMouseCapture();
            _rotateStartPoint = null;
        }

        private void RotateThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!_rotateStartPoint.HasValue) return;
            if (!(sender is Thumb thumb)) return;

            //サイズ変更の対象要素を取得する
            if (!(AdornedBy.GetAdornedElementFromTemplateChild(thumb) is FrameworkElement owner)) return;

            // ◆矩形回転時の中点が取得できていない…
            var selfCenterPos = new Point(owner.GetCanvasLeft() + owner.ActualWidth / 2.0, owner.GetCanvasTop() + owner.ActualHeight / 2.0);
            var p0 = selfCenterPos;
            var p1 = _rotateStartPoint.Value + new Vector(e.HorizontalChange, e.VerticalChange);
            var radian = CalcAngle(p1, p0);

            if (owner.RenderTransform is RotateTransform rotate)
            {
                rotate.CenterX = owner.ActualWidth / 2.0;
                rotate.CenterY = owner.ActualHeight / 2.0;
                rotate.Angle = ChopAngle(radian);
            }

            // 回転でAdonerの描画更新が発生しないので、所有者コントロールのサイズを変えることで更新させる。◆全然ダメ
            //owner.Width = owner.ActualWidth + 0.01;

            e.Handled = true;

            static double CalcAngle(in Point p1, in Point p2) =>
                (Math.Atan2((p2.Y - p1.Y), (p2.X - p1.X)) * 180d / Math.PI) - 90d;

            static double ChopAngle(double angle)
            {
                while (angle < 0) { angle += 360d; }
                while (angle > 360) { angle -= 360d; }
                return angle;
            }
        }

        #endregion

        #region SelfDragMove

        private struct DragMove
        {
            private readonly Point _basePoint;
            private readonly Vector _baseAddress;

            //public DragMove(Point point, Vector vec) => (_basePoint, _baseAddress) = (point, vec);

            public DragMove(UIElement d) =>
                (_basePoint, _baseAddress) = (GetCurrentMousePosition(d), (Vector)d.GetCanvasLeftTop());

            private static Point GetCurrentMousePosition(DependencyObject d) => Mouse.GetPosition(Window.GetWindow(d));

            public readonly Vector GetNewAddress(DependencyObject d)
            {
                var newPoint = GetCurrentMousePosition(d);
                var shift = newPoint - _basePoint;
                return _baseAddress + shift;
            }
        }
        private DragMove? _dragMove = null;

        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is FrameworkElement self)) return;
            self.CaptureMouse();
            self.Cursor = Cursors.Hand;

            _dragMove = new DragMove(self);
        }

        private void Border_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is FrameworkElement self)) return;
            self.ReleaseMouseCapture();
            self.Cursor = Cursors.Arrow;

            _dragMove = null;
        }

        private void Border_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!_dragMove.HasValue) return;
            if (!(sender is FrameworkElement self)) return;

            var leftTop = _dragMove.Value.GetNewAddress(self);
            var newLeft = Clamp(leftTop.X, 0, CanvasWidthMax - self.ActualWidth);
            var newTop = Clamp(leftTop.Y, 0, CanvasHeightMax - self.ActualHeight);

            self.SetCanvasLeftTop(newLeft, newTop);
        }
        #endregion

    }
}