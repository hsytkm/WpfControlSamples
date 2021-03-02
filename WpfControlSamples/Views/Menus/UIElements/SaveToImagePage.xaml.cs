using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
    public partial class SaveToImagePage : ContentControl
    {
        // 画像保存PATH
        private string _saveImagePath;

        public SaveToImagePage()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var path = System.IO.Path.GetTempFileName() + ".png";
            SaveImage(targetPanel, path);

            _saveImagePath = path;
            myTextBlock.Text = $"Save to {_saveImagePath}";
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_saveImagePath)) return;

            myImage.Source = _saveImagePath.FilePathToBitmapImage();
        }

        // 指定コントロールを画像ファイルとして保存する
        private static void SaveImage(FrameworkElement visual, string path, BitmapEncoder encoder = null)
        {
            if (visual is null) throw new ArgumentNullException(nameof(visual));
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException(nameof(path));

            var renderBitmap = new RenderTargetBitmap(
                (int)visual.ActualWidth, (int)visual.ActualHeight,
                96.0, 96.0, PixelFormats.Pbgra32);

            var targetSize = new Size(visual.ActualWidth, visual.ActualHeight);
            visual.Measure(targetSize);
            visual.Arrange(new Rect(targetSize));

            renderBitmap.Render(visual);

            encoder ??= new PngBitmapEncoder();
            var frame = BitmapFrame.Create(renderBitmap);
            encoder.Frames.Add(frame);

            using var fs = File.Open(path, FileMode.Create);
            encoder.Save(fs);
        }
    }
}
