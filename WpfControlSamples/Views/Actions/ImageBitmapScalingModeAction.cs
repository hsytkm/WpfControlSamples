#nullable disable
using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfControlSamples.Views.Actions
{
    /// <summary>
    /// 画像の表示倍率に応じて BitmapScalingMode を変更するTriggerAction
    /// </summary>
    class ImageBitmapScalingModeAction : TriggerAction<Image>
    {
        protected override void Invoke(object parameter)
        {
            if (!(parameter is SizeChangedEventArgs sizeChangedEventArgs)) return;
            if (!(sizeChangedEventArgs.Source is Image image)) return;
            if (!(image.Source is BitmapSource bitmapSource)) return;

            var viewSize = sizeChangedEventArgs.NewSize;
            var sourceSize = new Size(bitmapSource.PixelWidth, bitmapSource.PixelHeight);

            // 100% 超える拡大ならピクセルが見える設定にする
            var isZoomIn = sourceSize.Width > viewSize.Width || sourceSize.Height > viewSize.Height;
            var scalingMode = isZoomIn ? BitmapScalingMode.HighQuality : BitmapScalingMode.NearestNeighbor;

            RenderOptions.SetBitmapScalingMode(image, scalingMode);
        }
    }
}
