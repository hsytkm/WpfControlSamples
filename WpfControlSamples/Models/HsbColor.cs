using System;
using System.Windows.Media;

namespace WpfControlSamples.Models
{
    // C#のColor関連の便利拡張メソッド＋α 24選（HSV色空間への変換も）
    // https://qiita.com/soi/items/11a5c232d47585a3f83e
    struct HsbColor //: IEquatable<HsbColor>
    {
        /// <summary>
        /// 色相 0~360
        /// </summary>
        public double Hue
        {
            get => _hue;
            set
            {
                var h = value;
                while (h < 0d) { h += 360d; }
                while (h > 360d) { h -= 360d; }
                _hue = h;
            }
        }
        private double _hue;

        /// <summary>
        /// 彩度 0~1
        /// </summary>
        public double Saturation
        {
            get => _saturation;
            set => _saturation = Math.Clamp(value, 0d, 1d);
        }
        private double _saturation;

        /// <summary>
        /// 明度 0~1 (Value)
        /// </summary>
        public double Brightness
        {
            get => _brightness;
            set => _brightness = Math.Clamp(value, 0d, 1d);
        }
        private double _brightness;

        public static HsbColor FromHsb(double h, double s, double b) =>
            new() { Hue = h, Saturation = s, Brightness = b };

        public HsbColor ToComplementaryColor()
        {
            var hue = (Hue < 180d) ? Hue + 180d : Hue - 180d;
            return FromHsb(hue, Saturation, Brightness);
        }

        public override string ToString() => string.Format($"H:{Hue:F0}, S:{Saturation:F2}, B:{Brightness:F2}");

#if false
        public bool Equals(HsbColor other) => Equals(other, this);
        public override bool Equals(object obj)
        {
            if (obj is null || GetType() != obj.GetType()) return false;

            var other = (HsbColor)obj;
            return Hue == other.Hue && Saturation == other.Saturation && Brightness == other.Brightness;
        }

        // ◆実装に自身ない…
        public override int GetHashCode() =>
            (((int)Hue & 0xffff) << 16) | (((int)Saturation & 0xff) << 8) | ((int)Brightness & 0xff);
#endif
    }

    static class HsbColorExtension
    {
        public static HsbColor ToHsbColor(this Color source)
        {
            double nR = source.R / 255d;
            double nG = source.G / 255d;
            double nB = source.B / 255d;

            double max = Math.Max(nR, Math.Max(nG, nB));
            double min = Math.Min(nR, Math.Min(nG, nB));
            double diff = max - min;

            var hue = diff == 0 ? 0
                : max == nR ? 60d * (nG - nB) / diff
                : max == nG ? (60d * (nB - nR) / diff) + 120d
                : (60d * (nR - nG) / diff) + 240d;
            var saturation = max == 0 ? 0 : diff / max;
            var brightness = max;

            return HsbColor.FromHsb(hue, saturation, brightness);
        }

#if false
        // ◆何かずれてる…
        public static Color ToRgbColor(this HsbColor source)
        {
            double max = source.Brightness;
            double min = max * (1d - source.Saturation);
            int hueZone = (int)(source.Hue / 60d);
            double f = source.Hue / 60d - hueZone;
            double x0 = max * (1d - f * source.Saturation);
            double x1 = max * (1d - (1d - f) * source.Saturation);

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
#else
        public static Color ToRgbColor(this HsbColor source)
        {
            var hue = source.Hue;
            var saturation = source.Saturation;
            var value = source.Brightness;

            var temp1 = hue / 60d;
            var temp2 = Math.Floor(temp1);
            int hi = Convert.ToInt32(temp2) % 6;
            double f = temp1 - temp2;

            value *= 255d;
            var v = Convert.ToByte(value);
            var p = Convert.ToByte(value * (1d - saturation));
            var q = Convert.ToByte(value * (1d - f * saturation));
            var t = Convert.ToByte(value * (1d - (1d - f) * saturation));

            return hi switch
            {
                0 => Color.FromRgb(v, t, p),
                1 => Color.FromRgb(q, v, p),
                2 => Color.FromRgb(p, v, t),
                3 => Color.FromRgb(p, q, v),
                4 => Color.FromRgb(t, p, v),
                _ => Color.FromRgb(v, p, q),
            };
        }
#endif

    }

    static class ColorExtension
    {
        public static double ToLuminanceY(this Color color) => 0.257 * color.R + 0.504 * color.G + 0.098 * color.B;

        // Colorに対する読みやすそうな文字色(B/W)を取得
        public static Brush GetForegroundBrush(this Color color)
        {
            var y = color.ToLuminanceY();
            var thresh = 128d;              // てきとーだけど割と良い感じ
            return (y > thresh) ? Brushes.Black : Brushes.White;
        }
    }
}
