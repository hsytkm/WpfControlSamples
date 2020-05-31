using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace WpfControlSamples.Extensions
{
    static class BitmapImageExtension
    {
        /// <summary>
        /// 引数PATHを画像として読み出す
        /// </summary>
        /// <param name="imagePath">ファイルパス</param>
        /// <returns></returns>
        public static BitmapImage FilePathToBitmapImage(this string imagePath)
        {
            if (!File.Exists(imagePath)) throw new FileNotFoundException(imagePath);

            using var stream = File.Open(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return stream.ToBitmapImage();
        }

        /// <summary>
        /// 画像のStreamからBitmapImageを作成する
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static BitmapImage ToBitmapImage(this Stream stream)
        {
            if (stream is null) throw new ArgumentNullException(nameof(stream));

            var bi = new BitmapImage();
            bi.BeginInit();
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.StreamSource = stream;
            bi.EndInit();
            bi.Freeze();

            if (bi.Width == 1 && bi.Height == 1) throw new OutOfMemoryException();
            return bi;
        }

    }
}
