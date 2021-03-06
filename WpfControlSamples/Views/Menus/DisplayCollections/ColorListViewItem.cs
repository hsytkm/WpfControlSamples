﻿#nullable disable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;
using WpfControlSamples.Models;

namespace WpfControlSamples.Views.Menus
{
    class ColorListViewItem : IEquatable<ColorListViewItem>
    {
        public string Name { get; }
        public Color Color { get; }
        public string ColorLevel { get; }

        public HsbColor HsbColor { get; }
        public string HsbLevel { get; }

        public double Hue { get; }  // グルーピングのサンプル用

        public ColorListViewItem((string Name, Color Color) x)
        {
            Name = x.Name;
            Color = x.Color;
            ColorLevel = $"A={Color.A}, R={Color.R}, G={Color.G}, B={Color.B}";

            HsbColor = x.Color.ToHsbColor();
            HsbLevel = HsbColor.ToString();
            Hue = HsbColor.Hue;
        }

        public bool Equals([AllowNull] ColorListViewItem other) => this.Color == other.Color;
    }
}
