using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus;

public partial class FileDropToPage : ContentControl
{
    public FileDropToPage()
    {
        DataContext = new FileDropToViewModel();
        InitializeComponent();
    }

    private void SimpleWithoutAdorner_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton is not MouseButtonState.Pressed)
            return;

        if (sender is not FrameworkElement fe)
            return;

        if (fe.DataContext is not LocalFile localFile)
            return;

        // アプリ外（デスクトップとか）にドラッグするとめちゃくちゃ Exception が出ます。
        // catch できないし、動作は意図通りなので、知らんぷりします
        var obj = new DataObject(DataFormats.FileDrop, new[] { localFile.FullPath });
        _ = DragDrop.DoDragDrop(this, obj, DragDropEffects.Copy);
    }
}

class FileDropToViewModel : MyBindableBase
{
    public IReadOnlyList<LocalFile> LocalFilesSource { get; }

    public FileDropToViewModel()
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        LocalFilesSource = Directory.EnumerateFiles(path).Select(x => new LocalFile(x)).ToArray();
    }
}

public sealed record LocalFile(string FullPath, string Filename)
{
    public LocalFile(string FullPath) : this(FullPath, Path.GetFileName(FullPath)) { }
}
