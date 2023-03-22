using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shell;

namespace WpfControlSamples.Infrastructures;

/// <summary>
/// タスクバー上のアイコンを右クリックした際に表示されるリスト
/// </summary>
internal static class JumpListManager
{
    private static string GetExeFilePath() =>
        Environment.ProcessPath ?? Environment.GetCommandLineArgs()[0];

    /// <summary>
    /// 指定ディレクトリを JumpList.Recent に追加します
    /// </summary>
    internal static void AddToRecentDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
            return;

        static JumpTask createJumpTask(string dirFullPath) => new()
        {
            ApplicationPath = GetExeFilePath(),
            Title = dirFullPath,
            Description = dirFullPath,
            Arguments = dirFullPath,
            WorkingDirectory = dirFullPath,
            //CustomCategory
            //IconResourceIndex
            //IconResourcePath
        };

        var jumpTask = createJumpTask(directoryPath);
        JumpList.AddToRecentCategory(jumpTask);
    }
}
