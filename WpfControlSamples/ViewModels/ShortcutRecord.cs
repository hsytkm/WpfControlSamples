using System;
using System.IO;
using System.Runtime.InteropServices;

namespace WpfControlSamples.ViewModels
{
    /// <summary>dynamic で shortcut の リンク先 Path を取得します</summary>
    public record ShortcutRecord
    {
        // [[C#] ショートカットファイル(.lnk)の内容を取得する - ざこノート](https://note.dokeep.jp/post/csharp-shortcut-file-load/#windows-script-host-object-model-%E5%8F%82%E7%85%A7%E8%A8%AD%E5%AE%9A-%E4%BB%8A%E5%9B%9E%E4%BD%BF%E7%94%A8)
        public string? SourcePath { get; init; }
        public string? Arguments { get; init; }
        public string? Description { get; init; }
        public string? FullName { get; init; }
        public string? Hotkey { get; init; }
        public string? IconLocation { get; init; }
        public string? TargetPath { get; init; }
        public int? WindowStyle { get; init; }
        public string? WorkingDirectory { get; init; }

        private ShortcutRecord() { }

        public static ShortcutRecord Create(string shortcutPath)
        {
            if (Path.GetExtension(shortcutPath).ToLowerInvariant() != ".lnk")
                throw new ArgumentException("Invalid extension");

            dynamic? shell = null;   // IWshRuntimeLibrary.WshShell
            dynamic? lnk = null;     // IWshRuntimeLibrary.IWshShortcut
            try
            {
                // dynamic
                var type = Type.GetTypeFromProgID("WScript.Shell");
                if (type is null) throw new NullReferenceException();

                shell = Activator.CreateInstance(type);
                if (shell is null) throw new NullReferenceException();

                lnk = shell.CreateShortcut(shortcutPath);
                if (lnk is null) throw new NullReferenceException();

                return new ShortcutRecord()
                {
                    SourcePath = shortcutPath,
                    Arguments = (string)lnk.Arguments,
                    Description = (string)lnk.Description,
                    FullName = (string)lnk.FullName,
                    Hotkey = (string)lnk.Hotkey,
                    IconLocation = (string)lnk.IconLocation,
                    TargetPath = (string)lnk.TargetPath,
                    WindowStyle = (int)lnk.WindowStyle,
                    WorkingDirectory = (string)lnk.WorkingDirectory
                };
            }
            finally
            {
                if (lnk is not null) Marshal.ReleaseComObject(lnk);
                if (shell is not null) Marshal.ReleaseComObject(shell);
            }
        }

        public static string? GetFullPath(string shortcutPath) => Create(shortcutPath)?.TargetPath ?? null;
    }
}
