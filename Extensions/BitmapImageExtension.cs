using System;
using System.IO;
using System.Reflection;
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
            if (!File.Exists(imagePath)) throw new FileNotFoundException();

            var bi = new BitmapImage();
            using (var stream = File.Open(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = stream;
                bi.EndInit();
            }
            bi.Freeze();

            if (bi.Width == 1 && bi.Height == 1) throw new OutOfMemoryException();
            return bi;
        }

        /// <summary>
        /// 埋め込みリソースを画像として読み出す
        /// </summary>
        /// <param name="imagePath">ファイルパス</param>
        /// <returns></returns>
        public static BitmapImage EmbeddedResourceNameToBitmapImage(this string resourceName, Assembly assembly)
        {
            var bi = new BitmapImage();
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = stream;
                bi.EndInit();
            }
            bi.Freeze();

            if (bi.Width == 1 && bi.Height == 1) throw new OutOfMemoryException();
            return bi;
        }

    }
}
