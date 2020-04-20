using System;
using System.Linq;
using System.Windows.Media;

namespace WpfControlSamples.Models
{
    // C#のColor関連の便利拡張メソッド＋α 24選（HSV色空間への変換も）
    // https://qiita.com/soi/items/11a5c232d47585a3f83e
    struct HsbColor
    {
        /// <summary>
        /// 色相 0~360
        /// </summary>
        public double Hue
        {
            get => _hue;
            set => _hue = ColorExtension.ChopIn360(value);
        }
        private double _hue;

        /// <summary>
        /// 彩度 0~1
        /// </summary>
        public double Saturation
        {
            get => _saturation;
            set => _saturation = Math.Max(0, Math.Min(value, 1));
        }
        private double _saturation;

        /// <summary>
        /// 明度 0~1 (Value)
        /// </summary>
        public double Brightness
        {
            get => _brightness;
            set => _brightness = Math.Max(0, Math.Min(value, 1));
        }
        private double _brightness;

        public static HsbColor FromHSB(double hue, double saturation, double brightness) =>
            new HsbColor() { Hue = hue, Saturation = saturation, Brightness = brightness };

        public override string ToString() => string.Format($"H:{Hue:F0}, S:{Saturation:F2}, B:{Brightness:F2}");
    }

    static class HsbColorExt
    {
        public static HsbColor ToHsb(this Color source)
        {
            double nR = source.NormalizedRed();
            double nG = source.NormalizedGreen();
            double nB = source.NormalizedBlue();

            double[] nRGBs = new[] { nR, nG, nB };
            double max = nRGBs.Max();
            double min = nRGBs.Min();

            double diff = max - min;

            return new HsbColor()
            {
                Hue = max == min ? 0
                    : max == nR ? 60d * (nG - nB) / diff
                    : max == nG ? (60d * (nB - nR) / diff) + 120d
                    : (60d * (nR - nG) / diff) + 240d,
                Saturation = max == 0
                    ? 0
                    : diff / max,
                Brightness = max,
            };
        }

        public static Color ToRgb(this HsbColor source)
        {
            double max = source.Brightness;
            double min = max * (1 - source.Saturation);
            int hueZone = (int)(source.Hue / 60d);
            double f = source.Hue / 60d - hueZone;
            double x0 = max * (1 - f * source.Saturation);
            double x1 = max * (1 - (1 - f) * source.Saturation);

            var (nR, nG, nB) = hueZone switch
            {
                0 => (max, x1, min),
                1 => (x0, max, min),
                2 => (min, max, x1),
                3 => (min, x0, max),
                4 => (x1, min, max),
                _ => (max, min, x0),
            };

            return ColorExtension.NormalizedRgbToColor(nR, nG, nB);
        }
    }

    static class ColorExtension
    {
        public static double ChopIn360(double x) => Math.Max(0d, Math.Min(x, 360d));

        public static Color NormalizedRgbToColor(double r, double g, double b) =>
            Color.FromScRgb(1f, (float)r, (float)g, (float)b);

        public static double NormalizedRed(this ref Color color) => color.R / (double)byte.MaxValue;
        public static double NormalizedGreen(this ref Color color) => color.G / (double)byte.MaxValue;
        public static double NormalizedBlue(this ref Color color) => color.B / (double)byte.MaxValue;
    }
}
