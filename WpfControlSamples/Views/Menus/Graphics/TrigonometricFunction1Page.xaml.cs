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

        private Thumb[] _lineThumbPoints;
        private Line[] _lineLines;

        private Thumb[] _triangleThumbPoints;
        private Line[] _triangleLines;

        private Thumb[] _quadrangleThumbPoints;
        private Line[] _quadrangleLines;

        public TrigonometricFunction1Page()
        {
            InitializeComponent();
            DataContext = _viewModel;

            this.Loaded += (_, __) =>
            {
                _lineThumbPoints = new[] { thumbPoint0, thumbPoint1 };
                _lineLines = new[] { line1 };

                _triangleThumbPoints = new[] { thumbPoint3_0, thumbPoint3_1, thumbPoint3_2 };
                _triangleLines = new[] { triangleLine0, triangleLine1, triangleLine2 };

                _quadrangleThumbPoints = new[] { thumbPoint4_0, thumbPoint4_1, thumbPoint4_2, thumbPoint4_3 };
                _quadrangleLines = new[] { quadrangleLine0, quadrangleLine1, quadrangleLine2, quadrangleLine3 };

                _parentCanvas = GetParentCanvas(originPoint);
                _parentCanvas.SizeChanged += (_, __) => OnPointThumbChanged();

                OnPointThumbChanged();
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
        private void OnPointThumbChanged()
        {
            var canvasSize = GetActualSize(_parentCanvas);

            // 原点コントロールの更新
            originPoint.SetCanvasLeftTop(0, canvasSize.Height);

            // View ⇔ ViewModel の座標変換メソッド (View:左上, ViewModel:右下)
            var viewToViewModelCoordinateFunc = new Func<Point, Point>(p => new Point(p.X, canvasSize.Height - p.Y));
            var viewModelToViewCoordinateFunc = new Func<Point, Point>(p => new Point(p.X, canvasSize.Height - p.Y));
            _viewModel.ToViewCoordinate = viewModelToViewCoordinateFunc;

            // 各コントロールの更新
            var source = new (Thumb[] thumbPoints, Line[] lines, ObservableCollection<Point> viewModelPoints)[]
            {
                (_lineThumbPoints, _lineLines, _viewModel.LinePoints),
                (_triangleThumbPoints, _triangleLines, _viewModel.TrianglePoints),
                (_quadrangleThumbPoints, _quadrangleLines, _viewModel.QuadranglePoints),
            };
            foreach (var (thumbPoints, lines, viewModelPoints) in source)
            {
                UpdateViewAndViewModel(canvasSize, thumbPoints, lines, viewModelPoints, viewToViewModelCoordinateFunc);
            }
        }

        // Viewの点が変化したときの処理（ViewコントロールとViewModelプロパティの更新）
        private static void UpdateViewAndViewModel(Size canvasSize, Thumb[] thumbPoints, Line[] lines, ObservableCollection<Point> viewModelPoints, Func<Point, Point> toViewModelCoordinate)
        {
            var points = thumbPoints.Select(t => t.GetCanvasLeftTop())
                .Select(p => new Point(Clamp(p.X, 0, canvasSize.Width), Clamp(p.Y, 0, canvasSize.Height))).ToArray();

            // 点の更新
            for (int i = 0; i < thumbPoints.Length; ++i)
            {
                thumbPoints[i].SetCanvasLeftTop(points[i]);
                viewModelPoints[i] = toViewModelCoordinate(points[i]);
            }

            // 辺の更新
            for (int i = 0; i < lines.Length; ++i)
            {
                SetLineXY(lines[i], points[i], points[(i + 1) % thumbPoints.Length]);
            }
        }

        // 点のドラッグシフト
        private void PointThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!(sender is Thumb thumb)) return;
            ChangeThumbPosition(_parentCanvas, thumb, e.HorizontalChange, e.VerticalChange);
            OnPointThumbChanged();
            e.Handled = true;

            static void ChangeThumbPosition(Canvas parentCanvas, Thumb thumb, double horizontalChange, double verticalChange)
            {
                var canvasSizeMax = GetActualSize(parentCanvas);
                var oldThumbCanvasPos = thumb.GetCanvasLeftTop();

                var newLeft = Clamp(oldThumbCanvasPos.X + horizontalChange, 0, canvasSizeMax.Width);
                var newTop = Clamp(oldThumbCanvasPos.Y + verticalChange, 0, canvasSizeMax.Height);
                thumb.SetCanvasLeftTop(newLeft, newTop);
            }
        }
    }

    class TrigonometricFunction1ViewModel : MyBindableBase
    {
        // ViewModel から View への座標変換メソッド
        public Func<Point, Point> ToViewCoordinate { get; set; }

        #region Line
        public ObservableCollection<Point> LinePoints { get; } =
            new ObservableCollection<Point>(Enumerable.Repeat(default(Point), 2));

        public ObservableCollection<string> LinePointNames { get; } =
            new ObservableCollection<string>(Enumerable.Repeat("", 2));

        public Point[] LineAssistShapePoints
        {
            get => _lineAssistShapePoints;
            private set => SetProperty(ref _lineAssistShapePoints, value);
        }
        private Point[] _lineAssistShapePoints = Enumerable.Empty<Point>().ToArray();

        public bool LineIsValid
        {
            get => _lineIsValid;
            private set => SetProperty(ref _lineIsValid, value);
        }
        private bool _lineIsValid;

        public double LineDistance
        {
            get => _lineDistance;
            private set => SetProperty(ref _lineDistance, value);
        }
        private double _lineDistance;

        public double LineSlope
        {
            get => _lineSlope;
            private set => SetProperty(ref _lineSlope, value);
        }
        private double _lineSlope;

        public double LineIntercept
        {
            get => _lineIntercept;
            private set => SetProperty(ref _lineIntercept, value);
        }
        private double _lineIntercept;
        #endregion

        #region Triangle
        public ObservableCollection<Point> TrianglePoints { get; } =
            new ObservableCollection<Point>(Enumerable.Repeat(default(Point), 3));

        public ObservableCollection<string> TrianglePointNames { get; } =
            new ObservableCollection<string>(Enumerable.Repeat("", 3));

        public Point TriangleCenterPoint
        {
            get => _triangleCenterPoint;
            private set => SetProperty(ref _triangleCenterPoint, value);
        }
        private Point _triangleCenterPoint;

        // 三角形の外接円
        public Point TriangleCircumscribedCircleCenterPoint
        {
            get => _triangleCircumscribedCircleCenterPoint;
            private set => SetProperty(ref _triangleCircumscribedCircleCenterPoint, value);
        }
        private Point _triangleCircumscribedCircleCenterPoint;
        
        public Size TriangleCircumscribedCircleDiameterSize
        {
            get => _triangleCircumscribedCircleDiameterSize;
            private set => SetProperty(ref _triangleCircumscribedCircleDiameterSize, value);
        }
        private Size _triangleCircumscribedCircleDiameterSize;

        // 三角形の内接円
        public Point TriangleInscribedCircleCenterPoint
        {
            get => _triangleInscribedCircleCenterPoint;
            private set => SetProperty(ref _triangleInscribedCircleCenterPoint, value);
        }
        private Point _triangleInscribedCircleCenterPoint;

        public Size TriangleInscribedCircleDiameterSize
        {
            get => _triangleInscribedCircleDiameterSize;
            private set => SetProperty(ref _triangleInscribedCircleDiameterSize, value);
        }
        private Size _triangleInscribedCircleDiameterSize;

        public Point[] TriangleAssistShapePoints
        {
            get => _triangleAssistShapePoints;
            private set => SetProperty(ref _triangleAssistShapePoints, value);
        }
        private Point[] _triangleAssistShapePoints = Enumerable.Empty<Point>().ToArray();

        public bool TriangleIsValid
        {
            get => _triangleIsValid;
            private set => SetProperty(ref _triangleIsValid, value);
        }
        private bool _triangleIsValid;

        public double TriangleArea
        {
            get => _triangleArea;
            private set => SetProperty(ref _triangleArea, value);
        }
        private double _triangleArea;

        public double TriangleAngle012
        {
            get => _triangleAngle012;
            private set => SetProperty(ref _triangleAngle012, value);
        }
        private double _triangleAngle012;
        #endregion

        #region Quadrangle
        public ObservableCollection<Point> QuadranglePoints { get; } =
            new ObservableCollection<Point>(Enumerable.Repeat(default(Point), 4));

        public ObservableCollection<string> QuadranglePointNames { get; } =
            new ObservableCollection<string>(Enumerable.Repeat("", 4));

        public Point QuadrangleCenterPoint
        {
            get => _quadrangleCenterPoint;
            private set => SetProperty(ref _quadrangleCenterPoint, value);
        }
        private Point _quadrangleCenterPoint;

        public Point[] QuadrangleAssistShapePoints
        {
            get => _quadrangleAssistShapePoints;
            private set => SetProperty(ref _quadrangleAssistShapePoints, value);
        }
        private Point[] _quadrangleAssistShapePoints = Enumerable.Empty<Point>().ToArray();

        public bool QuadrangleIsValid
        {
            get => _QuadrangleIsValid;
            private set => SetProperty(ref _QuadrangleIsValid, value);
        }
        private bool _QuadrangleIsValid;

        public double QuadrangleArea
        {
            get => _QuadrangleArea;
            private set => SetProperty(ref _QuadrangleArea, value);
        }
        private double _QuadrangleArea;
        #endregion

        public TrigonometricFunction1ViewModel()
        {
            LinePoints.CollectionChanged += LinePoints_CollectionChanged;
            TrianglePoints.CollectionChanged += TrianglePoints_CollectionChanged;
            QuadranglePoints.CollectionChanged += QuadraglePoints_CollectionChanged;
        }

        private void LinePoints_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                var ps = LinePoints.ToArray();
                for (int i = 0; i < ps.Length; ++i)
                {
                    LinePointNames[i] = $"p{i} ({ps[i]:f1})";
                }

                LineIsValid = MathLineExtension.IsValidLine(ps[0], ps[1]);
                LineDistance = MathLineExtension.GetDistance(ps[0], ps[1]);
                LineSlope = MathLineExtension.GetSlope(ps[0], ps[1]);
                LineIntercept = MathLineExtension.GetIntercept(ps[0], ps[1]);
                LineAssistShapePoints = GetAssistRectanglePointsOnView(ps, ToViewCoordinate);
            }
        }

        private void TrianglePoints_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                var ps = TrianglePoints.ToArray();
                for (int i = 0; i < ps.Length; ++i)
                {
                    TrianglePointNames[i] = $"p{i} ({ps[i]:f1})";
                }

                TriangleIsValid = ps.IsValidPolygon();
                TriangleArea = ps.GetPolygonArea();
                TriangleAngle012 = ps.GetAngle();
                TriangleCenterPoint = ToViewCoordinate(ps.GetPolygonCenterPoint());
                TriangleCircumscribedCircleCenterPoint = ToViewCoordinate(ps.GetCircumscribedCirclePoint());
                var diameter0 = ps.GetCircumscribedCircleDiameter();
                TriangleCircumscribedCircleDiameterSize = new Size(diameter0, diameter0);

                TriangleInscribedCircleCenterPoint = ToViewCoordinate(ps.GetInscribedCirclePoint());
                var diameter1 = ps.GetInscribedCircleDiameter();
                TriangleInscribedCircleDiameterSize = new Size(diameter1, diameter1);

                TriangleAssistShapePoints = GetAssistRectanglePointsOnView(ps, ToViewCoordinate);
            }
        }

        private void QuadraglePoints_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                var ps = QuadranglePoints.ToArray();
                for (int i = 0; i < ps.Length; ++i)
                {
                    QuadranglePointNames[i] = $"p{i} ({ps[i]:f1})";
                }

                QuadrangleIsValid = ps.IsValidPolygon();
                QuadrangleArea = ps.GetPolygonArea();
                QuadrangleCenterPoint = ToViewCoordinate(ps.GetPolygonCenterPoint());
                QuadrangleAssistShapePoints = GetAssistRectanglePointsOnView(ps, ToViewCoordinate);
            }
        }

        // 多点に外接する四角形の点を取得(View座標系)
        private static Point[] GetAssistRectanglePointsOnView(Point[] source, Func<Point, Point> convCoordinate)
        {
            var points = source.GetCircumscribedRectanglePoints().Select(p => convCoordinate(p)).ToList();
            points.Add(points[0]);  // ViewのPolylineを閉じる必要があるので始点を追加する
            return points.ToArray();
        }

    }
}