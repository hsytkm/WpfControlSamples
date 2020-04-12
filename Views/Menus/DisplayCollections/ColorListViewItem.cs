using System;
using System.Windows.Media;

namespace WpfControlSamples.Views.Menus
{
    class ColorListViewItem
    {
        public string Name { get; }
        public Color Color { get; }
        public string ColorLevel => $"A={(Color.A)}, R={(Color.R)}, G={(Color.G)}, B={(Color.B)}";

        public ColorListViewItem((string Name, Color Color) x) =>
            (Name, Color) = (x.Name, x.Color);
    }
}
