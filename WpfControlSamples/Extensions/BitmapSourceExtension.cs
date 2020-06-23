using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfControlSamples.Extensions
{
    static class BitmapSourceExtension
    {
        /// <summary>
        /// 引数PATHの画像を埋め込みサムネイル優先で読み出す
        /// </summary>
        /// <param name="imagePath">ファイルパス</param>
        /// <param name="widthMax">埋め込みサムネイルが存在しなかった場合に、主画像をリサイズする最大幅</param>
        /// <returns></returns>
        public static BitmapSource FilePathToBitmapSourceFromEmbeddedThumbnail(this string imagePath, double widthMax)
        {
            if (!File.Exists(imagePath)) throw new FileNotFoundException(imagePath);

            using var stream = File.Open(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return stream.ToBitmapSourceFromEmbeddedThumbnail(widthMax);
        }

        /// <summary>
        /// 画像のStreamから埋め込みサムネイル優先でBitmapSourceを作成する
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="widthMax">埋め込みサムネイルが存在しなかった場合に、主画像をリサイズする最大幅</param>
        /// <returns></returns>
        private static BitmapSource ToBitmapSourceFromEmbeddedThumbnail(this Stream stream, double widthMax)
        {
            if (stream is null) throw new ArgumentNullException(nameof(stream));
            var image = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.None);

            BitmapSource bitmap;
            if (image.Thumbnail != null)
            {
                bitmap = image.Thumbnail;
            }
            else
            {
                var longSide = Math.Max(image.PixelWidth, image.PixelHeight);
                if (longSide == 0) throw new DivideByZeroException(nameof(longSide));
                var scale = widthMax / (double)longSide;
                var thumbnail = new TransformedBitmap(image, new ScaleTransform(scale, scale));
                var cache = new CachedBitmap(thumbnail, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                cache.Freeze();
                bitmap = (BitmapSource)cache;  // アップキャストは常に合法
            }
            return bitmap;
        }

    }
}
