using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

//<AdornerDecorator>
//    <ctrl:ResizableTextBox TextWrapping="Wrap" />
//</AdornerDecorator>
namespace WpfControlSamples.Views.Controls
{
    class ResizableTextBox : TextBox
    {
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.Loaded += new RoutedEventHandler(InitializeAdorner);

            this.AcceptsReturn = true;
        }

        private void InitializeAdorner(object sender, RoutedEventArgs e)
        {
            var layer = AdornerLayer.GetAdornerLayer(this);
            if (layer is null) throw new NullReferenceException();

            var adorner = new ResizingAdorner(this);
            layer.Add(adorner);
        }
    }

    class ResizingAdorner : Adorner
    {
        private readonly Thumb _resizeGrip;
        private readonly VisualCollection _visualChildren;

        public ResizingAdorner(UIElement adornedElement) : base(adornedElement)
        {
            _resizeGrip = new Thumb
            {
                Cursor = Cursors.SizeNWSE,
                Width = 18,
                Height = 18,
            };
            _resizeGrip.DragDelta += new DragDeltaEventHandler(ResizeGripDragDelta);

            var p1 = new FrameworkElementFactory(typeof(Path));
            p1.SetValue(Path.FillProperty, Brushes.White);
            p1.SetValue(Path.DataProperty, Geometry.Parse("M0,14L14,0L14,14z"));
            var p2 = new FrameworkElementFactory(typeof(Path));
            p2.SetValue(Path.StrokeProperty, Brushes.LightGray);
            p2.SetValue(Path.DataProperty, Geometry.Parse("M0,14L14,0"));
            var p3 = new FrameworkElementFactory(typeof(Path));
            p3.SetValue(Path.StrokeProperty, Brushes.LightGray);
            p3.SetValue(Path.DataProperty, Geometry.Parse("M4,14L14,4"));
            var p4 = new FrameworkElementFactory(typeof(Path));
            p4.SetValue(Path.StrokeProperty, Brushes.LightGray);
            p4.SetValue(Path.DataProperty, Geometry.Parse("M8,14L14,8"));
            var p5 = new FrameworkElementFactory(typeof(Path));
            p5.SetValue(Path.StrokeProperty, Brushes.LightGray);
            p5.SetValue(Path.DataProperty, Geometry.Parse("M12,14L14,12"));

            var grid = new FrameworkElementFactory(typeof(Grid));
            grid.SetValue(Grid.MarginProperty, new Thickness(2));
            grid.AppendChild(p1);
            grid.AppendChild(p2);
            grid.AppendChild(p3);
            grid.AppendChild(p4);
            grid.AppendChild(p5);

            _resizeGrip.Template = new ControlTemplate(typeof(Thumb))
            {
                VisualTree = grid
            };
            _visualChildren = new VisualCollection(this)
            {
                _resizeGrip
            };
        }

        private void ResizeGripDragDelta(object sender, DragDeltaEventArgs e)
        {
            var element = this.AdornedElement as FrameworkElement;

            var w = element.Width;
            var h = element.Height;
            if (w.Equals(double.NaN))
                w = element.DesiredSize.Width;
            if (h.Equals(double.NaN))
                h = element.DesiredSize.Height;

            w += e.HorizontalChange;
            h += e.VerticalChange;
            w = Math.Max(_resizeGrip.Width, w);
            h = Math.Max(_resizeGrip.Height, h);
            w = Math.Max(element.MinWidth, w);
            h = Math.Max(element.MinHeight, h);
            w = Math.Min(element.MaxWidth, w);
            h = Math.Min(element.MaxHeight, h);

            element.Width = w;
            element.Height = h;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var element = this.AdornedElement as FrameworkElement;

            var w = _resizeGrip.Width;
            var h = _resizeGrip.Height;
            var x = element.ActualWidth - w;
            var y = element.ActualHeight - h;
            _resizeGrip.Arrange(new Rect(x, y, w, h));

            return finalSize;
        }

        protected override int VisualChildrenCount
        {
            get { return _visualChildren.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visualChildren[index];
        }
    }
}
