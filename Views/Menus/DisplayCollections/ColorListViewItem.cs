using System;
using System.Windows.Media;
using WpfControlSamples.Models;

namespace WpfControlSamples.Views.Menus
{
    class ColorListViewItem
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

            HsbColor = x.Color.ToHsb();
            HsbLevel = HsbColor.ToString();
            Hue = HsbColor.Hue;
        }
    }
}
