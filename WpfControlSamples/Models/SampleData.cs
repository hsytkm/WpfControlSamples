using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace WpfControlSamples.Models
{
    static class SampleData
    {
        public static IList<string> Data { get; } =
            Enumerable.Range(1, 20).Select(x => "Data " + x).ToList();

        /// <summary>
        /// Xamarin.Forms.Colorsのリスト
        /// </summary>
        public static IList<(string Name, Color Color)> WpfColors { get; } =
            typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Select(x => (x.Name, (Color)(x.GetValue(null) ?? throw new NullReferenceException())))
                .ToList();

        /// <summary>
        /// Xamarin.Forms.Brushesのリスト
        /// </summary>
        public static IList<(string Name, SolidColorBrush Brush)> WpfSolidColorBrushes { get; } =
            typeof(Brushes).GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Select(x => (x.Name, (SolidColorBrush)(x.GetValue(null) ?? throw new NullReferenceException())))
                .ToList();

    }
}
