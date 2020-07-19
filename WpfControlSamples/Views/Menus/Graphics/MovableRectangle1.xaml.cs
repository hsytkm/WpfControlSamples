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
                nameof(CanvasWidthMax), typeof(double), typeof(MovableRectangle1), new FrameworkPropertyMetadata(double.PositiveInfinity));

        public double CanvasWidthMax
        {
            get => (double)GetValue(CanvasWidthMaxProperty);
            set => SetValue(CanvasWidthMaxProperty, value);
        }

        public static readonly DependencyProperty CanvasHeightMaxProperty =
            DependencyProperty.Register(
                nameof(CanvasHeightMax), typeof(double), typeof(MovableRectangle1), new FrameworkPropertyMetadata(double.PositiveInfinity));

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
            if (!(AdornedBy.GetAdornedElementFromTemplateChild(thumb) is FrameworkElement self)) return;

            // サイズ変更(横)
            if (thumb.HorizontalAlignment != HorizontalAlignment.Center)
            {
                if (thumb.HorizontalAlignment == HorizontalAlignment.Left)
                {
                    var widthMin = self.MinWidth;
                    var horiChange = Math.Min(e.HorizontalChange, self.Width - widthMin);

                    var leftMax = CanvasWidthMax - widthMin;
                    var oldLeft = self.GetCanvasLeft();
                    var newLeft = Clamp(oldLeft + horiChange, 0, leftMax);
                    Canvas.SetLeft(self, newLeft);

                    var widthMax = Math.Min(CanvasWidthMax - newLeft, self.MaxWidth);
                    self.Width = Clamp(self.Width - (newLeft - oldLeft), widthMin, widthMax);
                }
                else
                {
                    var widthMax = Math.Min(CanvasWidthMax - self.GetCanvasLeft(), self.MaxWidth);
                    self.Width = Clamp(self.Width + e.HorizontalChange, self.MinWidth, widthMax);
                }
            }

            // サイズ変更(縦)
            if (thumb.VerticalAlignment != VerticalAlignment.Center)
            {
                if (thumb.VerticalAlignment == VerticalAlignment.Top)
                {
                    var heightMin = self.MinHeight;
                    var vertChange = Math.Min(e.VerticalChange, self.Height - heightMin);

                    var topMax = CanvasHeightMax - heightMin;
                    var oldTop = self.GetCanvasTop();
                    var newTop = Clamp(oldTop + vertChange, 0, topMax);
                    Canvas.SetTop(self, newTop);

                    var heightMax = Math.Min(CanvasHeightMax - newTop, self.MaxHeight);
                    self.Height = Clamp(self.Height - (newTop - oldTop), heightMin, heightMax);
                }
                else
                {
                    var heightMax = Math.Min(CanvasHeightMax - self.GetCanvasTop(), self.MaxHeight);
                    self.Height = Clamp(self.Height + e.VerticalChange, self.MinHeight, heightMax);
                }
            }

            if (self.RenderTransform is RotateTransform rotate)
            {
                rotate.CenterX = self.Width / 2.0;
                rotate.CenterY = self.Height / 2.0;
            }

            //Debug.WriteLine($"T={Canvas.GetTop(self):f2}, L={Canvas.GetLeft(self):f2}, W={self.Width:f2}, H={self.Height:f2}");

            e.Handled = true;
        }
        #endregion

        #region Thumb_Rotate
        private Point? _startPoint = null;
        private void Thumb_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is FrameworkElement self)) return;
            self.CaptureMouse();

            var canvas = this.FindVisualParent<Canvas>();
            if (canvas is null) return;
            _startPoint = Mouse.GetPosition(canvas);

            if (!(this.RenderTransform is RotateTransform))
            {
                this.RenderTransform = new RotateTransform();
            }
        }

        private void Thumb_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is FrameworkElement self)) return;
            self.ReleaseMouseCapture();
            _startPoint = null;
        }

        private void RotateThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!_startPoint.HasValue) return;
            if (!(sender is Thumb thumb)) return;

            //サイズ変更の対象要素を取得する
            if (!(AdornedBy.GetAdornedElementFromTemplateChild(thumb) is FrameworkElement self)) return;

            // ◆矩形回転時の中点が取得できていない…
            var selfCenterPos = new Point(self.GetCanvasLeft() + self.ActualWidth / 2.0, self.GetCanvasTop() + self.ActualHeight / 2.0);

            var changePos = new Point(-e.HorizontalChange, -e.VerticalChange);

            var p1 = new Point(0, 0);
            var p2 = (selfCenterPos - _startPoint.Value) + changePos; 
            var radian = CalcAngle(p1, p2);

            //Debug.WriteLine($"P1={p1:f1}, P2={p2:f1}, Center={selfCenterPos:f1}, Cursor={cursorPos:f1}, Angle={radian:f1}, Delta={e.HorizontalChange:f1}, {e.VerticalChange:f1}");

            if (self.RenderTransform is RotateTransform rotate)
            {
                rotate.CenterX = self.ActualWidth / 2.0;
                rotate.CenterY = self.ActualHeight / 2.0;
                rotate.Angle = radian;
            }

            e.Handled = true;

            static double CalcAngle(in Point p1, in Point p2) =>
                Math.Atan2((p2.Y - p1.Y), (p2.X - p1.X)) * (180 / Math.PI) - 90.0;
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

            private Point GetCurrentMousePosition(DependencyObject d) => Mouse.GetPosition(Window.GetWindow(d));

            public Vector GetNewAddress(DependencyObject d)
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