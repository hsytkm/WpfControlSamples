using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WpfControlSamples.ViewModels;

namespace WpfControlSamples.Views.Behaviors
{
    /// <summary>ScrollViewer のプロパティ値を表示する（勉強用）</summary>
    [TypeConstraint(typeof(ScrollViewer))]
    public class ScrollViewerInspectorBehavior : Behavior<ScrollViewer>
    {
        public static readonly DependencyProperty ScrollViewerPropertiesProperty =
            DependencyProperty.Register(nameof(ScrollViewerProperties), typeof(IReadOnlyList<NameDoublePair>), typeof(ScrollViewerInspectorBehavior));
        internal IReadOnlyList<NameDoublePair> ScrollViewerProperties
        {
            get => (IReadOnlyList<NameDoublePair>)GetValue(ScrollViewerPropertiesProperty);
            set => SetValue(ScrollViewerPropertiesProperty, value);
        }

        public static readonly DependencyProperty ContentPresenterPropertiesProperty =
            DependencyProperty.Register(nameof(ContentPresenterProperties), typeof(IReadOnlyList<NameDoublePair>), typeof(ScrollViewerInspectorBehavior));
        internal IReadOnlyList<NameDoublePair> ContentPresenterProperties
        {
            get => (IReadOnlyList<NameDoublePair>)GetValue(ContentPresenterPropertiesProperty);
            set => SetValue(ContentPresenterPropertiesProperty, value);
        }

        public static readonly DependencyProperty HorizontalScrollBarPropertiesProperty =
            DependencyProperty.Register(nameof(HorizontalScrollBarProperties), typeof(IReadOnlyList<NameDoublePair>), typeof(ScrollViewerInspectorBehavior));
        internal IReadOnlyList<NameDoublePair> HorizontalScrollBarProperties
        {
            get => (IReadOnlyList<NameDoublePair>)GetValue(HorizontalScrollBarPropertiesProperty);
            set => SetValue(HorizontalScrollBarPropertiesProperty, value);
        }

        public static readonly DependencyProperty VerticalScrollBarPropertiesProperty =
            DependencyProperty.Register(nameof(VerticalScrollBarProperties), typeof(IReadOnlyList<NameDoublePair>), typeof(ScrollViewerInspectorBehavior));
        internal IReadOnlyList<NameDoublePair> VerticalScrollBarProperties
        {
            get => (IReadOnlyList<NameDoublePair>)GetValue(VerticalScrollBarPropertiesProperty);
            set => SetValue(VerticalScrollBarPropertiesProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ScrollChanged += ScrollViewer_ScrollChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.ScrollChanged -= ScrollViewer_ScrollChanged;
            base.OnDetaching();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender is not ScrollViewer scrollViewer) return;

            ScrollViewerProperties = GetScrollViewerProperties(scrollViewer, e);

            var contentPresenter = scrollViewer.Template.FindName("PART_ScrollContentPresenter", scrollViewer) as ContentPresenter;
            ContentPresenterProperties = GetContentPresenterProperties(contentPresenter);

            var horizontalScrollBar = scrollViewer.Template.FindName("PART_HorizontalScrollBar", scrollViewer) as ScrollBar;
            HorizontalScrollBarProperties = GetScrollBarProperties(horizontalScrollBar);

            var verticalScrollBar = scrollViewer.Template.FindName("PART_VerticalScrollBar", scrollViewer) as ScrollBar;
            VerticalScrollBarProperties = GetScrollBarProperties(verticalScrollBar);

            static IReadOnlyList<NameDoublePair> GetScrollViewerProperties(ScrollViewer scrollViewer, ScrollChangedEventArgs e) => new NameDoublePair[]
            {
                new($"ActualWidth", scrollViewer.ActualWidth),
                new($"ActualHeight", scrollViewer.ActualHeight),

                new($"ExtentWidth", e.ExtentWidth),
                new($"ExtentHeight", e.ExtentHeight),
                new($"ExtentWidthChange", e.ExtentWidthChange),
                new($"ExtentHeightChange", e.ExtentHeightChange),

                new($"ViewportWidth", e.ViewportWidth),
                new($"ViewportHeight", e.ViewportHeight),
                new($"ViewportWidthChange", e.ViewportWidthChange),
                new($"ViewportHeightChange", e.ViewportHeightChange),

                new($"HorizontalChange", e.HorizontalChange),
                new($"VerticalChange", e.VerticalChange),
                new($"HorizontalOffset", e.HorizontalOffset),
                new($"VerticalOffset", e.VerticalOffset),
            };

            static IReadOnlyList<NameDoublePair> GetContentPresenterProperties(ContentPresenter? presenter) => (presenter is null)
                ? Array.Empty<NameDoublePair>()
                : new NameDoublePair[]
                {
                    new($"ActualWidth", presenter.ActualWidth),
                    new($"ActualHeight", presenter.ActualHeight),
                };

            static IReadOnlyList<NameDoublePair> GetScrollBarProperties(ScrollBar? bar) => (bar is null)
                ? Array.Empty<NameDoublePair>()
                : new NameDoublePair[]
                {
                    new($"ActualWidth", bar.ActualWidth),
                    new($"ActualHeight", bar.ActualHeight),
                    new($"ViewportSize", bar.ViewportSize),
                };

        }

    }
}
