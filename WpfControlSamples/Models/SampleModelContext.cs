#nullable disable
using System;
using System.Diagnostics;
using System.Reflection;

namespace WpfControlSamples.Models
{
    class SampleModelContext
    {
        public string AppName { get; } =
            FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName;

    }
}
