using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

// c# - Copying From and To Clipboard loses image transparency - Stack Overflow
// https://stackoverflow.com/questions/44177115/copying-from-and-to-clipboard-loses-image-transparency
//   https://stackoverflow.com/questions/43675820/rgb-formats-in-net-color/43706643#43706643
//     https://stackoverflow.com/questions/2002322/how-do-i-save-a-bitmap-as-a-16-color-grayscale-gif-or-png-in-asp-net/43142048#43142048
namespace WpfControlSamples.Drawing
{
    /// <summary>
    /// 透過色あり画像をクリップボードから取得する。
    /// Windowsのクリップボードが透過色イメージをサポートしてないので標準Clipboardメソッドだと透過色が正しく扱われない。
    /// </summary>
    public static partial class ClipboardUtils
    {
        /// <summary>
        /// Retrieves an image from the given clipboard data object, in the order PNG, DIB, Bitmap, Image object.
        /// </summary>
        /// <returns>The extracted image, or null if no supported image type was found.</returns>
        public static System.Windows.Media.Imaging.BitmapSource? GetClipboardBitmapSource()
        {
            using var bitmap = GetClipboardDrawingBitmap();
            return bitmap?.ToBitmapSource1();
        }

        /// <summary>
        /// Retrieves an image from the given clipboard data object, in the order PNG, DIB, Bitmap, Image object.
        /// </summary>
        /// <returns>The extracted image, or null if no supported image type was found.</returns>
        public static Bitmap? GetClipboardDrawingBitmap()
        {
            var dataObject = Clipboard.GetDataObject();

            // Order: try PNG, move on to try 32-bit ARGB DIB, then try the normal Bitmap and Image types.
            if (dataObject.GetDataPresent("PNG", false))
            {
                if (dataObject.GetData("PNG", false) is MemoryStream pngStream)
                {
                    using var bitmap = new Bitmap(pngStream);
                    return (Bitmap)bitmap.Clone();
                }
            }

            if (dataObject.GetDataPresent(DataFormats.Dib, false))
            {
                if (dataObject.GetData(DataFormats.Dib, false) is MemoryStream dib)
                    return ImageFromClipboardDib(dib.ToArray());
            }

            if (dataObject.GetDataPresent(DataFormats.Bitmap))
            {
                if (dataObject.GetData(DataFormats.Bitmap) is Image image)
                    return new Bitmap(image);
            }

            if (dataObject.GetDataPresent(typeof(Image)))
            {
                if (dataObject.GetData(typeof(Image)) is Image image)
                    return new Bitmap(image);
            }

            return null;
        }

        private static Bitmap? ImageFromClipboardDib(ReadOnlySpan<byte> dibRoSpan)
        {
            if (dibRoSpan.Length < 4)
                return null;

            var headerSize = BinaryPrimitives.ReadInt32LittleEndian(dibRoSpan.Slice(0, 4));

            // Only supporting 40-byte DIB from clipboard
            if (headerSize != 40)
                return null;

            var imageIndex = headerSize;
            var width = BinaryPrimitives.ReadInt32LittleEndian(dibRoSpan.Slice(4, 4));
            var height = BinaryPrimitives.ReadInt32LittleEndian(dibRoSpan.Slice(8, 4));
            var planes = BinaryPrimitives.ReadInt16LittleEndian(dibRoSpan.Slice(12, 2));
            var bitCount = BinaryPrimitives.ReadInt16LittleEndian(dibRoSpan.Slice(14, 2));

            // Compression: 0 = RGB; 3 = BITFIELDS.
            var compression = BinaryPrimitives.ReadInt32LittleEndian(dibRoSpan.Slice(16, 4));

            // Not dealing with non-standard formats.
            if (planes != 1 || (compression != 0 && compression != 3))
                return null;

            if (compression == 3)
                imageIndex += 12;   // RGB

            if (dibRoSpan.Length < imageIndex)
                return null;

            var pixelFormat = BitCountToPixelFormat(bitCount);
            var pixelsSpan = dibRoSpan.Slice(imageIndex, dibRoSpan.Length - imageIndex);

            if (compression == 3)
            {
                var redMask = BinaryPrimitives.ReadInt32LittleEndian(dibRoSpan.Slice(headerSize, 4));
                var greenMask = BinaryPrimitives.ReadInt32LittleEndian(dibRoSpan.Slice(headerSize + 4, 4));
                var blueMask = BinaryPrimitives.ReadInt32LittleEndian(dibRoSpan.Slice(headerSize + 8, 4));

                // Fix for the undocumented use of 32bppARGB disguised as BITFIELDS. Despite lacking an alpha bit field,
                // the alpha bytes are still filled in, without any header indication of alpha usage.
                // Pure 32-bit RGB: check if a switch to ARGB can be made by checking for non-zero alpha.
                // Admitted, this may give a mess if the alpha bits simply aren't cleared, but why the hell wouldn't it use 24bpp then?
                if (bitCount == 32 && redMask == 0x00FF_0000 && greenMask == 0x0000_FF00 && blueMask == 0x0000_00FF)
                {
                    // Stride is always a multiple of 4; no need to take it into account for 32bpp.
                    for (var pixel = 3; pixel < pixelsSpan.Length; pixel += 4)
                    {
                        // 0 can mean transparent, but can also mean the alpha isn't filled in, so only check for non-zero alpha,
                        // which would indicate there is actual data in the alpha bytes.
                        if (pixelsSpan[pixel] == 0)
                            continue;

                        pixelFormat = PixelFormat.Format32bppPArgb; // override with alpha
                        break;
                    }
                }
                else
                {
                    // Could be supported with a system that parses the colour masks,
                    // but I don't think the clipboard ever uses these anyway.
                    return null;
                }
            }

            // Classic stride: fit within blocks of 4 bytes.
            var stride = (((((bitCount * width) + 7) / 8) + 3) / 4) * 4;
            return BuildImage(pixelsSpan, width, height, stride, pixelFormat);
        }

        private static PixelFormat BitCountToPixelFormat(int bitCount) => bitCount switch
        {
            32 => PixelFormat.Format32bppRgb,
            24 => PixelFormat.Format24bppRgb,
            16 => PixelFormat.Format16bppRgb555,
            _ => throw new NotSupportedException($"bitCount = {bitCount}"),
        };

        /// <summary>
        /// Creates a bitmap based on data, width, height, stride and pixel format.
        /// </summary>
        /// <param name="roSpan">Byte span of raw source data.</param>
        /// <param name="width">Width of the image.</param>
        /// <param name="height">Height of the image.</param>
        /// <param name="stride">Scanline length inside the data.</param>
        /// <param name="pixelFormat">Pixel format.</param>
        /// <returns>The new image.</returns>
        private static Bitmap BuildImage(ReadOnlySpan<byte> roSpan, int width, int height, int stride, PixelFormat pixelFormat)
        {
            var bitmap = new Bitmap(width, height, pixelFormat);
            var rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
            try
            {
                var newImageWidth = ((Image.GetPixelFormatSize(pixelFormat) * width) + 7) / 8;
                var targetStride = bitmapData.Stride;
                var targetPtr = bitmapData.Scan0;

                for (var y = 0; y < height; y++)
                {
                    var startIndex = y * stride;
                    var targetOffset = y * targetStride;
                    var span = roSpan.Slice(startIndex, newImageWidth);

                    // ◆ゼロアロケのコピーできるやろうけど面倒で調べてない
                    Marshal.Copy(span.ToArray(), 0, targetPtr + targetOffset, newImageWidth);
                }
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }

            // This is bmp; reverse image lines.
            bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);

            return bitmap;
        }
    }
}
