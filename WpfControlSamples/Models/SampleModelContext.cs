using System;
using System.Diagnostics;
using System.Reflection;

namespace WpfControlSamples.Models
{
    class SampleModelContext
    {
        // Assembly.GetExecutingAssembly().Location は、単一ファイルアプリで空文字を返すので使用しない!!
        // ダメ: FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location)?.ProductName
        public string AppName { get; } = Assembly.GetExecutingAssembly().GetName().Name ?? "unknown";

    }
}
