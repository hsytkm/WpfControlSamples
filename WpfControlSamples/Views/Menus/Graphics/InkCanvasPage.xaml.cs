using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class InkCanvasPage : ContentControl
    {
        public InkCanvasPage()
        {
            InitializeComponent();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.Strokes.Clear();
        }

        private void InkCanvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            if (!(sender is InkCanvas inkCanvas)) return;

            var renderTargetBitmap = new RenderTargetBitmap(
                (int)inkCanvas.ActualWidth, (int)inkCanvas.ActualHeight,
                96.0, 96.0, PixelFormats.Default);

            renderTargetBitmap.Render(inkCanvas);

            //await SaveToPngFileAsync(renderTargetBitmap);

            image.Source = renderTargetBitmap;
        }

        //private static async Task SaveToPngFileAsync(BitmapSource bitmapSource)
        //{
        //    using var ms = new MemoryStream();
        //    var encoder = new PngBitmapEncoder();
        //    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
        //    encoder.Save(ms);
        //    await ms.FlushAsync();
        //}
    }
}
