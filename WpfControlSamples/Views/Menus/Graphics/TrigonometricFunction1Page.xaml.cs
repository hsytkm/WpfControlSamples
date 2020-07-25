using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    static class AttachedMessage
    {
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.RegisterAttached(
                "Message", typeof(string), typeof(AttachedMessage));

        public static string GetMessage(UIElement element) =>
            (string)element.GetValue(MessageProperty);

        public static void SetMessage(UIElement element, string value) =>
            element.SetValue(MessageProperty, value);
    }

    public partial class TrigonometricFunction1Page : ContentControl
    {
        private readonly TrigonometricFunction1ViewModel _viewModel = new TrigonometricFunction1ViewModel();

        private Canvas _parentCanvas;
        private Thumb[] _oneDimThumbs;
        private Thumb[] _twoDimThumbs;

        public TrigonometricFunction1Page()
        {
            InitializeComponent();
            DataContext = _viewModel;

            this.Loaded += (_, __) =>
            {
                _oneDimThumbs = new[] { point1_0, point1_1 };
                _twoDimThumbs = new[] { point2_0, point2_1, point2_2 };
                _parentCanvas = GetParentCanvas(originPoint);
                _parentCanvas.SizeChanged += (_, __) => UpdateViewPoints();

                UpdateViewPoints();
            };
        }

        private static double Clamp(double self, double min, double max) => Math.Max(min, Math.Min(max, self));

        private static Canvas GetParentCanvas(DependencyObject d)
        {
            var canvas = d.FindVisualParent<Canvas>();
            if (canvas is null) throw new KeyNotFoundException();
            return canvas;
        }

        private static Size GetActualSize(FrameworkElement fe) => new Size(fe.ActualWidth, fe.ActualHeight);
        
        private static void SetLineXY(Line line, Point p0, Point p1)
        {
            line.X1 = p0.X;
            line.Y1 = p0.Y;
            line.X2 = p1.X;
            line.Y2 = p1.Y;
        }

        // 各点(Thumbs)更新時の処理
        private void UpdateViewPoints()
        {
            var canvasSize = GetActualSize(_parentCanvas);

            UpdateBasePoint(canvasSize);
            UpdateOneDimension(canvasSize);
            UpdateTwoDimension(canvasSize);
        }

        // 原点の更新
        private void UpdateBasePoint(in Size canvasSize)
        {
            originPoint.SetCanvasLeftTop(0, canvasSize.Height);
        }

        // ViewModelに通知する座標は原点を変える(View:左上, ViewModel:右下)
        private static Point ToViewModelCoordinate(Point p, double yoffset) => new Point(p.X, yoffset - p.Y);

        // 1次元コントロールの更新
        private void UpdateOneDimension(Size canvasSize)
        {
            var points = _oneDimThumbs.Select(t => t.GetCanvasLeftTop())
                .Select(p => new Point(Clamp(p.X, 0, canvasSize.Width), Clamp(p.Y, 0, canvasSize.Height))).ToArray();

            for (int i = 0; i < _oneDimThumbs.Length; ++i)
            {
                _oneDimThumbs[i].SetCanvasLeftTop(points[i]);
                _viewModel.OneDimPoints[i] = ToViewModelCoordinate(points[i], canvasSize.Height);
            }

            SetLineXY(line1, points[0], points[1]);
        }

        // 2次元コントロールの更新
        private void UpdateTwoDimension(Size canvasSize)
        {
            var points = _twoDimThumbs.Select(t => t.GetCanvasLeftTop())
                .Select(p => new Point(Clamp(p.X, 0, canvasSize.Width), Clamp(p.Y, 0, canvasSize.Height))).ToArray();

            for (int i = 0; i < _twoDimThumbs.Length; ++i)
            {
                _twoDimThumbs[i].SetCanvasLeftTop(points[i]);
                _viewModel.TwoDimPoints[i] = ToViewModelCoordinate(points[i], canvasSize.Height);
            }

            SetLineXY(line2_0, points[0], points[1]);
            SetLineXY(line2_1, points[1], points[2]);
            SetLineXY(line2_2, points[2], points[0]);
        }

        // 点のドラッグシフト
        private void PointThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!(sender is Thumb thumb)) return;
            ChangeThumbPosition(_parentCanvas, thumb, e.HorizontalChange, e.VerticalChange);
            UpdateViewPoints();
            e.Handled = true;

            static void ChangeThumbPosition(Canvas parentCanvas, Thumb thumb, double horizontalChange, double verticalChange)
            {
                if (thumb is null) throw new ArgumentNullException(nameof(thumb));

                var canvasSizeMax = GetActualSize(parentCanvas);
                var oldThumbCanvasPos = thumb.GetCanvasLeftTop();

                var leftMin = 0;
                var leftMax = canvasSizeMax.Width;
                var newLeft = Clamp(oldThumbCanvasPos.X + horizontalChange, leftMin, leftMax);

                var topMin = 0;
                var topMax = canvasSizeMax.Height;
                var newTop = Clamp(oldThumbCanvasPos.Y + verticalChange, topMin, topMax);

                thumb.SetCanvasLeftTop(newLeft, newTop);
            }
        }
    }

    class TrigonometricFunction1ViewModel : MyBindableBase
    {
        #region 1 dimension
        public ObservableCollection<Point> OneDimPoints { get; } =
            new ObservableCollection<Point>(Enumerable.Repeat(default(Point), 2));

        public string OneDimPoint0Name
        {
            get => _oneDimPoint0Name;
            private set => SetProperty(ref _oneDimPoint0Name, value);
        }
        private string _oneDimPoint0Name;

        public string OneDimPoint1Name
        {
            get => _oneDimPoint1Name;
            private set => SetProperty(ref _oneDimPoint1Name, value);
        }
        private string _oneDimPoint1Name;

        public double OneDimDistance
        {
            get => _oneDimDistance;
            private set => SetProperty(ref _oneDimDistance, value);
        }
        private double _oneDimDistance;

        public double OneDimSlope
        {
            get => _oneDimSlope;
            private set => SetProperty(ref _oneDimSlope, value);
        }
        private double _oneDimSlope;

        public double OneDimIntercept
        {
            get => _oneDimIntercept;
            private set => SetProperty(ref _oneDimIntercept, value);
        }
        private double _oneDimIntercept;
        #endregion

        #region 2 dimensions
        public ObservableCollection<Point> TwoDimPoints { get; } =
            new ObservableCollection<Point>(Enumerable.Repeat(default(Point), 3));

        public string TwoDimPoint0Name
        {
            get => _twoDimPoint0Name;
            private set => SetProperty(ref _twoDimPoint0Name, value);
        }
        private string _twoDimPoint0Name;

        public string TwoDimPoint1Name
        {
            get => _twoDimPoint1Name;
            private set => SetProperty(ref _twoDimPoint1Name, value);
        }
        private string _twoDimPoint1Name;

        public string TwoDimPoint2Name
        {
            get => _twoDimPoint2Name;
            private set => SetProperty(ref _twoDimPoint2Name, value);
        }
        private string _twoDimPoint2Name;

        public double TwoDimArea
        {
            get => _twoDimArea;
            private set => SetProperty(ref _twoDimArea, value);
        }
        private double _twoDimArea;

        public double TwoDimAngle012
        {
            get => _twoDimAngle012;
            private set => SetProperty(ref _twoDimAngle012, value);
        }
        private double _twoDimAngle012;
        #endregion

        public TrigonometricFunction1ViewModel()
        {
            OneDimPoints.CollectionChanged += OneDimPoints_CollectionChanged;
            TwoDimPoints.CollectionChanged += TwoDimPoints_CollectionChanged;
        }

        private void OneDimPoints_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                var p0 = OneDimPoints[0];
                var p1 = OneDimPoints[1];

                OneDimPoint0Name = $"p0 ({p0:f1})";
                OneDimPoint1Name = $"p1 ({p1:f1})";
                OneDimDistance = GetDistance(p0, p1);
                OneDimSlope = GetSlope(p0, p1);
                OneDimIntercept = GetIntercept(p0, p1);
            }

            // 2点の距離
            static double GetDistance(Point p0, Point p1)
            {
                var x = p1.X - p0.X;
                var y = p1.Y - p0.Y;
                return Math.Sqrt((x * x) + (y * y));
            }

            static double GetSlope(Point p0, Point p1)
            {
                var bb = p1.X - p0.X;
                return bb == 0 ? 0 : (p1.Y - p0.Y) / bb;
            }

            static double GetIntercept(Point p0, Point p1)
            {
                var a = GetSlope(p0, p1);
                return p0.Y - (a * p0.X);
            }
        }

        private void TwoDimPoints_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                var p0 = TwoDimPoints[0];
                var p1 = TwoDimPoints[1];
                var p2 = TwoDimPoints[2];

                TwoDimPoint0Name = $"p0 ({p0:f1})";
                TwoDimPoint1Name = $"p1 ({p1:f1})";
                TwoDimPoint2Name = $"p2 ({p2:f1})";
                TwoDimArea = GetPolygonArea(p0, p1, p2);
                TwoDimAngle012 = GetAngle(p0, p1, p2);
            }

            static double GetAngle(params Point[] ps)
            {
                if (ps.Length != 3) throw new ArgumentException("cannot calculate angle.");

                var ba = new Point(ps[0].X - ps[1].X, ps[0].Y - ps[1].Y);
                var bc = new Point(ps[2].X - ps[1].X, ps[2].Y - ps[1].Y);

                var babc = ba.X * bc.X + ba.Y * bc.Y;
                var ban = (ba.X * ba.X) + (ba.Y * ba.Y);
                var bcn = (bc.X * bc.X) + (bc.Y * bc.Y);
                var bb = Math.Sqrt(ban * bcn);
                if (bb == 0) return double.NaN;

                var radian = Math.Acos(babc / bb);
                var angle = radian * 180d / Math.PI;
                return angle;
            }
        }

        private static double GetPolygonArea(params Point[] ps)
        {
            if (ps.Length <= 2) throw new ArgumentException("cannot calculate area.");

            var dArea = 0d;
            for (int i = 0; i < ps.Length; ++i)
            {
                dArea += CalcOuterProduct(ps[i], ps[(i + 1) % ps.Length]);
            }
            return Math.Abs(dArea / 2.0);

            static double CalcOuterProduct(Point p0, Point p1) => p0.X * p1.Y - p0.Y * p1.X;
        }

    }
}