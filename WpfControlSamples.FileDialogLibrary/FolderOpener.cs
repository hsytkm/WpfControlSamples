using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Linq;

namespace WpfControlSamples.FileDialogLibrary
{
    /*  WPF標準機能ではフォルダ選択ダイアログがサポートされていないので、
     *  NuGet から下記パッケージを導入した。
     *  （ややこしくなるので本プロジェクトから分離した）
     *    - WindowsAPICodePack-Core
     *    - WindowsAPICodePack-Shell
     *
     *  どちらもライセンスは free っぽい。
     *    The library is not developed anymore by Microsoft
     *    and seems to have been left as 'free to use'.
     *
     *  LivetExtensions(zlibライセンス) も良さげ。
     *  海外では Ookii.Dialogs も有名っぽかった（未確認）
     *
     */
    public static class FolderOpener
    {
        /// <summary>
        /// FolderOpenDialog(Single)
        /// </summary>
        /// <returns>フォルダPATH</returns>
        public static string? OpenSingle(string? title = null)
        {
            using var dialog = new CommonOpenFileDialog(title)
            {
                IsFolderPicker = true
            };
            var result = dialog.ShowDialog();

            if (result != CommonFileDialogResult.Ok) return default;
            return dialog.FileName;
        }

        /// <summary>
        /// FolderOpenDialog(Multi)
        /// </summary>
        /// <returns>フォルダPATHの配列</returns>
        public static string[]? OpenMulti(string? title = null)
        {
            using var dialog = new CommonOpenFileDialog(title)
            {
                IsFolderPicker = true,
                Multiselect = true
            };
            var result = dialog.ShowDialog();

            if (result != CommonFileDialogResult.Ok) return default;
            return dialog.FileNames.ToArray();
        }
    }
}
