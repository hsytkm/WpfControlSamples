#nullable disable
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfControlSamples.Drawing
{
#if false   // プログラム上で透過色PNGを読む方。動作確認してません
    // c# - Copying From and To Clipboard loses image transparency - Stack Overflow
    // https://stackoverflow.com/questions/44177115/copying-from-and-to-clipboard-loses-image-transparency
    //
    // https://stackoverflow.com/questions/43675820/rgb-formats-in-net-color/43706643#43706643
    // https://stackoverflow.com/questions/2002322/how-do-i-save-a-bitmap-as-a-16-color-grayscale-gif-or-png-in-asp-net/43142048#43142048
    public static partial class ClipboardUtils
    {
        /// <summary>
        /// Retrieves an image from the given clipboard data object, in the order PNG, DIB, Bitmap, Image object.
        /// </summary>
        /// <param name="retrievedData">The clipboard data.</param>
        /// <returns>The extracted image, or null if no supported image type was found.</returns>
        public static Bitmap GetClipboardImage(IDataObject retrievedData)
        {
            // Order: try PNG, move on to try 32-bit ARGB DIB, then try the normal Bitmap and Image types.
            if (retrievedData.GetDataPresent("PNG", false))
            {
                if (retrievedData.GetData("PNG", false) is MemoryStream pngStream)
                {
                    using var bm = new Bitmap(pngStream);
                    return CloneImage(bm);
                }
            }

            if (retrievedData.GetDataPresent(DataFormats.Dib, false))
            {
                if (retrievedData.GetData(DataFormats.Dib, false) is MemoryStream dib)
                    return ImageFromClipboardDib(dib.ToArray());
            }

            if (retrievedData.GetDataPresent(DataFormats.Bitmap))
            {
                if (retrievedData.GetData(DataFormats.Bitmap) is Image image)
                    return new Bitmap(image);
            }

            if (retrievedData.GetDataPresent(typeof(Image)))
            {
                if (retrievedData.GetData(typeof(Image)) is Image image)
                    return new Bitmap(image);
            }

            return null;
        }

        private static Bitmap ImageFromClipboardDib(byte[] dibBytes)
        {
            if (dibBytes is null || dibBytes.Length < 4)
                return null;

            var headerSize = ReadIntFromByteArray(dibBytes, 0, 4);

            // Only supporting 40-byte DIB from clipboard
            if (headerSize != 40)
                return null;

            var header = new byte[40];
            Array.Copy(dibBytes, header, header.Length);

            var imageIndex = headerSize;
            var width = ReadIntFromByteArray(header, 0x04, sizeof(int));
            var height = ReadIntFromByteArray(header, 0x08, sizeof(int));
            var planes = ReadIntFromByteArray(header, 0x0C, sizeof(short));
            var bitCount = ReadIntFromByteArray(header, 0x0E, sizeof(short));

            var pixelFormat = bitCount switch
            {
                32 => PixelFormat.Format32bppRgb,
                24 => PixelFormat.Format24bppRgb,
                16 => PixelFormat.Format16bppRgb555,
                _ => throw new NotSupportedException($"bitCount = {bitCount}"),
            };

            //Compression: 0 = RGB; 3 = BITFIELDS.
            int compression = ReadIntFromByteArray(header, 0x10, sizeof(int));

            // Not dealing with non-standard formats.
            if (planes != 1 || (compression != 0 && compression != 3))
                return null;

            if (compression == 3)
                imageIndex += 12;

            if (dibBytes.Length < imageIndex)
                return null;

            var imageBytes = ArrayPool<byte>.Shared.Rent(dibBytes.Length - imageIndex);
            try
            {
                Array.Copy(dibBytes, imageIndex, imageBytes, 0, imageBytes.Length);

                // Classic stride: fit within blocks of 4 bytes.
                int stride = (((((bitCount * width) + 7) / 8) + 3) / 4) * 4;

                if (compression == 3)
                {
                    var redMask = ReadIntFromByteArray(dibBytes, headerSize + sizeof(int) * 0, sizeof(int));
                    var greenMask = ReadIntFromByteArray(dibBytes, headerSize + sizeof(int) * 1, sizeof(int));
                    var blueMask = ReadIntFromByteArray(dibBytes, headerSize + sizeof(int) * 2, sizeof(int));

                    // Fix for the undocumented use of 32bppARGB disguised as BITFIELDS. Despite lacking an alpha bit field,
                    // the alpha bytes are still filled in, without any header indication of alpha usage.
                    // Pure 32-bit RGB: check if a switch to ARGB can be made by checking for non-zero alpha.
                    // Admitted, this may give a mess if the alpha bits simply aren't cleared, but why the hell wouldn't it use 24bpp then?
                    if (bitCount == 32 && redMask == 0xFF0000 && greenMask == 0x00FF00 && blueMask == 0x0000FF)
                    {
                        // Stride is always a multiple of 4; no need to take it into account for 32bpp.
                        for (var pixel = 3; pixel < imageBytes.Length; pixel += 4)
                        {
                            // 0 can mean transparent, but can also mean the alpha isn't filled in, so only check for non-zero alpha,
                            // which would indicate there is actual data in the alpha bytes.
                            if (imageBytes[pixel] == 0)
                                continue;

                            pixelFormat = PixelFormat.Format32bppPArgb;
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

                Bitmap bitmap = BuildImage(imageBytes, width, height, stride, pixelFormat);

                // This is bmp; reverse image lines.
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);

                return bitmap;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(imageBytes, clearArray: true);
            }
        }

        /// <summary>
        /// Creates a bitmap based on data, width, height, stride and pixel format.
        /// </summary>
        /// <param name="sourceData">Byte array of raw source data.</param>
        /// <param name="width">Width of the image.</param>
        /// <param name="height">Height of the image.</param>
        /// <param name="stride">Scanline length inside the data.</param>
        /// <param name="pixelFormat">Pixel format.</param>
        /// <returns>The new image.</returns>
        private static Bitmap BuildImage(byte[] sourceData, int width, int height, int stride, PixelFormat pixelFormat)
        {
            var bitmap = new Bitmap(width, height, pixelFormat);
            var rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
            try
            {
                var newDataWidth = ((Image.GetPixelFormatSize(pixelFormat) * width) + 7) / 8;
                var targetStride = bitmapData.Stride;
                var targetPtr = bitmapData.Scan0;

                for (var y = 0; y < height; y++)
                {
                    Marshal.Copy(sourceData, y * stride, targetPtr + (y * targetStride), newDataWidth);
                }
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
            return bitmap;
        }

        /// <summary>
        /// Clones an image object to free it from any backing resources.
        /// Code taken from http://stackoverflow.com/a/3661892/ with some extra fixes.
        /// </summary>
        /// <param name="sourceBitmap">The image to clone</param>
        /// <returns>The cloned image</returns>
        private static Bitmap CloneImage(Bitmap sourceBitmap)
        {
            var rect = new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height);
            var targetImage = new Bitmap(rect.Width, rect.Height, sourceBitmap.PixelFormat);

            int actualDataWidth = ((Image.GetPixelFormatSize(sourceBitmap.PixelFormat) * rect.Width) + 7) / 8;

            var imageBytes = ArrayPool<byte>.Shared.Rent(actualDataWidth);
            BitmapData sourceData = sourceBitmap.LockBits(rect, ImageLockMode.ReadOnly, sourceBitmap.PixelFormat);
            BitmapData targetData = targetImage.LockBits(rect, ImageLockMode.WriteOnly, targetImage.PixelFormat);

            try
            {
                var origStride = Math.Abs(sourceData.Stride);   // Fix for negative stride in BMP format.
                var targetStride = targetData.Stride;
                var sourcePtr = sourceData.Scan0;
                var destPtr = targetData.Scan0;
                var height = sourceBitmap.Height;

                // Copy line by line, skipping by stride but copying actual data width
                for (var y = 0; y < height; y++)
                {
                    Marshal.Copy(sourcePtr, imageBytes, 0, actualDataWidth);
                    Marshal.Copy(imageBytes, 0, destPtr, actualDataWidth);
                    sourcePtr += origStride;
                    destPtr += targetStride;
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(imageBytes, clearArray: true);
                targetImage.UnlockBits(targetData);
                sourceBitmap.UnlockBits(sourceData);
            }

            // Fix for negative stride on BMP format.
            bool isFlipped = sourceData.Stride < 0;
            if (isFlipped)
                targetImage.RotateFlip(RotateFlipType.Rotate180FlipX);

            // For indexed images, restore the palette. This is not linking to a referenced
            // object in the original image; the getter of Palette creates a new object when called.
            if ((sourceBitmap.PixelFormat & PixelFormat.Indexed) != 0)
                targetImage.Palette = sourceBitmap.Palette;

            // Restore DPI settings
            targetImage.SetResolution(sourceBitmap.HorizontalResolution, sourceBitmap.VerticalResolution);

            return targetImage;
        }

        private static int ReadIntFromByteArray(ReadOnlySpan<byte> data, int startIndex, int bytes, bool littleEndian = true)
        {
            var lastByte = bytes - 1;

            if (data.Length < startIndex + bytes)
                throw new ArgumentOutOfRangeException(nameof(startIndex), "Data array is too small to read a " + bytes + "-byte value at offset " + startIndex + ".");

            int value = 0;
            for (var index = 0; index < bytes; index++)
            {
                var offset = startIndex + (littleEndian ? index : lastByte - index);
                value += (int)(data[offset] << (8 * index));
            }
            return value;
        }

    }
#endif
}
