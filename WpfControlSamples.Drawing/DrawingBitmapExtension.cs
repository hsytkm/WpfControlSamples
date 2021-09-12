using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace WpfControlSamples.Drawing
{
    /// <summary>
    /// Drawing.Bitmap to BitmapSource
    /// </summary>
    /// <see cref="https://github.com/hsytkm/CsharpImageConverter/blob/master/CsharpImageConverter.Core/DrawingBitmapExtension.cs"/>
    public static class DrawingBitmapExtension
    {
        /// <summary>Bitmapに異常がないかチェックします</summary>
        public static bool IsValid(this Bitmap bitmap)
        {
            if (bitmap.Width == 0 || bitmap.Height == 0) return false;
            return true;
        }

        /// <summary>Bitmapに異常がないかチェックします</summary>
        public static bool IsInvalid(this Bitmap bitmap) => !IsValid(bitmap);

        /// <summary>System.Windows.Media.Imaging.BitmapSource に変換します</summary>
        internal static System.Windows.Media.Imaging.BitmapSource ToBitmapSource1(this Bitmap bitmap)
        {
            if (bitmap.IsInvalid()) throw new ArgumentException("Invalid Image");

            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            _ = ms.Seek(0, SeekOrigin.Begin);

            var bitmapSource = System.Windows.Media.Imaging.BitmapFrame.Create(ms,
                System.Windows.Media.Imaging.BitmapCreateOptions.None,
                System.Windows.Media.Imaging.BitmapCacheOption.OnLoad);

            bitmapSource.Freeze();
            return bitmapSource;
        }

    }
}
