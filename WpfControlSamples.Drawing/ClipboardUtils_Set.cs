using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

// c# - Copying From and To Clipboard loses image transparency - Stack Overflow
// https://stackoverflow.com/questions/44177115/copying-from-and-to-clipboard-loses-image-transparency
namespace WpfControlSamples.Drawing
{
    /// <summary>
    /// 透過色あり画像をクリップボードに入れる。
    /// Windows標準で対応されていないので、一部のソフトでしか貼り付けられない（Office系はOK, MsPaintはNG）
    /// </summary>
    public static partial class ClipboardUtils
    {
        /// <summary>
        /// Copies the given image to the clipboard as PNG, DIB and standard Bitmap format.
        /// </summary>
        /// <param name="bitmapSource">Image to put on the clipboard.</param>
        public static void SetTransparentImageToClipboard(System.Windows.Media.Imaging.BitmapSource bitmapSource)
        {
            using var bitmap = bitmapSource.ToDrawingBitmap();
            SetTransparentImageToClipboard(bitmap);
        }

        /// <summary>
        /// Copies the given image to the clipboard as PNG, DIB and standard Bitmap format.
        /// </summary>
        /// <param name="tranceparentBitmap">Image to put on the clipboard.</param>
        /// <param name="notTranceparentBitmap">Optional specifically nontransparent version of the image to put on the clipboard.</param>
        public static void SetTransparentImageToClipboard(Bitmap tranceparentBitmap, Bitmap? notTranceparentBitmap = null)
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
            using var dibMemStream = DipBitmap.CreateMemoryStream(tranceparentBitmap);
            data.SetData(DataFormats.Dib, dibMemStream, false);

            // The 'copy=true' argument means the MemoryStreams can be safely disposed after the operation.
            Clipboard.SetDataObject(data, true);
        }

        private static class DipBitmap
        {
            /// <summary>
            /// Converts the image to Device Independent Bitmap format of type BITFIELDS.
            /// This is (wrongly) accepted by many applications as containing transparency,
            /// so I'm abusing it for that.
            /// </summary>
            /// <param name="bitmap">Bitmap to convert to DIB</param>
            /// <returns>The image converted to DIB, in bytes.</returns>
            public static MemoryStream CreateMemoryStream(Bitmap bitmap)
            {
                if (bitmap.PixelFormat == PixelFormat.Format32bppArgb)
                {
                    return CreateMemoryStreamImpl(bitmap);
                }
                else
                {
                    // 以下ホントにいるんかな？（フォーマット確認のため Bitmap 作り直し作業）
                    // Ensure image is 32bppARGB by painting it on a new 32bppARGB image.
                    using var newBitmap = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);

                    using (var gr = Graphics.FromImage(newBitmap))
                    {
                        gr.DrawImage(bitmap, new Rectangle(0, 0, newBitmap.Width, newBitmap.Height));
                    }
                    return CreateMemoryStreamImpl(newBitmap);
                }

                static MemoryStream CreateMemoryStreamImpl(Bitmap bitmap)
                {
                    var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
                    try
                    {
                        var stride = bitmapData.Stride;
                        var height = bitmap.Height;
                        var pixelsSize = stride * height;
                        var header = new DipBitmapHeader(bitmap.Width, height, pixelsSize);
                        var headerSize = DipBitmapHeader.Size;
                        var bitmapBytes = new byte[headerSize + pixelsSize];

                        Unsafe.WriteUnaligned(ref bitmapBytes[0], header);

                        // 上下反転して画素コピー
                        unsafe
                        {
                            fixed (byte* destHeadPtr = bitmapBytes)
                            {
                                var destPixelHeadPtr = destHeadPtr + headerSize;
                                var srcPixelHeadPtr = (byte*)bitmapData.Scan0;

                                for (var y = 0; y < height; y++)
                                {
                                    var destPtr = destPixelHeadPtr + ((height - y - 1) * stride);
                                    var srcPtr = srcPixelHeadPtr + (y * stride);

                                    for (var n = 0; n < stride; n += 4)
                                    {
                                        *(int*)(destPtr + n) = *(int*)(srcPtr + n);
                                    }
                                }
                            }
                        }
                        return new MemoryStream(bitmapBytes);
                    }
                    finally
                    {
                        bitmap.UnlockBits(bitmapData);
                    }
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

                    // These are all 0. Since .NET clears new arrays, don't bother writing them.
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
