#nullable disable
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfControlSamples.Extensions
{
    static class BitmapImageExtension2
    {
        private class ImagePixels
        {
            public int Width { get; }
            public int Height { get; }
            public int BytesPerPixel { get; }
            public int Stride { get; }
            public byte[] Pixels { get; }

            public ImagePixels(Int32Rect roi, int bytesPerPixel, byte[] pixels)
            {
                Width = roi.Width;
                Height = roi.Height;
                BytesPerPixel = bytesPerPixel;
                Stride = roi.Width * bytesPerPixel;
                Pixels = pixels;
            }
        }

        public static BitmapSource ToGrayScale(this BitmapImage bitmap, int ch)
        {
            if (bitmap is null) throw new ArgumentNullException(nameof(bitmap));
            if (!(0 <= ch && ch <= 2)) return null;  // chは 0~2 指定を想定

            // 画素値のbyte配列を取得
            var imagePixels = GetImagePixels(bitmap);

            // BGRA以外は未実装
            if (imagePixels.BytesPerPixel != 4) throw new NotImplementedException();

            // 特定のチャンネルをOFFする
            var pixels = imagePixels.Pixels;
            for (int y = 0; y < imagePixels.Height * imagePixels.Stride; y += imagePixels.Stride)
            {
                for (int n = y; n < y + imagePixels.Stride; n += imagePixels.BytesPerPixel)
                {
                    var pixel = pixels[n + ch];

                    // BGRA
                    pixels[n + 0] = pixel;
                    pixels[n + 1] = pixel;
                    pixels[n + 2] = pixel;
                    //pixels[n + 3] = 0;
                }
            }

            // 画素値のbyte配列からBitmapSourceを作成
            var newBitmap = CreateWriteableBitmap(imagePixels);

            newBitmap.Freeze();
            return newBitmap;
        }

        /// <summary>
        /// BitmapSource から1画素の値を読み出す
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Color GetImagePixel(this BitmapSource bitmap, int pointX, int pointY)
        {
            if (bitmap is null) throw new ArgumentNullException(nameof(bitmap));

            int imageWidth = bitmap.PixelWidth;
            int imageHeight = bitmap.PixelHeight;
            int pixelsByte = (bitmap.Format.BitsPerPixel + 7) / 8; // bit→Byte変換

            pointX = Math.Max(0, Math.Min(imageWidth - 1, pointX));
            pointY = Math.Max(0, Math.Min(imageHeight - 1, pointY));

            var roi = new Int32Rect(pointX, pointY, 1, 1);
            var stride = roi.Width * pixelsByte;
            var pixels = new byte[stride * roi.Height];
            bitmap.CopyPixels(roi, pixels, stride, 0);

            return Color.FromRgb(pixels[2], pixels[1], pixels[0]);
        }

        /// <summary>
        /// BitmapSource から画素値のbyte配列を読み出す
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private static ImagePixels GetImagePixels(BitmapSource bitmap)
        {
            if (bitmap is null) throw new ArgumentNullException(nameof(bitmap));

            int imageWidth = bitmap.PixelWidth;
            int imageHeight = bitmap.PixelHeight;
            int pixelsByte = (bitmap.Format.BitsPerPixel + 7) / 8; // bit→Byte変換

            var roi = new Int32Rect(0, 0, imageWidth, imageHeight);
            var stride = roi.Width * pixelsByte;
            var pixels = new byte[stride * roi.Height];
            bitmap.CopyPixels(pixels, stride, 0);

            return new ImagePixels(roi, pixelsByte, pixels);
        }

        /// <summary>
        /// byte配列からWriteableBitmap(BitmapSource)を作成する
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private static WriteableBitmap CreateWriteableBitmap(ImagePixels image)
        {
            var dpi = 96.0;
            var pixelFormat = PixelFormats.Bgra32;
            var bitmap = new WriteableBitmap(image.Width, image.Height, dpi, dpi, pixelFormat, null);

            try
            {
                bitmap.Lock();

                bitmap.WritePixels(
                    new Int32Rect(0, 0, image.Width, image.Height),
                    image.Pixels, image.Stride, 0);
            }
            finally
            {
                bitmap.Unlock();
            }
            return bitmap;
        }

    }
}
