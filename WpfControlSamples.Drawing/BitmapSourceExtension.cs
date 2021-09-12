using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfControlSamples.Drawing
{
    /// <summary>
    /// BitmapSource to Drawing.Bitmap
    /// </summary>
    /// <see cref="https://github.com/hsytkm/CsharpImageConverter/blob/master/CsharpImageConverter.Core/BitmapSourceExtension.cs"/>
    public static class BitmapSourceExtension
    {
        /// <summary>BitmapSourceに異常がないかチェックします</summary>
        public static bool IsValid(this BitmapSource bitmap)
        {
            if (bitmap.PixelWidth == 0 || bitmap.PixelHeight == 0) return false;
            return true;
        }

        /// <summary>BitmapSourceに異常がないかチェックします</summary>
        public static bool IsInvalid(this BitmapSource bitmap) => !IsValid(bitmap);

        /// <summary>System.Drawing.Bitmap に変換します</summary>
        public static System.Drawing.Bitmap ToDrawingBitmap(this BitmapSource bitmapSource)
        {
            if (bitmapSource.IsInvalid())
                throw new ArgumentException("Invalid Image");

            var bitmap = new System.Drawing.Bitmap(bitmapSource.PixelWidth, bitmapSource.PixelHeight,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(System.Drawing.Point.Empty, bitmap.Size),
                System.Drawing.Imaging.ImageLockMode.WriteOnly, bitmap.PixelFormat);

            try
            {
                bitmapSource.CopyPixels(Int32Rect.Empty, bitmapData.Scan0,
                    bitmapData.Height * bitmapData.Stride, bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }

            return bitmap;
        }
    }
}
