using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

// c# - Copying From and To Clipboard loses image transparency - Stack Overflow
// https://stackoverflow.com/questions/44177115/copying-from-and-to-clipboard-loses-image-transparency
namespace WpfControlSamples.Drawing
{
    /// <summary>
    /// 透過色あり画像をクリップボードに入れる。
    /// Windows標準で対応されていないので、一部のソフトでしか貼り付けられない（Office系はOK）
    /// </summary>
    public static partial class ClipboardUtils
    {
        /// <summary>
        /// Copies the given image to the clipboard as PNG, DIB and standard Bitmap format.
        /// </summary>
        /// <param name="bitmapSource">Image to put on the clipboard.</param>
        public static async ValueTask SetTransparentImageToClipboardAsync(System.Windows.Media.Imaging.BitmapSource bitmapSource)
        {
            using var bitmap = bitmapSource.ToDrawingBitmap();
            await SetTransparentImageToClipboardAsync(bitmap);
        }

        /// <summary>
        /// Copies the given image to the clipboard as PNG, DIB and standard Bitmap format.
        /// </summary>
        /// <param name="tranceparentBitmap">Image to put on the clipboard.</param>
        /// <param name="notTranceparentBitmap">Optional specifically nontransparent version of the image to put on the clipboard.</param>
        public static async ValueTask SetTransparentImageToClipboardAsync(Bitmap tranceparentBitmap, Bitmap? notTranceparentBitmap = null)
        {
            Clipboard.Clear();

            notTranceparentBitmap ??= tranceparentBitmap;
            var data = new DataObject();

            // As standard bitmap, without transparency support
            data.SetData(DataFormats.Bitmap, notTranceparentBitmap, true);

            // As PNG. Gimp will prefer this over the other two.
            using var pngMemStream = new MemoryStream();
            tranceparentBitmap.Save(pngMemStream, ImageFormat.Png);

            data.SetData("PNG", pngMemStream, false);

            // As DIB. This is (wrongly) accepted as ARGB by many applications.
            var dibData = ConvertToDib(tranceparentBitmap);

            using var dibMemStream = new MemoryStream();
            await dibMemStream.WriteAsync(dibData);

            data.SetData(DataFormats.Dib, dibMemStream, false);

            // The 'copy=true' argument means the MemoryStreams can be safely disposed after the operation.
            Clipboard.SetDataObject(data, true);
        }

        /// <summary>
        /// Converts the image to Device Independent Bitmap format of type BITFIELDS.
        /// This is (wrongly) accepted by many applications as containing transparency,
        /// so I'm abusing it for that.
        /// </summary>
        /// <param name="image">Image to convert to DIB</param>
        /// <returns>The image converted to DIB, in bytes.</returns>
        private static ReadOnlyMemory<byte> ConvertToDib(Image image)
        {
            // Ensure image is 32bppARGB by painting it on a new 32bppARGB image.
            using var bitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);

            using (var gr = Graphics.FromImage(bitmap))
            {
                gr.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
            }

            // Bitmap format has its lines reversed.
            bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);

            return DipBitmap.CreateRoMemory(bitmap);
        }

        private static class DipBitmap
        {
            public static ReadOnlyMemory<byte> CreateRoMemory(Bitmap bitmap)
            {
                var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
                try
                {
                    var pixelsSize = bitmapData.Stride * bitmap.Height;
                    var header = new DipBitmapHeader(bitmap.Width, bitmap.Height, pixelsSize);
                    var headerSize = DipBitmapHeader.Size;
                    var bitmapBytes = new byte[headerSize + pixelsSize];

                    Unsafe.WriteUnaligned(ref bitmapBytes[0], header);
                    Marshal.Copy(bitmapData.Scan0, bitmapBytes, headerSize, pixelsSize);

                    return bitmapBytes;
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
            }

            private readonly struct DipBitmapHeader
            {
                public const int Size = 40 + 12;

                // BITMAPINFOHEADER struct for DIB.
                public readonly int HeaderSize;         // 0~
                public readonly int Width;              // 4~
                public readonly int Height;             // 8~
                public readonly short Planes;           // 12~
                public readonly short BitCount;         // 14~
                public readonly int Compression;        // 16~
                public readonly int ImageSize;          // 20~
                public readonly int XPelsPerMeter;      // 24~
                public readonly int YPelsPerMeter;      // 28~
                public readonly int ClrUsed;            // 32~
                public readonly int ClrImportant;       // 36~

                // The aforementioned "BITFIELDS": colour masks applied to the int pixel value to get the R, G and B values.
                public readonly int ColorBitFieldsR;    // 40~
                public readonly int ColorBitFieldsG;    // 44~
                public readonly int ColorBitFieldsB;    // 48~51

                public DipBitmapHeader(int width, int height, int imageSize)
                {
                    HeaderSize = 40;
                    Width = width;
                    Height = height;
                    Planes = 1;
                    BitCount = 32;
                    Compression = 3;
                    ImageSize = imageSize;

                    // These are all 0. Since .net clears new arrays, don't bother writing them.
                    XPelsPerMeter = 0;
                    YPelsPerMeter = 0;
                    ClrUsed = 0;
                    ClrImportant = 0;

                    ColorBitFieldsR = 0x00FF_0000;
                    ColorBitFieldsG = 0x0000_FF00;
                    ColorBitFieldsB = 0x0000_00FF;
                }
            }
        }
    }
}
